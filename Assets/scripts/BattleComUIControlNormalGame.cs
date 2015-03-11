using Battle;
using System;
using System.Collections.Generic;

public class BattleComUIControlNormalGame : BattleUIControlImplBase
{
    private BattlePanel battleUI;

    public override void DoTalk(string talks)
    {
        base.Control.isTalking = true;
    }

    public override void Init()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgStart = (System.Action) Delegate.Combine(data2.OnMsgStart, new System.Action(this.OnMsgStart));
        BattleData data3 = base.battleGameData;
        data3.OnMsgLeave = (System.Action) Delegate.Combine(data3.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data4 = base.battleGameData;
        data4.OnMsgFighterInfoChange = (Action<int, SkillEffectResult>) Delegate.Combine(data4.OnMsgFighterInfoChange, new Action<int, SkillEffectResult>(this.OnMsgFighterInfoChange));
        BattleData data5 = base.battleGameData;
        data5.OnMsgFighterChange = (System.Action) Delegate.Combine(data5.OnMsgFighterChange, new System.Action(this.OnMsgFighterChange));
        BattleData data6 = base.battleGameData;
        data6.OnMsgFighterDestory = (Action<int>) Delegate.Combine(data6.OnMsgFighterDestory, new Action<int>(this.OnMsgFighterDestory));
        BattleData data7 = base.battleGameData;
        data7.OnMsgFighterUIVisible = (Action<int, bool>) Delegate.Combine(data7.OnMsgFighterUIVisible, new Action<int, bool>(this.OnMsgFighterUIVisible));
        BattleData data8 = base.battleGameData;
        data8.OnMsgBattleRunningChange = (Action<bool>) Delegate.Combine(data8.OnMsgBattleRunningChange, new Action<bool>(this.OnMsgBattleRunningChange));
    }

    public override bool IsLoadFinish()
    {
        return (this.battleUI != null);
    }

    private void OnMsgBattleRunningChange(bool isRunning)
    {
        if (this.battleUI != null)
        {
            this.battleUI.OnBattleRunningChange(isRunning);
        }
    }

    private void OnMsgEnter()
    {
        this.battleUI = null;
        GUIMgr.Instance.OpenUniqueGUIEntity("BattlePanel", new Action<GUIEntity>(this.OnUIInitFinish));
    }

    private void OnMsgFighterChange()
    {
        if (this.battleUI != null)
        {
            this.battleUI.OnFighterChange(base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetAllFighters());
        }
    }

    private void OnMsgFighterDestory(int posIndex)
    {
        if (this.battleUI != null)
        {
            this.battleUI.OnMsgFighterDestory(posIndex);
        }
    }

    private void OnMsgFighterInfoChange(int index, SkillEffectResult info)
    {
        if (this.battleUI != null)
        {
            BattleFighter fighter = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().GetFighter(index);
            if (fighter != null)
            {
                if (info == null)
                {
                    this.battleUI.UpdateFighterHPInfo(index, fighter);
                }
                else
                {
                    this.battleUI.OnFighterInfoChange(index, fighter, info);
                }
            }
        }
    }

    private void OnMsgFighterUIVisible(int posIndex, bool visible)
    {
        if (this.battleUI != null)
        {
            this.battleUI.OnMsgFighterUIVisible(posIndex, visible);
        }
    }

    private void OnMsgLeave()
    {
        GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
        this.battleUI = null;
    }

    private void OnMsgStart()
    {
        BattleCom_FighterManager component = base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
        List<BattleFighter> playerFighters = component.GetPlayerFighters();
        this.battleUI.InitFighters(playerFighters);
        for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
        {
            BattleFighter fighter = component.GetFighter(i);
            this.battleUI.UpdateFighterHPInfo(i, fighter);
        }
    }

    public override void OnPhaseEnding()
    {
        this.battleUI.SetGoBtn(true);
    }

    private void OnUIInitFinish(GUIEntity ui)
    {
        this.battleUI = ui as BattlePanel;
        this.battleUI.impl = this;
    }

    public override bool QuestCastSkill(int fighterIndex)
    {
        return base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().QuestCastSkill(fighterIndex);
    }

    public override void QuestGotoNextPhase()
    {
        if (!base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().isRuning)
        {
            base.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginPhaseStarting();
        }
    }

    public override void Tick()
    {
    }
}

