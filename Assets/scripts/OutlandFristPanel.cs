using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OutlandFristPanel : GUIEntity
{
    public UIButton _guideBtn;
    public UITexture _icon147;
    public UITexture _icon257;
    public UITexture _icon367;
    public UILabel _labelName147;
    public UILabel _labelName257;
    public UILabel _labelName367;
    public UIButton _shopBtn;
    private int _underway_entry;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    public UISprite skillTest;

    private void EnterOutlandS(outland_map_type_config maptype_data)
    {
        <EnterOutlandS>c__AnonStorey1C7 storeyc = new <EnterOutlandS>c__AnonStorey1C7 {
            maptype_data = maptype_data
        };
        if (storeyc.maptype_data != null)
        {
            OutlandTitle title = ActorData.getInstance().outlandTitles.Find(new Predicate<OutlandTitle>(storeyc.<>m__2CE));
            if (storeyc.maptype_data.open_day == "1")
            {
                if ((title.remain > 0) || ActorData.getInstance().outlandTitles[storeyc.maptype_data.entry].is_underway)
                {
                    GUIMgr.Instance.PushGUIEntity("OutlandSecondPanel", new Action<GUIEntity>(storeyc.<>m__2CF));
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e34));
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e2f));
            }
        }
    }

    private void GuideOnClick(GameObject go)
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.SetOutlandRule();
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheA, null);
    }

    private void OnClickEnterBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            outland_map_type_config _config = (outland_map_type_config) obj2;
            this.EnterOutlandS(_config);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.UpdateFameCoin();
        GUIMgr.Instance.FloatTitleBar();
        this.SetInfo(ActorData.getInstance().outlandTitles);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        if (this._guideBtn != null)
        {
            UIEventListener.Get(this._guideBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.GuideOnClick);
        }
        if (this._shopBtn != null)
        {
            UIEventListener.Get(this._shopBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.ShopOnClick);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public void SetInfo(List<OutlandTitle> titles)
    {
        this._underway_entry = -1;
        foreach (OutlandTitle title in titles)
        {
            Transform transform2;
            Transform transform4;
            Transform transform6;
            outland_map_type_config data = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(title.entry);
            if (data != null)
            {
                switch (data.entry)
                {
                    case 0:
                    {
                        object[] objArray1 = new object[] { data.name, "(", title.remain, "/", data.times, ")" };
                        this._labelName147.text = string.Concat(objArray1);
                        this._icon147.mainTexture = BundleMgr.Instance.CreateOutlandIcon(data.icon);
                        Transform transform = base.transform.FindChild("Center/147");
                        GUIDataHolder.setData(transform.gameObject, data);
                        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEnterBtn);
                        transform2 = base.transform.FindChild("Center/147/Group");
                        transform2.gameObject.SetActive(false);
                        if (!title.isopen)
                        {
                            goto Label_0144;
                        }
                        nguiTextureGrey.doChangeEnableGrey(this._icon147, false);
                        data.open_day = "1";
                        goto Label_015B;
                    }
                    case 1:
                    {
                        object[] objArray2 = new object[] { data.name, "(", title.remain, "/", data.times, ")" };
                        this._labelName257.text = string.Concat(objArray2);
                        this._icon257.mainTexture = BundleMgr.Instance.CreateOutlandIcon(data.icon);
                        Transform transform3 = base.transform.FindChild("Center/257");
                        GUIDataHolder.setData(transform3.gameObject, data);
                        UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEnterBtn);
                        transform4 = base.transform.FindChild("Center/257/Group");
                        transform4.gameObject.SetActive(false);
                        if (!title.isopen)
                        {
                            goto Label_0271;
                        }
                        nguiTextureGrey.doChangeEnableGrey(this._icon257, false);
                        data.open_day = "1";
                        goto Label_0288;
                    }
                    case 2:
                    {
                        Transform transform5 = base.transform.FindChild("Center/367");
                        GUIDataHolder.setData(transform5.gameObject, data);
                        UIEventListener.Get(transform5.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEnterBtn);
                        object[] objArray3 = new object[] { data.name, "(", title.remain, "/", data.times, ")" };
                        this._labelName367.text = string.Concat(objArray3);
                        this._icon367.mainTexture = BundleMgr.Instance.CreateOutlandIcon(data.icon);
                        transform6 = base.transform.FindChild("Center/367/Group");
                        transform6.gameObject.SetActive(false);
                        if (!title.isopen)
                        {
                            goto Label_039E;
                        }
                        nguiTextureGrey.doChangeEnableGrey(this._icon367, false);
                        data.open_day = "1";
                        goto Label_03B5;
                    }
                }
            }
            continue;
        Label_0144:
            nguiTextureGrey.doChangeEnableGrey(this._icon147, true);
            data.open_day = "0";
        Label_015B:
            transform2.gameObject.SetActive(ActorData.getInstance().outlandTitles[0].is_underway);
            continue;
        Label_0271:
            nguiTextureGrey.doChangeEnableGrey(this._icon257, true);
            data.open_day = "0";
        Label_0288:
            transform4.gameObject.SetActive(ActorData.getInstance().outlandTitles[1].is_underway);
            continue;
        Label_039E:
            nguiTextureGrey.doChangeEnableGrey(this._icon367, true);
            data.open_day = "0";
        Label_03B5:
            transform6.gameObject.SetActive(ActorData.getInstance().outlandTitles[2].is_underway);
        }
    }

    private void ShopOnClick(GameObject go)
    {
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (GUIEntity obj) {
                ((ShopPanel) obj).SetShopType(ShopCoinType.OutLandCoin);
                SocketMgr.Instance.RequestOutlandShopInfo();
            };
        }
        GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cacheB);
    }

    public void UpdateFameCoin()
    {
        UILabel component = base.transform.FindChild("Bottom/CoinInfo/Coin").GetComponent<UILabel>();
        if (component != null)
        {
            component.text = ActorData.getInstance().UserInfo.outlandCoin.ToString();
        }
    }

    [CompilerGenerated]
    private sealed class <EnterOutlandS>c__AnonStorey1C7
    {
        internal outland_map_type_config maptype_data;

        internal bool <>m__2CE(OutlandTitle o)
        {
            return (o.entry == this.maptype_data.entry);
        }

        internal void <>m__2CF(GUIEntity obj)
        {
            OutlandSecondPanel panel = (OutlandSecondPanel) obj;
            ActorData.getInstance().tempOutlandMapTypeConfig = this.maptype_data;
            ActorData.getInstance().outlandType = this.maptype_data.entry;
            panel.SetInfo(this.maptype_data);
        }
    }
}

