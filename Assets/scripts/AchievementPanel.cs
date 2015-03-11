using FastBuf;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class AchievementPanel : GUIEntity
{
    private UIGrid _grid;
    [CompilerGenerated]
    private static Predicate<Quest> <>f__am$cache3;
    public bool commit_lock;
    public GameObject itemPrefab;

    private int CompareByQuestID(Quest l, Quest r)
    {
        bool flag = l.is_finish;
        bool flag2 = r.is_finish;
        if (flag == flag2)
        {
            int num = (l.entry != 0x84) ? 0x2710 : l.entry;
            int num2 = (r.entry != 0x84) ? 0x2710 : r.entry;
            return (num - num2);
        }
        if (flag)
        {
            return -1;
        }
        if (flag2)
        {
            return 1;
        }
        return 0;
    }

    private void DeleteAllItems()
    {
        CommonFunc.DeleteChildItem(this.itemGrid.transform);
    }

    private unsafe string GetReward(int type, int entry, int count)
    {
        string name = string.Empty;
        switch (type)
        {
            case 0:
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
                if (_config != null)
                {
                    string str2 = TypeConvertUtil.Color2Hex(*((Color*) &(GameConstant.ConstQuantityColor[_config.quality])));
                    name = _config.name;
                }
                break;
            }
            case 1:
                if (count > 0)
                {
                    name = ConfigMgr.getInstance().GetWord(0x7a);
                    break;
                }
                break;

            case 2:
                if (count > 0)
                {
                    name = ConfigMgr.getInstance().GetWord(0x89);
                    break;
                }
                break;

            case 3:
                if (count > 0)
                {
                    name = ConfigMgr.getInstance().GetWord(0x31b);
                    break;
                }
                break;

            case 4:
            {
                item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(entry);
                if (_config2 != null)
                {
                    string str3 = TypeConvertUtil.Color2Hex(*((Color*) &(GameConstant.ConstQuantityColor[_config2.quality])));
                    name = _config2.name;
                }
                break;
            }
            case 5:
                if (count > 0)
                {
                    name = ConfigMgr.getInstance().GetWord(0x2a3a);
                    break;
                }
                break;

            case 6:
            {
                user_title_config _config3 = ConfigMgr.getInstance().getByEntry<user_title_config>(entry);
                if (_config3 != null)
                {
                    name = ConfigMgr.getInstance().GetWord(0x9897ad) + "\"" + _config3.name + "\"";
                }
                break;
            }
        }
        if (count > 0)
        {
            name = name + "X" + count.ToString();
        }
        return name;
    }

    private string GetRewardList(quest_config cfg)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        str2 = this.GetReward(cfg.gain_item_type1, cfg.gain_item_entry1, cfg.gain_item_count1);
        str = this.GetReward(cfg.gain_item_type2, cfg.gain_item_entry2, cfg.gain_item_count2);
        if (!string.IsNullOrEmpty(str))
        {
            str2 = str2 + "," + str;
        }
        str = this.GetReward(cfg.gain_item_type3, cfg.gain_item_entry3, cfg.gain_item_count3);
        if (!string.IsNullOrEmpty(str))
        {
            str2 = str2 + "," + str;
        }
        return str2;
    }

    private void OnClickItemButton(GameObject go)
    {
        this.ResponseGUIGuide(GuideEvent.EquipLevelUp_MissionPortal, go);
        if (!this.commit_lock)
        {
            int entry = go.GetComponent<AchievementStatus>().entry;
            SocketMgr.Instance.CommitQuest(entry);
            this.commit_lock = true;
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.FloatTitleBar();
        base.OnDeSerialization(pers);
        SocketMgr.Instance.RequestGetQuestList();
        this.commit_lock = false;
    }

    public override void OnInitialize()
    {
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.DockTitleBar();
    }

    public void Refresh()
    {
        this.DeleteAllItems();
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate (Quest quest) {
                IEnumerator enumerator = ConfigMgr.getInstance().getList<user_title_config>().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        user_title_config current = (user_title_config) enumerator.Current;
                        if (current.quest_entry == quest.entry)
                        {
                            return false;
                        }
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
                return true;
            };
        }
        List<Quest> list = ActorData.getInstance().QuestList.FindAll(<>f__am$cache3);
        list.Sort(new Comparison<Quest>(this.CompareByQuestID));
        HashSet<int> set = new HashSet<int>();
        foreach (Quest quest in list)
        {
            item_config _config3;
            if (set.Contains(quest.entry))
            {
                continue;
            }
            quest_config cfg = ConfigMgr.getInstance().getByEntry<quest_config>(quest.entry);
            if (cfg == null)
            {
                continue;
            }
            GameObject obj2 = UnityEngine.Object.Instantiate(this.itemPrefab) as GameObject;
            obj2.transform.parent = this.itemGrid.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localScale = Vector3.one;
            obj2.transform.rotation = Quaternion.identity;
            UILabel component = obj2.transform.FindChild("name").GetComponent<UILabel>();
            UILabel label2 = obj2.transform.FindChild("descript").GetComponent<UILabel>();
            UILabel label3 = obj2.transform.FindChild("award").GetComponent<UILabel>();
            UILabel label4 = obj2.transform.FindChild("progress").GetComponent<UILabel>();
            UILabel label5 = obj2.transform.FindChild("unfinished").GetComponent<UILabel>();
            UIButton button = obj2.transform.FindChild("btn").GetComponent<UIButton>();
            UISprite sprite = obj2.transform.FindChild("bg").GetComponent<UISprite>();
            UITexture texture = obj2.transform.FindChild("line").GetComponent<UITexture>();
            UISprite sprite2 = obj2.transform.FindChild("Award/qulity").GetComponent<UISprite>();
            UITexture texture2 = obj2.transform.FindChild("Award/texture").GetComponent<UITexture>();
            UISprite sprite3 = obj2.transform.FindChild("TopBorder").GetComponent<UISprite>();
            UIEventListener listener1 = UIEventListener.Get(button.gameObject);
            listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickItemButton));
            int num = 0;
            if (quest.countList.Count > 0)
            {
                num = Mathf.Max(quest.countList[0], 0);
            }
            switch (cfg.gain_item_type1)
            {
                case 0:
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(cfg.gain_item_entry1);
                    if (_config2 != null)
                    {
                        texture2.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        CommonFunc.SetQualityBorder(sprite2, _config2.quality);
                        texture2.width = 120;
                        texture2.height = 120;
                    }
                    goto Label_045F;
                }
                case 1:
                    texture2.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_HP");
                    CommonFunc.SetEquipQualityBorder(sprite2, 0, false);
                    goto Label_045F;

                case 2:
                    texture2.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/Texture/Ui_Main_Icon_coin");
                    CommonFunc.SetEquipQualityBorder(sprite2, 0, false);
                    goto Label_045F;

                case 3:
                {
                    texture2.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                    int num3 = 0x7d;
                    texture2.height = num3;
                    texture2.width = num3;
                    CommonFunc.SetEquipQualityBorder(sprite2, 0, false);
                    goto Label_045F;
                }
                case 4:
                    _config3 = ConfigMgr.getInstance().getByEntry<item_config>(cfg.gain_item_entry1);
                    if (_config3 == null)
                    {
                        goto Label_045F;
                    }
                    if (_config3.type != 3)
                    {
                        break;
                    }
                    texture2.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config3.icon);
                    obj2.transform.FindChild("Award/chip").gameObject.SetActive(true);
                    goto Label_03CE;

                case 5:
                    texture2.mainTexture = null;
                    CommonFunc.SetEquipQualityBorder(sprite2, 0, false);
                    goto Label_045F;

                case 6:
                    texture2.mainTexture = BundleMgr.Instance.CreateTextureObject("GUI/TitleIcon/chenhao");
                    CommonFunc.SetEquipQualityBorder(sprite2, 0, false);
                    goto Label_045F;

                default:
                    goto Label_045F;
            }
            texture2.mainTexture = BundleMgr.Instance.CreateItemIcon(_config3.icon);
        Label_03CE:
            CommonFunc.SetEquipQualityBorder(sprite2, _config3.quality, false);
            if (_config3.type == 3)
            {
                texture2.width = 120;
                texture2.height = 120;
            }
            else if (_config3.type == 2)
            {
                texture2.width = 0x52;
                texture2.height = 0x52;
            }
        Label_045F:
            component.text = cfg.name;
            label2.text = cfg.finish_desc;
            label3.text = this.GetRewardList(cfg);
            button.gameObject.SetActive(quest.is_finish);
            label5.gameObject.SetActive(false);
            sprite.spriteName = !quest.is_finish ? "Ui_Heroinfo_Bg_05" : "Ui_Chengjiu_Bg_00";
            sprite3.color = !quest.is_finish ? ((Color) new Color32(0xdb, 0xd1, 0xbd, 0xff)) : ((Color) new Color32(0xed, 230, 0xd5, 0xff));
            texture.color = !quest.is_finish ? ((Color) new Color32(0x65, 0x58, 0x4e, 0xff)) : ((Color) new Color32(70, 0x33, 0x25, 0xff));
            if (quest.is_finish)
            {
                label4.text = ConfigMgr.getInstance().GetWord(840);
            }
            else
            {
                string[] textArray1 = new string[] { "(", num.ToString(), "/", Mathf.Max(0, cfg.finish_count1).ToString(), ")" };
                label4.text = string.Concat(textArray1);
            }
            button.gameObject.AddComponent<AchievementStatus>().entry = cfg.entry;
            set.Add(quest.entry);
        }
        this.itemGrid.Reposition();
        this.RequestNewbieGeneration(GuideEvent.EquipLevelUp_MissionPortal, 0x84);
    }

    private void RequestNewbieGeneration(GuideEvent _event, int mission_entry)
    {
        if (GuideSystem.MatchEvent(_event))
        {
            int childCount = this.itemGrid.transform.childCount;
            for (int i = 0; i != childCount; i++)
            {
                Transform child = this.itemGrid.transform.GetChild(i);
                if (null != child)
                {
                    Transform transform2 = child.FindChild("btn");
                    if (null != transform2)
                    {
                        AchievementStatus component = transform2.GetComponent<AchievementStatus>();
                        if ((null != component) && (mission_entry == component.entry))
                        {
                            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Mission.tag_mission_award_button, transform2.gameObject);
                            return;
                        }
                    }
                }
            }
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    private void ResponseGUIGuide(GuideEvent _event, GameObject go)
    {
        if (GuideSystem.MatchEvent(_event))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Mission.tag_mission_award_button, go);
        }
    }

    public void UnlockCommitPortal()
    {
        this.DelayCallBack(0.3f, () => this.commit_lock = false);
    }

    private UIGrid itemGrid
    {
        get
        {
            if (null == this._grid)
            {
                this._grid = base.transform.FindChild("List/Grid").GetComponent<UIGrid>();
            }
            return this._grid;
        }
    }
}

