using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class SceneEventProcesser_PlayerMoveToNode : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator4C();
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator4C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if (this.$PC == 0)
            {
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

