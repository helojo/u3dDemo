using System;
using UnityEngine;

public class EffectTimeDelay : MonoBehaviour
{
    public bool IsPause;
    public EffectTimeDelayMode ModeSet;
    public float timeDelay;
    public Transform trans;

    private void OnEffectEventFire()
    {
        switch (this.ModeSet)
        {
            case EffectTimeDelayMode.GameObjectOn:
                this.trans.gameObject.SetActive(true);
                break;

            case EffectTimeDelayMode.GameObjectOff:
                this.trans.gameObject.SetActive(false);
                break;

            case EffectTimeDelayMode.EmitterOn:
                if (this.trans.particleEmitter != null)
                {
                    this.trans.particleEmitter.emit = true;
                }
                break;

            case EffectTimeDelayMode.EmitterOff:
                if (this.trans.particleEmitter != null)
                {
                    this.trans.particleEmitter.emit = false;
                }
                break;

            case EffectTimeDelayMode.AnimationStart:
                if (this.trans.animation != null)
                {
                    this.trans.animation.Play();
                }
                break;

            case EffectTimeDelayMode.AnimationStop:
                if (this.trans.animation != null)
                {
                    this.trans.animation.Stop();
                }
                break;
        }
    }

    private void Start()
    {
        if ((this.trans != null) && (this.ModeSet != EffectTimeDelayMode.None))
        {
        }
    }

    private void Update()
    {
        if ((((this.trans != null) && (this.ModeSet != EffectTimeDelayMode.None)) && !this.IsPause) && (this.timeDelay >= 0f))
        {
            this.timeDelay -= Time.deltaTime;
            if (this.timeDelay < 0f)
            {
                this.OnEffectEventFire();
            }
        }
    }

    public enum EffectTimeDelayMode
    {
        None,
        GameObjectOn,
        GameObjectOff,
        EmitterOn,
        EmitterOff,
        AnimationStart,
        AnimationStop
    }
}

