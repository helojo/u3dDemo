using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class DetainsDartSelFlagAndTeamPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2C;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2D;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2E;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache2F;
    private bool AnimbDirection = true;
    private int AnimCurFlagIndex;
    private int AnimCurMoveCnt;
    private int AnimFlagMaxCnt = 4;
    private bool AnimIsLastQuan;
    private bool AnimIsPlay = true;
    private int AnimMaxMoveWholeCnt = 2;
    private int AnimOneWholeRoundCnt;
    private float AnimSpeed;
    private float AnimTime;
    private int AnimWillDStayIndex;
    private int curFreshCnt__;
    private int curInviteFriendMainCardEntry__;
    private int curReFreshFlagNeed__;
    private int curSelFlagIndex__ = -1;
    private int curTeamIndex__ = -1;
    private bool isInviteFriend__;
    private bool isSelTeam__;
    private int maxFlagCnt__ = 4;
    private int maxOneTeamCnt__ = 5;
    public List<int> SelHeroEntryList = new List<int>();
    protected UITableManager<UIAutoGenItem<FlagGridItemTemplate, FlagGridItemModel>> TableFlagGrid = new UITableManager<UIAutoGenItem<FlagGridItemTemplate, FlagGridItemModel>>();
    protected UITableManager<UIAutoGenItem<TeamGridItemTemplate, TeamGridItemModel>> TableTeamGrid = new UITableManager<UIAutoGenItem<TeamGridItemTemplate, TeamGridItemModel>>();

    private void AnimInitDataInfo()
    {
        this.AnimFlagMaxCnt = 4;
        this.AnimOneWholeRoundCnt = 2 * (this.AnimFlagMaxCnt - 1);
        this.AnimIsPlay = false;
        this.AnimbDirection = true;
        this.AnimCurFlagIndex = 0;
        this.AnimMaxMoveWholeCnt = 2;
        this.AnimIsLastQuan = false;
        this.AnimCurMoveCnt = 0;
        this.AnimTime = 0f;
        this.AnimWillDStayIndex = 0;
        this.AnimSpeed = Time.deltaTime * 3f;
    }

    private void Close()
    {
        Debug.LogWarning("Close!");
        GUIMgr.Instance.PopGUIEntity();
        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.None;
    }

    public void ConfirmReFreshFlags()
    {
        if (XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            if (false)
            {
                this.RefreshFlagInfo();
            }
            XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex = -1;
            XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo.Clear();
        }
        else
        {
            int buyFalgNeedStoneCnt = this.GetBuyFalgNeedStoneCnt();
            buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(buyFalgNeedStoneCnt);
            if (_config != null)
            {
                this.curReFreshFlagNeed__ = _config.refresh_convoy_flag_cost_stone;
                if (ActorData.getInstance().Stone >= _config.refresh_convoy_flag_cost_stone)
                {
                    if (XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex >= 0)
                    {
                        this.SelectFlag(XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex);
                    }
                    SocketMgr.Instance.RequestC2S_RefreshConvoyFlag();
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186dc));
                }
            }
        }
    }

    private void FlagAnimFlagSelStateInit()
    {
        IEnumerator<UIAutoGenItem<FlagGridItemTemplate, FlagGridItemModel>> enumerator = this.TableFlagGrid.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                UIAutoGenItem<FlagGridItemTemplate, FlagGridItemModel> current = enumerator.Current;
                current.Model.Template.SpriteSelectState.gameObject.SetActive(false);
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    public int GetBuyFalgNeedStoneCnt()
    {
        int count = 0;
        count = ConfigMgr.getInstance().getList<buy_cost_config>().Count;
        if (XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt > (count - 1))
        {
            return (count - 1);
        }
        return XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt;
    }

    private void getPageIdToFriendIdListDicInfo()
    {
        int key = 0;
        XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic.Clear();
        for (int i = 0; i < ActorData.getInstance().FriendList.Count; i++)
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt == 0)
            {
                Debug.Log("GameDetainsDartMgr.Singleton.onePageMaxFriendCnt is Error！ PageCnt：" + XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt);
                return;
            }
            key = i / XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt;
            int num3 = i % XSingleton<GameDetainsDartMgr>.Singleton.onePageMaxFriendCnt;
            if (!XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic.ContainsKey(key))
            {
                List<long> list = new List<long>();
                XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic.Add(key, list);
                if (!XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic[key].Contains(ActorData.getInstance().FriendList[i].userInfo.id))
                {
                    XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic[key].Add(ActorData.getInstance().FriendList[i].userInfo.id);
                }
                else
                {
                    Debug.Log("FriendList have same Id ！！！");
                }
            }
            else if (!XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic[key].Contains(ActorData.getInstance().FriendList[i].userInfo.id))
            {
                XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic[key].Add(ActorData.getInstance().FriendList[i].userInfo.id);
            }
            else
            {
                Debug.Log("FriendList have same Id ！！！");
            }
        }
        foreach (KeyValuePair<int, List<long>> pair in XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic)
        {
            Debug.LogWarning(" GameDetainsDartMgr.Singleton.pageIdToPageFriendIdDic" + pair.Key + pair.Value);
        }
    }

    private int GetSelfCanBuyRobMaxTime()
    {
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            return _config.buy_convoy_rob_times;
        }
        return 0;
    }

    private void initFalgSelState()
    {
        for (int i = 0; i < this.maxFlagCnt__; i++)
        {
            this.TableFlagGrid[i].Model.Template.SpriteSelectState.gameObject.SetActive(false);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Close.OnUIMouseClick(u => this.Close());
        this.Btn_RefreshFlags.OnUIMouseClick(u => this.OnClickReFreshFlags());
        this.Btn_Next.OnUIMouseClick(u => this.NextTep());
        this.Btn_Invite.OnUIMouseClick(u => this.InviteFriend());
        this.isInviteFriend__ = false;
        this.isSelTeam__ = false;
        this.getPageIdToFriendIdListDicInfo();
        this.AnimIsPlay = false;
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.LabelTitle = base.FindChild<UILabel>("LabelTitle");
        this.Btn_Close = base.FindChild<UIButton>("Btn_Close");
        this.FlagsList = base.FindChild<UIPanel>("FlagsList");
        this.FlagGrid = base.FindChild<UIGrid>("FlagGrid");
        this.Btn_RefreshFlags = base.FindChild<UISprite>("Btn_RefreshFlags");
        this.LabelRefresh = base.FindChild<UILabel>("LabelRefresh");
        this.GoBottomInfo = base.FindChild<Transform>("GoBottomInfo");
        this.GoLeftBottom = base.FindChild<Transform>("GoLeftBottom");
        this.Btn_Invite = base.FindChild<UISprite>("Btn_Invite");
        this.InviteFriendInfo = base.FindChild<Transform>("InviteFriendInfo");
        this.SpriteFriendHeadQuality = base.FindChild<UISprite>("SpriteFriendHeadQuality");
        this.TextureFriendHeadIcon = base.FindChild<UITexture>("TextureFriendHeadIcon");
        this.GoRightBottom = base.FindChild<Transform>("GoRightBottom");
        this.TeamList = base.FindChild<UIPanel>("TeamList");
        this.TeamGrid = base.FindChild<UIGrid>("TeamGrid");
        this.LabelTeamTitle = base.FindChild<UILabel>("LabelTeamTitle");
        this.Btn_Next = base.FindChild<UIButton>("Btn_Next");
        this.AutoClose = base.FindChild<UISprite>("AutoClose");
        this.SpriteStone = base.FindChild<UISprite>("SpriteStone");
        this.LabelStoneNum = base.FindChild<UILabel>("LabelStoneNum");
        this.PanelBlackEff = base.FindChild<UIPanel>("PanelBlackEff");
        this.TableFlagGrid.InitFromGrid(this.FlagGrid);
        this.TableTeamGrid.InitFromGrid(this.TeamGrid);
    }

    private void InviteFriend()
    {
        Debug.LogWarning("InviteFriend!");
        if (XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            if (!this.isInviteFriend__)
            {
                if (<>f__am$cache2C == null)
                {
                    <>f__am$cache2C = ui => (ui as DetainsDartInviteFriendPanel).SetFriendInfo(XSingleton<GameDetainsDartMgr>.Singleton.curPageFriendInfo, true);
                }
                GUIMgr.Instance.PushGUIEntity<DetainsDartInviteFriendPanel>(<>f__am$cache2C);
            }
            this.isInviteFriend__ = true;
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic.Count > XSingleton<GameDetainsDartMgr>.Singleton.CurFriendPageId)
        {
            XSingleton<GameDetainsDartMgr>.Singleton.CurReqPageFriendIdList = XSingleton<GameDetainsDartMgr>.Singleton.pageIdToPageFriendIdDic[XSingleton<GameDetainsDartMgr>.Singleton.CurFriendPageId];
        }
    }

    private void NextTep()
    {
        Debug.LogWarning("Next!");
        if (XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Count > 0)
            {
                this.isSelTeam__ = true;
            }
            else
            {
                this.isSelTeam__ = false;
            }
            if (this.isSelTeam__)
            {
                if (<>f__am$cache2D == null)
                {
                    <>f__am$cache2D = delegate (GUIEntity ui) {
                        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Selet;
                        (ui as DetainsDartOnDoingPanel).SetDataInfo();
                    };
                }
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache2D);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186d9));
            }
        }
        else
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Count > 0)
            {
                this.isSelTeam__ = true;
            }
            else
            {
                this.isSelTeam__ = false;
            }
            if (this.isSelTeam__)
            {
                if (<>f__am$cache2E == null)
                {
                    <>f__am$cache2E = delegate (GUIEntity ui) {
                        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Selet;
                        (ui as DetainsDartOnDoingPanel).SetDataInfo();
                    };
                }
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache2E);
            }
            else
            {
                TipsDiag.SetText("请选择护送旗帜的队伍！");
            }
        }
    }

    private void OnClickReFreshFlags()
    {
        Debug.LogWarning("OnClickReFreshFlags!");
        if (XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex == 3)
        {
            string flagName = string.Empty;
            convoy_flag_config _config = ConfigMgr.getInstance().getByEntry<convoy_flag_config>(XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex);
            if (_config != null)
            {
                flagName = _config.flagName;
            }
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x186da), flagName));
        }
        else
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                int id = this.GetBuyFalgNeedStoneCnt();
                buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(id);
                if (_config != null)
                {
                    this.curReFreshFlagNeed__ = _config.refresh_convoy_flag_cost_stone;
                }
                MessageBox box = (MessageBox) obj;
                string str = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x186db), GameConstant.DefaultTextRedColor + this.curReFreshFlagNeed__, GameConstant.DefaultTextColor);
                if (<>f__am$cache2F == null)
                {
                    <>f__am$cache2F = delegate (GameObject box) {
                    };
                }
                box.SetDialog(str, box => this.ConfirmReFreshFlags(), <>f__am$cache2F, false);
            }, null);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.PlayFlagFreshAnim(XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex);
    }

    public void PlayAnim()
    {
        this.AnimInitDataInfo();
        this.AnimIsPlay = true;
    }

    private void PlayFlagFreshAnim(int willStayFlagIndex)
    {
        if (this.AnimIsPlay)
        {
            if (!this.PanelBlackEff.gameObject.activeSelf)
            {
                this.PanelBlackEff.gameObject.SetActive(true);
            }
            if (this.AnimCurMoveCnt > (this.AnimOneWholeRoundCnt * this.AnimMaxMoveWholeCnt))
            {
                this.AnimIsLastQuan = true;
                if (this.AnimIsLastQuan)
                {
                    if (this.AnimCurFlagIndex == willStayFlagIndex)
                    {
                        this.FlagAnimFlagSelStateInit();
                        this.TableFlagGrid[willStayFlagIndex].Model.Template.SpriteSelectState.gameObject.SetActive(true);
                        this.AnimIsPlay = false;
                    }
                }
                else
                {
                    this.AnimTime = 0f;
                }
            }
            this.AnimTime += Time.deltaTime;
            if (this.AnimTime >= this.AnimSpeed)
            {
                if (this.AnimbDirection)
                {
                    if (this.AnimCurFlagIndex < (this.AnimFlagMaxCnt - 1))
                    {
                        this.AnimCurFlagIndex++;
                        this.AnimCurMoveCnt++;
                        if (this.AnimCurFlagIndex == (this.AnimFlagMaxCnt - 1))
                        {
                            this.AnimbDirection = false;
                            this.SetCurSpeed();
                        }
                    }
                }
                else if (this.AnimCurFlagIndex > 0)
                {
                    this.AnimCurFlagIndex--;
                    this.AnimCurMoveCnt++;
                    if (this.AnimCurFlagIndex == 0)
                    {
                        this.AnimbDirection = true;
                        this.SetCurSpeed();
                    }
                }
                this.AnimTime = 0f;
                this.FlagAnimFlagSelStateInit();
                this.TableFlagGrid[this.AnimCurFlagIndex].Model.Template.SpriteSelectState.gameObject.SetActive(true);
            }
        }
        else if (this.PanelBlackEff.gameObject.activeSelf)
        {
            this.PanelBlackEff.gameObject.SetActive(false);
        }
    }

    private void RefreshFlagInfo()
    {
        Debug.LogWarning("RefreshFlagInfo!");
        this.FlagGrid.cellWidth = 220f;
        this.FlagGrid.cellHeight = 250f;
        this.TableFlagGrid.Count = this.maxFlagCnt__;
        for (int i = 0; i < this.maxFlagCnt__; i++)
        {
            convoy_flag_config _config = ConfigMgr.getInstance().getByEntry<convoy_flag_config>(i);
            if (_config != null)
            {
                int num2 = i;
                string flagName = string.Empty;
                convoy_flag_config _config2 = ConfigMgr.getInstance().getByEntry<convoy_flag_config>(i);
                if (_config2 != null)
                {
                    flagName = _config2.flagName;
                }
                this.TableFlagGrid[i].Model.Template.LabelFlagName.text = flagName;
                this.TableFlagGrid[i].Model.Template.LabelCourageNum.text = _config.arena_score_award.ToString();
                this.TableFlagGrid[i].Model.Template.LabelGoldNum.text = _config.gold_award.ToString();
                this.TableFlagGrid[i].Model.Template.SpriteSelectState.gameObject.SetActive(false);
                this.TableFlagGrid[i].Model.Template.SpriteFlagIcon.spriteName = "Ui_Detains_Icon_flag" + (i + 1);
            }
        }
        this.SelectFlag(XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex);
    }

    private void ReFreshTeamInfo()
    {
        Debug.LogWarning("ReFreshTeamInfo!");
        this.TeamGrid.cellWidth = 120f;
        this.TeamGrid.cellHeight = 100f;
        this.TableTeamGrid.Count = this.maxOneTeamCnt__;
        for (int i = 0; i < this.maxOneTeamCnt__; i++)
        {
            this.TableTeamGrid[i].Model.Template.Btn_Add.OnUIMouseClick(u => this.selectTeam());
            this.TableTeamGrid[i].Model.Template.Btn_Add.gameObject.SetActive(true);
            this.TableTeamGrid[i].Model.Template.SpriteAdd.gameObject.SetActive(true);
            this.TableTeamGrid[i].Model.Template.CardInfo.gameObject.SetActive(false);
        }
    }

    public void SelectFlag(int index)
    {
        this.initFalgSelState();
        Debug.LogWarning("SelectFlag:_" + index);
        this.curSelFlagIndex__ = index;
        if (index < this.TableFlagGrid.Count)
        {
            this.TableFlagGrid[index].Model.Template.SpriteSelectState.gameObject.SetActive(true);
        }
    }

    private void SelectHeroForFresh()
    {
        this.SetTeamHeadInfo();
    }

    private void selectTeam()
    {
        Debug.LogWarning("selectTeam!");
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity entity) {
            <selectTeam>c__AnonStorey1B9 storeyb = new <selectTeam>c__AnonStorey1B9 {
                <>f__this = this,
                ctp = (SelectHeroPanel) entity
            };
            storeyb.ctp.Depth = 600;
            storeyb.ctp.mBattleType = BattleType.DetainsDartEscort;
            storeyb.ctp.SetButtonState(BattleType.DetainsDartEscort);
            storeyb.ctp.SetZhuZhanStat(false);
            storeyb.ctp.SaveButtonText = ConfigMgr.getInstance().GetWord(0x186dd);
            if (XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo.cards.Count > 0)
            {
                storeyb.ctp.DetainsDartHeroCardIDList = XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo.cards;
            }
            else
            {
                storeyb.ctp.DetainsDartHeroEntryList = new List<int>();
            }
            storeyb.ctp.OnSaved = new Action<List<long>>(storeyb.<>m__288);
        }, null);
        this.isSelTeam__ = true;
    }

    private void SetCurSpeed()
    {
        switch ((this.AnimCurMoveCnt / this.AnimOneWholeRoundCnt))
        {
            case 0:
                this.AnimSpeed = Time.deltaTime * 2f;
                break;

            case 1:
                this.AnimSpeed = Time.deltaTime * 3f;
                break;

            case 2:
                this.AnimSpeed = Time.deltaTime * 3f;
                break;

            case 3:
                this.AnimSpeed = Time.deltaTime * 2f;
                break;

            case 4:
                this.AnimSpeed = Time.deltaTime * 2f;
                break;

            default:
                this.AnimSpeed = Time.deltaTime * 2f;
                break;
        }
    }

    public void SetFreshFlagNeed(int freshTimes)
    {
        int buyFalgNeedStoneCnt = this.GetBuyFalgNeedStoneCnt();
        buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(buyFalgNeedStoneCnt);
        if (_config != null)
        {
            this.LabelStoneNum.text = _config.refresh_convoy_flag_cost_stone.ToString();
        }
    }

    public void SetInviteFriendDataInfo(int friendMainCardEntry = 0, int quality = 0)
    {
        this.curInviteFriendMainCardEntry__ = friendMainCardEntry;
        this.Btn_Invite.gameObject.SetActive(!this.IsIniviteFriend);
        this.InviteFriendInfo.gameObject.SetActive(this.IsIniviteFriend);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(this.curInviteFriendMainCardEntry__);
        if (_config != null)
        {
            this.TextureFriendHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        CommonFunc.SetQualityBorder(this.SpriteFriendHeadQuality, quality);
    }

    private void SetTeamHeadInfo()
    {
        Debug.LogWarning("SetTeamHeadInfo!");
        this.TableTeamGrid.Count = this.maxOneTeamCnt__;
        for (int i = 0; i < this.maxOneTeamCnt__; i++)
        {
            this.TableTeamGrid[i].Model.Template.Btn_Add.gameObject.SetActive(true);
            this.TableTeamGrid[i].Model.Template.SpriteAdd.gameObject.SetActive(true);
            this.TableTeamGrid[i].Model.Template.CardInfo.gameObject.SetActive(false);
        }
        for (int j = 0; j < this.TableTeamGrid.Count; j++)
        {
            if ((XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex >= 0) && (XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo.Count > 0))
            {
                Debug.LogWarning("GameDetainsDartMgr.Singleton.curSelTeamIndex:" + XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex);
                Debug.LogWarning("GameDetainsDartMgr.Singleton.TeamIdToTeamCardsInfo.Count:" + XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo.Count);
                if (j < XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo[XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex].Count)
                {
                    this.ShowCard(j, XSingleton<GameDetainsDartMgr>.Singleton.TeamIdToTeamCardsInfo[XSingleton<GameDetainsDartMgr>.Singleton.curSelTeamIndex][j], 0);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186de));
                }
            }
        }
    }

    public void SetUIDataInfo()
    {
        XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Clear();
        this.getPageIdToFriendIdListDicInfo();
        this.RefreshFlagInfo();
        this.SetFreshFlagNeed(XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt);
        this.ReFreshTeamInfo();
        this.isInviteFriend__ = false;
        this.isSelTeam__ = false;
        this.SetInviteFriendDataInfo(0, 0);
    }

    public void ShowCard(int index, int entry, int starLv = 0)
    {
        this.isSelTeam__ = true;
        this.TableTeamGrid[index].Model.Template.Btn_Add.gameObject.SetActive(false);
        this.TableTeamGrid[index].Model.Template.SpriteAdd.gameObject.SetActive(false);
        this.TableTeamGrid[index].Model.Template.CardInfo.gameObject.SetActive(true);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(entry);
        if (_config != null)
        {
            this.TableTeamGrid[index].Model.Template.TextureHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.TableTeamGrid[index].Model.StarCnt = starLv;
        }
        Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) entry);
        if (cardByEntry != null)
        {
            CommonFunc.SetQualityBorder(this.TableTeamGrid[index].Model.Template.SpriteQuality, cardByEntry.cardInfo.quality);
            this.TableTeamGrid[index].Model.StarCnt = cardByEntry.cardInfo.starLv;
        }
    }

    protected UISprite AutoClose { get; set; }

    protected UIButton Btn_Close { get; set; }

    protected UISprite Btn_Invite { get; set; }

    protected UIButton Btn_Next { get; set; }

    protected UISprite Btn_RefreshFlags { get; set; }

    protected UIGrid FlagGrid { get; set; }

    protected UIPanel FlagsList { get; set; }

    protected Transform GoBottomInfo { get; set; }

    protected Transform GoLeftBottom { get; set; }

    protected Transform GoRightBottom { get; set; }

    protected Transform InviteFriendInfo { get; set; }

    public int InviteFriendMainCardEntry
    {
        get
        {
            return this.curInviteFriendMainCardEntry__;
        }
        set
        {
            this.curInviteFriendMainCardEntry__ = value;
        }
    }

    public bool IsIniviteFriend
    {
        get
        {
            return this.isInviteFriend__;
        }
        set
        {
            this.isInviteFriend__ = value;
        }
    }

    public bool IsSelTeam
    {
        get
        {
            return this.isSelTeam__;
        }
        set
        {
            this.isSelTeam__ = value;
        }
    }

    protected UILabel LabelRefresh { get; set; }

    protected UILabel LabelStoneNum { get; set; }

    protected UILabel LabelTeamTitle { get; set; }

    protected UILabel LabelTitle { get; set; }

    protected UIPanel PanelBlackEff { get; set; }

    protected UISprite SpriteFriendHeadQuality { get; set; }

    protected UISprite SpriteStone { get; set; }

    protected UIGrid TeamGrid { get; set; }

    protected UIPanel TeamList { get; set; }

    protected UITexture TextureFriendHeadIcon { get; set; }

    [CompilerGenerated]
    private sealed class <selectTeam>c__AnonStorey1B9
    {
        internal DetainsDartSelFlagAndTeamPanel <>f__this;
        internal SelectHeroPanel ctp;

        internal void <>m__288(List<long> list)
        {
            XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList = list;
            XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortInfo.cards = this.ctp.DetainsDartHeroCardIDList;
            this.<>f__this.SelectHeroForFresh();
        }
    }

    public class FlagGridItemModel : TableItemModel<DetainsDartSelFlagAndTeamPanel.FlagGridItemTemplate>
    {
        public override void Init(DetainsDartSelFlagAndTeamPanel.FlagGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class FlagGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ItemFlag = base.FindChild<Transform>("ItemFlag");
            this.SpriteFlagBg = base.FindChild<UISprite>("SpriteFlagBg");
            this.LabelFlagName = base.FindChild<UILabel>("LabelFlagName");
            this.SpriteFlagIcon = base.FindChild<UISprite>("SpriteFlagIcon");
            this.GoRewardInfo = base.FindChild<Transform>("GoRewardInfo");
            this.LabelGoldNum = base.FindChild<UILabel>("LabelGoldNum");
            this.LabelCourageNum = base.FindChild<UILabel>("LabelCourageNum");
            this.SpriteSelectState = base.FindChild<UISprite>("SpriteSelectState");
        }

        public Transform GoRewardInfo { get; private set; }

        public Transform ItemFlag { get; private set; }

        public UILabel LabelCourageNum { get; private set; }

        public UILabel LabelFlagName { get; private set; }

        public UILabel LabelGoldNum { get; private set; }

        public UISprite SpriteFlagBg { get; private set; }

        public UISprite SpriteFlagIcon { get; private set; }

        public UISprite SpriteSelectState { get; private set; }
    }

    public class TeamGridItemModel : TableItemModel<DetainsDartSelFlagAndTeamPanel.TeamGridItemTemplate>
    {
        public UITableManager<UITeamCardItem> StarTable = new UITableManager<UITeamCardItem>();

        public override void Init(DetainsDartSelFlagAndTeamPanel.TeamGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
            this.StarTable.InitFromGrid(base.Template.StarGrid.GetComponent<UIGrid>());
        }

        public int StarCnt
        {
            get
            {
                return this.StarTable.Count;
            }
            set
            {
                this.StarTable.Count = value;
                base.Template.CardLvInfo.transform.localPosition = new Vector3((float) (10 + (value * -11)), -30f, 0f);
                Vector3 localPosition = base.Template.CardLvInfo.transform.localPosition;
            }
        }

        public class UITeamCardItem : UITableItem
        {
            public override void OnCreate()
            {
                this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            }

            public UIGrid StarGrid { get; private set; }
        }
    }

    public class TeamGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.TeamItem = base.FindChild<Transform>("TeamItem");
            this.Btn_Add = base.FindChild<UISprite>("Btn_Add");
            this.CardInfo = base.FindChild<Transform>("CardInfo");
            this.SpriteQuality = base.FindChild<UISprite>("SpriteQuality");
            this.TextureHeadIcon = base.FindChild<UITexture>("TextureHeadIcon");
            this.CardLvInfo = base.FindChild<Transform>("CardLvInfo");
            this.StarGrid = base.FindChild<UIGrid>("StarGrid");
            this.SpriteStar = base.FindChild<UISprite>("SpriteStar");
            this.SpriteAdd = base.FindChild<UISprite>("SpriteAdd");
        }

        public UISprite Btn_Add { get; private set; }

        public Transform CardInfo { get; private set; }

        public Transform CardLvInfo { get; private set; }

        public UISprite SpriteAdd { get; private set; }

        public UISprite SpriteQuality { get; private set; }

        public UISprite SpriteStar { get; private set; }

        public UIGrid StarGrid { get; private set; }

        public Transform TeamItem { get; private set; }

        public UITexture TextureHeadIcon { get; private set; }
    }
}

