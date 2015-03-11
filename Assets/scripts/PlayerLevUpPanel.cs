using FastBuf;
using System;
using UnityEngine;

public class PlayerLevUpPanel : GUIEntity
{
    private string GetNewFunTipsStr(int _SrcLev, int _toLev)
    {
        level_limit_config _config = ConfigMgr.getInstance().getByEntry<level_limit_config>(0);
        if (_config != null)
        {
            if ((_SrcLev < _config.item_box) && (_toLev >= _config.item_box))
            {
                return string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a1));
            }
            if ((_SrcLev < _config.dungeons) && (_toLev >= _config.dungeons))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a2)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5af)));
            }
            if ((_SrcLev < _config.shop) && (_toLev >= _config.shop))
            {
                return string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a3));
            }
            if ((_SrcLev < _config.guild) && (_toLev >= _config.guild))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a4)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b0)));
            }
            if ((_SrcLev < _config.elite_dup) && (_toLev >= _config.elite_dup))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a5)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b1)));
            }
            if ((_SrcLev < _config.void_tower) && (_toLev >= _config.void_tower))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a6)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b2)));
            }
            if ((_SrcLev < _config.flamebattle) && (_toLev >= _config.flamebattle))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a7)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b3)));
            }
            if ((_SrcLev < _config.lifeskill) && (_toLev >= _config.lifeskill))
            {
                return string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a8));
            }
            if ((_SrcLev < _config.outland_beginner) && (_toLev >= _config.outland_beginner))
            {
                return (string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5a9)) + "\n" + string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b4)));
            }
            if ((_SrcLev < _config.outlan) && (_toLev >= _config.outlan))
            {
                return string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5aa));
            }
            if ((_SrcLev < _config.arena_ladder) && (_toLev >= _config.arena_ladder))
            {
                string[] textArray1 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0x5ab)), "\n", string.Format(ConfigMgr.getInstance().GetWord(0x5a0), ConfigMgr.getInstance().GetWord(0xbc5)), "\n", string.Format(ConfigMgr.getInstance().GetWord(0x5ae), ConfigMgr.getInstance().GetWord(0x5b5)) };
                return string.Concat(textArray1);
            }
        }
        return string.Empty;
    }

    private void UpdateChange(GameObject obj, int _param1, int _param2)
    {
        obj.transform.FindChild("Val1").GetComponent<UILabel>().text = _param1.ToString();
        obj.transform.FindChild("Val2").GetComponent<UILabel>().text = _param2.ToString();
    }

    public void UpdateData(int _SrcLev, int _toLev, int _SrcPhyForce)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("Lv").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("PhysicalCur").gameObject;
        GameObject obj4 = base.gameObject.transform.FindChild("PhysicalLimit").gameObject;
        GameObject obj5 = base.gameObject.transform.FindChild("CardLvLimit").gameObject;
        this.UpdateChange(gameObject, _SrcLev, _toLev);
        this.UpdateChange(obj5, _SrcLev, _toLev);
        user_lv_up_config _config = ConfigMgr.getInstance().getByEntry<user_lv_up_config>(_SrcLev);
        if (_config != null)
        {
            user_lv_up_config _config2 = ConfigMgr.getInstance().getByEntry<user_lv_up_config>(_toLev);
            if (_config2 != null)
            {
                this.UpdateChange(obj4, _config.phy_force_max, _config2.phy_force_max);
                this.UpdateChange(obj3, _SrcPhyForce, ActorData.getInstance().PhyForce);
                string newFunTipsStr = this.GetNewFunTipsStr(_SrcLev, _toLev);
                if (newFunTipsStr != string.Empty)
                {
                    base.transform.FindChild("NewFunTips/Tips1").GetComponent<UILabel>().text = newFunTipsStr;
                    base.transform.FindChild("NewFunTips").gameObject.SetActive(true);
                }
                else
                {
                    base.transform.FindChild("NewFunTips").gameObject.SetActive(false);
                }
            }
        }
    }
}

