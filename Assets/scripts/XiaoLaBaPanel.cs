using fastJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class XiaoLaBaPanel : GUIEntity
{
    private GameObject _costStoneGroup;
    public UILabel _TimeLabel;
    public bool isGuildMsg;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private bool mIsStart = true;

    private void OnClickSendBroadcast()
    {
        UIInput component = base.transform.FindChild("Input").GetComponent<UIInput>();
        string str = component.value;
        Debug.Log("xlb------------->" + component.value);
        str = str.Replace(@"\n", string.Empty).Replace(@"\r", string.Empty).Replace(@"\r\n", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\r\n", string.Empty).Replace("\n\r", string.Empty).Replace(Environment.NewLine, string.Empty);
        component.value = str;
        if (component.value.Length == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2881));
        }
        else if (ConfigMgr.getInstance().GetMaskWord(component.value).Contains("*"))
        {
            TipsDiag.SetText("输入文字中有敏感字！");
        }
        else if (!this.isGuildMsg)
        {
            if (ActorData.getInstance().Stone < GameConstValues.COST_STONE_FOR_ADD_BROADCAST)
            {
                <OnClickSendBroadcast>c__AnonStorey220 storey = new <OnClickSendBroadcast>c__AnonStorey220 {
                    <>f__this = this,
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__468), null);
            }
            else
            {
                SocketMgr.Instance.RequestAddBroadcast(component.value);
            }
        }
        else
        {
            this.SendGuildMsg(component.value);
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this._costStoneGroup = base.transform.FindChild("CostStoneGroup").gameObject;
        base.transform.FindChild("CostStoneGroup/CostStone").GetComponent<UILabel>().text = GameConstValues.COST_STONE_FOR_ADD_BROADCAST.ToString();
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
                object[] objArray1 = new object[] { TimeMgr.Instance.ServerDateTime.Year, ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Month), ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Day), "   ", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Hour), ":", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Minute), ":", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Second) };
                this._TimeLabel.text = string.Concat(objArray1);
            }
        }
    }

    [DebuggerHidden]
    public IEnumerator Send(string content)
    {
        return new <Send>c__Iterator96 { content = content, <$>content = content };
    }

    private void SendGuildMsg(string content)
    {
        Debug.Log("send 1 " + TimeMgr.Instance.ServerStampTime);
        Debug.Log("send 2 " + ActorData.getInstance().GuildMsgNoTime);
        Debug.Log("send 3 " + ActorData.getInstance().mask_guildMsg_time);
        if (TimeMgr.Instance.ServerStampTime > (ActorData.getInstance().mask_guildMsg_time + ActorData.getInstance().GuildMsgNoTime))
        {
            Debug.Log("send start");
            base.StartCoroutine(this.Send(content));
        }
    }

    public void SetCostStoneActive(bool isActive)
    {
        if (this._costStoneGroup != null)
        {
            this._costStoneGroup.SetActive(isActive);
        }
    }

    public void SetTitle(string title)
    {
        base.transform.FindChild("Label").GetComponent<UILabel>().text = title;
    }

    [CompilerGenerated]
    private sealed class <OnClickSendBroadcast>c__AnonStorey220
    {
        internal XiaoLaBaPanel <>f__this;
        internal string title;

        internal void <>m__468(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            e.Achieve<MessageBox>().SetDialog(this.title, delegate (GameObject _go) {
                GUIMgr.Instance.ExitModelGUI(this.<>f__this);
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }, null, false);
        }

        internal void <>m__469(GameObject _go)
        {
            GUIMgr.Instance.ExitModelGUI(this.<>f__this);
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <Send>c__Iterator96 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>content;
        internal Dictionary<string, object> <jsondata>__3;
        internal GUIEntity <mainui>__5;
        internal MainUI <mu>__6;
        internal string <strResult>__2;
        internal bool <valid>__4;
        internal WWW <www>__1;
        internal WWWForm <wwwFrom>__0;
        internal string content;

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
                    ActorData.getInstance().mask_guildMsg_time = TimeMgr.Instance.ServerStampTime;
                    if ((ActorData.getInstance().mGuildData != null) && (ActorData.getInstance().mGuildData.id != -1L))
                    {
                        Debug.Log("guild msg send start " + Time.time.ToString());
                        this.<wwwFrom>__0 = new WWWForm();
                        this.<wwwFrom>__0.AddField("NAME", ActorData.getInstance().UserInfo.name);
                        this.<wwwFrom>__0.AddField("MSG", this.content);
                        this.<wwwFrom>__0.AddField("USERID", ActorData.getInstance().SessionInfo.userid.ToString());
                        this.<wwwFrom>__0.AddField("APP", "Chat");
                        this.<wwwFrom>__0.AddField("ACT", "pushChat");
                        this.<wwwFrom>__0.AddField("MAINKEY", ServerInfo.lastGameServerId.ToString() + "|" + ActorData.getInstance().mGuildData.id.ToString());
                        this.<wwwFrom>__0.AddField("T", Time.time.ToString());
                        this.<www>__1 = new WWW(ServerInfo.getInstance().Chat_Url, this.<wwwFrom>__0);
                        break;
                    }
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186aa));
                    goto Label_033C;

                case 1:
                    break;

                default:
                    goto Label_033C;
            }
            while (!this.<www>__1.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (!string.IsNullOrEmpty(this.<www>__1.error))
            {
                Debug.LogWarning(this.<www>__1.error);
            }
            else
            {
                this.<strResult>__2 = this.<www>__1.text;
                Debug.Log("guild post result " + this.<strResult>__2);
                if (!string.IsNullOrEmpty(this.<strResult>__2))
                {
                    this.<jsondata>__3 = JSON.Instance.ToObject<Dictionary<string, object>>(this.<strResult>__2);
                    if (this.<jsondata>__3 == null)
                    {
                        goto Label_033C;
                    }
                    if (this.<jsondata>__3.ContainsKey("valid"))
                    {
                        this.<valid>__4 = StrParser.ParseBool(this.<jsondata>__3["valid"].ToString(), false);
                        if (this.<valid>__4)
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2882));
                            this.<mainui>__5 = GUIMgr.Instance.GetGUIEntity("MainUI");
                            if (this.<mainui>__5 != null)
                            {
                                Debug.Log("guild msg rsfresh");
                                this.<mu>__6 = (MainUI) this.<mainui>__5;
                                if (this.<mu>__6 != null)
                                {
                                    this.<mu>__6.ResetRefreshTime();
                                }
                            }
                            GUIMgr.Instance.ExitModelGUI("XiaoLaBaPanel");
                        }
                        else
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186ac));
                        }
                        Debug.Log("guild msg send end " + Time.time.ToString());
                    }
                }
                this.$PC = -1;
            }
        Label_033C:
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

