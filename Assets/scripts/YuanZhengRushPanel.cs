using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class YuanZhengRushPanel : GUIEntity
{
    public GameObject DupDropItemObj;
    public GameObject RushResultObj;
    public GameObject TipsDlg;

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

    public override void OnInitialize()
    {
    }

    private void OnPressItem(GameObject obj, bool _press)
    {
        if (this.TipsDlg != null)
        {
            DropData data = (DropData) GUIDataHolder.getData(obj);
            string name = string.Empty;
            string describe = string.Empty;
            int quality = 1;
            if (data.type == DropType.CARD)
            {
                card_config _config = (card_config) data.Data;
                if (_config != null)
                {
                    name = _config.name;
                    describe = _config.describe.Replace("[4d3325]", "[ffffff]");
                    quality = _config.quality;
                }
            }
            else if (data.type == DropType.ITEM)
            {
                item_config _config2 = (item_config) data.Data;
                if (_config2 != null)
                {
                    name = _config2.name;
                    describe = _config2.describe;
                    quality = _config2.quality;
                }
            }
            UIDragScrollView component = obj.GetComponent<UIDragScrollView>();
            if (_press)
            {
                this.TipsDlg.transform.parent = obj.transform.parent;
                this.TipsDlg.transform.localPosition = Vector3.zero;
                this.TipsDlg.transform.localScale = Vector3.one;
                this.TipsDlg.transform.FindChild("Name").GetComponent<UILabel>().text = CommonFunc.GetItemNameByQuality(quality) + name;
                UILabel label = this.TipsDlg.transform.FindChild("Desc").GetComponent<UILabel>();
                label.text = describe;
                this.TipsDlg.transform.FindChild("Border").GetComponent<UISprite>().height = label.height + 70;
                if (int.Parse(obj.transform.parent.name) > 4)
                {
                    this.TipsDlg.transform.localPosition = new Vector3(this.TipsDlg.transform.localPosition.x - 100f, this.TipsDlg.transform.localPosition.y, this.TipsDlg.transform.localPosition.z);
                }
                this.TipsDlg.SetActive(true);
            }
            else
            {
                this.TipsDlg.SetActive(false);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowDrag(List<FlameSmashReward> _Data)
    {
        return new <ShowDrag>c__IteratorA6 { _Data = _Data, <$>_Data = _Data, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator ShowLottery(List<LotteryReward> _Data, Item _additionalItem)
    {
        return new <ShowLottery>c__IteratorA7 { _Data = _Data, <$>_Data = _Data, <>f__this = this };
    }

    public void UpdateData(List<FlameSmashReward> rewardList)
    {
        base.transform.FindChild("TipsLabel").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x85c);
        foreach (FlameSmashReward reward in rewardList)
        {
            ActorData.getInstance().UpdateItemList(reward.reward.items);
            foreach (NewCard card in reward.reward.cards)
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
        this.TipsDlg.transform.parent = base.transform;
        CommonFunc.DeleteChildItem(base.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>().transform);
        base.StartCoroutine(this.ShowDrag(rewardList));
    }

    public void UpdateData(List<LotteryReward> rewardList, Item _additionalItem)
    {
        base.transform.FindChild("TipsLabel").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x864);
        this.TipsDlg.transform.parent = base.transform;
        CommonFunc.DeleteChildItem(base.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>().transform);
        base.StartCoroutine(this.ShowLottery(rewardList, _additionalItem));
    }

    private void UpdateDrop(GameObject obj, FlameSmashReward _data)
    {
        BattleReward reward = _data.reward;
        int node = _data.node;
        int index = 1;
        foreach (NewCard card in reward.cards)
        {
            if (index > 8)
            {
                return;
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
                    Data = _config
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
                    Data = _config2
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
        foreach (Item item in reward.items)
        {
            if (index > 8)
            {
                return;
            }
            GameObject obj5 = this.CreateNewItem(obj, index);
            item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
            DropData data3 = new DropData {
                type = DropType.ITEM,
                Data = _config3
            };
            GUIDataHolder.setData(obj5, data3);
            UIEventListener listener3 = UIEventListener.Get(obj5);
            listener3.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener3.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
            UITexture texture3 = obj5.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label5 = obj5.transform.FindChild("Label").GetComponent<UILabel>();
            UISprite sprite3 = obj5.transform.FindChild("frame").GetComponent<UISprite>();
            GameObject obj6 = obj5.transform.FindChild("Patch").gameObject;
            UILabel label6 = obj5.transform.FindChild("Num").GetComponent<UILabel>();
            if ((_config3.type == 3) || (_config3.type == 2))
            {
                obj6.SetActive(true);
            }
            else
            {
                obj6.SetActive(false);
            }
            if ((_config3.type == 3) || (_config3.type == 2))
            {
                obj5.transform.localPosition = new Vector3(-0.9f, 1f, 0f);
            }
            if (_config3.type == 3)
            {
                int num3 = 90;
                texture3.height = num3;
                texture3.width = num3;
            }
            label6.text = item.diff.ToString();
            CommonFunc.SetEquipQualityBorder(sprite3, _config3.quality, false);
            texture3.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
            texture3.width = 0x4a;
            texture3.height = 0x4a;
            index++;
        }
        if (index == 1)
        {
            Transform transform = obj.transform.FindChild("NoTips");
            if (transform != null)
            {
                transform.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateLottery(GameObject obj, List<LotteryReward> itemList)
    {
        int index = 1;
        foreach (LotteryReward reward in itemList)
        {
            if (index > 8)
            {
                break;
            }
            GameObject go = this.CreateNewItem(obj, index);
            UITexture component = go.transform.FindChild("Icon").GetComponent<UITexture>();
            UILabel label = go.transform.FindChild("Label").GetComponent<UILabel>();
            UISprite sprite = go.transform.FindChild("frame").GetComponent<UISprite>();
            UILabel label2 = go.transform.FindChild("Num").GetComponent<UILabel>();
            GameObject gameObject = go.transform.FindChild("Patch").gameObject;
            if ((reward.item != null) && (reward.item.entry != -1))
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(reward.item.entry);
                if (_config != null)
                {
                    CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                    if (_config.type == 3)
                    {
                        component.width = 0x5c;
                        component.height = 0x5c;
                        gameObject.SetActive(true);
                        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.icon);
                    }
                    else
                    {
                        component.width = 0x4a;
                        component.height = 0x4a;
                        gameObject.SetActive(_config.type == 2);
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    }
                }
                else
                {
                    Debug.Log(reward.item.entry + "<<<<<<<<<<<<<<<<<Error Item Entry");
                }
                label2.text = (reward.item.diff >= 0) ? reward.item.diff.ToString() : "1";
                DropData data = new DropData {
                    type = DropType.ITEM,
                    Data = _config
                };
                GUIDataHolder.setData(go, data);
                UIEventListener listener1 = UIEventListener.Get(go);
                listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener1.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
            }
            else if (reward.card != null)
            {
                if (reward.card.newCard.Count > 0)
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) reward.card.newCard[0].cardInfo.entry);
                    if (_config2 != null)
                    {
                        CommonFunc.SetEquipQualityBorder(sprite, _config2.quality, false);
                        label2.text = "1";
                        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                    }
                    else
                    {
                        Debug.Log(((int) reward.card.newCard[0].cardInfo.entry) + "<<<<<<<<<<<<<<<<<<Error Card Entry");
                    }
                    gameObject.SetActive(false);
                    DropData data2 = new DropData {
                        type = DropType.CARD,
                        Data = _config2
                    };
                    GUIDataHolder.setData(go, data2);
                    UIEventListener listener2 = UIEventListener.Get(go);
                    listener2.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener2.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
                }
                else if (reward.card.newItem.Count > 0)
                {
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(reward.card.newItem[0].entry);
                    if (_config3 != null)
                    {
                        CommonFunc.SetEquipQualityBorder(sprite, _config3.quality, false);
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                        if (_config3.type == 3)
                        {
                            component.width = 0x5c;
                            component.height = 0x5c;
                            gameObject.SetActive(true);
                            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config3.icon);
                        }
                        else
                        {
                            component.width = 0x4a;
                            component.height = 0x4a;
                            gameObject.SetActive(_config3.type == 2);
                            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
                        }
                    }
                    label2.text = (reward.card.newItem[0].diff >= 0) ? reward.card.newItem[0].diff.ToString() : "1";
                    DropData data3 = new DropData {
                        type = DropType.ITEM,
                        Data = _config3
                    };
                    GUIDataHolder.setData(go, data3);
                    UIEventListener listener3 = UIEventListener.Get(go);
                    listener3.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener3.onPress, new UIEventListener.BoolDelegate(this.OnPressItem));
                }
            }
            index++;
        }
    }

    [CompilerGenerated]
    private sealed class <ShowDrag>c__IteratorA6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<FlameSmashReward> _Data;
        internal List<FlameSmashReward> <$>_Data;
        internal List<FlameSmashReward>.Enumerator <$s_855>__3;
        internal YuanZhengRushPanel <>f__this;
        internal UIGrid <grid>__0;
        internal Vector3 <gridNewPostion>__5;
        internal int <index>__1;
        internal float <offsetY>__2;
        internal FlameSmashReward <reward>__4;
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
                        this.<$s_855>__3.Dispose();
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
                    this.<grid>__0 = this.<>f__this.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>();
                    this.<index>__1 = 1;
                    this.<offsetY>__2 = this.<grid>__0.cellHeight;
                    this.<$s_855>__3 = this._Data.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_02DC;

                default:
                    goto Label_02DC;
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
                        this.<RRObj>__6.transform.FindChild("Exp").gameObject.SetActive(false);
                        this.<RRObj>__6.transform.FindChild("Money/Val").GetComponent<UILabel>().text = this.<reward>__4.reward.gold.ToString();
                        this.<RRObj>__6.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x9d2aaf), this.<index>__1);
                        this.<index>__1++;
                        this.<>f__this.UpdateDrop(this.<RRObj>__6, this.<reward>__4);
                        this.$current = new WaitForSeconds(0.3f);
                        this.$PC = 2;
                        flag = true;
                        goto Label_02DE;
                }
                while (this.<$s_855>__3.MoveNext())
                {
                    this.<reward>__4 = this.<$s_855>__3.Current;
                    if (this.<index>__1 > 3)
                    {
                        this.<gridNewPostion>__5 = new Vector3(this.<grid>__0.transform.localPosition.x, this.<grid>__0.transform.localPosition.y + this.<grid>__0.cellHeight, this.<grid>__0.transform.localPosition.z);
                        TweenPosition.Begin(this.<grid>__0.gameObject, 0.2f, this.<gridNewPostion>__5);
                    }
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_02DE;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_855>__3.Dispose();
            }
            this.$current = null;
            this.$PC = 3;
            goto Label_02DE;
        Label_02DC:
            return false;
        Label_02DE:
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
    private sealed class <ShowLottery>c__IteratorA7 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<LotteryReward> _Data;
        internal List<LotteryReward> <$>_Data;
        internal List<LotteryReward>.Enumerator <$s_858>__5;
        internal Dictionary<int, List<LotteryReward>>.ValueCollection.Enumerator <$s_859>__9;
        internal YuanZhengRushPanel <>f__this;
        internal UIGrid <grid>__0;
        internal Vector3 <gridNewPostion>__11;
        internal int <index>__1;
        internal List<LotteryReward> <itemList>__7;
        internal List<LotteryReward> <lr>__8;
        internal float <offsetY>__2;
        internal int <page>__3;
        internal LotteryReward <reward>__6;
        internal Dictionary<int, List<LotteryReward>> <rewardDict>__4;
        internal List<LotteryReward> <rewardList>__10;
        internal GameObject <RRObj>__12;

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
                        this.<$s_859>__9.Dispose();
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
                    this.<grid>__0 = this.<>f__this.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>();
                    this.<index>__1 = 0;
                    this.<offsetY>__2 = this.<grid>__0.cellHeight;
                    this.<page>__3 = 0;
                    this.<rewardDict>__4 = new Dictionary<int, List<LotteryReward>>();
                    this.<$s_858>__5 = this._Data.GetEnumerator();
                    try
                    {
                        while (this.<$s_858>__5.MoveNext())
                        {
                            this.<reward>__6 = this.<$s_858>__5.Current;
                            if ((this.<index>__1 % 6) == 0)
                            {
                                this.<page>__3++;
                            }
                            if (!this.<rewardDict>__4.ContainsKey(this.<page>__3))
                            {
                                this.<itemList>__7 = new List<LotteryReward>();
                                this.<rewardDict>__4.Add(this.<page>__3, this.<itemList>__7);
                            }
                            if (this.<rewardDict>__4.ContainsKey(this.<page>__3))
                            {
                                this.<lr>__8 = this.<rewardDict>__4[this.<page>__3];
                                this.<lr>__8.Add(this.<reward>__6);
                            }
                            this.<index>__1++;
                        }
                    }
                    finally
                    {
                        this.<$s_858>__5.Dispose();
                    }
                    this.<index>__1 = 1;
                    this.<$s_859>__9 = this.<rewardDict>__4.Values.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_03D2;

                default:
                    goto Label_03D2;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        this.<RRObj>__12 = UnityEngine.Object.Instantiate(this.<>f__this.RushResultObj) as GameObject;
                        this.<RRObj>__12.transform.parent = this.<grid>__0.transform;
                        this.<RRObj>__12.transform.localScale = Vector3.one;
                        this.<RRObj>__12.transform.localPosition = new Vector3(0f, -this.<offsetY>__2 * (this.<index>__1 - 1), 0f);
                        this.<RRObj>__12.transform.FindChild("Exp").gameObject.SetActive(false);
                        this.<RRObj>__12.transform.FindChild("Money").gameObject.SetActive(false);
                        this.<RRObj>__12.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x50c), this.<index>__1);
                        this.<index>__1++;
                        this.<>f__this.UpdateLottery(this.<RRObj>__12, this.<rewardList>__10);
                        this.$current = new WaitForSeconds(0.3f);
                        this.$PC = 2;
                        flag = true;
                        goto Label_03D4;
                }
                while (this.<$s_859>__9.MoveNext())
                {
                    this.<rewardList>__10 = this.<$s_859>__9.Current;
                    if (this.<index>__1 > 3)
                    {
                        this.<gridNewPostion>__11 = new Vector3(this.<grid>__0.transform.localPosition.x, this.<grid>__0.transform.localPosition.y + this.<grid>__0.cellHeight, this.<grid>__0.transform.localPosition.z);
                        TweenPosition.Begin(this.<grid>__0.gameObject, 0.2f, this.<gridNewPostion>__11);
                    }
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_03D4;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_859>__9.Dispose();
            }
            this.$current = null;
            this.$PC = 3;
            goto Label_03D4;
        Label_03D2:
            return false;
        Label_03D4:
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
        public YuanZhengRushPanel.DropType type;
        public object Data;
    }

    private enum DropType
    {
        CARD,
        ITEM
    }
}

