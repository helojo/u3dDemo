using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionModelScale : RealTimeSkillActionBase
{
    private bool isChanged;
    private bool isFinish;

    public override void Break()
    {
        this.ResetModelScale();
    }

    protected override void Clear()
    {
        this.ResetModelScale();
    }

    [DebuggerHidden]
    protected override IEnumerator OnFinish(MonoBehaviour mb)
    {
        return new <OnFinish>c__Iterator40 { <>f__this = this };
    }

    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator3E { mb = mb, <$>mb = mb, <>f__this = this };
    }

    private void ResetModelScale()
    {
        if (!this.isFinish && this.isChanged)
        {
            BattleFighter actionObj = base.GetActionObj();
            if (actionObj != null)
            {
                actionObj.AddScale(1f / base.info.modelScale);
                if (base.info.hideModel)
                {
                    actionObj.GetAnimObj().SetActive(true);
                }
            }
            this.isFinish = true;
        }
    }

    [DebuggerHidden]
    private IEnumerator StartResetModelScale(MonoBehaviour mb)
    {
        return new <StartResetModelScale>c__Iterator3F { mb = mb, <$>mb = mb, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <OnFinish>c__Iterator40 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionModelScale <>f__this;

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
                    this.<>f__this.ResetModelScale();
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
    private sealed class <OnProcess>c__Iterator3E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionModelScale <>f__this;
        internal BattleFighter <obj>__0;
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
                    this.<>f__this.isFinish = false;
                    this.<obj>__0 = this.<>f__this.GetActionObj();
                    if (this.<obj>__0 != null)
                    {
                        this.<obj>__0.AddScale(this.<>f__this.info.modelScale);
                        this.<>f__this.isChanged = true;
                        if (this.<>f__this.info.hideModel)
                        {
                            this.<obj>__0.modelControler.GetCurModel().SetActive(false);
                        }
                    }
                    this.mb.StartCoroutine(this.<>f__this.StartResetModelScale(this.mb));
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
    private sealed class <StartResetModelScale>c__Iterator3F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal MonoBehaviour <$>mb;
        internal RealTimeSkillActionModelScale <>f__this;
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
                    this.$current = this.mb.StartCoroutine(TimeManager.GetInstance().NewWaitForSecond(this.<>f__this.info.modelScaleDurTime, new int?(this.<>f__this.casterID)));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.ResetModelScale();
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

