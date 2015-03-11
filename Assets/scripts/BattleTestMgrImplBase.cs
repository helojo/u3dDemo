using System;
using System.Runtime.CompilerServices;

public class BattleTestMgrImplBase
{
    public virtual void DoTest()
    {
        if (BattleSceneStarter.G_isTestSkill)
        {
            this.TestSkill();
        }
        else
        {
            this.TestScene();
        }
    }

    public virtual void Init()
    {
    }

    private void InitBattle()
    {
        BattleCom_FighterManager component = this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        for (int i = 0; i < 6; i++)
        {
            if (i < BattleSceneStarter.Instance.TestSkillCardID.Count)
            {
                int entry = BattleSceneStarter.Instance.TestSkillCardID[i];
                if (entry >= 0)
                {
                    component.createFighter(entry, i, 1f, BattleGlobalFunc.CreateDetailInfoByCard(entry), i, false);
                }
            }
        }
        component.OnFighterInitFinish();
        this.InitMonster();
    }

    private void InitMonster()
    {
        BattleCom_FighterManager component = this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        for (int i = 0; i < 6; i++)
        {
            int num2 = i + (6 * (1 + this.battleGameData.CurBattlePhase));
            if (num2 >= BattleSceneStarter.Instance.TestSkillCardID.Count)
            {
                num2 = i + 6;
            }
            if (i < BattleSceneStarter.Instance.TestSkillCardID.Count)
            {
                int entry = BattleSceneStarter.Instance.TestSkillCardID[num2];
                if (entry >= 0)
                {
                    if (BattleSceneStarter.G_isTestPK)
                    {
                        component.createFighter(entry, i + BattleGlobal.FighterNumberMax, 1f, BattleGlobalFunc.CreateDetailInfoByCard(entry), i + BattleGlobal.FighterNumberMax, false);
                    }
                    else
                    {
                        component.createMonsterFighter(entry, i + BattleGlobal.FighterNumberMax, BattleGlobalFunc.CreateDetailInfoOnMonster(entry), i + BattleGlobal.FighterNumberMax);
                    }
                }
            }
        }
    }

    public virtual void InitTest()
    {
        ShotState battle = ShotState.Battle;
        if (BattleSceneStarter.G_isTestWorldBoss)
        {
            battle = ShotState.CloseUp1;
        }
        else
        {
            battle = (ShotState) BattleSceneStarter.G_TestCameraState;
        }
        this.battleGameData.CurBattlePhase = 0;
        this.battleGameData.startPhase = BattleSceneStarter.G_TestState;
        this.battleGameData.cameraShotType = battle;
        this.battleGameData.startAnim = BattleSceneStarter.G_TestStartAnim;
        switch (battle)
        {
            case ShotState.CloseUp1:
            case ShotState.CloseUp3:
                this.battleGameData.IsBossBattle = true;
                break;

            default:
                this.battleGameData.IsBossBattle = false;
                break;
        }
        this.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().InitPlayerTeam();
        this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().GetCurCamera().SetShotState(this.battleGameData.cameraShotType, true);
        this.InitBattle();
    }

    public virtual void OnMsgPhaseChange()
    {
        this.InitMonster();
    }

    private void TestScene()
    {
        this.battleGameData.OnMsgFighterChange();
        BattleCom_Runtime component = BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
        if (BattleSceneStarter.G_isQuickTest)
        {
            component.isTestAutoFinish = true;
        }
    }

    private void TestSkill()
    {
        BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().isTestAutoFinish = false;
    }

    public BattleData battleGameData { get; set; }
}

