using fastJSON;
using System;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class PlatformInterface : MonoBehaviour
{
    [HideInInspector]
    private Action<int, string> actOnUpdateProgress;
    private const string ANDROID_JAVA_CLASS = "com.locojoy.mtd.UnityPlayerNativeActivity";
    [HideInInspector]
    public bool isLogin;
    public static PlatformInterface mInstance;
    private Dictionary<string, string> uncheckedOrders = new Dictionary<string, string>();

    private void Awake()
    {
        mInstance = this;
    }

    public void OnDebugLog(LogType type, string log)
    {
        AndroidJavaObject @static = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity").GetStatic<AndroidJavaObject>("currentActivity");
        if ((type == LogType.Error) || (type == LogType.Exception))
        {
            object[] args = new object[] { log };
            @static.Call("ReportEvent", args);
        }
        else
        {
            object[] objArray2 = new object[] { log };
            @static.Call("ReportLog", objArray2);
        }
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    public void PaltformInitCallBack(string args)
    {
    }

    public void PlatformBeforeLogin()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("BeforeLogin", new object[0]);
    }

    public void PlatformBindCafuTong(string discountType, string discountUrl)
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string str = string.Empty;
        dictionary["serverId"] = ServerInfo.lastGameServerId;
        dictionary["discountType"] = discountType;
        dictionary["discountUrl"] = discountUrl;
        str = JSON.Instance.ToJSON(dictionary);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { str };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("bindCardPay", args);
        Debug.Log("LJ PlatformBindCafuTong discountType:" + discountType + "|||discountUrl:" + discountUrl);
    }

    public void PlatformBindGroupCallback(string str)
    {
        Debug.Log("LJ-lcs PlatformBindGroupCallback args: " + str);
        GuildPanel.BindQQGroupCallback();
    }

    public void PlatformBindQQGroup(string strGuildId, string strGuildName)
    {
        Debug.Log("-----PlatformBindQQGroup------");
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string str = string.Empty;
        dictionary["strGuildId"] = strGuildId;
        dictionary["strGuildName"] = strGuildName;
        str = JSON.Instance.ToJSON(dictionary);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { str };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("BindQQGroup", args);
    }

    public void PlatformBindQQGroupCallBack(string agrs)
    {
        Debug.Log("-----PlatformBindQQGroupCallback------");
        GuildPanel.BindQQGroupCallback();
    }

    public void PlatformBuyProduct(string productId, string orderId)
    {
        ProductInfo productByIdentifier = PayMgr.Instance.GetProductByIdentifier(productId);
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string str = string.Empty;
        dictionary["productId"] = productId;
        dictionary["points"] = productByIdentifier.points;
        dictionary["serviceCode"] = productByIdentifier.serviceCode;
        dictionary["type"] = productByIdentifier.type;
        dictionary["serverId"] = ServerInfo.lastGameServerId;
        string word = string.Empty;
        if ((productByIdentifier.type == 1) || (productByIdentifier.type == 2))
        {
            word = ConfigMgr.getInstance().GetWord(0x31c);
        }
        else if (productByIdentifier.type == 3)
        {
            word = ConfigMgr.getInstance().GetWord(0x31d);
        }
        else
        {
            word = ConfigMgr.getInstance().GetWord(0x31b);
        }
        dictionary["name"] = word;
        str = JSON.Instance.ToJSON(dictionary);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { str };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("Pay", args);
    }

    private void PlatformBuyProductCallBack(string args)
    {
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(args);
        int error = int.Parse(dictionary["flag"].ToString());
        if (error == 0)
        {
            string openKey = dictionary["openKey"].ToString();
            string payToken = dictionary["payToken"].ToString();
            string pf = dictionary["pf"].ToString();
            string pfKey = dictionary["pfKey"].ToString();
            string sig = dictionary["sig"].ToString().Trim();
            int time = int.Parse(dictionary["time"].ToString());
            PayMgr.Instance.OnPurchaseProductOnTxResult(openKey, payToken, pf, pfKey, sig, time);
        }
        else
        {
            PayMgr.Instance.OnPurchaseProductOnTxFailedResult(error);
        }
    }

    public void PlatformBuyVipCallback(string args)
    {
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(args);
        int error = int.Parse(dictionary["flag"].ToString());
        if (error == 0)
        {
            string openKey = dictionary["openKey"].ToString();
            string payToken = dictionary["payToken"].ToString();
            string pf = dictionary["pf"].ToString();
            string pfKey = dictionary["pfKey"].ToString();
            string sig = dictionary["sig"].ToString().Trim();
            int time = int.Parse(dictionary["time"].ToString());
            PayMgr.Instance.OnPurchaseProductOnTxResult(openKey, payToken, pf, pfKey, sig, time);
        }
        else
        {
            PayMgr.Instance.OnPurchaseProductOnTxFailedResult(error);
        }
    }

    public void PlatformChecNeedkYYBUpdate()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("CheckNeedYYBUpdate", new object[0]);
        Debug.Log("LJ-yybgx PlatformChecNeedkYYBUpdate ");
    }

    public void PlatformGetMpInfo()
    {
        string str = string.Empty;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary["serverId"] = ServerInfo.lastGameServerId;
        str = JSON.Instance.ToJSON(dictionary);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { str };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("GetMpInfo", args);
    }

    public void PlatformGetMpInfoCallback(string args)
    {
        PayMgr.Instance.OnRequstMPInfo(args);
    }

    public void PlatformGetNicName()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("QueryMyInfo", new object[0]);
    }

    public string PlatformGetNoticeData(int type, string scene)
    {
        return string.Empty;
    }

    public void PlatformInit()
    {
        this.isLogin = false;
    }

    public bool PlatformIsQQInstalled()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { 0 };
        return class2.GetStatic<AndroidJavaObject>("currentActivity").Call<bool>("CheckPlatform", args);
    }

    public bool PlatformIsWeiXinInstalled()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { 1 };
        return class2.GetStatic<AndroidJavaObject>("currentActivity").Call<bool>("CheckPlatform", args);
    }

    public void PlatformLogin(TencentType type)
    {
        GameDefine.getInstance().IsLogining = true;
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { (int) type };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("Login", args);
    }

    public void PlatformLoginAuto()
    {
        GameDefine.getInstance().IsLogining = true;
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("LoginAuto", new object[0]);
    }

    public void PlatformLogout()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("Logout", new object[0]);
    }

    public void PlatformOpenWebView(string URL)
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { URL };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("openRQWebview", args);
    }

    public void PlatformQueryFriend()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("QueryFriendInfo", new object[0]);
    }

    public void PlatformQueryFriendCallback(string args)
    {
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(args);
        int num = int.Parse(dictionary["flag"].ToString());
        if (num == 0)
        {
            List<SocialPlatformFriend> list = new List<SocialPlatformFriend>(0x10);
            int num2 = int.Parse(dictionary["count"].ToString());
            for (int i = 0; i < num2; i++)
            {
                SocialPlatformFriend item = new SocialPlatformFriend();
                string str = "friendId" + i;
                item.OpenID = dictionary[str].ToString();
                str = "friendNic" + i;
                item.SocialName = dictionary[str].ToString();
                str = "friendSmall" + i;
                item.SamillIconID = dictionary[str].ToString();
                list.Add(item);
            }
            XSingleton<SocialFriend>.Singleton.CompletePlatformRequest(list);
        }
        else
        {
            XSingleton<SocialFriend>.Singleton.RequestPlatformFaiulre(string.Empty + num);
        }
    }

    public void PlatformQueryMyInfoCallback(string args)
    {
        Debug.Log("LJ PlatformQueryMyInfoCallback args: " + args);
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(args);
        if (int.Parse(dictionary["flag"].ToString()) == 0)
        {
            string name = dictionary["nicName"].ToString();
            string selfPic = dictionary["selfPic"].ToString();
            GameDefine.getInstance().OnGetNickName(name);
            GameDefine.getInstance().OnGetSelfPic(selfPic);
        }
    }

    public void PlatformShare(string type, string title, string summary, string targetUrl, string imgUrl)
    {
        string str = string.Empty;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary["type"] = type;
        dictionary["title"] = title;
        dictionary["summary"] = summary;
        dictionary["targetUrl"] = targetUrl;
        dictionary["imgUrl"] = imgUrl;
        str = JSON.Instance.ToJSON(dictionary);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { str };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("Share", args);
    }

    public void PlatformShareFriend(string title, string summary, string targetUrl, string imgUrl)
    {
        this.PlatformShare("1", title, summary, targetUrl, imgUrl);
    }

    public void PlatformShareZone(string title, string summary, string targetUrl, string imgUrl)
    {
        this.PlatformShare("0", title, summary, targetUrl, imgUrl);
    }

    public void PlatformYYBCommonDownloadGame()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("YYBCommonDownloadGame", new object[0]);
        Debug.Log("LJ-yybgx YYBCommonDownloadGame ");
    }

    public void PlatformYYBCommonDownloadGameProgressCallback(string str)
    {
        Debug.Log("LJ-yybgx YYBCommonDownloadGameProgress args: " + str);
        int num = int.Parse(JSON.Instance.ToObject<Dictionary<string, object>>(str)["CommonDownloadGameProgress"].ToString());
        GameDefine.getInstance().commonDownloadGameProgress = num;
        string word = ConfigMgr.getInstance().GetWord(0x186af);
        if (num == 100)
        {
            GameDefine.getInstance().isYYBUpdateDownloading = false;
        }
        if (this.actOnUpdateProgress != null)
        {
            this.actOnUpdateProgress(num, word);
        }
    }

    public void PlatformYYBSaveDownloadGame()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("YYBSaveDownloadGame", new object[0]);
        Debug.Log("LJ-yybgx YYBSaveDownloadGame ");
    }

    public void PlatformYYBSaveDownloadYYBProgressCallback(string str)
    {
        Debug.Log("LJ-yybgx PlatformYYBSaveDownloadYYBProgressCallback args: " + str);
        int num = int.Parse(JSON.Instance.ToObject<Dictionary<string, object>>(str)["SaveDownloadYYBProgress"].ToString());
        GameDefine.getInstance().saveDownloadAppProgress = num;
        if (num == 100)
        {
            GameDefine.getInstance().isYYBUpdateDownloading = false;
        }
        string word = ConfigMgr.getInstance().GetWord(0x186b0);
        if (this.actOnUpdateProgress != null)
        {
            this.actOnUpdateProgress(num, word);
        }
    }

    private void PlatfromExchangeAccount()
    {
    }

    public void PlatfromJoinQQGroup(string strOpenId)
    {
        Debug.Log("-----PlatfromJoinQQGroup------");
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        object[] args = new object[] { strOpenId };
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("JoinQQGroup", args);
    }

    private void PlatfromLoginCallBack(string args)
    {
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(args);
        int flag = -2;
        flag = int.Parse(dictionary["flag"].ToString());
        if (flag == 0)
        {
            object obj2;
            this.isLogin = true;
            GameDefine.getInstance().lastAccountName = dictionary["platform_user_id"].ToString();
            GameDefine.getInstance().sessionId = dictionary["session"].ToString();
            GameDefine.getInstance().lastAcctId = 0L;
            int type = int.Parse(dictionary["tencentType"].ToString());
            string idfa = string.Empty;
            if (dictionary.TryGetValue("idfa", out obj2))
            {
                idfa = obj2.ToString();
            }
            GameDefine.getInstance().OnLoginOK(type, dictionary["token"].ToString(), dictionary["pf"].ToString(), dictionary["pfKey"].ToString(), dictionary["qqPayToken"].ToString(), dictionary["channelId"].ToString(), idfa);
        }
        else if (flag == 0x7d5)
        {
            GameDefine.getInstance().OnTxTokenRefresh(flag, dictionary["token"].ToString());
        }
        else
        {
            GameDefine.getInstance().OnLoginFailed(flag);
        }
        GameDefine.getInstance().IsLogining = false;
    }

    private void PlatfromLoginOutCallBack(string agrs)
    {
        this.isLogin = false;
    }

    public void PlatfromShareCallBack(string args)
    {
        SharePanel.ShareCallBack(int.Parse(args));
    }

    private void PlatfromShowAccountCenter()
    {
    }

    public void PlatfromSwitchAccountNotify(string args)
    {
        GameDefine.getInstance().PlatfromSwitchAccountNotify(args);
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("SetSelectAccountFlag", new object[0]);
    }

    public void RefreshWXToken()
    {
        AndroidJavaClass class2 = new AndroidJavaClass("com.locojoy.mtd.UnityPlayerNativeActivity");
        class2.GetStatic<AndroidJavaObject>("currentActivity").Call("RefreshWXToken", new object[0]);
    }

    public void SetupUpdateProgressAction(Action<int, string> action)
    {
        this.actOnUpdateProgress = action;
    }

    public void ShowYYBDownloadDialog()
    {
        GameDefine.getInstance().isNeedUpdateFromYYB = true;
        Debug.Log("LJ-yybgx ShowYYBDownloadDialog ");
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

