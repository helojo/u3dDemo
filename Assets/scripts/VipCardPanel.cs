using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VipCardPanel : GUIEntity
{
    private UIGrid _gridObject;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache5;
    private List<GameObject> grids = new List<GameObject>();
    public GameObject itemPrefab;
    private VipRemainTime svip;
    private VipRemainTime vip;

    private int CompareFunction(ProductInfo l, ProductInfo r)
    {
        if (l.type != r.type)
        {
            return (this.ProductLevel(l) - this.ProductLevel(r));
        }
        return (l.points - r.points);
    }

    private void DeleteAllItems()
    {
        CommonFunc.DeleteChildItem(this.GridObject.transform);
    }

    private GameObject GetGrid(ProductInfo info)
    {
        <GetGrid>c__AnonStorey270 storey = new <GetGrid>c__AnonStorey270 {
            info = info
        };
        return this.grids.Find(new Predicate<GameObject>(storey.<>m__5AA));
    }

    private string GetStoneIconName(int points)
    {
        if (points <= 100)
        {
            return "Item_Icon_Stone";
        }
        if ((points > 100) && (points <= 0x3e8))
        {
            return "Item_Icon_Stone1";
        }
        if ((points > 0x3e8) && (points <= 0x1f40))
        {
            return "Item_Icon_Stone2";
        }
        return "Item_Icon_Stone3";
    }

    private void OnClickItem(GameObject go)
    {
        StonePurchaseContext component = go.GetComponent<StonePurchaseContext>();
        PayMgr.Instance.PurchaseProduct(component.info);
    }

    private void OnCreate()
    {
        List<ProductInfo> productList = PayMgr.Instance.productList;
        if (ConfigMgr.getInstance().getByEntry<product_vip_config>(0) == null)
        {
            Debug.LogError("invalid vip configure , platform = 0");
        }
        else
        {
            this.DeleteAllItems();
            productList.Sort(new Comparison<ProductInfo>(this.CompareFunction));
            long now = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            long num2 = ActorData.getInstance().VipData_MonthCard.month_card_1;
            long num3 = ActorData.getInstance().VipData_MonthCard.month_card_2;
            foreach (ProductInfo info in productList)
            {
                GameObject item = UnityEngine.Object.Instantiate(this.itemPrefab) as GameObject;
                this.grids.Add(item);
                item.transform.parent = this.GridObject.transform;
                item.transform.localPosition = new Vector3(0f, 0f, 0f);
                item.transform.localScale = new Vector3(1.06f, 1.06f, 1.06f);
                item.AddComponent<StonePurchaseContext>().info = info;
                UIEventListener listener1 = UIEventListener.Get(item);
                listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickItem));
                this.RefreshGridData(item, info, now, num2, num3);
            }
            this.GridObject.Reposition();
            base.transform.FindChild("VipInfomation").GetComponent<VipInfomation>().Refresh();
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnInitialize()
    {
        this.OnCreate();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void OpenVipDescPanel()
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = gui => VipDescriptPanel panel = (VipDescriptPanel) gui;
        }
        GUIMgr.Instance.PushGUIEntity("VipDescriptPanel", <>f__am$cache5);
    }

    private int ProductLevel(ProductInfo info)
    {
        if (info.type == 1)
        {
            return 0;
        }
        if (info.type == 2)
        {
            return 1;
        }
        if (info.type == 3)
        {
            return 2;
        }
        return 3;
    }

    public void Refresh()
    {
        List<ProductInfo> productList = PayMgr.Instance.productList;
        if (ConfigMgr.getInstance().getByEntry<product_vip_config>(0) == null)
        {
            Debug.LogError("invalid vip configure , platform = 0");
        }
        else
        {
            long now = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime);
            long num2 = ActorData.getInstance().VipData_MonthCard.month_card_1;
            long num3 = ActorData.getInstance().VipData_MonthCard.month_card_2;
            foreach (ProductInfo info in productList)
            {
                GameObject grid = this.GetGrid(info);
                this.RefreshGridData(grid, info, now, num2, num3);
            }
            base.transform.FindChild("VipInfomation").GetComponent<VipInfomation>().Refresh();
        }
    }

    private void RefreshGridData(GameObject go, ProductInfo product, long now, long vip1_time, long vip2_time)
    {
        UISprite component = go.transform.FindChild("iconr").GetComponent<UISprite>();
        UILabel label = go.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label2 = go.transform.FindChild("price").GetComponent<UILabel>();
        UISprite sprite2 = go.transform.FindChild("frame").GetComponent<UISprite>();
        UILabel label3 = go.transform.FindChild("info").GetComponent<UILabel>();
        UITexture texture = go.transform.FindChild("ex").GetComponent<UITexture>();
        UILabel label4 = go.transform.FindChild("remain/time").GetComponent<UILabel>();
        GameObject gameObject = go.transform.FindChild("remain").gameObject;
        Color color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
        Color color2 = (Color) new Color32(0x62, 0x92, 0xff, 0xff);
        Color color3 = (Color) new Color32(0xcd, 110, 0xfe, 0xff);
        if (product.type == 1)
        {
            label.text = ConfigMgr.getInstance().GetWord(0x31c);
            component.spriteName = "Ui_Shop_Icon_ptyk";
            texture.gameObject.SetActive(true);
            label3.gameObject.SetActive(true);
            label3.text = product.gift;
            gameObject.SetActive(false);
        }
        else if (product.type == 2)
        {
            label.text = ConfigMgr.getInstance().GetWord(0x31c);
            component.spriteName = "Ui_Shop_Icon_ptyk";
            texture.gameObject.SetActive(false);
            if (vip1_time < now)
            {
                label3.gameObject.SetActive(true);
                label3.text = product.gift;
                gameObject.SetActive(false);
            }
            else
            {
                label3.gameObject.SetActive(false);
                gameObject.SetActive(true);
                int num = ((int) (((vip1_time - now) / 0xe10L) / 0x18L)) + ((((vip1_time - now) % 0x15180L) != 0) ? 1 : 0);
                label4.text = string.Format(ConfigMgr.getInstance().GetWord(0xa344ea), num);
            }
        }
        else if (product.type == 3)
        {
            label.text = ConfigMgr.getInstance().GetWord(0x31d);
            component.spriteName = "Ui_Shop_Icon_gjyk";
            texture.gameObject.SetActive(false);
            if (vip2_time < now)
            {
                label3.gameObject.SetActive(true);
                label3.text = product.gift;
                gameObject.SetActive(false);
            }
            else
            {
                label3.gameObject.SetActive(false);
                gameObject.SetActive(true);
                int num2 = ((int) (((vip2_time - now) / 0xe10L) / 0x18L)) + ((((vip2_time - now) % 0x15180L) != 0) ? 1 : 0);
                label4.text = string.Format(ConfigMgr.getInstance().GetWord(0xa344ea), num2);
            }
        }
        else
        {
            label.text = product.points.ToString() + ConfigMgr.getInstance().GetWord(0x31b);
            component.spriteName = this.GetStoneIconName(product.points);
            int sendPoints = PayMgr.Instance.GetSendPoints(product.points);
            if (sendPoints == product.points)
            {
                texture.gameObject.SetActive(true);
            }
            else
            {
                texture.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
            if (sendPoints > 0)
            {
                string str = string.Format(ConfigMgr.getInstance().GetWord(0x3e8), sendPoints);
                label3.gameObject.SetActive(true);
                label3.text = str;
            }
            else
            {
                label3.gameObject.SetActive(false);
            }
        }
        label2.text = product.title;
    }

    public static string RemainTimeToString(int seconds)
    {
        if (seconds <= 0)
        {
            return string.Empty;
        }
        if (seconds < 60)
        {
            return ("(" + string.Format(ConfigMgr.getInstance().GetWord(0x71), seconds) + ")");
        }
        int num = seconds / 0x15180;
        seconds = seconds % 0x15180;
        int num2 = seconds / 0xe10;
        seconds = seconds % 0xe10;
        int num3 = seconds / 60;
        if (((num == 0) && (num2 == 0)) && (num3 == 0))
        {
            num3 = 1;
        }
        string str = "(";
        if (num > 0)
        {
            str = str + string.Format(ConfigMgr.getInstance().GetWord(0x65), num);
        }
        if (num2 > 0)
        {
            str = str + string.Format(ConfigMgr.getInstance().GetWord(0x66), num2);
        }
        if (num3 > 0)
        {
            str = str + string.Format(ConfigMgr.getInstance().GetWord(0x6d), num3);
        }
        return (str + ")");
    }

    private UIGrid GridObject
    {
        get
        {
            if (null == this._gridObject)
            {
                this._gridObject = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            }
            return this._gridObject;
        }
    }

    [CompilerGenerated]
    private sealed class <GetGrid>c__AnonStorey270
    {
        internal ProductInfo info;

        internal bool <>m__5AA(GameObject obj)
        {
            StonePurchaseContext component = obj.GetComponent<StonePurchaseContext>();
            return ((component != null) && (component.info == this.info));
        }
    }
}

