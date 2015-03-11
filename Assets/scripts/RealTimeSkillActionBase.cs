using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionBase
{
    [CompilerGenerated]
    private static Action<SkillEffect> <>f__am$cache10;
    [CompilerGenerated]
    private static Action<SkillEffect> <>f__am$cache11;
    public SkillActionType actionType;
    protected BattleData battleGameData;
    protected BattleFighter caster;
    protected int casterID;
    public GameObject casterModel;
    protected List<SkillEffect> effectOfPlayed = new List<SkillEffect>();
    public RealTimeSkillActionInfo info;
    protected bool isNeedTurn;
    protected SkillEffectResult logicResult;
    protected float moveCollsionDis;
    protected bool oneByOne;
    protected RealTimeSkill ownerSkill;
    public int SkillID;
    protected BattleFighter target;
    protected int targetID;
    public GameObject targetModel;

    public virtual void Break()
    {
    }

    protected virtual void Clear()
    {
    }

    private void ClearEffectOnFinish()
    {
        if (<>f__am$cache10 == null)
        {
            <>f__am$cache10 = delegate (SkillEffect obj) {
                if (obj.info.isDestoryOnFinish && (obj.effect != null))
                {
                    ObjectManager.DestoryObj(obj.effect, obj.info.delayDestoryOnFinish);
                }
            };
        }
        this.effectOfPlayed.ForEach(<>f__am$cache10);
    }

    private void ClearEffectOnSkillFinish()
    {
        if (<>f__am$cache11 == null)
        {
            <>f__am$cache11 = delegate (SkillEffect obj) {
                if (obj.info.isDestoryOnSkillFinish && (obj.effect != null))
                {
                    ObjectManager.DestoryObj(obj.effect, obj.info.delayDestoryOnFinish);
                }
            };
        }
        this.effectOfPlayed.ForEach(<>f__am$cache11);
    }

    public static RealTimeSkillActionBase CreateAction(RealTimeSkill _skill, RealTimeSkillActionInfo _info, BattleData _battleGameData, BattleFighter _caster, int _casterIndex, List<int> _allTargetIDs, float _moveCollsionDis, bool oneByOne, bool isNeedTurn)
    {
        RealTimeSkillActionBase base2 = null;
        switch (_info.actionType)
        {
            case SkillActionType.PlayAnimAndEffect:
                base2 = new RealTimeSkillActionPlayAnimAndEffect();
                break;

            case SkillActionType.Bullet:
                base2 = new RealTimeSkillActionBullet();
                break;

            case SkillActionType.SceneEffect:
                base2 = new RealTimeSkillActionSceneEffect();
                break;

            case SkillActionType.Move:
                base2 = new RealTimeSkillActionMove();
                break;

            case SkillActionType.Camera:
                base2 = new RealTimeSkillActionCamera();
                break;

            case SkillActionType.ShowTime:
                base2 = new RealTimeSkillActionShowTime();
                break;

            case SkillActionType.Sound:
                base2 = new RealTimeSkillActionSound();
                break;

            case SkillActionType.ModelScale:
                base2 = new RealTimeSkillActionModelScale();
                break;

            case SkillActionType.ModelShadow:
                base2 = new RealTimeSkillActionModelShadow();
                break;

            case SkillActionType.Summon:
                base2 = new RealTimeSkillActionSummon();
                break;
        }
        base2.ownerSkill = _skill;
        base2.actionType = _info.actionType;
        base2.info = _info;
        base2.battleGameData = _battleGameData;
        base2.caster = _caster;
        base2.casterID = _casterIndex;
        base2.moveCollsionDis = _moveCollsionDis;
        base2.oneByOne = oneByOne;
        base2.isNeedTurn = isNeedTurn;
        return base2;
    }

    public void DoClear()
    {
        this.ClearEffectOnSkillFinish();
        this.Clear();
    }

    protected BattleFighter GetActionObj()
    {
        switch (this.info.playerType)
        {
            case SkillActionPlayerType.caster:
                return this.caster;

            case SkillActionPlayerType.target:
                return this.target;

            case SkillActionPlayerType.Summon:
                return this.GetSummonObj();
        }
        return null;
    }

    protected int GetActionObjIndex()
    {
        return ((this.info.playerType != SkillActionPlayerType.caster) ? this.targetID : this.casterID);
    }

    protected GameObject GetCasterModelObj()
    {
        if (this.casterModel != null)
        {
            return this.casterModel;
        }
        if (this.caster != null)
        {
            return this.caster.GetAnimObj();
        }
        return null;
    }

    protected GameObject GetModelObj()
    {
        if (this.casterModel != null)
        {
            return ((this.info.playerType != SkillActionPlayerType.caster) ? this.targetModel : this.casterModel);
        }
        BattleFighter actionObj = this.GetActionObj();
        if (actionObj != null)
        {
            return actionObj.modelControler.GetCurModel();
        }
        return null;
    }

    protected BattleFighter GetSummonObj()
    {
        if ((this.logicResult != null) && (this.logicResult.summonTargetID >= 0))
        {
            return this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(this.logicResult.summonTargetID);
        }
        return null;
    }

    protected GameObject GetTargetModelObj()
    {
        if (this.targetModel != null)
        {
            return this.targetModel;
        }
        if (this.target != null)
        {
            return this.target.GetAnimObj();
        }
        return null;
    }

    protected virtual bool IsDoLogicFrist()
    {
        return false;
    }

    [DebuggerHidden]
    protected virtual IEnumerator OnFinish(MonoBehaviour mb)
    {
        return new <OnFinish>c__Iterator31();
    }

    private void OnLogicEffect()
    {
        if ((this.info.logicEffectInfoIndex >= 0) && (this.ownerSkill != null))
        {
            this.logicResult = this.ownerSkill.OnSubSkillTakeEffect(this.info.logicEffectInfoIndex, this.targetID);
        }
    }

    protected virtual void OnPrepare()
    {
    }

    [DebuggerHidden]
    protected virtual IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator2F();
    }

    protected void PlayEffect(float distance, List<SkillEffect> effectOfPlayed)
    {
        if (SettingMgr.mInstance.IsEffectEnable)
        {
            GameObject modelObj = this.GetModelObj();
            if (modelObj != null)
            {
                <PlayEffect>c__AnonStorey155 storey = new <PlayEffect>c__AnonStorey155();
                using (List<SkillEffectInfo>.Enumerator enumerator = this.info.effectInfos.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        storey.data = enumerator.Current;
                        if ((storey.data.effectPrefab != null) && (storey.data.distance >= distance))
                        {
                            SkillEffect item = null;
                            if (effectOfPlayed != null)
                            {
                                if (effectOfPlayed.Find(new Predicate<SkillEffect>(storey.<>m__124)) != null)
                                {
                                    continue;
                                }
                                item = new SkillEffect {
                                    info = storey.data
                                };
                                effectOfPlayed.Add(item);
                            }
                            float delayTime = BattleGlobal.ScaleTime(storey.data.delayTime);
                            float durTime = BattleGlobal.ScaleTime(storey.data.durtTime);
                            GameObject obj3 = null;
                            if (storey.data.attachType == SkillEffectAttachType.ONE_AttachTo)
                            {
                                obj3 = modelObj.GetComponent<HangControler>().AttachByPrefab(storey.data.effectPrefab, storey.data.hangPoint, durTime, delayTime, storey.data.offset, true, false);
                            }
                            else if (storey.data.attachType == SkillEffectAttachType.ONE_HangPointPos)
                            {
                                obj3 = modelObj.GetComponent<HangControler>().PlaceEffectByPrefab(storey.data.effectPrefab, storey.data.hangPoint, durTime, delayTime, storey.data.offset);
                            }
                            else if (storey.data.attachType == SkillEffectAttachType.ONE_HangPointProjectPos)
                            {
                                obj3 = modelObj.GetComponent<HangControler>().PlaceEffectByPrefab(storey.data.effectPrefab, storey.data.hangPoint, durTime, delayTime, storey.data.offset);
                                GameObject casterModelObj = this.GetCasterModelObj();
                                if (casterModelObj != null)
                                {
                                    Vector3 position = casterModelObj.transform.position;
                                    obj3.transform.position = new Vector3(obj3.transform.position.x, position.y, obj3.transform.position.z);
                                }
                            }
                            else if (storey.data.attachType == SkillEffectAttachType.Screen)
                            {
                                obj3 = BattleGlobalFunc.AttachEffectToScreen(storey.data.effectPrefab, storey.data.delayTime, storey.data.durtTime);
                            }
                            else if (storey.data.attachType == SkillEffectAttachType.TeamDir)
                            {
                                Vector3 pos = modelObj.transform.TransformPoint(storey.data.offset);
                                obj3 = ObjectManager.InstantiateObj(storey.data.effectPrefab);
                                ObjectManager.CreateTempObj(obj3, pos, storey.data.durtTime, storey.data.delayTime);
                            }
                            else if (storey.data.attachType == SkillEffectAttachType.ALL_Column)
                            {
                                List<Vector3> scenePathByPhase = this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetScenePathByPhase();
                                if ((scenePathByPhase != null) && (scenePathByPhase.Count > 0))
                                {
                                    Vector3 vector3 = scenePathByPhase[scenePathByPhase.Count - 1];
                                    Vector3 vector = modelObj.transform.position - vector3;
                                    Vector3 sceneFighterDirByPhase = this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
                                    vector = Vector3.Project(vector, sceneFighterDirByPhase);
                                    Vector3 vector6 = vector3 + vector;
                                    obj3 = ObjectManager.InstantiateObj(storey.data.effectPrefab);
                                    ObjectManager.CreateTempObj(obj3, vector6 + storey.data.offset, storey.data.durtTime, storey.data.delayTime);
                                }
                                else
                                {
                                    obj3 = modelObj.GetComponent<HangControler>().PlaceEffectByPrefab(storey.data.effectPrefab, storey.data.hangPoint, durTime, delayTime, storey.data.offset);
                                }
                            }
                            if (obj3 != null)
                            {
                                ObjectManager.SetTempObjLinkeObjID(obj3, this.casterID);
                            }
                            if (item != null)
                            {
                                item.effect = obj3;
                            }
                        }
                    }
                }
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PlaySound(MonoBehaviour mb)
    {
        return new <PlaySound>c__Iterator2E { mb = mb, <$>mb = mb, <>f__this = this };
    }

    public void Prepare()
    {
        this.OnPrepare();
    }

    [DebuggerHidden]
    public IEnumerator StartFinish(MonoBehaviour mb)
    {
        return new <StartFinish>c__Iterator30 { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [DebuggerHidden]
    public IEnumerator StartProcess(MonoBehaviour mb, int newTargetID, BattleFighter newTarget)
    {
        return new <StartProcess>c__Iterator2D { newTargetID = newTargetID, newTarget = newTarget, mb = mb, <$>newTargetID = newTargetID, <$>newTarget = newTarget, <$>mb = mb, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnFinish>c__Iterator31 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if (this.$PC == 0)
            {
            }
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
    private sealed class <OnProcess>c__Iterator2F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if (this.$PC == 0)
            {
            }
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
    private sealed class <PlayEffect>c__AnonStorey155
    {
        internal SkillEffectInfo data;

        internal bool <>m__124(SkillEffect obj)
        {
            return (obj.info == this.data);
        }
    }

    [CompilerGenerated]
    private sealed class <PlaySound>c__Iterator2E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionBase <>f__this;
        internal MonoBehaviour mb;

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
                    if (this.<>f__this.info.soundDelayTime <= 0f)
                    {
                        break;
                    }
                    this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.info.soundDelayTime, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    break;

                default:
                    goto Label_00A4;
            }
            SoundManager.mInstance.PlaySFX(this.<>f__this.info.soundName);
            this.$PC = -1;
        Label_00A4:
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
    private sealed class <StartFinish>c__Iterator30 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionBase <>f__this;
        internal MonoBehaviour mb;

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
                    this.<>f__this.ClearEffectOnSkillFinish();
                    this.$current = this.mb.StartCoroutine(this.<>f__this.OnFinish(this.mb));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.$PC = -1;
                    break;
            }
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
    private sealed class <StartProcess>c__Iterator2D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal BattleFighter <$>newTarget;
        internal int <$>newTargetID;
        internal RealTimeSkillActionBase <>f__this;
        internal MonoBehaviour mb;
        internal BattleFighter newTarget;
        internal int newTargetID;

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
                    this.<>f__this.targetID = this.newTargetID;
                    this.<>f__this.target = this.newTarget;
                    this.<>f__this.effectOfPlayed.Clear();
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0105;

                case 3:
                case 4:
                    if (TimeManager.GetInstance().IsShowTimeObj(this.<>f__this.casterID) && (BattleGlobal.GetShowTimeScale() < 0.1f))
                    {
                        this.$current = null;
                        this.$PC = 4;
                        goto Label_0256;
                    }
                    this.<>f__this.ClearEffectOnFinish();
                    if (!this.<>f__this.IsDoLogicFrist())
                    {
                        this.<>f__this.OnLogicEffect();
                    }
                    if (this.<>f__this.info.afterTime > 0f)
                    {
                        this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.info.afterTime, new int?(this.<>f__this.casterID)));
                        this.$PC = 5;
                        goto Label_0256;
                    }
                    goto Label_024D;

                case 5:
                    goto Label_024D;

                default:
                    goto Label_0254;
            }
            if (TimeManager.GetInstance().IsShowTimeObj(this.<>f__this.casterID) && (BattleGlobal.GetShowTimeScale() < 0.1f))
            {
                this.$current = null;
                this.$PC = 1;
                goto Label_0256;
            }
            if (this.<>f__this.info.delayTime > 0f)
            {
                this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.info.delayTime, new int?(this.<>f__this.casterID)));
                this.$PC = 2;
                goto Label_0256;
            }
        Label_0105:
            if (this.<>f__this.IsDoLogicFrist())
            {
                this.<>f__this.OnLogicEffect();
            }
            if (!string.IsNullOrEmpty(this.<>f__this.info.soundName))
            {
                this.mb.StartCoroutine(this.<>f__this.PlaySound(this.mb));
            }
            this.$current = this.mb.StartCoroutine(this.<>f__this.OnProcess(this.mb));
            this.$PC = 3;
            goto Label_0256;
        Label_024D:
            this.$PC = -1;
        Label_0254:
            return false;
        Label_0256:
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

