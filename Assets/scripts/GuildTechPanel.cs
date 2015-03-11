using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GuildTechPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4;
    private List<GameObject> CreateObjBuf = new List<GameObject>();
    public UIGrid gridroot;
    private bool LevUpState;
    private const int LineCount = 2;
    public GameObject TechObj;

    private void ClickBufferLevUp()
    {
        this.LevUpState = true;
        base.gameObject.transform.FindChild("BuildLvUp/Button1").gameObject.SetActive(false);
        base.gameObject.transform.FindChild("BuildLvUp/Button2").gameObject.SetActive(false);
        base.gameObject.transform.FindChild("BuildLvUp/Button3").gameObject.SetActive(true);
        this.UpdateLevUpState();
    }

    private void ClickBuildLevUp()
    {
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = delegate (GUIEntity guiE) {
                GuildLevUpPanel panel = guiE.Achieve<GuildLevUpPanel>();
                panel.Depth = 400;
                panel.UpdateData(GuildFuncType.Tech);
            };
        }
        GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", <>f__am$cache4, base.gameObject);
    }

    private void ClickSelBuffer()
    {
        this.LevUpState = false;
        base.gameObject.transform.FindChild("BuildLvUp/Button1").gameObject.SetActive(true);
        base.gameObject.transform.FindChild("BuildLvUp/Button2").gameObject.SetActive(true);
        base.gameObject.transform.FindChild("BuildLvUp/Button3").gameObject.SetActive(false);
        this.UpdateLevUpState();
    }

    private void ClosePanel()
    {
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    private void OnClickTechItem(GameObject obj)
    {
        <OnClickTechItem>c__AnonStorey206 storey = new <OnClickTechItem>c__AnonStorey206 {
            data = (Tech) GUIDataHolder.getData(obj)
        };
        if (this.LevUpState)
        {
            if (storey.data.level >= 9)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa6529a));
            }
            else if (storey.data.level >= ActorData.getInstance().mGuildData.tech.warmill_level)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652a1));
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("GuildLevUpPanel", new Action<GUIEntity>(storey.<>m__385), null);
            }
        }
        else if (ActorData.getInstance().mUserGuildMemberData.tech != storey.data.entry)
        {
            GUIMgr.Instance.DoModelGUI("GuildSelTechPanel", new Action<GUIEntity>(storey.<>m__386), null);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
    }

    public override void OnInitialize()
    {
        this.UpdateBuildLevUpLimit();
        this.UpdateBuildLev();
        this.UpdateContribution();
        this.UpdateTechItem();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.DockTitleBar();
    }

    public void UpdateBuildLev()
    {
        base.gameObject.transform.FindChild("Lev/val").GetComponent<UILabel>().text = (ActorData.getInstance().mGuildData.tech.warmill_level + 1).ToString();
    }

    private void UpdateBuildLevUpLimit()
    {
        GameObject gameObject = base.gameObject.transform.FindChild("BuildLvUp/Button1").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("BuildLvUp/Button2").gameObject;
        if (ActorData.getInstance().mUserGuildMemberData.position == 1)
        {
            gameObject.SetActive(true);
            obj3.SetActive(true);
        }
        else
        {
            obj3.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void UpdateContribution()
    {
        base.gameObject.transform.FindChild("BuildLvUp/Val").GetComponent<UILabel>().text = ActorData.getInstance().mGuildData.cur_contribution.ToString();
        this.UpdateOwnContribution();
    }

    private void UpdateItemData(GameObject _obj, Tech _data)
    {
        UILabel component = _obj.transform.FindChild("name").GetComponent<UILabel>();
        UILabel label2 = _obj.transform.FindChild("Lv").GetComponent<UILabel>();
        UILabel label3 = _obj.transform.FindChild("Desc").GetComponent<UILabel>();
        UITexture texture = _obj.transform.FindChild("Icon").GetComponent<UITexture>();
        guild_buff_config _config = ConfigMgr.getInstance().getByEntry<guild_buff_config>(_data.entry);
        if (_config == null)
        {
            Debug.LogWarning("Tech entry is error! " + _data.entry);
        }
        else
        {
            texture.mainTexture = BundleMgr.Instance.CreateSkillIcon(_config.icon);
            component.text = _config.name;
            label2.text = "LV. " + ((_data.level + 1)).ToString();
            float num = _config.func_base + (_config.func_param * _data.level);
            object[] objArray1 = new object[] { _config.desc, "[840100]+", (int) num, "[-]" };
            label3.text = string.Concat(objArray1);
        }
    }

    private void UpdateLevUpState()
    {
        GuildData mGuildData = ActorData.getInstance().mGuildData;
        int num = 0;
        foreach (GameObject obj2 in this.CreateObjBuf)
        {
            if (((this.LevUpState && (mGuildData != null)) && ((mGuildData.tech != null) && (mGuildData.tech.tech.Count > num))) && (mGuildData.tech.tech[num].level < 9))
            {
                obj2.transform.FindChild("UpTips").gameObject.SetActive(true);
            }
            else
            {
                obj2.transform.FindChild("UpTips").gameObject.SetActive(false);
            }
            num++;
        }
    }

    public void UpdateOwnContribution()
    {
        base.gameObject.transform.FindChild("Con/val").GetComponent<UILabel>().text = ActorData.getInstance().mUserGuildMemberData.contribution.ToString();
    }

    public void UpdateTechItem()
    {
        GuildData mGuildData = ActorData.getInstance().mGuildData;
        int num = ActorData.getInstance().mUserGuildMemberData.tech;
        int num2 = 0;
        int num3 = 0;
        GameObject obj2 = null;
        CommonFunc.DeleteChildItem(this.gridroot.transform);
        this.CreateObjBuf.Clear();
        foreach (Tech tech in mGuildData.tech.tech)
        {
            if ((num2 == 0) || ((num2 % 2) == 0))
            {
                obj2 = new GameObject();
                obj2.name = (num3 + 1).ToString();
                obj2.transform.parent = this.gridroot.transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.localScale = Vector3.one;
                num2 = 0;
                num3++;
            }
            GameObject obj3 = UnityEngine.Object.Instantiate(this.TechObj) as GameObject;
            GameObject gameObject = obj3.transform.FindChild("Check").gameObject;
            if (num == tech.entry)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            obj3.transform.parent = obj2.transform;
            obj3.transform.localPosition = new Vector3((float) (-238 + (num2 * 0x1ca)), 0f, 0f);
            obj3.transform.localScale = Vector3.one;
            this.UpdateItemData(obj3, tech);
            this.CreateObjBuf.Add(obj3);
            GUIDataHolder.setData(obj3, tech);
            UIEventListener listener1 = UIEventListener.Get(obj3);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickTechItem));
            num2++;
        }
        this.gridroot.repositionNow = true;
        this.UpdateLevUpState();
    }

    [CompilerGenerated]
    private sealed class <OnClickTechItem>c__AnonStorey206
    {
        internal Tech data;

        internal void <>m__385(GUIEntity guiE)
        {
            GuildLevUpPanel panel = guiE.Achieve<GuildLevUpPanel>();
            panel.Depth = 400;
            panel.UpdateBufferLevUp(this.data);
        }

        internal void <>m__386(GUIEntity guiE)
        {
            guiE.Achieve<GuildSelTechPanel>().UpdateData(this.data);
        }
    }
}

