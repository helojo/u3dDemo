namespace Fatefulness
{
    using FastBuf;
    using System;
    using UnityEngine;

    public class SubSkillIdentity : Concentrator
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
                throw new UnityException("forwarding intent is not matched with sub skill identity getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the sub skill identity getter!");
            }
            int id = intent2.GetInt32(SkillIntentDecl.skill_entry);
            int index = intent2.GetInt32(SkillIntentDecl.sub_skill_index);
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(id);
            if (_config == null)
            {
                throw new UnityException("invalid skill entry parameter!");
            }
            int num3 = -1;
            char[] separator = new char[] { '|' };
            string[] strArray = _config.sub_skill_list.Split(separator);
            if ((index >= 0) && (index < strArray.Length))
            {
                num3 = Convert.ToInt32(strArray[index]);
            }
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, num3);
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "SubSkillEntry";
        }
    }
}

