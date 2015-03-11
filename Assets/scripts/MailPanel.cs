using FastBuf;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MailPanel : GUIEntity
{
    public GameObject _Attachment;
    public UILabel _InboxText;
    public GameObject _LineGroup;
    public UILabel _ReplyCountTips;
    public UIInput _ReplyMialInput;
    public UILabel _ReplyTips;
    public UIScrollView _ScrollView;
    public UILabel _SendMailLabel;
    public GameObject _SendReplyBtn;
    public UILabel _SendTimeText;
    public GameObject _SingleMailLine;
    public UILabel _TargetID;
    public UILabel _TargetName;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private bool mIsReplyMail;
    private bool mIsStart;
    private Mail mMail;
    private bool mNeedReflashMiail;
    public GameObject SingleMailAttachment;

    private void ClosePanel()
    {
        if (this.mNeedReflashMiail)
        {
            MailListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<MailListPanel>();
            if (null != gUIEntity)
            {
                gUIEntity.UpdateMailList();
            }
        }
        GUIMgr.Instance.ExitModelGUI<MailPanel>();
    }

    private void OnClickCardBtn(GameObject go, bool isPress)
    {
        if (isPress)
        {
            <OnClickCardBtn>c__AnonStorey216 storey = new <OnClickCardBtn>c__AnonStorey216();
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                storey.cardEntry = (int) obj2;
                GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storey.<>m__432), base.gameObject);
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    private void OnClickItemBtn(GameObject go, bool isPress)
    {
        <OnClickItemBtn>c__AnonStorey218 storey = new <OnClickItemBtn>c__AnonStorey218 {
            go = go
        };
        if (isPress)
        {
            <OnClickItemBtn>c__AnonStorey217 storey2 = new <OnClickItemBtn>c__AnonStorey217 {
                <>f__ref$536 = storey
            };
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(storey.go);
            if (obj2 != null)
            {
                storey2.item = new Item();
                storey2.item.entry = (int) obj2;
                GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storey2.<>m__433), base.gameObject);
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    private void OnClickPickMailAffix(GameObject obj)
    {
        object obj2 = GUIDataHolder.getData(obj);
        if (obj2 != null)
        {
            Mail mail = obj2 as Mail;
            if (mail != null)
            {
                SocketMgr.Instance.RequestPickMailAffix(mail.mail_id);
            }
        }
    }

    private void OnClickReplyMial(GameObject go)
    {
        if (this.mIsReplyMail)
        {
            this._ScrollView.gameObject.SetActive(false);
            this._LineGroup.gameObject.SetActive(true);
            this._ReplyMialInput.gameObject.SetActive(true);
            this._SendMailLabel.text = ConfigMgr.getInstance().GetWord(0x98975a);
            this._ReplyTips.text = ConfigMgr.getInstance().GetWord(0x989758);
            this.mIsReplyMail = false;
            this._ReplyMialInput.text = string.Empty;
            this._ReplyMialInput.GetComponent<BoxCollider>().enabled = true;
            this._Attachment.gameObject.SetActive(false);
            this._ReplyCountTips.gameObject.SetActive(true);
            this._ReplyMialInput.GetComponent<CheckInputValid>()._isEnableMaskWord = true;
            this.mIsStart = true;
        }
        else
        {
            string text = this._ReplyMialInput.text;
            if (text.Length == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x98974c));
            }
            else if (this.mMail != null)
            {
                SocketMgr.Instance.RequestSendMail(this.mMail.sender.id, text);
            }
        }
    }

    private void OnClickSendBtn()
    {
        string content = base.gameObject.transform.FindChild("NewMailGroup/Input").GetComponent<UIInput>().text.Trim();
        if (content.Length == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x98974c));
        }
        else
        {
            long result = 0L;
            long.TryParse(base.gameObject.transform.FindChild("NewMailGroup/InputId").GetComponent<UIInput>().text, out result);
            if (result != 0)
            {
                SocketMgr.Instance.RequestSendMail(result, content);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989765));
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this._SendTimeText.active)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                object[] objArray1 = new object[] { TimeMgr.Instance.ServerDateTime.Year, ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Month), ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Day), "   ", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Hour), ":", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Minute) };
                this._SendTimeText.text = string.Concat(objArray1);
            }
        }
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                object[] objArray2 = new object[] { TimeMgr.Instance.ServerDateTime.Year, ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Month), ".", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Day), "   ", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Hour), ":", string.Format("{0:00}", TimeMgr.Instance.ServerDateTime.Minute) };
                this._InboxText.text = string.Concat(objArray2);
            }
        }
    }

    private void SelectFriend()
    {
        GUIMgr.Instance.DoModelGUI("FriendListPanel", delegate (GUIEntity obj) {
            FriendListPanel panel = (FriendListPanel) obj;
            panel.Depth = 500;
            panel.SelectCallBack(delegate (BriefUser _info) {
                if (_info != null)
                {
                    base.gameObject.transform.FindChild("NewMailGroup/InputId").GetComponent<UIInput>().text = _info.id.ToString();
                    this._TargetName.text = ConfigMgr.getInstance().GetWord(0x989758) + ":" + _info.name;
                }
            });
        }, null);
    }

    private void SetAttachment(Mail info)
    {
        if (info != null)
        {
            UIGrid component = base.transform.FindChild("ReadOrReplyGroup/Attachment/List/Grid").GetComponent<UIGrid>();
            foreach (Affix affix in info.affixList)
            {
                if (affix.type != AffixType.AffixType_None)
                {
                    GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleMailAttachment) as GameObject;
                    obj2.transform.parent = component.transform;
                    obj2.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                    obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                    this.SetAttachmentItem(obj2, affix);
                }
            }
            this._Attachment.gameObject.SetActive(info.affixList.Count > 0);
            component.repositionNow = true;
        }
    }

    private void SetAttachmentItem(GameObject obj, Affix _affix)
    {
        UITexture component = obj.transform.FindChild("Icon").GetComponent<UITexture>();
        UILabel label = obj.transform.FindChild("Count").GetComponent<UILabel>();
        UISprite sprite = obj.transform.FindChild("QualityBorder").GetComponent<UISprite>();
        Debug.Log(":::::::::::" + _affix.type);
        switch (_affix.type)
        {
            case AffixType.AffixType_Card:
                if (_affix.value >= 0)
                {
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_affix.value);
                    if (_config == null)
                    {
                        Debug.LogWarning("card_config entry is error!");
                    }
                    else
                    {
                        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetEquipQualityBorder(sprite, _config.quality, false);
                        component.width = 0x5c;
                        component.height = 0x5c;
                        GUIDataHolder.setData(obj, _config.entry);
                        UIEventListener.Get(obj).onPress = new UIEventListener.BoolDelegate(this.OnClickCardBtn);
                    }
                    break;
                }
                Debug.LogWarning("AffixType_Card entry is error!");
                break;

            case AffixType.AffixType_Gold:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Gold");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_Stone:
            case AffixType.AffixType_DonateStone:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_Eq:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_yqd");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_PhyForce:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Phyforce");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_Item:
                if (_affix.value >= 0)
                {
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(_affix.value);
                    if (_config2 == null)
                    {
                        Debug.LogWarning("equip_config entry is error!");
                    }
                    else
                    {
                        component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                        GUIDataHolder.setData(obj, _config2.entry);
                        UIEventListener.Get(obj).onPress = new UIEventListener.BoolDelegate(this.OnClickItemBtn);
                        obj.transform.FindChild("sprite").gameObject.SetActive((_config2.type == 2) || (_config2.type == 3));
                        CommonFunc.SetEquipQualityBorder(sprite, _config2.quality, false);
                        if (_config2.type == 3)
                        {
                            component.width = 0x5c;
                            component.height = 0x5c;
                        }
                        else
                        {
                            component.width = 80;
                            component.height = 80;
                        }
                    }
                    break;
                }
                Debug.LogWarning("AffixType_Equip entry is error!");
                break;

            case AffixType.AffixType_ArenaLadderScore:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Pk_Icon_coin");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_Contribute:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Gonghui_Icon_ghjx");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_OutlandCoin:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Out_Icon_stone");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_FlameBattleCoin:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Yuanzheng_Icon_gongxun");
                sprite.gameObject.SetActive(true);
                break;

            case AffixType.AffixType_LoLArenaScore:
                component.mainTexture = BundleMgr.Instance.CreateItemIcon("Ui_Pk_Icon_coin1");
                sprite.gameObject.SetActive(true);
                break;
        }
        label.text = (_affix.count <= 0) ? string.Empty : _affix.count.ToString();
    }

    public void SetMailContent(S2C_GetWholeMail res)
    {
        if (this.mMail.mail_id == res.mailId)
        {
            UILabel component = base.transform.FindChild("ReadOrReplyGroup/ScrollView/Message").gameObject.GetComponent<UILabel>();
            UILabel label2 = base.transform.FindChild("ReadOrReplyGroup/Input/Label").GetComponent<UILabel>();
            component.text = !res.isSys ? ConfigMgr.getInstance().GetMaskWord(res.content) : res.content;
            float y = label2.localSize.y;
            float num2 = component.localSize.y;
            Transform transform = component.transform.FindChild("lineGroup");
            CommonFunc.DeleteChildItem(transform);
            int num3 = 0x27;
            int num4 = component.height / num3;
            if ((component.height % num3) != 0)
            {
                num4++;
            }
            component.transform.GetComponent<UIDragScrollView>().enabled = num4 > 5;
            for (int i = 0; i < num4; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleMailLine) as GameObject;
                obj2.transform.parent = transform.transform;
                obj2.transform.localPosition = new Vector3(0f, (float) (-i * num3), -0.1f);
                obj2.transform.localScale = Vector3.one;
            }
            bool flag = y < num2;
            this._ScrollView.gameObject.SetActive(flag);
            this._LineGroup.gameObject.SetActive(!flag);
            this._ReplyMialInput.gameObject.SetActive(!flag);
            if (flag)
            {
                this._ScrollView.ResetPosition();
            }
        }
    }

    public void SetMailInfo(Mail info, bool _NeedReflashMiail = false)
    {
        this.mNeedReflashMiail = _NeedReflashMiail;
        this.SetShowPanelGroup(false);
        if (info != null)
        {
            this.mMail = info;
            UILabel component = base.transform.FindChild("ReadOrReplyGroup/ScrollView/Message").gameObject.GetComponent<UILabel>();
            UILabel label2 = base.transform.FindChild("ReadOrReplyGroup/Input/Label").GetComponent<UILabel>();
            component.text = !info.is_sys ? ConfigMgr.getInstance().GetMaskWord(info.content) : info.content;
            float y = label2.localSize.y;
            float num2 = component.localSize.y;
            Transform transform = component.transform.FindChild("lineGroup");
            CommonFunc.DeleteChildItem(transform);
            int num3 = 0x27;
            int num4 = component.height / num3;
            if ((component.height % num3) != 0)
            {
                num4++;
            }
            component.transform.GetComponent<UIDragScrollView>().enabled = num4 > 5;
            for (int i = 0; i < num4; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleMailLine) as GameObject;
                obj2.transform.parent = transform.transform;
                obj2.transform.localPosition = new Vector3(0f, (float) (-i * num3), -0.1f);
                obj2.transform.localScale = Vector3.one;
            }
            bool flag = y < num2;
            this._ScrollView.gameObject.SetActive(flag);
            this._LineGroup.gameObject.SetActive(!flag);
            this._ReplyMialInput.gameObject.SetActive(!flag);
            if (flag)
            {
                this._ScrollView.ResetPosition();
            }
            else
            {
                this._ReplyMialInput.GetComponent<CheckInputValid>()._isEnableMaskWord = false;
                this._ReplyMialInput.text = !info.is_sys ? ConfigMgr.getInstance().GetMaskWord(info.content) : info.content;
                this._ReplyMialInput.text = this._ReplyMialInput.text.Trim();
                this._ReplyMialInput.GetComponent<BoxCollider>().enabled = false;
            }
            this._ReplyTips.text = ConfigMgr.getInstance().GetWord(0x989757);
            UILabel label3 = base.transform.FindChild("ReadOrReplyGroup/Sign/Name").GetComponent<UILabel>();
            UILabel label4 = base.transform.FindChild("ReadOrReplyGroup/Input/Label").GetComponent<UILabel>();
            if (info.is_sys)
            {
                label3.text = ConfigMgr.getInstance().GetWord(0x989756);
                this._TargetID.text = string.Empty;
                if (!info.already_pick && (info.affixList.Count > 0))
                {
                    this._SendMailLabel.text = ConfigMgr.getInstance().GetWord(0x98974e);
                    GUIDataHolder.setData(this._SendReplyBtn.gameObject, info);
                    UIEventListener.Get(this._SendReplyBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPickMailAffix);
                }
                else if (!info.already_pick && (info.affixList.Count == 0))
                {
                    this._SendReplyBtn.SetActive(false);
                }
            }
            else
            {
                label3.text = info.sender.name;
                this._SendMailLabel.text = ConfigMgr.getInstance().GetWord(0x989764);
                this._TargetID.text = "ID:" + info.sender.id;
                this.mIsReplyMail = true;
                UIEventListener.Get(this._SendReplyBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickReplyMial);
            }
            this._ReplyCountTips.gameObject.SetActive(false);
            this._InboxText.text = TimeMgr.Instance.GetMailTime(info.send_time);
            this.SetAttachment(info);
        }
    }

    public void SetMailSendTo(BriefUser _info)
    {
        if (_info != null)
        {
            this.SetShowPanelGroup(true);
            UIInput component = base.gameObject.transform.FindChild("NewMailGroup/InputId").GetComponent<UIInput>();
            component.text = _info.id.ToString();
            component.GetComponent<BoxCollider>().enabled = false;
            this._TargetName.text = ConfigMgr.getInstance().GetWord(0x989758) + ":" + _info.name;
            UIButton button = base.transform.FindChild("NewMailGroup/SelectBtn").GetComponent<UIButton>();
            button.transform.FindChild("Label").GetComponent<UILabel>().color = (Color) new Color32(0x90, 0x90, 0x90, 0xff);
            button.isEnabled = false;
        }
    }

    private void SetShowPanelGroup(bool isNewMail)
    {
        base.transform.FindChild("ReadOrReplyGroup").gameObject.SetActive(!isNewMail);
        base.transform.FindChild("NewMailGroup").gameObject.SetActive(isNewMail);
        this.mIsStart = isNewMail;
    }

    [CompilerGenerated]
    private sealed class <OnClickCardBtn>c__AnonStorey216
    {
        internal int cardEntry;

        internal void <>m__432(GUIEntity entity)
        {
            (entity as ItemInfoPanel).ShowCardInfo(this.cardEntry, 1, true);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey217
    {
        internal MailPanel.<OnClickItemBtn>c__AnonStorey218 <>f__ref$536;
        internal Item item;

        internal void <>m__433(GUIEntity entity)
        {
            (entity as ItemInfoPanel).UpdateData(this.item, this.<>f__ref$536.go.transform);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey218
    {
        internal GameObject go;
    }
}

