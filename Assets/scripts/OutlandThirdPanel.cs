using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OutlandThirdPanel : GUIEntity
{
    public UITexture _fbIcon;
    public UIGrid _Grid;
    public UILabel _LabelName;
    public UILabel _Level;
    public GameObject _SingleOutlandLayerItem;
    public UISprite bgitmeSprite;
    private int mCurrentLayer = -1;
    private int mSweepStart = -1;
    private int startEntry = -1;

    private bool CheckRuningInOtherLevel()
    {
        int entry = ActorData.getInstance().tempOutlandMapTypeConfig.entry;
        if ((entry >= 0) && (entry <= 3))
        {
            int id = ActorData.getInstance().outlandTitles[entry].underway_entry;
            if (ActorData.getInstance().outlandTitles[entry].is_underway && (id != -1))
            {
                outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(id);
                outland_config _config2 = ConfigMgr.getInstance().getByEntry<outland_config>(this.startEntry);
                if (_config.toll_gate_entry != _config2.toll_gate_entry)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnClickItemBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            <OnClickItemBtn>c__AnonStorey1CA storeyca = new <OnClickItemBtn>c__AnonStorey1CA {
                info = (ClickItemInfo) obj2
            };
            if (this.CheckRuningInOtherLevel())
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyca.<>m__2D3), null);
            }
            else
            {
                switch (storeyca.info.FSType)
                {
                    case FloorStateType.FSType_conquered:
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e34));
                        break;

                    case FloorStateType.FSType_opened:
                    case FloorStateType.FSType_underway:
                        SocketMgr.Instance.RequestOutlandCreateMapReq(storeyca.info.floor.entry);
                        break;

                    case FloorStateType.FSType_locked:
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e35));
                        break;
                }
            }
        }
    }

    private void OnClickSweepBtn(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int num = (int) obj2;
            if (ActorData.getInstance().Stone < num)
            {
                <OnClickSweepBtn>c__AnonStorey1CB storeycb = new <OnClickSweepBtn>c__AnonStorey1CB {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeycb.<>m__2D4), null);
            }
            else if (this.CheckRuningInOtherLevel())
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", obj => ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x4e28), box => SocketMgr.Instance.RequestOutlandSweepReq(this.mSweepStart, true), null, false), null);
            }
            else
            {
                SocketMgr.Instance.RequestOutlandSweepReq(this.mSweepStart, true);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        this.SetInfo(ActorData.getInstance().outlandFloorList);
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    private void returnBtnClick(GameObject go)
    {
        if (ActorData.getInstance().isPreOutlandFight)
        {
            GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
        }
    }

    private void ReturnFirst()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    private void SetGridItemInfo(GameObject go, int entry, int i, FloorStateType floorState, OutlandFloorSimpleInfo floor)
    {
        Transform transform = go.transform.FindChild("Item");
        GameObject gameObject = transform.transform.FindChild("StateRunning").gameObject;
        GameObject obj3 = transform.transform.FindChild("StateOpen").gameObject;
        GameObject obj4 = transform.transform.FindChild("StateTongguan").gameObject;
        int num = ConfigMgr.getInstance().getByEntry<outland_config>(entry).toll_gate_entry + 1;
        UITexture component = transform.transform.FindChild("Background").GetComponent<UITexture>();
        int num2 = (ActorData.getInstance().outlandType != 3) ? ActorData.getInstance().outlandType : 2;
        component.mainTexture = BundleMgr.Instance.CreateOutlandIcon(string.Format("Ui_Out_Bg_t{0}f{1}", num2, i));
        GameObject obj5 = transform.transform.FindChild("Label_left").gameObject;
        GameObject obj6 = transform.transform.FindChild("Label_right").gameObject;
        UISprite sprite = transform.transform.FindChild("Label_left/nth").GetComponent<UISprite>();
        sprite.spriteName = string.Format("Ui_Out_Label_y{0}", i);
        transform.transform.FindChild("Label_right/nth").GetComponent<UISprite>().spriteName = string.Format("Ui_Out_Label_y{0}", i);
        switch (floorState)
        {
            case FloorStateType.FSType_conquered:
                obj5.SetActive(true);
                obj6.SetActive(false);
                gameObject.SetActive(false);
                obj3.SetActive(false);
                obj4.SetActive(true);
                nguiTextureGrey.doChangeEnableGrey(component, true);
                break;

            case FloorStateType.FSType_opened:
                sprite.spriteName = string.Format("Ui_Out_Label_r{0}", i);
                obj5.SetActive(true);
                obj6.SetActive(false);
                gameObject.SetActive(false);
                obj3.SetActive(true);
                obj4.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(component, false);
                break;

            case FloorStateType.FSType_underway:
                sprite.spriteName = string.Format("Ui_Out_Label_r{0}", i);
                obj5.SetActive(true);
                obj6.SetActive(false);
                gameObject.SetActive(true);
                obj3.SetActive(false);
                obj4.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(component, false);
                break;

            case FloorStateType.FSType_locked:
                obj5.SetActive(false);
                obj6.SetActive(true);
                gameObject.SetActive(false);
                obj3.SetActive(false);
                obj4.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(component, true);
                break;
        }
        ClickItemInfo data = new ClickItemInfo {
            floor = floor,
            FSType = floorState
        };
        GUIDataHolder.setData(transform.gameObject, data);
        UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickItemBtn);
    }

    public void SetInfo(List<OutlandFloorSimpleInfo> floorList)
    {
        ActorData.getInstance().outlandFloorList = floorList;
        int num = 0;
        this.startEntry = floorList[0].entry;
        FloorStateType type = FloorStateType.FSType_conquered;
        int num2 = 0;
        this.mSweepStart = -1;
        CommonFunc.DeleteChildItem(this._Grid.transform);
        CommonFunc.ResetClippingPanel(base.gameObject.transform.FindChild("ListSelect/List"));
        foreach (OutlandFloorSimpleInfo info in floorList)
        {
            int outlandType;
            outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(info.entry);
            GameObject go = UnityEngine.Object.Instantiate(this._SingleOutlandLayerItem) as GameObject;
            if (go == null)
            {
                continue;
            }
            go.transform.parent = this._Grid.transform;
            go.transform.localPosition = new Vector3(0f, 0f, -0.1f);
            go.transform.localScale = new Vector3(1f, 1f, 1f);
            num += (!info.is_pass || (info.remain <= 0)) ? 0 : info.cost_num;
            FloorStateType floorState = FloorStateType.FSType_Invalid;
            switch (type)
            {
                case FloorStateType.FSType_conquered:
                    if (info.remain > 0)
                    {
                        goto Label_01BD;
                    }
                    outlandType = ActorData.getInstance().outlandType;
                    if (((outlandType < 0) || (outlandType > 3)) || (info.entry == ActorData.getInstance().outlandTitles[outlandType].underway_entry))
                    {
                        break;
                    }
                    floorState = FloorStateType.FSType_conquered;
                    num2++;
                    goto Label_01D2;

                case FloorStateType.FSType_opened:
                case FloorStateType.FSType_underway:
                case FloorStateType.FSType_locked:
                    floorState = FloorStateType.FSType_locked;
                    goto Label_01D2;

                default:
                    goto Label_01D2;
            }
            if (ActorData.getInstance().outlandTitles[outlandType].is_pass_this)
            {
                floorState = FloorStateType.FSType_conquered;
                num2++;
            }
            else
            {
                floorState = FloorStateType.FSType_underway;
            }
            goto Label_01D2;
        Label_01BD:
            floorState = FloorStateType.FSType_opened;
        Label_01D2:
            this.SetGridItemInfo(go, _config.entry, _config.layer, floorState, info);
            type = floorState;
        }
        this._Grid.repositionNow = true;
        UIPanel component = base.gameObject.transform.FindChild("ListSelect/List").GetComponent<UIPanel>();
        if (component != null)
        {
            float y = 0f;
            if ((this._Grid.cellHeight * floorList.Count) < component.height)
            {
                y = 0f;
            }
            else if ((this._Grid.cellHeight * (floorList.Count - num2)) < component.height)
            {
                y = (this._Grid.cellHeight * floorList.Count) - component.height;
            }
            else
            {
                y = this._Grid.cellHeight * num2;
            }
            this._Grid.transform.localPosition = new Vector3(this._Grid.transform.localPosition.x, y, this._Grid.transform.localPosition.z);
        }
        List<int> list = StrParser.ParseDecIntList(ActorData.getInstance().tempOutlandMapTypeConfig.limit_level, -1);
        outland_config _config2 = ConfigMgr.getInstance().getByEntry<outland_config>(floorList[0].entry);
        this._LabelName.text = ActorData.getInstance().tempOutlandMapTypeConfig.name;
        this._Level.text = "Lv. " + list[_config2.toll_gate_entry];
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey1CA
    {
        internal OutlandThirdPanel.ClickItemInfo info;

        internal void <>m__2D3(GUIEntity obj)
        {
            ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x4e28), box => SocketMgr.Instance.RequestOutlandCreateMapReq(this.info.floor.entry), null, false);
        }

        internal void <>m__2D7(GameObject box)
        {
            SocketMgr.Instance.RequestOutlandCreateMapReq(this.info.floor.entry);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickSweepBtn>c__AnonStorey1CB
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__2D4(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__2D8(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    private class ClickItemInfo
    {
        public OutlandFloorSimpleInfo floor = new OutlandFloorSimpleInfo();
        public OutlandThirdPanel.FloorStateType FSType = OutlandThirdPanel.FloorStateType.FSType_Invalid;
    }

    public enum FloorStateType
    {
        FSType_conquered = 0,
        FSType_Invalid = -1,
        FSType_locked = 3,
        FSType_opened = 1,
        FSType_underway = 2
    }
}

