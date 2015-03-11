using FastBuf;
using LevelLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleState : MonoBehaviour
{
    private static BattleState _instance;
    [CompilerGenerated]
    private static Predicate<int> <>f__am$cache8;
    private Operation battleSceneAsync;
    private bool isEntered;
    public FastBuf.OutlandActivityData OutlandActivityData;

    public void DoNormalBattle(CombatCrewData combatData, List<MonsterDrop> drops, BattleNormalGameType gameType, bool autoEnable, string scene, int startPos, int visualType, string startAnim, IExtensible packObj, Action<bool, BattleNormalGameType, BattleNormalGameResult> callbackFunc)
    {
        SettingMgr.mInstance.SetScreenSleepOnBattle();
        BattleSecurityManager.Instance.Init();
        BattleStaticEntry.IsInBattle = true;
        Debug.Log("=====================????:" + combatData.attacker.actor.Count);
        this.SetGameType(BattleGameType.Normal);
        this.CurGame.battleGameData.attActor = combatData.attacker.actor;
        this.CurGame.battleGameData.defActor = combatData.defenderList;
        this.CurGame.battleGameData.drops = drops;
        this.CurGame.battleGameData.packObj = packObj;
        this.CurGame.battleGameData.randomSeed = combatData.seed;
        this.CurGame.battleGameData.phaseNumber = this.CurGame.battleGameData.defActor.Count;
        this.CurGame.battleGameData.startPhase = startPos;
        this.CurGame.battleGameData.cameraShotType = (ShotState) visualType;
        this.CurGame.battleGameData.IsStoryBattle = false;
        if (((gameType == BattleNormalGameType.PK) || (gameType == BattleNormalGameType.FriendPK)) || (((gameType == BattleNormalGameType.WarmmatchPk) || (gameType == BattleNormalGameType.WorldCupPk)) || (gameType == BattleNormalGameType.GuildBattle)))
        {
            this.CurGame.battleGameData.isAutoEnable = true;
        }
        else
        {
            this.CurGame.battleGameData.isAutoEnable = autoEnable;
        }
        this.CurGame.battleGameData.startAnim = startAnim;
        this.CurGame.battleGameData.CurBattlePhase = 0;
        this.CurGame.battleGameData.normalGameType = gameType;
        if (((visualType == 2) || (visualType == 4)) || (visualType == 6))
        {
            this.CurGame.battleGameData.IsBossBattle = true;
        }
        else
        {
            this.CurGame.battleGameData.IsBossBattle = false;
        }
        if (gameType == BattleNormalGameType.WorldBoss)
        {
            this.CurGame.battleGameData.IsBossBattle = true;
            this.CurGame.battleGameData.worldBossIsKilled = ((S2C_WorldBossCombat) packObj).is_end;
            this.CurGame.battleGameData.worldBossInitHP = (long) combatData.defenderList[0].actor[0].curHp;
        }
        else if (this.CurGame.battleGameData.IsDupBattle() && (packObj != null))
        {
            this.CurGame.battleGameData.dupID = ((S2C_DuplicateCombat) packObj).dupData.trenchEntry;
        }
        this.CurGame.battleGameData.callBackFunc = callbackFunc;
        base.StartCoroutine(this.StartDoBattle(scene));
    }

    public void EnterOutLand(FastBuf.OutlandActivityData data, int activityEntry, string scene, int initPlayerGrid, Action<bool, BattleNormalGameType, BattleNormalGameResult> callbackFunc, bool isNewMap)
    {
        this.SetGameType(BattleGameType.Grid);
        this.CurGame.battleGameData.gridGameData = data.data;
        this.CurGame.battleGameData.gridActivityEntry = activityEntry;
        this.CurGame.battleGameData.gridInitPlayer = initPlayerGrid;
        this.CurGame.battleGameData.IsKey = data.activity_data.floor_key;
        this.CurGame.battleGameData.callBackFunc = callbackFunc;
        this.CurGame.battleGameData.buffLists = data.data.buff_data;
        this.CurGame.battleGameData.isClearBoxReward = data.activity_data.clear_box_reward;
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = b => b == -1;
        }
        foreach (int num in this.CurGame.battleGameData.buffLists.FindAll(<>f__am$cache8).ToList<int>())
        {
            this.CurGame.battleGameData.buffLists.Remove(num);
        }
        this.CurGame.battleGameData.isNewMap = isNewMap;
        this.OutlandActivityData = data;
        base.StartCoroutine(this.StartDoBattleGrid(scene));
    }

    private void FixedUpdate()
    {
        if ((this.battleSceneAsync != null) && this.battleSceneAsync.IsDone())
        {
            this.battleSceneAsync = null;
            this.OnEnter();
        }
        ObjectManager.Instance.Tick();
        if ((this.CurGame != null) && this.isEntered)
        {
            this.CurGame.OnUpdate();
        }
    }

    public int GetBattleRemaintimeInt()
    {
        if (this.CurGame != null)
        {
            return this.CurGame.battleGameData.GetBattleRemainTimeInt();
        }
        return 0;
    }

    public float GetBattletime()
    {
        if (this.CurGame != null)
        {
            return this.CurGame.battleGameData.GetBattletime();
        }
        return 0f;
    }

    public int GetBattletimeInt()
    {
        if (this.CurGame != null)
        {
            return this.CurGame.battleGameData.GetBattletimeInt();
        }
        return 0;
    }

    public static BattleState GetInstance()
    {
        if (_instance == null)
        {
            GameObject target = new GameObject("BattleStateInstance");
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance = target.AddComponent<BattleState>();
            _instance.Init();
        }
        return _instance;
    }

    public static BattleNormalGame GetNormalGameInstance()
    {
        return ((GetInstance() == null) ? null : GetInstance().normalGame);
    }

    public string GetOutlandLayer()
    {
        if (!BattleSceneStarter.G_isTestEnable)
        {
            outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(this.CurGame.battleGameData.gridGameData.entry);
            outland_map_type_config _config2 = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(_config.outland_type);
            return string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), _config2.name, _config.layer);
        }
        return string.Empty;
    }

    private void Init()
    {
        this.normalGame = base.gameObject.AddComponent<BattleNormalGame>();
        this.normalGame.Init();
        this.gridGame = base.gameObject.AddComponent<BattleGridGame>();
        this.gridGame.Init();
    }

    public void OnEnter()
    {
        this.PrepareCommonResource();
        if (this.CurGame != null)
        {
            this.CurGame.OnEnter();
        }
        this.isEntered = true;
    }

    public void OnLeave()
    {
        SettingMgr.mInstance.SetScreenSleep();
        if (this.CurGame != null)
        {
            this.CurGame.OnLeave();
        }
        this.isEntered = false;
    }

    public void OnReset()
    {
        this.OnLeave();
        this.OnEnter();
    }

    private void PrepareCommonResource()
    {
        ObjectManager.SetCacheObjsRoot(new GameObject("CacheObjsRoot") { transform = { position = new Vector3(-100000f, -100000f, -10000f) } });
        SoundManager.mInstance.PlaySFX("playerDead", false, true);
        SoundManager.mInstance.PlaySFX("battle_monsterDead", false, true);
        SoundManager.mInstance.PlaySFX("battle_dropItem", false, true);
        SoundManager.mInstance.PlaySFX("battle_revive", false, true);
    }

    public void SetGameType(BattleGameType _type)
    {
        this.gameType = _type;
        if (this.gameType == BattleGameType.Normal)
        {
            this.CurGame = this.normalGame;
        }
        else
        {
            this.CurGame = this.gridGame;
        }
    }

    public void SetWaitBattle(Operation _async)
    {
        this.battleSceneAsync = _async;
    }

    [DebuggerHidden]
    private IEnumerator StartDoBattle(string scene)
    {
        return new <StartDoBattle>c__Iterator1B { scene = scene, <$>scene = scene };
    }

    [DebuggerHidden]
    private IEnumerator StartDoBattleGrid(string scene)
    {
        return new <StartDoBattleGrid>c__Iterator1C { scene = scene, <$>scene = scene };
    }

    public BattleGameBase CurGame { get; set; }

    private BattleGameType gameType { get; set; }

    private BattleGridGame gridGame { get; set; }

    private BattleNormalGame normalGame { get; set; }

    [CompilerGenerated]
    private sealed class <StartDoBattle>c__Iterator1B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>scene;
        internal Operation <asyncOperation>__0;
        internal string scene;

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
                    LoadingPerfab.BeginTransition();
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    return true;

                case 1:
                    CardPool.ClearCache();
                    this.<asyncOperation>__0 = BundleMgr.Instance.LoadLevelAsync(this.scene);
                    BattleState.GetInstance().SetWaitBattle(this.<asyncOperation>__0);
                    GameStateMgr.Instance.ChangeState("COMMUNITY_BATTLE_EVENT");
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
    private sealed class <StartDoBattleGrid>c__Iterator1C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>scene;
        internal Operation <asyncOperation>__0;
        internal string scene;

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
                    LoadingPerfab.BeginTransition();
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<asyncOperation>__0 = BundleMgr.Instance.LoadLevelAsync(this.scene);
                    BattleState.GetInstance().SetWaitBattle(this.<asyncOperation>__0);
                    GameStateMgr.Instance.ChangeState("COMMUNITY_OUTLANDGRID_EVENT");
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
}

