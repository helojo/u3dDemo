using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TowerPanel : GUIEntity
{
    public GameObject _EnterPanel;
    public UILabel _MashTimeLabel;
    public UILabel _MashTips;
    public UILabel _MaxLayerLabel;
    public UILabel _NextLayerTips;
    public UILabel _RemainCount;
    public GameObject _RewardPanel;
    public UILabel _RewardRemainCount;
    public GameObject _RewardShaoDangBtn;
    public GameObject _ShaoDangBtn;
    public GameObject _TargetItem;
    public UILabel _Team2Label;
    public UILabel _Team3Label;
    public UILabel _TeamlLabel;
    public GameObject _TiaoZhanBtn;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache16;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache17;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache18;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache19;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private VoidTowerData mData;
    private int mEndTime;
    private bool mIsStart;
    public GameObject MosterTipsDlg;
    public UIButton RankButton;

    private void ClosePanel()
    {
        CommonFunc.ShowFuncList(true);
    }

    private void OnClickContinueFight()
    {
        this.OnClickStartFightBtn();
    }

    private void OnClickExitFight()
    {
        if (<>f__am$cache16 == null)
        {
            <>f__am$cache16 = delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                if (<>f__am$cache18 == null)
                {
                    <>f__am$cache18 = box => SocketMgr.Instance.RequestVoidTowerReward();
                }
                box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x4ff), new object[0]), <>f__am$cache18, null, false);
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache16, null);
    }

    private void OnClickItemBtn(GameObject go, bool isPress)
    {
        if (isPress)
        {
            <OnClickItemBtn>c__AnonStorey26F storeyf = new <OnClickItemBtn>c__AnonStorey26F();
            if (GUIMgr.Instance.GetGUIEntity<ItemInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
            }
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                storeyf.item = new Item();
                storeyf.item.entry = (int) obj2;
                GUIMgr.Instance.DoModelGUI("ItemInfoPanel", new Action<GUIEntity>(storeyf.<>m__5A0), base.gameObject);
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("ItemInfoPanel");
        }
    }

    private void OnClickRankButton()
    {
        AllRankInOnePanel.Open(EN_RankListType.EN_RANKLIST_TOWER, false);
    }

    private void OnClickShaoDangBtn()
    {
        if (this.mData != null)
        {
            if (this.mIsStart)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x233));
            }
            else if (this.mData.record == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x235));
            }
            else if (this.mData.trench_entry >= this.mData.record)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x240));
            }
            else
            {
                SocketMgr.Instance.RequestVoidTowerSmashBegin();
            }
        }
    }

    private void OnClickStartFightBtn()
    {
        if (this.mData != null)
        {
            if (this.mIsStart)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x233));
            }
            else if (this.mData.attack_times == 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x22e), 0));
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity obj) {
                    SelectHeroPanel panel = (SelectHeroPanel) obj;
                    panel.Depth = 600;
                    panel.mBattleType = BattleType.TowerPk;
                    panel.SetTowerInfo(this.mData);
                }, null);
            }
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
        GUIMgr.Instance.FloatTitleBar();
        if (this.mData != null)
        {
            this._RemainCount.text = (this.mData.attack_times >= 0) ? this.mData.attack_times.ToString() : "0";
        }
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        SocketMgr.Instance.RequestVoidTower(1);
        if (null != this.RankButton)
        {
            EventDelegate.Add(this.RankButton.onClick, new EventDelegate.Callback(this.OnClickRankButton));
            Transform transform = this.RankButton.transform.Find("Label");
            if (null != transform)
            {
                transform.GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0xbc5);
            }
        }
    }

    private void OnPressMonsterItem(GameObject go, bool isPress)
    {
        if (isPress)
        {
            this.MosterTipsDlg.transform.parent = go.transform;
            this.MosterTipsDlg.transform.localPosition = Vector3.zero;
            this.MosterTipsDlg.transform.localScale = Vector3.one;
            object obj2 = GUIDataHolder.getData(go);
            if (obj2 != null)
            {
                monster_config _config = (monster_config) obj2;
                if (_config != null)
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry);
                    if (_config2 != null)
                    {
                        UISprite component = this.MosterTipsDlg.transform.FindChild("Group/IconHead/QualityBorder").GetComponent<UISprite>();
                        UITexture texture = this.MosterTipsDlg.transform.FindChild("Group/IconHead/Icon").GetComponent<UITexture>();
                        CommonFunc.SetQualityBorder(component, _config.quality);
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        this.MosterTipsDlg.transform.FindChild("Group/Name").GetComponent<UILabel>().text = _config2.name;
                        this.MosterTipsDlg.transform.FindChild("Group/Level").GetComponent<UILabel>().text = "LV." + _config.level;
                        this.MosterTipsDlg.transform.FindChild("Group/Boss").gameObject.SetActive(_config.type > 1);
                        UILabel label3 = this.MosterTipsDlg.transform.FindChild("Group/Describe/Desc").GetComponent<UILabel>();
                        label3.text = _config.description;
                        this.MosterTipsDlg.transform.FindChild("Group/Border").GetComponent<UISprite>().height = 0x73 + label3.height;
                        this.MosterTipsDlg.SetActive(true);
                    }
                }
            }
        }
        else
        {
            this.MosterTipsDlg.SetActive(false);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this._MashTips.text = string.Empty;
        this._MashTimeLabel.text = string.Empty;
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(false);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                if (TimeMgr.Instance.ServerStampTime < this.mEndTime)
                {
                    this._MashTimeLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x23c), TimeMgr.Instance.GetRemainTime2(this.mEndTime));
                    int num = (this.mEndTime - TimeMgr.Instance.ServerStampTime) % 10;
                    int num2 = this.mData.record * 3;
                    if (num2 > 0x63)
                    {
                        num2 = 0x63;
                    }
                    int num3 = num2 - ((this.mEndTime - TimeMgr.Instance.ServerStampTime) / 10);
                    if ((num3 <= 0) || (num3 < ((this.mData.trench_entry * 3) + 1)))
                    {
                        this._MashTips.text = ConfigMgr.getInstance().GetWord(0x23e);
                    }
                    else
                    {
                        this._MashTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x23d), num3, num);
                    }
                }
                else
                {
                    this._MashTips.text = string.Empty;
                    this._MashTimeLabel.text = string.Empty;
                    this.mIsStart = false;
                    SocketMgr.Instance.RequestVoidTowerSmash();
                }
            }
        }
    }

    public void ResetRemainCount(int reamincount)
    {
        this.mData.attack_times = reamincount;
        this._RemainCount.text = (reamincount >= 0) ? reamincount.ToString() : "0";
    }

    private void SetEnemyInfo(void_tower_trench_config vttc)
    {
        if (vttc != null)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = vttc.battlefield_entry.Split(separator);
            for (int i = 0; i < 3; i++)
            {
                Transform component = this._RewardPanel.transform.FindChild("List/Team" + (i + 1)).GetComponent<Transform>();
                if (i < strArray.Length)
                {
                    int id = int.Parse(strArray[i]);
                    battle_config bc = ConfigMgr.getInstance().getByEntry<battle_config>(id);
                    if (bc != null)
                    {
                        this.SetLayerEnemyInfo(component, bc);
                    }
                    component.gameObject.SetActive(true);
                }
                else
                {
                    component.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetEnterPanelInfo(VoidTowerData data)
    {
        void_tower_trench_config vttc = ConfigMgr.getInstance().getByEntry<void_tower_trench_config>(0);
        if (vttc != null)
        {
            this.SetEnemyInfo(vttc);
        }
    }

    private void SetLayerEnemyInfo(Transform obj, battle_config bc)
    {
        if (bc != null)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int item = -1;
                switch (i)
                {
                    case 0:
                        item = int.Parse(bc.monster_0);
                        break;

                    case 1:
                        item = int.Parse(bc.monster_1);
                        break;

                    case 2:
                        item = int.Parse(bc.monster_2);
                        break;

                    case 3:
                        item = int.Parse(bc.monster_3);
                        break;

                    case 4:
                        item = int.Parse(bc.monster_4);
                        break;

                    case 5:
                        item = int.Parse(bc.monster_5);
                        break;
                }
                if (item > 0)
                {
                    list.Add(item);
                }
            }
            int num3 = 0;
            for (int j = 0; j < 5; j++)
            {
                Transform transform = obj.FindChild((j + 1).ToString());
                if (num3 < list.Count)
                {
                    monster_config data = ConfigMgr.getInstance().getByEntry<monster_config>(list[num3]);
                    if (data != null)
                    {
                        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(data.card_entry);
                        if (_config2 != null)
                        {
                            transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        }
                        CommonFunc.SetQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), data.quality);
                        transform.FindChild("Level").GetComponent<UILabel>().text = data.level.ToString();
                        GUIDataHolder.setData(transform.gameObject, data);
                        UIEventListener.Get(transform.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressMonsterItem);
                        num3++;
                    }
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SetLayerInfo(VoidTowerData data)
    {
        void_tower_trench_config vttc = ConfigMgr.getInstance().getByEntry<void_tower_trench_config>(data.trench_entry);
        if (vttc != null)
        {
            this.SetLayerReward(vttc);
            this.SetEnemyInfo(vttc);
        }
    }

    private void SetLayerReward(void_tower_trench_config vttc)
    {
        if (vttc != null)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = vttc.drop_type.Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = vttc.drop_item_entry.Split(chArray2);
            char[] chArray3 = new char[] { '|' };
            string[] strArray3 = vttc.drop_item_param.Split(chArray3);
            for (int i = 0; i < strArray.Length; i++)
            {
                Transform component = base.transform.FindChild("RewardPanel/Item" + (i + 1)).GetComponent<Transform>();
                UITexture texture = component.FindChild("Icon").GetComponent<UITexture>();
                UILabel label = component.transform.FindChild("Count").GetComponent<UILabel>();
                UISprite sprite = component.transform.FindChild("QualityBorder").GetComponent<UISprite>();
                UILabel label2 = component.transform.FindChild("Name").GetComponent<UILabel>();
                ItemType type = (ItemType) int.Parse(strArray[i]);
                switch (type)
                {
                    case ItemType.ItemType_Gold:
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Gold");
                        label2.text = ConfigMgr.getInstance().GetWord(0x89);
                        if (int.Parse(strArray3[i]) > 1)
                        {
                            label.text = strArray3[i];
                        }
                        else
                        {
                            label.text = "0";
                        }
                        break;

                    case ItemType.ItemType_Stone:
                        texture.mainTexture = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                        label2.text = ConfigMgr.getInstance().GetWord(0x31b);
                        if (int.Parse(strArray3[i]) > 1)
                        {
                            label.text = strArray3[i];
                        }
                        else
                        {
                            label.text = "0";
                        }
                        break;

                    default:
                        if (type == ItemType.ItemType_Item)
                        {
                            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(int.Parse(strArray2[i]));
                            if (_config != null)
                            {
                                texture.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                                label2.text = _config.name;
                                if (int.Parse(strArray3[i]) > 1)
                                {
                                    label.text = strArray3[i];
                                }
                                else
                                {
                                    label.text = string.Empty;
                                }
                                CommonFunc.SetQualityColor(sprite, _config.quality);
                                GUIDataHolder.setData(component.gameObject, _config.entry);
                                UIEventListener.Get(component.gameObject).onPress = new UIEventListener.BoolDelegate(this.OnClickItemBtn);
                            }
                        }
                        break;
                }
            }
        }
    }

    private void SetRewardPanelInfo(VoidTowerData data)
    {
        int num = data.attack_times - 1;
        if (num < 0)
        {
            num = 0;
        }
        this._RewardRemainCount.text = num.ToString();
        this._RewardShaoDangBtn.gameObject.SetActive((data.trench_entry < data.record) && (data.trench_entry < 0x21));
        if (data.trench_entry < 0x22)
        {
            if (data.trench_entry == 0x21)
            {
                this._NextLayerTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x23b), 100);
                this._TeamlLabel.text = "100";
            }
            else
            {
                string str = (((data.trench_entry * 3) + 1)).ToString() + "-" + (((data.trench_entry + 1) * 3)).ToString();
                this._NextLayerTips.text = string.Format(ConfigMgr.getInstance().GetWord(0x23b), str);
                this._TeamlLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), string.Empty, ((data.trench_entry * 3) + 1).ToString());
                this._Team2Label.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), string.Empty, ((data.trench_entry * 3) + 2).ToString());
                this._Team3Label.text = string.Format(ConfigMgr.getInstance().GetWord(0x4e2a), string.Empty, ((data.trench_entry * 3) + 3).ToString());
            }
        }
        else
        {
            this._NextLayerTips.text = ConfigMgr.getInstance().GetWord(0x22f);
        }
    }

    private void SetShowPanel(bool rewardFlag)
    {
        if (rewardFlag)
        {
            if (this._EnterPanel.active)
            {
                this._EnterPanel.gameObject.SetActive(false);
            }
            if (!this._RewardPanel.active)
            {
                this._RewardPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            if (!this._EnterPanel.active)
            {
                this._EnterPanel.gameObject.SetActive(true);
            }
            if (this._RewardPanel.active)
            {
                this._RewardPanel.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateData(VoidTowerData data)
    {
        this.mData = data;
        if (data != null)
        {
            Debug.Log(data.record + " ****  " + data.trench_entry);
            int num = ((data.record * 3) <= 0x63) ? (data.record * 3) : 0x63;
            this._MaxLayerLabel.text = num + "/100";
            this._RemainCount.text = (data.attack_times >= 0) ? data.attack_times.ToString() : "0";
            this.SetLayerInfo(data);
            bool flag = false;
            if ((data.smash_time > 0) && (TimeMgr.Instance.ServerStampTime < data.smash_time))
            {
                this.mIsStart = true;
                this.mEndTime = data.smash_time;
                flag = true;
                this._TargetItem.gameObject.SetActive(false);
                this._ShaoDangBtn.gameObject.SetActive(false);
                this._TiaoZhanBtn.gameObject.SetActive(false);
            }
            else
            {
                flag = !data.reward_flag;
                this._TargetItem.gameObject.SetActive(true);
                this._ShaoDangBtn.gameObject.SetActive(true);
                this._TiaoZhanBtn.gameObject.SetActive(true);
            }
            this.SetShowPanel(!flag);
            if (data.reward_flag)
            {
                this.SetRewardPanelInfo(data);
            }
            if (data.reward_flag && (data.trench_entry == 0))
            {
                if (<>f__am$cache17 == null)
                {
                    <>f__am$cache17 = delegate (GUIEntity obj) {
                        MessageBox box = (MessageBox) obj;
                        if (<>f__am$cache19 == null)
                        {
                            <>f__am$cache19 = box => SocketMgr.Instance.RequestVoidTowerReward();
                        }
                        box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x22f), new object[0]), <>f__am$cache19, null, true);
                    };
                }
                GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache17, null);
            }
            else if (!data.reward_flag && (data.trench_entry > 0))
            {
                this.SetRewardPanelInfo(data);
                this.SetShowPanel(true);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickItemBtn>c__AnonStorey26F
    {
        internal Item item;

        internal void <>m__5A0(GUIEntity entity)
        {
            (entity as ItemInfoPanel).UpdateData(this.item);
        }
    }
}

