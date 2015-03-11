using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class BattleCom_StoryControl : BattleCom_Base
{
    private BattleStoryControlImplBase impl;

    [DebuggerHidden]
    public IEnumerator DoTalk()
    {
        return new <DoTalk>c__IteratorA { <>f__this = this };
    }

    [DebuggerHidden]
    public IEnumerator DoTalkOnStart()
    {
        return new <DoTalkOnStart>c__IteratorB { <>f__this = this };
    }

    [DebuggerHidden]
    public IEnumerator DoTalkString(string dialogue)
    {
        return new <DoTalkString>c__IteratorC { dialogue = dialogue, <$>dialogue = dialogue, <>f__this = this };
    }

    public override void OnCreateInit()
    {
        base.OnCreateInit();
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            this.impl = new BattleStoryControlImplNormalGame();
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
        }
        if (this.impl != null)
        {
            this.impl.battleGameData = base.battleGameData;
        }
    }

    [CompilerGenerated]
    private sealed class <DoTalk>c__IteratorA : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_StoryControl <>f__this;

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
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.impl.DoTalk());
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

    [CompilerGenerated]
    private sealed class <DoTalkOnStart>c__IteratorB : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_StoryControl <>f__this;

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
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.impl.DoTalkOnStart());
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

    [CompilerGenerated]
    private sealed class <DoTalkString>c__IteratorC : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>dialogue;
        internal BattleCom_StoryControl <>f__this;
        internal string dialogue;

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
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.impl.DoTalkString(this.dialogue));
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

