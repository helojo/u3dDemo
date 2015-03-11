using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendListPanel : GUIEntity
{
    private Action<BriefUser> _CallbackFunc;
    private List<Friend> _gameFriends;
    private UILabel _PageLabel;
    private int MaxPageCount = 10;
    private int mCurrPage;
    private int mFriendSumPage;
    public GameObject SingleFriendItem;

    private void InitFriendList(int page)
    {
        UIGrid component = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        CommonFunc.ResetClippingPanel(base.transform.FindChild("List"));
        int count = this.GameFriends.Count;
        int maxPageCount = (page >= (this.mFriendSumPage - 1)) ? (this.GameFriends.Count % this.MaxPageCount) : this.MaxPageCount;
        if ((this.GameFriends.Count > 0) && ((this.GameFriends.Count % this.MaxPageCount) == 0))
        {
            maxPageCount = this.MaxPageCount;
        }
        for (int i = 0; i < maxPageCount; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleFriendItem) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localPosition = new Vector3(0f, -i * component.cellHeight, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            Transform transform = obj2.transform.FindChild("Item1");
            if ((i + (page * this.MaxPageCount)) < count)
            {
                this.SetFriendInfo(transform, this.GameFriends[i + (page * this.MaxPageCount)]);
            }
        }
        if (this.mFriendSumPage == 0)
        {
            this._PageLabel.text = "0/0";
        }
        else
        {
            this._PageLabel.text = (page + 1) + "/" + this.mFriendSumPage;
        }
    }

    private void OnClickItemBtn(GameObject go)
    {
        if (this._CallbackFunc != null)
        {
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                Friend friend = obj2 as Friend;
                this._CallbackFunc(friend.userInfo);
                GUIMgr.Instance.ExitModelGUI(base.name);
            }
        }
    }

    private void OnClickLeft(GameObject go)
    {
        this.mCurrPage--;
        if (this.mCurrPage < 0)
        {
            this.mCurrPage = (this.mFriendSumPage > 0) ? (this.mFriendSumPage - 1) : 0;
        }
        this.InitFriendList(this.mCurrPage);
    }

    private void OnClickRight(GameObject go)
    {
        this.mCurrPage++;
        if (this.mCurrPage >= this.mFriendSumPage)
        {
            this.mCurrPage = 0;
        }
        this.InitFriendList(this.mCurrPage);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.SetFriends();
        this.mFriendSumPage = (int) Math.Ceiling((double) (((float) this.GameFriends.Count) / ((float) this.MaxPageCount)));
        base.transform.FindChild("FriendCount").GetComponent<UILabel>().text = this.GameFriends.Count + "/" + ActorData.getInstance().MaxFriendCount;
        Transform transform = base.transform.FindChild("Pages/bt_prv");
        Transform transform2 = base.transform.FindChild("Pages/bt_next");
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLeft);
        UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRight);
        this._PageLabel = base.transform.FindChild("Pages/LabelPageCount/lb_pagenum").GetComponent<UILabel>();
        this.InitFriendList(this.mCurrPage);
    }

    public void SelectCallBack(Action<BriefUser> callbackFunc)
    {
        this._CallbackFunc = callbackFunc;
    }

    private void SetFriendInfo(Transform obj, Friend _data)
    {
        if (_data != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_data.userInfo.head_entry);
            if (_config != null)
            {
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                obj.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[_config.class_type];
                UISprite component = obj.FindChild("QualityBorder").GetComponent<UISprite>();
                UISprite sprite3 = obj.FindChild("QualityBorder/QIcon").GetComponent<UISprite>();
                CommonFunc.SetPlayerHeadFrame(component, sprite3, _data.userInfo.head_frame_entry);
                obj.FindChild("Name").GetComponent<UILabel>().text = _data.userInfo.name;
                obj.FindChild("Level").GetComponent<UILabel>().text = _data.userInfo.level.ToString();
                obj.FindChild("LastLoginTime").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x989694), TimeMgr.Instance.GetSendTime(_data.userInfo.lastOnlineTime));
                obj.FindChild("Camp").GetComponent<UISprite>().spriteName = (_data.userInfo.faction != FactionType.Alliance) ? GameConstant.Const_BuLuoIcon : GameConstant.Const_LianMengIcon;
                GUIDataHolder.setData(obj.gameObject, _data);
                UIEventListener.Get(obj.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
            }
        }
    }

    private void SetFriends()
    {
        List<Friend> list = ActorData.getInstance().FriendList.ToList<Friend>();
        this._gameFriends = list;
    }

    private List<Friend> GameFriends
    {
        get
        {
            if (this._gameFriends == null)
            {
                this.SetFriends();
            }
            return this._gameFriends;
        }
    }
}

