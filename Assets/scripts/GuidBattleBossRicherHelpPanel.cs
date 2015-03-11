using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class GuidBattleBossRicherHelpPanel : GUIPanelEntity
{
    private bool bForTest = true;
    private int curAddOnebuffGetConCnt;
    private int curAddOnebuffNeedStoneCnt;
    private int curAddTenBuffGetConCnt;
    private int curAddTenbuffNeedStoneCnt;
    private int curBossId__;
    private float curBuffValue__;
    private int curBuyBuffCnt;
    private int curDupId__;
    private int curGuildBuybuffCnt;
    private int MaxBuybuffCnt = 100;
    private float oneTimeAddBuffValue;
    private bool oneTimeClick;
    private int oneTimeNeedStone__ = 0x7d0;
    protected UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>> TableGridDamage = new UITableManager<UIAutoGenItem<GridDamageItemTemplate, GridDamageItemModel>>();
    private const int tenBufCnt = 10;
    private int tenTimeNeedStone__;
    private bool tenTimesClick;
    private float totleAddBuff__;
    private int totleAddBuffCnt__;
    public List<HelpBaseItemInfo> xxTestHelpGoldInfoList = new List<HelpBaseItemInfo>();

    private void ClickHelpRank()
    {
        SocketMgr.Instance.RequestC2S_GuildDupSupportRank();
    }

    private void InitGuiData()
    {
        this.LabelCurAttackAddValue.text = ((this.totleAddBuffCnt__ * 0.01f) * 100f) + "%";
        this.oneTimeNeedStone__ = 0;
        this.tenTimeNeedStone__ = 0;
        this.curAddOnebuffGetConCnt = 0;
        this.curAddTenBuffGetConCnt = 0;
        this.xxTestHelpGoldInfoList.Clear();
        for (int i = 0; i < 2; i++)
        {
            HelpBaseItemInfo item = new HelpBaseItemInfo();
            switch (i)
            {
                case 0:
                {
                    item.addBuffDetail = 0.01f;
                    guilddup_buybuff_config _config = ConfigMgr.getInstance().getByEntry<guilddup_buybuff_config>(this.totleAddBuffCnt__);
                    if (_config != null)
                    {
                        this.oneTimeNeedStone__ = _config.cost_stone;
                        this.curAddOnebuffGetConCnt = _config.guildcon_get;
                    }
                    item.needMoney = this.oneTimeNeedStone__;
                    item.rewardNum = this.curAddOnebuffGetConCnt;
                    break;
                }
                case 1:
                    item.addBuffDetail = 0.1f;
                    for (int j = 0; j < 10; j++)
                    {
                        guilddup_buybuff_config _config2 = ConfigMgr.getInstance().getByEntry<guilddup_buybuff_config>(this.totleAddBuffCnt__ + j);
                        if (_config2 != null)
                        {
                            this.tenTimeNeedStone__ += _config2.cost_stone;
                            this.curAddTenBuffGetConCnt += _config2.guildcon_get;
                        }
                    }
                    item.needMoney = this.tenTimeNeedStone__;
                    item.rewardNum = this.curAddTenBuffGetConCnt;
                    break;
            }
            this.xxTestHelpGoldInfoList.Add(item);
        }
        this.SetHandOutItemInfo(this.xxTestHelpGoldInfoList);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.totleAddBuffCnt__ = -1;
        this.oneTimeClick = false;
        this.tenTimesClick = false;
        this.Btn_Close.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.AutoClose.OnUIMouseClick(u => GUIMgr.Instance.ExitModelGUI(this));
        this.Btn_HelpRank.OnUIMouseClick(u => this.ClickHelpRank());
        UIScrollView component = this.ScrollViewList.GetComponent<UIScrollView>();
        if (component != null)
        {
            component.enabled = false;
        }
        guilddup_trench_config _config = ConfigMgr.getInstance().getByEntry<guilddup_trench_config>(this.curBossId__);
        if (_config != null)
        {
            this.MaxBuybuffCnt = _config.max_buy_attack_times;
        }
        else
        {
            this.MaxBuybuffCnt = 100;
        }
        if (this.MaxBuybuffCnt < 0)
        {
            Debug.LogWarning("MaxBuybuffCnt that from config is Error!! ");
        }
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.GoCurAddInfo = base.FindChild<Transform>("GoCurAddInfo");
        this.LabelCurAttackAdd = base.FindChild<UILabel>("LabelCurAttackAdd");
        this.LabelCurAttackAddValue = base.FindChild<UILabel>("LabelCurAttackAddValue");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.GridDamage = base.FindChild<UIGrid>("GridDamage");
        this.LabelTltle = base.FindChild<UILabel>("LabelTltle");
        this.Btn_HelpRank = base.FindChild<UIButton>("Btn_HelpRank");
        this.AutoClose = base.FindChild<UIWidget>("AutoClose");
        this.TableGridDamage.InitFromGrid(this.GridDamage);
    }

    public void InitSupportTrench(int cnt)
    {
        this.totleAddBuffCnt__ = cnt;
        if (this.totleAddBuffCnt__ < 0)
        {
            this.totleAddBuffCnt__ = 0;
        }
        this.InitGuiData();
    }

    public void InitSupportTrench(int dupId, int bossId, int cnt)
    {
        this.curDupId__ = dupId;
        this.curBossId__ = bossId;
        this.totleAddBuffCnt__ = cnt;
    }

    private void InitUiGridData(int itemCnt)
    {
        this.TableGridDamage.Count = itemCnt;
    }

    private void OnClickHelp(int index)
    {
        if (index == 0)
        {
            this.oneTimeClick = true;
            this.curBuyBuffCnt = 1;
        }
        else if (index == 1)
        {
            this.tenTimesClick = true;
            this.curBuyBuffCnt = 10;
        }
        if ((this.totleAddBuffCnt__ + this.curBuyBuffCnt) <= this.MaxBuybuffCnt)
        {
            SocketMgr.Instance.RequestC2S_GuildDupSupportTrench(this.curDupId__, this.curBossId__, this.curBuyBuffCnt);
        }
        else
        {
            Debug.LogWarning("can not buy this times buff,  because ___totleAddBuffCnt__ + curBuyBuffCnt:" + (this.totleAddBuffCnt__ + this.curBuyBuffCnt));
        }
    }

    public void ReceiveGuildDupSupportTrench(S2C_GuildDupSupportTrench dupSupportTrenchData, bool bForTest = false)
    {
        if (!bForTest)
        {
            if (this.oneTimeClick)
            {
                this.curBuffValue__ = this.oneTimeAddBuffValue * this.curBuffValue__;
                this.totleAddBuff__ += this.curBuffValue__;
                this.oneTimeClick = false;
            }
            else if (this.tenTimesClick)
            {
                this.curBuffValue__ = (this.oneTimeAddBuffValue * this.curBuffValue__) * 10f;
                this.totleAddBuff__ += this.curBuffValue__;
                this.tenTimesClick = false;
            }
            this.LabelCurAttackAddValue.text = ((int) (this.totleAddBuff__ * 100f)).ToString();
            ActorData.getInstance().Stone = dupSupportTrenchData.cur_stone;
        }
        this.totleAddBuffCnt__ = dupSupportTrenchData.supportCount;
        SocketMgr.Instance.RequestC2S_GuildDupSupportTrenchRank(this.curDupId__, this.curBossId__);
        this.LabelCurAttackAddValue.text = ((this.totleAddBuffCnt__ * 0.01f) * 100f) + "%";
        this.InitSupportTrench(dupSupportTrenchData.supportCount);
    }

    public void SetHandOutItemInfo(List<HelpBaseItemInfo> hoItemInfoList)
    {
        this.InitUiGridData(hoItemInfoList.Count);
        int count = hoItemInfoList.Count;
        for (int i = 0; i < count; i++)
        {
            <SetHandOutItemInfo>c__AnonStorey20A storeya = new <SetHandOutItemInfo>c__AnonStorey20A {
                <>f__this = this
            };
            string str = string.Empty;
            if (hoItemInfoList[i].addBuffDetail > 0f)
            {
                str = (hoItemInfoList[i].addBuffDetail * 100f) + "%";
            }
            this.TableGridDamage[i].Model.Template.LabelAttackAdd.enabled = true;
            this.TableGridDamage[i].Model.Template.LabelAttackAddValue.text = str;
            this.TableGridDamage[i].Model.Template.LabelGetRewardValue.text = hoItemInfoList[i].rewardNum.ToString();
            this.TableGridDamage[i].Model.Template.LabelNeedGoldNum.text = hoItemInfoList[i].needMoney.ToString();
            storeya.xxIndex = i;
            this.TableGridDamage[i].Model.Template.Btn_Help.OnUIMouseClick(new Action<object>(storeya.<>m__39B));
            BoxCollider component = this.TableGridDamage[i].Model.Template.Btn_Help.gameObject.GetComponent<BoxCollider>();
            if (i == 0)
            {
                this.TableGridDamage[i].Model.Template.Btn_Help.Text(ConfigMgr.getInstance().GetWord(0x186bf));
                if (this.totleAddBuffCnt__ >= this.MaxBuybuffCnt)
                {
                    this.TableGridDamage[i].Model.Template.Btn_Help.SetState(UIButtonColor.State.Disabled, true);
                    component.enabled = false;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.enabled = true;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.text = ConfigMgr.getInstance().GetWord(0x186c1);
                    this.TableGridDamage[i].Model.Template.LabelAttackAdd.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelAttackAddValue.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardTitle.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardValue.gameObject.SetActive(false);
                }
                else
                {
                    this.TableGridDamage[i].Model.Template.Btn_Help.SetState(UIButtonColor.State.Normal, true);
                    component.enabled = true;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.enabled = false;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.text = string.Empty;
                    this.TableGridDamage[i].Model.Template.LabelAttackAdd.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelAttackAddValue.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardTitle.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardValue.gameObject.SetActive(true);
                }
            }
            else
            {
                if (((this.MaxBuybuffCnt - 10) >= 0) && (this.totleAddBuffCnt__ > (this.MaxBuybuffCnt - 10)))
                {
                    this.TableGridDamage[i].Model.Template.Btn_Help.SetState(UIButtonColor.State.Disabled, true);
                    component.enabled = false;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.enabled = true;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.text = ConfigMgr.getInstance().GetWord(0x186c1);
                    this.TableGridDamage[i].Model.Template.LabelAttackAdd.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelAttackAddValue.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardTitle.gameObject.SetActive(false);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardValue.gameObject.SetActive(false);
                }
                else
                {
                    this.TableGridDamage[i].Model.Template.Btn_Help.SetState(UIButtonColor.State.Normal, true);
                    component.enabled = true;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.enabled = false;
                    this.TableGridDamage[i].Model.Template.LabelMaxReward.text = string.Empty;
                    this.TableGridDamage[i].Model.Template.LabelAttackAdd.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelAttackAddValue.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardTitle.gameObject.SetActive(true);
                    this.TableGridDamage[i].Model.Template.LabelGetRewardValue.gameObject.SetActive(true);
                }
                this.TableGridDamage[i].Model.Template.Btn_Help.Text(ConfigMgr.getInstance().GetWord(0x186c0));
            }
        }
    }

    protected UIWidget AutoClose { get; set; }

    public int BossId
    {
        get
        {
            return this.curBossId__;
        }
        set
        {
            this.curBossId__ = value;
        }
    }

    protected UIButton Btn_Close { get; set; }

    protected UIButton Btn_HelpRank { get; set; }

    public int DupId
    {
        get
        {
            return this.curDupId__;
        }
        set
        {
            this.curDupId__ = value;
        }
    }

    protected Transform GoCurAddInfo { get; set; }

    protected UIGrid GridDamage { get; set; }

    protected UILabel LabelCurAttackAdd { get; set; }

    protected UILabel LabelCurAttackAddValue { get; set; }

    protected UILabel LabelTltle { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    public int TotleAddBuffCnt
    {
        get
        {
            return this.totleAddBuffCnt__;
        }
        set
        {
            this.totleAddBuffCnt__ = value;
        }
    }

    [CompilerGenerated]
    private sealed class <SetHandOutItemInfo>c__AnonStorey20A
    {
        internal GuidBattleBossRicherHelpPanel <>f__this;
        internal int xxIndex;

        internal void <>m__39B(object u)
        {
            this.<>f__this.OnClickHelp(this.xxIndex);
        }
    }

    public class GridDamageItemModel : TableItemModel<GuidBattleBossRicherHelpPanel.GridDamageItemTemplate>
    {
        public override void Init(GuidBattleBossRicherHelpPanel.GridDamageItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class GridDamageItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.DamageRankItem = base.FindChild<UIDragScrollView>("DamageRankItem");
            this.LabelAttackAdd = base.FindChild<UILabel>("LabelAttackAdd");
            this.LabelAttackAddValue = base.FindChild<UILabel>("LabelAttackAddValue");
            this.LabelGetRewardTitle = base.FindChild<UILabel>("LabelGetRewardTitle");
            this.LabelGetRewardValue = base.FindChild<UILabel>("LabelGetRewardValue");
            this.Btn_Help = base.FindChild<UIButton>("Btn_Help");
            this.SpriteNeedGold = base.FindChild<UISprite>("SpriteNeedGold");
            this.LabelNeedGoldNum = base.FindChild<UILabel>("LabelNeedGoldNum");
            this.LabelMaxReward = base.FindChild<UILabel>("LabelMaxReward");
        }

        public UIButton Btn_Help { get; private set; }

        public UIDragScrollView DamageRankItem { get; private set; }

        public UILabel LabelAttackAdd { get; private set; }

        public UILabel LabelAttackAddValue { get; private set; }

        public UILabel LabelGetRewardTitle { get; private set; }

        public UILabel LabelGetRewardValue { get; private set; }

        public UILabel LabelMaxReward { get; private set; }

        public UILabel LabelNeedGoldNum { get; private set; }

        public UISprite SpriteNeedGold { get; private set; }
    }

    public class HelpBaseItemInfo
    {
        public float addBuffDetail;
        public int needMoney;
        public int rewardNum;
    }
}

