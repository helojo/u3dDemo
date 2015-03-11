using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuildListPanel : GUIEntity
{
    public UILabel _PageNum;
    [CompilerGenerated]
    private static Comparison<BriefGuildData> <>f__am$cache7;
    public GameObject GuildObj;
    private int mCurrPage;
    private List<BriefGuildData> mGuildDataList = new List<BriefGuildData>();
    private int mMaxPage;
    public List<GameObject> PartList = new List<GameObject>();
    private bool ShowOpenedGuild;

    private void ClickCheckOff()
    {
        this.ShowOpenedGuild = false;
        this.DisplayGuildList();
    }

    private void ClickCheckOn()
    {
        this.ShowOpenedGuild = true;
        this.DisplayGuildList();
    }

    private void ClickCreateBtn()
    {
        UIInput component = this.PartList[1].transform.FindChild("Input").GetComponent<UIInput>();
        if (component.value.Length <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652bf));
        }
        else if (component.value.Contains("*"))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c0));
        }
        else if (ActorData.getInstance().Stone < GameConstValues.GUILD_CREATE_STONE)
        {
            <ClickCreateBtn>c__AnonStorey202 storey = new <ClickCreateBtn>c__AnonStorey202 {
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__35B), null);
        }
        else if (ActorData.getInstance().Gold < GameConstValues.GUILD_CREATE_GOLD)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
        }
        else
        {
            SocketMgr.Instance.RequestGuildCreate(component.value, CostType.E_CT_Stone);
        }
    }

    private void ClickGuildCreate()
    {
        this.PartList[0].SetActive(false);
        this.PartList[1].SetActive(true);
        this.PartList[2].SetActive(false);
    }

    private void ClickGuildList()
    {
        this.PartList[0].SetActive(true);
        this.PartList[1].SetActive(false);
        this.PartList[2].SetActive(false);
    }

    private void ClickGuildSearch()
    {
        this.PartList[0].SetActive(false);
        this.PartList[1].SetActive(false);
        this.PartList[2].SetActive(true);
    }

    private void ClickNextBtn()
    {
        this.mCurrPage++;
        if (this.mCurrPage >= this.mMaxPage)
        {
            this.mCurrPage = 0;
        }
        SocketMgr.Instance.RequestGetGuildList(this.mCurrPage, this.ShowOpenedGuild);
    }

    private void ClickPreBtn()
    {
        this.mCurrPage--;
        if (this.mCurrPage < 0)
        {
            this.mCurrPage = this.mMaxPage;
        }
        SocketMgr.Instance.RequestGetGuildList(this.mCurrPage, this.ShowOpenedGuild);
    }

    private void ClickSearchBtn()
    {
        UIInput component = this.PartList[2].transform.FindChild("Input").GetComponent<UIInput>();
        if (component.value.Length <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652c2));
        }
        else
        {
            try
            {
                long num = Convert.ToInt64(component.value);
                SocketMgr.Instance.RequestGuildSearch(num);
            }
            catch (FormatException)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa6528a));
            }
        }
    }

    private void DisplayGuildList()
    {
        UIGrid component = base.gameObject.transform.FindChild("GuildListPart/Scroll View/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        CommonFunc.ResetClippingPanel(base.transform.FindChild("GuildListPart/Scroll View"));
        foreach (BriefGuildData data in this.mGuildDataList)
        {
            if (this.ShowOpenedGuild)
            {
                if ((data.apply_type == 2) || (ActorData.getInstance().UserInfo.level < data.apply_level))
                {
                    continue;
                }
                guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>(data.level);
                if (data.num >= _config.member_limit)
                {
                    continue;
                }
            }
            GameObject obj2 = UnityEngine.Object.Instantiate(this.GuildObj) as GameObject;
            obj2.transform.parent = component.transform;
            obj2.transform.localScale = Vector3.one;
            obj2.transform.localPosition = Vector3.zero;
            this.UpdateGuildData(obj2, data);
        }
        component.repositionNow = true;
    }

    private void InitCreateGuildCost()
    {
        this.PartList[1].transform.FindChild("Stone/Cost").GetComponent<UILabel>().text = GameConstValues.GUILD_CREATE_STONE.ToString();
        this.PartList[1].transform.FindChild("Gold/Cost").GetComponent<UILabel>().text = GameConstValues.GUILD_CREATE_GOLD.ToString();
    }

    private void OnClickGuildApply(GameObject obj)
    {
        BriefGuildData data = (BriefGuildData) GUIDataHolder.getData(obj);
        if (data != null)
        {
            SocketMgr.Instance.RequestGuildApply(data.id);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnInitialize()
    {
        this.InitCreateGuildCost();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    private void UpdateGuildData(GameObject obj, BriefGuildData _data)
    {
        obj.transform.FindChild("Name").GetComponent<UILabel>().text = _data.name;
        UILabel component = obj.transform.FindChild("Button/Label").GetComponent<UILabel>();
        guild_hall_config _config = ConfigMgr.getInstance().getByEntry<guild_hall_config>(_data.level);
        if (_config != null)
        {
            obj.transform.FindChild("Count").GetComponent<UILabel>().text = _data.num + "/" + _config.member_limit;
        }
        else
        {
            obj.transform.FindChild("Count").GetComponent<UILabel>().text = string.Empty;
        }
        obj.transform.FindChild("Limit").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0xa652a6), _data.apply_level);
        obj.transform.FindChild("Lv").GetComponent<UILabel>().text = "LV. " + ((_data.level + 1)).ToString();
        UILabel label2 = obj.transform.FindChild("ApplyType").GetComponent<UILabel>();
        switch (_data.apply_type)
        {
            case 0:
                component.text = ConfigMgr.getInstance().GetWord(0xa652bd);
                label2.text = ConfigMgr.getInstance().GetWord(0x9d2ab0);
                break;

            case 1:
                component.text = ConfigMgr.getInstance().GetWord(0xa652be);
                label2.text = ConfigMgr.getInstance().GetWord(0x9d2ab1);
                break;

            case 2:
                label2.text = ConfigMgr.getInstance().GetWord(0x9d2ab2);
                component.text = ConfigMgr.getInstance().GetWord(0xa652bd);
                break;
        }
        obj.transform.FindChild("Desc").GetComponent<UILabel>().text = _data.notice;
        UIButton button = obj.transform.FindChild("Button").GetComponent<UIButton>();
        UIEventListener listener1 = UIEventListener.Get(button.gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickGuildApply));
        GUIDataHolder.setData(button.gameObject, _data);
        UITexture texture = obj.transform.FindChild("Icon").GetComponent<UITexture>();
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_data.icon);
        if (_config2 != null)
        {
            texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
        }
    }

    public void UpdateGuildList(S2C_GuildListReq res)
    {
        this.mGuildDataList = res.data;
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = (o1, o2) => o2.level - o1.level;
        }
        this.mGuildDataList.Sort(<>f__am$cache7);
        this.mMaxPage = (int) res.num;
        this.mCurrPage = (int) res.bias;
        this._PageNum.text = (this.mCurrPage + 1).ToString();
        Debug.Log(res.data.Count + "   <-----guild count" + res.num);
        this.DisplayGuildList();
    }

    public void UpdateSearchGuildList(BriefGuildData _data)
    {
        UIGrid component = this.PartList[2].transform.FindChild("Scroll View/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        GameObject obj2 = UnityEngine.Object.Instantiate(this.GuildObj) as GameObject;
        obj2.transform.parent = component.transform;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localPosition = Vector3.zero;
        this.UpdateGuildData(obj2, _data);
        component.repositionNow = true;
    }

    public List<BriefGuildData> GuildDataList
    {
        set
        {
            this.mGuildDataList = value;
        }
    }

    [CompilerGenerated]
    private sealed class <ClickCreateBtn>c__AnonStorey202
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__35B(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__35C(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    private enum PartType
    {
        GuildList,
        GuildCreate,
        GuildSearch
    }
}

