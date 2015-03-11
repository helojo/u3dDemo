using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScheduleDelayCallBack : MonoBehaviour
{
    private System.Action callBack;
    private float DelayTime;

    public static void CallBack(GameObject obj, float delay, System.Action callBack)
    {
        ScheduleDelayCallBack back = obj.AddComponent<ScheduleDelayCallBack>();
        if (back != null)
        {
            back.callBack = callBack;
            back.DelayTime = delay;
        }
    }

    [DebuggerHidden]
    public IEnumerator Start()
    {
        return new <Start>c__IteratorB9 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <Start>c__IteratorB9 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ScheduleDelayCallBack <>f__this;

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
                    this.$current = new WaitForSeconds(this.<>f__this.DelayTime);
                    this.$PC = 1;
                    return true;

                case 1:
                    if (this.<>f__this.callBack != null)
                    {
                        this.<>f__this.callBack();
                    }
                    UnityEngine.Object.Destroy(this.<>f__this);
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

