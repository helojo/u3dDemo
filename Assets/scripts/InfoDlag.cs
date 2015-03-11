using System;

public class InfoDlag : GUIEntity
{
    public void SetInfoCenterContent(string str)
    {
        UILabel component = base.transform.FindChild("Label").GetComponent<UILabel>();
        component.text = str;
        component.fontSize = 30;
        component.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Center;
    }

    public void SetInfoCenterContent2(string str)
    {
        UILabel component = base.transform.FindChild("Label").GetComponent<UILabel>();
        component.text = str;
        component.fontSize = 0x20;
        component.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Left;
    }

    public void SetInfoContent(string str)
    {
        base.transform.FindChild("Label").GetComponent<UILabel>().text = str;
    }
}

