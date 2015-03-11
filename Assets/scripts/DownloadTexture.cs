using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DownloadTexture : MonoBehaviour
{
    public string url;

    [DebuggerHidden]
    private IEnumerator Start()
    {
        return new <Start>c__Iterator50 { <>f__this = this };
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <Start>c__Iterator50 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DownloadTexture <>f__this;
        internal UITexture <texture>__1;
        internal WWW <www>__0;

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
                    this.<www>__0 = new WWW(this.<>f__this.url);
                    break;

                case 1:
                    break;

                default:
                    goto Label_00BE;
            }
            if (!this.<www>__0.isDone)
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (!string.IsNullOrEmpty(this.<www>__0.error))
            {
                this.<texture>__1 = this.<>f__this.GetComponent<UITexture>();
                if (this.<texture>__1 != null)
                {
                    this.<texture>__1.mainTexture = this.<www>__0.texture;
                }
            }
            UnityEngine.Object.Destroy(this.<>f__this);
            this.$PC = -1;
        Label_00BE:
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

