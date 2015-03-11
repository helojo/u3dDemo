using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LabelTextHelper : MonoBehaviour
{
    private bool isFinish;
    private UILabel label;
    private string targetText;

    public void FinishText()
    {
        this.TryInit();
        base.StopAllCoroutines();
        this.label.text = this.targetText;
        this.isFinish = true;
    }

    public bool GetIsFinish()
    {
        return this.isFinish;
    }

    private void SetText(string text)
    {
        this.TryInit();
        this.targetText = text;
        this.isFinish = false;
        base.StopCoroutine("StartShowText");
        base.StartCoroutine(this.StartShowText());
    }

    [DebuggerHidden]
    private IEnumerator StartShowText()
    {
        return new <StartShowText>c__Iterator83 { <>f__this = this };
    }

    private void TryInit()
    {
        if (this.label == null)
        {
            this.label = base.gameObject.GetComponent<UILabel>();
        }
    }

    public string text
    {
        get
        {
            return this.targetText;
        }
        set
        {
            this.SetText(value);
        }
    }

    [CompilerGenerated]
    private sealed class <StartShowText>c__Iterator83 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal LabelTextHelper <>f__this;
        internal List<char> <charArray>__0;
        internal int <i>__2;
        internal int <index>__1;

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
                    this.<>f__this.label.text = string.Empty;
                    this.<charArray>__0 = new List<char>(this.<>f__this.targetText.ToCharArray());
                    break;

                case 1:
                    break;

                default:
                    goto Label_018B;
            }
            if (this.<charArray>__0.Count > 0)
            {
                if (this.<charArray>__0[0] == '[')
                {
                    this.<index>__1 = this.<charArray>__0.IndexOf(']');
                    if (this.<index>__1 != -1)
                    {
                        this.<i>__2 = 0;
                        while (this.<i>__2 != (this.<index>__1 + 1))
                        {
                            this.<>f__this.label.text = this.<>f__this.label.text + this.<charArray>__0[this.<i>__2];
                            this.<i>__2++;
                        }
                        this.<charArray>__0.RemoveRange(0, this.<index>__1 + 1);
                    }
                }
                else
                {
                    this.<>f__this.label.text = this.<>f__this.label.text + this.<charArray>__0[0];
                    this.<charArray>__0.RemoveAt(0);
                }
                this.$current = new WaitForSeconds(0.05f);
                this.$PC = 1;
                return true;
            }
            this.<>f__this.isFinish = true;
            this.$PC = -1;
        Label_018B:
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

