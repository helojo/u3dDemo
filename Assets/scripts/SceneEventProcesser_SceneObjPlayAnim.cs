using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneEventProcesser_SceneObjPlayAnim : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator48 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator48 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesser_SceneObjPlayAnim <>f__this;
        internal GameObject <sceneObj>__0;

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
                this.<sceneObj>__0 = GameObject.Find(this.<>f__this.superData.SceneObjName);
                if (this.<sceneObj>__0 != null)
                {
                    if (!this.<sceneObj>__0.animation.Play(this.<>f__this.superData.AnimName))
                    {
                        Debug.LogWarning(this.<>f__this.superData.SceneObjName + " can't play " + this.<>f__this.superData.AnimName);
                    }
                }
                else
                {
                    Debug.LogWarning("Event cant't find obj " + this.<>f__this.superData.SceneObjName);
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

