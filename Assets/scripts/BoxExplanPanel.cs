using FastBuf;
using System;

public class BoxExplanPanel : GUIEntity
{
    public override void OnInitialize()
    {
        UILabel component = base.gameObject.transform.FindChild("CenterGrop/B1/Desc").GetComponent<UILabel>();
        UILabel label2 = base.gameObject.transform.FindChild("CenterGrop/B2/Desc").GetComponent<UILabel>();
        UILabel label3 = base.gameObject.transform.FindChild("CenterGrop/B3/Desc").GetComponent<UILabel>();
        open_box_config _config = ConfigMgr.getInstance().getByEntry<open_box_config>(0);
        if (_config != null)
        {
            component.text = _config.description;
        }
        open_box_config _config2 = ConfigMgr.getInstance().getByEntry<open_box_config>(1);
        if (_config2 != null)
        {
            label2.text = _config2.description;
        }
        open_box_config _config3 = ConfigMgr.getInstance().getByEntry<open_box_config>(2);
        if (_config3 != null)
        {
            label3.text = _config3.description;
        }
    }
}

