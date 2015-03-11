using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MailListPanel : GUIEntity
{
    public UILabel _DelMailLabel;
    public GameObject _NewMailBtn;
    private UILabel _RefreshMailLabel;
    public GameObject _ShowMailBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private bool mIsDel;
    private bool mIsStart;
    private List<long> mMailIdList = new List<long>();
    private List<long> mSysMailIdList = new List<long>();
    public GameObject SingleMailItem;

    private void ChangeListItemStat(bool isDel)
    {
        IEnumerator enumerator = base.transform.FindChild("List/Grid").GetComponent<UIGrid>().transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                Transform transform2 = current.FindChild("DelTime");
                UIToggle component = current.FindChild("SelectBtn").GetComponent<UIToggle>();
                component.transform.FindChild("Check").GetComponent<UISprite>().color = (Color) new Color32(0xff, 0xff, 0xff, 0);
                if (isDel)
                {
                    object obj2 = GUIDataHolder.getData(current.gameObject);
                    if (obj2 != null)
                    {
                        Mail mail = obj2 as Mail;
                        component.gameObject.SetActive(true);
                        component.transform.GetComponent<BoxCollider>().enabled = !mail.is_sys || (mail.is_sys && (mail.affixList.Count == 0));
                    }
                }
                else
                {
                    component.isChecked = false;
                    component.gameObject.SetActive(false);
                    object obj3 = GUIDataHolder.getData(component.gameObject);
                    if (obj3 != null)
                    {
                        Mail mail2 = obj3 as Mail;
                        if (mail2 != null)
                        {
                            if (mail2.is_sys)
                            {
                                if (this.mSysMailIdList.Contains(mail2.mail_id))
                                {
                                    this.mSysMailIdList.Remove(mail2.mail_id);
                                }
                                continue;
                            }
                            if (this.mMailIdList.Contains(mail2.mail_id))
                            {
                                this.mMailIdList.Remove(mail2.mail_id);
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        this.mIsDel = isDel;
    }

    private void CheckMailStat()
    {
        this.mMailIdList.Clear();
        this.mSysMailIdList.Clear();
        IEnumerator enumerator = base.transform.FindChild("List/Grid").GetComponent<UIGrid>().transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                UIToggle component = current.FindChild("SelectBtn").GetComponent<UIToggle>();
                if (component.isChecked)
                {
                    object obj2 = GUIDataHolder.getData(component.gameObject);
                    if (obj2 != null)
                    {
                        Mail mail = obj2 as Mail;
                        if (mail != null)
                        {
                            if (mail.is_sys)
                            {
                                this.mSysMailIdList.Add(mail.mail_id);
                            }
                            else
                            {
                                this.mMailIdList.Add(mail.mail_id);
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        int num = ((this.mMailIdList.Count != 0) || (this.mSysMailIdList.Count != 0)) ? 0x98976c : 0x98976b;
        this._DelMailLabel.text = ConfigMgr.getInstance().GetWord(num);
    }

    [DebuggerHidden]
    public IEnumerator CreateMailList()
    {
        return new <CreateMailList>c__Iterator8F { <>f__this = this };
    }

    private void DelMail()
    {
        if (this.mIsDel)
        {
            if ((this.mMailIdList.Count == 0) && (this.mSysMailIdList.Count == 0))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x98976d));
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x989761), box => SocketMgr.Instance.RequestDelMail(this.mMailIdList, this.mSysMailIdList), null, false), base.gameObject);
            }
        }
        else if (ActorData.getInstance().MailList.Count == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x98976e));
        }
        else
        {
            this._ShowMailBtn.gameObject.SetActive(true);
            this._NewMailBtn.gameObject.SetActive(false);
            this.ChangeListItemStat(true);
            base.transform.FindChild("RequestNewMailBtn").gameObject.SetActive(false);
        }
    }

    public void InitMailList()
    {
        base.StopAllCoroutines();
        base.StartCoroutine(this.CreateMailList());
    }

    private void OnClickMailItem(GameObject go)
    {
        <OnClickMailItem>c__AnonStorey215 storey = new <OnClickMailItem>c__AnonStorey215();
        if (!this.mIsDel)
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                storey.info = obj2 as Mail;
                if (storey.info != null)
                {
                    storey.needRefalshMail = false;
                    if (SettingMgr.mInstance.GetMailInt(storey.info.mail_id.ToString(), 0) == 0)
                    {
                        UISprite component = go.transform.FindChild("New").GetComponent<UISprite>();
                        SettingMgr.mInstance.SetMailInt(storey.info.mail_id.ToString(), 1);
                        storey.needRefalshMail = true;
                        component.gameObject.SetActive(false);
                    }
                    GUIMgr.Instance.DoModelGUI("MailPanel", new Action<GUIEntity>(storey.<>m__42E), null);
                }
            }
        }
    }

    private void OnClickSelectMail(GameObject go)
    {
        UIToggle component = go.GetComponent<UIToggle>();
        this.CheckMailStat();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.ShowMail();
        GUIMgr.Instance.FloatTitleBar();
        if (TimeMgr.Instance.ServerStampTime > ActorData.getInstance().NextRefreshNewMailTime)
        {
            SocketMgr.Instance.RequestGetMailList();
        }
        else
        {
            this.InitMailList();
        }
        this.SetRefreshTimeLabel();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        if (!ActorData.getInstance().mHaveMailRefresh)
        {
            this.InitMailList();
        }
        this._RefreshMailLabel = base.transform.FindChild("RequestNewMailBtn/Label").GetComponent<UILabel>();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
        CommonFunc.DeleteChildItem(base.transform.FindChild("List/Grid"));
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
                this.SetRefreshTimeLabel();
            }
        }
    }

    private void OpenMailPanel()
    {
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (GUIEntity obj) {
                MailPanel panel = (MailPanel) obj;
                panel.Depth = 400;
            };
        }
        GUIMgr.Instance.DoModelGUI("MailPanel", <>f__am$cacheB, null);
    }

    private void RequestNewMail()
    {
    }

    private void SetAttachment(GameObject obj, Mail info, out bool isErrorMail)
    {
        isErrorMail = true;
        if (info.is_sys)
        {
            UITexture component = obj.transform.FindChild("Item/Icon").GetComponent<UITexture>();
            UISprite sprite = obj.transform.FindChild("Item/QualityBorder").GetComponent<UISprite>();
            component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Mail_Icon_gm");
        }
    }

    private void SetMailInfo(GameObject obj, Mail info)
    {
        if (info != null)
        {
            UILabel component = obj.transform.FindChild("Title").GetComponent<UILabel>();
            if (info.is_sys)
            {
                component.text = ConfigMgr.getInstance().GetWord(0x989756);
            }
            else
            {
                component.text = info.sender.name;
            }
            UILabel label2 = obj.transform.FindChild("DelTime").GetComponent<UILabel>();
            int num = info.send_time + 0x93a80;
            if (info.is_sys)
            {
                num = info.send_time + 0x278d00;
            }
            if (TimeMgr.Instance.ServerStampTime < num)
            {
                label2.text = ConfigMgr.getInstance().GetWord(0x2d5) + TimeMgr.Instance.GetEndTime(num);
                TimeSpan span = new TimeSpan(0, 0, num - TimeMgr.Instance.ServerStampTime);
                if (span.Days <= 3)
                {
                    label2.color = (Color) new Color32(200, 0, 0, 0xff);
                }
            }
            else
            {
                label2.text = ConfigMgr.getInstance().GetWord(0x272b);
            }
            obj.transform.FindChild("Content").GetComponent<UILabel>().text = !info.is_sys ? ConfigMgr.getInstance().GetMaskWord(info.content) : info.content;
            UITexture texture = obj.transform.FindChild("Item/Icon").GetComponent<UITexture>();
            obj.transform.FindChild("SelectBtn/Lock").gameObject.SetActive(info.is_sys && (info.affixList.Count > 0));
            bool isErrorMail = true;
            if (info.is_sys)
            {
                this.SetAttachment(obj, info, out isErrorMail);
            }
            else
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.sender.head_entry);
                if (_config != null)
                {
                    texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
            }
            obj.transform.FindChild("Item/QualityBorder").GetComponent<UISprite>().gameObject.SetActive(!info.is_sys);
            UIToggle toggle = obj.transform.FindChild("SelectBtn").GetComponent<UIToggle>();
            GUIDataHolder.setData(toggle.gameObject, info);
            UIEventListener.Get(toggle.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickSelectMail);
            toggle.gameObject.SetActive(this.mIsDel);
            if (this.mIsDel)
            {
                this._DelMailLabel.text = ConfigMgr.getInstance().GetWord(0x98976b);
                toggle.transform.GetComponent<BoxCollider>().enabled = !info.is_sys || (info.is_sys && (info.affixList.Count == 0));
            }
            UISprite sprite2 = obj.transform.FindChild("New").GetComponent<UISprite>();
            int mailInt = SettingMgr.mInstance.GetMailInt(info.mail_id.ToString(), 0);
            sprite2.gameObject.SetActive(mailInt == 0);
            GUIDataHolder.setData(obj, info);
            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.OnClickMailItem);
        }
    }

    private void SetRefreshTimeLabel()
    {
        if (TimeMgr.Instance.ServerStampTime < ActorData.getInstance().NextRefreshNewMailTime)
        {
            this._RefreshMailLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x3e5), ActorData.getInstance().NextRefreshNewMailTime - TimeMgr.Instance.ServerStampTime);
        }
        else
        {
            this.mIsStart = false;
            this._RefreshMailLabel.text = ConfigMgr.getInstance().GetWord(0x3e6);
        }
    }

    private void ShowMail()
    {
        this._DelMailLabel.text = ConfigMgr.getInstance().GetWord(0x98976b);
        this._NewMailBtn.gameObject.SetActive(true);
        this._ShowMailBtn.gameObject.SetActive(false);
        this.ChangeListItemStat(false);
        base.transform.FindChild("RequestNewMailBtn").gameObject.SetActive(false);
    }

    private int SoryByTime(Mail m1, Mail m2)
    {
        int num = !m1.is_sys ? 0 : 1;
        int num2 = !m2.is_sys ? 0 : 1;
        int mailInt = SettingMgr.mInstance.GetMailInt(m1.mail_id.ToString(), 0);
        int num4 = SettingMgr.mInstance.GetMailInt(m2.mail_id.ToString(), 0);
        if (num != num2)
        {
            return (num2 - num);
        }
        if (mailInt != num4)
        {
            return (mailInt - num4);
        }
        return (m2.send_time - m1.send_time);
    }

    public void UpdateMailList()
    {
        this.InitMailList();
        if (ActorData.getInstance().MailList.Count == 0)
        {
            this.ShowMail();
        }
    }

    [CompilerGenerated]
    private sealed class <CreateMailList>c__Iterator8F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MailListPanel <>f__this;
        internal int <deltime>__3;
        internal GameObject <go>__4;
        internal UIGrid <grid>__0;
        internal int <i>__2;
        internal float <offsetY>__1;

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
                    this.<>f__this.mMailIdList.Clear();
                    this.<>f__this.mSysMailIdList.Clear();
                    CommonFunc.ResetClippingPanel(this.<>f__this.transform.FindChild("List"));
                    this.<grid>__0 = this.<>f__this.transform.FindChild("List/Grid").GetComponent<UIGrid>();
                    CommonFunc.DeleteChildItem(this.<grid>__0.transform);
                    this.$current = new WaitForSeconds(0.01f);
                    this.$PC = 1;
                    goto Label_02FD;

                case 1:
                    this.<offsetY>__1 = 0f;
                    ActorData.getInstance().MailList.Sort(new Comparison<Mail>(this.<>f__this.SoryByTime));
                    this.<i>__2 = 0;
                    goto Label_0296;

                case 2:
                    this.<offsetY>__1 -= this.<grid>__0.cellHeight;
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_02FB;

                default:
                    goto Label_02FB;
            }
        Label_0288:
            this.<i>__2++;
        Label_0296:
            if (this.<i>__2 < ActorData.getInstance().MailList.Count)
            {
                this.<deltime>__3 = ActorData.getInstance().MailList[this.<i>__2].send_time + 0x93a80;
                if (ActorData.getInstance().MailList[this.<i>__2].is_sys)
                {
                    this.<deltime>__3 = ActorData.getInstance().MailList[this.<i>__2].send_time + ((ActorData.getInstance().MailList[this.<i>__2].valid_time * 0xe10) * 0x18);
                }
                if (TimeMgr.Instance.ServerStampTime >= this.<deltime>__3)
                {
                    goto Label_0288;
                }
                if (this.<i>__2 >= 50)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x98975f));
                }
                else
                {
                    this.<go>__4 = UnityEngine.Object.Instantiate(this.<>f__this.SingleMailItem) as GameObject;
                    this.<go>__4.transform.parent = this.<grid>__0.transform;
                    this.<go>__4.transform.localPosition = new Vector3(0f, this.<offsetY>__1, -0.1f);
                    this.<go>__4.transform.localScale = new Vector3(1f, 1f, 1f);
                    this.<>f__this.SetMailInfo(this.<go>__4, ActorData.getInstance().MailList[this.<i>__2]);
                    this.$current = new WaitForSeconds(0.01f);
                    this.$PC = 2;
                    goto Label_02FD;
                }
            }
            this.<>f__this.transform.FindChild("NullMailTips").gameObject.SetActive(ActorData.getInstance().MailList.Count == 0);
            this.$current = null;
            this.$PC = 3;
            goto Label_02FD;
        Label_02FB:
            return false;
        Label_02FD:
            return true;
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
    private sealed class <OnClickMailItem>c__AnonStorey215
    {
        internal Mail info;
        internal bool needRefalshMail;

        internal void <>m__42E(GUIEntity obj)
        {
            MailPanel panel = (MailPanel) obj;
            panel.Depth = 400;
            panel.SetMailInfo(this.info, this.needRefalshMail);
            if (!this.info.is_whole)
            {
                SocketMgr.Instance.RequestWholeMail(this.info.is_sys, this.info.mail_id);
            }
        }
    }
}

