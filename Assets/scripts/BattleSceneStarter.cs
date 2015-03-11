using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("MTD/BattleSceneStarter")]
public class BattleSceneStarter : MonoBehaviour
{
    public static bool G_isMoveToNext = true;
    public static bool G_isQuickTest = true;
    public static bool G_isTestEnable;
    public static bool G_isTestNormalGame = true;
    public static bool G_isTestPK;
    public static bool G_isTestSkill;
    public static bool G_isTestWorldBoss;
    public static int G_TestCameraState;
    public static string G_TestStartAnim;
    public static int G_TestState;
    public static int G_TestWorldBossID = 0x4ea;
    public static BattleSceneStarter Instance;
    public bool isMoveToNext = true;
    public bool isQuickTest = true;
    public bool isTestNormalGame = true;
    public bool isTestPK;
    public bool isTestSkill;
    public bool isTestWorldBoss;
    public int testCameraState;
    public string TestGridPoint = string.Empty;
    public string TestGridPointModel = string.Empty;
    public List<int> TestSkillCardID = new List<int>();
    public string testStartAnim;
    public int testState;
    public int testWorldBossID = 0x4ea;

    private void OnGUI()
    {
    }

    private void Start()
    {
        Instance = this;
        if (UICamera.mainCamera == null)
        {
            G_isTestEnable = true;
            FsmVariables.GlobalVariables.FindFsmString("testLevelName").Value = Application.loadedLevelName;
            Application.LoadLevel("Starter");
        }
        else if (G_isTestEnable)
        {
            G_isTestPK = this.isTestPK;
            G_isTestSkill = this.isTestSkill;
            G_isTestWorldBoss = this.isTestWorldBoss;
            G_isTestNormalGame = this.isTestNormalGame;
            G_TestState = this.testState;
            G_TestCameraState = this.testCameraState;
            G_TestStartAnim = this.testStartAnim;
            G_TestWorldBossID = this.testWorldBossID;
            G_isQuickTest = this.isQuickTest;
            BattleCom_PhaseManager.G_moveEnable = G_isMoveToNext = this.isMoveToNext;
            if (G_isTestNormalGame)
            {
                BattleStaticEntry.StartTest(BattleGameType.Normal);
            }
            else
            {
                BattleStaticEntry.StartTest(BattleGameType.Grid);
            }
        }
    }
}

