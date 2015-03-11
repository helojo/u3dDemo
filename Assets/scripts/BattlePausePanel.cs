using HutongGames.PlayMaker;
using System;
using UnityEngine;

public class BattlePausePanel : GUIEntity
{
    public UILabel _MusicLabel;
    public bool FightIsOver;
    private float m_time = 1f;
    private float m_updateInterval = 0.5f;
    private bool mIsStart = true;

    private void ExitPanel()
    {
        if (this.FightIsOver)
        {
            BattleSecurityManager.Instance.EndPause();
            GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
        }
        else
        {
            BattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
            if (gUIEntity != null)
            {
                ActorData.getInstance().mFriendReward = null;
                if (gUIEntity.impl.battleGameData.gameType == BattleGameType.Normal)
                {
                    PlayMakerFSM component = base.transform.FindChild("Center/ibutton_exit").GetComponent<PlayMakerFSM>();
                    if (component != null)
                    {
                        FsmInt num = component.FsmVariables.FindFsmInt("BattleReturnType");
                        num.Value = (int) gUIEntity.impl.battleGameData.normalGameType;
                        if (ActorData.getInstance().mCurrDupReturnPrePara != null)
                        {
                            num.Value = 13;
                        }
                        component.SendEvent("CALL_BATTLE_EVENT");
                        BattleStaticEntry.ExitBattle();
                        Debug.Log(num.Value + "   00000000000000000000000");
                    }
                }
                else if (gUIEntity.impl.battleGameData.gameType == BattleGameType.Grid)
                {
                    gUIEntity.impl.battleGameData.OnMsgGridGameFinishOneBattle(false, true, null);
                    GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
                }
            }
        }
    }

    private void GameContinue()
    {
        BattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
        if (gUIEntity == null)
        {
            GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
        }
        else
        {
            BattleCom_Runtime component = gUIEntity.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
            if (component != null)
            {
                component.ResumeGame();
            }
            BattleSecurityManager.Instance.EndPause();
            GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        bool flag = SettingMgr.mInstance.BGM_Enable || SettingMgr.mInstance.SFX_Enable;
        this._MusicLabel.text = !flag ? "声音：关" : "声音：开";
        UIToggle component = base.transform.FindChild("Center/ibutton_close").GetComponent<UIToggle>();
        component.isChecked = flag;
        component.gameObject.SetActive(true);
        BattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
        if (gUIEntity != null)
        {
            BattleCom_Runtime runtime = gUIEntity.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
            if (runtime != null)
            {
                runtime.PauseGame();
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                BattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
                if (gUIEntity != null)
                {
                    PlayMakerFSM component = gUIEntity._TimeLabel.GetComponent<PlayMakerFSM>();
                    if (component != null)
                    {
                        component.FsmVariables.FindFsmBool("IsPause").Value = true;
                    }
                }
            }
        }
    }

    private void SetMusicState()
    {
        bool flag = SettingMgr.mInstance.BGM_Enable || SettingMgr.mInstance.SFX_Enable;
        flag = !flag;
        this._MusicLabel.text = !flag ? "声音：关" : "声音：开";
        base.transform.FindChild("Center/ibutton_close").GetComponent<UIToggle>().isChecked = flag;
        if (!flag)
        {
            SettingMgr.mInstance.BGM_Enable = false;
            SettingMgr.mInstance.SFX_Enable = false;
        }
        else
        {
            SettingMgr.mInstance.BGM_Enable = true;
            SettingMgr.mInstance.SFX_Enable = true;
        }
    }
}

