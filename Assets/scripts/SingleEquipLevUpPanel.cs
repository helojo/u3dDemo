using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SingleEquipLevUpPanel : MonoBehaviour
{
    private EquipInfo CurEquipInfo;

    private bool CheckEqupMax(EquipInfo equ)
    {
        if (equ.lv < CommonFunc.GetMaxCardOrEquipLvByQuality(equ.quality))
        {
            return false;
        }
        return true;
    }

    public void ClickAutoLevUp(Card CurCard, int _part)
    {
        EquipInfo curEquipInfo = this.CurEquipInfo;
        if (curEquipInfo != null)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(curEquipInfo.entry);
            if (_config == null)
            {
                Debug.LogWarning("Item Cfg Is Null!");
            }
            else if ((curEquipInfo.lv >= CurCard.cardInfo.level) || (curEquipInfo.lv >= ActorData.getInstance().Level))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
            }
            else if (ActorData.getInstance().Gold < this.GetCost(_config.lv_up_gold, curEquipInfo.lv, curEquipInfo.quality))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
            }
            else
            {
                SocketMgr.Instance.RequestEquipLvUp(CurCard.card_id, _part, true);
            }
        }
    }

    public void ClickLevUp(Card CurCard, int _part)
    {
        if (this.CurEquipInfo != null)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.CurEquipInfo.entry);
            if (_config == null)
            {
                Debug.LogWarning("Item Cfg Is Null!");
            }
            else if (this.CurEquipInfo.lv >= CurCard.cardInfo.level)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
            }
            else if (ActorData.getInstance().Gold < this.GetCost(_config.lv_up_gold, this.CurEquipInfo.lv, this.CurEquipInfo.quality))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
            }
            else if (this.CurEquipInfo.lv < ActorData.getInstance().Level)
            {
                SocketMgr.Instance.RequestEquipLvUp(CurCard.card_id, _part, false);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
            }
        }
    }

    public void ClickOneKeyLevUp(Card CurCard)
    {
        int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.ONEKEY_EQU_LEV_UP);
        if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
        }
        else if (this.GetOneKeyCost(CurCard) == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287d));
        }
        else if (this.GetOneKeyCost(CurCard) > ActorData.getInstance().Gold)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037b8));
        }
        else
        {
            SocketMgr.Instance.RequestOneKeyEquipLevUp(CurCard.card_id);
        }
    }

    private int GetAutoCost(EquipInfo equ, int cardLv)
    {
        int num = 0;
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(equ.entry);
        if (_config != null)
        {
            int num2 = cardLv;
            for (int i = equ.lv; i < num2; i++)
            {
                num += this.GetCost(_config.lv_up_gold, i, equ.quality);
            }
        }
        return num;
    }

    private int GetCost(int _base, int _Lev, int _quality)
    {
        _base = CommonFunc.GetEquipLvUpBase(_Lev);
        return (_base + ((_Lev - 1) * CommonFunc.GetEquipLvUpGrowLv(_Lev)));
    }

    private int GetOneKeyCost(Card CurCard)
    {
        int num = 0;
        foreach (EquipInfo info in CurCard.equipInfo)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                int level = CurCard.cardInfo.level;
                for (int i = info.lv; i < level; i++)
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

    public void SetAutoCostInfo(EquipInfo equ, int cardLv)
    {
        base.transform.FindChild("AutoCost/Cost").GetComponent<UILabel>().text = this.GetAutoCost(equ, cardLv).ToString();
        UILabel component = base.transform.FindChild("AutoCost/Count").GetComponent<UILabel>();
        component.text = (ActorData.getInstance().EquipAutoLevUpRemainCount != -1) ? ActorData.getInstance().EquipAutoLevUpRemainCount.ToString() : ConfigMgr.getInstance().GetWord(0x2875);
        component.gameObject.SetActive(ActorData.getInstance().EquipAutoLevUpRemainCount != -1);
        base.transform.FindChild("AutoCost/LvTips").GetComponent<UILabel>().text = (equ.lv >= cardLv) ? ConfigMgr.getInstance().GetWord(0x58e) : string.Format(ConfigMgr.getInstance().GetWord(0x58c), cardLv);
    }

    public void SetOneKeyCostInfo(Card CurCard)
    {
        UILabel component = base.transform.FindChild("OneKeyCost/Cost").GetComponent<UILabel>();
        component.text = this.GetOneKeyCost(CurCard).ToString();
        Debug.Log("---------->" + component.text);
    }

    private void Start()
    {
    }

    private void Update()
    {
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

    private void UpdateEquipAddData(GameObject obj, EquipAttribute _data, EquipInfo _info)
    {
        UILabel component = obj.transform.FindChild("Label").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Val").GetComponent<UILabel>();
        UILabel label3 = obj.transform.FindChild("Add").GetComponent<UILabel>();
        switch (_data.Type)
        {
            case AttributeType.ATK:
                component.text = ConfigMgr.getInstance().GetWord(320) + ":";
                break;

            case AttributeType.HP:
                component.text = ConfigMgr.getInstance().GetWord(0x141) + ":";
                break;

            case AttributeType.DEF:
                component.text = ConfigMgr.getInstance().GetWord(0x142) + ":";
                break;

            case AttributeType.SEF:
                component.text = ConfigMgr.getInstance().GetWord(0x143) + ":";
                break;

            case AttributeType.CRIT:
                component.text = ConfigMgr.getInstance().GetWord(0x144) + ":";
                break;

            case AttributeType.DODGE:
                component.text = ConfigMgr.getInstance().GetWord(0x145) + ":";
                break;

            case AttributeType.TENACITY:
                component.text = ConfigMgr.getInstance().GetWord(0x146) + ":";
                break;

            case AttributeType.HIT:
                component.text = ConfigMgr.getInstance().GetWord(0x147) + ":";
                break;

            case AttributeType.HP_RECOVER:
                component.text = ConfigMgr.getInstance().GetWord(0x148) + ":";
                break;

            case AttributeType.ENERGY_RECOVER:
                component.text = ConfigMgr.getInstance().GetWord(0x149) + ":";
                break;

            case AttributeType.SUCK:
                component.text = ConfigMgr.getInstance().GetWord(0x16e) + ":";
                break;
        }
        int num = _data.AddVal + ((this.CurEquipInfo.lv - 1) * _data.AddValGrow);
        string text = component.text;
        object[] objArray1 = new object[] { text, num, GameConstant.DefaultTextRedColor, " +", _data.AddValGrow };
        component.text = string.Concat(objArray1);
    }

    private void UpdateEquipAddLev(GameObject obj, EquipInfo _data)
    {
        UILabel component = obj.transform.FindChild("Val").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("Add").GetComponent<UILabel>();
        component.text = "Lv. " + _data.lv.ToString();
        label2.text = "Lv. " + ((_data.lv + 1)).ToString();
    }

    public void UpdateEquipData(EquipInfo _equ, int cardlv)
    {
        this.CurEquipInfo = _equ;
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_equ.entry);
        if (_config == null)
        {
            Debug.LogWarning("Item Cfg Is Null!");
        }
        else
        {
            base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>().text = _config.describe;
            base.gameObject.transform.FindChild("Desc/name").GetComponent<UILabel>().text = _config.name;
            base.gameObject.transform.FindChild("Cost/Cost").GetComponent<UILabel>().text = (_equ.lv >= cardlv) ? "0" : this.GetCost(_config.lv_up_gold, _equ.lv, _equ.quality).ToString();
            base.transform.FindChild("Cost/LvTips").GetComponent<UILabel>().text = (_equ.lv >= cardlv) ? ConfigMgr.getInstance().GetWord(0x58e) : string.Format(ConfigMgr.getInstance().GetWord(0x58d), _equ.lv + 1);
            GameObject gameObject = base.gameObject.transform.FindChild("Desc/EquipItem").gameObject;
            gameObject.SetActive(true);
            UITexture component = gameObject.transform.FindChild("Icon").GetComponent<UITexture>();
            CommonFunc.SetEquipQualityBorder(gameObject.transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
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
            if (_config.suck_lv > 0)
            {
                EquipAttribute attribute11 = new EquipAttribute {
                    Type = AttributeType.SUCK,
                    AddVal = _config.suck_lv,
                    AddValGrow = _config.suck_grow
                };
                list.Add(attribute11);
            }
            this.UpdateCtrl(list, _equ);
            GameObject obj3 = base.gameObject.transform.FindChild("Desc/Lev").gameObject;
            obj3.SetActive(true);
            this.UpdateEquipAddLev(obj3, _equ);
        }
    }

    public void UpdateLevUp(EquipInfo _newEquipData, int cardLv)
    {
        this.UpdateEquipData(_newEquipData, cardLv);
        TipsDiag.SetText("升级成功");
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
        ENERGY_RECOVER,
        SUCK
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct EquipAttribute
    {
        public SingleEquipLevUpPanel.AttributeType Type;
        public int AddVal;
        public int AddValGrow;
    }
}

