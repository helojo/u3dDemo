using FastBuf;
using HutongGames.PlayMaker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class EquipLevUpPanel : GUIEntity
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache10;
    public bool CanClickBtn = true;
    private bool CanClose = true;
    public Card CurCard;
    private EquipInfo CurEquipInfo = new EquipInfo();
    private int DefaultSelPart;
    private List<GameObject> EquipBuffer = new List<GameObject>();
    public GameObject EquipIcon;
    public List<GameObject> EquipObjList = new List<GameObject>();
    private int fakeID;
    private float Interval = 0.25f;
    private Vector2 mPosition;
    private bool mRealClose;
    private Vector2 mScaleFactor = new Vector2(20f, 0f);
    private float mTime;
    private Queue<string> TipsMsgList = new Queue<string>();
    private Vector2 TipsPos = new Vector2(-280f, -95f);

    private bool CheckEqupMax(EquipInfo equ)
    {
        if (equ.lv < CommonFunc.GetMaxCardOrEquipLvByQuality(equ.quality))
        {
            return false;
        }
        return true;
    }

    private void ClickAutoLevUp()
    {
        if (this.CanClickBtn && (this.CurCard != null))
        {
            int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.AUTO_EQU_LEV_UP);
            if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
            }
            else
            {
                PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
                if (component != null)
                {
                    FsmInt num2 = component.FsmVariables.FindFsmInt("EquipPart");
                    EquipInfo equipInfo = this.GetEquipInfo(num2.Value);
                    if (equipInfo != null)
                    {
                        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(equipInfo.entry);
                        if (_config == null)
                        {
                            Debug.LogWarning("Item Cfg Is Null!");
                        }
                        else if (ActorData.getInstance().Gold < this.GetCost(_config.lv_up_gold, equipInfo.lv, equipInfo.quality))
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                        }
                        else if (equipInfo.lv >= this.CurCard.cardInfo.level)
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
                        }
                        else if (equipInfo.lv < ActorData.getInstance().Level)
                        {
                            if (equipInfo.lv < CommonFunc.GetMaxCardOrEquipLvByQuality(equipInfo.quality))
                            {
                                this.CanClickBtn = false;
                                this.EnablePanelClick(true);
                                SocketMgr.Instance.RequestEquipLvUp(this.CurCard.card_id, num2.Value, true);
                            }
                            else
                            {
                                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287e));
                            }
                        }
                        else
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
                        }
                    }
                }
            }
        }
    }

    private void ClickEquip()
    {
        if (this.CurCard != null)
        {
            PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
            if (component != null)
            {
                FsmInt num = component.FsmVariables.FindFsmInt("EquipPart");
                this.DefaultSelPart = num.Value;
                EquipInfo equipInfo = this.GetEquipInfo(num.Value);
                this.CurEquipInfo.entry = equipInfo.entry;
                this.CurEquipInfo.part = equipInfo.part;
                this.CurEquipInfo.lv = equipInfo.lv;
                this.CurEquipInfo.quality = equipInfo.quality;
                this.UpdateEquipData(equipInfo);
            }
        }
    }

    private void ClickLevUp()
    {
        if (this.CanClickBtn && (this.CurCard != null))
        {
            PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
            if (component != null)
            {
                FsmInt num = component.FsmVariables.FindFsmInt("EquipPart");
                EquipInfo equipInfo = this.GetEquipInfo(num.Value);
                if (equipInfo != null)
                {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(equipInfo.entry);
                    if (_config == null)
                    {
                        Debug.LogWarning("Item Cfg Is Null!");
                    }
                    else if (ActorData.getInstance().Gold < this.GetCost(_config.lv_up_gold, equipInfo.lv, equipInfo.quality))
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                    }
                    else if (equipInfo.lv >= this.CurCard.cardInfo.level)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
                    }
                    else if (equipInfo.lv < ActorData.getInstance().Level)
                    {
                        if (equipInfo.lv < CommonFunc.GetMaxCardOrEquipLvByQuality(equipInfo.quality))
                        {
                            this.CanClickBtn = false;
                            this.EnablePanelClick(true);
                            SocketMgr.Instance.RequestEquipLvUp(this.CurCard.card_id, num.Value, false);
                        }
                        else
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287e));
                        }
                    }
                    else
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
                    }
                }
            }
        }
    }

    private void ClickSelHero()
    {
        this.CloseAllEffect();
        GUIMgr.Instance.DoModelGUI("SelectPanel", delegate (GUIEntity obj) {
            SelectPanel panel = (SelectPanel) obj;
            panel.CloseEvent = () => this.WaitPanel();
            panel.SetDelegate(delegate (Card _card) {
                this.ShowSelBtn(false);
                this.UpdateData(_card);
                this.WaitPanel();
            });
        }, null);
    }

    private void CloseAllEffect()
    {
        foreach (GameObject obj2 in this.EquipObjList)
        {
            obj2.transform.FindChild("Effect").gameObject.SetActive(false);
        }
    }

    private void ClosePanel()
    {
        if (this.CanClose)
        {
            EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
            this.mRealClose = true;
            this.UpdateOtherHeroPenal();
            GUIMgr.Instance.ExitModelGUI("EquipLevUpPanel");
            GUIMgr.Instance.Lock();
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = () => GUIMgr.Instance.UnLock();
            }
            this.DelayCallBack(0.4f, <>f__am$cache10);
        }
    }

    public void EnableBtn(bool _enable)
    {
        base.gameObject.transform.FindChild("BtnGroup/AutoBtn").GetComponent<UIButton>().isEnabled = _enable;
        base.gameObject.transform.FindChild("BtnGroup/LevUpBtn").GetComponent<UIButton>().isEnabled = _enable;
        base.gameObject.transform.FindChild("BtnGroup/OneKeyLevUpBtn").GetComponent<UIButton>().isEnabled = _enable;
    }

    private void EnableCheckClick(bool _enable)
    {
        foreach (GameObject obj2 in this.EquipObjList)
        {
            obj2.GetComponent<Collider>().enabled = _enable;
        }
    }

    public void EnablePanelClick(bool _enable)
    {
        base.gameObject.transform.FindChild("Col").gameObject.SetActive(_enable);
    }

    public void EnableSelHeroBtn(bool _enable)
    {
        if (!_enable)
        {
            this.ShowSelBtn(false);
        }
    }

    private List<string> GetAddStr(EquipInfo _data)
    {
        if (_data == null)
        {
            this.CanClickBtn = true;
            this.EnablePanelClick(false);
            return null;
        }
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_data.entry);
        int num = _data.lv - this.CurEquipInfo.lv;
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
        if ((_newList == null) || (_newList.Count == 0))
        {
            this.EnableBtn(true);
            return null;
        }
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

    private int GetCost(int _base, int _Lev, int _quality)
    {
        return (_base + ((_Lev - 1) * this.GetQualityparam(_quality)));
    }

    private EquipInfo GetEquipInfo(int _part)
    {
        foreach (EquipInfo info2 in this.CurCard.equipInfo)
        {
            if (info2.part == _part)
            {
                return info2;
            }
        }
        return null;
    }

    private EquipInfo GetEquLevByPart(Card _card, int Part)
    {
        foreach (EquipInfo info in _card.equipInfo)
        {
            if (info.part == Part)
            {
                return info;
            }
        }
        return null;
    }

    private void GetMsgData(List<string> _list)
    {
        _list.ForEach(obj => this.TipsMsgList.Enqueue(obj));
    }

    private EquipInfo GetNewEquipData(Card _card)
    {
        if (_card != null)
        {
            foreach (EquipInfo info in _card.equipInfo)
            {
                if (info.part == this.CurEquipInfo.part)
                {
                    return info;
                }
            }
        }
        return null;
    }

    private int GetOneKeyCost()
    {
        int num = 0;
        foreach (EquipInfo info in this.CurCard.equipInfo)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                int num2 = (CommonFunc.GetMaxCardOrEquipLvByQuality(info.quality) >= this.CurCard.cardInfo.level) ? this.CurCard.cardInfo.level : CommonFunc.GetMaxCardOrEquipLvByQuality(info.quality);
                for (int i = info.lv; i < num2; i++)
                {
                    num += this.GetCost(_config.lv_up_gold, i, info.quality);
                }
            }
        }
        return num;
    }

    private int GetQualityparam(int _quality)
    {
        switch (((E_QualiytDef) _quality))
        {
            case E_QualiytDef.e_qualitydef_white:
                return GameConstValues.EQUIP_LV_UP_GROW_WHITE;

            case E_QualiytDef.e_qualitydef_green:
                return GameConstValues.EQUIP_LV_UP_GROW_GREEN;

            case E_QualiytDef.e_qualitydef_blue:
                return GameConstValues.EQUIP_LV_UP_GROW_BLUE;

            case E_QualiytDef.e_qualitydef_blue_1:
                return GameConstValues.EQUIP_LV_UP_GROW_BLUE_1;

            case E_QualiytDef.e_qualitydef_purple:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE;

            case E_QualiytDef.e_qualitydef_purple_1:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_1;

            case E_QualiytDef.e_qualitydef_purple_2:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_2;

            case E_QualiytDef.e_qualitydef_orange:
                return GameConstValues.EQUIP_LV_UP_GROW_ORANGE;
        }
        return 0;
    }

    private void HideAllCtrl(GameObject obj)
    {
        IEnumerator enumerator = obj.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                current.gameObject.SetActive(false);
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
    }

    private void On_Swipe(Gesture gesture)
    {
        if ((this.fakeID != -1) && (this.mPosition.x != gesture.position.x))
        {
            this.mPosition = gesture.position;
            Vector2 vector = (Vector2) (Vector2.Scale(gesture.deltaPosition, -this.mScaleFactor) / 100f);
            FakeCharacter.GetInstance().Roate(this.fakeID, vector.x);
        }
    }

    public override void OnDestroy()
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void OneKeyLevUp()
    {
        if (this.CanClickBtn && (this.CurCard != null))
        {
            int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.ONEKEY_EQU_LEV_UP);
            if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
            }
            else if (this.GetOneKeyCost() == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2abb));
            }
            else if (this.GetOneKeyCost() > ActorData.getInstance().Gold)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037b8));
            }
            else
            {
                this.CanClickBtn = false;
                this.EnablePanelClick(true);
                SocketMgr.Instance.RequestOneKeyEquipLevUp(this.CurCard.card_id);
            }
        }
    }

    public override void OnInitialize()
    {
        GUIMgr.Instance.FloatTitleBar();
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        this.EnableBtn(false);
        this.CanClickBtn = false;
        base.OnInitialize();
    }

    public override void OnRelease()
    {
        if (this.fakeID > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.fakeID);
        }
        base.OnRelease();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        if (this.mRealClose)
        {
            GUIMgr.Instance.CloseGUIEntity(base.entity_id);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.TipsMsgList.Count > 0)
        {
            this.mTime += Time.deltaTime;
            if (this.mTime >= this.Interval)
            {
                TipsDiag.PushText(this.TipsMsgList.Dequeue(), this.TipsPos);
                this.mTime = 0f;
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator PlayEffect(int _part)
    {
        return new <PlayEffect>c__Iterator5C { _part = _part, <$>_part = _part, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator PushText(List<string> _infoList)
    {
        return new <PushText>c__Iterator5B { _infoList = _infoList, <$>_infoList = _infoList };
    }

    private void ResetPanel()
    {
        if (this.fakeID > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.fakeID);
            this.fakeID = 0;
        }
        this.EnableBtn(false);
        this.CanClickBtn = false;
        this.DefaultSelPart = 0;
        this.CurCard = null;
        this.CurEquipInfo = new EquipInfo();
        foreach (GameObject obj2 in this.EquipBuffer)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        base.gameObject.transform.FindChild("Character/Label").GetComponent<UILabel>().text = string.Empty;
        base.gameObject.transform.FindChild("Character/Lv").GetComponent<UILabel>().text = string.Empty;
        base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>().text = string.Empty;
        base.gameObject.transform.FindChild("Desc/name").GetComponent<UILabel>().text = string.Empty;
        base.gameObject.transform.FindChild("Cost/Cost").GetComponent<UILabel>().text = string.Empty;
        base.gameObject.transform.FindChild("Desc/EquipItem").gameObject.SetActive(false);
        base.gameObject.transform.FindChild("Desc/Lev").gameObject.SetActive(false);
        GameObject gameObject = base.gameObject.transform.FindChild("EquipAddInfo/Scroll View/Grid").gameObject;
        this.HideAllCtrl(gameObject);
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            component.FsmVariables.FindFsmInt("EquipPart").Value = 0;
        }
    }

    public void SelectEquip(int _Part)
    {
        base.gameObject.transform.FindChild("Equipment/Part" + (_Part + 1)).GetComponent<UIToggle>().value = true;
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component != null)
        {
            component.FsmVariables.FindFsmInt("EquipPart").Value = _Part;
            this.DefaultSelPart = _Part;
            EquipInfo equipInfo = this.GetEquipInfo(_Part);
            this.CurEquipInfo.entry = equipInfo.entry;
            this.CurEquipInfo.part = equipInfo.part;
            this.CurEquipInfo.lv = equipInfo.lv;
            this.CurEquipInfo.quality = equipInfo.quality;
            this.UpdateEquipData(equipInfo);
        }
    }

    private void ShowSelBtn(bool _show)
    {
        base.gameObject.transform.FindChild("BtnGroup/SelHeroBtn/Background").gameObject.SetActive(_show);
    }

    [DebuggerHidden]
    private IEnumerator StartEquAutoLevUp(int UpLev, bool _bGoldNotEnough)
    {
        return new <StartEquAutoLevUp>c__Iterator5D { UpLev = UpLev, _bGoldNotEnough = _bGoldNotEnough, <$>UpLev = UpLev, <$>_bGoldNotEnough = _bGoldNotEnough, <>f__this = this };
    }

    private void UpdateCard()
    {
        base.transform.FindChild("Cost").gameObject.SetActive(true);
        base.transform.FindChild("OneKeyCost").gameObject.SetActive(true);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.CurCard.cardInfo.entry);
        base.gameObject.transform.FindChild("Character/Label").GetComponent<UILabel>().text = _config.name;
        base.gameObject.transform.FindChild("Character/Lv").GetComponent<UILabel>().text = "Lv." + this.CurCard.cardInfo.level;
        UITexture component = base.gameObject.transform.FindChild("Character/Texture").GetComponent<UITexture>();
        if (this.fakeID > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.fakeID);
        }
        this.fakeID = FakeCharacter.GetInstance().SnapCardCharacterWithEquip((int) this.CurCard.cardInfo.entry, this.CurCard.cardInfo.quality, this.CurCard.equipInfo, ModelViewType.normal, component);
    }

    private void UpdateCtrl(List<EquipAttribute> _DataList, EquipInfo _equinfo)
    {
        UIGrid component = base.gameObject.transform.FindChild("EquipAddInfo/Scroll View/Grid").GetComponent<UIGrid>();
        this.HideAllCtrl(component.gameObject);
        int num = 1;
        foreach (EquipAttribute attribute in _DataList)
        {
            GameObject gameObject = component.transform.FindChild("Add" + num).gameObject;
            this.UpdateEquipAddData(gameObject, attribute, _equinfo);
            gameObject.SetActive(true);
            num++;
        }
    }

    public void UpdateData(Card _card)
    {
        this.EnableCheckClick(true);
        this.CurCard = _card;
        this.UpdateCard();
        this.UpdateEquipInfo(0);
        this.EnableBtn(true);
        this.CanClickBtn = true;
        this.UpdateOneKeyCost();
    }

    private void UpdateEquipAddData(GameObject obj, EquipAttribute _data, EquipInfo _info)
    {
        UILabel component = obj.transform.FindChild("Label").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Val").GetComponent<UILabel>();
        UILabel label3 = obj.transform.FindChild("Add").GetComponent<UILabel>();
        label2.text = string.Empty + (_data.AddVal + ((this.CurEquipInfo.lv - 1) * _data.AddValGrow));
        if (this.CheckEqupMax(_info))
        {
            label3.text = "MAX";
        }
        else
        {
            label3.text = "+" + _data.AddValGrow;
        }
        switch (_data.Type)
        {
            case AttributeType.ATK:
                component.text = ConfigMgr.getInstance().GetWord(0x155);
                break;

            case AttributeType.HP:
                component.text = ConfigMgr.getInstance().GetWord(340);
                break;

            case AttributeType.DEF:
                component.text = ConfigMgr.getInstance().GetWord(0x15a);
                break;

            case AttributeType.SEF:
                component.text = ConfigMgr.getInstance().GetWord(0x15b);
                break;

            case AttributeType.CRIT:
                component.text = ConfigMgr.getInstance().GetWord(350);
                break;

            case AttributeType.DODGE:
                component.text = ConfigMgr.getInstance().GetWord(0x162);
                break;

            case AttributeType.TENACITY:
                component.text = ConfigMgr.getInstance().GetWord(0x160);
                break;

            case AttributeType.HIT:
                component.text = ConfigMgr.getInstance().GetWord(0x161);
                break;

            case AttributeType.HP_RECOVER:
                component.text = ConfigMgr.getInstance().GetWord(0x163);
                break;

            case AttributeType.ENERGY_RECOVER:
                component.text = ConfigMgr.getInstance().GetWord(0x164);
                break;
        }
    }

    private void UpdateEquipAddLev(GameObject obj, EquipInfo _data)
    {
        UILabel component = obj.transform.FindChild("Val").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Add").GetComponent<UILabel>();
        component.text = "LV " + _data.lv.ToString();
        if (this.CheckEqupMax(_data))
        {
            label2.text = "LV MAX";
        }
        else
        {
            label2.text = "LV " + ((_data.lv + 1)).ToString();
        }
    }

    private void UpdateEquipData(EquipInfo _equ)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_equ.entry);
        if (_config == null)
        {
            Debug.LogWarning("Item Cfg Is Null!");
        }
        else
        {
            base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>().text = _config.describe;
            object[] objArray1 = new object[] { _config.name, " [A89079](MAX LV ", CommonFunc.GetMaxCardOrEquipLvByQuality(_equ.quality), ")" };
            base.gameObject.transform.FindChild("Desc/name").GetComponent<UILabel>().text = string.Concat(objArray1);
            base.gameObject.transform.FindChild("Cost/Cost").GetComponent<UILabel>().text = this.GetCost(_config.lv_up_gold, _equ.lv, _equ.quality).ToString();
            GameObject gameObject = base.gameObject.transform.FindChild("Desc/EquipItem").gameObject;
            gameObject.SetActive(true);
            UITexture component = gameObject.transform.FindChild("Icon").GetComponent<UITexture>();
            CommonFunc.SetQualityColor(gameObject.transform.FindChild("frame").GetComponent<UISprite>(), _config.quality);
            component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
            List<EquipAttribute> list = new List<EquipAttribute>();
            if (_config.attack > 0)
            {
                EquipAttribute item = new EquipAttribute {
                    Type = AttributeType.ATK,
                    AddVal = _config.attack,
                    AddValGrow = _config.attack_grow
                };
                list.Add(item);
            }
            if (_config.hp > 0)
            {
                EquipAttribute attribute2 = new EquipAttribute {
                    Type = AttributeType.HP,
                    AddVal = _config.hp,
                    AddValGrow = _config.hp_grow
                };
                list.Add(attribute2);
            }
            if (_config.physics_defence > 0)
            {
                EquipAttribute attribute3 = new EquipAttribute {
                    Type = AttributeType.DEF,
                    AddVal = _config.physics_defence,
                    AddValGrow = _config.physics_defence_grow
                };
                list.Add(attribute3);
            }
            if (_config.spell_defence > 0)
            {
                EquipAttribute attribute4 = new EquipAttribute {
                    Type = AttributeType.SEF,
                    AddVal = _config.spell_defence,
                    AddValGrow = _config.spell_defence_grow
                };
                list.Add(attribute4);
            }
            if (_config.crit_level > 0)
            {
                EquipAttribute attribute5 = new EquipAttribute {
                    Type = AttributeType.CRIT,
                    AddVal = _config.crit_level,
                    AddValGrow = _config.crit_level_grow
                };
                list.Add(attribute5);
            }
            if (_config.dodge_level > 0)
            {
                EquipAttribute attribute6 = new EquipAttribute {
                    Type = AttributeType.DODGE,
                    AddVal = _config.dodge_level,
                    AddValGrow = _config.dodge_level_grow
                };
                list.Add(attribute6);
            }
            if (_config.tenacity_level > 0)
            {
                EquipAttribute attribute7 = new EquipAttribute {
                    Type = AttributeType.TENACITY,
                    AddVal = _config.tenacity_level,
                    AddValGrow = _config.tenacity_level_grow
                };
                list.Add(attribute7);
            }
            if (_config.hit_level > 0)
            {
                EquipAttribute attribute8 = new EquipAttribute {
                    Type = AttributeType.HIT,
                    AddVal = _config.hit_level,
                    AddValGrow = _config.hit_level_grow
                };
                list.Add(attribute8);
            }
            if (_config.hp_recover > 0)
            {
                EquipAttribute attribute9 = new EquipAttribute {
                    Type = AttributeType.HP_RECOVER,
                    AddVal = _config.hp_recover,
                    AddValGrow = _config.hp_recover_grow
                };
                list.Add(attribute9);
            }
            if (_config.energy_recover > 0)
            {
                EquipAttribute attribute10 = new EquipAttribute {
                    Type = AttributeType.ENERGY_RECOVER,
                    AddVal = _config.energy_recover,
                    AddValGrow = _config.energy_recover_grow
                };
                list.Add(attribute10);
            }
            this.UpdateCtrl(list, _equ);
            GameObject obj3 = base.gameObject.transform.FindChild("Desc/Lev").gameObject;
            obj3.SetActive(true);
            this.UpdateEquipAddLev(obj3, _equ);
        }
    }

    private void UpdateEquipInfo(int part)
    {
        foreach (EquipInfo info in this.CurCard.equipInfo)
        {
            if (info.part > 5)
            {
                Debug.LogWarning("part Error " + info.part);
                break;
            }
            GameObject obj2 = this.EquipObjList[info.part];
            Transform transform = obj2.transform.FindChild("EquipItem");
            GameObject item = null;
            if (transform == null)
            {
                item = UnityEngine.Object.Instantiate(this.EquipIcon) as GameObject;
                item.transform.parent = obj2.transform;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;
                item.name = "EquipItem";
                this.EquipBuffer.Add(item);
            }
            else
            {
                item = transform.gameObject;
            }
            UIToggle component = obj2.transform.GetComponent<UIToggle>();
            if (info.part == this.DefaultSelPart)
            {
                this.CurEquipInfo.entry = info.entry;
                this.CurEquipInfo.part = info.part;
                this.CurEquipInfo.lv = info.lv;
                this.CurEquipInfo.quality = info.quality;
                this.UpdateEquipData(info);
                component.value = true;
                PlayMakerFSM rfsm = base.transform.GetComponent<PlayMakerFSM>();
                if (rfsm != null)
                {
                    rfsm.FsmVariables.FindFsmInt("EquipPart").Value = this.DefaultSelPart;
                }
            }
            else
            {
                component.value = false;
            }
            item.transform.FindChild("Label").GetComponent<UILabel>().text = info.lv.ToString();
            UITexture texture = item.transform.FindChild("Icon").GetComponent<UITexture>();
            UISprite sprite = item.transform.FindChild("frame").GetComponent<UISprite>();
            if (this.CheckEqupMax(info))
            {
                item.transform.FindChild("Max").gameObject.SetActive(true);
            }
            else
            {
                item.transform.FindChild("Max").gameObject.SetActive(false);
            }
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                CommonFunc.SetQualityColor(sprite, _config.quality);
                if (_config != null)
                {
                    texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                }
            }
        }
    }

    public void UpdateLevUp(Card _newCard, bool _auto, bool _bGoldNotEnough, int _cost)
    {
        <UpdateLevUp>c__AnonStorey187 storey = new <UpdateLevUp>c__AnonStorey187 {
            _cost = _cost
        };
        this.EnablePanelClick(false);
        this.CanClickBtn = true;
        if (_auto)
        {
            int num = this.GetEquLevByPart(_newCard, this.DefaultSelPart).lv - this.GetEquLevByPart(this.CurCard, this.DefaultSelPart).lv;
            this.DelayCallBack(2f, new System.Action(storey.<>m__1DC));
        }
        this.CurCard = _newCard;
        EquipInfo newEquipData = this.GetNewEquipData(this.CurCard);
        List<string> addStr = this.GetAddStr(newEquipData);
        this.GetMsgData(addStr);
        this.UpdateEquipInfo(this.DefaultSelPart);
        base.StartCoroutine(this.PlayEffect(this.DefaultSelPart));
        this.UpdateOneKeyCost();
        TipsDiag.SetText("升级成功");
    }

    private void UpdateOneKeyCost()
    {
        base.gameObject.transform.FindChild("OneKeyCost/Cost").GetComponent<UILabel>().text = this.GetOneKeyCost().ToString();
    }

    public void UpdateOneKeyLevUp(Card _newCard, int _cost)
    {
        <UpdateOneKeyLevUp>c__AnonStorey186 storey = new <UpdateOneKeyLevUp>c__AnonStorey186 {
            _cost = _cost
        };
        this.EnablePanelClick(false);
        this.CanClickBtn = true;
        this.DelayCallBack(2f, new System.Action(storey.<>m__1DB));
        List<string> allAddStr = this.GetAllAddStr(_newCard.equipInfo, this.CurCard.equipInfo);
        this.CurCard = _newCard;
        base.StartCoroutine(this.PushText(allAddStr));
        this.UpdateEquipInfo(this.DefaultSelPart);
        this.UpdateOneKeyCost();
        base.StartCoroutine(this.PlayEffect(-1));
        TipsDiag.SetText("升级成功");
    }

    private void UpdateOtherHeroPenal()
    {
        if (this.CurCard != null)
        {
            HeroInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
            if ((gUIEntity != null) && !gUIEntity.Hidden)
            {
                gUIEntity.LevelUpEquipSucess(this.CurCard);
            }
            HeroPanel panel2 = GUIMgr.Instance.GetGUIEntity<HeroPanel>();
            if ((panel2 != null) && !panel2.Hidden)
            {
                panel2.UpdateItemCardInfo(this.CurCard);
            }
            BreakEquipPanel panel3 = GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>();
            if ((panel3 != null) && !panel3.Hidden)
            {
                panel3.LevelUpEquipSucess(this.CurCard);
            }
            Debug.Log("update other panel");
        }
    }

    private void WaitPanel()
    {
        this.CanClose = false;
        this.DelayCallBack(0.4f, () => this.CanClose = true);
    }

    [CompilerGenerated]
    private sealed class <PlayEffect>c__Iterator5C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int _part;
        internal int <$>_part;
        internal List<GameObject>.Enumerator <$s_449>__1;
        private static Action<GameObject> <>f__am$cacheA;
        internal EquipLevUpPanel <>f__this;
        internal List<GameObject> <EffList>__0;
        internal GameObject <EffObj>__3;
        internal GameObject <EffObj>__4;
        internal GameObject <obj>__2;

        private static void <>m__1E2(GameObject obj)
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
                        if (this._part < this.<>f__this.EquipObjList.Count)
                        {
                            this.<EffObj>__4 = this.<>f__this.EquipObjList[this._part].transform.FindChild("Effect").gameObject;
                            this.<EffObj>__4.SetActive(true);
                            this.$current = new WaitForSeconds(2f);
                            this.$PC = 2;
                            goto Label_01AC;
                        }
                        break;
                    }
                    this.<EffList>__0 = new List<GameObject>();
                    this.<$s_449>__1 = this.<>f__this.EquipObjList.GetEnumerator();
                    try
                    {
                        while (this.<$s_449>__1.MoveNext())
                        {
                            this.<obj>__2 = this.<$s_449>__1.Current;
                            this.<EffObj>__3 = this.<obj>__2.transform.FindChild("Effect").gameObject;
                            this.<EffObj>__3.SetActive(true);
                            this.<EffList>__0.Add(this.<EffObj>__3);
                        }
                    }
                    finally
                    {
                        this.<$s_449>__1.Dispose();
                    }
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 1;
                    goto Label_01AC;

                case 1:
                    if (<>f__am$cacheA == null)
                    {
                        <>f__am$cacheA = new Action<GameObject>(EquipLevUpPanel.<PlayEffect>c__Iterator5C.<>m__1E2);
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
    private sealed class <PushText>c__Iterator5B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<string> _infoList;
        internal List<string> <$>_infoList;
        internal List<string>.Enumerator <$s_448>__0;
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
                        this.<$s_448>__0.Dispose();
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
                    this.<$s_448>__0 = this._infoList.GetEnumerator();
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
                while (this.<$s_448>__0.MoveNext())
                {
                    this.<text>__1 = this.<$s_448>__0.Current;
                    TipsDiag.PushText(this.<text>__1);
                    this.$current = new WaitForSeconds(0.25f);
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
                this.<$s_448>__0.Dispose();
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
    private sealed class <StartEquAutoLevUp>c__Iterator5D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool _bGoldNotEnough;
        internal bool <$>_bGoldNotEnough;
        internal int <$>UpLev;
        internal EquipLevUpPanel <>f__this;
        internal int <n>__0;
        internal EquipInfo <newData>__1;
        internal List<string> <popStr>__2;
        internal int UpLev;

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
                    if (this.<>f__this.CurCard != null)
                    {
                        this.<n>__0 = 0;
                        while (this.<n>__0 < this.UpLev)
                        {
                            this.<newData>__1 = this.<>f__this.GetNewEquipData(this.<>f__this.CurCard);
                            if (this.<newData>__1 == null)
                            {
                                break;
                            }
                            this.<newData>__1.lv++;
                            TipsDiag.SetText("升级成功");
                            this.<popStr>__2 = this.<>f__this.GetAddStr(this.<newData>__1);
                            this.<>f__this.StartCoroutine(this.<>f__this.PushText(this.<popStr>__2));
                            this.<>f__this.UpdateEquipInfo(this.<>f__this.DefaultSelPart);
                            this.<>f__this.UpdateOneKeyCost();
                            this.$current = new WaitForSeconds(1.8f);
                            this.$PC = 1;
                            goto Label_0185;
                        Label_010A:
                            this.<n>__0++;
                        }
                        this.$current = new WaitForSeconds(0.3f);
                        this.$PC = 2;
                        goto Label_0185;
                    }
                    Debug.LogWarning("StartEquAutoLevUp card is null!");
                    break;

                case 1:
                    goto Label_010A;

                case 2:
                    if (this._bGoldNotEnough)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                    }
                    this.<>f__this.CanClickBtn = true;
                    this.<>f__this.EnablePanelClick(false);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0185:
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
    private sealed class <UpdateLevUp>c__AnonStorey187
    {
        internal int _cost;

        internal void <>m__1DC()
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2a61), this._cost));
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateOneKeyLevUp>c__AnonStorey186
    {
        internal int _cost;

        internal void <>m__1DB()
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2a61), this._cost));
        }
    }

    private enum AttributeType
    {
        ATK,
        HP,
        DEF,
        SEF,
        CRIT,
        DODGE,
        TENACITY,
        HIT,
        HP_RECOVER,
        ENERGY_RECOVER
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct EquipAttribute
    {
        public EquipLevUpPanel.AttributeType Type;
        public int AddVal;
        public int AddValGrow;
    }
}

