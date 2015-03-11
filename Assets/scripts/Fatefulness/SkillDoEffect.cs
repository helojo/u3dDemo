namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillDoEffect : Concentrator
    {
        protected AiManager aimgr;
        protected string effect_res = string.Empty;
        protected AiSkill skill_object;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("resource", this.effect_res);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.input);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationString(out this.effect_res);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("forwarding intent is not matched with skill do effect!");
            }
            if (!string.IsNullOrEmpty(this.effect_res))
            {
                Transition port = base.GetPort("variable");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named variable");
                }
                Intent intent2 = new Intent(Intent.IntentType.review);
                port.Review(intent2);
                AiActor target = intent2.GetObject<AiActor>(IntentDecl.variable);
                if (target != null)
                {
                    this.InitEvn();
                    this.aimgr.OnEventShowEffect(this.skill_object, this.effect_res, target);
                }
            }
        }

        protected void InitEvn()
        {
            Intent intent = base.root_fragment.global_context;
            if (intent == null)
            {
                throw new UnityException("need global context to explain the skill do effect!");
            }
            if (this.aimgr == null)
            {
                this.aimgr = intent.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                if (this.aimgr == null)
                {
                    throw new UnityException("need global context about AIManager to explain the skill do effect!");
                }
            }
            if (this.skill_object == null)
            {
                this.skill_object = intent.GetObject<AiSkill>(SkillIntentDecl.skill_ai_skill);
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "DoEffect";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("resource", obj => this.effect_res = obj.ToString(), () => this.effect_res);
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
            this.skill_object = null;
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationString(this.effect_res);
        }
    }
}

