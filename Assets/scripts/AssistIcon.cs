using System;
using UnityEngine;

public class AssistIcon : MonoBehaviour
{
    public GameObject freeIcon;
    public UILabel friName;
    public UITexture head;
    public bool QQ;
    public GameObject QQIcon;
    public string QQname;
    public bool state = true;

    public void ResetDate(int t)
    {
        if (t <= 0)
        {
            this.state = true;
        }
        else
        {
            this.state = false;
            this.freeIcon.SetActive(false);
        }
    }

    private void Start()
    {
        if (this.QQ)
        {
            Color color = new Color(0f, 0.37f, 0.6f, 255f);
            this.friName.color = color;
            this.friName.text = this.QQname;
        }
    }
}

