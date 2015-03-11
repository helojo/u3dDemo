using System;

public class GuildModNoticePanel : GUIEntity
{
    private void ClickModifyBtn()
    {
        UIInput component = base.gameObject.transform.FindChild("Input").GetComponent<UIInput>();
        if (component.value.Length <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa2));
        }
        else if (ConfigMgr.getInstance().GetMaskWord(component.text.Trim()).Contains("*"))
        {
            TipsDiag.SetText("输入文字中有敏感字！");
        }
        else
        {
            SocketMgr.Instance.RequestGuildSetNotice(component.value, ActorData.getInstance().mGuildData.apply_type, ActorData.getInstance().mGuildData.apply_level);
        }
    }

    public override void OnInitialize()
    {
        UIInput component = base.gameObject.transform.FindChild("Input").GetComponent<UIInput>();
        component.defaultText = ActorData.getInstance().mGuildData.notice;
        component.text = ActorData.getInstance().mGuildData.notice;
    }
}

