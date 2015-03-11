using System;
using UnityEngine;

public class AssisPay : MonoBehaviour
{
    public static AssisPay inst;
    public UILabel price;

    private void Awake()
    {
        inst = this;
    }

    public void OnClickBtn(AssistBtnType btnType)
    {
        switch (btnType)
        {
            case AssistBtnType.close:
                base.gameObject.SetActive(false);
                break;
        }
    }
}

