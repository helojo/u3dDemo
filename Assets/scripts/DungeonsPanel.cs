using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class DungeonsPanel : GUIEntity
{
    private const string ContinueIcon = "Ui_Dicheng_Label_jx";
    private int CurCount;
    private List<DungeonsActivityData> Data = new List<DungeonsActivityData>();
    public GameObject DifficultPart;
    private string[] DName = new string[] { "Ui_Dicheng_Label_chsg", "Ui_Dicheng_Label_yqgd" };
    private float m_interval = 1f;
    private dungeons_activity_config mDungeonsActivityCfg;
    private int[] NextOpenTime = new int[2];
    private UILabel NextTimeLabel1;
    private UILabel NextTimeLabel2;
    public List<GameObject> RootList = new List<GameObject>();
    private const string StartIcon = "Ui_Dicheng_Label_ts";
    private float Times;

    private void ClosePanel()
    {
        CommonFunc.ShowFuncList(true);
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    private string ConvertTime(int _Times)
    {
        TimeSpan span = new TimeSpan(0, 0, _Times);
        return string.Format(ConfigMgr.getInstance().GetWord(0x6b), span.Hours, span.Minutes, span.Seconds);
    }

    private DungeonsOneData GetActivetyData(dungeons_activity_config _CfgData)
    {
        foreach (DungeonsActivityData data in this.Data)
        {
            if (data.activity_data.entry == _CfgData.entry)
            {
                return data.activity_data;
            }
        }
        return null;
    }

    private bool GetIsOpen(int _entry, S2C_DungeonsDataReq _data)
    {
        if (_entry == 0)
        {
            return _data.dungeons_1_is_open;
        }
        return ((_entry == 1) && _data.dungeons_2_is_open);
    }

    private int GetTimes(int _entry)
    {
        if (_entry == 0)
        {
            return ActorData.getInstance().UserInfo.dungeons_1_times;
        }
        if (_entry == 1)
        {
            return ActorData.getInstance().UserInfo.dungeons_2_times;
        }
        return 0;
    }

    private void OnClickCloseTime(GameObject obj)
    {
        TipsDiag.SetText("开启时间还没到!");
    }

    private void OnClickDifficult(GameObject obj)
    {
        <OnClickDifficult>c__AnonStorey1BA storeyba = new <OnClickDifficult>c__AnonStorey1BA {
            <>f__this = this,
            Diff = (int) GUIDataHolder.getData(obj)
        };
        int num = CommonFunc.GetConfigEntry(this.mDungeonsActivityCfg.unlock_lv)[storeyba.Diff];
        if (ActorData.getInstance().Level < num)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9ba3d9));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storeyba.<>m__290));
        }
    }

    private void OnClickDungeons(GameObject obj)
    {
        DungeonsDataEx ex = (DungeonsDataEx) GUIDataHolder.getData(obj);
        if (DupLevInfoPanel.PhyIsEnough(CommonFunc.GetConfigEntry(ex.CfgData.cost_phy_force)[0]))
        {
            this.PopDifficultSel(obj.transform.parent, ex.CfgData, ex.ServerData);
            this.mDungeonsActivityCfg = ex.CfgData;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        foreach (GameObject obj2 in this.RootList)
        {
            obj2.SetActive(true);
        }
        this.DifficultPart.SetActive(false);
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
        CommonFunc.ShowFuncList(false);
    }

    public override void OnInitialize()
    {
        SocketMgr.Instance.RequestDungeonsData();
        CommonFunc.ShowFuncList(false);
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void PopDifficultSel(Transform _trans, dungeons_activity_config _cfg, DungeonsOneData _serverData)
    {
        foreach (GameObject obj2 in this.RootList)
        {
            if (obj2.transform == _trans)
            {
                obj2.SetActive(false);
            }
            else
            {
                obj2.SetActive(true);
            }
        }
        this.DifficultPart.transform.parent = _trans.parent;
        this.DifficultPart.transform.localScale = Vector3.one;
        this.DifficultPart.transform.localPosition = Vector3.zero;
        this.DifficultPart.SetActive(true);
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            List<int> configEntry = CommonFunc.GetConfigEntry(_cfg.cost_phy_force);
            int num = _config.dungeons_times - this.GetTimes(_cfg.entry);
            this.CurCount = num;
            if (num < 0)
            {
                Debug.Log("DungeonsCount is " + num);
                num = 0;
            }
            this.DifficultPart.transform.FindChild("Count/Val").GetComponent<UILabel>().text = num.ToString();
            this.DifficultPart.transform.FindChild("Cost/Val").GetComponent<UILabel>().text = configEntry[0].ToString();
            this.DifficultPart.transform.FindChild("Cost/Name").GetComponent<UISprite>().spriteName = this.DName[_cfg.entry];
            Transform transform = this.DifficultPart.transform.FindChild("Count");
            Transform transform2 = this.DifficultPart.transform.FindChild("Cost");
            if (_cfg.entry == 1)
            {
                transform.localPosition = new Vector3(111f, 241f, 0f);
                transform2.localPosition = new Vector3(180f, 241f, 0f);
            }
            else if (_cfg.entry == 0)
            {
                transform.localPosition = new Vector3(2.6f, 241f, 0f);
                transform2.localPosition = new Vector3(68f, 241f, 0f);
            }
            this.UpdateDifficultData(_cfg);
        }
    }

    private void UpdataDungeonsData(GameObject _obj, dungeons_activity_config _cfg, bool isOpen)
    {
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            List<int> configEntry = CommonFunc.GetConfigEntry(_cfg.cost_phy_force);
            _obj.transform.FindChild("Root/Count/Val").GetComponent<UILabel>().text = (_config.dungeons_times - this.GetTimes(_cfg.entry)).ToString();
            _obj.transform.FindChild("Root/Cost/Val").GetComponent<UILabel>().text = configEntry[0].ToString();
            GameObject gameObject = _obj.transform.FindChild("Root/Enter").gameObject;
            GameObject obj3 = _obj.transform.FindChild("Root/OpenTimes").gameObject;
            UITexture component = _obj.transform.FindChild("Root/Texture").gameObject.GetComponent<UITexture>();
            if (!isOpen)
            {
                Debug.Log("Lock");
                UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickCloseTime);
                nguiTextureGrey.doChangeEnableGrey(component, true);
            }
            else
            {
                DungeonsDataEx data = new DungeonsDataEx {
                    CfgData = _cfg
                };
                GUIDataHolder.setData(gameObject, data);
                UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickDungeons);
                nguiTextureGrey.doChangeEnableGrey(component, false);
            }
        }
    }

    public void UpdateData(S2C_DungeonsDataReq _data)
    {
        ArrayList list = ConfigMgr.getInstance().getList<dungeons_activity_config>();
        int num = 1;
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                dungeons_activity_config current = (dungeons_activity_config) enumerator.Current;
                bool isOpen = this.GetIsOpen(current.entry, _data);
                string name = "DungeonsPart/" + num;
                GameObject gameObject = base.gameObject.transform.FindChild(name).gameObject;
                gameObject.transform.FindChild("Root/Label").GetComponent<UISprite>().spriteName = this.DName[current.entry];
                gameObject.transform.FindChild("Root/Desc/Label").GetComponent<UILabel>().text = current.description + current.description2;
                this.UpdataDungeonsData(gameObject, current, isOpen);
                num++;
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    private void UpdateDifficultData(dungeons_activity_config _cfg)
    {
        List<int> configEntry = CommonFunc.GetConfigEntry(_cfg.unlock_lv);
        int num = 1;
        foreach (int num2 in configEntry)
        {
            string name = "Button" + num;
            GameObject gameObject = this.DifficultPart.transform.FindChild(name).gameObject;
            int data = num - 1;
            UILabel component = gameObject.transform.FindChild("Label").GetComponent<UILabel>();
            GUIDataHolder.setData(gameObject, data);
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickDifficult);
            if (ActorData.getInstance().UserInfo.level < num2)
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x9ba3c4), num2);
            }
            else
            {
                switch (num)
                {
                    case 1:
                        component.text = ConfigMgr.getInstance().GetWord(0x2868);
                        break;

                    case 2:
                        component.text = ConfigMgr.getInstance().GetWord(0x2867);
                        break;

                    case 3:
                        component.text = ConfigMgr.getInstance().GetWord(0x2869);
                        break;
                }
            }
            num++;
        }
    }

    private void UpdateDungeonsServerData(GameObject _obj, dungeons_activity_config _cfg, dungeons_config DCfg, DungeonsOneData _DaData, DungeonsData DGData)
    {
        _obj.transform.FindChild("Root/Count/Val").GetComponent<UILabel>().text = _DaData.remain.ToString();
        _obj.transform.FindChild("Root/Cost/Val").GetComponent<UILabel>().text = DCfg.phyforce_cost.ToString();
        GameObject gameObject = _obj.transform.FindChild("Root/Enter").gameObject;
        GUIDataHolder.setData(gameObject, _cfg);
        UIEventListener listener1 = UIEventListener.Get(gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickDungeons));
    }

    [CompilerGenerated]
    private sealed class <OnClickDifficult>c__AnonStorey1BA
    {
        internal DungeonsPanel <>f__this;
        internal int Diff;

        internal void <>m__290(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.OpenTypeIsPush = true;
            panel.UpdateDungeonsData(this.<>f__this.mDungeonsActivityCfg, this.Diff, this.<>f__this.CurCount);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DungeonsDataEx
    {
        public dungeons_activity_config CfgData;
        public DungeonsOneData ServerData;
    }
}

