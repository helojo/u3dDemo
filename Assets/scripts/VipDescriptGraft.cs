using System;
using UnityEngine;

public class VipDescriptGraft : MonoBehaviour
{
    public bool canClose;
    public static VipDescriptGraft inst;
    private SpringPanel sp;
    private UIScrollBar uisb;
    private VipDescriptPanel vdp;

    private void Awake()
    {
        inst = this;
        this.vdp = base.transform.parent.GetComponent<VipDescriptPanel>();
        this.uisb = base.GetComponent<UIScrollBar>();
        this.vdp.panel = base.transform.GetComponent<UIPanel>();
        this.sp = base.GetComponent<SpringPanel>();
        base.enabled = false;
    }

    private void Update()
    {
        this.vdp.ReSetNum();
        if (this.sp.enabled)
        {
            this.sp.strength += 1.4f;
            this.canClose = true;
        }
        if (this.canClose && !this.sp.enabled)
        {
            base.enabled = false;
        }
    }
}

