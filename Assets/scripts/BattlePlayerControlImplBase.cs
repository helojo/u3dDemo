using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattlePlayerControlImplBase
{
    public virtual float DoStartAnim(string name)
    {
        return 0f;
    }

    public virtual void Init()
    {
    }

    public virtual void OnDrop(MonsterDrop dropInfo, Vector3 pos)
    {
    }

    [DebuggerHidden]
    public virtual IEnumerator OnPhaseStarting()
    {
        return new <OnPhaseStarting>c__Iterator8();
    }

    public virtual void QuestSkip()
    {
    }

    public virtual void Tick()
    {
    }

    public BattleData battleGameData { get; set; }

    public BattleCom_PlayerControl ControlBaseCom { get; set; }

    [CompilerGenerated]
    private sealed class <OnPhaseStarting>c__Iterator8 : IEnumerator, IDisposable, IEnumerator<object>
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

