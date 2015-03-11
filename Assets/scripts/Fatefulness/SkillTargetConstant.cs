namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillTargetConstant : Concentrator
    {
        protected AiManager aimgr;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with skill target getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the skill target getter!");
            }
            int id = intent2.GetInt32(SkillIntentDecl.skill_target_id);
            AiActor actorById = this.AiMgr.GetActorById(id);
            if (actorById == null)
            {
                throw new UnityException("can not find the skill actor id = " + id);
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, actorById);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Target";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
        }

        protected AiManager AiMgr
        {
            get
            {
                if (this.aimgr == null)
                {
                    Intent intent = base.root_fragment.global_context;
                    if (intent == null)
                    {
                        throw new UnityException("need global context to explain the skill target getter!");
                    }
                    if (this.aimgr == null)
                    {
                        this.aimgr = intent.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                        if (this.aimgr == null)
                        {
                            throw new UnityException("need global context about AIManager to explain the skill target getter!");
                        }
                    }
                }
                return this.aimgr;
            }
        }
    }
}

