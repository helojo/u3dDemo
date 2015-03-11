using System;
using UnityEngine;

public class ActiveTips : MonoBehaviour
{
    public UILabel describeLabel;
    public UITexture iconUIT;
    public UILabel nameLabel;
    public UISprite quaUIs;
    public GameObject spriteCardType;
    public UITexture texQuality_new;
    public Transform tr;
    public UILabel uil;
    public UIPanel uip;
    public UISprite uis;

    private void Awake()
    {
        this.tr = base.transform;
    }

    public void ResetSize()
    {
        float num = this.uil.localSize.y - 78f;
        if (num < 0f)
        {
            num = 0f;
        }
        this.uis.height = 0xc5 + ((int) num);
    }

    private void Start()
    {
        this.uip.depth = 0xbd;
    }
}

