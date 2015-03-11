using FastBuf;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class BreakEquipPanel : GUIEntity
{
    public GameObject _BreakGroup;
    public GameObject _CombiningGroup;
    public GameObject _EquipLvPanel;
    public GameObject _EquipTips;
    public GameObject _FromGroup;
    public GameObject _InfoPane;
    public UILabel _JingHuaBtnLabel;
    private UIToggle _JingHuaTab;
    public GameObject _Menu1;
    public GameObject _Menu2;
    public GameObject _Menu3;
    public GameObject _Menu4;
    private UIToggle _ShengJiTab;
    public GameObject _SingleFromItem;
    public GameObject _SubPanel;
    [CompilerGenerated]
    private static Action<Transform> <>f__am$cache20;
    private bool mCanBreak;
    private Card mCard;
    private long mCardId = -1L;
    private List<Card> mCurrShowCardList;
    private List<Transform> mEffectList = new List<Transform>();
    private List<Transform> mEquipGirdList = new List<Transform>();
    private bool mIsChanged;
    private bool mIsOpenEquipLvGroup = true;
    private bool mIsOpenSubGroup;
    private int mMaterialEntry;
    private int mOneMatialEntry = -1;
    private int mPartIdx = -1;
    private int mSecondMatialEntry = -1;
    private SingleEquipLevUpPanel mSingleEquipLevUpPanel;
    private Transform mTipsCombMaterialTf;
    private Transform mTipsMaterialTf;
    private int mUseItemEntry = -1;

    private bool CheckCanCombByItemEntry(int itemEntry)
    {
        bool flag = false;
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemEntry);
        if (_config == null)
        {
            return false;
        }
        if (_config.type == 2)
        {
            item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.param_0);
            if (_config2 == null)
            {
                return false;
            }
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config2.param_0);
            int num = (itemByEntry != null) ? itemByEntry.num : 0;
            if (num >= _config2.param_1)
            {
                flag = true;
            }
            return flag;
        }
        if (_config.param_0 > 0)
        {
            Item item2 = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_0);
            int num2 = (item2 != null) ? item2.num : 0;
            flag = num2 >= _config.param_1;
        }
        if (flag && (_config.param_2 > 0))
        {
            Item item3 = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_2);
            int num3 = (item3 != null) ? item3.num : 0;
            flag = num3 >= _config.param_3;
        }
        if (flag && (_config.param_4 > 0))
        {
            Item item4 = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_4);
            int num4 = (item4 != null) ? item4.num : 0;
            flag = num4 >= _config.param_5;
        }
        return flag;
    }

    private void ClearEffect()
    {
        if (<>f__am$cache20 == null)
        {
            <>f__am$cache20 = obj => obj.gameObject.SetActive(false);
        }
        this.mEffectList.ForEach(<>f__am$cache20);
    }

    private void ClickAutoLevUp()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Strengthen_Function))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Strengthen.tag_equip_strengthen_function, null);
        }
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.ClickAutoLevUp(this.mCard, this.mPartIdx);
        }
    }

    private void ClickLevUp()
    {
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.ClickLevUp(this.mCard, this.mPartIdx);
        }
    }

    private void ClickOneKeyLevUp()
    {
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.ClickOneKeyLevUp(this.mCard);
        }
    }

    public void CloseDupPanelEvent(GameObject go)
    {
        this.SmashReturnEvent();
    }

    public void CombineSucess()
    {
        if (this._Menu3.gameObject.activeSelf)
        {
            if (this.IsBackReturn(2))
            {
                this.OnClickMenu2Tab(this._Menu2.gameObject);
            }
            else
            {
                this.OnClickMenu3Tab(this._Menu3.gameObject);
            }
        }
        else
        {
            if (this.IsBackReturn(1))
            {
                this.OnClickMenu1();
            }
            else
            {
                this.OnClickMenu2Tab(this._Menu2.gameObject);
            }
            this.UpdateMenu1Material();
        }
    }

    public void DelItemUpdateData()
    {
        this.UpdateData(this.mCard, this.mPartIdx, this.mCard.equipInfo[this.mPartIdx]);
    }

    private List<string> GetAddStr(EquipInfo _data)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_data.entry);
        int num = _data.lv - this.mCard.equipInfo[this.mPartIdx].lv;
        List<string> list = new List<string>();
        string str = "[00ff00]";
        string str2 = " + ";
        if (_config.attack_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x155), str2, _config.attack_grow * num }));
        }
        if (_config.hp_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(340), str2, _config.hp_grow * num }));
        }
        if (_config.physics_defence_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15a), str2, _config.physics_defence_grow * num }));
        }
        if (_config.spell_defence_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15b), str2, _config.spell_defence_grow * num }));
        }
        if (_config.crit_level_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(350), str2, _config.crit_level_grow * num }));
        }
        if (_config.dodge_level_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x162), str2, _config.dodge_level_grow * num }));
        }
        if (_config.tenacity_level_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x160), str2, _config.tenacity_level_grow * num }));
        }
        if (_config.hit_level_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x161), str2, _config.hit_level_grow * num }));
        }
        if (_config.hp_recover_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x163), str2, _config.hp_recover_grow * num }));
        }
        if (_config.energy_recover_grow > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x164), str2, _config.energy_recover_grow * num }));
        }
        return list;
    }

    private List<string> GetAllAddStr(List<EquipInfo> _newList, List<EquipInfo> _OldList)
    {
        List<string> list = new List<string>();
        string str = "[00ff00]";
        string str2 = " + ";
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        int num10 = 0;
        int num11 = 0;
        foreach (EquipInfo info in _newList)
        {
            EquipInfo info2 = _OldList[num];
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            int num12 = info.lv - info2.lv;
            if (_config.attack_grow > 0)
            {
                num2 += _config.attack_grow * num12;
            }
            if (_config.hp_grow > 0)
            {
                num3 += _config.hp_grow * num12;
            }
            if (_config.physics_defence_grow > 0)
            {
                num4 += _config.physics_defence_grow * num12;
            }
            if (_config.spell_defence_grow > 0)
            {
                num5 += _config.spell_defence_grow * num12;
            }
            if (_config.crit_level_grow > 0)
            {
                num6 += _config.crit_level_grow * num12;
            }
            if (_config.dodge_level_grow > 0)
            {
                num7 += _config.dodge_level_grow * num12;
            }
            if (_config.tenacity_level_grow > 0)
            {
                num8 += _config.tenacity_level_grow * num12;
            }
            if (_config.hit_level_grow > 0)
            {
                num9 += _config.hit_level_grow * num12;
            }
            if (_config.hp_recover_grow > 0)
            {
                num10 += _config.hp_recover_grow * num12;
            }
            if (_config.energy_recover_grow > 0)
            {
                num11 += _config.energy_recover_grow * num12;
            }
            num++;
        }
        if (num2 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x155), str2, num2 }));
        }
        if (num3 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(340), str2, num3 }));
        }
        if (num4 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15a), str2, num4 }));
        }
        if (num5 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15b), str2, num5 }));
        }
        if (num6 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(350), str2, num6 }));
        }
        if (num7 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x162), str2, num7 }));
        }
        if (num8 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x160), str2, num8 }));
        }
        if (num9 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x161), str2, num9 }));
        }
        if (num10 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x163), str2, num10 }));
        }
        if (num11 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x164), str2, num11 }));
        }
        return list;
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

    private int GetCurrSubStat(bool isClickEquipLv)
    {
        if (isClickEquipLv)
        {
            if (this.mIsOpenSubGroup)
            {
                return (!this.mIsOpenEquipLvGroup ? 6 : 5);
            }
            return (!this.mIsOpenEquipLvGroup ? 3 : 4);
        }
        if (this.mIsOpenEquipLvGroup)
        {
            return (!this.mIsOpenSubGroup ? 5 : 6);
        }
        return (!this.mIsOpenSubGroup ? 1 : 2);
    }

    private bool IsBackReturn(int _MenuIdx)
    {
        switch (_MenuIdx)
        {
            case 1:
            {
                <IsBackReturn>c__AnonStorey17C storeyc = new <IsBackReturn>c__AnonStorey17C();
                if (this.mCard == null)
                {
                    return false;
                }
                EquipInfo info = this.mCard.equipInfo[this.mPartIdx];
                storeyc.bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                if (storeyc.bec == null)
                {
                    return false;
                }
                bool flag = true;
                if (storeyc.bec.need_item_1 >= 0)
                {
                    flag &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyc.<>m__1C8));
                }
                if (flag && (storeyc.bec.need_item_2 >= 0))
                {
                    flag &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyc.<>m__1C9));
                }
                if (flag && (storeyc.bec.need_item_3 >= 0))
                {
                    flag &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyc.<>m__1CA));
                }
                return flag;
            }
            case 2:
            {
                <IsBackReturn>c__AnonStorey17E storeye = new <IsBackReturn>c__AnonStorey17E();
                object obj2 = GUIDataHolder.getData(this._Menu2.gameObject);
                if (obj2 == null)
                {
                    return false;
                }
                int id = (int) obj2;
                storeye.ic = ConfigMgr.getInstance().getByEntry<item_config>(id);
                if (storeye.ic == null)
                {
                    return false;
                }
                if (storeye.ic.type == 2)
                {
                    <IsBackReturn>c__AnonStorey17D storeyd = new <IsBackReturn>c__AnonStorey17D {
                        ic_target = ConfigMgr.getInstance().getByEntry<item_config>(storeye.ic.param_0)
                    };
                    return XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyd.<>m__1CB));
                }
                bool flag2 = true;
                if (storeye.ic.param_0 >= 0)
                {
                    flag2 &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeye.<>m__1CC));
                }
                if (flag2 && (storeye.ic.param_2 >= 0))
                {
                    flag2 &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeye.<>m__1CD));
                }
                if (flag2 && (storeye.ic.param_4 >= 0))
                {
                    flag2 &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeye.<>m__1CE));
                }
                return flag2;
            }
        }
        return false;
    }

    private void JumpToDuplicate(GameObject go)
    {
        <JumpToDuplicate>c__AnonStorey17A storeya = new <JumpToDuplicate>c__AnonStorey17A {
            <>f__this = this
        };
        SoundManager.mInstance.PlaySFX("sound_ui_t_1");
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storeya.info = obj2 as MapData;
            if (storeya.info != null)
            {
                Debug.Log(storeya.info.entry + "  " + storeya.info.subEntry);
                SocketMgr.Instance.RequestGetDuplicateRemain(storeya.info.entry, DuplicateType.DupType_Normal);
                SocketMgr.Instance.RequestGetDuplicateRemain(storeya.info.entry, DuplicateType.DupType_Elite);
                GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storeya.<>m__1C4));
            }
        }
    }

    private void JumpToTower(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().void_tower)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().void_tower));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("TowerPanel", null);
        }
    }

    private void JumpToWorldCup(GameObject go)
    {
        GUIMgr.Instance.PushGUIEntity("WorldCupPanel", null);
    }

    public void LevelUpEquipSucess(Card _cardInfo)
    {
        this.UpdateData(_cardInfo, this.mPartIdx, _cardInfo.equipInfo[this.mPartIdx]);
        this.UpdateShowCardByEntry(_cardInfo);
    }

    private void OnClickBreak(GameObject go)
    {
        SoundManager.mInstance.PlaySFX("sound_ui_t_1");
        if (!this.mCanBreak)
        {
            base.StopAllCoroutines();
            base.StartCoroutine(this.PopEquipTips(this.mTipsMaterialTf, this.mEquipGirdList[this.mPartIdx].FindChild("Up").gameObject.activeSelf));
        }
        else if (this.mCard != null)
        {
            EquipInfo info = this.mCard.equipInfo[this.mPartIdx];
            if (info != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
                if (_config != null)
                {
                    if (_config.quality >= (this.mCard.cardInfo.quality + 1))
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287f));
                    }
                    else
                    {
                        Debug.Log(_config.quality + " ------------   " + this.mCard.cardInfo.quality);
                        break_equip_config _config2 = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                        if (_config2 != null)
                        {
                            if (ActorData.getInstance().Gold < _config2.cost_gold)
                            {
                                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                            }
                            else if (_config2.break_equip_entry < 0)
                            {
                                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287c));
                            }
                            else if ((this.mCardId > -1L) && (this.mPartIdx > -1))
                            {
                                SocketMgr.Instance.RequestEquipBreak(this.mCardId, this.mPartIdx);
                            }
                            else
                            {
                                TipsDiag.SetText("error card id!");
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnClickCombiningBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            TempData data = (TempData) obj2;
            if (data != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(data.TargetEntry);
                if (_config != null)
                {
                    bool flag = false;
                    if (_config.param_0 > 0)
                    {
                        Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_0);
                        int num = (itemByEntry != null) ? itemByEntry.num : 0;
                        flag = num >= _config.param_1;
                    }
                    if (flag && (_config.param_2 > 0))
                    {
                        Item item2 = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_2);
                        int num2 = (item2 != null) ? item2.num : 0;
                        flag = num2 >= _config.param_3;
                    }
                    if (flag && (_config.param_4 > 0))
                    {
                        Item item3 = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(_config.param_4);
                        int num3 = (item3 != null) ? item3.num : 0;
                        flag = num3 >= _config.param_5;
                    }
                    if (flag)
                    {
                        SocketMgr.Instance.RequestItemMachining(1, data.TargetEntry);
                    }
                    else
                    {
                        base.StopAllCoroutines();
                        base.StartCoroutine(this.PopEquipTips(this.mTipsCombMaterialTf, !data.mIsPart));
                    }
                }
            }
        }
    }

    private void OnClickCombiningMaterialItem(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int id = (int) obj2;
            item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(id);
            if (ic != null)
            {
                this.SetItemFromInfo(id, ic);
                this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
                if (!this._Menu3.activeSelf)
                {
                    this._Menu3.transform.localPosition = new Vector3(225.7f, this._Menu3.transform.localPosition.y, this._Menu3.transform.localPosition.z);
                    this._Menu3.gameObject.SetActive(true);
                    this._BreakGroup.gameObject.SetActive(false);
                    this._CombiningGroup.gameObject.SetActive(false);
                    this._FromGroup.gameObject.SetActive(true);
                    this._Menu3.transform.FindChild("FragSprite").gameObject.SetActive(ic.type == 2);
                    GUIDataHolder.setData(this._Menu3, id);
                }
                else
                {
                    this._Menu3.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu4.transform.localPosition = new Vector3(346.98f, this._Menu4.transform.localPosition.y, this._Menu4.transform.localPosition.z);
                    this.SetMenuInfo(this._Menu4.transform, id);
                    this._BreakGroup.gameObject.SetActive(false);
                    this._CombiningGroup.gameObject.SetActive(false);
                    this._FromGroup.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnClickCombiningMaterialItem2(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int entry = (int) obj2;
            this.mSecondMatialEntry = entry;
            this.SetCombiningGroupInfo(entry);
            this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
            this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
            this.SetMenuInfo(this._Menu3.transform, entry);
            this._Menu3.transform.localPosition = new Vector3(225.7f, this._Menu3.transform.localPosition.y, this._Menu3.transform.localPosition.z);
            GUIDataHolder.setData(this._Menu3.gameObject, entry);
            UIEventListener.Get(this._Menu3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMenu3Tab);
        }
    }

    private void OnClickEquipBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            EquipInfo info = obj2 as EquipInfo;
            if (info != null)
            {
                this.ShowOneLayer();
                this.SetEquipInfo(info);
            }
        }
    }

    private void OnClickJingHuaBtn(GameObject go)
    {
    }

    private void OnClickJingHuaTab(GameObject go)
    {
        this._SubPanel.gameObject.SetActive(true);
        this._EquipLvPanel.gameObject.SetActive(false);
    }

    private void OnClickLeft()
    {
        if ((this.mCurrShowCardList != null) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, false);
            this.mPartIdx = 0;
            this.ShowOneLayer();
            this.UpdateData(cardInfo, 0, cardInfo.equipInfo[0]);
            this.ClearEffect();
        }
    }

    private void OnClickMaterialItem(GameObject go)
    {
        this._EquipTips.gameObject.SetActive(false);
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int id = (int) obj2;
            item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(id);
            if (ic != null)
            {
                this.mSecondMatialEntry = -1;
                this.mOneMatialEntry = -1;
                if (ic.param_0 < 0)
                {
                    this.SetItemFromInfo(id, ic);
                    this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu3.transform.localPosition = this._Menu2.transform.localPosition;
                    this._Menu3.gameObject.SetActive(true);
                    this._BreakGroup.gameObject.SetActive(false);
                    this._CombiningGroup.gameObject.SetActive(false);
                    this._FromGroup.gameObject.SetActive(true);
                    GUIDataHolder.setData(this._Menu3.gameObject, null);
                }
                else
                {
                    UITexture component = this._Menu2.transform.FindChild("Icon").GetComponent<UITexture>();
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                    component.alpha = 1f;
                    CommonFunc.SetEquipQualityBorder(this._Menu2.transform.FindChild("QualityBorder").GetComponent<UISprite>(), ic.quality, false);
                    this.mOneMatialEntry = id;
                    this.SetCombiningGroupInfo(id);
                    GUIDataHolder.setData(this._Menu2.gameObject, id);
                    UIEventListener.Get(this._Menu2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMenu2Tab);
                    this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.gameObject.SetActive(true);
                    this._Menu3.gameObject.SetActive(false);
                    this._BreakGroup.gameObject.SetActive(false);
                    this._CombiningGroup.gameObject.SetActive(true);
                    this._FromGroup.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnClickMenu1()
    {
        this._Menu1.transform.FindChild("Border").gameObject.SetActive(true);
        this._Menu2.gameObject.SetActive(false);
        this._Menu3.gameObject.SetActive(false);
        this._Menu4.gameObject.SetActive(false);
        this._BreakGroup.gameObject.SetActive(true);
        this._CombiningGroup.gameObject.SetActive(false);
        this._FromGroup.gameObject.SetActive(false);
        this._EquipTips.gameObject.SetActive(false);
        if (ActorData.getInstance().mItemDataDirty)
        {
            this.UpdateMenu1Material();
            ActorData.getInstance().mItemDataDirty = false;
        }
    }

    private void OnClickMenu2()
    {
    }

    private void OnClickMenu2Tab(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int entry = (int) obj2;
            this.SetCombiningGroupInfo(entry);
            this._CombiningGroup.gameObject.SetActive(true);
            this._Menu2.transform.FindChild("Border").gameObject.SetActive(true);
            this._FromGroup.gameObject.SetActive(false);
            this._Menu3.gameObject.SetActive(false);
            this._Menu4.gameObject.SetActive(false);
            this._EquipTips.gameObject.SetActive(false);
        }
    }

    private void OnClickMenu3Tab(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int entry = (int) obj2;
            this.SetCombiningGroupInfo(entry);
            this._Menu3.transform.FindChild("Border").gameObject.SetActive(true);
            this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
            this._CombiningGroup.gameObject.SetActive(true);
            this._FromGroup.gameObject.SetActive(false);
            this._Menu4.gameObject.SetActive(false);
            this._EquipTips.gameObject.SetActive(false);
        }
    }

    private void OnClickPutOn(GameObject go)
    {
        SoundManager.mInstance.PlaySFX("sound_ui_t_1");
        if (((this.mCardId > -1L) && (this.mPartIdx > -1)) && (this.mUseItemEntry > -1))
        {
            SocketMgr.Instance.RequestUseItem(this.mCardId, this.mUseItemEntry, 1, this.mPartIdx);
        }
    }

    private void OnClickRight()
    {
        if ((this.mCurrShowCardList != null) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, true);
            this.mPartIdx = 0;
            this.ShowOneLayer();
            this.UpdateData(cardInfo, 0, cardInfo.equipInfo[0]);
            this.ClearEffect();
        }
    }

    private void OnClickShengJi()
    {
    }

    private void OnClickShengJiTab(GameObject go)
    {
        this._SubPanel.gameObject.SetActive(false);
        this._EquipLvPanel.gameObject.SetActive(true);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        ActorData.getInstance().mCurrDupReturnPrePara = null;
        GUIMgr.Instance.FloatTitleBar();
        this._EquipTips.gameObject.SetActive(false);
        if (GuideSystem.MatchEvent(GuideEvent.Strengthen_Function))
        {
            <OnDeSerialization>c__AnonStorey178 storey = new <OnDeSerialization>c__AnonStorey178 {
                <>f__this = this,
                tns = this.mSingleEquipLevUpPanel.transform.FindChild("AutoBtn")
            };
            if (null == storey.tns)
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                this.DelayCallBack(0.1f, new System.Action(storey.<>m__1BF));
            }
        }
        this.ClearEffect();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(this._InfoPane.transform.FindChild("JingHuaBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickJingHuaBtn);
        this.mSingleEquipLevUpPanel = this._EquipLvPanel.GetComponent<SingleEquipLevUpPanel>();
        this._ShengJiTab = base.transform.FindChild("Tab/ShengJiTab").GetComponent<UIToggle>();
        this._JingHuaTab = base.transform.FindChild("Tab/JingHuaTab").GetComponent<UIToggle>();
        UIEventListener.Get(this._ShengJiTab.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShengJiTab);
        UIEventListener.Get(this._JingHuaTab.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickJingHuaTab);
        this.mEquipGirdList.Clear();
        this.mEffectList.Clear();
        Transform transform = base.transform.FindChild("InfoPanel/Equipment");
        for (int i = 0; i < 6; i++)
        {
            Transform item = transform.FindChild(i.ToString());
            this.mEquipGirdList.Add(item);
            Transform transform3 = item.FindChild("Effect");
            this.mEffectList.Add(transform3);
        }
        this.SetArrowPostion();
    }

    public void OnKillPanel()
    {
        this._Menu1.transform.FindChild("Border").gameObject.SetActive(true);
        this._Menu2.gameObject.SetActive(false);
        this._Menu3.gameObject.SetActive(false);
        this._Menu4.gameObject.SetActive(false);
        this._BreakGroup.gameObject.SetActive(true);
        this._CombiningGroup.gameObject.SetActive(false);
        this._FromGroup.gameObject.SetActive(false);
        GUIMgr.Instance.PopGUIEntity();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public void OpenJingHuaPanel()
    {
        this._EquipLvPanel.gameObject.SetActive(false);
        this._SubPanel.gameObject.SetActive(true);
        this._JingHuaTab.value = true;
    }

    [DebuggerHidden]
    public IEnumerator OpenSubGroup(int openType)
    {
        return new <OpenSubGroup>c__Iterator58 { openType = openType, <$>openType = openType, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator PlayEffect(int _part)
    {
        return new <PlayEffect>c__Iterator56 { _part = _part, <$>_part = _part, <>f__this = this };
    }

    public void PopAddText(List<string> _infoList)
    {
        base.StartCoroutine(this.PushText(_infoList));
    }

    [DebuggerHidden]
    private IEnumerator PopEquipTips(Transform _ItemTf, bool isBreak)
    {
        return new <PopEquipTips>c__Iterator57 { _ItemTf = _ItemTf, isBreak = isBreak, <$>_ItemTf = _ItemTf, <$>isBreak = isBreak, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator PushText(List<string> _infoList)
    {
        return new <PushText>c__Iterator55 { _infoList = _infoList, <$>_infoList = _infoList };
    }

    public void ResetDupJumpStat(DupReturnPrePanelPara _papa)
    {
        this.SetCurrShowCardList(_papa.heroInfoShowCardList);
        this.UpdateData(_papa.heroInfoPanelCardInfo, _papa.EquipPartIdx, _papa.heroInfoPanelCardInfo.equipInfo[_papa.EquipPartIdx]);
        item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(_papa.materialEntry);
        if (ic != null)
        {
            if (ic.param_0 < 0)
            {
                this.SetItemFromInfo(_papa.materialEntry, ic);
                this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
                this._Menu3.transform.localPosition = this._Menu2.transform.localPosition;
                this._Menu3.gameObject.SetActive(true);
                this._BreakGroup.gameObject.SetActive(false);
                this._CombiningGroup.gameObject.SetActive(false);
                this._FromGroup.gameObject.SetActive(true);
            }
            else
            {
                if (_papa.secondMaterialEntry > 0)
                {
                    GUIDataHolder.setData(this._Menu2.gameObject, _papa.oneMaterialEntry);
                    UIEventListener.Get(this._Menu2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMenu2Tab);
                    GUIDataHolder.setData(this._Menu3.gameObject, _papa.secondMaterialEntry);
                    UIEventListener.Get(this._Menu3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMenu3Tab);
                    this.SetMenuInfo(this._Menu2.transform, _papa.oneMaterialEntry);
                    this.SetMenuInfo(this._Menu3.transform, _papa.secondMaterialEntry);
                    this.SetMenuInfo(this._Menu4.transform, _papa.materialEntry);
                    this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu3.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.gameObject.SetActive(true);
                    this._Menu3.gameObject.SetActive(true);
                    this._Menu4.gameObject.SetActive(true);
                }
                else if (_papa.oneMaterialEntry > 0)
                {
                    GUIDataHolder.setData(this._Menu2.gameObject, _papa.oneMaterialEntry);
                    UIEventListener.Get(this._Menu2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMenu2Tab);
                    this.SetMenuInfo(this._Menu2.transform, _papa.oneMaterialEntry);
                    this.SetMenuInfo(this._Menu3.transform, _papa.materialEntry);
                    this._Menu1.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.transform.FindChild("Border").gameObject.SetActive(false);
                    this._Menu2.gameObject.SetActive(true);
                    this._Menu3.gameObject.SetActive(true);
                    this._Menu3.transform.localPosition = new Vector3(225.7f, this._Menu3.transform.localPosition.y, this._Menu3.transform.localPosition.z);
                }
                this.SetItemFromInfo(_papa.materialEntry, ic);
                this._BreakGroup.gameObject.SetActive(false);
                this._CombiningGroup.gameObject.SetActive(false);
                this._FromGroup.gameObject.SetActive(true);
            }
            this._SubPanel.gameObject.SetActive(true);
            this._EquipLvPanel.gameObject.SetActive(false);
            this._JingHuaTab.value = true;
            this._ShengJiTab.value = false;
        }
    }

    private void ReturnPrePage()
    {
        if (this._Menu4.gameObject.activeSelf)
        {
            this.OnClickMenu3Tab(this._Menu3.gameObject);
        }
        else if (this._Menu3.gameObject.activeSelf)
        {
            if (GUIDataHolder.getData(this._Menu3) != null)
            {
                this.OnClickMenu2Tab(this._Menu2.gameObject);
            }
            else
            {
                this.OnClickMenu1();
            }
        }
        else
        {
            this.OnClickMenu1();
        }
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

    private void SetCardInfo(Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                base.transform.FindChild("InfoPanel/Info/Name").GetComponent<UILabel>().text = _config.name;
                base.transform.FindChild("InfoPanel/Info/Level").GetComponent<UILabel>().text = "Lv. " + _card.cardInfo.level;
                UITexture component = base.transform.FindChild("InfoPanel/Head/Icon").GetComponent<UITexture>();
                component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.image);
                component.alpha = 1f;
                CommonFunc.SetQualityBorder(base.transform.FindChild("InfoPanel/Head/QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                Transform transform = base.transform.FindChild("InfoPanel/Head/Star");
                for (int i = 0; i < 5; i++)
                {
                    UISprite sprite2 = transform.FindChild(string.Empty + (i + 1)).GetComponent<UISprite>();
                    sprite2.gameObject.SetActive(i < _card.cardInfo.starLv);
                    sprite2.transform.localPosition = new Vector3((float) (i * 20), 0f, 0f);
                }
                transform.localPosition = new Vector3(-10f - ((_card.cardInfo.starLv - 1) * 10f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
            }
        }
    }

    private void SetCombiningGroupInfo(int entry)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config != null)
        {
            this._CombiningGroup.transform.FindChild("ShuXin/Name").GetComponent<UILabel>().text = _config.name;
            this._CombiningGroup.transform.FindChild("ShuXin/Desc").GetComponent<UILabel>().text = _config.describe;
            UILabel component = this._CombiningGroup.transform.FindChild("Cost/CostNum").GetComponent<UILabel>();
            if (_config.combine_price > 0)
            {
                component.text = _config.combine_price.ToString();
            }
            Debug.Log(entry + "----" + _config.name);
            Transform transform = this._CombiningGroup.transform.FindChild("Material/Item");
            Transform transform2 = this._CombiningGroup.transform.FindChild("Material/Item2");
            Transform transform3 = this._CombiningGroup.transform.FindChild("Material/Item3");
            transform.gameObject.SetActive(false);
            transform2.gameObject.SetActive(false);
            transform3.gameObject.SetActive(false);
            if (_config.type == 2)
            {
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_config.param_0);
                Debug.Log("----------------->" + _config.param_0);
                this.SetCombMaterialItemInfo(transform, _config2.param_0, _config2.param_1, entry);
            }
            else
            {
                if (_config.param_0 != -1)
                {
                    this.SetCombMaterialItemInfo(transform, _config.param_0, _config.param_1, entry);
                }
                if (_config.param_2 != -1)
                {
                    this.SetCombMaterialItemInfo(transform2, _config.param_2, _config.param_3, entry);
                }
                if (_config.param_4 != -1)
                {
                    this.SetCombMaterialItemInfo(transform3, _config.param_4, _config.param_5, entry);
                }
            }
        }
    }

    private void SetCombMaterialItemInfo(Transform _item, int intEntry, int _NeedCount, int _TargetEntry)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(intEntry);
        if (_config != null)
        {
            UITexture component = _item.FindChild("Icon").GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            component.alpha = 1f;
            CommonFunc.SetEquipQualityBorder(_item.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
            UILabel label = _item.FindChild("Count").GetComponent<UILabel>();
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(intEntry);
            int num = (itemByEntry != null) ? itemByEntry.num : 0;
            string str = (num >= _NeedCount) ? GameConstant.DefaultTextColor : GameConstant.DefaultTextRedColor;
            object[] objArray1 = new object[] { GameConstant.DefaultTextColor, num, "/", _NeedCount };
            label.text = string.Concat(objArray1);
            if (num < _NeedCount)
            {
                this.mTipsCombMaterialTf = _item;
            }
            Debug.Log(string.Concat(new object[] { " Entry: ", _config.entry, " Type:  ", _config.type, "  Name : ", _config.name }));
            _item.FindChild("FragSprite").gameObject.SetActive(_config.type == 2);
            GUIDataHolder.setData(_item.gameObject, intEntry);
            if (_config.type == 2)
            {
                UIEventListener.Get(_item.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombiningMaterialItem);
            }
            else
            {
                UIEventListener.Get(_item.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombiningMaterialItem2);
            }
            TempData data = new TempData {
                TargetEntry = _TargetEntry,
                ItemCount = num,
                NeedCount = _NeedCount,
                mIsPart = _config.type == 2
            };
            Transform transform = this._CombiningGroup.transform.FindChild("OkBtn");
            GUIDataHolder.setData(transform.gameObject, data);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCombiningBtn);
            _item.gameObject.SetActive(true);
        }
    }

    public void SetCurrShowCardList(List<Card> _CurrShowCardList)
    {
        this.mCurrShowCardList = _CurrShowCardList;
    }

    private void SetDropInfo(GameObject obj, DropFromType _type, int _pram1, int _pram2)
    {
        UISprite component = obj.transform.FindChild("Background").GetComponent<UISprite>();
        UISprite sprite2 = obj.transform.FindChild("Type").GetComponent<UISprite>();
        UILabel label = obj.transform.FindChild("IsOpen").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("Head/Icon").GetComponent<UITexture>();
        bool flag = false;
        UILabel label2 = obj.transform.FindChild("Label").GetComponent<UILabel>();
        switch (_type)
        {
            case DropFromType.E_NORMAL_DUPLICATE:
            {
                duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config != null)
                {
                    trench_normal_config _config2 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(_pram2);
                    if (_config2 != null)
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.monster_picture);
                        label2.text = _config.name + "-" + _config2.name;
                        flag = (ActorData.getInstance().NormalProgress >= _pram2) && (ActorData.getInstance().Level >= _config.unlock_lv);
                        sprite2.spriteName = "Ui_Heroinfo_Label_pt";
                        sprite2.gameObject.SetActive(true);
                        if (flag)
                        {
                            MapData data = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Normal
                            };
                            GUIDataHolder.setData(obj, data);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                    }
                }
                break;
            }
            case DropFromType.E_ELITE_DUPLICATE:
            {
                duplicate_config _config3 = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config3 != null)
                {
                    trench_elite_config _config4 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(_pram2);
                    if (_config4 != null)
                    {
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config4.monster_picture);
                        label2.text = _config3.name + "-" + _config4.name;
                        sprite2.spriteName = "Ui_Heroinfo_Label_jy";
                        sprite2.gameObject.SetActive(true);
                        flag = (ActorData.getInstance().EliteProgress >= _pram2) && (ActorData.getInstance().Level >= _config3.unlock_lv);
                        if (flag)
                        {
                            MapData data2 = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Elite
                            };
                            GUIDataHolder.setData(obj, data2);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                    }
                }
                break;
            }
            case DropFromType.E_WORLDBOSS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_boss;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b0);
                break;

            case DropFromType.E_WARMMATCH:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_cup;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b2);
                UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToWorldCup);
                break;

            case DropFromType.E_VOIDTOWER:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().void_tower;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b3);
                UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToTower);
                break;

            case DropFromType.E_DUNGEONS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().dungeons;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b7);
                break;

            case DropFromType.E_OPENBOX:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().item_box;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b4);
                break;

            case DropFromType.E_SHOP:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b5);
                break;

            case DropFromType.E_GUILDSHOP:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b6);
                break;

            case DropFromType.E_LOTTERY:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(440);
                break;

            default:
                label2.text = string.Empty;
                break;
        }
        if (flag)
        {
            label.text = ConfigMgr.getInstance().GetWord(220);
            label.color = (Color) new Color32(0, 150, 0x1a, 0xff);
        }
        else
        {
            label.text = ConfigMgr.getInstance().GetWord(0xdd);
        }
        nguiTextureGrey.doChangeEnableGrey(texture, !flag);
    }

    private void SetEquipGroupInfo(Card _card)
    {
        if (_card != null)
        {
            Transform transform = base.transform.FindChild("InfoPanel/Equipment");
            this._ShengJiTab.transform.FindChild("New").gameObject.SetActive(false);
            this._JingHuaTab.transform.FindChild("New").gameObject.SetActive(false);
            for (int i = 0; i < 6; i++)
            {
                Transform transform2 = this.mEquipGirdList[i];
                EquipInfo data = _card.equipInfo[i];
                GUIDataHolder.setData(transform2.gameObject, data);
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEquipBtn);
                this.SetSingleEquipInfo(data);
                if (i == this.mPartIdx)
                {
                    transform2.GetComponent<UIToggle>().value = true;
                    this.OnClickEquipBtn(transform2.gameObject);
                }
            }
        }
    }

    private void SetEquipInfo(EquipInfo _info)
    {
        <SetEquipInfo>c__AnonStorey179 storey = new <SetEquipInfo>c__AnonStorey179();
        if (_info != null)
        {
            this.mPartIdx = _info.part;
            this.UpdateEquipLvPanelInfo(_info);
            storey.bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(_info.entry);
            if (storey.bec != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_info.entry);
                if (_config != null)
                {
                    base.transform.FindChild("InfoPanel/Desc/Label").GetComponent<UILabel>().text = _config.describe;
                    List<string> itemAttributeDesc = CommonFunc.GetItemAttributeDesc(_info);
                    for (int i = 0; i < 6; i++)
                    {
                        UILabel label2 = base.transform.FindChild("InfoPanel/ShuXin/Label" + (i + 1)).GetComponent<UILabel>();
                        if (i < itemAttributeDesc.Count)
                        {
                            label2.text = itemAttributeDesc[i];
                        }
                        else
                        {
                            label2.text = string.Empty;
                        }
                    }
                    UITexture component = base.transform.FindChild("SubPanel/TopGroup/Menu1/Icon").GetComponent<UITexture>();
                    UISprite sprite = base.transform.FindChild("SubPanel/TopGroup/Menu1/QualityBorder").GetComponent<UISprite>();
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(storey.bec.break_equip_entry);
                    if (_config2 == null)
                    {
                        this._BreakGroup.transform.FindChild("Material").gameObject.SetActive(false);
                        this._BreakGroup.transform.FindChild("ShuXin").gameObject.SetActive(false);
                        this._BreakGroup.transform.FindChild("DontBreakTips").gameObject.SetActive(true);
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                        CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                    }
                    else
                    {
                        this._BreakGroup.transform.FindChild("Material").gameObject.SetActive(true);
                        this._BreakGroup.transform.FindChild("ShuXin").gameObject.SetActive(true);
                        this._BreakGroup.transform.FindChild("DontBreakTips").gameObject.SetActive(false);
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        component.alpha = 1f;
                        CommonFunc.SetEquipQualityBorder(sprite, _config2.quality, false);
                        this.SetMaterialItem(0, storey.bec.need_item_1, storey.bec.need_num_1);
                        this.SetMaterialItem(1, storey.bec.need_item_2, storey.bec.need_num_2);
                        this.SetMaterialItem(2, storey.bec.need_item_3, storey.bec.need_num_3);
                        this._BreakGroup.transform.FindChild("Material/Cost/CostNum").GetComponent<UILabel>().text = storey.bec.cost_gold.ToString();
                        this._BreakGroup.transform.FindChild("ShuXin/Name").GetComponent<UILabel>().text = _config2.name;
                        this.mCanBreak = true;
                        if (storey.bec.need_item_1 >= 0)
                        {
                            this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storey.<>m__1C0));
                            if (!this.mCanBreak)
                            {
                                this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item1");
                            }
                        }
                        if (this.mCanBreak && (storey.bec.need_item_2 >= 0))
                        {
                            this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storey.<>m__1C1));
                            if (!this.mCanBreak)
                            {
                                this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item2");
                            }
                        }
                        if (this.mCanBreak && (storey.bec.need_item_3 >= 0))
                        {
                            this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storey.<>m__1C2));
                            if (!this.mCanBreak)
                            {
                                this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item3");
                            }
                        }
                        Transform transform = base.transform.FindChild("SubPanel/BreakGroup/ZhuangBei");
                        Transform transform2 = base.transform.FindChild("SubPanel/BreakGroup/Material");
                        UIEventListener.Get(base.transform.FindChild("SubPanel/BreakGroup/Material/OkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickBreak);
                        UILabel label5 = base.transform.FindChild("SubPanel/BreakGroup/ZhuangBei/Count").GetComponent<UILabel>();
                        Item itemByEntry = ActorData.getInstance().GetItemByEntry(storey.bec.entry);
                        if (itemByEntry != null)
                        {
                            label5.text = itemByEntry.num.ToString();
                            if (itemByEntry.num > 0)
                            {
                                UIEventListener.Get(base.transform.FindChild("SubPanel/BreakGroup/ZhuangBei/PutOnBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPutOn);
                                this.mUseItemEntry = itemByEntry.entry;
                            }
                            transform.gameObject.SetActive(itemByEntry.num > 0);
                            transform2.gameObject.SetActive(itemByEntry.num <= 0);
                        }
                        List<string> list2 = CommonFunc.GetItemAttributeDesc(storey.bec.break_equip_entry, _info.lv);
                        for (int j = 0; j < 6; j++)
                        {
                            UILabel label6 = base.transform.FindChild("SubPanel/BreakGroup/ShuXin/Label" + (j + 1)).GetComponent<UILabel>();
                            if (j < list2.Count)
                            {
                                label6.text = list2[j];
                            }
                            else
                            {
                                label6.text = string.Empty;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetItemDetails(item_config ic)
    {
        if (ic != null)
        {
            this.mMaterialEntry = ic.entry;
            char[] separator = new char[] { '|' };
            string[] strArray = ic.drop_from_type.Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = ic.drop_parm_0.Split(chArray2);
            char[] chArray3 = new char[] { '|' };
            string[] strArray3 = ic.drop_parm_1.Split(chArray3);
            if (strArray.Length == strArray2.Length)
            {
                UIPanel component = this._FromGroup.transform.FindChild("List").GetComponent<UIPanel>();
                component.transform.localPosition = Vector3.zero;
                component.clipOffset = Vector3.zero;
                UIGrid grid = this._FromGroup.transform.FindChild("List/Grid").GetComponent<UIGrid>();
                CommonFunc.DeleteChildItem(grid.transform);
                int index = 0;
                float y = 0f;
                foreach (string str in strArray)
                {
                    if (str != string.Empty)
                    {
                        GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleFromItem) as GameObject;
                        obj2.transform.parent = grid.transform;
                        obj2.transform.localPosition = new Vector3(0f, y, -0.1f);
                        obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                        y -= grid.cellHeight;
                        DropFromType type = (DropFromType) int.Parse(str);
                        int num4 = int.Parse(strArray2[index]);
                        int num5 = int.Parse(strArray3[index]);
                        Transform transform = obj2.transform.FindChild("Item");
                        this.SetDropInfo(transform.gameObject, type, num4, num5);
                        index++;
                    }
                }
                grid.repositionNow = true;
            }
        }
    }

    private void SetItemFromInfo(int entry, item_config ic)
    {
        if (ic != null)
        {
            UITexture component = this._Menu3.transform.FindChild("Icon").GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
            component.alpha = 1f;
            CommonFunc.SetEquipQualityBorder(this._Menu3.transform.FindChild("QualityBorder").GetComponent<UISprite>(), ic.quality, false);
            this._FromGroup.transform.FindChild("Info/Name").GetComponent<UILabel>().text = ic.name;
            this.SetItemDetails(ic);
        }
    }

    private void SetMaterialItem(int idx, int entry, int needCount)
    {
        Transform transform = base.transform.FindChild("SubPanel/BreakGroup/Material/Item" + (idx + 1));
        transform.gameObject.SetActive(entry > -1);
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config != null)
        {
            UITexture component = transform.transform.FindChild("Icon").GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            component.alpha = 1f;
            CommonFunc.SetEquipQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
            UILabel label = transform.transform.FindChild("Count").GetComponent<UILabel>();
            Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(entry);
            int num = (itemByEntry != null) ? itemByEntry.num : 0;
            string str = (num >= needCount) ? GameConstant.DefaultTextColor : GameConstant.DefaultTextRedColor;
            object[] objArray1 = new object[] { str, num, GameConstant.DefaultTextColor, "/", needCount };
            label.text = string.Concat(objArray1);
            GUIDataHolder.setData(transform.gameObject, _config.entry);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickMaterialItem);
            Debug.Log(entry + " ::::: " + needCount);
        }
    }

    private void SetMenuInfo(Transform _Menu, int itemEntry)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemEntry);
        if (_config != null)
        {
            UITexture component = _Menu.transform.FindChild("Icon").GetComponent<UITexture>();
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            component.alpha = 1f;
            CommonFunc.SetEquipQualityBorder(_Menu.transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
            _Menu.gameObject.SetActive(true);
            _Menu.FindChild("FragSprite").gameObject.SetActive(_config.type == 2);
        }
        else
        {
            Debug.Log("Item Cfg error :" + itemEntry);
        }
        this._EquipTips.gameObject.SetActive(false);
    }

    private void SetSingleEquipInfo(EquipInfo info)
    {
        if ((info.part >= 0) && (info.part < this.mEquipGirdList.Count))
        {
            Transform transform = this.mEquipGirdList[info.part];
            GUIDataHolder.setData(transform.gameObject, info);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                UITexture component = transform.FindChild("Icon").GetComponent<UITexture>();
                component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                component.alpha = 1f;
                CommonFunc.SetEquipQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                transform.FindChild("LvUpTip").GetComponent<UISprite>().gameObject.SetActive((this.mCard.cardInfo.level - info.lv) >= 5);
                UISprite sprite3 = transform.FindChild("Up").GetComponent<UISprite>();
                transform.FindChild("Level").GetComponent<UILabel>().text = info.lv.ToString();
                if (_config.quality > (this.mCard.cardInfo.quality + 1))
                {
                    sprite3.gameObject.SetActive(false);
                }
                else
                {
                    break_equip_config _config2 = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                    if (_config2 != null)
                    {
                        if (_config2.break_equip_entry < 0)
                        {
                            sprite3.gameObject.SetActive(false);
                        }
                        else
                        {
                            TweenAlpha alpha = sprite3.GetComponent<TweenAlpha>();
                            if (CommonFunc.CheckMaterialEnough(_config2.equip_entry) && (_config.quality < (this.mCard.cardInfo.quality + 1)))
                            {
                                sprite3.spriteName = "Ui_Heroinfo_Icon_arrow";
                                sprite3.gameObject.SetActive(true);
                            }
                            else
                            {
                                sprite3.spriteName = "Ui_Heroinfo_Icon_arrowgrey";
                                sprite3.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    private void ShowOneLayer()
    {
        this._Menu2.gameObject.SetActive(false);
        this._Menu3.gameObject.SetActive(false);
        this._Menu4.gameObject.SetActive(false);
        this._FromGroup.gameObject.SetActive(false);
        this._CombiningGroup.gameObject.SetActive(false);
        this._BreakGroup.gameObject.SetActive(true);
    }

    public void SmashReturnEvent()
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mMaterialEntry);
        if (_config != null)
        {
            if (_config.param_0 < 0)
            {
                this.UpdateMenu1Material();
                if (this.mCanBreak)
                {
                    this.OnClickMenu1();
                }
            }
            else if (this.mSecondMatialEntry > 0)
            {
                if (this.CheckCanCombByItemEntry(this.mSecondMatialEntry))
                {
                    this.OnClickMenu3Tab(this._Menu3.gameObject);
                }
            }
            else if ((this.mOneMatialEntry > 0) && this.CheckCanCombByItemEntry(this.mOneMatialEntry))
            {
                this.OnClickMenu2Tab(this._Menu2.gameObject);
            }
        }
    }

    public void UpdateCardInfo(long _CardId)
    {
        if ((this.mCard != null) && (_CardId == this.mCard.card_id))
        {
            this.mCard = ActorData.getInstance().GetCardByID(this.mCard.card_id);
            this.UpdateData(this.mCard, this.mPartIdx, this.mCard.equipInfo[this.mPartIdx]);
        }
    }

    public void UpdateData(Card _card, int partIdx, EquipInfo _info)
    {
        this.mCard = _card;
        if (_info != null)
        {
            this.mCardId = _card.card_id;
            this.mPartIdx = partIdx;
            this.SetEquipInfo(_info);
            this.SetEquipGroupInfo(_card);
            this.SetCardInfo(_card);
        }
    }

    public void UpdateEquipLvPanelInfo(EquipInfo _info)
    {
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.UpdateEquipData(_info, this.mCard.cardInfo.level);
            this.mSingleEquipLevUpPanel.SetAutoCostInfo(_info, this.mCard.cardInfo.level);
            this.mSingleEquipLevUpPanel.SetOneKeyCostInfo(this.mCard);
        }
    }

    public void UpdateLevUp(Card _newCard, bool _auto, bool _bGoldNotEnough, int _cost)
    {
        List<string> addStr = this.GetAddStr(_newCard.equipInfo[this.mPartIdx]);
        base.StartCoroutine(this.PushText(addStr));
        base.StartCoroutine(this.PlayEffect(this.mPartIdx));
        this.mIsChanged = true;
        this.UpdateData(_newCard, this.mPartIdx, _newCard.equipInfo[this.mPartIdx]);
        this.UpdateShowCardByEntry(_newCard);
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.UpdateLevUp(_newCard.equipInfo[this.mPartIdx], this.mCard.cardInfo.level);
        }
    }

    private void UpdateMenu1Material()
    {
        <UpdateMenu1Material>c__AnonStorey17B storeyb = new <UpdateMenu1Material>c__AnonStorey17B();
        if (this.mCard != null)
        {
            EquipInfo info = this.mCard.equipInfo[this.mPartIdx];
            storeyb.bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
            if (storeyb.bec != null)
            {
                this.SetMaterialItem(0, storeyb.bec.need_item_1, storeyb.bec.need_num_1);
                this.SetMaterialItem(1, storeyb.bec.need_item_2, storeyb.bec.need_num_2);
                this.SetMaterialItem(2, storeyb.bec.need_item_3, storeyb.bec.need_num_3);
                this.mCanBreak = true;
                if (storeyb.bec.need_item_1 >= 0)
                {
                    this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyb.<>m__1C5));
                    if (!this.mCanBreak)
                    {
                        this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item1");
                    }
                }
                if (this.mCanBreak && (storeyb.bec.need_item_2 >= 0))
                {
                    this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyb.<>m__1C6));
                    if (!this.mCanBreak)
                    {
                        this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item2");
                    }
                }
                if (this.mCanBreak && (storeyb.bec.need_item_3 >= 0))
                {
                    this.mCanBreak &= XSingleton<UserItemPackageMgr>.Singleton.Exists(new UserItemPackageMgr.ForeachCondition(storeyb.<>m__1C7));
                    if (!this.mCanBreak)
                    {
                        this.mTipsMaterialTf = this._BreakGroup.transform.FindChild("Material/Item3");
                    }
                }
            }
        }
    }

    public void UpdateOneKeyLevUp(Card _newCard, int _cost)
    {
        List<string> allAddStr = this.GetAllAddStr(_newCard.equipInfo, this.mCard.equipInfo);
        base.StartCoroutine(this.PushText(allAddStr));
        base.StartCoroutine(this.PlayEffect(-1));
        this.UpdateData(_newCard, this.mPartIdx, _newCard.equipInfo[this.mPartIdx]);
        this.UpdateShowCardByEntry(_newCard);
        if (this.mSingleEquipLevUpPanel != null)
        {
            this.mSingleEquipLevUpPanel.UpdateLevUp(_newCard.equipInfo[this.mPartIdx], this.mCard.cardInfo.level);
            this.mSingleEquipLevUpPanel.SetOneKeyCostInfo(_newCard);
        }
    }

    private void UpdateOtherHeroPenal()
    {
        if (this.mCard != null)
        {
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.LevelUpEquipSucess(this.mCard);
            }
        }
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
    private sealed class <IsBackReturn>c__AnonStorey17C
    {
        internal break_equip_config bec;

        internal bool <>m__1C8(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_1) && (e.Item.num >= this.bec.need_num_1));
        }

        internal bool <>m__1C9(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_2) && (e.Item.num >= this.bec.need_num_2));
        }

        internal bool <>m__1CA(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_3) && (e.Item.num >= this.bec.need_num_3));
        }
    }

    [CompilerGenerated]
    private sealed class <IsBackReturn>c__AnonStorey17D
    {
        internal item_config ic_target;

        internal bool <>m__1CB(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.ic_target.param_0) && (e.Item.num >= this.ic_target.param_1));
        }
    }

    [CompilerGenerated]
    private sealed class <IsBackReturn>c__AnonStorey17E
    {
        internal item_config ic;

        internal bool <>m__1CC(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.ic.param_0) && (e.Item.num >= this.ic.param_1));
        }

        internal bool <>m__1CD(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.ic.param_2) && (e.Item.num >= this.ic.param_3));
        }

        internal bool <>m__1CE(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.ic.param_4) && (e.Item.num >= this.ic.param_5));
        }
    }

    [CompilerGenerated]
    private sealed class <JumpToDuplicate>c__AnonStorey17A
    {
        internal BreakEquipPanel <>f__this;
        internal MapData info;

        internal void <>m__1C4(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.OpenTypeIsPush = true;
            ActorData.getInstance().CurDupEntry = this.info.entry;
            ActorData.getInstance().CurTrenchEntry = this.info.subEntry;
            ActorData.getInstance().CurDupType = this.info.type;
            panel.UpdateData(this.info.entry, this.info.subEntry, this.info.type);
            panel.SetCloseCallBack(new UIEventListener.VoidDelegate(this.<>f__this.CloseDupPanelEvent));
            DupReturnPrePanelPara para = new DupReturnPrePanelPara {
                enterDuptype = EnterDupType.From_EquipBreak,
                EquipPartIdx = this.<>f__this.mPartIdx,
                heroInfoPanelCardInfo = this.<>f__this.mCard,
                heroInfoShowCardList = this.<>f__this.mCurrShowCardList,
                materialEntry = this.<>f__this.mMaterialEntry,
                oneMaterialEntry = this.<>f__this.mOneMatialEntry,
                secondMaterialEntry = this.<>f__this.mSecondMatialEntry
            };
            ActorData.getInstance().mCurrDupReturnPrePara = para;
        }
    }

    [CompilerGenerated]
    private sealed class <OnDeSerialization>c__AnonStorey178
    {
        internal BreakEquipPanel <>f__this;
        internal Transform tns;

        internal void <>m__1BF()
        {
            this.<>f__this._ShengJiTab.value = true;
            this.<>f__this._JingHuaTab.value = false;
            this.<>f__this._EquipLvPanel.gameObject.SetActive(true);
            this.<>f__this._SubPanel.gameObject.SetActive(false);
            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Strengthen.tag_equip_strengthen_function, this.tns.gameObject);
        }
    }

    [CompilerGenerated]
    private sealed class <OpenSubGroup>c__Iterator58 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>openType;
        internal BreakEquipPanel <>f__this;
        internal int openType;

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
                    switch (this.openType)
                    {
                        case 1:
                            TweenPosition.Begin(this.<>f__this._SubPanel, 0.2f, new Vector3(146.62f, 0f, 0f)).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._SubPanel, 0.1f, 1f);
                            TweenPosition.Begin(this.<>f__this._InfoPane, 0.2f, new Vector3(-225.65f, 0f, 0f)).method = UITweener.Method.Linear;
                            this.<>f__this.mIsOpenSubGroup = true;
                            goto Label_037D;

                        case 2:
                            TweenPosition.Begin(this.<>f__this._SubPanel, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._SubPanel, 0.1f, 0f);
                            TweenPosition.Begin(this.<>f__this._InfoPane, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            this.<>f__this.mIsOpenSubGroup = false;
                            goto Label_037D;

                        case 3:
                            TweenPosition.Begin(this.<>f__this._EquipLvPanel, 0.2f, new Vector3(146.62f, 0f, 0f)).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._EquipLvPanel, 0.1f, 1f);
                            TweenPosition.Begin(this.<>f__this._InfoPane, 0.2f, new Vector3(-225.65f, 0f, 0f)).method = UITweener.Method.Linear;
                            this.<>f__this.mIsOpenEquipLvGroup = true;
                            goto Label_037D;

                        case 4:
                            TweenPosition.Begin(this.<>f__this._EquipLvPanel, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._EquipLvPanel, 0.1f, 0f);
                            TweenPosition.Begin(this.<>f__this._InfoPane, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            this.<>f__this.mIsOpenEquipLvGroup = false;
                            goto Label_037D;

                        case 5:
                            TweenPosition.Begin(this.<>f__this._SubPanel, 0.2f, new Vector3(146.62f, 0f, 0f)).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._SubPanel, 0.1f, 1f);
                            TweenPosition.Begin(this.<>f__this._EquipLvPanel, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._EquipLvPanel, 0.1f, 0f);
                            this.<>f__this.mIsOpenSubGroup = true;
                            this.<>f__this.mIsOpenEquipLvGroup = false;
                            goto Label_037D;

                        case 6:
                            TweenPosition.Begin(this.<>f__this._EquipLvPanel, 0.2f, new Vector3(146.62f, 0f, 0f)).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._EquipLvPanel, 0.1f, 1f);
                            TweenPosition.Begin(this.<>f__this._SubPanel, 0.2f, Vector3.zero).method = UITweener.Method.Linear;
                            TweenAlpha.Begin(this.<>f__this._SubPanel, 0.1f, 0f);
                            this.<>f__this.mIsOpenSubGroup = false;
                            this.<>f__this.mIsOpenEquipLvGroup = true;
                            goto Label_037D;
                    }
                    break;

                case 1:
                    this.$PC = -1;
                    goto Label_0397;

                default:
                    goto Label_0397;
            }
        Label_037D:
            this.$current = null;
            this.$PC = 1;
            return true;
        Label_0397:
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
    private sealed class <PlayEffect>c__Iterator56 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int _part;
        internal int <$>_part;
        internal List<Transform>.Enumerator <$s_432>__1;
        private static Action<GameObject> <>f__am$cacheA;
        internal BreakEquipPanel <>f__this;
        internal List<GameObject> <EffList>__0;
        internal GameObject <EffObj>__3;
        internal GameObject <EffObj>__4;
        internal Transform <obj>__2;

        private static void <>m__1CF(GameObject obj)
        {
            obj.SetActive(false);
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
                    SoundManager.mInstance.PlaySFX("sound_wuqiup");
                    if (this._part != -1)
                    {
                        if (this._part < this.<>f__this.mEquipGirdList.Count)
                        {
                            this.<EffObj>__4 = this.<>f__this.mEquipGirdList[this._part].transform.FindChild("Effect").gameObject;
                            this.<EffObj>__4.SetActive(true);
                            this.$current = new WaitForSeconds(2f);
                            this.$PC = 2;
                            goto Label_01AC;
                        }
                        break;
                    }
                    this.<EffList>__0 = new List<GameObject>();
                    this.<$s_432>__1 = this.<>f__this.mEquipGirdList.GetEnumerator();
                    try
                    {
                        while (this.<$s_432>__1.MoveNext())
                        {
                            this.<obj>__2 = this.<$s_432>__1.Current;
                            this.<EffObj>__3 = this.<obj>__2.transform.FindChild("Effect").gameObject;
                            this.<EffObj>__3.SetActive(true);
                            this.<EffList>__0.Add(this.<EffObj>__3);
                        }
                    }
                    finally
                    {
                        this.<$s_432>__1.Dispose();
                    }
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    goto Label_01AC;

                case 1:
                    if (<>f__am$cacheA == null)
                    {
                        <>f__am$cacheA = new Action<GameObject>(BreakEquipPanel.<PlayEffect>c__Iterator56.<>m__1CF);
                    }
                    this.<EffList>__0.ForEach(<>f__am$cacheA);
                    break;

                case 2:
                    this.<EffObj>__4.SetActive(false);
                    break;

                default:
                    goto Label_01AA;
            }
            this.$PC = -1;
        Label_01AA:
            return false;
        Label_01AC:
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
    private sealed class <PopEquipTips>c__Iterator57 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Transform _ItemTf;
        internal Transform <$>_ItemTf;
        internal bool <$>isBreak;
        internal BreakEquipPanel <>f__this;
        internal UILabel <desc>__0;
        internal bool isBreak;

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
                    if (this._ItemTf != null)
                    {
                        this.<>f__this._EquipTips.transform.parent = this._ItemTf;
                        this.<desc>__0 = this.<>f__this._EquipTips.transform.FindChild("Desc").GetComponent<UILabel>();
                        this.<desc>__0.text = ConfigMgr.getInstance().GetWord(!this.isBreak ? 0x501 : 0x500);
                        this.<>f__this._EquipTips.transform.localPosition = Vector3.zero;
                        this.<>f__this._EquipTips.gameObject.SetActive(true);
                        this.$current = new WaitForSeconds(3f);
                        this.$PC = 1;
                        return true;
                    }
                    break;

                case 1:
                    this.<>f__this._EquipTips.gameObject.SetActive(false);
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
    private sealed class <PushText>c__Iterator55 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<string> _infoList;
        internal List<string> <$>_infoList;
        internal List<string>.Enumerator <$s_431>__0;
        internal string <text>__1;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_431>__0.Dispose();
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
                    this.<$s_431>__0 = this._infoList.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_00CA;

                default:
                    goto Label_00CA;
            }
            try
            {
                while (this.<$s_431>__0.MoveNext())
                {
                    this.<text>__1 = this.<$s_431>__0.Current;
                    TipsDiag.PushText(this.<text>__1);
                    this.$current = new WaitForSeconds(0.35f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_00CC;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_431>__0.Dispose();
            }
            this.$current = null;
            this.$PC = 2;
            goto Label_00CC;
        Label_00CA:
            return false;
        Label_00CC:
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
    private sealed class <SetEquipInfo>c__AnonStorey179
    {
        internal break_equip_config bec;

        internal bool <>m__1C0(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_1) && (e.Item.num >= this.bec.need_num_1));
        }

        internal bool <>m__1C1(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_2) && (e.Item.num >= this.bec.need_num_2));
        }

        internal bool <>m__1C2(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_3) && (e.Item.num >= this.bec.need_num_3));
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateMenu1Material>c__AnonStorey17B
    {
        internal break_equip_config bec;

        internal bool <>m__1C5(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_1) && (e.Item.num >= this.bec.need_num_1));
        }

        internal bool <>m__1C6(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_2) && (e.Item.num >= this.bec.need_num_2));
        }

        internal bool <>m__1C7(UserItemPackageMgr.UserItem e)
        {
            return ((e.Item.entry == this.bec.need_item_3) && (e.Item.num >= this.bec.need_num_3));
        }
    }

    private class TempData
    {
        public int ItemCount;
        public bool mIsPart;
        public int NeedCount;
        public int TargetEntry = -1;
    }
}

