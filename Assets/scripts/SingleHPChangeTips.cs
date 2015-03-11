using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SingleHPChangeTips : MonoBehaviour
{
    private GameObject attachObj;
    private GameObject labelObj;
    private Vector3 offset;

    public void AttachToObj(GameObject BattleFighterObj, SkillEffectResult info, int type)
    {
        if (BattleFighterObj == null)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            if (this.labelObj == null)
            {
                this.labelObj = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, "Label");
            }
            GameObject obj2 = BattleGlobalFunc.DeepFindChildObjectByName(BattleFighterObj, "Top");
            if (obj2 != null)
            {
                this.attachObj = obj2;
                this.offset.y = 0.2f;
            }
            else
            {
                this.attachObj = BattleFighterObj;
                this.offset.y = 2.2f;
            }
            UILabel component = this.labelObj.GetComponent<UILabel>();
            switch (type)
            {
                case 1:
                {
                    if (info.changeValue <= 0)
                    {
                        component.text = info.changeValue.ToString();
                        base.StartCoroutine(this.ShowNormalDamage());
                        return;
                    }
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    component.gradientBottom = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    component.text = "+" + info.changeValue;
                    Vector3 vector = new Vector3(this.labelObj.transform.localPosition.x, this.labelObj.transform.localPosition.y + 30f, this.labelObj.transform.localPosition.z);
                    this.labelObj.transform.localPosition = vector;
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;
                }
                case 2:
                    component.gradientTop = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    component.gradientBottom = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 3:
                case 4:
                    component.text = "c";
                    component.gradientTop = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    component.gradientBottom = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 5:
                    component.fontSize = 0x2a;
                    if (info.changeValue >= 0)
                    {
                        component.text = "b\n+" + info.changeValue;
                        component.gradientTop = (Color) new Color32(0, 0xff, 0x41, 0xff);
                        component.gradientBottom = (Color) new Color32(0, 0xff, 0x41, 0xff);
                        break;
                    }
                    component.text = "b\n" + info.changeValue;
                    component.gradientTop = (Color) new Color32(0xff, 0xb9, 0, 0xff);
                    component.gradientBottom = (Color) new Color32(0xff, 0x4f, 0, 0xff);
                    break;

                case 6:
                    component.fontSize = 0x1c;
                    component.text = "+" + info.changeValue;
                    component.gradientTop = (Color) new Color32(0xe8, 0xe2, 0, 0xff);
                    component.gradientBottom = (Color) new Color32(0x8e, 0x7a, 0x10, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 7:
                    component.text = info.changeValue.ToString();
                    component.gradientTop = (Color) new Color32(0, 0x33, 0xf8, 0xff);
                    component.gradientBottom = (Color) new Color32(0x21, 13, 0x76, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 14:
                    component.text = "r\n+" + info.changeValue + "a";
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    component.gradientBottom = (Color) new Color32(0, 0xff, 0x41, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 0x12:
                    component.text = "v";
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    component.gradientBottom = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 20:
                    component.text = "k";
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    component.gradientBottom = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 0x15:
                    component.text = "y";
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    component.gradientBottom = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                case 0x16:
                    component.text = "z";
                    component.fontSize = 0x1c;
                    component.gradientTop = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    component.gradientBottom = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
                    base.StartCoroutine(this.ShowAddEnerage());
                    return;

                default:
                    UnityEngine.Object.Destroy(base.gameObject);
                    return;
            }
            base.StartCoroutine(this.ShowComboHpProcess());
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowAddEnerage()
    {
        return new <ShowAddEnerage>c__Iterator6A { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator ShowComboHpProcess()
    {
        return new <ShowComboHpProcess>c__Iterator6C { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator ShowNormalDamage()
    {
        return new <ShowNormalDamage>c__Iterator6B { <>f__this = this };
    }

    private void Update()
    {
        if (this.attachObj != null)
        {
            Vector3 pos = this.attachObj.transform.position + this.offset;
            base.transform.localPosition = BattleGlobalFunc.WorldToGUI(pos);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowAddEnerage>c__Iterator6A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SingleHPChangeTips <>f__this;
        internal Vector3 <upPos>__0;

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
                    this.<upPos>__0 = new Vector3(this.<>f__this.labelObj.transform.localPosition.x, this.<>f__this.labelObj.transform.localPosition.y + 100f, this.<>f__this.labelObj.transform.localPosition.z);
                    TweenPosition.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(1f), this.<upPos>__0);
                    TweenAlpha.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(1.3f), 0f);
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(1.3f));
                    this.$PC = 1;
                    goto Label_011B;

                case 1:
                    UnityEngine.Object.Destroy(this.<>f__this.gameObject);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_011B;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_011B:
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

    [CompilerGenerated]
    private sealed class <ShowComboHpProcess>c__Iterator6C : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SingleHPChangeTips <>f__this;
        internal Vector3 <downPos>__1;
        internal Vector3 <orginScale>__0;

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
                    this.<orginScale>__0 = this.<>f__this.labelObj.transform.localScale;
                    this.<>f__this.labelObj.transform.localScale = new Vector3(this.<orginScale>__0.x * 3f, this.<orginScale>__0.y * 3f, this.<orginScale>__0.z);
                    this.<downPos>__1 = new Vector3(this.<>f__this.labelObj.transform.localPosition.x, this.<>f__this.labelObj.transform.localPosition.y - 50f, this.<>f__this.labelObj.transform.localPosition.z);
                    this.<>f__this.labelObj.transform.localPosition = this.<downPos>__1;
                    TweenScale.Begin(this.<>f__this.labelObj, 0.15f, (Vector3) (this.<orginScale>__0 * 1f));
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(0.5f));
                    this.$PC = 1;
                    goto Label_0209;

                case 1:
                    TweenPosition.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(0.5f), new Vector3(this.<downPos>__1.x, this.<downPos>__1.y, this.<downPos>__1.z + 10f));
                    TweenAlpha.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(1f), 0f);
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(1f));
                    this.$PC = 2;
                    goto Label_0209;

                case 2:
                    UnityEngine.Object.Destroy(this.<>f__this.gameObject);
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0209;

                case 3:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0209:
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

    [CompilerGenerated]
    private sealed class <ShowNormalDamage>c__Iterator6B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SingleHPChangeTips <>f__this;
        internal Vector3 <orginScale>__0;
        internal int <RandomHight>__1;
        internal Vector3 <upPos>__2;
        internal Vector3 <upPos2>__3;

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
                    this.<orginScale>__0 = this.<>f__this.labelObj.transform.localScale;
                    this.<>f__this.labelObj.transform.localScale = Vector3.zero;
                    this.<RandomHight>__1 = UnityEngine.Random.RandomRange(0, 60);
                    this.<upPos>__2 = new Vector3(this.<>f__this.labelObj.transform.localPosition.x, this.<>f__this.labelObj.transform.localPosition.y + this.<RandomHight>__1, this.<>f__this.labelObj.transform.localPosition.z);
                    this.<upPos2>__3 = new Vector3(this.<>f__this.labelObj.transform.localPosition.x, this.<>f__this.labelObj.transform.localPosition.y + 120f, this.<>f__this.labelObj.transform.localPosition.z);
                    TweenPosition.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(0.15f), this.<upPos>__2);
                    TweenScale.Begin(this.<>f__this.labelObj, 0.15f, (Vector3) (this.<orginScale>__0 * 0.6f)).method = UITweener.Method.EaseOut;
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(0.18f));
                    this.$PC = 1;
                    goto Label_0286;

                case 1:
                    TweenScale.Begin(this.<>f__this.labelObj, 0.3f, Vector3.one).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(0.6f));
                    this.$PC = 2;
                    goto Label_0286;

                case 2:
                    TweenPosition.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(0.6f), this.<upPos2>__3).method = UITweener.Method.Linear;
                    TweenAlpha.Begin(this.<>f__this.labelObj, BattleGlobal.ScaleTime(0.6f), 0f);
                    this.$current = new WaitForSeconds(BattleGlobal.ScaleTime(0.6f));
                    this.$PC = 3;
                    goto Label_0286;

                case 3:
                    UnityEngine.Object.Destroy(this.<>f__this.gameObject);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_0286;

                case 4:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0286:
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

