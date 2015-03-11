using Battle;
using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleFighter : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<RealTimeBuffer> <>f__am$cache17;
    [CompilerGenerated]
    private static Action<RealTimeShowEffect> <>f__am$cache18;
    private string avatarModelName;
    public BattleData battleGameData;
    private List<RealTimeBuffer> buffers = new List<RealTimeBuffer>();
    public int CardEntry = -1;
    public CombatDetailActor detailActor;
    public MonsterDrop dropInfo;
    public TssSdtInt Energy;
    public CombatEquip equip;
    public TssSdtLong HP;
    private bool isBegin;
    private bool isDead;
    public bool isDeadFinish;
    public TssSdtLong MaxHP;
    public ModelControler modelControler = new ModelControler();
    public MovementControler moveControler = new MovementControler();
    private float scaleTarget;
    private float scaleTargetVelocity;
    private List<RealTimeShowEffect> showEffects = new List<RealTimeShowEffect>();

    public void AddBuffer(RealTimeBuffer newBuffer)
    {
        this.buffers.Add(newBuffer);
        newBuffer.OnAttach();
    }

    public void AddScale(float _scale)
    {
        this.modelControler.AddScale(_scale);
    }

    private void CancelOnDead()
    {
        base.StopAllCoroutines();
    }

    public void ChangeToAvatar()
    {
        if (!string.IsNullOrEmpty(this.avatarModelName))
        {
            this.modelControler.ShowModel(this.avatarModelName, false);
        }
    }

    public void ClearBuffer()
    {
        if (<>f__am$cache17 == null)
        {
            <>f__am$cache17 = delegate (RealTimeBuffer obj) {
                if (obj != null)
                {
                    obj.OnRemove();
                }
            };
        }
        this.buffers.ForEach(<>f__am$cache17);
        this.buffers.Clear();
        this.ClearShowEffects();
    }

    public void ClearBufferOnPhase()
    {
        <ClearBufferOnPhase>c__AnonStoreyE8 ye = new <ClearBufferOnPhase>c__AnonStoreyE8 {
            <>f__this = this,
            deleteList = new List<RealTimeBuffer>()
        };
        this.buffers.ForEach(new Action<RealTimeBuffer>(ye.<>m__53));
        ye.deleteList.ForEach(new Action<RealTimeBuffer>(ye.<>m__54));
        this.ClearShowEffects();
    }

    private void ClearResource()
    {
        this.modelControler.Clear();
    }

    private void ClearShowEffects()
    {
        if (<>f__am$cache18 == null)
        {
            <>f__am$cache18 = delegate (RealTimeShowEffect obj) {
                if (obj != null)
                {
                    obj.Clear();
                    ObjectManager.DestoryObj(obj.gameObject);
                }
            };
        }
        this.showEffects.ForEach(<>f__am$cache18);
        this.showEffects.Clear();
    }

    [DebuggerHidden]
    private IEnumerator DoShowReviveObj(long _hp, int _energy)
    {
        return new <DoShowReviveObj>c__Iterator10 { <>f__this = this };
    }

    public GameObject GetAnimObj()
    {
        return this.modelControler.GetCurModel();
    }

    public Vector3 GetAnimObjDir()
    {
        return this.GetAnimObj().transform.forward;
    }

    public RealTimeBuffer GetBufferByEntry(int entry)
    {
        <GetBufferByEntry>c__AnonStoreyE9 ye = new <GetBufferByEntry>c__AnonStoreyE9 {
            entry = entry
        };
        return this.buffers.Find(new Predicate<RealTimeBuffer>(ye.<>m__57));
    }

    public HangControler GetHang()
    {
        return this.GetAnimObj().GetComponent<HangControler>();
    }

    public int GetIndexAtLive()
    {
        return this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetIndexAtLive(this);
    }

    public void HideTalkPop()
    {
    }

    public void Init(BattleData _data, float _scale, bool _isPlayer, int _cardEntry, CombatDetailActor _detailActor)
    {
        this.battleGameData = _data;
        this.CardEntry = _cardEntry;
        this.isPlayer = _isPlayer;
        this.MaxHP = (TssSdtLong) _detailActor.maxHp;
        this.HP = (TssSdtLong) _detailActor.curHp;
        this.Energy = _detailActor.energy;
        this.equip = _detailActor.equip;
        this.Scale = _scale;
        this.detailActor = _detailActor;
        this.isBegin = false;
        this.IsBigBoss = false;
        GameObject defaultAnimObj = CardPlayer.CreateCardPlayer(_cardEntry, this.equip, CardPlayerStateType.Battle, _detailActor.quality);
        if (defaultAnimObj == null)
        {
            defaultAnimObj = CardPlayer.CreateCardPlayer(0, this.equip, CardPlayerStateType.Battle, 0);
        }
        this.modelControler.Init(base.gameObject, defaultAnimObj);
        this.modelControler.OnModelChangeEvent = (System.Action) Delegate.Combine(this.modelControler.OnModelChangeEvent, new System.Action(this.OnChangeModel));
        if (this.isPlayer)
        {
            base.gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        this.OnMsgTimeScaleChange();
        float radius = 1f;
        if (_detailActor != null)
        {
            radius = _detailActor.radius;
        }
        else
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_cardEntry);
            if (_config != null)
            {
                if (!string.IsNullOrEmpty(_config.avatar_model))
                {
                    this.avatarModelName = _config.avatar_model;
                }
                radius = _config.radius;
            }
            else
            {
                Debug.LogWarning("can't find card_config " + _cardEntry.ToString());
            }
        }
        this.moveControler.Init(radius, base.gameObject);
        this.moveControler.SetModelScale(this.Scale);
        this.moveControler.SetAnimObj(defaultAnimObj);
        this.moveControler.OnMoveFinishEvent = (System.Action) Delegate.Combine(this.moveControler.OnMoveFinishEvent, new System.Action(this.OnMoveFinishEvent));
        base.gameObject.transform.localScale = new Vector3(this.Scale, this.Scale, this.Scale);
        this.detailActor.radius *= this.Scale;
        this.SetHp((long) this.HP);
        this.isBegin = true;
    }

    public bool IsDead()
    {
        return this.isDead;
    }

    public void OnAttackAwayFinish()
    {
        this.moveControler.SetCollsionEnable(true);
        this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().OnActorAttackAwayFinish(this.PosIndex);
    }

    private void OnChangeModel()
    {
        this.moveControler.SetAnimObj(this.modelControler.GetCurModel());
    }

    private void OnDead()
    {
        this.isDead = true;
        this.moveControler.SetDead(true);
        this.ClearBuffer();
        base.StartCoroutine(this.StartOnDead());
    }

    public void OnDestory()
    {
        this.ClearBuffer();
        this.ClearResource();
        base.StopAllCoroutines();
        if (this.battleGameData.OnMsgFighterDestory != null)
        {
            this.battleGameData.OnMsgFighterDestory(this.PosIndex);
        }
    }

    public void OnDoDispear()
    {
        this.SetAnimObjVisible(false);
    }

    private void OnMoveFinishEvent()
    {
        if (this.battleGameData.OnMsgPlayerMoveFinished != null)
        {
            this.battleGameData.OnMsgPlayerMoveFinished(this.PosIndex);
        }
    }

    private void OnMsgTimeScaleChange()
    {
        bool isShowTime = TimeManager.GetInstance().IsShowTimeObj(this.PosIndex);
        this.SetTimeSpeed(isShowTime);
    }

    private void OnScaleTick()
    {
        this.modelControler.OnScaleTick();
    }

    public void OnStateBuffChange()
    {
        bool isPause = false;
        foreach (RealTimeBuffer buffer in this.buffers)
        {
            if (buffer.IsNeedPauseAnim())
            {
                isPause = true;
                break;
            }
        }
        this.modelControler.SetPasueAnim(isPause);
    }

    public void OnSubShowEffect(string effectName)
    {
        RealTimeShowEffect item = RealTimeShowEffect.CreateNewShowEffect(effectName, this.PosIndex, this.battleGameData);
        item.ToDoStart();
        this.showEffects.Add(item);
    }

    public void ProcessSkillResult(SkillEffectResult result)
    {
        if (!this.IsDead())
        {
            if (result.IsHpChange())
            {
                base.StartCoroutine(this.ShowHeadTipUITemp());
                this.SetHp(result.value);
                result.value = (long) this.HP;
            }
            else if (result.IsEnergyChange())
            {
                if (result.energyChangeType == HitEnergyType.Disable)
                {
                    return;
                }
                this.SetEnergy((int) result.value);
                result.value = (long) this.Energy;
                if (result.energyChangeType == HitEnergyType.RestoreOnBattleWin)
                {
                    BattleSecurityManager.Instance.AddEnergyChangeData(this.ServerIdx, result.energyChangeType, result.changeValue);
                }
            }
            if (this.battleGameData.OnMsgFighterInfoChange != null)
            {
                this.battleGameData.OnMsgFighterInfoChange(this.PosIndex, result);
            }
        }
    }

    public void RemoveAvatar()
    {
        if (!string.IsNullOrEmpty(this.avatarModelName))
        {
            this.modelControler.DeleteModel(this.avatarModelName);
        }
    }

    public void RemoveBuffer(int bufferEntry)
    {
        <RemoveBuffer>c__AnonStoreyE6 ye = new <RemoveBuffer>c__AnonStoreyE6 {
            bufferEntry = bufferEntry
        };
        RealTimeBuffer item = this.buffers.Find(new Predicate<RealTimeBuffer>(ye.<>m__51));
        if (item != null)
        {
            this.buffers.Remove(item);
            item.OnRemove();
        }
    }

    public void RestoreHPEnergy()
    {
        int hpRecover = this.detailActor.hpRecover;
        SkillEffectResult result = new SkillEffectResult(SkillHitType.Hit, (long) (this.HP + hpRecover)) {
            changeValue = hpRecover,
            effectType = SubSkillType.Heal,
            hpChangeType = HitHpType.RestoreOnBattleWin
        };
        this.ProcessSkillResult(result);
        long hP = (long) this.HP;
        this.detailActor.curHp = (ulong) hP;
        BattleSecurityManager.Instance.AddHpChangeData(this.ServerIdx, ChangeHpType.HpType_RestoreOnBattleWin, result.changeValue);
        int energyRecover = this.detailActor.energyRecover;
        SkillEffectResult result2 = new SkillEffectResult(SkillHitType.Hit, (long) (this.Energy + energyRecover)) {
            changeValue = energyRecover,
            effectType = SubSkillType.AddEnergy,
            energyChangeType = HitEnergyType.RestoreOnBattleWin
        };
        this.ProcessSkillResult(result2);
        this.detailActor.energy = (int) this.Energy;
    }

    public void Revive(long _hp, int _energy)
    {
        SoundManager.mInstance.PlaySFX("battle_revive");
        this.isDead = false;
        this.isDeadFinish = false;
        this.CancelOnDead();
        this.moveControler.SetDead(false);
        base.StartCoroutine(this.DoShowReviveObj(_hp, _energy));
        this.SetHp(_hp);
        this.SetEnergy(_energy);
        if (this.battleGameData.OnMsgFighterInfoChange != null)
        {
            this.battleGameData.OnMsgFighterInfoChange(this.PosIndex, null);
        }
    }

    private void SetAnimObjVisible(bool visible)
    {
        this.ShowHeadTipUI(visible);
        this.modelControler.GetCurModel().SetActive(visible);
    }

    public void SetBufferEffectActive(int bufferEntry, string objName, bool active)
    {
        <SetBufferEffectActive>c__AnonStoreyE7 ye = new <SetBufferEffectActive>c__AnonStoreyE7 {
            bufferEntry = bufferEntry
        };
        RealTimeBuffer buffer = this.buffers.Find(new Predicate<RealTimeBuffer>(ye.<>m__52));
        if (buffer != null)
        {
            buffer.SetEffectActive(objName, active);
        }
    }

    public void SetDead()
    {
        this.HP = 0L;
        this.OnDead();
        this.moveControler.StopMove();
    }

    public void SetEnergy(int _energy)
    {
        this.Energy = _energy;
        this.Energy = Mathf.Clamp((int) this.Energy, 0, AiDef.MAX_ENERGY);
    }

    public void SetHp(long _hp)
    {
        this.HP = _hp;
        if (this.HP < 0L)
        {
            this.HP = 0L;
        }
        if (this.HP > this.MaxHP)
        {
            this.HP = this.MaxHP;
        }
        if (this.HP == 0L)
        {
            this.SetDead();
        }
    }

    public void SetMaxHP(long _maxHP)
    {
        this.MaxHP = _maxHP;
    }

    public void SetPosition(Vector3 pos)
    {
        this.moveControler.SetPosForce(pos);
    }

    public void SetTimeSpeed(bool isShowTime)
    {
        float speed = !isShowTime ? BattleGlobal.ScaleSpeed(1f) : BattleGlobal.ScaleSpeed_ShowTime(1f);
        this.modelControler.SetAnimSpeed(speed);
        this.moveControler.SetShowTimeSpeedScale(speed);
    }

    private void ShowHeadTipUI(bool show)
    {
        if (this.battleGameData.OnMsgFighterUIVisible != null)
        {
            this.battleGameData.OnMsgFighterUIVisible(this.PosIndex, show);
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowHeadTipUITemp()
    {
        return new <ShowHeadTipUITemp>c__IteratorF { <>f__this = this };
    }

    private void ShowOnDeadTip()
    {
    }

    public void ShowTalkPop(string text)
    {
    }

    public void StartBattle()
    {
        this.moveControler.StartBattle();
    }

    [DebuggerHidden]
    private IEnumerator StartOnDead()
    {
        return new <StartOnDead>c__IteratorE { <>f__this = this };
    }

    private void Update()
    {
        this.moveControler.Tick();
        this.OnScaleTick();
    }

    public bool IsBigBoss { get; set; }

    public bool isPlayer { get; private set; }

    public int PosIndex { get; set; }

    public float Scale { get; private set; }

    public int ServerIdx { get; set; }

    [CompilerGenerated]
    private sealed class <ClearBufferOnPhase>c__AnonStoreyE8
    {
        internal BattleFighter <>f__this;
        internal List<RealTimeBuffer> deleteList;

        internal void <>m__53(RealTimeBuffer obj)
        {
            if ((obj != null) && !obj.isCanPastToNextPhase())
            {
                obj.OnRemove();
                this.deleteList.Add(obj);
            }
        }

        internal void <>m__54(RealTimeBuffer obj)
        {
            this.<>f__this.buffers.Remove(obj);
        }
    }

    [CompilerGenerated]
    private sealed class <DoShowReviveObj>c__Iterator10 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleFighter <>f__this;
        internal MaterialFSM <materialControl>__0;

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
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<materialControl>__0 = this.<>f__this.GetAnimObj().GetComponent<MaterialFSM>();
                    this.<materialControl>__0.ResetMaterial();
                    this.<materialControl>__0.ResetColor();
                    this.<>f__this.GetAnimObj().GetComponent<AnimFSM>().PlayAnim(BattleGlobal.ReviveAnimName, 1f, 0f, false);
                    this.<>f__this.SetAnimObjVisible(true);
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
    private sealed class <GetBufferByEntry>c__AnonStoreyE9
    {
        internal int entry;

        internal bool <>m__57(RealTimeBuffer obj)
        {
            return (obj.bufferEntry == this.entry);
        }
    }

    [CompilerGenerated]
    private sealed class <RemoveBuffer>c__AnonStoreyE6
    {
        internal int bufferEntry;

        internal bool <>m__51(RealTimeBuffer obj)
        {
            return (obj.bufferEntry == this.bufferEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <SetBufferEffectActive>c__AnonStoreyE7
    {
        internal int bufferEntry;

        internal bool <>m__52(RealTimeBuffer obj)
        {
            return (obj.bufferEntry == this.bufferEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowHeadTipUITemp>c__IteratorF : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleFighter <>f__this;

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
                    this.<>f__this.ShowHeadTipUI(true);
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.ShowHeadTipUI(false);
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
    private sealed class <StartOnDead>c__IteratorE : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <_animObj>__0;
        internal BattleFighter <>f__this;
        internal MaterialFSM <materialControl>__2;
        internal MaterialFSM <materialControl>__3;
        internal MaterialFSM <materialControl>__4;
        internal MaterialFSM <materialControl>__5;
        internal float <time>__1;

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
                    this.<>f__this.ShowHeadTipUI(false);
                    this.<_animObj>__0 = this.<>f__this.modelControler.GetCurModel();
                    this.<_animObj>__0.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.DeadAnimName, 1f, 0f, false);
                    if (this.<>f__this.isBegin)
                    {
                        SoundManager.mInstance.PlaySFX(!this.<>f__this.isPlayer ? "battle_monsterDead" : "playerDead");
                    }
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 1;
                    goto Label_0281;

                case 1:
                    if (this.<>f__this.isDead)
                    {
                        if ((this.<_animObj>__0 != null) && this.<_animObj>__0.activeSelf)
                        {
                            this.<time>__1 = BattleGlobal.ScaleTime(0.5f);
                            this.<materialControl>__2 = this.<_animObj>__0.GetComponent<MaterialFSM>();
                            this.<materialControl>__2.StartAlphaChange(this.<time>__1, 0f);
                            this.$current = new WaitForSeconds(this.<time>__1);
                            this.$PC = 2;
                            goto Label_0281;
                        }
                        break;
                    }
                    goto Label_027F;

                case 2:
                    break;

                case 3:
                    if (this.<>f__this.isDead)
                    {
                        this.<>f__this.SetAnimObjVisible(false);
                        this.<>f__this.isDeadFinish = true;
                        this.$PC = -1;
                    }
                    else
                    {
                        this.<materialControl>__5 = this.<_animObj>__0.GetComponent<MaterialFSM>();
                        this.<materialControl>__5.ResetMaterial();
                    }
                    goto Label_027F;

                default:
                    goto Label_027F;
            }
            if (!this.<>f__this.isDead)
            {
                this.<materialControl>__3 = this.<_animObj>__0.GetComponent<MaterialFSM>();
                this.<materialControl>__3.ResetMaterial();
            }
            else
            {
                if ((this.<>f__this.dropInfo != null) && (this.<>f__this.dropInfo.items.Count > 0))
                {
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_PlayerControl>().OnDrop(this.<>f__this.dropInfo, this.<_animObj>__0.transform.position);
                }
                if (!this.<>f__this.isDead)
                {
                    this.<materialControl>__4 = this.<_animObj>__0.GetComponent<MaterialFSM>();
                    this.<materialControl>__4.ResetMaterial();
                }
                else
                {
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(0.5f));
                    this.$PC = 3;
                    goto Label_0281;
                }
            }
        Label_027F:
            return false;
        Label_0281:
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

