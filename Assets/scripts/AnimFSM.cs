using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimFSM : MonoBehaviour
{
    private HashSet<string> addedEventState = new HashSet<string>();
    private System.Action animEvent;
    private static readonly float AnimFadeTime = 0.3f;
    private AnimFSMInfo curState;
    private AnimFSMInfoTable info;
    private float speed = 1f;

    private void CheckFinishEvent(AnimationState state)
    {
        if ((state.wrapMode == WrapMode.Once) && !this.addedEventState.Contains(state.name))
        {
            this.addedEventState.Add(state.name);
            AnimationClip clip = base.animation.GetClip(state.name);
            if (clip != null)
            {
                AnimationEvent evt = new AnimationEvent {
                    functionName = "OnAnimFinish"
                };
                if (clip.length < AnimFadeTime)
                {
                    evt.time = clip.length - 0.01f;
                }
                else
                {
                    evt.time = clip.length - AnimFadeTime;
                }
                clip.AddEvent(evt);
            }
        }
    }

    private bool CheckPlayAnimable(AnimFSMInfo newState)
    {
        if ((this.curState != null) && (this.curState.priority > newState.priority))
        {
            return false;
        }
        return true;
    }

    public float GetStateClipLength(string newAnimName)
    {
        AnimFSMInfo state = this.info.GetState(newAnimName);
        if (state == null)
        {
            Debug.LogWarning("Anim Name is Error!");
            return 0f;
        }
        return base.animation.GetClip(state.animName).length;
    }

    public bool IsHasAnim(string animName)
    {
        this.TryInitDefault();
        AnimFSMInfo info = this.info.GetState(animName);
        if (info == null)
        {
            return false;
        }
        AnimationState state = base.animation[info.animName];
        return (state != null);
    }

    private void OnAnimFinish(AnimationEvent _event)
    {
        if (((this.curState != null) && !this.curState.loop) && (_event.animationState.name == this.curState.animName))
        {
            this.PlayNextAnim();
        }
        if (this.animEvent != null)
        {
            this.animEvent();
            this.animEvent = null;
        }
    }

    public bool PlayAnim(string newAnimName, float oneSpeed = 1, float startTime = 0, bool isForceLoop = false)
    {
        this.TryInitDefault();
        AnimFSMInfo newState = this.info.GetState(newAnimName);
        if (newState == null)
        {
            return false;
        }
        if (!this.CheckPlayAnimable(newState))
        {
            return false;
        }
        if ((this.curState != null) && (this.curState.animName == newState.animName))
        {
            base.animation.Stop(this.curState.animName);
        }
        this.curState = newState;
        AnimFSMInfo curState = this.curState;
        AnimationState state = base.animation[curState.animName];
        if (state != null)
        {
            curState.loop = curState.loop || isForceLoop;
            state.wrapMode = !curState.loop ? WrapMode.Once : WrapMode.Loop;
            state.speed = this.speed * oneSpeed;
            state.time = startTime;
            this.CheckFinishEvent(state);
            if (startTime > 0f)
            {
                base.animation.Play(curState.animName);
            }
            else
            {
                base.animation.CrossFade(curState.animName);
            }
        }
        else if (curState.name == "stand_normal")
        {
            this.PlayAnim("stand", 1f, 0f, false);
        }
        else
        {
            this.PlayNextAnim();
        }
        return true;
    }

    public bool PlayAnimAndAddFinishEvent(string newAnimName, float oneSpeed, float startTime, bool isForceLoop, System.Action e)
    {
        this.animEvent = e;
        return this.PlayAnim(newAnimName, oneSpeed, startTime, isForceLoop);
    }

    private void PlayNextAnim()
    {
        string next = this.curState.next;
        this.curState = null;
        this.PlayAnim(next, 1f, 0f, false);
    }

    public void PlayStandAnim()
    {
        this.TryInitDefault();
        this.PlayAnim(BattleGlobal.StandAnimName, 1f, 0f, false);
    }

    public void ResetAnim()
    {
        this.curState = null;
    }

    public void ResetSpeed()
    {
        this.SetSpeed(1f);
    }

    public void SetAnimSpeed(string newAnimName, float oneSpeed)
    {
        AnimFSMInfo info = this.info.GetState(newAnimName);
        if (info != null)
        {
            AnimationState state = base.animation[info.animName];
            if (state != null)
            {
                state.speed = this.speed * oneSpeed;
            }
        }
    }

    public void SetSpeed(float _speed)
    {
        this.speed = _speed;
        IEnumerator enumerator = base.animation.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                AnimationState current = (AnimationState) enumerator.Current;
                current.speed = this.speed;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public void SetStateTable(string tableName)
    {
        this.info = AnimFSMInfoManager.Instance().GetStateInfo(tableName);
    }

    public void StopCurAnim(string curAnim)
    {
        if ((this.curState != null) && (this.curState.name == curAnim))
        {
            this.PlayNextAnim();
        }
    }

    public void StopCurAnimForce()
    {
        if (this.curState != null)
        {
            this.PlayNextAnim();
        }
    }

    private void TryInitDefault()
    {
        if (this.info == null)
        {
            this.info = AnimFSMInfoManager.Instance().GetStateInfo("battle");
        }
    }
}

