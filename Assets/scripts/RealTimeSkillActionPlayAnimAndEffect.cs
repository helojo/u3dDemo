using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionPlayAnimAndEffect : RealTimeSkillActionBase
{
    private bool isFirst = true;

    [DebuggerHidden]
    protected override IEnumerator OnFinish(MonoBehaviour mb)
    {
        return new <OnFinish>c__Iterator34 { <>f__this = this };
    }

    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator32 { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator ResetAnimSpeed(MonoBehaviour mb)
    {
        return new <ResetAnimSpeed>c__Iterator33 { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnFinish>c__Iterator34 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionPlayAnimAndEffect <>f__this;
        internal BattleFighter <obj2PlayAnim>__0;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if ((this.$PC == 0) && this.<>f__this.info.isNeedStopAnim)
            {
                this.<obj2PlayAnim>__0 = this.<>f__this.GetActionObj();
                this.<obj2PlayAnim>__0.GetComponent<AnimFSM>().StopCurAnimForce();
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
    private sealed class <OnProcess>c__Iterator32 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionPlayAnimAndEffect <>f__this;
        internal float <lookToTime>__2;
        internal float <lookToTimeDefault>__1;
        internal GameObject <modelObj>__0;
        internal Vector3 <targetPos>__3;
        internal MonoBehaviour mb;

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
                    this.<modelObj>__0 = this.<>f__this.GetModelObj();
                    if ((this.<>f__this.oneByOne || this.<>f__this.isFirst) && ((this.<>f__this.isNeedTurn && (this.<>f__this.info.anim != SkillAnimType.FangYu)) && (this.<>f__this.info.anim != SkillAnimType.None)))
                    {
                        this.<lookToTimeDefault>__1 = 0.2f;
                        this.<lookToTime>__2 = BattleGlobal.ScaleTime(this.<lookToTimeDefault>__1);
                        if (this.<>f__this.target != null)
                        {
                            this.<targetPos>__3 = this.<>f__this.target.transform.position;
                            this.<targetPos>__3.y = this.<>f__this.caster.transform.position.y;
                            iTween.LookTo(this.<>f__this.caster.gameObject, this.<targetPos>__3, this.<lookToTime>__2);
                        }
                    }
                    if (this.<modelObj>__0 != null)
                    {
                        BattleGlobalFunc.PlaySkillAnim(this.<modelObj>__0, this.<>f__this.info.anim, this.<>f__this.info.animName, this.<>f__this.info.playAnimSpeed, this.<>f__this.info.playAnimStartTime, this.<>f__this.info.playAnimLoop);
                    }
                    this.<>f__this.PlayEffect(0f, this.<>f__this.effectOfPlayed);
                    if (this.<>f__this.info.playAnimSpeedResetTime > 0f)
                    {
                        this.mb.StartCoroutine(this.<>f__this.ResetAnimSpeed(this.mb));
                    }
                    this.<>f__this.isFirst = false;
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

    [CompilerGenerated]
    private sealed class <ResetAnimSpeed>c__Iterator33 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionPlayAnimAndEffect <>f__this;
        internal BattleFighter <obj2PlayAnim>__0;
        internal MonoBehaviour mb;

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
                    this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.info.playAnimSpeedResetTime, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<obj2PlayAnim>__0 = this.<>f__this.GetActionObj();
                    if (this.<obj2PlayAnim>__0 != null)
                    {
                        BattleGlobalFunc.SetAnimSpeed(this.<obj2PlayAnim>__0.GetAnimObj(), this.<>f__this.info.anim, this.<>f__this.info.animName, 1f);
                    }
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

