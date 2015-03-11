using System;

public class SetSignatureDlag : GUIEntity
{
    private void OnClickOkBtn()
    {
        UIInput component = base.transform.FindChild("Input").GetComponent<UIInput>();
        string text = component.text;
        string maskWord = ConfigMgr.getInstance().GetMaskWord(text);
        if (maskWord.Contains("*"))
        {
            TipsDiag.SetText("输入文字中有敏感字！");
        }
        else
        {
            maskWord = maskWord.Replace(@"\n", string.Empty).Replace(@"\r", string.Empty).Replace(@"\r\n", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\r\n", string.Empty).Replace("\n\r", string.Empty);
            if (text.Length > 60)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(7));
            }
            else if (text == ActorData.getInstance().UserInfo.signature)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x3ef));
            }
            else
            {
                SocketMgr.Instance.RequestSetSignature(component.text);
            }
        }
    }
}

