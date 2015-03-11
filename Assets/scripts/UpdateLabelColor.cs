using System;
using UnityEngine;

public class UpdateLabelColor : MonoBehaviour
{
    public Color32 SelCor = new Color32(0xff, 0xff, 0xff, 0xff);
    public Color32 UnSelCol = new Color32(0xfc, 0xde, 0x8d, 0xff);

    public void UpdateColor(GameObject _Toggle)
    {
        UILabel component = base.gameObject.GetComponent<UILabel>();
        if (_Toggle.GetComponent<UIToggle>().value)
        {
            component.color = (Color) this.SelCor;
        }
        else
        {
            component.color = (Color) this.UnSelCol;
        }
    }
}

