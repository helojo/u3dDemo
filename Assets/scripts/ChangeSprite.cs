using System;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField]
    public Color disable;
    [SerializeField]
    public string disableSpriteName;
    [SerializeField]
    public UILabel lable;
    [SerializeField]
    public Color noraml;
    [SerializeField]
    public string normalSpriteName;
    [SerializeField]
    public UISprite sprite;

    public void SetEnable(bool flag)
    {
        this.sprite.spriteName = !flag ? this.disableSpriteName : this.normalSpriteName;
        this.lable.color = !flag ? this.disable : this.noraml;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

