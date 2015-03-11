using System;
using UnityEngine;

[RequireComponent(typeof(UIWidget)), AddComponentMenu("NGUI/UI/Localize"), ExecuteInEditMode]
public class UILocalize : MonoBehaviour
{
    public string key;
    private bool mStarted;

    private void OnEnable()
    {
        if (this.mStarted)
        {
            this.OnLocalize();
        }
    }

    private void OnLocalize()
    {
        if (string.IsNullOrEmpty(this.key))
        {
            UILabel component = base.GetComponent<UILabel>();
            if (component != null)
            {
                this.key = component.text;
            }
        }
        if (!string.IsNullOrEmpty(this.key))
        {
            this.value = Localization.Get(this.key);
        }
    }

    private void Start()
    {
        this.mStarted = true;
        this.OnLocalize();
    }

    public string value
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                UIWidget component = base.GetComponent<UIWidget>();
                UILabel label = component as UILabel;
                UISprite sprite = component as UISprite;
                if (label != null)
                {
                    UIInput input = NGUITools.FindInParents<UIInput>(label.gameObject);
                    if ((input != null) && (input.label == label))
                    {
                        input.defaultText = value;
                    }
                    else
                    {
                        label.text = value;
                    }
                }
                else if (sprite != null)
                {
                    sprite.spriteName = value;
                    sprite.MakePixelPerfect();
                }
            }
        }
    }
}

