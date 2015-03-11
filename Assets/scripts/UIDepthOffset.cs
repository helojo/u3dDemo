using System;
using UnityEngine;

public class UIDepthOffset : MonoBehaviour
{
    public int depthOffset;
    private UIPanel panel;

    private void Start()
    {
        this.panel = UIPanel.Find(base.transform, true, base.gameObject.layer);
    }

    public int raycastDepth
    {
        get
        {
            if (this.panel == null)
            {
                return this.depthOffset;
            }
            return (this.depthOffset + (this.panel.depth * 0x3e8));
        }
    }
}

