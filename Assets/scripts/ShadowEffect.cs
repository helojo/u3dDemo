using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    public bool cloneIt;

    public static void Begin(GameObject go)
    {
        ShadowEffect component = go.GetComponent<ShadowEffect>();
        if (component == null)
        {
            component = go.AddComponent<ShadowEffect>();
        }
        component.cloneIt = true;
    }

    [DebuggerHidden]
    private IEnumerator Process()
    {
        return new <Process>c__IteratorBA { <>f__this = this };
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (this.cloneIt)
        {
            base.StartCoroutine(this.Process());
            this.cloneIt = false;
        }
    }

    [CompilerGenerated]
    private sealed class <Process>c__IteratorBA : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ShadowEffect <>f__this;
        internal GameObject <cloneGo>__0;
        internal MaterialFSM <mfsm>__1;

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
                    this.<>f__this.cloneIt = false;
                    this.<cloneGo>__0 = UnityEngine.Object.Instantiate(this.<>f__this.gameObject) as GameObject;
                    this.<cloneGo>__0.transform.parent = this.<>f__this.gameObject.transform.parent;
                    this.<cloneGo>__0.transform.localPosition = this.<>f__this.gameObject.transform.localPosition;
                    this.<cloneGo>__0.transform.localScale = this.<>f__this.gameObject.transform.localScale;
                    this.<cloneGo>__0.transform.localRotation = this.<>f__this.gameObject.transform.localRotation;
                    iTween.ScaleAdd(this.<cloneGo>__0, new Vector3(0.5f, 0.5f, 0.5f), 0.1f);
                    this.<mfsm>__1 = this.<cloneGo>__0.AddComponent<MaterialFSM>();
                    this.<mfsm>__1.StartAlphaChange(0f, 0.6f);
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 1;
                    goto Label_018B;

                case 1:
                    this.<mfsm>__1.StartAlphaChange(0.2f, 0f);
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 2;
                    goto Label_018B;

                case 2:
                    UnityEngine.Object.Destroy(this.<cloneGo>__0);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_018B:
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

