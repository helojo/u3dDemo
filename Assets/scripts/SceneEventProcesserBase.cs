using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class SceneEventProcesserBase
{
    protected SceneEventProcesserBase()
    {
    }

    public static SceneEventProcesserBase CreateProcesser(SceneEventProcesserSuperData data, SceneEventControler owner)
    {
        SceneEventProcesserBase base2 = null;
        switch (data.type)
        {
            case SceneEventProcesserType.SceneObjPlayAnim:
                base2 = new SceneEventProcesser_SceneObjPlayAnim();
                break;

            case SceneEventProcesserType.SceneObjControl:
                break;

            case SceneEventProcesserType.CreatePlayer:
                base2 = new SceneEventProcesser_CreatePlayer();
                break;

            case SceneEventProcesserType.MoveToNode:
                base2 = new SceneEventProcesser_PlayerMoveToNode();
                break;

            case SceneEventProcesserType.TeamMoveControl:
                base2 = new SceneEventProcesser_TeamControl();
                break;

            case SceneEventProcesserType.TalkBox:
                base2 = new SceneEventProcesser_TalkBox();
                break;

            case SceneEventProcesserType.PlayerPlayAnim:
                base2 = new SceneEventProcesser_PlayerPlayAnim();
                break;

            case SceneEventProcesserType.WaitRandomTime:
                base2 = new SceneEventProcesser_WaitRandomTime();
                break;

            default:
                Debug.LogError("CreateProcesser error");
                break;
        }
        if (base2 != null)
        {
            base2.superData = data;
            base2.ownerControl = owner;
        }
        return base2;
    }

    [DebuggerHidden]
    public IEnumerator DoProcess()
    {
        return new <DoProcess>c__Iterator46 { <>f__this = this };
    }

    protected BattleFighter GetFighter()
    {
        BattleNormalGame normalGameInstance = BattleState.GetNormalGameInstance();
        int index = 0;
        BattleFighter fighter = normalGameInstance.GetFighterManager().GetFighter(index);
        if (fighter != null)
        {
            return fighter;
        }
        for (int i = 0; i < BattleGlobal.FighterNumberOneSide; i++)
        {
            fighter = normalGameInstance.GetFighterManager().GetFighter(i);
            if (fighter != null)
            {
                return fighter;
            }
        }
        return null;
    }

    protected abstract IEnumerator Process();

    public SceneEventControler ownerControl { get; set; }

    public SceneEventProcesserSuperData superData { get; set; }

    [CompilerGenerated]
    private sealed class <DoProcess>c__Iterator46 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesserBase <>f__this;
        internal float <startTime>__0;

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
                    this.<startTime>__0 = Time.time;
                    this.$current = this.<>f__this.ownerControl.StartCoroutine(this.<>f__this.Process());
                    this.$PC = 1;
                    goto Label_009F;

                case 1:
                case 2:
                    if ((this.<startTime>__0 + this.<>f__this.superData.timeOfDuration) > Time.time)
                    {
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_009F;
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_009F:
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

