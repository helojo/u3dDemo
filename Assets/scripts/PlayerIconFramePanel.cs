using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerIconFramePanel : GUIEntity
{
    public GameObject _SingleIconFrameItem;
    [CompilerGenerated]
    private static Func<HeadFrameData, bool> <>f__am$cache1;
    [CompilerGenerated]
    private static Func<HeadFrameData, bool> <>f__am$cache2;

    private void OnClickFrameItemBtn(GameObject go)
    {
        TempData data = GUIDataHolder.getData(go) as TempData;
        if (data != null)
        {
            if (ActorData.getInstance().HeadFrameEntry == data.entry)
            {
                GUIMgr.Instance.ExitModelGUI("PlayerIconFramePanel");
            }
            else if (data.active)
            {
                SocketMgr.Instance.RequestChangeHeadFrame(data.entry);
            }
            else
            {
                TipsDiag.SetText(data.strDec);
            }
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public void SetIconFrames(List<HeadFrameData> headFrameDatas)
    {
        TempData data5;
        Transform transform = base.transform.FindChild("List");
        Transform transform2 = base.transform.FindChild("List/Group2");
        int num = 4;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = h => h.bActive;
        }
        List<HeadFrameData> list = headFrameDatas.Where<HeadFrameData>(<>f__am$cache1).ToList<HeadFrameData>();
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = h => !h.bActive;
        }
        List<HeadFrameData> list2 = headFrameDatas.Where<HeadFrameData>(<>f__am$cache2).ToList<HeadFrameData>();
        foreach (HeadFrameData data in list)
        {
            headbox_config _config = ConfigMgr.getInstance().getByEntry<headbox_config>(data.headFrameEntry);
            if ((_config != null) && (_config.is_open != 0))
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleIconFrameItem) as GameObject;
                obj2.transform.parent = transform;
                obj2.transform.localPosition = new Vector3(0f, (float) num, -0.1f);
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                UILabel component = obj2.transform.FindChild("Item/Label").GetComponent<UILabel>();
                UISprite sprite = obj2.transform.FindChild("Item/FrameBg").GetComponent<UISprite>();
                UISprite sprite2 = obj2.transform.FindChild("Item/TagBg").GetComponent<UISprite>();
                obj2.transform.FindChild("Item/Lock").GetComponent<UISprite>().gameObject.SetActive(!data.bActive);
                sprite.spriteName = _config.icon1;
                sprite2.spriteName = _config.icon2;
                component.text = _config.name;
                data5 = new TempData {
                    entry = _config.entry,
                    active = true,
                    strDec = string.Empty
                };
                TempData data2 = data5;
                Transform transform3 = obj2.transform.FindChild("Item");
                GUIDataHolder.setData(transform3.gameObject, data2);
                UIEventListener.Get(transform3.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFrameItemBtn);
                num -= sprite.height + 10;
            }
        }
        transform2.transform.localPosition = new Vector3(0f, (float) (num - 30), -0.1f);
        num = (num - 30) - 4;
        foreach (HeadFrameData data3 in list2)
        {
            headbox_config _config2 = ConfigMgr.getInstance().getByEntry<headbox_config>(data3.headFrameEntry);
            if ((_config2 != null) && (_config2.is_open != 0))
            {
                GameObject obj3 = UnityEngine.Object.Instantiate(this._SingleIconFrameItem) as GameObject;
                obj3.transform.parent = transform;
                obj3.transform.localPosition = new Vector3(0f, (float) num, -0.1f);
                obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                UILabel label2 = obj3.transform.FindChild("Item/Label").GetComponent<UILabel>();
                UISprite sprite4 = obj3.transform.FindChild("Item/FrameBg").GetComponent<UISprite>();
                UISprite sprite5 = obj3.transform.FindChild("Item/TagBg").GetComponent<UISprite>();
                obj3.transform.FindChild("Item/Lock").GetComponent<UISprite>().gameObject.SetActive(!data3.bActive);
                sprite4.spriteName = _config2.icon1;
                sprite5.spriteName = _config2.icon2;
                label2.text = _config2.name;
                data5 = new TempData {
                    entry = _config2.entry,
                    active = false,
                    strDec = _config2.desire
                };
                TempData data4 = data5;
                Transform transform4 = obj3.transform.FindChild("Item");
                GUIDataHolder.setData(transform4.gameObject, data4);
                UIEventListener.Get(transform4.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFrameItemBtn);
                num -= sprite4.height + 10;
            }
        }
    }
}

