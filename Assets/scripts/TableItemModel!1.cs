using System;
using System.Runtime.CompilerServices;

public abstract class TableItemModel<T> where T: TableItemTemplate, new()
{
    public virtual void Init(T template, UITableItem item)
    {
        this.Item = item;
        this.Template = template;
    }

    public UITableItem Item { get; private set; }

    public T Template { get; private set; }
}

