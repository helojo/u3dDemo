using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleCom_ObjManager : MonoBehaviour
{
    private List<EffectSlot> effectList = new List<EffectSlot>();
    private BattleObjPool objPool = new BattleObjPool();
    private static bool ObjPoolEnbale;

    private void ClearEffect()
    {
        foreach (EffectSlot slot in this.effectList)
        {
            this.DestroyBattleObj(slot.effect);
        }
        this.effectList.Clear();
    }

    public void ClearObjs()
    {
        this.objPool.Clear();
    }

    public GameObject CreateBattleObj(string name)
    {
        GameObject obj2 = !ObjPoolEnbale ? null : this.GetRemainObj(name);
        if (obj2 == null)
        {
            UnityEngine.Object original = BundleMgr.Instance.LoadResource(name, ".prefab", typeof(GameObject));
            if (original != null)
            {
                obj2 = UnityEngine.Object.Instantiate(original) as GameObject;
                obj2.name = name;
                return obj2;
            }
            Debug.LogWarning("Can't find " + name);
        }
        return obj2;
    }

    public void DestroyBattleObj(GameObject obj)
    {
        if (obj != null)
        {
            if (ObjPoolEnbale)
            {
                this.objPool.PushGameObj(obj.name, obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }

    public GameObject GetRemainObj(string name)
    {
        return (!ObjPoolEnbale ? null : this.objPool.PullObj(name));
    }

    public GameObject InstantiateObj(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }
        GameObject obj2 = !ObjPoolEnbale ? null : this.GetRemainObj(obj.name);
        if (obj2 == null)
        {
            obj2 = UnityEngine.Object.Instantiate(obj) as GameObject;
            obj2.name = obj.name;
        }
        return obj2;
    }

    private void OnMsgEnter()
    {
        this.ClearEffect();
        this.ClearObjs();
    }

    private void OnMsgLeave()
    {
        this.ClearEffect();
        this.ClearObjs();
    }

    private GameObject PlayEffect(string effectName, Vector3 pos, float remainTime)
    {
        return this.PlayEffectObj(this.CreateBattleObj(effectName), pos, remainTime);
    }

    private GameObject PlayEffectObj(GameObject effect, Vector3 pos, float remainTime)
    {
        if (null != effect)
        {
            effect.transform.position = pos;
            this.effectList.Add(new EffectSlot(effect, Time.time, remainTime));
        }
        return effect;
    }

    private void Start()
    {
        base.StartCoroutine(this.UpdateObjPool());
    }

    private void UpdateEffect()
    {
        <UpdateEffect>c__AnonStoreyE0 ye = new <UpdateEffect>c__AnonStoreyE0 {
            <>f__this = this,
            curTime = Time.time
        };
        this.effectList.RemoveAll(new Predicate<EffectSlot>(ye.<>m__42));
    }

    [DebuggerHidden]
    private IEnumerator UpdateObjPool()
    {
        return new <UpdateObjPool>c__Iterator5 { <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <UpdateEffect>c__AnonStoreyE0
    {
        internal BattleCom_ObjManager <>f__this;
        internal float curTime;

        internal bool <>m__42(BattleCom_ObjManager.EffectSlot obj)
        {
            if ((obj.startTime + obj.remainTime) >= this.curTime)
            {
                return false;
            }
            if (obj.effect != null)
            {
                this.<>f__this.DestroyBattleObj(obj.effect);
            }
            return true;
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateObjPool>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleCom_ObjManager <>f__this;

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
                case 1:
                    this.<>f__this.UpdateEffect();
                    this.<>f__this.objPool.Update();
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    return true;

                default:
                    break;
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

    [StructLayout(LayoutKind.Sequential)]
    private struct EffectSlot
    {
        public GameObject effect;
        public float startTime;
        public float remainTime;
        public EffectSlot(GameObject _effect, float _startTime, float _remainTime)
        {
            this.effect = _effect;
            this.startTime = _startTime;
            this.remainTime = _remainTime;
        }
    }
}

