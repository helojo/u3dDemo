namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SKillHurtTypeConstant : Concentrator
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
            SkillHurtType enumerator = (SkillHurtType) intent2.GetEnumerator(SkillIntentDecl.skill_hurt_type);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, enumerator);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "HurtType";
        }
    }
}

