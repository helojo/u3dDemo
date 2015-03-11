using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SettingPanel : GUIEntity
{
    private Transform _WaitPanel;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1;

    private void OnAgreeLableClickBtn(GameObject go)
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.SetUserAgreeLabel();
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cache1, null);
    }

    private void OnBossPushClose()
    {
        SettingMgr.mInstance.WorldBossPush = false;
    }

    private void OnBossPushOpen()
    {
        SettingMgr.mInstance.WorldBossPush = true;
    }

    private void OnCardPushClose()
    {
        SettingMgr.mInstance.FreeTakeCardPush = false;
    }

    private void OnCardPushOpen()
    {
        SettingMgr.mInstance.FreeTakeCardPush = true;
    }

    public override void OnDestroy()
    {
        this.SaveSetting();
    }

    private void OnGuildShopRefreshPushClose()
    {
        SettingMgr.mInstance.GuildShopRefreshPush = false;
    }

    private void OnGuildShopRefreshPushOpen()
    {
        SettingMgr.mInstance.GuildShopRefreshPush = true;
    }

    public override void OnInitialize()
    {
        if (SettingMgr.mInstance.BGM_Enable)
        {
            base.transform.FindChild("Music/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("Music/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.SFX_Enable)
        {
            base.transform.FindChild("Sound/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("Sound/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.PushNotification_Enable)
        {
            base.transform.FindChild("MsgPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("MsgPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.ScreenSleep_Enable)
        {
            base.transform.FindChild("PowerSave/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("PowerSave/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.BattleRenderLevel == BattleRenderLevelType.High)
        {
            base.transform.FindChild("Effect/High").GetComponent<UIToggle>().isChecked = true;
        }
        else if (SettingMgr.mInstance.BattleRenderLevel == BattleRenderLevelType.Medium)
        {
            base.transform.FindChild("Effect/Medium").GetComponent<UIToggle>().isChecked = true;
        }
        else if (SettingMgr.mInstance.BattleRenderLevel == BattleRenderLevelType.Low)
        {
            base.transform.FindChild("Effect/Low").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.SkillFullPush)
        {
            base.transform.FindChild("SkillFullPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("SkillFullPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.WorldBossPush)
        {
            base.transform.FindChild("BossPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("BossPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.ShopRefreshPush)
        {
            base.transform.FindChild("ShopRefreshPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("ShopRefreshPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.FreeTakeCardPush)
        {
            base.transform.FindChild("CardPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("CardPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.PushYqdChouJiang_Enable)
        {
            base.transform.FindChild("ShopYQDPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("ShopYQDPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.PushTiLiFull_Enable)
        {
            base.transform.FindChild("TiLiFullPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("TiLiFullPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.GuildShopRefreshPush)
        {
            base.transform.FindChild("GuildShopRefreshPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("GuildShopRefreshPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.WorldCupBeiAttackPush)
        {
            base.transform.FindChild("WorldCupBeAttackPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("WorldCupBeAttackPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.WorldCupApplyPush)
        {
            base.transform.FindChild("WorldCupApplyPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("WorldCupApplyPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        if (SettingMgr.mInstance.MeiRiJiangLiPush)
        {
            base.transform.FindChild("MeiRiJiangLiPush/Open").GetComponent<UIToggle>().isChecked = true;
        }
        else
        {
            base.transform.FindChild("MeiRiJiangLiPush/Close").GetComponent<UIToggle>().isChecked = true;
        }
        Transform transform = base.transform.FindChild("UserAgreeLable");
        if (transform != null)
        {
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnAgreeLableClickBtn);
        }
        this._WaitPanel = base.transform.FindChild("WaitPanel");
    }

    private void OnMeiRiJiangLiClose()
    {
        SettingMgr.mInstance.MeiRiJiangLiPush = false;
    }

    private void OnMeiRiJiangLiOpen()
    {
        SettingMgr.mInstance.MeiRiJiangLiPush = true;
    }

    private void OnMusicClose()
    {
        SettingMgr.mInstance.BGM_Enable = false;
    }

    private void OnMusicOpen()
    {
        SettingMgr.mInstance.BGM_Enable = true;
    }

    private void OnPushMsgClose()
    {
        SettingMgr.mInstance.PushNotification_Enable = false;
    }

    private void OnPushMsgOpen()
    {
        SettingMgr.mInstance.PushNotification_Enable = true;
    }

    private void OnPushTiLiFullClose()
    {
        SettingMgr.mInstance.PushTiLiFull_Enable = false;
    }

    private void OnPushTiLiFullOpen()
    {
        SettingMgr.mInstance.PushTiLiFull_Enable = true;
    }

    private void OnScreenSleepClose()
    {
        SettingMgr.mInstance.ScreenSleep_Enable = false;
    }

    private void OnScreenSleepOpen()
    {
        SettingMgr.mInstance.ScreenSleep_Enable = true;
    }

    private void OnSelectHigh()
    {
        base.StartCoroutine(this.SetBattleRenderLevel(BattleRenderLevelType.High));
    }

    private void OnSelectLow()
    {
        base.StartCoroutine(this.SetBattleRenderLevel(BattleRenderLevelType.Low));
    }

    private void OnSelectMiddle()
    {
        base.StartCoroutine(this.SetBattleRenderLevel(BattleRenderLevelType.Medium));
    }

    private void OnShopRefreshPushClose()
    {
        SettingMgr.mInstance.ShopRefreshPush = false;
    }

    private void OnShopRefreshPushOpen()
    {
        SettingMgr.mInstance.ShopRefreshPush = true;
    }

    private void OnSkillFullPushClose()
    {
        SettingMgr.mInstance.SkillFullPush = false;
    }

    private void OnSkillFullPushOpen()
    {
        SettingMgr.mInstance.SkillFullPush = true;
    }

    private void OnSoundClose()
    {
        SettingMgr.mInstance.SFX_Enable = false;
    }

    private void OnSoundOpen()
    {
        SettingMgr.mInstance.SFX_Enable = true;
    }

    private void OnWorldCupApplyClose()
    {
        SettingMgr.mInstance.WorldCupApplyPush = false;
    }

    private void OnWorldCupApplyOpen()
    {
        SettingMgr.mInstance.WorldCupApplyPush = true;
    }

    private void OnWorldCupBeiAttackClose()
    {
        SettingMgr.mInstance.WorldCupBeiAttackPush = false;
    }

    private void OnWorldCupBeiAttackOpen()
    {
        SettingMgr.mInstance.WorldCupBeiAttackPush = true;
    }

    private void OnYqdChouJiangClose()
    {
        SettingMgr.mInstance.PushYqdChouJiang_Enable = false;
    }

    private void OnYqdChouJiangOpen()
    {
        SettingMgr.mInstance.PushYqdChouJiang_Enable = true;
    }

    private void OpenGm()
    {
        if (GameDefine.getInstance().isDebugLog)
        {
            GMToolMgr.showGMBtn = !GMToolMgr.showGMBtn;
        }
    }

    private void SaveSetting()
    {
        SettingMgr.mInstance.SaveSetting();
    }

    [DebuggerHidden]
    private IEnumerator SetBattleRenderLevel(BattleRenderLevelType _type)
    {
        return new <SetBattleRenderLevel>c__Iterator9B { _type = _type, <$>_type = _type, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <SetBattleRenderLevel>c__Iterator9B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleRenderLevelType _type;
        internal BattleRenderLevelType <$>_type;
        internal SettingPanel <>f__this;

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
                    this.<>f__this._WaitPanel.gameObject.SetActive(true);
                    SettingMgr.mInstance.BattleRenderLevel = this._type;
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    goto Label_0099;

                case 1:
                    this.<>f__this._WaitPanel.gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0099;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0099:
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

