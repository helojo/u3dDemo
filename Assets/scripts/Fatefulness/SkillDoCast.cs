namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class SkillDoCast : SkillDoEffect
    {
        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("forwarding intent is not matched with skill do cast!");
            }
            if (!string.IsNullOrEmpty(base.effect_res))
            {
                Transition port = base.GetPort("variable");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named variable");
                }
                Intent intent2 = new Intent(Intent.IntentType.review);
                port.Review(intent2);
                object obj2 = intent2.GetObject(IntentDecl.variable);
                object obj3 = intent2.GetObject(IntentDecl.list_datasource);
                List<AiActor> targetList = new List<AiActor>();
                if (obj2 != null)
                {
                    targetList.Add(obj2 as AiActor);
                }
                if (obj3 != null)
                {
                    foreach (object obj4 in obj3 as List<object>)
                    {
                        targetList.Add(obj4 as AiActor);
                    }
                }
                base.InitEvn();
                base.aimgr.OnEventDoSkillCasting(base.skill_object, base.effect_res, targetList);
            }
        }

        public override string Name()
        {
            return "DoCast";
        }
    }
}

