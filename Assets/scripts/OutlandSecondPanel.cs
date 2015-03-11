using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OutlandSecondPanel : GUIEntity
{
    public UIGrid _Grid;
    public UILabel _LabelIntroduce;
    public UILabel _LabelName;
    public GameObject _SingleOutlandItem;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    public UISprite bgitmeSprite;
    public UITexture fbIcon;
    private bool isJxz;

    private void EnterOutlandLevel(OutlandPressInfo info)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<outland_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                <EnterOutlandLevel>c__AnonStorey1C9 storeyc = new <EnterOutlandLevel>c__AnonStorey1C9 {
                    temp = current as outland_config
                };
                if ((storeyc.temp.toll_gate_entry == info.toll_gate_entry) && (storeyc.temp.outland_type == info.outlantType))
                {
                    if ((ActorData.getInstance().outlandTitles.Find(new Predicate<OutlandTitle>(storeyc.<>m__2D2)).remain > 0) || ActorData.getInstance().outlandTitles[storeyc.temp.outland_type].is_underway)
                    {
                        ActorData.getInstance().outlandPageEntry = storeyc.temp.entry;
                        ActorData.getInstance().bOpenThirdPanel = true;
                        SocketMgr.Instance.RequestOutlandPageMapReq(storeyc.temp.entry);
                    }
                    else
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e34));
                    }
                    return;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    private void OnClickItemBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            OutlandPressInfo info = (OutlandPressInfo) obj2;
            if (ActorData.getInstance().Level < info.limitLevel)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x4e29), info.limitLevel));
            }
            else
            {
                int entry = ActorData.getInstance().tempOutlandMapTypeConfig.entry;
                if ((entry >= 0) && (entry < 3))
                {
                    int id = ActorData.getInstance().outlandTitles[entry].underway_entry;
                    if (id != -1)
                    {
                        outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(id);
                        if ((ActorData.getInstance().outlandTitles[info.outlantType].remain <= 0) && (info.toll_gate_entry != _config.toll_gate_entry))
                        {
                            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x4e34), new object[0]));
                            return;
                        }
                    }
                }
                this.EnterOutlandLevel(info);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        if (ActorData.getInstance().isPreOutlandFight)
        {
            GUIMgr.Instance.FloatTitleBar();
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("15Panel/Bottom/StartBtn").GetComponent<UIButton>().gameObject).onClick = new UIEventListener.VoidDelegate(this.startBtnClick);
        UIEventListener.Get(base.transform.FindChild("15Panel/Bottom/ShopBtn").GetComponent<UIButton>().gameObject).onClick = new UIEventListener.VoidDelegate(this.shopBtnClick);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    private void returnBtnClick(GameObject go)
    {
        if (ActorData.getInstance().isPreOutlandFight)
        {
            GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
        }
    }

    private void ReturnFirst()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void SetGridItemInfo(bool isOpen, bool isRecommended, GameObject go, outland_map_type_config outlandMapType, int sort, bool _isJxz, int limitLevel)
    {
        OutlandPressInfo info;
        Transform transform = go.transform.FindChild("Item");
        info.outlantType = outlandMapType.entry;
        info.toll_gate_entry = (limitLevel - 0x19) / 15;
        info.limitLevel = limitLevel;
        GUIDataHolder.setData(transform.gameObject, info);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
        this.bgitmeSprite = transform.transform.FindChild("bg").GetComponent<UISprite>();
        if (this.bgitmeSprite != null)
        {
            int num = sort + 1;
            this.bgitmeSprite.spriteName = !isOpen ? ("Ui_Out_Icon_lev" + num + num) : ("Ui_Out_Icon_lev" + num);
        }
        UILabel component = transform.transform.FindChild("LV").GetComponent<UILabel>();
        if (component != null)
        {
            component.gameObject.SetActive(true);
            component.text = "LV" + limitLevel;
        }
        transform.transform.FindChild("recommend").GetComponent<UISprite>().gameObject.SetActive(isRecommended);
        transform.transform.FindChild("Group").gameObject.SetActive(_isJxz);
    }

    public void SetInfo(outland_map_type_config outlandMapType)
    {
        if (outlandMapType != null)
        {
            base.transform.FindChild("Info").gameObject.SetActive(!ActorData.getInstance().isPreOutlandFight);
            base.transform.FindChild("ListSelect").gameObject.SetActive(!ActorData.getInstance().isPreOutlandFight);
            base.transform.FindChild("15Panel").gameObject.SetActive(ActorData.getInstance().isPreOutlandFight);
            if (ActorData.getInstance().isPreOutlandFight)
            {
                UILabel component = base.transform.FindChild("15Panel/Info/Name").gameObject.GetComponent<UILabel>();
                if (component != null)
                {
                    component.text = outlandMapType.name;
                }
                UILabel label2 = base.transform.FindChild("15Panel/Info/Introduce/Label").gameObject.GetComponent<UILabel>();
                if (label2 != null)
                {
                    label2.text = outlandMapType.outland_desc;
                }
                base.transform.FindChild("15Panel/Info/FBicon").gameObject.GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateOutlandIcon(outlandMapType.icon);
            }
            else
            {
                ArrayList list = ConfigMgr.getInstance().getList<outland_config>();
                if (list != null)
                {
                    List<outland_config> list2 = new List<outland_config>();
                    IEnumerator enumerator = list.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            object current = enumerator.Current;
                            list2.Add((outland_config) current);
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable == null)
                        {
                        }
                        disposable.Dispose();
                    }
                    List<int> list3 = StrParser.ParseDecIntList(outlandMapType.limit_level, -1);
                    CommonFunc.DeleteChildItem(this._Grid.transform);
                    this._LabelName.text = outlandMapType.name;
                    this._LabelIntroduce.text = outlandMapType.outland_desc;
                    this.fbIcon.mainTexture = BundleMgr.Instance.CreateOutlandIcon(outlandMapType.icon);
                    int sort = 0;
                    foreach (int num2 in list3)
                    {
                        outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(ActorData.getInstance().outlandTitles[outlandMapType.entry].underway_entry);
                        this.isJxz = ActorData.getInstance().outlandTitles[outlandMapType.entry].is_underway && (_config.toll_gate_entry == sort);
                        GameObject go = UnityEngine.Object.Instantiate(this._SingleOutlandItem) as GameObject;
                        if (go != null)
                        {
                            go.transform.parent = this._Grid.transform;
                            go.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                            go.transform.localScale = new Vector3(1f, 1f, 1f);
                            bool isOpen = ActorData.getInstance().Level >= num2;
                            int[] numArray = new int[] { 0x1c, 40, 0x37 };
                            int index = (ActorData.getInstance().Level - 0x19) / 15;
                            index = (index <= 2) ? index : 2;
                            index = (index >= 0) ? index : 0;
                            bool isRecommended = num2 == numArray[index];
                            this.SetGridItemInfo(isOpen, isRecommended, go, outlandMapType, sort, this.isJxz, num2);
                            sort++;
                        }
                    }
                }
                this._Grid.repositionNow = true;
            }
        }
    }

    private void shopBtnClick(GameObject go)
    {
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (GUIEntity obj) {
                ((ShopPanel) obj).SetShopType(ShopCoinType.OutLandCoin);
                SocketMgr.Instance.RequestOutlandShopInfo();
            };
        }
        GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cache7);
    }

    private void startBtnClick(GameObject go)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<outland_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                <startBtnClick>c__AnonStorey1C8 storeyc = new <startBtnClick>c__AnonStorey1C8 {
                    temp = current as outland_config
                };
                if ((storeyc.temp.toll_gate_entry == 0) && (storeyc.temp.outland_type == 3))
                {
                    if ((ActorData.getInstance().outlandTitles.Find(new Predicate<OutlandTitle>(storeyc.<>m__2D0)).remain > 0) || ActorData.getInstance().outlandTitles[storeyc.temp.outland_type].is_underway)
                    {
                        ActorData.getInstance().outlandPageEntry = storeyc.temp.entry;
                        ActorData.getInstance().bOpenThirdPanel = true;
                        SocketMgr.Instance.RequestOutlandPageMapReq(storeyc.temp.entry);
                    }
                    else
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e34));
                    }
                    return;
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public void UpdateFameCoin()
    {
    }

    [CompilerGenerated]
    private sealed class <EnterOutlandLevel>c__AnonStorey1C9
    {
        internal outland_config temp;

        internal bool <>m__2D2(OutlandTitle o)
        {
            return (o.entry == this.temp.outland_type);
        }
    }

    [CompilerGenerated]
    private sealed class <startBtnClick>c__AnonStorey1C8
    {
        internal outland_config temp;

        internal bool <>m__2D0(OutlandTitle o)
        {
            return (o.entry == this.temp.outland_type);
        }
    }
}

