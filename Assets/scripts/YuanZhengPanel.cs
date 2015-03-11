using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class YuanZhengPanel : GUIEntity
{
    public UIPanel _MapRoot;
    private Transform _ShuTuiBtn;
    public GameObject _Tips;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheA;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheB;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cacheC;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cacheD;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private int mCurrNode;
    private ArrayList mFameCfgList;
    private FlameBattleInfo mFlameInfo;
    private bool mIsStart;
    private List<Transform> mNodeGirdList = new List<Transform>();

    private void ExitPanel()
    {
    }

    private string GetBoxIcon(int node, bool isOpen)
    {
        if (((node == 5) || (node == 11)) || (node == 0x11))
        {
            return (!isOpen ? "Ui_Tower_Icon_yin" : "Ui_Tower_Icon_yinopen");
        }
        if (node == 0x13)
        {
            return (!isOpen ? "Ui_Tower_Icon_jin" : "Ui_Tower_Icon_jinopen");
        }
        return (!isOpen ? "Ui_Tower_Icon_tong" : "Ui_Tower_Icon_tongopen");
    }

    private int GetGoldTips(int node)
    {
        level_limit_config _config = ConfigMgr.getInstance().getByEntry<level_limit_config>(0);
        if (_config != null)
        {
            int num = GameConstValues.FLAMEBATTLE_GOLD_CONST_1 + ((ActorData.getInstance().Level - _config.flamebattle) * GameConstValues.FLAMEBATTLE_GOLD_CONST_2);
            int num2 = 1 + (node / 4);
            float num3 = 1f + (((node - 1) * GameConstValues.FLAMEBATTLE_GOLD_RATE) / 10000f);
            return (int) ((num * num2) * num3);
        }
        return 0;
    }

    private void GetMapScrollMapVal()
    {
        UIScrollBar component = base.gameObject.transform.FindChild("Scroll Bar").GetComponent<UIScrollBar>();
        ActorData.getInstance().YuanZhenMapVal = component.scrollValue;
    }

    public override void GUIStart()
    {
    }

    public void InitFlameBattleInfo(FlameBattleInfo info)
    {
        this.mFlameInfo = info;
        if (info != null)
        {
            this.UpdateShuTuiBtnStat();
            this.mCurrNode = info.cur_node;
            int num = info.cur_node / 6;
            int maxShowNode = ((num + 1) * 6) - 1;
            if (maxShowNode > 0x13)
            {
                maxShowNode = 0x13;
            }
            Debug.Log(string.Concat(new object[] { maxShowNode, " <-----  ", info.cur_node, " last_reset_node:  ", info.last_reset_node }));
            for (int i = 0; i < this.mFameCfgList.Count; i++)
            {
                this.mNodeGirdList[i].gameObject.SetActive(true);
                UITexture component = this.mNodeGirdList[i].gameObject.transform.FindChild("Sprite").GetComponent<UITexture>();
                Transform transform = this.mNodeGirdList[i].gameObject.transform.FindChild("BoxBg");
                flame_battle_config _config = (flame_battle_config) this.mFameCfgList[i];
                this.mNodeGirdList[i].gameObject.transform.FindChild("Arrows").gameObject.SetActive(i == info.cur_node);
                if (_config != null)
                {
                    if (_config.type == 0)
                    {
                        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(info.target_data_list[i / 2].head_entry);
                        if (_config2 != null)
                        {
                            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.image);
                        }
                        nguiTextureGrey.doChangeEnableGrey(component, i < info.cur_node);
                        GUIDataHolder.setData(this.mNodeGirdList[i].gameObject, i);
                        UIEventListener.Get(this.mNodeGirdList[i].gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickNodeBtn);
                        this.mNodeGirdList[i].transform.GetComponent<BoxCollider>().enabled = i <= info.cur_node;
                        UISprite frame = this.mNodeGirdList[i].gameObject.transform.FindChild("Border/frame").GetComponent<UISprite>();
                        UISprite sprite2 = this.mNodeGirdList[i].gameObject.transform.FindChild("Border/QIcon").GetComponent<UISprite>();
                        CommonFunc.SetPlayerHeadFrame(frame, sprite2, info.target_data_list[i / 2].head_frame);
                        this.mNodeGirdList[i].gameObject.transform.FindChild("Border").gameObject.SetActive(true);
                        transform.gameObject.SetActive(false);
                    }
                    else
                    {
                        this.mNodeGirdList[i].transform.FindChild("Sprite").GetComponent<TweenRotation>().enabled = i == info.cur_node;
                        component.mainTexture = BundleMgr.Instance.CreateBoxIcon(this.GetBoxIcon(i, i < info.cur_node));
                        if (i == info.cur_node)
                        {
                            UIEventListener.Get(this.mNodeGirdList[i].gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPickRewardBtn);
                        }
                        else if (i > info.cur_node)
                        {
                            GUIDataHolder.setData(this.mNodeGirdList[i].gameObject, (i / 2) + 1);
                            UIEventListener.Get(this.mNodeGirdList[i].gameObject).onClick = new UIEventListener.VoidDelegate(this.OnPopTisp);
                        }
                        if (i == 0x13)
                        {
                            component.width = 100;
                            component.height = 100;
                        }
                        else if (((i == 5) || (i == 11)) || (i == 0x11))
                        {
                            component.width = 80;
                            component.height = 80;
                        }
                        else
                        {
                            component.width = 70;
                            component.height = 70;
                        }
                        this.mNodeGirdList[i].gameObject.transform.FindChild("Border").gameObject.SetActive(false);
                        transform.gameObject.SetActive(true);
                        transform.localPosition = new Vector3(0f, -11f, 0f);
                    }
                }
            }
            this.SetNodeShowStat(maxShowNode);
            this.SetRemainCount();
        }
    }

    private void InitGuiControlEvent()
    {
        this.mFameCfgList = ConfigMgr.getInstance().getList<flame_battle_config>();
        UIEventListener.Get(base.transform.FindChild("Bottom/Group/RuleBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRuleBtn);
        UIEventListener.Get(base.transform.FindChild("Bottom/Group/ResetBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickResetBtn);
        UIEventListener.Get(base.transform.FindChild("Bottom/Group/ShopBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShopBtn);
        this._ShuTuiBtn = base.transform.FindChild("Bottom/Group/ShuTuiBtn");
        UIEventListener.Get(this._ShuTuiBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickShuTuiBtn);
        UIEventListener.Get(this._Tips.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTipsBtn);
        Transform transform4 = base.transform.FindChild("Map/Grid/Item");
        for (int i = 0; i < this.mFameCfgList.Count; i++)
        {
            Transform item = transform4.FindChild("node" + i);
            this.mNodeGirdList.Add(item);
        }
    }

    private void OnClickNodeBtn(GameObject go)
    {
        <OnClickNodeBtn>c__AnonStorey276 storey = new <OnClickNodeBtn>c__AnonStorey276 {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storey.idx = (int) obj2;
            storey.info = this.mFlameInfo.target_data_list[storey.idx / 2];
            if ((storey.info != null) && (GUIMgr.Instance.GetGUIEntity<TargetTeamPanel>() == null))
            {
                ActorData.getInstance().mCurrDupReturnPrePara = null;
                GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storey.<>m__5C2), base.gameObject);
            }
        }
    }

    private void OnClickPickRewardBtn(GameObject go)
    {
        SocketMgr.Instance.RequestGainFlameBattleReward();
    }

    private void OnClickResetBtn(GameObject go)
    {
        GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
            MessageBox box = (MessageBox) obj;
            if (this.mFlameInfo.surplus_reset_count < 1)
            {
                Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Flame_Refresh);
                if (ticketItemBySubType != null)
                {
                    int num = ticketItemBySubType.num;
                    if ((this.mFlameInfo != null) && (this.mFlameInfo.surplus_reset_count > 0))
                    {
                        num += this.mFlameInfo.surplus_reset_count;
                    }
                    if (<>f__am$cacheC == null)
                    {
                        <>f__am$cacheC = mb => SocketMgr.Instance.RequestResetFlameBattle(true);
                    }
                    box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x855), num), <>f__am$cacheC, null, false);
                }
                else
                {
                    box.SetDialog(ConfigMgr.getInstance().GetWord(0x838), null, null, true);
                }
            }
            else
            {
                if (<>f__am$cacheD == null)
                {
                    <>f__am$cacheD = mb => SocketMgr.Instance.RequestResetFlameBattle(false);
                }
                box.SetDialog(ConfigMgr.getInstance().GetWord(0x834), <>f__am$cacheD, null, false);
            }
        }, base.gameObject);
    }

    private void OnClickRuleBtn(GameObject go)
    {
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate (GUIEntity obj) {
                WorldCupRulePanel panel = (WorldCupRulePanel) obj;
                panel.Depth = 800;
                panel.SetYuanZhengRule();
            };
        }
        GUIMgr.Instance.DoModelGUI("WorldCupRulePanel", <>f__am$cacheB, null);
    }

    private void OnClickShopBtn(GameObject go)
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (GUIEntity obj) {
                ((ShopPanel) obj).SetShopType(ShopCoinType.YuanZhengCoin);
                SocketMgr.Instance.RequestFlameBattleShopInfo();
            };
        }
        GUIMgr.Instance.PushGUIEntity("ShopPanel", <>f__am$cacheA);
    }

    private void OnClickShuTuiBtn(GameObject go)
    {
        if ((this.mFlameInfo.last_reset_node < 1) || (this.mFlameInfo.last_reset_node <= this.mFlameInfo.cur_node))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x595));
            this.UpdateShuTuiBtnStat();
        }
        else
        {
            SocketMgr.Instance.RequestFlameBattleSmash();
        }
    }

    private void OnClickTipsBtn(GameObject go)
    {
        this._Tips.SetActive(false);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        this.UpdateFameCoin();
        GUIMgr.Instance.FloatTitleBar();
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.OnlyShowFunBtn(true);
        }
        this.ScrollMap();
        this.ResetMapRange();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.InitGuiControlEvent();
        if (ActorData.getInstance().mFlameBattleInfo == null)
        {
            SocketMgr.Instance.RequestFlameBattleInfo();
        }
        else
        {
            this.InitFlameBattleInfo(ActorData.getInstance().mFlameBattleInfo);
        }
        this.mIsStart = true;
    }

    private void OnPopTisp(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            this._Tips.transform.FindChild("Gold").GetComponent<UILabel>().text = this.GetGoldTips((int) obj2).ToString();
            this._Tips.SetActive(true);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
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
                if (((TimeMgr.Instance.ServerDateTime.Hour == GameConstValues.FRESH_USERDATA_TIME) && (TimeMgr.Instance.ServerDateTime.Minute == 0)) && (TimeMgr.Instance.ServerDateTime.Second == 1))
                {
                    SocketMgr.Instance.RequestFlameBattleInfo();
                    Debug.Log("----RequestFlameBattleInfo----");
                }
            }
        }
    }

    private void ResetMapRange()
    {
        UIPanel component = base.transform.FindChild("Map").GetComponent<UIPanel>();
        int activeHeight = GUIMgr.Instance.Root.activeHeight;
        float width = (((float) Screen.width) / ((float) Screen.height)) * activeHeight;
        component.SetRect(0f, 0f, width, (float) activeHeight);
    }

    public void ScrollMap()
    {
        base.transform.FindChild("Scroll Bar").GetComponent<UIScrollBar>().scrollValue = ActorData.getInstance().YuanZhenMapVal;
    }

    private void SetFlameSmashEndData(int endNode)
    {
        if ((endNode < 0) || (endNode >= this.mFameCfgList.Count))
        {
            Debug.Log("smash end Node index error");
        }
        else
        {
            int num = 0;
            for (int i = 0; i < this.mFameCfgList.Count; i++)
            {
                if (i >= endNode)
                {
                    break;
                }
                flame_battle_config _config = (flame_battle_config) this.mFameCfgList[i];
                if (_config.type == 0)
                {
                    num++;
                }
            }
            for (int j = 0; j < num; j++)
            {
                FlameBattleTargetData data = this.mFlameInfo.target_data_list[j];
                foreach (TargetCard card in data.target_card_list)
                {
                    card.card_cur_hp = 0;
                    card.card_cur_energy = 0;
                }
            }
        }
    }

    private void SetNodeShowStat(int maxShowNode)
    {
        for (int i = 0; i < this.mFameCfgList.Count; i++)
        {
            this.mNodeGirdList[i].gameObject.SetActive(i <= maxShowNode);
        }
    }

    private void SetRemainCount()
    {
        UILabel component = base.transform.FindChild("Bottom/Group/RemainCount").GetComponent<UILabel>();
        UISprite sprite = base.transform.FindChild("Bottom/Group/Icon").GetComponent<UISprite>();
        UILabel label2 = sprite.transform.FindChild("ItemCount").GetComponent<UILabel>();
        Item ticketItemBySubType = ActorData.getInstance().GetTicketItemBySubType(TicketType.Ticket_Flame_Refresh);
        if (((ticketItemBySubType != null) && (this.mFlameInfo != null)) && (this.mFlameInfo.surplus_reset_count < 1))
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(ticketItemBySubType.entry);
            if (_config != null)
            {
                sprite.spriteName = _config.icon;
            }
            label2.text = "X" + ticketItemBySubType.num;
            sprite.gameObject.SetActive(true);
            component.text = string.Empty;
        }
        else
        {
            sprite.gameObject.SetActive(false);
            component.text = string.Format(ConfigMgr.getInstance().GetWord(0x839), (this.mFlameInfo != null) ? ((int) this.mFlameInfo.surplus_reset_count) : 0);
        }
    }

    public void SmashSccuess(byte currNode)
    {
        this.SetFlameSmashEndData(currNode);
        this.mFlameInfo.cur_node = currNode;
        this.InitFlameBattleInfo(this.mFlameInfo);
        Debug.Log(currNode + "   UUUUUUUUUUUUUU");
        this._ShuTuiBtn.gameObject.SetActive(false);
    }

    public void UpdateFameCoin()
    {
        base.transform.FindChild("Bottom/Group/CoinValue/Label").GetComponent<UILabel>().text = ActorData.getInstance().UserInfo.flamebattleCoin.ToString();
        this.SetRemainCount();
    }

    public void UpdateNodeInfo(int currNode)
    {
        this.mCurrNode = currNode;
        UIEventListener listener1 = UIEventListener.Get(this.mNodeGirdList[currNode - 1].gameObject);
        listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Remove(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickPickRewardBtn));
        this.mNodeGirdList[currNode - 1].gameObject.transform.FindChild("Sprite").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateBoxIcon(this.GetBoxIcon(currNode - 1, true));
        TweenRotation component = this.mNodeGirdList[currNode - 1].transform.FindChild("Sprite").GetComponent<TweenRotation>();
        this.mNodeGirdList[currNode - 1].transform.FindChild("Arrows").gameObject.SetActive(false);
        component.enabled = false;
        if (currNode < 20)
        {
            Debug.Log("currNode:" + currNode);
            if ((currNode % 6) == 0)
            {
                int maxShowNode = (((currNode / 6) + 1) * 6) - 1;
                if (maxShowNode > 0x13)
                {
                    maxShowNode = 0x13;
                }
                this.SetNodeShowStat(maxShowNode);
            }
            flame_battle_config _config = (flame_battle_config) this.mFameCfgList[currNode];
            if ((_config != null) && (_config.type == 0))
            {
                nguiTextureGrey.doChangeEnableGrey(this.mNodeGirdList[currNode].gameObject.transform.FindChild("Sprite").GetComponent<UITexture>(), false);
                this.mNodeGirdList[currNode].transform.GetComponent<BoxCollider>().enabled = true;
                this.mNodeGirdList[currNode].gameObject.transform.FindChild("Arrows").gameObject.SetActive(true);
            }
            this.UpdateShuTuiBtnStat();
        }
    }

    public void UpdateShuTuiBtnStat()
    {
        if (this.mFlameInfo != null)
        {
            this._ShuTuiBtn.gameObject.SetActive((this.mFlameInfo.last_reset_node > this.mFlameInfo.cur_node) && ((ActorData.getInstance().VipType + 1) > 9));
        }
        else
        {
            this._ShuTuiBtn.gameObject.SetActive(false);
        }
    }

    public void UpdateTargetTeamInfo(S2C_RefreshFlameBattleTarget res)
    {
        this.mFlameInfo.target_data_list[res.target_index] = res.target_data;
        UITexture component = this.mNodeGirdList[res.target_index * 2].gameObject.transform.FindChild("Sprite").GetComponent<UITexture>();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(res.target_data.head_entry);
        if (_config != null)
        {
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickNodeBtn>c__AnonStorey276
    {
        internal YuanZhengPanel <>f__this;
        internal int idx;
        internal FlameBattleTargetData info;

        internal void <>m__5C2(GUIEntity obj)
        {
            TargetTeamPanel panel = (TargetTeamPanel) obj;
            panel.SetYuanZhengInfo(this.info);
            panel.SetYuanZhengNodeInfo(this.idx);
            panel.SetPkBtnStat(this.idx == this.<>f__this.mCurrNode);
            if (this.<>f__this.mCurrNode == this.idx)
            {
                SocketMgr.Instance.RequestGetFlameBattleTargetStatus();
            }
            this.<>f__this.GetMapScrollMapVal();
        }
    }
}

