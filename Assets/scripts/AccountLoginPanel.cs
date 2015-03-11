using FastBuf;
using System;
using UnityEngine;

public class AccountLoginPanel : GUIEntity
{
    public GameObject _ButtonGroup;
    public GameObject _CurrAccountBtn;
    public GameObject _CurrAccountInfo;
    public GameObject _DelAccountBtn;
    public GameObject _InputGroup;
    public GameObject _LoginBtn;
    public GameObject _MoreAccountInfo;
    public GameObject _QuickLoginBtn;
    public GameObject _RegeditOrLoginBtn;
    public WWWForm form;
    private string page_url;

    private void OnClickCancelBtn(GameObject go)
    {
        Debug.Log(go.name);
    }

    private void OnClickCurrAccount(GameObject go)
    {
        this._MoreAccountInfo.SetActive(!this._MoreAccountInfo.activeSelf);
    }

    private void OnClickDelAccount(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog("确定要删除该帐号吗?", delegate (GameObject box) {
            PlayerPrefs.SetString("lastAccountName", WWW.EscapeURL(string.Empty));
            this._MoreAccountInfo.SetActive(false);
            this._QuickLoginBtn.SetActive(true);
            this._CurrAccountInfo.SetActive(false);
            GameDefine.getInstance().lastIsMacaddrLogin = 0;
            LoginPanel component = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
            if (component != null)
            {
                component.ClearCurrAccount();
            }
            UIEventListener.Get(this._LoginBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnPopInputAccountPanel);
        }, null, false), base.gameObject);
    }

    private void OnClickOkBtn(GameObject go)
    {
        UIInput component = this._InputGroup.transform.FindChild("InputAccount").GetComponent<UIInput>();
        if (component.value.Trim().Length == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xc9));
        }
        else if ((GameDefine.getInstance().clientPlatformType != PlatformType.P_Test) && !CommonFunc.EmailGestRUE(component.value))
        {
            TipsDiag.SetText("邮件格式不正确");
        }
        else
        {
            UIInput input2 = this._InputGroup.transform.FindChild("InputPassword").GetComponent<UIInput>();
            GameDefine.getInstance().lastPassWord = input2.value;
            GameDefine.getInstance().lastAccountName = component.value;
            GameDefine.getInstance().use_macaddr_login = false;
            LoginPanel panel = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
            if (panel != null)
            {
                panel.SetCurrLoginAccount();
            }
            GUIMgr.Instance.ExitModelGUI("AccountLoginPanel");
        }
    }

    private void OnClickQuickLoginBtn(GameObject go)
    {
        Debug.Log("------------------->");
        GUIMgr.Instance.ExitModelGUI("AccountLoginPanel");
        GameDefine.getInstance().lastAccountName = string.Empty;
        GameDefine.getInstance().use_macaddr_login = true;
        GameDefine.getInstance().lastIsMacaddrLogin = 1;
        LoginPanel component = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
        if (component != null)
        {
            component.ShowAccountName();
        }
    }

    private void OnExitInputPanel(GameObject go)
    {
        this._InputGroup.gameObject.SetActive(false);
        this._ButtonGroup.gameObject.SetActive(true);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(base.transform.FindChild("CancelBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCancelBtn);
        UIEventListener.Get(this._ButtonGroup.transform.FindChild("QuickLoginBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQuickLoginBtn);
        UIEventListener.Get(this._InputGroup.transform.FindChild("ExitBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnExitInputPanel);
        UIEventListener.Get(this._ButtonGroup.transform.FindChild("RegistBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnRegeditBtn);
        UIEventListener.Get(this._MoreAccountInfo.transform.FindChild("OtherBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnPopInputAccountPanel);
        UIEventListener.Get(this._ButtonGroup.transform.FindChild("CurrAccountInfo").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCurrAccount);
        UIEventListener.Get(this._DelAccountBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickDelAccount);
        this._InputGroup.transform.FindChild("InputAccount").GetComponent<UIInput>().value = GameDefine.getInstance().lastAccountName;
        if (GameDefine.getInstance().lastAccountName == string.Empty)
        {
            if (GameDefine.getInstance().lastIsMacaddrLogin == 1)
            {
                Debug.Log("1111111111");
                this._CurrAccountInfo.transform.FindChild("Label").GetComponent<UILabel>().text = "游客@";
                this._MoreAccountInfo.transform.FindChild("Info/CurrLabel").GetComponent<UILabel>().text = "当前：游客@";
                this._QuickLoginBtn.SetActive(false);
                this._CurrAccountBtn.SetActive(true);
                UIEventListener.Get(this._LoginBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnMacAddrLogin);
            }
            else
            {
                Debug.Log("4444444444444444");
                this._QuickLoginBtn.SetActive(true);
                this._CurrAccountBtn.SetActive(false);
                UIEventListener.Get(this._LoginBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnPopInputAccountPanel);
                Debug.Log("----------_LoginBtn-----------");
            }
        }
        else
        {
            this._QuickLoginBtn.SetActive(false);
            this._CurrAccountBtn.transform.FindChild("Label").GetComponent<UILabel>().text = GameDefine.getInstance().lastAccountName;
            this._MoreAccountInfo.transform.FindChild("Info/CurrLabel").GetComponent<UILabel>().text = "当前：" + GameDefine.getInstance().lastAccountName;
            this._CurrAccountBtn.SetActive(true);
            UIEventListener.Get(this._LoginBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
        }
    }

    private void OnMacAddrLogin(GameObject go)
    {
        GameDefine.getInstance().use_macaddr_login = true;
        LoginPanel component = GameObject.Find("UI Root/Camera/LoginPanel").GetComponent<LoginPanel>();
        if (component != null)
        {
            component.SetCurrLoginAccount();
        }
        GUIMgr.Instance.ExitModelGUI("AccountLoginPanel");
    }

    private void OnPopInputAccountPanel(GameObject go)
    {
        this._InputGroup.transform.FindChild("LoginBtn/Label").GetComponent<UILabel>().text = "登 录";
        UIEventListener.Get(this._RegeditOrLoginBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
        this._InputGroup.gameObject.SetActive(true);
        this._ButtonGroup.gameObject.SetActive(false);
    }

    private void OnRegeditBtn(GameObject go)
    {
        this._InputGroup.transform.FindChild("LoginBtn/Label").GetComponent<UILabel>().text = "注 册";
        this._InputGroup.gameObject.SetActive(true);
        this._ButtonGroup.gameObject.SetActive(false);
    }
}

