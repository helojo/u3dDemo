using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BattleStoryControlImplBase
{
    protected BattleStoryControlImplBase()
    {
    }

    public abstract IEnumerator DoTalk();
    public abstract IEnumerator DoTalkOnStart();
    [DebuggerHidden]
    public IEnumerator DoTalkString(string talks)
    {
        return new <DoTalkString>c__IteratorD { talks = talks, <$>talks = talks, <>f__this = this };
    }

    public BattleData battleGameData { get; set; }

    [CompilerGenerated]
    private sealed class <DoTalkString>c__IteratorD : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>talks;
        internal BattleStoryControlImplBase <>f__this;
        internal BattleCom_UIControl <control>__0;
        internal string talks;

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
                    if (!(this.talks != string.Empty))
                    {
                        break;
                    }
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(1f));
                    this.$PC = 1;
                    goto Label_00B8;

                case 1:
                    break;

                case 2:
                    goto Label_009F;

                default:
                    goto Label_00B6;
            }
            this.<control>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>();
            this.<control>__0.DoTalk(this.talks);
        Label_009F:
            while (this.<control>__0.isTalking)
            {
                this.$current = null;
                this.$PC = 2;
                goto Label_00B8;
            }
            this.$PC = -1;
        Label_00B6:
            return false;
        Label_00B8:
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

