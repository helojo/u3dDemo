using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleData
{
    private bool _isAuto;
    private int _startPhase;
    private float _timeScale = 1f;
    private float _timeScale_ShowTime = 1f;
    [CompilerGenerated]
    private static Action<AudioSource> <>f__am$cache44;
    public List<CombatDetailActor> attActor;
    private TssSdtFloat battleTime = 0f;
    public List<int> buffLists;
    public Action<bool, BattleNormalGameType, BattleNormalGameResult> callBackFunc;
    public ShotState cameraShotType;
    public int CurBattlePhase = -1;
    public List<CombatTeam> defActor;
    public List<MonsterDrop> drops;
    public int dupID;
    public int gridActivityEntry = -1;
    public int gridDupEntry = -1;
    public int gridEliteEntry = -1;
    public OutlandMapData gridGameData;
    public int gridInitPlayer;
    public bool IsBossBattle;
    public bool isClearBoxReward;
    public bool isNewMap;
    public bool IsPass;
    public bool isPKMode;
    public bool isRealTimeBattleWin;
    public bool IsStoryBattle;
    public List<AudioSource> needClearSound = new List<AudioSource>();
    public BattleNormalGameType normalGameType;
    public Action<bool> OnMsgAutoChange;
    public System.Action OnMsgBattleForwardOnPassMode;
    public Action<bool, bool> OnMsgBattleGridTiggerResult;
    public Action<List<int>> OnMsgBattlePlayerBuff;
    public Action<bool> OnMsgBattleRunningChange;
    public Action<long, long> OnMsgBossHPChange;
    public System.Action OnMsgBreak;
    public Action<bool> OnMsgEnableControl;
    public System.Action OnMsgEnter;
    public System.Action OnMsgFighterChange;
    public Action<int> OnMsgFighterDestory;
    public Action<int, SkillEffectResult> OnMsgFighterInfoChange;
    public Action<int, bool> OnMsgFighterUIVisible;
    public Action<ItemType> OnMsgGetItem;
    public System.Action OnMsgGridBufferUpdate;
    public Action<bool, bool, BattleNormalGameResult> OnMsgGridGameFinishOneBattle;
    public System.Action OnMsgLeave;
    public Action<bool, bool> OnMsgOutlandCameraEnable;
    public System.Action OnMsgPhaseChange;
    public System.Action OnMsgPhaseStartFinish;
    public Action<int> OnMsgPlayerMoveFinished;
    public System.Action OnMsgSkillShowTimeClean;
    public System.Action OnMsgStart;
    public System.Action OnMsgStartRealBattle;
    public System.Action OnMsgTimeScaleChange;
    public Action<int, int> OnMsgUseSkill;
    public IExtensible packObj;
    public int phaseNumber;
    public int point;
    public uint randomSeed;
    public int round;
    public string startAnim;
    public long worldBossFinishedHP;
    public long worldBossInitHP;
    public bool worldBossIsKilled;
    public long worldBossOverHurtHP;

    public void ClearSound()
    {
        if (<>f__am$cache44 == null)
        {
            <>f__am$cache44 = delegate (AudioSource obj) {
                if (obj != null)
                {
                    obj.Stop();
                    UnityEngine.Object.Destroy(obj.gameObject);
                }
            };
        }
        this.needClearSound.ForEach(<>f__am$cache44);
        this.needClearSound.Clear();
    }

    public float GetBattleRemainTime()
    {
        return Mathf.Max((float) (90f - this.battleTime), (float) 0f);
    }

    public int GetBattleRemainTimeInt()
    {
        return Mathf.CeilToInt(this.GetBattleRemainTime());
    }

    public float GetBattletime()
    {
        return (float) this.battleTime;
    }

    public int GetBattletimeInt()
    {
        return Mathf.CeilToInt(this.GetBattletime());
    }

    public int GetCurScenePhase()
    {
        return (this.startPhase + this.CurBattlePhase);
    }

    public float GetGridEventProcess()
    {
        int count = this.gridGameData.events.Count;
        int num2 = 0;
        foreach (OutlandEvent event2 in this.gridGameData.events)
        {
            if (event2.state == 1)
            {
                num2++;
            }
        }
        if (num2 == (count - 1))
        {
            return 1f;
        }
        return Mathf.Clamp01(((float) num2) / ((float) count));
    }

    public void InitBattleValue()
    {
        this._isAuto = SettingMgr.mInstance.GetCommonBool("BATTLEAUTO");
        this._timeScale = SettingMgr.mInstance.GetCommonFloat("BATTLESCALE");
        if (this._timeScale <= 0f)
        {
            this._timeScale = 1f;
        }
        BattleGlobal.SetTimeScale(this._timeScale);
    }

    private bool IsAutoForce()
    {
        if (((this.normalGameType != BattleNormalGameType.PK) && (this.normalGameType != BattleNormalGameType.FriendPK)) && (((this.normalGameType != BattleNormalGameType.ArenaLadder) && (this.normalGameType != BattleNormalGameType.WorldCupPk)) && (this.normalGameType != BattleNormalGameType.GuildBattle)))
        {
            return false;
        }
        return true;
    }

    public bool IsCloseUpShot()
    {
        return ((this.cameraShotType >= ShotState.CloseUp1) && (this.cameraShotType <= ShotState.CloseUp10));
    }

    public bool IsDupBattle()
    {
        return ((this.normalGameType == BattleNormalGameType.Dup) || (this.normalGameType == BattleNormalGameType.DupElite));
    }

    public bool IsPKBattle()
    {
        return (((this.normalGameType == BattleNormalGameType.FriendPK) || (this.normalGameType == BattleNormalGameType.YuanZhengPk)) || (this.normalGameType == BattleNormalGameType.ArenaLadder));
    }

    public float OnBattletime(float deltaTime)
    {
        return (this.battleTime += deltaTime);
    }

    public float ResetBattletime()
    {
        return (this.battleTime = 0f);
    }

    public GameObject battleComObject { get; set; }

    public BattleGameType gameType { get; set; }

    public bool isAuto
    {
        get
        {
            return ((this._isAuto && this.isAutoEnable) || this.IsAutoForce());
        }
        set
        {
            this._isAuto = value;
            SettingMgr.mInstance.SetCommonBool("BATTLEAUTO", this._isAuto);
            if (this.OnMsgAutoChange != null)
            {
                this.OnMsgAutoChange(this._isAuto);
            }
        }
    }

    public bool isAutoEnable { get; set; }

    public bool IsKey { get; set; }

    public bool isSkipMode { get; set; }

    public int startPhase
    {
        get
        {
            return this._startPhase;
        }
        set
        {
            this._startPhase = value;
        }
    }

    public float timeScale
    {
        get
        {
            return this._timeScale;
        }
        set
        {
            this._timeScale = value;
            SettingMgr.mInstance.SetCommonFloat("BATTLESCALE", this._timeScale);
            BattleGlobal.SetTimeScale(this._timeScale);
            if (this.OnMsgTimeScaleChange != null)
            {
                this.OnMsgTimeScaleChange();
            }
        }
    }

    public float timeScale_ShowTime
    {
        get
        {
            return this._timeScale_ShowTime;
        }
        set
        {
            this._timeScale_ShowTime = value;
            BattleGlobal.SetShowTimeScale(this._timeScale_ShowTime);
            if (this.OnMsgTimeScaleChange != null)
            {
                this.OnMsgTimeScaleChange();
            }
        }
    }
}

