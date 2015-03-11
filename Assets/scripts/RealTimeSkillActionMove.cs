using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionMove : RealTimeSkillActionBase
{
    private bool isBreak;
    private bool oldCollsion;
    private Vector3 oldPos;
    private Quaternion oldRot;

    public override void Break()
    {
        this.isBreak = true;
    }

    private float GetMoveDis(Vector3 targetPos)
    {
        Vector3 position = base.caster.transform.position;
        position.y = 0f;
        targetPos.y = 0f;
        return (Vector3.Distance(position, targetPos) - base.info.moveDisCollision);
    }

    private Vector3 GetMoveTargetPos()
    {
        Vector3 vector = base.target.transform.position - base.caster.transform.position;
        float magnitude = vector.magnitude;
        vector.Normalize();
        magnitude = (magnitude - base.target.moveControler.GetRadius()) - base.info.moveDisCollision;
        return (base.caster.transform.position + ((Vector3) (vector * magnitude)));
    }

    [DebuggerHidden]
    protected override IEnumerator OnFinish(MonoBehaviour mb)
    {
        return new <OnFinish>c__Iterator39 { <>f__this = this };
    }

    protected override void OnPrepare()
    {
        this.oldPos = base.caster.transform.position;
        this.oldRot = base.caster.transform.rotation;
        this.oldCollsion = base.caster.moveControler.GetCollsionEnable();
        base.caster.moveControler.SetCollsionEnable(false);
    }

    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator38 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnFinish>c__Iterator39 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionMove <>f__this;

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
                if (this.<>f__this.info.moveIsNeedMoveBack)
                {
                    this.<>f__this.caster.transform.position = this.<>f__this.oldPos;
                    this.<>f__this.caster.transform.rotation = this.<>f__this.oldRot;
                }
                this.<>f__this.caster.moveControler.SetCollsionEnable(this.<>f__this.oldCollsion);
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
    private sealed class <OnProcess>c__Iterator38 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionMove <>f__this;
        internal Vector3 <dir>__0;
        internal Vector3 <forwardDir>__2;
        internal float <oldAcc>__5;
        internal float <oldSpeed>__4;
        internal Vector3 <targetPos>__3;
        internal Vector3 <targetPos>__6;
        internal Vector3 <teamDir>__1;

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
                    if (this.<>f__this.caster != null)
                    {
                        if (this.<>f__this.info.moveIsJump)
                        {
                            this.<dir>__0 = Vector3.zero;
                            if (this.<>f__this.info.moveIsJumpRandom)
                            {
                                this.<dir>__0.x = UnityEngine.Random.Range((float) -1f, (float) 1f);
                                this.<dir>__0.z = UnityEngine.Random.Range((float) -1f, (float) 1f);
                                this.<dir>__0.Normalize();
                                this.<dir>__0.y = 0f;
                            }
                            else
                            {
                                this.<teamDir>__1 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_ScenePosManager>().GetSceneFighterDirByPhase();
                                this.<forwardDir>__2 = Vector3.Project(this.<>f__this.target.transform.forward, this.<teamDir>__1);
                                this.<dir>__0 = (Vector3) (Quaternion.LookRotation(this.<forwardDir>__2) * this.<>f__this.info.moveJumpDir.normalized);
                            }
                            this.<>f__this.caster.transform.position = this.<>f__this.target.transform.position + ((Vector3) (this.<dir>__0 * (this.<>f__this.info.moveDisCollision + this.<>f__this.target.moveControler.GetRadius())));
                            this.<targetPos>__3 = this.<>f__this.target.transform.position;
                            this.<targetPos>__3.y = this.<>f__this.caster.transform.position.y;
                            this.<>f__this.caster.transform.LookAt(this.<targetPos>__3);
                        }
                        else
                        {
                            this.<oldSpeed>__4 = this.<>f__this.caster.moveControler.GetMoveSpeed();
                            this.<>f__this.caster.moveControler.SetMoveSpeed(this.<>f__this.info.moveSpeed);
                            this.<>f__this.caster.moveControler.SetMoveAnim(this.<>f__this.info.moveAnim);
                            this.<oldAcc>__5 = this.<>f__this.caster.moveControler.GetMoveAcceleration();
                            if (this.<>f__this.info.moveAcceleration > 0f)
                            {
                                this.<>f__this.caster.moveControler.SetMoveAcceleration(this.<>f__this.info.moveAcceleration);
                            }
                            this.<targetPos>__6 = this.<>f__this.GetMoveTargetPos();
                            this.<>f__this.PlayEffect(this.<>f__this.GetMoveDis(this.<targetPos>__6), this.<>f__this.effectOfPlayed);
                            while ((this.<>f__this.GetMoveDis(this.<targetPos>__6) > 0.1f) && !this.<>f__this.isBreak)
                            {
                                this.<>f__this.caster.moveControler.StartMoveToPos(this.<targetPos>__6, -1f);
                                this.<>f__this.PlayEffect(this.<>f__this.GetMoveDis(this.<targetPos>__6), this.<>f__this.effectOfPlayed);
                                this.$current = null;
                                this.$PC = 1;
                                return true;
                            Label_034F:
                                this.<targetPos>__6 = this.<>f__this.GetMoveTargetPos();
                            }
                            this.<>f__this.caster.moveControler.StopMove();
                            this.<>f__this.caster.moveControler.SetMoveAnim(null);
                            this.<>f__this.caster.moveControler.SetMoveSpeed(this.<oldSpeed>__4);
                            if (this.<>f__this.info.moveAcceleration > 0f)
                            {
                                this.<>f__this.caster.moveControler.SetMoveAcceleration(this.<oldAcc>__5);
                            }
                        }
                        this.$PC = -1;
                        break;
                    }
                    break;

                case 1:
                    goto Label_034F;
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

