using FastBuf;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class RecruitPanel : GUIEntity
{
    private UITexture _EqAvartarTex;
    private UIButton _eqMultiButton;
    private BoxCollider _eqMultiCollider;
    private UIButton _eqSingleButton;
    private BoxCollider _eqSingleCollider;
    private UILabel _freeEqLabel;
    private UILabel _freeStoneLabel;
    private UITexture _StoneAvartarTex;
    private UIButton _stoneMultiButton;
    private BoxCollider _stoneMultiCollider;
    private UIButton _stoneSingleButton;
    private BoxCollider _stoneSingleCollider;
    public static function actived_function;
    private int EqFakeID = -1;
    private int StoneFakeID = -1;

    private void GenerateFocusObjects()
    {
        if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit))
        {
            List<GameObject> vari = new List<GameObject> {
                this.EqSingleButton.transform.FindChild("Icom").gameObject,
                this.EqSingleButton.transform.FindChild("name").gameObject
            };
            GuideSystem.ActivedGuide.RequestMultiGeneration(GuideRegister_Recruit.tag_recruit_press_button, vari);
        }
        else if (GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
        {
            List<GameObject> list2 = new List<GameObject> {
                this.StoneSingleButton.transform.FindChild("Icom").gameObject,
                this.StoneSingleButton.transform.FindChild("name").gameObject
            };
            GuideSystem.ActivedGuide.RequestMultiGeneration(GuideRegister_Recruit.tag_recruit_press_button, list2);
        }
    }

    public static void NewbieGenerate()
    {
        RecruitPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<RecruitPanel>();
        if (null == activityGUIEntity)
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else
        {
            activityGUIEntity.GenerateFocusObjects();
        }
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
                <OnClickRecruitButton>c__AnonStorey25B storeyb = new <OnClickRecruitButton>c__AnonStorey25B {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyb.<>m__555), null);
            }
            else if ((null != component) && ((component.card_entry >= 0) && (component.opt_entry >= 0)))
            {
                if (GuideSystem.MatchEvent(GuideEvent.GoldRecruit) || GuideSystem.MatchEvent(GuideEvent.StoneRecruit))
                {
                    GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Recruit.tag_recruit_press_button, null);
                }
                Debug.Log("send msg recruit");
                actived_function = (component.card_entry > 0) ? function.gold : function.stone;
                bool flag = false;
                object obj3 = GUIDataHolder.getData(go);
                if ((obj3 != null) && (((int) obj3) == 1))
                {
                    flag = true;
                }
                SocketMgr.Instance.RequestDrawLotteryCard(component.card_entry, component.opt_entry, false, flag);
                this.RefreshBtnStatus();
                GUIMgr.Instance.Lock();
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.GenerateFocusObjects();
        SocketMgr.Instance.RequestLotteryCardInfo();
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
    }

    public override void OnDestroy()
    {
        FakeCharacter.GetInstance().DestroyCharater(this.EqFakeID);
        FakeCharacter.GetInstance().DestroyCharater(this.StoneFakeID);
    }

    public override void OnInitialize()
    {
        RefreshButton(0, 0, this.StoneSingleButton, true, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
        RefreshButton(0, 1, this.StoneMultiButton, true, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
        RefreshButton(1, 2, this.EqSingleButton, false, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
        RefreshButton(1, 3, this.EqMultiButton, false, new UIEventListener.VoidDelegate(this.OnClickRecruitButton));
        EventCenter.Instance.DoEvent(EventCenter.EventType.UIShow_Recruit, null);
        this.EqFakeID = FakeCharacter.GetInstance().SnapCardCharacter(200, 0, null, ModelViewType.side, this.EqAvartarTex, 1);
        this.StoneFakeID = FakeCharacter.GetInstance().SnapCardCharacter(0xc9, 0, null, ModelViewType.side, this.StoneAvartarTex, 1);
        this.DelayCallBack(0.5f, delegate {
            GameObject objectByID = FakeCharacter.GetInstance().GetObjectByID(this.EqFakeID);
            if (null != objectByID)
            {
                Animation component = objectByID.GetComponent<Animation>();
                if (null != component)
                {
                    AnimationState state = component["zhanli01"];
                    state.wrapMode = WrapMode.Loop;
                }
            }
            GameObject obj3 = FakeCharacter.GetInstance().GetObjectByID(this.StoneFakeID);
            if (null != obj3)
            {
                Animation animation2 = obj3.GetComponent<Animation>();
                if (null != animation2)
                {
                    AnimationState state2 = animation2["zhanli01"];
                    state2.wrapMode = WrapMode.Loop;
                }
            }
        });
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.RefreshBtnStatus();
        if (GameDataMgr.Instance.boostRecruit.valid)
        {
            UpdateLotteryOptionStatus(this.StoneSingleButton.gameObject, this.FreeStoneLabel);
            UpdateLotteryOptionStatus(this.EqSingleButton.gameObject, this.FreeEqLabel);
            UpdateLotteryOptionStatus(this.EqMultiButton.gameObject, null);
            UpdateLotteryOptionStatus(this.StoneMultiButton.gameObject, null);
        }
    }

    private void RefreshBtnStatus()
    {
        bool valid = GameDataMgr.Instance.boostRecruit.valid;
        this.StoneSingleCollider.enabled = valid;
        this.StoneMultiCollider.enabled = valid;
        this.EqSingleCollider.enabled = valid;
        this.EqMultiCollider.enabled = valid;
        this.StoneSingleButton.enabled = valid;
        this.StoneMultiButton.enabled = valid;
        this.EqSingleButton.enabled = valid;
        this.EqMultiButton.enabled = valid;
    }

    public static void RefreshButton(int card_entry, int opt_entry, UIButton btn, bool is_stone, UIEventListener.VoidDelegate clickDelg)
    {
        lottery_card_option_config _config = ConfigMgr.getInstance().getByEntry<lottery_card_option_config>(opt_entry);
        if (_config != null)
        {
            btn.transform.FindChild("price").GetComponent<UILabel>().text = !is_stone ? _config.cost_gold.ToString() : _config.cost_stone.ToString();
            LotteryOptionStatus status = btn.gameObject.AddComponent<LotteryOptionStatus>();
            status.card_entry = card_entry;
            status.opt_entry = opt_entry;
            UIEventListener.Get(btn.gameObject).onClick = null;
            UIEventListener listener1 = UIEventListener.Get(btn.gameObject);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, clickDelg);
        }
    }

    private void UpdateCurrentEq(UILabel label)
    {
        label.gameObject.SetActive(true);
        label.text = ConfigMgr.getInstance().GetWord(0x84) + " : " + ActorData.getInstance().UserInfo.eq.ToString();
    }

    public static void UpdateLotteryOptionStatus(GameObject btn, UILabel free_tm)
    {
        BoostRecruit boostRecruit = GameDataMgr.Instance.boostRecruit;
        LotteryOptionStatus component = btn.GetComponent<LotteryOptionStatus>();
        if (null != component)
        {
            lottery_card_option_config _config = ConfigMgr.getInstance().getByEntry<lottery_card_option_config>(component.opt_entry);
            if (_config != null)
            {
                UILabel label = btn.transform.FindChild("price").GetComponent<UILabel>();
                bool flag = false;
                if ((_config.free_cd < 0) || (_config.free_type < 0))
                {
                    if (null != free_tm)
                    {
                        free_tm.gameObject.SetActive(false);
                    }
                }
                else
                {
                    long time = 0L;
                    int num2 = boostRecruit.FreeCount(component.opt_entry);
                    if (boostRecruit.FreeTime(component.opt_entry, out time))
                    {
                        if (null != free_tm)
                        {
                            if (num2 > 0)
                            {
                                free_tm.gameObject.SetActive(true);
                                free_tm.text = string.Format(ConfigMgr.getInstance().GetWord(0x26ac), num2);
                            }
                            else
                            {
                                free_tm.gameObject.SetActive(false);
                            }
                        }
                        flag = true;
                        label.text = ConfigMgr.getInstance().GetWord(100);
                        GUIDataHolder.setData(label.gameObject, 0);
                    }
                    else
                    {
                        string str = string.Empty;
                        long num3 = time / 0xe10L;
                        time = time % 0xe10L;
                        str = ((str + num3.ToString("D2") + ":") + ((time / 60L)).ToString("D2") + ":") + ((time % 60L)).ToString("D2");
                        if (null != free_tm)
                        {
                            free_tm.gameObject.SetActive(true);
                            string word = string.Format(ConfigMgr.getInstance().GetWord(0x98), str);
                            if (num2 == 0)
                            {
                                word = ConfigMgr.getInstance().GetWord(0x26ad);
                            }
                            free_tm.text = word;
                        }
                    }
                }
                UISprite sprite = btn.transform.parent.FindChild("ico").GetComponent<UISprite>();
                GUIDataHolder.setData(btn, null);
                if (_config.cost_stone > 0)
                {
                    if (flag)
                    {
                        sprite.spriteName = "Item_Icon_Stone";
                    }
                    else if (_config.count == 1)
                    {
                        Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Stone_Draw);
                        if (ticketItemBySubType != null)
                        {
                            label.text = "X" + ticketItemBySubType.num;
                            item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
                            if (_config2 != null)
                            {
                                sprite.spriteName = _config2.icon;
                            }
                            GUIDataHolder.setData(label.gameObject, 0);
                            GUIDataHolder.setData(btn, 1);
                        }
                        else
                        {
                            label.text = _config.cost_stone.ToString();
                            GUIDataHolder.setData(label.gameObject, _config.cost_stone);
                            sprite.spriteName = "Item_Icon_Stone";
                        }
                    }
                    else
                    {
                        Item item2 = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Stone_Ten_Draw);
                        if (item2 != null)
                        {
                            label.text = "X" + item2.num;
                            item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                            if (_config3 != null)
                            {
                                sprite.spriteName = _config3.icon;
                            }
                            GUIDataHolder.setData(label.gameObject, 0);
                            GUIDataHolder.setData(btn, 1);
                        }
                        else
                        {
                            label.text = _config.cost_stone.ToString();
                            sprite.spriteName = "Item_Icon_Stone";
                            GUIDataHolder.setData(label.gameObject, _config.cost_stone);
                        }
                    }
                }
                if (_config.cost_gold > 0)
                {
                    if (flag)
                    {
                        sprite.spriteName = "Item_Icon_Gold";
                    }
                    else if (_config.count == 1)
                    {
                        Item item3 = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Gold_Draw);
                        if (item3 != null)
                        {
                            label.text = "X" + item3.num;
                            item_config _config4 = ConfigMgr.getInstance().getByEntry<item_config>(item3.entry);
                            if (_config4 != null)
                            {
                                sprite.spriteName = _config4.icon;
                            }
                            GUIDataHolder.setData(btn, 1);
                            GUIDataHolder.setData(label.gameObject, 0);
                        }
                        else
                        {
                            label.text = _config.cost_gold.ToString();
                            sprite.spriteName = "Item_Icon_Gold";
                            GUIDataHolder.setData(label.gameObject, _config.cost_gold);
                        }
                    }
                    else
                    {
                        Item item4 = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Gold_Ten_Draw);
                        if (item4 != null)
                        {
                            label.text = "X" + item4.num;
                            item_config _config5 = ConfigMgr.getInstance().getByEntry<item_config>(item4.entry);
                            if (_config5 != null)
                            {
                                sprite.spriteName = _config5.icon;
                            }
                            GUIDataHolder.setData(btn, 1);
                            GUIDataHolder.setData(label.gameObject, 0);
                        }
                        else
                        {
                            label.text = _config.cost_gold.ToString();
                            sprite.spriteName = "Item_Icon_Gold";
                            GUIDataHolder.setData(label.gameObject, _config.cost_gold);
                        }
                    }
                }
            }
        }
    }

    private UITexture EqAvartarTex
    {
        get
        {
            if (null == this._EqAvartarTex)
            {
                this._EqAvartarTex = base.transform.FindChild("left/avartar").GetComponent<UITexture>();
            }
            return this._EqAvartarTex;
        }
    }

    private UIButton EqMultiButton
    {
        get
        {
            if (null == this._eqMultiButton)
            {
                this._eqMultiButton = base.transform.FindChild("left/btn1/btn").GetComponent<UIButton>();
            }
            return this._eqMultiButton;
        }
    }

    private BoxCollider EqMultiCollider
    {
        get
        {
            if (null == this._eqMultiCollider)
            {
                this._eqMultiCollider = base.transform.FindChild("left/btn1/btn").GetComponent<BoxCollider>();
            }
            return this._eqMultiCollider;
        }
    }

    private UIButton EqSingleButton
    {
        get
        {
            if (null == this._eqSingleButton)
            {
                this._eqSingleButton = base.transform.FindChild("left/btn0/btn").GetComponent<UIButton>();
            }
            return this._eqSingleButton;
        }
    }

    private BoxCollider EqSingleCollider
    {
        get
        {
            if (null == this._eqSingleCollider)
            {
                this._eqSingleCollider = base.transform.FindChild("left/btn0/btn").GetComponent<BoxCollider>();
            }
            return this._eqSingleCollider;
        }
    }

    private UILabel FreeEqLabel
    {
        get
        {
            if (null == this._freeEqLabel)
            {
                this._freeEqLabel = base.transform.FindChild("left/time").GetComponent<UILabel>();
            }
            return this._freeEqLabel;
        }
    }

    private UILabel FreeStoneLabel
    {
        get
        {
            if (null == this._freeStoneLabel)
            {
                this._freeStoneLabel = base.transform.FindChild("right/time").GetComponent<UILabel>();
            }
            return this._freeStoneLabel;
        }
    }

    private UITexture StoneAvartarTex
    {
        get
        {
            if (null == this._StoneAvartarTex)
            {
                this._StoneAvartarTex = base.transform.FindChild("right/avartar").GetComponent<UITexture>();
            }
            return this._StoneAvartarTex;
        }
    }

    private UIButton StoneMultiButton
    {
        get
        {
            if (null == this._stoneMultiButton)
            {
                this._stoneMultiButton = base.transform.FindChild("right/btn1/btn").GetComponent<UIButton>();
            }
            return this._stoneMultiButton;
        }
    }

    private BoxCollider StoneMultiCollider
    {
        get
        {
            if (null == this._stoneMultiCollider)
            {
                this._stoneMultiCollider = base.transform.FindChild("right/btn1/btn").GetComponent<BoxCollider>();
            }
            return this._stoneMultiCollider;
        }
    }

    private UIButton StoneSingleButton
    {
        get
        {
            if (null == this._stoneSingleButton)
            {
                this._stoneSingleButton = base.transform.FindChild("right/btn0/btn").GetComponent<UIButton>();
            }
            return this._stoneSingleButton;
        }
    }

    private BoxCollider StoneSingleCollider
    {
        get
        {
            if (null == this._stoneSingleCollider)
            {
                this._stoneSingleCollider = base.transform.FindChild("right/btn0/btn").GetComponent<BoxCollider>();
            }
            return this._stoneSingleCollider;
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickRecruitButton>c__AnonStorey25B
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__555(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__556(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    public enum function
    {
        stone,
        gold,
        soul
    }
}

