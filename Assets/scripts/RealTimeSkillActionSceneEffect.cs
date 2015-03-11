using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillActionSceneEffect : RealTimeSkillActionBase
{
    private int curChangeSceneIndex = -1;

    public override void Break()
    {
        this.OnStop();
    }

    protected override void Clear()
    {
        this.OnStop();
    }

    [DebuggerHidden]
    protected override IEnumerator OnFinish(MonoBehaviour mb)
    {
        return new <OnFinish>c__Iterator37 { <>f__this = this };
    }

    [DebuggerHidden]
    protected override IEnumerator OnProcess(MonoBehaviour mb)
    {
        return new <OnProcess>c__Iterator36 { <>f__this = this };
    }

    private void OnStop()
    {
        if (base.battleGameData != null)
        {
            base.battleGameData.battleComObject.GetComponent<BattleCom_SceneEffect>().StopChangeSceneColor(this.curChangeSceneIndex);
            this.curChangeSceneIndex = -1;
        }
    }

    [CompilerGenerated]
    private sealed class <OnFinish>c__Iterator37 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionSceneEffect <>f__this;

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
                    this.<>f__this.OnStop();
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
    private sealed class <OnProcess>c__Iterator36 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal RealTimeSkillActionSceneEffect <>f__this;
        internal Color <dark>__0;
        internal List<int> <excludeObjs>__1;

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
                    if (this.<>f__this.info.sceneEffectState == BattleSceneEffectState.Drak)
                    {
                        this.<dark>__0 = new Color(0.3f, 0.3f, 0.3f, 1f);
                        this.<excludeObjs>__1 = new List<int>();
                        this.<excludeObjs>__1.Add(this.<>f__this.casterID);
                        if (this.<>f__this.battleGameData != null)
                        {
                            this.<>f__this.curChangeSceneIndex = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_SceneEffect>().ChangeSceneColor(this.<dark>__0, this.<>f__this.info.sceneEffectSmoothEndTime, this.<>f__this.info.sceneEffectDurTime, this.<excludeObjs>__1);
                        }
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

