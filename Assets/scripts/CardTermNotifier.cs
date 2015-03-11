using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CardTermNotifier : MonoBehaviour
{
    [DebuggerHidden]
    private IEnumerator DoFrame()
    {
        return new <DoFrame>c__Iterator59 { <>f__this = this };
    }

    public void StartMe()
    {
        base.StartCoroutine(this.DoFrame());
    }

    private void TryFillCard()
    {
        foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
        {
            CardOriginal co = pair.Value;
            if ((null != co) && co.dirty)
            {
                CardPool.FillCardInfo(co.gameObject, co);
                co.dirty = false;
                break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <DoFrame>c__Iterator59 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal CardTermNotifier <>f__this;

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
                case 1:
                    this.<>f__this.TryFillCard();
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    return true;

                default:
                    break;
                    this.$PC = -1;
                    break;
            }
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

