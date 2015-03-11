using Battle;
using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkill : MonoBehaviour
{
    private List<RealTimeSkillActionBase> actions;
    private List<ActionGroup> AIGroups = new List<ActionGroup>();
    public List<int> allTargetIDs;
    public BattleData battleGameData;
    public int casterID;
    private ActionGroup castingActionsOfAI;
    private RealTimeSkillActionBase curAction;
    private string curAiEffectName;
    private List<ActionGroup> groups = new List<ActionGroup>();
    private bool isShowTiming;
    public bool isStarted;
    public int lastTargetID;
    public int mainTargetID;
    public float moveCollsionDis;
    private skill_config skillConfig;
    public int skillEntry;
    public int skillID;
    public RealTimeSkillInfo skillInfo;
    private RealTimeSkillState state;

    public void Clear()
    {
        if (this.actions != null)
        {
            foreach (RealTimeSkillActionBase base2 in this.actions)
            {
                if (base2 != null)
                {
                    base2.DoClear();
                }
            }
        }
        foreach (ActionGroup group in this.AIGroups)
        {
            if (group != null)
            {
                group.ClearAll();
            }
        }
        base.StopAllCoroutines();
    }

    public void DoAICasting(string effectName, List<int> targets)
    {
        RealTimeSkillInfo skillData = RealSkillInfoManager.GetSkillData(effectName);
        if (!skillData.oneByOne || (this.curAiEffectName != effectName))
        {
            this.curAiEffectName = effectName;
            this.castingActionsOfAI = new ActionGroup();
            this.AIGroups.Add(this.castingActionsOfAI);
            this.castingActionsOfAI.InitAction(skillData, this, this.casterID);
        }
        this.castingActionsOfAI.allTargetIDs = targets;
        this.SetState(RealTimeSkillState.AICasting);
    }

    private int GetMainTarget()
    {
        if (this.mainTargetID >= 0)
        {
            return this.mainTargetID;
        }
        return ((this.allTargetIDs.Count <= 0) ? -1 : this.allTargetIDs[0]);
    }

    public BattleFighter GetTarget(int _id)
    {
        return this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(_id);
    }

    public void Init(int _skillEntry, int _skillID, BattleData _battleGameData, int _casterID, List<int> _targetID, float _moveCollsionDis)
    {
        this.skillEntry = _skillEntry;
        this.skillID = _skillID;
        this.casterID = _casterID;
        this.allTargetIDs = _targetID;
        this.lastTargetID = -1;
        this.mainTargetID = -1;
        this.battleGameData = _battleGameData;
        this.moveCollsionDis = _moveCollsionDis;
        if (_targetID.Count > 0)
        {
            this.mainTargetID = _targetID[0];
        }
        this.skillConfig = ConfigMgr.getInstance().getByEntry<skill_config>(this.skillEntry);
        if ((this.skillConfig != null) && !string.IsNullOrEmpty(this.skillConfig.effects))
        {
            this.InitEffectByName(this.skillConfig.effects);
            base.gameObject.name = base.gameObject.name + "  " + this.skillConfig.effects;
        }
    }

    public void InitEffect(RealTimeSkillInfo info)
    {
        if (info != null)
        {
            this.skillInfo = info;
            BattleFighter fighter = this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(this.casterID);
            this.actions = new List<RealTimeSkillActionBase>();
            foreach (RealTimeSkillActionInfo info2 in info.actionInfoes)
            {
                this.actions.Add(RealTimeSkillActionBase.CreateAction(this, info2, this.battleGameData, fighter, this.casterID, this.allTargetIDs, this.moveCollsionDis, info.oneByOne, info.isNeedTurn));
            }
            ActionGroup item = null;
            bool flag = false;
            for (int i = 0; i < this.actions.Count; i++)
            {
                RealTimeSkillActionBase base2 = this.actions[i];
                if (base2.info.isCastBegin)
                {
                    flag = true;
                }
                if (item == null)
                {
                    item = new ActionGroup();
                }
                bool isCastEnd = base2.info.isCastEnd;
                if ((base2.info.logicEffectInfoIndex >= 0) && !flag)
                {
                    flag = true;
                    isCastEnd = true;
                }
                if (flag)
                {
                    item.castingActions.Add(base2);
                }
                else
                {
                    item.prepareActions.Add(base2);
                }
                if (isCastEnd)
                {
                    flag = false;
                    this.groups.Add(item);
                    item = null;
                }
            }
            if (item != null)
            {
                this.groups.Add(item);
            }
        }
    }

    private void InitEffectByName(string effectName)
    {
        this.InitEffect(RealSkillInfoManager.GetSkillData(effectName));
    }

    public bool IsFinish()
    {
        return (this.state == RealTimeSkillState.Finished);
    }

    public void OnFinish()
    {
        this.OnFinishShowTime();
    }

    public void OnFinishShowTime()
    {
        if (this.isShowTiming)
        {
            base.StopCoroutine("OnShowTimeProcess");
            TimeManager.GetInstance().ClearShowTimeObj();
            if (GUIMgr.Instance.GetGUIEntity<BattlePausePanel>() == null)
            {
                this.battleGameData.timeScale_ShowTime = 1f;
            }
            this.isShowTiming = false;
        }
    }

    [DebuggerHidden]
    private IEnumerator OnShowTimeProcess()
    {
        return new <OnShowTimeProcess>c__Iterator2C { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator OnStartProcess()
    {
        return new <OnStartProcess>c__Iterator2B { <>f__this = this };
    }

    public void OnStartShowTime()
    {
        TimeManager.GetInstance().ClearShowTimeObj();
        TimeManager.GetInstance().AddShowTimeObj(this.casterID);
        this.battleGameData.timeScale_ShowTime = BattleGlobal.PauseTimeScale;
        this.isShowTiming = true;
    }

    public SkillEffectResult OnSubSkillTakeEffect(int index, int _targetID)
    {
        SkillEffectResult result = this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().DoSkillLogicEffectResult(_targetID, this.skillID, index);
        if ((BattleSceneStarter.G_isTestEnable && BattleSceneStarter.G_isTestSkill) && !string.IsNullOrEmpty(this.skillInfo.testBufferName))
        {
            BattleFighter fighter = this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(_targetID);
            if (fighter != null)
            {
                RealTimeBuffer newBuffer = new RealTimeBuffer();
                newBuffer.InitTest(this.skillInfo.testBufferName, fighter);
                fighter.AddBuffer(newBuffer);
            }
        }
        return result;
    }

    public void SetState(RealTimeSkillState newState)
    {
        this.state = newState;
        BattleCom_Runtime component = this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
        component.SendSkillState(this.casterID, this.skillID, this.state);
        if (newState == RealTimeSkillState.Casting)
        {
            SkillEffectCastResult result = component.DoSkillCasting(this.skillID);
            if ((result != null) && (result.mainTargetList != null))
            {
                this.allTargetIDs = result.mainTargetList;
            }
        }
        else if (newState == RealTimeSkillState.AICastStart)
        {
            component.OnSkillAICastStart(this.skillID);
        }
        else if (newState == RealTimeSkillState.AICastingFinished)
        {
            component.OnSkillAICastFinish(this.skillID);
        }
    }

    public void StartProcess()
    {
        if (this.actions == null)
        {
            this.SetState(RealTimeSkillState.Casting);
            this.allTargetIDs.ForEach(obj => this.OnSubSkillTakeEffect(0, obj));
            this.SetState(RealTimeSkillState.Finished);
        }
        else if (this.skillInfo != null)
        {
            base.StartCoroutine(this.OnStartProcess());
        }
    }

    public void Stop()
    {
        if (this.curAction != null)
        {
            this.curAction.Break();
        }
        BattleFighter fighter = this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(this.casterID);
        GameObject animObj = fighter.GetAnimObj();
        if (animObj != null)
        {
            MaterialFSM component = animObj.GetComponent<MaterialFSM>();
            if (component != null)
            {
                component.StartChangeMaterialTemp(MaterialFSM.MaterialBeHurt, BattleGlobal.ScaleTime(0.1f));
            }
        }
        if (fighter.GetAnimObj() != null)
        {
            fighter.GetAnimObj().GetComponent<AnimFSM>().StopCurAnimForce();
        }
        this.OnFinishShowTime();
        this.SetState(RealTimeSkillState.Finished);
        this.Clear();
        UnityEngine.Object.Destroy(base.gameObject);
    }

    [CompilerGenerated]
    private sealed class <OnShowTimeProcess>c__Iterator2C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkill <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.<>f__this.skillInfo.ShowTimeTime <= 0f)
                    {
                        break;
                    }
                    this.<>f__this.battleGameData.OnMsgSkillShowTimeClean();
                    this.<>f__this.OnStartShowTime();
                    this.$current = this.<>f__this.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.skillInfo.ShowTimeTime, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.OnFinishShowTime();
                    break;

                default:
                    goto Label_00B4;
            }
            this.$PC = -1;
        Label_00B4:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnStartProcess>c__Iterator2B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_293>__1;
        internal List<ActionGroup>.Enumerator <$s_294>__5;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_295>__7;
        internal List<int>.Enumerator <$s_296>__9;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_297>__11;
        internal List<RealTimeSkillActionBase>.Enumerator <$s_298>__13;
        internal RealTimeSkill <>f__this;
        internal RealTimeSkillActionBase <action>__12;
        internal RealTimeSkillActionBase <action>__14;
        internal RealTimeSkillActionBase <action>__2;
        internal RealTimeSkillActionBase <action>__8;
        internal BattleFighter <actor>__4;
        internal BattleCom_FighterManager <fightManager>__3;
        internal ActionGroup <group>__6;
        internal bool <isShowTimeing>__0;
        internal int <oneTargetID>__10;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                case 2:
                    try
                    {
                        switch (num)
                        {
                            case 1:
                                try
                                {
                                }
                                finally
                                {
                                    this.<$s_295>__7.Dispose();
                                }
                                return;

                            case 2:
                                try
                                {
                                    try
                                    {
                                    }
                                    finally
                                    {
                                        this.<$s_297>__11.Dispose();
                                    }
                                }
                                finally
                                {
                                    this.<$s_296>__9.Dispose();
                                }
                                return;
                        }
                    }
                    finally
                    {
                        this.<$s_294>__5.Dispose();
                    }
                    break;

                case 5:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_298>__13.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<isShowTimeing>__0 = this.<>f__this.skillInfo.ShowTimeTime > 0f;
                    if (this.<isShowTimeing>__0)
                    {
                        this.<>f__this.StartCoroutine(this.<>f__this.OnShowTimeProcess());
                    }
                    this.<$s_293>__1 = this.<>f__this.actions.GetEnumerator();
                    try
                    {
                        while (this.<$s_293>__1.MoveNext())
                        {
                            this.<action>__2 = this.<$s_293>__1.Current;
                            this.<action>__2.Prepare();
                        }
                    }
                    finally
                    {
                        this.<$s_293>__1.Dispose();
                    }
                    if (this.<>f__this.skillInfo.isTeamDirection)
                    {
                        this.<fightManager>__3 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                        this.<actor>__4 = this.<fightManager>__3.GetFighter(this.<>f__this.casterID);
                        if (this.<actor>__4 != null)
                        {
                            this.<actor>__4.transform.rotation = !this.<actor>__4.isPlayer ? Quaternion.LookRotation(-this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase()) : Quaternion.LookRotation(this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase());
                        }
                    }
                    this.<>f__this.curAction = null;
                    this.<$s_294>__5 = this.<>f__this.groups.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    goto Label_0481;

                case 4:
                    goto Label_050A;

                case 5:
                    goto Label_0533;

                default:
                    goto Label_05C7;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        goto Label_01F8;

                    case 2:
                        goto Label_02CB;
                }
                while (this.<$s_294>__5.MoveNext())
                {
                    this.<group>__6 = this.<$s_294>__5.Current;
                    this.<$s_295>__7 = this.<group>__6.prepareActions.GetEnumerator();
                    num = 0xfffffffd;
                Label_01F8:
                    try
                    {
                        while (this.<$s_295>__7.MoveNext())
                        {
                            this.<action>__8 = this.<$s_295>__7.Current;
                            this.<>f__this.curAction = this.<action>__8;
                            this.$current = this.<>f__this.StartCoroutine(this.<action>__8.StartProcess(this.<>f__this, this.<>f__this.GetMainTarget(), this.<>f__this.GetTarget(this.<>f__this.GetMainTarget())));
                            this.$PC = 1;
                            flag = true;
                            goto Label_05C9;
                        }
                    }
                    finally
                    {
                        if (!flag)
                        {
                        }
                        this.<$s_295>__7.Dispose();
                    }
                    this.<>f__this.SetState(RealTimeSkillState.Casting);
                    this.<$s_296>__9 = this.<>f__this.allTargetIDs.GetEnumerator();
                    num = 0xfffffffd;
                Label_02CB:
                    try
                    {
                        switch (num)
                        {
                            case 2:
                                goto Label_0306;
                        }
                        while (this.<$s_296>__9.MoveNext())
                        {
                            this.<oneTargetID>__10 = this.<$s_296>__9.Current;
                            this.<$s_297>__11 = this.<group>__6.castingActions.GetEnumerator();
                            num = 0xfffffffd;
                        Label_0306:
                            try
                            {
                                while (this.<$s_297>__11.MoveNext())
                                {
                                    this.<action>__12 = this.<$s_297>__11.Current;
                                    this.<>f__this.curAction = this.<action>__12;
                                    this.$current = this.<>f__this.StartCoroutine(this.<action>__12.StartProcess(this.<>f__this, this.<oneTargetID>__10, this.<>f__this.GetTarget(this.<oneTargetID>__10)));
                                    this.$PC = 2;
                                    flag = true;
                                    goto Label_05C9;
                                }
                                continue;
                            }
                            finally
                            {
                                if (!flag)
                                {
                                }
                                this.<$s_297>__11.Dispose();
                            }
                        }
                        continue;
                    }
                    finally
                    {
                        if (!flag)
                        {
                        }
                        this.<$s_296>__9.Dispose();
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_294>__5.Dispose();
            }
            this.<>f__this.curAction = null;
            this.<>f__this.SetState(RealTimeSkillState.AICastStart);
            this.<>f__this.SetState(RealTimeSkillState.AICasting);
            while (this.<>f__this.state == RealTimeSkillState.AICasting)
            {
                if (this.<>f__this.castingActionsOfAI == null)
                {
                    goto Label_0496;
                }
                this.<>f__this.castingActionsOfAI.Init();
                this.<>f__this.StartCoroutine(this.<>f__this.castingActionsOfAI.StartProcess(this.<>f__this));
            Label_0481:
                while (!this.<>f__this.castingActionsOfAI.IsFinish())
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_05C9;
                }
            Label_0496:
                this.<>f__this.SetState(RealTimeSkillState.AICastingFinished);
            }
            if (this.<>f__this.castingActionsOfAI != null)
            {
                this.$current = this.<>f__this.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.castingActionsOfAI.afterTime, new int?(this.<>f__this.casterID)));
                this.$PC = 4;
                goto Label_05C9;
            }
        Label_050A:
            this.<>f__this.actions.Reverse();
            this.<$s_298>__13 = this.<>f__this.actions.GetEnumerator();
            num = 0xfffffffd;
        Label_0533:
            try
            {
                while (this.<$s_298>__13.MoveNext())
                {
                    this.<action>__14 = this.<$s_298>__13.Current;
                    this.$current = this.<>f__this.StartCoroutine(this.<action>__14.StartFinish(this.<>f__this));
                    this.$PC = 5;
                    flag = true;
                    goto Label_05C9;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_298>__13.Dispose();
            }
            this.<>f__this.SetState(RealTimeSkillState.Finished);
            goto Label_05C7;
            this.$PC = -1;
        Label_05C7:
            return false;
        Label_05C9:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

