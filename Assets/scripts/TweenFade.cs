using System;
using UnityEngine;

public class TweenFade : MonoBehaviour
{
    public void AttachFadeIn(float duration)
    {
        this.Reset();
        TweenAlpha alpha = base.gameObject.AddComponent<TweenAlpha>();
        alpha.duration = duration;
        alpha.from = 0f;
        alpha.to = 1f;
    }

    public void AttachFadeout(float duration, bool deactivate)
    {
        this.Reset();
        TweenAlpha alpha = base.gameObject.AddComponent<TweenAlpha>();
        alpha.duration = duration;
        alpha.from = 1f;
        alpha.to = 0f;
        if (deactivate)
        {
            alpha.eventReceiver = base.gameObject;
            alpha.callWhenFinished = "FadeOutFinished";
        }
    }

    private void FadeOutFinished()
    {
        NGUITools.SetActive(base.gameObject, false);
    }

    public void Reset()
    {
        TweenAlpha alpha = base.gameObject.AddComponent<TweenAlpha>();
        if (alpha != null)
        {
            UnityEngine.Object.Destroy(alpha);
        }
    }
}

