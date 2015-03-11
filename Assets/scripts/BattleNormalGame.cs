using System;
using UnityEngine;

public class BattleNormalGame : BattleGameBase
{
    private GameObject battleComObject;
    private bool isStarted;

    private void CacheResources()
    {
        base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().CacheResources();
    }

    public BattleCom_FighterManager GetFighterManager()
    {
        return this.battleComObject.GetComponent<BattleCom_FighterManager>();
    }

    public override void Init()
    {
        base.Init();
        base.battleGameData.gameType = BattleGameType.Normal;
        BattleData data = new BattleData {
            gameType = BattleGameType.Normal
        };
        base.battleGameData = data;
        base.battleGameData.InitBattleValue();
        this.battleComObject = new GameObject("BattleComObject Normal");
        UnityEngine.Object.DontDestroyOnLoad(this.battleComObject);
        base.battleGameData.battleComObject = this.battleComObject;
        this.battleComObject.AddComponent<BattleCom_ScenePosManager>();
        this.battleComObject.AddComponent<BattleCom_PlayerControl>();
        this.battleComObject.AddComponent<BattleCom_FighterManager>();
        this.battleComObject.AddComponent<BattleCom_CameraManager>();
        this.battleComObject.AddComponent<BattleCom_UIControl>();
        this.battleComObject.AddComponent<BattleCom_StoryControl>();
        this.battleComObject.AddComponent<BattleCom_TestManager>();
        this.battleComObject.AddComponent<BattleCom_Runtime>();
        this.battleComObject.AddComponent<BattleCom_PhaseManager>();
        this.battleComObject.AddComponent<BattleCom_SceneEffect>();
        this.battleComObject.SendMessage("OnMsgCreateInit", base.battleGameData);
    }

    public override void OnEnter()
    {
        this.OnSceneInfoInit();
        base.battleGameData.OnMsgEnter();
    }

    public override void OnLeave()
    {
        base.OnLeave();
        this.isStarted = false;
        if (base.battleGameData.OnMsgLeave != null)
        {
            base.battleGameData.OnMsgLeave();
        }
    }

    private void OnSceneInfoInit()
    {
        this.GetFighterManager().InitPlayerTeam();
        this.battleComObject.GetComponent<BattleCom_ScenePosManager>().InitSceneInfo();
        this.battleComObject.GetComponent<BattleCom_CameraManager>().InitBindCamera();
    }

    public override void OnUpdate()
    {
        if (base.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().IsLoadFinish() && !this.isStarted)
        {
            this.StartBattle();
        }
    }

    private void StartBattle()
    {
        this.isStarted = true;
        base.battleGameData.timeScale_ShowTime = 1f;
        BattleGlobal.SetShowTimeScale(1f);
        base.battleGameData.OnMsgStart();
        this.CacheResources();
        base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginPhaseStarting();
    }
}

