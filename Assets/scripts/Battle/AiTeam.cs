namespace Battle
{
    using FastBuf;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AiTeam
    {
        [CompilerGenerated]
        private static Func<AiActor, bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<AiActor, bool> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<AiActor, bool> <>f__am$cache6;
        private readonly List<AiActor> m_actorList = new List<AiActor>();

        public AiTeam()
        {
            this.Target = null;
            this.IsAlarm = false;
            this.IsAttacker = false;
        }

        public AiActor AddActor(CombatDetailActor baseData, AiManager aiManager)
        {
            AiActor item = new AiActor(baseData, aiManager, this.IsAttacker, this.m_actorList.Count, true);
            this.m_actorList.Add(item);
            aiManager.AddActorToMap(item);
            return item;
        }

        public AiActor AddNoBattleActor(CombatDetailActor baseData, AiManager aiManager)
        {
            AiActor item = new AiActor(baseData, aiManager, this.IsAttacker, this.m_actorList.Count, false);
            this.m_actorList.Add(item);
            aiManager.AddActorToMap(item);
            return item;
        }

        public void ClearActorBuff()
        {
            foreach (AiActor actor in this.AliveActorList)
            {
                actor.RemoveAllBuffAndKeepNeedBuff();
            }
        }

        public void ClearSummonActor()
        {
            foreach (AiActor actor in this.m_actorList)
            {
                if (actor.ToDeadTime > 0f)
                {
                    actor.OnToDeadTime();
                }
            }
        }

        public void CrearActorData()
        {
            this.ClearSummonActor();
            this.ClearActorBuff();
        }

        public void EachAlive<T>(EachCondition<T> cond) where T: AiActor
        {
            for (int i = 0; i < this.m_actorList.Count; i++)
            {
                if ((this.m_actorList[i].IsAlive && this.m_actorList[i].IsBattleObj) && cond(this.m_actorList[i] as T))
                {
                    break;
                }
            }
        }

        public int GetAllActorCount()
        {
            return this.m_actorList.Count;
        }

        public float GetEnemyTeamAttributeEffect(SkillLogicEffectType type, SkillLogicEffectOperateType operateType)
        {
            <GetEnemyTeamAttributeEffect>c__AnonStoreyDA yda = new <GetEnemyTeamAttributeEffect>c__AnonStoreyDA {
                type = type,
                operateType = operateType,
                result = 0f,
                buffIdList = new List<int>()
            };
            this.EachAlive<AiActor>(new EachCondition<AiActor>(yda.<>m__39));
            return yda.result;
        }

        public float GetSelfAttributeEffect(SkillLogicEffectType type, SkillLogicEffectOperateType operateType)
        {
            <GetSelfAttributeEffect>c__AnonStoreyD9 yd = new <GetSelfAttributeEffect>c__AnonStoreyD9 {
                type = type,
                operateType = operateType,
                result = 0f,
                buffIdList = new List<int>()
            };
            this.EachAlive<AiActor>(new EachCondition<AiActor>(yd.<>m__38));
            return yd.result;
        }

        public bool HasStateInTeamBuff(StateBuffType stateType)
        {
            foreach (AiActor actor in this.AliveActorList)
            {
                foreach (AiBuff buff in actor.GetBuffs())
                {
                    if (buff.IsTeamBuff() && (buff.State == stateType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsTeammate(int actorID)
        {
            <IsTeammate>c__AnonStoreyDB ydb = new <IsTeammate>c__AnonStoreyDB {
                actorID = actorID
            };
            return (this.m_actorList.Find(new Predicate<AiActor>(ydb.<>m__3A)) != null);
        }

        public void OnTeamBuffChange()
        {
            foreach (AiActor actor in this.AliveActorList)
            {
                actor.OnBuffChanged();
            }
        }

        public int UseHurtHpShare(int hurtValue, AiActor target)
        {
            int hurtHpShareBuffEntry = target.GetHurtHpShareBuffEntry();
            if (hurtHpShareBuffEntry >= 0)
            {
                List<AiActor> list = new List<AiActor>();
                foreach (AiActor actor in this.AliveActorList)
                {
                    if (((actor != null) && (actor != target)) && actor.IsHasBuff(hurtHpShareBuffEntry))
                    {
                        list.Add(actor);
                    }
                }
                if (list.Count > 0)
                {
                    <UseHurtHpShare>c__AnonStoreyD8 yd = new <UseHurtHpShare>c__AnonStoreyD8();
                    int num2 = list.Count + 1;
                    yd.newHurt = hurtValue / num2;
                    yd.newHurt = Mathf.Max(1, yd.newHurt);
                    list.ForEach(new Action<AiActor>(yd.<>m__37));
                    return yd.newHurt;
                }
            }
            return hurtValue;
        }

        public int UseHurtReduce(int hurtValue)
        {
            <UseHurtReduce>c__AnonStoreyD7 yd = new <UseHurtReduce>c__AnonStoreyD7 {
                hurtValue = hurtValue
            };
            this.EachAlive<AiActor>(new EachCondition<AiActor>(yd.<>m__36));
            return yd.hurtValue;
        }

        public List<AiActor> ActorList
        {
            get
            {
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = item => item.IsBattleObj;
                }
                return this.m_actorList.Where<AiActor>(<>f__am$cache4).ToList<AiActor>();
            }
        }

        public List<AiActor> AliveActorList
        {
            get
            {
                if (<>f__am$cache5 == null)
                {
                    <>f__am$cache5 = item => item.IsAlive && item.IsBattleObj;
                }
                return this.m_actorList.Where<AiActor>(<>f__am$cache5).ToList<AiActor>();
            }
        }

        public List<AiActor> DeadActorList
        {
            get
            {
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = item => (item.IsDead && item.IsBattleObj) && !item.IsSummonActor;
                }
                return this.m_actorList.Where<AiActor>(<>f__am$cache6).ToList<AiActor>();
            }
        }

        public bool IsAlarm { get; set; }

        public bool IsAttacker { get; set; }

        public AiActor Target { get; set; }

        [CompilerGenerated]
        private sealed class <GetEnemyTeamAttributeEffect>c__AnonStoreyDA
        {
            internal List<int> buffIdList;
            internal SkillLogicEffectOperateType operateType;
            internal float result;
            internal SkillLogicEffectType type;

            internal bool <>m__39(AiActor actor)
            {
                foreach (AiBuff buff in actor.GetBuffs())
                {
                    if (buff.IsEnemyTeamBuff() && !this.buffIdList.Contains(buff.Entry))
                    {
                        this.result += buff.GetAttributeEffect(this.type, this.operateType);
                        this.buffIdList.Add(buff.Entry);
                    }
                }
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <GetSelfAttributeEffect>c__AnonStoreyD9
        {
            internal List<int> buffIdList;
            internal SkillLogicEffectOperateType operateType;
            internal float result;
            internal SkillLogicEffectType type;

            internal bool <>m__38(AiActor actor)
            {
                foreach (AiBuff buff in actor.GetBuffs())
                {
                    if (buff.IsTeamBuff() && !this.buffIdList.Contains(buff.Entry))
                    {
                        this.result += buff.GetAttributeEffect(this.type, this.operateType);
                        this.buffIdList.Add(buff.Entry);
                    }
                }
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <IsTeammate>c__AnonStoreyDB
        {
            internal int actorID;

            internal bool <>m__3A(AiActor obj)
            {
                return (obj.Id == this.actorID);
            }
        }

        [CompilerGenerated]
        private sealed class <UseHurtHpShare>c__AnonStoreyD8
        {
            internal int newHurt;

            internal void <>m__37(AiActor obj)
            {
                obj.OnOtherHurt(this.newHurt);
            }
        }

        [CompilerGenerated]
        private sealed class <UseHurtReduce>c__AnonStoreyD7
        {
            internal int hurtValue;

            internal bool <>m__36(AiActor actor)
            {
                foreach (AiBuff buff in actor.GetBuffs())
                {
                    if (buff.IsTeamBuff())
                    {
                        this.hurtValue = buff.UseHurtReduce(this.hurtValue);
                        if (this.hurtValue <= 0)
                        {
                            this.hurtValue = 0;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public delegate bool EachCondition<T>(T item) where T: AiActor;
    }
}

