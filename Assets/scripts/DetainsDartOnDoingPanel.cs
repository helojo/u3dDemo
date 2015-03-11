using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class DetainsDartOnDoingPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2A;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2B;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2C;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2D;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2E;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache2F;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache30;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache31;
    [CompilerGenerated]
    private static Action<object> <>f__am$cache32;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache33;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache34;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache35;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache36;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache37;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache38;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache39;
    private int buyInterceptNeedStone;
    private float curLeftTime__;
    private RoadState curSelRoadState = RoadState.None;
    private int EscortTime__;
    private int freshPlayerCnt__ = 3;
    private GameObject GoSelRoad;
    private int InterceptTime__ = 300;
    private bool IsEscortTimeStart = true;
    private bool IsInterceptTimeStart = true;
    protected UITableManager<UIAutoGenItem<PlayerTableItemTemplate, PlayerTableItemModel>> TablePlayerTable = new UITableManager<UIAutoGenItem<PlayerTableItemTemplate, PlayerTableItemModel>>();

    private void CloseEffectObj()
    {
        if (this.GoSelRoad != null)
        {
            UnityEngine.Object.Destroy(this.GoSelRoad);
        }
        if ((this.SpriteSelRoadTitleTips != null) && this.SpriteSelRoadTitleTips.gameObject.activeSelf)
        {
            this.SpriteSelRoadTitleTips.gameObject.SetActive(false);
        }
    }

    private void Colse()
    {
        GUIMgr.Instance.ClearExceptMainUI();
    }

    private void ConfirmBuyInterceptTime()
    {
        SocketMgr.Instance.RequestC2S_BuyConvoyRobTimes();
    }

    private void ConfirmReFreshInterceptTeams()
    {
        SocketMgr.Instance.RequestC2S_RefreshConvoyTarget();
    }

    private bool GetTimeStateIsLastState(ConvoyInfo convoyInfo)
    {
        int num = convoyInfo.beginTime + (convoyInfo.duration / 2);
        return (TimeMgr.Instance.ServerStampTime >= num);
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_BattleRecord.OnUIMouseClick(u => this.ShowBattleRecordInfo());
        this.Btn_TurnLeft.OnUIMouseClick(u => this.TurnLeft());
        this.Btn_TurnRight.OnUIMouseClick(u => this.TurnRight());
        this.CloseButton.OnUIMouseClick(u => this.Colse());
        this.Btn_StartEscort.OnUIMouseClick(u => this.StartEscort());
        this.Btn_Rule.OnUIMouseClick(u => this.ShowRuleInfo());
        this.Btn_ReScan.OnUIMouseClick(u => this.OnClickReScan());
        this.GoldBox.OnUIMouseClick(u => this.OnClickReward());
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
            SocketMgr.Instance.RequestC2S_GetConvoyInfo();
        }
        this.RewardInfo.gameObject.SetActive(true);
        this.InitRoadInfo();
        this.InitUIInfo();
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.Arrows = base.FindChild<UISprite>("Arrows");
        this.Btn_TurnRight = base.FindChild<UISprite>("Btn_TurnRight");
        this.Btn_TurnLeft = base.FindChild<UISprite>("Btn_TurnLeft");
        this.CloseButton = base.FindChild<UIButton>("CloseButton");
        this.Btn_BattleRecord = base.FindChild<UIButton>("Btn_BattleRecord");
        this.NewRecordTips = base.FindChild<UISprite>("NewRecordTips");
        this.GoLeftTtimeInfo = base.FindChild<Transform>("GoLeftTtimeInfo");
        this.LabelTime = base.FindChild<UILabel>("LabelTime");
        this.Btn_StartEscort = base.FindChild<UIButton>("Btn_StartEscort");
        this.Btn_Rule = base.FindChild<UIButton>("Btn_Rule");
        this.GoScanInfo = base.FindChild<Transform>("GoScanInfo");
        this.Btn_ReScan = base.FindChild<UIButton>("Btn_ReScan");
        this.LabelScanNeed = base.FindChild<UILabel>("LabelScanNeed");
        this.LabelScanNum = base.FindChild<UILabel>("LabelScanNum");
        this.GoInterceptTimeInfo = base.FindChild<Transform>("GoInterceptTimeInfo");
        this.LabelInterceptTime = base.FindChild<UILabel>("LabelInterceptTime");
        this.AutoClose = base.FindChild<UISprite>("AutoClose");
        this.MapBg = base.FindChild<UITexture>("MapBg");
        this.GoMapObjsInfo = base.FindChild<Transform>("GoMapObjsInfo");
        this.SpriteBuff_Left = base.FindChild<UISprite>("SpriteBuff_Left");
        this.SpriteBuffSelectLeft = base.FindChild<UISprite>("SpriteBuffSelectLeft");
        this.SpriteBuff_Mid = base.FindChild<UISprite>("SpriteBuff_Mid");
        this.SpriteBuffSelectMid = base.FindChild<UISprite>("SpriteBuffSelectMid");
        this.SpriteBuff_Right = base.FindChild<UISprite>("SpriteBuff_Right");
        this.SpriteBuffSelectRight = base.FindChild<UISprite>("SpriteBuffSelectRight");
        this.GoPlayCardsInfo = base.FindChild<Transform>("GoPlayCardsInfo");
        this.PlayerTable = base.FindChild<UIGrid>("PlayerTable");
        this.LabelTitle = base.FindChild<UILabel>("LabelTitle");
        this.RewardInfo = base.FindChild<Transform>("RewardInfo");
        this.Bnt_Rewad = base.FindChild<UISprite>("Bnt_Rewad");
        this.GoldBox = base.FindChild<UIWidget>("GoldBox");
        this.SpriteSelRoadTitleTips = base.FindChild<UISprite>("SpriteSelRoadTitleTips");
        this.TablePlayerTable.InitFromGrid(this.PlayerTable);
    }

    private void InitRoadInfo()
    {
        this.SpriteBuffSelectLeft.gameObject.SetActive(false);
        this.SpriteBuffSelectMid.gameObject.SetActive(false);
        this.SpriteBuffSelectRight.gameObject.SetActive(false);
    }

    private void InitUIInfo()
    {
        this.InitRoadInfo();
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
        {
            this.Btn_TurnRight.gameObject.SetActive(true);
            switch (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState)
            {
                case GameDetainsDartMgr.EscortState.Selet:
                    this.Btn_Rule.gameObject.SetActive(true);
                    this.Btn_StartEscort.gameObject.SetActive(true);
                    this.Btn_BattleRecord.gameObject.SetActive(false);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(false);
                    this.GoPlayCardsInfo.gameObject.SetActive(false);
                    this.GoScanInfo.gameObject.SetActive(false);
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.RewardInfo.gameObject.SetActive(false);
                    this.SpriteBuff_Left.OnUIMouseClick(u => this.OnSelRoad_Left());
                    this.SpriteBuff_Mid.OnUIMouseClick(u => this.OnSelRoad_Mid());
                    this.SpriteBuff_Right.OnUIMouseClick(u => this.OnSelRoad_Right());
                    if (this.GoSelRoad != null)
                    {
                        this.GoSelRoad.gameObject.SetActive(true);
                    }
                    this.SpriteSelRoadTitleTips.gameObject.SetActive(true);
                    this.Arrows.gameObject.SetActive(false);
                    this.Btn_TurnLeft.gameObject.SetActive(false);
                    this.Btn_TurnRight.gameObject.SetActive(false);
                    break;

                case GameDetainsDartMgr.EscortState.Doing:
                    this.Btn_Rule.gameObject.SetActive(false);
                    this.Btn_StartEscort.gameObject.SetActive(false);
                    this.Btn_BattleRecord.gameObject.SetActive(true);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(true);
                    this.GoPlayCardsInfo.gameObject.SetActive(true);
                    this.GoScanInfo.gameObject.SetActive(false);
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.RewardInfo.gameObject.SetActive(false);
                    if (<>f__am$cache2A == null)
                    {
                        <>f__am$cache2A = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Left.OnUIMouseClick(<>f__am$cache2A);
                    if (<>f__am$cache2B == null)
                    {
                        <>f__am$cache2B = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Mid.OnUIMouseClick(<>f__am$cache2B);
                    if (<>f__am$cache2C == null)
                    {
                        <>f__am$cache2C = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Right.OnUIMouseClick(<>f__am$cache2C);
                    if (this.GoSelRoad != null)
                    {
                        this.GoSelRoad.gameObject.SetActive(false);
                    }
                    this.SpriteSelRoadTitleTips.gameObject.SetActive(false);
                    this.Arrows.gameObject.SetActive(false);
                    if ((XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage + 1) < XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count)
                    {
                        this.Btn_TurnLeft.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.Btn_TurnLeft.gameObject.SetActive(false);
                    }
                    break;

                case GameDetainsDartMgr.EscortState.Done:
                    this.Btn_Rule.gameObject.SetActive(false);
                    this.Btn_StartEscort.gameObject.SetActive(false);
                    this.Btn_BattleRecord.gameObject.SetActive(true);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(false);
                    this.GoPlayCardsInfo.gameObject.SetActive(false);
                    this.GoScanInfo.gameObject.SetActive(false);
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.RewardInfo.gameObject.SetActive(true);
                    if (<>f__am$cache2D == null)
                    {
                        <>f__am$cache2D = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Left.OnUIMouseClick(<>f__am$cache2D);
                    if (<>f__am$cache2E == null)
                    {
                        <>f__am$cache2E = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Mid.OnUIMouseClick(<>f__am$cache2E);
                    if (<>f__am$cache2F == null)
                    {
                        <>f__am$cache2F = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Right.OnUIMouseClick(<>f__am$cache2F);
                    if (this.GoSelRoad != null)
                    {
                        this.GoSelRoad.gameObject.SetActive(false);
                    }
                    this.SpriteSelRoadTitleTips.gameObject.SetActive(false);
                    this.Arrows.gameObject.SetActive(false);
                    this.Btn_TurnLeft.gameObject.SetActive(true);
                    break;

                case GameDetainsDartMgr.EscortState.GettingReward:
                    this.Btn_Rule.gameObject.SetActive(false);
                    this.Btn_StartEscort.gameObject.SetActive(false);
                    this.Btn_BattleRecord.gameObject.SetActive(true);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(false);
                    this.GoPlayCardsInfo.gameObject.SetActive(false);
                    this.GoScanInfo.gameObject.SetActive(false);
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.RewardInfo.gameObject.SetActive(false);
                    if (<>f__am$cache30 == null)
                    {
                        <>f__am$cache30 = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Left.OnUIMouseClick(<>f__am$cache30);
                    if (<>f__am$cache31 == null)
                    {
                        <>f__am$cache31 = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Mid.OnUIMouseClick(<>f__am$cache31);
                    if (<>f__am$cache32 == null)
                    {
                        <>f__am$cache32 = delegate (object u) {
                        };
                    }
                    this.SpriteBuff_Right.OnUIMouseClick(<>f__am$cache32);
                    if (this.GoSelRoad != null)
                    {
                        this.GoSelRoad.gameObject.SetActive(false);
                    }
                    this.SpriteSelRoadTitleTips.gameObject.SetActive(false);
                    this.Arrows.gameObject.SetActive(false);
                    this.Btn_TurnLeft.gameObject.SetActive(true);
                    break;
            }
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
            this.Btn_TurnRight.gameObject.SetActive(false);
            this.Btn_TurnLeft.gameObject.SetActive(true);
            if (XSingleton<GameDetainsDartMgr>.Singleton.InterceptListInfo.Count > 0)
            {
                this.Arrows.gameObject.SetActive(false);
            }
            else
            {
                this.Arrows.gameObject.SetActive(true);
            }
            if (this.GoSelRoad != null)
            {
                this.GoSelRoad.gameObject.SetActive(false);
            }
            this.SpriteSelRoadTitleTips.gameObject.SetActive(false);
            switch (XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState)
            {
                case GameDetainsDartMgr.InterceptState.SeletPlayer:
                    this.Btn_Rule.gameObject.SetActive(true);
                    this.Btn_StartEscort.gameObject.SetActive(false);
                    this.Btn_BattleRecord.gameObject.SetActive(false);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(false);
                    this.GoPlayCardsInfo.gameObject.SetActive(false);
                    this.GoScanInfo.gameObject.SetActive(true);
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.RewardInfo.gameObject.SetActive(false);
                    break;

                case GameDetainsDartMgr.InterceptState.Battle:
                    this.Btn_Rule.gameObject.SetActive(false);
                    this.Btn_StartEscort.gameObject.SetActive(false);
                    this.Btn_BattleRecord.gameObject.SetActive(false);
                    this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
                    this.GoLeftTtimeInfo.gameObject.SetActive(false);
                    this.GoPlayCardsInfo.gameObject.SetActive(true);
                    this.GoScanInfo.gameObject.SetActive(true);
                    this.GoInterceptTimeInfo.gameObject.SetActive(true);
                    this.RewardInfo.gameObject.SetActive(false);
                    break;
            }
            buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(XSingleton<GameDetainsDartMgr>.Singleton.buyInterceptTimes);
            if (_config != null)
            {
                this.buyInterceptNeedStone = _config.buy_convoy_rob_cost_stone;
            }
            int num = 0;
            variable_config _config2 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
            if (_config2 != null)
            {
                num = (_config2.convoy_reftarget_incgold * XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt) + _config2.convoy_reftarget_base_gold;
                if (num >= _config2.convoy_reftarget_maxgold)
                {
                    num = _config2.convoy_reftarget_maxgold;
                }
            }
            this.LabelScanNeed.text = string.Format("{0}", num);
            this.LabelScanNum.text = string.Format("{0}/{1}", XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes, XSingleton<GameDetainsDartMgr>.Singleton.maxInterceptCnt);
        }
        this.SetRewardInfo();
    }

    private bool JudgeIsEscortDone(ConvoyInfo escortInfo)
    {
        int num = escortInfo.beginTime + escortInfo.duration;
        return (num >= TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime));
    }

    private bool JudgeIsLastState(ConvoyRobTarget convoyRobTarget)
    {
        int num2 = 0;
        int lineId = convoyRobTarget.lineId;
        if (convoyRobTarget.type == ConvoyRobTargetType.ConvoyRobTarget_Robot)
        {
            return false;
        }
        convoy_line_config _config = ConfigMgr.getInstance().getByEntry<convoy_line_config>(convoyRobTarget.lineId);
        if (_config != null)
        {
            num2 = _config.half_time;
        }
        int num4 = convoyRobTarget.beginTime + num2;
        return (TimeMgr.Instance.ServerStampTime >= num4);
    }

    private void OnClickPlayer(int index)
    {
        Debug.LogWarning("OnClickPlayer :__" + index);
    }

    private void OnClickPlayer(int index, ConvoyRobTarget info)
    {
        <OnClickPlayer>c__AnonStorey1B8 storeyb = new <OnClickPlayer>c__AnonStorey1B8 {
            info = info
        };
        Debug.LogWarning("OnClickPlayer :__" + index);
        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo = new ConvoyRobTarget();
        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.targetId = storeyb.info.targetId;
        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.convoyIndex = storeyb.info.convoyIndex;
        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTargetInfo.type = storeyb.info.type;
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
            IEnumerator<UIAutoGenItem<PlayerTableItemTemplate, PlayerTableItemModel>> enumerator = this.TablePlayerTable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    UIAutoGenItem<PlayerTableItemTemplate, PlayerTableItemModel> current = enumerator.Current;
                    current.Model.Template.SpriteSelPlayer.gameObject.SetActive(false);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            this.TablePlayerTable[index].Model.Template.SpriteSelPlayer.gameObject.SetActive(true);
            GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storeyb.<>m__277), base.gameObject);
        }
    }

    private void OnClickReScan()
    {
        <OnClickReScan>c__AnonStorey1B5 storeyb = new <OnClickReScan>c__AnonStorey1B5 {
            <>f__this = this
        };
        if ((this.Arrows != null) && this.Arrows.gameObject.activeSelf)
        {
            this.Arrows.gameObject.SetActive(false);
        }
        storeyb.xxTipsText = string.Empty;
        if (XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes <= 0)
        {
            int num = 0;
            int num2 = 0;
            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
            if (_config != null)
            {
                num = _config.buy_convoy_rob_times;
                num2 = _config.buy_convoy_rob_obtaintimes;
            }
            if (num <= 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x186e1), num));
            }
            else
            {
                buy_cost_config _config2 = ConfigMgr.getInstance().getByEntry<buy_cost_config>(XSingleton<GameDetainsDartMgr>.Singleton.buyInterceptTimes);
                if (_config2 != null)
                {
                    this.buyInterceptNeedStone = _config2.buy_convoy_rob_cost_stone;
                }
                object[] objArray1 = new object[] { GameConstant.DefaultTextRedColor, this.buyInterceptNeedStone, ConfigMgr.getInstance().GetWord(0x31b), GameConstant.DefaultTextColor };
                storeyb.xxTipsText = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x186df), string.Concat(objArray1), num2);
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyb.<>m__260), null);
            }
        }
        else
        {
            int num3 = 0;
            variable_config _config3 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
            if (_config3 != null)
            {
                num3 = (_config3.convoy_reftarget_incgold * XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt) + _config3.convoy_reftarget_base_gold;
                if (num3 >= _config3.convoy_reftarget_maxgold)
                {
                    num3 = _config3.convoy_reftarget_maxgold;
                }
            }
            storeyb.xxTipsText = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x186e0), GameConstant.DefaultTextRedColor + num3 + GameConstant.DefaultTextColor, ConfigMgr.getInstance().GetWord(0x89));
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeyb.<>m__261), null);
        }
    }

    private void OnClickReward()
    {
        if ((XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage >= 0) && (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage < XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count))
        {
            SocketMgr.Instance.RequestC2S_ConvoyEnd(XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage].index);
        }
    }

    private void OnPressOver(bool isDown, object objectTemp)
    {
        Debug.LogWarning("OnPressOver!");
        string str = string.Empty;
        if (isDown)
        {
            str = "Ui_Info_Btn_landown";
        }
        else
        {
            str = "Ui_Info_Btn_lan";
        }
        UISprite sprite = (UISprite) objectTemp;
        sprite.spriteName = str;
    }

    private void OnSelRoad_Left()
    {
        this.curSelRoadState = RoadState.Left;
        this.SelectRoad();
    }

    private void OnSelRoad_Mid()
    {
        this.curSelRoadState = RoadState.Mid;
        this.SelectRoad();
    }

    private void OnSelRoad_Right()
    {
        this.curSelRoadState = RoadState.Right;
        this.SelectRoad();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if ((XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept) && (XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState == GameDetainsDartMgr.InterceptState.Battle))
        {
            if (this.IsInterceptTimeStart)
            {
                if ((this.InterceptTime__ - TimeMgr.Instance.ServerStampTime) > 0)
                {
                    if (XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime > TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime))
                    {
                        this.LabelInterceptTime.text = string.Format("{0}后敌人消失", TimeMgr.Instance.GetRemainTime2(this.InterceptTime__));
                        if (!this.LabelInterceptTime.gameObject.activeSelf)
                        {
                            this.LabelInterceptTime.gameObject.SetActive(true);
                        }
                    }
                    else if (this.GoInterceptTimeInfo.gameObject.activeSelf)
                    {
                        this.LabelInterceptTime.text = string.Empty;
                        this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    }
                }
                else if (this.GoInterceptTimeInfo.gameObject.activeSelf)
                {
                    this.LabelInterceptTime.text = string.Empty;
                    this.GoInterceptTimeInfo.gameObject.SetActive(false);
                    this.InterceptTime__ = 0;
                    SocketMgr.Instance.RequestC2S_GetRobTargetList();
                }
            }
            else
            {
                this.GoInterceptTimeInfo.gameObject.SetActive(false);
            }
        }
        if ((XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort) && (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Doing))
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0)
            {
                this.IsEscortTimeStart = true;
            }
            if (this.IsEscortTimeStart)
            {
                if ((XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage) && (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage >= 0))
                {
                    if ((this.EscortTime__ - TimeMgr.Instance.ServerStampTime) > 0)
                    {
                        this.LabelTime.text = string.Format("剩余时间{0}", TimeMgr.Instance.GetRemainTime2(this.EscortTime__));
                        if (!this.LabelTime.gameObject.activeSelf)
                        {
                            this.LabelTime.gameObject.SetActive(true);
                        }
                    }
                    else if (this.LabelTime.gameObject.activeSelf)
                    {
                        this.LabelTime.text = string.Empty;
                        this.LabelTime.gameObject.SetActive(false);
                        SocketMgr.Instance.RequestC2S_GetConvoyInfo();
                    }
                }
            }
            else if (this.LabelTime.gameObject.activeSelf)
            {
                this.LabelTime.text = string.Empty;
                this.LabelTime.gameObject.SetActive(false);
            }
        }
    }

    private void SelectRoad()
    {
        this.InitRoadInfo();
        switch (this.curSelRoadState)
        {
            case RoadState.Left:
                this.SpriteBuffSelectLeft.gameObject.SetActive(true);
                break;

            case RoadState.Mid:
                this.SpriteBuffSelectMid.gameObject.SetActive(true);
                break;

            case RoadState.Right:
                this.SpriteBuffSelectRight.gameObject.SetActive(true);
                break;
        }
        this.CloseEffectObj();
    }

    public void SetDataInfo()
    {
        this.InitUIInfo();
        this.SetPlayerGridInfo(0);
    }

    public void SetDataInfo(bool isTest, ConvoyInfo escortInfo)
    {
        this.InitUIInfo();
        if (!isTest)
        {
            Debug.LogWarning("escortInfo.isComplete:_____________" + escortInfo.isComplete);
            if (escortInfo.isComplete == 1)
            {
                List<ConvoyInfo> targetListInfo = new List<ConvoyInfo>();
                this.SetPlayerGridInfo(targetListInfo);
                this.EscortTime__ = 0;
            }
            else
            {
                List<ConvoyInfo> list2 = new List<ConvoyInfo> {
                    escortInfo
                };
                this.SetPlayerGridInfo(list2);
                if (list2.Count > 0)
                {
                    int num = 0x1770;
                    convoy_line_config _config = ConfigMgr.getInstance().getByEntry<convoy_line_config>(escortInfo.lineId);
                    if (_config != null)
                    {
                        num = _config.total_time;
                    }
                    this.EscortTime__ = XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage].beginTime + num;
                }
                else
                {
                    this.EscortTime__ = 0;
                }
            }
        }
        string word = ConfigMgr.getInstance().GetWord(0x186e6);
        if (this.EscortTime__ > 0)
        {
            this.LabelTime.gameObject.SetActive(true);
            this.LabelTime.text = string.Format(word, this.EscortTime__);
            XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
            XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
        }
        else
        {
            this.LabelTime.gameObject.SetActive(false);
        }
    }

    public void SetDataInfo(bool isTest, List<ConvoyRobTarget> convoyRobTargetList)
    {
        this.InterceptTime__ = XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime;
        this.LabelInterceptTime.text = string.Format(ConfigMgr.getInstance().GetWord(0x186e7), TimeMgr.Instance.GetRemainTime2(this.InterceptTime__));
        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Intercept;
        XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = 0;
        XSingleton<GameDetainsDartMgr>.Singleton.InterceptListInfo = convoyRobTargetList;
        if (convoyRobTargetList != null)
        {
            this.InitUIInfo();
            if (isTest)
            {
                this.InterceptTime__ = 300;
            }
            else
            {
                this.SetPlayerGridInfo(convoyRobTargetList);
                if (convoyRobTargetList.Count > 0)
                {
                    this.InterceptTime__ = XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime;
                    Debug.LogWarning("11111111111111111111111111InterceptTime______:" + this.InterceptTime__);
                }
                else
                {
                    this.InterceptTime__ = 0;
                }
            }
            if (convoyRobTargetList.Count <= 0)
            {
                this.IsInterceptTimeStart = false;
            }
            else
            {
                this.IsInterceptTimeStart = true;
            }
            if (XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime > TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime))
            {
                this.IsInterceptTimeStart = true;
            }
            else
            {
                this.IsInterceptTimeStart = false;
            }
        }
    }

    public void SetInterceptBtnData(int times)
    {
        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes = times;
        buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(XSingleton<GameDetainsDartMgr>.Singleton.buyInterceptTimes);
        if (_config != null)
        {
            this.buyInterceptNeedStone = _config.buy_convoy_rob_cost_stone;
        }
        int num = 0;
        variable_config _config2 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config2 != null)
        {
            num = (_config2.convoy_reftarget_incgold * XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt) + _config2.convoy_reftarget_base_gold;
            if (num >= _config2.convoy_reftarget_maxgold)
            {
                num = _config2.convoy_reftarget_maxgold;
            }
        }
        this.LabelScanNeed.text = string.Format("{0}", num);
        this.LabelScanNum.text = string.Format("{0}/{1}", XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes, XSingleton<GameDetainsDartMgr>.Singleton.maxInterceptCnt);
    }

    public void SetNewRecordTipsInfo()
    {
        this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
    }

    private void SetPlayerGridInfo(List<ConvoyInfo> targetListInfo)
    {
        if (targetListInfo.Count > 0)
        {
            this.PlayerTable.transform.localPosition = new Vector3(-400f, 0f, 0f);
            this.PlayerTable.cellWidth = 400f;
            this.PlayerTable.cellHeight = 120f;
            this.TablePlayerTable.Cache = false;
            if (targetListInfo.Count <= 0)
            {
                this.TablePlayerTable.Count = 0;
            }
            else
            {
                this.TablePlayerTable.Count = targetListInfo.Count;
                for (int i = 0; i < targetListInfo.Count; i++)
                {
                    <SetPlayerGridInfo>c__AnonStorey1B7 storeyb = new <SetPlayerGridInfo>c__AnonStorey1B7 {
                        <>f__this = this
                    };
                    Vector3 vector = new Vector3();
                    int num4 = i;
                    switch (targetListInfo[num4].lineId)
                    {
                        case 0:
                            if (this.GetTimeStateIsLastState(targetListInfo[i]))
                            {
                                vector = new Vector3(400f, -110f, 0f);
                            }
                            else
                            {
                                vector = new Vector3(50f, 0f, 0f);
                            }
                            break;

                        case 1:
                            if (this.GetTimeStateIsLastState(targetListInfo[i]))
                            {
                                vector = new Vector3(560f, 0f, 0f);
                            }
                            else
                            {
                                vector = new Vector3(220f, 50f, 0f);
                            }
                            break;

                        case 2:
                            if (this.GetTimeStateIsLastState(targetListInfo[i]))
                            {
                                vector = new Vector3(700f, 50f, 0f);
                            }
                            else
                            {
                                vector = new Vector3(250f, 150f, 0f);
                            }
                            break;
                    }
                    this.TablePlayerTable[i].Model.Template.Item.Root.transform.localPosition = vector;
                    storeyb.indexTemp = i;
                    this.TablePlayerTable[i].Model.Template.TexturSmHeadIcon.OnUIMouseClick(new Action<object>(storeyb.<>m__276));
                    this.TablePlayerTable[i].Model.Template.SpriteSelPlayer.gameObject.SetActive(false);
                    this.TablePlayerTable[i].Root.gameObject.SetActive(true);
                    if (((targetListInfo != null) && (targetListInfo.Count > 0)) && (targetListInfo[0].cards.Count > 0))
                    {
                        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(ActorData.getInstance().HeadEntry);
                        if (_config != null)
                        {
                            this.TablePlayerTable[i].Model.Template.TexturSmHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        }
                        CommonFunc.SetPlayerHeadFrame(this.TablePlayerTable[i].Model.Template.SpriteSmQuality, this.TablePlayerTable[i].Model.Template.QIcon, ActorData.getInstance().HeadFrameEntry);
                        this.TablePlayerTable[i].Model.Template.LabelCardLv.text = string.Empty;
                        if (targetListInfo[i].flagId >= 0)
                        {
                            this.TablePlayerTable[i].Model.Template.SpriteFlag.spriteName = "Ui_Detains_Icon_flag" + (targetListInfo[i].flagId + 1);
                        }
                        else
                        {
                            this.TablePlayerTable[i].Model.Template.SpriteFlag.spriteName = "Ui_Detains_Icon_flag" + 1;
                        }
                    }
                }
            }
        }
    }

    private void SetPlayerGridInfo(List<ConvoyRobTarget> targetListInfo)
    {
        this.PlayerTable.transform.localPosition = new Vector3(-400f, 0f, 0f);
        this.PlayerTable.cellWidth = 400f;
        this.PlayerTable.cellHeight = 120f;
        this.TablePlayerTable.Cache = false;
        if (targetListInfo.Count <= 0)
        {
            this.TablePlayerTable.Count = 0;
        }
        else
        {
            this.TablePlayerTable.Count = targetListInfo.Count;
            for (int i = 0; i < targetListInfo.Count; i++)
            {
                <SetPlayerGridInfo>c__AnonStorey1B6 storeyb = new <SetPlayerGridInfo>c__AnonStorey1B6 {
                    <>f__this = this
                };
                Vector3 vector = new Vector3();
                int num4 = i;
                switch (targetListInfo[num4].lineId)
                {
                    case 0:
                        if (this.JudgeIsLastState(targetListInfo[i]))
                        {
                            vector = new Vector3(400f, -110f, 0f);
                        }
                        else
                        {
                            vector = new Vector3(50f, 0f, 0f);
                        }
                        break;

                    case 1:
                        if (this.JudgeIsLastState(targetListInfo[i]))
                        {
                            vector = new Vector3(560f, 0f, 0f);
                        }
                        else
                        {
                            vector = new Vector3(220f, 50f, 0f);
                        }
                        break;

                    case 2:
                        if (this.JudgeIsLastState(targetListInfo[i]))
                        {
                            vector = new Vector3(700f, 50f, 0f);
                        }
                        else
                        {
                            vector = new Vector3(250f, 150f, 0f);
                        }
                        break;
                }
                this.TablePlayerTable[i].Model.Template.Item.Root.transform.localPosition = vector;
                storeyb.indexTemp = i;
                storeyb.xxInfo = targetListInfo[storeyb.indexTemp];
                this.TablePlayerTable[i].Model.Template.TexturSmHeadIcon.OnUIMouseClick(new Action<object>(storeyb.<>m__275));
                this.TablePlayerTable[i].Model.Template.SpriteSelPlayer.gameObject.SetActive(false);
                if (targetListInfo != null)
                {
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(targetListInfo[i].headEntry);
                    if (_config != null)
                    {
                        this.TablePlayerTable[i].Model.Template.TexturSmHeadIcon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                    }
                    CommonFunc.SetPlayerHeadFrame(this.TablePlayerTable[i].Model.Template.SpriteSmQuality, this.TablePlayerTable[i].Model.Template.QIcon, targetListInfo[i].head_frame_entry);
                    this.TablePlayerTable[i].Model.Template.LabelCardLv.text = targetListInfo[i].lvl.ToString();
                    if (targetListInfo[i].flagId >= 0)
                    {
                        this.TablePlayerTable[i].Model.Template.SpriteFlag.spriteName = "Ui_Detains_Icon_flag" + (targetListInfo[i].flagId + 1);
                    }
                    else
                    {
                        this.TablePlayerTable[i].Model.Template.SpriteFlag.spriteName = "Ui_Detains_Icon_flag" + 1;
                    }
                }
            }
        }
    }

    private void SetPlayerGridInfo(int playCnt = 0)
    {
        this.PlayerTable.transform.localPosition = new Vector3(-400f, 0f, 0f);
        this.PlayerTable.cellWidth = 400f;
        this.PlayerTable.cellHeight = 120f;
        this.TablePlayerTable.Count = playCnt;
        if (this.GoSelRoad == null)
        {
            this.GoSelRoad = new GameObject();
            this.GoSelRoad.name = "GoSelRoad";
            this.GoSelRoad.transform.parent = base.transform;
            this.GoSelRoad.transform.localPosition = new Vector3(0f, -50f, 0f);
            this.GoSelRoad.transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject obj2 = ObjectManager.CreateObj("EffectPrefabs/xiaozhushou");
            obj2.transform.parent = this.GoSelRoad.transform;
            obj2.transform.localPosition = this.SpriteBuff_Left.transform.localPosition;
            GameObject obj3 = ObjectManager.CreateObj("EffectPrefabs/xiaozhushou");
            obj3.transform.parent = this.GoSelRoad.transform;
            obj3.transform.localPosition = this.SpriteBuff_Mid.transform.localPosition;
            GameObject obj4 = ObjectManager.CreateObj("EffectPrefabs/xiaozhushou");
            obj4.transform.parent = this.GoSelRoad.transform;
            obj4.transform.localPosition = this.SpriteBuff_Right.transform.localPosition;
        }
        else
        {
            this.GoSelRoad.gameObject.SetActive(true);
        }
    }

    public void SetRewardInfo()
    {
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Doing)
            {
                if ((XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage >= 0) && (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage < XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count))
                {
                    ConvoyInfo info = new ConvoyInfo();
                    info = XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage];
                    DateTime time = TimeMgr.Instance.ConvertToDateTime((long) info.beginTime);
                    DateTime time2 = time.AddSeconds((double) info.duration);
                    if (time2 <= TimeMgr.Instance.ServerDateTime)
                    {
                        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Done;
                        this.RewardInfo.gameObject.SetActive(true);
                        this.TablePlayerTable.Count = 0;
                    }
                    else
                    {
                        this.RewardInfo.gameObject.SetActive(false);
                    }
                    Debug.LogWarning(string.Concat(new object[] { "i.xxBeginTime___:", time, "_____________i.EndTime___:", time2 }));
                }
                else
                {
                    this.RewardInfo.gameObject.SetActive(false);
                }
            }
            else if (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Selet)
            {
                this.RewardInfo.gameObject.SetActive(false);
            }
            else if (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Done)
            {
                this.RewardInfo.gameObject.SetActive(true);
            }
        }
    }

    private void ShowBattleRecordInfo()
    {
        Debug.LogWarning("ShowBattleRecordInfo!");
        if (!XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            SocketMgr.Instance.RequestC2S_GetConvoyEnemyList();
        }
        else
        {
            if (<>f__am$cache34 == null)
            {
                <>f__am$cache34 = ui => (ui as DetainsDartBattleRecordPanel).SetEnermyInfo(XSingleton<GameDetainsDartMgr>.Singleton.curPageEnermyInfo);
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartBattleRecordPanel>(<>f__am$cache34);
        }
    }

    private void ShowRuleInfo()
    {
        Debug.LogWarning("ShowRuleInfo!");
        GuildBattleNewRulePanel panel = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
        if (panel != null)
        {
            panel.GetRuleInfo_DetainsDart();
        }
        else
        {
            if (<>f__am$cache33 == null)
            {
                <>f__am$cache33 = delegate (GUIEntity u) {
                    GuildBattleNewRulePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.GetRuleInfo_DetainsDart();
                    }
                };
            }
            GUIMgr.Instance.DoModelGUI<GuildBattleNewRulePanel>(<>f__am$cache33, null);
        }
    }

    private void StartEscort()
    {
        Debug.LogWarning("StartEscort!");
        if (this.curSelRoadState < RoadState.Left)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e2));
        }
        else if ((XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Count <= 0) || (XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList.Count > 5))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e3));
        }
        else
        {
            SocketMgr.Instance.RequestC2S_BeginConvoy((int) this.curSelRoadState, XSingleton<GameDetainsDartMgr>.Singleton.curReqEscortCardIdList, XSingleton<GameDetainsDartMgr>.Singleton.curReqFriendId);
        }
    }

    private void TurnLeft()
    {
        Debug.LogWarning("TurnLeft:___" + XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage);
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
        {
            if ((XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0) && (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage))
            {
                if (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count <= (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage + 1))
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e4));
                }
                else
                {
                    GameDetainsDartMgr local1 = XSingleton<GameDetainsDartMgr>.Singleton;
                    local1.curLeftRightPage++;
                    if (<>f__am$cache35 == null)
                    {
                        <>f__am$cache35 = delegate (GUIEntity ui) {
                            XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
                            if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage == 0)
                            {
                                XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                            }
                            else if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage > 0)
                            {
                                XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
                            }
                            (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache35);
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e5));
            }
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.None)
        {
            if (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0)
            {
                if (<>f__am$cache36 == null)
                {
                    <>f__am$cache36 = delegate (GUIEntity ui) {
                        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
                        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
                        GameDetainsDartMgr singleton = XSingleton<GameDetainsDartMgr>.Singleton;
                        singleton.curLeftRightPage++;
                        (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                    };
                }
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache36);
            }
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
            if (<>f__am$cache37 == null)
            {
                <>f__am$cache37 = delegate (GUIEntity ui) {
                    XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = -1;
                    XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                    (ui as DetainsDartMainUIPanel).SetDataInfo(false, null);
                };
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(<>f__am$cache37);
        }
    }

    private void TurnRight()
    {
        Debug.LogWarning("TurnRight___:" + XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage);
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
        {
            if ((XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0) && (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage))
            {
                if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage > 0)
                {
                    if (<>f__am$cache38 == null)
                    {
                        <>f__am$cache38 = delegate (GUIEntity ui) {
                            XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
                            GameDetainsDartMgr singleton = XSingleton<GameDetainsDartMgr>.Singleton;
                            singleton.curLeftRightPage--;
                            if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage == 0)
                            {
                                XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
                            }
                            else if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage > 0)
                            {
                                XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                            }
                            else
                            {
                                TipsDiag.SetText("当前数据错误curPage:" + XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage);
                            }
                            (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache38);
                }
                else
                {
                    if (<>f__am$cache39 == null)
                    {
                        <>f__am$cache39 = delegate (GUIEntity ui) {
                            XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = -1;
                            XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                            (ui as DetainsDartMainUIPanel).SetDataInfo(false, null);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(<>f__am$cache39);
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e5));
            }
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.None)
        {
            SocketMgr.Instance.RequestC2S_GetRobTargetList();
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
        }
    }

    protected UISprite Arrows { get; set; }

    protected UISprite AutoClose { get; set; }

    protected UISprite Bnt_Rewad { get; set; }

    protected UIButton Btn_BattleRecord { get; set; }

    protected UIButton Btn_ReScan { get; set; }

    protected UIButton Btn_Rule { get; set; }

    protected UIButton Btn_StartEscort { get; set; }

    protected UISprite Btn_TurnLeft { get; set; }

    protected UISprite Btn_TurnRight { get; set; }

    protected UIButton CloseButton { get; set; }

    protected Transform GoInterceptTimeInfo { get; set; }

    protected UIWidget GoldBox { get; set; }

    protected Transform GoLeftTtimeInfo { get; set; }

    protected Transform GoMapObjsInfo { get; set; }

    protected Transform GoPlayCardsInfo { get; set; }

    protected Transform GoScanInfo { get; set; }

    protected UILabel LabelInterceptTime { get; set; }

    protected UILabel LabelScanNeed { get; set; }

    protected UILabel LabelScanNum { get; set; }

    protected UILabel LabelTime { get; set; }

    protected UILabel LabelTitle { get; set; }

    protected UITexture MapBg { get; set; }

    protected UISprite NewRecordTips { get; set; }

    protected UIGrid PlayerTable { get; set; }

    protected Transform RewardInfo { get; set; }

    protected UISprite SpriteBuff_Left { get; set; }

    protected UISprite SpriteBuff_Mid { get; set; }

    protected UISprite SpriteBuff_Right { get; set; }

    protected UISprite SpriteBuffSelectLeft { get; set; }

    protected UISprite SpriteBuffSelectMid { get; set; }

    protected UISprite SpriteBuffSelectRight { get; set; }

    protected UISprite SpriteSelRoadTitleTips { get; set; }

    [CompilerGenerated]
    private sealed class <OnClickPlayer>c__AnonStorey1B8
    {
        internal ConvoyRobTarget info;

        internal void <>m__277(GUIEntity obj)
        {
            TargetTeamPanel panel = (TargetTeamPanel) obj;
            ConvoyRobTarget info = new ConvoyRobTarget();
            info = this.info;
            panel.SetInterceptInfo(info);
            panel.SetPkBtnStat(true);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickReScan>c__AnonStorey1B5
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        private static UIEventListener.VoidDelegate <>f__am$cache3;
        internal DetainsDartOnDoingPanel <>f__this;
        internal string xxTipsText;

        internal void <>m__260(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (GameObject box) {
                };
            }
            box.SetDialog(this.xxTipsText, box => this.<>f__this.ConfirmBuyInterceptTime(), <>f__am$cache2, false);
        }

        internal void <>m__261(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (GameObject box) {
                };
            }
            box.SetDialog(this.xxTipsText, box => this.<>f__this.ConfirmReFreshInterceptTeams(), <>f__am$cache3, false);
        }

        internal void <>m__278(GameObject box)
        {
            this.<>f__this.ConfirmBuyInterceptTime();
        }

        private static void <>m__279(GameObject box)
        {
        }

        internal void <>m__27A(GameObject box)
        {
            this.<>f__this.ConfirmReFreshInterceptTeams();
        }

        private static void <>m__27B(GameObject box)
        {
        }
    }

    [CompilerGenerated]
    private sealed class <SetPlayerGridInfo>c__AnonStorey1B6
    {
        internal DetainsDartOnDoingPanel <>f__this;
        internal int indexTemp;
        internal ConvoyRobTarget xxInfo;

        internal void <>m__275(object u)
        {
            this.<>f__this.OnClickPlayer(this.indexTemp, this.xxInfo);
        }
    }

    [CompilerGenerated]
    private sealed class <SetPlayerGridInfo>c__AnonStorey1B7
    {
        internal DetainsDartOnDoingPanel <>f__this;
        internal int indexTemp;

        internal void <>m__276(object u)
        {
            this.<>f__this.OnClickPlayer(this.indexTemp);
        }
    }

    public class PlayerTableItemModel : TableItemModel<DetainsDartOnDoingPanel.PlayerTableItemTemplate>
    {
        public override void Init(DetainsDartOnDoingPanel.PlayerTableItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class PlayerTableItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.SmallCardItem = base.FindChild<Transform>("SmallCardItem");
            this.SpriteSmQuality = base.FindChild<UISprite>("SpriteSmQuality");
            this.QIcon = base.FindChild<UISprite>("QIcon");
            this.TexturSmHeadIcon = base.FindChild<UITexture>("TexturSmHeadIcon");
            this.SpriteSmHeadBg = base.FindChild<UISprite>("SpriteSmHeadBg");
            this.LabelCardLv = base.FindChild<UILabel>("LabelCardLv");
            this.GoStartInfo = base.FindChild<UIGrid>("GoStartInfo");
            this.SpriteStar = base.FindChild<UISprite>("SpriteStar");
            this.SpriteFlag = base.FindChild<UISprite>("SpriteFlag");
            this.SpriteFlagBg = base.FindChild<UISprite>("SpriteFlagBg");
            this.SpriteFlagKuang = base.FindChild<UISprite>("SpriteFlagKuang");
            this.SpriteSelPlayer = base.FindChild<UISprite>("SpriteSelPlayer");
        }

        public UIGrid GoStartInfo { get; private set; }

        public UILabel LabelCardLv { get; private set; }

        public UISprite QIcon { get; private set; }

        public Transform SmallCardItem { get; private set; }

        public UISprite SpriteFlag { get; private set; }

        public UISprite SpriteFlagBg { get; private set; }

        public UISprite SpriteFlagKuang { get; private set; }

        public UISprite SpriteSelPlayer { get; private set; }

        public UISprite SpriteSmHeadBg { get; private set; }

        public UISprite SpriteSmQuality { get; private set; }

        public UISprite SpriteStar { get; private set; }

        public UITexture TexturSmHeadIcon { get; private set; }
    }

    private enum RoadState
    {
        Left = 0,
        Mid = 1,
        None = -1,
        Right = 2
    }
}

