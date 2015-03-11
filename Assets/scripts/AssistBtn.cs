using System;
using UnityEngine;

public class AssistBtn : MonoBehaviour
{
    public AssistBtnType btnType;

    private void OnClick()
    {
        AssisPay.inst.OnClickBtn(this.btnType);
    }
}

