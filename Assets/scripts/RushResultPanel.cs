using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class RushResultPanel : GUIEntity
{
    private UIDragScrollView CurDragObj;
    public GameObject DupDropItemObj;
    private float interval = 0.1f;
    private bool mCurrFunBtn = true;
    private bool mCurrTitleBar = true;
    private bool press;
    public GameObject RushResultObj;
    private float times;
    public GameObject TipsDlg;

    private void ClosePanel()
    {
        if (ActorData.getInstance().mCurrDupReturnPrePara != null)
        {
            CommonFunc.ShowFuncList(true);
        }
        GUIMgr.Instance.ExitModelGUI("RushResultPanel");
        if (this.mCurrFunBtn)
        {
            CommonFunc.ShowFuncList(true);
        }
        if (this.mCurrTitleBar)
        {
            CommonFunc.ShowTitlebar(true);
        }
    }

    private GameObject CreateNewItem(GameObject obj, int index)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(this.DupDropItemObj) as GameObject;
        Transform transform = obj.transform.FindChild(index.ToString()).transform;
        transform.gameObject.SetActive(true);
        obj2.transform.parent = transform;
        obj2.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        obj2.transform.localPosition = new Vector3(-0.9f, 1.5f, 0f);
        return obj2;
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
    }

    public override void OnInitialize()
    {
        this.mCurrTitleBar = CommonFunc.CheckTitleBarStat();
        this.mCurrFunBtn = CommonFunc.CheckTitleBarFunList();
        if (this.mCurrFunBtn)
        {
            CommonFunc.ShowFuncList(false);
        }
        if (this.mCurrTitleBar)
        {
            CommonFunc.ShowTitlebar(false);
        }
    }

    private void OnPressItem(GameObject obj, bool _press)
    {
        if (this.TipsDlg != null)
        {
            DropData data = (DropData) GUIDataHolder.getData(obj);
            string name = string.Empty;
            string describe = string.Empty;
            int quality = 1;
            int entry = -1;
            if (data.type == DropType.CARD)
            {
                card_config _config = (card_config) data.Data;
                name = _config.name;
                describe = _config.describe;
                quality = _config.quality;
            }
            else if (data.type == DropType.ITEM)
            {
                item_config _config2 = (item_config) data.Data;
                name = _config2.name;
                describe = _config2.describe;
                quality = _config2.quality;
                entry = _config2.entry;
            }
            UIDragScrollView component = obj.GetComponent<UIDragScrollView>();
            if (_press)
            {
                this.TipsDlg.transform.parent = obj.transform.parent;
                UISprite sprite = this.TipsDlg.transform.FindChild("Sprite").GetComponent<UISprite>();
                this.TipsDlg.transform.localPosition = (data.index > 4) ? new Vector3((0f - sprite.localSize.x) - 90f, 0f, 0f) : Vector3.zero;
                this.TipsDlg.transform.localScale = Vector3.one;
                this.TipsDlg.transform.FindChild("Name").GetComponent<UILabel>().text = CommonFunc.GetItemNameByQuality(quality) + name;
                UILabel label = this.TipsDlg.transform.FindChild("Desc").GetComponent<UILabel>();
                label.text = describe;
                UILabel label2 = this.TipsDlg.transform.FindChild("Count").GetComponent<UILabel>();
                Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(entry);
                label2.text = ((itemByEntry != null) ? itemByEntry.num : 0).ToString();
                sprite.height = label.height + 0x55;
                this.press = true;
                this.CurDragObj = component;
                this.TipsDlg.SetActive(true);
            }
            else
            {
                this.press = false;
                component.enabled = true;
                this.TipsDlg.SetActive(false);
            }
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        if (ActorData.getInstance().isInOutland)
        {
            TitleBar gUIEntity = GUIMgr.Instance.GetGUIEntity<TitleBar>();
            if (gUIEntity != null)
            {
                gUIEntity.transform.FindChild("TopRight").gameObject.SetActive(true);
            }
        }
        ActorData.getInstance().isInOutland = false;
        UIGrid component = base.gameObject.transform.FindChild("Scroll View/Grid").GetComponent<UIGrid>();
        base.gameObject.transform.FindChild("Scroll View").GetComponent<ResetListPos>().PanelReset();
        CommonFunc.DeleteChildItem(component.transform);
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.press)
        {
            this.times += Time.deltaTime;
            if (this.times >= this.interval)
            {
                this.times = 0f;
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowDrag(List<BattleReward> _Data)
    {
        return new <ShowDrag>c__Iterator77 { _Data = _Data, <$>_Data = _Data, <>f__this = this };
    }

    public void UpdateData(S2C_DuplicateSmash _Data)
    {
        if (ActorData.getInstance().mCurrDupReturnPrePara != null)
        {
            CommonFunc.ShowFuncList(false);
        }
        this.TipsDlg.transform.parent = base.transform;
        UIGrid component = base.gameObject.transform.FindChild("Scroll View/Grid").GetComponent<UIGrid>();
        if (_Data.level != ActorData.getInstance().Level)
        {
            if (_Data.dupData.dupType == DuplicateType.DupType_Normal)
            {
                ActorData data1 = ActorData.getInstance();
                data1.PhyForce -= _Data.times * ActorData.getInstance().m_nNormalDupCostPhysical;
            }
            else if (_Data.dupData.dupType == DuplicateType.DupType_Elite)
            {
                ActorData data2 = ActorData.getInstance();
                data2.PhyForce -= _Data.times * ActorData.getInstance().m_nEliteDupCostPhysical;
            }
        }
        ActorData.getInstance().UserInfo.exp = _Data.exp;
        ActorData.getInstance().Level = _Data.level;
        ActorData.getInstance().Gold = _Data.gold;
        ActorData.getInstance().PhyForce = _Data.phyForce;
        ActorData.getInstance().UserInfo.passTime = _Data.phyForce_passtime;
        ActorData.getInstance().InitAddPhyForce(_Data.phyForce_passtime);
        foreach (BattleReward reward in _Data.reward)
        {
            ActorData.getInstance().UpdateItemList(reward.items);
            foreach (NewCard card in reward.cards)
            {
                if (card.newCard.Count > 0)
                {
                    ActorData.getInstance().CardList.Add(card.newCard[0]);
                }
                else if (card.newItem.Count > 0)
                {
                    ActorData.getInstance().UpdateItemList(card.newItem);
                }
            }
        }
        if (_Data.more_reward != null)
        {
            ActorData.getInstance().UpdateItemList(_Data.more_reward.items);
        }
        if (_Data.dupData.dupType == DuplicateType.DupType_Elite)
        {
            List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(_Data.dupData.dupEntry, out list);
            foreach (FastBuf.TrenchData data in list)
            {
                if (data.entry == _Data.dupData.trenchEntry)
                {
                    data.remain -= _Data.times;
                    if (data.remain < 0)
                    {
                        data.remain = 0;
                    }
                }
            }
        }
        base.StartCoroutine(this.ShowDrag(_Data.reward));
        GameObject gameObject = base.gameObject.transform.FindChild("GetPart").gameObject;
        this.UpdateDrop(gameObject, _Data.more_reward);
    }

    public void UpdateData(S2C_OutlandSweepReq _Data)
    {
        ActorData.getInstance().PhyForce = _Data.phyForce;
        ActorData.getInstance().Stone = _Data.stone;
        UIGrid component = base.gameObject.transform.FindChild("Scroll View/Grid").GetComponent<UIGrid>();
        foreach (OutlandFloorSweepDropInfo info in _Data.sweepData.floorDrop)
        {
            GameObject rRObj = UnityEngine.Object.Instantiate(this.RushResultObj) as GameObject;
            rRObj.transform.parent = component.transform;
            rRObj.transform.localScale = Vector3.one;
            rRObj.transform.localPosition = Vector3.zero;
            this.UpdateFloor(rRObj, info);
        }
        component.repositionNow = true;
    }

    private void UpdateDrop(GameObject obj, BattleReward _reward)
    {
        int index = this.UpdateDropCards(obj, _reward.cards, 1);
        if (this.UpdateDropItems(obj, _reward.items, index) == 1)
        {
            Transform transform = obj.transform.FindChild("NoTips");
            if (transform != null)
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    private int UpdateDropCards(GameObject obj, List<NewCard> cards, int index)
    {
        foreach (NewCard card in cards)
        {
            if (index > 8)
            {
                return index;
            }
            if (card.newCard.Count > 0)
            {
                GameObject go = this.CreateNewItem(obj, index);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.newCard[0].cardInfo.entry);
                UITexture component = go.transform.FindChild("Icon").GetComponent<UITexture>();
                UILabel label = go.transform.FindChild("Label").GetComponent<UILabel>();
                UISprite sprite = go.transform.FindChild("frame").GetComponent<UISprite>();
                UILabel label2 = go.transform.FindChild("Num").GetComponent<UILabel>();
                DropData data = new DropData {
                    type = DropType.CARD,
                    Data = _config,
                    index = index
                };
                GUIDataHolder.setData(go, data);
                UIEventListener listener1 = UIEventListener.Get(go);
                listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener1.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
                CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                label2.text = "1";
                component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            }
            else if (card.newItem.Count > 0)
            {
                GameObject obj3 = this.CreateNewItem(obj, index);
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(card.newItem[0].entry);
                DropData data2 = new DropData {
                    type = DropType.ITEM,
                    Data = _config2,
                    index = index
                };
                GUIDataHolder.setData(obj3, data2);
                UIEventListener listener2 = UIEventListener.Get(obj3);
                listener2.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener2.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
                UITexture texture2 = obj3.transform.FindChild("Icon").GetComponent<UITexture>();
                UISprite sprite2 = obj3.transform.FindChild("frame").GetComponent<UISprite>();
                GameObject gameObject = obj3.transform.FindChild("Patch").gameObject;
                UILabel label3 = obj3.transform.FindChild("Num").GetComponent<UILabel>();
                CommonFunc.SetEquipQualityBorder(sprite2, _config2.quality, false);
                gameObject.SetActive(true);
                UILabel label4 = obj3.transform.FindChild("Label").GetComponent<UILabel>();
                label3.text = card.newItem[0].diff.ToString();
                texture2.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                texture2.width = 0x4a;
                texture2.height = 0x4a;
            }
            index++;
        }
        return index;
    }

    private int UpdateDropItems(GameObject obj, List<Item> items, int index)
    {
        foreach (Item item in items)
        {
            if (index > 8)
            {
                return index;
            }
            GameObject go = this.CreateNewItem(obj, index);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
            DropData data = new DropData {
                type = DropType.ITEM,
                Data = _config,
                index = index
            };
            GUIDataHolder.setData(go, data);
            UIEventListener listener1 = UIEventListener.Get(go);
            listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener1.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
            UITexture component = go.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label = go.transform.FindChild("Label").GetComponent<UILabel>();
            UISprite sprite = go.transform.FindChild("frame").GetComponent<UISprite>();
            GameObject gameObject = go.transform.FindChild("Patch").gameObject;
            UILabel label2 = go.transform.FindChild("Num").GetComponent<UILabel>();
            if ((_config.type == 3) || (_config.type == 2))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            if ((_config.type == 3) || (_config.type == 2))
            {
                go.transform.localPosition = new Vector3(-0.9f, 1f, 0f);
            }
            if (_config.type == 3)
            {
                int num2 = 90;
                component.height = num2;
                component.width = num2;
            }
            label2.text = item.diff.ToString();
            CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            component.width = 0x4a;
            component.height = 0x4a;
            index++;
        }
        return index;
    }

    private void UpdateFloor(GameObject RRObj, OutlandFloorSweepDropInfo oneFloorDrop)
    {
        outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(oneFloorDrop.entry);
        if (_config != null)
        {
            outland_map_type_config _config2 = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(_config.outland_type);
            RRObj.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), _config2.name, _config.layer);
        }
        RRObj.transform.FindChild("Money").gameObject.SetActive(false);
        RRObj.transform.FindChild("Exp/Val").GetComponent<UILabel>().text = ((oneFloorDrop.add_exp >= 0) ? oneFloorDrop.add_exp : 0).ToString();
        int num = (oneFloorDrop.add_outlandcoin >= 0) ? oneFloorDrop.add_outlandcoin : 0;
        RRObj.transform.FindChild("MoneyOutland").gameObject.SetActive(true);
        RRObj.transform.FindChild("MoneyOutland/Val").GetComponent<UILabel>().text = num.ToString();
        ActorData.getInstance().UpdateItemList(oneFloorDrop.goods);
        if (this.UpdateDropItems(RRObj, oneFloorDrop.goods, 1) == 1)
        {
            Transform transform = RRObj.transform.FindChild("NoTips");
            if (transform != null)
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowDrag>c__Iterator77 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<BattleReward> _Data;
        internal List<BattleReward> <$>_Data;
        internal List<BattleReward>.Enumerator <$s_595>__3;
        internal RushResultPanel <>f__this;
        internal UIGrid <grid>__0;
        internal Vector3 <gridNewPostion>__5;
        internal int <index>__1;
        internal float <offsetY>__2;
        internal BattleReward <reward>__4;
        internal GameObject <RRObj>__6;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                case 2:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_595>__3.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<>f__this.gameObject.transform.FindChild("Scroll View").GetComponent<ResetListPos>().PanelReset();
                    this.<grid>__0 = this.<>f__this.gameObject.transform.FindChild("Scroll View/Grid").GetComponent<UIGrid>();
                    this.<index>__1 = 1;
                    this.<offsetY>__2 = this.<grid>__0.cellHeight;
                    this.<$s_595>__3 = this._Data.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_030A;

                default:
                    goto Label_030A;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        this.<RRObj>__6 = UnityEngine.Object.Instantiate(this.<>f__this.RushResultObj) as GameObject;
                        this.<RRObj>__6.transform.parent = this.<grid>__0.transform;
                        this.<RRObj>__6.transform.localScale = Vector3.one;
                        this.<RRObj>__6.transform.localPosition = new Vector3(0f, -this.<offsetY>__2 * (this.<index>__1 - 1), 0f);
                        this.<RRObj>__6.transform.FindChild("Exp/Val").GetComponent<UILabel>().text = this.<reward>__4.exp.ToString();
                        this.<RRObj>__6.transform.FindChild("Money/Val").GetComponent<UILabel>().text = this.<reward>__4.gold.ToString();
                        this.<RRObj>__6.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x9d2aaf), this.<index>__1);
                        this.<>f__this.UpdateDrop(this.<RRObj>__6, this.<reward>__4);
                        this.$current = new WaitForSeconds(0.3f);
                        this.$PC = 2;
                        flag = true;
                        goto Label_030C;

                    case 2:
                        this.<index>__1++;
                        break;
                }
                while (this.<$s_595>__3.MoveNext())
                {
                    this.<reward>__4 = this.<$s_595>__3.Current;
                    if (this.<index>__1 > 2)
                    {
                        this.<gridNewPostion>__5 = new Vector3(this.<grid>__0.transform.localPosition.x, this.<grid>__0.transform.localPosition.y + this.<grid>__0.cellHeight, this.<grid>__0.transform.localPosition.z);
                        TweenPosition.Begin(this.<grid>__0.gameObject, 0.2f, this.<gridNewPostion>__5);
                    }
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_030C;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_595>__3.Dispose();
            }
            this.$current = null;
            this.$PC = 3;
            goto Label_030C;
        Label_030A:
            return false;
        Label_030C:
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

    [StructLayout(LayoutKind.Sequential)]
    private struct DropData
    {
        public RushResultPanel.DropType type;
        public object Data;
        public int index;
    }

    private enum DropType
    {
        CARD,
        ITEM
    }
}

