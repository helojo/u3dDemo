namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillCasterConstant : Concentrator
    {
        private AiManager aimgr;
        private AiActor caster;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with skill caster getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the skill caster getter!");
            }
            if (this.aimgr == null)
            {
                this.aimgr = intent2.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                if (this.aimgr == null)
                {
                    throw new UnityException("need global context about AIManager to explain the skill caster getter!");
                }
            }
            if (this.caster == null)
            {
                int id = intent2.GetInt32(SkillIntentDecl.skill_caster_id);
                this.caster = this.aimgr.GetActorById(id);
                if (this.caster == null)
                {
                    throw new UnityException("can not find the skill actor id = " + id);
                }
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.caster);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Caster";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
            this.caster = null;
        }
    }
}

