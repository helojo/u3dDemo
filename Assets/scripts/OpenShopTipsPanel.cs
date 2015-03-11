using System;

public class OpenShopTipsPanel : GUIEntity
{
    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public void UpdateData()
    {
        UILabel component = base.transform.FindChild("Tips").GetComponent<UILabel>();
        if (ActorData.getInstance().ShowGoblinShopTipsNew)
        {
            component.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x504), GameConstant.DefaultTextRedColor + ConfigMgr.getInstance().GetWord(0x505) + GameConstant.DefaultTextColor);
            ActorData.getInstance().TodayOpenGoblinShopTips = true;
        }
        else if (ActorData.getInstance().ShowSecretShopTipsNew)
        {
            component.text = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x504), GameConstant.DefaultTextRedColor + ConfigMgr.getInstance().GetWord(0x506) + GameConstant.DefaultTextColor);
            ActorData.getInstance().TodayOpenSecretShopTips = true;
        }
    }
}

