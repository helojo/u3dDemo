using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneEventProcesser_TalkBox : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator49 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator49 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesser_TalkBox <>f__this;
        internal BattleFighter <fighter>__0;

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
                    this.<fighter>__0 = this.<>f__this.GetFighter();
                    if (this.<fighter>__0 == null)
                    {
                        break;
                    }
                    this.$current = new WaitForSeconds(1.5f);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<fighter>__0.HideTalkPop();
                    break;

                default:
                    goto Label_0071;
            }
            this.$PC = -1;
        Label_0071:
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

