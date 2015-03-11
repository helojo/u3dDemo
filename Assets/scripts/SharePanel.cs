using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SharePanel : GUIEntity
{
    private string captureImgPath;
    private Texture2D captureTexture;
    private string curContent;
    public static bool isShareing;
    private UIEventListener.VoidDelegate mOkCallBack;
    public static bool mShareQQ;

    [DebuggerHidden]
    private IEnumerator Capture()
    {
        return new <Capture>c__IteratorA0 { <>f__this = this };
    }

    public static bool CheckIsQQInstalled()
    {
        if (GameDefine.getInstance().GetTencentType() != TencentType.QQ)
        {
            return true;
        }
        if (PlatformInterface.mInstance.PlatformIsQQInstalled())
        {
            return true;
        }
        ShareCallBack(0x3ec);
        return false;
    }

    public static void G_ShareFriend(string title, string text, string url, string imgPath)
    {
        if (CheckIsQQInstalled())
        {
            ShareImgGameObj.Share(title, text, url, imgPath, true);
        }
    }

    public static void G_ShareFriendBigImg(string imgPath)
    {
        if (CheckIsQQInstalled())
        {
            PlatformInterface.mInstance.PlatformShareFriend(string.Empty, string.Empty, string.Empty, imgPath);
        }
    }

    public static void G_ShareZone(string title, string text, string url, string imgPath)
    {
        if (CheckIsQQInstalled())
        {
            ShareImgGameObj.Share(title, text, url, imgPath, false);
        }
    }

    public static void G_ShareZoneBigImg(string imgPath)
    {
        if (CheckIsQQInstalled())
        {
            PlatformInterface.mInstance.PlatformShareZone(string.Empty, string.Empty, string.Empty, imgPath);
        }
    }

    public static bool IsCanShare()
    {
        return ((GameDefine.getInstance().GetTencentType() != TencentType.GUEST) && GameDefine.getInstance().isReleaseServer);
    }

    public override void OnDestroy()
    {
        isShareing = false;
        if (this.mOkCallBack != null)
        {
            this.mOkCallBack(base.gameObject);
        }
    }

    private void OnFirendBtnClick(GameObject go)
    {
        G_ShareFriendBigImg(this.captureImgPath);
        isShareing = false;
        GUIMgr.Instance.ExitModelGUI("SharePanel");
    }

    public override void OnInitialize()
    {
        this.captureImgPath = Application.persistentDataPath + "/capture.png";
        Transform transform = base.transform.FindChild("WEIXIN");
        Transform transform2 = base.transform.FindChild("QQ");
        if (GameDefine.getInstance().GetTencentType() == TencentType.QQ)
        {
            transform.gameObject.SetActive(false);
            UIEventListener.Get(transform2.FindChild("FirendBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnFirendBtnClick);
            UIEventListener.Get(transform2.FindChild("ZoneBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnZoneBtnClick);
        }
        else
        {
            transform2.gameObject.SetActive(false);
            UIEventListener.Get(transform.FindChild("FirendBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnFirendBtnClick);
            UIEventListener.Get(transform.FindChild("ZoneBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnZoneBtnClick);
        }
    }

    private void OnZoneBtnClick(GameObject go)
    {
        G_ShareZoneBigImg(this.captureImgPath);
        isShareing = false;
        GUIMgr.Instance.ExitModelGUI("SharePanel");
    }

    public void SetTexture(Texture2D texture)
    {
        if (texture != null)
        {
            UITexture component = base.transform.FindChild("Texture").GetComponent<UITexture>();
            component.gameObject.SetActive(true);
            component.mainTexture = texture;
            int height = component.height;
            component.height = (component.width * texture.height) / texture.width;
            if (component.height > 400)
            {
                component.height = height;
                component.width = (component.height * texture.width) / texture.height;
            }
        }
    }

    public static void ShareCallBack(int type)
    {
        if (type == 0)
        {
            SocketMgr.Instance.OnShareOK();
        }
        CommonFunc.TencentShareCallBack(type);
    }

    public void ShowDupShare()
    {
    }

    public void StartCapture(UIEventListener.VoidDelegate _OkCallBack)
    {
        this.mOkCallBack = _OkCallBack;
        base.StartCoroutine(this.Capture());
    }

    private void tryInitCapture()
    {
        if (this.captureTexture == null)
        {
            this.captureTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        }
    }

    [CompilerGenerated]
    private sealed class <Capture>c__IteratorA0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SharePanel <>f__this;
        internal byte[] <data>__0;

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
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_0157;

                case 1:
                    this.<>f__this.tryInitCapture();
                    this.<>f__this.captureTexture.ReadPixels(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), 0, 0);
                    this.<>f__this.captureTexture.Apply();
                    this.<>f__this.captureImgPath = Application.persistentDataPath + "/" + Time.frameCount.ToString() + "_capture.png";
                    if (File.Exists(this.<>f__this.captureImgPath))
                    {
                        File.Delete(this.<>f__this.captureImgPath);
                    }
                    this.<data>__0 = this.<>f__this.captureTexture.EncodeToPNG();
                    File.WriteAllBytes(this.<>f__this.captureImgPath, this.<data>__0);
                    break;

                case 2:
                    break;

                default:
                    goto Label_0155;
            }
            while (!File.Exists(this.<>f__this.captureImgPath))
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_0157;
            }
            Debug.Log("SHARE: save Img OK");
            this.<>f__this.SetTexture(this.<>f__this.captureTexture);
            this.$PC = -1;
        Label_0155:
            return false;
        Label_0157:
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
}

