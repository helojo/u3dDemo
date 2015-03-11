using System;

public class ExchangeCodePanel : GUIEntity
{
    private void OnClickSendBtn()
    {
        UIInput component = base.transform.FindChild("Input").GetComponent<UIInput>();
        if (component.text.Trim().Length == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2eea));
        }
        else
        {
            SocketMgr.Instance.RequestPickGift(component.text.Trim());
        }
    }
}

