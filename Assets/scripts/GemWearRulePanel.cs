using System;
using Toolbox;

public class GemWearRulePanel : GUIEntity
{
    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        base.FindChild<UIButton>("CancelBtn").OnUIMouseClick(s => GUIMgr.Instance.ExitModelGUI(this));
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
    }
}

