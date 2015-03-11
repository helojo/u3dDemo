namespace Fatefulness
{
    using FastBuf;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SubSkillList : Concentrator
    {
        private List<object> entry_list = new List<object>();

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.output);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with sub skill list getter!");
            }
            if (this.entry_list.Count <= 0)
            {
                Intent intent2 = base.root_fragment.global_context;
                if (intent2 == null)
                {
                    throw new UnityException("need global context to explain the sub skill list getter!");
                }
                int id = intent2.GetInt32(SkillIntentDecl.skill_entry);
                skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(id);
                if (_config == null)
                {
                    throw new UnityException("invalid skill entry parameter!");
                }
                char[] separator = new char[] { '|' };
                foreach (string str in _config.sub_skill_list.Split(separator))
                {
                    this.entry_list.Add(Convert.ToInt32(str));
                }
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.list_datasource, this.entry_list);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "SubSkillList";
        }

        public override void Reset()
        {
            base.Reset();
            this.entry_list.Clear();
        }
    }
}

