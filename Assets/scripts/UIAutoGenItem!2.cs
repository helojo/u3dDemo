using System;
using System.Runtime.CompilerServices;

public class UIAutoGenItem<T1, T2> : UITableItem where T1: TableItemTemplate, new() where T2: TableItemModel<T1>, new()
{
    public override void OnCreate()
    {
        this.Template = Activator.CreateInstance<T1>();
        this.Template.Init(this);
        this.Model = Activator.CreateInstance<T2>();
        this.Model.Init(this.Template, this);
    }

    public T2 Model { get; private set; }

    public T1 Template { get; private set; }
}

