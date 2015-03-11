using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class PushNotifyPanel : GUIEntity
{
    public UILabel _MsgText;
    public GameObject _QQGroup;
    public UILabel _TipsText;
    public GameObject _WeiXinGroup;
    private string captureImgPath;
    private Texture2D captureTexture;
    public bool mIsToZone;
    private ShareType mShareType;

    [DebuggerHidden]
    private IEnumerator Capture(bool isToZone)
    {
        return new <Capture>c__Iterator9E { isToZone = isToZone, <$>isToZone = isToZone, <>f__this = this };
    }

    private void OnClickQQFriend(GameObject go)
    {
        this.StartCapture(false);
    }

    private void OnClickZoneBtn(GameObject go)
    {
        this.StartCapture(true);
    }

    public override void OnDestroy()
    {
        UnityEngine.Object.Destroy(this.captureTexture);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener.Get(this._QQGroup.transform.FindChild("FirendBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQFriend);
        UIEventListener.Get(this._QQGroup.transform.FindChild("ZoneBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickZoneBtn);
        UIEventListener.Get(this._WeiXinGroup.transform.FindChild("FirendBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickQQFriend);
        UIEventListener.Get(this._WeiXinGroup.transform.FindChild("ZoneBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickZoneBtn);
    }

    [DebuggerHidden]
    private IEnumerator PlayCardBreakAmin(Transform HeadIcon)
    {
        return new <PlayCardBreakAmin>c__Iterator9D { HeadIcon = HeadIcon, <$>HeadIcon = HeadIcon, <>f__this = this };
    }

    public void PlayShakeEffect()
    {
        Space self = Space.Self;
        iTween.LoopType none = iTween.LoopType.none;
        object[] args = new object[] { 
            "amount", new Vector3(0.05f, 0.05f, 0f), "name", string.Empty, "time", 0.15f, "delay", 0, "looptype", none, "oncomplete", "iTweenOnComplete", "oncompleteparams", 0, "onstart", "iTweenOnStart", 
            "onstartparams", 0, "ignoretimescale", false, "space", self, "axis", string.Empty
         };
        iTween.ShakePosition(base.gameObject, iTween.Hash(args));
    }

    private void SetShareInfo(ShareType _shareType, object _para1, object _para2 = null)
    {
        this.mShareType = _shareType;
        Transform transform = base.transform.FindChild("Info/FuBen");
        Transform transform2 = base.transform.FindChild("Info/CardBreak");
        Transform transform3 = base.transform.FindChild("Info/ArenaLadder");
        Transform transform4 = base.transform.FindChild("Info/PubRecruit");
        transform.gameObject.SetActive(false);
        transform2.gameObject.SetActive(false);
        transform3.gameObject.SetActive(false);
        transform4.gameObject.SetActive(false);
        UITexture component = base.transform.FindChild("Info/Picture").GetComponent<UITexture>();
        UILabel label = base.transform.FindChild("Info/HeadIcon/DownTips").GetComponent<UILabel>();
        switch (_shareType)
        {
            case ShareType.Fuben:
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_pt");
                transform.FindChild("Tips").GetComponent<UILabel>().text = _para1.ToString();
                transform.FindChild("FuBenName").GetComponent<UILabel>().text = _para2.ToString();
                label.text = ConfigMgr.getInstance().GetWord(0x9ba3c1) + "--" + _para2.ToString();
                transform.gameObject.SetActive(true);
                break;

            case ShareType.JingYingFuben:
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_jy");
                transform.FindChild("Tips").GetComponent<UILabel>().text = _para1.ToString();
                transform.FindChild("FuBenName").GetComponent<UILabel>().text = _para2.ToString();
                label.text = ConfigMgr.getInstance().GetWord(0x9ba3c2) + "--" + _para2.ToString();
                transform.gameObject.SetActive(true);
                break;

            case ShareType.CardBreakToPurple:
            {
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_kpjj");
                Transform headIcon = transform2.transform.FindChild("CardIcon");
                Card card = (Card) _para1;
                if (card != null)
                {
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                    if (_config != null)
                    {
                        headIcon.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetQualityBorder(headIcon.FindChild("QualityBorder").GetComponent<UISprite>(), card.cardInfo.quality);
                        headIcon.FindChild("Name").GetComponent<UILabel>().text = _config.name;
                        label.text = ConfigMgr.getInstance().GetWord(0x4f8) + _config.name;
                        transform2.gameObject.SetActive(true);
                        base.StartCoroutine(this.PlayCardBreakAmin(headIcon));
                    }
                    break;
                }
                break;
            }
            case ShareType.ArenaLadderHistoryRankTop:
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_jjc");
                transform3.gameObject.SetActive(true);
                transform3.FindChild("Title").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x8a8);
                transform3.FindChild("Rank").GetComponent<UILabel>().text = _para1.ToString();
                transform3.FindChild("Rankup").GetComponent<UILabel>().text = _para1.ToString();
                label.text = string.Empty;
                break;

            case ShareType.PubRecruit:
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_cj");
                label.text = ConfigMgr.getInstance().GetWord(0x4f7);
                transform4.gameObject.SetActive(true);
                break;

            case ShareType.LoLArenaLadderHistoryRankTop:
                component.mainTexture = BundleMgr.Instance.CreateShareIcon("Ui_Share_Bg_jjc");
                transform3.gameObject.SetActive(true);
                transform3.FindChild("Title").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0x8a9);
                transform3.FindChild("Rank").GetComponent<UILabel>().text = _para1.ToString();
                transform3.FindChild("Rankup").GetComponent<UILabel>().text = _para1.ToString();
                label.text = string.Empty;
                break;
        }
        Transform transform6 = base.transform.FindChild("Info/HeadIcon");
        transform6.FindChild("Icon").GetComponent<UITexture>().mainTexture = XSingleton<SocialFriend>.Singleton.OwnerHead;
        transform6.FindChild("Name").GetComponent<UILabel>().text = GameDefine.getInstance().TencentLoginNickName;
        component.width = 0x2aa;
        component.height = 0x1e4;
    }

    public void ShareFaile()
    {
        base.transform.FindChild("Info/Logo").gameObject.SetActive(false);
        base.transform.FindChild("Info/HeadIcon").gameObject.SetActive(false);
        base.transform.FindChild("Info/ArenaLadder/Rankup").gameObject.SetActive(false);
        if (GameDefine.getInstance().GetTencentType() == TencentType.QQ)
        {
            if (this.mIsToZone)
            {
                this._QQGroup.transform.FindChild("ZoneBtn").GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                this._QQGroup.transform.FindChild("FirendBtn").GetComponent<UIButton>().isEnabled = true;
            }
            this._QQGroup.SetActive(true);
        }
        else if (GameDefine.getInstance().GetTencentType() == TencentType.WEIXIN)
        {
            if (this.mIsToZone)
            {
                this._WeiXinGroup.transform.FindChild("ZoneBtn").GetComponent<UIButton>().isEnabled = true;
            }
            else
            {
                this._WeiXinGroup.transform.FindChild("FirendBtn").GetComponent<UIButton>().isEnabled = true;
            }
            this._WeiXinGroup.SetActive(true);
        }
    }

    public void ShareSuccess()
    {
        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x514));
        base.transform.FindChild("Info/Logo").gameObject.SetActive(false);
        base.transform.FindChild("Info/HeadIcon").gameObject.SetActive(false);
        if (this.mShareType == ShareType.ArenaLadderHistoryRankTop)
        {
            base.transform.FindChild("Info/ArenaLadder/Rankup").gameObject.SetActive(false);
        }
        if (GameDefine.getInstance().GetTencentType() == TencentType.QQ)
        {
            if (this.mIsToZone)
            {
                this._QQGroup.transform.FindChild("ZoneBtn").GetComponent<UIButton>().isEnabled = false;
            }
            else
            {
                this._QQGroup.transform.FindChild("FirendBtn").GetComponent<UIButton>().isEnabled = true;
            }
            this._QQGroup.SetActive(true);
        }
        else if (GameDefine.getInstance().GetTencentType() == TencentType.WEIXIN)
        {
            if (this.mIsToZone)
            {
                this._WeiXinGroup.transform.FindChild("ZoneBtn").GetComponent<UIButton>().isEnabled = false;
            }
            else
            {
                this._WeiXinGroup.transform.FindChild("FirendBtn").GetComponent<UIButton>().isEnabled = true;
            }
            this._WeiXinGroup.SetActive(true);
        }
    }

    public void StartCapture(bool isToZone)
    {
        GUIMgr.Instance.Lock();
        base.StartCoroutine(this.Capture(isToZone));
    }

    private void tryInitCapture()
    {
    }

    public void UpdateData(ShareType _shareType, object _para1, object _para2 = null)
    {
        base.Depth = 0x25d;
        if ((_shareType == ShareType.ArenaLadderHistoryRankTop) || (_shareType == ShareType.LoLArenaLadderHistoryRankTop))
        {
            ResultPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ResultPanel>();
            if ((activityGUIEntity != null) && (activityGUIEntity.Depth > base.Depth))
            {
                base.Depth = activityGUIEntity.Depth + 3;
            }
        }
        bool flag = GameDefine.getInstance().GetTencentType() == TencentType.QQ;
        this._QQGroup.SetActive(flag);
        this._WeiXinGroup.SetActive(!flag);
        this.SetShareInfo(_shareType, _para1, _para2);
    }

    [CompilerGenerated]
    private sealed class <Capture>c__Iterator9E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>isToZone;
        internal Rect <_rect>__4;
        internal PushNotifyPanel <>f__this;
        internal byte[] <data>__5;
        internal Transform <lt>__0;
        internal Vector3 <ltScreenPos>__2;
        internal Transform <rb>__1;
        internal Vector3 <rbScreenPos>__3;
        internal bool isToZone;

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
                    this.<>f__this._QQGroup.SetActive(false);
                    this.<>f__this._WeiXinGroup.SetActive(false);
                    this.<>f__this.mIsToZone = this.isToZone;
                    this.<>f__this.transform.FindChild("Info/Logo").gameObject.SetActive(true);
                    this.<>f__this.transform.FindChild("Info/HeadIcon").gameObject.SetActive(true);
                    if (this.<>f__this.mShareType == PushNotifyPanel.ShareType.ArenaLadderHistoryRankTop)
                    {
                        this.<>f__this.transform.FindChild("Info/ArenaLadder/Rankup").gameObject.SetActive(true);
                    }
                    this.<lt>__0 = this.<>f__this.transform.FindChild("LT");
                    this.<rb>__1 = this.<>f__this.transform.FindChild("RB");
                    this.<ltScreenPos>__2 = UICamera.currentCamera.WorldToViewportPoint(this.<lt>__0.transform.position);
                    this.<rbScreenPos>__3 = UICamera.currentCamera.WorldToViewportPoint(this.<rb>__1.transform.position);
                    this.<_rect>__4 = new Rect(this.<ltScreenPos>__2.x, this.<rbScreenPos>__3.y, this.<rbScreenPos>__3.x - this.<ltScreenPos>__2.x, this.<ltScreenPos>__2.y - this.<rbScreenPos>__3.y);
                    this.<_rect>__4.x *= Screen.width;
                    this.<_rect>__4.y *= Screen.height;
                    this.<_rect>__4.width *= Screen.width;
                    this.<_rect>__4.height *= Screen.height;
                    Debug.Log("share " + this.<_rect>__4.ToString());
                    this.<>f__this.captureTexture = new Texture2D((int) this.<_rect>__4.width, (int) this.<_rect>__4.height, TextureFormat.RGB24, false);
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_03AD;

                case 1:
                    this.<>f__this.captureTexture.ReadPixels(this.<_rect>__4, 0, 0);
                    this.<>f__this.captureTexture.Apply();
                    this.<>f__this.captureImgPath = Application.persistentDataPath + "/" + Time.frameCount.ToString() + "_capture.png";
                    if (File.Exists(this.<>f__this.captureImgPath))
                    {
                        File.Delete(this.<>f__this.captureImgPath);
                    }
                    this.<data>__5 = this.<>f__this.captureTexture.EncodeToPNG();
                    File.WriteAllBytes(this.<>f__this.captureImgPath, this.<data>__5);
                    break;

                case 2:
                    break;

                default:
                    goto Label_03AB;
            }
            while (!File.Exists(this.<>f__this.captureImgPath))
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_03AD;
            }
            Debug.Log("share " + this.<>f__this.captureImgPath);
            if (this.isToZone)
            {
                SharePanel.G_ShareZoneBigImg(this.<>f__this.captureImgPath);
            }
            else
            {
                SharePanel.G_ShareFriendBigImg(this.<>f__this.captureImgPath);
            }
            UnityEngine.Object.DestroyObject(this.<>f__this.captureTexture);
            this.<>f__this.captureTexture = null;
            GUIMgr.Instance.UnLock();
            this.$PC = -1;
        Label_03AB:
            return false;
        Label_03AD:
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
    private sealed class <PlayCardBreakAmin>c__Iterator9D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Transform <$>HeadIcon;
        internal PushNotifyPanel <>f__this;
        internal Transform HeadIcon;

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
                    this.$current = new WaitForSeconds(0.8f);
                    this.$PC = 1;
                    goto Label_00B9;

                case 1:
                    this.HeadIcon.gameObject.SetActive(true);
                    TweenScale.Begin(this.HeadIcon.gameObject, 0.4f, Vector3.one).method = UITweener.Method.BounceIn;
                    this.$current = new WaitForSeconds(0.18f);
                    this.$PC = 2;
                    goto Label_00B9;

                case 2:
                    this.<>f__this.PlayShakeEffect();
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_00B9;

                case 3:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00B9:
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

    public enum ShareType
    {
        Fuben,
        JingYingFuben,
        CardBreakToPurple,
        ArenaLadderHistoryRankTop,
        PubRecruit,
        LoLArenaLadderHistoryRankTop
    }
}

