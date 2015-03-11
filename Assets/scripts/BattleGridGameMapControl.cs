using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleGridGameMapControl : BattleCom_Base
{
    [CompilerGenerated]
    private static Action<BattleGrid> <>f__am$cache21;
    [CompilerGenerated]
    private static Action<BattleGrid> <>f__am$cache22;
    [CompilerGenerated]
    private static Action<BattleGrid> <>f__am$cache23;
    [CompilerGenerated]
    private static Func<BattleGrid, bool> <>f__am$cache24;
    [CompilerGenerated]
    private static Func<BattleGrid, bool> <>f__am$cache25;
    [CompilerGenerated]
    private static Func<BattleGrid, bool> <>f__am$cache26;
    public Action<BattleGrid> actMoveTo;
    public List<BattleGrid> allGrids = new List<BattleGrid>();
    public static int allMonsters;
    public int cameraIndex;
    public GameObject cameraObj;
    public int curTargetGridIndex = -1;
    public Dictionary<int, string> dictionaryBuff = new Dictionary<int, string>();
    public static string GridMapDataName = "OUTLAND";
    public List<GameObject> gridModels = new List<GameObject>();
    private GameObject gridObjList;
    private List<BattleGrid> grids;
    private bool isCanControl = true;
    private bool isGridMoving;
    private bool isMoving;
    public bool isNoneTrigger;
    public static int killMonsters;
    private Vector2 lastClickPosition = Vector2.zero;
    private float lastMoveTime;
    private static float moveInteval = 0.3f;
    private bool moveNeedBeCacenled;
    private List<List<Vector3>> movePaths = new List<List<Vector3>>();
    private int newTargetGridIndex = -1;
    public static int numberOfGrid = 0x19;
    private OutlandBattlePanel opOutlandBattlePanel;
    public GameObject player;
    public List<BattleGrid> showGrids = new List<BattleGrid>();

    private void AddShowGrids(BattleGrid grid)
    {
        if (!this.showGrids.Contains(grid))
        {
            this.showGrids.Add(grid);
        }
    }

    public static int Axis2Index(int x, int y)
    {
        return ((y * numberOfGrid) + x);
    }

    public void BreakFromBattle()
    {
    }

    private void CancelMove(int _newTargetGridIndex)
    {
        this.moveNeedBeCacenled = true;
        this.newTargetGridIndex = _newTargetGridIndex;
    }

    public int ChangeCamera()
    {
        this.cameraIndex = (this.cameraIndex != 0) ? 0 : 1;
        this.OnCameraChange();
        return this.cameraIndex;
    }

    private void CheckMovePath(ref List<int> path)
    {
        List<int> list = new List<int>();
        bool flag = true;
        foreach (int num in path)
        {
            BattleGrid grid = this.GetGrid(num);
            list.Add(num);
            if (!flag && grid.IsMovoPassGrid())
            {
                if (num != path[path.Count - 1])
                {
                    list.RemoveAt(list.Count - 1);
                }
                break;
            }
            flag = false;
        }
        if (path.Count >= 2)
        {
            this.LastPosIndex = path[path.Count - 2];
        }
        path = list;
    }

    private void CutMovePath(int m, List<int> path)
    {
        List<Vector3> item = new List<Vector3>();
        for (int i = m; i < path.Count; i++)
        {
            BattleGrid grid = this.GetGrid(path[i]);
            if (((grid.buffShowType == 1) && (i != (path.Count - 1))) && ((i != 0) && (grid.eventState == OutlandEventState.E_OES_UNFINISHED)))
            {
                item.Add(grid.GetWorldPos());
                this.movePaths.Add(item);
                this.CutMovePath(i + 1, path);
                return;
            }
            item.Add(grid.GetWorldPos());
        }
        Debug.Log(" movePaths add wu");
        this.movePaths.Add(item);
    }

    public BattleGrid GetGrid(int index)
    {
        VectorInt2 num = Index2Axis(index);
        return this.GetGrid(num.x, num.y);
    }

    public BattleGrid GetGrid(int x, int y)
    {
        if (((x < numberOfGrid) && (y < numberOfGrid)) && ((x >= 0) && (y >= 0)))
        {
            return this.grids[x + (numberOfGrid * y)];
        }
        return null;
    }

    public string GetMapName()
    {
        if ((base.battleGameData != null) && (base.battleGameData.gridGameData != null))
        {
            outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(BattleState.GetInstance().OutlandActivityData.activity_data.entry);
            object[] objArray1 = new object[] { GridMapDataName, base.battleGameData.gridGameData.type_entry, base.battleGameData.gridGameData.entry.ToString(), _config.layer.ToString(), base.battleGameData.gridGameData.map_entry.ToString(), ActorData.getInstance().SessionInfo.userid, ServerInfo.lastGameServerId.ToString() };
            return string.Concat(objArray1);
        }
        return GridMapDataName;
    }

    public List<BattleGrid> GetNearGrids(int x, int y)
    {
        List<BattleGrid> list = new List<BattleGrid>();
        BattleGrid grid = this.GetGrid(x, y);
        if (grid != null)
        {
            foreach (VectorInt2 num in grid.GetSurroundPoints())
            {
                BattleGrid item = this.GetGrid(x + num.x, y + num.y);
                if (item != null)
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }

    public static VectorInt2 Index2Axis(int index)
    {
        int num = index / numberOfGrid;
        return new VectorInt2(index - (num * numberOfGrid), num);
    }

    private void InitGrid()
    {
        if (base.battleGameData.gridGameData != null)
        {
            outland_map_config _config = ConfigMgr.getInstance().getByEntry<outland_map_config>(base.battleGameData.gridGameData.map_entry);
            if (_config != null)
            {
                <InitGrid>c__AnonStoreyF3 yf = new <InitGrid>c__AnonStoreyF3 {
                    <>f__this = this
                };
                StrParser.ParseDecIntList(_config.base_point, -1).ForEach(new Action<int>(yf.<>m__6E));
                base.battleGameData.gridGameData.events.ForEach(new Action<OutlandEvent>(yf.<>m__6F));
                this.SetOlBattlePanelSider(killMonsters, allMonsters);
                yf.index = 0;
                yf.modelList = StrParser.ParseStringList(_config.model_point, "|");
                this.allGrids.ForEach(new Action<BattleGrid>(yf.<>m__70));
                if (<>f__am$cache21 == null)
                {
                    <>f__am$cache21 = obj => obj.CheckSurroundPointsNear();
                }
                this.allGrids.ForEach(<>f__am$cache21);
                for (int i = 0; i < 8; i++)
                {
                    FieldInfo field = typeof(outland_map_config).GetField("portal_" + i.ToString());
                    if (field != null)
                    {
                        string str = field.GetValue(_config) as string;
                        if (!string.IsNullOrEmpty(str))
                        {
                            List<string> list2 = StrParser.ParseStringList(str, "|");
                            if ((list2 != null) && (list2.Count >= 2))
                            {
                                int index = StrParser.ParseDecInt(list2[0], -1);
                                int num3 = StrParser.ParseDecInt(list2[1], -1);
                                BattleGrid grid = this.GetGrid(index);
                                BattleGrid grid2 = this.GetGrid(num3);
                                if (grid != null)
                                {
                                    if ((grid2 != null) && (grid2.EventModel != null))
                                    {
                                        grid.SetPortal(num3, "wy_chuansongmen_shuangxiang");
                                        grid2.SetPortal(index, "wy_chuansongmen_shuangxiang");
                                    }
                                    else
                                    {
                                        grid.SetPortal(num3, "wy_chuansongmen_rukou");
                                        if (grid2 != null)
                                        {
                                            grid2.AfterCreateEventModel(ObjectManager.CreateObj("wy_chuansongmen_chukou"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void InitGridGameBattleDate()
    {
        this.gridObjList = new GameObject("GridObjList");
        this.cameraObj = Camera.main.gameObject;
        this.isCanControl = true;
        this.isMoving = false;
        this.curMovedNumber = 0;
        this.curTargetGridIndex = -1;
        this.InitGridModels();
        this.InitGrid();
        this.InitPlayer();
        if (!BattleSceneStarter.G_isTestEnable)
        {
            this.SetBattleIcon();
            this.InitShadowGrid();
        }
        this.Update();
        base.StartCoroutine(this.StartInit());
    }

    private void InitGridModels()
    {
        GameObject obj2 = GameObject.Find("models");
        if (obj2 != null)
        {
            for (int i = 0; i < obj2.transform.childCount; i++)
            {
                this.gridModels.Add(obj2.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Can't find Grid Model Obj");
        }
        this.gridCenterObj = GameObject.Find("grid_center");
    }

    public void InitPlayer()
    {
        if (base.battleGameData.gridGameData != null)
        {
            int headEntry = ActorData.getInstance().UserInfo.headEntry;
            Card cardByEntry = ActorData.getInstance().GetCardByEntry((uint) headEntry);
            this.player = CardPlayer.CreateCardPlayerWithEquip((int) cardByEntry.cardInfo.entry, cardByEntry.equipInfo, CardPlayerStateType.Battle, cardByEntry.cardInfo.quality);
            Transform transform = this.player.transform;
            transform.position += (Vector3) (Vector3.up * 0.1f);
        }
        else
        {
            this.player = CardPlayer.CreateCardPlayer(0, null, CardPlayerStateType.Normal, 1);
        }
        if (!this.LoadMapPoint() && (this.allGrids.Count > 0))
        {
            if (BattleSceneStarter.G_isTestEnable)
            {
                base.battleGameData.gridInitPlayer = 0x19d;
            }
            VectorInt2 num2 = Index2Axis(base.battleGameData.gridInitPlayer);
            BattleGrid grid = this.GetGrid(num2.x, num2.y);
            if (grid != null)
            {
                grid.Show(true);
            }
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = obj => obj.Show(true);
            }
            this.GetNearGrids(num2.x, num2.y).ForEach(<>f__am$cache22);
            this.SetPlayerToGrid(num2.x, num2.y);
        }
    }

    public void InitPlayerBuff(string strBuffer, int buffId, HangPointType type)
    {
        if (!this.dictionaryBuff.ContainsValue(strBuffer))
        {
            this.player.GetComponent<HangControler>().AttachEffect("EffectPrefabs/" + strBuffer + string.Empty, strBuffer, type, (Vector3) (Vector3.up * 0.3f));
            this.dictionaryBuff.Add(buffId, strBuffer);
        }
    }

    private void InitShadowGrid()
    {
        this.dictionaryBuff.Clear();
        foreach (int num in base.battleGameData.buffLists)
        {
            outland_buffer_config _config = ConfigMgr.getInstance().getByEntry<outland_buffer_config>(num);
            if (_config != null)
            {
                switch (_config.buffer_sub_type)
                {
                    case 2:
                    case 3:
                        this.InitPlayerBuff(_config.effect, num, HangPointType.Feet);
                        break;

                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        if (_config.show_type != 1)
                        {
                            this.InitPlayerBuff(_config.effect, num, HangPointType.Body);
                        }
                        break;

                    case 8:
                        this.ShowGridShadowModel(OutlandBufferSubType.OL_BT_COMPASS, _config.effect);
                        break;

                    case 9:
                        this.ShowGridShadowModel(OutlandBufferSubType.OL_BT_MAP, _config.effect);
                        break;

                    case 10:
                        this.ShowGridShadowModel(OutlandBufferSubType.OL_BT_TELESCOPE, _config.effect);
                        break;
                }
            }
        }
    }

    public bool IsCanControled()
    {
        return (!this.isMoving && this.isCanControl);
    }

    private bool LoadMapPoint()
    {
        if (BattleState.GetInstance().CurGame.battleGameData.isNewMap)
        {
            Debug.Log("LoadMapPoint ??????????");
            SettingMgr.mInstance.SetCommonString(this.GetMapName().ToUpper(), string.Empty);
            return false;
        }
        Debug.Log("LoadMapPoint ???????????? ????????????==" + this.GetMapName().ToUpper());
        string commonString = SettingMgr.mInstance.GetCommonString(this.GetMapName().ToUpper());
        if ((base.battleGameData.gridGameData != null) && !string.IsNullOrEmpty(commonString))
        {
            List<int> list = StrParser.ParseDecIntList(commonString, -1);
            if (base.battleGameData.gridGameData.entry == list[0])
            {
                int index = list[1];
                list.RemoveRange(0, 2);
                list.ForEach(delegate (int obj) {
                    BattleGrid grid = this.GetGrid(obj);
                    if (grid != null)
                    {
                        grid.Show(true);
                    }
                });
                VectorInt2 num2 = Index2Axis(index);
                return this.SetPlayerToGrid(num2.x, num2.y);
            }
        }
        return false;
    }

    public void MovePlayerToGrid(int index)
    {
        BattleGrid grid = this.GetGrid(index);
        if (((grid != null) && grid.isShow) && (this.curTargetGridIndex != index))
        {
            if (this.isMoving)
            {
                this.CancelMove(index);
            }
            else
            {
                this.curTargetGridIndex = index;
                this.moveNeedBeCacenled = false;
                this.StartMovePlayerToGrid(index);
                if (this.actMoveTo != null)
                {
                    this.actMoveTo(grid);
                }
            }
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        float num = this.lastClickPosition.x - gesture.position.x;
        float num2 = this.lastClickPosition.y - gesture.position.y;
        if (((num <= -5f) || (num > 5f)) || ((num2 > 5f) || (num2 <= -5f)))
        {
            this.lastClickPosition = gesture.position;
            if ((this.lastMoveTime + moveInteval) <= Time.time)
            {
                RaycastHit hit;
                this.lastMoveTime = Time.time;
                if ((((gesture.fingerIndex == 0) && (Camera.main != null)) && (UICamera.hoveredObject == null)) && ((this.isCanControl && Physics.Raycast(Camera.main.ScreenPointToRay((Vector3) gesture.position), out hit, 100f)) && (UICamera.hoveredObject == null)))
                {
                    int index = StrParser.ParseDecInt(hit.collider.gameObject.name);
                    this.MovePlayerToGrid(index);
                }
            }
        }
    }

    private void OnCameraChange()
    {
        SettingMgr.mInstance.SetGlobalInt("BATTLEGRIDCAMERAINDEX", this.cameraIndex);
        CameraChanger component = this.cameraObj.GetComponent<CameraChanger>();
        if (component != null)
        {
            component.SetCamera(this.cameraIndex, false);
        }
    }

    public override void OnCreateInit()
    {
        base.OnCreateInit();
        this.grids = new List<BattleGrid>(new BattleGrid[numberOfGrid * numberOfGrid]);
        BattleData battleGameData = base.battleGameData;
        battleGameData.OnMsgEnter = (System.Action) Delegate.Combine(battleGameData.OnMsgEnter, new System.Action(this.OnMsgEnter));
        BattleData data2 = base.battleGameData;
        data2.OnMsgLeave = (System.Action) Delegate.Combine(data2.OnMsgLeave, new System.Action(this.OnMsgLeave));
        BattleData data3 = base.battleGameData;
        data3.OnMsgGridGameFinishOneBattle = (Action<bool, bool, BattleNormalGameResult>) Delegate.Combine(data3.OnMsgGridGameFinishOneBattle, new Action<bool, bool, BattleNormalGameResult>(this.OnMsgGridGameFinishOneBattle));
        BattleData data4 = base.battleGameData;
        data4.OnMsgBattleGridTiggerResult = (Action<bool, bool>) Delegate.Combine(data4.OnMsgBattleGridTiggerResult, new Action<bool, bool>(this.OnMsgBattleGridTiggerResult));
        BattleData data5 = base.battleGameData;
        data5.OnMsgBattlePlayerBuff = (Action<List<int>>) Delegate.Combine(data5.OnMsgBattlePlayerBuff, new Action<List<int>>(this.OnMsgBattlePlayerBuff));
        BattleData data6 = base.battleGameData;
        data6.OnMsgEnableControl = (Action<bool>) Delegate.Combine(data6.OnMsgEnableControl, new Action<bool>(this.OnMsgEnableControl));
        BattleData data7 = base.battleGameData;
        data7.OnMsgOutlandCameraEnable = (Action<bool, bool>) Delegate.Combine(data7.OnMsgOutlandCameraEnable, new Action<bool, bool>(this.OnMsgOutlandCameraEnable));
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvent();
    }

    private void OnGridMoveFinish()
    {
        this.isGridMoving = false;
    }

    private bool OnMoveCanceled(List<int> path)
    {
        RaycastHit hit;
        this.isGridMoving = false;
        Ray ray = new Ray(this.player.transform.position + new Vector3(0f, 10f, 0f), Vector3.down);
        if (!Physics.Raycast(ray, out hit, 100f))
        {
            return false;
        }
        int index = StrParser.ParseDecInt(hit.collider.gameObject.name);
        if (this.GetGrid(index) == null)
        {
            return false;
        }
        if ((index == path[path.Count - 1]) && this.GetGrid(index).IsMovoPassGrid())
        {
            return false;
        }
        iTween.Stop(this.player);
        path.RemoveAt(path.Count - 1);
        foreach (int num2 in path)
        {
            if (num2 != index)
            {
                VectorInt2 num3 = Index2Axis(num2);
                this.ShowNearGrid(num3.x, num3.y);
            }
        }
        VectorInt2 num4 = Index2Axis(index);
        if (this.GetGrid(index) != null)
        {
            this.curX = num4.x;
            this.curY = num4.y;
        }
        this.isMoving = false;
        if (this.newTargetGridIndex >= 0)
        {
            this.curTargetGridIndex = this.newTargetGridIndex;
            this.StartMovePlayerToGrid(this.newTargetGridIndex);
            this.newTargetGridIndex = -1;
        }
        return true;
    }

    public void OnMsgBattleGridTiggerResult(bool ispass, bool isBuff = false)
    {
        Debug.Log("OnMsgBattleGridTiggerResult");
        if (base.battleGameData != null)
        {
            base.battleGameData.IsPass = ispass;
            BattleGrid grid = this.GetGrid(base.battleGameData.point);
            this.ReturnToMap(ispass);
            if (!ispass)
            {
                if (grid.eventMonsterSubType == OutlandMonsterSubType.EN_MONTER_SUB_BOX)
                {
                    grid.EventModel.SetActive(true);
                    grid.tiggerModel.SetActive(false);
                }
            }
            else if (grid != null)
            {
                grid.SetState(1);
                if (isBuff)
                {
                    OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
                    if (null != gUIEntity)
                    {
                    }
                    if (grid.buffShowType == 1)
                    {
                        grid.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + grid.gridModel);
                        grid.AfterCreateEventModel(grid.EventModel);
                    }
                }
            }
        }
    }

    public void OnMsgBattlePlayerBuff(List<int> buffs)
    {
        foreach (KeyValuePair<int, string> pair in this.dictionaryBuff)
        {
            if (buffs.Contains(pair.Key))
            {
                break;
            }
            this.player.GetComponent<HangControler>().DetachEffect(pair.Value, HangPointType.Back, 0.7f);
        }
    }

    public void OnMsgEnableControl(bool enable)
    {
        this.isCanControl = enable;
    }

    private void OnMsgEnter()
    {
        this.InitGridGameBattleDate();
    }

    public void OnMsgGridGameFinishOneBattle(bool isWin, bool isBreak, BattleNormalGameResult result)
    {
        base.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>().ClearFighters();
        for (int i = 0; i < 6; i++)
        {
            GameObject obj2 = GameObject.Find("fighter " + i.ToString());
            if (obj2 != null)
            {
                UnityEngine.Object.DestroyObject(obj2, 0.5f);
            }
        }
        BattleStaticEntry.IsInBattle = false;
        if (!isBreak)
        {
            BattleBack data = new BattleBack {
                result = isWin
            };
            OutlandBattleBack back2 = new OutlandBattleBack();
            foreach (BattleGameResultActorInfo info in result.actorInfoes.attackers)
            {
                if (info.cardEntry != -1)
                {
                    OutlandBattleBackCardInfo item = new OutlandBattleBackCardInfo {
                        card_cur_energy = (ushort) info.energy,
                        card_cur_hp = (int) info.hp,
                        card_entry = (ushort) info.cardEntry
                    };
                    back2.self_card_data.Add(item);
                }
            }
            SocketMgr.Instance.RequestOutlandCombatEndReq(base.battleGameData.gridGameData.entry, base.battleGameData.gridGameData.map_entry, base.battleGameData.point, data, back2, this);
            SoundManager.mInstance.ResumeMusic(0.5f);
            GameDataMgr.Instance.DirtyActorStage = true;
        }
        else
        {
            base.StartCoroutine(this.StartTrans(true));
            this.OnMsgBattleGridTiggerResult(false, false);
            OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
            if (gUIEntity != null)
            {
                gUIEntity.CombatBackBuffDele();
            }
        }
    }

    private void OnMsgLeave()
    {
        this.gridModels.Clear();
        this.allGrids.Clear();
        this.showGrids.Clear();
        allMonsters = 0;
        killMonsters = 0;
        this.dictionaryBuff.Clear();
        this.grids = new List<BattleGrid>(new BattleGrid[numberOfGrid * numberOfGrid]);
        CameraChanger component = this.cameraObj.GetComponent<CameraChanger>();
        if (component != null)
        {
            component.enabled = false;
        }
        CardPlayer.DestroyCardPlayer(this.player);
        this.player = null;
        this.isCanControl = true;
        this.isMoving = false;
    }

    public void OnMsgOutlandCameraEnable(bool cameraEnable, bool isQuit)
    {
        if (isQuit)
        {
            SettingMgr.mInstance.SetCommonString(this.GetMapName().ToUpper(), string.Empty);
        }
        base.StartCoroutine(this.StartTrans(cameraEnable));
    }

    public void RefreshGridState(List<OutlandEvent> events)
    {
        events.ForEach(delegate (OutlandEvent obj) {
            BattleGrid grid = this.GetGrid(obj.point);
            if (grid != null)
            {
                grid.SetState(obj.state);
            }
        });
    }

    public void ReturnToMap(bool forward)
    {
        VectorInt2 num;
        this.curTargetGridIndex = -1;
        this.isCanControl = true;
        if (forward)
        {
            num.x = this.curX;
            num.y = this.curY;
        }
        else
        {
            num = Index2Axis(this.LastPosIndex);
        }
        this.SetPlayerToGrid(num.x, num.y);
        this.Update();
    }

    private void SaveCurMapPoint()
    {
        if (base.battleGameData.gridGameData != null)
        {
            <SaveCurMapPoint>c__AnonStoreyF4 yf = new <SaveCurMapPoint>c__AnonStoreyF4 {
                saveData = new List<int>()
            };
            yf.saveData.Add(base.battleGameData.gridGameData.entry);
            yf.saveData.Add(this.LastPosIndex);
            this.allGrids.ForEach(new Action<BattleGrid>(yf.<>m__73));
            yf.data = string.Empty;
            yf.saveData.ForEach(new Action<int>(yf.<>m__74));
            yf.data.Remove(yf.data.Length - 1);
            SettingMgr.mInstance.SetCommonString(this.GetMapName().ToUpper(), yf.data);
        }
    }

    public void SetBattleIcon()
    {
        if (this.opOutlandBattlePanel == null)
        {
            this.opOutlandBattlePanel = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
        }
        if (this.opOutlandBattlePanel != null)
        {
            this.opOutlandBattlePanel.HideBuffGird(false);
            this.opOutlandBattlePanel.SetBuffIcon(base.battleGameData.buffLists);
        }
    }

    public void SetGrid(int x, int y)
    {
        BattleGrid item = new BattleGrid {
            X = x,
            Y = y,
            Control = this
        };
        this.grids[(y * numberOfGrid) + x] = item;
        this.allGrids.Add(item);
    }

    public void SetOlBattlePanelSider(int sgridcount, int allGridcount)
    {
        if (this.opOutlandBattlePanel == null)
        {
            this.opOutlandBattlePanel = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
        }
        if (this.opOutlandBattlePanel != null)
        {
            this.opOutlandBattlePanel.SetPrSlider(sgridcount, allGridcount);
            if (killMonsters < allMonsters)
            {
                this.opOutlandBattlePanel.SetBoxIcon(0);
            }
            else if (killMonsters == allMonsters)
            {
                this.opOutlandBattlePanel.SetBoxIcon(!BattleState.GetInstance().CurGame.battleGameData.isClearBoxReward ? 1 : 2);
            }
        }
    }

    public bool SetPlayerToGrid(int x, int y)
    {
        if (this.GetGrid(x, y) == null)
        {
            return false;
        }
        int num = x;
        this.moveStartX = num;
        this.curX = num;
        num = y;
        this.moveStartY = num;
        this.curY = num;
        this.ShowGrid(x, y);
        this.ShowNearGrid(x, y);
        if (this.player != null)
        {
            this.player.transform.position = this.GetGrid(x, y).GetWorldPos();
        }
        return true;
    }

    public void SetPortalPlayer(int targetGridIndex)
    {
        VectorInt2 num = Index2Axis(targetGridIndex);
        this.SetPlayerToGrid(num.x, num.y);
        this.curTargetGridIndex = -1;
    }

    public void ShowGrid(int x, int y)
    {
        BattleGrid grid = this.GetGrid(x, y);
        if (grid != null)
        {
            grid.Show(false);
        }
    }

    public void ShowGridShadowModel(OutlandBufferSubType sbSubType, string gridEffect)
    {
        List<BattleGrid> list = new List<BattleGrid>();
        switch (sbSubType)
        {
            case OutlandBufferSubType.OL_BT_COMPASS:
                if (<>f__am$cache26 == null)
                {
                    <>f__am$cache26 = g => (g.buffShowType == 1) && (g.eventState == OutlandEventState.E_OES_UNFINISHED);
                }
                list = this.allGrids.Where<BattleGrid>(<>f__am$cache26).ToList<BattleGrid>();
                break;

            case OutlandBufferSubType.OL_BT_MAP:
                if (<>f__am$cache24 == null)
                {
                    <>f__am$cache24 = g => ((g.eventType == OutlandEventType.E_OET_TREASURE) && (g.eventState == OutlandEventState.E_OES_UNFINISHED)) && !g.isShow;
                }
                list = this.allGrids.Where<BattleGrid>(<>f__am$cache24).ToList<BattleGrid>();
                break;

            case OutlandBufferSubType.OL_BT_TELESCOPE:
                if (<>f__am$cache25 == null)
                {
                    <>f__am$cache25 = g => (((g.eventMonsterSubType == OutlandMonsterSubType.EN_MOSTER_SUB_BIG_BOSS) || (g.eventMonsterSubType == OutlandMonsterSubType.EN_MOSTER_SUB_SMALL_BOSS)) && (g.eventState == OutlandEventState.E_OES_UNFINISHED)) && !g.isShow;
                }
                list = this.allGrids.Where<BattleGrid>(<>f__am$cache25).ToList<BattleGrid>();
                break;
        }
        foreach (BattleGrid grid in list)
        {
            if (sbSubType == OutlandBufferSubType.OL_BT_COMPASS)
            {
                grid.Show(false);
                grid.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + grid.gridModel + string.Empty);
                grid.AfterCreateEventModel(grid.EventModel);
                grid.SetState(1);
            }
            else
            {
                grid.isShowShadow = true;
                if (grid.EventShadowModel != null)
                {
                    break;
                }
                grid.EventShadowModel = ObjectManager.CreateObj("EffectPrefabs/" + gridEffect + string.Empty);
                grid.AfterCreateEventModel(grid.EventShadowModel);
            }
        }
    }

    private void ShowNearGrid(int x, int y)
    {
        base.StartCoroutine(this.StartShowNearGrid(x, y));
    }

    private void Start()
    {
        EasyTouch.On_TouchDown += new EasyTouch.TouchDownHandler(this.On_TouchDown);
    }

    [DebuggerHidden]
    private IEnumerator StartInit()
    {
        return new <StartInit>c__Iterator14 { <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartMovePlayer(List<int> path)
    {
        return new <StartMovePlayer>c__Iterator16 { path = path, <$>path = path, <>f__this = this };
    }

    private void StartMovePlayerToGrid(int index)
    {
        VectorInt2 num = Index2Axis(index);
        if ((num.x == this.curX) && (num.y == this.curY))
        {
            if (Vector3.Distance(this.GetGrid(index).GetWorldPos(), this.player.transform.position) > 0.1f)
            {
                this.isMoving = true;
                base.StartCoroutine(this.StartMovePlayer(new List<int> { index, index }));
                return;
            }
        }
        else
        {
            List<int> path = null;
            path = BattleGridPath.GetPath(this.allGrids, false, this.curX, this.curY, num.x, num.y);
            if (path != null)
            {
                this.CheckMovePath(ref path);
                if (path.Count > 1)
                {
                    this.isMoving = true;
                    base.StartCoroutine(this.StartMovePlayer(path));
                    return;
                }
            }
        }
        this.curTargetGridIndex = -1;
    }

    [DebuggerHidden]
    private IEnumerator StartShowNearGrid(int x, int y)
    {
        return new <StartShowNearGrid>c__Iterator17 { x = x, y = y, <$>x = x, <$>y = y, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator StartTrans(bool cameraEnable)
    {
        return new <StartTrans>c__Iterator15 { cameraEnable = cameraEnable, <$>cameraEnable = cameraEnable, <>f__this = this };
    }

    private void TriggerGrid(int x, int y)
    {
        base.battleGameData.point = Axis2Index(x, y);
        switch (this.GetGrid(x, y).OnTrigger())
        {
            case BattleTriggerType.None:
                this.isNoneTrigger = false;
                this.ShowNearGrid(x, y);
                break;

            case BattleTriggerType.Trigger:
                this.isNoneTrigger = false;
                this.isCanControl = false;
                break;

            case BattleTriggerType.NoTrigger:
                this.OnMsgBattleGridTiggerResult(false, false);
                break;
        }
    }

    private void UnsubscribeEvent()
    {
        EasyTouch.On_TouchDown -= new EasyTouch.TouchDownHandler(this.On_TouchDown);
    }

    private void Update()
    {
        if (<>f__am$cache23 == null)
        {
            <>f__am$cache23 = delegate (BattleGrid obj) {
                if (obj != null)
                {
                    obj.OnUpdate();
                }
            };
        }
        this.allGrids.ForEach(<>f__am$cache23);
    }

    private int curMovedNumber { get; set; }

    public int curX { get; set; }

    public int curY { get; set; }

    public GameObject gridCenterObj { get; set; }

    private int LastPosIndex { get; set; }

    public int moveStartX { get; set; }

    public int moveStartY { get; set; }

    [CompilerGenerated]
    private sealed class <InitGrid>c__AnonStoreyF3
    {
        internal BattleGridGameMapControl <>f__this;
        internal int index;
        internal List<string> modelList;

        internal void <>m__6E(int obj)
        {
            VectorInt2 num = BattleGridGameMapControl.Index2Axis(obj);
            this.<>f__this.SetGrid(num.x, num.y);
        }

        internal void <>m__6F(OutlandEvent obj)
        {
            BattleGrid grid = this.<>f__this.GetGrid(obj.point);
            if (grid != null)
            {
                grid.SetEventData(obj.entry, obj.state);
            }
        }

        internal void <>m__70(BattleGrid gridObj)
        {
            <InitGrid>c__AnonStoreyF2 yf = new <InitGrid>c__AnonStoreyF2 {
                <>f__ref$243 = this,
                gridObj = gridObj
            };
            string str = "1";
            OutlandUser user = null;
            if (yf.gridObj.eventType == OutlandEventType.E_OET_RUSH)
            {
                user = this.<>f__this.battleGameData.gridGameData.user.Find(new Predicate<OutlandUser>(yf.<>m__7B));
            }
            if (this.index < this.modelList.Count)
            {
                str = this.modelList[this.index];
            }
            if (string.IsNullOrEmpty(str))
            {
                str = "1";
            }
            yf.gridObj.Init(this.<>f__this.gridObjList, str, user);
            if ((yf.gridObj.buffShowType == 1) && (yf.gridObj.eventState == OutlandEventState.E_OES_FINISHED))
            {
                yf.gridObj.EventModel = ObjectManager.CreateObj("BattlePrefabs/" + yf.gridObj.gridModel);
                yf.gridObj.AfterCreateEventModel(yf.gridObj.EventModel);
            }
            this.index++;
        }

        private sealed class <InitGrid>c__AnonStoreyF2
        {
            internal BattleGridGameMapControl.<InitGrid>c__AnonStoreyF3 <>f__ref$243;
            internal BattleGrid gridObj;

            internal bool <>m__7B(OutlandUser userObj)
            {
                return (userObj.point == BattleGridGameMapControl.Axis2Index(this.gridObj.X, this.gridObj.Y));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SaveCurMapPoint>c__AnonStoreyF4
    {
        internal string data;
        internal List<int> saveData;

        internal void <>m__73(BattleGrid obj)
        {
            if (obj.isShow)
            {
                this.saveData.Add(BattleGridGameMapControl.Axis2Index(obj.X, obj.Y));
            }
        }

        internal void <>m__74(int obj)
        {
            this.data = this.data + obj.ToString();
            this.data = this.data + "|";
        }
    }

    [CompilerGenerated]
    private sealed class <StartInit>c__Iterator14 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattleGridGameMapControl <>f__this;
        internal CameraChanger <cameraChanger>__1;
        internal BattleCom_UIControl <uiControl>__0;

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
                    this.<uiControl>__0 = this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>();
                    break;

                case 1:
                    break;

                default:
                    goto Label_00E4;
            }
            if (!this.<uiControl>__0.IsLoadFinish())
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            this.<>f__this.cameraIndex = SettingMgr.mInstance.GetGlobalInt("BATTLEGRIDCAMERAINDEX", 0);
            this.<cameraChanger>__1 = this.<>f__this.cameraObj.GetComponent<CameraChanger>();
            if (this.<cameraChanger>__1 != null)
            {
                this.<cameraChanger>__1.SetLookAt(this.<>f__this.player);
                this.<cameraChanger>__1.SetCamera(this.<>f__this.cameraIndex, true);
            }
            TransSceneUI.EndTransition();
            LoadingPerfab.EndTransition();
            this.$PC = -1;
        Label_00E4:
            return false;
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

    [CompilerGenerated]
    private sealed class <StartMovePlayer>c__Iterator16 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<int> <$>path;
        internal List<List<Vector3>>.Enumerator <$s_190>__2;
        internal BattleGridGameMapControl <>f__this;
        internal HangControler <con>__5;
        internal HangControler <con>__8;
        internal VectorInt2 <endAxis>__6;
        internal BattleGrid <grid>__4;
        internal int <indexPath>__1;
        internal BattleGrid <lastGrid>__0;
        internal List<Vector3> <movePath1>__3;
        internal BattleGrid <tiggerGrid>__7;
        internal List<int> path;

        internal bool <>m__7C(int g)
        {
            return (this.<>f__this.GetGrid(g).GetWorldPos() == this.<movePath1>__3[this.<movePath1>__3.Count - 1]);
        }

        internal void <>m__7D(int obj)
        {
            VectorInt2 num = BattleGridGameMapControl.Index2Axis(obj);
            this.<>f__this.ShowNearGrid(num.x, num.y);
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                case 2:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_190>__2.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    if (this.path.Count <= 0)
                    {
                        goto Label_0BA6;
                    }
                    this.<lastGrid>__0 = this.<>f__this.GetGrid(this.path[this.path.Count - 1]);
                    if (this.<lastGrid>__0 != null)
                    {
                        ObjectManager.CreateTempObj("EffectPrefabs/Jiantoudianji", this.<lastGrid>__0.GetWorldPos(), 1f);
                    }
                    this.<>f__this.movePaths.Clear();
                    Debug.Log("start " + this.<>f__this.movePaths.Count.ToString());
                    this.<>f__this.CutMovePath(0, this.path);
                    if (this.<>f__this.movePaths.Count <= 1)
                    {
                        if (this.<>f__this.movePaths.Count == 1)
                        {
                            Debug.Log("???????? " + this.<>f__this.movePaths.Count.ToString());
                            this.<>f__this.isGridMoving = true;
                            Debug.Log("path " + this.<>f__this.movePaths[0].Count.ToString());
                            if (((this.<lastGrid>__0 != null) && this.<lastGrid>__0.IsMovoPassGrid()) && ((this.<lastGrid>__0.eventType == OutlandEventType.E_OET_MONSTER) || (this.<lastGrid>__0.eventType == OutlandEventType.E_OET_DOOR)))
                            {
                                this.<>f__this.movePaths[0][this.<>f__this.movePaths[0].Count - 1] = (Vector3) ((this.<>f__this.movePaths[0][this.<>f__this.movePaths[0].Count - 1] + this.<>f__this.movePaths[0][this.<>f__this.movePaths[0].Count - 2]) / 2f);
                            }
                            this.<>f__this.movePaths[0].RemoveAt(0);
                            if (this.<>f__this.movePaths[0].Count > 1)
                            {
                                object[] args = new object[] { "path", this.<>f__this.movePaths[0].ToArray(), "movetopath", true, "speed", 15, "easetype", iTween.EaseType.linear, "orienttopath", true, "oncomplete", "OnGridMoveFinish", "oncompletetarget", this.<>f__this.gameObject };
                                iTween.MoveTo(this.<>f__this.player, iTween.Hash(args));
                            }
                            else
                            {
                                object[] objArray4 = new object[] { "position", this.<>f__this.movePaths[0][0], "movetopath", true, "speed", 5, "easetype", iTween.EaseType.linear, "orienttopath", true, "oncomplete", "OnGridMoveFinish", "oncompletetarget", this.<>f__this.gameObject };
                                iTween.MoveTo(this.<>f__this.player, iTween.Hash(objArray4));
                            }
                            this.<>f__this.player.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.MoveAnimName, 1f, 0f, false);
                        }
                        goto Label_0912;
                    }
                    Debug.Log("???????? " + this.<>f__this.movePaths.Count.ToString());
                    this.<indexPath>__1 = 0;
                    this.<$s_190>__2 = this.<>f__this.movePaths.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                case 2:
                    break;

                case 3:
                    goto Label_0912;

                case 4:
                    goto Label_0AD9;

                case 5:
                    this.<con>__8.DetachEffect("GRIDPLAYERGUANGQUAN", HangPointType.Top, 0.1f);
                    goto Label_0B1C;

                default:
                    goto Label_0BB9;
            }
            try
            {
                switch (num)
                {
                    case 1:
                        goto Label_050B;

                    case 2:
                        goto Label_0539;
                }
                while (this.<$s_190>__2.MoveNext())
                {
                    this.<movePath1>__3 = this.<$s_190>__2.Current;
                    Debug.Log(this.<movePath1>__3.Count.ToString());
                    Debug.Log("indexPath==" + this.<indexPath>__1);
                    this.<>f__this.isGridMoving = true;
                    this.<indexPath>__1++;
                    Debug.Log("indexPath==" + this.<indexPath>__1);
                    if ((((this.<indexPath>__1 == this.<>f__this.movePaths.Count) && (this.<lastGrid>__0 != null)) && this.<lastGrid>__0.IsMovoPassGrid()) && ((this.<lastGrid>__0.eventType == OutlandEventType.E_OET_MONSTER) || (this.<lastGrid>__0.eventType == OutlandEventType.E_OET_DOOR)))
                    {
                        this.<movePath1>__3[this.<movePath1>__3.Count - 1] = (Vector3) ((this.<movePath1>__3[this.<movePath1>__3.Count - 1] + this.<movePath1>__3[this.<movePath1>__3.Count - 2]) / 2f);
                    }
                    Debug.Log("movePath1==" + this.<movePath1>__3.Count);
                    if (this.<indexPath>__1 == 1)
                    {
                        this.<movePath1>__3.RemoveAt(0);
                    }
                    if (this.<movePath1>__3.Count > 1)
                    {
                        object[] objArray1 = new object[] { "path", this.<movePath1>__3.ToArray(), "movetopath", true, "speed", 15, "easetype", iTween.EaseType.linear, "orienttopath", true, "oncomplete", "OnGridMoveFinish", "oncompletetarget", this.<>f__this.gameObject };
                        iTween.MoveTo(this.<>f__this.player, iTween.Hash(objArray1));
                    }
                    else
                    {
                        object[] objArray2 = new object[] { "position", this.<movePath1>__3[0], "movetopath", true, "speed", 5, "easetype", iTween.EaseType.linear, "orienttopath", true, "oncomplete", "OnGridMoveFinish", "oncompletetarget", this.<>f__this.gameObject };
                        iTween.MoveTo(this.<>f__this.player, iTween.Hash(objArray2));
                    }
                    this.<>f__this.player.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.MoveAnimName, 1f, 0f, false);
                    if (this.<indexPath>__1 >= this.<>f__this.movePaths.Count)
                    {
                        continue;
                    }
                    this.<grid>__4 = this.<>f__this.GetGrid(this.path.Find(new Predicate<int>(this.<>m__7C)));
                    this.<>f__this.TriggerGrid(this.<grid>__4.X, this.<grid>__4.Y);
                    this.<con>__5 = this.<>f__this.player.GetComponent<HangControler>();
                    this.<con>__5.AttachEffect("EffectPrefabs/Public_Public_BeStatus_Palsy", "GRIDPLAYERGUANGQUAN", HangPointType.Top);
                    this.<>f__this.player.GetComponent<AnimFSM>().StopCurAnim(BattleGlobal.MoveAnimName);
                Label_050B:
                    while (this.<grid>__4.eventState == OutlandEventState.E_OES_UNFINISHED)
                    {
                        Debug.Log("??????.......");
                        this.$current = new WaitForSeconds(0.1f);
                        this.$PC = 1;
                        flag = true;
                        goto Label_0BBB;
                    }
                    this.$current = new WaitForSeconds(3f);
                    this.$PC = 2;
                    flag = true;
                    goto Label_0BBB;
                Label_0539:
                    this.<>f__this.player.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.MoveAnimName, 1f, 0f, false);
                    this.<con>__5.DetachEffect("GRIDPLAYERGUANGQUAN", HangPointType.Top, 0.1f);
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_190>__2.Dispose();
            }
        Label_0912:
            while (this.<>f__this.isGridMoving)
            {
                if (this.<>f__this.moveNeedBeCacenled)
                {
                    this.<>f__this.moveNeedBeCacenled = false;
                    if (this.<>f__this.OnMoveCanceled(this.path))
                    {
                        goto Label_0BB9;
                    }
                    this.<>f__this.newTargetGridIndex = -1;
                }
                else
                {
                    this.$current = null;
                    this.$PC = 3;
                    goto Label_0BBB;
                }
            }
            this.<>f__this.player.GetComponent<AnimFSM>().StopCurAnim(BattleGlobal.MoveAnimName);
            this.<>f__this.player.GetComponent<AnimFSM>().PlayAnim(BattleGlobal.FightStandAnimName, 1f, 0f, false);
            this.<endAxis>__6 = BattleGridGameMapControl.Index2Axis(this.path[this.path.Count - 1]);
            this.<tiggerGrid>__7 = this.<>f__this.GetGrid(this.<endAxis>__6.x, this.<endAxis>__6.y);
            if (this.<tiggerGrid>__7 != null)
            {
                int x = this.<endAxis>__6.x;
                this.<>f__this.moveStartX = x;
                this.<>f__this.curX = x;
                x = this.<endAxis>__6.y;
                this.<>f__this.moveStartY = x;
                this.<>f__this.curY = x;
            }
            this.path.RemoveAt(this.path.Count - 1);
            this.path.ForEach(new Action<int>(this.<>m__7D));
            this.<>f__this.TriggerGrid(this.<endAxis>__6.x, this.<endAxis>__6.y);
            if (((this.<tiggerGrid>__7 == null) || (this.<tiggerGrid>__7.buffShowType != 1)) || (this.<tiggerGrid>__7.eventState != OutlandEventState.E_OES_UNFINISHED))
            {
                goto Label_0B1C;
            }
            this.<con>__8 = this.<>f__this.player.GetComponent<HangControler>();
            this.<con>__8.AttachEffect("EffectPrefabs/Public_Public_BeStatus_Palsy", "GRIDPLAYERGUANGQUAN", HangPointType.Top);
        Label_0AD9:
            while (this.<tiggerGrid>__7.eventState == OutlandEventState.E_OES_UNFINISHED)
            {
                Debug.Log("??????.......");
                this.$current = new WaitForSeconds(0.1f);
                this.$PC = 4;
                goto Label_0BBB;
            }
            this.$current = new WaitForSeconds(1.5f);
            this.$PC = 5;
            goto Label_0BBB;
        Label_0B1C:
            if (this.<>f__this.isNoneTrigger || !this.<>f__this.isCanControl)
            {
                if (this.path.Count >= 2)
                {
                    this.<>f__this.LastPosIndex = this.path[this.path.Count - 2];
                }
            }
            else
            {
                this.<>f__this.LastPosIndex = BattleGridGameMapControl.Axis2Index(this.<endAxis>__6.x, this.<endAxis>__6.y);
            }
            this.<>f__this.SaveCurMapPoint();
        Label_0BA6:
            this.<>f__this.isMoving = false;
            this.$PC = -1;
        Label_0BB9:
            return false;
        Label_0BBB:
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

    [CompilerGenerated]
    private sealed class <StartShowNearGrid>c__Iterator17 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>x;
        internal int <$>y;
        internal List<BattleGrid>.Enumerator <$s_192>__1;
        internal BattleGridGameMapControl <>f__this;
        internal BattleGrid <grid>__2;
        internal List<BattleGrid> <gridList>__0;
        internal int x;
        internal int y;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_192>__1.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    this.<gridList>__0 = this.<>f__this.GetNearGrids(this.x, this.y);
                    this.<$s_192>__1 = this.<gridList>__0.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00E1;
            }
            try
            {
                while (this.<$s_192>__1.MoveNext())
                {
                    this.<grid>__2 = this.<$s_192>__1.Current;
                    if (!this.<grid>__2.isShowStarted)
                    {
                        this.<grid>__2.Show(false);
                        this.$current = new WaitForSeconds(0.4f);
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_192>__1.Dispose();
            }
            this.$PC = -1;
        Label_00E1:
            return false;
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

    [CompilerGenerated]
    private sealed class <StartTrans>c__Iterator15 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal bool <$>cameraEnable;
        internal BattleGridGameMapControl <>f__this;
        internal bool cameraEnable;

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
                    TransSceneUI.BeginTransition();
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_00CE;

                case 1:
                    this.<>f__this.cameraObj.SetActive(this.cameraEnable);
                    this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_CameraManager>().SetEnable(!this.cameraEnable);
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 2;
                    goto Label_00CE;

                case 2:
                    (this.<>f__this.battleGameData.battleComObject.GetComponent<BattleCom_UIControl>().impl as BattleUIGridImpl).OnChangeUIToGrid(false);
                    TransSceneUI.EndTransition();
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00CE:
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

