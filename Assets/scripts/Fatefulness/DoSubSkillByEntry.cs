namespace Fatefulness
{
    using System;
    using UnityEngine;

    public class DoSubSkillByEntry : SkillDoSubSkill
    {
        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("Entry", Transition.Direction.input);
        }

        protected override void DoDeSerialization(IOContext io_context)
        {
        }

        protected override void DoSerialization(IOContext io_context)
        {
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with do subskill by entry concentrator!");
            }
            Transition port = base.GetPort("Entry");
            if (port == null)
            {
                throw new UnityException("unexisted transition port named Entry");
            }
            Intent intent2 = new Intent(Intent.IntentType.review);
            port.Review(intent2);
            base.sub_skill_entry = intent2.GetInt32(IntentDecl.variable);
            base.Excude(intent);
        }

        public override string Name()
        {
            return "DoSubSkill";
        }

        public override void RegisterSerializableSegment()
        {
        }
    }
}

