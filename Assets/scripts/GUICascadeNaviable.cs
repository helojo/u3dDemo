using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GUICascadeNaviable : GUINavigable
{
    private System.Action actMoveOut;
    private float horz_offset = 400f;
    private float vert_offset = 100f;

    public override void FadeIn(System.Action actCompleted)
    {
        <FadeIn>c__AnonStorey1E9 storeye = new <FadeIn>c__AnonStorey1E9 {
            actCompleted = actCompleted,
            <>f__this = this
        };
        base.entity.Flexible(true);
        base.entity.Hidden = false;
        TweenAlpha alpha = TweenAlpha.Begin(base.gameObject, 0.25f, 0.8f);
        alpha.method = UITweener.Method.EaseOut;
        alpha.from = 0f;
        alpha.SetOnFinished((EventDelegate.Callback) null);
        TweenScale scale = TweenScale.Begin(base.gameObject, 0.3f, Vector3.one);
        scale.method = UITweener.Method.EaseIn;
        scale.from = new Vector3(0.8f, 0.8f, 0.8f);
        scale.delay = 0.1f;
        scale.SetOnFinished(new EventDelegate.Callback(storeye.<>m__31D));
        TweenAlpha alpha2 = TweenAlpha.Begin(base.gameObject, 0.1f, 1f);
        alpha2.method = UITweener.Method.EaseOut;
        alpha2.delay = 0.15f;
        alpha2.from = 0.8f;
        alpha2.SetOnFinished((EventDelegate.Callback) null);
        TweenPosition position = TweenPosition.Begin(base.gameObject, 0.3f, Vector3.zero);
        position.method = UITweener.Method.EaseIn;
        position.from = new Vector3(-this.horz_offset, this.vert_offset, 0f);
        position.delay = 0.1f;
        position.SetOnFinished((EventDelegate.Callback) null);
    }

    public override void FadeOut(System.Action actCompleted)
    {
        <FadeOut>c__AnonStorey1EA storeyea = new <FadeOut>c__AnonStorey1EA {
            actCompleted = actCompleted,
            <>f__this = this
        };
        if (base.gameObject.activeSelf)
        {
            base.entity.Flexible(true);
            TweenScale scale = TweenScale.Begin(base.gameObject, 0.45f, new Vector3(0.8f, 0.8f, 0.8f));
            scale.method = UITweener.Method.EaseOut;
            scale.from = Vector3.one;
            scale.SetOnFinished((EventDelegate.Callback) null);
            TweenAlpha alpha = TweenAlpha.Begin(base.gameObject, 0.3f, 0.7f);
            alpha.method = UITweener.Method.EaseOut;
            alpha.from = 1f;
            alpha.SetOnFinished((EventDelegate.Callback) null);
            TweenPosition position = TweenPosition.Begin(base.gameObject, 0.35f, new Vector3(-this.horz_offset, this.vert_offset, 0f));
            position.method = UITweener.Method.EaseOut;
            position.from = Vector3.zero;
            position.SetOnFinished(new EventDelegate.Callback(storeyea.<>m__31E));
        }
        else
        {
            Vector3 localPosition = base.transform.localPosition;
            base.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            base.gameObject.transform.localPosition = new Vector3(-this.horz_offset, this.vert_offset, 0f);
        }
    }

    public override void MoveOut(System.Action actCompleted)
    {
        this.actMoveOut = actCompleted;
        base.transform.localPosition = Vector3.zero;
        Vector3 pos = new Vector3(GUIMgr.Instance.Root.manualWidth * 1.5f, 130f, 0f);
        base.entity.Flexible(true);
        TweenPosition position = TweenPosition.Begin(base.gameObject, 0.3f, pos);
        position.method = UITweener.Method.EaseIn;
        position.SetOnFinished((EventDelegate.Callback) (() => this.OnMoveOutFinished()));
        TweenScale scale = TweenScale.Begin(base.gameObject, 0.4f, new Vector3(0.7f, 0.7f, 0.7f));
        scale.method = UITweener.Method.EaseIn;
        scale.SetOnFinished((EventDelegate.Callback) null);
    }

    private void OnMoveOutFinished()
    {
        if (this.actMoveOut != null)
        {
            this.actMoveOut();
        }
    }

    public override void Reset()
    {
        TweenAlpha component = base.GetComponent<TweenAlpha>();
        if (null != component)
        {
            component.cachedRect.alpha = 1f;
            component.from = 1f;
            component.to = 1f;
        }
        TweenPosition position = base.GetComponent<TweenPosition>();
        if (null != position)
        {
            position.value = Vector3.zero;
            position.from = Vector3.zero;
            position.to = Vector3.zero;
        }
        TweenScale scale = base.GetComponent<TweenScale>();
        if (null != scale)
        {
            scale.value = Vector3.one;
            scale.from = Vector3.one;
            scale.to = Vector3.one;
        }
        base.transform.localScale = Vector3.one;
        base.transform.localPosition = Vector3.zero;
    }

    [CompilerGenerated]
    private sealed class <FadeIn>c__AnonStorey1E9
    {
        internal GUICascadeNaviable <>f__this;
        internal System.Action actCompleted;

        internal void <>m__31D()
        {
            if (this.actCompleted != null)
            {
                this.actCompleted();
            }
            this.<>f__this.entity.Flexible(false);
        }
    }

    [CompilerGenerated]
    private sealed class <FadeOut>c__AnonStorey1EA
    {
        internal GUICascadeNaviable <>f__this;
        internal System.Action actCompleted;

        internal void <>m__31E()
        {
            this.<>f__this.entity.Flexible(false);
            this.<>f__this.entity.Hidden = true;
            if (this.actCompleted != null)
            {
                this.actCompleted();
            }
        }
    }
}

