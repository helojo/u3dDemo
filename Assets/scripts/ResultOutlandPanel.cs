using Battle;
using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class ResultOutlandPanel : GUIEntity
{
    public UIButton _dataBtn;
    private bool _isEnd;
    private bool _isPass;
    private bool _isQuit;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    public bool CanReturn = true;
    public GameObject DropItem;
    public static ResultOutlandPanel inst;
    public UIScrollView itemView;
    private BattleType mBattleReturnType;
    public static bool show;
    public string tempStr = string.Empty;

    private void ClickData()
    {
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = obj => ((BattleBalancePanel) obj).SetValue();
        }
        GUIMgr.Instance.DoModelGUI("BattleBalancePanel", <>f__am$cacheC, null);
    }

    private void ClickShare()
    {
    }

    private void ClickSkipfunc1()
    {
        this.SetUIRootFsmVal(1);
        this.SentEndEvent(7);
    }

    private void ClickSkipfunc2()
    {
        this.SetUIRootFsmVal(2);
        this.SentEndEvent(7);
    }

    private void ClickSkipfunc3()
    {
        this.SetUIRootFsmVal(3);
        this.SentEndEvent(7);
    }

    private void ClickSkipfunc4()
    {
        this.SetUIRootFsmVal(4);
        this.SentEndEvent(7);
    }

    private void CreateDropControl(List<DropData> _DataList, UIGrid grid)
    {
        foreach (DropData data in _DataList)
        {
            int num;
            GameObject go = UnityEngine.Object.Instantiate(this.DropItem) as GameObject;
            go.transform.parent = grid.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
            GUIDataHolder.setData(go, data);
            UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickItemBtn);
            UISprite component = go.transform.FindChild("frame").GetComponent<UISprite>();
            UITexture texture = go.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label = go.transform.FindChild("Num").GetComponent<UILabel>();
            GameObject gameObject = go.transform.FindChild("Patch").gameObject;
            label.text = string.Empty;
            switch (data.type)
            {
                case RewardType.Card:
                {
                    NewCard card = (NewCard) data.data;
                    if (card.newCard.Count > 0)
                    {
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.newCard[0].cardInfo.entry);
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetQualityColor(component, card.newCard[0].cardInfo.quality);
                        gameObject.SetActive(false);
                    }
                    else if (card.newItem.Count > 0)
                    {
                        item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(card.newItem[0].entry);
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        CommonFunc.SetQualityColor(component, _config2.quality);
                        label.text = card.newItem[0].diff.ToString();
                        gameObject.SetActive(true);
                    }
                    num = 0x5c;
                    texture.width = num;
                    texture.height = num;
                    break;
                }
                case RewardType.Item:
                {
                    Item item = (Item) data.data;
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
                    texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                    CommonFunc.SetQualityColor(component, _config3.quality);
                    label.text = item.diff.ToString();
                    if ((_config3.type == 3) || (_config3.type == 2))
                    {
                        num = (_config3.type != 3) ? 0x48 : 0x5c;
                        texture.width = num;
                        texture.height = num;
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
        grid.repositionNow = true;
    }

    private void EnableItemListDrag(int count)
    {
        if (this.itemView != null)
        {
            if (count >= 5)
            {
                this.itemView.enabled = true;
            }
            else
            {
                this.itemView.enabled = false;
            }
        }
    }

    private List<DropData> GetDropData(BattleReward _data)
    {
        List<DropData> list = new List<DropData>();
        ActorData.getInstance().UpdateItemList(_data.items);
        foreach (Item item in _data.items)
        {
            DropData data = new DropData {
                type = RewardType.Item,
                data = item
            };
            list.Add(data);
        }
        ActorData.getInstance().UpdateNewCard(_data.cards);
        foreach (NewCard card in _data.cards)
        {
            DropData data2 = new DropData {
                type = RewardType.Card,
                data = card
            };
            list.Add(data2);
        }
        return list;
    }

    private void OnClickCloseBtn()
    {
    }

    private void OnClickItemBtn(GameObject go, bool isPress)
    {
        <OnClickItemBtn>c__AnonStorey1D0 storeyd = new <OnClickItemBtn>c__AnonStorey1D0 {
            go = go
        };
        if (isPress)
        {
            <OnClickItemBtn>c__AnonStorey1CF storeycf = new <OnClickItemBtn>c__AnonStorey1CF {
                <>f__ref$464 = storeyd
            };
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(storeyd.go);
            if (obj2 != null)
            {
                DropData data = (DropData) obj2;
                if (data.type == RewardType.Item)
                {
                    storeycf.item = data.data as Item;
                    if (storeycf.item != null)
                    {
                        GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storeycf.<>m__2DB), null);
                    }
                }
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    private void OnClickOKBtn(GameObject go)
    {
        if (this._isQuit)
        {
            ActorData.getInstance().isOutlandGrid = true;
            BattleStaticEntry.ExitBattle();
            GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ResultOutlandPanel");
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(this._isPass, false);
            BattleState.GetInstance().CurGame.battleGameData.OnMsgOutlandCameraEnable(true, false);
        }
    }

    private void OnClickReplayBtn(GameObject go)
    {
        BattleState.GetInstance().CurGame.battleGameData.OnMsgOutlandCameraEnable(true, false);
        GUIMgr.Instance.ExitModelGUI("ResultOutlandPanel");
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.Depth = 600;
                panel.mBattleType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cacheB, null);
    }

    private void OnClickSkillBtn(GameObject go)
    {
    }

    public override void OnInitialize()
    {
        inst = this;
        Transform transform = base.transform.FindChild("Failed/Combat/BtnReplay");
        if (transform != null)
        {
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickReplayBtn);
        }
        Transform transform2 = base.transform.FindChild("Win/BtnShare");
        if (transform2 != null)
        {
            transform2.gameObject.SetActive(false);
        }
        Transform transform3 = base.transform.FindChild("BtnOk");
        if (transform3 != null)
        {
            UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOKBtn);
        }
        Transform transform4 = base.transform.FindChild("Win/Pass/BtnSkill");
        if (transform4 != null)
        {
            UIEventListener.Get(transform4.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSkillBtn);
        }
    }

    private void SentEndEvent(int _returnType)
    {
        PlayMakerFSM component = base.transform.FindChild("BtnOk").GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            component.FsmVariables.FindFsmInt("BattleReturnType").Value = _returnType;
            component.SendEvent("CALL_BATTLE_EVENT");
            BattleStaticEntry.ExitBattle();
            ActorData.getInstance().JumpMainReplayDup = false;
        }
    }

    private void SetHeroInfo(Transform _obj, Card _card, float _hpValue, float _cdValue)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                _obj.FindChild("Lv").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
                _obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetQualityBorder(_obj.FindChild("QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                UISlider component = _obj.transform.FindChild("Hp").GetComponent<UISlider>();
                component.gameObject.SetActive(true);
                UISlider slider2 = _obj.transform.FindChild("Cd").GetComponent<UISlider>();
                slider2.gameObject.SetActive(true);
                for (int i = 0; i < 5; i++)
                {
                    UISprite sprite2 = _obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    sprite2.gameObject.SetActive(i < _card.cardInfo.starLv);
                    sprite2.transform.localPosition = new Vector3((float) (i * 0x17), 0f, 0f);
                }
                Transform transform = _obj.transform.FindChild("Star");
                transform.localPosition = new Vector3(-15.8f - ((_card.cardInfo.starLv - 1) * 11.5f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
                component.sliderValue = _hpValue;
                slider2.sliderValue = (component.sliderValue != 0f) ? _cdValue : 0f;
                _obj.transform.FindChild("Dead").gameObject.SetActive(component.sliderValue == 0f);
            }
        }
    }

    private void SetUIRootFsmVal(int _val)
    {
        PlayMakerFSM component = GUIMgr.Instance.Root.GetComponent<PlayMakerFSM>();
        if (null != component)
        {
            component.FsmVariables.FindFsmInt("OpenFunc").Value = _val;
        }
    }

    private void UpdateBossItem(List<OutlandBossInfo> _boosInfos)
    {
        <UpdateBossItem>c__AnonStorey1CE storeyce = new <UpdateBossItem>c__AnonStorey1CE {
            _boosInfos = _boosInfos
        };
        bool flag = false;
        for (int i = 0; i < storeyce._boosInfos.Count; i++)
        {
            <UpdateBossItem>c__AnonStorey1CD storeycd = new <UpdateBossItem>c__AnonStorey1CD {
                index = i + 1
            };
            OutlandBossInfo info = storeyce._boosInfos[i];
            if ((info != null) && (info.eventID != -1))
            {
                outland_event_config _config = ConfigMgr.getInstance().getByEntry<outland_event_config>(info.eventID);
                if (_config != null)
                {
                    monster_config _config2 = ConfigMgr.getInstance().getByEntry<monster_config>(StrParser.ParseDecInt(_config.model, -1));
                    if (_config2 != null)
                    {
                        card_config _config3 = ConfigMgr.getInstance().getByEntry<card_config>(_config2.card_entry);
                        if (_config3 != null)
                        {
                            Transform transform = base.transform.FindChild("Win/Pass/Boss/" + storeycd.index);
                            if (transform != null)
                            {
                                transform.gameObject.SetActive(true);
                                UITexture component = transform.transform.FindChild("Icon").GetComponent<UITexture>();
                                if (component != null)
                                {
                                    component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config3.image);
                                }
                                UISprite sprite = transform.transform.FindChild("QualityBorder").GetComponent<UISprite>();
                                if (sprite != null)
                                {
                                    switch (_config.sub_type)
                                    {
                                        case 2:
                                            sprite.spriteName = "Ui_Hero_Frame_1";
                                            break;

                                        case 3:
                                            sprite.spriteName = "Ui_Hero_Frame_4";
                                            break;
                                    }
                                }
                                if (info.state == 1)
                                {
                                    <UpdateBossItem>c__AnonStorey1CC storeycc = new <UpdateBossItem>c__AnonStorey1CC {
                                        <>f__ref$462 = storeyce,
                                        <>f__ref$461 = storeycd
                                    };
                                    flag = true;
                                    storeycc.skill = transform.transform.FindChild("Skill");
                                    if (storeycc.skill != null)
                                    {
                                        this.DelayCallBack(((float) storeycd.index) / 2f, new System.Action(storeycc.<>m__2DA));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (!flag)
        {
        }
    }

    private void UpdateDropItem(BattleReward _data)
    {
        List<DropData> dropData = this.GetDropData(_data);
        UIGrid component = base.gameObject.transform.FindChild("Win/Combat/GetItem/Scroll View/Grid").GetComponent<UIGrid>();
        this.CreateDropControl(dropData, component);
        this.EnableItemListDrag(dropData.Count);
    }

    private void UpdateDropItemForQuit(List<ItemProperty> itemList)
    {
        List<DropData> list = new List<DropData>();
        foreach (ItemProperty property in itemList)
        {
            if (property.type == ItemType.ItemType_Item)
            {
                DropData data = new DropData {
                    type = RewardType.Item
                };
                Item item = new Item {
                    entry = property.item_entry,
                    diff = property.item_param
                };
                data.data = item;
                list.Add(data);
            }
        }
        UIGrid component = base.gameObject.transform.FindChild("Win/Pass/GetItem/Scroll View/Grid").GetComponent<UIGrid>();
        this.CreateDropControl(list, component);
        this.EnableItemListDrag(list.Count);
    }

    private void UpdateHeroGroup(List<Card> _CardList)
    {
        for (int i = 0; i < 5; i++)
        {
            Transform transform = base.transform.FindChild("HeroGroup/" + (i + 1));
            if (i < _CardList.Count)
            {
                float num2 = 1f;
                float num3 = 0f;
                if (this.mBattleReturnType == BattleType.YuanZhengPk)
                {
                    FlameBattleSelfData flameCardByEntry = ActorData.getInstance().GetFlameCardByEntry((int) _CardList[i].cardInfo.entry);
                    if (flameCardByEntry != null)
                    {
                        num2 = ((float) flameCardByEntry.card_cur_hp) / ((float) CommonFunc.GetCardCurrMaxHp(_CardList[i]));
                        if (num2 != 0f)
                        {
                            num3 = ((float) flameCardByEntry.card_cur_energy) / ((float) AiDef.MAX_ENERGY);
                        }
                    }
                }
                else if ((this.mBattleReturnType >= BattleType.OutLandBattle_tollGate_0) && (this.mBattleReturnType <= BattleType.OutLandBattle_tollGate_3))
                {
                    OutlandBattleBackCardInfo outlandCardByEntry = ActorData.getInstance().GetOutlandCardByEntry((int) _CardList[i].cardInfo.entry);
                    if (outlandCardByEntry != null)
                    {
                        num2 = ((float) outlandCardByEntry.card_cur_hp) / ((float) CommonFunc.GetCardCurrMaxHp(_CardList[i]));
                        if (num2 != 0f)
                        {
                            num3 = ((float) outlandCardByEntry.card_cur_energy) / ((float) AiDef.MAX_ENERGY);
                        }
                    }
                }
                this.SetHeroInfo(transform, _CardList[i], num2, num3);
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateOutlandCombatResult(S2C_OutlandCombatEndReq _data, bool isQuit)
    {
        if (this._dataBtn != null)
        {
            this._dataBtn.gameObject.SetActive(true);
        }
        this._isQuit = isQuit;
        if (_data != null)
        {
            this._isPass = _data.is_pass;
            this.mBattleReturnType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
            if (_data.is_pass)
            {
                GameObject gameObject = base.gameObject.transform.FindChild("Win/Money").gameObject;
                UILabel component = base.gameObject.transform.FindChild("Win/Money/Val").gameObject.GetComponent<UILabel>();
                if ((component != null) && (gameObject != null))
                {
                    if (_data.reward.outland_coin <= 0)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    else
                    {
                        component.text = _data.reward.outland_coin.ToString();
                    }
                }
                switch (ActorData.getInstance().outlandType)
                {
                    case 0:
                        this.UpdateHeroGroup(ActorData.getInstance().mDefaultOutlandPkList_0);
                        break;

                    case 1:
                        this.UpdateHeroGroup(ActorData.getInstance().mDefaultOutlandPkList_1);
                        break;

                    case 2:
                        this.UpdateHeroGroup(ActorData.getInstance().mDefaultOutlandPkList_2);
                        break;

                    case 3:
                        this.UpdateHeroGroup(ActorData.getInstance().mDefaultOutlandPkList_3);
                        break;
                }
                this.UpdateDropItem(_data.reward);
            }
            this.UpdateOutlandWinOrFaildPanel(_data.is_pass, isQuit);
        }
    }

    public void UpdateOutlandQuitResult(S2C_OutlandQuit _data, bool isQuit)
    {
        if (this._dataBtn != null)
        {
            this._dataBtn.gameObject.SetActive(false);
        }
        this.mBattleReturnType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
        this._isQuit = isQuit;
        if (_data != null)
        {
            this._isPass = _data.isPass;
            if (_data.isPass)
            {
                base.gameObject.transform.FindChild("Win/Money").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Win/Pass/MoneyOutland").gameObject.SetActive(true);
                GameObject gameObject = base.gameObject.transform.FindChild("Win/Pass/MoneyOutland").gameObject;
                UILabel component = base.gameObject.transform.FindChild("Win/Pass/MoneyOutland/Val").gameObject.GetComponent<UILabel>();
                if ((component != null) && (gameObject != null))
                {
                    if (_data.cumul_outland_coin <= 0)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    else
                    {
                        component.text = _data.cumul_outland_coin.ToString();
                    }
                }
                GameObject obj3 = base.gameObject.transform.FindChild("Win/Pass/Exp").gameObject;
                UILabel label2 = base.gameObject.transform.FindChild("Win/Pass/Exp/Val").gameObject.GetComponent<UILabel>();
                if ((obj3 != null) && (label2 != null))
                {
                    if (_data.add_exp < 0)
                    {
                        _data.add_exp = 0;
                    }
                    label2.text = _data.add_exp.ToString();
                }
                if (_data.bossInfo != null)
                {
                    List<OutlandBossInfo> list = new List<OutlandBossInfo>();
                    foreach (OutlandBossInfo info in _data.bossInfo)
                    {
                        if (info.entry == ActorData.getInstance().outlandEntry)
                        {
                            list.Add(info);
                        }
                    }
                    if ((list != null) && (list.Count > 0))
                    {
                        this.UpdateBossItem(list);
                    }
                }
                if ((_data.box_item_list != null) && (_data.box_item_list.itemList.Count > 0))
                {
                    this.UpdateDropItemForQuit(_data.box_item_list.itemList);
                }
            }
            else
            {
                GameObject obj4 = base.gameObject.transform.FindChild("Failed/Pass/PassMoney").gameObject;
                UILabel label3 = base.gameObject.transform.FindChild("Failed/Pass/PassMoney/Val").gameObject.GetComponent<UILabel>();
                if ((label3 != null) && (obj4 != null))
                {
                    if (_data.cumul_outland_coin <= 0)
                    {
                        obj4.gameObject.SetActive(false);
                        base.gameObject.transform.FindChild("Failed/Pass/Endlab").gameObject.GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x4e31);
                    }
                    else
                    {
                        label3.text = _data.cumul_outland_coin.ToString();
                    }
                }
            }
            this.UpdateOutlandWinOrFaildPanel(_data.isPass, isQuit);
        }
    }

    private void UpdateOutlandWinOrFaildPanel(bool _isWin, bool isQuit)
    {
        if (_isWin)
        {
            GameObject gameObject = base.gameObject.transform.FindChild("Win").gameObject;
            if (gameObject != null)
            {
                gameObject.SetActive(true);
                if (isQuit)
                {
                    gameObject.transform.FindChild("Pass").gameObject.SetActive(true);
                    UILabel component = base.gameObject.transform.FindChild("Win/Money/Label").gameObject.GetComponent<UILabel>();
                    if (component != null)
                    {
                        component.text = ConfigMgr.getInstance().GetWord(0x4e32);
                    }
                }
                else
                {
                    gameObject.transform.FindChild("Combat").gameObject.SetActive(true);
                    UILabel label2 = base.gameObject.transform.FindChild("Win/Money/Label").gameObject.GetComponent<UILabel>();
                    if (label2 != null)
                    {
                        label2.text = ConfigMgr.getInstance().GetWord(0x4e33);
                    }
                }
            }
            base.gameObject.transform.FindChild("Failed").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("Win").gameObject.SetActive(false);
            GameObject obj3 = base.gameObject.transform.FindChild("Failed").gameObject;
            if (obj3 != null)
            {
                obj3.SetActive(true);
                if (isQuit)
                {
                    ActorData.getInstance().outlandAllHerosDeadList[ActorData.getInstance().outlandType] = true;
                    obj3.transform.FindChild("Pass").gameObject.SetActive(true);
                }
                else
                {
                    obj3.transform.FindChild("Combat").gameObject.SetActive(true);
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1CF
    {
        internal ResultOutlandPanel.<OnClickItemBtn>c__AnonStorey1D0 <>f__ref$464;
        internal Item item;

        internal void <>m__2DB(GUIEntity entity)
        {
            (entity as ItemInfoPanel).UpdateData(this.item, this.<>f__ref$464.go.transform);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1D0
    {
        internal GameObject go;
    }

    [CompilerGenerated]
    private sealed class <UpdateBossItem>c__AnonStorey1CC
    {
        internal ResultOutlandPanel.<UpdateBossItem>c__AnonStorey1CD <>f__ref$461;
        internal ResultOutlandPanel.<UpdateBossItem>c__AnonStorey1CE <>f__ref$462;
        internal Transform skill;

        internal void <>m__2DA()
        {
            this.skill.gameObject.SetActive(true);
            TweenScale.Begin(this.skill.gameObject, 0.8f, new Vector3(1f, 1f, 1f)).method = UITweener.Method.BounceIn;
            if (this.<>f__ref$461.index == this.<>f__ref$462._boosInfos.Count)
            {
            }
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateBossItem>c__AnonStorey1CD
    {
        internal int index;
    }

    [CompilerGenerated]
    private sealed class <UpdateBossItem>c__AnonStorey1CE
    {
        internal List<OutlandBossInfo> _boosInfos;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DropData
    {
        public RewardType type;
        public object data;
    }
}

