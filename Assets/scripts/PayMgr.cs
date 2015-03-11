using fastJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class PayMgr : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache5;
    [CompilerGenerated]
    private static System.Action <>f__am$cache6;
    [CompilerGenerated]
    private static System.Action <>f__am$cache7;
    private static int inPurchaseIndex;
    public static PayMgr Instance;
    public static bool IsInPurchase;
    public List<ProductInfo> productList = new List<ProductInfo>();
    private Dictionary<int, int> sendPoints = new Dictionary<int, int>();

    private void Awake()
    {
        Instance = this;
    }

    [DebuggerHidden]
    private IEnumerator DoNextGetPayFlag()
    {
        return new <DoNextGetPayFlag>c__IteratorA5();
    }

    public void FinishAndUnlockPayUI()
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = delegate {
                IsInPurchase = false;
                WaitPanelHelper.HideWaitPanel("PurchaseProductOnTx");
                GUIMgr.Instance.UnLock();
            };
        }
        this.DelayCallBack(1f, <>f__am$cache5);
    }

    public ProductInfo GetProductByIdentifier(string identifier)
    {
        foreach (ProductInfo info in this.productList)
        {
            if (info.identifier == identifier)
            {
                return info;
            }
        }
        return null;
    }

    public int GetSendPoints(int points)
    {
        int num = 0;
        if (this.sendPoints.TryGetValue(points, out num))
        {
            return num;
        }
        return 0;
    }

    public void Init()
    {
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    [DebuggerHidden]
    private IEnumerator OnPurchaseProductDelayBack(float time, int curPurchaseIndex)
    {
        return new <OnPurchaseProductDelayBack>c__IteratorA4 { time = time, curPurchaseIndex = curPurchaseIndex, <$>time = time, <$>curPurchaseIndex = curPurchaseIndex, <>f__this = this };
    }

    public void OnPurchaseProductOnTxFailedResult(int error)
    {
        Debug.Log("OnPurchaseProductOnTxFailedResult");
        this.FinishAndUnlockPayUI();
    }

    public void OnPurchaseProductOnTxResult(string openKey, string payToken, string pf, string pfKey, string sig, int time)
    {
        Debug.Log("OnPurchaseProductOnTxResult");
        this.FinishAndUnlockPayUI();
        SocketMgr.Instance.SendQueryTxBalance();
        this.RequstMPInfo();
        this.DelayCallBack(3f, delegate {
            SocketMgr.Instance.SendQueryTxBalance();
            this.RequstMPInfo();
        });
        this.DelayCallBack(10f, delegate {
            SocketMgr.Instance.SendQueryTxBalance();
            this.RequstMPInfo();
        });
    }

    public void OnPurchaseVIPOnAndroidResult(string openKey, string payToken, string pf, string pfKey, string sig, int time)
    {
        SocketMgr.Instance.SendQueryTxBalance();
        SocketMgr.Instance.SendQueryVIP();
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = delegate {
                SocketMgr.Instance.SendQueryTxBalance();
                SocketMgr.Instance.SendQueryVIP();
            };
        }
        this.DelayCallBack(3f, <>f__am$cache6);
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate {
                SocketMgr.Instance.SendQueryTxBalance();
                SocketMgr.Instance.SendQueryVIP();
            };
        }
        this.DelayCallBack(10f, <>f__am$cache7);
    }

    public void OnRequstMPInfo(string info)
    {
        this.sendPoints.Clear();
        object obj2 = JSON.Instance.ToObject<Dictionary<string, object>>(info)["mp_info"];
        Dictionary<string, object> dictionary2 = (Dictionary<string, object>) obj2;
        object obj3 = dictionary2["utp_mpinfo"];
        List<object> list = (List<object>) obj3;
        foreach (object obj4 in list)
        {
            Dictionary<string, object> dictionary3 = (Dictionary<string, object>) obj4;
            this.sendPoints.Add(StrParser.ParseDecInt(dictionary3["num"].ToString()), StrParser.ParseDecInt(dictionary3["send_num"].ToString()));
        }
        VipCardPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<VipCardPanel>();
        if (null != gUIEntity)
        {
            gUIEntity.Refresh();
        }
    }

    public void ParseAppProducts(List<object> content)
    {
        this.productList.Clear();
        foreach (object obj2 in content)
        {
            Dictionary<string, object> dictionary = (Dictionary<string, object>) obj2;
            string str = string.Empty;
            if (dictionary.ContainsKey("ProductId"))
            {
                str = dictionary["ProductId"].ToString();
            }
            else
            {
                Debug.LogWarning("ProductId");
            }
            string str2 = string.Empty;
            if (dictionary.ContainsKey("Price"))
            {
                str2 = dictionary["Price"].ToString();
            }
            else
            {
                Debug.LogWarning("Price");
            }
            string str3 = string.Empty;
            if (dictionary.ContainsKey("Points"))
            {
                str3 = dictionary["Points"].ToString();
            }
            else
            {
                Debug.LogWarning("Points");
            }
            string str4 = string.Empty;
            if (dictionary.ContainsKey("Gift"))
            {
                str4 = dictionary["Gift"].ToString();
            }
            else
            {
                Debug.LogWarning("Gift");
            }
            string str5 = string.Empty;
            if (dictionary.ContainsKey("PriceName"))
            {
                str5 = dictionary["PriceName"].ToString();
            }
            else
            {
                Debug.LogWarning("PriceName");
            }
            string str6 = string.Empty;
            if (dictionary.ContainsKey("Type"))
            {
                str6 = dictionary["Type"].ToString();
            }
            else
            {
                Debug.LogWarning("Type");
            }
            string str7 = string.Empty;
            if (dictionary.ContainsKey("ServiceCode"))
            {
                str7 = dictionary["ServiceCode"].ToString();
            }
            else
            {
                Debug.LogWarning("ServiceCode");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ProductInfo item = new ProductInfo {
                    identifier = str
                };
                if (!string.IsNullOrEmpty(str6))
                {
                    item.type = Convert.ToInt32(str6);
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    float result = 0f;
                    float.TryParse(str2, out result);
                    item.RMBPrice = (int) result;
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    item.points = Convert.ToInt32(str3);
                }
                item.serviceCode = str7;
                item.gift = str4;
                item.title = str5;
                this.productList.Add(item);
            }
        }
    }

    public void PurchaseProduct(ProductInfo pinfo)
    {
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            this.PurchaseProductOnTx(pinfo);
        }
    }

    public void PurchaseProductOnTx(ProductInfo pinfo)
    {
        Debug.Log("PurchaseProductOnTx");
        WaitPanelHelper.ShowWaitPanel("PurchaseProductOnTx");
        IsInPurchase = true;
        GUIMgr.Instance.Lock();
        inPurchaseIndex++;
        base.StartCoroutine(this.OnPurchaseProductDelayBack(15f, inPurchaseIndex));
        PlatformInterface.mInstance.PlatformBuyProduct(pinfo.identifier, string.Empty);
    }

    public void RequstMPInfo()
    {
        PlatformInterface.mInstance.PlatformGetMpInfo();
    }

    public void StartNextGetPayFlag()
    {
        base.StartCoroutine(this.DoNextGetPayFlag());
    }

    public string[] productIdentifiers
    {
        get
        {
            List<string> list = new List<string>();
            foreach (ProductInfo info in this.productList)
            {
                list.Add(info.identifier);
            }
            return list.ToArray();
        }
    }

    [CompilerGenerated]
    private sealed class <DoNextGetPayFlag>c__IteratorA5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    return true;

                case 1:
                    SocketMgr.Instance.SendQueryCaifuTong();
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnPurchaseProductDelayBack>c__IteratorA4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>curPurchaseIndex;
        internal float <$>time;
        internal PayMgr <>f__this;
        internal int curPurchaseIndex;
        internal float time;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.$current = new WaitForSeconds(this.time);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (PayMgr.IsInPurchase && (this.curPurchaseIndex == PayMgr.inPurchaseIndex))
                    {
                        this.<>f__this.FinishAndUnlockPayUI();
                        Debug.Log("OnPurchaseProductDelayBack");
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

