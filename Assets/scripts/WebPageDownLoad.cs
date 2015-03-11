using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WebPageDownLoad : MonoBehaviour
{
    private byte[] byte_content;
    private string content = string.Empty;
    private Action<byte[]> download_byte_callback;
    private Action<string> download_callback;
    public WWWForm form;
    private string page_url;

    public static void Begin(GameObject go, string url, Action<string> callback)
    {
        if (go == null)
        {
            go = GameObject.Find("/UI Root");
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (callback != null)
            {
                callback("NetworkReachability.NotReachable");
            }
            Debug.LogWarning("NetworkReachability.NotReachable");
        }
        else
        {
            WebPageDownLoad load = go.AddComponent<WebPageDownLoad>();
            load.page_url = Uri.EscapeUriString(url);
            load.download_callback = callback;
            load.Run();
        }
    }

    public static void Begin(GameObject go, string url, Action<byte[]> callback)
    {
        if (go != null)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (callback != null)
                {
                    callback(null);
                }
                Debug.LogWarning("NetworkReachability.NotReachable");
            }
            else
            {
                WebPageDownLoad load = go.AddComponent<WebPageDownLoad>();
                load.page_url = Uri.EscapeUriString(url);
                load.download_byte_callback = callback;
                load.Run();
            }
        }
    }

    public static void Begin(GameObject go, string url, WWWForm form, Action<string> callback)
    {
        if (go == null)
        {
            go = GameObject.Find("/UI Root");
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            if (callback != null)
            {
                callback(string.Empty);
            }
            Debug.LogWarning("NetworkReachability.NotReachable");
        }
        else
        {
            WebPageDownLoad load = go.AddComponent<WebPageDownLoad>();
            load.page_url = Uri.EscapeUriString(url);
            load.download_callback = callback;
            load.form = form;
            load.Run();
        }
    }

    [DebuggerHidden]
    private IEnumerator DoDownLoad()
    {
        return new <DoDownLoad>c__IteratorBC { <>f__this = this };
    }

    private void Run()
    {
        base.StartCoroutine(this.DoDownLoad());
    }

    [DebuggerHidden]
    private IEnumerator TryDownLoadPage(float timeOutSecond)
    {
        return new <TryDownLoadPage>c__IteratorBB { timeOutSecond = timeOutSecond, <$>timeOutSecond = timeOutSecond, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <DoDownLoad>c__IteratorBC : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal WebPageDownLoad <>f__this;
        internal int <i>__0;
        internal IEnumerator <looper>__1;

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
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_015C;

                case 1:
                    this.<i>__0 = 0;
                    goto Label_00D3;

                case 2:
                    this.<looper>__1 = this.<>f__this.TryDownLoadPage((this.<i>__0 * 2f) + 1f);
                    break;

                case 3:
                    break;

                case 4:
                    if (this.<>f__this.download_callback != null)
                    {
                        this.<>f__this.download_callback(this.<>f__this.content);
                    }
                    if (this.<>f__this.download_byte_callback != null)
                    {
                        this.<>f__this.download_byte_callback(this.<>f__this.byte_content);
                    }
                    UnityEngine.Object.Destroy(this.<>f__this);
                    this.$PC = -1;
                    goto Label_015A;

                default:
                    goto Label_015A;
            }
            if (this.<looper>__1.MoveNext())
            {
                this.$current = null;
                this.$PC = 3;
                goto Label_015C;
            }
            if (!string.IsNullOrEmpty(this.<>f__this.content))
            {
                goto Label_00DF;
            }
            this.<i>__0++;
        Label_00D3:
            if (this.<i>__0 < 5)
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_015C;
            }
        Label_00DF:
            this.$current = null;
            this.$PC = 4;
            goto Label_015C;
        Label_015A:
            return false;
        Label_015C:
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
    private sealed class <TryDownLoadPage>c__IteratorBB : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>timeOutSecond;
        internal WebPageDownLoad <>f__this;
        internal StreamReader <sr>__3;
        internal float <startTime>__0;
        internal MemoryStream <strem>__2;
        internal WWW <www>__1;
        internal float timeOutSecond;

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
                    this.<startTime>__0 = 0f;
                    try
                    {
                        if (this.<>f__this.form != null)
                        {
                            this.<www>__1 = new WWW(this.<>f__this.page_url, this.<>f__this.form);
                        }
                        else
                        {
                            this.<www>__1 = new WWW(this.<>f__this.page_url);
                        }
                    }
                    catch
                    {
                        goto Label_018F;
                    }
                    this.<startTime>__0 = 0f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_018F;
            }
            while (!this.<www>__1.isDone && (this.<startTime>__0 < this.timeOutSecond))
            {
                this.<startTime>__0 += Time.deltaTime;
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            try
            {
                if ((this.<www>__1.isDone && string.IsNullOrEmpty(this.<www>__1.error)) && (this.<www>__1.bytes != null))
                {
                    this.<>f__this.byte_content = this.<www>__1.bytes;
                    this.<strem>__2 = new MemoryStream(this.<www>__1.bytes);
                    this.<sr>__3 = new StreamReader(this.<strem>__2);
                    this.<>f__this.content = this.<sr>__3.ReadToEnd();
                    this.<sr>__3.Close();
                    this.<www>__1 = null;
                }
            }
            catch
            {
                goto Label_018F;
            }
            this.$PC = -1;
        Label_018F:
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

