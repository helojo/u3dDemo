using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UpdateMessageBox : GUIEntity
{
    private UIEventListener.VoidDelegate _CancelCallBack;
    private UIEventListener.VoidDelegate _OkCallBack;
    private UILabel m_dlMsgLable;
    private UISlider m_dlSlider;
    private bool mCurrFunBtn = true;
    private bool mCurrTitleBar = true;
    public bool MultiLayered;

    public void EnableOKBtn(bool _enable)
    {
        base.transform.FindChild("OkBtn").GetComponent<UIButton>().isEnabled = _enable;
    }

    private void GetCheckState()
    {
        UIToggle component = base.gameObject.transform.FindChild("Toggle").GetComponent<UIToggle>();
        ActorData.getInstance().EncouragePopMsg = component.value;
    }

    private void OnClickCommonDownlodBtn(GameObject obj)
    {
        if (this._CancelCallBack != null)
        {
            this._CancelCallBack(obj);
        }
    }

    private void OnClickSaveDownloadBtn(GameObject obj)
    {
        if (this._OkCallBack != null)
        {
            this._OkCallBack(base.gameObject);
        }
    }

    public override void OnInitialize()
    {
        Transform transform = base.transform.FindChild("CancelBtn");
        UIEventListener.Get(base.transform.FindChild("OkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSaveDownloadBtn);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCommonDownlodBtn);
    }

    public void OnUpdateProgress(int progress, string labelText)
    {
        if ((progress == 100) || (progress == 0))
        {
            UIButton component = base.transform.FindChild("OkBtn").GetComponent<UIButton>();
            UIButton button2 = base.transform.FindChild("CancelBtn").GetComponent<UIButton>();
            if (!component.active && !button2.active)
            {
                GameDefine.getInstance().isYYBUpdateDownloading = false;
                component.gameObject.SetActive(true);
                button2.gameObject.SetActive(true);
                this.m_dlSlider.gameObject.SetActive(false);
                this.m_dlSlider.value = 0f;
                this.m_dlMsgLable.gameObject.SetActive(false);
                this.m_dlMsgLable.text = string.Empty;
            }
        }
        else if (progress > 0)
        {
            GameDefine.getInstance().isYYBUpdateDownloading = true;
            UIButton button3 = base.transform.FindChild("OkBtn").GetComponent<UIButton>();
            UIButton button4 = base.transform.FindChild("CancelBtn").GetComponent<UIButton>();
            if (button3.active && button4.active)
            {
                button3.gameObject.SetActive(false);
                button4.gameObject.SetActive(false);
                this.m_dlSlider.gameObject.SetActive(true);
                this.m_dlMsgLable.gameObject.SetActive(true);
            }
            if (null != this.m_dlSlider)
            {
                this.m_dlSlider.value = ((float) progress) / 100f;
            }
            if (null != this.m_dlMsgLable)
            {
                this.m_dlMsgLable.text = labelText;
            }
        }
    }

    public void SetCheckBtn()
    {
        base.gameObject.transform.FindChild("Toggle").gameObject.SetActive(true);
    }

    public void SetDialog(string _text, UIEventListener.VoidDelegate _OkDelgate, UIEventListener.VoidDelegate _CancelDelgate, bool _OnlyShowOkBtn)
    {
        base.Depth = 0x3e8;
        base.transform.FindChild("Label").GetComponent<UILabel>().text = _text;
        this._OkCallBack = _OkDelgate;
        this._CancelCallBack = _CancelDelgate;
        if (_OnlyShowOkBtn)
        {
            UIButton component = base.transform.FindChild("OkBtn").GetComponent<UIButton>();
            UIButton button2 = base.transform.FindChild("CancelBtn").GetComponent<UIButton>();
            component.transform.localPosition = new Vector3(0f, -90f, 0f);
            button2.gameObject.SetActive(false);
            component.gameObject.SetActive(true);
            UIEventListener.Get(base.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSaveDownloadBtn);
        }
    }

    public static void ShowMessageBox(string tipText, UIEventListener.VoidDelegate _OkDelgate, UIEventListener.VoidDelegate _CancelDelgate, bool _OnlyShowOkBtn)
    {
        <ShowMessageBox>c__AnonStorey1AD storeyad = new <ShowMessageBox>c__AnonStorey1AD {
            tipText = tipText,
            _OkDelgate = _OkDelgate,
            _CancelDelgate = _CancelDelgate,
            _OnlyShowOkBtn = _OnlyShowOkBtn
        };
        GUIMgr.Instance.DoModelGUI("UpdateMessageBox", new Action<GUIEntity>(storeyad.<>m__23B), null);
    }

    public UILabel DownloadMsgLabel
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

    public UISlider DownloadSlider
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

    [CompilerGenerated]
    private sealed class <ShowMessageBox>c__AnonStorey1AD
    {
        internal UIEventListener.VoidDelegate _CancelDelgate;
        internal UIEventListener.VoidDelegate _OkDelgate;
        internal bool _OnlyShowOkBtn;
        internal string tipText;

        internal void <>m__23B(GUIEntity o)
        {
            (o as UpdateMessageBox).SetDialog(this.tipText, this._OkDelgate, this._CancelDelgate, this._OnlyShowOkBtn);
        }
    }
}

