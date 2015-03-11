using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ActTypeMgr : MonoBehaviour
{
    public GameObject Acrive_collectExchange_Pre;
    public GameObject Active_present_Pre;
    public GameObject Active_Reward_Pre;
    public UISprite activeDetailBgSprite;
    public UILabel actTitle;
    public ActiveBtn btn;
    public List<Act_CollectExchange_Reward> collect_Rewards = new List<Act_CollectExchange_Reward>();
    public UILabel describeLabel;
    public GameObject goInfoBoxForCharge;
    public GameObject goTitleBg;
    public GameObject infoPanel;
    public UIScrollView LabelUisv;
    public UIScrollView ObjUisv;
    public UILabel parameterLabel;
    public List<Act_Pres_Reward> pres_Rewards = new List<Act_Pres_Reward>();
    public List<Act_Rech_Reward> rech_Rewards = new List<Act_Rech_Reward>();
    public GameObject rewardPanel;
    public ActiveBtn2 RuleOrRewardBtn;
    public ActiveShowType showType;
    public UILabel timeDescribeLabel;
    public UIGrid uig;
    public UITexture uit;

    private void Delete_CollectExchange_List()
    {
        for (int i = 0; i < this.collect_Rewards.Count; i++)
        {
            UnityEngine.Object.Destroy(this.collect_Rewards[i].gameObject);
        }
        this.collect_Rewards.Clear();
    }

    private void Delete_Pres_List()
    {
        for (int i = 0; i < this.pres_Rewards.Count; i++)
        {
            UnityEngine.Object.Destroy(this.pres_Rewards[i].gameObject);
        }
        this.pres_Rewards.Clear();
    }

    private void Delete_Rech_List()
    {
        for (int i = 0; i < this.rech_Rewards.Count; i++)
        {
            UnityEngine.Object.Destroy(this.rech_Rewards[i].gameObject);
        }
        this.rech_Rewards.Clear();
    }

    public void InitResetRewardPanelState(bool state, ActiveShowType clientShowType = 0, bool bResetPos = false)
    {
        if (this.RuleOrRewardBtn != null)
        {
            this.RuleOrRewardBtn.InitBtnState(!state);
        }
        this.ResetRewardPanel(state, clientShowType, bResetPos);
    }

    private void ResetCollectExchange(ActiveInfo _info, ActiveShowType clientShowType, bool bResetPos = true)
    {
        this.InitResetRewardPanelState(true, clientShowType, false);
        if (ActivePanel.inst != null)
        {
            this.actTitle.text = ActivePanel.inst.curInfo.activity_name;
            this.describeLabel.text = ActivePanel.inst.curInfo.activity_describe;
            if (this.describeLabel != null)
            {
                UIWidget component = this.describeLabel.GetComponent<UIWidget>();
                if (component != null)
                {
                    Debug.Log("View BUG________________ xxWidget.autoResizeBoxCollider______________:" + component.autoResizeBoxCollider);
                    component.autoResizeBoxCollider = true;
                }
            }
            this.timeDescribeLabel.text = ActivePanel.inst.curInfo.activity_time_describe;
            string str = string.Empty;
            int num = 0;
            if (ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_consume)
            {
                num = 2;
            }
            if (ActivePanel.inst.curInfo.dayParameter != -1)
            {
                str = string.Format(ConfigMgr.getInstance().GetWord(0x9d2b8c + num), ActivePanel.inst.curInfo.dayParameter);
            }
            if ((ActivePanel.inst.curInfo.dayParameter != -1) && (ActivePanel.inst.curInfo.ActParameter != -1))
            {
                str = str + "\n";
            }
            if (ActivePanel.inst.curInfo.ActParameter != -1)
            {
                string word = ConfigMgr.getInstance().GetWord(0x9d2b8d + num);
                str = str + string.Format(word, ActivePanel.inst.curInfo.ActParameter);
            }
            this.parameterLabel.text = str;
            this.Delete_CollectExchange_List();
            bool flag = false;
            if (ActivePanel.inst.curInfo.rewards_configs.Count > 1)
            {
                flag = true;
            }
            for (int i = 0; i < ActivePanel.inst.curInfo.rewards_configs.Count; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this.Acrive_collectExchange_Pre) as GameObject;
                obj2.transform.parent = this.uig.transform;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = new Vector3(0f, -this.uig.cellHeight * i, 0f);
                UIDragScrollView view = obj2.GetComponent<UIDragScrollView>();
                view.scrollView = this.ObjUisv;
                view.enabled = flag;
                Act_CollectExchange_Reward reward = obj2.GetComponent<Act_CollectExchange_Reward>();
                reward.slotId = i;
                reward.entry = ActivePanel.inst.curInfo.rewards_configs[i].entry;
                reward.reward_name = ActivePanel.inst.curInfo.rewards_configs[i].reward_describe;
                reward.reward_items = ActivePanel.inst.curInfo.rewards_configs[i].reward_items;
                reward.needCollectItems = new PressItem[ActivePanel.inst.curInfo.rewards_configs[i].exchangeNeedConfig.Count];
                for (int j = 0; j < ActivePanel.inst.curInfo.rewards_configs[i].exchangeNeedConfig.Count; j++)
                {
                    TxActivityCollectConfig config = ActivePanel.inst.curInfo.rewards_configs[i].exchangeNeedConfig[j];
                    PressItem item = new PressItem();
                    string name = string.Empty;
                    Texture2D textured = null;
                    int quality = 0;
                    int num5 = 100;
                    int num6 = 0;
                    int collectCount = 0;
                    AffixType type = AffixType.AffixType_None;
                    string format = string.Empty;
                    switch ((config.affixType + 1))
                    {
                        case AffixType.AffixType_Card:
                            name = string.Empty;
                            textured = null;
                            quality = 0;
                            format = string.Empty;
                            num5 = 0x65;
                            type = AffixType.AffixType_None;
                            collectCount = 0;
                            goto Label_09B7;

                        case AffixType.AffixType_Equip:
                        {
                            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(config.collectId);
                            if (_config == null)
                            {
                                break;
                            }
                            textured = BundleMgr.Instance.CreateHeadIcon(_config.image);
                            name = _config.name;
                            quality = _config.quality;
                            format = _config.describe;
                            num5 = 100;
                            type = AffixType.AffixType_Card;
                            collectCount = 1;
                            goto Label_09B7;
                        }
                        case AffixType.AffixType_Gem:
                        {
                            equip_config _config2 = ConfigMgr.getInstance().getByEntry<equip_config>(config.collectId);
                            if (_config2 == null)
                            {
                                goto Label_04A8;
                            }
                            textured = BundleMgr.Instance.CreateItemIcon(_config2.icon);
                            name = _config2.name;
                            quality = _config2.quality;
                            format = _config2.describe;
                            num5 = 0x65;
                            type = AffixType.AffixType_Equip;
                            goto Label_09B7;
                        }
                        case AffixType.AffixType_Gold:
                        {
                            gem_config _config3 = ConfigMgr.getInstance().getByEntry<gem_config>(config.collectId);
                            if (_config3 == null)
                            {
                                goto Label_0516;
                            }
                            textured = BundleMgr.Instance.CreateItemIcon(_config3.atlas_name);
                            name = _config3.name;
                            quality = 0;
                            format = _config3.atlas_name;
                            num5 = 0x65;
                            type = AffixType.AffixType_Gem;
                            goto Label_09B7;
                        }
                        case AffixType.AffixType_Stone:
                            textured = BundleMgr.Instance.CreateItemIcon("Item_Icon_Gold");
                            collectCount = config.collectCount;
                            name = CommonFunc.GetShowStr(collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x89);
                            quality = 0;
                            format = "不多不少，亮闪闪的{0}枚金币啊。\n[078dff]( 金钱不是万能的，但没有金钱是万万不能的。 )";
                            format = string.Format(format, CommonFunc.GetShowStr(collectCount, 0x186a0));
                            num5 = 0x66;
                            type = AffixType.AffixType_Gold;
                            goto Label_09B7;

                        case AffixType.AffixType_Courage:
                            textured = BundleMgr.Instance.CreateItemIcon("Item_Icon_Stone");
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x31b);
                            quality = 0;
                            format = "硬通货，人见人爱。";
                            num5 = 0x66;
                            type = AffixType.AffixType_Stone;
                            goto Label_09B7;

                        case AffixType.AffixType_Eq:
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x88);
                            textured = null;
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_Courage;
                            goto Label_09B7;

                        case AffixType.AffixType_RealStone:
                            textured = BundleMgr.Instance.CreateItemIcon("Item_Icon_yqd");
                            name = CommonFunc.GetShowStr(collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x2a39);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_Eq;
                            goto Label_09B7;

                        case AffixType.AffixType_DonateStone:
                            name = "RealStone";
                            textured = null;
                            quality = 0;
                            format = "RealStoneDes";
                            num5 = 0x66;
                            type = AffixType.AffixType_RealStone;
                            goto Label_09B7;

                        case AffixType.AffixType_PhyForce:
                            textured = null;
                            name = "DonateStone";
                            quality = 0;
                            format = "DonateStoneDes";
                            num5 = 0x66;
                            type = AffixType.AffixType_DonateStone;
                            goto Label_09B7;

                        case AffixType.AffixType_SkillBook:
                            textured = BundleMgr.Instance.CreateItemIcon("Item_Icon_Phyforce");
                            name = ConfigMgr.getInstance().GetWord(0x7a);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_PhyForce;
                            goto Label_09B7;

                        case AffixType.AffixType_GoldKey:
                            textured = null;
                            name = ConfigMgr.getInstance().GetWord(0x2a34);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_SkillBook;
                            goto Label_09B7;

                        case AffixType.AffixType_SilverKey:
                            textured = BundleMgr.Instance.CreateItemIcon("jm_icon_a03");
                            name = ConfigMgr.getInstance().GetWord(0x2860);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_GoldKey;
                            goto Label_09B7;

                        case AffixType.AffixType_CopperKey:
                            textured = BundleMgr.Instance.CreateItemIcon("jm_icon_a02");
                            name = ConfigMgr.getInstance().GetWord(0x2861);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_SilverKey;
                            goto Label_09B7;

                        case AffixType.AffixType_Item:
                            textured = BundleMgr.Instance.CreateItemIcon("jm_icon_a01");
                            name = ConfigMgr.getInstance().GetWord(0x2862);
                            quality = 0;
                            format = name;
                            num5 = 0x66;
                            type = AffixType.AffixType_CopperKey;
                            goto Label_09B7;

                        case AffixType.AffixType_ArenaLadderScore:
                        {
                            name = "Item";
                            item_config _config4 = ConfigMgr.getInstance().getByEntry<item_config>(config.collectId);
                            if (_config4 == null)
                            {
                                goto Label_0829;
                            }
                            textured = BundleMgr.Instance.CreateItemIcon(_config4.icon);
                            name = _config4.name;
                            quality = _config4.quality;
                            format = _config4.describe;
                            num5 = 0x65;
                            type = AffixType.AffixType_Item;
                            if (_config4.type != 3)
                            {
                                goto Label_0820;
                            }
                            num5 = 0x67;
                            goto Label_09B7;
                        }
                        case AffixType.AffixType_Contribute:
                            textured = BundleMgr.Instance.CreateItemIcon("Ui_Pk_Icon_coin");
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x88);
                            quality = 0;
                            format = "竞技场的通用货币，可以在竞技场勇气商店使用";
                            num5 = 0x66;
                            type = AffixType.AffixType_ArenaLadderScore;
                            goto Label_09B7;

                        case AffixType.AffixType_OutlandCoin:
                            textured = BundleMgr.Instance.CreateItemIcon("Ui_Gonghui_Icon_ghjx");
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x4e46);
                            quality = 0;
                            format = "公会的通用货币，可以在公会商店使用";
                            num5 = 0x66;
                            type = AffixType.AffixType_Contribute;
                            goto Label_09B7;

                        case AffixType.AffixType_FlameBattleCoin:
                            textured = BundleMgr.Instance.CreateItemIcon("Ui_Out_Icon_stone");
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x851);
                            quality = 0;
                            format = "外域的通用货币，可以在外域晶石商店使用";
                            num5 = 0x66;
                            type = AffixType.AffixType_OutlandCoin;
                            goto Label_09B7;

                        case AffixType.AffixType_LoLArenaScore:
                            textured = BundleMgr.Instance.CreateItemIcon("Ui_Yuanzheng_Icon_gongxun");
                            name = CommonFunc.GetShowStr(config.collectCount, 0x186a0) + ConfigMgr.getInstance().GetWord(0x83a);
                            quality = 0;
                            format = "冰封王座的通用货币，可以在冰封王座功勋商店使用";
                            num5 = 0x66;
                            type = AffixType.AffixType_FlameBattleCoin;
                            goto Label_09B7;

                        case (AffixType.AffixType_Num | AffixType.AffixType_Equip):
                            name = "AffixType_Num";
                            textured = null;
                            quality = 0;
                            format = "AffixType_NumDes";
                            num5 = 0x66;
                            type = AffixType.AffixType_Num;
                            goto Label_09B7;

                        default:
                            goto Label_09B7;
                    }
                    textured = null;
                    name = "xxCardConfig.name";
                    quality = 0;
                    format = string.Empty;
                    num5 = 100;
                    type = AffixType.AffixType_Card;
                    collectCount = 0;
                    goto Label_09B7;
                Label_04A8:
                    textured = null;
                    name = "xxEquipConfig.name";
                    quality = 0;
                    format = string.Empty;
                    num5 = 0x65;
                    type = AffixType.AffixType_Equip;
                    goto Label_09B7;
                Label_0516:
                    textured = null;
                    name = "xxGemConfig.name";
                    quality = 0;
                    format = string.Empty;
                    num5 = 0x65;
                    type = AffixType.AffixType_Gem;
                    goto Label_09B7;
                Label_0820:
                    num5 = 0x65;
                    goto Label_09B7;
                Label_0829:
                    textured = null;
                    name = "xxItemConfig.name";
                    quality = 0;
                    format = string.Empty;
                    num5 = 0x65;
                    type = AffixType.AffixType_Item;
                Label_09B7:
                    item.itemId = config.collectId.ToString();
                    item.itemDes = format;
                    item.itemName = name;
                    item.itemIconTex = textured;
                    item.itemNum = config.collectCount;
                    item.itemQuality = quality;
                    item.itemIconTex = textured;
                    item.itemType = num5;
                    item.cardStar = num6;
                    item.affixType = type;
                    reward.needCollectItems[j] = item;
                }
                reward.flag = ActivePanel.inst.curInfo.rewards_configs[i].flag;
                this.collect_Rewards.Add(reward);
                reward.ResetData(clientShowType);
            }
            if (ActivePanel.inst != null)
            {
                UIPanel parentPnl = this.ObjUisv.gameObject.GetComponent<UIPanel>();
                ActivePanel.inst.AutoSetDepth(parentPnl, this.ObjUisv);
                ActivePanel.inst.AutoSetDepthError();
                if (bResetPos)
                {
                    ActivePanel.inst.SetScrollListInitPos(this.ObjUisv);
                    Debug.Log("点击生成自物体重新设置层级问题ResetCollectExchange！");
                }
            }
        }
    }

    private void ResetDescribe(ActiveInfo _info, ActiveShowType clientShowType)
    {
        this.InitResetRewardPanelState(true, clientShowType, false);
        this.actTitle.text = ActivePanel.inst.curInfo.activity_name;
        this.describeLabel.text = ActivePanel.inst.curInfo.activity_describe;
        if (this.describeLabel != null)
        {
            UIWidget component = this.describeLabel.GetComponent<UIWidget>();
            if (component != null)
            {
                Debug.Log("View BUG________________ xxWidget.autoResizeBoxCollider______________:" + component.autoResizeBoxCollider);
                component.autoResizeBoxCollider = true;
            }
        }
        if (ActivePanel.inst != null)
        {
            ActivePanel.inst.AutoSetDepthError();
            ActivePanel.inst.SetScrollListInitPos(this.ObjUisv);
            Debug.Log("点击生成自物体重新设置层级问题ResetDescribe！");
        }
    }

    public void ResetInfo(ActiveInfo info, bool bResetPos = true, bool canBuy = true)
    {
        if (info != null)
        {
            this.showType = info.showType;
            switch (info.showType)
            {
                case ActiveShowType.describe:
                    this.ResetDescribe(info, info.showType);
                    break;

                case ActiveShowType.recharge:
                    this.ResetRecharge(info, info.showType, bResetPos);
                    break;

                case ActiveShowType.present:
                    this.ResetPresent(info, info.showType, canBuy);
                    break;

                case ActiveShowType.pic:
                    this.ResetPic(info, info.showType);
                    break;

                case ActiveShowType.collectExchange:
                    this.ResetCollectExchange(info, info.showType, bResetPos);
                    break;

                default:
                    this.ResetDescribe(info, info.showType);
                    break;
            }
        }
    }

    private void ResetPic(ActiveInfo _info, ActiveShowType clientShowType)
    {
        if (ActivePanel.inst.curInfo.activity_showUsePic)
        {
            if (ActivePanel.inst == null)
            {
                Debug.Log("Error:________ActivePanel.inst is null!!!!!_____OperateType:_____ResetPic");
            }
            else
            {
                this.InitResetRewardPanelState(true, ActiveShowType.describe, false);
                string str = ActivePanel.inst.curInfo.activity_PicNameDetail;
                if (this.uit != null)
                {
                    this.uit.mainTexture = BundleMgr.Instance.CreateActiveBackGround(str);
                    if (this.btn != null)
                    {
                        this.btn.flag = ActivePanel.inst.curInfo.rewards_configs[0].flag;
                        Debug.Log(string.Concat(new object[] { "==============btn.flag: ", this.btn.flag, "  _info.flag:  ", ActivePanel.inst.curInfo.rewards_configs[0].flag }));
                        this.btn.gameObject.SetActive(false);
                        if (ActivePanel.inst != null)
                        {
                            ActivePanel.inst.AutoSetDepthError();
                            ActivePanel.inst.SetScrollListInitPos(this.ObjUisv);
                            Debug.Log("点击生成自物体重新设置层级问题ResetPic！");
                        }
                    }
                }
            }
        }
    }

    private void ResetPresent(ActiveInfo _info, ActiveShowType clientShowType, bool canBuy = true)
    {
        this.InitResetRewardPanelState(true, clientShowType, false);
        if (ActivePanel.inst == null)
        {
            Debug.Log("Error:________ActivePanel.inst is null!!!!!_____OperateType:_____ResetPresent");
        }
        else
        {
            this.actTitle.text = ActivePanel.inst.curInfo.activity_name;
            this.describeLabel.text = ActivePanel.inst.curInfo.activity_describe;
            if (this.describeLabel != null)
            {
                UIWidget component = this.describeLabel.GetComponent<UIWidget>();
                if (component != null)
                {
                    Debug.Log("View BUG________________ xxWidget.autoResizeBoxCollider______________:" + component.autoResizeBoxCollider);
                    component.autoResizeBoxCollider = true;
                }
            }
            this.timeDescribeLabel.text = ActivePanel.inst.curInfo.activity_time_describe;
            this.Delete_Pres_List();
            bool flag = true;
            if (ActivePanel.inst.curInfo.rewards_configs.Count > 3)
            {
                flag = true;
            }
            for (int i = 0; i < ActivePanel.inst.curInfo.storeList.Count; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this.Active_present_Pre) as GameObject;
                obj2.transform.parent = this.uig.transform;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = new Vector3(0f, -this.uig.cellHeight * i, 0f);
                UIDragScrollView view = obj2.GetComponent<UIDragScrollView>();
                view.scrollView = this.ObjUisv;
                view.enabled = flag;
                Act_Pres_Reward item = obj2.GetComponent<Act_Pres_Reward>();
                item.slotId = i;
                item.entry = ActivePanel.inst.curInfo.storeList[i].commodityId;
                item.reward_name = ActivePanel.inst.curInfo.storeList[i].reward_describe;
                item.reward_items = ActivePanel.inst.curInfo.storeList[i].reward_items;
                item.price = ActivePanel.inst.curInfo.storeList[i].reward_Price;
                item.reward_describe = ActivePanel.inst.curInfo.storeList[i].mainDescribe;
                Debug.Log("ActivePanel.inst.curInfo.storeList[i].mainDescribe:" + ActivePanel.inst.curInfo.storeList[i].mainDescribe);
                item.flag = ActivePanel.inst.curInfo.storeList[i].flag;
                item.purchaseCountOfDay = ActivePanel.inst.curInfo.storeList[i].purchaseCountOfDay;
                item.purchaseCount = ActivePanel.inst.curInfo.storeList[i].purchaseCount;
                item.serverPurCntOfDay = ActivePanel.inst.curInfo.storeList[i].serverPurCntOfDay;
                item.serverPurCnt = ActivePanel.inst.curInfo.storeList[i].serverPurCnt;
                this.pres_Rewards.Add(item);
                item.ResetData(clientShowType);
            }
            if (ActivePanel.inst != null)
            {
                ActivePanel.inst.AutoSetDepthError();
                ActivePanel.inst.SetScrollListInitPos(this.ObjUisv);
                Debug.Log("点击生成自物体重新设置层级问题ResetPresent！");
            }
        }
    }

    private void ResetRecharge(ActiveInfo _info, ActiveShowType clientShowType, bool bResetPos = true)
    {
        this.InitResetRewardPanelState(true, clientShowType, bResetPos);
        if (ActivePanel.inst == null)
        {
            Debug.Log("Error:________ActivePanel.inst is null!!!!!_____OperateType:_____ResetRecharge");
        }
        else
        {
            this.actTitle.text = ActivePanel.inst.curInfo.activity_name;
            this.describeLabel.text = ActivePanel.inst.curInfo.activity_describe;
            this.timeDescribeLabel.text = ActivePanel.inst.curInfo.activity_time_describe;
            string str = string.Empty;
            int num = 0;
            if (ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_consume)
            {
                num = 2;
            }
            if (ActivePanel.inst.curInfo.dayParameter != -1)
            {
                str = string.Format(ConfigMgr.getInstance().GetWord(0x9d2b8c + num), ActivePanel.inst.curInfo.dayParameter);
            }
            if ((ActivePanel.inst.curInfo.dayParameter != -1) && (ActivePanel.inst.curInfo.ActParameter != -1))
            {
                str = str + "\n";
            }
            if (ActivePanel.inst.curInfo.ActParameter != -1)
            {
                string word = ConfigMgr.getInstance().GetWord(0x9d2b8d + num);
                str = str + string.Format(word, ActivePanel.inst.curInfo.ActParameter);
            }
            this.parameterLabel.text = str;
            this.Delete_Rech_List();
            bool flag = false;
            if (ActivePanel.inst.curInfo.rewards_configs.Count > 1)
            {
                flag = true;
            }
            for (int i = 0; i < ActivePanel.inst.curInfo.rewards_configs.Count; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this.Active_Reward_Pre) as GameObject;
                obj2.transform.parent = this.uig.transform;
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = new Vector3(0f, -this.uig.cellHeight * i, 0f);
                UIDragScrollView component = obj2.GetComponent<UIDragScrollView>();
                component.scrollView = this.ObjUisv;
                component.enabled = flag;
                Act_Rech_Reward item = obj2.GetComponent<Act_Rech_Reward>();
                item.slotId = i;
                item.entry = ActivePanel.inst.curInfo.rewards_configs[i].entry;
                item.reward_name = ActivePanel.inst.curInfo.rewards_configs[i].reward_describe;
                item.reward_items = ActivePanel.inst.curInfo.rewards_configs[i].reward_items;
                item.flag = ActivePanel.inst.curInfo.rewards_configs[i].flag;
                item.subTimeEnable = ActivePanel.inst.curInfo.rewards_configs[i].subTimeEnable;
                item.start_time = ActivePanel.inst.curInfo.rewards_configs[i].start_time;
                item.duration = ActivePanel.inst.curInfo.rewards_configs[i].duration;
                item.cd_time = ActivePanel.inst.curInfo.rewards_configs[i].cd_time;
                this.rech_Rewards.Add(item);
                item.ResetData(clientShowType);
            }
            if (bResetPos && (ActivePanel.inst != null))
            {
                ActivePanel.inst.AutoSetDepthError();
                ActivePanel.inst.SetScrollListInitPos(this.ObjUisv);
                Debug.Log("点击生成自物体重新设置层级问题ResetRecharge！");
            }
        }
    }

    public void ResetRewardPanel(bool state, ActiveShowType clientShowType, bool bResetPos = true)
    {
        if (this.rewardPanel != null)
        {
            this.rewardPanel.SetActive(state);
        }
        if (this.infoPanel != null)
        {
            this.infoPanel.SetActive(!state);
        }
        if (state)
        {
            if (ActivePanel.inst == null)
            {
                return;
            }
            if ((clientShowType == ActiveShowType.recharge) || (clientShowType == ActiveShowType.collectExchange))
            {
                if ((ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_login) || (ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_collect_exchange))
                {
                    if (this.activeDetailBgSprite != null)
                    {
                        this.activeDetailBgSprite.SetDimensions(0x20a, 0x1d8);
                        Vector3 localPosition = this.activeDetailBgSprite.transform.localPosition;
                        this.activeDetailBgSprite.transform.localPosition = new Vector3(localPosition.x, -38f, localPosition.z);
                        if (this.goTitleBg != null)
                        {
                            this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 206f, this.goTitleBg.transform.localPosition.z);
                        }
                    }
                    if (this.goInfoBoxForCharge != null)
                    {
                        this.goInfoBoxForCharge.SetActive(false);
                    }
                    if (bResetPos && (this.ObjUisv != null))
                    {
                        this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -90f, 520f, 330f);
                    }
                }
                else
                {
                    if (this.activeDetailBgSprite != null)
                    {
                        this.activeDetailBgSprite.SetDimensions(0x20a, 0x19e);
                        Vector3 vector2 = this.activeDetailBgSprite.transform.localPosition;
                        this.activeDetailBgSprite.transform.localPosition = new Vector3(vector2.x, -9f, vector2.z);
                        if (this.goTitleBg != null)
                        {
                            this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 177f, this.goTitleBg.transform.localPosition.z);
                        }
                    }
                    if (this.goInfoBoxForCharge != null)
                    {
                        this.goInfoBoxForCharge.SetActive(true);
                    }
                    if (this.ObjUisv != null)
                    {
                        this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -67f, 520f, 290f);
                    }
                }
            }
            else if (clientShowType == ActiveShowType.describe)
            {
                if (this.goTitleBg != null)
                {
                    this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 177f, this.goTitleBg.transform.localPosition.z);
                }
                if (this.ObjUisv != null)
                {
                    this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -55f, 510f, 398f);
                }
            }
            else if (clientShowType == ActiveShowType.present)
            {
                if (this.goTitleBg != null)
                {
                    this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 177f, this.goTitleBg.transform.localPosition.z);
                }
                if (this.ObjUisv != null)
                {
                    this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -90f, 520f, 330f);
                }
            }
            else if (clientShowType == ActiveShowType.pic)
            {
                if (this.goTitleBg != null)
                {
                    this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 177f, this.goTitleBg.transform.localPosition.z);
                }
                if (this.ObjUisv != null)
                {
                    this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -55f, 510f, 398f);
                }
            }
            else
            {
                if (this.goTitleBg != null)
                {
                    this.goTitleBg.transform.localPosition = new Vector3(this.goTitleBg.transform.localPosition.x, 177f, this.goTitleBg.transform.localPosition.z);
                }
                if (this.ObjUisv != null)
                {
                    this.ObjUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -67f, 520f, 290f);
                }
            }
        }
        else
        {
            if (ActivePanel.inst == null)
            {
                return;
            }
            if ((clientShowType == ActiveShowType.recharge) || (clientShowType == ActiveShowType.collectExchange))
            {
                if ((ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_login) || (ActivePanel.inst.curInfo.activity_type == ActivityType.e_tencent_activity_collect_exchange))
                {
                    if (this.LabelUisv != null)
                    {
                        this.LabelUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -60f, 510f, 400f);
                    }
                }
                else if (this.LabelUisv != null)
                {
                    this.LabelUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -35f, 520f, 342f);
                }
            }
            else if (this.LabelUisv != null)
            {
                this.LabelUisv.gameObject.GetComponent<UIPanel>().baseClipRegion = new Vector4(110f, -60f, 510f, 400f);
            }
            if (this.LabelUisv != null)
            {
                ActivePanel.inst.SetScrollListInitPos(this.LabelUisv);
            }
            if (this.describeLabel != null)
            {
                this.describeLabel.text = ActivePanel.inst.curInfo.activity_describe;
                UIWidget component = this.describeLabel.GetComponent<UIWidget>();
                if (component != null)
                {
                    Debug.Log("View BUG________________ xxWidget.autoResizeBoxCollider______________:" + component.autoResizeBoxCollider);
                    BoxCollider collider = this.describeLabel.GetComponent<BoxCollider>();
                    if (collider != null)
                    {
                        Debug.Log("describeLabel.height::::" + this.describeLabel.height);
                        SocketMgr.Instance.testRuleBoxcollider(collider.size, component.autoResizeBoxCollider);
                    }
                }
            }
        }
        if (ActivePanel.inst != null)
        {
            ActivePanel.inst.AutoSetDepthError();
        }
    }
}

