using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShareImgGameObj : MonoBehaviour
{
    [DebuggerHidden]
    private IEnumerator Capture(string title, string text, string url, string imgPath, bool isFriendShare)
    {
        return new <Capture>c__Iterator9F { imgPath = imgPath, isFriendShare = isFriendShare, title = title, text = text, url = url, <$>imgPath = imgPath, <$>isFriendShare = isFriendShare, <$>title = title, <$>text = text, <$>url = url, <>f__this = this };
    }

    public void DoShare(string title, string text, string url, string imgPath, bool isFriendShare)
    {
        base.StartCoroutine(this.Capture(title, text, url, imgPath, isFriendShare));
    }

    public static void Share(string title, string text, string url, string imgPath, bool isFriendShare)
    {
        Debug.Log("SHARE: img is not exists");
        GameObject target = new GameObject {
            name = "ShareTempObj"
        };
        UnityEngine.Object.DontDestroyOnLoad(target);
        target.AddComponent<ShareImgGameObj>().DoShare(title, text, url, imgPath, isFriendShare);
    }

    [CompilerGenerated]
    private sealed class <Capture>c__Iterator9F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>imgPath;
        internal bool <$>isFriendShare;
        internal string <$>text;
        internal string <$>title;
        internal string <$>url;
        internal ShareImgGameObj <>f__this;
        internal byte[] <data>__2;
        internal string <realImgPath>__0;
        internal Texture2D <texture>__1;
        internal string imgPath;
        internal bool isFriendShare;
        internal string text;
        internal string title;
        internal string url;

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
                    goto Label_01B5;

                case 1:
                    this.<realImgPath>__0 = Application.persistentDataPath + "/" + Time.frameCount.ToString() + "_temp_share.png";
                    Debug.Log("SHARE: imgpath " + this.<realImgPath>__0);
                    this.<texture>__1 = BundleMgr.Instance.CreateTextureObject(this.imgPath);
                    if (this.<texture>__1 == null)
                    {
                        goto Label_013E;
                    }
                    if (File.Exists(this.<realImgPath>__0))
                    {
                        File.Delete(this.<realImgPath>__0);
                    }
                    try
                    {
                        this.<data>__2 = this.<texture>__1.EncodeToPNG();
                        File.WriteAllBytes(this.<realImgPath>__0, this.<data>__2);
                    }
                    catch
                    {
                        Debug.Log("SHARE: save Img Failed");
                        this.<realImgPath>__0 = string.Empty;
                    }
                    if (string.IsNullOrEmpty(this.<realImgPath>__0))
                    {
                        goto Label_013E;
                    }
                    break;

                case 2:
                    break;

                default:
                    goto Label_01B3;
            }
            while (!File.Exists(this.<realImgPath>__0))
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_01B5;
            }
            Debug.Log("SHARE: save Img OK");
        Label_013E:
            Debug.Log("SHARE: do share");
            if (this.isFriendShare)
            {
                PlatformInterface.mInstance.PlatformShareFriend(this.title, this.text, this.url, this.<realImgPath>__0);
            }
            else
            {
                PlatformInterface.mInstance.PlatformShareZone(this.title, this.text, this.url, this.<realImgPath>__0);
            }
            UnityEngine.Object.Destroy(this.<>f__this.gameObject);
            this.$PC = -1;
        Label_01B3:
            return false;
        Label_01B5:
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

