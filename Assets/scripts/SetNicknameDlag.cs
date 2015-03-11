using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SetNicknameDlag : GUIEntity
{
    public UILabel _OkBtnLabel;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private bool mIsStart;

    private void CreateHeroName()
    {
        if (base.transform.FindChild("Input").GetComponent<UIInput>().text.Trim() == string.Empty)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xc9));
        }
    }

    private void OnClickOkBtn()
    {
        <OnClickOkBtn>c__AnonStorey21D storeyd = new <OnClickOkBtn>c__AnonStorey21D();
        if (this.mIsStart)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x25d));
        }
        else
        {
            UIInput component = base.transform.FindChild("Input").GetComponent<UIInput>();
            storeyd.signatureName = component.text;
            if (ConfigMgr.getInstance().GetMaskWord(storeyd.signatureName).Contains("*"))
            {
                TipsDiag.SetText("输入文字中有敏感字！");
            }
            else if (storeyd.signatureName.Length == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x5f));
            }
            else if (storeyd.signatureName.Length > 60)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(7));
            }
            else if (ActorData.getInstance().Stone < 100)
            {
                <OnClickOkBtn>c__AnonStorey21C storeyc = new <OnClickOkBtn>c__AnonStorey21C {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyc.<>m__457), null);
            }
            else if (storeyd.signatureName == ActorData.getInstance().UserInfo.name)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x3f2));
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyd.<>m__458), base.gameObject);
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.mIsStart = TimeMgr.Instance.ServerStampTime < ActorData.getInstance().UserInfo.modifyNickNameCd;
        base.transform.FindChild("CostStone").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x62), "[930b00]" + 100 + "[503d2e]");
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
                if (TimeMgr.Instance.ServerStampTime <= ActorData.getInstance().UserInfo.modifyNickNameCd)
                {
                    this._OkBtnLabel.text = TimeMgr.Instance.GetRemainTime(ActorData.getInstance().UserInfo.modifyNickNameCd);
                }
                else
                {
                    this.mIsStart = false;
                    this._OkBtnLabel.text = ConfigMgr.getInstance().GetWord(0xa1);
                }
            }
        }
    }

    private void PasteBtnEvent()
    {
        base.transform.FindChild("Input").GetComponent<UIInput>().text = CommonFunc.GetRamdomName();
    }

    [CompilerGenerated]
    private sealed class <OnClickOkBtn>c__AnonStorey21C
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__457(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (GameObject _go) {
                    GUIMgr.Instance.ExitModelGUI("SetNicknameDlag");
                    GUIMgr.Instance.ExitModelGUI("PlayerInfoPanel");
                    GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                };
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__45A(GameObject _go)
        {
            GUIMgr.Instance.ExitModelGUI("SetNicknameDlag");
            GUIMgr.Instance.ExitModelGUI("PlayerInfoPanel");
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickOkBtn>c__AnonStorey21D
    {
        internal string signatureName;

        internal void <>m__458(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            box.MultiLayered = true;
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x3f0), 100), box => SocketMgr.Instance.RequestModifyNickName(this.signatureName), null, false);
        }

        internal void <>m__459(GameObject box)
        {
            SocketMgr.Instance.RequestModifyNickName(this.signatureName);
        }
    }
}

