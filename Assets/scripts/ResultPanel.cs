using Battle;
using FastBuf;
using Holoville.HOTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class ResultPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache8;
    public bool CanReturn = true;
    private S2C_DuplicateEndReq data_2;
    public GameObject DropItem;
    public UIScrollView itemView;
    private BattleType mBattleReturnType;
    public static bool show;
    public string tempStr = string.Empty;

    private void CheckArenaLadderHistroyTop(PushNotifyPanel.ShareType type, int curOrder, int bestOrder)
    {
        <CheckArenaLadderHistroyTop>c__AnonStorey1D6 storeyd = new <CheckArenaLadderHistroyTop>c__AnonStorey1D6 {
            type = type,
            curOrder = curOrder,
            bestOrder = bestOrder
        };
        if ((storeyd.curOrder >= 1) && (storeyd.curOrder < storeyd.bestOrder))
        {
            GUIMgr.Instance.DoModelGUI("PushNotifyPanel", new Action<GUIEntity>(storeyd.<>m__2E1), base.gameObject);
        }
    }

    private bool CheckIsFirstPass(DupCommonData dupData)
    {
        string[] strArray;
        <CheckIsFirstPass>c__AnonStorey1D1 storeyd = new <CheckIsFirstPass>c__AnonStorey1D1();
        if (dupData == null)
        {
            return false;
        }
        duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(dupData.dupEntry);
        if (_config == null)
        {
            return false;
        }
        strArray = (dupData.dupType != DuplicateType.DupType_Normal) ? (strArray = _config.elite_entry.Split(new char[] { '|' })) : (strArray = _config.normal_entry.Split(new char[] { '|' }));
        if (strArray.Length < 1)
        {
            return false;
        }
        storeyd.lastDupEntry = int.Parse(strArray[strArray.Length - 1]);
        if (storeyd.lastDupEntry != dupData.trenchEntry)
        {
            return false;
        }
        List<FastBuf.TrenchData> list = null;
        if (dupData.dupType == DuplicateType.DupType_Normal)
        {
            ActorData.getInstance().TrenchNormalDataDict.TryGetValue(dupData.dupEntry, out list);
        }
        else if (dupData.dupType == DuplicateType.DupType_Elite)
        {
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(dupData.dupEntry, out list);
        }
        if (list == null)
        {
            return false;
        }
        FastBuf.TrenchData data = list.Find(new Predicate<FastBuf.TrenchData>(storeyd.<>m__2DD));
        return ((data == null) || (data.grade == 0));
    }

    private void CheckPushNotifyPanel(S2C_DuplicateEndReq data)
    {
        <CheckPushNotifyPanel>c__AnonStorey1D2 storeyd = new <CheckPushNotifyPanel>c__AnonStorey1D2 {
            data = data
        };
        if (((storeyd.data != null) && SharePanel.IsCanShare()) && (SharePanel.mShareQQ || this.CheckIsFirstPass(storeyd.data.dupData)))
        {
            <CheckPushNotifyPanel>c__AnonStorey1D3 storeyd2 = new <CheckPushNotifyPanel>c__AnonStorey1D3 {
                <>f__ref$466 = storeyd,
                dc = ConfigMgr.getInstance().getByEntry<duplicate_config>(storeyd.data.dupData.dupEntry)
            };
            if (storeyd2.dc != null)
            {
                GUIMgr.Instance.DoModelGUI("PushNotifyPanel", new Action<GUIEntity>(storeyd2.<>m__2DE), null);
            }
        }
    }

    private bool CheckPushNotiTXPanel(S2C_DuplicateEndReq data)
    {
        <CheckPushNotiTXPanel>c__AnonStorey1D4 storeyd2;
        <CheckPushNotiTXPanel>c__AnonStorey1D5 storeyd = new <CheckPushNotiTXPanel>c__AnonStorey1D5();
        if (data != null)
        {
            if (!SharePanel.IsCanShare())
            {
                return false;
            }
            if (XSingleton<SocialFriend>.Singleton.State != SocialFriend.SocialState.Ready)
            {
                return false;
            }
            int ownerRank = XSingleton<SocialFriend>.Singleton.OwnerRank;
            Debug.Log("ResultPanel： " + ownerRank);
            XSingleton<SocialFriend>.Singleton.AddOwner(true, data.dupData.dupType);
            storeyd.newRank = XSingleton<SocialFriend>.Singleton.OwnerRank;
            if (storeyd.newRank >= ownerRank)
            {
                return false;
            }
            storeyd2 = new <CheckPushNotiTXPanel>c__AnonStorey1D4 {
                <>f__ref$469 = storeyd,
                normal = -1,
                elite = -1
            };
            if (data.dupData.dupType == DuplicateType.DupType_Elite)
            {
                storeyd2.elite = XSingleton<SocialFriend>.Singleton.Owner.UserInfo.Elite;
                goto Label_00FA;
            }
            if (data.dupData.dupType == DuplicateType.DupType_Normal)
            {
                storeyd2.normal = XSingleton<SocialFriend>.Singleton.Owner.UserInfo.Normal;
                goto Label_00FA;
            }
        }
        return false;
    Label_00FA:
        GUIMgr.Instance.DoModelGUI<PushNotiTXPanel>(new Action<GUIEntity>(storeyd2.<>m__2DF), null);
        return true;
    }

    private void ClickData()
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = obj => ((BattleBalancePanel) obj).SetValue();
        }
        GUIMgr.Instance.DoModelGUI("BattleBalancePanel", <>f__am$cache8, null);
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

    private void CreateDropControl(List<DropData> _DataList)
    {
        UIGrid component = base.gameObject.transform.FindChild("Win/GetItem/Scroll View/Grid").GetComponent<UIGrid>();
        foreach (DropData data in _DataList)
        {
            GameObject go = UnityEngine.Object.Instantiate(this.DropItem) as GameObject;
            go.transform.parent = component.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
            GUIDataHolder.setData(go, data);
            UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickItemBtn);
            UISprite sprite = go.transform.FindChild("frame").GetComponent<UISprite>();
            UITexture texture = go.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label = go.transform.FindChild("Num").GetComponent<UILabel>();
            GameObject gameObject = go.transform.FindChild("Patch").gameObject;
            GameObject obj4 = go.transform.FindChild("Tips").gameObject;
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
                        CommonFunc.SetQualityColor(sprite, card.newCard[0].cardInfo.quality);
                        gameObject.SetActive(false);
                    }
                    else if (card.newItem.Count > 0)
                    {
                        item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(card.newItem[0].entry);
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        CommonFunc.SetEquipQualityBorder(sprite, _config2.quality, false);
                        label.text = card.newItem[0].diff.ToString();
                        gameObject.SetActive(true);
                    }
                    texture.width = 0x5c;
                    texture.height = 0x5c;
                    break;
                }
                case RewardType.Item:
                {
                    Item item = (Item) data.data;
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
                    texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                    CommonFunc.SetEquipQualityBorder(sprite, _config3.quality, false);
                    label.text = item.diff.ToString();
                    obj4.gameObject.SetActive((data.mCardList != null) && (data.mCardList.Count > 0));
                    if ((_config3.type == 3) || (_config3.type == 2))
                    {
                        texture.width = (_config3.type != 3) ? 0x4a : 0x5c;
                        texture.height = (_config3.type != 3) ? 0x4a : 0x5c;
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        texture.width = 0x4a;
                        texture.height = 0x4a;
                        gameObject.SetActive(false);
                    }
                    break;
                }
            }
        }
        component.repositionNow = true;
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

    private int GetCardLevUpExp(Card _card)
    {
        card_lv_up_config _config = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(_card.cardInfo.level);
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
        switch (_config2.lv_up_type)
        {
            case 1:
                return _config.stage1_exp;

            case 2:
                return _config.stage2_exp;

            case 3:
                return _config.stage3_exp;

            case 4:
                return _config.stage4_exp;
        }
        return 0;
    }

    private List<DropData> GetDropData(BattleReward _data)
    {
        List<DropData> list = new List<DropData>();
        foreach (Item item in _data.items)
        {
            DropData data = new DropData {
                type = RewardType.Item,
                data = item,
                mCardList = XSingleton<EquipBreakMateMgr>.Singleton.GetCardListByLackOneItemByEquip(item.entry, item.num)
            };
            list.Add(data);
        }
        ActorData.getInstance().UpdateItemList(_data.items);
        foreach (NewCard card in _data.cards)
        {
            DropData data2 = new DropData {
                type = RewardType.Card,
                data = card
            };
            list.Add(data2);
        }
        ActorData.getInstance().UpdateNewCard(_data.cards);
        return list;
    }

    private void OnClickCloseBtn()
    {
        if (this.CanReturn)
        {
            if (this.mBattleReturnType == BattleType.GuildBattle)
            {
                ActorData.getInstance().m_entryFrom = 1;
            }
            if (ActorData.getInstance().mCurrDupReturnPrePara != null)
            {
                this.mBattleReturnType = BattleType.ChipFromJumpEvent;
            }
            this.SentEndEvent((int) this.mBattleReturnType);
        }
    }

    private void OnClickItemBtn(GameObject go, bool isPress)
    {
        <OnClickItemBtn>c__AnonStorey1D8 storeyd = new <OnClickItemBtn>c__AnonStorey1D8 {
            go = go
        };
        if (isPress)
        {
            <OnClickItemBtn>c__AnonStorey1D7 storeyd2 = new <OnClickItemBtn>c__AnonStorey1D7 {
                <>f__ref$472 = storeyd
            };
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(storeyd.go);
            if (obj2 != null)
            {
                storeyd2.data = (DropData) obj2;
                if (storeyd2.data.type == RewardType.Item)
                {
                    storeyd2.item = storeyd2.data.data as Item;
                    if (storeyd2.item != null)
                    {
                        GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storeyd2.<>m__2E2), base.gameObject);
                    }
                }
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    private void OnClickReplayBtn(GameObject go)
    {
        ActorData.getInstance().JumpMainReplayDup = true;
    }

    public override void OnInitialize()
    {
        Transform transform = base.transform.FindChild("BtnReplay");
        if (transform != null)
        {
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickReplayBtn);
        }
    }

    private void PlaySliderAnim(UISlider _slider, UILabel levLabel, int _lev, int _srcLev, float _toVal)
    {
        base.StartCoroutine(this.StartPlaySliderAnim(_slider, levLabel, _lev, _srcLev, _toVal));
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
                    sprite2.transform.localPosition = new Vector3((float) (i * 0x13), 0f, 0f);
                }
                Transform transform = _obj.transform.FindChild("Star");
                transform.localPosition = new Vector3(-15.8f - ((_card.cardInfo.starLv - 1) * 9.5f), transform.localPosition.y, 0f);
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

    private void ShowDupInfo(DupCommonData dupData)
    {
        Transform transform = base.transform.FindChild("Win/DupDesc");
        UILabel component = transform.FindChild("Label").GetComponent<UILabel>();
        if (dupData.dupType == DuplicateType.DupType_Normal)
        {
            duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(dupData.dupEntry);
            if (_config != null)
            {
                trench_normal_config _config2 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(dupData.trenchEntry);
                if (_config2 != null)
                {
                    this.tempStr = ConfigMgr.getInstance().GetWord(0x9ba3c1) + _config.name + "-" + _config2.name;
                }
            }
        }
        else if (dupData.dupType == DuplicateType.DupType_Elite)
        {
            duplicate_config _config3 = ConfigMgr.getInstance().getByEntry<duplicate_config>(dupData.dupEntry);
            if (_config3 != null)
            {
                trench_elite_config _config4 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(dupData.trenchEntry);
                if (_config4 != null)
                {
                    this.tempStr = ConfigMgr.getInstance().GetWord(0x9ba3c2) + _config3.name + "-" + _config4.name;
                }
            }
        }
        component.text = this.tempStr;
        transform.gameObject.SetActive(true);
    }

    public void ShowStarEffect(GameObject obj)
    {
        obj.SetActive(true);
        Space self = Space.Self;
        iTween.LoopType none = iTween.LoopType.none;
        object[] args = new object[] { 
            "amount", new Vector3(0.1f, 0.1f, 0f), "name", string.Empty, "time", 0.15f, "delay", 0, "looptype", none, "oncomplete", "iTweenOnComplete", "oncompleteparams", 0, "onstart", "iTweenOnStart", 
            "onstartparams", 0, "ignoretimescale", false, "space", self, "axis", string.Empty
         };
        iTween.ShakePosition(base.gameObject, iTween.Hash(args));
    }

    [DebuggerHidden]
    private IEnumerator StartBtnOK()
    {
        return new <StartBtnOK>c__Iterator74 { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator startLabelAnim(UILabel label, int _lev, int _srcLev)
    {
        return new <startLabelAnim>c__Iterator76 { _srcLev = _srcLev, label = label, _lev = _lev, <$>_srcLev = _srcLev, <$>label = label, <$>_lev = _lev };
    }

    [DebuggerHidden]
    private IEnumerator StartPlaySliderAnim(UISlider _slider, UILabel levLabel, int _lev, int _srcLev, float _toVal)
    {
        return new <StartPlaySliderAnim>c__Iterator75 { _lev = _lev, _srcLev = _srcLev, _slider = _slider, levLabel = levLabel, _toVal = _toVal, <$>_lev = _lev, <$>_srcLev = _srcLev, <$>_slider = _slider, <$>levLabel = levLabel, <$>_toVal = _toVal, <>f__this = this };
    }

    private void UpdaeFaildData()
    {
    }

    public void UpdateArenaLadderResult(S2C_ArenaLadderCombatEnd res)
    {
        this.mBattleReturnType = BattleType.ArenaLadder;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        if (res.isWin)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
            base.transform.FindChild("Win/Money").gameObject.SetActive(false);
            this.UpdatePlayCards(ActorData.getInstance().mDefaultArenaLadderList, 0);
        }
        this.UpdateWinOrFaildPanel(res.isWin);
        Debug.Log(string.Concat(new object[] { "CurrOrder: ", res.curOrder, "    --------------- BestOrder   ", res.bestOrder }));
        if (SharePanel.IsCanShare())
        {
            this.CheckArenaLadderHistroyTop(PushNotifyPanel.ShareType.ArenaLadderHistoryRankTop, res.curOrder, res.bestOrder);
        }
    }

    public void UpdateChallengeArenaLadderResult(S2C_LoLArenaCombatEnd res)
    {
        this.mBattleReturnType = BattleType.ChallengeDefense;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        if (res.isWin)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
            base.transform.FindChild("Win/Money").gameObject.SetActive(false);
            this.UpdatePlayCards(ArenaFormationUtility.GetFighterFormation(), 0);
        }
        this.UpdateWinOrFaildPanel(res.isWin);
        Debug.Log(string.Concat(new object[] { "CurrOrder: ", res.curOrder, "    --------------- BestOrder   ", res.bestOrder }));
        if (SharePanel.IsCanShare())
        {
            this.CheckArenaLadderHistroyTop(PushNotifyPanel.ShareType.LoLArenaLadderHistoryRankTop, res.curOrder, res.bestOrder);
        }
    }

    private void UpdateDeatinsDartData(int _gold, int _stone, int curcourage, int curGold, int curStone, int _courage)
    {
        base.transform.FindChild("Win/DetainsDartInfo").gameObject.SetActive(true);
        UILabel component = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Stone/Val").GetComponent<UILabel>();
        GameObject gameObject = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Stone").gameObject;
        if (component != null)
        {
            if (_stone >= 0)
            {
                gameObject.gameObject.SetActive(true);
            }
            else
            {
                gameObject.gameObject.SetActive(false);
            }
            component.text = _stone.ToString();
            if (curStone >= 0)
            {
                ActorData.getInstance().Stone = curStone;
            }
        }
        UILabel label2 = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Money/Val").GetComponent<UILabel>();
        GameObject obj3 = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Money").gameObject;
        if (label2 != null)
        {
            if (_gold >= 0)
            {
                obj3.gameObject.SetActive(true);
            }
            else
            {
                obj3.gameObject.SetActive(false);
            }
            label2.text = _gold.ToString();
            if (curGold >= 0)
            {
                ActorData.getInstance().Gold = curGold;
            }
        }
        UILabel label3 = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Courage/Val").GetComponent<UILabel>();
        GameObject obj4 = base.gameObject.transform.FindChild("Win/DetainsDartInfo/Courage").gameObject;
        if (label3 != null)
        {
            if (curcourage > 0)
            {
                obj4.gameObject.SetActive(true);
            }
            else
            {
                obj4.gameObject.SetActive(false);
            }
            label3.text = _courage.ToString();
            if (curcourage >= 0)
            {
                ActorData.getInstance().ArenaLadderCoin = curcourage;
            }
        }
    }

    public void UpdateDetainsDartBattleBackSelfResult(S2C_ConvoyAvengeCombatEnd res)
    {
        this.mBattleReturnType = BattleType.DetainsDartBattleBack;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
        base.transform.FindChild("Win/Money").gameObject.SetActive(false);
        base.transform.FindChild("Win/Star").gameObject.SetActive(false);
        if (res.isWin)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            this.UpdatePlayCards(ActorData.getInstance().mDefaultDetainsDart_BattleBackSelf_List, 0);
        }
        this.UpdateWinOrFaildPanel(res.isWin);
        if (res.isWin)
        {
            this.UpdateDeatinsDartData(res.incGold, -1, res.arenaLadderScore, res.currGold, -1, res.incArenaLadderScore);
        }
    }

    public void UpdateDetainsDartInterceptResult(S2C_RobConvoyCombatEnd res)
    {
        this.mBattleReturnType = BattleType.DerainsDartInterceptBattle;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
        base.transform.FindChild("Win/Money").gameObject.SetActive(false);
        base.transform.FindChild("Win/Star").gameObject.SetActive(false);
        if (res.isWin)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            this.UpdatePlayCards(ActorData.getInstance().mDefaultDetainsDart_Intercept_List, 0);
        }
        this.UpdateWinOrFaildPanel(res.isWin);
        if (res.isWin)
        {
            this.UpdateDeatinsDartData(res.incGold, res.incStone, -1, res.currGold, res.currStone, -1);
        }
    }

    private void UpdateDropItem(BattleReward _data)
    {
        List<DropData> dropData = this.GetDropData(_data);
        this.CreateDropControl(dropData);
        this.EnableItemListDrag(dropData.Count);
    }

    public void UpdateDungeonsResult(S2C_NewDungeonsCombatEndReq _data)
    {
        this.mBattleReturnType = BattleType.Dungeons;
        if ((_data.level != ActorData.getInstance().Level) && (_data.dupData.dupType == DuplicateType.DupType_Undertown))
        {
            ActorData data1 = ActorData.getInstance();
            data1.PhyForce -= ActorData.getInstance().m_nDungeonCostPhysical - 2;
        }
        if (_data.is_pass)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
        }
        base.transform.FindChild("Win/Exp").gameObject.SetActive(true);
        base.transform.FindChild("Win/Money").gameObject.SetActive(false);
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        base.transform.FindChild("Win/Star").gameObject.SetActive(false);
        this.UpdateWinOrFaildPanel(_data.is_pass);
        this.UpdateExpAndMoney((int) _data.reward.gold, _data.exp, (int) _data.reward.exp, _data.level);
        this.UpdatePlayCards(_data.cards, (int) _data.reward.card_exp);
        this.UpdateDropItem(_data.reward);
        ActorData.getInstance().PhyForce = _data.phyForce;
    }

    private void UpdateDupProcess(S2C_DuplicateEndReq _data)
    {
        this.CanReturn = false;
        SocketMgr.Instance.RequestGetDuplicateProgress();
        int dupEntry = _data.dupData.dupEntry;
        SocketMgr.Instance.RequestGetDuplicateRemain(dupEntry, DuplicateType.DupType_Normal);
        SocketMgr.Instance.RequestGetDuplicateRemain(dupEntry, DuplicateType.DupType_Elite);
    }

    private void UpdateExpAndMoney(int _gold, int _exp, int _getExp, int _level)
    {
        base.gameObject.transform.FindChild("Win/Exp/Val").GetComponent<UILabel>().text = _getExp.ToString();
        base.gameObject.transform.FindChild("Win/Money/Val").GetComponent<UILabel>().text = _gold.ToString();
        if (_level > 0)
        {
            ActorData.getInstance().Level = _level;
        }
        ActorData data1 = ActorData.getInstance();
        data1.Gold += _gold;
        if (_exp >= 0)
        {
            ActorData.getInstance().UserInfo.exp = _exp;
        }
    }

    public void UpdateFriendPkResult(S2C_FriendCombat data)
    {
        this.mBattleReturnType = BattleType.FriendPk;
        if (data != null)
        {
            base.transform.FindChild("Win").gameObject.SetActive(data.isWin);
            base.transform.FindChild("Failed").gameObject.SetActive(!data.isWin);
            if (data.isWin)
            {
                base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
                base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
                base.transform.FindChild("Win/Star").gameObject.SetActive(false);
                base.transform.FindChild("Win/Hero").gameObject.SetActive(true);
                base.transform.FindChild("Win/Money").gameObject.SetActive(false);
                base.transform.FindChild("Win/GetItem").gameObject.SetActive(false);
                this.UpdatePlayCards(ActorData.getInstance().mDefaultFriendPkList, 0);
            }
            base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
            ActorData.getInstance().FriendPkEndReturn = true;
        }
    }

    public void UpdateGuildBattleResult(S2C_GuildBattleCombatEnd _data)
    {
        this.mBattleReturnType = BattleType.GuildBattle;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        base.transform.FindChild("Win/BtnShare").gameObject.SetActive(false);
        base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
        base.transform.FindChild("Win/Money").gameObject.SetActive(false);
        base.transform.FindChild("Res").gameObject.SetActive(true);
        base.transform.FindChild("Contribute").gameObject.SetActive(true);
        base.transform.FindChild("Failed/Label").gameObject.SetActive(false);
        if (_data.isWin)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            this.UpdateHeroGroup(ActorData.getInstance().mDefaultGuildBattleList);
        }
        this.UpdateWinOrFaildPanel(_data.isWin);
        base.gameObject.transform.FindChild("Res/Val").GetComponent<UILabel>().text = _data.resourceCount.ToString();
        base.gameObject.transform.FindChild("Contribute/Val").GetComponent<UILabel>().text = _data.contribute.ToString();
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
                        num2 = ((float) flameCardByEntry.card_cur_hp) / ((float) flameCardByEntry.card_max_hp);
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

    public void UpdateOutlandResult(S2C_OutlandQuit _data, int toll_gate)
    {
        this.mBattleReturnType += toll_gate;
        if (_data.isPass)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(false);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            base.transform.FindChild("Win/Outland").gameObject.SetActive(true);
            base.transform.FindChild("Win/ui_win_light").gameObject.SetActive(false);
            switch (toll_gate)
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
        base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
        base.transform.FindChild("Win/Money").gameObject.SetActive(false);
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        this.UpdateOutlandWinOrFaildPanel(_data.isPass);
    }

    private void UpdateOutlandWinOrFaildPanel(bool _isWin)
    {
        if (_isWin)
        {
            base.gameObject.transform.FindChild("Win").gameObject.SetActive(true);
            base.gameObject.transform.FindChild("Failed").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("Win").gameObject.SetActive(false);
            base.gameObject.transform.FindChild("Failed").gameObject.SetActive(true);
            ActorData.getInstance().mFriendReward = null;
        }
    }

    private void UpdatePlayCards(List<Card> ListCard, int GetExp)
    {
        int num = 1;
        foreach (Card card in ListCard)
        {
            string name = "Win/Hero/" + num;
            if (num > 5)
            {
                Debug.LogWarning("List Is Too Big ,Length is " + ListCard.Count);
            }
            else
            {
                GameObject gameObject = base.gameObject.transform.FindChild(name).gameObject;
                gameObject.SetActive(true);
                UILabel component = gameObject.transform.FindChild("Exp/Exp").GetComponent<UILabel>();
                UILabel label2 = gameObject.transform.FindChild("Exp/TExp").GetComponent<UILabel>();
                GameObject obj3 = gameObject.transform.FindChild("Exp/ExpFull").gameObject;
                if (CommonFunc.CardExpIsFull(card))
                {
                    component.text = string.Empty;
                    obj3.SetActive(true);
                    label2.gameObject.SetActive(false);
                }
                else
                {
                    component.text = "+" + GetExp;
                    obj3.SetActive(false);
                    label2.gameObject.SetActive(true);
                }
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                if (_config == null)
                {
                    break;
                }
                gameObject.transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                for (int i = 0; i < 5; i++)
                {
                    UISprite sprite = gameObject.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    sprite.gameObject.SetActive(i < card.cardInfo.starLv);
                    sprite.transform.localPosition = new Vector3((float) (i * 0x13), 0f, 0f);
                }
                Transform transform = gameObject.transform.FindChild("Star");
                transform.localPosition = new Vector3(-13f - ((card.cardInfo.starLv - 1) * 9.5f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
                CommonFunc.SetQualityBorder(gameObject.transform.FindChild("QualityBorder").GetComponent<UISprite>(), card.cardInfo.quality);
                UISlider slider = gameObject.transform.FindChild("Progress Bar").GetComponent<UISlider>();
                Card cardByEntry = ActorData.getInstance().GetCardByEntry(card.cardInfo.entry);
                float num3 = ((float) cardByEntry.cardInfo.curExp) / ((float) this.GetCardLevUpExp(cardByEntry));
                float num4 = ((float) card.cardInfo.curExp) / ((float) this.GetCardLevUpExp(card));
                slider.value = num3;
                int level = cardByEntry.cardInfo.level;
                int curExp = (int) cardByEntry.cardInfo.curExp;
                UILabel levLabel = gameObject.transform.FindChild("Lv").GetComponent<UILabel>();
                levLabel.text = level.ToString();
                gameObject.transform.FindChild("LvUp").gameObject.gameObject.SetActive(card.cardInfo.level > level);
                this.PlaySliderAnim(slider, levLabel, card.cardInfo.level, level, num4);
                ActorData.getInstance().UpdateCard(card);
                num++;
            }
        }
    }

    public void UpdateResultData(S2C_DuplicateEndReq data)
    {
        if (data.level != ActorData.getInstance().Level)
        {
            if (data.dupData.dupType == DuplicateType.DupType_Normal)
            {
                ActorData data1 = ActorData.getInstance();
                data1.PhyForce -= ActorData.getInstance().m_nNormalDupCostPhysical - 1;
            }
            else if (data.dupData.dupType == DuplicateType.DupType_Elite)
            {
                ActorData data2 = ActorData.getInstance();
                data2.PhyForce -= ActorData.getInstance().m_nEliteDupCostPhysical - 2;
            }
        }
        this.UpdateWinOrFaildPanel(data.is_pass);
        if (data.is_pass)
        {
            Debug.Log("------------------------检测通关-----------------------------");
            this.UpdateWinData(data);
            this.ShowDupInfo(data.dupData);
            this.data_2 = data;
            show = true;
            this.UpdateDupProcess(data);
            this.UpdateResultData_2();
        }
        ActorData.getInstance().PhyForce = data.phyForce;
        SocketMgr.Instance.CheckLivenessReward();
    }

    public void UpdateResultData_2()
    {
        if (show)
        {
            if (!this.CheckPushNotiTXPanel(this.data_2))
            {
                this.CheckPushNotifyPanel(this.data_2);
            }
            show = false;
        }
    }

    private void UpdateStar(int _grade)
    {
        for (int i = 1; i <= 3; i++)
        {
            string name = "Win/Star/" + i;
            base.gameObject.transform.FindChild(name).gameObject.SetActive(i <= _grade);
        }
    }

    public void UpdateTowerPkResult(S2C_VoidTowerCombatEndReq data)
    {
        this.mBattleReturnType = BattleType.TowerPk;
        if (data != null)
        {
            base.transform.FindChild("Win").gameObject.SetActive(data.is_pass);
            base.transform.FindChild("Failed").gameObject.SetActive(!data.is_pass);
            if (data.is_pass)
            {
                base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
                base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
                base.transform.FindChild("Win/Star").gameObject.SetActive(false);
                base.transform.FindChild("Win/Hero").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Win/Money/Val").GetComponent<UILabel>().text = data.reward.gold.ToString();
                base.transform.FindChild("Win/Money").gameObject.SetActive(true);
                this.UpdateDropItem(data.reward);
                base.transform.FindChild("Win/GetItem").gameObject.SetActive(true);
                this.UpdatePlayCards(ActorData.getInstance().mDefaultTowerPkList, 0);
            }
            else
            {
                base.transform.FindChild("Win/GetItem").gameObject.SetActive(false);
            }
            base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
            SocketMgr.Instance.CheckLivenessReward();
        }
    }

    public void UpdateWarmmatchPkResult(S2C_WarmmatchCombatEndReq data)
    {
        this.mBattleReturnType = BattleType.WarmmatchPk;
        base.transform.FindChild("Win/WinSprite").gameObject.SetActive(data.battle_result);
        base.transform.FindChild("Win").gameObject.SetActive(data.battle_result);
        base.transform.FindChild("Failed").gameObject.SetActive(!data.battle_result);
        if (data.battle_result)
        {
            base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            base.transform.FindChild("Win/Hero").gameObject.SetActive(true);
            base.transform.FindChild("Win/Money").gameObject.SetActive(false);
            this.UpdateDropItem(data.reward);
            base.transform.FindChild("Win/GetItem").gameObject.SetActive(true);
            this.UpdatePlayCards(ActorData.getInstance().mDefaultWarmmatchList, 0);
        }
        else
        {
            base.transform.FindChild("Win/GetItem").gameObject.SetActive(false);
        }
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        SocketMgr.Instance.CheckLivenessReward();
    }

    private void UpdateWinData(S2C_DuplicateEndReq data)
    {
        ActorData.getInstance().mFriendReward = data.friend_reward;
        this.UpdateStar(data.grade);
        this.UpdateExpAndMoney((int) data.reward.gold, data.exp, (int) data.reward.exp, data.level);
        this.UpdatePlayCards(data.cards, (int) data.reward.card_exp);
        this.UpdateDropItem(data.reward);
        UILabel component = base.transform.FindChild("Win/TeamLv").GetComponent<UILabel>();
        component.text = "LV: " + ActorData.getInstance().UserInfo.level.ToString();
        component.gameObject.SetActive(true);
        if (GameDefine.getInstance().IsCanShowQQVIP())
        {
            if (ActorData.getInstance().UserInfo.qq_vip_type > 0)
            {
                base.transform.FindChild("Win/QQReward").gameObject.SetActive(true);
            }
            else
            {
                base.transform.FindChild("QQTips").gameObject.SetActive(true);
            }
        }
        else
        {
            base.transform.FindChild("QQTips").gameObject.SetActive(false);
            base.transform.FindChild("Win/QQReward").gameObject.SetActive(false);
        }
        if (ActorData.getInstance().CheckShopTipsStatBool)
        {
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = entity => ((OpenShopTipsPanel) entity).UpdateData();
            }
            GUIMgr.Instance.DoModelGUI("OpenShopTipsPanel", <>f__am$cache7, base.gameObject);
        }
    }

    private void UpdateWinOrFaildPanel(bool _isWin)
    {
        if (_isWin)
        {
            base.gameObject.transform.FindChild("Win").gameObject.SetActive(true);
            base.gameObject.transform.FindChild("Failed").gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.transform.FindChild("Win").gameObject.SetActive(false);
            base.gameObject.transform.FindChild("Failed").gameObject.SetActive(true);
            ActorData.getInstance().mFriendReward = null;
        }
    }

    public void UpdateWorldBossKillAleady()
    {
        this.mBattleReturnType = BattleType.WorldBoss;
        this.UpdateWinOrFaildPanel(true);
        base.gameObject.transform.FindChild("Win/KillTips").gameObject.SetActive(true);
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        this.UpdateExpAndMoney(0, -1, 0, -1);
    }

    public void UpdateWorldBossResult(S2C_WorldBossCombatEndReq _data)
    {
        this.mBattleReturnType = BattleType.WorldBoss;
        if (_data.world_data.boss.curLife <= 0L)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            base.transform.FindChild("Win/Hero").gameObject.SetActive(true);
            base.transform.FindChild("Win/Money").gameObject.SetActive(true);
            base.transform.FindChild("Win/GetItem").gameObject.SetActive(false);
            this.UpdatePlayCards(ActorData.getInstance().mDefaultWorldBossList, 0);
            this.UpdateWinOrFaildPanel(true);
        }
        else
        {
            this.UpdateWinOrFaildPanel(false);
            Transform transform = base.transform.FindChild("Failed/WorldBossInfo");
            transform.FindChild("Money/Val").GetComponent<UILabel>().text = _data.gold.ToString();
            transform.FindChild("Exp/Val").GetComponent<UILabel>().text = "0";
            transform.gameObject.SetActive(true);
        }
        base.transform.FindChild("WorldBossDam").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x9d2ab5), _data.world_data.dam);
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        this.UpdateExpAndMoney(_data.gold, -1, 0, -1);
    }

    public void UpdateWorldCupPkResult(S2C_LeagueFight data)
    {
        this.mBattleReturnType = BattleType.WorldCupPk;
        if (data != null)
        {
            base.transform.FindChild("Win").gameObject.SetActive(data.isWin);
            base.transform.FindChild("Failed").gameObject.SetActive(!data.isWin);
            if (data.isWin)
            {
                base.transform.FindChild("Win/Exp").gameObject.SetActive(false);
                base.transform.FindChild("Win/Star").gameObject.SetActive(false);
                base.transform.FindChild("Win/Money").gameObject.SetActive(false);
                base.transform.FindChild("Win/GetItem").gameObject.SetActive(false);
                base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
                this.UpdatePlayCards(ActorData.getInstance().mDefaultWorldCupPkList, 0);
            }
            base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        }
    }

    public void UpdateYuanZhengResult(S2C_FlameBattleEnd _data)
    {
        this.mBattleReturnType = BattleType.YuanZhengPk;
        base.transform.FindChild("BtnReplay").gameObject.SetActive(false);
        if (_data.battle_result)
        {
            base.transform.FindChild("Win/WinSprite").gameObject.SetActive(true);
            base.transform.FindChild("Win/Star").gameObject.SetActive(false);
            this.UpdateHeroGroup(ActorData.getInstance().mDefaultYuanZhengPkList);
        }
        this.UpdateWinOrFaildPanel(_data.battle_result);
        this.UpdateExpAndMoney(0, -1, 0, 0);
    }

    [CompilerGenerated]
    private sealed class <CheckArenaLadderHistroyTop>c__AnonStorey1D6
    {
        internal int bestOrder;
        internal int curOrder;
        internal PushNotifyPanel.ShareType type;

        internal void <>m__2E1(GUIEntity entity)
        {
            (entity as PushNotifyPanel).UpdateData(this.type, this.curOrder, this.bestOrder - this.curOrder);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckIsFirstPass>c__AnonStorey1D1
    {
        internal int lastDupEntry;

        internal bool <>m__2DD(FastBuf.TrenchData e)
        {
            return (e.entry == this.lastDupEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckPushNotifyPanel>c__AnonStorey1D2
    {
        internal S2C_DuplicateEndReq data;
    }

    [CompilerGenerated]
    private sealed class <CheckPushNotifyPanel>c__AnonStorey1D3
    {
        internal ResultPanel.<CheckPushNotifyPanel>c__AnonStorey1D2 <>f__ref$466;
        internal duplicate_config dc;

        internal void <>m__2DE(GUIEntity entity)
        {
            PushNotifyPanel panel = entity as PushNotifyPanel;
            bool flag = this.<>f__ref$466.data.dupData.dupType == DuplicateType.DupType_Normal;
            string str = !flag ? ConfigMgr.getInstance().GetWord(0x4f6) : ConfigMgr.getInstance().GetWord(0x4f5);
            panel.UpdateData(!flag ? PushNotifyPanel.ShareType.JingYingFuben : PushNotifyPanel.ShareType.Fuben, str, this.dc.name);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckPushNotiTXPanel>c__AnonStorey1D4
    {
        internal ResultPanel.<CheckPushNotiTXPanel>c__AnonStorey1D5 <>f__ref$469;
        internal int elite;
        internal int normal;

        internal void <>m__2DF(GUIEntity ui)
        {
            (ui as PushNotiTXPanel).Show(this.normal, this.elite, this.<>f__ref$469.newRank);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckPushNotiTXPanel>c__AnonStorey1D5
    {
        internal int newRank;
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1D7
    {
        internal ResultPanel.<OnClickItemBtn>c__AnonStorey1D8 <>f__ref$472;
        internal ResultPanel.DropData data;
        internal Item item;

        internal void <>m__2E2(GUIEntity entity)
        {
            (entity as ItemInfoPanel).ShowItemInfo(this.item, this.data.mCardList, this.<>f__ref$472.go.transform);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1D8
    {
        internal GameObject go;
    }

    [CompilerGenerated]
    private sealed class <StartBtnOK>c__Iterator74 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ResultPanel <>f__this;
        internal Transform <BtnOk>__0;

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
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<BtnOk>__0 = this.<>f__this.transform.FindChild("BtnOk");
                    if (this.<BtnOk>__0 != null)
                    {
                        this.<BtnOk>__0.gameObject.SetActive(true);
                    }
                    this.$PC = -1;
                    break;
            }
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

    [CompilerGenerated]
    private sealed class <startLabelAnim>c__Iterator76 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int _lev;
        internal int _srcLev;
        internal int <$>_lev;
        internal int <$>_srcLev;
        internal UILabel <$>label;
        internal UILabel label;

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
                case 4:
                    this.label.text = this._srcLev.ToString();
                    TweenColor.Begin(this.label.gameObject, 0.3f, new Color(1f, 1f, 1f));
                    TweenAlpha.Begin(this.label.gameObject, 0.3f, 1f);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_0197;

                case 1:
                    TweenAlpha.Begin(this.label.gameObject, 0.2f, 0f);
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 2;
                    goto Label_0197;

                case 2:
                    this.label.text = this._lev.ToString();
                    TweenColor.Begin(this.label.gameObject, 0.3f, new Color(0f, 1f, 0f));
                    TweenAlpha.Begin(this.label.gameObject, 0.3f, 1f);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 3;
                    goto Label_0197;

                case 3:
                    TweenAlpha.Begin(this.label.gameObject, 0.2f, 0f);
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 4;
                    goto Label_0197;

                default:
                    break;
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0197:
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

    [CompilerGenerated]
    private sealed class <StartPlaySliderAnim>c__Iterator75 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int _lev;
        internal UISlider _slider;
        internal int _srcLev;
        internal float _toVal;
        internal int <$>_lev;
        internal UISlider <$>_slider;
        internal int <$>_srcLev;
        internal float <$>_toVal;
        internal UILabel <$>levLabel;
        internal ResultPanel <>f__this;
        internal UILabel levLabel;

        internal void <>m__2E4()
        {
            this.<>f__this.StartCoroutine(this.<>f__this.startLabelAnim(this.levLabel, this._lev, this._srcLev));
            this._slider.value = 0f;
            Holoville.HOTween.HOTween.To(this._slider, 0.25f, "value", this._toVal);
        }

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
                    this.$current = new WaitForSeconds(0.25f);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this._lev <= this._srcLev)
                    {
                        Holoville.HOTween.HOTween.To(this._slider, 0.25f, "value", this._toVal);
                        break;
                    }
                    Holoville.HOTween.HOTween.To(this._slider, 0.25f, "value", 1f);
                    this.<>f__this.DelayCallBack(0.25f, new System.Action(this.<>m__2E4));
                    break;

                default:
                    goto Label_00B7;
            }
            this.$PC = -1;
        Label_00B7:
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

    private class DropData
    {
        public object data;
        public List<Card> mCardList;
        public RewardType type;
    }
}

