using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class GemWearPanel : GUIEntity
{
    private Transform _GemInfoPanel;
    public GameObject _GemTips;
    [CompilerGenerated]
    private static Comparison<Item> <>f__am$cacheC;
    [CompilerGenerated]
    private static Func<Item, BagItemData> <>f__am$cacheD;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheE;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheF;
    public int[] C_Limits = new int[6];
    public UITableManager<BagTableItem> ItemTable = new UITableManager<BagTableItem>();
    public int m_cardgem_maxquality;
    private static int m_nHoleOpenCount;
    public string m_strEnchase = string.Empty;
    public Card mCard;
    private long mCardId = -1L;
    public BagTableItem mCurrSelectItemEntry;
    private List<Card> mCurrShowCardList;
    private List<Transform> mGemList = new List<Transform>();

    private bool CanWear(int entry)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config != null)
        {
            for (int i = 0; i < this.mCard.cardGemInfo.Count; i++)
            {
                if (this.mCard.cardGemInfo[i].entry >= 0)
                {
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(this.mCard.cardGemInfo[i].entry);
                    if ((_config2 != null) && (_config.sub_type == _config2.sub_type))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void CreateItemList(bool resetClip = true)
    {
        if (resetClip)
        {
            this.ResetClipViewport();
        }
        List<Item> itemListByType = ActorData.getInstance().GetItemListByType(ShowItemType.Gem);
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = delegate (Item item1, Item item2) {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item1.entry);
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(item2.entry);
                if (_config.sub_type != _config2.sub_type)
                {
                    return _config.sub_type - _config2.sub_type;
                }
                if (_config.quality != _config2.quality)
                {
                    return _config2.quality - _config.quality;
                }
                return 1;
            };
        }
        itemListByType.Sort(<>f__am$cacheC);
        if (<>f__am$cacheD == null)
        {
            <>f__am$cacheD = t => new BagItemData { Data = t, Config = ConfigMgr.getInstance().getByEntry<item_config>(t.entry) };
        }
        BagItemData[] dataArray = itemListByType.Select<Item, BagItemData>(<>f__am$cacheD).ToArray<BagItemData>();
        this.ItemTable.Count = dataArray.Length;
        for (int i = 0; i < dataArray.Length; i++)
        {
            BagTableItem item = this.ItemTable[i];
            item.ItemData = dataArray[i];
            item.OnClick = new Action<BagTableItem>(this.OnClickItemBtn);
        }
        if (this.ItemTable.Count < 1)
        {
            this._GemInfoPanel.gameObject.SetActive(false);
            base.transform.FindChild("GemInfo/Desc").gameObject.SetActive(true);
        }
        else
        {
            this.SetItemDetails(this.ItemTable[0]);
            base.transform.FindChild("GemInfo/Desc").gameObject.SetActive(false);
        }
    }

    public string GetAddDeltaByID(int entry, int flag = 1)
    {
        string str = (flag != 1) ? " -" : " +";
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        string str2 = string.Empty;
        if (_config != null)
        {
            string str3;
            switch (_config.sub_type)
            {
                case 0:
                {
                    str3 = str2;
                    object[] objArray1 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x156), str, _config.strength };
                    return string.Concat(objArray1);
                }
                case 1:
                {
                    str3 = str2;
                    object[] objArray2 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x158), str, _config.intelligence };
                    return string.Concat(objArray2);
                }
                case 2:
                {
                    str3 = str2;
                    object[] objArray3 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x157), str, _config.agility };
                    return string.Concat(objArray3);
                }
                case 3:
                {
                    str3 = str2;
                    object[] objArray4 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x159), str, _config.stamina };
                    return string.Concat(objArray4);
                }
                case 4:
                {
                    str3 = str2;
                    object[] objArray5 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x155), str, _config.attack };
                    return string.Concat(objArray5);
                }
                case 5:
                {
                    str3 = str2;
                    object[] objArray6 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x15b), str, _config.spell_defence };
                    return string.Concat(objArray6);
                }
                case 6:
                {
                    str3 = str2;
                    object[] objArray7 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x15a), str, _config.physics_defence };
                    return string.Concat(objArray7);
                }
                case 7:
                {
                    str3 = str2;
                    object[] objArray8 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x144), str, _config.crit_level };
                    return string.Concat(objArray8);
                }
                case 8:
                {
                    str3 = str2;
                    object[] objArray9 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x15f), str, _config.critdmg_level };
                    return string.Concat(objArray9);
                }
                case 9:
                {
                    str3 = str2;
                    object[] objArray10 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x146), str, _config.tenacity_level };
                    return string.Concat(objArray10);
                }
                case 10:
                {
                    str3 = str2;
                    object[] objArray11 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x148), str, _config.hp_recover };
                    return string.Concat(objArray11);
                }
                case 11:
                {
                    str3 = str2;
                    object[] objArray12 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x149), str, _config.energy_recover };
                    return string.Concat(objArray12);
                }
                case 12:
                {
                    str3 = str2;
                    object[] objArray13 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x4e61), str, _config.be_heal_mod };
                    return string.Concat(objArray13);
                }
                case 13:
                {
                    str3 = str2;
                    object[] objArray14 = new object[] { str3, ConfigMgr.getInstance().GetWord(0x4e62), str, _config.heal_mod };
                    return string.Concat(objArray14);
                }
            }
        }
        return str2;
    }

    public Card GetCardInfo(Card _currCard, bool isForward)
    {
        int num = 0;
        for (int i = 0; i < this.mCurrShowCardList.Count; i++)
        {
            if (this.mCurrShowCardList[i].card_id == _currCard.card_id)
            {
                num = i;
            }
        }
        int num3 = !isForward ? (num - 1) : (num + 1);
        if (num3 < 0)
        {
            num3 = this.mCurrShowCardList.Count - 1;
        }
        else if (num3 >= this.mCurrShowCardList.Count)
        {
            num3 = 0;
        }
        return this.mCurrShowCardList[num3];
    }

    private int GetEmptyHoleIndex()
    {
        for (int i = 0; i < m_nHoleOpenCount; i++)
        {
            if ((this.mCard.cardGemInfo[i].entry < 0) && (this.mCard.cardInfo.level >= this.C_Limits[i]))
            {
                return i;
            }
        }
        return -1;
    }

    private void GetGemAttrByLevels(out int two_type, out int two_value, out int four_type, out int four_value, out int six_type, out int six_value, int two_level, int four_level, int six_level)
    {
        two_value = 0;
        two_type = -1;
        four_value = 0;
        four_type = -1;
        six_value = 0;
        six_type = -1;
        card_gem_attr_config _config = ConfigMgr.getInstance().getByEntry<card_gem_attr_config>((int) this.mCard.cardInfo.entry);
        if (_config != null)
        {
            int[] numArray1 = new int[,] { { _config.attrtype_2_1, _config.attrtype_2_2, _config.attrtype_2_3, _config.attrtype_2_4, _config.attrtype_2_5, _config.attrtype_2_6 }, { _config.attrtype_4_1, _config.attrtype_4_2, _config.attrtype_4_3, _config.attrtype_4_4, _config.attrtype_4_5, _config.attrtype_4_6 }, { _config.attrtype_6_1, _config.attrtype_6_2, _config.attrtype_6_3, _config.attrtype_6_4, _config.attrtype_6_5, _config.attrtype_6_6 } };
            int[,] numArray = numArray1;
            int[] numArray3 = new int[,] { { _config.attrvalue_2_1, _config.attrvalue_2_2, _config.attrvalue_2_3, _config.attrvalue_2_4, _config.attrvalue_2_5, _config.attrvalue_2_6 }, { _config.attrvalue_4_1, _config.attrvalue_4_2, _config.attrvalue_4_3, _config.attrvalue_4_4, _config.attrvalue_4_5, _config.attrvalue_4_6 }, { _config.attrvalue_6_1, _config.attrvalue_6_2, _config.attrvalue_6_3, _config.attrvalue_6_4, _config.attrvalue_6_5, _config.attrvalue_6_6 } };
            int[,] numArray2 = numArray3;
            if (two_level > this.m_cardgem_maxquality)
            {
                two_level = this.m_cardgem_maxquality;
            }
            if (four_level > this.m_cardgem_maxquality)
            {
                four_level = this.m_cardgem_maxquality;
            }
            if (six_level > this.m_cardgem_maxquality)
            {
                six_level = this.m_cardgem_maxquality;
            }
            two_type = numArray[0, two_level];
            two_value = numArray2[0, two_level];
            four_type = numArray[1, four_level];
            four_value = numArray2[1, four_level];
            six_type = numArray[2, six_level];
            six_value = numArray2[2, six_level];
        }
    }

    private int GetGemNumByLevel(int lv)
    {
        int num = 0;
        for (int i = 0; i < this.mCard.cardGemInfo.Count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mCard.cardGemInfo[i].entry);
            if ((_config != null) && (lv <= _config.quality))
            {
                num++;
            }
        }
        return num;
    }

    private int GetMinAttrLevel(card_gem_attr_config gc)
    {
        int[] numArray = new int[] { gc.attrtype_2_1, gc.attrtype_2_2, gc.attrtype_2_3, gc.attrtype_2_4, gc.attrtype_2_5, gc.attrtype_2_6 };
        for (int i = 0; i < numArray.Length; i++)
        {
            if (numArray[i] != -1)
            {
                return i;
            }
        }
        return -1;
    }

    private string getTypeString(int type)
    {
        string str = string.Empty;
        switch (type)
        {
            case 0:
                return (str + ConfigMgr.getInstance().GetWord(0x156));

            case 1:
                return (str + ConfigMgr.getInstance().GetWord(0x158));

            case 2:
                return (str + ConfigMgr.getInstance().GetWord(0x157));

            case 3:
                return (str + ConfigMgr.getInstance().GetWord(0x159));

            case 4:
                return (str + ConfigMgr.getInstance().GetWord(0x155));

            case 5:
                return (str + ConfigMgr.getInstance().GetWord(0x15b));

            case 6:
                return (str + ConfigMgr.getInstance().GetWord(0x15a));

            case 7:
                return (str + ConfigMgr.getInstance().GetWord(0x144));

            case 8:
                return (str + ConfigMgr.getInstance().GetWord(0x15f));

            case 9:
                return (str + ConfigMgr.getInstance().GetWord(0x146));

            case 10:
                return (str + ConfigMgr.getInstance().GetWord(0x148));

            case 11:
                return (str + ConfigMgr.getInstance().GetWord(0x149));

            case 12:
                return (str + ConfigMgr.getInstance().GetWord(0x4e61));

            case 13:
                return (str + ConfigMgr.getInstance().GetWord(0x4e62));
        }
        return str;
    }

    public void InitCardInfo(Card _card)
    {
        this.mCard = _card;
        if (_card != null)
        {
            this.mCardId = _card.card_id;
            Transform transform = base.transform.FindChild("InfoPanel");
            this.UpdateShowCardByEntry(_card);
            this.SetCardInfo(_card, transform);
            this.SetGemInfo(_card, true);
            this.SetWearDrawDesc();
        }
    }

    private void InitGemHoleState(Transform part)
    {
        part.FindChild("Delta").gameObject.SetActive(false);
        part.FindChild("IconAdd").gameObject.SetActive(false);
        part.FindChild("bg_l").gameObject.SetActive(false);
        part.FindChild("UnOpen").gameObject.SetActive(true);
    }

    private void InitGuiControlEvent()
    {
        this.mGemList.Clear();
        Transform transform = base.transform.FindChild("InfoPanel/Gem");
        for (int i = 0; i < 6; i++)
        {
            Transform item = transform.FindChild(i.ToString());
            if (m_nHoleOpenCount > i)
            {
                this.mGemList.Add(item);
            }
            else
            {
                this.InitGemHoleState(item);
            }
        }
        this._GemInfoPanel = base.transform.FindChild("GemInfo/Group2");
    }

    private void OnClickBagGem()
    {
        BagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.InitItemList(ShowItemType.Gem);
            activityGUIEntity.mFirstShowGemTable = true;
            activityGUIEntity.SelectGemCheckbox();
        }
        else
        {
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = delegate (GUIEntity obj) {
                    BagPanel panel = (BagPanel) obj;
                    panel.InitItemList(ShowItemType.Gem);
                    panel.mFirstShowGemTable = true;
                    panel.SelectGemCheckbox();
                };
            }
            GUIMgr.Instance.PushGUIEntity("BagPanel", <>f__am$cacheF);
        }
    }

    private void OnClickGemBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            <OnClickGemBtn>c__AnonStorey189 storey = new <OnClickGemBtn>c__AnonStorey189 {
                info = obj2 as CardGemInfo
            };
            if (storey.info.entry >= 0)
            {
                int num = 0;
                Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storey.<>m__1EB));
                if (item != null)
                {
                    num = item.num;
                }
                if (num >= 0x3e7)
                {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(storey.info.entry);
                    if (_config != null)
                    {
                        TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x152), _config.name));
                    }
                }
                else
                {
                    SocketMgr.Instance.RequestRemoveCardGem(this.mCard.card_id, storey.info.part);
                }
            }
        }
    }

    private void OnClickItemBtn(BagTableItem item)
    {
        Item data = item.ItemData.Data;
        this.SetItemDetails(item);
        Debug.Log(data.entry + ":" + data.num);
        SoundManager.mInstance.PlaySFX("sound_ui_t_8");
    }

    private void OnClickLeft()
    {
        if ((this.mCurrShowCardList != null) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, false);
            this.InitCardInfo(cardInfo);
        }
    }

    private void OnClickRight()
    {
        if ((this.mCurrShowCardList != null) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, true);
            this.InitCardInfo(cardInfo);
        }
    }

    private void OnClickRule()
    {
        if (<>f__am$cacheE == null)
        {
            <>f__am$cacheE = delegate (GUIEntity obj) {
            };
        }
        GUIMgr.Instance.DoModelGUI("GemWearRulePanel", <>f__am$cacheE, null);
    }

    private void OnClickWear()
    {
        int emptyHoleIndex = this.GetEmptyHoleIndex();
        if (emptyHoleIndex == -1)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e4f));
        }
        else if (!this.CanWear(this.mCurrSelectItemEntry.ItemData.Data.entry))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e50));
        }
        else if (this.mCurrSelectItemEntry != null)
        {
            SocketMgr.Instance.RequestEnchaseCardGem(this.mCard.card_id, emptyHoleIndex, this.mCurrSelectItemEntry.ItemData.Data.entry);
        }
    }

    public void OnClosePanel()
    {
        GUIMgr.Instance.PopGUIEntity();
        HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.InitCardInfo(this.mCard);
            activityGUIEntity.SetCurrShowCardList(this.mCurrShowCardList);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        if (!ActorData.getInstance().IsPopPanel)
        {
            this.ResetClipViewport();
            this.UpdateData();
        }
        ActorData.getInstance().IsPopPanel = false;
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.transform.FindChild("TopRight").gameObject.SetActive(false);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.multiCamera = true;
        m_nHoleOpenCount = CommonFunc.GetMaxHoleCount();
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config != null)
        {
            this.C_Limits[0] = _config.card_gem1_openlv;
            this.C_Limits[1] = _config.card_gem2_openlv;
            this.C_Limits[2] = _config.card_gem3_openlv;
            this.C_Limits[3] = _config.card_gem4_openlv;
            this.C_Limits[4] = _config.card_gem5_openlv;
            this.C_Limits[5] = _config.card_gem6_openlv;
            this.m_cardgem_maxquality = _config.cardgem_maxquality;
        }
        UIGrid grid = base.FindChild<UIGrid>("Grid");
        this.ItemTable.InitFromGrid(grid);
        this.InitGuiControlEvent();
        this.SetArrowPostion();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                enumerator.Current.Clear();
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.transform.FindChild("TopRight").gameObject.SetActive(true);
        }
    }

    private void ResetClipViewport()
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        Transform top = base.transform.FindChild("GemInfo/ListTopLeft");
        Transform bottom = base.transform.FindChild("GemInfo/ListBottomRight");
        Transform bounds = base.transform.FindChild("GemInfo/List");
        GUIMgr.Instance.ResetListViewpot(top, bottom, bounds, 0f);
    }

    private void SetArrowPostion()
    {
        if (GUIMgr.Instance.Root.activeWidth < 0x3e8)
        {
            Transform transform = base.transform.FindChild("Left/LBtn");
            Transform transform2 = base.transform.FindChild("Right/RBtn");
            transform.transform.localPosition = new Vector3(20f, -255f, transform.transform.localPosition.z);
            transform2.transform.localPosition = new Vector3(-38f, -255f, transform2.transform.localPosition.z);
        }
    }

    private void SetCardInfo(Card _card, Transform obj)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                obj.FindChild("Name").GetComponent<UILabel>().text = GameConstant.C_Label[_card.cardInfo.quality] + CommonFunc.GetCardNameByQuality(_card.cardInfo.quality, _config.name);
                obj.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetQualityBorder(obj.FindChild("Head/QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                Transform transform = obj.FindChild("Head/Star");
                for (int i = 0; i < 5; i++)
                {
                    UISprite component = transform.FindChild(string.Empty + (i + 1)).GetComponent<UISprite>();
                    component.gameObject.SetActive(i < _card.cardInfo.starLv);
                    component.transform.localPosition = new Vector3((float) (i * 20), 0f, 0f);
                }
                transform.localPosition = new Vector3(-10f - ((_card.cardInfo.starLv - 1) * 10f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
            }
        }
    }

    public void SetCurrShowCardList(List<Card> _CurrShowCardList)
    {
        this.mCurrShowCardList = _CurrShowCardList;
    }

    private void SetGemInfo(Card _card, bool isCanClickEquip)
    {
        if ((_card != null) && (ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry) != null))
        {
            Transform transform = base.transform.FindChild("InfoPanel/Gem");
            for (int i = 0; i < m_nHoleOpenCount; i++)
            {
                Transform transform2 = this.mGemList[i];
                CardGemInfo info = _card.cardGemInfo[i];
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGemBtn);
                transform2.GetComponent<UIButton>().isEnabled = isCanClickEquip;
                this.SetSingleGemInfo(info, i);
            }
        }
    }

    private void SetItemDetails(BagTableItem tItem)
    {
        if (this.mCurrSelectItemEntry != null)
        {
            this.mCurrSelectItemEntry.ShowChecked(false);
        }
        this.mCurrSelectItemEntry = tItem;
        Item data = tItem.ItemData.Data;
        if (data != null)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(data.entry);
            if (_config != null)
            {
                Transform transform = base.transform.FindChild("GemInfo/Group2");
                this.mCurrSelectItemEntry.ShowChecked(true);
                UISprite component = transform.FindChild("Item/QualityBorder").GetComponent<UISprite>();
                CommonFunc.SetEquipQualityBorder(component, _config.quality, false);
                component.gameObject.SetActive(true);
                transform.FindChild("Name").GetComponent<UILabel>().text = _config.name;
                transform.FindChild("Num").GetComponent<UILabel>().text = data.num.ToString();
                transform.FindChild("Item/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                transform.FindChild("Val").GetComponent<UILabel>().text = this.GetAddDeltaByID(_config.entry, 1);
                GUIDataHolder.setData(base.transform.FindChild("GemInfo/Group2/WearBtn").gameObject, data);
                if (!transform.active)
                {
                    transform.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetSingleGemInfo(CardGemInfo info, int index)
    {
        Transform transform = this.mGemList[index];
        UILabel component = transform.FindChild("Delta").GetComponent<UILabel>();
        UISprite sprite = transform.FindChild("IconAdd").GetComponent<UISprite>();
        UITexture texture = transform.FindChild("Icon").GetComponent<UITexture>();
        UISprite sprite2 = transform.FindChild("QualityBorder").GetComponent<UISprite>();
        sprite2.gameObject.SetActive(info.entry >= 0);
        GUIDataHolder.setData(transform.gameObject, info);
        if (info.entry < 0)
        {
            texture.mainTexture = null;
            CommonFunc.SetEquipQualityBorder(sprite2, 7, false);
            sprite.gameObject.SetActive(true);
            if (this.mCard.cardInfo.level >= this.C_Limits[index])
            {
                component.text = "[decbaa]" + ConfigMgr.getInstance().GetWord(0x4e4d);
                sprite.spriteName = string.Empty;
            }
            else
            {
                component.text = "[decbaa]" + string.Format(ConfigMgr.getInstance().GetWord(0x4e29), this.C_Limits[index]);
                sprite.spriteName = "Ui_Heroinfo_Icon_lock";
            }
        }
        else
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                CommonFunc.SetEquipQualityBorder(sprite2, _config.quality, false);
                this.m_strEnchase = this.GetAddDeltaByID(info.entry, 1);
                component.text = "[c0f940]" + this.m_strEnchase;
                sprite.gameObject.SetActive(false);
            }
        }
    }

    private void SetWearDrawDesc()
    {
        int[] numArray = new int[6];
        for (int i = 0; i < 6; i++)
        {
            CardGemInfo info = this.mCard.cardGemInfo[i];
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (((_config != null) && (_config.quality >= 0)) && (_config.quality < 6))
            {
                for (int j = 0; j <= _config.quality; j++)
                {
                    numArray[j]++;
                }
            }
        }
        card_gem_attr_config gc = ConfigMgr.getInstance().getByEntry<card_gem_attr_config>((int) this.mCard.cardInfo.entry);
        if (gc != null)
        {
            int minAttrLevel = this.GetMinAttrLevel(gc);
            int[] numArray2 = new int[] { -1, -1, -1 };
            int[] numArray3 = new int[] { 1, 1, 1 };
            int[] numArray4 = new int[] { 2, 4, 6 };
            for (int k = 0; k < 3; k++)
            {
                for (int n = 5; n >= 0; n--)
                {
                    if (numArray[n] >= numArray4[k])
                    {
                        numArray2[k] = n;
                        break;
                    }
                }
            }
            for (int m = 0; m < 3; m++)
            {
                if (numArray2[m] < minAttrLevel)
                {
                    numArray2[m] = minAttrLevel;
                    numArray3[m] = 0;
                }
            }
            base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo").gameObject.SetActive(minAttrLevel >= 0);
            base.transform.FindChild("InfoPanel/BaseInfo/Desc").gameObject.SetActive(minAttrLevel < 0);
            int num7 = 0;
            int num8 = -1;
            int num9 = 0;
            int num10 = -1;
            int num11 = 0;
            int num12 = -1;
            this.GetGemAttrByLevels(out num8, out num7, out num10, out num9, out num12, out num11, numArray2[0], numArray2[1], numArray2[2]);
            int num13 = 0;
            int num14 = -1;
            int num15 = 0;
            int num16 = -1;
            int num17 = 0;
            int num18 = -1;
            this.GetGemAttrByLevels(out num14, out num13, out num16, out num15, out num18, out num17, numArray2[0] + 1, numArray2[1] + 1, numArray2[2] + 1);
            UILabel component = base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Two").GetComponent<UILabel>();
            component.gameObject.SetActive(num8 != -1);
            if (num8 != -1)
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e51), 2, numArray2[0] + 1);
                string str = (numArray3[0] != 1) ? "[decbaa]" : "[c0f940]";
                object[] objArray1 = new object[] { str, this.getTypeString(num8), "+", num7 };
                base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Two/Val").GetComponent<UILabel>().text = string.Concat(objArray1);
            }
            UILabel label3 = base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Four").GetComponent<UILabel>();
            label3.gameObject.SetActive((num10 != -1) && (m_nHoleOpenCount >= 4));
            if ((num10 != -1) && (m_nHoleOpenCount >= 4))
            {
                label3.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e51), 4, numArray2[1] + 1);
                string str2 = (numArray3[1] != 1) ? "[decbaa]" : "[c0f940]";
                object[] objArray2 = new object[] { str2, this.getTypeString(num10), "+", num9 };
                base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Four/Val").GetComponent<UILabel>().text = string.Concat(objArray2);
            }
            UILabel label5 = base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Six").GetComponent<UILabel>();
            label5.gameObject.SetActive((num12 != -1) && (m_nHoleOpenCount >= 6));
            if ((num12 != -1) && (m_nHoleOpenCount >= 6))
            {
                label5.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e51), 6, numArray2[2] + 1);
                string str3 = (numArray3[2] != 1) ? "[decbaa]" : "[c0f940]";
                object[] objArray3 = new object[] { str3, this.getTypeString(num12), "+", num11 };
                base.transform.FindChild("InfoPanel/BaseInfo/RewardInfo/Six/Val").GetComponent<UILabel>().text = string.Concat(objArray3);
            }
        }
    }

    private void ShowItemList()
    {
        bool flag = true;
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                item_config config = current.ItemData.Config;
                if (config == null)
                {
                    current.DoHide(true);
                }
                else
                {
                    current.DoHide(config.type != 10);
                    if (flag && current.Root.gameObject.activeSelf)
                    {
                        this.SetItemDetails(current);
                        flag = false;
                    }
                    current.Drag.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
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
        int totalCountNoHide = this.ItemTable.TotalCountNoHide;
        for (int i = 0; i < this.ItemTable.Count; i++)
        {
            BagTableItem item2 = this.ItemTable[i];
            if ((totalCountNoHide <= 12) && (item2.Drag != null))
            {
                item2.Drag.draggableCamera = null;
            }
        }
        this.ItemTable.RepositionLayout();
        this.ResetClipViewport();
    }

    public void UpdateData()
    {
        this.CreateItemList(true);
        this.ShowItemList();
        if (this.ItemTable.Count > 0)
        {
            this.SetItemDetails(this.ItemTable[0]);
        }
    }

    internal void UpdateData(List<Item> list)
    {
        bool flag = true;
        IEnumerator<BagTableItem> enumerator = this.ItemTable.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                BagTableItem current = enumerator.Current;
                if (current.ItemData.Data.entry == list[0].entry)
                {
                    flag = false;
                    goto Label_005D;
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
    Label_005D:
        if (flag)
        {
            this.UpdateData();
        }
        else
        {
            IEnumerator<BagTableItem> enumerator2 = this.ItemTable.GetEnumerator();
            try
            {
                while (enumerator2.MoveNext())
                {
                    BagTableItem tItem = enumerator2.Current;
                    foreach (Item item3 in list)
                    {
                        if (tItem.ItemData.Data.entry == item3.entry)
                        {
                            tItem.ItemData.Data.num = item3.num;
                            tItem.ItemData = tItem.ItemData;
                            if (this.mCurrSelectItemEntry.ItemData.Data.entry == item3.entry)
                            {
                                if (item3.num == 0)
                                {
                                    BagTableItem fristItemNoHide = this.ItemTable.FristItemNoHide;
                                    if (fristItemNoHide == null)
                                    {
                                        this._GemInfoPanel.gameObject.SetActive(false);
                                        base.transform.FindChild("GemInfo/Desc").gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        this.SetItemDetails(fristItemNoHide);
                                    }
                                }
                                else
                                {
                                    this.SetItemDetails(tItem);
                                    base.transform.FindChild("GemInfo/Desc").gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (enumerator2 == null)
                {
                }
                enumerator2.Dispose();
            }
        }
        this.ItemTable.RepositionLayout();
    }

    public void UpdateShowCardByEntry(Card _card)
    {
        if (this.mCurrShowCardList != null)
        {
            for (int i = 0; i < this.mCurrShowCardList.Count; i++)
            {
                if (this.mCurrShowCardList[i].cardInfo.entry == _card.cardInfo.entry)
                {
                    this.mCurrShowCardList[i] = _card;
                    break;
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickGemBtn>c__AnonStorey189
    {
        internal CardGemInfo info;

        internal bool <>m__1EB(Item e)
        {
            return (e.entry == this.info.entry);
        }
    }

    public class BagItemData
    {
        public item_config Config { get; set; }

        public Item Data { get; set; }
    }

    public class BagTableItem : UITableItem
    {
        private GemWearPanel.BagItemData _item;
        private UISprite Checked;
        private UILabel Count;
        public UIDragCamera Drag;
        private UITexture Icon;
        private Transform Item1;
        public Action<GemWearPanel.BagTableItem> OnClick;
        private UISprite QualityBorder;

        internal void Clear()
        {
            this.Icon.mainTexture = null;
        }

        public void DoHide(bool flag)
        {
            if (this._item.Data.num == 0)
            {
                flag = true;
            }
            base.Hide = flag;
        }

        public override void OnCreate()
        {
            this.Icon = base.Root.FindChild<UITexture>("Icon");
            Transform ui = base.Root.FindChild<Transform>("Item1");
            this.QualityBorder = base.Root.FindChild<UISprite>("QualityBorder");
            this.Count = base.Root.FindChild<UILabel>("Count");
            this.Drag = ui.GetComponent<UIDragCamera>();
            this.Checked = base.Root.FindChild<UISprite>("Checked");
            ui.OnUIMouseClick(delegate (object u) {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            });
            this.Item1 = ui;
        }

        public void ShowChecked(bool show)
        {
            this.Checked.gameObject.SetActive(show);
        }

        public GemWearPanel.BagItemData ItemData
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
                this.DoHide(this._item.Data.num == 0);
                item_config config = value.Config;
                if (config != null)
                {
                    this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(config.icon);
                    CommonFunc.SetEquipQualityBorder(this.QualityBorder, config.quality, false);
                    this.QualityBorder.gameObject.SetActive(true);
                    this.Item1.GetComponent<UIToggle>().enabled = true;
                    if (value.Data.num > 1)
                    {
                        this.Count.text = value.Data.num.ToString();
                    }
                    else
                    {
                        this.Count.text = string.Empty;
                    }
                }
            }
        }
    }
}

