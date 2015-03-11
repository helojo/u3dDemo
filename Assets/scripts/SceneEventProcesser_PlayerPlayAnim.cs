using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class SceneEventProcesser_PlayerPlayAnim : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator4A { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator4A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesser_PlayerPlayAnim <>f__this;
        internal BattleNormalGame <battle>__0;
        internal BattleFighter <fighter>__1;

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
                if (this.<>f__this.superData.IsAllPlayer)
                {
                    this.<battle>__0 = BattleState.GetNormalGameInstance();
                    if (this.<battle>__0 == null)
                    {
                    }
                }
                else
                {
                    this.<fighter>__1 = this.<>f__this.GetFighter();
                    if (this.<fighter>__1 == null)
                    {
                    }
                }
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

