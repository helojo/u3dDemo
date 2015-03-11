using System;
using UnityEngine;

public abstract class GUINavigable : MonoBehaviour
{
    protected GUIEntity entity;

    protected GUINavigable()
    {
    }

    private void Awake()
    {
        this.entity = base.GetComponent<GUIEntity>();
    }

    public abstract void FadeIn(System.Action act);
    public abstract void FadeOut(System.Action act);
    public abstract void MoveOut(System.Action act);
    public abstract void Reset();
}

