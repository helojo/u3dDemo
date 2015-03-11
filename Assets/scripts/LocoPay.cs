using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LocoPay
{
    private const string kPAY_CHECK_URL = "http://mpay.locojoy.com/api/GetOrderResult.ashx";
    private static string kPAY_MD5_KEY = "fa5250412fa0b60a396dd29a165e972ec0e074eac2e5fc13";
    private const string kPAY_PAY_URL = "http://mpay.locojoy.com/Pay.aspx";

    private string GetPayParamster(string productId, string orderId)
    {
        object[] args = new object[] { ActorData.getInstance().SessionInfo.userid, orderId, GameDefine.getInstance().gameId, ServerInfo.getInstance().getCurrentGameServerInfo().serverId, productId, GameDefine.getInstance().clientChannel, GameDefine.getInstance().device_mac, kPAY_MD5_KEY };
        string str2 = TypeConvertUtil.getMd5Hash(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", args));
        object[] objArray2 = new object[] { ActorData.getInstance().SessionInfo.userid, orderId, GameDefine.getInstance().gameId, ServerInfo.getInstance().getCurrentGameServerInfo().serverId, productId, GameDefine.getInstance().clientChannel, GameDefine.getInstance().device_mac, str2, UnityEngine.Random.Range(0, 0x186a0) };
        return string.Format("UserId={0}&BillNo={1}&GameId={2}&GameServerId={3}&ItemId={4}&channalId={5}&mac={6}&sign={7}&R={8}", objArray2);
    }

    private string GetPurchaseProductURL(string productId, string orderId)
    {
        return string.Format("{0}?{1}", "http://mpay.locojoy.com/Pay.aspx", this.GetPayParamster(productId, orderId));
    }

    public void PurchaseProduct(string productId, string orderId)
    {
        <PurchaseProduct>c__AnonStorey277 storey = new <PurchaseProduct>c__AnonStorey277 {
            url = this.GetPurchaseProductURL(productId, orderId)
        };
        Debug.Log("locoPayURL: " + storey.url);
        GUIMgr.Instance.DoModelGUI("WebViewPanel", new Action<GUIEntity>(storey.<>m__5C5), null);
    }

    public static void testMD5()
    {
        object[] args = new object[] { 0xbebc317, "59a3a068-5f1d-4ced-843a-0d6ab52efe80", 50, 0x270f, "dgjq_locojoy_chs1", 0xbed4904, "TestMac1111", kPAY_MD5_KEY };
        if (TypeConvertUtil.getMd5Hash(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", args)) != "1b04275515061ef2d6080b80701134f8")
        {
            Debug.Log("error");
        }
        else
        {
            Debug.Log("OK!");
        }
    }

    [CompilerGenerated]
    private sealed class <PurchaseProduct>c__AnonStorey277
    {
        internal string url;

        internal void <>m__5C5(GUIEntity obj)
        {
            WebViewPanel panel = obj as WebViewPanel;
            Debug.Log(this.url);
            panel.Load(this.url);
        }
    }
}

