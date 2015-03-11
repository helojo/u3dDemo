using FastBuf;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoulBox : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache4;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache5;
    private ItemInfoPanel item_tips;

    public static int CostWithVIPLevel()
    {
        int id = Mathf.Max(0, ActorData.getInstance().UserInfo.vip_level.level - 1);
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(id);
        if (_config == null)
        {
            return 0x184;
        }
        if (_config.soul_box_cost < 0)
        {
            return 0x184;
        }
        return _config.soul_box_cost;
    }

    private void CreateItemTipsObject(Action<GUIEntity> e)
    {
        <CreateItemTipsObject>c__AnonStorey267 storey = new <CreateItemTipsObject>c__AnonStorey267 {
            e = e,
            <>f__this = this
        };
        if (null != this.item_tips)
        {
            this.item_tips.gameObject.SetActive(true);
            if (storey.e != null)
            {
                storey.e(this.item_tips);
            }
        }
        else
        {
            GUIMgr.Instance.DoModelGUI<ItemInfoPanel>(new Action<GUIEntity>(storey.<>m__57D), base.gameObject);
        }
    }

    public void EnableButton(bool enabled)
    {
        base.transform.FindChild("btn1/btn").GetComponent<BoxCollider>().enabled = enabled;
        base.transform.FindChild("btn1/btn").GetComponent<UIButton>().enabled = enabled;
        base.transform.FindChild("btn2/btn").GetComponent<BoxCollider>().enabled = enabled;
        base.transform.FindChild("btn2/btn").GetComponent<UIButton>().enabled = enabled;
    }

    private void FillHeroIcon(Transform root, int identity, bool _isChip = true)
    {
        UITexture component = root.FindChild("Icon").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(identity);
        if (_config != null)
        {
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        SoulEntry entry = root.gameObject.AddComponent<SoulEntry>();
        entry.entry = identity;
        entry.mIsChip = _isChip;
    }

    private static int FuncLevel(int func)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<vip_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                vip_config current = (vip_config) enumerator.Current;
                if ((current.func.IndexOf("|") == -1) && (func == Convert.ToInt32(current.func)))
                {
                    return (current.entry + 1);
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return 0x63;
    }

    public static bool FuncRectuitiable()
    {
        int level = ActorData.getInstance().UserInfo.vip_level.level;
        int num2 = FuncLevel(7);
        return (level >= num2);
    }

    public static bool FuncShowable()
    {
        int level = ActorData.getInstance().UserInfo.vip_level.level;
        int num2 = FuncLevel(6);
        return (level >= num2);
    }

    private void OnClickBuyButton(GameObject go)
    {
        if (!FuncRectuitiable())
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (GUIEntity e) {
                    MessageBox box = e as MessageBox;
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = uigo => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                    }
                    box.SetDialog(ConfigMgr.getInstance().GetWord(0x26b6), <>f__am$cache5, null, false);
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache3, null);
        }
        else
        {
            if (ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_SoulBox) != null)
            {
                SocketMgr.Instance.RequestSoulBox(true, false);
            }
            else
            {
                if (ActorData.getInstance().Stone < CostWithVIPLevel())
                {
                    <OnClickBuyButton>c__AnonStorey26A storeya = new <OnClickBuyButton>c__AnonStorey26A {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeya.<>m__583), null);
                    return;
                }
                SocketMgr.Instance.RequestSoulBox(false, false);
            }
            RecruitPanel.actived_function = RecruitPanel.function.soul;
            this.EnableButton(false);
        }
    }

    private void OnClickBuyTenButton(GameObject go)
    {
        if (!FuncRectuitiable())
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GUIEntity e) {
                    MessageBox box = e as MessageBox;
                    if (<>f__am$cache4 == null)
                    {
                        <>f__am$cache4 = uigo => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                    }
                    box.SetDialog(ConfigMgr.getInstance().GetWord(0x26b6), <>f__am$cache4, null, false);
                };
            }
            GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache2, null);
        }
        else
        {
            int num = CostWithVIPLevel() * 10;
            if (ActorData.getInstance().Stone < num)
            {
                <OnClickBuyTenButton>c__AnonStorey269 storey = new <OnClickBuyTenButton>c__AnonStorey269 {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__581), null);
            }
            else
            {
                SocketMgr.Instance.RequestSoulBox(false, true);
                RecruitPanel.actived_function = RecruitPanel.function.soul;
                this.EnableButton(false);
            }
        }
    }

    private void OnClickExitButton(GameObject go)
    {
        GUIMgr.Instance.ExitModelGUI(this);
    }

    private void OnClickRuleBtn(GameObject go)
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.SetSoulBoxRule();
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cache1, null);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        base.transform.FindChild("btn1/btn").gameObject.SetActive(true);
        UILabel component = base.transform.FindChild("btn1/price").GetComponent<UILabel>();
        int num = CostWithVIPLevel();
        UISprite sprite = base.transform.FindChild("btn1/btn/discount").GetComponent<UISprite>();
        UISprite sprite2 = base.transform.FindChild("btn1/ico").GetComponent<UISprite>();
        base.transform.FindChild("btn2/price").GetComponent<UILabel>().text = (num * 10).ToString();
        Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_SoulBox);
        if (FuncRectuitiable() && (ticketItemBySubType != null))
        {
            component.text = "X" + ticketItemBySubType.num;
            sprite.gameObject.SetActive(false);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
            if (_config != null)
            {
                sprite2.spriteName = _config.icon;
            }
        }
        else
        {
            sprite2.spriteName = "Item_Icon_Stone";
            component.text = num.ToString();
            sprite.gameObject.SetActive(true);
            if (num >= 0x184)
            {
                sprite.gameObject.SetActive(false);
            }
            else if (num >= 0x170)
            {
                sprite.gameObject.SetActive(true);
                sprite.spriteName = "Ui_Hunxia_Icon_1";
            }
            else
            {
                sprite.gameObject.SetActive(true);
                sprite.spriteName = "Ui_Zhaomu_Icon_9";
            }
        }
        SocketMgr.Instance.RequestSoulBoxInfo();
    }

    public override void OnDestroy()
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener listener1 = UIEventListener.Get(base.transform.FindChild("btn1/btn").gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickBuyButton));
        UIEventListener listener2 = UIEventListener.Get(base.transform.FindChild("ExitBtn").gameObject);
        listener2.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener2.onClick, new UIEventListener.VoidDelegate(this.OnClickExitButton));
        UIEventListener.Get(base.transform.FindChild("RuleBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRuleBtn);
        UIEventListener listener3 = UIEventListener.Get(base.transform.FindChild("hero_week").gameObject);
        listener3.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener3.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
        UIEventListener listener4 = UIEventListener.Get(base.transform.FindChild("hero_today0").gameObject);
        listener4.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener4.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
        UIEventListener listener5 = UIEventListener.Get(base.transform.FindChild("hero_today1").gameObject);
        listener5.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener5.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
        UIEventListener listener6 = UIEventListener.Get(base.transform.FindChild("hero_today2").gameObject);
        listener6.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener6.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
        UIEventListener.Get(base.transform.FindChild("btn2/btn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickBuyTenButton);
    }

    private void OnPressItem(GameObject go, bool pressed)
    {
        <OnPressItem>c__AnonStorey268 storey = new <OnPressItem>c__AnonStorey268();
        if (!pressed)
        {
            if (null != this.item_tips)
            {
                this.item_tips.gameObject.SetActive(false);
            }
        }
        else
        {
            storey.se = go.GetComponent<SoulEntry>();
            if ((null != storey.se) && (storey.se.entry >= 0))
            {
                storey.star_lv = 1;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(storey.se.entry);
                if (_config != null)
                {
                    storey.star_lv = _config.evolve_lv;
                }
                this.CreateItemTipsObject(new Action<GUIEntity>(storey.<>m__57F));
            }
        }
    }

    public void RefreshHero()
    {
        Transform root = base.transform.FindChild("hero_week");
        Transform[] transformArray = new Transform[] { base.transform.FindChild("hero_today0"), base.transform.FindChild("hero_today1"), base.transform.FindChild("hero_today2") };
        this.FillHeroIcon(root, ActorData.getInstance().WeekSoulCardEntry, false);
        int count = ActorData.getInstance().DailySoulCardEntries.Count;
        for (int i = 0; i != Mathf.Min(count, 3); i++)
        {
            this.FillHeroIcon(transformArray[i], ActorData.getInstance().DailySoulCardEntries[i], true);
        }
    }

    [CompilerGenerated]
    private sealed class <CreateItemTipsObject>c__AnonStorey267
    {
        internal SoulBox <>f__this;
        internal Action<GUIEntity> e;

        internal void <>m__57D(GUIEntity _e)
        {
            this.<>f__this.item_tips = _e as ItemInfoPanel;
            if (this.e != null)
            {
                this.e(this.<>f__this.item_tips);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyButton>c__AnonStorey26A
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__583(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__587(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyTenButton>c__AnonStorey269
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__581(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__586(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnPressItem>c__AnonStorey268
    {
        internal SoulEntry se;
        internal int star_lv;

        internal void <>m__57F(GUIEntity e)
        {
            (e as ItemInfoPanel).SoulBoxShowCard(this.se.entry, this.star_lv, false, this.se.mIsChip);
        }
    }
}

