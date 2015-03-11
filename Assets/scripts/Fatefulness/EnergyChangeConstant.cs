namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class EnergyChangeConstant : Concentrator
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
                throw new UnityException("forwarding intent is not matched with skill damage getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the skill damage getter!");
            }
            int num = intent2.GetInt32(SkillIntentDecl.energy_changed_value);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, num);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "EnergyChange";
        }
    }
}

