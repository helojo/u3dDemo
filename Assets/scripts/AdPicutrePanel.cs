using System;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class AdPicutrePanel : GUIEntity
{
    private int _index;
    public UILabel _lable;
    public UITexture _picTexture;
    private List<string> tempAdUrls = new List<string>();

    public void ClosePanel()
    {
        if (this._index == (XSingleton<AdManager>.Singleton.AdPics.Count - 1))
        {
            string adTime = TimeMgr.Instance.ServerDateTime.ToString("yyyy-MM-dd") + GameDefine.getInstance().lastAccountName + ServerInfo.lastGameServerId.ToString();
            XSingleton<AdManager>.Singleton.SaveAd(adTime, this.tempAdUrls);
            GUIMgr.Instance.ExitModelGUI(base.name);
        }
        else if (XSingleton<AdManager>.Singleton.AdPics.Count > (this._index + 1))
        {
            this._picTexture.mainTexture = XSingleton<AdManager>.Singleton.AdPics[this._index + 1].texture;
            this._lable.text = XSingleton<AdManager>.Singleton.AdPics[this._index + 1].desc;
            if (XSingleton<AdManager>.Singleton.AdPics[this._index + 1].ShowType == 0)
            {
                this.tempAdUrls.Add(XSingleton<AdManager>.Singleton.AdPics[this._index + 1].strPictureName);
            }
            this._index++;
        }
    }

    private void OnClickButtonCollider(GameObject go, bool state)
    {
        UIButton component = base.transform.FindChild("CancelBtn").GetComponent<UIButton>();
        if (component != null)
        {
            component.OnPress(state);
            if (!state)
            {
                this.ClosePanel();
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        UIEventListener listener1 = UIEventListener.Get(base.transform.FindChild("BtnLayer/Collider").gameObject);
        listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener1.onPress, new UIEventListener.BoolDelegate(this.OnClickButtonCollider));
    }

    public void SetPicture()
    {
        if (XSingleton<AdManager>.Singleton.AdPics.Count > 0)
        {
            this._picTexture.mainTexture = XSingleton<AdManager>.Singleton.AdPics[0].texture;
            this._lable.text = XSingleton<AdManager>.Singleton.AdPics[0].desc;
            if (XSingleton<AdManager>.Singleton.AdPics[0].ShowType == 0)
            {
                this.tempAdUrls.Add(XSingleton<AdManager>.Singleton.AdPics[0].strPictureName);
            }
        }
    }
}

