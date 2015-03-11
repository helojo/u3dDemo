using System;
using UnityEngine;

public class DynamicTweenAlpha : UITweener
{
    public float from = 1f;
    private UITexture target;
    public float to = 1f;

    private void Awake()
    {
        this.target = base.GetComponent<UITexture>();
    }

    protected override void OnUpdate(float factor, bool finished)
    {
        if (null != this.target)
        {
            float num = Mathf.Lerp(this.from, this.to, factor);
            if ((null != this.target.drawCall) && (null != this.target.drawCall.dynamicMaterial))
            {
                this.target.drawCall.dynamicMaterial.SetFloat("_Alpha", num);
            }
        }
    }
}

