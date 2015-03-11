using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleDropObject : MonoBehaviour
{
    public bool isFinish;
    private bool isStart;
    public ItemProperty property;

    public void BePick(float delayTime, bool manual)
    {
        if (!this.isStart)
        {
            this.isStart = true;
            base.GetComponent<BoxCollider>().enabled = false;
            base.StartCoroutine(this.StartShow(delayTime, manual));
        }
    }

    public static BattleDropObject CreateDropObj(ItemProperty _property, Vector3 pos, Quaternion dir)
    {
        GameObject obj2 = ObjectManager.CreateObj(BattleGlobal.DropPrefab);
        obj2.transform.position = pos;
        obj2.transform.rotation = dir;
        BoxCollider collider = obj2.AddComponent<BoxCollider>();
        collider.size = new Vector3(7f, 7f, 7f);
        collider.center = new Vector3(0f, 0.5f, 0f);
        BattleDropObject obj3 = obj2.AddComponent<BattleDropObject>();
        obj3.StartDrop(_property);
        return obj3;
    }

    public void StartDrop(ItemProperty _property)
    {
        this.property = _property;
    }

    [DebuggerHidden]
    private IEnumerator StartShow(float delayTime, bool manual)
    {
        return new <StartShow>c__Iterator24 { delayTime = delayTime, manual = manual, <$>delayTime = delayTime, <$>manual = manual, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <StartShow>c__Iterator24 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>delayTime;
        internal bool <$>manual;
        internal BattleDropObject <>f__this;
        internal Transform <child>__8;
        internal GameObject <effect>__2;
        internal GameObject <effect>__6;
        internal Vector3 <effectWorldPos>__1;
        internal int <index>__7;
        internal Vector3 <moveRef>__4;
        internal float <remainTime>__5;
        internal Vector3 <targetPos>__3;
        internal Vector3 <uiPos>__0;
        internal float delayTime;
        internal bool manual;

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
                    if (BattlePanel.GetInstance() != null)
                    {
                        this.<uiPos>__0 = BattlePanel.GetInstance().GetBoxPos();
                        this.$current = new WaitForSeconds(this.delayTime);
                        this.$PC = 1;
                        goto Label_03ED;
                    }
                    goto Label_03EB;

                case 1:
                    this.<uiPos>__0.z = 10f;
                    this.<effectWorldPos>__1 = BattleGlobalFunc.GUIToWorld(this.<uiPos>__0);
                    if (!this.manual)
                    {
                        this.<effect>__6 = ObjectManager.CreateTempObj(BattleGlobal.DropEffectPrefab, this.<>f__this.transform.position, 1.1f);
                        object[] args = new object[] { "position", this.<effectWorldPos>__1, "time", 1 };
                        iTween.MoveTo(this.<effect>__6, iTween.Hash(args));
                        this.$current = new WaitForSeconds(0.2f);
                        this.$PC = 5;
                    }
                    else
                    {
                        this.<>f__this.animation.Play("kaiqi");
                        this.$current = new WaitForSeconds(1.3f);
                        this.$PC = 2;
                    }
                    goto Label_03ED;

                case 2:
                {
                    this.<effect>__2 = ObjectManager.CreateTempObj("BattlePrefabs/Item3DObject", this.<>f__this.transform.position, 3.1f);
                    RecruitAnimation.ItemObjectMorph(this.<effect>__2, this.<>f__this.property.item_entry, this.<>f__this.property.item_param);
                    this.<effect>__2.transform.position = this.<>f__this.transform.position + new Vector3(0f, 1f, 0f);
                    object[] objArray1 = new object[] { "position", this.<effect>__2.transform.position + new Vector3(0f, 1f, 0f), "time", 0.5f };
                    iTween.MoveTo(this.<effect>__2, iTween.Hash(objArray1));
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 3;
                    goto Label_03ED;
                }
                case 3:
                    this.<targetPos>__3 = this.<effectWorldPos>__1;
                    this.<moveRef>__4 = Vector3.zero;
                    this.<remainTime>__5 = 0.6f;
                    break;

                case 4:
                    break;

                case 5:
                    goto Label_032F;

                case 6:
                    if (BattlePanel.GetInstance() != null)
                    {
                        BattlePanel.GetInstance().AddItemCount();
                    }
                    this.<>f__this.isFinish = true;
                    this.$PC = -1;
                    goto Label_03EB;

                default:
                    goto Label_03EB;
            }
            if ((Vector3.Distance(this.<effect>__2.transform.position, this.<targetPos>__3) > 0.1f) && (this.<remainTime>__5 > 0f))
            {
                this.<targetPos>__3 = this.<effectWorldPos>__1;
                this.<effect>__2.transform.position = Vector3.SmoothDamp(this.<effect>__2.transform.position, this.<targetPos>__3, ref this.<moveRef>__4, 0.3f);
                this.<remainTime>__5 -= Time.deltaTime;
                this.$current = null;
                this.$PC = 4;
                goto Label_03ED;
            }
            ObjectManager.DestoryObj(this.<effect>__2);
        Label_032F:
            this.<index>__7 = 0;
            while (this.<index>__7 < this.<>f__this.transform.childCount)
            {
                this.<child>__8 = this.<>f__this.transform.GetChild(this.<index>__7);
                if (this.<child>__8 != null)
                {
                    this.<child>__8.gameObject.SetActive(false);
                }
                this.<index>__7++;
            }
            this.$current = new WaitForSeconds(0.6f);
            this.$PC = 6;
            goto Label_03ED;
        Label_03EB:
            return false;
        Label_03ED:
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

