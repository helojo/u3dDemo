using FastBuf;
using System;
using System.Runtime.CompilerServices;
using Toolbox;

public class GuildSelTechPanel : GUIEntity
{
    private Tech mCurTech;

    private void ClickOKBtn()
    {
        SocketMgr.Instance.RequestGuildSelectBuf(this.mCurTech.entry);
    }

    private int GetTechLv(int Entry)
    {
        foreach (Tech tech in ActorData.getInstance().mGuildData.tech.tech)
        {
            if (tech.entry == Entry)
            {
                return tech.level;
            }
        }
        return 0;
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

    public void UpdateData(Tech tech)
    {
        this.mCurTech = tech;
        guild_buff_config _config = ConfigMgr.getInstance().getByEntry<guild_buff_config>(tech.entry);
        if (_config != null)
        {
            UITexture component = base.gameObject.transform.FindChild("Title/Icon").GetComponent<UITexture>();
            base.gameObject.transform.FindChild("Title/ToIcon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateSkillIcon(_config.icon);
            if (ActorData.getInstance().mUserGuildMemberData.tech >= 0)
            {
                base.gameObject.transform.FindChild("Title1").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Title").gameObject.SetActive(true);
                int id = ActorData.getInstance().mUserGuildMemberData.tech;
                guild_buff_config _config2 = ConfigMgr.getInstance().getByEntry<guild_buff_config>(id);
                base.gameObject.transform.FindChild("Title/Sel").GetComponent<UILabel>().text = _config2.name + "LV " + (this.GetTechLv(id) + 1);
                base.gameObject.transform.FindChild("Title/ToSel").GetComponent<UILabel>().text = _config.name + "LV " + (tech.level + 1);
                component.mainTexture = BundleMgr.Instance.CreateSkillIcon(_config2.icon);
            }
            else
            {
                base.gameObject.transform.FindChild("Title1").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Title").gameObject.SetActive(false);
                base.gameObject.transform.FindChild("Title1/Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652a3), _config.name);
            }
            UILabel label = base.gameObject.transform.FindChild("Desc/Label").GetComponent<UILabel>();
            if (ActorData.getInstance().mUserGuildMemberData.bufftimes >= 0)
            {
                label.text = ActorData.getInstance().mUserGuildMemberData.bufftimes.ToString();
            }
            else
            {
                label.text = "0";
            }
            if (ActorData.getInstance().mUserGuildMemberData.bufftimes > 0)
            {
                base.gameObject.transform.FindChild("Cost").gameObject.SetActive(false);
            }
            else
            {
                base.gameObject.transform.FindChild("Cost").gameObject.SetActive(true);
                base.gameObject.transform.FindChild("Cost/Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652a5), GameConstValues.GUILD_SELECT_BUFF_COST);
            }
        }
    }

    private UIWidget AutoClose { get; set; }
}

