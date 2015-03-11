using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleCom_PlayerControl : BattleCom_Base
{
    public BattlePlayerControlImplBase impl;

    public float DoStartAnim(string name)
    {
        return this.impl.DoStartAnim(name);
    }

    private void FixedUpdate()
    {
        this.impl.Tick();
    }

    public override void OnCreateInit()
    {
        if (base.battleGameData.gameType == BattleGameType.Normal)
        {
            BattleCom_PlayerControlNormalGame game = new BattleCom_PlayerControlNormalGame {
                battleGameData = base.battleGameData,
                ControlBaseCom = this
            };
            this.impl = game;
        }
        else if (base.battleGameData.gameType == BattleGameType.Grid)
        {
            BattleGridPlayerControl control = new BattleGridPlayerControl {
                battleGameData = base.battleGameData,
                ControlBaseCom = this
            };
            this.impl = control;
        }
        this.impl.Init();
    }

    public void OnDrop(MonsterDrop dropInfo, Vector3 pos)
    {
        this.impl.OnDrop(dropInfo, pos);
    }

    [DebuggerHidden]
    public IEnumerator OnPhaseStarting()
    {
        return new <OnPhaseStarting>c__Iterator7 { <>f__this = this };
    }

    public void QuestSkip()
    {
        this.impl.QuestSkip();
    }

    public bool isCanControlCamera { get; set; }

    [CompilerGenerated]
    private sealed class <OnPhaseStarting>c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_PlayerControl <>f__this;

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
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.impl.OnPhaseStarting());
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

