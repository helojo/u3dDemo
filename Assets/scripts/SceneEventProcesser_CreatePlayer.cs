using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneEventProcesser_CreatePlayer : SceneEventProcesserBase
{
    [DebuggerHidden]
    protected override IEnumerator Process()
    {
        return new <Process>c__Iterator4B { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Process>c__Iterator4B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SceneEventProcesser_CreatePlayer <>f__this;
        internal GameObject <newObj>__0;

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
                this.<newObj>__0 = CardPlayer.CreateNormalObj(this.<>f__this.superData.ModelName);
                if (this.<newObj>__0 != null)
                {
                    this.<newObj>__0.transform.position = new Vector3(-10000f, -10000f, -10000f);
                    this.<newObj>__0.name = this.<>f__this.superData.NewSceneObjName;
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

