using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class LoginPanel : GUIEntity
{
    public GameObject _AccountLoginBtn;
    public GameObject _CurrAccountGroup;
    public GameObject _EnterGameBtn;
    public GameObject _SingleRoleItem;
    public GameObject _TencentGroup;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache14;
    [CompilerGenerated]
    private static Predicate<ServerInfo.GameServerInfo> <>f__am$cache15;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache16;
    private int m_currSelectSvrId = -1;
    private bool m_currSvrCanLogin;
    private UILabel m_dlMsgLable;
    private UISlider m_dlSlider;
    private float m_time = 1f;
    private float m_updateInterval = 3f;
    private bool mCheckBundle;
    public bool mCheckUserInfo;
    private bool mHaveRole;
    private bool mIsStart;
    public bool mLockBtnState;
    private float mWaitTencentReturnTime = 15f;
    public GameObject SingleServerListItem;
    public GameObject SingleSvrGroup;
    public bool TencentReturnOk;

    public void CheckPlatform()
    {
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            PlatformInterface.mInstance.PlatformLoginAuto();
            GUIMgr.Instance.Lock();
            this._TencentGroup.SetActive(false);
            this._EnterGameBtn.SetActive(false);
            Transform transform = this._TencentGroup.transform.FindChild("QQ");
            Transform transform2 = this._TencentGroup.transform.FindChild("WeiXin");
            Transform transform3 = this._TencentGroup.transform.FindChild("Guest");
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQBtn);
            UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickWeiXinBtn);
            if (transform3 != null)
            {
                this._TencentGroup.transform.localPosition = new Vector3(-97.3f, 0f, 0f);
                transform3.gameObject.SetActive(false);
            }
            this.CheckWeiXin();
            PlatformInterface.mInstance.PlatformBeforeLogin();
        }
        else
        {
            this._TencentGroup.SetActive(false);
            this._EnterGameBtn.SetActive(true);
            GUIMgr.Instance.DoModelGUI("AccountLoginPanel", null, null);
            this.StartDownLoadServerList();
        }
    }

    public static void CheckSwitchAccountNo()
    {
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            obj2.GetComponent<LoginPanel>().CheckPlatform();
        }
    }

    public static void CheckSwitchAccountYes()
    {
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            obj2.GetComponent<LoginPanel>().OnLogout();
        }
        else
        {
            GameStateMgr.IsGameReturnLogin = true;
            GameStateMgr.IsGameReturnLogout = true;
            GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
        }
    }

    private void CheckTencentReturn()
    {
        this.TencentReturnOk = false;
        this.DelayCallBack(3f, delegate {
            if (!this.TencentReturnOk)
            {
                WaitPanelHelper.ShowWaitPanel("CheckTencentReturn");
            }
        });
        this.DelayCallBack(this.mWaitTencentReturnTime, delegate {
            if (!this.TencentReturnOk)
            {
                WaitPanelHelper.HideWaitPanel("CheckTencentReturn");
                this.SetTencentGroup(true);
            }
        });
    }

    public void CheckWeiXin()
    {
    }

    public void ClearCurrAccount()
    {
        this._CurrAccountGroup.transform.FindChild("Label").GetComponent<UILabel>().text = string.Empty;
        this._CurrAccountGroup.SetActive(false);
        this._AccountLoginBtn.SetActive(false);
    }

    private void CloseServerList(GameObject go)
    {
        this.CloseServerListUI();
    }

    private void CloseServerListUI()
    {
        base.transform.FindChild("LoginGroup/ServerList").gameObject.SetActive(false);
    }

    private void DownloadSvrListFaild()
    {
        if (<>f__am$cache14 == null)
        {
            <>f__am$cache14 = delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                if (<>f__am$cache16 == null)
                {
                    <>f__am$cache16 = delegate (GameObject box) {
                    };
                }
                box.SetDialog("dddddddddd", <>f__am$cache16, null, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache14, base.gameObject);
    }

    private void EnableBtn()
    {
        base.gameObject.transform.FindChild("LoginGroup/EnterGameBtn").GetComponent<UIButton>().isEnabled = true;
        base.gameObject.transform.FindChild("LoginGroup/CurrServer").GetComponent<UIButton>().isEnabled = true;
        this.mLockBtnState = false;
    }

    public static LoginPanel G_GetLoginPanel()
    {
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            return obj2.GetComponent<LoginPanel>();
        }
        return null;
    }

    private int GetCurrNum(ServerInfo.GameServerInfo currInfo)
    {
        int num = 0;
        foreach (ServerInfo.GameServerInfo info in ServerInfo.getInstance().gameServerInfos)
        {
            if (info == currInfo)
            {
                return (num + 1);
            }
            num++;
        }
        return 0;
    }

    private ServerInfo.GameServerInfo GetPreLoginSvrInfo()
    {
        foreach (ServerInfo.GameServerInfo info in ServerInfo.getInstance().gameServerInfos)
        {
            if (info.serverId == ServerInfo.lastGameServerId)
            {
                return info;
            }
        }
        if (<>f__am$cache15 == null)
        {
            <>f__am$cache15 = e => e.index == ServerInfo.getInstance().gameServerMaxIndex;
        }
        return ServerInfo.getInstance().gameServerInfos.Find(<>f__am$cache15);
    }

    private string GetStatColor(int status)
    {
        if (status == 1)
        {
            return "[fe893d]";
        }
        if (status == 2)
        {
            return "[00ff00]";
        }
        if (status == 3)
        {
            return "[ff0000]";
        }
        if (status == 4)
        {
            return "[888888]";
        }
        return "[ffffff]";
    }

    public override void GUIStart()
    {
        this.SetBgImage(BundleMgr.Instance.CreateLoginTexture("Ui_Login_Bg_00"));
        base.transform.FindChild("TopLeft/versions").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x3e7) + " " + GameDefine.getInstance().WholeVersion + "+";
        UIEventListener.Get(base.transform.FindChild("LoginGroup/CurrServer").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSvrListBtn);
        Transform transform2 = base.transform.FindChild("LoginGroup/ServerList");
        UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.CloseServerList);
        UIEventListener.Get(transform2.FindChild("TuiJianBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTuiJianBtn);
        base.Ready();
    }

    public void InitRoleList(List<RoleInfo> roleInfoList)
    {
        UIGrid component = base.gameObject.transform.FindChild("LoginGroup/ServerList/RoleList/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        if (roleInfoList == null)
        {
            this.mHaveRole = false;
        }
        else
        {
            this.mHaveRole = roleInfoList.Count > 0;
            for (int i = 0; i < roleInfoList.Count; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(this._SingleRoleItem) as GameObject;
                go.transform.parent = component.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = new Vector3(1f, 1f, 1f);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(roleInfoList[i].head_entry);
                if (_config != null)
                {
                    go.transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
                UISprite frame = go.transform.FindChild("FrameBg").GetComponent<UISprite>();
                UISprite sprite2 = go.transform.FindChild("TagBg").GetComponent<UISprite>();
                CommonFunc.SetPlayerHeadFrame(frame, sprite2, roleInfoList[i].head_frame);
                go.transform.FindChild("Level").GetComponent<UILabel>().text = roleInfoList[i].level.ToString();
                go.transform.FindChild("name").GetComponent<UILabel>().text = roleInfoList[i].nick_name;
                ServerInfo.GameServerInfo gameServerInfoBySvrId = ServerInfo.getInstance().GetGameServerInfoBySvrId(roleInfoList[i].server_id);
                if (gameServerInfoBySvrId != null)
                {
                    string[] textArray1 = new string[] { this.GetStatColor(gameServerInfoBySvrId.status), gameServerInfoBySvrId.index.ToString(), ConfigMgr.getInstance().GetWord(0x241), "  ", gameServerInfoBySvrId.name };
                    go.transform.FindChild("SvrName").GetComponent<UILabel>().text = string.Concat(textArray1);
                    GUIDataHolder.setData(go, gameServerInfoBySvrId);
                }
                UIEventListener.Get(go).onClick = new UIEventListener.VoidDelegate(this.OnClickRoleItem);
            }
            component.repositionNow = true;
        }
    }

    public void InitServerList()
    {
        ActorData.getInstance().Clear();
        this.mCheckUserInfo = false;
        UIGrid component = base.gameObject.transform.FindChild("LoginGroup/ServerList/QuGroup/List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        int num = 0;
        int num2 = ServerInfo.getInstance().gameServerMaxIndex / 10;
        if ((ServerInfo.getInstance().gameServerMaxIndex % 10) != 0)
        {
            num2++;
        }
        Debug.Log("MaxSvrCount:" + ServerInfo.getInstance().gameServerMaxIndex);
        for (int i = num2; i > 0; i--)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleSvrGroup) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localPosition = new Vector3(0f, -num * component.cellHeight, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            Transform transform = obj2.transform.FindChild("Item");
            obj2.transform.FindChild("Item/Name").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x242), ((i - 1) * 10) + 1, i * 10);
            GUIDataHolder.setData(transform.gameObject, i - 1);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSvrGroupBtn);
            if (num == 0)
            {
                this.SetSvrListinfo(i - 1);
            }
            num++;
        }
        component.repositionNow = true;
        this.SetLoginSvrInfo();
        base.transform.FindChild("LoginGroup/CurrServer").gameObject.SetActive(true);
        this._EnterGameBtn.gameObject.SetActive(true);
        this.EnableBtn();
        if (GameStateMgr.IsGameReturnLogin)
        {
            this.ShowAccountName();
            Transform transform2 = this._TencentGroup.transform.FindChild("QQ");
            Transform transform3 = this._TencentGroup.transform.FindChild("WeiXin");
            UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQBtn);
            UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickWeiXinBtn);
            Transform transform4 = this._TencentGroup.transform.FindChild("Guest");
            if (transform4 != null)
            {
                this._TencentGroup.transform.localPosition = new Vector3(-97.3f, 0f, 0f);
                transform4.gameObject.SetActive(false);
            }
            this.CheckWeiXin();
        }
        if (GameStateMgr.IsGameReturnLogout)
        {
            PlatformInterface.mInstance.PlatformLogout();
            this._AccountLoginBtn.gameObject.SetActive(false);
            this._TencentGroup.gameObject.SetActive(true);
            base.transform.FindChild("LoginGroup/CurrServer").gameObject.SetActive(false);
            this._EnterGameBtn.SetActive(false);
            this._CurrAccountGroup.transform.FindChild("Label").GetComponent<UILabel>().text = string.Empty;
            this._CurrAccountGroup.SetActive(false);
        }
        GameStateMgr.IsGameReturnLogin = false;
        GameStateMgr.IsGameReturnLogout = false;
    }

    private void OnClickAccountLoginBtn()
    {
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(9), box => this.OnLogout(), null, false), null);
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("AccountLoginPanel", null, null);
        }
    }

    private void OnClickGuestBtn(GameObject go)
    {
        GUIMgr.Instance.Lock();
        PlatformInterface.mInstance.PlatformLogin(TencentType.GUEST);
        this.CheckTencentReturn();
    }

    private void OnClickListBtn(GameObject obj)
    {
        object obj2 = GUIDataHolder.getData(obj);
        if (obj2 != null)
        {
            ServerInfo.GameServerInfo info = (ServerInfo.GameServerInfo) obj2;
            this.SetSelectSvr(info);
            base.transform.FindChild("LoginGroup/ServerList").gameObject.SetActive(false);
        }
    }

    private void onClickLogin()
    {
        if (GameDefine.getInstance().gameServerIP == string.Empty)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xd8));
        }
        else
        {
            int num;
            if (!GameDefine.getInstance().CheckLoginFailedTime(out num))
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x34d), num));
            }
            else if (((GameDefine.getInstance().clientPlatformType == PlatformType.P_Test) && (GameDefine.getInstance().lastIsMacaddrLogin == 1)) && (GameDefine.getInstance().lastAccountName == string.Empty))
            {
                TipsDiag.SetText("Test平台不能使用游客帐号登陆！");
            }
            else
            {
                base.transform.FindChild("LoadingSlider").gameObject.SetActive(true);
                base.transform.FindChild("LoginGroup").gameObject.SetActive(false);
                BundleMgr.Instance.CheckBundles();
                this.mCheckBundle = true;
            }
        }
    }

    private void OnClickQQBtn(GameObject go)
    {
        GUIMgr.Instance.Lock();
        PlatformInterface.mInstance.PlatformLogin(TencentType.QQ);
        this.CheckTencentReturn();
    }

    private void OnClickRoleItem(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            ServerInfo.GameServerInfo info = (ServerInfo.GameServerInfo) obj2;
            this.SetSelectSvr(info);
            base.transform.FindChild("LoginGroup/ServerList").gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Data Error");
            base.transform.FindChild("LoginGroup/ServerList").gameObject.SetActive(false);
        }
    }

    private void OnClickSvrGroupBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int groupIdx = (int) obj2;
            if (groupIdx >= 0)
            {
                this.SetSvrListinfo(groupIdx);
                base.transform.FindChild("LoginGroup/ServerList/RoleList").gameObject.SetActive(false);
                base.transform.FindChild("LoginGroup/ServerList/List").gameObject.SetActive(true);
            }
        }
    }

    private void OnClickSvrListBtn(GameObject go)
    {
        base.transform.FindChild("LoginGroup/ServerList/List").gameObject.SetActive(!this.mHaveRole);
        base.transform.FindChild("LoginGroup/ServerList/RoleList").gameObject.SetActive(this.mHaveRole);
        base.transform.FindChild("LoginGroup/ServerList").gameObject.SetActive(true);
        base.transform.FindChild("LoginGroup/ServerList/TuiJianBtn").GetComponent<UIToggle>().isChecked = this.mHaveRole;
    }

    private void OnClickTuiJianBtn(GameObject go)
    {
        base.transform.FindChild("LoginGroup/ServerList/List").gameObject.SetActive(false);
        base.transform.FindChild("LoginGroup/ServerList/RoleList").gameObject.SetActive(true);
    }

    private void OnClickWeiXinBtn(GameObject go)
    {
        GUIMgr.Instance.Lock();
        PlatformInterface.mInstance.PlatformLogin(TencentType.WEIXIN);
        this.CheckTencentReturn();
    }

    public override void OnDestroy()
    {
        this.SetBgImage(null);
    }

    public void OnGetNickName(string name)
    {
        Debug.Log("GetNick " + name);
        this._CurrAccountGroup.transform.FindChild("Label").GetComponent<UILabel>().text = name;
        this._CurrAccountGroup.SetActive(true);
    }

    private void OnLogin()
    {
        if (!this.mLockBtnState)
        {
            SocketMgr.Instance.RequestLogin(GameDefine.getInstance().lastAcctId, GameDefine.getInstance().lastAccountName, GameDefine.getInstance().device_mac, GameDefine.getInstance().clientPlatformType, GameDefine.getInstance().tx_openKey, GameDefine.getInstance().childPlatform, GameDefine.getInstance().sessionId, GameDefine.getInstance().use_macaddr_login);
            this.mIsStart = true;
            this.mLockBtnState = true;
            if (this.m_currSelectSvrId != -1)
            {
                ServerInfo.lastGameServerId = this.m_currSelectSvrId;
            }
        }
    }

    public void OnLogout()
    {
        PlatformInterface.mInstance.PlatformLogout();
        this._AccountLoginBtn.gameObject.SetActive(false);
        this._TencentGroup.gameObject.SetActive(true);
        base.transform.FindChild("LoginGroup/CurrServer").gameObject.SetActive(false);
        this._EnterGameBtn.SetActive(false);
        this._CurrAccountGroup.transform.FindChild("Label").GetComponent<UILabel>().text = string.Empty;
        this._CurrAccountGroup.SetActive(false);
        this.CloseServerListUI();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                this.mLockBtnState = false;
            }
        }
        if (this.mCheckBundle)
        {
            if (BundleMgr.Instance.completedDownload)
            {
                this.mCheckBundle = false;
                this.DownloadMsgLabel.text = ConfigMgr.getInstance().GetWord(0x593);
                this.DownloadSlider.value = 0f;
                List<ServerInfo.GameAdInfo> urls = XSingleton<AdManager>.Singleton.SeletAdInfos();
                if ((urls != null) && (urls.Count > 0))
                {
                    XSingleton<AdManager>.Singleton.OnCompleted = new System.Action(this.OnLogin);
                    XSingleton<AdManager>.Singleton.BeginDown(urls);
                }
                else
                {
                    this.OnLogin();
                }
                GameConstValues.Init();
            }
            else
            {
                int progressDownload = BundleMgr.Instance.progressDownload;
                int totalDownload = BundleMgr.Instance.totalDownload;
                string msgDownload = BundleMgr.Instance.msgDownload;
                this.DownloadSlider.value = (progressDownload > 0) ? (((float) progressDownload) / ((float) totalDownload)) : 0f;
                this.DownloadMsgLabel.text = msgDownload;
            }
        }
        else if (this.mCheckUserInfo)
        {
            this.DownloadSlider.value = ActorData.getInstance().LoadingUserInfoProgress;
        }
    }

    public static void RestLoginPanel()
    {
        GameObject obj2 = GameObject.Find("UI Root/Camera/LoginPanel");
        if (obj2 != null)
        {
            LoginPanel component = obj2.GetComponent<LoginPanel>();
            if (component != null)
            {
                component.RestPanel();
            }
        }
    }

    public void RestPanel()
    {
        base.transform.FindChild("LoadingSlider").gameObject.SetActive(false);
        base.transform.FindChild("LoginGroup").gameObject.SetActive(true);
    }

    private void SetBgImage(Texture2D tex)
    {
        Transform transform = base.transform.FindChild("background");
        if (null != transform)
        {
            UITexture component = transform.GetComponent<UITexture>();
            if (null != component)
            {
                component.mainTexture = tex;
            }
        }
    }

    public void SetCurrLoginAccount()
    {
        if (GameDefine.getInstance().lastAccountName != string.Empty)
        {
            if (GameDefine.getInstance().clientPlatformType == PlatformType.P_Test)
            {
                this.ShowAccountName();
            }
            else if ((GameDefine.getInstance().clientPlatformType != PlatformType.P_LJ_iOS) && GameDefine.getInstance().use_macaddr_login)
            {
                this.ShowAccountName();
            }
            this._AccountLoginBtn.SetActive(true);
        }
    }

    public void SetLoginSvrInfo()
    {
        UILabel component = base.transform.FindChild("LoginGroup/CurrServer/Label").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("LoginGroup/CurrServer/Busy").GetComponent<UILabel>();
        ServerInfo.GameServerInfo preLoginSvrInfo = this.GetPreLoginSvrInfo();
        if (preLoginSvrInfo == null)
        {
            ServerInfo.lastGameServerId = -1;
            this.m_currSelectSvrId = -1;
            component.text = string.Empty;
            label2.text = ConfigMgr.getInstance().GetWord(0x4f1);
            GameDefine.getInstance().gameServerIP = string.Empty;
            this.m_currSvrCanLogin = false;
        }
        else
        {
            this.m_currSelectSvrId = preLoginSvrInfo.serverId;
            object[] objArray1 = new object[] { this.GetStatColor(preLoginSvrInfo.status), this.GetCurrNum(preLoginSvrInfo), ConfigMgr.getInstance().GetWord(0x241), "  ", preLoginSvrInfo.name };
            component.text = string.Concat(objArray1);
            label2.text = this.GetStatColor(preLoginSvrInfo.status) + ConfigMgr.getInstance().GetWord(0xd1 + preLoginSvrInfo.status);
            this.m_currSvrCanLogin = preLoginSvrInfo.status != 4;
            GameDefine.getInstance().gameServerIP = preLoginSvrInfo.ip;
            GameDefine.getInstance().gameServerPort = preLoginSvrInfo.port.ToString();
            SocketMgr.Instance.Connect(preLoginSvrInfo.ip, preLoginSvrInfo.port, preLoginSvrInfo.backIPList);
            object[] objArray2 = new object[] { this.GetStatColor(preLoginSvrInfo.status), this.GetCurrNum(preLoginSvrInfo), ConfigMgr.getInstance().GetWord(0x241), "  ", preLoginSvrInfo.name };
            base.transform.FindChild("LoginGroup/ServerList/Group/CurrSvrName").GetComponent<UILabel>().text = string.Concat(objArray2);
            base.transform.FindChild("LoginGroup/ServerList/Group/CurrSvrStat").GetComponent<UILabel>().text = this.GetStatColor(preLoginSvrInfo.status) + ConfigMgr.getInstance().GetWord(0xd1 + preLoginSvrInfo.status);
            Transform transform = base.transform.FindChild("LoginGroup/ServerList/Group");
            GUIDataHolder.setData(transform.gameObject, preLoginSvrInfo);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickListBtn);
        }
    }

    private void SetSelectSvr(ServerInfo.GameServerInfo info)
    {
        object[] objArray1 = new object[] { this.GetStatColor(info.status), this.GetCurrNum(info), ConfigMgr.getInstance().GetWord(0x241), "  ", info.name };
        base.transform.FindChild("LoginGroup/CurrServer/Label").GetComponent<UILabel>().text = string.Concat(objArray1);
        base.transform.FindChild("LoginGroup/CurrServer/Busy").GetComponent<UILabel>().text = this.GetStatColor(info.status) + ConfigMgr.getInstance().GetWord(0xd1 + info.status);
        GameDefine.getInstance().gameServerIP = info.ip;
        GameDefine.getInstance().gameServerPort = info.port.ToString();
        SocketMgr.Instance.Connect(info.ip, info.port, info.backIPList);
        this.m_currSelectSvrId = info.serverId;
        this.m_currSvrCanLogin = info.status != 4;
    }

    private void SetSvrListinfo(int groupIdx)
    {
        Transform transform = base.transform.FindChild("LoginGroup/ServerList/List/Grid");
        int num = ServerInfo.getInstance().gameServerMaxIndex / 10;
        int num2 = (groupIdx >= num) ? (ServerInfo.getInstance().gameServerMaxIndex % 10) : 10;
        List<ServerInfo.GameServerInfo> serverInfoByPage = ServerInfo.getInstance().GetServerInfoByPage(groupIdx);
        for (int i = 0; i < 10; i++)
        {
            Transform transform2 = transform.FindChild((i + 1).ToString());
            if (i < serverInfoByPage.Count)
            {
                UILabel component = transform2.transform.FindChild("State").GetComponent<UILabel>();
                ServerInfo.GameServerInfo data = serverInfoByPage[i];
                component.text = this.GetStatColor(data.status) + ConfigMgr.getInstance().GetWord(0xd1 + data.status);
                string[] textArray1 = new string[] { this.GetStatColor(data.status), data.index.ToString(), ConfigMgr.getInstance().GetWord(0x241), "  ", data.name };
                transform2.transform.FindChild("Name").GetComponent<UILabel>().text = string.Concat(textArray1);
                transform2.gameObject.SetActive(true);
                GUIDataHolder.setData(transform2.gameObject, data);
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickListBtn);
            }
            else
            {
                transform2.gameObject.SetActive(false);
            }
        }
    }

    public void SetTencentGroup(bool _isShow)
    {
        WaitPanelHelper.HideWaitPanel("CheckTencentReturn");
        this._TencentGroup.gameObject.SetActive(_isShow);
        this.TencentReturnOk = true;
    }

    public void ShowAccountName()
    {
        UILabel component = this._CurrAccountGroup.transform.FindChild("Label").GetComponent<UILabel>();
        component.text = !GameDefine.getInstance().use_macaddr_login ? GameDefine.getInstance().lastAccountName : "游客@";
        if (GameDefine.getInstance().IsThirdPlatform())
        {
            if (GameDefine.getInstance().GetTencentType() == TencentType.GUEST)
            {
                component.text = ConfigMgr.getInstance().GetWord(0x4fb);
            }
            else
            {
                component.text = GameDefine.getInstance().TencentLoginNickName;
            }
        }
        this._CurrAccountGroup.SetActive(true);
        this._AccountLoginBtn.SetActive(true);
        this._EnterGameBtn.SetActive(true);
    }

    public void StartDownLoadServerList()
    {
        this.TencentReturnOk = true;
        WaitPanelHelper.HideWaitPanel("CheckTencentReturn");
        GameObject obj2 = GameObject.Find("UI Root");
        if (null != obj2)
        {
            PlayMakerFSM component = obj2.GetComponent<PlayMakerFSM>();
            if (null != component)
            {
                component.SendEvent("START_DOWNLOAD_SERVERLIST");
            }
            this._TencentGroup.SetActive(false);
            GUIMgr.Instance.UnLock();
            this._AccountLoginBtn.gameObject.SetActive(true);
        }
    }

    private UILabel DownloadMsgLabel
    {
        get
        {
            if (null == this.m_dlMsgLable)
            {
                this.m_dlMsgLable = base.transform.FindChild("LoadingSlider/LoadingTips").GetComponent<UILabel>();
            }
            return this.m_dlMsgLable;
        }
    }

    private UISlider DownloadSlider
    {
        get
        {
            if (null == this.m_dlSlider)
            {
                this.m_dlSlider = base.transform.FindChild("LoadingSlider").GetComponent<UISlider>();
            }
            return this.m_dlSlider;
        }
    }
}

