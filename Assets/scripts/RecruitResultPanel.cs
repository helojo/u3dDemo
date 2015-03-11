using FastBuf;
using Newbie;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RecruitResultPanel : GUIEntity
{
    public GameObject _ShareBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1;

    private void ClickShare()
    {
        if (SharePanel.IsCanShare())
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = entity => (entity as PushNotifyPanel).UpdateData(PushNotifyPanel.ShareType.PubRecruit, null, null);
            }
            GUIMgr.Instance.DoModelGUI("PushNotifyPanel", <>f__am$cache1, null);
        }
    }

    private void ClickSoulBoxRecruitButton(GameObject go)
    {
        bool flag = false;
        object obj2 = GUIDataHolder.getData(go);
        if ((obj2 != null) && (((int) obj2) == 1))
        {
            flag = true;
        }
        if (flag)
        {
            SocketMgr.Instance.RequestSoulBox(true, false);
        }
        else
        {
            if (ActorData.getInstance().Stone < SoulBox.CostWithVIPLevel())
            {
                <ClickSoulBoxRecruitButton>c__AnonStorey25C storeyc = new <ClickSoulBoxRecruitButton>c__AnonStorey25C {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyc.<>m__557), null);
                return;
            }
            SocketMgr.Instance.RequestSoulBox(false, false);
        }
        EnableButton(false);
    }

    public static void EnableButton(bool enable)
    {
        RecruitResultPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<RecruitResultPanel>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.transform.FindChild("TopLeft/Close").GetComponent<BoxCollider>().enabled = enable;
            activityGUIEntity.transform.FindChild("TopLeft/Close").GetComponent<UIButton>().enabled = enable;
            activityGUIEntity.transform.FindChild("Bottom/btn0/btn").GetComponent<BoxCollider>().enabled = enable;
            activityGUIEntity.transform.FindChild("Bottom/btn1/btn").GetComponent<BoxCollider>().enabled = enable;
            activityGUIEntity.transform.FindChild("Bottom/btn3/btn").GetComponent<BoxCollider>().enabled = enable;
        }
    }

    private void OnClickCloseButton(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Recruit.tag_recruit_comeback, go);
        }
        GameStateMgr.Instance.ChangeState("RECRUIT_COMMUNITY_EVENT");
    }

    private void OnClickRecruitButton(GameObject go)
    {
        if (GameDataMgr.Instance.boostRecruit.valid)
        {
            LotteryOptionStatus component = go.GetComponent<LotteryOptionStatus>();
            object obj2 = GUIDataHolder.getData(go.transform.FindChild("price").GetComponent<UILabel>().gameObject);
            int num = -1;
            if (obj2 != null)
            {
                num = (int) obj2;
            }
            lottery_card_option_config _config = ConfigMgr.getInstance().getByEntry<lottery_card_option_config>(component.opt_entry);
            if ((ActorData.getInstance().Stone < _config.cost_stone) && (num > 0))
            {
                <OnClickRecruitButton>c__AnonStorey25D storeyd = new <OnClickRecruitButton>c__AnonStorey25D {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyd.<>m__558), null);
            }
            else if (null != component)
            {
                bool flag = false;
                object obj3 = GUIDataHolder.getData(go);
                if ((obj3 != null) && (((int) obj3) == 1))
                {
                    flag = true;
                }
                if ((component.card_entry >= 0) && (component.opt_entry >= 0))
                {
                    SocketMgr.Instance.RequestDrawLotteryCard(component.card_entry, component.opt_entry, false, flag);
                    EnableButton(false);
                }
            }
        }
    }

    public override void OnInitialize()
    {
        RecruitPanel.function function = RecruitPanel.actived_function;
        if (function != RecruitPanel.function.stone)
        {
            if (function != RecruitPanel.function.gold)
            {
                base.transform.FindChild("Bottom/btn0").gameObject.SetActive(false);
                base.transform.FindChild("Bottom/btn1").gameObject.SetActive(false);
                base.transform.FindChild("Bottom/btn3").gameObject.SetActive(true);
                this.UpdateSoulBtnStat();
            }
            else
            {
                base.transform.FindChild("Bottom/btn3").gameObject.SetActive(false);
                RecruitPanel.RefreshButton(1, 2, base.transform.FindChild("Bottom/btn0/btn").GetComponent<UIButton>(), false, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
                RecruitPanel.RefreshButton(1, 3, base.transform.FindChild("Bottom/btn1/btn").GetComponent<UIButton>(), false, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
            }
        }
        else
        {
            base.transform.FindChild("Bottom/btn3").gameObject.SetActive(false);
            RecruitPanel.RefreshButton(0, 0, base.transform.FindChild("Bottom/btn0/btn").GetComponent<UIButton>(), true, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
            RecruitPanel.RefreshButton(0, 1, base.transform.FindChild("Bottom/btn1/btn").GetComponent<UIButton>(), true, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
        }
        UIEventListener listener1 = UIEventListener.Get(this.CloseButton);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickCloseButton));
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        switch (RecruitPanel.actived_function)
        {
            case RecruitPanel.function.stone:
                RecruitPanel.UpdateLotteryOptionStatus(base.transform.FindChild("Bottom/btn0/btn").gameObject, null);
                RecruitPanel.UpdateLotteryOptionStatus(base.transform.FindChild("Bottom/btn1/btn").gameObject, null);
                break;

            case RecruitPanel.function.gold:
                RecruitPanel.UpdateLotteryOptionStatus(base.transform.FindChild("Bottom/btn0/btn").gameObject, null);
                RecruitPanel.UpdateLotteryOptionStatus(base.transform.FindChild("Bottom/btn1/btn").gameObject, null);
                break;
        }
    }

    public void UpdateSoulBtnStat()
    {
        Transform transform = base.transform.FindChild("Bottom/btn3/btn");
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.ClickSoulBoxRecruitButton);
        UILabel component = base.transform.FindChild("Bottom/btn3/btn/price").GetComponent<UILabel>();
        Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_SoulBox);
        if (ticketItemBySubType != null)
        {
            GUIDataHolder.setData(transform.gameObject, 1);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
            if (_config != null)
            {
                base.transform.FindChild("Bottom/btn3/ico").GetComponent<UISprite>().spriteName = _config.icon;
            }
            component.text = "X" + ticketItemBySubType.num;
        }
        else
        {
            GUIDataHolder.setData(transform.gameObject, 0);
            base.transform.FindChild("Bottom/btn3/ico").GetComponent<UISprite>().spriteName = "Item_Icon_Stone";
            component.text = SoulBox.CostWithVIPLevel().ToString();
        }
    }

    public GameObject CloseButton
    {
        get
        {
            Transform transform = base.transform.FindChild("TopLeft/Close");
            if (null == transform)
            {
                return null;
            }
            return transform.gameObject;
        }
    }

    [CompilerGenerated]
    private sealed class <ClickSoulBoxRecruitButton>c__AnonStorey25C
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__557(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__55A(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickRecruitButton>c__AnonStorey25D
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__558(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__55B(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }
}

