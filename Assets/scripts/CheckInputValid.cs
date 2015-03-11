using System;
using UnityEngine;

public class CheckInputValid : MonoBehaviour
{
    public bool _isEnableMaskWord = true;
    public GameObject[] eventReceiver;

    public void OnInputChanged()
    {
        UIInput component = base.transform.GetComponent<UIInput>();
        if (this._isEnableMaskWord)
        {
            string maskWord = ConfigMgr.getInstance().GetMaskWord(component);
            if (maskWord != component.text)
            {
                component.text = maskWord;
            }
        }
    }
}

