using FastBuf;
using System;
using System.Runtime.CompilerServices;
using Toolbox;

[GUITip("ItemInfo")]
public class GUITipItem : GUITipUI
{
    private int _itemId;

    public void Initialize()
    {
        this.Name = base.FindChild<UILabel>("Name");
        this.Count = base.FindChild<UILabel>("Count");
        this.Label = base.FindChild<UILabel>("Label");
        this.Desc = base.FindChild<UILabel>("Desc");
        this.Icon = base.FindChild<UITexture>("Icon");
        this.ICount = base.FindChild<UILabel>("ICount");
        this.Bg = base.FindChild<UISprite>("Bg");
        this.QualityBorder = base.FindChild<UISprite>("QualityBorder");
    }

    public override void OnCreate()
    {
        base.OnCreate();
        this.Initialize();
    }

    public override void OnDraw()
    {
        base.OnDraw();
    }

    protected UISprite Bg { get; set; }

    protected UILabel Count { get; set; }

    protected UILabel Desc { get; set; }

    protected UITexture Icon { get; set; }

    protected UILabel ICount { get; set; }

    public int ItemID
    {
        get
        {
            return this._itemId;
        }
        set
        {
            this.ICount.ActiveSelfObject(false);
            if (this._itemId != value)
            {
                this._itemId = value;
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this._itemId);
                if (_config != null)
                {
                    int itemCountByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemCountByEntry(this._itemId);
                    this.Count.text = string.Format("{0:0}", itemCountByEntry);
                    this.Desc.text = _config.describe;
                    this.Name.text = _config.name;
                    this.Icon.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    CommonFunc.SetEquipQualityBorder(this.QualityBorder, _config.quality, false);
                }
            }
        }
    }

    protected UILabel Label { get; set; }

    protected UILabel Name { get; set; }

    protected UISprite QualityBorder { get; set; }
}

