using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleGrid
{
    [CompilerGenerated]
    private static Predicate<GameObject> <>f__am$cache29;
    [CompilerGenerated]
    private static System.Action <>f__am$cache2A;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2B;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2C;
    private static readonly int[] B01Index = new int[] { 3 };
    public int buffEntry;
    public int buffShowType;
    private static readonly int[] C01Index;
    private static readonly int[] D01Index;
    private static readonly int[] defaultIndex = new int[] { 0, 1, 2, 3, 4, 5 };
    private static readonly int[] E01Index;
    public OutlandBufferSubType eventBuffSubType;
    public int eventEntry = -1;
    public OutlandMonsterSubType eventMonsterSubType = OutlandMonsterSubType.EN_MONSTER_SUB_ELITE;
    public OutlandEventState eventState;
    public OutlandEventType eventType = OutlandEventType.E_OET_NUM;
    private static readonly int[] F01Index;
    public string gridBuffDes;
    public string gridBuffName;
    public string gridEffect;
    public string gridModel;
    private static readonly int[] H01Index;
    public bool isBoss;
    private bool isBriage;
    public bool isShow;
    public bool isShowStarted;
    private static readonly int[] M01Index;
    private string modelName = string.Empty;
    private static float modelRadiusX = 2.273f;
    private static float modelRadiusY = 2.616f;
    private static readonly VectorInt2[] nearGridIndex0 = new VectorInt2[] { new VectorInt2(0, 1), new VectorInt2(1, 0), new VectorInt2(1, -1), new VectorInt2(0, -1), new VectorInt2(-1, -1), new VectorInt2(-1, 0) };
    private static readonly VectorInt2[] nearGridIndex1 = new VectorInt2[] { new VectorInt2(0, 1), new VectorInt2(1, 1), new VectorInt2(1, 0), new VectorInt2(0, -1), new VectorInt2(-1, 0), new VectorInt2(-1, 1) };
    private int portalTargetGridIndex = -1;
    private List<VectorInt2> surroundPoints;

    static BattleGrid()
    {
        int[] numArray4 = new int[4];
        numArray4[1] = 2;
        numArray4[2] = 3;
        numArray4[3] = 5;
        C01Index = numArray4;
        int[] numArray5 = new int[2];
        numArray5[1] = 3;
        D01Index = numArray5;
        E01Index = new int[] { 2, 4 };
        F01Index = new int[] { 1, 3, 5 };
        H01Index = new int[] { 3 };
        M01Index = new int[] { 1, 3, 5 };
    }

    public void AfterCreateEventModel(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (this.isShowShadow)
            {
                gameObject.transform.position = this.GetWorldPos();
            }
            else
            {
                gameObject.transform.parent = this.model.transform;
                gameObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
        }
    }

    public static Vector3 CalWorldPos(int X, int Y, GameObject basePosObj)
    {
        Vector3 vector = new Vector3(modelRadiusX * X, 0f, (modelRadiusY * 0.5f) * (X % 2));
        Vector3 vector2 = new Vector3(0f, 0f, modelRadiusY * Y);
        Vector3 zero = Vector3.zero;
        if (basePosObj != null)
        {
            zero = basePosObj.transform.position;
        }
        return ((vector + vector2) + zero);
    }

    public void ChangeModel(GameObject modelParentObj, string modelInfoText)
    {
        if (this.model != null)
        {
            UnityEngine.Object.Destroy(this.model);
        }
        float y = 0f;
        if (!string.IsNullOrEmpty(modelInfoText))
        {
            List<string> list = StrParser.ParseStringList(modelInfoText, "_");
            this.modelName = list[0];
            if (list.Count > 1)
            {
                y = StrParser.ParseFloat(list[1]);
            }
        }
        if ((this.Control != null) && (this.Control.gridModels.Count > 0))
        {
            GameObject original = this.Control.gridModels.Find(obj => obj.name == this.modelName);
            if (original == null)
            {
                if (<>f__am$cache29 == null)
                {
                    <>f__am$cache29 = obj => obj.name == "A01";
                }
                original = this.Control.gridModels.Find(<>f__am$cache29);
                Debug.LogWarning("Error Grid Model: " + modelInfoText);
            }
            this.isBriage = this.modelName.StartsWith("D");
            this.model = UnityEngine.Object.Instantiate(original) as GameObject;
            this.model.isStatic = false;
            this.model.name = BattleGridGameMapControl.Axis2Index(this.X, this.Y).ToString();
            if (modelParentObj != null)
            {
                this.model.transform.parent = modelParentObj.transform;
            }
            this.model.transform.position = this.GetWorldPos();
            this.model.transform.rotation = Quaternion.Euler(new Vector3(0f, y, 0f));
            this.model.AddComponent<BoxCollider>().size = new Vector3(2.2f, 0.5f, 2.2f);
        }
    }

    public void CheckSurroundPointsNear()
    {
        this.surroundPoints.RemoveAll(delegate (VectorInt2 obj) {
            BattleGrid grid = this.Control.GetGrid(this.X + obj.x, this.Y + obj.y);
            if ((grid != null) && grid.IsSurround(this.X, this.Y))
            {
                return false;
            }
            return true;
        });
    }

    private void CreateGridModel()
    {
        if ((this.eventState != OutlandEventState.E_OES_UNFINISHED) || (this.eventEntry == -1))
        {
            return;
        }
        if (this.gridModel == null)
        {
            this.eventEntry = -1;
            return;
        }
        switch (this.eventType)
        {
            case OutlandEventType.E_OET_TREASURE:
                this.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + this.gridModel);
                goto Label_01C8;

            case OutlandEventType.E_OET_MONSTER:
            {
                if (this.eventMonsterSubType == OutlandMonsterSubType.EN_MONTER_SUB_BOX)
                {
                    this.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + this.gridModel);
                    break;
                }
                monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(StrParser.ParseDecInt(this.gridModel, -1));
                if (_config != null)
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry);
                    if (_config2 != null)
                    {
                        this.EventModel = CardPlayer.CreateCardPlayer(_config2.entry, null, CardPlayerStateType.Battle, 0);
                    }
                }
                break;
            }
            case OutlandEventType.E_OET_DOOR:
                this.EventModel = ObjectManager.CreateObj("EffectPrefabs/" + this.gridModel);
                goto Label_01C8;

            case OutlandEventType.E_OET_BUFF:
                if (this.buffShowType != 1)
                {
                    this.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + this.gridModel);
                }
                goto Label_01C8;

            default:
                goto Label_01C8;
        }
        switch (this.eventMonsterSubType)
        {
            case OutlandMonsterSubType.EN_MONSTER_SUB_ELITE:
                this.huangseModel = ObjectManager.CreateObj("EffectPrefabs/wy-effect-player-zise");
                this.AfterCreateEventModel(this.huangseModel);
                break;

            case OutlandMonsterSubType.EN_MOSTER_SUB_SMALL_BOSS:
                this.ziseModel = ObjectManager.CreateObj("EffectPrefabs/wy-effect-player-huangse");
                this.AfterCreateEventModel(this.ziseModel);
                break;

            case OutlandMonsterSubType.EN_MOSTER_SUB_BIG_BOSS:
                this.EventModel.transform.localScale = (Vector3) (this.EventModel.transform.localScale * 1.5f);
                break;
        }
    Label_01C8:
        this.AfterCreateEventModel(this.EventModel);
    }

    public int GetIndex()
    {
        return BattleGridGameMapControl.Axis2Index(this.X, this.Y);
    }

    public List<VectorInt2> GetSurroundPoints()
    {
        return this.surroundPoints;
    }

    public Vector3 GetWorldPos()
    {
        return CalWorldPos(this.X, this.Y, this.Control.gridCenterObj);
    }

    public void Init(GameObject modelParentObj, string modelName, OutlandUser _otherPlayer)
    {
        this.ChangeModel(modelParentObj, modelName);
        this.model.SetActive(false);
        this.InitSurroundPoints();
    }

    public void InitSurroundPoints()
    {
        VectorInt2[] numArray = ((this.X % 2) != 0) ? nearGridIndex1 : nearGridIndex0;
        int[] defaultIndex = null;
        if (this.modelName.StartsWith("B"))
        {
            defaultIndex = B01Index;
        }
        else if (this.modelName.StartsWith("C"))
        {
            defaultIndex = C01Index;
        }
        else if (this.modelName.StartsWith("D"))
        {
            defaultIndex = D01Index;
        }
        else if (this.modelName.StartsWith("E"))
        {
            defaultIndex = E01Index;
        }
        else if (this.modelName.StartsWith("F"))
        {
            defaultIndex = F01Index;
        }
        else if (this.modelName.StartsWith("H"))
        {
            defaultIndex = H01Index;
        }
        else if (this.modelName.StartsWith("M"))
        {
            defaultIndex = M01Index;
        }
        else
        {
            defaultIndex = BattleGrid.defaultIndex;
        }
        int num = 0;
        float y = this.model.transform.rotation.eulerAngles.y;
        if (y > 0f)
        {
            y++;
        }
        if (y < 0f)
        {
            y--;
        }
        num = ((int) y) / 60;
        this.surroundPoints = new List<VectorInt2>();
        foreach (int num3 in defaultIndex)
        {
            int index = num + num3;
            index = index % 6;
            this.surroundPoints.Add(numArray[index]);
        }
    }

    public bool IsCanBeTrigger()
    {
        return (((this.eventState == OutlandEventState.E_OES_UNFINISHED) && (this.eventEntry >= 0)) || (this.portalTargetGridIndex >= 0));
    }

    public bool IsFighterGrid()
    {
        return this.IsCanBeTrigger();
    }

    public bool IsMovoPassGrid()
    {
        return (((this.eventType == OutlandEventType.E_OET_MONSTER) || (this.eventType == OutlandEventType.E_OET_DOOR)) && (this.eventState == OutlandEventState.E_OES_UNFINISHED));
    }

    public bool IsSurround(int _x, int _y)
    {
        return this.surroundPoints.Contains(new VectorInt2(_x - this.X, _y - this.Y));
    }

    private void MessageboxNextOk(GameObject go)
    {
        SocketMgr.Instance.RequestOutlandEnterNextFloorReq(this.Control.battleGameData.gridGameData.entry, this.Control.battleGameData.gridGameData.map_entry, this.GetIndex());
        BattleState.GetInstance().CurGame.battleGameData.OnMsgLeave();
    }

    private void MessageboxOk(GameObject go)
    {
        SocketMgr.Instance.RequestOutlandQuit(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry);
        BattleState.GetInstance().CurGame.battleGameData.OnMsgLeave();
    }

    private void MessasgeCanle(GameObject go)
    {
        BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
    }

    public BattleTriggerType OnTrigger()
    {
        if (this.IsCanBeTrigger())
        {
            if (BattleSceneStarter.G_isTestEnable)
            {
                BattleTestMgrImplGrid impl = this.Control.battleGameData.battleComObject.GetComponent<BattleCom_TestManager>().impl as BattleTestMgrImplGrid;
                if (impl != null)
                {
                    impl.DoBattleTest();
                }
                this.SetState(1);
                goto Label_0386;
            }
            if (this.portalTargetGridIndex >= 0)
            {
                <OnTrigger>c__AnonStoreyF0 yf = new <OnTrigger>c__AnonStoreyF0 {
                    <>f__this = this,
                    portalModel = ObjectManager.CreateObj("EffectPrefabs/chuansong_chufa")
                };
                this.AfterCreateEventModel(yf.portalModel);
                ScheduleMgr.Schedule(0.8f, new System.Action(yf.<>m__67));
                return BattleTriggerType.None;
            }
            if (this.eventType != OutlandEventType.E_OET_BUFF)
            {
                this.ShowGridEventModel();
            }
            switch (this.eventType)
            {
                case OutlandEventType.E_OET_TREASURE:
                    this.Control.isNoneTrigger = true;
                    SocketMgr.Instance.RequestOutlandDropEvent(this.Control.battleGameData.gridGameData.entry, this.Control.battleGameData.gridGameData.map_entry, this.GetIndex());
                    return BattleTriggerType.None;

                case OutlandEventType.E_OET_MONSTER:
                    this.Control.isNoneTrigger = true;
                    if (this.eventMonsterSubType != OutlandMonsterSubType.EN_MONTER_SUB_BOX)
                    {
                        if (<>f__am$cache2B == null)
                        {
                            <>f__am$cache2B = delegate (GUIEntity entity) {
                                SelectHeroPanel panel = (SelectHeroPanel) entity;
                                panel.Depth = 800;
                                panel.mBattleType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
                                panel.SetZhuZhanStat(false);
                            };
                        }
                        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache2B, null);
                    }
                    else
                    {
                        (BattleState.GetInstance().CurGame.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().impl as BattleUIGridImpl).OnChangeUIToGrid(true);
                        if (<>f__am$cache2A == null)
                        {
                            <>f__am$cache2A = delegate {
                                List<long> list = new List<long>();
                                List<Card> list2 = new List<Card>();
                                List<Card> defaultOutlandPkList = ActorData.getInstance().GetDefaultOutlandPkList(ActorData.getInstance().outlandType);
                                defaultOutlandPkList.ForEach(new Action<Card>(list2.Add));
                                foreach (Card card in list2)
                                {
                                    OutlandBattleBackCardInfo outlandCardByEntry = ActorData.getInstance().GetOutlandCardByEntry((int) card.cardInfo.entry);
                                    if ((outlandCardByEntry != null) && (outlandCardByEntry.card_cur_hp == 0))
                                    {
                                        defaultOutlandPkList.Remove(card);
                                    }
                                }
                                if (defaultOutlandPkList.Count == 0)
                                {
                                    (BattleState.GetInstance().CurGame.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().impl as BattleUIGridImpl).OnChangeUIToGrid(false);
                                    if (<>f__am$cache2C == null)
                                    {
                                        <>f__am$cache2C = delegate (GUIEntity entity) {
                                            SelectHeroPanel panel = (SelectHeroPanel) entity;
                                            panel.Depth = 800;
                                            panel.mBattleType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
                                            panel.SetZhuZhanStat(false);
                                        };
                                    }
                                    GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache2C, null);
                                }
                                else
                                {
                                    foreach (Card card2 in defaultOutlandPkList)
                                    {
                                        list.Add(card2.card_id);
                                    }
                                    if (list.Count > 0)
                                    {
                                        SocketMgr.Instance.SelectHeroOutlandCombatReq(list);
                                    }
                                }
                            };
                        }
                        ScheduleMgr.Schedule(2f, <>f__am$cache2A);
                    }
                    goto Label_0386;

                case OutlandEventType.E_OET_DOOR:
                {
                    this.Control.isNoneTrigger = true;
                    if (!this.Control.battleGameData.IsKey)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e2b));
                        return BattleTriggerType.NoTrigger;
                    }
                    OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
                    if (null != gUIEntity)
                    {
                        if (gUIEntity.rewardState != 1)
                        {
                            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                                ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x4e2c), new UIEventListener.VoidDelegate(this.MessageboxOk), new UIEventListener.VoidDelegate(this.MessasgeCanle), false);
                            }, null);
                        }
                        else
                        {
                            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                                ((MessageBox) obj).SetDialog(ConfigMgr.getInstance().GetWord(0x4e2e), new UIEventListener.VoidDelegate(this.MessasgeCanle), null, true);
                            }, null);
                        }
                    }
                    goto Label_0386;
                }
                case OutlandEventType.E_OET_RUSH:
                    goto Label_0386;

                case OutlandEventType.E_OET_BUFF:
                    this.Control.isNoneTrigger = false;
                    TipsDiag.SetText(this.gridBuffName + "\n" + this.gridBuffDes);
                    switch (this.eventBuffSubType)
                    {
                        case OutlandBufferSubType.OL_BT_ATTACK:
                        case OutlandBufferSubType.OL_BT_DEFENCE:
                            this.Control.InitPlayerBuff(this.gridEffect, this.buffEntry, HangPointType.Feet);
                            break;

                        case OutlandBufferSubType.OL_BT_ENERGY:
                        case OutlandBufferSubType.OL_BT_BLESS:
                        case OutlandBufferSubType.OL_BT_DISPEL:
                        case OutlandBufferSubType.OL_BT_DISPEL_GOOD:
                            this.Control.InitPlayerBuff(this.gridEffect, this.buffEntry, HangPointType.Body);
                            break;

                        case OutlandBufferSubType.OL_BT_COMPASS:
                            this.Control.ShowGridShadowModel(OutlandBufferSubType.OL_BT_COMPASS, this.gridEffect);
                            break;

                        case OutlandBufferSubType.OL_BT_MAP:
                            this.Control.ShowGridShadowModel(OutlandBufferSubType.OL_BT_MAP, this.gridEffect);
                            break;

                        case OutlandBufferSubType.OL_BT_TELESCOPE:
                            this.Control.ShowGridShadowModel(OutlandBufferSubType.OL_BT_TELESCOPE, this.gridEffect);
                            break;
                    }
                    SocketMgr.Instance.RequestOutlandBufferEventReq(this.Control.battleGameData.gridGameData.entry, this.Control.battleGameData.gridGameData.map_entry, this.GetIndex());
                    return BattleTriggerType.None;
            }
        }
        return BattleTriggerType.None;
    Label_0386:
        return BattleTriggerType.Trigger;
    }

    public void OnUpdate()
    {
        if (this.eventType == OutlandEventType.E_OET_MONSTER)
        {
            if (this.EventModel != null)
            {
                this.EventModel.transform.LookAt(this.Control.player.transform);
            }
            if (this.EventShadowModel != null)
            {
                this.EventShadowModel.transform.LookAt(this.Control.player.transform);
            }
        }
    }

    public void SetEventData(int entry, int state)
    {
        this.eventState = (OutlandEventState) state;
        this.eventEntry = entry;
        outland_event_config _config = ConfigMgr.getInstance().getByEntry<outland_event_config>(this.eventEntry);
        if (_config != null)
        {
            this.eventType = (OutlandEventType) _config.type;
            switch (this.eventType)
            {
                case OutlandEventType.E_OET_TREASURE:
                case OutlandEventType.E_OET_DOOR:
                    this.gridModel = _config.model;
                    break;

                case OutlandEventType.E_OET_MONSTER:
                    this.eventMonsterSubType = (OutlandMonsterSubType) _config.sub_type;
                    this.gridModel = _config.model;
                    this.gridEffect = _config.effect;
                    BattleGridGameMapControl.allMonsters++;
                    if (this.eventState == OutlandEventState.E_OES_FINISHED)
                    {
                        BattleGridGameMapControl.killMonsters++;
                    }
                    break;

                case OutlandEventType.E_OET_BUFF:
                {
                    outland_buffer_config _config2 = ConfigMgr.getInstance().getByEntry<outland_buffer_config>(_config.param);
                    if (_config2 != null)
                    {
                        this.eventBuffSubType = (OutlandBufferSubType) _config2.buffer_sub_type;
                        this.gridEffect = _config2.effect;
                        this.gridModel = _config2.model;
                        this.buffEntry = _config2.entry;
                        this.gridBuffDes = _config2.remark2;
                        this.buffShowType = _config2.show_type;
                        this.gridBuffName = _config2.remark;
                    }
                    break;
                }
            }
        }
    }

    public void SetPortal(int targetGridIndex, string effectName)
    {
        this.portalTargetGridIndex = targetGridIndex;
        this.EventModel = ObjectManager.CreateObj("EffectPrefabs/" + effectName);
        this.AfterCreateEventModel(this.EventModel);
    }

    public void SetState(int state)
    {
        OutlandEventState state2 = (OutlandEventState) state;
        if (state2 != this.eventState)
        {
            this.eventState = state2;
            if ((this.eventState == OutlandEventState.E_OES_FINISHED) && (this.buffShowType == 0))
            {
                if (this.EventModel != null)
                {
                    ObjectManager.DestoryObj(this.EventModel);
                    this.EventModel = null;
                }
                if (this.tiggerModel != null)
                {
                    ObjectManager.DestoryObj(this.tiggerModel);
                    this.tiggerModel = null;
                }
                if (this.eventType == OutlandEventType.E_OET_MONSTER)
                {
                    if (this.ziseModel != null)
                    {
                        ObjectManager.DestoryObj(this.ziseModel);
                        this.ziseModel = null;
                    }
                    if (this.huangseModel != null)
                    {
                        ObjectManager.DestoryObj(this.huangseModel);
                        this.huangseModel = null;
                    }
                    BattleGridGameMapControl.killMonsters++;
                    Debug.Log(string.Concat(new object[] { "allMonsters=", BattleGridGameMapControl.allMonsters, "***** killMonsters=", BattleGridGameMapControl.killMonsters, string.Empty }));
                    this.Control.SetOlBattlePanelSider(BattleGridGameMapControl.killMonsters, BattleGridGameMapControl.allMonsters);
                }
            }
        }
    }

    public void Show(bool isRightNow)
    {
        if (!this.isShowStarted)
        {
            this.isShowStarted = true;
            if (isRightNow)
            {
                this.model.SetActive(true);
                this.isShow = true;
            }
            else
            {
                this.Control.StartCoroutine(this.StartShow());
            }
            if (this.isBriage)
            {
                this.ShowBriageNearGrid(isRightNow);
            }
            if (this.isShowShadow)
            {
                ObjectManager.DestoryObj(this.EventShadowModel);
                this.EventShadowModel = null;
                this.isShowShadow = false;
            }
            this.CreateGridModel();
        }
    }

    private void ShowBriageNearGrid(bool isRightNow)
    {
        <ShowBriageNearGrid>c__AnonStoreyEF yef = new <ShowBriageNearGrid>c__AnonStoreyEF {
            isRightNow = isRightNow
        };
        this.Control.GetNearGrids(this.X, this.Y).ForEach(new Action<BattleGrid>(yef.<>m__66));
    }

    public void ShowGridEventModel()
    {
        if (!string.IsNullOrEmpty(this.gridEffect))
        {
            if (this.tiggerModel != null)
            {
                this.tiggerModel.SetActive(true);
                this.tiggerModel.transform.LookAt(this.Control.player.transform);
            }
            else
            {
                this.tiggerModel = ObjectManager.CreateObj("BattlePrefabs/" + this.gridEffect + string.Empty);
                this.AfterCreateEventModel(this.tiggerModel);
                this.tiggerModel.transform.LookAt(this.Control.player.transform);
            }
            this.EventModel.SetActive(false);
        }
    }

    private void ShowNextNearGrid(bool isRightNow)
    {
        <ShowNextNearGrid>c__AnonStoreyEE yee = new <ShowNextNearGrid>c__AnonStoreyEE {
            isRightNow = isRightNow
        };
        this.Control.GetNearGrids(this.X, this.Y).ForEach(new Action<BattleGrid>(yee.<>m__65));
    }

    [DebuggerHidden]
    private IEnumerator StartShow()
    {
        return new <StartShow>c__Iterator13 { <>f__this = this };
    }

    public BattleGridGameMapControl Control { get; set; }

    public GameObject EventModel { get; set; }

    public GameObject EventShadowModel { get; set; }

    public GameObject huangseModel { get; set; }

    public bool isPortal { get; set; }

    public bool isShowShadow { get; set; }

    public GameObject model { get; set; }

    public GameObject tiggerModel { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public GameObject ziseModel { get; set; }

    [CompilerGenerated]
    private sealed class <OnTrigger>c__AnonStoreyF0
    {
        internal BattleGrid <>f__this;
        internal GameObject portalModel;

        internal void <>m__67()
        {
            this.<>f__this.Control.SetPortalPlayer(this.<>f__this.portalTargetGridIndex);
            ObjectManager.DestoryObj(this.portalModel);
            this.portalModel = null;
        }
    }

    [CompilerGenerated]
    private sealed class <ShowBriageNearGrid>c__AnonStoreyEF
    {
        internal bool isRightNow;

        internal void <>m__66(BattleGrid obj)
        {
            if (obj != null)
            {
                obj.Show(this.isRightNow);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowNextNearGrid>c__AnonStoreyEE
    {
        internal bool isRightNow;

        internal void <>m__65(BattleGrid obj)
        {
            if (obj != null)
            {
                obj.Show(this.isRightNow);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <StartShow>c__Iterator13 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleGrid <>f__this;
        internal Vector3 <oldPos>__0;
        internal Vector3 <startPos>__1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<oldPos>__0 = this.<>f__this.model.transform.position;
                    this.<startPos>__1 = this.<oldPos>__0;
                    this.<startPos>__1.y -= 3f;
                    this.<>f__this.model.transform.position = this.<startPos>__1;
                    this.<>f__this.model.SetActive(true);
                    if (this.<>f__this.EventModel != null)
                    {
                        this.<>f__this.EventModel.SetActive(false);
                    }
                    iTween.MoveTo(this.<>f__this.model, this.<oldPos>__0, 1f);
                    this.$current = new WaitForSeconds(0.4f);
                    this.$PC = 1;
                    goto Label_0145;

                case 1:
                    if (this.<>f__this.EventModel == null)
                    {
                        break;
                    }
                    this.<>f__this.EventModel.SetActive(true);
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 2;
                    goto Label_0145;

                case 2:
                    break;

                default:
                    goto Label_0143;
            }
            this.<>f__this.isShow = true;
            this.$PC = -1;
        Label_0143:
            return false;
        Label_0145:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

