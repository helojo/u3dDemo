using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(UITexture))]
public class WebTextureLoader : MonoBehaviour
{
    private string _url;
    public Action<bool> OnCompleted;

    private void Complete(bool success)
    {
        if (this.OnCompleted != null)
        {
            this.OnCompleted(success);
        }
    }

    [DebuggerHidden]
    private IEnumerator Load(string url)
    {
        return new <Load>c__IteratorBD { url = url, <$>url = url, <>f__this = this };
    }

    public string Url
    {
        get
        {
            return this._url;
        }
        set
        {
            if ((value != this._url) && !string.IsNullOrEmpty(value))
            {
                this._url = value;
                base.StartCoroutine(this.Load(value));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Load>c__IteratorBD : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>url;
        internal WebTextureLoader <>f__this;
        internal UITexture <texture>__1;
        internal WWW <www>__0;
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
                    this.<www>__0 = new WWW(this.url);
                    break;

                case 1:
                    break;

                default:
                    goto Label_00DC;
            }
            if (!this.<www>__0.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (!string.IsNullOrEmpty(this.<www>__0.error))
            {
                this.<>f__this.Complete(false);
            }
            else
            {
                this.<texture>__1 = this.<>f__this.GetComponent<UITexture>();
                if (this.<texture>__1 == null)
                {
                    this.<>f__this.Complete(false);
                }
                else
                {
                    this.<texture>__1.mainTexture = this.<www>__0.texture;
                    this.<>f__this.Complete(true);
                    this.$PC = -1;
                }
            }
        Label_00DC:
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

