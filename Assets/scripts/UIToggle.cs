﻿using AnimationOrTween;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Toggle")]
public class UIToggle : UIWidgetContainer
{
    public Animation activeAnimation;
    public UIWidget activeSprite;
    [HideInInspector, SerializeField]
    private Animation checkAnimation;
    [SerializeField, HideInInspector]
    private UISprite checkSprite;
    public static UIToggle current;
    [SerializeField, HideInInspector]
    private GameObject eventReceiver;
    [HideInInspector, SerializeField]
    private string functionName = "OnActivate";
    public int group;
    public bool instantTween;
    public static BetterList<UIToggle> list = new BetterList<UIToggle>();
    private bool mIsActive = true;
    private bool mStarted;
    public List<EventDelegate> onChange = new List<EventDelegate>();
    public bool optionCanBeNone;
    public bool startsActive;
    [SerializeField, HideInInspector]
    private bool startsChecked;

    public static UIToggle GetActiveToggle(int group)
    {
        for (int i = 0; i < list.size; i++)
        {
            UIToggle toggle = list[i];
            if (((toggle != null) && (toggle.group == group)) && toggle.mIsActive)
            {
                return toggle;
            }
        }
        return null;
    }

    private void OnClick()
    {
        if (base.enabled)
        {
            this.value = !this.value;
        }
    }

    private void OnDisable()
    {
        list.Remove(this);
    }

    private void OnEnable()
    {
        list.Add(this);
    }

    private void Set(bool state)
    {
        if (!this.mStarted)
        {
            this.mIsActive = state;
            this.startsActive = state;
            if (this.activeSprite != null)
            {
                this.activeSprite.alpha = !state ? 0f : 1f;
            }
        }
        else if (this.mIsActive != state)
        {
            if ((this.group != 0) && state)
            {
                int num = 0;
                int size = list.size;
                while (num < size)
                {
                    UIToggle toggle = list[num];
                    if ((toggle != this) && (toggle.group == this.group))
                    {
                        toggle.Set(false);
                    }
                    if (list.size != size)
                    {
                        size = list.size;
                        num = 0;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            this.mIsActive = state;
            if (this.activeSprite != null)
            {
                if (this.instantTween)
                {
                    this.activeSprite.alpha = !this.mIsActive ? 0f : 1f;
                }
                else
                {
                    TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, !this.mIsActive ? 0f : 1f);
                }
            }
            if (current == null)
            {
                current = this;
                if (EventDelegate.IsValid(this.onChange))
                {
                    EventDelegate.Execute(this.onChange);
                }
                else if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.functionName))
                {
                    this.eventReceiver.SendMessage(this.functionName, this.mIsActive, SendMessageOptions.DontRequireReceiver);
                }
                current = null;
            }
            if (this.activeAnimation != null)
            {
                ActiveAnimation animation = ActiveAnimation.Play(this.activeAnimation, !state ? AnimationOrTween.Direction.Reverse : AnimationOrTween.Direction.Forward);
                if (this.instantTween)
                {
                    animation.Finish();
                }
            }
        }
    }

    private void Start()
    {
        if (this.startsChecked)
        {
            this.startsChecked = false;
            this.startsActive = true;
        }
        if (!Application.isPlaying)
        {
            if ((this.checkSprite != null) && (this.activeSprite == null))
            {
                this.activeSprite = this.checkSprite;
                this.checkSprite = null;
            }
            if ((this.checkAnimation != null) && (this.activeAnimation == null))
            {
                this.activeAnimation = this.checkAnimation;
                this.checkAnimation = null;
            }
            if (Application.isPlaying && (this.activeSprite != null))
            {
                this.activeSprite.alpha = !this.startsActive ? 0f : 1f;
            }
            if (EventDelegate.IsValid(this.onChange))
            {
                this.eventReceiver = null;
                this.functionName = null;
            }
        }
        else
        {
            this.mIsActive = !this.startsActive;
            this.mStarted = true;
            bool instantTween = this.instantTween;
            this.instantTween = true;
            this.Set(this.startsActive);
            this.instantTween = instantTween;
        }
    }

    [Obsolete("Use 'value' instead")]
    public bool isChecked
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value;
        }
    }

    public bool value
    {
        get
        {
            return this.mIsActive;
        }
        set
        {
            if (((this.group == 0) || value) || (this.optionCanBeNone || !this.mStarted))
            {
                this.Set(value);
            }
        }
    }
}

