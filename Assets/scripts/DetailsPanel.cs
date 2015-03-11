using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DetailsPanel : GUIEntity
{
    public UIGrid _Grid;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache4;
    private EnterDupType m_EnterDupType;
    private int mItemEntry = -1;
    public GameObject SingleDetailsItem;

    public void CloseDupPanelEvent(GameObject go)
    {
        GUIMgr.Instance.FloatTitleBar();
    }

    private void ClosePanel()
    {
        ActorData.getInstance().IsPopPanel = true;
        GUIMgr.Instance.PopGUIEntity();
    }

    public void InitItemDetails(Item _item, EnterDupType _type)
    {
        this.m_EnterDupType = _type;
        if (_item != null)
        {
            item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(_item.entry);
            if (ic != null)
            {
                this.SetItemDetails(ic);
            }
        }
    }

    public void InitList(Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config.entry, _config.evolve_lv);
                if (cardExCfg != null)
                {
                    item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(cardExCfg.item_entry);
                    if (ic != null)
                    {
                        Debug.Log(ic.name + ":" + ic.entry);
                        this.SetItemDetails(ic);
                    }
                }
            }
        }
    }

    public void InitList(int itemEntry)
    {
        item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(itemEntry);
        if (ic != null)
        {
            this.SetItemDetails(ic);
        }
    }

    private void JumpToDungeons(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().dungeons)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().void_tower));
        }
        else if (!CommonFunc.CheckIsFrozenFun(ELimitFuncType.E_LimitFunc_Dungeons))
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (GUIEntity obj) {
                };
            }
            GUIMgr.Instance.PushGUIEntity("DungeonsPanel", <>f__am$cache4);
        }
    }

    private void JumpToDuplicate(GameObject go)
    {
        <JumpToDuplicate>c__AnonStorey185 storey = new <JumpToDuplicate>c__AnonStorey185 {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.info = obj2 as MapData;
            if (storey.info != null)
            {
                Debug.Log(storey.info.entry + "  " + storey.info.subEntry);
                SocketMgr.Instance.RequestGetDuplicateRemain(storey.info.entry, DuplicateType.DupType_Normal);
                SocketMgr.Instance.RequestGetDuplicateRemain(storey.info.entry, DuplicateType.DupType_Elite);
                GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storey.<>m__1D9));
            }
        }
    }

    private void JumpToTower(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().void_tower)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().void_tower));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("TowerPanel", null);
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnDeSerialization(pers);
        GUIMgr.Instance.FloatTitleBar();
    }

    private void SetDropInfo(GameObject obj, DropFromType _type, string _strType, int _pram1, int _pram2, item_config ic)
    {
        UISprite component = obj.transform.FindChild("Background").GetComponent<UISprite>();
        UISprite sprite2 = obj.transform.FindChild("Type").GetComponent<UISprite>();
        UILabel label = obj.transform.FindChild("IsOpen").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("Head/Icon").GetComponent<UITexture>();
        bool flag = false;
        UILabel label2 = obj.transform.FindChild("Label").GetComponent<UILabel>();
        switch (_type)
        {
            case DropFromType.E_NORMAL_DUPLICATE:
            {
                duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config != null)
                {
                    trench_normal_config _config2 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(_pram2);
                    if (_config2 != null)
                    {
                        label2.text = _config.name + "-" + _config2.name;
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.monster_picture);
                        flag = (ActorData.getInstance().NormalProgress >= _pram2) && (ActorData.getInstance().Level >= _config.unlock_lv);
                        if (flag)
                        {
                            MapData data = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Normal
                            };
                            GUIDataHolder.setData(obj, data);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                        sprite2.spriteName = !flag ? "Ui_Heroinfo_Label_ptgrey" : "Ui_Heroinfo_Label_pt";
                        sprite2.gameObject.SetActive(true);
                    }
                }
                break;
            }
            case DropFromType.E_ELITE_DUPLICATE:
            {
                duplicate_config _config3 = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config3 != null)
                {
                    trench_elite_config _config4 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(_pram2);
                    if (_config4 != null)
                    {
                        label2.text = _config3.name + "-" + _config4.name;
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config4.monster_picture);
                        flag = (ActorData.getInstance().EliteProgress >= _pram2) && (ActorData.getInstance().Level >= _config3.unlock_lv);
                        if (flag)
                        {
                            MapData data2 = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Elite
                            };
                            GUIDataHolder.setData(obj, data2);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                        sprite2.spriteName = !flag ? "Ui_Heroinfo_Label_jygrey" : "Ui_Heroinfo_Label_jy";
                        sprite2.gameObject.SetActive(true);
                    }
                }
                break;
            }
            case DropFromType.E_WORLDBOSS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_boss;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(0x1b0);
                break;

            case DropFromType.E_WARMMATCH:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_cup;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(0x1b2);
                break;

            case DropFromType.E_VOIDTOWER:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().void_tower;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b3);
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToTower);
                break;

            case DropFromType.E_DUNGEONS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().dungeons;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b7);
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDungeons);
                break;

            case DropFromType.E_OPENBOX:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().item_box;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(0x1b4);
                break;

            case DropFromType.E_SHOP:
                flag = true;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(0x1b5);
                break;

            case DropFromType.E_GUILDSHOP:
                flag = true;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(0x1b6);
                break;

            case DropFromType.E_LOTTERY:
                flag = true;
                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(ic.icon);
                label2.text = ConfigMgr.getInstance().GetWord(440);
                break;

            default:
                label2.text = !string.IsNullOrEmpty(_strType) ? string.Empty : ConfigMgr.getInstance().GetWord(StrParser.ParseDecInt(_strType));
                break;
        }
        if (flag)
        {
            label.text = ConfigMgr.getInstance().GetWord(220);
            label.color = (Color) new Color32(0, 150, 0x1a, 0xff);
        }
        else
        {
            label.text = ConfigMgr.getInstance().GetWord(0xdd);
        }
        nguiTextureGrey.doChangeEnableGrey(texture, !flag);
    }

    private void SetItemDetails(item_config ic)
    {
        CommonFunc.DeleteChildItem(this._Grid.transform);
        if (ic != null)
        {
            this.mItemEntry = ic.entry;
            char[] separator = new char[] { '|' };
            string[] strArray = ic.drop_from_type.Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = ic.drop_parm_0.Split(chArray2);
            char[] chArray3 = new char[] { '|' };
            string[] strArray3 = ic.drop_parm_1.Split(chArray3);
            Debug.Log(ic.drop_from_type);
            Debug.Log(ic.drop_parm_0.Length);
            if (strArray.Length != strArray2.Length)
            {
                base.transform.FindChild("NullTips").GetComponent<UILabel>().text = (ic.entry != 0x4ef) ? ConfigMgr.getInstance().GetWord(0x8ac) : ConfigMgr.getInstance().GetWord(0x8ab);
                base.transform.FindChild("NullTips").gameObject.SetActive(true);
            }
            else
            {
                int index = 0;
                float cellHeight = this._Grid.cellHeight;
                foreach (string str in strArray)
                {
                    if ((str != string.Empty) && (int.Parse(str) <= 11))
                    {
                        GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleDetailsItem) as GameObject;
                        obj2.transform.parent = this._Grid.transform;
                        obj2.transform.localPosition = new Vector3(0f, -cellHeight * index, -0.1f);
                        obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                        DropFromType type = (DropFromType) int.Parse(str);
                        int num4 = !(strArray2[index] == string.Empty) ? int.Parse(strArray2[index]) : 0;
                        int num5 = !(strArray3[index] == string.Empty) ? int.Parse(strArray3[index]) : 0;
                        this.SetDropInfo(obj2, type, str, num4, num5, ic);
                        index++;
                    }
                }
                if (index <= 0)
                {
                    UILabel component = base.transform.FindChild("NullTips").GetComponent<UILabel>();
                    component.gameObject.SetActive(index == 0);
                    if (strArray[0] != string.Empty)
                    {
                        component.text = ConfigMgr.getInstance().GetWord(int.Parse(strArray[0]));
                    }
                    else
                    {
                        base.transform.FindChild("NullTips").GetComponent<UILabel>().text = (ic.entry != 0x4ef) ? ConfigMgr.getInstance().GetWord(0x8ac) : ConfigMgr.getInstance().GetWord(0x8ab);
                    }
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <JumpToDuplicate>c__AnonStorey185
    {
        internal DetailsPanel <>f__this;
        internal MapData info;

        internal void <>m__1D9(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.OpenTypeIsPush = true;
            ActorData.getInstance().CurDupEntry = this.info.entry;
            ActorData.getInstance().CurTrenchEntry = this.info.subEntry;
            ActorData.getInstance().CurDupType = this.info.type;
            panel.UpdateData(this.info.entry, this.info.subEntry, this.info.type);
            DupReturnPrePanelPara para = new DupReturnPrePanelPara {
                enterDuptype = this.<>f__this.m_EnterDupType,
                heroPanelPartEntry = this.<>f__this.mItemEntry
            };
            ActorData.getInstance().mCurrDupReturnPrePara = para;
        }
    }
}

