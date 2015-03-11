using FastBuf;
using System;
using UnityEngine;

public class GuildCommonFunc
{
    public static unsafe void UpdateGuildMemberData(GameObject obj, GuildBriefUser _data)
    {
        obj.transform.FindChild("Root/Name/Label").GetComponent<UILabel>().text = _data.name;
        obj.transform.FindChild("Root/Lev/Label").GetComponent<UILabel>().text = _data.level.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.head_entry);
        if (_config == null)
        {
            Debug.LogWarning("Head entry is " + _data.head_entry);
        }
        else
        {
            obj.transform.FindChild("Head/Qframe").GetComponent<UISprite>().color = *((Color*) &(GameConstant.ConstQuantityColor[_config.quality]));
            obj.transform.FindChild("Head/Job/Job").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[_config.class_type];
            obj.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            obj.transform.FindChild("Root/Faction/Icon").GetComponent<UISprite>().spriteName = (_data.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
        }
    }

    public static void UpdateGuildMemberData2(GameObject obj, GuildBriefUser _data)
    {
        obj.transform.FindChild("Name/Label").GetComponent<UILabel>().text = _data.name;
        obj.transform.FindChild("Head/Label").GetComponent<UILabel>().text = "LV. " + _data.level.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.head_entry);
        if (_config == null)
        {
            Debug.LogWarning("Head entry is " + _data.head_entry);
        }
        else
        {
            UISprite component = obj.transform.FindChild("Head/Qframe").GetComponent<UISprite>();
            UISprite sprite2 = obj.transform.FindChild("Head/Qframe/QIcon").GetComponent<UISprite>();
            CommonFunc.SetPlayerHeadFrame(component, sprite2, _data.head_frame_entry);
            obj.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            obj.transform.FindChild("Faction/Icon").GetComponent<UISprite>().spriteName = (_data.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
        }
    }
}

