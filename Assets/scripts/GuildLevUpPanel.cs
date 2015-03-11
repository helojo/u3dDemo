using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class GuildLevUpPanel : GUIEntity
{
    private GuildFuncType CurType;
    public UILabel lb_btn;
    public UILabel lb_title;
    private int TechEntry;
    private UILabel TitleLabel;

    private void ClickLvUp()
    {
        switch (this.CurType)
        {
            case GuildFuncType.Tech:
                if (ActorData.getInstance().mGuildData.tech.warmill_level < ActorData.getInstance().mGuildData.tech.hall_level)
                {
                    SocketMgr.Instance.RequestGuildBuildLevUp(GuildBuild.E_GB_MILL);
                    break;
                }
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c3));
                break;

            case GuildFuncType.Shop:
                if (ActorData.getInstance().mGuildData.tech.shop_level < ActorData.getInstance().mGuildData.tech.hall_level)
                {
                    SocketMgr.Instance.RequestGuildBuildLevUp(GuildBuild.E_GB_SHOP);
                    break;
                }
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c4));
                break;

            case GuildFuncType.PaoKu:
                if (ActorData.getInstance().mGuildData.tech.parkour_level < ActorData.getInstance().mGuildData.tech.hall_level)
                {
                    if (ActorData.getInstance().mGuildData.tech.parkour_level == (GameConstValues.GUILD_PAOKU_MAX_LEVEL - 1))
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186cf));
                        return;
                    }
                    SocketMgr.Instance.RequestGuildBuildLevUp(GuildBuild.E_GB_PARKOUR);
                    break;
                }
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c5));
                return;

            case GuildFuncType.Hall:
                SocketMgr.Instance.RequestGuildBuildLevUp(GuildBuild.E_GB_HALL);
                break;

            case GuildFuncType.BufferLevUp:
                foreach (Tech tech in ActorData.getInstance().mGuildData.tech.tech)
                {
                    if (this.TechEntry == tech.entry)
                    {
                        if (tech.level >= 9)
                        {
                            guild_buff_config _config = ConfigMgr.getInstance().getByEntry<guild_buff_config>(tech.entry);
                            if (_config != null)
                            {
                                TipsDiag.SetText(_config.name + ConfigMgr.getInstance().GetWord(0xa6529a));
                            }
                        }
                        else if (tech.level >= ActorData.getInstance().mGuildData.tech.warmill_level)
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652a1));
                        }
                        else
                        {
                            SocketMgr.Instance.RequestGuildBufferLevUp(this.TechEntry);
                        }
                        break;
                    }
                }
                break;
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.AutoClose = base.gameObject.transform.FindChild("AutoClose").GetComponent<UIWidget>();
        if (this.AutoClose != null)
        {
            this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        }
    }

    public void UpdateBuffer()
    {
        foreach (Tech tech in ActorData.getInstance().mGuildData.tech.tech)
        {
            if (this.TechEntry == tech.entry)
            {
                this.UpdateBufferLevUp(tech);
                break;
            }
        }
    }

    public void UpdateBufferLevUp(Tech tech)
    {
        UILabel component = base.gameObject.transform.FindChild("Title/Label").GetComponent<UILabel>();
        UILabel label2 = base.gameObject.transform.FindChild("Title/Lv").GetComponent<UILabel>();
        UILabel label3 = base.gameObject.transform.FindChild("Title/ToLv").GetComponent<UILabel>();
        GameObject gameObject = base.gameObject.transform.FindChild("Title/Sp").gameObject;
        UILabel label4 = base.gameObject.transform.FindChild("Cost/Label").GetComponent<UILabel>();
        UILabel label5 = base.gameObject.transform.FindChild("Desc/Label1").GetComponent<UILabel>();
        UILabel label6 = base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>();
        this.CurType = GuildFuncType.BufferLevUp;
        base.gameObject.transform.FindChild("BackPic/Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0xa9867e);
        this.TechEntry = tech.entry;
        guild_buff_config _config = ConfigMgr.getInstance().getByEntry<guild_buff_config>(tech.entry);
        if (_config != null)
        {
            int num = tech.level + 1;
            component.text = _config.name + ":";
            int num2 = _config.level_up_cost;
            if (tech.level < 9)
            {
                label2.text = "LV. " + num;
                gameObject.SetActive(true);
                label4.text = num2.ToString();
                label3.text = "LV. " + ++num;
                label5.text = label3.text + ":";
                label6.text = _config.desc + ((_config.func_base + (_config.func_param * (num - 1)))).ToString();
            }
            else
            {
                label2.text = "LV. MAX";
                gameObject.SetActive(false);
                label4.text = string.Empty;
                label3.text = string.Empty;
                label5.text = string.Empty;
                label6.text = ConfigMgr.getInstance().GetWord(0xa6529a);
                base.gameObject.transform.FindChild("Cost/Label1").GetComponent<UILabel>().text = string.Empty;
            }
        }
    }

    public void UpdateData()
    {
        this.UpdateData(this.CurType);
    }

    public void UpdateData(GuildFuncType _type)
    {
        this.CurType = _type;
        UILabel component = base.gameObject.transform.FindChild("Title/Label").GetComponent<UILabel>();
        UILabel label2 = base.gameObject.transform.FindChild("Title/Lv").GetComponent<UILabel>();
        UILabel label3 = base.gameObject.transform.FindChild("Title/ToLv").GetComponent<UILabel>();
        GameObject gameObject = base.gameObject.transform.FindChild("Title/Sp").gameObject;
        UILabel label4 = base.gameObject.transform.FindChild("Cost/Label").GetComponent<UILabel>();
        UILabel label5 = base.gameObject.transform.FindChild("Desc/Label1").GetComponent<UILabel>();
        UILabel label6 = base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>();
        int num = 0;
        int id = 0;
        base.gameObject.transform.FindChild("BackPic/Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0xa652ba);
        switch (_type)
        {
            case GuildFuncType.Tech:
            {
                component.text = ConfigMgr.getInstance().GetWord(0xa65295) + ":";
                id = (int) ActorData.getInstance().mGuildData.tech.warmill_level;
                guild_mill_config _config5 = ConfigMgr.getInstance().getByEntry<guild_mill_config>(id);
                if (_config5 != null)
                {
                    num = _config5.level_up_cost;
                    if (id == GameConstValues.GUILD_BUILDING_MAX_LEVEL)
                    {
                        label6.text = ConfigMgr.getInstance().GetWord(0xa6529a);
                        gameObject.SetActive(false);
                        label2.text = "LV. MAX";
                        label3.text = string.Empty;
                        label5.text = string.Empty;
                        base.gameObject.transform.FindChild("Cost/Label1").GetComponent<UILabel>().text = string.Empty;
                        label4.text = string.Empty;
                    }
                    else
                    {
                        gameObject.SetActive(true);
                        label2.text = "LV. " + ((id + 1)).ToString();
                        label3.text = "LV. " + ((id + 2)).ToString();
                        label4.text = num.ToString();
                        label5.text = "LV. " + ((id + 2)).ToString() + ":";
                        List<int> configEntry = CommonFunc.GetConfigEntry(ConfigMgr.getInstance().getByEntry<guild_mill_config>(id + 1).buff);
                        string str = string.Empty;
                        foreach (int num3 in configEntry)
                        {
                            if (num3 >= 0)
                            {
                                guild_buff_config _config7 = ConfigMgr.getInstance().getByEntry<guild_buff_config>(num3);
                                str = str + " " + _config7.name;
                            }
                        }
                        if (str.Length > 0)
                        {
                            label6.text = ConfigMgr.getInstance().GetWord(0xa6529b) + str;
                        }
                        else
                        {
                            label6.text = ConfigMgr.getInstance().GetWord(0xa6529c);
                        }
                    }
                    break;
                }
                return;
            }
            case GuildFuncType.Shop:
            {
                component.text = ConfigMgr.getInstance().GetWord(0xa65293) + ":";
                id = (int) ActorData.getInstance().mGuildData.tech.shop_level;
                guild_shop_config _config3 = ConfigMgr.getInstance().getByEntry<guild_shop_config>(id);
                if (_config3 != null)
                {
                    num = _config3.level_up_cost;
                    if (id == GameConstValues.GUILD_BUILDING_MAX_LEVEL)
                    {
                        gameObject.SetActive(false);
                        label2.text = "LV. MAX";
                        label3.text = string.Empty;
                        base.gameObject.transform.FindChild("Cost/Label1").GetComponent<UILabel>().text = string.Empty;
                        label4.text = string.Empty;
                        label5.text = string.Empty;
                        label6.text = ConfigMgr.getInstance().GetWord(0xa6529a);
                    }
                    else
                    {
                        guild_shop_config _config4 = ConfigMgr.getInstance().getByEntry<guild_shop_config>(id + 1);
                        gameObject.SetActive(true);
                        label2.text = "LV. " + ((id + 1)).ToString();
                        label3.text = "LV. " + ((id + 2)).ToString();
                        label4.text = num.ToString();
                        label5.text = "LV. " + ((id + 2)).ToString() + ":";
                        label6.text = _config4.desc;
                    }
                    break;
                }
                return;
            }
            case GuildFuncType.PaoKu:
            {
                this.lb_title.text = ConfigMgr.getInstance().GetWord(0x186d0);
                this.lb_btn.text = ConfigMgr.getInstance().GetWord(0x186d1);
                component.text = ConfigMgr.getInstance().GetWord(0x186cd) + ":";
                id = (int) ActorData.getInstance().mGuildData.tech.parkour_level;
                parkour_config _config8 = ConfigMgr.getInstance().getByEntry<parkour_config>(id);
                if (_config8 != null)
                {
                    num = _config8.level_up_cost;
                    if (id == (GameConstValues.GUILD_PAOKU_MAX_LEVEL - 1))
                    {
                        label6.text = ConfigMgr.getInstance().GetWord(0xa6529a);
                        gameObject.SetActive(false);
                        label2.text = "LV. MAX";
                        label3.text = string.Empty;
                        label5.text = string.Empty;
                        base.gameObject.transform.FindChild("Cost/Label1").GetComponent<UILabel>().text = string.Empty;
                        label4.text = string.Empty;
                    }
                    else
                    {
                        parkour_config _config9 = ConfigMgr.getInstance().getByEntry<parkour_config>(id + 1);
                        if (_config9 == null)
                        {
                            return;
                        }
                        gameObject.SetActive(true);
                        label2.text = "LV. " + ((id + 1)).ToString();
                        label3.text = "LV. " + ((id + 2)).ToString();
                        label4.text = num.ToString();
                        label5.text = "LV. " + ((id + 2)).ToString() + ":";
                        label6.text = string.Format(ConfigMgr.getInstance().GetWord(0x186ce), _config9.desc);
                    }
                    break;
                }
                return;
            }
            case GuildFuncType.Hall:
            {
                component.text = ConfigMgr.getInstance().GetWord(0xa65294) + ":";
                id = (int) ActorData.getInstance().mGuildData.tech.hall_level;
                guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>(id);
                if (_config != null)
                {
                    num = _config.level_up_cost;
                    if (id == GameConstValues.GUILD_BUILDING_MAX_LEVEL)
                    {
                        gameObject.SetActive(false);
                        label2.text = "LV. MAX";
                        label3.text = string.Empty;
                        base.gameObject.transform.FindChild("Cost/Label1").GetComponent<UILabel>().text = string.Empty;
                        label4.text = string.Empty;
                        label5.text = string.Empty;
                        label6.text = ConfigMgr.getInstance().GetWord(0xa6529a);
                    }
                    else
                    {
                        guild_hall_config _config2 = ConfigMgr.getInstance().getByEntry<guild_hall_config>(id + 1);
                        gameObject.SetActive(true);
                        label2.text = "LV. " + ((id + 1)).ToString();
                        label3.text = "LV. " + ((id + 2)).ToString();
                        label4.text = num.ToString();
                        label5.text = "LV. " + ((id + 2)).ToString();
                        label6.text = string.Format(ConfigMgr.getInstance().GetWord(0xa65299), _config2.member_limit);
                    }
                    break;
                }
                return;
            }
        }
    }

    private UIWidget AutoClose { get; set; }
}

