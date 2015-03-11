using FastBuf;
using Newbie;
using System;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class PlayerInfoPanel : GUIEntity
{
    public UISprite _ExpBg;
    public UISprite _ExpForground;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache5;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache6;
    public GameObject TileTipsObj;

    private void ClickBindBtn()
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = delegate (GUIEntity obj) {
            };
        }
        GUIMgr.Instance.DoModelGUI("BindAccountPanel", <>f__am$cache4, base.gameObject);
    }

    private void ClickClosePanel()
    {
        GUIMgr.Instance.ExitModelGUI(base.name);
    }

    private void ExitGameToLogin()
    {
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = delegate (GameObject go) {
                        ActorData.getInstance().OnExit();
                        GameStateMgr.IsGameReturnLogin = true;
                        GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
                        XSingleton<AdManager>.Singleton._isAd = false;
                        ActorData.getInstance().guildMsgKey = 0;
                    };
                }
                box.SetDialog(ConfigMgr.getInstance().GetWord(14), <>f__am$cache6, null, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache3, null);
    }

    private void InitUserInfo()
    {
        this.UpdateHead();
        this.UpdateHeadFrame();
        base.transform.FindChild("Top/Sign/Name").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.name;
        base.transform.FindChild("Top/Sign/Level").GetComponent<UILabel>().text = "LV." + ActorData.getInstance().UserInfo.level.ToString();
        UILabel component = base.transform.FindChild("Top/Sign/Exp").GetComponent<UILabel>();
        user_lv_up_config _config = ConfigMgr.getInstance().getByEntry<user_lv_up_config>(ActorData.getInstance().Level);
        if (_config != null)
        {
            if (ActorData.getInstance().Level < CommonFunc.GetUserMaxLv())
            {
                component.text = ActorData.getInstance().UserInfo.exp + "/" + _config.need_exp;
            }
            else
            {
                component.text = "MAX";
            }
        }
        base.transform.FindChild("Center/Id").GetComponent<UILabel>().text = ActorData.getInstance().SessionInfo.userid.ToString();
        base.transform.FindChild("Top/Sign/Camp").GetComponent<UISprite>().spriteName = (ActorData.getInstance().UserInfo.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
        if (ActorData.getInstance().UserInfo.exp == 0)
        {
            this._ExpForground.gameObject.SetActive(false);
        }
        else
        {
            float num = ((float) ActorData.getInstance().UserInfo.exp) / ((float) _config.need_exp);
            if (num > 1f)
            {
                num = 1f;
            }
            this._ExpForground.width = (int) ((this._ExpBg.width - 5) * num);
            this._ExpForground.gameObject.SetActive(true);
        }
        this.SetCurrTitle();
        base.transform.FindChild("Center/HeroLeveTopNum").GetComponent<UILabel>().text = ActorData.getInstance().Level.ToString();
        UIEventListener.Get(base.transform.FindChild("Top/Sign/ChangeNickname").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenSetNicknameDlag);
        this.TileTipsObj = base.transform.FindChild("Bottom/ChangeTitleBtn/Tips").gameObject;
        this.TileTipsObj.SetActive(ActorData.getInstance().mHaveNewTitle);
    }

    private void OnClickChangeIconButton()
    {
        if (GuideSystem.MatchEvent(GuideEvent.HelpMe))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_HelpMe.tag_helpme_press_change_button, null);
        }
    }

    private void OnClickChangeIconFrameButton()
    {
        SocketMgr.Instance.RequestGetHeadFrameList();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        CommonFunc.ShowFuncList(false);
        this.InitUserInfo();
        if (GuideSystem.MatchEvent(GuideEvent.HelpMe))
        {
            Transform transform = base.transform.FindChild("Center/ChangeIconBtn");
            if (null == transform)
            {
                GuideSystem.ActivedGuide.RequestCancel();
                Utility.NewbiestUnlock();
                Utility.EnforceClear();
            }
            else
            {
                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_HelpMe.tag_helpme_press_change_button, transform.gameObject);
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.InitUserInfo();
        this.ShowBindBtn();
    }

    private void OpenSetNicknameDlag(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("SetNicknameDlag", null, base.gameObject);
    }

    private void OpenTitlePanel()
    {
        ActorData.getInstance().mHaveNewTitle = false;
        ActorData.getInstance().TitleCount = ActorData.getInstance().TitleList.Count;
        this.TileTipsObj.SetActive(ActorData.getInstance().mHaveNewTitle);
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = e => TitlePanel panel = (TitlePanel) e;
        }
        GUIMgr.Instance.DoModelGUI("TitlePanel", <>f__am$cache5, null);
    }

    public void SetCurrTitle()
    {
        int id = 0x16;
        if ((TimeMgr.Instance.ServerStampTime < ActorData.getInstance().UserInfo.title_time) || (ActorData.getInstance().UserInfo.title_time == ulong.MaxValue))
        {
            id = ActorData.getInstance().UserInfo.titleEntry;
        }
        else
        {
            ActorData.getInstance().UserInfo.titleEntry = id;
        }
        UILabel component = base.transform.FindChild("Bottom/Title").GetComponent<UILabel>();
        UITexture texture = base.transform.FindChild("Bottom/Icon").GetComponent<UITexture>();
        user_title_config _config = ConfigMgr.getInstance().getByEntry<user_title_config>(id);
        if (_config != null)
        {
            component.text = _config.name;
            texture.mainTexture = BundleMgr.Instance.CreateTitleIcon(_config.icon.ToString());
        }
    }

    private void ShowBindBtn()
    {
        base.gameObject.transform.FindChild("BindBtn").gameObject.SetActive(false);
    }

    public void UpdateHead()
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(ActorData.getInstance().UserInfo.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("Top/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            UISprite component = base.transform.FindChild("Top/Head/QualityBorder").GetComponent<UISprite>();
            Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) _config.entry);
            if (cardByEntry != null)
            {
                CommonFunc.SetQualityBorder(component, cardByEntry.cardInfo.quality);
            }
            else
            {
                CommonFunc.SetQualityBorder(component, 0);
            }
            MainUI gUIEntity = GUIMgr.Instance.GetGUIEntity<MainUI>();
            if (gUIEntity != null)
            {
                gUIEntity.Create3DRole(ActorData.getInstance().UserInfo.headEntry);
            }
        }
    }

    public void UpdateHeadFrame()
    {
        UISprite component = base.transform.FindChild("Top/Head/FrameBg").GetComponent<UISprite>();
        UISprite sprite2 = base.transform.FindChild("Top/Head/TagBg").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(component, sprite2, ActorData.getInstance().UserInfo.headFrameEntry);
    }

    public void UpdateNickName()
    {
        base.transform.FindChild("Top/Sign/Name").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.name;
    }

    public void UpdateTitle()
    {
        this.SetCurrTitle();
    }
}

