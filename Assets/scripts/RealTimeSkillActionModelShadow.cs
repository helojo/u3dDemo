using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionModelShadow : RealTimeSkillActionBase
{
    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator41 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnProcess>c__Iterator41 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionModelShadow <>f__this;
        internal BattleFighter <obj>__0;

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
                    this.<obj>__0 = this.<>f__this.GetActionObj();
                    if (this.<obj>__0 != null)
                    {
                        ShadowEffect.Begin(this.<obj>__0.modelControler.GetCurModel());
                    }
                    this.$current = null;
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

