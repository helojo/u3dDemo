using FastBuf;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

internal class DetainsDartMainUIPanel : GUIPanelEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache22;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache23;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache24;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache25;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache26;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache27;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache28;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache29;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2A;
    private int AmBeginHour;
    private float AmBeginTime;
    private float AmDunTime;
    private int AmEndHour;
    private int curEscortCnt__ = -1;
    private int curInterceptCnt__ = -1;
    private int curSelFlagId;
    private int gridCnt__ = 2;
    private bool isTest = true;
    private int limitEscortCnt__ = -1;
    private int limitInterceptCnt__ = -1;
    private int PmBegineHour;
    private float PmBenginTime;
    private float PmDunTime;
    private int PmEndHour;
    private int reFreshFlagTimes = -1;
    private int reFreshTargetTimes = -1;
    protected UITableManager<UIAutoGenItem<TypeGridItemTemplate, TypeGridItemModel>> TableTypeGrid = new UITableManager<UIAutoGenItem<TypeGridItemTemplate, TypeGridItemModel>>();
    private int timeScale;

    private void Colse()
    {
        Debug.LogWarning("Colse!");
        GUIMgr.Instance.ClearExceptMainUI();
    }

    public override void Initialize()
    {
        base.Initialize();
        this.Btn_Rule.OnUIMouseClick(u => this.ShowRuleInfo());
        this.Btn_BattleRecord.OnUIMouseClick(u => this.ShowBattleRecordInfo());
        this.Btn_TurnLeft.OnUIMouseClick(u => this.TurnLeft());
        this.CloseButton.OnUIMouseClick(u => this.Colse());
        if (this.Btn_TurnRight != null)
        {
            this.Btn_TurnRight.gameObject.SetActive(false);
        }
        this.SetNewRecordTipsInfo();
        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
        this.SetGridInfo();
        this.SetOpenTimeInfo();
        this.ResetMapRange();
        SocketMgr.Instance.RequestC2S_GetConvoyInfo();
    }

    public override void InitializePanel()
    {
        base.InitializePanel();
        this.ScrollViewList = base.FindChild<UIPanel>("ScrollViewList");
        this.TypeGrid = base.FindChild<UIGrid>("TypeGrid");
        this.Btn_TurnRight = base.FindChild<UISprite>("Btn_TurnRight");
        this.Btn_TurnLeft = base.FindChild<UISprite>("Btn_TurnLeft");
        this.CloseButton = base.FindChild<UIButton>("CloseButton");
        this.GoBottom = base.FindChild<Transform>("GoBottom");
        this.Btn_Rule = base.FindChild<UIButton>("Btn_Rule");
        this.Btn_BattleRecord = base.FindChild<UIButton>("Btn_BattleRecord");
        this.NewRecordTips = base.FindChild<UISprite>("NewRecordTips");
        this.AutoClose = base.FindChild<UISprite>("AutoClose");
        this.GoOpenTimeInfo = base.FindChild<Transform>("GoOpenTimeInfo");
        this.LabelTimeTitle = base.FindChild<UILabel>("LabelTimeTitle");
        this.LabelTime = base.FindChild<UILabel>("LabelTime");
        this.MapBg = base.FindChild<UITexture>("MapBg");
        this.LabelTitle = base.FindChild<UILabel>("LabelTitle");
        this.TableTypeGrid.InitFromGrid(this.TypeGrid);
    }

    private bool JudgeIsDuringInActiveTime()
    {
        return (((TimeMgr.Instance.ServerDateTime.Hour >= this.AmBeginHour) && ((TimeMgr.Instance.ServerDateTime.Hour >= this.PmBegineHour) || (TimeMgr.Instance.ServerDateTime.Hour < this.AmEndHour))) && (TimeMgr.Instance.ServerDateTime.Hour < this.PmEndHour));
    }

    private void OnClickEscort()
    {
        Debug.LogWarning("OnClickEscort!");
        if (!this.JudgeIsDuringInActiveTime())
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186e8));
        }
        else if (this.curEscortCnt__ <= 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186eb));
        }
        else
        {
            if (<>f__am$cache29 == null)
            {
                <>f__am$cache29 = delegate (GUIEntity ui) {
                    XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
                    XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Selet;
                    (ui as DetainsDartSelFlagAndTeamPanel).SetUIDataInfo();
                };
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartSelFlagAndTeamPanel>(<>f__am$cache29);
        }
    }

    private void OnClickIntercept()
    {
        Debug.LogWarning("OnClickIntercept!");
        if (!XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            SocketMgr.Instance.RequestC2S_GetRobTargetList();
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.None)
        {
            if (<>f__am$cache2A == null)
            {
                <>f__am$cache2A = delegate (GUIEntity ui) {
                    XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = 0;
                    if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage == -1)
                    {
                        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                    }
                    else if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage == 0)
                    {
                        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Intercept;
                    }
                    if (XSingleton<GameDetainsDartMgr>.Singleton.InterceptListInfo.Count > 0)
                    {
                        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState = GameDetainsDartMgr.InterceptState.SeletPlayer;
                        SocketMgr.Instance.RequestC2S_GetRobTargetList();
                    }
                    else
                    {
                        XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState = GameDetainsDartMgr.InterceptState.Battle;
                        (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.InterceptListInfo);
                    }
                };
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache2A);
        }
    }

    private void OnPressOver(bool isDown, int index)
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
        this.TableTypeGrid[index].Model.Template.SpriteBg.spriteName = str;
    }

    private void ResetMapRange()
    {
        UIPanel component = base.gameObject.GetComponent<UIPanel>();
        int activeHeight = GUIMgr.Instance.Root.activeHeight;
        float width = (((float) Screen.width) / ((float) Screen.height)) * activeHeight;
        component.SetRect(0f, 0f, width, (float) activeHeight);
    }

    public void SetDataInfo(bool test = true, S2C_GetConvoyInfo message = null)
    {
        XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = -1;
        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
        vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
        if (_config != null)
        {
            this.limitEscortCnt__ = _config.convoy_times;
            this.limitInterceptCnt__ = _config.convoy_rob_times;
            XSingleton<GameDetainsDartMgr>.Singleton.maxEscortCnt = _config.convoy_times;
            XSingleton<GameDetainsDartMgr>.Singleton.maxInterceptCnt = _config.convoy_rob_times;
        }
        if (test)
        {
            this.curEscortCnt__ = 2;
            this.curInterceptCnt__ = 2;
            this.reFreshFlagTimes = 0;
            this.curSelFlagId = 0;
            this.reFreshTargetTimes = 0;
        }
        else if (message == null)
        {
            this.curEscortCnt__ = XSingleton<GameDetainsDartMgr>.Singleton.curEscortCnt;
            this.curInterceptCnt__ = XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes;
            this.reFreshFlagTimes = XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt;
            this.curSelFlagId = XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex;
            this.reFreshTargetTimes = XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt;
        }
        else
        {
            this.curEscortCnt__ = message.remainConvoyTimes;
            this.curInterceptCnt__ = message.remainRobTimes;
            this.reFreshFlagTimes = message.refreshFlagTimes;
            this.curSelFlagId = message.currFlagId;
            this.reFreshTargetTimes = message.refreshTargetTimes;
        }
        this.SetGridInfo();
        this.SetTurnLeftBtnShowState();
        this.SetNewRecordTipsInfo();
    }

    private void SetGridInfo()
    {
        this.TypeGrid.cellWidth = 330f;
        this.TypeGrid.cellHeight = 120f;
        this.TableTypeGrid.Count = this.gridCnt__;
        for (int i = 0; i < this.gridCnt__; i++)
        {
            <SetGridInfo>c__AnonStorey1B4 storeyb = new <SetGridInfo>c__AnonStorey1B4 {
                <>f__this = this
            };
            string word = string.Empty;
            string format = string.Empty;
            int num2 = -1;
            int num3 = -1;
            switch (i)
            {
                case 0:
                    word = ConfigMgr.getInstance().GetWord(0x186e9);
                    format = "{0} / {1}";
                    num2 = this.curEscortCnt__;
                    num3 = this.limitEscortCnt__;
                    this.TableTypeGrid[i].Model.Template.SpriteSword_Left.spriteName = "Ui_Detains_Bg_jian";
                    this.TableTypeGrid[i].Model.Template.SpriteSword_Right.spriteName = "Ui_Detains_Bg_jian";
                    this.TableTypeGrid[i].Model.Template.SpriteBg.OnUIMouseClick(new Action<object>(storeyb.<>m__253));
                    break;

                case 1:
                    word = ConfigMgr.getInstance().GetWord(0x186ea);
                    format = "{0} / {1}";
                    num2 = this.curInterceptCnt__;
                    num3 = this.limitInterceptCnt__;
                    this.TableTypeGrid[i].Model.Template.SpriteSword_Left.spriteName = "Ui_Detains_Bg_fu";
                    this.TableTypeGrid[i].Model.Template.SpriteSword_Right.spriteName = "Ui_Detains_Bg_fu";
                    this.TableTypeGrid[i].Model.Template.SpriteBg.OnUIMouseClick(new Action<object>(storeyb.<>m__254));
                    break;
            }
            this.TableTypeGrid[i].Model.Template.LabelTypeTitle.text = word;
            this.TableTypeGrid[i].Model.Template.LabelNum.text = string.Format(format, num2, num3);
            this.TableTypeGrid[i].Model.Template.SpriteTexIcon.spriteName = "Ui_Detains_Icon_Base_" + i;
            storeyb.indexTemp = i;
            this.TableTypeGrid[i].Model.Template.SpriteBg.OnUIMousePress(new Action<bool, object>(storeyb.<>m__255));
        }
        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
    }

    public void SetNewRecordTipsInfo()
    {
        this.NewRecordTips.gameObject.SetActive(ActorData.getInstance().HaveNewDetainsDartLog);
    }

    private void SetOpenTimeInfo()
    {
        convoy_time_config _config = ConfigMgr.getInstance().getByEntry<convoy_time_config>(0);
        if (_config != null)
        {
            this.AmBeginTime = _config.begin_time;
            this.AmDunTime = _config.duration;
        }
        convoy_time_config _config2 = ConfigMgr.getInstance().getByEntry<convoy_time_config>(1);
        if (_config2 != null)
        {
            this.PmBenginTime = _config2.begin_time;
            this.PmDunTime = _config2.duration;
        }
        this.AmBeginHour = TimeSpan.FromSeconds((double) this.AmBeginTime).Hours;
        this.AmEndHour = TimeSpan.FromSeconds((double) this.AmBeginTime).Hours + TimeSpan.FromSeconds((double) this.AmDunTime).Hours;
        this.PmBegineHour = TimeSpan.FromSeconds((double) this.PmBenginTime).Hours;
        this.PmEndHour = TimeSpan.FromSeconds((double) this.PmBenginTime).Hours + TimeSpan.FromSeconds((double) this.PmDunTime).Hours;
        object[] args = new object[] { TimeSpan.FromSeconds((double) this.AmBeginTime).Hours, TimeSpan.FromSeconds((double) this.AmBeginTime).Hours + TimeSpan.FromSeconds((double) this.AmDunTime).Hours, TimeSpan.FromSeconds((double) this.PmBenginTime).Hours, TimeSpan.FromSeconds((double) this.PmBenginTime).Hours + TimeSpan.FromSeconds((double) this.PmDunTime).Hours };
        this.LabelTime.text = string.Format("{0}:00 - {1}:00        {2}:00 - {3}:00", args);
    }

    private void SetTurnLeftBtnShowState()
    {
        if (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0)
        {
            this.Btn_TurnLeft.gameObject.SetActive(true);
        }
        else
        {
            this.Btn_TurnLeft.gameObject.SetActive(false);
        }
    }

    private void ShowBattleRecordInfo()
    {
        if (!XSingleton<GameDetainsDartMgr>.Singleton.isTest)
        {
            SocketMgr.Instance.RequestC2S_GetConvoyEnemyList();
        }
        else
        {
            if (<>f__am$cache23 == null)
            {
                <>f__am$cache23 = ui => (ui as DetainsDartBattleRecordPanel).SetEnermyInfo(XSingleton<GameDetainsDartMgr>.Singleton.curPageEnermyInfo);
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartBattleRecordPanel>(<>f__am$cache23);
        }
    }

    private void ShowRuleInfo()
    {
        GuildBattleNewRulePanel panel = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
        if (panel != null)
        {
            panel.GetRuleInfo_DetainsDart();
        }
        else
        {
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = delegate (GUIEntity u) {
                    GuildBattleNewRulePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildBattleNewRulePanel>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.GetRuleInfo_DetainsDart();
                    }
                };
            }
            GUIMgr.Instance.DoModelGUI<GuildBattleNewRulePanel>(<>f__am$cache22, null);
        }
    }

    private void TurnLeft()
    {
        Debug.LogWarning("TurnLeft!");
        if (!this.JudgeIsDuringInActiveTime() && (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count <= 0))
        {
            TipsDiag.SetText("ConfigMgr.getInstance().GetWord(100072)");
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
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
                    if (<>f__am$cache24 == null)
                    {
                        <>f__am$cache24 = delegate (GUIEntity ui) {
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
                    GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache24);
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
                if (<>f__am$cache25 == null)
                {
                    <>f__am$cache25 = delegate (GUIEntity ui) {
                        XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Escort;
                        XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
                        GameDetainsDartMgr singleton = XSingleton<GameDetainsDartMgr>.Singleton;
                        singleton.curLeftRightPage++;
                        (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                    };
                }
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache25);
            }
        }
        else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Intercept)
        {
            if (<>f__am$cache26 == null)
            {
                <>f__am$cache26 = delegate (GUIEntity ui) {
                    XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = -1;
                    XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                    (ui as DetainsDartMainUIPanel).SetDataInfo(false, null);
                };
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(<>f__am$cache26);
        }
    }

    private void TurnRight()
    {
        Debug.LogWarning("TurnRight!");
        if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
        {
            if ((XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > 0) && (XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Count > XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage))
            {
                if (XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage > 0)
                {
                    if (<>f__am$cache27 == null)
                    {
                        <>f__am$cache27 = delegate (GUIEntity ui) {
                            XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.Doing;
                            GameDetainsDartMgr singleton = XSingleton<GameDetainsDartMgr>.Singleton;
                            singleton.curLeftRightPage--;
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
                    GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache27);
                }
                else
                {
                    if (<>f__am$cache28 == null)
                    {
                        <>f__am$cache28 = delegate (GUIEntity ui) {
                            XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = -1;
                            XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
                            (ui as DetainsDartMainUIPanel).SetDataInfo(false, null);
                        };
                    }
                    GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(<>f__am$cache28);
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

    protected UISprite AutoClose { get; set; }

    protected UIButton Btn_BattleRecord { get; set; }

    protected UIButton Btn_Rule { get; set; }

    protected UISprite Btn_TurnLeft { get; set; }

    protected UISprite Btn_TurnRight { get; set; }

    protected UIButton CloseButton { get; set; }

    protected Transform GoBottom { get; set; }

    protected Transform GoOpenTimeInfo { get; set; }

    protected UILabel LabelTime { get; set; }

    protected UILabel LabelTimeTitle { get; set; }

    protected UILabel LabelTitle { get; set; }

    protected UITexture MapBg { get; set; }

    protected UISprite NewRecordTips { get; set; }

    protected UIPanel ScrollViewList { get; set; }

    protected UIGrid TypeGrid { get; set; }

    [CompilerGenerated]
    private sealed class <SetGridInfo>c__AnonStorey1B4
    {
        internal DetainsDartMainUIPanel <>f__this;
        internal int indexTemp;

        internal void <>m__253(object u)
        {
            this.<>f__this.OnClickEscort();
        }

        internal void <>m__254(object u)
        {
            this.<>f__this.OnClickIntercept();
        }

        internal void <>m__255(bool isDown, object o)
        {
            this.<>f__this.OnPressOver(isDown, this.indexTemp);
        }
    }

    public class TypeGridItemModel : TableItemModel<DetainsDartMainUIPanel.TypeGridItemTemplate>
    {
        public override void Init(DetainsDartMainUIPanel.TypeGridItemTemplate template, UITableItem item)
        {
            base.Init(template, item);
        }
    }

    public class TypeGridItemTemplate : TableItemTemplate
    {
        public override void Init(UITableItem item)
        {
            base.Init(item);
            this.ItemBattle = base.FindChild<Transform>("ItemBattle");
            this.LabelNum = base.FindChild<UILabel>("LabelNum");
            this.SpriteBg = base.FindChild<UISprite>("SpriteBg");
            this.LabelTypeTitle = base.FindChild<UILabel>("LabelTypeTitle");
            this.SpriteTexIcon = base.FindChild<UISprite>("SpriteTexIcon");
            this.SpriteTexIconBg = base.FindChild<UISprite>("SpriteTexIconBg");
            this.SpriteSword_Right = base.FindChild<UISprite>("SpriteSword_Right");
            this.SpriteSword_Left = base.FindChild<UISprite>("SpriteSword_Left");
        }

        public Transform ItemBattle { get; private set; }

        public UILabel LabelNum { get; private set; }

        public UILabel LabelTypeTitle { get; private set; }

        public UISprite SpriteBg { get; private set; }

        public UISprite SpriteSword_Left { get; private set; }

        public UISprite SpriteSword_Right { get; private set; }

        public UISprite SpriteTexIcon { get; private set; }

        public UISprite SpriteTexIconBg { get; private set; }
    }
}

