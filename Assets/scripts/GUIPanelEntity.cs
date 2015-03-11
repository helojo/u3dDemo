using System;

public class GUIPanelEntity : GUIEntity
{
    public virtual void Initialize()
    {
    }

    public virtual void InitializePanel()
    {
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.InitializePanel();
        this.Initialize();
    }
}

