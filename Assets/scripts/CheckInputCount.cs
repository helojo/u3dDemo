using System;
using UnityEngine;

public class CheckInputCount : MonoBehaviour
{
    public UILabel _TipsLabel;

    public void OnInputChanged()
    {
        UIInput component = base.transform.GetComponent<UIInput>();
        this._TipsLabel.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x10f), GameConstant.DefaultTextRedColor + (component.characterLimit - component.text.Length) + "[-]");
    }

    private void Start()
    {
        UIInput component = base.transform.GetComponent<UIInput>();
        this._TipsLabel.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x10f), GameConstant.DefaultTextRedColor + (component.characterLimit - component.text.Length) + "[-]");
    }

    private void Update()
    {
    }
}

