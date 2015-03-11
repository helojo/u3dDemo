using FastBuf;
using Holoville.HOTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class HeroListPanel : GUIEntity
{
    public UITableManager<CardTableItem> CardTable = new UITableManager<CardTableItem>();
    private float interval = 1f;
    private bool mCalledUsedEvent;
    private GameObject mClickObj;
    private bool mIsDrag;
    private bool mIsPress;
    private bool mIsUesOk;
    private bool mPlayAddExpAmin;
    private int mUseItemEntry = -1;
    private string mUseItemName = string.Empty;
    private Dictionary<GameObject, List<MyHOTweenPara>> myHOTweenParaDict = new Dictionary<GameObject, List<MyHOTweenPara>>();
    private Dictionary<GameObject, bool> myPressUseDict = new Dictionary<GameObject, bool>();
    private float times;

    private void ClearEffct()
    {
        IEnumerator<CardTableItem> enumerator = this.CardTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                CardTableItem current = enumerator.Current;
                if (current.FlashBorder.gameObject.activeSelf)
                {
                    current.FlashBorder.gameObject.SetActive(false);
                }
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    [DebuggerHidden]
    public IEnumerator CreateHeroList()
    {
        return new <CreateHeroList>c__Iterator61 { <>f__this = this };
    }

    private void ExitPanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private int GetCanUseExpItemCount(Card cardInfo, int itemCount, int addExpValue)
    {
        int level = ActorData.getInstance().Level;
        int id = cardInfo.cardInfo.level;
        int curExp = (int) cardInfo.cardInfo.curExp;
        int num4 = 0;
        while (id <= level)
        {
            if (num4 >= itemCount)
            {
                return num4;
            }
            card_lv_up_config _config = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(id);
            if (_config == null)
            {
                return num4;
            }
            num4++;
            for (curExp += addExpValue; curExp >= _config.stage1_exp; curExp -= _config.stage1_exp)
            {
                id++;
            }
        }
        return num4;
    }

    private void GetUseExpItemEndInfo(int maxLv, int cardLv, int cardExp, int addExpValue, out int _endLv, out int _endExp)
    {
        int id = cardLv;
        int num2 = cardExp + addExpValue;
        while (true)
        {
            card_lv_up_config _config = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(id);
            if ((_config == null) || (num2 < _config.stage1_exp))
            {
                break;
            }
            if (id == maxLv)
            {
                num2 = _config.stage1_exp;
                break;
            }
            id++;
            num2 -= _config.stage1_exp;
        }
        _endLv = id;
        _endExp = num2;
    }

    public void InitHeroList(int itemEntry)
    {
        base.Depth = 500;
        this.mUseItemEntry = itemEntry;
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemEntry);
        if (_config != null)
        {
            this.mUseItemName = "[00ff00]" + _config.name + "[ffffff]";
        }
        base.StartCoroutine(this.CreateHeroList());
    }

    private void On_SwipeStart(Gesture gesture)
    {
    }

    private void OnClickCardBtn(GameObject go)
    {
        if (!this.mIsUesOk)
        {
            Debug.Log("--->3");
            if (this.mUseItemEntry != -1)
            {
                Item itemByEntry = ActorData.getInstance().GetItemByEntry(this.mUseItemEntry);
                if (itemByEntry == null)
                {
                    TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x14d), this.mUseItemName));
                }
                else if (ConfigMgr.getInstance().getByEntry<item_config>(itemByEntry.entry) != null)
                {
                    object obj2 = GUIDataHolder.getData(go);
                    if (obj2 != null)
                    {
                        Card card = obj2 as Card;
                        if (card != null)
                        {
                            card_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(card.cardInfo.level);
                            if (_config2 != null)
                            {
                                if ((card.cardInfo.level > ActorData.getInstance().Level) || ((card.cardInfo.level == ActorData.getInstance().Level) && (card.cardInfo.curExp >= _config2.stage1_exp)))
                                {
                                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x150));
                                }
                                else
                                {
                                    SocketMgr.Instance.RequestUseItem(card.card_id, this.mUseItemEntry, 1, -1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDragCardBtn(GameObject go, Vector2 delta)
    {
        this.mIsDrag = true;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        this.CardTable.InitFromGrid(component);
    }

    private void OnPressCardBtn(GameObject go, bool isPress)
    {
        this.mIsPress = isPress;
        if (isPress)
        {
            this.mClickObj = go;
            this.mIsDrag = false;
            this.mIsUesOk = false;
            this.myPressUseDict[go] = false;
            this.mCalledUsedEvent = false;
            this.times = 0f;
        }
        else
        {
            this.mClickObj = null;
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                Card card = obj2 as Card;
                if ((card != null) && (!this.mIsDrag && !this.myPressUseDict[go]))
                {
                    Item itemByEntry = ActorData.getInstance().GetItemByEntry(this.mUseItemEntry);
                    if (itemByEntry == null)
                    {
                        TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x14d), this.mUseItemName));
                    }
                    else
                    {
                        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemByEntry.entry);
                        if (_config != null)
                        {
                            card_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(card.cardInfo.level);
                            if (_config2 != null)
                            {
                                int level = ActorData.getInstance().Level;
                                if ((card.cardInfo.level > ActorData.getInstance().Level) || ((card.cardInfo.level == ActorData.getInstance().Level) && (card.cardInfo.curExp >= _config2.stage1_exp)))
                                {
                                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x150));
                                }
                                else
                                {
                                    Sequence mySeq = new Sequence();
                                    Transform transform = go.transform.FindChild("ExpSlider");
                                    UISprite component = transform.FindChild("Background").GetComponent<UISprite>();
                                    UISprite sprite2 = transform.FindChild("Foreground").GetComponent<UISprite>();
                                    UILabel label = go.transform.FindChild("Level").GetComponent<UILabel>();
                                    int addExpValue = _config.param_0;
                                    base.StartCoroutine(this.UpdateExpSliderAmin(go, level, card.cardInfo.level, (int) card.cardInfo.curExp, addExpValue, mySeq));
                                    SocketMgr.Instance.RequestUseItem(card.card_id, this.mUseItemEntry, 1, -1);
                                    base.StartCoroutine(this.PlayEffect(go));
                                    Debug.Log("------use exp item one time over!");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnPressItemBtn(bool isPress, CardTableItem hero)
    {
        GameObject gameObject = hero.Item1.gameObject;
        this.mIsPress = isPress;
        if (isPress)
        {
            this.mClickObj = gameObject;
            GUIDataHolder.setData(gameObject, hero.ItemData);
            this.mIsDrag = false;
            this.mIsUesOk = false;
            this.myPressUseDict[gameObject] = false;
            this.mCalledUsedEvent = false;
            this.times = 0f;
        }
        else
        {
            this.mClickObj = null;
            Card itemData = hero.ItemData;
            if (itemData != null)
            {
                Debug.Log(itemData.cardInfo.level + "---------" + itemData.cardInfo.curExp);
                if (!this.mIsDrag && !this.myPressUseDict[gameObject])
                {
                    Item itemByEntry = ActorData.getInstance().GetItemByEntry(this.mUseItemEntry);
                    if (itemByEntry == null)
                    {
                        TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x14d), this.mUseItemName));
                    }
                    else
                    {
                        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemByEntry.entry);
                        if (_config != null)
                        {
                            card_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(itemData.cardInfo.level);
                            if (_config2 != null)
                            {
                                int level = ActorData.getInstance().Level;
                                if ((itemData.cardInfo.level > ActorData.getInstance().Level) || ((itemData.cardInfo.level == ActorData.getInstance().Level) && (itemData.cardInfo.curExp >= _config2.stage1_exp)))
                                {
                                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x150));
                                }
                                else
                                {
                                    int num3;
                                    int num4;
                                    Sequence mySeq = new Sequence();
                                    Transform transform = gameObject.transform.FindChild("ExpSlider");
                                    UISprite component = transform.FindChild("ExpBackground").GetComponent<UISprite>();
                                    UISprite sprite2 = transform.FindChild("ExpForeground").GetComponent<UISprite>();
                                    UILabel label = gameObject.transform.FindChild("Level").GetComponent<UILabel>();
                                    int addExpValue = _config.param_0;
                                    this.GetUseExpItemEndInfo(level, itemData.cardInfo.level, (int) itemData.cardInfo.curExp, addExpValue, out num3, out num4);
                                    Debug.Log(num3 + "  ---------->>>>>>>: " + num4);
                                    base.StartCoroutine(this.UpdateExpSliderAmin(gameObject, level, itemData.cardInfo.level, (int) itemData.cardInfo.curExp, addExpValue, mySeq));
                                    SocketMgr.Instance.RequestUseItem(itemData.card_id, this.mUseItemEntry, 1, -1);
                                    base.StartCoroutine(this.PlayEffect(gameObject));
                                    Debug.Log("------use exp item one time over!");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if ((this.mIsPress && !this.mIsDrag) && !this.mCalledUsedEvent)
        {
            this.times += Time.deltaTime;
            if (this.times >= this.interval)
            {
                this.times = 0f;
                Debug.Log("ispress------");
                if (this.mClickObj != null)
                {
                    base.StartCoroutine(this.UserExpItem(this.mClickObj));
                    this.mClickObj.transform.FindChild("FlashBorder").gameObject.SetActive(true);
                    this.mCalledUsedEvent = true;
                }
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PlayEffect(GameObject go)
    {
        return new <PlayEffect>c__Iterator62 { go = go, <$>go = go };
    }

    [DebuggerHidden]
    public IEnumerator PlayExpSliderAmin(GameObject obj)
    {
        return new <PlayExpSliderAmin>c__Iterator64 { obj = obj, <$>obj = obj, <>f__this = this };
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        Transform bounds = base.transform.FindChild("List");
        GUIMgr.Instance.ResetListViewpot(top, bottom, bounds, 0f);
    }

    private void SetCardBaseInfo(Transform obj, card_config cc)
    {
        if (cc != null)
        {
            obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(cc.image);
            obj.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[cc.class_type];
        }
    }

    private void SetCardInfo(Transform obj, Card _card)
    {
        if (_card != null)
        {
            card_config cc = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (cc != null)
            {
                this.SetCardBaseInfo(obj, cc);
                obj.FindChild("Name").GetComponent<UILabel>().text = CommonFunc.GetCardNameByQuality(_card.cardInfo.quality, cc.name);
                obj.FindChild("Level").GetComponent<UILabel>().text = "LV." + _card.cardInfo.level.ToString();
                Transform transform = obj.transform.FindChild("ExpSlider");
                UISprite component = transform.FindChild("Background").GetComponent<UISprite>();
                UISprite sprite2 = transform.FindChild("Foreground").GetComponent<UISprite>();
                UILabel label3 = obj.transform.FindChild("ExpSlider/FullExp").GetComponent<UILabel>();
                card_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(_card.cardInfo.level);
                if (_config2 != null)
                {
                    float num = ((float) _card.cardInfo.curExp) / ((float) _config2.stage1_exp);
                    if (num > 1f)
                    {
                        num = 1f;
                    }
                    sprite2.fillAmount = num;
                    if ((ActorData.getInstance().Level == _card.cardInfo.level) && (_card.cardInfo.curExp >= _config2.stage1_exp))
                    {
                        label3.gameObject.SetActive(true);
                    }
                    else
                    {
                        label3.gameObject.SetActive(false);
                    }
                }
                CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                for (int i = 0; i < 5; i++)
                {
                    UISprite sprite4 = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    sprite4.gameObject.SetActive(i < _card.cardInfo.starLv);
                    sprite4.transform.localPosition = new Vector3((float) (i * 0x16), 0f, 0f);
                }
                Transform transform2 = obj.transform.FindChild("Star");
                transform2.gameObject.SetActive(true);
                transform2.localPosition = new Vector3(transform2.localPosition.x - ((_card.cardInfo.starLv - 1) * 11), transform2.localPosition.y, 0f);
                GUIDataHolder.setData(obj.gameObject, _card);
                UIEventListener.Get(obj.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressCardBtn);
                UIEventListener.Get(obj.gameObject).onDrag = new UIEventListener.VectorDelegate(this.OnDragCardBtn);
            }
        }
    }

    private int SortCardType(Card c1, Card c2)
    {
        if (c1.cardInfo.level != c2.cardInfo.level)
        {
            return (c2.cardInfo.level - c1.cardInfo.level);
        }
        if (c1.cardInfo.quality != c2.cardInfo.quality)
        {
            return (c2.cardInfo.quality - c1.cardInfo.quality);
        }
        if (c1.cardInfo.curExp != c2.cardInfo.curExp)
        {
            return (int) (c2.cardInfo.curExp - c1.cardInfo.curExp);
        }
        return 0;
    }

    [DebuggerHidden]
    public IEnumerator UpdateExpSlider(UISprite expSprite, float from, float to)
    {
        return new <UpdateExpSlider>c__Iterator66 { from = from, <$>from = from };
    }

    [DebuggerHidden]
    public IEnumerator UpdateExpSliderAmin(GameObject obj, int cardMaxLv, int currLv, int currExp, int addExpValue, Sequence mySeq)
    {
        return new <UpdateExpSliderAmin>c__Iterator65 { obj = obj, currLv = currLv, currExp = currExp, cardMaxLv = cardMaxLv, addExpValue = addExpValue, mySeq = mySeq, <$>obj = obj, <$>currLv = currLv, <$>currExp = currExp, <$>cardMaxLv = cardMaxLv, <$>addExpValue = addExpValue, <$>mySeq = mySeq, <>f__this = this };
    }

    public void UpdateExpStat(List<Card> changeCard)
    {
        <UpdateExpStat>c__AnonStorey19A storeya = new <UpdateExpStat>c__AnonStorey19A {
            changeCard = changeCard,
            <>f__this = this
        };
        this.mPlayAddExpAmin = false;
        this.myHOTweenParaDict.Clear();
        base.StopAllCoroutines();
        this.ClearEffct();
        this.DelayCallBack(0.2f, new System.Action(storeya.<>m__20E));
        Daily gUIEntity = GUIMgr.Instance.GetGUIEntity<Daily>();
        if ((gUIEntity != null) && gUIEntity.CheckQuestExistsByType(0x16))
        {
            SocketMgr.Instance.RequestRewardFlag();
        }
    }

    [DebuggerHidden]
    public IEnumerator UserExpItem(GameObject obj)
    {
        return new <UserExpItem>c__Iterator63 { obj = obj, <$>obj = obj, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <CreateHeroList>c__Iterator61 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        private static Comparison<Card> <>f__am$cache6;
        internal HeroListPanel <>f__this;
        internal int <i>__1;
        internal List<Card> <showCardList>__0;
        internal HeroListPanel.CardTableItem <tItem>__2;

        private static int <>m__20F(Card c1, Card c2)
        {
            if (c2.cardInfo.level != c1.cardInfo.level)
            {
                return (c2.cardInfo.level - c1.cardInfo.level);
            }
            if (c2.cardInfo.quality != c1.cardInfo.quality)
            {
                return (c2.cardInfo.quality - c1.cardInfo.quality);
            }
            if (c2.cardInfo.curExp != c1.cardInfo.curExp)
            {
                return (int) (c2.cardInfo.curExp - c1.cardInfo.curExp);
            }
            if (c2.cardInfo.starLv != c1.cardInfo.starLv)
            {
                return (c2.cardInfo.starLv - c1.cardInfo.starLv);
            }
            return 0;
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
                    this.<>f__this.ResetClipViewport();
                    this.<showCardList>__0 = ActorData.getInstance().CardList;
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = new Comparison<Card>(HeroListPanel.<CreateHeroList>c__Iterator61.<>m__20F);
                    }
                    this.<showCardList>__0.Sort(<>f__am$cache6);
                    this.<>f__this.CardTable.Count = this.<showCardList>__0.Count;
                    this.<>f__this.myPressUseDict.Clear();
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.<showCardList>__0.Count)
                    {
                        this.<tItem>__2 = this.<>f__this.CardTable[this.<i>__1];
                        this.<tItem>__2.ItemData = this.<showCardList>__0[this.<i>__1];
                        this.<tItem>__2.OnPress = new Action<bool, HeroListPanel.CardTableItem>(this.<>f__this.OnPressItemBtn);
                        UIEventListener.Get(this.<tItem>__2.Item1.gameObject).onDrag = new UIEventListener.VectorDelegate(this.<>f__this.OnDragCardBtn);
                        if (!this.<>f__this.myPressUseDict.ContainsKey(this.<tItem>__2.Item1.gameObject))
                        {
                            this.<>f__this.myPressUseDict.Add(this.<tItem>__2.Item1.gameObject, false);
                        }
                        if ((this.<showCardList>__0.Count > 6) && (this.<tItem>__2.Drag != null))
                        {
                            this.<tItem>__2.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
                        }
                        if ((this.<i>__1 % 10) == 0)
                        {
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_0206;
                        }
                    Label_01C6:
                        this.<i>__1++;
                    }
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0206;

                case 1:
                    goto Label_01C6;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0206:
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
    private sealed class <PlayEffect>c__Iterator62 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>go;
        internal GameObject <newObj>__0;
        internal GameObject go;

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
                    this.go.transform.FindChild("FlashBorder").gameObject.SetActive(true);
                    this.<newObj>__0 = ObjectManager.CreateObj("EffectPrefabs/jingyanyaoshui");
                    ObjectManager.CreateTempObj(this.<newObj>__0, Vector3.zero, 2f);
                    this.<newObj>__0.transform.parent = this.go.transform;
                    this.<newObj>__0.transform.localScale = Vector3.one;
                    this.<newObj>__0.transform.localPosition = new Vector3(-120f, -50f, 0f);
                    this.$current = new WaitForSeconds(0.55f);
                    this.$PC = 1;
                    goto Label_0117;

                case 1:
                    this.go.transform.FindChild("FlashBorder").gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_0117;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0117:
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
    private sealed class <PlayExpSliderAmin>c__Iterator64 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal HeroListPanel <>f__this;
        internal int <addCount>__1;
        internal int <cardExp>__8;
        internal int <cardLv>__7;
        internal float <currPercent>__9;
        internal float <delayTime>__0;
        internal UISprite <expBg>__4;
        internal UISprite <expForeground>__3;
        internal UILabel <FullExp>__6;
        internal int <fullExpWidth>__11;
        internal bool <isLoop>__10;
        internal UILabel <Level>__5;
        internal HeroListPanel.MyHOTweenPara <mp>__2;
        internal GameObject <newObj>__12;
        internal card_lv_up_config <uc>__13;
        internal GameObject obj;

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
                    this.<delayTime>__0 = 0.1f;
                    this.<addCount>__1 = 0;
                    goto Label_04FA;

                case 1:
                    goto Label_04FA;

                case 2:
                    this.<Level>__5.text = "LV." + this.<cardLv>__7.ToString();
                    this.<isLoop>__10 = false;
                    Debug.Log("满级别" + this.<fullExpWidth>__11);
                    this.<FullExp>__6.gameObject.SetActive(true);
                    this.<>f__this.myHOTweenParaDict[this.obj].Clear();
                    goto Label_0545;

                case 3:
                    Holoville.HOTween.HOTween.To(this.<expForeground>__3, 0f, "fillAmount", 0);
                    this.<Level>__5.text = "LV." + this.<cardLv>__7.ToString();
                    this.<cardExp>__8 -= this.<uc>__13.stage1_exp;
                    Debug.Log(this.<cardLv>__7 + "------------------");
                    this.<addCount>__1++;
                    break;

                case 4:
                    this.<addCount>__1++;
                    if (this.<>f__this.myHOTweenParaDict[this.obj].Count > 0)
                    {
                        this.<>f__this.myHOTweenParaDict[this.obj].RemoveAt(0);
                    }
                    goto Label_04FA;

                case 5:
                    this.$PC = -1;
                    goto Label_0545;

                default:
                    goto Label_0545;
            }
        Label_044B:
            while (this.<isLoop>__10)
            {
                this.<isLoop>__10 = this.<>f__this.mPlayAddExpAmin;
                this.<uc>__13 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(this.<cardLv>__7);
                if (this.<uc>__13 == null)
                {
                    goto Label_0545;
                }
                if (this.<cardExp>__8 >= this.<uc>__13.stage1_exp)
                {
                    if (this.<cardLv>__7 == this.<mp>__2.cardMaxLv)
                    {
                        Holoville.HOTween.HOTween.To(this.<expForeground>__3, this.<delayTime>__0, "fillAmount", 1);
                        this.$current = new WaitForSeconds(this.<delayTime>__0);
                        this.$PC = 2;
                    }
                    else
                    {
                        this.<cardLv>__7++;
                        Holoville.HOTween.HOTween.To(this.<expForeground>__3, this.<delayTime>__0, "fillAmount", 1);
                        this.$current = new WaitForSeconds(this.<delayTime>__0);
                        this.$PC = 3;
                    }
                    goto Label_0547;
                }
                this.<currPercent>__9 = ((float) this.<cardExp>__8) / ((float) this.<uc>__13.stage1_exp);
                this.<isLoop>__10 = false;
            }
            Debug.Log(this.<currPercent>__9 + "     <--->   currPercent");
            Holoville.HOTween.HOTween.To(this.<expForeground>__3, this.<delayTime>__0, "fillAmount", this.<currPercent>__9);
            this.$current = new WaitForSeconds(this.<delayTime>__0);
            this.$PC = 4;
            goto Label_0547;
        Label_04FA:
            if (this.<>f__this.mPlayAddExpAmin || (this.<>f__this.myHOTweenParaDict[this.obj].Count > 0))
            {
                this.<delayTime>__0 = 0.1f - (0.01f * this.<addCount>__1);
                if (this.<delayTime>__0 < 0f)
                {
                    this.<delayTime>__0 = 0.01f;
                }
                if (this.<>f__this.myHOTweenParaDict[this.obj].Count < 1)
                {
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 1;
                    goto Label_0547;
                }
                this.<mp>__2 = this.<>f__this.myHOTweenParaDict[this.obj][0];
                this.<expForeground>__3 = this.<mp>__2.obj.transform.FindChild("ExpSlider/ExpForeground").GetComponent<UISprite>();
                this.<expBg>__4 = this.<mp>__2.obj.transform.FindChild("ExpSlider/ExpBackground").GetComponent<UISprite>();
                this.<Level>__5 = this.<mp>__2.obj.transform.FindChild("Level").GetComponent<UILabel>();
                this.<FullExp>__6 = this.<mp>__2.obj.transform.FindChild("ExpSlider/FullExp").GetComponent<UILabel>();
                this.<cardLv>__7 = this.<mp>__2.currLv;
                this.<cardExp>__8 = this.<mp>__2.currExp;
                this.<currPercent>__9 = 0f;
                this.<isLoop>__10 = true;
                this.<fullExpWidth>__11 = this.<expBg>__4.width - 4;
                this.<cardExp>__8 += this.<mp>__2.addExpValue;
                this.<newObj>__12 = ObjectManager.CreateObj("EffectPrefabs/jingyanyaoshui");
                ObjectManager.CreateTempObj(this.<newObj>__12, Vector3.zero, 2f);
                this.<newObj>__12.transform.parent = this.obj.transform;
                this.<newObj>__12.transform.localScale = Vector3.one;
                this.<newObj>__12.transform.localPosition = new Vector3(-120f, -50f, 0f);
                goto Label_044B;
            }
            this.$current = null;
            this.$PC = 5;
            goto Label_0547;
        Label_0545:
            return false;
        Label_0547:
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
    private sealed class <UpdateExpSlider>c__Iterator66 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>from;
        internal float from;

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
                    Debug.Log(this.from);
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_006D;

                case 1:
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_006D;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_006D:
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
    private sealed class <UpdateExpSliderAmin>c__Iterator65 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>addExpValue;
        internal int <$>cardMaxLv;
        internal int <$>currExp;
        internal int <$>currLv;
        internal Sequence <$>mySeq;
        internal GameObject <$>obj;
        internal HeroListPanel <>f__this;
        internal int <cardExp>__5;
        internal int <cardLv>__4;
        internal float <currPercent>__6;
        internal float <delayTime>__8;
        internal UISprite <expBg>__1;
        internal UISprite <expForeground>__0;
        internal UILabel <FullExp>__3;
        internal int <fullExpWidth>__9;
        internal bool <isLoop>__7;
        internal UILabel <Level>__2;
        internal card_lv_up_config <uc>__10;
        internal int addExpValue;
        internal int cardMaxLv;
        internal int currExp;
        internal int currLv;
        internal Sequence mySeq;
        internal GameObject obj;

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
                    this.<>f__this.mPlayAddExpAmin = true;
                    this.<expForeground>__0 = this.obj.transform.FindChild("ExpSlider/ExpForeground").GetComponent<UISprite>();
                    this.<expBg>__1 = this.obj.transform.FindChild("ExpSlider/ExpBackground").GetComponent<UISprite>();
                    this.<Level>__2 = this.obj.transform.FindChild("Level").GetComponent<UILabel>();
                    this.<FullExp>__3 = this.obj.transform.FindChild("ExpSlider/FullExp").GetComponent<UILabel>();
                    this.obj.transform.FindChild("FlashBorder").gameObject.SetActive(true);
                    this.<cardLv>__4 = this.currLv;
                    this.<cardExp>__5 = this.currExp;
                    this.<currPercent>__6 = 0f;
                    this.<isLoop>__7 = true;
                    this.<delayTime>__8 = 0.1f;
                    this.<fullExpWidth>__9 = this.<expBg>__1.width - 4;
                    Debug.Log(string.Concat(new object[] { "MAX:", this.cardMaxLv, "  currLv:", this.currLv, "  cardExp:", this.<cardExp>__5 }));
                    this.<cardExp>__5 += this.addExpValue;
                    break;

                case 1:
                    this.<Level>__2.text = "LV." + this.<cardLv>__4.ToString();
                    this.mySeq.Play();
                    this.<isLoop>__7 = false;
                    Debug.Log("满级别" + this.<fullExpWidth>__9);
                    this.<FullExp>__3.gameObject.SetActive(true);
                    this.<>f__this.mPlayAddExpAmin = false;
                    if (this.<>f__this.myHOTweenParaDict.ContainsKey(this.obj))
                    {
                        this.<>f__this.myHOTweenParaDict[this.obj].Clear();
                    }
                    this.obj.transform.FindChild("FlashBorder").gameObject.SetActive(false);
                    goto Label_0461;

                case 2:
                    this.<expForeground>__0.fillAmount = 0f;
                    this.<Level>__2.text = "LV." + this.<cardLv>__4.ToString();
                    this.<cardExp>__5 -= this.<uc>__10.stage1_exp;
                    Debug.Log(this.<cardLv>__4 + "------------------");
                    break;

                case 3:
                    this.<>f__this.mPlayAddExpAmin = false;
                    this.obj.transform.FindChild("FlashBorder").gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0463;

                case 4:
                    this.$PC = -1;
                    goto Label_0461;

                default:
                    goto Label_0461;
            }
            while (this.<isLoop>__7)
            {
                this.<isLoop>__7 = this.<>f__this.mPlayAddExpAmin;
                this.<uc>__10 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(this.<cardLv>__4);
                if (this.<uc>__10 == null)
                {
                    goto Label_0461;
                }
                if (this.<cardExp>__5 >= this.<uc>__10.stage1_exp)
                {
                    if (this.<cardLv>__4 == this.cardMaxLv)
                    {
                        Holoville.HOTween.HOTween.To(this.<expForeground>__0, this.<delayTime>__8, "fillAmount", 1);
                        this.$current = new WaitForSeconds(this.<delayTime>__8);
                        this.$PC = 1;
                    }
                    else
                    {
                        this.<cardLv>__4++;
                        Holoville.HOTween.HOTween.To(this.<expForeground>__0, this.<delayTime>__8, "fillAmount", 1);
                        this.$current = new WaitForSeconds(this.<delayTime>__8);
                        this.$PC = 2;
                    }
                    goto Label_0463;
                }
                this.<currPercent>__6 = ((float) this.<cardExp>__5) / ((float) this.<uc>__10.stage1_exp);
                this.<isLoop>__7 = false;
            }
            Debug.Log(this.<currPercent>__6 + " ******* currPercent");
            Holoville.HOTween.HOTween.To(this.<expForeground>__0, this.<delayTime>__8, "fillAmount", this.<currPercent>__6);
            this.$current = new WaitForSeconds(this.<delayTime>__8);
            this.$PC = 3;
            goto Label_0463;
        Label_0461:
            return false;
        Label_0463:
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
    private sealed class <UpdateExpStat>c__AnonStorey19A
    {
        internal HeroListPanel <>f__this;
        internal List<Card> changeCard;

        internal void <>m__20E()
        {
            foreach (Card card in this.changeCard)
            {
                IEnumerator<HeroListPanel.CardTableItem> enumerator = this.<>f__this.CardTable.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        HeroListPanel.CardTableItem current = enumerator.Current;
                        if (current.ItemData.card_id == card.card_id)
                        {
                            current.ItemData = card;
                            Debug.Log(card.cardInfo.level + "---->::;" + card.cardInfo.curExp);
                            continue;
                        }
                    }
                }
                finally
                {
                    if (enumerator == null)
                    {
                    }
                    enumerator.Dispose();
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <UserExpItem>c__Iterator63 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GameObject <$>obj;
        internal HeroListPanel <>f__this;
        internal int <addExpValue>__7;
        internal int <canUseItemCount>__17;
        internal int <cardExp>__15;
        internal int <cardLv>__16;
        internal int <cardMaxLv>__6;
        internal card_config <cc>__2;
        internal card_lv_up_config <cluc>__5;
        internal card_lv_up_config <cu>__23;
        internal int <currExp>__20;
        internal int <currLv>__19;
        internal int <currUseItemCount>__18;
        internal object <data>__0;
        internal float <delayTime>__14;
        internal int <endExp>__26;
        internal int <endLv>__25;
        internal Transform <Exp>__11;
        internal UISprite <expBg>__12;
        internal UISprite <expForeground>__13;
        internal item_config <ic>__4;
        internal Card <info>__1;
        internal Item <item>__3;
        internal UILabel <Level>__10;
        internal List<HeroListPanel.MyHOTweenPara> <myHotWeenParaList>__21;
        internal HeroListPanel.MyHOTweenPara <para>__24;
        internal UILabel <useCount>__9;
        internal Transform <useItem>__8;
        internal float <waitTime>__22;
        internal GameObject obj;

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
                    this.<>f__this.mIsUesOk = true;
                    this.<>f__this.myPressUseDict[this.obj] = true;
                    this.<data>__0 = GUIDataHolder.getData(this.obj);
                    if (this.<data>__0 != null)
                    {
                        this.<info>__1 = this.<data>__0 as Card;
                        if (this.<info>__1 == null)
                        {
                            break;
                        }
                        this.<cc>__2 = ConfigMgr.getInstance().getByEntry<card_config>((int) this.<info>__1.cardInfo.entry);
                        Debug.Log("---------调用长按吃经验---------" + this.<cc>__2.name);
                        if (this.<>f__this.mUseItemEntry == -1)
                        {
                            break;
                        }
                        this.<item>__3 = ActorData.getInstance().GetItemByEntry(this.<>f__this.mUseItemEntry);
                        if (this.<item>__3 == null)
                        {
                            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x14d), this.<>f__this.mUseItemName));
                            break;
                        }
                        this.<ic>__4 = ConfigMgr.getInstance().getByEntry<item_config>(this.<item>__3.entry);
                        if (this.<ic>__4 == null)
                        {
                            break;
                        }
                        this.<cluc>__5 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(this.<info>__1.cardInfo.level);
                        if (this.<cluc>__5 == null)
                        {
                            break;
                        }
                        this.<cardMaxLv>__6 = ActorData.getInstance().Level;
                        if ((this.<info>__1.cardInfo.level > ActorData.getInstance().Level) || ((this.<info>__1.cardInfo.level == ActorData.getInstance().Level) && (this.<info>__1.cardInfo.curExp >= this.<cluc>__5.stage1_exp)))
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x150));
                            break;
                        }
                        this.<addExpValue>__7 = this.<ic>__4.param_0;
                        this.<useItem>__8 = this.obj.transform.FindChild("UseExpItem");
                        this.<useCount>__9 = this.<useItem>__8.FindChild("Count").GetComponent<UILabel>();
                        this.<Level>__10 = this.obj.transform.FindChild("Level").GetComponent<UILabel>();
                        this.<Exp>__11 = this.obj.transform.FindChild("ExpSlider");
                        this.<expBg>__12 = this.<Exp>__11.FindChild("ExpBackground").GetComponent<UISprite>();
                        this.<expForeground>__13 = this.<Exp>__11.FindChild("ExpForeground").GetComponent<UISprite>();
                        this.<delayTime>__14 = 0.3f;
                        this.<cardExp>__15 = (int) this.<info>__1.cardInfo.curExp;
                        this.<cardLv>__16 = this.<info>__1.cardInfo.level;
                        this.<useItem>__8.gameObject.SetActive(true);
                        this.<canUseItemCount>__17 = this.<>f__this.GetCanUseExpItemCount(this.<info>__1, this.<item>__3.num, this.<addExpValue>__7);
                        if (this.<canUseItemCount>__17 == 0)
                        {
                            break;
                        }
                        Debug.Log("本次能使用经验数量：" + this.<canUseItemCount>__17);
                        this.<currUseItemCount>__18 = 0;
                        this.<currLv>__19 = this.<info>__1.cardInfo.level;
                        this.<currExp>__20 = (int) this.<info>__1.cardInfo.curExp;
                        this.<>f__this.mPlayAddExpAmin = true;
                        if (!this.<>f__this.myHOTweenParaDict.ContainsKey(this.obj))
                        {
                            this.<myHotWeenParaList>__21 = new List<HeroListPanel.MyHOTweenPara>();
                            this.<>f__this.myHOTweenParaDict.Add(this.obj, this.<myHotWeenParaList>__21);
                        }
                        this.<>f__this.StartCoroutine(this.<>f__this.PlayExpSliderAmin(this.obj));
                        this.<waitTime>__22 = 0.1f;
                        while (this.<>f__this.mIsPress)
                        {
                            if (this.<currUseItemCount>__18 == this.<item>__3.num)
                            {
                                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x14d), this.<>f__this.mUseItemName));
                                this.<>f__this.mIsPress = false;
                                continue;
                            }
                            this.<cu>__23 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(this.<currLv>__19);
                            if (this.<cu>__23 == null)
                            {
                                this.<>f__this.mIsPress = false;
                                continue;
                            }
                            if (!this.<>f__this.mIsPress)
                            {
                                goto Label_0668;
                            }
                            this.<currUseItemCount>__18++;
                            this.<useCount>__9.text = "+" + this.<currUseItemCount>__18;
                            this.<para>__24 = new HeroListPanel.MyHOTweenPara();
                            this.<para>__24.obj = this.obj;
                            this.<para>__24.cardMaxLv = this.<cardMaxLv>__6;
                            this.<para>__24.currExp = this.<currExp>__20;
                            this.<para>__24.currLv = this.<currLv>__19;
                            this.<para>__24.addExpValue = this.<addExpValue>__7;
                            this.<>f__this.myHOTweenParaDict[this.obj].Add(this.<para>__24);
                            this.<>f__this.GetUseExpItemEndInfo(this.<cardMaxLv>__6, this.<currLv>__19, this.<currExp>__20, this.<addExpValue>__7, out this.<endLv>__25, out this.<endExp>__26);
                            Debug.Log(string.Concat(new object[] { this.<currLv>__19, "  ---   ", this.<endLv>__25, "   ", this.<currExp>__20, " : ", this.<endExp>__26, " --- >", this.<currUseItemCount>__18 }));
                            this.<currLv>__19 = this.<endLv>__25;
                            this.<currExp>__20 = this.<endExp>__26;
                            this.<waitTime>__22 = 0.1f - (0.01f * this.<currUseItemCount>__18);
                            if (this.<waitTime>__22 < 0f)
                            {
                                this.<waitTime>__22 = 0.01f;
                            }
                            this.$current = new WaitForSeconds(this.<waitTime>__22);
                            this.$PC = 1;
                            goto Label_074F;
                        Label_0658:
                            SoundManager.mInstance.PlaySFX("sound_expshui");
                        Label_0668:
                            if ((this.<cardMaxLv>__6 == this.<currLv>__19) && (this.<currExp>__20 >= this.<cu>__23.stage1_exp))
                            {
                                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x150));
                                this.<>f__this.mIsPress = false;
                            }
                        }
                        this.<>f__this.mPlayAddExpAmin = false;
                        this.<useItem>__8.gameObject.SetActive(false);
                        this.<useCount>__9.text = string.Empty;
                        SocketMgr.Instance.RequestUseItem(this.<info>__1.card_id, this.<>f__this.mUseItemEntry, this.<currUseItemCount>__18, -1);
                        this.obj.transform.FindChild("FlashBorder").gameObject.SetActive(false);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_074F;
                    }
                    break;

                case 1:
                    goto Label_0658;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_074F:
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

    public class CardTableItem : UITableItem
    {
        private Card _Card;
        public UIDragCamera Drag;
        private UISprite ExpBackground;
        private UISprite ExpForeground;
        private Transform ExpSlider;
        public UISprite FlashBorder;
        private UILabel FullExp;
        private UITexture Icon;
        public Transform Item1;
        private UISprite JobIcon;
        private UILabel Level;
        private UILabel Name;
        public Action<bool, HeroListPanel.CardTableItem> OnPress;
        private UISprite QualityBorder;

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            Transform ui = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Name = base.Root.FindChild<UILabel>("Name");
            this.Level = base.Root.FindChild<UILabel>("Level");
            this.JobIcon = base.Root.FindChild<UISprite>("JobIcon");
            this.ExpSlider = base.Root.FindChild<Transform>("ExpSlider");
            this.ExpBackground = base.Root.FindChild<UISprite>("ExpBackground");
            this.ExpForeground = base.Root.FindChild<UISprite>("ExpForeground");
            this.FullExp = base.Root.FindChild<UILabel>("FullExp");
            this.FlashBorder = base.Root.FindChild<UISprite>("FlashBorder");
            ui.OnUIMousePress(delegate (bool isDown, object o) {
                if (this.OnPress != null)
                {
                    this.OnPress(isDown, this);
                }
            });
            this.Item1 = ui;
            this.Drag = ui.GetComponent<UIDragCamera>();
        }

        public Card ItemData
        {
            get
            {
                return this._Card;
            }
            set
            {
                this._Card = value;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) value.cardInfo.entry);
                if (_config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    this.Name.text = CommonFunc.GetCardNameByQuality(value.cardInfo.quality, _config.name);
                    this.Level.text = "LV." + value.cardInfo.level;
                    CommonFunc.SetQualityBorder(this.QualityBorder, value.cardInfo.quality);
                    this.JobIcon.spriteName = GameConstant.CardJobIcon[_config.class_type];
                    card_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(value.cardInfo.level);
                    if (_config2 != null)
                    {
                        float num = ((float) value.cardInfo.curExp) / ((float) _config2.stage1_exp);
                        if (num > 1f)
                        {
                            num = 1f;
                        }
                        this.ExpForeground.fillAmount = num;
                        if ((ActorData.getInstance().Level == value.cardInfo.level) && (value.cardInfo.curExp >= _config2.stage1_exp))
                        {
                            this.FullExp.gameObject.SetActive(true);
                        }
                        else
                        {
                            this.FullExp.gameObject.SetActive(false);
                        }
                    }
                    Transform transform = this.Item1.FindChild("StarGroup");
                    for (int i = 0; i < 5; i++)
                    {
                        int num4 = i + 1;
                        UISprite component = this.Item1.transform.FindChild("StarGroup/" + num4.ToString()).GetComponent<UISprite>();
                        component.gameObject.SetActive(i < value.cardInfo.starLv);
                        component.transform.localPosition = new Vector3((float) (i * 0x16), 0f, 0f);
                    }
                    transform.gameObject.SetActive(true);
                    transform.localPosition = new Vector3(-126.3f - ((value.cardInfo.starLv - 1) * 11), transform.localPosition.y, 0f);
                }
            }
        }
    }

    public class MyHOTweenPara
    {
        public int addExpValue;
        public int cardMaxLv;
        public int currExp;
        public int currLv;
        public GameObject obj;
    }
}

