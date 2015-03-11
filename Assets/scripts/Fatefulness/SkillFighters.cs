namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkillFighters : Concentrator
    {
        private AiManager aimgr;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with skill target set getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the skill target set getter!");
            }
            if (this.aimgr == null)
            {
                this.aimgr = intent2.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                if (this.aimgr == null)
                {
                    throw new UnityException("need global context about AIManager to explain the skill target set getter!");
                }
            }
            List<object> list = new List<object>();
            foreach (AiActor actor in this.aimgr.AllAliveActors)
            {
                list.Add(actor);
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Fighters";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
        }
    }
}

