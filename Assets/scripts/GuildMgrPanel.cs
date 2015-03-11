using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuildMgrPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache5;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache6;
    private GameObject mNewsSp;

    public void CheckApplyStat()
    {
        if (this.mNewsSp != null)
        {
            this.mNewsSp.SetActive(ActorData.getInstance().GuildApplyList.Count > 0);
        }
    }

    private void ClickApplyList()
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (GUIEntity obj) {
                GuildApplyPanel panel = obj.Achieve<GuildApplyPanel>();
                if (panel != null)
                {
                    panel.UpdateData(ActorData.getInstance().GuildApplyList);
                }
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildApplyPanel", <>f__am$cache1, base.gameObject);
    }

    private void ClickGuildDismiss()
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = delegate (GUIEntity obj) {
                if (<>f__am$cache6 == null)
                {
                    <>f__am$cache6 = go => SocketMgr.Instance.RequestGuildDismiss();
                }
                obj.Achieve<MessageBox>().SetDialog(ConfigMgr.getInstance().GetWord(0xa6528e), <>f__am$cache6, null, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache4, base.gameObject);
    }

    private void ClickGuildLevUp()
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = guiE => guiE.Achieve<GuildLevUpPanel>().UpdateData(GuildFuncType.Hall);
        }
        GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", <>f__am$cache5, base.gameObject);
    }

    private void ClickGuildSetting()
    {
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate (GUIEntity obj) {
                GuildSettingPanel panel = (GuildSettingPanel) obj;
                panel.Depth = 0x191;
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildSettingPanel", <>f__am$cache3, null);
    }

    private void ClickModifyNotise()
    {
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = delegate (GUIEntity obj) {
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildModNoticePanel", <>f__am$cache2, base.gameObject);
    }

    public void Init()
    {
        this.mNewsSp = base.transform.FindChild("Button1/News").gameObject;
        this.CheckApplyStat();
        this.UpdateGuildLevel();
    }

    public void UpdateGuildLevel()
    {
        if (ActorData.getInstance().mGuildData != null)
        {
            base.transform.FindChild("Info/GuildName").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.name;
            base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = "LV." + (ActorData.getInstance().mGuildData.tech.hall_level + 1);
        }
    }
}

