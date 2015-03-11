using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    private List<int> showTimeObjIDs = new List<int>();

    public void AddShowTimeObj(int objID)
    {
        this.showTimeObjIDs.Add(objID);
    }

    public void ClearShowTimeObj()
    {
        this.showTimeObjIDs.Clear();
    }

    public float GetDeltaTimeByObjID(int objID)
    {
        float deltaTime = Time.deltaTime;
        if (this.showTimeObjIDs.Contains(objID))
        {
            return BattleGlobal.ScaleSpeed(deltaTime);
        }
        return BattleGlobal.ScaleSpeed_ShowTime(deltaTime);
    }

    public static TimeManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject target = new GameObject("TimeManager");
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance = target.AddComponent<TimeManager>();
        }
        return _instance;
    }

    public void GetShowTimeObj(List<int> result)
    {
        <GetShowTimeObj>c__AnonStorey28F storeyf = new <GetShowTimeObj>c__AnonStorey28F {
            result = result
        };
        this.showTimeObjIDs.ForEach(new Action<int>(storeyf.<>m__5EE));
    }

    public bool IsShowTimeObj(int objID)
    {
        return !this.showTimeObjIDs.Contains(objID);
    }

    [DebuggerHidden]
    public IEnumerator NewWaitForSecond(float durTime, int? objID)
    {
        return new <NewWaitForSecond>c__IteratorB5 { durTime = durTime, objID = objID, <$>durTime = durTime, <$>objID = objID, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator WaitForSecond(float startTime, float durTime, int? objID)
    {
        return new <WaitForSecond>c__IteratorB6 { durTime = durTime, objID = objID, <$>durTime = durTime, <$>objID = objID, <>f__this = this };
    }

    [CompilerGenerated]
    private sealed class <GetShowTimeObj>c__AnonStorey28F
    {
        internal List<int> result;

        internal void <>m__5EE(int obj)
        {
            this.result.Add(obj);
        }
    }

    [CompilerGenerated]
    private sealed class <NewWaitForSecond>c__IteratorB5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>durTime;
        internal int? <$>objID;
        internal TimeManager <>f__this;
        internal float durTime;
        internal int? objID;

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
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.WaitForSecond(Time.time, this.durTime, this.objID));
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
    private sealed class <WaitForSecond>c__IteratorB6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal float <$>durTime;
        internal int? <$>objID;
        internal TimeManager <>f__this;
        internal float <deltaTime>__0;
        internal float durTime;
        internal int? objID;

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
                    if (this.durTime > 0f)
                    {
                        this.<deltaTime>__0 = 0f;
                        if (this.objID.HasValue)
                        {
                            this.<deltaTime>__0 = this.<>f__this.GetDeltaTimeByObjID(this.objID.Value);
                        }
                        else
                        {
                            this.<deltaTime>__0 = Time.deltaTime;
                        }
                        this.durTime -= this.<deltaTime>__0;
                        this.$current = null;
                        this.$PC = 1;
                        return true;
                    }
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

