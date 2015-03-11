using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITweener : MonoBehaviour
{
    [HideInInspector]
    public AnimationCurve animationCurve;
    [HideInInspector]
    public string callWhenFinished;
    public static UITweener current;
    [HideInInspector]
    public float delay;
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public GameObject eventReceiver;
    [HideInInspector]
    public bool ignoreTimeScale;
    private float mAmountPerDelta;
    private float mDuration;
    [HideInInspector]
    public Method method;
    private float mFactor;
    private List<EventDelegate> mPingPongTemp;
    private bool mStarted;
    private float mStartTime;
    private List<EventDelegate> mTemp;
    [HideInInspector]
    public List<EventDelegate> onFinished;
    [HideInInspector]
    public List<EventDelegate> onPingPong;
    [HideInInspector]
    public bool steeperCurves;
    [HideInInspector]
    public Style style;
    [HideInInspector]
    public int tweenGroup;

    protected UITweener()
    {
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        this.animationCurve = new AnimationCurve(keys);
        this.ignoreTimeScale = true;
        this.duration = 1f;
        this.onFinished = new List<EventDelegate>();
        this.onPingPong = new List<EventDelegate>();
        this.mAmountPerDelta = 1000f;
    }

    public void AddOnFinished(EventDelegate del)
    {
        EventDelegate.Add(this.onFinished, del);
    }

    public void AddOnFinished(EventDelegate.Callback del)
    {
        EventDelegate.Add(this.onFinished, del);
    }

    public static T Begin<T>(GameObject go, float duration) where T: UITweener
    {
        T component = go.GetComponent<T>();
        if ((component != null) && (component.tweenGroup != 0))
        {
            component = null;
            T[] components = go.GetComponents<T>();
            int index = 0;
            int length = components.Length;
            while (index < length)
            {
                component = components[index];
                if ((component != null) && (component.tweenGroup == 0))
                {
                    break;
                }
                component = null;
                index++;
            }
        }
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        component.mStarted = false;
        component.duration = duration;
        component.mFactor = 0f;
        component.mAmountPerDelta = Mathf.Abs(component.mAmountPerDelta);
        component.style = Style.Once;
        Keyframe[] keys = new Keyframe[] { new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f) };
        component.animationCurve = new AnimationCurve(keys);
        component.eventReceiver = null;
        component.callWhenFinished = null;
        component.enabled = true;
        if (duration <= 0f)
        {
            component.Sample(1f, true);
            component.enabled = false;
        }
        return component;
    }

    private float BounceLogic(float val)
    {
        if (val < 0.363636f)
        {
            val = (7.5685f * val) * val;
            return val;
        }
        if (val < 0.727272f)
        {
            val = ((7.5625f * (val -= 0.545454f)) * val) + 0.75f;
            return val;
        }
        if (val < 0.90909f)
        {
            val = ((7.5625f * (val -= 0.818181f)) * val) + 0.9375f;
            return val;
        }
        val = ((7.5625f * (val -= 0.9545454f)) * val) + 0.984375f;
        return val;
    }

    private void DoPingPongEvent()
    {
        if (this.onPingPong != null)
        {
            this.mPingPongTemp = this.onPingPong;
            this.onPingPong = new List<EventDelegate>();
            EventDelegate.Execute(this.mPingPongTemp);
            for (int i = 0; i < this.mPingPongTemp.Count; i++)
            {
                EventDelegate ev = this.mPingPongTemp[i];
                if (ev != null)
                {
                    EventDelegate.Add(this.onPingPong, ev, ev.oneShot);
                }
            }
            this.mPingPongTemp = null;
        }
    }

    private float easeOutElastic(float value)
    {
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        float num4 = 0f;
        if (value == 0f)
        {
            return 0f;
        }
        if ((value /= num) == 1f)
        {
            return 1f;
        }
        if ((num4 == 0f) || (num4 < 1f))
        {
            num4 = 1f;
            num3 = num2 / 4f;
        }
        else
        {
            num3 = (num2 / 6.283185f) * Mathf.Asin(1f / num4);
        }
        return (((num4 * Mathf.Pow(2f, -10f * value)) * Mathf.Sin((((value * num) - num3) * 6.283185f) / num2)) + 1f);
    }

    private void OnDisable()
    {
        this.mStarted = false;
    }

    protected abstract void OnUpdate(float factor, bool isFinished);
    [Obsolete("Use PlayForward() instead")]
    public void Play()
    {
        this.Play(true);
    }

    public void Play(bool forward)
    {
        this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        if (!forward)
        {
            this.mAmountPerDelta = -this.mAmountPerDelta;
        }
        base.enabled = true;
        this.Update();
    }

    public void PlayForward()
    {
        this.Play(true);
    }

    public void PlayReverse()
    {
        this.Play(false);
    }

    public void RemoveOnFinished(EventDelegate del)
    {
        if (this.onFinished != null)
        {
            this.onFinished.Remove(del);
        }
        if (this.mTemp != null)
        {
            this.mTemp.Remove(del);
        }
    }

    public void Reset()
    {
        if (!this.mStarted)
        {
            this.SetStartToCurrentValue();
            this.SetEndToCurrentValue();
        }
    }

    public void ResetFactor(float f)
    {
        this.mFactor = f;
    }

    public void ResetToBeginning()
    {
        this.mStarted = false;
        this.mFactor = (this.mAmountPerDelta >= 0f) ? 0f : 1f;
        this.Sample(this.mFactor, false);
    }

    public void Sample(float factor, bool isFinished)
    {
        float f = Mathf.Clamp01(factor);
        if (this.method == Method.EaseIn)
        {
            f = 1f - Mathf.Sin(1.570796f * (1f - f));
            if (this.steeperCurves)
            {
                f *= f;
            }
        }
        else if (this.method == Method.EaseOut)
        {
            f = Mathf.Sin(1.570796f * f);
            if (this.steeperCurves)
            {
                f = 1f - f;
                f = 1f - (f * f);
            }
        }
        else if (this.method == Method.EaseInOut)
        {
            f -= Mathf.Sin(f * 6.283185f) / 6.283185f;
            if (this.steeperCurves)
            {
                f = (f * 2f) - 1f;
                float num3 = Mathf.Sign(f);
                f = 1f - Mathf.Abs(f);
                f = 1f - (f * f);
                f = ((num3 * f) * 0.5f) + 0.5f;
            }
        }
        else if (this.method == Method.BounceIn)
        {
            f = this.BounceLogic(f);
        }
        else if (this.method == Method.BounceOut)
        {
            f = 1f - this.BounceLogic(1f - f);
        }
        else if (this.method == Method.EaseInOutSine)
        {
            f = -0.5f * (Mathf.Cos((3.141593f * f) / 1f) - 1f);
        }
        else if (this.method == Method.EaseInExpo)
        {
            f = Mathf.Pow(2f, 10f * ((f / 1f) - 1f));
        }
        else if (this.method == Method.EaseOutExpo)
        {
            f = -Mathf.Pow(2f, (-10f * f) / 1f) + 1f;
        }
        else if (this.method == Method.EaseInOutExpo)
        {
            f /= 0.5f;
            if (f < 1f)
            {
                f = 0.5f * Mathf.Pow(2f, 10f * (f - 1f));
            }
            else
            {
                f--;
                f = 0.5f * (-Mathf.Pow(2f, -10f * f) + 2f);
            }
        }
        else if (this.method == Method.EaseOutElastic)
        {
            f = this.easeOutElastic(f);
        }
        else if (this.method == Method.SpringIn)
        {
            float num4 = 1f;
            if (f < 0.25f)
            {
                float num5 = 1f - (4f * f);
                num4 = 1f - ((((0.8f * num5) * num5) * num5) * num5);
            }
            float num6 = !this.steeperCurves ? ((float) 15) : ((float) 0x19);
            f = num4 - ((0.2f * (1f - f)) * Mathf.Cos(num6 * f));
        }
        this.OnUpdate((this.animationCurve == null) ? f : this.animationCurve.Evaluate(f), isFinished);
    }

    public virtual void SetEndToCurrentValue()
    {
    }

    public void SetOnFinished(EventDelegate del)
    {
        EventDelegate.Set(this.onFinished, del);
    }

    public void SetOnFinished(EventDelegate.Callback del)
    {
        EventDelegate.Set(this.onFinished, del);
    }

    public virtual void SetStartToCurrentValue()
    {
    }

    public virtual void Start()
    {
        this.Update();
    }

    public void Stop()
    {
        float num = !this.ignoreTimeScale ? Time.time : RealTime.time;
        this.mStartTime = num - this.delay;
    }

    public void Toggle()
    {
        if (this.mFactor > 0f)
        {
            this.mAmountPerDelta = -this.amountPerDelta;
        }
        else
        {
            this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        }
        base.enabled = true;
    }

    private void Update()
    {
        float num = !this.ignoreTimeScale ? Time.deltaTime : RealTime.deltaTime;
        float num2 = !this.ignoreTimeScale ? Time.time : RealTime.time;
        if (!this.mStarted)
        {
            this.mStarted = true;
            this.mStartTime = num2 + this.delay;
        }
        if (num2 >= this.mStartTime)
        {
            this.mFactor += this.amountPerDelta * num;
            if (this.style == Style.Loop)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor -= Mathf.Floor(this.mFactor);
                }
            }
            else if (this.style == Style.PingPong)
            {
                if (this.mFactor > 1f)
                {
                    this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                    this.DoPingPongEvent();
                }
                else if (this.mFactor < 0f)
                {
                    this.mFactor = -this.mFactor;
                    this.mFactor -= Mathf.Floor(this.mFactor);
                    this.mAmountPerDelta = -this.mAmountPerDelta;
                    this.DoPingPongEvent();
                }
            }
            if ((this.style == Style.Once) && (((this.duration == 0f) || (this.mFactor > 1f)) || (this.mFactor < 0f)))
            {
                this.mFactor = Mathf.Clamp01(this.mFactor);
                this.Sample(this.mFactor, true);
                if (((this.duration == 0f) || ((this.mFactor == 1f) && (this.mAmountPerDelta > 0f))) || ((this.mFactor == 0f) && (this.mAmountPerDelta < 0f)))
                {
                    base.enabled = false;
                }
                if (current == null)
                {
                    current = this;
                    if (this.onFinished != null)
                    {
                        this.mTemp = this.onFinished;
                        this.onFinished = new List<EventDelegate>();
                        EventDelegate.Execute(this.mTemp);
                        for (int i = 0; i < this.mTemp.Count; i++)
                        {
                            EventDelegate ev = this.mTemp[i];
                            if (ev != null)
                            {
                                EventDelegate.Add(this.onFinished, ev, ev.oneShot);
                            }
                        }
                        this.mTemp = null;
                    }
                    if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
                    {
                        this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
                    }
                    current = null;
                }
            }
            else
            {
                this.Sample(this.mFactor, false);
            }
        }
    }

    public float amountPerDelta
    {
        get
        {
            if (this.mDuration != this.duration)
            {
                this.mDuration = this.duration;
                this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration));
            }
            return this.mAmountPerDelta;
        }
    }

    public AnimationOrTween.Direction direction
    {
        get
        {
            return ((this.mAmountPerDelta >= 0f) ? AnimationOrTween.Direction.Forward : AnimationOrTween.Direction.Reverse);
        }
    }

    public float tweenFactor
    {
        get
        {
            return this.mFactor;
        }
        set
        {
            this.mFactor = Mathf.Clamp01(value);
        }
    }

    public enum Method
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        BounceIn,
        BounceOut,
        EaseInOutSine,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseOutElastic,
        SpringIn
    }

    public enum Style
    {
        Once,
        Loop,
        PingPong
    }
}

