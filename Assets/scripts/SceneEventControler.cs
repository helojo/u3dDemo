using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("MTD/EventControler")]
public class SceneEventControler : MonoBehaviour
{
    public List<SceneEventProcesserSuperData> processerDataList = new List<SceneEventProcesserSuperData>();
    private List<SceneEventProcesserBase> processerList = new List<SceneEventProcesserBase>();
    private SceneEventTriggerBase trigger;
    public SceneEventTriggerSuperData triggerData = new SceneEventTriggerSuperData();

    [DebuggerHidden]
    private IEnumerator DoLoopProcesser(List<SceneEventProcesserBase> loopList)
    {
        return new <DoLoopProcesser>c__Iterator45 { loopList = loopList, <$>loopList = loopList, <>f__this = this };
    }

    private void InitProcesser()
    {
        this.processerDataList.ForEach(obj => this.processerList.Add(SceneEventProcesserBase.CreateProcesser(obj, this)));
    }

    private void InitTrigger()
    {
        this.trigger = SceneEventTriggerBase.CreateTrigger(this.triggerData, this);
    }

    private void OnDestroy()
    {
        this.trigger.DoRemove();
    }

    private void OnDrawGizmos()
    {
        if (this.triggerData.type == SceneEventTriggerType.Position)
        {
            Gizmos.DrawWireSphere(base.transform.position, this.triggerData.radius);
        }
    }

    [DebuggerHidden]
    private IEnumerator OnProcesser()
    {
        return new <OnProcesser>c__Iterator44 { <>f__this = this };
    }

    public void OnTrigger()
    {
        base.StartCoroutine(this.OnProcesser());
    }

    private void Start()
    {
        this.InitTrigger();
        this.InitProcesser();
    }

    [CompilerGenerated]
    private sealed class <DoLoopProcesser>c__Iterator45 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<SceneEventProcesserBase> <$>loopList;
        internal List<SceneEventProcesserBase>.Enumerator <$s_308>__0;
        internal SceneEventControler <>f__this;
        internal SceneEventProcesserBase <processerInLoop>__1;
        internal List<SceneEventProcesserBase> loopList;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_308>__0.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    if (this.loopList[0].superData.loopType != SceneEventProcesserLoopType.loop)
                    {
                        this.$PC = -1;
                        goto Label_00D5;
                    }
                    break;

                case 1:
                    goto Label_0053;

                default:
                    goto Label_00D5;
            }
        Label_003F:
            this.<$s_308>__0 = this.loopList.GetEnumerator();
            num = 0xfffffffd;
        Label_0053:
            try
            {
                while (this.<$s_308>__0.MoveNext())
                {
                    this.<processerInLoop>__1 = this.<$s_308>__0.Current;
                    this.$current = this.<>f__this.StartCoroutine(this.<processerInLoop>__1.DoProcess());
                    this.$PC = 1;
                    flag = true;
                    return true;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_308>__0.Dispose();
            }
            goto Label_003F;
        Label_00D5:
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
    private sealed class <OnProcesser>c__Iterator44 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<SceneEventProcesserBase>.Enumerator <$s_307>__1;
        internal SceneEventControler <>f__this;
        internal List<SceneEventProcesserBase> <loopList>__0;
        internal SceneEventProcesserBase <processer>__2;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                case 2:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_307>__1.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<loopList>__0 = new List<SceneEventProcesserBase>();
                    this.<$s_307>__1 = this.<>f__this.processerList.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    this.<loopList>__0.Clear();
                    goto Label_018F;

                default:
                    goto Label_0196;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        this.<loopList>__0.Clear();
                        break;
                }
                while (this.<$s_307>__1.MoveNext())
                {
                    this.<processer>__2 = this.<$s_307>__1.Current;
                    if (this.<processer>__2.superData.loopType != SceneEventProcesserLoopType.once)
                    {
                        this.<loopList>__0.Add(this.<processer>__2);
                    }
                    else
                    {
                        if (this.<loopList>__0.Count > 0)
                        {
                            this.$current = this.<>f__this.StartCoroutine(this.<>f__this.DoLoopProcesser(this.<loopList>__0));
                            this.$PC = 1;
                            flag = true;
                        }
                        else
                        {
                            this.$current = this.<>f__this.StartCoroutine(this.<processer>__2.DoProcess());
                            this.$PC = 2;
                            flag = true;
                        }
                        goto Label_0198;
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_307>__1.Dispose();
            }
            if (this.<loopList>__0.Count > 0)
            {
                this.$current = this.<>f__this.StartCoroutine(this.<>f__this.DoLoopProcesser(this.<loopList>__0));
                this.$PC = 3;
                goto Label_0198;
            }
        Label_018F:
            this.$PC = -1;
        Label_0196:
            return false;
        Label_0198:
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

