using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class BattleStoryControlImplNormalGame : BattleStoryControlImplBase
{
    [DebuggerHidden]
    public override IEnumerator DoTalk()
    {
        return new <DoTalk>c__Iterator27();
    }

    [DebuggerHidden]
    public override IEnumerator DoTalkOnStart()
    {
        return new <DoTalkOnStart>c__Iterator26();
    }

    [CompilerGenerated]
    private sealed class <DoTalk>c__Iterator27 : IEnumerator, IDisposable, IEnumerator<object>
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

    [CompilerGenerated]
    private sealed class <DoTalkOnStart>c__Iterator26 : IEnumerator, IDisposable, IEnumerator<object>
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

