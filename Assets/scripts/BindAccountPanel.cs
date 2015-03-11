using System;

public class BindAccountPanel : GUIEntity
{
    private void ClosePanel()
    {
        GUIMgr.Instance.ExitModelGUI("BindAccountPanel");
    }

    private void OnClickBind()
    {
        UIInput component = base.gameObject.transform.FindChild("InputAccount").GetComponent<UIInput>();
        UIInput input2 = base.gameObject.transform.FindChild("InputPassword").GetComponent<UIInput>();
    }
}

