using System;

public class GuildContributePanel : GUIEntity
{
    private void ClickOk()
    {
        PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
        if (component == null)
        {
            GUIMgr.Instance.ExitModelGUI("GuildContributePanel");
        }
        else
        {
            switch (component.FsmVariables.FindFsmInt("Myvalue").Value)
            {
                case 0:
                    SocketMgr.Instance.RequestGuildContribute(GameConstValues.GUILD_CONTRIBUTION_TIMES_1);
                    break;

                case 1:
                    SocketMgr.Instance.RequestGuildContribute(GameConstValues.GUILD_CONTRIBUTION_TIMES_2);
                    break;

                case 2:
                    SocketMgr.Instance.RequestGuildContribute(GameConstValues.GUILD_CONTRIBUTION_TIMES_3);
                    break;

                case 3:
                    SocketMgr.Instance.RequestGuildContribute(GameConstValues.GUILD_CONTRIBUTION_TIMES_4);
                    break;
            }
            Daily gUIEntity = GUIMgr.Instance.GetGUIEntity<Daily>();
            if ((gUIEntity != null) && gUIEntity.CheckQuestExistsByType(0x15))
            {
                SocketMgr.Instance.RequestRewardFlag();
            }
            GUIMgr.Instance.ExitModelGUI("GuildContributePanel");
        }
    }

    public override void OnInitialize()
    {
        int num = GameConstValues.GUILD_CONTRIBUTION_GOLD;
        int num2 = GameConstValues.GUILD_CONTRIBUTION;
        int num3 = GameConstValues.GUILD_CONTRIBUTION_TIMES_1;
        int num4 = GameConstValues.GUILD_CONTRIBUTION_TIMES_2;
        int num5 = GameConstValues.GUILD_CONTRIBUTION_TIMES_3;
        int num6 = GameConstValues.GUILD_CONTRIBUTION_TIMES_4;
        base.gameObject.transform.FindChild("Toggle1").gameObject.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b5), num * num3, num2 * num3);
        base.gameObject.transform.FindChild("Toggle2").gameObject.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b5), num * num4, num2 * num4);
        base.gameObject.transform.FindChild("Toggle3").gameObject.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b5), num * num5, num2 * num5);
        base.gameObject.transform.FindChild("Toggle4").gameObject.transform.FindChild("Label").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b5), num * num6, num2 * num6);
    }
}

