using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class SceneEventProcesser_TeamControl : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator47 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator47 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        private static Action<BattleFighter, int> <>f__am$cache4;
        private static Action<BattleFighter, int> <>f__am$cache5;
        internal SceneEventProcesser_TeamControl <>f__this;
        internal BattleNormalGame <battle>__0;

        private static void <>m__12A(BattleFighter _fighter, int _index)
        {
        }

        private static void <>m__12B(BattleFighter _fighter, int _index)
        {
        }

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
                this.<battle>__0 = BattleState.GetNormalGameInstance();
                if (this.<battle>__0.GetFighterManager().teamMove != null)
                {
                    if (this.<>f__this.superData.comonEnable)
                    {
                        if (<>f__am$cache4 == null)
                        {
                            <>f__am$cache4 = new Action<BattleFighter, int>(SceneEventProcesser_TeamControl.<Process>c__Iterator47.<>m__12A);
                        }
                        this.<battle>__0.GetFighterManager().DoToPlayerAllFighter(<>f__am$cache4);
                        this.<battle>__0.GetFighterManager().teamMove.ResumeMove();
                    }
                    else
                    {
                        this.<battle>__0.GetFighterManager().teamMove.StopMove();
                        if (<>f__am$cache5 == null)
                        {
                            <>f__am$cache5 = new Action<BattleFighter, int>(SceneEventProcesser_TeamControl.<Process>c__Iterator47.<>m__12B);
                        }
                        this.<battle>__0.GetFighterManager().DoToPlayerAllFighter(<>f__am$cache5);
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

