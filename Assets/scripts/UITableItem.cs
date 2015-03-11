using System;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public abstract class UITableItem
{
    private bool hide;

    public T FindChild<T>(string name) where T: Component
    {
        return this.Root.FindChild<T>(name);
    }

    public void Init(Transform root)
    {
        this.Root = root;
    }

    public abstract void OnCreate();

    public virtual bool Hide
    {
        get
        {
            return this.hide;
        }
        set
        {
            this.hide = value;
            if (this.Root.gameObject.activeSelf != !this.hide)
            {
                this.Root.gameObject.SetActive(!this.hide);
            }
        }
    }

    public Transform Root { get; private set; }
}

