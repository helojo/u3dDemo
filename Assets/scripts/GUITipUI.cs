using System;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class GUITipUI
{
    public T FindChild<T>(string name) where T: Component
    {
        return this.Root.transform.FindChild<T>(name);
    }

    public virtual void OnCreate()
    {
    }

    public virtual void OnDraw()
    {
        this.LastUpdate = true;
    }

    public void UpdatePos(Vector3 pos, Vector3 offset)
    {
        this.Root.transform.localPosition = offset + pos;
    }

    public int Key { get; set; }

    public bool LastUpdate { get; set; }

    public GameObject Root { get; set; }
}

