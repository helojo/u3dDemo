using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapAnimCtrl : MonoBehaviour
{
    private float m_interval = 1f;
    private bool PlayAnim = true;
    private float Times;

    [DebuggerHidden]
    private IEnumerator PlayObj()
    {
        return new <PlayObj>c__Iterator6D { <>f__this = this };
    }

    private void Update()
    {
        this.Times += Time.deltaTime;
        if (this.Times >= this.m_interval)
        {
            if ((UnityEngine.Random.Range(1, 9) == 3) && this.PlayAnim)
            {
                base.StartCoroutine(this.PlayObj());
            }
            this.Times = 0f;
        }
    }

    [CompilerGenerated]
    private sealed class <PlayObj>c__Iterator6D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MapAnimCtrl <>f__this;
        internal float <newScale>__0;

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
                    this.<>f__this.PlayAnim = false;
                    TweenAlpha.Begin(this.<>f__this.gameObject, 0.8f, 0f);
                    this.$current = new WaitForSeconds(0.8f);
                    this.$PC = 1;
                    goto Label_00EF;

                case 1:
                    this.<newScale>__0 = UnityEngine.Random.Range((float) 0.55f, (float) 0.75f);
                    this.<>f__this.transform.localScale = new Vector3(this.<newScale>__0, this.<newScale>__0, 1f);
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 2;
                    goto Label_00EF;

                case 2:
                    TweenAlpha.Begin(this.<>f__this.gameObject, 0.8f, 1f);
                    this.<>f__this.PlayAnim = true;
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00EF:
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

