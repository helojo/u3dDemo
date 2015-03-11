using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_TestManager : BattleCom_Base
{
    public BattleTestMgrImplBase impl;

    public override void OnCreateInit()
    {
        base.OnCreateInit();
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgStart = (System.Action) Delegate.Combine(data2.OnMsgStart, new System.Action(this.OnMsgStart));
        BattleData data3 = base.battleGameData;
        data3.OnMsgPhaseChange = (System.Action) Delegate.Combine(data3.OnMsgPhaseChange, new System.Action(this.OnMsgPhaseChange));
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            this.impl = new BattleTestMgrImplBase();
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
            this.impl = new BattleTestMgrImplGrid();
        }
        this.impl.battleGameData = base.battleGameData;
        this.impl.Init();
    }

    private void OnGUI()
    {
        if (BattleSceneStarter.G_isTestEnable && BattleSceneStarter.G_isTestSkill)
        {
        }
    }

    private void OnMsgEnter()
    {
        if (BattleSceneStarter.G_isTestEnable)
        {
            this.impl.InitTest();
        }
    }

    private void OnMsgPhaseChange()
    {
        if (BattleSceneStarter.G_isTestEnable)
        {
            this.impl.OnMsgPhaseChange();
        }
    }

    private void OnMsgStart()
    {
        if (BattleSceneStarter.G_isTestEnable)
        {
            this.impl.DoTest();
        }
    }

    public static void Pause()
    {
        if (BattleState.GetNormalGameInstance().battleGameData.timeScale_ShowTime > 0.1)
        {
            TimeManager.GetInstance().ClearShowTimeObj();
            BattleState.GetNormalGameInstance().battleGameData.timeScale_ShowTime = BattleGlobal.PauseTimeScale;
        }
        else
        {
            BattleState.GetNormalGameInstance().battleGameData.timeScale_ShowTime = 1f;
        }
    }

    public static void ResetTest()
    {
        <ResetTest>c__AnonStoreyE3 ye = new <ResetTest>c__AnonStoreyE3();
        BattleCom_FighterManager component = BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        ye.scenePosManager = BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>();
        component.DoToPlayerAllFighter(new Action<BattleFighter, int>(ye.<>m__4D));
    }

    public static void TestRealSkill(RealTimeSkillInfo skillInfo, bool multi, bool isTeamTarget, int AttackPos, int targetPos)
    {
        <TestRealSkill>c__AnonStoreyE4 ye = new <TestRealSkill>c__AnonStoreyE4();
        BattleCom_Runtime component = BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
        BattleCom_FighterManager manager = BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        if (!component.IsAnySkillRunning())
        {
            ye.targetList = new List<int>();
            if (isTeamTarget)
            {
                manager.DoToMonsterAllFighter(new Action<BattleFighter, int>(ye.<>m__4E));
            }
            else
            {
                ye.targetList.Add(targetPos);
            }
            RealTimeSkill newSkill = new GameObject { name = "Skill_Test " }.AddComponent<RealTimeSkill>();
            newSkill.Init(0x989680, 0x989680, BattleState.GetNormalGameInstance().battleGameData, AttackPos, ye.targetList, 5f);
            newSkill.InitEffect(skillInfo);
            component.skillManager.AddSkill(0x989680, newSkill);
            newSkill.StartProcess();
        }
    }

    [CompilerGenerated]
    private sealed class <ResetTest>c__AnonStoreyE3
    {
        internal BattleCom_ScenePosManager scenePosManager;

        internal void <>m__4D(BattleFighter arg1, int arg2)
        {
            arg1.transform.position = this.scenePosManager.GetSceneFighterEndPosByPhase(arg2);
        }
    }

    [CompilerGenerated]
    private sealed class <TestRealSkill>c__AnonStoreyE4
    {
        internal List<int> targetList;

        internal void <>m__4E(BattleFighter arg1, int arg2)
        {
            this.targetList.Add(arg2);
        }
    }
}

