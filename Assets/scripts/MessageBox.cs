using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MessageBox : GUIEntity
{
    private UIEventListener.VoidDelegate _CancelCallBack;
    private UIEventListener.VoidDelegate _OkCallBack;
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

    private void OnClickCancelBtn(GameObject obj)
    {
        if (this._CancelCallBack != null)
        {
            this._CancelCallBack(obj);
        }
        GUIMgr.Instance.ExitModelGUI("MessageBox");
    }

    private void OnClickOkBtn(GameObject obj)
    {
        if (this._OkCallBack != null)
        {
            this._OkCallBack(base.gameObject);
        }
        if (!this.MultiLayered)
        {
            GUIMgr.Instance.ExitModelGUI("MessageBox");
        }
    }

    public override void OnInitialize()
    {
        Transform transform = base.transform.FindChild("CancelBtn");
        UIEventListener.Get(base.transform.FindChild("OkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCancelBtn);
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
            UIEventListener.Get(base.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOkBtn);
        }
    }

    public static void ShowMessageBox(string tipText, UIEventListener.VoidDelegate _OkDelgate, UIEventListener.VoidDelegate _CancelDelgate, bool _OnlyShowOkBtn)
    {
        <ShowMessageBox>c__AnonStorey1AC storeyac = new <ShowMessageBox>c__AnonStorey1AC {
            tipText = tipText,
            _OkDelgate = _OkDelgate,
            _CancelDelgate = _CancelDelgate,
            _OnlyShowOkBtn = _OnlyShowOkBtn
        };
        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyac.<>m__23A), null);
    }

    [CompilerGenerated]
    private sealed class <ShowMessageBox>c__AnonStorey1AC
    {
        internal UIEventListener.VoidDelegate _CancelDelgate;
        internal UIEventListener.VoidDelegate _OkDelgate;
        internal bool _OnlyShowOkBtn;
        internal string tipText;

        internal void <>m__23A(GUIEntity o)
        {
            (o as MessageBox).SetDialog(this.tipText, this._OkDelgate, this._CancelDelgate, this._OnlyShowOkBtn);
        }
    }
}

