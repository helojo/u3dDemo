using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class BattleUIGridImpl : BattleUIControlImplBase
{
    private BattlePanel battleUI;
    private OutlandBattlePanel gridGameUI;

    public override void Init()
    {
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgStart = (System.Action) Delegate.Combine(battleGameData.OnMsgStart, new System.Action(this.OnMsgStart));
        BattleData data2 = base.battleGameData;
        data2.OnMsgEnter = (System.Action) Delegate.Combine(data2.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data3 = base.battleGameData;
        data3.OnMsgLeave = (System.Action) Delegate.Combine(data3.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data4 = base.battleGameData;
        data4.OnMsgGridGameFinishOneBattle = (Action<bool, bool, BattleNormalGameResult>) Delegate.Combine(data4.OnMsgGridGameFinishOneBattle, new Action<bool, bool, BattleNormalGameResult>(this.OnMsgGridGameFinishOneBattle));
        BattleData data5 = base.battleGameData;
        data5.OnMsgFighterInfoChange = (Action<int, SkillEffectResult>) Delegate.Combine(data5.OnMsgFighterInfoChange, new Action<int, SkillEffectResult>(this.OnMsgFighterInfoChange));
        BattleData data6 = base.battleGameData;
        data6.OnMsgFighterChange = (System.Action) Delegate.Combine(data6.OnMsgFighterChange, new System.Action(this.OnMsgFighterChange));
        BattleData data7 = base.battleGameData;
        data7.OnMsgFighterDestory = (Action<int>) Delegate.Combine(data7.OnMsgFighterDestory, new Action<int>(this.OnMsgFighterDestory));
        BattleData data8 = base.battleGameData;
        data8.OnMsgFighterUIVisible = (Action<int, bool>) Delegate.Combine(data8.OnMsgFighterUIVisible, new Action<int, bool>(this.OnMsgFighterUIVisible));
        BattleData data9 = base.battleGameData;
        data9.OnMsgBattleRunningChange = (Action<bool>) Delegate.Combine(data9.OnMsgBattleRunningChange, new Action<bool>(this.OnMsgBattleRunningChange));
    }

    public override bool IsLoadFinish()
    {
        return (BattleSceneStarter.G_isTestEnable || (this.gridGameUI != null));
    }

    public void OnChangeUIToGrid(bool isBoxMonster = false)
    {
        if (this.gridGameUI != null)
        {
            if (!isBoxMonster)
            {
                this.gridGameUI.gameObject.SetActive(true);
                if (BattleState.GetInstance().CurGame.battleGameData.IsKey)
                {
                    this.gridGameUI.SetKey();
                }
            }
            else
            {
                this.gridGameUI.gameObject.SetActive(false);
            }
        }
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
        if (!BattleSceneStarter.G_isTestEnable)
        {
            GUIMgr.Instance.OpenUniqueGUIEntity("OutlandBattlePanel", new Action<GUIEntity>(this.OnOutlandUIInitFinish));
        }
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

    private void OnMsgGridGameFinishOneBattle(bool isWin, bool isBreak, BattleNormalGameResult result)
    {
        GUIMgr.Instance.CloseUniqueGUIEntityImmediate("BattlePanel");
        this.battleUI = null;
    }

    private void OnMsgInt()
    {
    }

    private void OnMsgLeave()
    {
        GUIMgr.Instance.CloseUniqueGUIEntity("OutlandBattlePanel");
        this.gridGameUI = null;
    }

    private void OnMsgStart()
    {
        if (this.battleUI != null)
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
    }

    private void OnOutlandUIInitFinish(GUIEntity ui)
    {
        this.gridGameUI = ui as OutlandBattlePanel;
        if (this.gridGameUI != null)
        {
            this.gridGameUI.layerLabel.text = BattleState.GetInstance().GetOutlandLayer();
            this.gridGameUI.SetPrSlider(BattleGridGameMapControl.killMonsters, BattleGridGameMapControl.allMonsters);
            this.gridGameUI.SetBuffIcon(BattleState.GetInstance().CurGame.battleGameData.buffLists);
            if (BattleState.GetInstance().CurGame.battleGameData.IsKey)
            {
                this.gridGameUI.SetKey();
            }
            else
            {
                this.gridGameUI.SetKeyEnable(false);
            }
            if (BattleGridGameMapControl.killMonsters < BattleGridGameMapControl.allMonsters)
            {
                this.gridGameUI.SetBoxIcon(0);
            }
            else if (BattleGridGameMapControl.killMonsters == BattleGridGameMapControl.allMonsters)
            {
                if (BattleState.GetInstance().CurGame.battleGameData.isClearBoxReward)
                {
                    this.gridGameUI.SetBoxIcon(2);
                }
                else
                {
                    this.gridGameUI.SetBoxIcon(1);
                }
            }
        }
    }

    private void OnUIInitFinish(GUIEntity ui)
    {
        this.battleUI = ui as BattlePanel;
        if (this.battleUI != null)
        {
            this.battleUI.impl = this;
            this.gridGameUI.gameObject.SetActive(false);
        }
    }

    public override bool QuestCastSkill(int fighterIndex)
    {
        return base.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().QuestCastSkill(fighterIndex);
    }

    [DebuggerHidden]
    public IEnumerator StartCreateBattleGUI()
    {
        return new <StartCreateBattleGUI>c__Iterator1A { <>f__this = this };
    }

    public override void Tick()
    {
    }

    [CompilerGenerated]
    private sealed class <StartCreateBattleGUI>c__Iterator1A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleUIGridImpl <>f__this;

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
                    this.<>f__this.battleUI = null;
                    GUIMgr.Instance.OpenUniqueGUIEntity("BattlePanel", new Action<GUIEntity>(this.<>f__this.OnUIInitFinish));
                    break;

                case 1:
                    break;

                default:
                    goto Label_0082;
            }
            if (this.<>f__this.battleUI == null)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            this.$PC = -1;
        Label_0082:
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

