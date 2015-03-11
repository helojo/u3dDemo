namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class SkillHitTypeConstant : Concentrator
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with hit type getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the hit type getter!");
            }
            SkillHitType enumerator = (SkillHitType) intent2.GetEnumerator(SkillIntentDecl.skill_hit_type);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, enumerator);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "HitType";
        }
    }
}

