namespace Battle
{
    using FastBuf;
    using Fatefulness;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AiSkill : ObjectTracer
    {
        private Fragment aiFate;
        private AiManager aiManager;
        private string aiSkillId = string.Empty;
        public static int Counter;
        public static bool G_IsShowHpLog;
        public static bool G_IsShowSkillLog;
        private bool isAllwaysOneTarget;
        private bool isEnergyAdded;
        private int m_curSubSkillIdx;
        private List<AiActor> oldTargets = new List<AiActor>();
        private skill_configTss skillData;

        public AiSkill(int skillUniId, int skillEntry, AiActor caster, AiActor target, AiManager _aiManager)
        {
            this.SkillUniId = skillUniId;
            this.Caster = caster;
            this.InitTarget = target;
            this.CurActiveTarget = null;
            this.TargetInitPos = this.InitTarget.Pos;
            this.SkillEntry = skillEntry;
            this.skillData = new skill_configTss(ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry));
            this.SubSkillList = this.skillData.sub_skill_list.Split("|".ToCharArray());
            this.CurSubSkillIdx = -1;
            this.isAllwaysOneTarget = this.skillData.range_type == 6;
            this.aiManager = _aiManager;
            this.aiManager.AddSkill(this);
            this.aiSkillId = string.Format("{0}_{1}", caster.ServerIdx, Counter++);
            this.InitAI();
        }

        public SkillEffectCastResult DoSkillCasting()
        {
            SkillEffectCastResult result = new SkillEffectCastResult {
                mainTargetList = this.GetTargetIDList(this.InitTarget)
            };
            if (result.mainTargetList.Contains(this.InitTarget.Id))
            {
                result.mainTargetList.Remove(this.InitTarget.Id);
                result.mainTargetList.Insert(0, this.InitTarget.Id);
            }
            return result;
        }

        private AiActor GetActiveTarget()
        {
            if (this.isAllwaysOneTarget)
            {
                return this.InitTarget;
            }
            return this.Caster.FindTarget(this.SkillEntry, this.CurActiveTarget, this.oldTargets);
        }

        private int GetHitLevel()
        {
            return S_GetHitLevel(this.skillData, this.Caster);
        }

        public List<int> GetTargetIDList(AiActor _target)
        {
            <GetTargetIDList>c__AnonStoreyD6 yd = new <GetTargetIDList>c__AnonStoreyD6 {
                targetList = new List<int>()
            };
            switch (this.skillData.range_type)
            {
                case 1:
                case 6:
                    yd.targetList.Add(_target.Id);
                    break;

                case 2:
                    _target.SelfTeam.AliveActorList.ForEach(new Action<AiActor>(yd.<>m__2F));
                    break;

                case 3:
                {
                    AiTeam selfTeam = _target.SelfTeam;
                    if ((this.skillData.is_target_enemy == 1) && (this.skillData.target_type == 0))
                    {
                        selfTeam = _target.EnemyTeam;
                    }
                    AiUtil.FindNearTarget(this.TargetInitPos, selfTeam.AliveActorList, this.Caster.HeadDirection, this.skillData.range_value_near, this.skillData.range_value_far).ForEach(new Action<AiActor>(yd.<>m__30));
                    break;
                }
                case 4:
                    _target.SelfTeam.AliveActorList.ForEach(new Action<AiActor>(yd.<>m__31));
                    _target.EnemyTeam.AliveActorList.ForEach(new Action<AiActor>(yd.<>m__32));
                    break;
            }
            return yd.targetList;
        }

        private void InitAI()
        {
            if (!string.IsNullOrEmpty(this.skillData.skill_ai))
            {
                this.aiFate = AiFragmentManager.GetInstance().GetFrament(this.skillData.skill_ai);
                if (this.aiFate == null)
                {
                    Debug.LogWarning("failed to load skill fate named" + this.skillData.skill_ai);
                }
                else
                {
                    List<int> targetIDList = this.GetTargetIDList(this.InitTarget);
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutObject(SkillIntentDecl.skill_ai_manager, this.aiManager);
                    context.PutInt32(SkillIntentDecl.skill_caster_id, this.Caster.Id);
                    context.PutObject(SkillIntentDecl.skill_target_id_list, targetIDList);
                    context.PutObject(SkillIntentDecl.skill_ai_skill, this);
                    context.PutObject(SkillIntentDecl.skill_ai_buffer, null);
                    context.PutInt32(SkillIntentDecl.skill_entry, this.SkillEntry);
                    if (targetIDList.Count > 0)
                    {
                        context.PutObject(SkillIntentDecl.skill_target_id, targetIDList[0]);
                    }
                    this.aiFate.Deploy(context);
                    Intent intent2 = new Intent(Intent.IntentType.forward);
                    intent2.PutString(IntentDecl.port, "Preload");
                    this.aiFate.Dispatch(intent2);
                }
            }
        }

        public void OnFinish()
        {
            if ((this.aiFate != null) && !string.IsNullOrEmpty(this.skillData.skill_ai))
            {
                AiFragmentManager.GetInstance().PushFrament(this.skillData.skill_ai, this.aiFate);
            }
            this.aiFate = null;
        }

        public void OnSkillAICastFinish()
        {
            if (this.aiFate != null)
            {
                AiActor activeTarget = this.GetActiveTarget();
                if (activeTarget != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "CastFinish");
                    this.CurActiveTarget = activeTarget;
                    this.oldTargets.Add(this.CurActiveTarget);
                    List<int> targetIDList = this.GetTargetIDList(this.CurActiveTarget);
                    if (targetIDList.Contains(activeTarget.Id))
                    {
                        targetIDList.Remove(activeTarget.Id);
                        targetIDList.Insert(0, activeTarget.Id);
                    }
                    context.PutObject(SkillIntentDecl.skill_activity_target_id, activeTarget.Id);
                    context.PutObject(SkillIntentDecl.skill_target_id_list, targetIDList);
                    this.aiFate.Dispatch(context);
                }
            }
            if (this.Caster != null)
            {
                this.aiManager.OnCastSkillFinishedEvent(this.Caster.Id, this.SkillEntry);
            }
        }

        public void OnSkillAICastStart()
        {
            if (this.aiFate != null)
            {
                AiActor activeTarget = this.GetActiveTarget();
                if (activeTarget != null)
                {
                    Intent context = new Intent(Intent.IntentType.forward);
                    context.PutString(IntentDecl.port, "Cast");
                    this.CurActiveTarget = activeTarget;
                    this.oldTargets.Add(this.CurActiveTarget);
                    List<int> targetIDList = this.GetTargetIDList(this.CurActiveTarget);
                    if (targetIDList.Contains(activeTarget.Id))
                    {
                        targetIDList.Remove(activeTarget.Id);
                        targetIDList.Insert(0, activeTarget.Id);
                    }
                    context.PutObject(SkillIntentDecl.skill_activity_target_id, activeTarget.Id);
                    context.PutObject(SkillIntentDecl.skill_target_id_list, targetIDList);
                    this.aiFate.Dispatch(context);
                }
            }
            if (this.Caster != null)
            {
                this.aiManager.OnCastSkillEvent(this.Caster.Id, this.SkillEntry);
            }
        }

        public SkillEffectResult OnTakeSkillEffect(AiActor _target, int subSkillIdx)
        {
            if (this.Caster != null)
            {
                this.aiManager.OnCastSkillConfirmedEvent(this.Caster.Id, _target.Id, this.SkillEntry);
            }
            if (this.aiFate != null)
            {
                Intent context = new Intent(Intent.IntentType.forward);
                context.PutString(IntentDecl.port, "Confirm");
                context.PutObject(SkillIntentDecl.skill_target_id, _target.Id);
                context.PutInt32(SkillIntentDecl.sub_skill_index, subSkillIdx);
                this.aiFate.Dispatch(context);
            }
            this.CurSubSkillIdx = subSkillIdx;
            if (this.SubSkillEntry >= 0)
            {
                return this.OnTakeSkillEffectByID(_target, this.SubSkillEntry);
            }
            return null;
        }

        public SkillEffectResult OnTakeSkillEffectByID(AiActor _target, int _subSkillEntry)
        {
            if (!this.isEnergyAdded)
            {
                this.isEnergyAdded = true;
                if (this.Category == SkillCategory.Normal)
                {
                    this.Caster.OnEnergyChange(this.Caster.FixedEnergyRecoverOnAttack, HitEnergyType.ByCastSkill);
                }
            }
            return AiSkillFunction.G_OnTakeSkillEffectByID(this.aiManager, this, this.Caster, _target, _subSkillEntry, this.aiFate, this.GetHitLevel(), this.Caster.GetSkillLevel(this.SkillEntry), null);
        }

        public void OnTick()
        {
            if ((this.isAllwaysOneTarget && (this.InitTarget != null)) && (this.InitTarget.IsDead && (this.Caster.CastingSkill == this)))
            {
                this.Caster.BreakSkill(true, true);
            }
            if (this.aiFate != null)
            {
                this.aiFate.Tick();
            }
        }

        public void OnUpdate()
        {
            if (this.aiFate != null)
            {
                this.aiFate.Update();
            }
        }

        public static int S_GetHitLevel(skill_configTss skillData, AiActor caster)
        {
            return ((((int) skillData.skill_hit_level) + caster.GetSkillLevel(skillData.entry)) - 1);
        }

        public static int S_GetHitLevel(skill_config skillData, AiActor caster)
        {
            return ((skillData.skill_hit_level + caster.GetSkillLevel(skillData.entry)) - 1);
        }

        public int StartCast()
        {
            this.CurSubSkillIdx = 0;
            return Convert.ToInt32(this.SubSkillList[this.CurSubSkillIdx]);
        }

        public int SubSkillID2Entry(int id)
        {
            if ((id >= 0) && (id < this.SubSkillList.Count<string>()))
            {
                return Convert.ToInt32(this.SubSkillList[id]);
            }
            return -1;
        }

        public virtual void Trace(ObjectMetaData meta)
        {
            meta.descript = "...";
            Debugger.TraceDetails<int>(meta, "skill entry", this.SkillEntry);
            Debugger.TraceDetails<string[]>(meta, "sub skill list", this.SubSkillList);
            Debugger.TraceDetails<AiActor>(meta, "caster", this.Caster);
            Debugger.TraceDetails<AiActor>(meta, "current target", this.InitTarget);
            Debugger.TraceDetails<int>(meta, "sub skill index", this.CurSubSkillIdx);
            Debugger.TraceDetails<int>(meta, "sub skill entry", this.SubSkillEntry);
        }

        public string AiSkillId
        {
            get
            {
                return this.aiSkillId;
            }
        }

        private AiActor Caster { get; set; }

        public SkillCategory Category { get; set; }

        private AiActor CurActiveTarget { get; set; }

        private int CurSubSkillIdx
        {
            get
            {
                return this.m_curSubSkillIdx;
            }
            set
            {
                this.m_curSubSkillIdx = value;
            }
        }

        private AiActor InitTarget { get; set; }

        public int SkillEntry { get; set; }

        public int SkillUniId { get; set; }

        public int SubSkillEntry
        {
            get
            {
                if ((this.CurSubSkillIdx < 0) || (this.CurSubSkillIdx >= this.SubSkillList.Count<string>()))
                {
                    return -1;
                }
                if (string.IsNullOrEmpty(this.SubSkillList[this.CurSubSkillIdx]))
                {
                    return -1;
                }
                return Convert.ToInt32(this.SubSkillList[this.CurSubSkillIdx]);
            }
        }

        private string[] SubSkillList { get; set; }

        private Vector3 TargetInitPos { get; set; }

        [CompilerGenerated]
        private sealed class <GetTargetIDList>c__AnonStoreyD6
        {
            internal List<int> targetList;

            internal void <>m__2F(AiActor obj)
            {
                this.targetList.Add(obj.Id);
            }

            internal void <>m__30(AiActor obj)
            {
                this.targetList.Add(obj.Id);
            }

            internal void <>m__31(AiActor obj)
            {
                this.targetList.Add(obj.Id);
            }

            internal void <>m__32(AiActor obj)
            {
                this.targetList.Add(obj.Id);
            }
        }

        public class SkillTargetInfo
        {
            public bool isTargetHit;
            public int targetID;
        }
    }
}

