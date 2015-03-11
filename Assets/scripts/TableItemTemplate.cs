using System;
using System.Runtime.CompilerServices;

public abstract class TableItemTemplate
{
    public T FindChild<T>(string name) where T: Component
    {
        return this.Item.FindChild<T>(name);
    }

    public virtual void Init(UITableItem item)
    {
        this.Item = item;
    }

    public UITableItem Item { get; private set; }
}

