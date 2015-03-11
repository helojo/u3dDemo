namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkillTargetSetConstant : Concentrator
    {
        private AiManager aimgr;
        private List<object> target_list = new List<object>();

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            this.target_list.Clear();
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
            if (this.target_list.Count <= 0)
            {
                List<int> list = intent2.GetObject<List<int>>(SkillIntentDecl.skill_target_id_list);
                if (list == null)
                {
                    throw new UnityException("need global context list about skill target to explain the skill target set datasource getter!");
                }
                foreach (int num in list)
                {
                    AiActor actorById = this.aimgr.GetActorById(num);
                    if (actorById == null)
                    {
                        throw new UnityException("can not find the skill actor id = " + num);
                    }
                    this.target_list.Add(actorById);
                }
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, this.target_list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "TargetSet";
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
            this.target_list.Clear();
        }
    }
}

