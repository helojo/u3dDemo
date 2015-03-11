using FastBuf;
using System;
using Toolbox;
using UnityEngine;

public class FriendResultPanel : GUIEntity
{
    private long PlayerId;

    private void ClickAdd()
    {
        if (this.PlayerId == 0)
        {
            Debug.LogWarning("Friend is null!");
        }
        else if (XSingleton<SocialFriend>.Singleton.Contains(this.PlayerId))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c46));
        }
        else
        {
            SocketMgr.Instance.RequestAddFriend(this.PlayerId);
        }
    }

    private void ClickCancel()
    {
        GUIMgr.Instance.ExitModelGUI("FriendResultPanel");
    }

    private void UpdateBtnState(bool _isFriend)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("ButtonAdd").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("ButtonCancel").gameObject;
        GameObject obj4 = base.gameObject.transform.FindChild("ButtonOk").gameObject;
        if (_isFriend)
        {
            gameObject.SetActive(false);
            obj3.SetActive(false);
            obj4.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
            obj3.SetActive(true);
            obj4.SetActive(false);
        }
    }

    public void UpdateData(FriendReward friend)
    {
        this.PlayerId = friend.userInfo.id;
        ActorData data1 = ActorData.getInstance();
        data1.Eq += friend.eq;
        base.gameObject.transform.FindChild("Player/Name").GetComponent<UILabel>().text = friend.userInfo.name;
        base.gameObject.transform.FindChild("Player/Lv/Label").GetComponent<UILabel>().text = friend.userInfo.level.ToString();
        base.gameObject.transform.FindChild("Player/AllPoint/val").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.eq.ToString();
        base.gameObject.transform.FindChild("Player/CurPoint/val").GetComponent<UILabel>().text = friend.eq.ToString();
        UITexture component = base.gameObject.transform.FindChild("Player/Icon/Image").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) friend.userInfo.leaderInfo.cardInfo.entry);
        if (_config != null)
        {
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        UILabel label = base.gameObject.transform.FindChild("Tips").GetComponent<UILabel>();
        if (friend.flag)
        {
            label.text = ConfigMgr.getInstance().GetWord(0x9d2ab4);
        }
        else
        {
            label.text = ConfigMgr.getInstance().GetWord(0x9d2ab3);
        }
        this.UpdateBtnState(friend.flag);
    }
}

