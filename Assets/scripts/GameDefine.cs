using FastBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameDefine
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache37;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache38;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache39;
    private string APPID_QQ = string.Empty;
    private string APPID_WEIXIN = string.Empty;
    public bool bundleExternal;
    public string bundleURL = string.Empty;
    public string bundleVersion = string.Empty;
    public int childPlatform;
    public string clientChannel;
    public string clientChannelGUEST;
    public string clientChannelQQ;
    public string clientChannelWEIXIN;
    public PlatformType clientPlatformType;
    public int commonDownloadGameProgress;
    public int CurGameID;
    public string device_mac;
    public string device_model;
    public string gameId = "66";
    private int GameID_Android;
    private int GameID_IOS;
    public string gameServerIP = string.Empty;
    public string gameServerPort = string.Empty;
    private static GameDefine instance;
    private string ios_idfa = string.Empty;
    public bool isAppStore;
    public static bool isBeta = true;
    public bool isDebugLog;
    public bool isLocoJoyPlatformPay;
    public bool IsLogining;
    public bool isNeedUpdateFromYYB;
    public bool isReleaseServer = true;
    public bool isThirdPlatform;
    public bool isUpdateMsgBoxShow;
    public bool isYYBUpdateDownloading;
    public long lastAcctId;
    public DateTime lastLoginFailedSecond;
    public int loginFailedGroupTimes;
    public int loginFailedTimes = -1;
    public int MemorySizeLimit = 0x400;
    public static bool OpenActiveCode;
    public int saveDownloadAppProgress;
    public string sessionId = string.Empty;
    public string TencentLoginNickName = string.Empty;
    public string TencentLoginSelfPic = string.Empty;
    public string TencentLoginSelfPicLarge = string.Empty;
    public string TencentLoginSelfPicMiddle = string.Empty;
    public string TencentLoginSelfPicSmall = string.Empty;
    public TencentType tencentType;
    public string tx_channelId;
    public string tx_openKey;
    public string tx_pf;
    public string tx_pfKey;
    public string tx_qqPayToken;
    public string url_ac_locojoy_com = "192.168.28.221:38081";
    public List<string> url_webServer_ips = new List<string>();
    public bool use_macaddr_login;
    private byte version4;

    private GameDefine()
    {
    }

    private void _OnDebugLogCallback(string name, string stack, LogType type)
    {
        if (((type != LogType.Log) && (type != LogType.Warning)) || getInstance().isDebugLog)
        {
            PlatformInterface.mInstance.OnDebugLog(type, type.ToString() + " " + name + " " + stack);
        }
    }

    public void CheckLoginFailedData()
    {
        if (this.loginFailedTimes == -1)
        {
            this.loginFailedTimes = PlayerPrefs.GetInt("loginFailedTimes", 0);
            this.loginFailedGroupTimes = PlayerPrefs.GetInt("loginFailedGroupTimes", 0);
            string str = PlayerPrefs.GetString("lastLoginFailedSecond", string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                this.lastLoginFailedSecond = new DateTime(Convert.ToInt64(str));
            }
        }
    }

    public bool CheckLoginFailedTime(out int outSeconds)
    {
        this.CheckLoginFailedData();
        if (this.loginFailedGroupTimes == 1)
        {
            if (this.loginFailedTimes >= 3)
            {
                int loginFailedTimeToNow = this.GetLoginFailedTimeToNow();
                if (loginFailedTimeToNow > 300)
                {
                    this.SetLoginFailedGroupTimes(0);
                    this.SetLoginFailedTimes(0);
                    outSeconds = 0;
                    return true;
                }
                outSeconds = 300 - loginFailedTimeToNow;
                return false;
            }
            outSeconds = 0;
            return true;
        }
        if (this.loginFailedTimes >= 3)
        {
            int num2 = this.GetLoginFailedTimeToNow();
            if (num2 > 10)
            {
                this.SetLoginFailedGroupTimes(1);
                this.SetLoginFailedTimes(0);
                outSeconds = 0;
                return true;
            }
            outSeconds = 10 - num2;
            return false;
        }
        outSeconds = 0;
        return true;
    }

    public string GetInstallURL()
    {
        return ServerInfo.getInstance().update_url;
    }

    public static GameDefine getInstance()
    {
        if (instance == null)
        {
            instance = new GameDefine();
            instance.Init();
        }
        return instance;
    }

    public string GetIOSIDFA()
    {
        return this.ios_idfa;
    }

    public int GetLoginFailedTimeToNow()
    {
        TimeSpan span = (TimeSpan) (DateTime.Now - this.lastLoginFailedSecond);
        return (int) span.TotalSeconds;
    }

    public string getPhysicalAddress()
    {
        string str = string.Empty;
        try
        {
            foreach (NetworkInterface interface2 in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = interface2.GetPhysicalAddress().ToString();
                }
            }
        }
        catch (Exception)
        {
        }
        return str;
    }

    public string GetPlayerInfoURL()
    {
        return ServerInfo.getInstance().playerInfo_url;
    }

    public string GetRegistWebUrl()
    {
        if (this.clientPlatformType != PlatformType.P_Test)
        {
            return string.Empty;
        }
        if (isBeta)
        {
            return "http://testaccountreg.locojoy.com/Register.ashx?";
        }
        return "http://accountjointsapi.locojoy.com/Register.ashx?";
    }

    public string GetShareURL()
    {
        return ServerInfo.getInstance().share_url;
    }

    public TencentType GetTencentType()
    {
        return this.tencentType;
    }

    public string getVersionDesc()
    {
        return string.Format("Ver:{0}", this.clientVersion);
    }

    protected void Init()
    {
        Time.timeScale = 1f;
        this.LoadMainVersion();
        this.InitDebug();
        this.device_mac = TypeConvertUtil.getMd5Hash(SystemInfo.deviceUniqueIdentifier);
        this.device_mac = TypeConvertUtil.formatMd5(this.device_mac);
        this.device_model = "android";
        PlatformInterface.mInstance.PlatformChecNeedkYYBUpdate();
        Debug.Log(" USE __ENABLE_DATATYPE_ENC__");
    }

    private void InitDebug()
    {
        Application.RegisterLogCallback(new Application.LogCallback(this._OnDebugLogCallback));
    }

    public bool IsAppStore()
    {
        return this.isAppStore;
    }

    public bool IsCanShowQQVIP()
    {
        return ((this.tencentType == TencentType.QQ) && this.isReleaseServer);
    }

    public bool IsLocoJoyPlatformPay()
    {
        return this.isLocoJoyPlatformPay;
    }

    public bool IsThirdPlatform()
    {
        return this.isThirdPlatform;
    }

    public void LoadMainVersion()
    {
        IniParser parser = new IniParser(Path.Combine(Application.streamingAssetsPath, "MT2Serverconfig.txt"), false, true);
        this.clientPlatformType = StrParser.ParseEnum<PlatformType>(parser.GetSetting("BUILD", "PLATFORM"));
        this.url_ac_locojoy_com = parser.GetSetting("BUILD", "HOST");
        this.url_webServer_ips = StrParser.ParseStringList(parser.GetSetting("BUILD", "IP"));
        this.clientChannel = parser.GetSetting("BUILD", "CHANNEL");
        this.clientChannelQQ = parser.GetSetting("BUILD", "CHANNELQQ");
        this.clientChannelWEIXIN = parser.GetSetting("BUILD", "CHANNELWEXIN");
        this.clientChannelGUEST = parser.GetSetting("BUILD", "CHANNELGUEST");
        this.isThirdPlatform = StrParser.ParseBool(parser.GetSetting("BUILD", "ISTHIRDPLATFORM"), false);
        this.GameID_Android = StrParser.ParseDecInt(parser.GetSetting("BUILD", "GAMEIDANDROID"), 0);
        this.GameID_IOS = StrParser.ParseDecInt(parser.GetSetting("BUILD", "GAMEIDIOS"), 0);
        this.APPID_QQ = parser.GetSetting("BUILD", "APPIDQQ");
        this.APPID_WEIXIN = parser.GetSetting("BUILD", "APPIDWEIXIN");
        this.CurGameID = this.GameID_IOS;
        this.bundleExternal = StrParser.ParseBool(parser.GetSetting("BUNDLE", "EXTERNAL"), false);
        this.bundleURL = parser.GetSetting("BUNDLE", "URL");
        this.bundleVersion = parser.GetSetting("BUNDLE", "VERSION");
        this.isDebugLog = StrParser.ParseBool(parser.GetSetting("DEBUG", "LOG"), false);
        this.MemorySizeLimit = StrParser.ParseDecInt(parser.GetSetting("DEBUG", "MEMORYSIZELIMIT"), 0x400);
    }

    public void OnGameLoginFailed()
    {
        this.CheckLoginFailedData();
        this.SetLoginFailedTimes(this.loginFailedTimes + 1);
        this.lastLoginFailedSecond = DateTime.Now;
        PlayerPrefs.SetString("lastLoginFailedSecond", this.lastLoginFailedSecond.Ticks.ToString());
    }

    public void OnGameLoginOK()
    {
        this.SetLoginFailedGroupTimes(0);
        this.SetLoginFailedTimes(0);
    }

    public void OnGetNickName(string name)
    {
        this.TencentLoginNickName = name;
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            LoginPanel component = obj2.GetComponent<LoginPanel>();
            if (component != null)
            {
                component.OnGetNickName(name);
            }
        }
    }

    public void OnGetSelfPic(string selfPic)
    {
        this.TencentLoginSelfPic = selfPic;
    }

    public void OnLoginFailed(int errorType)
    {
        this.IsLogining = false;
        Debug.Log("OnLoginFailed");
        GUIMgr.Instance.UnLock();
        WaitPanelHelper.HideAll();
        CommonFunc.TencentLoginCallBack(errorType);
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            LoginPanel component = obj2.GetComponent<LoginPanel>();
            if (component != null)
            {
                component.SetTencentGroup(true);
            }
        }
    }

    public void OnLoginOK(int type, string openKey, string pf, string pfKey, string payToken, string txChannelId, string idfa)
    {
        this.tx_openKey = openKey;
        this.tx_pf = pf;
        this.tx_pfKey = pfKey;
        this.tx_qqPayToken = payToken;
        if (this.IsLogining)
        {
            this.IsLogining = false;
            GUIMgr.Instance.UnLock();
            if (type == 0)
            {
                this.tencentType = TencentType.QQ;
                this.clientChannel = this.clientChannelQQ;
                this.clientPlatformType = PlatformType.P_QQ_Android;
                TssSDKInterface.OnLoginOK(TencentType.QQ, this.lastAccountName, this.APPID_QQ);
            }
            else if (type == 1)
            {
                this.tencentType = TencentType.WEIXIN;
                this.clientChannel = this.clientChannelWEIXIN;
                this.clientPlatformType = PlatformType.P_WC_Android;
                TssSDKInterface.OnLoginOK(TencentType.WEIXIN, this.lastAccountName, this.APPID_WEIXIN);
            }
            else if (type == 2)
            {
                this.tencentType = TencentType.GUEST;
                this.clientChannel = this.clientChannelGUEST;
                this.clientPlatformType = PlatformType.P_iOS_Guest;
            }
            this.tx_channelId = txChannelId;
            WaitPanelHelper.HideAll();
            GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
            if (obj2 != null)
            {
                LoginPanel component = obj2.GetComponent<LoginPanel>();
                if (component != null)
                {
                    component.StartDownLoadServerList();
                }
            }
        }
        else
        {
            SocketMgr.Instance.SendQueryTxBalance();
        }
    }

    public void OnTxTokenRefresh(int flag, string openKey)
    {
        this.tx_openKey = openKey;
        if (this.IsLogining)
        {
            getInstance().OnLoginFailed(flag);
        }
        else
        {
            SocketMgr.Instance.SendQueryTxBalance();
        }
    }

    public void PlatfromSwitchAccountNotify(string args)
    {
        if (<>f__am$cache37 == null)
        {
            <>f__am$cache37 = delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                if (<>f__am$cache38 == null)
                {
                    <>f__am$cache38 = box => LoginPanel.CheckSwitchAccountYes();
                }
                if (<>f__am$cache39 == null)
                {
                    <>f__am$cache39 = box => LoginPanel.CheckSwitchAccountNo();
                }
                box.SetDialog(ConfigMgr.getInstance().GetWord(0x19d), <>f__am$cache38, <>f__am$cache39, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache37, null);
    }

    public void SaveMainVersion()
    {
        IniParser parser = new IniParser(Path.Combine(Application.streamingAssetsPath, "MT2Serverconfig.txt"), false, true);
        parser.AddSetting("BUILD", "PLATFORM", this.clientPlatformType.ToString());
        parser.AddSetting("BUILD", "IP", this.url_ac_locojoy_com.ToString());
        parser.AddSetting("BUILD", "CHANNEL", this.clientChannel);
        parser.AddSetting("BUILD", "ISTHIRDPLATFORM", this.isThirdPlatform);
        parser.AddSetting("BUNDLE", "EXTERNAL", this.bundleExternal.ToString());
        parser.AddSetting("BUNDLE", "VERSION", this.bundleVersion);
        parser.AddSetting("BUNDLE", "URL", this.bundleURL);
        parser.SaveSettings();
    }

    private void SetLoginFailedGroupTimes(int times)
    {
        this.loginFailedGroupTimes = times;
        PlayerPrefs.SetInt("loginFailedGroupTimes", this.loginFailedGroupTimes);
    }

    private void SetLoginFailedTimes(int times)
    {
        this.loginFailedTimes = times;
        PlayerPrefs.SetInt("loginFailedTimes", this.loginFailedTimes);
    }

    public void SetVersion4(byte v4)
    {
    }

    public void SetVersionByString(string version)
    {
        char[] separator = new char[] { '.' };
        string[] strArray = version.Split(separator);
        if (strArray.Length >= 4)
        {
            this.SetVersion4((byte) StrParser.ParseDecInt(strArray[3]));
        }
    }

    public int AccountIsActiveCode
    {
        get
        {
            return PlayerPrefs.GetInt(getInstance().lastAccountName + "IsActiveCode", 0);
        }
        set
        {
            PlayerPrefs.SetInt(getInstance().lastAccountName + "IsActiveCode", value);
        }
    }

    public string clientVersion
    {
        get
        {
            return string.Format("{0}.{1}.{2}", 1, 4, 7);
        }
    }

    public string lastAccountName
    {
        get
        {
            return WWW.UnEscapeURL(PlayerPrefs.GetString("lastAccountName"));
        }
        set
        {
            PlayerPrefs.SetString("lastAccountName", WWW.EscapeURL(value));
        }
    }

    public int lastIsMacaddrLogin
    {
        get
        {
            return PlayerPrefs.GetInt("lastIsMacaddrLogin", 0);
        }
        set
        {
            PlayerPrefs.SetInt("lastIsMacaddrLogin", value);
        }
    }

    public string lastPassWord
    {
        get
        {
            return WWW.UnEscapeURL(PlayerPrefs.GetString("_lastPassWord"));
        }
        set
        {
            PlayerPrefs.SetString("_lastPassWord", WWW.EscapeURL(value));
        }
    }

    public string PatchVersion
    {
        get
        {
            return (this.WholeVersion + ".0");
        }
    }

    public string WholeVersion
    {
        get
        {
            object[] args = new object[] { 1, 4, 7, (int) this.version4 };
            return string.Format("{0}.{1}.{2}.{3}", args);
        }
    }
}

