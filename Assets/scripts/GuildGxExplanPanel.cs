using System;

public class GuildGxExplanPanel : GUIEntity
{
    public override void OnDestroy()
    {
        CommonFunc.ShowFuncList(true);
        CommonFunc.ShowTitlebar(true);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        CommonFunc.ShowFuncList(false);
        CommonFunc.ShowTitlebar(false);
    }
}

