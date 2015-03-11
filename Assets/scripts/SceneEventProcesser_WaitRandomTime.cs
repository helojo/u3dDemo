using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneEventProcesser_WaitRandomTime : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator4D { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator4D : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesser_WaitRandomTime <>f__this;
        internal float <time>__0;

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
                    this.<time>__0 = UnityEngine.Random.Range(this.<>f__this.superData.minRandomWaitTime, this.<>f__this.superData.maxRandomWaitTime);
                    this.$current = new WaitForSeconds(this.<time>__0);
                    this.$PC = 1;
                    return true;

                case 1:
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

