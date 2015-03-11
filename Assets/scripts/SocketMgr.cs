using Battle;
using FastBuf;
using ManagedLZO;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Toolbox;
using UnityEngine;

public class SocketMgr : MonoBehaviour
{
    private bool _hasRecvBuf;
    private byte _majorVer;
    private byte _minorVer;
    private int _newEntry;
    private OpcodeType _opcode;
    private byte _option;
    private Queue<Packet> _pendingSendQueue = new Queue<Packet>();
    private byte _protoVer;
    private int _reconnectTimes;
    private byte _reserVer;
    private int _size;
    private Socket _tcpSock;
    private static int _totalBytesReceived;
    private static int _totalBytesSent;
    [CompilerGenerated]
    private static Action<long> <>f__am$cache26;
    [CompilerGenerated]
    private static Action<CombatDetailActor> <>f__am$cache27;
    [CompilerGenerated]
    private static Action<CombatTeam> <>f__am$cache28;
    [CompilerGenerated]
    private static Comparison<CardInfo> <>f__am$cache29;
    [CompilerGenerated]
    private static System.Action <>f__am$cache2A;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2B;
    [CompilerGenerated]
    private static Action<bool, BattleNormalGameType, BattleNormalGameResult> <>f__am$cache2C;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2D;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2E;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache2F;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache30;
    [CompilerGenerated]
    private static System.Action <>f__am$cache31;
    [CompilerGenerated]
    private static System.Action <>f__am$cache32;
    [CompilerGenerated]
    private static System.Action <>f__am$cache33;
    [CompilerGenerated]
    private static Predicate<GuildMember> <>f__am$cache34;
    [CompilerGenerated]
    private static System.Action <>f__am$cache35;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache36;
    [CompilerGenerated]
    private static ActivityShopPanel.CalFixedCost <>f__am$cache37;
    [CompilerGenerated]
    private static ActivityShopPanel.CalRefreshCost <>f__am$cache38;
    [CompilerGenerated]
    private static ActivityShopPanel.CalFixedCost <>f__am$cache39;
    [CompilerGenerated]
    private static ActivityShopPanel.CalRefreshCost <>f__am$cache3A;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3B;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3C;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3D;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3E;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache3F;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache40;
    [CompilerGenerated]
    private static Action<CombatDetailActor> <>f__am$cache41;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache42;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache43;
    private BattleGridGameMapControl battleGridGameMapControl;
    public System.Action EnterAction;
    public System.Action FSMAction;
    public static int HEADER_SIZE = 11;
    public static SocketMgr Instance;
    private string ip;
    private bool isEndConnect;
    public bool isLockGUI;
    private bool isReSending;
    public bool isSending;
    public System.Action LoadingAction;
    public static bool NetConnectIsOk;
    public static byte PACKAGE_OPTION_MASK_COMPRESS = 1;
    private int port;
    private OutlandBattleBack resulttBattleBack;
    private Packet send_pak;
    public OpcodeType sendingOpCode;
    public static int SOCKET_CONNECT__RECONNECT_TIMES = 2;
    public static int SOCKET_CONNECT_BEGIN_TIME_OUT = 7;
    public static int SOCKET_CONNECT_TIME_OUT = 7;
    private int version;
    private byte[] volite_buf = new byte[CircleBuffer.MAX_RECEIVE_BUFFER_SIZE];

    private void Awake()
    {
        Instance = this;
    }

    public void C2S_CollectExchange(FastBuf.C2S_CollectExchange req)
    {
        Packet pak = new Packet(OpcodeType.C2S_COLLECTEXCHANGE) {
            PacketObject = req
        };
        this.Send(pak);
    }

    public void C2S_TXBUYSTOREITEM(C2S_TXBuyStoreItem req)
    {
        Packet pak = new Packet(OpcodeType.C2S_TXBUYSTOREITEM) {
            PacketObject = req
        };
        this.Send(pak);
    }

    public bool CheckActiveIsNewAndNotClick(List<ActiveInfo> activeList)
    {
        bool flag = false;
        foreach (ActiveInfo info in activeList)
        {
            if (info.is_new && this.ChickNew(info))
            {
                return true;
            }
            flag = false;
        }
        return flag;
    }

    public void CheckActiveState(List<ActiveInfo> activeList)
    {
        if ((this.SetCurActiveCompleteIsOk(activeList) || this.CheckActiveIsNewAndNotClick(activeList)) || ActorData.getInstance().GetAllActiveCompleteIsOk())
        {
            if (GameDataMgr.Instance != null)
            {
                GameDataMgr.Instance.ActiveIsNew = true;
            }
        }
        else if (GameDataMgr.Instance != null)
        {
            GameDataMgr.Instance.ActiveIsNew = false;
        }
        if ((GameDataMgr.Instance != null) && !GameDataMgr.Instance.ActiveIsNew)
        {
            Instance.RequestRewardFlag();
        }
    }

    public void CheckLivenessReward()
    {
    }

    protected bool CheckResultCodeAndReLogin(OpResult result)
    {
        <CheckResultCodeAndReLogin>c__AnonStorey142 storey = new <CheckResultCodeAndReLogin>c__AnonStorey142 {
            result = result
        };
        OpResult result2 = storey.result;
        switch (result2)
        {
            case OpResult.OpResult_Ok:
                return true;

            case OpResult.OpResult_ReLogin:
            case OpResult.OpResult_ReLoad:
                Debug.LogWarning("ErrorCode:" + storey.result.ToString());
                LoginPanel.RestLoginPanel();
                GameStateMgr.IsGameReturnLogin = true;
                GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
                return false;

            case OpResult.OpResult_Kick:
                Debug.LogWarning("ErrorCode:" + storey.result.ToString());
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652b7));
                GameStateMgr.IsGameReturnLogin = true;
                BattleStaticEntry.TryClearBattleOnError();
                GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
                return false;

            case OpResult.OpResult_Ver_Err_0:
            case OpResult.OpResult_Ver_Err_1:
            case OpResult.OpResult_Ver_Err_2:
                if (!GameDefine.getInstance().isNeedUpdateFromYYB)
                {
                    if (<>f__am$cache3E == null)
                    {
                        <>f__am$cache3E = delegate (GUIEntity obj) {
                            MessageBox box = (MessageBox) obj;
                            box.Depth = 0x44c;
                            if (<>f__am$cache42 == null)
                            {
                                <>f__am$cache42 = box => Application.OpenURL(GameDefine.getInstance().GetInstallURL());
                            }
                            box.SetDialog(ConfigMgr.getInstance().GetWord(410), <>f__am$cache42, null, true);
                        };
                    }
                    GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache3E, null);
                }
                else
                {
                    GameDefine.getInstance().isUpdateMsgBoxShow = true;
                    if (<>f__am$cache3D == null)
                    {
                        <>f__am$cache3D = delegate (GUIEntity obj) {
                            <CheckResultCodeAndReLogin>c__AnonStorey141 storey = new <CheckResultCodeAndReLogin>c__AnonStorey141 {
                                gui = (UpdateMessageBox) obj
                            };
                            storey.gui.DownloadMsgLabel.gameObject.SetActive(false);
                            storey.gui.DownloadSlider.gameObject.SetActive(false);
                            PlatformInterface.mInstance.SetupUpdateProgressAction(new Action<int, string>(storey.<>m__FE));
                            storey.gui.SetDialog(ConfigMgr.getInstance().GetWord(0x186ae), new UIEventListener.VoidDelegate(storey.<>m__FF), new UIEventListener.VoidDelegate(storey.<>m__100), false);
                            storey.gui.Depth = 0x4e2;
                        };
                    }
                    GUIMgr.Instance.DoModelGUI("UpdateMessageBox", <>f__am$cache3D, null);
                }
                break;

            case OpResult.OpResult_Ver_Err_3:
                if (<>f__am$cache3F == null)
                {
                    <>f__am$cache3F = delegate (GUIEntity obj) {
                        MessageBox box = (MessageBox) obj;
                        box.Depth = 0x44c;
                        if (<>f__am$cache43 == null)
                        {
                            <>f__am$cache43 = delegate (GameObject box) {
                            };
                        }
                        box.SetDialog(ConfigMgr.getInstance().GetWord(0x19b), <>f__am$cache43, null, true);
                    };
                }
                GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache3F, null);
                return false;

            case OpResult.OpResult_Repeated_Req:
            case OpResult.OpResult_Cheat:
                Debug.LogWarning("ErrorCode:" + storey.result.ToString());
                TipsDiag.SetText(ConfigMgr.getInstance().GetErrorCode(storey.result));
                BattleStaticEntry.TryExitBattleOnError();
                return false;

            case OpResult.OpResult_QueryGuildBindInfo_Failed:
            case OpResult.OpResult_GuildUnBindQQGroup_Failed:
            case OpResult.OpResult_GetJoinQQGroupKey_Failed:
                return false;

            case OpResult.OpResult_Login_S:
            case OpResult.OpResult_Login_NoChar:
                return false;

            case OpResult.OpResult_Login_Failed:
                GameStateMgr.IsGameReturnLogin = true;
                GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
                LoginPanel.RestLoginPanel();
                TipsDiag.SetText(ConfigMgr.getInstance().GetErrorCode(storey.result));
                GameDefine.getInstance().OnGameLoginFailed();
                return false;

            case OpResult.OpResult_QQFriendsInGame_Wait:
                return false;

            case OpResult.OpResult_CAN_PAY_FLAG_Wait:
                return false;

            default:
                switch (result2)
                {
                    case OpResult.OpResult_Pay_Stone_S:
                        Debug.Log("PAY OpResult.OpResult_Pay_Stone_S");
                        this.isSending = true;
                        this.isReSending = true;
                        this.ReSendLastPakLogic(0.2f);
                        return false;

                    case OpResult.OpResult_Tx_Login_Invalid:
                        Debug.LogWarning("ErrorCode:" + storey.result.ToString());
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__F7), null);
                        return false;

                    case OpResult.OpResult_Not_Enough_ShakeCount:
                        return false;

                    default:
                        Debug.LogWarning("ErrorCode:" + storey.result.ToString());
                        TipsDiag.SetText(ConfigMgr.getInstance().GetErrorCode(storey.result));
                        return false;
                }
                break;
        }
        return false;
    }

    private void CheckSendLoop()
    {
        if ((this._pendingSendQueue.Count != 0) && !this.isSending)
        {
            this.TrySend();
        }
    }

    private bool ChickNew(ActiveInfo info)
    {
        if (!SettingMgr.mInstance.GetActiveBool(string.Empty + info.activity_unid))
        {
            return true;
        }
        info.is_new = false;
        return false;
    }

    private void CloseWaitGUI()
    {
        WaitPanelHelper.HideWaitPanel("SocketMgrWaitGUI");
    }

    public void CommitDailyQuest(int quest_entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_COMMITDAILYQUEST);
        C2S_CommitDailyQuest quest = new C2S_CommitDailyQuest {
            dailyQuestEntry = quest_entry
        };
        pak.PacketObject = quest;
        this.Send(pak);
    }

    public void CommitQuest(int entry)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_COMMITQUEST);
        C2S_CommitQuest quest = new C2S_CommitQuest();
        pak.PacketObject = quest;
        quest.questEntry = entry;
        this.Send(pak);
    }

    public void Connect(string _ip, int _port, List<string> backUpIPs)
    {
        this.ip = _ip;
        this.port = _port;
        this.ip = CommonFunc.CheckHostIP(this.ip, backUpIPs);
        this.DebugLog("Connect gameserver :" + this.ip + ":" + this.port.ToString());
    }

    private void DebugLog(string text)
    {
    }

    private void DoWaitGUI()
    {
        if (this.IsHasNeedLockUIMsg())
        {
            WaitPanelHelper.ShowWaitPanel("SocketMgrWaitGUI");
        }
    }

    public void DRAWACTIVITYPRIZE(C2S_DrawActivityPrize req)
    {
        Packet pak = new Packet(OpcodeType.C2S_DRAWACTIVITYPRIZE) {
            PacketObject = req
        };
        this.Send(pak);
    }

    private void EnterGrid(S2C_OutlandDataEnter res)
    {
        ActorData.getInstance().OutlandCardStatList.Clear();
        if (res.data.card_simple_data.self_card_data.Count > 0)
        {
            OutlandBattleBack back = new OutlandBattleBack();
            foreach (OutlandBattleBackCardInfo info in res.data.card_simple_data.self_card_data)
            {
                if (info.card_entry != -1)
                {
                    OutlandBattleBackCardInfo item = new OutlandBattleBackCardInfo {
                        card_cur_energy = (ushort) info.card_cur_energy,
                        card_cur_hp = info.card_cur_hp,
                        card_entry = info.card_entry
                    };
                    back.self_card_data.Add(item);
                }
            }
            foreach (OutlandBattleBackCardInfo info3 in back.self_card_data)
            {
                ActorData.getInstance().UpdateOutlandCardStatList(info3);
            }
        }
        OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
        if (gUIEntity != null)
        {
            if (res.data.activity_data.floor_key)
            {
                gUIEntity.SetKey();
            }
            else
            {
                gUIEntity.SetKeyEnable(false);
            }
        }
        if (res.phyForce >= 0)
        {
            ActorData.getInstance().PhyForce = res.phyForce;
        }
        BattleStaticEntry.DoDupBattleGrid(res.data, res.isNewMap);
        if (res.isNewMap)
        {
            Debug.Log("S2C_OutlandDataEnter EnterGrid 进入新地图");
        }
        else
        {
            Debug.Log("S2C_OutlandDataEnter EnterGrid 进入旧地图");
        }
    }

    private void EnterParkourScene(string type_event)
    {
        CardPool.ClearCache();
        int paokuMapEntry = ActorData.getInstance().paokuMapEntry;
        guildrun_config _config = ConfigMgr.getInstance().getByEntry<guildrun_config>(paokuMapEntry);
        ParkourInit._instance.SetParkouChapteAndRandomPropIndex(ActorData.getInstance().paokuPropIndex, paokuMapEntry);
        if (_config != null)
        {
            GameStateMgr.Instance.ChangeStateWithParameter(type_event, "Parkour_" + _config.map);
        }
    }

    private void ForwardNetWorkEvent(OpcodeType opCodeEvent, object paket)
    {
    }

    public void GetActiveList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETACTIVITYDATA);
        C2S_GetActivityData data = new C2S_GetActivityData();
        pak.PacketObject = data;
        this.Send(pak);
    }

    [DebuggerHidden]
    private IEnumerable getNetWorkEventReceiverList()
    {
        return new <getNetWorkEventReceiverList>c__Iterator19 { $PC = -2 };
    }

    public void InitVersion(byte v4)
    {
        byte[] bytes = new byte[] { 1, 4, 7, v4 };
        this.version = TypeConvertUtil.bytesToint(bytes, 0, false);
    }

    private bool IsHasNeedLockUIMsg()
    {
        if ((this.isSending && (this.send_pak != null)) && this.IsNeedLockUIOP(this.send_pak.OpCode))
        {
            return true;
        }
        foreach (Packet packet in this._pendingSendQueue)
        {
            if (this.IsNeedLockUIOP(packet.OpCode))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsNeedLockUIOP(OpcodeType opCode)
    {
        return (((((opCode != OpcodeType.C2S_COMMITANTIDATA) && (opCode != OpcodeType.C2S_OBTAINANTIDATA)) && ((opCode != OpcodeType.C2S_QQFRIENDSINGAME) && (opCode != OpcodeType.C2S_GETBROADCASTLIST))) && ((opCode != OpcodeType.C2S_RECORDNEWBIE) && (opCode != OpcodeType.C2S_SYNCCHANGEDATA))) && (opCode != OpcodeType.C2S_GETNEWREWARDFLAG));
    }

    private bool IsSessionNoInit(Packet pak)
    {
        return (((pak.OpCode != OpcodeType.C2S_LOGIN) && (pak.OpCode != OpcodeType.C2S_REGISTER)) && !ActorData.getInstance().IsInited());
    }

    private bool LocalCommand(string cmd)
    {
        List<string> list = StrParser.ParseStringList(cmd, " ");
        if (list.Count == 1)
        {
            if (list[0].ToLower() == "kick")
            {
                GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
                GameDefine.getInstance().lastAccountName = string.Empty;
                return true;
            }
            if (list[0].ToLower() == "battlelog")
            {
                AiSkill.G_IsShowSkillLog = !AiSkill.G_IsShowSkillLog;
                return true;
            }
            if (list[0].ToLower() == "battlehpchange")
            {
                AiSkill.G_IsShowHpLog = !AiSkill.G_IsShowHpLog;
                return true;
            }
            if (list[0].ToLower() == "share")
            {
                SharePanel.mShareQQ = true;
                return true;
            }
            if (list[0].ToLower() == "pay")
            {
                Instance.SendQueryTxBalance();
                return true;
            }
            if (list[0].ToLower() == "push")
            {
                PlatformInterface.mInstance.PlatfromSwitchAccountNotify(string.Empty);
                return true;
            }
        }
        return false;
    }

    private void LockGUI()
    {
        if (this.IsHasNeedLockUIMsg())
        {
            this.isLockGUI = true;
            GUIMgr.Instance.Lock();
        }
    }

    public void LogOut()
    {
        GameStateMgr.Instance.ChangeState("RELOAD_EVENT");
        GameDefine.getInstance().lastAccountName = string.Empty;
    }

    public static void MakePacketHeader(ref byte[] protobuf, int version, byte option, int length, short opcode)
    {
        byte[] buffer = TypeConvertUtil.intToBytes(version, true);
        protobuf[0] = buffer[0];
        protobuf[1] = buffer[1];
        protobuf[2] = buffer[2];
        protobuf[3] = buffer[3];
        protobuf[4] = option;
        buffer = TypeConvertUtil.intToBytes(length, true);
        protobuf[5] = buffer[0];
        protobuf[6] = buffer[1];
        protobuf[7] = buffer[2];
        protobuf[8] = buffer[3];
        buffer = TypeConvertUtil.shortToBytes(opcode, true);
        protobuf[9] = buffer[0];
        protobuf[10] = buffer[1];
    }

    private void onConnectFailed()
    {
        this.DebugLog("onConnectFailed");
        NetConnectIsOk = false;
        this.ReSendLastPak();
        if (this.IsHasNeedLockUIMsg())
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x259));
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnEndConnect(IAsyncResult ar)
    {
        this.DebugLog("OnEndConnect");
        try
        {
            if (this._tcpSock != null)
            {
                this._tcpSock.EndConnect(ar);
            }
        }
        catch (Exception)
        {
        }
        this.isEndConnect = true;
    }

    private void OnPkFinish(GUIEntity ui)
    {
        if (PaokuInPanel._instance != null)
        {
            base.StartCoroutine(PaokuInPanel._instance.PlayAngryIcon());
        }
    }

    private void OnReceive(CircleBuffer segment)
    {
        this.CloseWaitGUI();
        byte[] dataBuffer = segment.DataBuffer;
        do
        {
            try
            {
                if (this._size == 0)
                {
                    if (segment.Length < HEADER_SIZE)
                    {
                        Debug.Log("segment.Length < HEADER_SIZE :");
                        this._hasRecvBuf = false;
                        break;
                    }
                    this._reserVer = dataBuffer[0];
                    this._minorVer = dataBuffer[1];
                    this._protoVer = dataBuffer[2];
                    this._majorVer = dataBuffer[3];
                    this._option = dataBuffer[4];
                    this._size = TypeConvertUtil.bytesToint(dataBuffer, 5, true);
                    this._opcode = (OpcodeType) TypeConvertUtil.bytesToShort(dataBuffer, 9, true);
                }
                if ((this._size > 0) && (this._size > segment.Length))
                {
                    Debug.Log("_size > segment.Length:" + this._size);
                    this._hasRecvBuf = false;
                    break;
                }
                bool flag = (this._option & PACKAGE_OPTION_MASK_COMPRESS) == PACKAGE_OPTION_MASK_COMPRESS;
                _totalBytesReceived += segment.Length;
                Stream stm = null;
                if (!flag)
                {
                    stm = new MemoryStream(dataBuffer, HEADER_SIZE, this._size - HEADER_SIZE);
                }
                else
                {
                    int length = (this._size - HEADER_SIZE) - 4;
                    byte[] destinationArray = new byte[length];
                    Array.Copy(dataBuffer, HEADER_SIZE + 4, destinationArray, 0, length);
                    byte[] dst = new byte[TypeConvertUtil.bytesToint(dataBuffer, HEADER_SIZE, true)];
                    MiniLZO.Decompress(destinationArray, dst);
                    stm = TypeConvertUtil.BytesToStream(dst);
                }
                Packet packet = PacketMgr.Instance.DeserializeGamePacket(this._opcode, stm);
                this.DebugLog("Rev Finish");
                if (!PacketMgr.Instance.HandleGamePacket(packet))
                {
                    this.ForwardNetWorkEvent(this._opcode, packet);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
            this._hasRecvBuf = true;
            segment.Shrink(this._size);
            this._size = 0;
        }
        while (segment.Length > 0);
    }

    [PacketHandler(OpcodeType.S2C_COMMITANTIDATA, typeof(S2C_CommitAntiData))]
    public void OnRecieveTssData2Server(Packet pak)
    {
        S2C_CommitAntiData packetObject = (S2C_CommitAntiData) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    private void onSessionInfoMissed()
    {
        NetConnectIsOk = false;
    }

    public void OnShareOK()
    {
        Packet pak = new Packet(OpcodeType.C2S_SHARE);
        C2S_Share share = new C2S_Share();
        pak.PacketObject = share;
        this.Send(pak);
    }

    private void ProcessSweepResult(S2C_OutlandSweepReq res)
    {
        <ProcessSweepResult>c__AnonStoreyFD yfd = new <ProcessSweepResult>c__AnonStoreyFD {
            res = res
        };
        DupLevInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DupLevInfoPanel>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.ResetPanel();
        }
        ActorData.getInstance().PhyForce = yfd.res.phyForce;
        ActorData.getInstance().UserInfo.exp = yfd.res.exp;
        ActorData.getInstance().OutlandCoin = yfd.res.outland_coin;
        ActorData.getInstance().Level = yfd.res.level;
        ActorData.getInstance().isInOutland = true;
        GUIMgr.Instance.DoModelGUI("RushResultPanel", new Action<GUIEntity>(yfd.<>m__8C), null);
        ActorData.getInstance().bOpenThirdPanel = false;
        Instance.RequestOutlandPageMapReq(ActorData.getInstance().outlandPageEntry);
        ActorData.getInstance().bOpenOutlandTitleInfo = false;
        Instance.RequestOutlandsData(null, null);
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERCOMBAT, typeof(S2C_ArenaLadderCombat))]
    public void ReceivArenaLadderCombat(Packet pak)
    {
        <ReceivArenaLadderCombat>c__AnonStorey10B storeyb = new <ReceivArenaLadderCombat>c__AnonStorey10B {
            <>f__this = this,
            res = (S2C_ArenaLadderCombat) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyb.res.result))
        {
            BattleState.GetInstance().DoNormalBattle(storeyb.res.combatData, null, BattleNormalGameType.ArenaLadder, false, "cj_zc01", 1, 3, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storeyb.<>m__9E));
        }
    }

    [PacketHandler(OpcodeType.S2C_GOBLINSHOPBUY, typeof(S2C_GoblinShopBuy))]
    public void Receive_S2C_GoblinShopBuy(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GoblinShopBuy packetObject = pak.PacketObject as S2C_GoblinShopBuy;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.buyResult.currency_info.stone;
            ActorData.getInstance().Gold = packetObject.buyResult.currency_info.gold;
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if ((null != activityGUIEntity) && (null != activityGUIEntity.ActivedGoblinShop))
            {
                activityGUIEntity.ActivedGoblinShop.UpdateBuyResult(packetObject.buyResult);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GOBLINSHOPFIX, typeof(S2C_GoblinShopFix))]
    public void Receive_S2C_GoblinShopFix(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GoblinShopFix packetObject = pak.PacketObject as S2C_GoblinShopFix;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().goblinFixed = true;
            ActorData.getInstance().Stone = packetObject.stone;
            ShopTabEntity gUIEntity = GUIMgr.Instance.GetGUIEntity<ShopTabEntity>();
            if (gUIEntity != null)
            {
                gUIEntity.gameObject.transform.FindChild("Goblin/Top/bt_fixed").gameObject.SetActive(false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GOBLINSHOPREFRESH, typeof(S2C_GoblinShopRefresh))]
    public void Receive_S2C_GoblinShopRefresh(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GoblinShopRefresh packetObject = pak.PacketObject as S2C_GoblinShopRefresh;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedGoblinShop = activityGUIEntity.ActivedGoblinShop;
                if (activedGoblinShop != null)
                {
                    List<ActivityShopPanel.ShopDataItem> items = new List<ActivityShopPanel.ShopDataItem>();
                    foreach (ShopItem item in packetObject.itemList)
                    {
                        goblin_shop_config _config = ConfigMgr.getInstance().getByEntry<goblin_shop_config>(item.entry);
                        if (_config != null)
                        {
                            ActivityShopPanel.ShopDataItem item2 = new ActivityShopPanel.ShopDataItem {
                                CostType = _config.cost_type,
                                Limit = _config.limit,
                                Item = item,
                                EntryId = _config.goods_entry
                            };
                            items.Add(item2);
                        }
                    }
                    ActorData.getInstance().UserInfo.refreshGoblinCount = packetObject.refreshCount;
                    ActorData.getInstance().Stone = packetObject.currencyInfo.stone;
                    activedGoblinShop.UpdateData(items);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_QQFRIENDSINGAME, typeof(S2C_QQFriendsInGame))]
    public void Receive_S2C_QQFRIENDSINGAME(Packet pak)
    {
        S2C_QQFriendsInGame packetObject = (S2C_QQFriendsInGame) pak.PacketObject;
        if (packetObject.result == OpResult.OpResult_QQFriendsInGame_Error)
        {
            XSingleton<SocialFriend>.Singleton.RequestServerError();
        }
        else if ((packetObject.result == OpResult.OpResult_QQFriendsInGame_Wait) && !XSingleton<SocialFriend>.Singleton.HadInited)
        {
            ScheduleMgr.Schedule(1f, () => this.RequestGameQQFriends());
        }
        else if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<SocialFriend>.Singleton.Update(packetObject.userList);
        }
    }

    [PacketHandler(OpcodeType.S2C_SECRETSHOPBUY, typeof(S2C_SecretShopBuy))]
    public void Receive_S2C_SecretShopBuy(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_SecretShopBuy packetObject = pak.PacketObject as S2C_SecretShopBuy;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.buyResult.currency_info.stone;
            ActorData.getInstance().Gold = packetObject.buyResult.currency_info.gold;
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedSecretShop = activityGUIEntity.ActivedSecretShop;
                if (activedSecretShop != null)
                {
                    activedSecretShop.UpdateBuyResult(packetObject.buyResult);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SECRETSHOPFIX, typeof(S2C_SecretShopFix))]
    public void Receive_S2C_SecretShopFix(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_SecretShopFix packetObject = pak.PacketObject as S2C_SecretShopFix;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().secretFixed = true;
            ActorData.getInstance().Stone = packetObject.stone;
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedSecretShop = activityGUIEntity.ActivedSecretShop;
                if (activedSecretShop != null)
                {
                    activedSecretShop.HideFixedBT = true;
                }
            }
            ShopTabEntity gUIEntity = GUIMgr.Instance.GetGUIEntity<ShopTabEntity>();
            if (gUIEntity != null)
            {
                gUIEntity.gameObject.transform.FindChild("Secret/Top/bt_fixed").gameObject.SetActive(false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SECRETSHOPREFRESH, typeof(S2C_SecretShopRefresh))]
    public void Receive_S2C_SecretShopRefresh(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_SecretShopRefresh packetObject = pak.PacketObject as S2C_SecretShopRefresh;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedSecretShop = activityGUIEntity.ActivedSecretShop;
                if (activedSecretShop != null)
                {
                    List<ActivityShopPanel.ShopDataItem> items = new List<ActivityShopPanel.ShopDataItem>();
                    foreach (ShopItem item in packetObject.itemList)
                    {
                        secret_shop_config _config = ConfigMgr.getInstance().getByEntry<secret_shop_config>(item.entry);
                        if (_config != null)
                        {
                            ActivityShopPanel.ShopDataItem item2 = new ActivityShopPanel.ShopDataItem {
                                CostType = _config.cost_type,
                                Limit = _config.limit,
                                Item = item,
                                EntryId = _config.goods_entry
                            };
                            items.Add(item2);
                        }
                    }
                    ActorData.getInstance().UserInfo.refreshSecretCount = packetObject.refreshCount;
                    ActorData.getInstance().Stone = packetObject.currencyInfo.stone;
                    activedSecretShop.UpdateData(items);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ACCEPTALLFRIENDPHYFORCE, typeof(S2C_AcceptAllFriendPhyForce))]
    public void ReceiveAcceptAllFriendPhyForce(Packet pak)
    {
        S2C_AcceptAllFriendPhyForce packetObject = (S2C_AcceptAllFriendPhyForce) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = packetObject.phyForce - ActorData.getInstance().PhyForce;
            if (num > 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9896b5), num));
            }
            ActorData.getInstance().PhyForce = packetObject.phyForce;
            ActorData.getInstance().UserInfo.remainPhyForceAccept = packetObject.remainPhyForceAccept;
            ActorData.getInstance().SetAllFriendTiLiState(packetObject.friendList);
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ACCEPTFRIENDPHYFORCE, typeof(S2C_AcceptFriendPhyForce))]
    public void ReceiveAcceptFriendPhyForce(Packet pak)
    {
        S2C_AcceptFriendPhyForce packetObject = (S2C_AcceptFriendPhyForce) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = packetObject.phyForce - ActorData.getInstance().PhyForce;
            if (num > 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9896b9), num, packetObject.remainPhyForceAccept));
            }
            ActorData.getInstance().PhyForce = packetObject.phyForce;
            ActorData.getInstance().UserInfo.remainPhyForceAccept = packetObject.remainPhyForceAccept;
            ActorData.getInstance().SetLingQuTiLiState(packetObject.targetId);
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateFriendByID(packetObject.targetId);
                activityGUIEntity.SetFriendTips();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ADDBROADCAST, typeof(S2C_AddBroadcast))]
    public void ReceiveAddBroadcast(Packet pak)
    {
        S2C_AddBroadcast packetObject = (S2C_AddBroadcast) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().AddBroadCast(packetObject.newBroadcast);
            ActorData.getInstance().Stone = packetObject.stone;
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2882));
            GUIMgr.Instance.ExitModelGUI("XiaoLaBaPanel");
        }
    }

    [PacketHandler(OpcodeType.S2C_ADDFRIEND, typeof(S2C_AddFriend))]
    public void ReceiveAddFriend(Packet pak)
    {
        S2C_AddFriend packetObject = (S2C_AddFriend) pak.PacketObject;
        GUIMgr.Instance.ExitModelGUI("FriendResultPanel");
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            AddFriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AddFriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReqAddFriendSucceed();
            }
            GUIMgr.Instance.ExitModelGUI("FriendInfoPanel");
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989693));
        }
    }

    [PacketHandler(OpcodeType.S2C_AGREEALLFRIEND, typeof(S2C_AgreeAllFriend))]
    public void ReceiveAgreeAllFriend(Packet pak)
    {
        S2C_AgreeAllFriend packetObject = (S2C_AgreeAllFriend) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().FriendList = packetObject.friendList;
            ActorData.getInstance().FriendReqList = packetObject.friendReqList;
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateList();
            }
            if (packetObject.friendReqList.Count > 0)
            {
                if (ActorData.getInstance().FriendList.Count >= ActorData.getInstance().MaxFriendCount)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b6));
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896b7));
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a7));
            }
            AddFriendPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<AddFriendPanel>();
            if (panel2 != null)
            {
                panel2.AgreeFriendSuccessd();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_AGREEFRIEND, typeof(S2C_AgreeFriend))]
    public void ReceiveAgreeFriend(Packet pak)
    {
        S2C_AgreeFriend packetObject = (S2C_AgreeFriend) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().DelItemFriendReqList(packetObject.targetId);
            ActorData.getInstance().FriendList = packetObject.friendList;
            if (GUIMgr.Instance.GetActivityGUIEntity<FriendInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("FriendInfoPanel");
            }
            AddFriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AddFriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.AgreeFriendSuccessd();
            }
            FriendPanel panel3 = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (panel3 != null)
            {
                panel3.UpdateList();
            }
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a7));
        }
    }

    [PacketHandler(OpcodeType.S2C_GETARENAALLRANK, typeof(S2C_GetArenaAllRank))]
    public void ReceiveAllArenaRank(Packet pak)
    {
        S2C_GetArenaAllRank packetObject = (S2C_GetArenaAllRank) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ArenaPortal activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaPortal>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.RefreshRankInfo(packetObject.arenaladderank, packetObject.lolarenarank);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_RANKLISTINFO, typeof(S2C_RankListInfo))]
    public void ReceiveAllRankInOneList(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_RankListInfo packetObject = (S2C_RankListInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            AllRankInOnePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<AllRankInOnePanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(packetObject.myinfo, packetObject.playerinfo);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERBUYATTACK, typeof(S2C_ArenaLadderBuyAttack))]
    public void ReceiveArenaLadderBuyAttack(Packet pak)
    {
        S2C_ArenaLadderBuyAttack packetObject = (S2C_ArenaLadderBuyAttack) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ArenaLadderPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaLadderPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ResetAttactCount(packetObject.buyAttackCount, packetObject.attackCount);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERCOMBATEND, typeof(S2C_ArenaLadderCombatEnd))]
    public void ReceiveArenaLadderCombatEnd(Packet pak)
    {
        <ReceiveArenaLadderCombatEnd>c__AnonStorey10C storeyc = new <ReceiveArenaLadderCombatEnd>c__AnonStorey10C {
            res = (S2C_ArenaLadderCombatEnd) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyc.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storeyc.<>m__9F), null);
            this.RequestGetQuestList();
            this.RequestGetTitleList();
            if (storeyc.res.is_new_mail)
            {
                this.RequestGetMailList();
            }
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERCOMBATHISTORY, typeof(S2C_ArenaLadderCombatHistory))]
    public void ReceiveArenaLadderCombatHistory(Packet pak)
    {
        S2C_ArenaLadderCombatHistory packetObject = (S2C_ArenaLadderCombatHistory) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LeagueHistoryPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LeagueHistoryPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateData(packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERINFO, typeof(S2C_ArenaLadderInfo))]
    public void ReceiveArenaLadderInfo(Packet pak)
    {
        S2C_ArenaLadderInfo packetObject = (S2C_ArenaLadderInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ArenaLadderPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaLadderPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateDate(packetObject);
            }
            if (packetObject.is_new_mail)
            {
                this.RequestGetMailList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERRANKLIST, typeof(S2C_ArenaLadderRankList))]
    public void ReceiveArenaLadderRankList(Packet pak)
    {
        S2C_ArenaLadderRankList packetObject = (S2C_ArenaLadderRankList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            RankingListPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<RankingListPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowArenaRanking(packetObject.enemys);
                activityGUIEntity.transform.FindChild("WaitPanel").gameObject.SetActive(false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERREFRESHATTACKTIME, typeof(S2C_ArenaLadderRefreshAttackTime))]
    public void ReceiveArenaLadderRefreshAttackTime(Packet pak)
    {
        S2C_ArenaLadderRefreshAttackTime packetObject = (S2C_ArenaLadderRefreshAttackTime) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ArenaLadderPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaLadderPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ResetAttactTime();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERSHOPBUY, typeof(S2C_ArenaLadderShopBuy))]
    public void ReceiveArenaLadderShopBuy(Packet pak)
    {
        S2C_ArenaLadderShopBuy packetObject = (S2C_ArenaLadderShopBuy) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.arena_ladder_score = packetObject.buyResult.currency_info.arena_ladder_score;
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateItemBySolt(packetObject.buyResult.slot);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETARENALADDERSHOPINFO, typeof(S2C_GetArenaLadderShopInfo))]
    public void ReceiveArenaLadderShopInfo(Packet pak)
    {
        S2C_GetArenaLadderShopInfo packetObject = (S2C_GetArenaLadderShopInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.ArenaLadderCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ARENALADDERSHOPREFRESH, typeof(S2C_ArenaLadderShopRefresh))]
    public void ReceiveArenaLadderShopRefresh(Packet pak)
    {
        S2C_ArenaLadderShopRefresh packetObject = (S2C_ArenaLadderShopRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.refreshArenaLadderShopCount = packetObject.refreshCount;
            ActorData.getInstance().UserInfo.arena_ladder_score = packetObject.currencyInfo.arena_ladder_score;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.ArenaLadderCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENADISABLECARD, typeof(S2C_LoLArenaDisableCard))]
    public void ReceiveBanArenaHero(Packet pak)
    {
        S2C_LoLArenaDisableCard packetObject = (S2C_LoLArenaDisableCard) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ArenaBanHero activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaBanHero>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.BanHero(packetObject.card_id, OpResult.OpResult_Ok == packetObject.result);
                activityGUIEntity.RefreshSelfPower((int) packetObject.self_team_power);
                activityGUIEntity.RefreshEnemyPower((int) packetObject.enemy_team_power);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_FLAMEBATTLEEND, typeof(S2C_FlameBattleEnd))]
    public void ReceiveBattleEnd(Packet pak)
    {
        <ReceiveBattleEnd>c__AnonStorey13E storeye = new <ReceiveBattleEnd>c__AnonStorey13E {
            res = (S2C_FlameBattleEnd) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeye.res.result))
        {
            if (ActorData.getInstance().mFlameBattleInfo != null)
            {
                ActorData.getInstance().mFlameBattleInfo.cur_node = storeye.res.cur_node;
            }
            ActorData.getInstance().UpdateBattleEndData();
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storeye.<>m__F0), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENAENTERCOMBAT, typeof(S2C_LoLArenaEnterCombat))]
    public void ReceiveBeginChallengeArenaCombat(Packet pak)
    {
        <ReceiveBeginChallengeArenaCombat>c__AnonStorey10E storeye = new <ReceiveBeginChallengeArenaCombat>c__AnonStorey10E {
            <>f__this = this,
            res = (S2C_LoLArenaEnterCombat) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeye.res.result))
        {
            BattleState.GetInstance().DoNormalBattle(storeye.res.combatData, null, BattleNormalGameType.ArenaLadder, false, "cj_zc01", 1, 3, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storeye.<>m__A2));
        }
    }

    [PacketHandler(OpcodeType.S2C_BINDACCOUNT, typeof(S2C_BindAccount))]
    public void ReceiveBindAccount(Packet pak)
    {
        S2C_BindAccount packetObject = (S2C_BindAccount) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    [PacketHandler(OpcodeType.S2C_GETBROADCASTLIST, typeof(S2C_GetBroadcastList))]
    public void ReceiveBroadcastList(Packet pak)
    {
        S2C_GetBroadcastList packetObject = (S2C_GetBroadcastList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.65f;
            ActorData.getInstance().AddBroadCastList(packetObject.broadcastList);
        }
    }

    [PacketHandler(OpcodeType.S2C_BUYSKILLPOINT, typeof(S2C_BuySkillPoint))]
    public void ReceiveBuySkillPoint(Packet pak)
    {
        S2C_BuySkillPoint packetObject = (S2C_BuySkillPoint) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().CurSkillPoint = packetObject.skillPoint;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("HeroInfoPanel");
            if (gUIEntity != null)
            {
                HeroInfoPanel panel = gUIEntity.Achieve<HeroInfoPanel>();
                if (panel != null)
                {
                    panel.SkillPoint = packetObject.skillPoint;
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHBUY, typeof(S2C_WarmmatchBuy))]
    public void ReceiveBuyWarmmatchCount(Packet pak)
    {
        S2C_WarmmatchBuy packetObject = (S2C_WarmmatchBuy) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ArenaPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdatePkCount(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPCOMBATBEGIN, typeof(S2C_GuildDupCombatBegin))]
    public void ReceiveC2S_GuildDupCombatBegin(Packet pak)
    {
        <ReceiveC2S_GuildDupCombatBegin>c__AnonStorey128 storey = new <ReceiveC2S_GuildDupCombatBegin>c__AnonStorey128 {
            <>f__this = this
        };
        GUIMgr.Instance.UnLock();
        storey.res = pak.PacketObject as S2C_GuildDupCombatBegin;
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            guilddup_trench_config _config = ConfigMgr.getInstance().getByEntry<guilddup_trench_config>(storey.res.guildDupTrenchId);
            if (_config == null)
            {
                TipsDiag.SetText("GuildDupTrenchID:" + storey.res.guildDupTrenchId);
            }
            else
            {
                BattleState.GetInstance().DoNormalBattle(storey.res.combat_data, null, BattleNormalGameType.GuildDup, true, _config.scene, _config.start_pos, _config.visual_type, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storey.<>m__C7));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_CALPOWER, typeof(S2C_CalPower))]
    public void ReceiveCalPower(Packet pak)
    {
        S2C_CalPower packetObject = (S2C_CalPower) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result) && (packetObject.cardPowerList.Count > 0))
        {
            ActorData.getInstance().UpateCardFightPower(packetObject.cardIdList, packetObject.cardPowerList);
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetCardPower();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REQCARDBREAK, typeof(S2C_ReqCardBreak))]
    public void ReceiveCardBreak(Packet pak)
    {
        S2C_ReqCardBreak packetObject = (S2C_ReqCardBreak) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            List<long> cardIdList = new List<long> {
                packetObject.cardInfo.card_id
            };
            SoundManager.mInstance.PlaySFX("sound_tupo");
            this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.CardBreakSuccess(packetObject.cardInfo);
                ActorData.getInstance().UpdateCard(packetObject.cardInfo);
                if (ActorData.getInstance().UserInfo.headEntry == packetObject.cardInfo.cardInfo.entry)
                {
                    MainUI gUIEntity = GUIMgr.Instance.GetGUIEntity<MainUI>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.Create3DRole(ActorData.getInstance().UserInfo.headEntry);
                    }
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REQCARDEVOLVE, typeof(S2C_ReqCardEvolve))]
    public void ReceiveCardEvolve(Packet pak)
    {
        S2C_ReqCardEvolve packetObject = (S2C_ReqCardEvolve) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().UpdateCard(packetObject.cardInfo);
            ActorData.getInstance().UpdateItemList(packetObject.changeItems);
            HeroInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
            if ((gUIEntity != null) && !gUIEntity.Hidden)
            {
                gUIEntity.UpStarSucess(packetObject.cardInfo);
                List<long> cardIdList = new List<long> {
                    packetObject.cardInfo.card_id
                };
                this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            }
            SoundManager.mInstance.PlaySFX("sound_shengxing");
        }
        else
        {
            HeroInfoPanel panel2 = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
            if (null != panel2)
            {
                panel2.mUpStarOk = true;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENARANKLIST, typeof(S2C_LoLArenaRankList))]
    public void ReceiveChallengeArena(Packet pak)
    {
        S2C_LoLArenaRankList packetObject = (S2C_LoLArenaRankList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENABUYATTACK, typeof(S2C_LoLArenaBuyAttack))]
    public void ReceiveChallengeArenaBuyAttack(Packet pak)
    {
        S2C_LoLArenaBuyAttack packetObject = (S2C_LoLArenaBuyAttack) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
            challengeData.attack_count = packetObject.attackCount;
            challengeData.attack_buy_count = packetObject.buyAttackCount;
            challengeData.remain_attack_time = 0;
            challengeData.remain_attack_count = Mathf.Max(0, 5 - packetObject.attackCount);
            ChallengeArenaPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChallengeArenaPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.RefreshAttackCountInfo();
                activityGUIEntity.RefreshAttackCooldown();
                activityGUIEntity.UpdateCostStatus();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENAINFO, typeof(S2C_LoLArenaInfo))]
    public void ReceiveChallengeArenaInfo(Packet pak)
    {
        S2C_LoLArenaInfo packetObject = (S2C_LoLArenaInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ChallengeArenaPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChallengeArenaPanel>();
            if (null != activityGUIEntity)
            {
                ChallengeArenaData challengeData = ActorData.getInstance().ChallengeData;
                challengeData.rank = packetObject.selfOrder;
                challengeData.top_rank = packetObject.bestOrder;
                challengeData.attack_count = packetObject.attackCount;
                challengeData.attack_buy_count = packetObject.buyAttackCount;
                challengeData.remain_attack_count = Mathf.Max(0, 5 - packetObject.attackCount);
                challengeData.remain_attack_time = packetObject.remainAttackTime;
                challengeData.modify_time = packetObject.lastSetFormationTime;
                challengeData.enemies.Clear();
                foreach (LoLArenaEnemy enemy in packetObject.enemys)
                {
                    <ReceiveChallengeArenaInfo>c__AnonStorey10D storeyd = new <ReceiveChallengeArenaInfo>c__AnonStorey10D();
                    ArenaLadderEnemy item = new ArenaLadderEnemy {
                        order = enemy.order,
                        targetId = enemy.targetId,
                        target_type = enemy.target_type,
                        team_power = enemy.team_power,
                        level = enemy.level,
                        head_entry = enemy.head_entry,
                        head_frame_entry = enemy.head_frame_entry,
                        win_count = enemy.win_count,
                        name = enemy.name,
                        guild_name = enemy.guild_name
                    };
                    int count = enemy.cardIdList.Count;
                    int num2 = enemy.cardList.Count;
                    storeyd.card_dic = new Dictionary<long, CardInfo>();
                    for (int i = 0; (i != count) && (i != num2); i++)
                    {
                        storeyd.card_dic.Add(enemy.cardIdList[i], enemy.cardList[i]);
                    }
                    item.cardIdList = enemy.cardIdList;
                    item.cardIdList.Sort(new Comparison<long>(storeyd.<>m__A0));
                    item.cardList = enemy.cardList;
                    if (<>f__am$cache29 == null)
                    {
                        <>f__am$cache29 = delegate (CardInfo info_l, CardInfo info_r) {
                            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) info_l.entry);
                            card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) info_r.entry);
                            int num = (_config != null) ? _config.card_position : -1;
                            int num2 = (_config2 != null) ? _config2.card_position : -1;
                            return num - num2;
                        };
                    }
                    item.cardList.Sort(<>f__am$cache29);
                    challengeData.enemies.Add(item);
                }
                activityGUIEntity.Invalidate();
            }
            if (packetObject.is_new_mail)
            {
                this.RequestGetMailList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENAREFRESHATTACKTIME, typeof(S2C_LoLArenaRefreshAttackTime))]
    public void ReceiveChallengeArenaRefreshAttackCooldown(Packet pak)
    {
        S2C_LoLArenaRefreshAttackTime packetObject = (S2C_LoLArenaRefreshAttackTime) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().ChallengeData.remain_attack_time = packetObject.remainAttackTime;
            ChallengeArenaPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChallengeArenaPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.RefreshAttackCountInfo();
                activityGUIEntity.RefreshAttackCooldown();
                activityGUIEntity.UpdateCostStatus();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLOLARENAFORMATION, typeof(S2C_GetLoLArenaFormation))]
    public void ReceiveChallengeFormation(Packet pak)
    {
        S2C_GetLoLArenaFormation packetObject = (S2C_GetLoLArenaFormation) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleFormation challengeArenaFormation = ActorData.getInstance().ChallengeArenaFormation;
            challengeArenaFormation.captain_id = packetObject.formation.captain_id;
            challengeArenaFormation.card_id = packetObject.formation.card_id;
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENACOMBATHISTORY, typeof(S2C_LoLArenaCombatHistory))]
    public void ReceiveChallengeHistory(Packet pak)
    {
        S2C_LoLArenaCombatHistory packetObject = (S2C_LoLArenaCombatHistory) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LeagueHistoryPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LeagueHistoryPanel>();
            if (null != activityGUIEntity)
            {
                List<ArenaLadderCombatRecord> historyList = new List<ArenaLadderCombatRecord>();
                int count = packetObject.itemList.Count;
                for (int i = 0; i != count; i++)
                {
                    LoLArenaCombatRecord record = packetObject.itemList[i];
                    ArenaLadderCombatRecord item = new ArenaLadderCombatRecord {
                        combat_time = record.combat_time,
                        head_entry = record.head_entry,
                        head_frame_entry = record.head_frame_entry,
                        is_attacker = record.is_attacker,
                        level = record.level,
                        order_change = record.order_change,
                        target_name = record.target_name
                    };
                    historyList.Add(item);
                }
                activityGUIEntity.UpdateData(historyList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENABEFORECOMBAT, typeof(S2C_LoLArenaBeforeCombat))]
    public void ReceiveChallengeSelfBanInfo(Packet pak)
    {
        S2C_LoLArenaBeforeCombat packetObject = (S2C_LoLArenaBeforeCombat) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            <ReceiveChallengeSelfBanInfo>c__AnonStorey112 storey = new <ReceiveChallengeSelfBanInfo>c__AnonStorey112();
            BattleFormation challengeArenaFormation = ActorData.getInstance().ChallengeArenaFormation;
            storey.data = ActorData.getInstance().ChallengeData;
            storey.data.ResetActivityFormation();
            if ((challengeArenaFormation != null) && (storey.data.activity_enemy != null))
            {
                <ReceiveChallengeSelfBanInfo>c__AnonStorey110 storey2 = new <ReceiveChallengeSelfBanInfo>c__AnonStorey110 {
                    <>f__ref$274 = storey,
                    formation_card_list = ArenaFormationUtility.Identities2CardList(challengeArenaFormation.card_id, true)
                };
                packetObject.card_info.ForEach(new Action<LoLArenaCardData>(storey2.<>m__A4));
                ArenaBanHero activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaBanHero>();
                if (null != activityGUIEntity)
                {
                    activityGUIEntity.Invalidate();
                    activityGUIEntity.RefreshSelfPower((int) packetObject.self_team_power);
                    activityGUIEntity.RefreshEnemyPower((int) packetObject.enemy_team_power);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENASHOPBUY, typeof(S2C_LoLArenaShopBuy))]
    public void ReceiveChallengeShopBuy(Packet pak)
    {
        S2C_LoLArenaShopBuy packetObject = (S2C_LoLArenaShopBuy) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.lol_arena_score = packetObject.buyResult.currency_info.lolarena_score;
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.UpdateItemBySolt(packetObject.buyResult.slot);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLOLARENASHOPINFO, typeof(S2C_GetLoLArenaShopInfo))]
    public void ReceiveChallengeShopInfo(Packet pak)
    {
        S2C_GetLoLArenaShopInfo packetObject = (S2C_GetLoLArenaShopInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.ArenaChallengeCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENASHOPREFRESH, typeof(S2C_LoLArenaShopRefresh))]
    public void ReceiveChallengeShopRefresh(Packet pak)
    {
        S2C_LoLArenaShopRefresh packetObject = (S2C_LoLArenaShopRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.refreshLoLArenaShopCount = packetObject.refreshCount;
            ActorData.getInstance().UserInfo.lol_arena_score = packetObject.currencyInfo.lolarena_score;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.ArenaChallengeCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_CHANGEHEADFRAME, typeof(S2C_ChangeHeadFrame))]
    public void ReceiveChangeHeadFrame(Packet pak)
    {
        S2C_ChangeHeadFrame packetObject = (S2C_ChangeHeadFrame) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GUIMgr.Instance.ExitModelGUI("PlayerIconFramePanel");
            ActorData.getInstance().HeadFrameEntry = packetObject.headFrameEntry;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("PlayerInfoPanel");
            if (gUIEntity != null)
            {
                PlayerInfoPanel panel = (PlayerInfoPanel) gUIEntity;
                if (panel != null)
                {
                    panel.UpdateHeadFrame();
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_COMMITDAILYQUEST, typeof(S2C_CommitDailyQuest))]
    public void ReceiveCommitDailyQuest(Packet pak)
    {
        S2C_CommitDailyQuest packetObject = (S2C_CommitDailyQuest) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (<>f__am$cache36 == null)
            {
                <>f__am$cache36 = delegate (GUIEntity entity) {
                    Instance.RequestDailyQuest();
                    Daily activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<Daily>();
                    if (null != activityGUIEntity)
                    {
                        activityGUIEntity.commit_lock = false;
                    }
                };
            }
            RewardPanel.ShowDailyReward(packetObject.rewards, packetObject.level, packetObject.exp, packetObject.phyForce, <>f__am$cache36);
            Instance.RequestRewardFlag();
        }
        else
        {
            Daily daily = GUIMgr.Instance.GetActivityGUIEntity<Daily>();
            if (null != daily)
            {
                daily.commit_lock = false;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_COMMITQUEST, typeof(S2C_CommitQuest))]
    public void ReceiveCommitQuestResult(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_CommitQuest packetObject = (S2C_CommitQuest) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            <ReceiveCommitQuestResult>c__AnonStorey100 storey = new <ReceiveCommitQuestResult>c__AnonStorey100 {
                panel = GUIMgr.Instance.GetActivityGUIEntity<AchievementPanel>()
            };
            ActorData.getInstance().QuestList = packetObject.questList;
            RewardPanel.ShowAchievementReward(packetObject, new Action<GUIEntity>(storey.<>m__8F));
            if (null != storey.panel)
            {
                storey.panel.Refresh();
            }
            Instance.RequestRewardFlag();
        }
        else
        {
            AchievementPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AchievementPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.commit_lock = false;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_COURAGESHOPBUY, typeof(S2C_CourageShopBuy))]
    public void ReceiveCourageShopBuyResult(Packet pak)
    {
        <ReceiveCourageShopBuyResult>c__AnonStorey134 storey = new <ReceiveCourageShopBuyResult>c__AnonStorey134 {
            res = (S2C_CourageShopBuy) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().Stone = storey.res.buyResult.currency_info.stone;
            ActorData.getInstance().Gold = storey.res.buyResult.currency_info.gold;
            ActorData.getInstance().UpdateNewCard(storey.res.buyResult.cardList);
            ActorData.getInstance().UpdateItem(storey.res.buyResult.item);
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if ((null != activityGUIEntity) && (null != activityGUIEntity.ActivedCourageShop))
            {
                activityGUIEntity.ActivedCourageShop.RefreshCurrency();
                ShopItem item = ActorData.getInstance().courageShopItemList.Find(new Predicate<ShopItem>(storey.<>m__E0));
                if (item != null)
                {
                    item.buyCount = Mathf.Max(0, storey.res.buyResult.buyCount);
                }
            }
            if ((null != activityGUIEntity) && (null != activityGUIEntity.ActivedCourageShop))
            {
                activityGUIEntity.ActivedCourageShop.Refresh();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_COURAGESHOPREFRESH, typeof(S2C_CourageShopRefresh))]
    public void ReceiveCourageShopForceRefresh(Packet pak)
    {
        S2C_CourageShopRefresh packetObject = (S2C_CourageShopRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.currencyInfo.stone;
            ActorData.getInstance().UserInfo.refreshCourageShopCount = packetObject.refreshCount;
            ActorData.getInstance().courageShopItemList = packetObject.itemList;
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if ((null != activityGUIEntity) && (null != activityGUIEntity.ActivedCourageShop))
            {
                activityGUIEntity.ActivedCourageShop.Refresh();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETCOURAGESHOPINFO, typeof(S2C_GetCourageShopInfo))]
    public void ReceiveCourageShopItemList(Packet pak)
    {
        S2C_GetCourageShopInfo packetObject = (S2C_GetCourageShopInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().courageShopItemList = packetObject.itemList;
        }
        ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
        if ((null != activityGUIEntity) && (null != activityGUIEntity.ActivedCourageShop))
        {
            activityGUIEntity.ActivedCourageShop.Refresh();
        }
    }

    [PacketHandler(OpcodeType.S2C_GETDAILYQUEST, typeof(S2C_GetDailyQuest))]
    public void ReceiveDailyQuest(Packet pak)
    {
        S2C_GetDailyQuest packetObject = (S2C_GetDailyQuest) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            Daily activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<Daily>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.Refresh(packetObject.rewardConfigs);
            }
            ActorData.getInstance().haveReceiveDailyReward = packetObject.haveReceiveReward;
        }
    }

    [PacketHandler(OpcodeType.S2C_DELETEFRIEND, typeof(S2C_DeleteFriend))]
    public void ReceiveDeleteFriend(Packet pak)
    {
        S2C_DeleteFriend packetObject = (S2C_DeleteFriend) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            Debug.Log("S2C_DeleteFriend Succeed");
            ActorData.getInstance().DeleteFriend(packetObject.targetId);
            GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
            GUIMgr.Instance.ExitModelGUI("FriendInfoPanel");
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DELETEMAIL, typeof(S2C_DeleteMail))]
    public void ReceiveDelMail(Packet pak)
    {
        S2C_DeleteMail packetObject = (S2C_DeleteMail) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.mailIdList.Count > 0)
            {
                ActorData.getInstance().DelMailList(packetObject.mailIdList);
            }
            if (packetObject.sysMailIdList.Count > 0)
            {
                ActorData.getInstance().DelMailList(packetObject.sysMailIdList);
            }
            MailListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<MailListPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateMailList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DRAWACTIVITYPRIZE, typeof(S2C_DrawActivityPrize))]
    public void ReceiveDRAWACTIVITYPRIZE(Packet pak)
    {
        <ReceiveDRAWACTIVITYPRIZE>c__AnonStorey101 storey = new <ReceiveDRAWACTIVITYPRIZE>c__AnonStorey101 {
            res = (S2C_DrawActivityPrize) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__90), null);
            if (ActivePanel.inst != null)
            {
                ActivePanel.inst.ResetState(storey.res.activity_type, storey.res.activity_entry, storey.res.subEntry, storey.res.flag);
            }
        }
        else
        {
            Debug.Log("收到领取失败---------------");
        }
    }

    [PacketHandler(OpcodeType.S2C_DRAWLOTTERYCARD, typeof(S2C_DrawLotteryCard))]
    public void ReceiveDrawLotteryCard(Packet pak)
    {
        <ReceiveDrawLotteryCard>c__AnonStorey136 storey = new <ReceiveDrawLotteryCard>c__AnonStorey136 {
            res = (S2C_DrawLotteryCard) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GameDataMgr.Instance.boostRecruit.FreeTime(storey.res.optionEntry, storey.res.freeCd, storey.res.free_count);
            GameDataMgr.Instance.DirtyActorStage = true;
            if (storey.res.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(storey.res.ticket);
            }
            foreach (LotteryReward reward in storey.res.rewardList)
            {
                ActorData.getInstance().UpdateNewCard(reward.card);
                ActorData.getInstance().UpdateItem(reward.item);
                if (reward.stone != -1)
                {
                    ActorData data1 = ActorData.getInstance();
                    data1.Stone += reward.stone;
                }
            }
            ActorData.getInstance().UpdateItemList(storey.res.itemList);
            if (null != GUIMgr.Instance.GetGUIEntity<RecruitPanel>())
            {
                string str = string.Empty;
                string str2 = string.Empty;
                string str3 = string.Empty;
                string str4 = string.Empty;
                string str5 = string.Empty;
                foreach (LotteryReward reward2 in storey.res.rewardList)
                {
                    if ((reward2.item != null) && (reward2.item.entry != -1))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            str3 = str3 + "|";
                        }
                        if (!string.IsNullOrEmpty(str2))
                        {
                            str2 = str2 + "|";
                        }
                        str2 = str2 + reward2.item.entry.ToString();
                        str3 = str3 + reward2.item.diff.ToString();
                    }
                    foreach (Card card in reward2.card.newCard)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + "|";
                        }
                        str = str + card.cardInfo.entry.ToString();
                    }
                    foreach (Item item in reward2.card.newItem)
                    {
                        int cardExCfgByItemPart = CommonFunc.GetCardExCfgByItemPart(item.entry);
                        if (cardExCfgByItemPart == -1)
                        {
                            if (!string.IsNullOrEmpty(str2))
                            {
                                str2 = str2 + "|";
                            }
                            if (!string.IsNullOrEmpty(str3))
                            {
                                str3 = str3 + "|";
                            }
                            str2 = str2 + item.entry.ToString();
                            str3 = str3 + item.diff.ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + "|";
                            }
                            str = str + cardExCfgByItemPart.ToString();
                            if (!string.IsNullOrEmpty(str4))
                            {
                                str4 = str4 + "|";
                            }
                            if (!string.IsNullOrEmpty(str5))
                            {
                                str5 = str5 + "|";
                            }
                            str4 = str4 + cardExCfgByItemPart.ToString();
                            str5 = str5 + item.diff.ToString();
                        }
                    }
                }
                Debug.Log("card_para : " + str + "   item_para : " + str2 + "   item_num : " + str3);
                string[] textArray2 = new string[] { str, ",", str2, ",", str3, ",", str4, ",", str5 };
                GameStateMgr.Instance.ChangeStateWithParameter("COMMUNITY_RECRUIT_EVENT", string.Concat(textArray2));
            }
            else
            {
                <ReceiveDrawLotteryCard>c__AnonStorey135 storey2 = new <ReceiveDrawLotteryCard>c__AnonStorey135 {
                    <>f__ref$310 = storey
                };
                GUIMgr.Instance.ExitModelGUI("RecruitResultPanel");
                GameDataMgr.Instance.boostRecruit.HiddenLastResult();
                storey2.card_list = new List<int>();
                storey2.item_list = new List<int>();
                storey2.item_num_list = new List<int>();
                storey2.morph_list = new List<int>();
                storey2.morph_num_list = new List<int>();
                ScheduleMgr.Schedule(0.5f, new System.Action(storey2.<>m__E1));
            }
            ActorData.getInstance().Stone = storey.res.stone;
            ActorData.getInstance().Gold = storey.res.gold;
        }
        else if (storey.res.result != OpResult.OpResult_Pay_Stone_S)
        {
            RecruitResultPanel.EnableButton(true);
            GameDataMgr.Instance.boostRecruit.valid = true;
        }
    }

    [PacketHandler(OpcodeType.S2C_DRAWREDPACKAGE, typeof(S2C_DrawRedPackage))]
    public void ReceiveDrawRedPackage(Packet pak)
    {
        S2C_DrawRedPackage packetObject = (S2C_DrawRedPackage) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.currStone;
            ActorData.getInstance().UpdateFriendHongBaoInfo(packetObject.friendid, packetObject.currStone, true, packetObject.friend_redpackage_num);
            FriendInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<FriendInfoPanel>();
            if (null != gUIEntity)
            {
                gUIEntity.UpdateHongBaoFriendInfo(packetObject.friendid, true, packetObject.friend_redpackage_num);
            }
            FriendPanel panel2 = GUIMgr.Instance.GetGUIEntity<FriendPanel>();
            if (null != panel2)
            {
                panel2.UpdateFriendByID(packetObject.friendid);
                panel2.SetFriendTips();
            }
            ActorData.getInstance().UpdateGuildMemberInfo(packetObject.friendid, packetObject.currStone, true, packetObject.friend_redpackage_num);
            GuildPanel panel3 = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (null != panel3)
            {
                panel3.UpdateMemberInfo(packetObject.friendid);
            }
            GuildMemberCtrlDlag dlag = GUIMgr.Instance.GetGUIEntity<GuildMemberCtrlDlag>();
            if (null != dlag)
            {
                dlag.UpdateMemberInfo(packetObject.friendid);
            }
            GoldTreePanel panel4 = GUIMgr.Instance.GetGUIEntity<GoldTreePanel>();
            if (null != panel4)
            {
                panel4.UpdateData(packetObject);
            }
            Debug.Log("ReceiveDrawRedPackage success--");
        }
        else if (packetObject.result == OpResult.OpResult_RedPackage_NoRedPackage)
        {
            ActorData.getInstance().UpdateFriendHongBaoInfo(packetObject.friendid, packetObject.currStone, false, 0);
            FriendInfoPanel panel5 = GUIMgr.Instance.GetGUIEntity<FriendInfoPanel>();
            if (null != panel5)
            {
                panel5.UpdateHongBaoFriendInfo(packetObject.friendid, false, 0);
            }
            FriendPanel panel6 = GUIMgr.Instance.GetGUIEntity<FriendPanel>();
            if (null != panel6)
            {
                panel6.UpdateFriendByID(packetObject.friendid);
                panel6.SetFriendTips();
            }
            ActorData.getInstance().UpdateGuildMemberInfo(packetObject.friendid, packetObject.currStone, false, 0);
            GuildPanel panel7 = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (null != panel7)
            {
                panel7.UpdateMemberInfo(packetObject.friendid);
            }
            GuildMemberCtrlDlag dlag2 = GUIMgr.Instance.GetGUIEntity<GuildMemberCtrlDlag>();
            if (null != dlag2)
            {
                dlag2.UpdateMemberInfo(packetObject.friendid);
            }
            GoldTreePanel panel8 = GUIMgr.Instance.GetGUIEntity<GoldTreePanel>();
            if (null != panel8)
            {
                panel8.PickOver(packetObject.friendid, packetObject.friendType);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DUNGEONSDATAREQ, typeof(S2C_DungeonsDataReq))]
    public void ReceiveDungeonsData(Packet pak)
    {
        S2C_DungeonsDataReq packetObject = (S2C_DungeonsDataReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.dungeons_1_times = packetObject.dungeons_1_times;
            ActorData.getInstance().UserInfo.dungeons_2_times = packetObject.dungeons_2_times;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("DungeonsPanel");
            if (gUIEntity != null)
            {
                DungeonsPanel panel = gUIEntity.Achieve<DungeonsPanel>();
                if (panel != null)
                {
                    panel.UpdateData(packetObject);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DUPLICATEBUYSMASHTIMESREQ, typeof(S2C_DuplicateBuySmashTimesReq))]
    public void ReceiveDuplicateBuySmashTimes(Packet pak)
    {
        S2C_DuplicateBuySmashTimesReq packetObject = (S2C_DuplicateBuySmashTimesReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().UpdateItem(packetObject.item);
            DupLevInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DupLevInfoPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateRushRemain();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DUPLICATEBUYTIMESREQ, typeof(S2C_DuplicateBuyTimesReq))]
    public void ReceiveDuplicateBuyTimes(Packet pak)
    {
        S2C_DuplicateBuyTimesReq packetObject = (S2C_DuplicateBuyTimesReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            List<FastBuf.TrenchData> list = new List<FastBuf.TrenchData>();
            ActorData.getInstance().TrenchEliteDataDict.TryGetValue(packetObject.dupData.dupEntry, out list);
            int count = 0;
            foreach (FastBuf.TrenchData data in list)
            {
                if (data.entry == packetObject.dupData.trenchEntry)
                {
                    data.remain = packetObject.times;
                    count = packetObject.times;
                    data.buy_times++;
                }
            }
            DupLevInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DupLevInfoPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateCount(count);
            }
            DupMap map = GUIMgr.Instance.GetGUIEntity<DupMap>();
            if (map != null)
            {
                map.UpdateLevelCountLabel(false);
            }
            ActorData.getInstance().Stone = packetObject.stone;
        }
    }

    [PacketHandler(OpcodeType.S2C_DUPLICATESMASH, typeof(S2C_DuplicateSmash))]
    public void ReceiveDuplicateSmash(Packet pak)
    {
        S2C_DuplicateSmash packetObject = (S2C_DuplicateSmash) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            User userInfo = ActorData.getInstance().UserInfo;
            userInfo.dup_smash_times -= packetObject.times;
            if (ActorData.getInstance().UserInfo.dup_smash_times < 0)
            {
                ActorData.getInstance().UserInfo.dup_smash_times = 0;
            }
            ActorData.getInstance().UpdateItem(packetObject.item);
            ActorData.getInstance().mItemDataDirty = true;
            RushResultPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<RushResultPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateData(packetObject);
                HeroInfoPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null != panel2)
                {
                    panel2.ResetEquipInfo();
                }
                DupLevInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DupLevInfoPanel>();
                if (gUIEntity != null)
                {
                    gUIEntity.UpdateBattleComplete();
                }
                DupMap map = GUIMgr.Instance.GetGUIEntity<DupMap>();
                if (map != null)
                {
                    map.UpdateLevelCountLabel(false);
                }
            }
            this.CheckLivenessReward();
        }
    }

    [PacketHandler(OpcodeType.S2C_GETDUPLICATEREWARDINFO, typeof(S2C_GetDuplicateRewardInfo))]
    public void ReceiveDupRewardInfo(Packet pak)
    {
        S2C_GetDuplicateRewardInfo packetObject = (S2C_GetDuplicateRewardInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.85f;
            ActorData.getInstance().DupRewardInfo = packetObject.rewardInfoList;
        }
    }

    [PacketHandler(OpcodeType.S2C_ENCHASECARDGEM, typeof(S2C_EnchaseCardGem))]
    public void ReceiveEnchaseCardGem(Packet pak)
    {
        S2C_EnchaseCardGem packetObject = (S2C_EnchaseCardGem) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GemWearPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GemWearPanel>();
            if (gUIEntity != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(packetObject.changeitem.entry);
                if (_config != null)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e56) + _config.name + "\n[00ff00]" + gUIEntity.GetAddDeltaByID(packetObject.changeitem.entry, 1));
                    List<long> cardIdList = new List<long> {
                        packetObject.CardInfo.card_id
                    };
                    this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
                    ActorData.getInstance().UpdateCard(packetObject.CardInfo);
                    gUIEntity.InitCardInfo(packetObject.CardInfo);
                    List<Item> list2 = new List<Item> {
                        packetObject.changeitem
                    };
                    ActorData.getInstance().UpdateItemList(list2);
                    gUIEntity.UpdateData(list2);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LOLARENACOMBATEND, typeof(S2C_LoLArenaCombatEnd))]
    public void ReceiveEndChallengeArenaCombat(Packet pak)
    {
        <ReceiveEndChallengeArenaCombat>c__AnonStorey10F storeyf = new <ReceiveEndChallengeArenaCombat>c__AnonStorey10F {
            res = (S2C_LoLArenaCombatEnd) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyf.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI<ResultPanel>(new Action<GUIEntity>(storeyf.<>m__A3), null);
            this.RequestGetQuestList();
            this.RequestGetTitleList();
            if (storeyf.res.is_new_mail)
            {
                this.RequestGetMailList();
            }
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_DUPLICATECOMBAT, typeof(S2C_DuplicateCombat))]
    public void ReceiveEnterDup(Packet pak)
    {
        S2C_DuplicateCombat packetObject = (S2C_DuplicateCombat) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        Debug.Log("=================================================dian????????????=res.result====" + packetObject.result);
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().PhyForce = packetObject.phyForce;
            BattleStaticEntry.DoDupBattle(packetObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_EQUIPBREAK, typeof(S2C_EquipBreak))]
    public void ReceiveEquipBreak(Packet pak)
    {
        S2C_EquipBreak packetObject = (S2C_EquipBreak) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().UpdateItemList(packetObject.items);
            BreakEquipPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BreakEquipPanel>();
            if (activityGUIEntity != null)
            {
                ActorData.getInstance().mCardChangedType = CardChangedType.EquipBreak;
                activityGUIEntity.LevelUpEquipSucess(packetObject.CardInfo);
                HeroInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
                if (gUIEntity != null)
                {
                    gUIEntity.UpdateEquipBreakData(packetObject.CardInfo);
                }
            }
            else
            {
                HeroInfoPanel panel3 = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (panel3 != null)
                {
                    panel3.ResetHeroInfoAndPopChange(packetObject.CardInfo);
                }
            }
            List<long> cardIdList = new List<long> {
                packetObject.CardInfo.card_id
            };
            Instance.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            TipsDiag.SetText("装备进化成功!");
            ActorData.getInstance().UpdateCard(packetObject.CardInfo);
        }
    }

    [PacketHandler(OpcodeType.S2C_EQUIPBREAKQUICK, typeof(S2C_EquipBreakQuick))]
    public void ReceiveEquipBreakQuick(Packet pak)
    {
        S2C_EquipBreakQuick packetObject = (S2C_EquipBreakQuick) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().UpdateItemList(packetObject.items);
            BreakEquipPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BreakEquipPanel>();
            if (activityGUIEntity != null)
            {
                ActorData.getInstance().mCardChangedType = CardChangedType.EquipBreak;
                activityGUIEntity.LevelUpEquipSucess(packetObject.CardInfo);
                HeroInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
                if (gUIEntity != null)
                {
                    gUIEntity.UpdateEquipBreakData(packetObject.CardInfo);
                }
            }
            else
            {
                HeroInfoPanel panel3 = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (panel3 != null)
                {
                    panel3.ResetHeroInfoAndPopChange(packetObject.CardInfo);
                }
            }
            List<long> cardIdList = new List<long> {
                packetObject.CardInfo.card_id
            };
            Instance.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            TipsDiag.SetText("装备进化成功!");
            ActorData.getInstance().UpdateCard(packetObject.CardInfo);
        }
    }

    [PacketHandler(OpcodeType.S2C_EQUIPLVUP, typeof(S2C_EquipLvUp))]
    public void ReceiveEquipLvUp(Packet pak)
    {
        S2C_EquipLvUp packetObject = (S2C_EquipLvUp) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = ActorData.getInstance().Gold - packetObject.gold;
            ActorData.getInstance().Gold = packetObject.gold;
            SoundManager.mInstance.PlaySFX("sound_ui_z_6");
            ActorData.getInstance().UpdateCard(packetObject.CardInfo);
            ActorData.getInstance().UserInfo.auto_equip_lv_up_count = packetObject.auto_equip_lv_up_count;
            EquipLevUpPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<EquipLevUpPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateLevUp(packetObject.CardInfo, packetObject.bAuto, packetObject.bGoldNotEnough, num);
            }
            BreakEquipPanel panel2 = GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>();
            if (panel2 != null)
            {
                panel2.UpdateLevUp(packetObject.CardInfo, packetObject.bAuto, packetObject.bGoldNotEnough, num);
            }
            HeroInfoPanel panel3 = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
            if (panel3 != null)
            {
                panel3.LevelUpEquipSucess(packetObject.CardInfo);
            }
            List<long> cardIdList = new List<long> {
                packetObject.CardInfo.card_id
            };
            this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            this.CheckLivenessReward();
        }
        else
        {
            EquipLevUpPanel panel4 = GUIMgr.Instance.GetGUIEntity<EquipLevUpPanel>();
            if (panel4 != null)
            {
                panel4.EnablePanelClick(false);
                panel4.CanClickBtn = true;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFLAMEBATTLEINFO, typeof(S2C_GetFlameBattleInfo))]
    public void ReceiveFlameBattleInfo(Packet pak)
    {
        S2C_GetFlameBattleInfo packetObject = (S2C_GetFlameBattleInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().YuanZhengCardStatList = packetObject.flame_battle_info.self_data_list;
            ActorData.getInstance().mFlameBattleInfo = packetObject.flame_battle_info;
            YuanZhengPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.InitFlameBattleInfo(packetObject.flame_battle_info);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_FLAMEBATTLESHOPBUY, typeof(S2C_FlameBattleShopBuy))]
    public void ReceiveFlameBattleShopBuy(Packet pak)
    {
        S2C_FlameBattleShopBuy packetObject = (S2C_FlameBattleShopBuy) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ActorData.getInstance().FlamebattleCoin = packetObject.buyResult.currency_info.flame_battle_coin;
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateItemBySolt(packetObject.buyResult.slot);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFLAMEBATTLESHOPINFO, typeof(S2C_GetFlameBattleShopInfo))]
    public void ReceiveFlameBattleShopInfo(Packet pak)
    {
        S2C_GetFlameBattleShopInfo packetObject = (S2C_GetFlameBattleShopInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.YuanZhengCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_FLAMEBATTLESHOPREFRESH, typeof(S2C_FlameBattleShopRefresh))]
    public void ReceiveFlameBattleShopRefresh(Packet pak)
    {
        S2C_FlameBattleShopRefresh packetObject = (S2C_FlameBattleShopRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().FlamebattleCoin = packetObject.currencyInfo.flame_battle_coin;
            ActorData.getInstance().UserInfo.refreshFlameBattleShopCount = packetObject.refreshCount;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.YuanZhengCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_FLAMEBATTLESMASH, typeof(S2C_FlameBattleSmash))]
    public void ReceiveFlameBattleSmash(Packet pak)
    {
        <ReceiveFlameBattleSmash>c__AnonStorey140 storey = new <ReceiveFlameBattleSmash>c__AnonStorey140 {
            res = (S2C_FlameBattleSmash) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().Gold = storey.res.gold;
            YuanZhengPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SmashSccuess(storey.res.cur_node);
            }
            YuanZhengRushPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengRushPanel>();
            if (panel2 != null)
            {
                panel2.UpdateData(storey.res.reward);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("YuanZhengRushPanel", new Action<GUIEntity>(storey.<>m__F2), base.gameObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_FLAMEBATTLESTART, typeof(S2C_FlameBattleStart))]
    public void ReceiveFlameBattleStart(Packet pak)
    {
        S2C_FlameBattleStart packetObject = (S2C_FlameBattleStart) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleState.GetInstance().DoNormalBattle(packetObject.combat_data, null, BattleNormalGameType.YuanZhengPk, true, "cj_yuanzheng", 1, 5, null, null, delegate (bool isWin, BattleNormalGameType _type, BattleNormalGameResult result) {
                ActorData.getInstance().mCurrAttackersList.Clear();
                ActorData.getInstance().mCurrdefendersList.Clear();
                foreach (BattleGameResultActorInfo info in result.actorInfoes.attackers)
                {
                    if (info.cardEntry >= 0)
                    {
                        FlameBattleBackCardInfo item = new FlameBattleBackCardInfo {
                            card_entry = (ushort) info.cardEntry
                        };
                        if (result.isTimeOut)
                        {
                            item.card_cur_hp = 0;
                            item.card_cur_energy = 0;
                            item.card_max_hp = (ulong) info.maxHp;
                        }
                        else
                        {
                            item.card_cur_hp = (uint) info.hp;
                            item.card_cur_energy = (ushort) info.energy;
                            item.card_max_hp = (ulong) info.maxHp;
                        }
                        ActorData.getInstance().mCurrAttackersList.Add(item);
                    }
                }
                foreach (BattleGameResultActorInfo info3 in result.actorInfoes.defenders)
                {
                    if (info3.cardEntry >= 0)
                    {
                        FlameBattleBackCardInfo info4 = new FlameBattleBackCardInfo {
                            card_entry = (ushort) info3.cardEntry
                        };
                        if (result.isTimeOut)
                        {
                            info4.card_cur_hp = 0;
                            info4.card_cur_energy = 0;
                            info4.card_max_hp = (ulong) info3.maxHp;
                        }
                        else
                        {
                            info4.card_cur_hp = (uint) info3.hp;
                            info4.card_cur_energy = (ushort) info3.energy;
                            info4.card_max_hp = (ulong) info3.maxHp;
                        }
                        ActorData.getInstance().mCurrdefendersList.Add(info4);
                    }
                }
                FlameBattleBack back = new FlameBattleBack {
                    self_card_data = ActorData.getInstance().mCurrAttackersList,
                    target_card_data = ActorData.getInstance().mCurrdefendersList
                };
                BattleBack data = new BattleBack {
                    result = !result.isTimeOut ? isWin : true
                };
                this.RequestBattleEnd(data, back);
            });
        }
    }

    [PacketHandler(OpcodeType.S2C_FRIENDCOMBAT, typeof(S2C_FriendCombat))]
    public void ReceiveFriendCombat(Packet pak)
    {
        <ReceiveFriendCombat>c__AnonStorey124 storey = new <ReceiveFriendCombat>c__AnonStorey124 {
            res = (S2C_FriendCombat) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__BE), null);
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_GAINFLAMEBATTLEREWARD, typeof(S2C_GainFlameBattleReward))]
    public void ReceiveGainFlameBattleReward(Packet pak)
    {
        <ReceiveGainFlameBattleReward>c__AnonStorey13F storeyf = new <ReceiveGainFlameBattleReward>c__AnonStorey13F {
            res = (S2C_GainFlameBattleReward) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyf.res.result))
        {
            RewardPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<RewardPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowYuanZhengBattleReward(storeyf.res);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeyf.<>m__F1), null);
            }
            ActorData.getInstance().mFlameBattleInfo.cur_node = storeyf.res.cur_node;
            YuanZhengPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.UpdateNodeInfo(storeyf.res.cur_node);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETACTIVITYDATA, typeof(S2C_GetActivityData))]
    public void ReceiveGetActiveList(Packet pak)
    {
        ActiveList.actives.Clear();
        S2C_GetActivityData packetObject = (S2C_GetActivityData) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            for (int i = 0; i < packetObject.datas.Count; i++)
            {
                int num2;
                TxActivityRewardDescribeConfig config;
                int num4;
                TencentStoreCommodity commodity;
                int num5;
                int num6;
                TxActivityRewardDescribeConfig config4;
                ActiveInfo item = new ActiveInfo {
                    is_new = packetObject.datas[i].is_new,
                    activity_unid = packetObject.datas[i].activity_unid,
                    entry = packetObject.datas[i].activity_entry,
                    activity_type = packetObject.datas[i].activity_type,
                    sort_priority = packetObject.datas[i].sort_priority,
                    activity_name = packetObject.datas[i].activity_name
                };
                foreach (string str in packetObject.datas[i].activity_describe)
                {
                    item.activity_describe = item.activity_describe + str;
                }
                item.activity_time_describe = packetObject.datas[i].activity_brief;
                item.startTime = packetObject.datas[i].start_time;
                item.cdTime = packetObject.datas[i].cd_time;
                item.holdOnTime = packetObject.datas[i].duration;
                item.activity_PicName = packetObject.datas[i].background_img;
                item.activity_PicNameDetail = packetObject.datas[i].detial_img;
                item.activity_showUsePic = packetObject.datas[i].show_use_pic;
                item.dayParameter = packetObject.datas[i].dayParameter;
                item.ActParameter = packetObject.datas[i].parameter;
                ActivityType type = packetObject.datas[i].activity_type;
                switch (type)
                {
                    case ActivityType.e_tencent_activity_login:
                        num2 = 0;
                        goto Label_0395;

                    case ActivityType.e_tencent_activity_charge:
                        goto Label_03E6;

                    case ActivityType.e_tencent_activity_shop_package:
                        item.showType = ActiveShowType.present;
                        num4 = 0;
                        goto Label_07EB;

                    default:
                        switch (type)
                        {
                            case ActivityType.e_tencent_activity_consume:
                                goto Label_03E6;

                            case ActivityType.e_tencent_activity_first_charge:
                                item.showType = ActiveShowType.pic;
                                Debug.Log("=========rres.datas[i].subActRewards.Count：" + packetObject.datas[i].subActRewards.Count);
                                num5 = 0;
                                goto Label_08E5;

                            case ActivityType.e_tencent_activity_notice:
                                goto Label_0ABA;

                            case ActivityType.e_tencent_activity_collect_exchange:
                                item.showType = ActiveShowType.collectExchange;
                                num6 = 0;
                                goto Label_0A98;

                            default:
                                goto Label_0ABA;
                        }
                        break;
                }
            Label_0237:
                config = new TxActivityRewardDescribeConfig();
                config.type = packetObject.datas[i].subActRewards[num2].type;
                config.entry = packetObject.datas[i].subActRewards[num2].entry;
                config.reward_describe = packetObject.datas[i].subActRewards[num2].name;
                config.reward_items = packetObject.datas[i].subActRewards[num2].tips_describe;
                config.flag = packetObject.datas[i].subActRewards[num2].flag;
                config.subTimeEnable = packetObject.datas[i].subActRewards[num2].subTimeEnable;
                config.start_time = (int) packetObject.datas[i].subActRewards[num2].start_time;
                config.duration = packetObject.datas[i].subActRewards[num2].duration;
                config.cd_time = packetObject.datas[i].subActRewards[num2].cd_time;
                item.rewards_configs.Add(config);
                num2++;
            Label_0395:
                if (num2 < packetObject.datas[i].subActRewards.Count)
                {
                    goto Label_0237;
                }
                if (packetObject.datas[i].subActRewards.Count <= 0)
                {
                    item.showType = ActiveShowType.describe;
                }
                else
                {
                    item.showType = ActiveShowType.recharge;
                }
                goto Label_0AC6;
            Label_03E6:
                item.showType = ActiveShowType.recharge;
                for (int j = 0; j < packetObject.datas[i].subActRewards.Count; j++)
                {
                    TxActivityRewardDescribeConfig config2 = new TxActivityRewardDescribeConfig {
                        type = packetObject.datas[i].subActRewards[j].type,
                        entry = packetObject.datas[i].subActRewards[j].entry,
                        reward_describe = packetObject.datas[i].subActRewards[j].name,
                        reward_items = packetObject.datas[i].subActRewards[j].tips_describe,
                        flag = packetObject.datas[i].subActRewards[j].flag,
                        subTimeEnable = packetObject.datas[i].subActRewards[j].subTimeEnable,
                        start_time = (int) packetObject.datas[i].subActRewards[j].start_time,
                        duration = packetObject.datas[i].subActRewards[j].duration,
                        cd_time = packetObject.datas[i].subActRewards[j].cd_time
                    };
                    item.rewards_configs.Add(config2);
                }
                goto Label_0AC6;
            Label_0584:
                commodity = new TencentStoreCommodity();
                commodity.commodityId = packetObject.datas[i].commodies[num4].commodityId;
                commodity.reward_describe = packetObject.datas[i].commodies[num4].name;
                commodity.reward_items = packetObject.datas[i].commodies[num4].tips_describe;
                commodity.flag = packetObject.datas[i].commodies[num4].flag;
                Debug.Log("========res.commodies[j].canBuy: " + packetObject.datas[i].commodies[num4].flag);
                if (packetObject.datas[i].commodies[num4].costStone != -1)
                {
                    commodity.reward_Price = packetObject.datas[i].commodies[num4].costStone;
                    commodity.costType = CostType.E_CT_Stone;
                }
                else
                {
                    commodity.reward_Price = packetObject.datas[i].commodies[num4].costGold;
                    commodity.costType = CostType.E_CT_Gold;
                }
                Debug.Log("=========res.commodies[j].describe==服务器传来活动子类奖励：" + packetObject.datas[i].commodies[num4].describe);
                commodity.purchaseCountOfDay = packetObject.datas[i].commodies[num4].purchaseCountOfDay;
                commodity.purchaseCount = packetObject.datas[i].commodies[num4].purchaseCount;
                commodity.serverPurCntOfDay = packetObject.datas[i].commodies[num4].serverPurCntOfDay;
                commodity.serverPurCnt = packetObject.datas[i].commodies[num4].serverPurCnt;
                commodity.mainDescribe = packetObject.datas[i].commodies[num4].describe;
                Debug.Log("礼包描述字段___：" + packetObject.datas[i].commodies[num4].describe);
                item.storeList.Add(commodity);
                num4++;
            Label_07EB:
                if (num4 < packetObject.datas[i].commodies.Count)
                {
                    goto Label_0584;
                }
                goto Label_0AC6;
            Label_0846:
                item.flag = packetObject.datas[i].subActRewards[num5].flag;
                Debug.Log("=========info.flagt：" + item.flag);
                TxActivityRewardDescribeConfig config3 = new TxActivityRewardDescribeConfig {
                    flag = packetObject.datas[i].subActRewards[num5].flag,
                    entry = packetObject.datas[i].subActRewards[num5].entry
                };
                item.rewards_configs.Add(config3);
                num5++;
            Label_08E5:
                if (num5 < packetObject.datas[i].subActRewards.Count)
                {
                    goto Label_0846;
                }
                goto Label_0AC6;
            Label_0916:
                config4 = new TxActivityRewardDescribeConfig();
                config4.type = packetObject.datas[i].subActRewards[num6].type;
                config4.entry = packetObject.datas[i].subActRewards[num6].entry;
                config4.reward_describe = packetObject.datas[i].subActRewards[num6].name;
                config4.reward_items = packetObject.datas[i].subActRewards[num6].tips_describe;
                config4.exchangeNeedConfig = packetObject.datas[i].subActRewards[num6].exchangeNeedConfig;
                config4.flag = packetObject.datas[i].subActRewards[num6].flag;
                config4.subTimeEnable = packetObject.datas[i].subActRewards[num6].subTimeEnable;
                config4.start_time = (int) packetObject.datas[i].subActRewards[num6].start_time;
                config4.duration = packetObject.datas[i].subActRewards[num6].duration;
                config4.cd_time = packetObject.datas[i].subActRewards[num6].cd_time;
                item.rewards_configs.Add(config4);
                num6++;
            Label_0A98:
                if (num6 < packetObject.datas[i].subActRewards.Count)
                {
                    goto Label_0916;
                }
                goto Label_0AC6;
            Label_0ABA:
                item.showType = ActiveShowType.describe;
            Label_0AC6:
                ActiveList.actives.Add(item);
            }
            ActiveList.actives.Sort(new Comparison<ActiveInfo>(this.SortActiveList));
            this.CheckActiveState(ActiveList.actives);
            if (packetObject.datas.Count > 0)
            {
                if (ActiveMgr.inst != null)
                {
                    ActiveMgr.inst.ShowActive(true);
                }
            }
            else if (ActiveMgr.inst != null)
            {
                ActiveMgr.inst.ShowActive(false);
            }
            if ((packetObject.datas.Count > 0) && (ActivePanel.inst != null))
            {
                ActivePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ActivePanel>();
                if (gUIEntity != null)
                {
                    gUIEntity.InitActiveBtnInfo();
                }
            }
            else
            {
                ActivePanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ActivePanel>();
                if ((activityGUIEntity != null) && (ActivePanel.inst != null))
                {
                    activityGUIEntity.InitActiveBtnInfo();
                }
            }
            if (ActiveMgr.press)
            {
                GUIMgr.Instance.PushGUIEntity("ActivePanel", null);
                GUIMgr.Instance.FloatTitleBar();
                ActiveMgr.press = false;
                if (Instance != null)
                {
                    Instance.RequestRewardFlag();
                }
            }
        }
        else if (!this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    [PacketHandler(OpcodeType.S2C_GETASSISTUSERLIST, typeof(S2C_GetAssistUserList))]
    public void ReceiveGetAssistUserList(Packet pak)
    {
        S2C_GetAssistUserList packetObject = (S2C_GetAssistUserList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().AssistUserList = packetObject.userList;
            ActorData.getInstance().UpdateAssistToTowerData();
        }
        else
        {
            Debug.LogWarning(packetObject.result);
        }
    }

    [PacketHandler(OpcodeType.S2C_GETBATTLEFORMATION, typeof(S2C_GetBattleFormation))]
    public void ReceiveGetBattleFormation(Packet pak)
    {
        ActorData.getInstance().LoadingUserInfoProgress = 0.2f;
        S2C_GetBattleFormation packetObject = (S2C_GetBattleFormation) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            switch (packetObject.formation.type)
            {
                case BattleFormationType.BattleFormationType_Arena_Def:
                    ActorData.getInstance().ArenaFormation = packetObject.formation;
                    break;

                case BattleFormationType.BattleFormationType_League_Def:
                    ActorData.getInstance().WorldCupFormation = packetObject.formation;
                    break;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETCARDBAG, typeof(S2C_GetCardBag))]
    public void ReceiveGetCardBag(Packet pak)
    {
        S2C_GetCardBag packetObject = (S2C_GetCardBag) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.4f;
            ActorData.getInstance().CardList = packetObject.cardList;
            List<long> cardIdList = new List<long>();
            foreach (Card card in packetObject.cardList)
            {
                cardIdList.Add(card.card_id);
            }
            this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            ActorData.getInstance().GetFormationData();
            Debug.Log("get cards info");
        }
    }

    [PacketHandler(OpcodeType.S2C_GETDUPLICATEPROGRESS, typeof(S2C_GetDuplicateProgress))]
    public void ReceiveGetDuplicateProgress(Packet pak)
    {
        S2C_GetDuplicateProgress packetObject = (S2C_GetDuplicateProgress) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.1f;
            if (((packetObject.normalProgress - ActorData.getInstance().NormalProgress) > 0) && (ActorData.getInstance().NormalProgress != 0))
            {
                ActorData.getInstance().OpenNewDup = true;
            }
            Debug.Log("---------------------------获得玩家副本----------ActorData.getInstance ().NormalProgress--" + ActorData.getInstance().NormalProgress);
            ActorData.getInstance().NormalProgress = packetObject.normalProgress;
            ActorData.getInstance().EliteProgress = packetObject.eliteProgress;
            ActorData.getInstance().HeroProgress = packetObject.heroProgress;
            if ((packetObject.normalProgress == 1) && (GuideSystem.progress < 7))
            {
                GuideSystem.SkipToEvent(GuideEvent.EquipLevelUp_Portal, true);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETDUPLICATEREMAIN, typeof(S2C_GetDuplicateRemain))]
    public void ReceiveGetDuplicateRemain(Packet pak)
    {
        DupMap map;
        S2C_GetDuplicateRemain packetObject = (S2C_GetDuplicateRemain) pak.PacketObject;
        if (!this.CheckResultCodeAndReLogin(packetObject.result))
        {
            return;
        }
        switch (packetObject.dupType)
        {
            case DuplicateType.DupType_Normal:
                if (!ActorData.getInstance().TrenchNormalDataDict.ContainsKey(packetObject.dupEntry))
                {
                    ActorData.getInstance().TrenchNormalDataDict.Add(packetObject.dupEntry, packetObject.remainList);
                }
                else
                {
                    ActorData.getInstance().TrenchNormalDataDict[packetObject.dupEntry] = packetObject.remainList;
                }
                goto Label_0169;

            case DuplicateType.DupType_Elite:
                if (!ActorData.getInstance().TrenchEliteDataDict.ContainsKey(packetObject.dupEntry))
                {
                    ActorData.getInstance().TrenchEliteDataDict.Add(packetObject.dupEntry, packetObject.remainList);
                    break;
                }
                ActorData.getInstance().TrenchEliteDataDict[packetObject.dupEntry] = packetObject.remainList;
                break;

            case DuplicateType.DupType_Hero:
                if (!ActorData.getInstance().TrenchHeroDataDict.ContainsKey(packetObject.dupEntry))
                {
                    ActorData.getInstance().TrenchHeroDataDict.Add(packetObject.dupEntry, packetObject.remainList);
                }
                else
                {
                    ActorData.getInstance().TrenchHeroDataDict[packetObject.dupEntry] = packetObject.remainList;
                }
                goto Label_0169;

            default:
                goto Label_0169;
        }
        ResultPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ResultPanel>();
        if (gUIEntity != null)
        {
            gUIEntity.CanReturn = true;
        }
    Label_0169:
        map = (DupMap) GUIMgr.Instance.GetGUIEntity("DupMap");
        if (map != null)
        {
            map.UpdateDupRewardInfo();
            map.UpdateLevelCountLabel(false);
        }
        DupLevInfoPanel panel2 = (DupLevInfoPanel) GUIMgr.Instance.GetGUIEntity("DupLevInfoPanel");
        if (panel2 != null)
        {
            panel2.UpdateAckCount();
            panel2.UpdateGrade();
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFLAMEBATTLETARGETSTATUS, typeof(S2C_GetFlameBattleTargetStatus))]
    public void ReceiveGetFlameBattleTargetStatus(Packet pak)
    {
        S2C_GetFlameBattleTargetStatus packetObject = (S2C_GetFlameBattleTargetStatus) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TargetTeamPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TargetTeamPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetRefreshBtnStat(packetObject.status);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFRIENDFORMATION, typeof(S2C_GetFriendFormation))]
    public void ReceiveGetFriendFormation(Packet pak)
    {
        <ReceiveGetFriendFormation>c__AnonStorey123 storey = new <ReceiveGetFriendFormation>c__AnonStorey123 {
            res = (S2C_GetFriendFormation) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().CurrFriendPkInfo = storey.res;
            GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storey.<>m__BC), GUIMgr.Instance.GetGUIEntity<FriendInfoPanel>().gameObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFRIENDLIST, typeof(S2C_GetFriendList))]
    public void ReceiveGetFriendList(Packet pak)
    {
        S2C_GetFriendList packetObject = (S2C_GetFriendList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.5f;
            ActorData.getInstance().FriendList = packetObject.friendList;
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETFRIENDREQLIST, typeof(S2C_GetFriendReqList))]
    public void ReceiveGetFriendReqList(Packet pak)
    {
        S2C_GetFriendReqList packetObject = (S2C_GetFriendReqList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.6f;
            ActorData.getInstance().FriendReqList = packetObject.userList;
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetFriendTips();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDLISTREQ, typeof(S2C_GuildListReq))]
    public void ReceiveGetGuildList(Packet pak)
    {
        S2C_GuildListReq packetObject = (S2C_GuildListReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("GuildListPanel");
            if (gUIEntity != null)
            {
                GuildListPanel panel = gUIEntity.Achieve<GuildListPanel>();
                if (panel != null)
                {
                    panel.UpdateGuildList(packetObject);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REQHEADFRAMELIST, typeof(S2C_GetHeadFrameList))]
    public void ReceiveGetHeadFrameList(Packet pak)
    {
        <ReceiveGetHeadFrameList>c__AnonStorey146 storey = new <ReceiveGetHeadFrameList>c__AnonStorey146 {
            res = (S2C_GetHeadFrameList) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("PlayerIconFramePanel", new Action<GUIEntity>(storey.<>m__FB), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_GETITEMLIST, typeof(S2C_GetItemList))]
    public void ReceiveGetItemList(Packet pak)
    {
        S2C_GetItemList packetObject = (S2C_GetItemList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.3f;
            ActorData.getInstance().ItemList = packetObject.itemList;
        }
    }

    [PacketHandler(OpcodeType.S2C_GETJOINLEAGUE, typeof(S2C_GetJoinLeague))]
    public void ReceiveGetJoinLeague(Packet pak)
    {
        S2C_GetJoinLeague packetObject = (S2C_GetJoinLeague) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().JoinLeagueInfo = packetObject;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("WorldCupPanel");
            if (gUIEntity != null)
            {
                (gUIEntity as WorldCupPanel).JoinLeagueSucess();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLEAGUEOPPONENTFORMATION, typeof(S2C_GetLeagueOpponentFormation))]
    public void ReceiveGetLeagueOpponentFormation(Packet pak)
    {
        <ReceiveGetLeagueOpponentFormation>c__AnonStorey105 storey = new <ReceiveGetLeagueOpponentFormation>c__AnonStorey105 {
            res = (S2C_GetLeagueOpponentFormation) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            TargetInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TargetInfoPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.SetTargetTeamInfo(storey.res.cardList);
            }
            else
            {
                ActorData.getInstance().CurrWorldCupPkInfo = storey.res;
                ActorData.getInstance().CurrWorldCupPkTargetId = storey.res.targetId;
                if (ActorData.getInstance().IsOnlyShowTargetTeam)
                {
                    GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storey.<>m__94), GUIMgr.Instance.GetGUIEntity<WorldCupPanel>().gameObject);
                }
                else
                {
                    GUIMgr.Instance.PushGUIEntity("SelectHeroPanel", new Action<GUIEntity>(storey.<>m__95));
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLEAGUEOPPONENTLIST, typeof(S2C_GetLeagueOpponentList))]
    public void ReceiveGetLeagueOpponentList(Packet pak)
    {
        S2C_GetLeagueOpponentList packetObject = (S2C_GetLeagueOpponentList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("WorldCupPanel");
            if (gUIEntity != null)
            {
                (gUIEntity as WorldCupPanel).UpdatePlayerList(packetObject.opponentList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLEAGUERANKLIST, typeof(S2C_GetLeagueRankList))]
    public void ReceiveGetLeagueRankList(Packet pak)
    {
        S2C_GetLeagueRankList packetObject = (S2C_GetLeagueRankList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            RankingListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<RankingListPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(packetObject.userList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLEAGUEREWARD, typeof(S2C_GetLeagueReward))]
    public void ReceiveGetLeagueReward(Packet pak)
    {
        S2C_GetLeagueReward packetObject = (S2C_GetLeagueReward) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().CurrLeagueReward = packetObject;
            if (GUIMgr.Instance.GetGUIEntity<WorldCupPanel>() != null)
            {
                this.RequestGetJoinLeague();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLOTTERYCARDINFO, typeof(S2C_GetLotteryCardInfo))]
    public void ReceiveGetLotteryCardInfo(Packet pak)
    {
        S2C_GetLotteryCardInfo packetObject = (S2C_GetLotteryCardInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mLotteryCardList = packetObject.lotteryList;
            ActorData.getInstance().mLotteryCardDicountList = packetObject.discountList;
            GameDataMgr.Instance.boostRecruit.RefreshAllSaleRemainTimes(packetObject.discountList);
        }
    }

    [PacketHandler(OpcodeType.S2C_GETMAILLIST, typeof(S2C_GetMailList))]
    public void ReceiveGetMailList(Packet pak)
    {
        S2C_GetMailList packetObject = (S2C_GetMailList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().MailList = packetObject.mailList;
            ActorData.getInstance().LoadingUserInfoProgress = 0.98f;
            ActorData.getInstance().NextRefreshNewMailTime = TimeMgr.Instance.ServerStampTime + CommonFunc.GetRefreshMailInterval();
            MailListPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MailListPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.InitMailList();
            }
            ActorData.getInstance().mHaveMailRefresh = true;
        }
    }

    [PacketHandler(OpcodeType.S2C_GETQUESTLIST, typeof(S2C_GetQuestList))]
    public void ReceiveGetQuestList(Packet pak)
    {
        S2C_GetQuestList packetObject = (S2C_GetQuestList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().QuestList = packetObject.questList;
            AchievementPanel gUIEntity = (AchievementPanel) GUIMgr.Instance.GetGUIEntity("AchievementPanel");
            if ((null != gUIEntity) && !gUIEntity.Hidden)
            {
                gUIEntity.Refresh();
            }
            if (GUIMgr.Instance.GetGUIEntity<TitlePanel>() != null)
            {
                this.RequestGetTitleList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETSKILLPOINT, typeof(S2C_GetSkillPoint))]
    public void ReceiveGetSkillPoint(Packet pak)
    {
        S2C_GetSkillPoint packetObject = (S2C_GetSkillPoint) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().CurSkillPoint = packetObject.skillPoint;
            HeroInfoPanel gUIEntity = (HeroInfoPanel) GUIMgr.Instance.GetGUIEntity("HeroInfoPanel");
            if (gUIEntity != null)
            {
                gUIEntity.SkillPoint = packetObject.skillPoint;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETTITLELIST, typeof(S2C_GetTitleList))]
    public void ReceiveGetTitleList(Packet pak)
    {
        S2C_GetTitleList packetObject = (S2C_GetTitleList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.6f;
            if (packetObject.titleList.Count > ActorData.getInstance().TitleCount)
            {
                ActorData.getInstance().mHaveNewTitle = true;
            }
            ActorData.getInstance().TitleList = packetObject.titleList;
            Debug.Log(packetObject.titleList.Count + "~~~~~~~~~~");
            TitlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TitlePanel>();
            if (gUIEntity != null)
            {
                gUIEntity.initTitleList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETUSERINFO, typeof(S2C_GetUserInfo))]
    public void ReceiveGetUserInfo(Packet pak)
    {
        <ReceiveGetUserInfo>c__AnonStorey143 storey = new <ReceiveGetUserInfo>c__AnonStorey143 {
            res = (S2C_GetUserInfo) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            if (storey.res.userInfo.frozen_time > TimeMgr.Instance.ServerStampTime)
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__F8), null);
            }
            else
            {
                ActorData.getInstance().UserInfo = storey.res.userInfo;
                ActorData.getInstance().UserInfo.vip_level.level = storey.res.userInfo.vip_level.level + 1;
                Debug.Log("get user info:" + storey.res.userInfo.name);
                ActorData.getInstance().InitAddPhyForce(storey.res.userInfo.passTime);
                GameDataMgr.Instance.Login();
                SignInPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<SignInPanel>();
                if (null != gUIEntity)
                {
                    gUIEntity.UpdateSignBtn();
                }
                TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                if (null != activityGUIEntity)
                {
                    activityGUIEntity.UpdateTiLi(false);
                }
                ActorData.getInstance().mRequestCallCard = false;
                ActorData.getInstance().mChatList.Clear();
                ActorData.getInstance().PushSendTiliFullMsg = 0;
                ActorData.getInstance().PushSendBoostRecruit = 0;
                ActorData.getInstance().PushSendShopRefresh = 0;
                ActorData.getInstance().PushSendSkillPointFull = 0;
                ActorData.getInstance().mUserGuildMemberData = null;
                Instance.RequestGetSkillPoint();
                ActorData.getInstance().mFlameBattleInfo = null;
                ActorData.getInstance().GuildApplyList.Clear();
                ActorData.getInstance().ReadShopTips();
                ActorData.getInstance().HaveNewArenaLog = false;
                ActorData.getInstance().HaveNewArenaChallengeLog = false;
                ActorData.getInstance().UserInfo.vip_level.level = storey.res.userInfo.vip_level.level + 1;
                if (ActorData.getInstance().UserInfo.level >= CommonFunc.LevelLimitCfg().quest)
                {
                    Instance.RequestGetQuestList();
                }
                if (ActorData.getInstance().UserInfo.level >= CommonFunc.LevelLimitCfg().daily_misson)
                {
                    Instance.RequestDailyQuest();
                }
                if (ActorData.getInstance().UserInfo.level >= CommonFunc.LevelLimitCfg().daily_misson)
                {
                    Instance.RequestDailyQuest();
                }
                if (ActorData.getInstance().UserInfo.level >= CommonFunc.LevelLimitCfg().pub)
                {
                    Instance.RequestLotteryCardInfo();
                }
                ActorData.getInstance().RequestOtherInfo();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GIVEFRIENDPHYFORCE, typeof(S2C_GiveFriendPhyForce))]
    public void ReceiveGiveFriendPhyForce(Packet pak)
    {
        S2C_GiveFriendPhyForce packetObject = (S2C_GiveFriendPhyForce) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord((packetObject.targetId != 0) ? 0x9896a8 : 0x598));
            ActorData.getInstance().SetZhenSongTiLiState(packetObject.targetId);
            GUIMgr.Instance.ExitModelGUI("FriendInfoPanel");
        }
    }

    [PacketHandler(OpcodeType.S2C_GMCOMMAND, typeof(S2C_GmCommand))]
    public void ReceiveGmCommand(Packet pak)
    {
        S2C_GmCommand packetObject = (S2C_GmCommand) pak.PacketObject;
        Debug.Log("GM Succeed");
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDAPPLICATIONREQ, typeof(S2C_GuildApplicationReq))]
    public void ReceiveGuildApplication(Packet pak)
    {
        S2C_GuildApplicationReq packetObject = (S2C_GuildApplicationReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().GuildApplyList = packetObject.applications;
            GuildPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuildPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.CheckGuildApplyStat();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDAPPLICATIONPROCESS, typeof(S2C_GuildApplicationProcess))]
    public void ReceiveGuildApplicationProcess(Packet pak)
    {
        S2C_GuildApplicationProcess packetObject = (S2C_GuildApplicationProcess) pak.PacketObject;
        if ((this.CheckResultCodeAndReLogin(packetObject.result) || (packetObject.result == OpResult.OpResult_Guild_Apply_Too_Much)) || (((packetObject.result == OpResult.OpResult_Guild_Application_Not_Exist) || (packetObject.result == OpResult.OpResult_Tartget_Guild_Already)) || (packetObject.result == OpResult.OpResult_Guild_Already)))
        {
            ActorData.getInstance().SetGuildData(packetObject.data, packetObject.member_data);
            ActorData.getInstance().GuildApplyList = packetObject.applications;
            GuildPanel gUIEntity = (GuildPanel) GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (gUIEntity != null)
            {
                gUIEntity.GetNewData();
                gUIEntity.UpdateGuildMember();
                gUIEntity.UpdateMemberCount();
                gUIEntity.CheckGuildApplyStat();
            }
            GuildApplyPanel panel2 = (GuildApplyPanel) GUIMgr.Instance.GetGUIEntity("GuildApplyPanel");
            if (panel2 != null)
            {
                panel2.UpdateMemberCount();
                panel2.UpdateData(packetObject.applications);
            }
            GuildMgrPanel panel3 = (GuildMgrPanel) GUIMgr.Instance.GetGUIEntity("GuildMgrPanel");
            if (panel3 != null)
            {
                panel3.CheckApplyStat();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDAPPLY, typeof(S2C_GuildApply))]
    public void ReceiveGuildApply(Packet pak)
    {
        S2C_GuildApply packetObject = (S2C_GuildApply) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.user_data.guild_id > 0L)
            {
                ActorData.getInstance().SetGuildData(packetObject.data, packetObject.user_data, packetObject.member_data);
                GUIMgr.Instance.PopGUIEntity();
                if (<>f__am$cache30 == null)
                {
                    <>f__am$cache30 = obj => obj.Achieve<GuildPanel>().UpdateData();
                }
                GUIMgr.Instance.PushGUIEntity("GuildPanel", <>f__am$cache30);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65286));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLEBUYATTACKTIMES, typeof(S2C_GuildBattleBuyAttackTimes))]
    public void ReceiveGuildBattleBuyTimes(Packet pak)
    {
        S2C_GuildBattleBuyAttackTimes packetObject = (S2C_GuildBattleBuyAttackTimes) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().nCurAttackTimes = packetObject.curAttackTimes;
            ActorData.getInstance().nTotalAttackTime = packetObject.totalAttackTimes;
            GuildBattlePanel gUIEntity = (GuildBattlePanel) GUIMgr.Instance.GetGUIEntity("GuildBattlePanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildBattleTimesInfo();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLECOMBAT, typeof(S2C_GuildBattleCombat))]
    public void ReceiveGuildBattleCombat(Packet pak)
    {
        <ReceiveGuildBattleCombat>c__AnonStorey126 storey = new <ReceiveGuildBattleCombat>c__AnonStorey126 {
            <>f__this = this,
            res = (S2C_GuildBattleCombat) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().m_resourceEntry = storey.res.resourceEntry;
            guild_source_point_config _config = ConfigMgr.getInstance().getByEntry<guild_source_point_config>(storey.res.resourceEntry);
            BattleState.GetInstance().DoNormalBattle(storey.res.combatData, null, BattleNormalGameType.GuildBattle, true, _config.scene, 1, 0, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storey.<>m__C2));
        }
        else
        {
            Debug.Log("ReceiveGuildBattleCombat err " + storey.res.result);
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLECOMBATEND, typeof(S2C_GuildBattleCombatEnd))]
    public void ReceiveGuildBattleCombatEnd(Packet pak)
    {
        <ReceiveGuildBattleCombatEnd>c__AnonStorey127 storey = new <ReceiveGuildBattleCombatEnd>c__AnonStorey127 {
            res = (S2C_GuildBattleCombatEnd) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().mUserGuildMemberData.contribution += storey.res.contribute;
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__C3), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLEDAMAGERANK, typeof(S2C_GuildBattleDamageRank))]
    public void ReceiveGuildBattleDamageRank(Packet pak)
    {
        S2C_GuildBattleDamageRank packetObject = (S2C_GuildBattleDamageRank) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildRankListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildRankListPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateDamageData(packetObject.rank);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLEINFO, typeof(S2C_GuildBattleInfo))]
    public void ReceiveGuildBattleInfo(Packet pak)
    {
        <ReceiveGuildBattleInfo>c__AnonStorey125 storey = new <ReceiveGuildBattleInfo>c__AnonStorey125 {
            res = (S2C_GuildBattleInfo) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.PushGUIEntity("GuildBattlePanel", new Action<GUIEntity>(storey.<>m__C1));
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLEWINTIMESRANK, typeof(S2C_GuildBattleWinTimesRank))]
    public void ReceiveGuildBattleMarsRank(Packet pak)
    {
        S2C_GuildBattleWinTimesRank packetObject = (S2C_GuildBattleWinTimesRank) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildRankListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildRankListPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateWinTimesData(packetObject.rank);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBATTLERESOURCERANK, typeof(S2C_GuildBattleResourceRank))]
    public void ReceiveGuildBattleResourceRank(Packet pak)
    {
        S2C_GuildBattleResourceRank packetObject = (S2C_GuildBattleResourceRank) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildRankListPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildRankListPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateResourceData(packetObject.rank);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_QUERYGUILDBINDINFO, typeof(S2C_QueryGuildBindInfo))]
    public void ReceiveGuildBindQQInfo(Packet pak)
    {
        S2C_QueryGuildBindInfo packetObject = (S2C_QueryGuildBindInfo) pak.PacketObject;
        Debug.Log("ReceiveGuildBindQQInfo: " + packetObject.result);
        if (packetObject.result == OpResult.OpResult_QueryGuildBindInfo_s)
        {
            if (<>f__am$cache31 == null)
            {
                <>f__am$cache31 = () => Instance.RequestGuildBindQQInfo();
            }
            ScheduleMgr.Schedule(1f, <>f__am$cache31);
            Debug.Log("RequestGuildBindQQInfo Again!");
        }
        else if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildBindQQInfo(packetObject.qq_group_openid, packetObject.qq_group_name, packetObject.is_qq_group_member);
            }
        }
        else
        {
            Debug.Log(" Query QQ BindInfo Failded!----------- ");
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBUFFLEVELUP, typeof(S2C_GuildBuffLevelUp))]
    public void ReceiveGuildBufferLevUp(Packet pak)
    {
        S2C_GuildBuffLevelUp packetObject = (S2C_GuildBuffLevelUp) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mGuildData = packetObject.data;
            GuildLevUpPanel gUIEntity = (GuildLevUpPanel) GUIMgr.Instance.GetGUIEntity("GuildLevUpPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateBuffer();
            }
            GuildTechPanel panel2 = (GuildTechPanel) GUIMgr.Instance.GetGUIEntity("GuildTechPanel");
            if (panel2 != null)
            {
                panel2.UpdateContribution();
                panel2.UpdateTechItem();
            }
            GuildPanel panel3 = (GuildPanel) GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (panel3 != null)
            {
                panel3.GetNewData();
                panel3.UpdateContribution();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBUILDLEVELUP, typeof(S2C_GuildBuildLevelup))]
    public void ReceiveGuildBuildLevUp(Packet pak)
    {
        S2C_GuildBuildLevelup packetObject = (S2C_GuildBuildLevelup) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mGuildData = packetObject.data;
            SoundManager.mInstance.PlaySFX("sound_ui_z_12");
            GuildLevUpPanel gUIEntity = (GuildLevUpPanel) GUIMgr.Instance.GetGUIEntity("GuildLevUpPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData();
            }
            GuildHallPanel panel2 = (GuildHallPanel) GUIMgr.Instance.GetGUIEntity("GuildHallPanel");
            if (panel2 != null)
            {
                panel2.UpdateContribution();
                panel2.UpdateHallLev();
                panel2.UpdateMemberCnt();
            }
            GuildShopPanel panel3 = (GuildShopPanel) GUIMgr.Instance.GetGUIEntity("GuildShopPanel");
            if (panel3 != null)
            {
                panel3.UpdateFreeRefreshCount();
                panel3.UpdateShopItem();
                panel3.UpdateContribution();
                panel3.UpdateBuildLev();
            }
            GuildTechPanel panel4 = (GuildTechPanel) GUIMgr.Instance.GetGUIEntity("GuildTechPanel");
            if (panel4 != null)
            {
                panel4.UpdateTechItem();
                panel4.UpdateBuildLev();
                panel4.UpdateContribution();
            }
            GuildPanel panel5 = (GuildPanel) GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (panel5 != null)
            {
                panel5.GetNewData();
                panel5.UpdateGuildLevel();
                panel5.UpdateMemberCount();
                panel5.UpdateFuncListLev();
                panel5.UpdateContribution();
            }
            GuildMgrPanel panel6 = (GuildMgrPanel) GUIMgr.Instance.GetGUIEntity("GuildMgrPanel");
            if (panel6 != null)
            {
                panel6.UpdateGuildLevel();
            }
            PaokuPanel panel7 = (PaokuPanel) GUIMgr.Instance.GetGUIEntity("PaokuPanel");
            if (panel7 != null)
            {
                panel7.InitUIData();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDBUYGOODS, typeof(S2C_GuildBuyGoods))]
    public void ReceiveGuildBuy(Packet pak)
    {
        S2C_GuildBuyGoods packetObject = (S2C_GuildBuyGoods) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().UpdateBattleRewardData(packetObject.reward);
            GuildShopPanel gUIEntity = (GuildShopPanel) GUIMgr.Instance.GetGUIEntity("GuildShopPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateOwnContribution();
                gUIEntity.UpdateShopItem();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDCONTRIBUTE, typeof(S2C_GuildContribute))]
    public void ReceiveGuildContribute(Packet pak)
    {
        S2C_GuildContribute packetObject = (S2C_GuildContribute) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().SetGuildData(packetObject.data, packetObject.user_data);
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65285));
            GUIMgr.Instance.ExitModelGUI("MessageBox");
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (gUIEntity != null)
            {
                GuildPanel panel = gUIEntity.Achieve<GuildPanel>();
                if (panel != null)
                {
                    panel.GetNewData();
                    panel.UpdateContribution();
                    Instance.RequestGuildData(true, null);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDCREATE, typeof(S2C_GuildCreate))]
    public void ReceiveGuildCreate(Packet pak)
    {
        S2C_GuildCreate packetObject = (S2C_GuildCreate) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            SoundManager.mInstance.PlaySFX("sound_ui_z_11");
            ActorData.getInstance().SetGuildData(packetObject.data, packetObject.user_data, packetObject.member_data);
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().Gold = packetObject.gold;
            GUIMgr.Instance.PopGUIEntity();
            if (<>f__am$cache2F == null)
            {
                <>f__am$cache2F = delegate (GUIEntity obj) {
                    GuildPanel panel = obj.Achieve<GuildPanel>();
                    if (panel != null)
                    {
                        panel.UpdateData();
                    }
                };
            }
            GUIMgr.Instance.PushGUIEntity("GuildPanel", <>f__am$cache2F);
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDISMISS, typeof(S2C_GuildDismiss))]
    public void ReceiveGuildDimiss(Packet pak)
    {
        S2C_GuildDismiss packetObject = (S2C_GuildDismiss) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().ClearGuildData();
            GUIMgr.Instance.ExitModelGUI("GuildMgrPanel");
            GUIMgr.Instance.PopGUIEntity();
            GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (null != gUIEntity)
            {
                gUIEntity.ClosePanel();
            }
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65288));
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDATAREQ, typeof(S2C_GuildDataReq))]
    public void ReceiveGuildeData(Packet pak)
    {
        S2C_GuildDataReq packetObject = (S2C_GuildDataReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.6f;
            ActorData.getInstance().SetGuildData(packetObject.data, packetObject.user_data, packetObject.member_data);
            GuildPanel gUIEntity = (GuildPanel) GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildMember();
            }
            if (ActorData.getInstance().bOpenUI)
            {
                MainUI nui = (MainUI) GUIMgr.Instance.GetGUIEntity("MainUI");
                if (nui != null)
                {
                    nui.UpdateGuild();
                }
                if (ActorData.getInstance().isLeavePaoku)
                {
                    GUIMgr.Instance.PushGUIEntity("PaokuPanel", null);
                    if (this.FSMAction != null)
                    {
                        this.FSMAction();
                    }
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDEXPELMEMBER, typeof(S2C_GuildExpelMember))]
    public void ReceiveGuildExpelMember(Packet pak)
    {
        S2C_GuildExpelMember packetObject = (S2C_GuildExpelMember) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mGuildData = packetObject.data;
            ActorData.getInstance().SetGuildMemberData(packetObject.member_data);
            GUIMgr.Instance.ExitModelGUI("MessageBox");
            GUIMgr.Instance.ExitModelGUI("GuildMemberPanel");
            GUIMgr.Instance.ExitModelGUI("GuildMemberCtrlDlag");
            GuildHallPanel gUIEntity = (GuildHallPanel) GUIMgr.Instance.GetGUIEntity("GuildHallPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildMember();
            }
            GuildPanel panel2 = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (panel2 != null)
            {
                panel2.GetNewData();
                panel2.UpdateGuildMember();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETJOINQQGROUPKEY, typeof(S2C_GetJoinQQGroupKey))]
    public void ReceiveGuildJoinQQGroup(Packet pak)
    {
        S2C_GetJoinQQGroupKey packetObject = (S2C_GetJoinQQGroupKey) pak.PacketObject;
        Debug.Log("ReceiveGuildJoinQQGroup: " + packetObject.result);
        if (packetObject.result == OpResult.OpResult_GetJoinQQGroupKey_s)
        {
            if (<>f__am$cache33 == null)
            {
                <>f__am$cache33 = () => Instance.RequestGuildJoinQQGroup();
            }
            ScheduleMgr.Schedule(1f, <>f__am$cache33);
            Debug.Log("RequestGuildJoinQQGroup Again!");
        }
        else if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            PlatformInterface.mInstance.PlatfromJoinQQGroup(packetObject.join_key);
        }
        else
        {
            Debug.Log(" Join QQ Group Failded!----------- ");
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDQUIT, typeof(S2C_GuildQuit))]
    public void ReceiveGuildQuit(Packet pak)
    {
        S2C_GuildQuit packetObject = (S2C_GuildQuit) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().ClearGuildData();
            GUIMgr.Instance.PopGUIEntity();
            GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.ClosePanel();
            }
            XSingleton<GameGuildMgr>.Reset();
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65287));
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDREFRESHGOODS, typeof(S2C_GuildRefreshGoods))]
    public void ReceiveGuildRefresh(Packet pak)
    {
        S2C_GuildRefreshGoods packetObject = (S2C_GuildRefreshGoods) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            GuildShopPanel gUIEntity = (GuildShopPanel) GUIMgr.Instance.GetGUIEntity("GuildShopPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateOwnContribution();
                gUIEntity.UpdateShopItem();
                gUIEntity.UpdateFreeRefreshCount();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDSEARCH, typeof(S2C_GuildSearch))]
    public void ReceiveGuildSearch(Packet pak)
    {
        S2C_GuildSearch packetObject = (S2C_GuildSearch) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.data.id > 0L)
            {
                GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("GuildListPanel");
                if (gUIEntity != null)
                {
                    GuildListPanel panel = gUIEntity.Achieve<GuildListPanel>();
                    if (panel != null)
                    {
                        panel.UpdateSearchGuildList(packetObject.data);
                    }
                }
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa65289));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDSELECTBUFF, typeof(S2C_GuildSelectBuff))]
    public void ReceiveGuildSelectBuf(Packet pak)
    {
        S2C_GuildSelectBuff packetObject = (S2C_GuildSelectBuff) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            GUIMgr.Instance.ExitModelGUI("GuildSelTechPanel");
            GuildTechPanel gUIEntity = (GuildTechPanel) GUIMgr.Instance.GetGUIEntity("GuildTechPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateTechItem();
                gUIEntity.UpdateContribution();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDSETNOITCE, typeof(S2C_GuildSetNoitce))]
    public void ReceiveGuildSetNotice(Packet pak)
    {
        S2C_GuildSetNoitce packetObject = (S2C_GuildSetNoitce) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mGuildData = packetObject.data;
            GUIMgr.Instance.ExitModelGUI("GuildModNoticePanel");
            GUIMgr.Instance.ExitModelGUI("GuildSettingPanel");
            GuildPanel gUIEntity = (GuildPanel) GUIMgr.Instance.GetGUIEntity("GuildPanel");
            if (gUIEntity != null)
            {
                gUIEntity.GetNewData();
                gUIEntity.UpdateNotice();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDTRANSFER, typeof(S2C_GuildTransfer))]
    public void ReceiveGuildTransfer(Packet pak)
    {
        S2C_GuildTransfer packetObject = (S2C_GuildTransfer) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().SetGuildData(packetObject.data, packetObject.user_data, packetObject.member_data);
            GUIMgr.Instance.ExitModelGUI("MessageBox");
            GUIMgr.Instance.ExitModelGUI("GuildMemberPanel");
            GuildHallPanel gUIEntity = (GuildHallPanel) GUIMgr.Instance.GetGUIEntity("GuildHallPanel");
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildMember();
            }
            GuildPanel panel2 = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (panel2 != null)
            {
                panel2.UpdateGuildMember();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDUNBINDQQGROUP, typeof(S2C_GuildUnBindQQGroup))]
    public void ReceiveGuildUnBindQQGroup(Packet pak)
    {
        S2C_GuildUnBindQQGroup packetObject = (S2C_GuildUnBindQQGroup) pak.PacketObject;
        Debug.Log("ReceiveGuildUnBindQQGroup: " + packetObject.result);
        if (packetObject.result == OpResult.OpResult_GuildUnBindQQGroup_s)
        {
            if (<>f__am$cache32 == null)
            {
                <>f__am$cache32 = () => Instance.RequestGuildUnBindQQGroup();
            }
            ScheduleMgr.Schedule(1f, <>f__am$cache32);
            Debug.Log("RequestGuildUnBindQQGroup Again!");
        }
        else if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateGuildUnBindQQInfo();
            }
        }
        else
        {
            Debug.Log(" Query QQ UnBindInfo Failded!----------- ");
        }
    }

    [PacketHandler(OpcodeType.S2C_ITEMMACHINING, typeof(S2C_ItemMachining))]
    public void ReceiveItemMachining(Packet pak)
    {
        S2C_ItemMachining packetObject = (S2C_ItemMachining) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            SoundManager.mInstance.PlaySFX("sound_newdog");
            if (packetObject.newCard.Count > 0)
            {
                ActorData.getInstance().CardMachining(packetObject.newCard);
                HeroPanel panel = GUIMgr.Instance.GetGUIEntity<HeroPanel>();
                if ((panel != null) && panel.gameObject.activeSelf)
                {
                    panel.UpdateData();
                    panel.ResetTogglePage();
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) packetObject.newCard[0].newCard[0].cardInfo.entry);
                    if (_config != null)
                    {
                        CardAnimPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<CardAnimPanel>();
                        if (activityGUIEntity != null)
                        {
                            activityGUIEntity.UpdateData(_config.entry, packetObject.newCard[0].newCard[0].cardInfo.starLv, _config.name);
                        }
                        List<long> cardIdList = new List<long> {
                            packetObject.newCard[0].newCard[0].card_id
                        };
                        this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
                    }
                }
            }
            if (packetObject.changeItems.Count > 0)
            {
                GUIMgr.Instance.ExitModelGUI("FragmentCombinePanel");
                ActorData.getInstance().UpdateItemList(packetObject.changeItems);
                FragmentBagPanel panel3 = GUIMgr.Instance.GetGUIEntity<FragmentBagPanel>();
                if ((panel3 != null) && panel3.gameObject.activeSelf)
                {
                    panel3.UpdateData(packetObject.changeItems);
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x34e));
                }
                BreakEquipPanel panel4 = GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>();
                if ((panel4 != null) && panel4.gameObject.activeSelf)
                {
                    panel4.CombineSucess();
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x34e));
                }
                BagPanel panel5 = GUIMgr.Instance.GetGUIEntity<BagPanel>();
                if ((panel5 != null) && panel5.gameObject.activeSelf)
                {
                    panel5.UpdateData(false);
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x34e));
                }
            }
            TitleBar gUIEntity = GUIMgr.Instance.GetGUIEntity<TitleBar>();
            if (gUIEntity != null)
            {
                gUIEntity.CheckTips();
            }
            if (<>f__am$cache2A == null)
            {
                <>f__am$cache2A = () => GUIMgr.Instance.UnLock();
            }
            ScheduleMgr.Schedule(0.2f, <>f__am$cache2A);
        }
    }

    [PacketHandler(OpcodeType.S2C_LEAGUEAPPLY, typeof(S2C_LeagueApply))]
    public void ReceiveLeagueApply(Packet pak)
    {
        S2C_LeagueApply packetObject = (S2C_LeagueApply) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().JoinLeagueInfo.leagueEntry = packetObject.leagueEntry;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("WorldCupPanel");
            if (gUIEntity != null)
            {
                (gUIEntity as WorldCupPanel).LeagueApplySuccess();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LEAGUEFIGHT, typeof(S2C_LeagueFight))]
    public void ReceiveLeagueFight(Packet pak)
    {
        <ReceiveLeagueFight>c__AnonStorey106 storey = new <ReceiveLeagueFight>c__AnonStorey106 {
            res = (S2C_LeagueFight) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__97), null);
            ActorData.getInstance().Stone = storey.res.stone;
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLEAGUEHISTORY, typeof(S2C_GetLeagueHistory))]
    public void ReceiveLeagueHistory(Packet pak)
    {
        <ReceiveLeagueHistory>c__AnonStorey107 storey = new <ReceiveLeagueHistory>c__AnonStorey107 {
            res = (S2C_GetLeagueHistory) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("LeagueHistoryPanel", new Action<GUIEntity>(storey.<>m__98), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_CHECKLIVENESSREWARD, typeof(S2C_CheckLivenessReward))]
    public void ReceiveLivenessHasReward(Packet pak)
    {
        S2C_CheckLivenessReward packetObject = (S2C_CheckLivenessReward) pak.PacketObject;
        ActorData.getInstance().LivenessCanPick = packetObject.isCanPick;
        LittleHelperPanel gUIEntity = (LittleHelperPanel) GUIMgr.Instance.GetGUIEntity("LittleHelperPanel");
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if ((null != gUIEntity) && !gUIEntity.Hidden)
            {
                gUIEntity.SetRewardEnable(packetObject.isCanPick);
            }
        }
        else if ((null != gUIEntity) && !gUIEntity.Hidden)
        {
            gUIEntity.SetRewardEnable(false);
        }
    }

    [PacketHandler(OpcodeType.S2C_GETLIVENESSLIST, typeof(S2C_GetLivenessList))]
    public void ReceiveLivenessList(Packet pak)
    {
        S2C_GetLivenessList packetObject = (S2C_GetLivenessList) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LivenessTaskList = packetObject.taskList;
            ActorData.getInstance().Liveness = packetObject.liveness;
            LittleHelperPanel gUIEntity = (LittleHelperPanel) GUIMgr.Instance.GetGUIEntity("LittleHelperPanel");
            if ((null != gUIEntity) && !gUIEntity.Hidden)
            {
                gUIEntity.Refresh(packetObject.liveness, packetObject.lastRewardEntry);
            }
            this.CheckLivenessReward();
        }
    }

    [PacketHandler(OpcodeType.S2C_PICKLIVENESSREWARD, typeof(S2C_PickLivenessReward))]
    public void ReceiveLivenessReward(Packet pak)
    {
        S2C_PickLivenessReward packetObject = (S2C_PickLivenessReward) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            Debug.Log("Get Liveness Reward Succeed");
            GUIMgr.Instance.UnLock();
            RewardPanel.ShowLivenessReward(packetObject);
            Instance.RequestLivenessList();
        }
    }

    [PacketHandler(OpcodeType.S2C_LOGIN, typeof(S2C_Login))]
    public void ReceiveLogin(Packet pak)
    {
        S2C_Login packetObject = (S2C_Login) pak.PacketObject;
        Debug.Log("Receive--------Login:" + packetObject.result.ToString());
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TimeMgr.Instance.InitServerTime((int) packetObject.Info.time, packetObject.Info.timeZone);
            ActorData.getInstance().Init(packetObject.Info.sessionInfo);
            GameStateMgr.Instance.ChangeState("LOGIN_LOADCHAR_EVENT");
            ActorData.getInstance().RequestAllInfo();
            GameDefine.getInstance().OnGameLoginOK();
            string userName = ActorData.getInstance().SessionInfo.userid.ToString();
            SettingMgr.mInstance.InitWithUserName(userName);
        }
        else if (packetObject.result == OpResult.OpResult_Login_S)
        {
            GameDefine.getInstance().OnGameLoginOK();
            this.ReSendLastPakLogic(1f);
        }
        else if (packetObject.result == OpResult.OpResult_Login_NoChar)
        {
            GameDefine.getInstance().OnGameLoginOK();
            TimeMgr.Instance.InitServerTime((int) packetObject.Info.time, packetObject.Info.timeZone);
            SelectCampPanel.ShowSelectCamp();
        }
        else if ((packetObject.result == OpResult.OpResult_Login_Account_No_Activate) || (packetObject.result == OpResult.OpResult_Login_Mac_No_Activate))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x34b));
            LoginPanel.RestLoginPanel();
        }
        else if (packetObject.result == OpResult.OpResult_Register_Limit)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetErrorCode(packetObject.result));
            GameDefine.getInstance().OnGameLoginFailed();
            LoginPanel.RestLoginPanel();
        }
        else
        {
            WaitPanelHelper.HideAll();
        }
    }

    [PacketHandler(OpcodeType.S2C_SETLOLARENAMEMBER, typeof(S2C_SetLoLArenaMember))]
    public void ReceiveLolArenaMember(Packet pak)
    {
        S2C_SetLoLArenaMember packetObject = (S2C_SetLoLArenaMember) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().ChallengeArenaFormation = new BattleFormation();
            ActorData.getInstance().ChallengeArenaFormation.captain_id = packetObject.formation.captain_id;
            ActorData.getInstance().ChallengeArenaFormation.card_id = packetObject.formation.card_id;
            ChallengeArenaPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChallengeArenaPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.RefreshFormation();
                this.RequestChallengeArenaInfo();
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity<ChallengeArenaPanel>(null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_MODIFYNICKNAME, typeof(S2C_ModifyNickName))]
    public void ReceiveModifyNickName(Packet pak)
    {
        S2C_ModifyNickName packetObject = (S2C_ModifyNickName) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.name = packetObject.name;
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().UserInfo.modifyNickNameCd = packetObject.cdTime;
            GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
            GUIMgr.Instance.ExitModelGUI("SetNicknameDlag");
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("PlayerInfoPanel");
            if (gUIEntity != null)
            {
                PlayerInfoPanel panel = (PlayerInfoPanel) gUIEntity;
                if (panel != null)
                {
                    panel.UpdateNickName();
                }
            }
            GUIEntity entity2 = GUIMgr.Instance.GetGUIEntity("MainUI");
            if (entity2 != null)
            {
                MainUI nui = (MainUI) entity2;
                if (nui != null)
                {
                    nui.UpdateName();
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_RECORDNEWBIE, typeof(S2C_RecordNewbie))]
    public void ReceiveNewbieStep(Packet pak)
    {
        S2C_RecordNewbie packetObject = (S2C_RecordNewbie) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            Debug.Log("Receive Record Newbie Succeed");
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWDUNGEONSCOMBATREQ, typeof(S2C_NewDungeonsCombatReq))]
    public void ReceiveNewDungeonsCombat(Packet pak)
    {
        <ReceiveNewDungeonsCombat>c__AnonStorey11D storeyd = new <ReceiveNewDungeonsCombat>c__AnonStorey11D {
            <>f__this = this,
            res = (S2C_NewDungeonsCombatReq) pak.PacketObject
        };
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(storeyd.res.result))
        {
            dungeons_activity_config _config = ConfigMgr.getInstance().getByEntry<dungeons_activity_config>(storeyd.res.dupData.dupEntry);
            if (_config == null)
            {
                Debug.LogWarning("dungeons_activity_config Error entry is " + storeyd.res.dupData.dupEntry);
            }
            else
            {
                ActorData.getInstance().PhyForce = storeyd.res.phyForce;
                BattleState.GetInstance().DoNormalBattle(storeyd.res.combat_data, storeyd.res.reward_view, BattleNormalGameType.Dungeons, true, _config.scene, _config.start_pos, _config.visual_type, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storeyd.<>m__B5));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWDUNGEONSCOMBATENDREQ, typeof(S2C_NewDungeonsCombatEndReq))]
    public void ReceiveNewDungeonsCombatEnd(Packet pak)
    {
        <ReceiveNewDungeonsCombatEnd>c__AnonStorey11E storeye = new <ReceiveNewDungeonsCombatEnd>c__AnonStorey11E {
            res = (S2C_NewDungeonsCombatEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeye.res.result))
        {
            ActorData.getInstance().UserInfo.dungeons_1_times = storeye.res.dungeons_1_times;
            ActorData.getInstance().UserInfo.dungeons_2_times = storeye.res.dungeons_2_times;
            ActorData.getInstance().UpdateBattleRewardData(storeye.res.reward);
            GUIMgr.Instance.ClearAll();
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storeye.<>m__B6), base.gameObject);
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_ONEKEYEQUIPLVUP, typeof(S2C_OneKeyEquipLvUp))]
    public void ReceiveOneKeyEquipLevUp(Packet pak)
    {
        S2C_OneKeyEquipLvUp packetObject = (S2C_OneKeyEquipLvUp) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = ActorData.getInstance().Gold - packetObject.gold;
            ActorData.getInstance().Gold = packetObject.gold;
            SoundManager.mInstance.PlaySFX("sound_ui_z_6");
            ActorData.getInstance().UpdateCard(packetObject.CardInfo);
            EquipLevUpPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<EquipLevUpPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateOneKeyLevUp(packetObject.CardInfo, num);
            }
            HeroInfoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HeroInfoPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.LevelUpEquipSucess(packetObject.CardInfo);
            }
            BreakEquipPanel panel3 = GUIMgr.Instance.GetActivityGUIEntity<BreakEquipPanel>();
            if (panel3 != null)
            {
                panel3.UpdateOneKeyLevUp(packetObject.CardInfo, num);
            }
        }
        else
        {
            EquipLevUpPanel panel4 = GUIMgr.Instance.GetActivityGUIEntity<EquipLevUpPanel>();
            if (panel4 != null)
            {
                panel4.EnablePanelClick(false);
                panel4.CanClickBtn = true;
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SHARE, typeof(S2C_Share))]
    public void ReceiveOnShareOK(Packet pak)
    {
    }

    [PacketHandler(OpcodeType.S2C_OPENBOXOPEN, typeof(S2C_OpenBoxOpen))]
    public void ReceiveOpenBoxOpen(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_OpenBoxOpen packetObject = (S2C_OpenBoxOpen) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UpdateItem(packetObject.gold_key);
            ActorData.getInstance().UpdateItem(packetObject.silver_key);
            ActorData.getInstance().UpdateItem(packetObject.copper_key);
            ActorData.getInstance().UpdateBattleRewardData(packetObject.reward);
            ActorData.getInstance().Stone = packetObject.stone;
            ActorData.getInstance().Gold = packetObject.gold;
            User userInfo = ActorData.getInstance().UserInfo;
            userInfo.courage += (int) packetObject.reward.courage;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("BoxPanel");
            if (gUIEntity != null)
            {
                BoxPanel panel = gUIEntity.Achieve<BoxPanel>();
                if (panel != null)
                {
                    panel.OpenUpdata(packetObject);
                }
            }
        }
        else
        {
            GUIEntity entity2 = GUIMgr.Instance.GetGUIEntity("BoxPanel");
            if (entity2 != null)
            {
                BoxPanel panel2 = entity2.Achieve<BoxPanel>();
                if (panel2 != null)
                {
                    panel2.CanClick = true;
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OPENBOXREQ, typeof(S2C_OpenBoxReq))]
    public void ReceiveOpenBoxReq(Packet pak)
    {
        S2C_OpenBoxReq packetObject = (S2C_OpenBoxReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.goldKey = packetObject.data.gold_key;
            ActorData.getInstance().UserInfo.silverKey = packetObject.data.silver_key;
            ActorData.getInstance().UserInfo.copperKey = packetObject.data.copper_key;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("BoxPanel");
            if (gUIEntity != null)
            {
                BoxPanel panel = gUIEntity.Achieve<BoxPanel>();
                if (panel != null)
                {
                    panel.UpdateData(packetObject.data);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDBUFFEREVENTREQ, typeof(S2C_OutlandBufferEventReq))]
    public void ReceiveOutlandBufferEventReq(Packet pak)
    {
        S2C_OutlandBufferEventReq packetObject = (S2C_OutlandBufferEventReq) pak.PacketObject;
        Debug.Log(packetObject.data.Count);
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            Debug.Log("ReceiveOutlandDropEvent=" + packetObject.isgetkey);
            BattleState.GetInstance().CurGame.battleGameData.IsKey = packetObject.isgetkey;
            OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
            if (gUIEntity != null)
            {
                if (packetObject.isgetkey)
                {
                    gUIEntity.SetKey();
                }
                gUIEntity.HideBuffGird(false);
                gUIEntity.SetBuffIcon(packetObject.data);
            }
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(true, true);
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattlePlayerBuff(packetObject.data);
            foreach (OutlandBattleBackCardInfo info in packetObject.outland_battle_data.self_card_data)
            {
                Debug.Log(string.Concat(new object[] { "复活卡牌 id=", info.card_entry, "hp=", info.card_cur_hp, "energy=", info.card_cur_energy }));
                ActorData.getInstance().UpdateOutlandCardStatList(info);
            }
        }
        else
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDCOMBATENDREQ, typeof(S2C_OutlandCombatEndReq))]
    public void ReceiveOutlandCombatEndReq(Packet pak)
    {
        <ReceiveOutlandCombatEndReq>c__AnonStoreyF9 yf = new <ReceiveOutlandCombatEndReq>c__AnonStoreyF9 {
            res = (S2C_OutlandCombatEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yf.res.result))
        {
            if (yf.res.isdead)
            {
                this.RequestOutlandQuit(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry);
            }
            else
            {
                if (yf.res.gold > 0)
                {
                    ActorData.getInstance().Gold = yf.res.gold;
                }
                if (yf.res.stone > 0)
                {
                    ActorData.getInstance().Stone = yf.res.stone;
                }
                if (yf.res.courage > 0)
                {
                    ActorData.getInstance().Courage = yf.res.courage;
                }
                if (yf.res.phyForce > 0)
                {
                    ActorData.getInstance().PhyForce = yf.res.phyForce;
                }
                if (yf.res.eq > 0)
                {
                    ActorData.getInstance().Eq = yf.res.eq;
                }
                if (yf.res.outland_coin > 0)
                {
                    ActorData.getInstance().OutlandCoin = yf.res.outland_coin;
                }
                if (yf.res.reward.items.Count > 0)
                {
                    ActorData.getInstance().UpdateItemList(yf.res.reward.items);
                }
                if (yf.res.reward.cards.Count > 0)
                {
                    ActorData.getInstance().UpdateNewCard(yf.res.reward.cards);
                }
                Debug.Log("ReceiveOutlandCombatEndReq=" + yf.res.isgetkey);
                BattleState.GetInstance().CurGame.battleGameData.IsKey = yf.res.isgetkey;
                OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
                if (gUIEntity != null)
                {
                    gUIEntity.HideBuffGird(false);
                    gUIEntity.SetBuffIcon(yf.res.buffs);
                }
                BattleState.GetInstance().CurGame.battleGameData.OnMsgBattlePlayerBuff(yf.res.buffs);
                GUIMgr.Instance.PushGUIEntity("ResultOutlandPanel", new Action<GUIEntity>(yf.<>m__87));
                if (this.resulttBattleBack != null)
                {
                    foreach (OutlandBattleBackCardInfo info in this.resulttBattleBack.self_card_data)
                    {
                        ActorData.getInstance().UpdateOutlandCardStatList(info);
                    }
                }
            }
        }
        else if (yf.res.result == OpResult.OpResult_Repeated_Req)
        {
            BattleStaticEntry.ExitBattle();
            GameStateMgr.Instance.ChangeState("EXIT_OUTLAND_GRID_EVENT");
        }
        else
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgOutlandCameraEnable(true, false);
            OutlandBattlePanel panel2 = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
            if (panel2 != null)
            {
                panel2.CombatBackBuffDele();
            }
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDCOMBATREQ, typeof(S2C_OutlandCombatReq))]
    public void ReceiveOutlandCombatReq(Packet pak)
    {
        S2C_OutlandCombatReq packetObject = (S2C_OutlandCombatReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleGridGame curGame = BattleState.GetInstance().CurGame as BattleGridGame;
            Debug.Log("服务器返回战斗数据 自方英雄数量==" + packetObject.combat_data.attacker.actor.Count);
            if (<>f__am$cache27 == null)
            {
                <>f__am$cache27 = c => Debug.Log("英雄id==" + c.entry);
            }
            packetObject.combat_data.attacker.actor.ForEach(<>f__am$cache27);
            Debug.Log("服务器返回战斗数据 敌方数量==" + packetObject.combat_data.defenderList.Count);
            if (<>f__am$cache28 == null)
            {
                <>f__am$cache28 = delegate (CombatTeam c) {
                    if (<>f__am$cache41 == null)
                    {
                        <>f__am$cache41 = f => Debug.Log("英雄id==" + f.entry);
                    }
                    c.actor.ForEach(<>f__am$cache41);
                };
            }
            packetObject.combat_data.defenderList.ForEach(<>f__am$cache28);
            curGame.DoBattle(packetObject.point, packetObject.combat_data, packetObject.reward_view);
        }
        else
        {
            Debug.Log("服务器返回战斗数据异常 码==" + packetObject.result);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDCREATEMAPREQ, typeof(S2C_OutlandCreateMapReq))]
    public void ReceiveOutlandCreateMapReq(Packet pak)
    {
        <ReceiveOutlandCreateMapReq>c__AnonStoreyFE yfe = new <ReceiveOutlandCreateMapReq>c__AnonStoreyFE {
            res = (S2C_OutlandCreateMapReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yfe.res.result) && (GUIMgr.Instance.GetGUIEntity<OutlandThirdPanel>() != null))
        {
            GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(yfe.<>m__8D));
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDDATAENTER, typeof(S2C_OutlandDataEnter))]
    public void ReceiveOutlandDataEnter(Packet pak)
    {
        S2C_OutlandDataEnter packetObject = (S2C_OutlandDataEnter) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.exp = packetObject.exp;
            ActorData.getInstance().Level = packetObject.level;
            this.EnterGrid(packetObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDDROPEVENT, typeof(S2C_OutlandDropEvent))]
    public void ReceiveOutlandDropEvent(Packet pak)
    {
        <ReceiveOutlandDropEvent>c__AnonStoreyFA yfa = new <ReceiveOutlandDropEvent>c__AnonStoreyFA {
            res = (S2C_OutlandDropEvent) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yfa.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(yfa.<>m__88), null);
            OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
            if ((gUIEntity != null) && yfa.res.isgetkey)
            {
                gUIEntity.SetKey();
            }
            Debug.Log("ReceiveOutlandDropEvent=" + yfa.res.isgetkey);
            BattleState.GetInstance().CurGame.battleGameData.IsKey = yfa.res.isgetkey;
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(yfa.res.isPass, false);
        }
        else
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDENTERNEXTFLOORREQ, typeof(S2C_OutlandEnterNextFloorReq))]
    public void ReceiveOutlandEnterNextFloorReq(Packet pak)
    {
        S2C_OutlandEnterNextFloorReq packetObject = (S2C_OutlandEnterNextFloorReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleStaticEntry.DoDupBattleGrid(packetObject.data, true);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDGETFLOORBOXREWARD, typeof(S2C_OutlandGetFloorBoxReward))]
    public void ReceiveOutlandGetFloorBoxReward(Packet pak)
    {
        <ReceiveOutlandGetFloorBoxReward>c__AnonStoreyFB yfb = new <ReceiveOutlandGetFloorBoxReward>c__AnonStoreyFB {
            res = (S2C_OutlandGetFloorBoxReward) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yfb.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(yfb.<>m__89), null);
            OutlandBattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<OutlandBattlePanel>();
            if (null != gUIEntity)
            {
                gUIEntity.SetBoxIcon(2);
            }
            BattleState.GetInstance().CurGame.battleGameData.isClearBoxReward = true;
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDGETMONSTERINFO, typeof(S2C_OutlandGetMonsterInfo))]
    public void ReceiveOutlandGetMonsterInfo(Packet pak)
    {
        <ReceiveOutlandGetMonsterInfo>c__AnonStoreyF8 yf = new <ReceiveOutlandGetMonsterInfo>c__AnonStoreyF8 {
            res = (S2C_OutlandGetMonsterInfo) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yf.res.result))
        {
            GUIMgr.Instance.PushGUIEntity("TargetTeamPanel", new Action<GUIEntity>(yf.<>m__83));
        }
        else
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDGETREWARDREQ, typeof(S2C_OutlandGetRewardReq))]
    public void ReceiveOutlandGetRewardReq(Packet pak)
    {
        S2C_OutlandGetRewardReq packetObject = (S2C_OutlandGetRewardReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDPAGEMAPREQ, typeof(S2C_OutlandPageMapReq))]
    public void ReceiveOutlandPageMapReq(Packet pak)
    {
        S2C_OutlandPageMapReq packetObject = (S2C_OutlandPageMapReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().outlandFloorList = packetObject.page.floor_info;
            if (ActorData.getInstance().bOpenThirdPanel)
            {
                GUIMgr.Instance.PushGUIEntity("OutlandThirdPanel", delegate (GUIEntity obj) {
                    OutlandThirdPanel panel = (OutlandThirdPanel) obj;
                    if (this.LoadingAction != null)
                    {
                        this.LoadingAction();
                    }
                });
            }
            else
            {
                OutlandThirdPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<OutlandThirdPanel>();
                if (activityGUIEntity != null)
                {
                    activityGUIEntity.SetInfo(ActorData.getInstance().outlandFloorList);
                }
            }
        }
        else if (this.LoadingAction != null)
        {
            this.LoadingAction();
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDQUIT, typeof(S2C_OutlandQuit))]
    public void ReceiveOutlandQuit(Packet pak)
    {
        <ReceiveOutlandQuit>c__AnonStoreyFC yfc = new <ReceiveOutlandQuit>c__AnonStoreyFC {
            res = (S2C_OutlandQuit) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yfc.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("OutlandBattlePanel");
            BattleState.GetInstance().CurGame.battleGameData.OnMsgOutlandCameraEnable(true, true);
            ScheduleMgr.Schedule(1.5f, new System.Action(yfc.<>m__8A));
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDDATAREQ, typeof(S2C_OutlandDataReq))]
    public void ReceiveOutlandsData(Packet pak)
    {
        S2C_OutlandDataReq packetObject = (S2C_OutlandDataReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().outlandTitles = packetObject.title;
            CommonFunc.SetPreOutlandFight();
            if (ActorData.getInstance().bOpenOutlandTitleInfo)
            {
                if (ActorData.getInstance().isPreOutlandFight)
                {
                    <ReceiveOutlandsData>c__AnonStoreyF7 yf = new <ReceiveOutlandsData>c__AnonStoreyF7 {
                        <>f__this = this,
                        outlandMap = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(3)
                    };
                    if ((yf.outlandMap == null) || (yf.outlandMap == null))
                    {
                        if (this.LoadingAction != null)
                        {
                            this.LoadingAction();
                        }
                    }
                    else
                    {
                        GUIMgr.Instance.PushGUIEntity("OutlandSecondPanel", new Action<GUIEntity>(yf.<>m__81));
                    }
                }
                else
                {
                    GUIMgr.Instance.PushGUIEntity("OutlandFristPanel", delegate (GUIEntity obj) {
                        OutlandFristPanel panel = (OutlandFristPanel) obj;
                        if (((ActorData.getInstance().isOutlandGrid && (ActorData.getInstance().outlandType >= 0)) && ((ActorData.getInstance().outlandType <= 3) && !ActorData.getInstance().outlandAllHerosDeadList[ActorData.getInstance().outlandType])) && ((ActorData.getInstance().outlandTitles.Count >= 4) && ActorData.getInstance().outlandTitles[ActorData.getInstance().outlandType].is_underway))
                        {
                            ActorData.getInstance().isOutlandGrid = false;
                            ActorData.getInstance().outlandAllHerosDeadList[ActorData.getInstance().outlandType] = false;
                            if (<>f__am$cache40 == null)
                            {
                                <>f__am$cache40 = delegate (GUIEntity objs) {
                                    ((OutlandSecondPanel) objs).SetInfo(ActorData.getInstance().tempOutlandMapTypeConfig);
                                    ActorData.getInstance().bOpenThirdPanel = true;
                                    Instance.RequestOutlandPageMapReq(ActorData.getInstance().outlandPageEntry);
                                };
                            }
                            GUIMgr.Instance.PushGUIEntity("OutlandSecondPanel", <>f__am$cache40);
                        }
                        else if (this.LoadingAction != null)
                        {
                            this.LoadingAction();
                        }
                        if (this.EnterAction != null)
                        {
                            this.EnterAction();
                        }
                    });
                }
            }
            else if (ActorData.getInstance().isPreOutlandFight)
            {
                outland_map_type_config outlandMapType = ConfigMgr.getInstance().getByEntry<outland_map_type_config>(3);
                OutlandSecondPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<OutlandSecondPanel>();
                if (activityGUIEntity != null)
                {
                    activityGUIEntity.SetInfo(outlandMapType);
                }
            }
            else
            {
                OutlandFristPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<OutlandFristPanel>();
                if (panel2 != null)
                {
                    panel2.SetInfo(packetObject.title);
                }
            }
        }
        else if (this.LoadingAction != null)
        {
            this.LoadingAction();
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDSHOPBUY, typeof(S2C_OutlandShopBuy))]
    public void ReceiveOutlandShopBuy(Packet pak)
    {
        S2C_OutlandShopBuy packetObject = (S2C_OutlandShopBuy) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UpdateNewCard(packetObject.buyResult.cardList);
            ActorData.getInstance().UpdateItem(packetObject.buyResult.item);
            ActorData.getInstance().OutlandCoin = packetObject.buyResult.currency_info.outland_coin;
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateItemBySolt(packetObject.buyResult.slot);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDSHOPINFO, typeof(S2C_OutlandShopInfo))]
    public void ReceiveOutlandShopInfo(Packet pak)
    {
        S2C_OutlandShopInfo packetObject = (S2C_OutlandShopInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.OutLandCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDSHOPREFRESH, typeof(S2C_OutlandShopRefresh))]
    public void ReceiveOutlandShopRefresh(Packet pak)
    {
        S2C_OutlandShopRefresh packetObject = (S2C_OutlandShopRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().OutlandCoin = packetObject.currencyInfo.outland_coin;
            ActorData.getInstance().UserInfo.refreshOutlandCount = packetObject.refreshCount;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            ShopPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowItemData(ShopCoinType.OutLandCoin, packetObject.itemList);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OUTLANDSWEEPREQ, typeof(S2C_OutlandSweepReq))]
    public void ReceiveOutlandSweepReq(Packet pak)
    {
        int num;
        S2C_OutlandSweepReq packetObject = (S2C_OutlandSweepReq) pak.PacketObject;
        OpResult result = packetObject.result;
        if (result != OpResult.OpResult_Ok)
        {
            if (result != OpResult.OpResult_Not_Enough_VipLevel)
            {
                this.CheckResultCodeAndReLogin(packetObject.result);
                return;
            }
            ArrayList list = ConfigMgr.getInstance().getList<vip_config>();
            num = 0;
            IEnumerator enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    vip_config current = (vip_config) enumerator.Current;
                    if (current.outland_sweep_openlv)
                    {
                        num = current.entry + 1;
                        goto Label_009C;
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
        }
        else
        {
            this.ProcessSweepResult(packetObject);
            return;
        }
    Label_009C:
        TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x4e55), num));
    }

    [PacketHandler(OpcodeType.S2C_PARKOUREND, typeof(S2C_ParkourEnd))]
    public void ReceiveParkourEnd(Packet pak)
    {
        S2C_ParkourEnd packetObject = (S2C_ParkourEnd) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.gold > 0)
            {
                ActorData.getInstance().Gold = packetObject.gold;
            }
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            if (ActorData.getInstance().isPaokuWin)
            {
                GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("ResultPaokuPanel");
                if (gUIEntity != null)
                {
                    ResultPaokuPanel panel = (ResultPaokuPanel) gUIEntity;
                    if (panel != null)
                    {
                        panel.InitData(packetObject);
                    }
                }
            }
            else
            {
                ActorData.getInstance().isLeavePaoku = true;
                Time.timeScale = 1f;
                ParkourManager._instance.DestoryParkourAsset();
                GameStateMgr.Instance.ChangeState("EXIT_PARKOUR_EVENT");
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_PARKOURRESET, typeof(S2C_ParkourReset))]
    public void ReceiveParkourReset(Packet pak)
    {
        S2C_ParkourReset packetObject = (S2C_ParkourReset) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            ActorData.getInstance().UserInfo.stone = packetObject.stone;
            this.Replay();
        }
    }

    [PacketHandler(OpcodeType.S2C_PARKOURREVIVE, typeof(S2C_ParkourRevive))]
    public void ReceiveParkourRevive(Packet pak)
    {
        S2C_ParkourRevive packetObject = (S2C_ParkourRevive) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().mUserGuildMemberData = packetObject.user_data;
            ActorData.getInstance().UserInfo.stone = packetObject.stone;
            GUIMgr.Instance.ClearAll();
            ParkourManager._instance.Resurrection();
            GUIMgr.Instance.OpenUniqueGUIEntity("PaokuInPanel", new Action<GUIEntity>(this.OnPkFinish));
        }
    }

    [PacketHandler(OpcodeType.S2C_PARKOURSTART, typeof(S2C_ParkourStart))]
    public void ReceiveParkourStart(Packet pak)
    {
        S2C_ParkourStart packetObject = (S2C_ParkourStart) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().paokuMapEntry = packetObject.entry;
            ActorData.getInstance().paokuPropIndex = packetObject.map_index;
            this.EnterParkourScene("ENTER_PARKOUR");
        }
        PaokuPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<PaokuPanel>();
        if (null != gUIEntity)
        {
            gUIEntity.mIsSendParkourStart = false;
        }
    }

    [PacketHandler(OpcodeType.S2C_PICKGIFT, typeof(S2C_PickGift))]
    public void ReceivePickGift(Packet pak)
    {
        <ReceivePickGift>c__AnonStorey144 storey = new <ReceivePickGift>c__AnonStorey144 {
            res = (S2C_PickGift) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.ExitModelGUI("ExchangeCodePanel");
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__F9), null);
        }
        else if (storey.res.result == OpResult.OpResult_Gift_S)
        {
            base.Invoke("ReSendLastPak", 1f);
        }
    }

    [PacketHandler(OpcodeType.S2C_PICKLEAGUEREWARD, typeof(S2C_PickLeagueReward))]
    public void ReceivePickLeagueReward(Packet pak)
    {
        <ReceivePickLeagueReward>c__AnonStorey104 storey = new <ReceivePickLeagueReward>c__AnonStorey104 {
            res = (S2C_PickLeagueReward) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().UpdateBattleRewardData(storey.res.reward);
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__93), null);
            ActorData.getInstance().UpdateLeagueReward(storey.res.type);
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("WorldCupPanel");
            if (gUIEntity != null)
            {
                (gUIEntity as WorldCupPanel).PickLeagueRewardSuccess();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_PICKMAILAFFIX, typeof(S2C_PickMailAffix))]
    public void ReceivePickMailAffix(Packet pak)
    {
        <ReceivePickMailAffix>c__AnonStorey133 storey = new <ReceivePickMailAffix>c__AnonStorey133 {
            res = (S2C_PickMailAffix) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__D4), null);
            GUIMgr.Instance.CloseUniqueGUIEntityImmediate("MailPanel");
            ActorData.getInstance().DelteMail(storey.res.sysMailId);
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("MailListPanel");
            if (gUIEntity != null)
            {
                ((MailListPanel) gUIEntity).UpdateMailList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_PREPAREFRIENDCOMBAT, typeof(S2C_PrepareFriendCombat))]
    public void ReceivePrepareFriendCombat(Packet pak)
    {
        S2C_PrepareFriendCombat packetObject = (S2C_PrepareFriendCombat) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleState.GetInstance().DoNormalBattle(packetObject.crewData, null, BattleNormalGameType.FriendPK, false, "cj_zc01", 1, 3, null, null, delegate (bool isWin, BattleNormalGameType _type, BattleNormalGameResult result) {
                if (ActorData.getInstance().CurrFriendPkInfo != null)
                {
                    this.RequestFriendCombat(ActorData.getInstance().CurrFriendPkInfo.targetInfo.id, isWin);
                }
            });
        }
    }

    [PacketHandler(OpcodeType.S2C_PREPARELEAGUEFIGHT, typeof(S2C_PrepareLeagueFight))]
    public void ReceivePrepareLeagueFight(Packet pak)
    {
        S2C_PrepareLeagueFight packetObject = (S2C_PrepareLeagueFight) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleState.GetInstance().DoNormalBattle(packetObject.crewData, null, BattleNormalGameType.WorldCupPk, false, "cj_zc01", 1, 3, null, null, (isWin, _type, result) => this.RequestLeagueFight(ActorData.getInstance().JoinLeagueInfo.leagueEntry, ActorData.getInstance().JoinLeagueInfo.groupId, ActorData.getInstance().CurrWorldCupPkTargetId, ActorData.getInstance().IsCostStone, isWin));
        }
        else
        {
            this.RequestGetLeagueOpponentList(ActorData.getInstance().CurrWorldCupPkInfo.leagueEntry, ActorData.getInstance().CurrWorldCupPkInfo.groupId);
            if (GUIMgr.Instance.GetGUIEntity<SelectHeroPanel>() != null)
            {
                GUIMgr.Instance.PopGUIEntity();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_DUPLICATEENDREQ, typeof(S2C_DuplicateEndReq))]
    public void ReceiveQuitDup(Packet pak)
    {
        <ReceiveQuitDup>c__AnonStoreyFF yff = new <ReceiveQuitDup>c__AnonStoreyFF {
            res = (S2C_DuplicateEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(yff.res.result))
        {
            if (yff.res.is_new_mail)
            {
                Instance.RequestGetMailList();
            }
            if ((yff.res.qqCardCoolDown != null) && (yff.res.qqCardCoolDown.Count > 0))
            {
                XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(yff.<>m__8E));
            }
            BattleStaticEntry.ExitDupBattle(yff.res);
        }
        else
        {
            Debug.LogWarning("ReceiveQuitDup Error  " + yff.res.result.ToString());
            BattleStaticEntry.ExitBattle();
        }
    }

    [PacketHandler(OpcodeType.S2C_REDPACKAGERECORD, typeof(S2C_RedPackageRecord))]
    public void ReceiveRedPackageRecord(Packet pak)
    {
        S2C_RedPackageRecord packetObject = (S2C_RedPackageRecord) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            HongBaoPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<HongBaoPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REFRESHFLAMEBATTLETARGET, typeof(S2C_RefreshFlameBattleTarget))]
    public void ReceiveRefreshFlameBattleTarget(Packet pak)
    {
        S2C_RefreshFlameBattleTarget packetObject = (S2C_RefreshFlameBattleTarget) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().FlamebattleCoin = packetObject.flamebattleCoin;
            TargetTeamPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TargetTeamPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetYuanZhengInfo(packetObject.target_data);
            }
            YuanZhengPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (panel2 != null)
            {
                panel2.UpdateTargetTeamInfo(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_QQFRIENDSINGAME_FRESH, typeof(S2C_QQFriendsInGame_Fresh))]
    public void ReceiveRefreshQQFriendInGame(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_QQFriendsInGame_Fresh packetObject = pak.PacketObject as S2C_QQFriendsInGame_Fresh;
        if (this.CheckResultCodeAndReLogin(packetObject.result) && (XSingleton<SocialFriend>.Singleton.State == SocialFriend.SocialState.Ready))
        {
            foreach (QQFriendUser_Fresh fresh2 in packetObject.userList)
            {
                XSingleton<SocialFriend>.Singleton.UpdateQQFriendGame(fresh2);
            }
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateQQFriendList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REFUSEFRIEND, typeof(S2C_RefuseFriend))]
    public void ReceiveRefuseFriend(Packet pak)
    {
        S2C_RefuseFriend packetObject = (S2C_RefuseFriend) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.refuseAll)
            {
                ActorData.getInstance().FriendReqList.Clear();
            }
            else
            {
                ActorData.getInstance().DelItemFriendReqList(packetObject.targetId);
            }
            if (GUIMgr.Instance.GetActivityGUIEntity<FriendInfoPanel>() != null)
            {
                GUIMgr.Instance.ExitModelGUI("FriendInfoPanel");
            }
            AddFriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AddFriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.RefuseFriendSuccessd();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REGISTER, typeof(S2C_Register))]
    public void ReceiveRegister(Packet pak)
    {
        S2C_Register packetObject = (S2C_Register) pak.PacketObject;
        Debug.Log("*****ReceiveRegister*****" + packetObject.result.ToString());
        if (packetObject.result != OpResult.OpResult_ReLoad)
        {
            if (packetObject.result == OpResult.OpResult_Register_Success)
            {
                ActorData.getInstance().Init(packetObject.Info.sessionInfo);
                TimeMgr.Instance.InitServerTime((int) packetObject.Info.time, packetObject.Info.timeZone);
                string userName = ActorData.getInstance().SessionInfo.userid.ToString();
                SettingMgr.mInstance.InitWithUserName(userName);
                if (<>f__am$cache35 == null)
                {
                    <>f__am$cache35 = delegate {
                        LoadingPerfab.BeginTransition();
                        GameStateMgr.Instance.ChangeState("LOGIN_LOADCHAR_EVENT");
                        ActorData.getInstance().RequestAllInfo();
                    };
                }
                GuideSimulate_Battle.StartBattle(<>f__am$cache35);
            }
            else
            {
                Debug.LogWarning("ErrorCode:" + packetObject.result.ToString());
                TipsDiag.SetText(ConfigMgr.getInstance().GetErrorCode(packetObject.result));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REGISTRATION, typeof(S2C_Registration))]
    public void ReceiveRegistration(Packet pak)
    {
        <ReceiveRegistration>c__AnonStorey145 storey = new <ReceiveRegistration>c__AnonStorey145 {
            res = (S2C_Registration) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().RegistrationReward = 1;
            ActorData.getInstance().RegistrationCount = storey.res.registrationDays;
            SignInPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<SignInPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.HideBtn(false);
                gUIEntity.UpdateCheckState();
            }
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__FA), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_REMOVECARDGEM, typeof(S2C_RemoveCardGem))]
    public void ReceiveRemoveCardGem(Packet pak)
    {
        S2C_RemoveCardGem packetObject = (S2C_RemoveCardGem) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GemWearPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GemWearPanel>();
            if (gUIEntity != null)
            {
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(packetObject.changeitem.entry);
                if (_config != null)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x4e4c) + _config.name + "\n[ff0000]" + gUIEntity.GetAddDeltaByID(packetObject.changeitem.entry, 0));
                    List<Item> list = new List<Item> {
                        packetObject.changeitem
                    };
                    ActorData.getInstance().UpdateItemList(list);
                    List<long> cardIdList = new List<long> {
                        packetObject.CardInfo.card_id
                    };
                    this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
                    ActorData.getInstance().UpdateCard(packetObject.CardInfo);
                    gUIEntity.InitCardInfo(packetObject.CardInfo);
                    gUIEntity.UpdateData(list);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REQMONTHCARDDATA, typeof(S2C_ReqMonthCardData))]
    public void ReceiveReqMonthCardData(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ReqMonthCardData packetObject = (S2C_ReqMonthCardData) pak.PacketObject;
        if (GameDefine.getInstance().isDebugLog)
        {
            Debug.Log(string.Concat(new object[] { "PAY: ReceiveReqMonthCardData ", packetObject.result, " month_card_1_first: ", packetObject.data.month_card_1_first, " month_card_1: ", packetObject.data.month_card_1, " month_card_2: ", packetObject.data.month_card_2 }));
        }
        ActorData.getInstance().VipData_MonthCard = packetObject.data;
        ActorData.getInstance().bMonthVipCardChange = true;
        VipCardPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<VipCardPanel>();
        if (null != gUIEntity)
        {
            gUIEntity.Refresh();
        }
        ActorData.getInstance().OnLoadDataFinish();
    }

    [PacketHandler(OpcodeType.S2C_PICKDUPLICATEREWARD, typeof(S2C_PickDuplicateReward))]
    public void ReceiveRequestDupReward(Packet pak)
    {
        <ReceiveRequestDupReward>c__AnonStorey11F storeyf = new <ReceiveRequestDupReward>c__AnonStorey11F {
            res = (S2C_PickDuplicateReward) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyf.res.result))
        {
            DuplicateRewardInfo item = ActorData.getInstance().DupRewardInfo.Find(new Predicate<DuplicateRewardInfo>(storeyf.<>m__B7));
            if (item == null)
            {
                item = new DuplicateRewardInfo {
                    duplicateEntry = storeyf.res.duplicateEntry
                };
                ActorData.getInstance().DupRewardInfo.Add(item);
            }
            if (storeyf.res.duplicateType == 0)
            {
                item.normalStar.Add(storeyf.res.pickStar);
            }
            else if (storeyf.res.duplicateType == 1)
            {
                item.eliteStar.Add(storeyf.res.pickStar);
            }
            DupMap.RefreshDupRewardUI();
            RewardPanel.ShowPickDuplicateReward(storeyf.res);
        }
    }

    [PacketHandler(OpcodeType.S2C_RESETFLAMEBATTLE, typeof(S2C_ResetFlameBattle))]
    public void ReceiveResetFlameBattle(Packet pak)
    {
        S2C_ResetFlameBattle packetObject = (S2C_ResetFlameBattle) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().YuanZhenMapVal = 0f;
            ActorData.getInstance().YuanZhengCardStatList = packetObject.flame_battle_info.self_data_list;
            ActorData.getInstance().FlamebattleCoin = packetObject.flamebattleCoin;
            ActorData.getInstance().mFlameBattleInfo = packetObject.flame_battle_info;
            if (packetObject.ticket != null)
            {
                ActorData.getInstance().UpdateTicketItem(packetObject.ticket);
            }
            YuanZhengPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.InitFlameBattleInfo(packetObject.flame_battle_info);
                activityGUIEntity.ScrollMap();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETNEWREWARDFLAG, typeof(S2C_GetNewRewardFlag))]
    public void ReceiveRewardFlag(Packet pak)
    {
        S2C_GetNewRewardFlag packetObject = (S2C_GetNewRewardFlag) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GameDataMgr.Instance.ActorRewardStage = packetObject.flag;
        }
    }

    [PacketHandler(OpcodeType.S2C_RUNESTONEPURCHASE, typeof(S2C_RunestonePurchase))]
    public void ReceiveRuneStonePurchase(Packet pak)
    {
        ActorData.getInstance().IsSendPak = false;
        S2C_RunestonePurchase packetObject = (S2C_RunestonePurchase) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            if (packetObject.type == RunestonePurchaseType.E_RPT_PhyForce)
            {
                if (packetObject.phyForce >= 0)
                {
                    ActorData.getInstance().UserInfo.phyforce_buy_times = packetObject.phyforce_buy_times;
                    ActorData.getInstance().UserInfo.max_phy_buy_times = packetObject.max_phy_buy_times;
                    ActorData.getInstance().UserInfo.buy_phy_cost = packetObject.buy_phy_cost;
                    ActorData.getInstance().PhyForce = packetObject.phyForce;
                }
                else
                {
                    Debug.Log("Error phyForce:" + packetObject.phyForce);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_BEGINCONVOY, typeof(S2C_BeginConvoy))]
    public void ReceiveS2C_BeginConvoy(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_BeginConvoy packetObject = pak.PacketObject as S2C_BeginConvoy;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            this.RequestC2S_GetConvoyInfo();
        }
    }

    [PacketHandler(OpcodeType.S2C_BUYCONVOYROBTIMES, typeof(S2C_BuyConvoyRobTimes))]
    public void ReceiveS2C_BuyConvoyRobTimes(Packet pak)
    {
        <ReceiveS2C_BuyConvoyRobTimes>c__AnonStorey11C storeyc = new <ReceiveS2C_BuyConvoyRobTimes>c__AnonStorey11C();
        GUIMgr.Instance.UnLock();
        storeyc.result = pak.PacketObject as S2C_BuyConvoyRobTimes;
        if (this.CheckResultCodeAndReLogin(storeyc.result.result))
        {
            XSingleton<GameDetainsDartMgr>.Singleton.buyInterceptTimes = storeyc.result.buyRobTimes;
            ActorData.getInstance().Stone = storeyc.result.stone;
            DetainsDartOnDoingPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DetainsDartOnDoingPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.SetInterceptBtnData(storeyc.result.remainRobTimes);
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(new Action<GUIEntity>(storeyc.<>m__B4));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_CONVOYAVENGECOMBAT, typeof(S2C_ConvoyAvengeCombat))]
    public void ReceiveS2C_ConvoyAvengeCombat(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ConvoyAvengeCombat packetObject = pak.PacketObject as S2C_ConvoyAvengeCombat;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (<>f__am$cache2C == null)
            {
                <>f__am$cache2C = delegate (bool isWin, BattleNormalGameType _type, BattleNormalGameResult resultInfo) {
                    BattleBack backData = new BattleBack {
                        securityDataList = BattleSecurityManager.Instance.GetSecurityDataList(),
                        end_time = (uint) TimeMgr.Instance.ServerStampTime,
                        result = isWin
                    };
                    Instance.RequestC2S_ConvoyAvengeCombatEnd(backData);
                };
            }
            BattleState.GetInstance().DoNormalBattle(packetObject.combatData, null, BattleNormalGameType.DetainDartBattleBack, true, "cj_aihao", 1, 5, null, null, <>f__am$cache2C);
        }
    }

    [PacketHandler(OpcodeType.S2C_CONVOYAVENGECOMBATEND, typeof(S2C_ConvoyAvengeCombatEnd))]
    public void ReceiveS2C_ConvoyAvengeCombatEnd(Packet pak)
    {
        <ReceiveS2C_ConvoyAvengeCombatEnd>c__AnonStorey118 storey = new <ReceiveS2C_ConvoyAvengeCombatEnd>c__AnonStorey118();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_ConvoyAvengeCombatEnd;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            Debug.LogWarning("result : " + this.CheckResultCodeAndReLogin(storey.result.result));
            Debug.LogWarning(string.Concat(new object[] { "result : ", storey.result.currGold, storey.result.incGold, storey.result.arenaLadderScore, storey.result.isWin }));
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__AD), base.gameObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_CONVOYEND, typeof(S2C_ConvoyEnd))]
    public void ReceiveS2C_ConvoyEnd(Packet pak)
    {
        <ReceiveS2C_ConvoyEnd>c__AnonStorey116 storey = new <ReceiveS2C_ConvoyEnd>c__AnonStorey116 {
            <>f__this = this
        };
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_ConvoyEnd;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__AB), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_CONVOYFRIENDFORMATION, typeof(S2C_ConvoyFriendFormation))]
    public void ReceiveS2C_ConvoyFriendFormation(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ConvoyFriendFormation packetObject = pak.PacketObject as S2C_ConvoyFriendFormation;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameDetainsDartMgr>.Singleton.CurPageFriendInfo_new = packetObject.formations;
            if (<>f__am$cache2E == null)
            {
                <>f__am$cache2E = ui => (ui as DetainsDartInviteFriendPanel).SetFriendInfo(XSingleton<GameDetainsDartMgr>.Singleton.curPageFriendInfo, false);
            }
            GUIMgr.Instance.PushGUIEntity<DetainsDartInviteFriendPanel>(<>f__am$cache2E);
        }
    }

    [PacketHandler(OpcodeType.S2C_CROSS_ACCEPTFRIENDPHYFORCE, typeof(S2C_Cross_AcceptFriendPhyForce))]
    public void ReceiveS2C_Cross_AcceptFriendPhyForce(Packet pak)
    {
        <ReceiveS2C_Cross_AcceptFriendPhyForce>c__AnonStorey122 storey = new <ReceiveS2C_Cross_AcceptFriendPhyForce>c__AnonStorey122();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_Cross_AcceptFriendPhyForce;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(storey.<>m__BB));
            int num = storey.result.phyForce - ActorData.getInstance().PhyForce;
            if (num > 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9896b9), num, storey.result.remainPhyForceAccept));
            }
            ActorData.getInstance().PhyForce = storey.result.phyForce;
            ActorData.getInstance().UserInfo.remainPhyForceAccept = storey.result.remainPhyForceAccept;
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.OnUpdateUIData();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_CROSS_GIVEFRIENDPHYFORCE, typeof(S2C_Cross_GiveFriendPhyForce))]
    public void ReceiveS2C_Cross_GiveFriendPhyForce(Packet pak)
    {
        <ReceiveS2C_Cross_GiveFriendPhyForce>c__AnonStorey121 storey = new <ReceiveS2C_Cross_GiveFriendPhyForce>c__AnonStorey121();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_Cross_GiveFriendPhyForce;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(storey.<>m__BA));
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9896a8));
            QQFriendInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<QQFriendInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.OnUpdateUIData();
            }
            FriendPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (panel2 != null)
            {
                panel2.OnUpdateUIData();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETCONVOYENEMYLIST, typeof(S2C_GetConvoyEnemyList))]
    public void ReceiveS2C_GetConvoyEnemyList(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GetConvoyEnemyList packetObject = pak.PacketObject as S2C_GetConvoyEnemyList;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().HaveNewDetainsDartLog = false;
            XSingleton<GameDetainsDartMgr>.Singleton.EnemyListInfo = packetObject.enemys;
            DetainsDartBattleRecordPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartBattleRecordPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetEnermyInfo(XSingleton<GameDetainsDartMgr>.Singleton.EnemyListInfo);
            }
            else
            {
                if (<>f__am$cache2D == null)
                {
                    <>f__am$cache2D = ui => (ui as DetainsDartBattleRecordPanel).SetEnermyInfo(XSingleton<GameDetainsDartMgr>.Singleton.EnemyListInfo);
                }
                GUIMgr.Instance.DoModelGUI<DetainsDartBattleRecordPanel>(<>f__am$cache2D, null);
            }
            DetainsDartMainUIPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<DetainsDartMainUIPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.SetNewRecordTipsInfo();
            }
            DetainsDartOnDoingPanel panel3 = GUIMgr.Instance.GetGUIEntity<DetainsDartOnDoingPanel>();
            if (panel3 != null)
            {
                panel3.SetNewRecordTipsInfo();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETCONVOYINFO, typeof(S2C_GetConvoyInfo))]
    public void ReceiveS2C_GetConvoyInfo(Packet pak)
    {
        <ReceiveS2C_GetConvoyInfo>c__AnonStorey115 storey = new <ReceiveS2C_GetConvoyInfo>c__AnonStorey115();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_GetConvoyInfo;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            GUIMgr.Instance.DockTitleBar();
            XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Clear();
            int num = 0;
            foreach (ConvoyInfo info in storey.result.convoyList)
            {
                ConvoyInfo item = new ConvoyInfo {
                    beginTime = info.beginTime,
                    beRobTimes = info.beRobTimes,
                    cards = info.cards,
                    duration = info.duration,
                    flagId = info.flagId,
                    lineId = info.lineId,
                    index = num,
                    isComplete = info.isComplete
                };
                if (info.lineId != -1)
                {
                    XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo.Add(item);
                }
                num++;
            }
            if ((storey.result.currFlagId >= 0) && (storey.result.currFlagId <= 3))
            {
                XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex = storey.result.currFlagId;
            }
            else
            {
                TipsDiag.SetText("服务器发送过来需默认选择的旗帜ID是错误的：" + storey.result.currFlagId);
            }
            XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt = storey.result.refreshFlagTimes;
            XSingleton<GameDetainsDartMgr>.Singleton.curEscortCnt = storey.result.remainConvoyTimes;
            XSingleton<GameDetainsDartMgr>.Singleton.curInterceptTimes = storey.result.remainRobTimes;
            XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt = storey.result.refreshTargetTimes;
            XSingleton<GameDetainsDartMgr>.Singleton.buyInterceptTimes = storey.result.buyRobTimes;
            if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState == GameDetainsDartMgr.DetainsDartState.Escort)
            {
                if (((XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Doing) || (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Done)) || ((XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.GettingReward) || (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.GettingReward)))
                {
                    DetainsDartOnDoingPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartOnDoingPanel>();
                    if (activityGUIEntity != null)
                    {
                        activityGUIEntity.SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                    }
                    else
                    {
                        if (<>f__am$cache2B == null)
                        {
                            <>f__am$cache2B = ui => (ui as DetainsDartOnDoingPanel).SetDataInfo(false, XSingleton<GameDetainsDartMgr>.Singleton.EscortListInfo[XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage]);
                        }
                        GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(<>f__am$cache2B);
                    }
                }
                else if (XSingleton<GameDetainsDartMgr>.Singleton.curEscortState == GameDetainsDartMgr.EscortState.Selet)
                {
                    XSingleton<GameDetainsDartMgr>.Singleton.UpdateDetainsDartInfo(storey.result);
                    DetainsDartMainUIPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartMainUIPanel>();
                    if (panel2 != null)
                    {
                        panel2.SetDataInfo(false, storey.result);
                    }
                    else
                    {
                        GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(new Action<GUIEntity>(storey.<>m__A9));
                    }
                }
            }
            else if (XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState != GameDetainsDartMgr.DetainsDartState.Intercept)
            {
                XSingleton<GameDetainsDartMgr>.Singleton.UpdateDetainsDartInfo(storey.result);
                DetainsDartMainUIPanel panel3 = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartMainUIPanel>();
                if (panel3 != null)
                {
                    panel3.SetDataInfo(false, storey.result);
                }
                else
                {
                    GUIMgr.Instance.PushGUIEntity<DetainsDartMainUIPanel>(new Action<GUIEntity>(storey.<>m__AA));
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETGOBLINSHOPINFO, typeof(S2C_GetGoblinShopInfo))]
    public void ReceiveS2C_GetGoblinShopInfo(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GetGoblinShopInfo packetObject = pak.PacketObject as S2C_GetGoblinShopInfo;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedGoblinShop = activityGUIEntity.ActivedGoblinShop;
                if (null != activedGoblinShop)
                {
                    List<ActivityShopPanel.ShopDataItem> items = new List<ActivityShopPanel.ShopDataItem>();
                    foreach (ShopItem item in packetObject.itemList)
                    {
                        goblin_shop_config _config = ConfigMgr.getInstance().getByEntry<goblin_shop_config>(item.entry);
                        if (_config != null)
                        {
                            ActivityShopPanel.ShopDataItem item2 = new ActivityShopPanel.ShopDataItem {
                                CostType = _config.cost_type,
                                Limit = _config.limit,
                                Item = item,
                                EntryId = _config.goods_entry
                            };
                            items.Add(item2);
                        }
                    }
                    activedGoblinShop.Type = ActivityShopPanel.ShopType.Goblin;
                    activedGoblinShop.FixedMessage = ConfigMgr.getInstance().GetWord(0x2c3f);
                    activedGoblinShop.ShowShop("奸商", items);
                    activedGoblinShop.OnFixed = () => this.Request_C2S_GoblinShopFix();
                    activedGoblinShop.OnBuy = delegate (ShopItem item) {
                        BuyShopItemInfo buy = new BuyShopItemInfo {
                            cost = item.cost,
                            shopEntry = item.entry,
                            slot = item.slot
                        };
                        this.Request_C2S_GoblinShopBuy(buy);
                        return true;
                    };
                    activedGoblinShop.OnRefresh = () => this.Request_C2S_GoblinShopRefresh();
                    if (<>f__am$cache37 == null)
                    {
                        <>f__am$cache37 = delegate {
                            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                            if (_config == null)
                            {
                                return 0;
                            }
                            return _config.fix_goblin_shop_stone;
                        };
                    }
                    activedGoblinShop.CalFixed = <>f__am$cache37;
                    activedGoblinShop.StopTime = ActorData.getInstance().goblinShopOpenTime + ActorData.getInstance().goblinShopDuration;
                    if (<>f__am$cache38 == null)
                    {
                        <>f__am$cache38 = delegate (out int time) {
                            time = ActorData.getInstance().UserInfo.refreshGoblinCount;
                            ArrayList list = ConfigMgr.getInstance().getList<goblin_shop_refresh_config>();
                            for (int j = 0; j != list.Count; j++)
                            {
                                goblin_shop_refresh_config _config = (goblin_shop_refresh_config) list[j];
                                if (((time + 1) < _config.count) && (j > 0))
                                {
                                    goblin_shop_refresh_config _config2 = (goblin_shop_refresh_config) list[j - 1];
                                    return _config2.cost_value;
                                }
                            }
                            goblin_shop_refresh_config _config3 = (goblin_shop_refresh_config) list[list.Count - 1];
                            return _config3.cost_value;
                        };
                    }
                    activedGoblinShop.CalCost = <>f__am$cache38;
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETGUILDDUPINFO, typeof(S2C_GetGuildDupInfo))]
    public void ReceiveS2C_GetGuildDupInfo(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GetGuildDupInfo packetObject = pak.PacketObject as S2C_GetGuildDupInfo;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameGuildMgr>.Singleton.UpdateDupState(packetObject.guildDupStatusInfo);
            GuildDupList gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupList>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowGuildDup(packetObject.guildDupStatusInfo);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETGUILDDUPTRENCHINFO, typeof(S2C_GetGuildDupTrenchInfo))]
    public void ReceiveS2C_GetGuildDupTrenchInfo(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GetGuildDupTrenchInfo packetObject = pak.PacketObject as S2C_GetGuildDupTrenchInfo;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildDupTrenchMap gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupTrenchMap>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowTrenchState(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETROBTARGETLIST, typeof(S2C_GetRobTargetList))]
    public void ReceiveS2C_GetRobTargetList(Packet pak)
    {
        <ReceiveS2C_GetRobTargetList>c__AnonStorey117 storey = new <ReceiveS2C_GetRobTargetList>c__AnonStorey117();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_GetRobTargetList;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            int num = 0;
            variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
            if (_config != null)
            {
                num = _config.convoy_rob_befresh_interval;
            }
            XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime = storey.result.refreshTime + num;
            Debug.LogWarning("当前拦截目标剩余时间：__" + TimeMgr.Instance.GetRemainTime2(XSingleton<GameDetainsDartMgr>.Singleton.curInterceptLeftTime));
            XSingleton<GameDetainsDartMgr>.Singleton.curRefreshInterceptCnt = storey.result.refreshTargetTimes;
            XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.Intercept;
            XSingleton<GameDetainsDartMgr>.Singleton.curLeftRightPage = 0;
            if (storey.result.targets.Count > 0)
            {
                XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState = GameDetainsDartMgr.InterceptState.Battle;
            }
            else
            {
                XSingleton<GameDetainsDartMgr>.Singleton.curInterceptState = GameDetainsDartMgr.InterceptState.SeletPlayer;
            }
            DetainsDartOnDoingPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartOnDoingPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetDataInfo(false, storey.result.targets);
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity<DetainsDartOnDoingPanel>(new Action<GUIEntity>(storey.<>m__AC));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETSECRETSHOPINFO, typeof(S2C_GetSecretShopInfo))]
    public void ReceiveS2C_GetSecretShopInfo(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GetSecretShopInfo packetObject = pak.PacketObject as S2C_GetSecretShopInfo;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ShopTabEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            if (null != activityGUIEntity)
            {
                ActivityShopPanel activedSecretShop = activityGUIEntity.ActivedSecretShop;
                if (null != activedSecretShop)
                {
                    List<ActivityShopPanel.ShopDataItem> items = new List<ActivityShopPanel.ShopDataItem>();
                    foreach (ShopItem item in packetObject.itemList)
                    {
                        secret_shop_config _config = ConfigMgr.getInstance().getByEntry<secret_shop_config>(item.entry);
                        if (_config != null)
                        {
                            ActivityShopPanel.ShopDataItem item2 = new ActivityShopPanel.ShopDataItem {
                                CostType = _config.cost_type,
                                Limit = _config.limit,
                                Item = item,
                                EntryId = _config.goods_entry
                            };
                            items.Add(item2);
                        }
                    }
                    activedSecretShop.Type = ActivityShopPanel.ShopType.Secret;
                    activedSecretShop.FixedMessage = ConfigMgr.getInstance().GetWord(0x2c3e);
                    activedSecretShop.ShowShop("黑商", items);
                    activedSecretShop.StopTime = ActorData.getInstance().secretShopOpenTime + ActorData.getInstance().secretShopDuration;
                    activedSecretShop.OnFixed = () => this.Request_C2S_SecretShopFix();
                    activedSecretShop.OnRefresh = () => this.Request_C2S_SecretShopRefresh();
                    activedSecretShop.OnBuy = delegate (ShopItem item) {
                        BuyShopItemInfo buy = new BuyShopItemInfo {
                            cost = item.cost,
                            shopEntry = item.entry,
                            slot = item.slot
                        };
                        this.Request_C2S_SecretShopBuy(buy);
                        return true;
                    };
                    if (<>f__am$cache39 == null)
                    {
                        <>f__am$cache39 = delegate {
                            vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                            if (_config == null)
                            {
                                return 0;
                            }
                            return _config.fix_secret_shop_stone;
                        };
                    }
                    activedSecretShop.CalFixed = <>f__am$cache39;
                    if (<>f__am$cache3A == null)
                    {
                        <>f__am$cache3A = delegate (out int time) {
                            time = ActorData.getInstance().UserInfo.refreshSecretCount;
                            ArrayList list = ConfigMgr.getInstance().getList<secret_shop_refresh_config>();
                            for (int j = 0; j != list.Count; j++)
                            {
                                secret_shop_refresh_config _config = (secret_shop_refresh_config) list[j];
                                if (((time + 1) < _config.count) && (j > 0))
                                {
                                    secret_shop_refresh_config _config2 = (secret_shop_refresh_config) list[j - 1];
                                    return _config2.cost_value;
                                }
                            }
                            secret_shop_refresh_config _config3 = (secret_shop_refresh_config) list[list.Count - 1];
                            return _config3.cost_value;
                        };
                    }
                    activedSecretShop.CalCost = <>f__am$cache3A;
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GIVEALL_TX_FRIENDPHYFORCE, typeof(S2C_GiveAll_TX_FriendPhyForce))]
    public void ReceiveS2C_GiveAll_TX_FriendPhyForce(Packet pack)
    {
        <ReceiveS2C_GiveAll_TX_FriendPhyForce>c__AnonStorey120 storey = new <ReceiveS2C_GiveAll_TX_FriendPhyForce>c__AnonStorey120 {
            result = pack.PacketObject as S2C_GiveAll_TX_FriendPhyForce
        };
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            XSingleton<SocialFriend>.Singleton.Each(new SocialFriend.EachCondtion(storey.<>m__B8));
            QQFriendInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<QQFriendInfoPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.OnUpdateUIData();
            }
            FriendPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (panel2 != null)
            {
                panel2.OnUpdateUIData();
            }
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x59b));
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPCOMBATEND, typeof(S2C_GuildDupCombatEnd))]
    public void ReceiveS2C_GuildDupCombatEnd(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GuildDupCombatEnd packetObject = pak.PacketObject as S2C_GuildDupCombatEnd;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            XSingleton<GameGuildMgr>.Singleton.UpdateDeathHero(packetObject.deadCardEntry);
            XSingleton<GameGuildMgr>.Singleton.ReleaseLock(false);
            BattleStaticEntry.OnExitGuildDupBattle(packetObject);
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPDISTRIBUTEHISTORY, typeof(S2C_GuildDupDistributeHistory))]
    public void ReceiveS2C_GuildDupDistributeHistory(Packet pak)
    {
        <ReceiveS2C_GuildDupDistributeHistory>c__AnonStorey130 storey = new <ReceiveS2C_GuildDupDistributeHistory>c__AnonStorey130();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_GuildDupDistributeHistory;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            GuidBattleBossHandRecordPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossHandRecordPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveGuildDupDistributeHistory(storey.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleBossHandRecordPanel>(new Action<GUIEntity>(storey.<>m__D0), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPITEMMANUALDISTRIBUTE, typeof(S2C_GuildDupItemManualDistribute))]
    public void ReceiveS2C_GuildDupItemManualDistribute(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GuildDupItemManualDistribute packetObject = pak.PacketObject as S2C_GuildDupItemManualDistribute;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuidBattleItemHandQueuePanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleItemHandQueuePanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetHandOutItemInfo(activityGUIEntity.xxTestHandPlayerQueueInfoList, 0, true, false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPITEMQUEUEINFO, typeof(S2C_GuildDupItemQueueInfo))]
    public void ReceiveS2C_GuildDupItemQueueInfo(Packet pak)
    {
        <ReceiveS2C_GuildDupItemQueueInfo>c__AnonStorey12D storeyd = new <ReceiveS2C_GuildDupItemQueueInfo>c__AnonStorey12D();
        GUIMgr.Instance.UnLock();
        storeyd.result = pak.PacketObject as S2C_GuildDupItemQueueInfo;
        if (this.CheckResultCodeAndReLogin(storeyd.result.result))
        {
            GuidBattleBossHandOutPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossHandOutPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveBossDamageRank(storeyd.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleBossHandOutPanel>(new Action<GUIEntity>(storeyd.<>m__CC), null);
            }
            GuidBattleItemHandQueuePanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleItemHandQueuePanel>();
            if (panel2 != null)
            {
                bool isGuidMaster = false;
                GuildMemberData mGuildMemberData = ActorData.getInstance().mGuildMemberData;
                if (mGuildMemberData != null)
                {
                    if (<>f__am$cache34 == null)
                    {
                        <>f__am$cache34 = t => t.userInfo.id == ActorData.getInstance().SessionInfo.userid;
                    }
                    GuildMember member = mGuildMemberData.member.Find(<>f__am$cache34);
                    if (member != null)
                    {
                        if (member.position == 1)
                        {
                            isGuidMaster = true;
                        }
                        else
                        {
                            isGuidMaster = false;
                        }
                    }
                }
                panel2.SetHandOutItemInfo(panel2.xxTestHandPlayerQueueInfoList, storeyd.result.manualTimes, false, isGuidMaster);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPLICATEDAMAGERANK, typeof(S2C_GuildDuplicateDamageRank))]
    public void ReceiveS2C_GuildDuplicateDamageRank(Packet pack)
    {
        <ReceiveS2C_GuildDuplicateDamageRank>c__AnonStorey129 storey = new <ReceiveS2C_GuildDuplicateDamageRank>c__AnonStorey129();
        GUIMgr.Instance.UnLock();
        storey.result = pack.PacketObject as S2C_GuildDuplicateDamageRank;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            GuidBattleDamageBarRankPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuidBattleDamageBarRankPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.ReceiveDupDamageRank(storey.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleDamageBarRankPanel>(new Action<GUIEntity>(storey.<>m__C8), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPLICATEPASSRANK, typeof(S2C_GuildDuplicatePassRank))]
    public void ReceiveS2C_GuildDuplicatePassRank(Packet pak)
    {
        <ReceiveS2C_GuildDuplicatePassRank>c__AnonStorey12B storeyb = new <ReceiveS2C_GuildDuplicatePassRank>c__AnonStorey12B();
        GUIMgr.Instance.UnLock();
        storeyb.result = pak.PacketObject as S2C_GuildDuplicatePassRank;
        if (this.CheckResultCodeAndReLogin(storeyb.result.result))
        {
            PanelGuildDupTimeRank gUIEntity = GUIMgr.Instance.GetGUIEntity<PanelGuildDupTimeRank>();
            if (gUIEntity != null)
            {
                gUIEntity.ReceiveDupRank(storeyb.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<PanelGuildDupTimeRank>(new Action<GUIEntity>(storeyb.<>m__CA), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPSUPPORTRANK, typeof(S2C_GuildDupSupportRank))]
    public void ReceiveS2C_GuildDupSupportRank(Packet pak)
    {
        <ReceiveS2C_GuildDupSupportRank>c__AnonStorey12F storeyf = new <ReceiveS2C_GuildDupSupportRank>c__AnonStorey12F();
        GUIMgr.Instance.UnLock();
        storeyf.result = pak.PacketObject as S2C_GuildDupSupportRank;
        if (this.CheckResultCodeAndReLogin(storeyf.result.result))
        {
            GuidBattleBossRicherRankPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossRicherRankPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveGuildDupSupportRank(storeyf.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleBossRicherRankPanel>(new Action<GUIEntity>(storeyf.<>m__CF), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPSUPPORTTRENCH, typeof(S2C_GuildDupSupportTrench))]
    public void ReceiveS2C_GuildDupSupportTrench(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GuildDupSupportTrench packetObject = pak.PacketObject as S2C_GuildDupSupportTrench;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuidBattleBossRicherHelpPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossRicherHelpPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveGuildDupSupportTrench(packetObject, false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDDUPSUPPORTTRENCHRANK, typeof(S2C_GuildDupSupportTrenchRank))]
    public void ReceiveS2C_GuildDupSupportTrenchRank(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_GuildDupSupportTrenchRank packetObject = pak.PacketObject as S2C_GuildDupSupportTrenchRank;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GuildDupBeginBattle gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupBeginBattle>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowSupportList(packetObject.supports);
                gUIEntity.SetSupportCout(packetObject);
            }
            GuidBattleBossRicherHelpPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossRicherHelpPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.InitSupportTrench(packetObject.supportCount);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDTRENCHDAMAGERANK, typeof(S2C_GuildTrenchDamageRank))]
    public void ReceiveS2C_GuildTrenchDamageRank(Packet pak)
    {
        <ReceiveS2C_GuildTrenchDamageRank>c__AnonStorey12A storeya = new <ReceiveS2C_GuildTrenchDamageRank>c__AnonStorey12A();
        GUIMgr.Instance.UnLock();
        storeya.result = pak.PacketObject as S2C_GuildTrenchDamageRank;
        if (this.CheckResultCodeAndReLogin(storeya.result.result))
        {
            GuidBattleDamageBarRankPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleDamageBarRankPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveTrenchDamageRank(storeya.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleDamageBarRankPanel>(new Action<GUIEntity>(storeya.<>m__C9), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GUILDWHOLESVRTRENCHDAMAGERANK, typeof(S2C_GuildWholeSvrTrenchDamageRank))]
    public void ReceiveS2C_GuildWholeSvrTrenchDamageRank(Packet pak)
    {
        <ReceiveS2C_GuildWholeSvrTrenchDamageRank>c__AnonStorey12C storeyc = new <ReceiveS2C_GuildWholeSvrTrenchDamageRank>c__AnonStorey12C();
        GUIMgr.Instance.UnLock();
        storeyc.result = pak.PacketObject as S2C_GuildWholeSvrTrenchDamageRank;
        if (this.CheckResultCodeAndReLogin(storeyc.result.result))
        {
            GuidBattleBossDamageRankPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuidBattleBossDamageRankPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.ReceiveBossDamageRank(storeyc.result, 1, 10, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleBossDamageRankPanel>(new Action<GUIEntity>(storeyc.<>m__CB), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFEFRIENDREWARD, typeof(S2C_NewLifeFriendReward))]
    public void ReceiveS2C_NewLifeFriendReward(Packet pack)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeFriendReward packetObject = pack.PacketObject as S2C_NewLifeFriendReward;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LifeSkillFriendAndRewardPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillFriendAndRewardPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateRewardList(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFERECVFRIENDREWARD, typeof(S2C_NewLifeRecvFriendReward))]
    public void ReceiveS2C_NewLifeRecvFriendReward(Packet pak)
    {
        <ReceiveS2C_NewLifeRecvFriendReward>c__AnonStorey132 storey = new <ReceiveS2C_NewLifeRecvFriendReward>c__AnonStorey132();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_NewLifeRecvFriendReward;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            GUIMgr.Instance.DoModelGUI<RewardPanel>(new Action<GUIEntity>(storey.<>m__D2), null);
            LifeSkillFriendAndRewardPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillFriendAndRewardPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateRewardList(storey.result.reward, true);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESENDANDRECEIVETIMES, typeof(S2C_NewLifeSendAndReceiveTimes))]
    public void ReceiveS2C_NewLifeSendAndReceiveTimes(Packet pack)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSendAndReceiveTimes packetObject = pack.PacketObject as S2C_NewLifeSendAndReceiveTimes;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<LifeSkillManager>.Singleton.UpdateRemain(packetObject.remain);
            LifeSkillFriendAndRewardPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<LifeSkillFriendAndRewardPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateFriendList(packetObject.friendList, false);
                gUIEntity.UpdateLimit(packetObject.limit_max - packetObject.recved_num, packetObject.limit_max);
            }
            LifeSkillSoltPanel panel2 = GUIMgr.Instance.GetGUIEntity<LifeSkillSoltPanel>();
            if (panel2 != null)
            {
                panel2.UpdateRemain();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLENDHANGUP, typeof(S2C_NewLifeSkillEndHangUp))]
    public void ReceiveS2C_NewLifeSkillEndHangUp(Packet packet)
    {
        <ReceiveS2C_NewLifeSkillEndHangUp>c__AnonStorey131 storey = new <ReceiveS2C_NewLifeSkillEndHangUp>c__AnonStorey131();
        GUIMgr.Instance.UnLock();
        storey.result = packet.PacketObject as S2C_NewLifeSkillEndHangUp;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            NewLifeCellInfo cellInfo = XSingleton<LifeSkillManager>.Singleton.GetCellInfo((NewLifeSkillType) storey.result.entry, storey.result.cell_index);
            XSingleton<LifeSkillManager>.Singleton.UpdateCardData(storey.result.card_data);
            XSingleton<LifeSkillManager>.Singleton.UpdateLifeSkillMapData(storey.result.data);
            LifeSkillRewardDetailPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillRewardDetailPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowSkillType((NewLifeSkillType) storey.result.entry);
            }
            if (cellInfo != null)
            {
                LifeSkillPanel panel2 = GUIMgr.Instance.GetGUIEntity<LifeSkillPanel>();
                if (panel2 == null)
                {
                    return;
                }
                panel2.RemoveCard(storey.result.cell_index, cellInfo.card_entry);
            }
            RewardPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<RewardPanel>();
            if (null != gUIEntity)
            {
                gUIEntity.ShowLifeSkillCollectReward(storey.result);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<RewardPanel>(new Action<GUIEntity>(storey.<>m__D1), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLGETFRIEND, typeof(S2C_NewLifeSkillGetFriend))]
    public void ReceiveS2C_NewLifeSkillGetFriend(Packet pack)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSkillGetFriend packetObject = pack.PacketObject as S2C_NewLifeSkillGetFriend;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LifeSkillFriendAndRewardPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillFriendAndRewardPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateFriendList(packetObject);
            }
            LifeSkillSelectFriendPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillSelectFriendPanel>();
            if (panel2 != null)
            {
                panel2.UpdateFriendList(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLHANGUP, typeof(S2C_NewLifeSkillHangUp))]
    public void ReceiveS2C_NewLifeSkillHangUp(Packet packet)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSkillHangUp packetObject = packet.PacketObject as S2C_NewLifeSkillHangUp;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<LifeSkillManager>.Singleton.UpdateCardData(packetObject.card_data);
            XSingleton<LifeSkillManager>.Singleton.UpdateLifeSkillMapData(packetObject.data);
            LifeSkillPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.AddCard(packetObject.cell_index, packetObject.card_entry);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLMAPENTER, typeof(S2C_NewLifeSkillMapEnter))]
    public void ReceiveS2C_NewLifeSkillMapEnter(Packet packet)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSkillMapEnter packetObject = packet.PacketObject as S2C_NewLifeSkillMapEnter;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LifeSkillPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowResult(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLMAPSIMPLEDATA, typeof(S2C_NewLifeSkillMapSimpleData))]
    public void ReceiveS2C_NewLifeSkillMapSimpleData(Packet paket)
    {
        S2C_NewLifeSkillMapSimpleData packetObject = paket.PacketObject as S2C_NewLifeSkillMapSimpleData;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<LifeSkillManager>.Singleton.CompletedSimplyData(packetObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLPROFIT, typeof(S2C_NewLifeSkillProfit))]
    public void ReceiveS2C_NewLifeSkillProfit(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSkillProfit packetObject = pak.PacketObject as S2C_NewLifeSkillProfit;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            LifeSkillRewardDetailPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillRewardDetailPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowRewardDetail(packetObject.rewardInfo);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_NEWLIFESKILLRANDREWARD, typeof(S2C_NewLifeSkillRandReward))]
    public void ReceiveS2C_NewLifeSkillRandReward(Packet packet)
    {
        GUIMgr.Instance.UnLock();
        S2C_NewLifeSkillRandReward packetObject = packet.PacketObject as S2C_NewLifeSkillRandReward;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<LifeSkillManager>.Singleton.UpdateRemain(packetObject.remain);
            XSingleton<LifeSkillManager>.Singleton.UpdateLifeSkillMapData(packetObject.data);
            XSingleton<LifeSkillManager>.Singleton.UpdateNextCostStone(packetObject.next_cost_stone);
            ActorData.getInstance().Stone = packetObject.stone;
            LifeSkillSoltPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<LifeSkillSoltPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowResult(packetObject.RandInfo);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_OPENGUILDDUP, typeof(S2C_OpenGuildDup))]
    public void ReceiveS2C_OpenGuildDup(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_OpenGuildDup packetObject = pak.PacketObject as S2C_OpenGuildDup;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameGuildMgr>.Singleton.UpdateDupState(packetObject.guildDupStatusInfo);
            GuildDupList gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupList>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowGuildDup(packetObject.guildDupStatusInfo);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_QQFRIENDSINGAME_BEYOND_SHARE, typeof(S2C_QQFriendsInGame_Beyond_Share))]
    public void ReceiveS2C_QQFriendsInGame_Beyond_Share(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_QQFriendsInGame_Beyond_Share packetObject = pak.PacketObject as S2C_QQFriendsInGame_Beyond_Share;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c48));
        }
    }

    [PacketHandler(OpcodeType.S2C_QQFRIENDSINGAME_SHARE, typeof(S2C_QQFriendsInGame_Share))]
    public void ReceiveS2C_QQFriendsInGame_Share(Packet p)
    {
        GUIMgr.Instance.UnLock();
        S2C_QQFriendsInGame_Share packetObject = p.PacketObject as S2C_QQFriendsInGame_Share;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2c49));
        }
    }

    [PacketHandler(OpcodeType.S2C_REFRESHCONVOYFLAG, typeof(S2C_RefreshConvoyFlag))]
    public void ReceiveS2C_RefreshConvoyFlag(Packet pak)
    {
        <ReceiveS2C_RefreshConvoyFlag>c__AnonStorey11B storeyb = new <ReceiveS2C_RefreshConvoyFlag>c__AnonStorey11B();
        GUIMgr.Instance.UnLock();
        storeyb.result = pak.PacketObject as S2C_RefreshConvoyFlag;
        if (this.CheckResultCodeAndReLogin(storeyb.result.result))
        {
            XSingleton<GameDetainsDartMgr>.Singleton.curSelFlagIndex = storeyb.result.flagId;
            XSingleton<GameDetainsDartMgr>.Singleton.curRefreshFlagCnt = storeyb.result.curTimes;
            ActorData.getInstance().Stone = storeyb.result.stone;
            DetainsDartSelFlagAndTeamPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartSelFlagAndTeamPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetFreshFlagNeed(storeyb.result.curTimes);
                activityGUIEntity.PlayAnim();
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity<DetainsDartSelFlagAndTeamPanel>(new Action<GUIEntity>(storeyb.<>m__B3));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REFRESHCONVOYTARGET, typeof(S2C_RefreshConvoyTarget))]
    public void ReceiveS2C_RefreshConvoyTarget(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_RefreshConvoyTarget packetObject = pak.PacketObject as S2C_RefreshConvoyTarget;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Gold = packetObject.gold;
            this.RequestC2S_GetRobTargetList();
        }
    }

    [PacketHandler(OpcodeType.S2C_RELEASEGUILDDUPLOCK, typeof(S2C_ReleaseGuildDupLock))]
    public void ReceiveS2C_ReleaseGuildDupLock(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ReleaseGuildDupLock packetObject = pak.PacketObject as S2C_ReleaseGuildDupLock;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameGuildMgr>.Singleton.ReleaseLock(false);
        }
    }

    [PacketHandler(OpcodeType.S2C_REQGUILDDUPITEMDISTRIBUTE, typeof(S2C_ReqGuildDupItemDistribute))]
    public void ReceiveS2C_ReqGuildDupItemDistribute(Packet pak)
    {
        <ReceiveS2C_ReqGuildDupItemDistribute>c__AnonStorey12E storeye = new <ReceiveS2C_ReqGuildDupItemDistribute>c__AnonStorey12E();
        GUIMgr.Instance.UnLock();
        storeye.result = pak.PacketObject as S2C_ReqGuildDupItemDistribute;
        if (this.CheckResultCodeAndReLogin(storeye.result.result))
        {
            GuidBattleBossHandOutPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossHandOutPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveBossDamageRank(storeye.result, false);
            }
            else
            {
                GUIMgr.Instance.DoModelGUI<GuidBattleBossHandOutPanel>(new Action<GUIEntity>(storeye.<>m__CE), null);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_RESETGUILDDUP, typeof(S2C_ResetGuildDup))]
    public void ReceiveS2C_ResetGuildDup(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ResetGuildDup packetObject = pak.PacketObject as S2C_ResetGuildDup;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameGuildMgr>.Singleton.UpdateDupState(packetObject.guildDupStatusInfo);
            ActorData.getInstance().mGuildData.dupEnergy = packetObject.guildDupEnergy;
            GuildDupTrenchMap gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupTrenchMap>();
            if (gUIEntity != null)
            {
                gUIEntity.ShowTrenchState(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_ROBCONVOYCOMBATBEGIN, typeof(S2C_RobConvoyCombatBegin))]
    public void ReceiveS2C_RobConvoyCombatBegin(Packet pak)
    {
        <ReceiveS2C_RobConvoyCombatBegin>c__AnonStorey11A storeya = new <ReceiveS2C_RobConvoyCombatBegin>c__AnonStorey11A();
        GUIMgr.Instance.UnLock();
        storeya.result = pak.PacketObject as S2C_RobConvoyCombatBegin;
        if (this.CheckResultCodeAndReLogin(storeya.result.result))
        {
            foreach (CombatTeam team in storeya.result.combatData.defenderList)
            {
                foreach (CombatDetailActor actor in team.actor)
                {
                    Debug.LogWarning(string.Concat(new object[] { "当前战斗对手||||||||   血量__： ", actor.curHp, "            怒气___：", actor.energy, "              攻击___:", actor.attack }));
                }
            }
            foreach (CombatDetailActor actor2 in storeya.result.combatData.attacker.actor)
            {
                string name = string.Empty;
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(actor2.entry);
                if (_config != null)
                {
                    name = _config.name;
                }
                Debug.LogWarning(string.Concat(new object[] { "当前战斗玩家自己||||||||   卡牌名字___:", name, "            血量__： ", actor2.curHp, "            怒气___：", actor2.energy, "              攻击___:", actor2.attack }));
            }
            BattleState.GetInstance().DoNormalBattle(storeya.result.combatData, null, BattleNormalGameType.DetainDartIntercept, true, "cj_aihao", 0, 5, null, null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(storeya.<>m__B2));
        }
    }

    [PacketHandler(OpcodeType.S2C_ROBCONVOYCOMBATEND, typeof(S2C_RobConvoyCombatEnd))]
    public void ReceiveS2C_RobConvoyCombatEnd(Packet pak)
    {
        <ReceiveS2C_RobConvoyCombatEnd>c__AnonStorey119 storey = new <ReceiveS2C_RobConvoyCombatEnd>c__AnonStorey119();
        GUIMgr.Instance.UnLock();
        storey.result = pak.PacketObject as S2C_RobConvoyCombatEnd;
        if (this.CheckResultCodeAndReLogin(storey.result.result))
        {
            Debug.LogWarning("result : " + this.CheckResultCodeAndReLogin(storey.result.result));
            Debug.LogWarning(string.Concat(new object[] { "result : ", storey.result.currGold, storey.result.currStone, storey.result.incGold, storey.result.incStone, storey.result.isWin }));
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__B1), base.gameObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_TRYLOCKGUILDDUP, typeof(S2C_TryLockGuildDup))]
    public void ReceiveS2C_TryLockGuildDup(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_TryLockGuildDup packetObject = pak.PacketObject as S2C_TryLockGuildDup;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            XSingleton<GameGuildMgr>.Singleton.UpdateDeathHero(packetObject.deadCardEntry);
            XSingleton<GameGuildMgr>.Singleton.Lock(packetObject.guildDupId, packetObject.guildDupTrenchId, (int) packetObject.breakLockTime);
            GuildDupBeginBattle gUIEntity = GUIMgr.Instance.GetGUIEntity<GuildDupBeginBattle>();
            if (gUIEntity != null)
            {
                gUIEntity.LockGuildResult(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SEARCHFRIEND, typeof(S2C_SearchFriend))]
    public void ReceiveSearchFriend(Packet pak)
    {
        S2C_SearchFriend packetObject = (S2C_SearchFriend) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.userList.Count == 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989692));
            }
            else
            {
                AddFriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AddFriendPanel>();
                if (activityGUIEntity != null)
                {
                    activityGUIEntity.ShowSearchedFriend(packetObject.userList);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SELLITEM, typeof(S2C_SellItem))]
    public void ReceiveSellItem(Packet pak)
    {
        S2C_SellItem packetObject = (S2C_SellItem) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UpdateItemList(packetObject.items);
            ActorData.getInstance().Gold = packetObject.gold;
            BagPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BagPanel>();
            if ((gUIEntity != null) && gUIEntity.gameObject.activeSelf)
            {
                gUIEntity.UpdateData(packetObject.items);
            }
            FragmentBagPanel panel2 = GUIMgr.Instance.GetGUIEntity<FragmentBagPanel>();
            if ((panel2 != null) && panel2.gameObject.activeSelf)
            {
                panel2.UpdateData(packetObject.items);
            }
            BreakEquipPanel panel3 = GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>();
            if (null != panel3)
            {
                panel3.DelItemUpdateData();
            }
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x277c));
        }
    }

    [PacketHandler(OpcodeType.S2C_SENDMAIL, typeof(S2C_SendMail))]
    public void ReceiveSendMail(Packet pak)
    {
        S2C_SendMail packetObject = (S2C_SendMail) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x989750));
            GUIMgr.Instance.ExitModelGUI("MailPanel");
        }
    }

    [PacketHandler(OpcodeType.S2C_SETBATTLEMEMBER, typeof(S2C_SetBattleMember))]
    public void ReceiveSetBattleMember(Packet pak)
    {
        S2C_SetBattleMember packetObject = (S2C_SetBattleMember) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            switch (packetObject.formation.type)
            {
                case BattleFormationType.BattleFormationType_Arena_Def:
                {
                    ActorData.getInstance().ArenaFormation = packetObject.formation;
                    ArenaLadderPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ArenaLadderPanel>();
                    if (activityGUIEntity != null)
                    {
                        activityGUIEntity.UpdateTeamInfo();
                    }
                    break;
                }
                case BattleFormationType.BattleFormationType_League_Def:
                {
                    ActorData.getInstance().WorldCupFormation = packetObject.formation;
                    GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("WorldCupPanel");
                    if (gUIEntity != null)
                    {
                        (gUIEntity as WorldCupPanel).UpdateTeamInfo();
                    }
                    break;
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SETHEAD, typeof(S2C_SetHead))]
    public void ReceiveSetHead(Packet pak)
    {
        S2C_SetHead packetObject = (S2C_SetHead) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            GUIMgr.Instance.ExitModelGUI("ChangePlayerIconPanel");
            ActorData.getInstance().HeadEntry = packetObject.headEntry;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("PlayerInfoPanel");
            if (gUIEntity != null)
            {
                PlayerInfoPanel panel = (PlayerInfoPanel) gUIEntity;
                if (panel != null)
                {
                    panel.UpdateHead();
                }
            }
            else
            {
                MainUI nui = GUIMgr.Instance.GetGUIEntity<MainUI>();
                if (nui != null)
                {
                    nui.Create3DRole(ActorData.getInstance().UserInfo.headEntry);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SETSIGNATURE, typeof(S2C_SetSignature))]
    public void ReceiveSetSignature(Packet pak)
    {
        S2C_SetSignature packetObject = (S2C_SetSignature) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.signature = packetObject.signature;
            GUIMgr.Instance.ExitModelGUI("SetSignatureDlag");
            GUIEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity("FriendPanel");
            if (activityGUIEntity != null)
            {
                FriendPanel panel = (FriendPanel) activityGUIEntity;
                if (panel != null)
                {
                    panel.UpdateSignature();
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SETUSERTITLE, typeof(S2C_SetUserTitle))]
    public void ReceiveSetUserTitle(Packet pak)
    {
        S2C_SetUserTitle packetObject = (S2C_SetUserTitle) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().UserInfo.titleEntry = packetObject.cur_title;
            ActorData.getInstance().UserInfo.title_time = packetObject.title_time;
            TitlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TitlePanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateTitle();
            }
            PlayerInfoPanel panel2 = GUIMgr.Instance.GetGUIEntity<PlayerInfoPanel>();
            if (panel2 != null)
            {
                panel2.UpdateTitle();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SHAKEGOLDTREE, typeof(S2C_ShakeGoldTree))]
    public void ReceiveShakeGoldTree(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_ShakeGoldTree packetObject = pak.PacketObject as S2C_ShakeGoldTree;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData data = ActorData.getInstance();
            data.Gold = packetObject.gold;
            data.Stone = packetObject.stone;
            data.UserInfo.cur_shake_count = packetObject.cur_shake_count;
            GoldTreePanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GoldTreePanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ShowShakeResult(packetObject);
            }
        }
        else if (packetObject.result == OpResult.OpResult_Not_Enough_ShakeCount)
        {
            GoldTreePanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<GoldTreePanel>();
            if (panel2 != null)
            {
                panel2.ShowError(false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_REQCARDSKILLLVLUP, typeof(S2C_ReqCardSkillLvlUp))]
    public void ReceiveSkillLelvUp(Packet pak)
    {
        S2C_ReqCardSkillLvlUp packetObject = (S2C_ReqCardSkillLvlUp) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().CurSkillPoint = packetObject.skillPoint;
            ActorData.getInstance().Gold = packetObject.gold;
            ActorData.getInstance().UpdateCardSkill(packetObject.card_id, packetObject.skillIndex, packetObject.skillLv);
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("HeroInfoPanel");
            if (gUIEntity != null)
            {
                HeroInfoPanel panel = gUIEntity.Achieve<HeroInfoPanel>();
                if (panel != null)
                {
                    panel.UpdateSkillInfo(packetObject.card_id, packetObject.skillIndex, packetObject.skillPoint, packetObject.skillLv);
                    List<long> cardIdList = new List<long> {
                        packetObject.card_id
                    };
                    this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
                }
            }
            this.CheckLivenessReward();
        }
        else
        {
            GUIEntity entity2 = GUIMgr.Instance.GetGUIEntity("HeroInfoPanel");
            if (entity2 != null)
            {
                HeroInfoPanel panel2 = entity2.Achieve<HeroInfoPanel>();
                if (panel2 != null)
                {
                    panel2.EnableLevUpBtn(true);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SOULBOX, typeof(S2C_SoulBox))]
    public void ReceiveSoulBox(Packet pak)
    {
        <ReceiveSoulBox>c__AnonStorey138 storey = new <ReceiveSoulBox>c__AnonStorey138 {
            res = (S2C_SoulBox) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GameDataMgr.Instance.DirtyActorStage = true;
            foreach (LotteryReward reward in storey.res.rewardList)
            {
                ActorData.getInstance().UpdateNewCard(reward.card);
                ActorData.getInstance().UpdateItem(reward.item);
            }
            ActorData.getInstance().Stone = storey.res.stone;
            ActorData.getInstance().UpdateItem(storey.res.additionalItem);
            if (!storey.res.bTenPick)
            {
                if (storey.res.ticket != null)
                {
                    ActorData.getInstance().UpdateTicketItem(storey.res.ticket);
                }
                if (null != GUIMgr.Instance.GetActivityGUIEntity<SoulBox>())
                {
                    string str = string.Empty;
                    string str2 = string.Empty;
                    string str3 = string.Empty;
                    string str4 = string.Empty;
                    string str5 = string.Empty;
                    foreach (LotteryReward reward2 in storey.res.rewardList)
                    {
                        if ((reward2.item != null) && (reward2.item.entry != -1))
                        {
                            if (!string.IsNullOrEmpty(str3))
                            {
                                str3 = str3 + "|";
                            }
                            if (!string.IsNullOrEmpty(str2))
                            {
                                str2 = str2 + "|";
                            }
                            str2 = str2 + reward2.item.entry.ToString();
                            str3 = str3 + reward2.item.diff.ToString();
                        }
                        foreach (Card card in reward2.card.newCard)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + "|";
                            }
                            str = str + card.cardInfo.entry.ToString();
                        }
                        foreach (Item item in reward2.card.newItem)
                        {
                            int cardExCfgByItemPart = CommonFunc.GetCardExCfgByItemPart(item.entry);
                            if (cardExCfgByItemPart == -1)
                            {
                                if (!string.IsNullOrEmpty(str2))
                                {
                                    str2 = str2 + "|";
                                }
                                if (!string.IsNullOrEmpty(str3))
                                {
                                    str3 = str3 + "|";
                                }
                                str2 = str2 + item.entry.ToString();
                                str3 = str3 + item.diff.ToString();
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    str = str + "|";
                                }
                                str = str + cardExCfgByItemPart.ToString();
                                if (!string.IsNullOrEmpty(str4))
                                {
                                    str4 = str4 + "|";
                                }
                                if (!string.IsNullOrEmpty(str5))
                                {
                                    str5 = str5 + "|";
                                }
                                str4 = str4 + cardExCfgByItemPart.ToString();
                                str5 = str5 + item.diff.ToString();
                            }
                        }
                    }
                    string[] textArray1 = new string[] { str, ",", str2, ",", str3, ",", str4, ",", str5 };
                    GameStateMgr.Instance.ChangeStateWithParameter("COMMUNITY_RECRUIT_EVENT", string.Concat(textArray1));
                }
                else
                {
                    <ReceiveSoulBox>c__AnonStorey137 storey2 = new <ReceiveSoulBox>c__AnonStorey137 {
                        <>f__ref$312 = storey
                    };
                    GUIMgr.Instance.ExitModelGUI("RecruitResultPanel");
                    GameDataMgr.Instance.boostRecruit.HiddenLastResult();
                    storey2.card_list = new List<int>();
                    storey2.item_list = new List<int>();
                    storey2.item_num_list = new List<int>();
                    storey2.morph_list = new List<int>();
                    storey2.morph_num_list = new List<int>();
                    ScheduleMgr.Schedule(0.5f, new System.Action(storey2.<>m__E2));
                }
            }
            else
            {
                Debug.Log(storey.res.rewardList.Count + "-----------------------");
                YuanZhengRushPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<YuanZhengRushPanel>();
                if (activityGUIEntity != null)
                {
                    activityGUIEntity.UpdateData(storey.res.rewardList, storey.res.additionalItem);
                }
                else
                {
                    GUIMgr.Instance.DoModelGUI("YuanZhengRushPanel", new Action<GUIEntity>(storey.<>m__E3), null);
                }
                SoulBox gUIEntity = GUIMgr.Instance.GetGUIEntity<SoulBox>();
                if (null != gUIEntity)
                {
                    gUIEntity.EnableButton(true);
                }
            }
        }
        else if (storey.res.result != OpResult.OpResult_Pay_Stone_S)
        {
            SoulBox box2 = GUIMgr.Instance.GetActivityGUIEntity<SoulBox>();
            if (null != box2)
            {
                box2.EnableButton(true);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETSOULBOXINFO, typeof(S2C_GetSoulBoxInfo))]
    public void ReceiveSoulBoxInfo(Packet pak)
    {
        S2C_GetSoulBoxInfo packetObject = (S2C_GetSoulBoxInfo) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = packetObject.weekly_card_entry;
            char[] separator = new char[] { '|' };
            string[] strArray = packetObject.daily_card_entry.Split(separator);
            List<int> list = new List<int>();
            foreach (string str in strArray)
            {
                list.Add(Convert.ToInt32(str));
            }
            ActorData.getInstance().WeekSoulCardEntry = num;
            ActorData.getInstance().DailySoulCardEntries = list;
            SoulBox activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SoulBox>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.RefreshHero();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_SYNCCHANGEDATA, typeof(S2C_SyncChangeData))]
    public void ReceiveSyncChangeData(Packet pak)
    {
        S2C_SyncChangeData packetObject = (S2C_SyncChangeData) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().PhyForce = packetObject.phyForce;
            ActorData.getInstance().InitAddPhyForce(packetObject.passTime);
            ActorData.getInstance().HaveNewArenaLog = packetObject.arenaLadder;
            ActorData.getInstance().HaveNewArenaChallengeLog = packetObject.lolarena;
            ActorData.getInstance().HaveNewDetainsDartLog = packetObject.convoyEnemy;
        }
    }

    [PacketHandler(OpcodeType.S2C_USEITEM, typeof(S2C_UseItem))]
    public void ReceivetUseItem(Packet pak)
    {
        <ReceivetUseItem>c__AnonStorey114 storey = new <ReceivetUseItem>c__AnonStorey114 {
            res = (S2C_UseItem) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().UpdateItem(storey.res.useItem);
            ActorData.getInstance().UpdateCardList(storey.res.changeCard);
            if (storey.res.gold > 0)
            {
                ActorData.getInstance().Gold = storey.res.gold;
            }
            if (storey.res.changeCard.Count > 0)
            {
                List<long> cardIdList = new List<long> {
                    storey.res.changeCard[0].card_id
                };
                this.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            }
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("BagPanel");
            if ((gUIEntity != null) && !gUIEntity.Hidden)
            {
                BagPanel panel = (BagPanel) gUIEntity;
                List<Item> list = new List<Item> {
                    storey.res.useItem
                };
                panel.UpdateData(list);
            }
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(storey.res.useItem.entry);
            if (_config != null)
            {
                if (_config.type == 4)
                {
                    HeroListPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroListPanel>();
                    if (activityGUIEntity != null)
                    {
                        activityGUIEntity.UpdateExpStat(storey.res.changeCard);
                    }
                    BreakEquipPanel panel3 = GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>();
                    if ((panel3 != null) && (storey.res.changeCard.Count > 0))
                    {
                        panel3.UpdateCardInfo(storey.res.changeCard[0].card_id);
                    }
                }
                else if (_config.type == 8)
                {
                    <ReceivetUseItem>c__AnonStorey113 storey2 = new <ReceivetUseItem>c__AnonStorey113();
                    ActorData.getInstance().TitleList.Add(storey.res.titleData);
                    storey2.utc = ConfigMgr.getInstance().getByEntry<user_title_config>(storey.res.titleData.title_entry);
                    if (storey2.utc != null)
                    {
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey2.<>m__A6), null);
                    }
                }
                else if (_config.type == 9)
                {
                    foreach (Item item in storey.res.reward.items)
                    {
                        ActorData.getInstance().UpdateItem(item);
                    }
                    GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__A7), null);
                    BagPanel panel4 = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
                    if (panel4 != null)
                    {
                        panel4.UpdateUIInfo(panel4.mShowItemType, true);
                    }
                }
                else
                {
                    if (GUIMgr.Instance.GetGUIEntity<BreakEquipPanel>() != null)
                    {
                        GUIMgr.Instance.ExitModelGUI("BreakEquipPanel");
                    }
                    GUIEntity entity2 = GUIMgr.Instance.GetGUIEntity("HeroInfoPanel");
                    if ((entity2 != null) && !entity2.Hidden)
                    {
                        ((HeroInfoPanel) entity2).UpdateEqupInfo(storey.res.changeCard);
                    }
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_VERIFYPERMITCODE, typeof(S2C_VerifyPermitCode))]
    public void ReceiveVerifyPermitCode(Packet pak)
    {
        S2C_VerifyPermitCode packetObject = (S2C_VerifyPermitCode) pak.PacketObject;
        if (packetObject.result == OpResult.OpResult_Verify_Permit_Code_S)
        {
            this.ReSendLastPakLogic(1f);
        }
        else if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            SelectCampPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<SelectCampPanel>();
            if (gUIEntity != null)
            {
                GameDefine.getInstance().AccountIsActiveCode = 1;
                gUIEntity.ShowSelectCampGroup();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERREQ, typeof(S2C_VoidTowerReq))]
    public void ReceiveVoidTower(Packet pak)
    {
        S2C_VoidTowerReq packetObject = (S2C_VoidTowerReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.8f;
            ActorData.getInstance().TowerRemainFightCount = packetObject.data.attack_times;
            if (packetObject.flag != 0)
            {
                if ((packetObject.data.smash_time > 0) && (TimeMgr.Instance.ServerStampTime > packetObject.data.smash_time))
                {
                    this.RequestVoidTowerSmash();
                }
                else
                {
                    TowerPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
                    if (gUIEntity != null)
                    {
                        gUIEntity.UpdateData(packetObject.data);
                    }
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERCOMBAT, typeof(S2C_VoidTowerCombat))]
    public void ReceiveVoidTowerCombat(Packet pak)
    {
        S2C_VoidTowerCombat packetObject = (S2C_VoidTowerCombat) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            void_tower_trench_config _config = ConfigMgr.getInstance().getByEntry<void_tower_trench_config>(packetObject.dupData.trench_entry);
            if (_config != null)
            {
                BattleState.GetInstance().DoNormalBattle(packetObject.combat_data, null, BattleNormalGameType.TowerPk, true, _config.scene, 1, 0, null, packetObject, delegate (bool isWin, BattleNormalGameType _type, BattleNormalGameResult result) {
                    BattleBack data = new BattleBack {
                        result = isWin
                    };
                    this.RequestVoidTowerCombatEnd(data);
                });
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERCOMBATENDREQ, typeof(S2C_VoidTowerCombatEndReq))]
    public void ReceiveVoidTowerCombatEnd(Packet pak)
    {
        <ReceiveVoidTowerCombatEnd>c__AnonStorey13A storeya = new <ReceiveVoidTowerCombatEnd>c__AnonStorey13A {
            res = (S2C_VoidTowerCombatEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeya.res.result))
        {
            ActorData.getInstance().UpdateBattleRewardData(storeya.res.reward);
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storeya.<>m__E6), base.gameObject);
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERREWARD, typeof(S2C_VoidTowerReward))]
    public void ReceiveVoidTowerReward(Packet pak)
    {
        S2C_VoidTowerReward packetObject = (S2C_VoidTowerReward) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().TowerRemainFightCount = packetObject.data.attack_times;
            TowerPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERSMASH, typeof(S2C_VoidTowerSmash))]
    public void ReceiveVoidTowerSmash(Packet pak)
    {
        <ReceiveVoidTowerSmash>c__AnonStorey139 storey = new <ReceiveVoidTowerSmash>c__AnonStorey139 {
            res = (S2C_VoidTowerSmash) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            ActorData.getInstance().UpdateBattleRewardData(storey.res.reward);
            TowerPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(storey.res.data);
            }
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__E4), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_VOIDTOWERSMASHBEGINREQ, typeof(S2C_VoidTowerSmashBeginReq))]
    public void ReceiveVoidTowerSmashBegin(Packet pak)
    {
        S2C_VoidTowerSmashBeginReq packetObject = (S2C_VoidTowerSmashBeginReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            TipsDiag.SetText("开启扫荡....");
            TowerPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<TowerPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdateData(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHREQ, typeof(S2C_WarmmatchReq))]
    public void ReceiveWarmmatch(Packet pak)
    {
        S2C_WarmmatchReq packetObject = (S2C_WarmmatchReq) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ArenaPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdatePlayerList(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHCOMBAT, typeof(S2C_WarmmatchCombat))]
    public void ReceiveWarmmatchCombat(Packet pak)
    {
        S2C_WarmmatchCombat packetObject = (S2C_WarmmatchCombat) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            BattleState.GetInstance().DoNormalBattle(packetObject.combat_data, null, BattleNormalGameType.WarmmatchPk, false, "cj_zc01", 1, 3, null, null, delegate (bool isWin, BattleNormalGameType _type, BattleNormalGameResult result) {
                BattleBack data = new BattleBack {
                    result = isWin
                };
                this.RequestWarmmatchCombatEnd(data);
            });
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHCOMBATENDREQ, typeof(S2C_WarmmatchCombatEndReq))]
    public void ReceiveWarmmatchCombatEnd(Packet pak)
    {
        <ReceiveWarmmatchCombatEnd>c__AnonStorey109 storey = new <ReceiveWarmmatchCombatEnd>c__AnonStorey109 {
            res = (S2C_WarmmatchCombatEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.CloseUniqueGUIEntity("BattlePanel");
            ActorData.getInstance().Courage = storey.res.courage;
            ActorData.getInstance().Gold = storey.res.gold;
            ActorData.getInstance().UpdateBattleRewardData(storey.res.reward);
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__9C), null);
            Debug.Log("ResultPanel------------------>");
        }
        else
        {
            BattleStaticEntry.TryExitBattleOnError();
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHGAINS, typeof(S2C_WarmmatchGains))]
    public void ReceiveWarmmatchGains(Packet pak)
    {
        <ReceiveWarmmatchGains>c__AnonStorey10A storeya = new <ReceiveWarmmatchGains>c__AnonStorey10A {
            res = (S2C_WarmmatchGains) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeya.res.result))
        {
            ArenaPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdatePlayerList(storeya.res.data);
            }
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeya.<>m__9D), null);
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHREFRESH, typeof(S2C_WarmmatchRefresh))]
    public void ReceiveWarmmatchRefresh(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_WarmmatchRefresh packetObject = (S2C_WarmmatchRefresh) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().ArenaRefleshTime = TimeMgr.Instance.ServerStampTime + 0x10;
            ArenaPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<ArenaPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.UpdatePlayerList(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WARMMATCHTARGETREQ, typeof(S2C_WarmmatchTargetReq))]
    public void ReceiveWarmmatchTarget(Packet pak)
    {
        <ReceiveWarmmatchTarget>c__AnonStorey108 storey = new <ReceiveWarmmatchTarget>c__AnonStorey108 {
            res = (S2C_WarmmatchTargetReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            if (ActorData.getInstance().IsOnlyShowTargetTeam)
            {
                GUIMgr.Instance.DoModelGUI("TargetTeamPanel", new Action<GUIEntity>(storey.<>m__99), GUIMgr.Instance.GetGUIEntity<ArenaPanel>().gameObject);
            }
            else
            {
                GUIMgr.Instance.PushGUIEntity("SelectHeroPanel", new Action<GUIEntity>(storey.<>m__9A));
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_GETWHOLEMAILCONTENT, typeof(S2C_GetWholeMail))]
    public void ReceiveWholeMail(Packet pak)
    {
        S2C_GetWholeMail packetObject = (S2C_GetWholeMail) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            MailPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MailPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.SetMailContent(packetObject);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSREQ, typeof(S2C_WorldBossReq))]
    public void ReceiveWorldBoss(Packet pak)
    {
        <ReceiveWorldBoss>c__AnonStorey13B storeyb = new <ReceiveWorldBoss>c__AnonStorey13B {
            res = (S2C_WorldBossReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyb.res.result))
        {
            ActorData.getInstance().WorldBossState = (int) storeyb.res.data.boss.state;
            if (storeyb.res.data.boss.state == WorldBossState.E_WBS_DYING)
            {
                WorldBossPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<WorldBossPanel>();
                if (gUIEntity == null)
                {
                    GUIMgr.Instance.PushGUIEntity("WorldBossPanel", new Action<GUIEntity>(storeyb.<>m__E7));
                }
                else
                {
                    gUIEntity.UpdateDataBossDYing(storeyb.res.data);
                }
            }
            else if (storeyb.res.data.user.pick_reward_flag)
            {
                WorldBossPanel panel2 = GUIMgr.Instance.GetGUIEntity<WorldBossPanel>();
                if (panel2 == null)
                {
                    GUIMgr.Instance.PushGUIEntity("WorldBossPanel", new Action<GUIEntity>(storeyb.<>m__E8));
                }
                else
                {
                    panel2.UpdateBossData(storeyb.res.data);
                }
            }
            else if (storeyb.res.data.boss.state == WorldBossState.E_WBS_ROAR)
            {
                WorldBossPanel panel3 = GUIMgr.Instance.GetGUIEntity<WorldBossPanel>();
                if (panel3 == null)
                {
                    GUIMgr.Instance.PushGUIEntity("WorldBossPanel", new Action<GUIEntity>(storeyb.<>m__E9));
                }
                else
                {
                    panel3.UpdateBossData(storeyb.res.data);
                }
            }
            else
            {
                SMT_TM data = new SMT_TM();
                world_boss_config _config = ConfigMgr.getInstance().getByEntry<world_boss_config>(0);
                CommonFunc.CalOpenTime(_config.start_time, _config.duration, _config.loop, _config.cd, out data);
                if (data.type == SMT_TM.MTTM_Type.TM_OPEN)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037d5));
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037ad));
                }
            }
            Debug.Log("----->ReceiveWorldBoss Succeed");
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSBUYTIMES, typeof(S2C_WorldBossBuyTimes))]
    public void ReceiveWorldBossBuyTimes(Packet pak)
    {
        S2C_WorldBossBuyTimes packetObject = (S2C_WorldBossBuyTimes) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.data.stone;
            WorldBossPanel gUIEntity = (WorldBossPanel) GUIMgr.Instance.GetGUIEntity("WorldBossPanel");
            if (gUIEntity != null)
            {
                gUIEntity.WorldBossData = packetObject.data;
            }
            if (<>f__am$cache3C == null)
            {
                <>f__am$cache3C = delegate (GUIEntity Gobj) {
                    SelectHeroPanel panel = Gobj.Achieve<SelectHeroPanel>();
                    panel.Depth = 600;
                    panel.mBattleType = BattleType.WorldBoss;
                };
            }
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache3C, null);
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSCOMBAT, typeof(S2C_WorldBossCombat))]
    public void ReceiveWorldBossCombat(Packet pak)
    {
        S2C_WorldBossCombat packetObject = (S2C_WorldBossCombat) pak.PacketObject;
        GUIMgr.Instance.UnLock();
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            world_boss_config _config = ConfigMgr.getInstance().getByEntry<world_boss_config>(packetObject.world_data.boss.entry);
            BattleState.GetInstance().DoNormalBattle(packetObject.battle_data, null, BattleNormalGameType.WorldBoss, false, _config.scene, 0, _config.visual_type, null, packetObject, delegate (bool arg1, BattleNormalGameType arg2, BattleNormalGameResult result) {
                BattleBack back = new BattleBack {
                    damage_amount = (ulong) result.worldBossHpChange,
                    result = arg1,
                    deaths = 0,
                    time = 30
                };
                this.RequestWorldBossCombatEnd(back);
            });
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSCOMBATENDREQ, typeof(S2C_WorldBossCombatEndReq))]
    public void ReceiveWorldBossCombatEnd(Packet pak)
    {
        <ReceiveWorldBossCombatEnd>c__AnonStorey13C storeyc = new <ReceiveWorldBossCombatEnd>c__AnonStorey13C {
            res = (S2C_WorldBossCombatEndReq) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyc.res.result))
        {
            ActorData.getInstance().Gold = storeyc.res.world_data.gold;
            GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storeyc.<>m__EB), base.gameObject);
        }
        else
        {
            if (<>f__am$cache3B == null)
            {
                <>f__am$cache3B = obj => ((ResultPanel) obj).UpdateWorldBossKillAleady();
            }
            GUIMgr.Instance.DoModelGUI("ResultPanel", <>f__am$cache3B, base.gameObject);
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSENCOURAGE, typeof(S2C_WorldBossEncourage))]
    public void ReceiveWorldBossEncourage(Packet pak)
    {
        S2C_WorldBossEncourage packetObject = (S2C_WorldBossEncourage) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.data.stone;
            GUIEntity gUIEntity = GUIMgr.Instance.GetGUIEntity("BoxPanel");
            if (gUIEntity != null)
            {
                WorldBossPanel panel = gUIEntity.Achieve<WorldBossPanel>();
                if (panel != null)
                {
                    panel.UpdateEncourageInfo(packetObject.data.user);
                }
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSGAINS, typeof(S2C_WorldBossGains))]
    public void ReceiveWorldBossGains(Packet pak)
    {
        <ReceiveWorldBossGains>c__AnonStorey13D storeyd = new <ReceiveWorldBossGains>c__AnonStorey13D {
            res = (S2C_WorldBossGains) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storeyd.res.result))
        {
            TipsDiag.SetText("领取成功!");
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storeyd.<>m__ED), null);
            WorldBossPanel gUIEntity = (WorldBossPanel) GUIMgr.Instance.GetGUIEntity("WorldBossPanel");
            if (gUIEntity != null)
            {
                gUIEntity.EnableRewardBtn(false);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_WORLDBOSSREBORN, typeof(S2C_WorldBossReborn))]
    public void ReceiveWorldBossReborn(Packet pak)
    {
        S2C_WorldBossReborn packetObject = (S2C_WorldBossReborn) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.data.stone;
            WorldBossPanel gUIEntity = (WorldBossPanel) GUIMgr.Instance.GetGUIEntity("WorldBossPanel");
            if (gUIEntity != null)
            {
                ActorData.getInstance().BossAtkRebornInterval = 0;
                gUIEntity.RebornInterval = 0;
                gUIEntity.UpdateBossData(packetObject.data);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_LIFESKILLINVITEFRIEND, typeof(S2C_LifeSkillInviteFriend))]
    public void RecevieS2C_LifeSkillInviteFriend(Packet pak)
    {
    }

    [PacketHandler(OpcodeType.S2C_CAN_PAY_FLAG, typeof(S2C_Can_Pay_Flag))]
    public void ReciveQueryCaifuTong(Packet pak)
    {
        Debug.Log("-----ReciveQueryCaifuTong------");
        S2C_Can_Pay_Flag packetObject = (S2C_Can_Pay_Flag) pak.PacketObject;
        Debug.Log("LJ ReciveQueryCaifuTong  res.result    :" + packetObject.result);
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            if (packetObject.flag == 0)
            {
                Debug.Log("LJ ReciveQueryCaifuTong  res.discounttype    :" + packetObject.discounttype);
                Debug.Log("LJ ReciveQueryCaifuTong  res.discounturl    :" + packetObject.discounturl);
                PlatformInterface.mInstance.PlatformBindCafuTong(packetObject.discounttype, packetObject.discounturl);
            }
        }
        else if (packetObject.result == OpResult.OpResult_CAN_PAY_FLAG_Wait)
        {
            PayMgr.Instance.StartNextGetPayFlag();
        }
    }

    [PacketHandler(OpcodeType.S2C_QUERYTXBALANCE, typeof(S2C_QueryTxBalance))]
    public void ReciveQueryTxBalance(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_QueryTxBalance packetObject = (S2C_QueryTxBalance) pak.PacketObject;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().LoadingUserInfoProgress = 0.94f;
            if (GameDefine.getInstance().isDebugLog)
            {
                Debug.Log(string.Concat(new object[] { "PAY: ReciveQueryTxBalance ", packetObject.result, " balance: ", packetObject.balance, " vip_level ", packetObject.vip_level.level, " change_stone: ", packetObject.vip_level.change_stone }));
            }
            ActorData.getInstance().Stone = packetObject.balance;
            ActorData.getInstance().SetVIP(packetObject.vip_level);
            Instance.RequestGetMailList();
            ActorData.getInstance().bChargeChange = true;
            Instance.GetActiveList();
        }
    }

    public void RecordNewbieStep(int step)
    {
        Packet pak = new Packet(OpcodeType.C2S_RECORDNEWBIE);
        C2S_RecordNewbie newbie = new C2S_RecordNewbie {
            step = step
        };
        pak.PacketObject = newbie;
        this.Send(pak);
    }

    public void Replay()
    {
        ParkourManager._instance.DestoryParkourAsset();
        this.EnterParkourScene("REPLAY_PARKOUR_EVENT");
    }

    public void RequesArenaLadderBuyAttack(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERBUYATTACK);
        C2S_ArenaLadderBuyAttack attack = new C2S_ArenaLadderBuyAttack {
            use_ticket = use_ticket
        };
        pak.PacketObject = attack;
        this.Send(pak);
    }

    public void RequesArenaLadderCombatHistory()
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERCOMBATHISTORY);
        C2S_ArenaLadderCombatHistory history = new C2S_ArenaLadderCombatHistory();
        pak.PacketObject = history;
        this.Send(pak);
    }

    public void RequesArenaLadderRefreshAttackTime(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERREFRESHATTACKTIME);
        C2S_ArenaLadderRefreshAttackTime time = new C2S_ArenaLadderRefreshAttackTime {
            use_ticket = use_ticket
        };
        pak.PacketObject = time;
        this.Send(pak);
    }

    public void RequesArenaLadderShopBuy(BuyShopItemInfo buyInfo)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERSHOPBUY);
        C2S_ArenaLadderShopBuy buy = new C2S_ArenaLadderShopBuy {
            buyInfo = buyInfo
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void Request_C2S_GetGoblinShopInfo()
    {
        GUIMgr.Instance.Lock();
        C2S_GetGoblinShopInfo info = new C2S_GetGoblinShopInfo();
        Packet pak = new Packet(OpcodeType.C2S_GETGOBLINSHOPINFO) {
            PacketObject = info
        };
        this.Send(pak);
    }

    public void Request_C2S_GetSecretShopInfo()
    {
        GUIMgr.Instance.Lock();
        C2S_GetSecretShopInfo info = new C2S_GetSecretShopInfo();
        Packet pak = new Packet(OpcodeType.C2S_GETSECRETSHOPINFO) {
            PacketObject = info
        };
        this.Send(pak);
    }

    public void Request_C2S_GoblinShopBuy(BuyShopItemInfo buy)
    {
        GUIMgr.Instance.Lock();
        C2S_GoblinShopBuy buy2 = new C2S_GoblinShopBuy {
            buyInfo = buy
        };
        Packet pak = new Packet(OpcodeType.C2S_GOBLINSHOPBUY) {
            PacketObject = buy2
        };
        this.Send(pak);
    }

    public void Request_C2S_GoblinShopFix()
    {
        GUIMgr.Instance.Lock();
        C2S_GoblinShopFix fix = new C2S_GoblinShopFix();
        Packet pak = new Packet(OpcodeType.C2S_GOBLINSHOPFIX) {
            PacketObject = fix
        };
        this.Send(pak);
    }

    public void Request_C2S_GoblinShopRefresh()
    {
        GUIMgr.Instance.Lock();
        C2S_GoblinShopRefresh refresh = new C2S_GoblinShopRefresh();
        Packet pak = new Packet(OpcodeType.C2S_GOBLINSHOPREFRESH) {
            PacketObject = refresh
        };
        this.Send(pak);
    }

    public void Request_C2S_SecretShopBuy(BuyShopItemInfo buy)
    {
        GUIMgr.Instance.Lock();
        C2S_SecretShopBuy buy2 = new C2S_SecretShopBuy {
            buyInfo = buy
        };
        Packet pak = new Packet(OpcodeType.C2S_SECRETSHOPBUY) {
            PacketObject = buy2
        };
        this.Send(pak);
    }

    public void Request_C2S_SecretShopFix()
    {
        GUIMgr.Instance.Lock();
        C2S_SecretShopFix fix = new C2S_SecretShopFix();
        Packet pak = new Packet(OpcodeType.C2S_SECRETSHOPFIX) {
            PacketObject = fix
        };
        this.Send(pak);
    }

    public void Request_C2S_SecretShopRefresh()
    {
        GUIMgr.Instance.Lock();
        C2S_SecretShopRefresh refresh = new C2S_SecretShopRefresh();
        Packet pak = new Packet(OpcodeType.C2S_SECRETSHOPREFRESH) {
            PacketObject = refresh
        };
        this.Send(pak);
    }

    public void RequestAcceptAllFriendPhyForce()
    {
        Packet pak = new Packet(OpcodeType.C2S_ACCEPTALLFRIENDPHYFORCE);
        C2S_AcceptAllFriendPhyForce force = new C2S_AcceptAllFriendPhyForce();
        pak.PacketObject = force;
        this.Send(pak);
    }

    public void RequestAcceptFriendPhyForce(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_ACCEPTFRIENDPHYFORCE);
        C2S_AcceptFriendPhyForce force = new C2S_AcceptFriendPhyForce {
            targetId = targetId
        };
        pak.PacketObject = force;
        this.Send(pak);
    }

    public void RequestAddBroadcast(string content)
    {
        Packet pak = new Packet(OpcodeType.C2S_ADDBROADCAST);
        C2S_AddBroadcast broadcast = new C2S_AddBroadcast {
            content = content
        };
        pak.PacketObject = broadcast;
        Instance.Send(pak);
    }

    public void RequestAddFriend(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_ADDFRIEND);
        C2S_AddFriend friend = new C2S_AddFriend {
            targetId = targetId
        };
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestAgreeAllFriend()
    {
        Packet pak = new Packet(OpcodeType.C2S_AGREEALLFRIEND);
        C2S_AgreeAllFriend friend = new C2S_AgreeAllFriend();
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestAgreeFriend(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_AGREEFRIEND);
        C2S_AddFriend friend = new C2S_AddFriend {
            targetId = targetId
        };
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestAllArenaRank()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETARENAALLRANK);
        C2S_GetArenaAllRank rank = new C2S_GetArenaAllRank();
        pak.PacketObject = rank;
        this.Send(pak);
    }

    public void RequestAllRankInOneList(int rankType)
    {
        Packet pak = new Packet(OpcodeType.C2S_RANKLISTINFO);
        C2S_RankListInfo info = new C2S_RankListInfo {
            type = rankType
        };
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestArenaLadderCombat(long target_id, List<long> cardPosList, int enemyOrder, int enemyType)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERCOMBAT);
        C2S_ArenaLadderCombat combat = new C2S_ArenaLadderCombat {
            cardPosList = cardPosList,
            targetId = target_id,
            enemyOrder = enemyOrder,
            enemyType = enemyType,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestArenaLadderCombatEnd(BattleBack data, long targetId, int targetType, int targetOrder)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERCOMBATEND);
        C2S_ArenaLadderCombatEnd end = new C2S_ArenaLadderCombatEnd {
            battleData = data,
            targetId = targetId,
            targetOrder = targetOrder,
            targetType = targetType
        };
        end.battleData.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        end.battleData.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = end;
        this.Send(pak);
    }

    public void RequestArenaLadderInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERINFO);
        C2S_ArenaLadderInfo info = new C2S_ArenaLadderInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestArenaLadderRankList()
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERRANKLIST);
        C2S_ArenaLadderRankList list = new C2S_ArenaLadderRankList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestArenaLadderShopInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETARENALADDERSHOPINFO);
        C2S_GetArenaLadderShopInfo info = new C2S_GetArenaLadderShopInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestArenaLadderShopRefresh(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_ARENALADDERSHOPREFRESH);
        C2S_ArenaLadderShopRefresh refresh = new C2S_ArenaLadderShopRefresh {
            use_ticket = use_ticket
        };
        pak.PacketObject = refresh;
        this.Send(pak);
    }

    public void RequestBanArenaHero(long card_id, int order)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENADISABLECARD);
        C2S_LoLArenaDisableCard card = new C2S_LoLArenaDisableCard {
            card_id = card_id,
            targetOrder = order
        };
        pak.PacketObject = card;
        this.Send(pak);
    }

    public void RequestBattleEnd(BattleBack data, FlameBattleBack flame_battle_data)
    {
        Packet pak = new Packet(OpcodeType.C2S_FLAMEBATTLEEND);
        C2S_FlameBattleEnd end = new C2S_FlameBattleEnd {
            data = data
        };
        end.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        end.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        end.flame_battle_data = flame_battle_data;
        pak.PacketObject = end;
        this.Send(pak);
    }

    public void RequestBindAccount(long _accountId, string _accountName, string _token, string _mac, PlatformType _type)
    {
        Packet pak = new Packet(OpcodeType.C2S_BINDACCOUNT);
        C2S_BindAccount account = new C2S_BindAccount {
            account_id = _accountId,
            account_name = _accountName,
            platform = _type,
            token = _token,
            mac = _mac
        };
        pak.PacketObject = account;
        this.Send(pak);
    }

    public void RequestBroadcastList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETBROADCASTLIST);
        C2S_GetBroadcastList list = new C2S_GetBroadcastList();
        pak.PacketObject = list;
        Instance.Send(pak);
    }

    public void RequestBuySkillPoint()
    {
        Packet pak = new Packet(OpcodeType.C2S_BUYSKILLPOINT);
        C2S_BuySkillPoint point = new C2S_BuySkillPoint();
        pak.PacketObject = point;
        Instance.Send(pak);
    }

    public void RequestBuyWarmmatchCount(uint times)
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHBUY);
        C2S_WarmmatchBuy buy = new C2S_WarmmatchBuy {
            times = times
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void RequestC2S_AcceptAll_TX_FriendPhyForce()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_ACCEPTALL_TX_FRIENDPHYFORCE) {
            PacketObject = new C2S_AcceptAll_TX_FriendPhyForce()
        };
        this.Send(pak);
    }

    public void RequestC2S_BeginConvoy(int roadId, List<long> cardIdList, long friendId)
    {
        GUIMgr.Instance.Lock();
        C2S_BeginConvoy convoy = new C2S_BeginConvoy {
            lineId = roadId,
            cardIds = cardIdList,
            inviteFriendId = friendId
        };
        Packet pak = new Packet(OpcodeType.C2S_BEGINCONVOY, convoy);
        this.Send(pak);
    }

    public void RequestC2S_BuyConvoyRobTimes()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_BUYCONVOYROBTIMES, new C2S_BuyConvoyRobTimes());
        this.Send(pak);
    }

    public void RequestC2S_Clear_QQFriend_CoolDown(long userID)
    {
        GUIMgr.Instance.Lock();
        Packet packet2 = new Packet(OpcodeType.C2S_CLEAR_QQFRIEND_COOLDOWN);
        C2S_Clear_QQFriend_CoolDown down = new C2S_Clear_QQFriend_CoolDown {
            targetId = userID
        };
        packet2.PacketObject = down;
        Packet pak = packet2;
        this.Send(pak);
    }

    public void RequestC2S_ConvoyAvengeCombat(long playerId, long friendId, List<long> teamCardPosList, int startTime, int convIndex)
    {
        GUIMgr.Instance.Lock();
        C2S_ConvoyAvengeCombat combat = new C2S_ConvoyAvengeCombat {
            targetId = playerId,
            friendId = friendId,
            cardPosList = teamCardPosList,
            start_time = startTime,
            convoyIndex = convIndex
        };
        Packet pak = new Packet(OpcodeType.C2S_CONVOYAVENGECOMBAT, combat);
        this.Send(pak);
    }

    public void RequestC2S_ConvoyAvengeCombatEnd(BattleBack backData)
    {
        GUIMgr.Instance.Lock();
        C2S_ConvoyAvengeCombatEnd end = new C2S_ConvoyAvengeCombatEnd {
            data = backData
        };
        Packet pak = new Packet(OpcodeType.C2S_CONVOYAVENGECOMBATEND, end);
        this.Send(pak);
    }

    public void RequestC2S_ConvoyEnd(int convoyTeamIndex)
    {
        GUIMgr.Instance.Lock();
        C2S_ConvoyEnd end = new C2S_ConvoyEnd {
            convoyIndex = convoyTeamIndex
        };
        Packet pak = new Packet(OpcodeType.C2S_CONVOYEND, end);
        this.Send(pak);
    }

    public void RequestC2S_ConvoyFriendFormation(List<long> curPageFriendList)
    {
        GUIMgr.Instance.Lock();
        C2S_ConvoyFriendFormation formation = new C2S_ConvoyFriendFormation {
            friendIds = curPageFriendList
        };
        Packet pak = new Packet(OpcodeType.C2S_CONVOYFRIENDFORMATION, formation);
        this.Send(pak);
    }

    public void RequestC2S_Cross_AcceptFriendPhyForce(long userid)
    {
        GUIMgr.Instance.Lock();
        C2S_Cross_AcceptFriendPhyForce force = new C2S_Cross_AcceptFriendPhyForce {
            targetId = userid
        };
        Packet pak = new Packet(OpcodeType.C2S_CROSS_ACCEPTFRIENDPHYFORCE) {
            PacketObject = force
        };
        this.Send(pak);
    }

    public void RequestC2S_Cross_GiveFriendPhyForce(long userid)
    {
        GUIMgr.Instance.Lock();
        C2S_Cross_GiveFriendPhyForce force = new C2S_Cross_GiveFriendPhyForce {
            targetId = userid
        };
        Packet pak = new Packet(OpcodeType.C2S_CROSS_GIVEFRIENDPHYFORCE) {
            PacketObject = force
        };
        this.Send(pak);
    }

    public void RequestC2S_GetConvoyEnemyList()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GETCONVOYENEMYLIST, new C2S_GetConvoyEnemyList());
        this.Send(pak);
    }

    public void RequestC2S_GetConvoyInfo()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GETCONVOYINFO, new C2S_GetConvoyInfo());
        this.Send(pak);
    }

    public void RequestC2S_GetGuildDupInfo()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GETGUILDDUPINFO, new C2S_GetGuildDupInfo());
        this.Send(pak);
    }

    public void RequestC2S_GetGuildDupTrenchInfo(int guildDupID)
    {
        GUIMgr.Instance.Lock();
        C2S_GetGuildDupTrenchInfo info = new C2S_GetGuildDupTrenchInfo {
            guildDupId = guildDupID
        };
        Packet pak = new Packet(OpcodeType.C2S_GETGUILDDUPTRENCHINFO, info);
        this.Send(pak);
    }

    public void RequestC2S_GetRobTargetList()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GETROBTARGETLIST, new C2S_GetRobTargetList());
        this.Send(pak);
    }

    public void RequestC2S_GiveAll_TX_FriendPhyForce()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GIVEALL_TX_FRIENDPHYFORCE, new C2S_GiveAll_TX_FriendPhyForce());
        this.Send(pak);
    }

    public void RequestC2S_GuildDupCombatBegin(int guildDupID, int trenchID, List<long> cardPoslist)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDupCombatBegin begin = new C2S_GuildDupCombatBegin {
            cardPosList = cardPoslist,
            guildDupId = guildDupID,
            guildDupTrenchId = trenchID,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPCOMBATBEGIN, begin);
        this.Send(pak);
    }

    public void RequestC2S_GuildDupCombatEnd(BattleBack back, List<int> selfDeadCard, int dupId, int trenchID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDupCombatEnd end = new C2S_GuildDupCombatEnd {
            data = back,
            selfDeadCard = selfDeadCard,
            guildDupId = dupId,
            guildDupTrenchId = trenchID
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPCOMBATEND, end);
        this.Send(pak);
    }

    public void RequestC2S_GuildDupDistributeHistory()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPDISTRIBUTEHISTORY);
        C2S_GuildDupDistributeHistory history = new C2S_GuildDupDistributeHistory();
        pak.PacketObject = history;
        this.Send(pak);
    }

    public void RequestC2S_GuildDupItemQueueInfo(List<int> itemIdList, int duplicateID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDupItemQueueInfo info = new C2S_GuildDupItemQueueInfo {
            items = itemIdList
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPITEMQUEUEINFO, info);
        this.Send(pak);
    }

    public void RequestC2S_GuildDuplicateDamageRank(int duplicateID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDuplicateDamageRank rank = new C2S_GuildDuplicateDamageRank {
            duplicateId = duplicateID
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPLICATEDAMAGERANK, rank);
        this.Send(pak);
    }

    public void RequestC2S_GuildDuplicatePassRank(int dulicateID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDuplicatePassRank rank = new C2S_GuildDuplicatePassRank {
            duplicateId = dulicateID
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPLICATEPASSRANK, rank);
        this.Send(pak);
    }

    public void RequestC2S_GuildDupSupportRank()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPSUPPORTRANK);
        C2S_GuildDupSupportRank rank = new C2S_GuildDupSupportRank();
        pak.PacketObject = rank;
        this.Send(pak);
    }

    public void RequestC2S_GuildDupSupportTrench(int dupId, int trenchId, int supportTimes)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPSUPPORTTRENCH);
        C2S_GuildDupSupportTrench trench = new C2S_GuildDupSupportTrench {
            duplicateId = dupId,
            trenchId = trenchId,
            supportTimes = supportTimes
        };
        pak.PacketObject = trench;
        this.Send(pak);
    }

    public void RequestC2S_GuildDupSupportTrenchRank(int dupId, int trenchId)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPSUPPORTTRENCHRANK);
        C2S_GuildDupSupportTrenchRank rank = new C2S_GuildDupSupportTrenchRank {
            duplicateId = dupId,
            trenchId = trenchId
        };
        pak.PacketObject = rank;
        this.Send(pak);
    }

    public void RequestC2S_GuildTrenchDamageRank(int duplicateID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildTrenchDamageRank rank = new C2S_GuildTrenchDamageRank {
            duplicateId = duplicateID
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDTRENCHDAMAGERANK, rank);
        this.Send(pak);
    }

    public void RequestC2S_GuildWholeSvrTrenchDamageRank(int duplicateID, int trenchID)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildWholeSvrTrenchDamageRank rank = new C2S_GuildWholeSvrTrenchDamageRank {
            duplicateId = duplicateID,
            trenchId = trenchID
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDWHOLESVRTRENCHDAMAGERANK, rank);
        this.Send(pak);
    }

    public void RequestC2S_LifeSkillInviteFriend(long firendID, LifeSkillType type)
    {
    }

    public void RequestC2S_NewLifeFriendReward()
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeFriendReward reward = new C2S_NewLifeFriendReward();
        this.Send(new Packet(OpcodeType.C2S_NEWLIFEFRIENDREWARD, reward));
    }

    public void RequestC2S_NewLifeRecvFriendReward(long targetID, int indexer, bool reall = false, NewFriendType type = 0)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeRecvFriendReward reward = new C2S_NewLifeRecvFriendReward {
            recv_all = reall,
            reward_index = indexer,
            user_type = (int) type,
            targetId = targetID
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFERECVFRIENDREWARD, reward));
    }

    public void RequestC2S_NewLifeSendAndReceiveTimes(NewSkillLifeFriendOpt op_type, long targetID, bool all = false)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSendAndReceiveTimes times = new C2S_NewLifeSendAndReceiveTimes {
            recvall = !all ? 0 : 1,
            opt_type = (int) op_type,
            targetId = targetID
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESENDANDRECEIVETIMES, times));
    }

    public void RequestC2S_NewLifeSkillEndHangUp(NewLifeSkillType entry, NewSkillCellType cellIndex)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillEndHangUp up = new C2S_NewLifeSkillEndHangUp {
            cell_index = (int) cellIndex,
            entry = (int) entry
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLENDHANGUP, up));
    }

    public void RequestC2S_NewLifeSkillGetFriend(NewLifeSkillType type, NewFriendType uType)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillGetFriend friend = new C2S_NewLifeSkillGetFriend {
            entry = (int) type,
            user_type = (int) uType
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLGETFRIEND, friend));
    }

    public void RequestC2S_NewLifeSkillHangUp(NewLifeSkillType entry, NewSkillCellType cellIndex, int cardEntry, int scheme_entry, long friendId = -1)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillHangUp up = new C2S_NewLifeSkillHangUp {
            card_entry = cardEntry,
            cell_index = (int) cellIndex,
            scheme_entry = scheme_entry,
            entry = (int) entry,
            user_id = friendId
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLHANGUP, up));
    }

    public void RequestC2S_NewLifeSkillMapEnter(NewLifeSkillType entry)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillMapEnter enter = new C2S_NewLifeSkillMapEnter {
            entry = (int) entry
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLMAPENTER, enter));
    }

    public void RequestC2S_NewLifeSkillMapSimpleData()
    {
        C2S_NewLifeSkillMapSimpleData data = new C2S_NewLifeSkillMapSimpleData();
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLMAPSIMPLEDATA, data));
    }

    public void RequestC2S_NewLifeSkillProfit(NewLifeSkillType type)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillProfit profit = new C2S_NewLifeSkillProfit {
            entry = (int) type
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLPROFIT, profit));
    }

    public void RequestC2S_NewLifeSkillRandReward(NewLifeSkillType entry, NewSkillCellType cellIndex, int free = 0)
    {
        GUIMgr.Instance.Lock();
        C2S_NewLifeSkillRandReward reward = new C2S_NewLifeSkillRandReward {
            cell_index = (int) cellIndex,
            entry = (int) entry,
            free = free
        };
        this.Send(new Packet(OpcodeType.C2S_NEWLIFESKILLRANDREWARD, reward));
    }

    public void RequestC2S_OpenGuildDup(int guildDupId)
    {
        GUIMgr.Instance.Lock();
        C2S_OpenGuildDup dup = new C2S_OpenGuildDup {
            guildDupId = guildDupId
        };
        Packet pak = new Packet(OpcodeType.C2S_OPENGUILDDUP, dup);
        this.Send(pak);
    }

    public void RequestC2S_QQFriendsInGame_Beyond_Share(long userid)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_QQFRIENDSINGAME_BEYOND_SHARE);
        C2S_QQFriendsInGame_Beyond_Share share = new C2S_QQFriendsInGame_Beyond_Share {
            userid = userid
        };
        pak.PacketObject = share;
        this.Send(pak);
    }

    public void RequestC2S_QQFriendsInGame_Share(long userid)
    {
        GUIMgr.Instance.Lock();
        C2S_QQFriendsInGame_Share share = new C2S_QQFriendsInGame_Share {
            userid = userid
        };
        Packet pak = new Packet(OpcodeType.C2S_QQFRIENDSINGAME_SHARE) {
            PacketObject = share
        };
        this.Send(pak);
    }

    public void RequestC2S_RefreshConvoyFlag()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_REFRESHCONVOYFLAG, new C2S_RefreshConvoyFlag());
        this.Send(pak);
    }

    public void RequestC2S_RefreshConvoyTarget()
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_REFRESHCONVOYTARGET, new C2S_RefreshConvoyTarget());
        this.Send(pak);
    }

    public void RequestC2S_ReleaseGuildDupLock(int guildDupId, int guildDupTrenchId)
    {
        GUIMgr.Instance.Lock();
        C2S_ReleaseGuildDupLock @lock = new C2S_ReleaseGuildDupLock {
            guildDupId = guildDupId,
            guildDupTrenchId = guildDupTrenchId
        };
        Packet pak = new Packet(OpcodeType.C2S_RELEASEGUILDDUPLOCK, @lock);
        this.Send(pak);
    }

    public void RequestC2S_ReqGuildDupItemDistribute(int duplicateID, int reqItemId)
    {
        GUIMgr.Instance.Lock();
        C2S_ReqGuildDupItemDistribute distribute = new C2S_ReqGuildDupItemDistribute {
            itemId = reqItemId,
            duplicateId = duplicateID
        };
        Packet pak = new Packet(OpcodeType.C2S_REQGUILDDUPITEMDISTRIBUTE, distribute);
        this.Send(pak);
    }

    public void RequestC2S_ResetGuildDup(int guildDupId)
    {
        GUIMgr.Instance.Lock();
        C2S_ResetGuildDup dup = new C2S_ResetGuildDup {
            guildDupId = guildDupId
        };
        Packet pak = new Packet(OpcodeType.C2S_RESETGUILDDUP, dup);
        this.Send(pak);
    }

    public void RequestC2S_RobConvoyCombatBegin(long playerId, int teamIndex, ConvoyRobTargetType targetType, List<long> cardIdList, int startTime)
    {
        GUIMgr.Instance.Lock();
        C2S_RobConvoyCombatBegin begin = new C2S_RobConvoyCombatBegin {
            targetId = playerId,
            convoyIndex = teamIndex,
            cardPosList = cardIdList,
            type = targetType,
            start_time = startTime
        };
        Packet pak = new Packet(OpcodeType.C2S_ROBCONVOYCOMBATBEGIN, begin);
        this.Send(pak);
    }

    public void RequestC2S_RobConvoyCombatEnd(long playerId, int teamIndex, BattleBack battleBackData, ConvoyRobTargetType targetType)
    {
        GUIMgr.Instance.Lock();
        C2S_RobConvoyCombatEnd end = new C2S_RobConvoyCombatEnd {
            targetId = playerId,
            convoyIndex = teamIndex,
            data = battleBackData,
            type = targetType
        };
        Packet pak = new Packet(OpcodeType.C2S_ROBCONVOYCOMBATEND, end);
        this.Send(pak);
    }

    public void RequestC2S_TryLockGuildDup(int guildDupId, int guilddupTrenchID)
    {
        GUIMgr.Instance.Lock();
        C2S_TryLockGuildDup dup = new C2S_TryLockGuildDup {
            guildDupId = guildDupId,
            guildDupTrenchId = guilddupTrenchID
        };
        Packet pak = new Packet(OpcodeType.C2S_TRYLOCKGUILDDUP, dup);
        this.Send(pak);
    }

    public void RequestCallengeHistory()
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENACOMBATHISTORY);
        C2S_LoLArenaCombatHistory history = new C2S_LoLArenaCombatHistory();
        pak.PacketObject = history;
        this.Send(pak);
    }

    public void RequestCalPower(List<long> cardIdList, bool needTeamPower, BattleFormationType formationType)
    {
        Packet pak = new Packet(OpcodeType.C2S_CALPOWER);
        C2S_CalPower power = new C2S_CalPower();
        pak.PacketObject = power;
        if (cardIdList == null)
        {
            power.cardIdList = new List<long>();
        }
        else
        {
            power.cardIdList = cardIdList;
        }
        power.needTeamPower = needTeamPower;
        power.formationType = formationType;
        this.Send(pak);
    }

    public void RequestCardBag()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETCARDBAG);
        C2S_GetCardBag bag = new C2S_GetCardBag();
        pak.PacketObject = bag;
        Instance.Send(pak);
    }

    public void RequestCardBreak(long cardId)
    {
        Packet pak = new Packet(OpcodeType.C2S_REQCARDBREAK);
        C2S_ReqCardBreak @break = new C2S_ReqCardBreak();
        pak.PacketObject = @break;
        @break.card_id = cardId;
        Instance.Send(pak);
    }

    public void RequestCardEvolve(long cardId)
    {
        Packet pak = new Packet(OpcodeType.C2S_REQCARDEVOLVE);
        C2S_ReqCardEvolve evolve = new C2S_ReqCardEvolve();
        pak.PacketObject = evolve;
        evolve.card_id = cardId;
        Instance.Send(pak);
    }

    public void RequestCC2S_GuildDupItemManualDistribute(int duplicateID, int reqItemId, long playerId)
    {
        GUIMgr.Instance.Lock();
        C2S_GuildDupItemManualDistribute distribute = new C2S_GuildDupItemManualDistribute {
            itemId = reqItemId,
            targetId = playerId
        };
        Packet pak = new Packet(OpcodeType.C2S_GUILDDUPITEMMANUALDISTRIBUTE, distribute);
        this.Send(pak);
    }

    public void RequestChallengeArenaBuyAttack()
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENABUYATTACK);
        C2S_LoLArenaBuyAttack attack = new C2S_LoLArenaBuyAttack();
        pak.PacketObject = attack;
        this.Send(pak);
    }

    public void RequestChallengeArenaInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENAINFO);
        C2S_LoLArenaInfo info = new C2S_LoLArenaInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestChallengeArenaRankList()
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENARANKLIST);
        C2S_LoLArenaRankList list = new C2S_LoLArenaRankList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestChallengeArenaResetAttackCooldown()
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENAREFRESHATTACKTIME);
        C2S_LoLArenaRefreshAttackTime time = new C2S_LoLArenaRefreshAttackTime();
        pak.PacketObject = time;
        this.Send(pak);
    }

    public void RequestChallengeFormation()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLOLARENAFORMATION);
        C2S_GetLoLArenaFormation formation = new C2S_GetLoLArenaFormation();
        pak.PacketObject = formation;
        this.Send(pak);
    }

    public void RequestChallengeSelfBanInfo(int order)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENABEFORECOMBAT);
        C2S_LoLArenaBeforeCombat combat = new C2S_LoLArenaBeforeCombat {
            targetOrder = order
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestChallengeShopBuy(BuyShopItemInfo info)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENASHOPBUY);
        C2S_LoLArenaShopBuy buy = new C2S_LoLArenaShopBuy {
            buyInfo = info
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void RequestChallengeShopInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLOLARENASHOPINFO);
        C2S_GetLoLArenaShopInfo info = new C2S_GetLoLArenaShopInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestChangeHeadFrame(int headFrameEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_CHANGEHEADFRAME);
        C2S_ChangeHeadFrame frame = new C2S_ChangeHeadFrame {
            headFrameEntry = (short) headFrameEntry
        };
        pak.PacketObject = frame;
        this.Send(pak);
    }

    public void RequestCourageShopBuy(int slot, int entry, int cost, int cost_type)
    {
        Packet pak = new Packet(OpcodeType.C2S_COURAGESHOPBUY);
        C2S_CourageShopBuy buy = new C2S_CourageShopBuy {
            buyInfo = { shopEntry = entry, slot = slot, cost = cost }
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void RequestCourageShopItemList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETCOURAGESHOPINFO);
        C2S_GetCourageShopInfo info = new C2S_GetCourageShopInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestDailyQuest()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETDAILYQUEST);
        C2S_GetDailyQuest quest = new C2S_GetDailyQuest();
        pak.PacketObject = quest;
        this.Send(pak);
    }

    public void RequestDeleteFriend(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_DELETEFRIEND);
        C2S_DeleteFriend friend = new C2S_DeleteFriend {
            targetId = targetId
        };
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestDelMail(List<long> mailIdList, List<long> sysMailIdList)
    {
        Packet pak = new Packet(OpcodeType.C2S_DELETEMAIL);
        C2S_DeleteMail mail = new C2S_DeleteMail {
            mailIdList = mailIdList,
            sysMailIdList = sysMailIdList
        };
        pak.PacketObject = mail;
        this.Send(pak);
    }

    public void RequestDrawLotteryCard(int entry, int option, bool is_niudan, bool use_ticket)
    {
        if (GameDataMgr.Instance.boostRecruit.valid)
        {
            long time = 0L;
            bool flag = GameDataMgr.Instance.boostRecruit.FreeTime(option, out time);
            Packet pak = new Packet(OpcodeType.C2S_DRAWLOTTERYCARD);
            C2S_DrawLotteryCard card = new C2S_DrawLotteryCard {
                lotteryEntry = entry,
                optionEntry = option,
                isFree = flag,
                isCostNiudan = is_niudan,
                use_ticket = use_ticket
            };
            pak.PacketObject = card;
            this.Send(pak);
            GameDataMgr.Instance.boostRecruit.valid = false;
        }
    }

    public void RequestDrawRedPackage(long friendid, string friendname, string friendopenid, int friendType, string myname)
    {
        Packet pak = new Packet(OpcodeType.C2S_DRAWREDPACKAGE);
        C2S_DrawRedPackage package = new C2S_DrawRedPackage {
            friendid = friendid,
            friendname = friendname,
            friendopenid = friendopenid,
            friendType = friendType,
            myname = myname
        };
        pak.PacketObject = package;
        Instance.Send(pak);
    }

    public void RequestDungeonsData()
    {
        Packet pak = new Packet(OpcodeType.C2S_DUNGEONSDATAREQ);
        C2S_DungeonsDataReq req = new C2S_DungeonsDataReq {
            sessionInfo = ActorData.getInstance().SessionInfo
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestDungeonsEnter(int _entry, int _levelEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_DUNGEONSENTER);
        C2S_DungeonsEnter enter = new C2S_DungeonsEnter {
            sessionInfo = ActorData.getInstance().SessionInfo,
            level_entry = _levelEntry,
            entry = _entry
        };
        pak.PacketObject = enter;
        this.Send(pak);
    }

    public void RequestDuplicateBuySmashTimes()
    {
        Packet pak = new Packet(OpcodeType.C2S_DUPLICATEBUYSMASHTIMESREQ);
        C2S_DuplicateBuySmashTimesReq req = new C2S_DuplicateBuySmashTimesReq();
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestDuplicateBuyTimes(DupCommonData _data, int _times)
    {
        Packet pak = new Packet(OpcodeType.C2S_DUPLICATEBUYTIMESREQ);
        C2S_DuplicateBuyTimesReq req = new C2S_DuplicateBuyTimesReq {
            dupData = _data,
            times = _times
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestDuplicateSmash(DupCommonData _dupData, int _times)
    {
        Packet pak = new Packet(OpcodeType.C2S_DUPLICATESMASH);
        C2S_DuplicateSmash smash = new C2S_DuplicateSmash {
            dupData = _dupData,
            times = _times
        };
        pak.PacketObject = smash;
        this.Send(pak);
    }

    public void RequestDupReward(int dupEntry, int dupType, int starNum)
    {
        Packet pak = new Packet(OpcodeType.C2S_PICKDUPLICATEREWARD);
        C2S_PickDuplicateReward reward = new C2S_PickDuplicateReward {
            sessionInfo = ActorData.getInstance().SessionInfo,
            pickStar = starNum,
            duplicateEntry = dupEntry,
            duplicateType = dupType
        };
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestDupRewardInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETDUPLICATEREWARDINFO);
        C2S_GetDuplicateRewardInfo info = new C2S_GetDuplicateRewardInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestEnchaseCardGem(long card_id, int hole_index, int item_entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_ENCHASECARDGEM);
        C2S_EnchaseCardGem gem = new C2S_EnchaseCardGem();
        pak.PacketObject = gem;
        gem.card_id = card_id;
        gem.card_hole_index = hole_index;
        gem.cardgem_entry = item_entry;
        Instance.Send(pak);
    }

    public void RequestEndChallengeArenaCombat(BattleBack data, long enemy_id, int enemy_type, int enemy_order)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENACOMBATEND);
        C2S_LoLArenaCombatEnd end = new C2S_LoLArenaCombatEnd {
            battleData = data,
            targetId = enemy_id,
            targetOrder = enemy_order,
            targetType = enemy_type
        };
        end.battleData.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        end.battleData.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = end;
        this.Send(pak);
    }

    public void RequestEnterDup(int dupEntry, int trenchEntry, DuplicateType _dupType, List<long> cardGUIDs, long _friendUserId)
    {
        Packet pak = new Packet(OpcodeType.C2S_DUPLICATECOMBAT);
        C2S_DuplicateCombat combat = new C2S_DuplicateCombat();
        ActorData.getInstance().OpenNewDup = false;
        combat.dupData.dupEntry = dupEntry;
        combat.dupData.trenchEntry = trenchEntry;
        combat.dupData.dupType = _dupType;
        combat.cardPosList = cardGUIDs;
        combat.friendUserId = _friendUserId;
        combat.start_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestEquipBreak(long cardId, int part)
    {
        Packet pak = new Packet(OpcodeType.C2S_EQUIPBREAK);
        C2S_EquipBreak @break = new C2S_EquipBreak();
        pak.PacketObject = @break;
        @break.card_id = cardId;
        @break.part = part;
        Instance.Send(pak);
    }

    public void RequestEquipBreakQuick(long cardId, int part)
    {
        Packet pak = new Packet(OpcodeType.C2S_EQUIPBREAKQUICK);
        C2S_EquipBreakQuick quick = new C2S_EquipBreakQuick();
        pak.PacketObject = quick;
        quick.card_id = cardId;
        quick.part = part;
        Instance.Send(pak);
    }

    public void RequestEquipLvUp(long _cardId, int _part, bool _auto)
    {
        Packet pak = new Packet(OpcodeType.C2S_EQUIPLVUP);
        C2S_EquipLvUp up = new C2S_EquipLvUp {
            card_id = _cardId,
            part = _part,
            bAuto = _auto
        };
        pak.PacketObject = up;
        Instance.Send(pak);
    }

    public void RequestFlameBattleInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFLAMEBATTLEINFO);
        C2S_GetFlameBattleInfo info = new C2S_GetFlameBattleInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestFlameBattleShopBuy(BuyShopItemInfo buyInfo)
    {
        Packet pak = new Packet(OpcodeType.C2S_FLAMEBATTLESHOPBUY);
        C2S_FlameBattleShopBuy buy = new C2S_FlameBattleShopBuy {
            buyInfo = buyInfo
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void RequestFlameBattleShopInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFLAMEBATTLESHOPINFO);
        C2S_GetFlameBattleShopInfo info = new C2S_GetFlameBattleShopInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestFlameBattleShopRefresh(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_FLAMEBATTLESHOPREFRESH);
        C2S_FlameBattleShopRefresh refresh = new C2S_FlameBattleShopRefresh {
            use_ticket = use_ticket
        };
        pak.PacketObject = refresh;
        this.Send(pak);
    }

    public void RequestFlameBattleSmash()
    {
        Packet pak = new Packet(OpcodeType.C2S_FLAMEBATTLESMASH);
        C2S_FlameBattleSmash smash = new C2S_FlameBattleSmash();
        pak.PacketObject = smash;
        this.Send(pak);
    }

    public void RequestFlameBattleStart(List<long> cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_FLAMEBATTLESTART);
        C2S_FlameBattleStart start = new C2S_FlameBattleStart {
            cardPosList = cardPosList,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        pak.PacketObject = start;
        this.Send(pak);
    }

    public void RequestForceRefreshCourageShop()
    {
        Packet pak = new Packet(OpcodeType.C2S_COURAGESHOPREFRESH);
        C2S_CourageShopRefresh refresh = new C2S_CourageShopRefresh();
        pak.PacketObject = refresh;
        this.Send(pak);
    }

    public void RequestFriendCombat(long targetId, bool isWin)
    {
        Packet pak = new Packet(OpcodeType.C2S_FRIENDCOMBAT);
        C2S_FriendCombat combat = new C2S_FriendCombat {
            targetId = targetId,
            isWin = isWin,
            battleSecurityData = BattleSecurityManager.Instance.GetSecrityData(0)
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestGainFlameBattleReward()
    {
        Packet pak = new Packet(OpcodeType.C2S_GAINFLAMEBATTLEREWARD);
        C2S_GainFlameBattleReward reward = new C2S_GainFlameBattleReward();
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestGameQQFriends()
    {
        Packet pak = new Packet(OpcodeType.C2S_QQFRIENDSINGAME);
        C2S_QQFriendsInGame game = new C2S_QQFriendsInGame();
        pak.PacketObject = game;
        this.Send(pak);
    }

    public void RequestGetAssistUserList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETASSISTUSERLIST);
        C2S_GetAssistUserList list = new C2S_GetAssistUserList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetBattleFormation(BattleFormationType type)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETBATTLEFORMATION);
        C2S_GetBattleFormation formation = new C2S_GetBattleFormation {
            type = type
        };
        pak.PacketObject = formation;
        this.Send(pak);
    }

    public void RequestGetDuplicateProgress()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETDUPLICATEPROGRESS);
        C2S_GetDuplicateProgress progress = new C2S_GetDuplicateProgress();
        pak.PacketObject = progress;
        this.Send(pak);
    }

    public void RequestGetDuplicateRemain(int _dupEntry, DuplicateType _type)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETDUPLICATEREMAIN);
        C2S_GetDuplicateRemain remain = new C2S_GetDuplicateRemain {
            dupEntry = _dupEntry,
            dupType = _type
        };
        pak.PacketObject = remain;
        this.Send(pak);
    }

    public void RequestGetFlameBattleTargetStatus()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFLAMEBATTLETARGETSTATUS);
        C2S_GetFlameBattleTargetStatus status = new C2S_GetFlameBattleTargetStatus();
        pak.PacketObject = status;
        this.Send(pak);
    }

    public void RequestGetFriendFormation(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFRIENDFORMATION);
        C2S_GetFriendFormation formation = new C2S_GetFriendFormation {
            targetId = targetId
        };
        pak.PacketObject = formation;
        this.Send(pak);
    }

    public void RequestGetFriendList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFRIENDLIST);
        C2S_GetFriendList list = new C2S_GetFriendList();
        pak.PacketObject = list;
        Instance.Send(pak);
    }

    public void RequestGetFriendReqList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETFRIENDREQLIST);
        C2S_GetFriendReqList list = new C2S_GetFriendReqList();
        pak.PacketObject = list;
        Instance.Send(pak);
    }

    public void RequestGetGuildList(int offset, bool is_filter)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDLISTREQ);
        C2S_GuildListReq req = new C2S_GuildListReq {
            bias = (uint) offset,
            is_filter = is_filter
        };
        pak.PacketObject = req;
        Instance.Send(pak);
    }

    public void RequestGetHeadFrameList()
    {
        Packet pak = new Packet(OpcodeType.C2S_REQHEADFRAMELIST);
        C2S_GetHeadFrameList list = new C2S_GetHeadFrameList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetItemList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETITEMLIST);
        C2S_GetItemList list = new C2S_GetItemList();
        pak.PacketObject = list;
        Instance.Send(pak);
    }

    public void RequestGetJoinLeague()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETJOINLEAGUE);
        C2S_GetJoinLeague league = new C2S_GetJoinLeague();
        pak.PacketObject = league;
        Instance.Send(pak);
    }

    public void RequestGetLeagueOpponentFormation(int leagueEntry, int groupId, long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLEAGUEOPPONENTFORMATION);
        C2S_GetLeagueOpponentFormation formation = new C2S_GetLeagueOpponentFormation {
            groupId = groupId,
            leagueEntry = leagueEntry,
            targetId = targetId
        };
        pak.PacketObject = formation;
        this.Send(pak);
    }

    public void RequestGetLeagueOpponentList(int leagueEntry, int groupId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLEAGUEOPPONENTLIST);
        C2S_GetLeagueOpponentList list = new C2S_GetLeagueOpponentList {
            groupId = groupId,
            leagueEntry = leagueEntry
        };
        Debug.Log(groupId + " ** " + leagueEntry);
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetLeagueRankList(int leagueEntry, int groupId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLEAGUERANKLIST);
        C2S_GetLeagueRankList list = new C2S_GetLeagueRankList {
            leagueEntry = leagueEntry,
            groupId = groupId
        };
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetLeagueReward()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLEAGUEREWARD);
        C2S_GetLeagueReward reward = new C2S_GetLeagueReward();
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestGetMailList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETMAILLIST);
        C2S_GetMailList list = new C2S_GetMailList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetQuestList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETQUESTLIST);
        C2S_GetQuestList list = new C2S_GetQuestList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetSkillPoint()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETSKILLPOINT);
        C2S_GetSkillPoint point = new C2S_GetSkillPoint();
        pak.PacketObject = point;
        Instance.Send(pak);
    }

    public void RequestGetTitleList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETTITLELIST);
        C2S_GetTitleList list = new C2S_GetTitleList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestGetUserInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETUSERINFO);
        C2S_GetUserInfo info = new C2S_GetUserInfo();
        LoginPanel panel = LoginPanel.G_GetLoginPanel();
        if (panel != null)
        {
            panel.mCheckUserInfo = true;
            ActorData.getInstance().LoadingUserInfoProgress = 0f;
        }
        pak.PacketObject = info;
        Instance.Send(pak);
    }

    public void RequestGiveFriendPhyForce(long targetId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GIVEFRIENDPHYFORCE);
        C2S_GiveFriendPhyForce force = new C2S_GiveFriendPhyForce {
            targetId = targetId
        };
        pak.PacketObject = force;
        this.Send(pak);
    }

    public void RequestGmCommand(string cmd)
    {
        if (!this.LocalCommand(cmd))
        {
            Packet pak = new Packet(OpcodeType.C2S_GMCOMMAND);
            C2S_GmCommand command = new C2S_GmCommand {
                command = cmd.ToLower()
            };
            pak.PacketObject = command;
            this.Send(pak);
        }
    }

    public void RequestGuildApplication()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDAPPLICATIONREQ);
        C2S_GuildApplicationReq req = new C2S_GuildApplicationReq();
        pak.PacketObject = req;
        Instance.Send(pak);
    }

    public void RequestGuildApplicationProcess(long _id, bool _result)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDAPPLICATIONPROCESS);
        C2S_GuildApplicationProcess process = new C2S_GuildApplicationProcess {
            user_id = _id,
            result = _result
        };
        pak.PacketObject = process;
        Instance.Send(pak);
    }

    public void RequestGuildApply(long _guildId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDAPPLY);
        C2S_GuildApply apply = new C2S_GuildApply {
            guild_id = _guildId
        };
        pak.PacketObject = apply;
        Instance.Send(pak);
    }

    public void RequestGuildBattleBuyTimes()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLEBUYATTACKTIMES);
        C2S_GuildBattleBuyAttackTimes times = new C2S_GuildBattleBuyAttackTimes();
        pak.PacketObject = times;
        Instance.Send(pak);
    }

    public void RequestGuildBattleCombat(int resoureEntry, long targetId, List<long> cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLECOMBAT);
        C2S_GuildBattleCombat combat = new C2S_GuildBattleCombat {
            resourceEntry = resoureEntry,
            targetId = targetId,
            cardPosList = cardPosList
        };
        pak.PacketObject = combat;
        Instance.Send(pak);
    }

    public void RequestGuildBattleCombatEnd(BattleBack _BattleBack, long targetid, int resEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLECOMBATEND);
        C2S_GuildBattleCombatEnd end = new C2S_GuildBattleCombatEnd {
            battleData = _BattleBack,
            targetId = targetid,
            resourceEntry = resEntry
        };
        end.battleData.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        end.battleData.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = end;
        Instance.Send(pak);
    }

    public void RequestGuildBattleDamageRank()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLEDAMAGERANK);
        C2S_GuildBattleDamageRank rank = new C2S_GuildBattleDamageRank();
        pak.PacketObject = rank;
        Instance.Send(pak);
    }

    public void RequestGuildBattleInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLEINFO);
        C2S_GuildBattleInfo info = new C2S_GuildBattleInfo();
        pak.PacketObject = info;
        Instance.Send(pak);
    }

    public void RequestGuildBattleMarsRank()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLEWINTIMESRANK);
        C2S_GuildBattleWinTimesRank rank = new C2S_GuildBattleWinTimesRank();
        pak.PacketObject = rank;
        Instance.Send(pak);
    }

    public void RequestGuildBattleResourceRank()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBATTLERESOURCERANK);
        C2S_GuildBattleResourceRank rank = new C2S_GuildBattleResourceRank();
        pak.PacketObject = rank;
        Instance.Send(pak);
    }

    public void RequestGuildBindQQInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_QUERYGUILDBINDINFO);
        C2S_QueryGuildBindInfo info = new C2S_QueryGuildBindInfo();
        pak.PacketObject = info;
        Instance.Send(pak);
    }

    public void RequestGuildBufferLevUp(int _bufferEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBUFFLEVELUP);
        C2S_GuildBuffLevelUp up = new C2S_GuildBuffLevelUp {
            buff_entry = _bufferEntry
        };
        pak.PacketObject = up;
        Instance.Send(pak);
    }

    public void RequestGuildBuildLevUp(GuildBuild _type)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBUILDLEVELUP);
        C2S_GuildBuildLevelup levelup = new C2S_GuildBuildLevelup {
            entry = (int) _type
        };
        pak.PacketObject = levelup;
        Instance.Send(pak);
    }

    public void RequestGuildBuy(int _index)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDBUYGOODS);
        C2S_GuildBuyGoods goods = new C2S_GuildBuyGoods {
            entry = _index
        };
        pak.PacketObject = goods;
        Instance.Send(pak);
    }

    public void RequestGuildContribute(int Times)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDCONTRIBUTE);
        C2S_GuildContribute contribute = new C2S_GuildContribute {
            times = Times
        };
        pak.PacketObject = contribute;
        Instance.Send(pak);
    }

    public void RequestGuildCreate(string _name, CostType _type)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDCREATE);
        C2S_GuildCreate create = new C2S_GuildCreate {
            name = _name,
            type = _type
        };
        pak.PacketObject = create;
        Instance.Send(pak);
    }

    public void RequestGuildData(bool _OpenUI, System.Action _fsmAction = null)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDDATAREQ);
        C2S_GuildDataReq req = new C2S_GuildDataReq();
        ActorData.getInstance().bOpenUI = _OpenUI;
        pak.PacketObject = req;
        this.FSMAction = _fsmAction;
        Instance.Send(pak);
    }

    public void RequestGuildDismiss()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDDISMISS);
        C2S_GuildDismiss dismiss = new C2S_GuildDismiss();
        pak.PacketObject = dismiss;
        Instance.Send(pak);
    }

    public void RequestGuildExpelMember(long _id)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDEXPELMEMBER);
        C2S_GuildExpelMember member = new C2S_GuildExpelMember {
            user_id = _id
        };
        pak.PacketObject = member;
        Instance.Send(pak);
    }

    public void RequestGuildJoinQQGroup()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETJOINQQGROUPKEY);
        C2S_GetJoinQQGroupKey key = new C2S_GetJoinQQGroupKey();
        pak.PacketObject = key;
        Instance.Send(pak);
    }

    public void RequestGuildQuit()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDQUIT);
        C2S_GuildQuit quit = new C2S_GuildQuit();
        pak.PacketObject = quit;
        Instance.Send(pak);
    }

    public void RequestGuildRefresh()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDREFRESHGOODS);
        C2S_GuildRefreshGoods goods = new C2S_GuildRefreshGoods();
        pak.PacketObject = goods;
        Instance.Send(pak);
    }

    public void RequestGuildSearch(long _guildId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDSEARCH);
        C2S_GuildSearch search = new C2S_GuildSearch {
            guild_id = _guildId
        };
        pak.PacketObject = search;
        Instance.Send(pak);
    }

    public void RequestGuildSelectBuf(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDSELECTBUFF);
        C2S_GuildSelectBuff buff = new C2S_GuildSelectBuff {
            entry = _entry
        };
        pak.PacketObject = buff;
        Instance.Send(pak);
    }

    public void RequestGuildSetNotice(string _notice, int _ApplyType, int _ApplyLv)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDSETNOITCE);
        C2S_GuildSetNoitce noitce = new C2S_GuildSetNoitce {
            notice = _notice,
            apply_type = _ApplyType,
            apply_level = _ApplyLv
        };
        pak.PacketObject = noitce;
        Instance.Send(pak);
    }

    public void RequestGuildTransfer(long _id)
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDTRANSFER);
        C2S_GuildTransfer transfer = new C2S_GuildTransfer {
            user_id = _id
        };
        pak.PacketObject = transfer;
        Instance.Send(pak);
    }

    public void RequestGuildUnBindQQGroup()
    {
        Packet pak = new Packet(OpcodeType.C2S_GUILDUNBINDQQGROUP);
        C2S_GuildUnBindQQGroup group = new C2S_GuildUnBindQQGroup();
        pak.PacketObject = group;
        Instance.Send(pak);
    }

    public void RequestItemMachining(int type, int entry)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_ITEMMACHINING);
        C2S_ItemMachining machining = new C2S_ItemMachining();
        pak.PacketObject = machining;
        machining.type = type;
        machining.entry = entry;
        Instance.Send(pak);
    }

    public void RequestLeagueApply(int leagueEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_LEAGUEAPPLY);
        C2S_LeagueApply apply = new C2S_LeagueApply {
            leagueEntry = leagueEntry
        };
        pak.PacketObject = apply;
        Instance.Send(pak);
    }

    public void RequestLeagueFight(int leagueEntry, int groupId, long targetId, bool isCostStone, bool isWin)
    {
        Packet pak = new Packet(OpcodeType.C2S_LEAGUEFIGHT);
        C2S_LeagueFight fight = new C2S_LeagueFight {
            targetId = targetId,
            isWin = isWin,
            groupId = groupId,
            leagueEntry = leagueEntry,
            type = PVPCombatType.E_COMBAT_NORMAL
        };
        pak.PacketObject = fight;
        this.Send(pak);
    }

    public void RequestLeagueHistory()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLEAGUEHISTORY);
        C2S_GetLeagueHistory history = new C2S_GetLeagueHistory();
        pak.PacketObject = history;
        this.Send(pak);
    }

    public void RequestLivenessAward()
    {
        Packet pak = new Packet(OpcodeType.C2S_PICKLIVENESSREWARD);
        C2S_PickLivenessReward reward = new C2S_PickLivenessReward();
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestLivenessList()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLIVENESSLIST);
        C2S_GetLivenessList list = new C2S_GetLivenessList();
        pak.PacketObject = list;
        this.Send(pak);
    }

    public void RequestLogin(long acctId, string acctName, string deviceMAC, PlatformType platform, string token, int childPlatform, string sessionId, bool use_macaddr_login)
    {
        Debug.Log(string.Concat(new object[] { "Login: acc_name:", acctName, " acct_id:", acctId, " mac:", deviceMAC, " token:", token, " platform:", platform }));
        Packet pak = new Packet(OpcodeType.C2S_LOGIN);
        C2S_Login login = new C2S_Login {
            account_name = acctName,
            account_id = acctId,
            mac = deviceMAC,
            platform = platform,
            token = token,
            childPlatform = childPlatform,
            use_macaddr_login = use_macaddr_login,
            sessionId = sessionId,
            channel_id = GameDefine.getInstance().tx_channelId,
            idfa = GameDefine.getInstance().GetIOSIDFA()
        };
        pak.PacketObject = login;
        this.Send(pak);
    }

    public void RequestLotteryCardInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETLOTTERYCARDINFO);
        C2S_GetLotteryCardInfo info = new C2S_GetLotteryCardInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestModifyNickName(string name)
    {
        Packet pak = new Packet(OpcodeType.C2S_MODIFYNICKNAME);
        C2S_ModifyNickName name2 = new C2S_ModifyNickName {
            name = name
        };
        pak.PacketObject = name2;
        Instance.Send(pak);
    }

    public void RequestNewDungeonsCombat(List<long> _list, DupCommonData _DupData)
    {
        Packet pak = new Packet(OpcodeType.C2S_NEWDUNGEONSCOMBATREQ);
        C2S_NewDungeonsCombatReq req = new C2S_NewDungeonsCombatReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            cardPosList = _list,
            dupData = _DupData,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestNewDungeonsCombatEnd(DupCommonData _dupData, BattleBack _bbData)
    {
        Packet pak = new Packet(OpcodeType.C2S_NEWDUNGEONSCOMBATENDREQ);
        C2S_NewDungeonsCombatEndReq req = new C2S_NewDungeonsCombatEndReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            dupData = _dupData,
            data = _bbData
        };
        req.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        req.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOneKeyEquipLevUp(long _cardId)
    {
        Packet pak = new Packet(OpcodeType.C2S_ONEKEYEQUIPLVUP);
        C2S_OneKeyEquipLvUp up = new C2S_OneKeyEquipLvUp {
            card_id = _cardId
        };
        pak.PacketObject = up;
        Instance.Send(pak);
    }

    public void RequestOpenBoxOpen(OpenBoxType _OpenType, OpenBoxPayType _PayType)
    {
        Packet pak = new Packet(OpcodeType.C2S_OPENBOXOPEN);
        C2S_OpenBoxOpen open = new C2S_OpenBoxOpen {
            open_type = _OpenType,
            pay_type = _PayType
        };
        pak.PacketObject = open;
        this.Send(pak);
    }

    public void RequestOpenBoxReq()
    {
        Packet pak = new Packet(OpcodeType.C2S_OPENBOXREQ);
        C2S_OpenBoxReq req = new C2S_OpenBoxReq();
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandBufferEventReq(int _entry, int _map_entry, int _point)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDBUFFEREVENTREQ);
        C2S_OutlandBufferEventReq req = new C2S_OutlandBufferEventReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandCombatEndReq(int _entry, int _map_entry, int _point, BattleBack data, OutlandBattleBack result, BattleGridGameMapControl gameMapControl)
    {
        this.battleGridGameMapControl = gameMapControl;
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDCOMBATENDREQ);
        C2S_OutlandCombatEndReq req = new C2S_OutlandCombatEndReq();
        this.resulttBattleBack = result;
        req.sessionInfo = ActorData.getInstance().SessionInfo;
        req.entry = _entry;
        req.map_entry = _map_entry;
        req.point = _point;
        req.data = data;
        req.outland_battle_data = result;
        req.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        req.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandCombatReq(int _entry, int _map_entry, int _point, List<long> _cardPosList, long _friendUserId)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDCOMBATREQ);
        C2S_OutlandCombatReq req = new C2S_OutlandCombatReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point,
            cardPosList = _cardPosList,
            friendUserId = _friendUserId,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandCreateMapReq(int entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDCREATEMAPREQ);
        C2S_OutlandCreateMapReq req = new C2S_OutlandCreateMapReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = entry
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandDataEnter(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDDATAENTER);
        C2S_OutlandDataEnter enter = new C2S_OutlandDataEnter();
        this._newEntry = _entry;
        enter.sessionInfo = ActorData.getInstance().SessionInfo;
        enter.entry = _entry;
        pak.PacketObject = enter;
        this.Send(pak);
    }

    public void RequestOutlandDropEvent(int _entry, int _map_entry, int _point)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDDROPEVENT);
        C2S_OutlandDropEvent event2 = new C2S_OutlandDropEvent {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point
        };
        pak.PacketObject = event2;
        this.Send(pak);
    }

    public void RequestOutlandEnterNextFloorReq(int _entry, int _map_entry, int _point)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDENTERNEXTFLOORREQ);
        C2S_OutlandEnterNextFloorReq req = new C2S_OutlandEnterNextFloorReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandGetFloorBoxReward(int _entry, int _map_entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDGETFLOORBOXREWARD);
        C2S_OutlandGetFloorBoxReward reward = new C2S_OutlandGetFloorBoxReward {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry
        };
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestOutlandGetMonsterInfo(int _entry, int _map_entry, int _point)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDGETMONSTERINFO);
        C2S_OutlandGetMonsterInfo info = new C2S_OutlandGetMonsterInfo {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point
        };
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestOutlandGetRewardReq(int _entry, int _map_entry, int _point)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDGETREWARDREQ);
        C2S_OutlandGetRewardReq req = new C2S_OutlandGetRewardReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            map_entry = _map_entry,
            point = _point
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandPageMapReq(int entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDPAGEMAPREQ);
        C2S_OutlandPageMapReq req = new C2S_OutlandPageMapReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = entry
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestOutlandQuit(int _entry, int _map_entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDQUIT);
        C2S_OutlandQuit quit = new C2S_OutlandQuit {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry
        };
        ActorData.getInstance().outlandEntry = _entry;
        quit.map_entry = _map_entry;
        pak.PacketObject = quit;
        this.Send(pak);
    }

    public void RequestOutlandsData(System.Action action = null, System.Action loading_action = null)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDDATAREQ);
        C2S_OutlandDataReq req = new C2S_OutlandDataReq {
            sessionInfo = ActorData.getInstance().SessionInfo
        };
        pak.PacketObject = req;
        this.EnterAction = action;
        this.LoadingAction = loading_action;
        this.Send(pak);
    }

    public void RequestOutlandShopBuy(BuyShopItemInfo _buyInfo)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDSHOPBUY);
        C2S_OutlandShopBuy buy = new C2S_OutlandShopBuy {
            sessionInfo = ActorData.getInstance().SessionInfo,
            buyInfo = _buyInfo
        };
        pak.PacketObject = buy;
        this.Send(pak);
    }

    public void RequestOutlandShopInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDSHOPINFO);
        C2S_OutlandShopInfo info = new C2S_OutlandShopInfo {
            sessionInfo = ActorData.getInstance().SessionInfo
        };
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestOutlandShopRefresh(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDSHOPREFRESH);
        C2S_OutlandShopRefresh refresh = new C2S_OutlandShopRefresh {
            sessionInfo = ActorData.getInstance().SessionInfo,
            use_ticket = use_ticket
        };
        pak.PacketObject = refresh;
        this.Send(pak);
    }

    public void RequestOutlandSweepReq(int entry, bool bSweepAll = false)
    {
        Packet pak = new Packet(OpcodeType.C2S_OUTLANDSWEEPREQ);
        C2S_OutlandSweepReq req = new C2S_OutlandSweepReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = entry,
            sweepAll = bSweepAll
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestParkourEnd(GuildParkour guildParkour)
    {
        Packet pak = new Packet(OpcodeType.C2S_PARKOUREND);
        C2S_ParkourEnd end = new C2S_ParkourEnd {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = ActorData.getInstance().paokuMapEntry,
            data = guildParkour,
            map_index = ActorData.getInstance().paokuPropIndex
        };
        pak.PacketObject = end;
        Instance.Send(pak);
    }

    public void RequestParkourReset(int _entry, int _cardEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_PARKOURRESET);
        C2S_ParkourReset reset = new C2S_ParkourReset {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            card_entry = _cardEntry,
            map_index = ActorData.getInstance().paokuPropIndex
        };
        pak.PacketObject = reset;
        Instance.Send(pak);
    }

    public void RequestParkourRevive(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_PARKOURREVIVE);
        C2S_ParkourRevive revive = new C2S_ParkourRevive {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            card_entry = ActorData.getInstance().paokuCardEntry,
            map_index = ActorData.getInstance().paokuPropIndex
        };
        pak.PacketObject = revive;
        Instance.Send(pak);
    }

    public void RequestParkourStart(int _entry, int _cardEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_PARKOURSTART);
        C2S_ParkourStart start = new C2S_ParkourStart {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            card_entry = _cardEntry
        };
        pak.PacketObject = start;
        Instance.Send(pak);
    }

    public void RequestPickGift(string giftCode)
    {
        Packet pak = new Packet(OpcodeType.C2S_PICKGIFT);
        C2S_PickGift gift = new C2S_PickGift {
            giftCode = giftCode
        };
        pak.PacketObject = gift;
        this.Send(pak);
    }

    public void RequestPickLeagueReward(LeagueRewardType type)
    {
        Packet pak = new Packet(OpcodeType.C2S_PICKLEAGUEREWARD);
        C2S_PickLeagueReward reward = new C2S_PickLeagueReward {
            type = type
        };
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestPickMailAffix(long sysMailId)
    {
        Packet pak = new Packet(OpcodeType.C2S_PICKMAILAFFIX);
        C2S_PickMailAffix affix = new C2S_PickMailAffix {
            sysMailId = sysMailId
        };
        pak.PacketObject = affix;
        this.Send(pak);
    }

    public void RequestPrepareFriendCombat(List<long> cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_PREPAREFRIENDCOMBAT);
        C2S_PrepareFriendCombat combat = new C2S_PrepareFriendCombat {
            cardPosList = cardPosList
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestPrepareLeagueFight(int leagueEntry, int groupId, long targetId, List<long> cardPosList, int targetRank, int myRank, bool isCostStone)
    {
        Packet pak = new Packet(OpcodeType.C2S_PREPARELEAGUEFIGHT);
        C2S_PrepareLeagueFight fight = new C2S_PrepareLeagueFight {
            cardPosList = cardPosList,
            targetRank = targetRank,
            leagueEntry = leagueEntry,
            groupId = groupId,
            targetId = targetId,
            isCostStone = isCostStone,
            myRank = myRank
        };
        pak.PacketObject = fight;
        Debug.Log(leagueEntry + ":::" + groupId);
        this.Send(pak);
    }

    public void RequestQuitDup(bool isWin, int _deadNumbers)
    {
        Packet pak = new Packet(OpcodeType.C2S_DUPLICATEENDREQ);
        C2S_DuplicateEndReq req = new C2S_DuplicateEndReq {
            data = { result = isWin, deaths = _deadNumbers, securityDataList = BattleSecurityManager.Instance.GetSecurityDataList(), end_time = TimeMgr.Instance.ServerStampTime }
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestRedPackageRecord()
    {
        Packet pak = new Packet(OpcodeType.C2S_REDPACKAGERECORD);
        C2S_RedPackageRecord record = new C2S_RedPackageRecord();
        pak.PacketObject = record;
        Instance.Send(pak);
    }

    public void RequestRefreshChallengeShop(bool use_ticket = false)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENASHOPREFRESH);
        C2S_LoLArenaShopRefresh refresh = new C2S_LoLArenaShopRefresh {
            use_ticket = use_ticket
        };
        pak.PacketObject = refresh;
        this.Send(pak);
    }

    public void RequestRefreshFlameBattleTarget()
    {
        Packet pak = new Packet(OpcodeType.C2S_REFRESHFLAMEBATTLETARGET);
        C2S_RefreshFlameBattleTarget target = new C2S_RefreshFlameBattleTarget();
        pak.PacketObject = target;
        this.Send(pak);
    }

    public void RequestRefreshQQFriendInGame()
    {
        GUIMgr.Instance.Lock();
        C2S_QQFriendsInGame_Fresh fresh = new C2S_QQFriendsInGame_Fresh();
        Packet pak = new Packet(OpcodeType.C2S_QQFRIENDSINGAME_FRESH) {
            PacketObject = fresh
        };
        this.Send(pak);
    }

    public void RequestRefuseFriend(long targetId, bool refuseAll = false)
    {
        Packet pak = new Packet(OpcodeType.C2S_REFUSEFRIEND);
        C2S_RefuseFriend friend = new C2S_RefuseFriend {
            targetId = targetId,
            refuseAll = refuseAll
        };
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestRegister(string acctName, long acctId, PlatformType platform, string mac, string name, int headicon, int camp, int role_entry, bool isUseMac)
    {
        acctName = acctName.Trim();
        Packet pak = new Packet(OpcodeType.C2S_REGISTER);
        C2S_Register register = new C2S_Register {
            platform = platform,
            account_name = acctName,
            account_id = acctId,
            platform = platform,
            mac = mac,
            use_macaddr_login = isUseMac,
            channel_id = GameDefine.getInstance().tx_channelId,
            imei = SystemInfo.deviceUniqueIdentifier,
            idfa = GameDefine.getInstance().GetIOSIDFA(),
            real_mac_addr = string.Empty
        };
        string[] textArray1 = new string[] { SystemInfo.deviceName, "|", SystemInfo.deviceType.ToString(), "|", SystemInfo.deviceModel };
        register.device_type = string.Concat(textArray1);
        register.name = name;
        register.headicon = headicon;
        register.camp = camp;
        register.role_entry = role_entry;
        GuideSimulate_Battle.is_lm = 1 == camp;
        pak.PacketObject = register;
        this.Send(pak);
    }

    public void RequestRegistration()
    {
        Packet pak = new Packet(OpcodeType.C2S_REGISTRATION);
        C2S_Registration registration = new C2S_Registration();
        pak.PacketObject = registration;
        this.Send(pak);
    }

    public void RequestRemoveCardGem(long card_id, int hole_index)
    {
        Packet pak = new Packet(OpcodeType.C2S_REMOVECARDGEM);
        C2S_RemoveCardGem gem = new C2S_RemoveCardGem();
        pak.PacketObject = gem;
        gem.card_id = card_id;
        gem.card_hole_index = hole_index;
        Instance.Send(pak);
    }

    public void RequestResetFlameBattle(bool use_ticket)
    {
        Packet pak = new Packet(OpcodeType.C2S_RESETFLAMEBATTLE);
        C2S_ResetFlameBattle battle = new C2S_ResetFlameBattle {
            use_ticket = use_ticket
        };
        pak.PacketObject = battle;
        this.Send(pak);
    }

    public void RequestRewardFlag()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETNEWREWARDFLAG);
        C2S_GetNewRewardFlag flag = new C2S_GetNewRewardFlag();
        pak.PacketObject = flag;
        this.Send(pak);
    }

    public void RequestRuneStonePurchase(RunestonePurchaseType _type, int _param)
    {
        ActorData.getInstance().IsSendPak = true;
        Packet pak = new Packet(OpcodeType.C2S_RUNESTONEPURCHASE);
        C2S_RunestonePurchase purchase = new C2S_RunestonePurchase {
            type = _type,
            param = (uint) _param
        };
        pak.PacketObject = purchase;
        this.Send(pak);
    }

    public void RequestSearchFriend(bool isByIdOrName, long targetId, string targetName)
    {
        Packet pak = new Packet(OpcodeType.C2S_SEARCHFRIEND);
        C2S_SearchFriend friend = new C2S_SearchFriend {
            isByIdOrName = isByIdOrName,
            targetId = targetId,
            targetName = targetName
        };
        pak.PacketObject = friend;
        this.Send(pak);
    }

    public void RequestSellItem(List<Item> items)
    {
        Packet pak = new Packet(OpcodeType.C2S_SELLITEM);
        C2S_SellItem item = new C2S_SellItem();
        pak.PacketObject = item;
        item.items = items;
        Instance.Send(pak);
    }

    public void RequestSendMail(long targetId, string content)
    {
        Packet pak = new Packet(OpcodeType.C2S_SENDMAIL);
        C2S_SendMail mail = new C2S_SendMail {
            targetId = targetId,
            content = content
        };
        pak.PacketObject = mail;
        this.Send(pak);
    }

    public void RequestSetBattleMember(List<long> memberId, BattleFormationType type)
    {
        Packet pak = new Packet(OpcodeType.C2S_SETBATTLEMEMBER);
        C2S_SetBattleMember member = new C2S_SetBattleMember {
            memberId = memberId,
            type = type
        };
        pak.PacketObject = member;
        this.Send(pak);
    }

    public void RequestSetChallengeArenaBattleMember(List<long> id_list)
    {
        Packet pak = new Packet(OpcodeType.C2S_SETLOLARENAMEMBER);
        C2S_SetLoLArenaMember member = new C2S_SetLoLArenaMember {
            memberId = id_list
        };
        pak.PacketObject = member;
        this.Send(pak);
    }

    public void RequestSetHead(int headEntry)
    {
        Packet pak = new Packet(OpcodeType.C2S_SETHEAD);
        C2S_SetHead head = new C2S_SetHead {
            headEntry = headEntry
        };
        pak.PacketObject = head;
        Instance.Send(pak);
    }

    public void RequestSetSignature(string signature)
    {
        Packet pak = new Packet(OpcodeType.C2S_SETSIGNATURE);
        C2S_SetSignature signature2 = new C2S_SetSignature {
            signature = signature
        };
        pak.PacketObject = signature2;
        Instance.Send(pak);
    }

    public void RequestSetUserTitle(int title_entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_SETUSERTITLE);
        C2S_SetUserTitle title = new C2S_SetUserTitle {
            title_entry = title_entry
        };
        pak.PacketObject = title;
        Instance.Send(pak);
    }

    public void RequestShakeGoldTree(int shakecount)
    {
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_SHAKEGOLDTREE);
        C2S_ShakeGoldTree tree = new C2S_ShakeGoldTree {
            shake_count = shakecount
        };
        pak.PacketObject = tree;
        Instance.Send(pak);
    }

    public void RequestSkillLvUp(long cardId, int skillIndex)
    {
        Packet pak = new Packet(OpcodeType.C2S_REQCARDSKILLLVLUP);
        C2S_ReqCardSkillLvlUp up = new C2S_ReqCardSkillLvlUp {
            card_id = cardId,
            skillIndex = skillIndex
        };
        pak.PacketObject = up;
        Instance.Send(pak);
    }

    public void RequestSoulBox(bool use_ticket, bool bTenPick)
    {
        Packet pak = new Packet(OpcodeType.C2S_SOULBOX);
        C2S_SoulBox box = new C2S_SoulBox {
            use_ticket = use_ticket,
            bTenPick = bTenPick
        };
        pak.PacketObject = box;
        this.Send(pak);
        RecruitPanel.actived_function = RecruitPanel.function.soul;
    }

    public void RequestSoulBoxInfo()
    {
        Packet pak = new Packet(OpcodeType.C2S_GETSOULBOXINFO);
        C2S_GetSoulBoxInfo info = new C2S_GetSoulBoxInfo();
        pak.PacketObject = info;
        this.Send(pak);
    }

    public void RequestSyncChangeData()
    {
        if (!GuideSimulate_Battle.sim_mode)
        {
            Packet pak = new Packet(OpcodeType.C2S_SYNCCHANGEDATA);
            C2S_SyncChangeData data = new C2S_SyncChangeData();
            pak.PacketObject = data;
            this.Send(pak);
        }
    }

    public void RequestUseItem(long card_id, int itemEntry, int useNum, int part)
    {
        Packet pak = new Packet(OpcodeType.C2S_USEITEM);
        C2S_UseItem item = new C2S_UseItem();
        pak.PacketObject = item;
        item.card_id = card_id;
        item.useNum = useNum;
        item.itemEntry = itemEntry;
        item.part = part;
        Instance.Send(pak);
    }

    public void RequestVerifyPermitCode(string permitCode, long account_id, string account_name, long channelId, PlatformType platform, string mac, bool use_macaddr_login)
    {
        Packet pak = new Packet(OpcodeType.C2S_VERIFYPERMITCODE);
        C2S_VerifyPermitCode code = new C2S_VerifyPermitCode {
            platform = platform,
            account_id = account_id,
            account_name = account_name,
            channelId = channelId,
            permitCode = permitCode,
            mac = mac,
            use_macaddr_login = use_macaddr_login
        };
        pak.PacketObject = code;
        this.Send(pak);
    }

    public void RequestVoidTower(int _flag)
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERREQ);
        C2S_VoidTowerReq req = new C2S_VoidTowerReq {
            flag = _flag
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestVoidTowerCombat(List<long> cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERCOMBAT);
        C2S_VoidTowerCombat combat = new C2S_VoidTowerCombat {
            cardPosList = cardPosList,
            start_time = (uint) TimeMgr.Instance.ServerStampTime
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestVoidTowerCombatEnd(BattleBack data)
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERCOMBATENDREQ);
        C2S_VoidTowerCombatEndReq req = new C2S_VoidTowerCombatEndReq {
            data = data
        };
        req.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        req.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestVoidTowerReward()
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERREWARD);
        C2S_VoidTowerReward reward = new C2S_VoidTowerReward();
        pak.PacketObject = reward;
        this.Send(pak);
    }

    public void RequestVoidTowerSmash()
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERSMASH);
        C2S_VoidTowerSmash smash = new C2S_VoidTowerSmash();
        pak.PacketObject = smash;
        this.Send(pak);
    }

    public void RequestVoidTowerSmashBegin()
    {
        Packet pak = new Packet(OpcodeType.C2S_VOIDTOWERSMASHBEGINREQ);
        C2S_VoidTowerSmashBeginReq req = new C2S_VoidTowerSmashBeginReq();
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestWarmmatch()
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHREQ);
        C2S_WarmmatchReq req = new C2S_WarmmatchReq();
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestWarmmatchCombat(long target_id, List<long> cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHCOMBAT);
        C2S_WarmmatchCombat combat = new C2S_WarmmatchCombat {
            cardPosList = cardPosList,
            target_id = target_id
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestWarmmatchCombatEnd(BattleBack data)
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHCOMBATENDREQ);
        C2S_WarmmatchCombatEndReq req = new C2S_WarmmatchCombatEndReq {
            data = data
        };
        req.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        req.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestWarmmatchGains()
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHGAINS);
        C2S_WarmmatchGains gains = new C2S_WarmmatchGains();
        pak.PacketObject = gains;
        this.Send(pak);
    }

    public void RequestWarmmatchRefresh()
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHREFRESH);
        C2S_WarmmatchRefresh refresh = new C2S_WarmmatchRefresh();
        pak.PacketObject = refresh;
        GUIMgr.Instance.Lock();
        this.Send(pak);
    }

    public void RequestWarmmatchTarget(long target_id)
    {
        Packet pak = new Packet(OpcodeType.C2S_WARMMATCHTARGETREQ);
        C2S_WarmmatchTargetReq req = new C2S_WarmmatchTargetReq {
            target_id = target_id
        };
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestWholeMail(bool isSys, long mailId)
    {
        Packet pak = new Packet(OpcodeType.C2S_GETWHOLEMAILCONTENT);
        C2S_GetWholeMail mail = new C2S_GetWholeMail {
            isSys = isSys,
            mailId = mailId
        };
        pak.PacketObject = mail;
        this.Send(pak);
    }

    public void RequestWorldBoss(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSREQ);
        C2S_WorldBossReq req = new C2S_WorldBossReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry
        };
        pak.PacketObject = req;
        Debug.Log("RequestWorldBoss");
        this.Send(pak);
    }

    public void RequestWorldBossBuyTimes(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSBUYTIMES);
        C2S_WorldBossBuyTimes times = new C2S_WorldBossBuyTimes {
            entry = _entry
        };
        pak.PacketObject = times;
        this.Send(pak);
    }

    public void RequestWorldBossCombat(int _entry, List<long> _cardPosList)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSCOMBAT);
        C2S_WorldBossCombat combat = new C2S_WorldBossCombat {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            cardPosList = _cardPosList
        };
        pak.PacketObject = combat;
        this.Send(pak);
    }

    public void RequestWorldBossCombatEnd(BattleBack _BattleBack)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSCOMBATENDREQ);
        C2S_WorldBossCombatEndReq req = new C2S_WorldBossCombatEndReq {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = 0,
            data = _BattleBack
        };
        req.data.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
        req.data.end_time = (uint) TimeMgr.Instance.ServerStampTime;
        pak.PacketObject = req;
        this.Send(pak);
    }

    public void RequestWorldBossEncourage(int _entry, WorldBossEncourageType _type)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSENCOURAGE);
        C2S_WorldBossEncourage encourage = new C2S_WorldBossEncourage {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry,
            type = _type
        };
        pak.PacketObject = encourage;
        this.Send(pak);
    }

    public void RequestWorldBossGains(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSGAINS);
        C2S_WorldBossGains gains = new C2S_WorldBossGains {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry
        };
        pak.PacketObject = gains;
        this.Send(pak);
    }

    public void RequestWorldBossReborn(int _entry)
    {
        Packet pak = new Packet(OpcodeType.C2S_WORLDBOSSREBORN);
        C2S_WorldBossReborn reborn = new C2S_WorldBossReborn {
            sessionInfo = ActorData.getInstance().SessionInfo,
            entry = _entry
        };
        pak.PacketObject = reborn;
        this.Send(pak);
    }

    public void RequstBeginChallengeArenaCombat(long enemy_id, List<long> pos_list, int enemy_order, int enemy_type)
    {
        Packet pak = new Packet(OpcodeType.C2S_LOLARENAENTERCOMBAT);
        C2S_LoLArenaEnterCombat combat = new C2S_LoLArenaEnterCombat();
        pak.PacketObject = combat;
        combat.cardPosList = pos_list;
        combat.targetId = enemy_id;
        combat.enemyOrder = enemy_order;
        combat.enemyType = enemy_type;
        combat.start_time = (uint) TimeMgr.Instance.ServerStampTime;
        this.Send(pak);
    }

    private void ReSendLastPak()
    {
        this.DebugLog("ReSend");
        this.LockGUI();
        this.isSending = false;
        this.StartSending(this.send_pak);
    }

    public void ReSendLastPakLogic(float delayTime)
    {
        this.DebugLog("ReSendLastPakLogic");
        this.LockGUI();
        if (delayTime > 0f)
        {
            ScheduleMgr.Schedule(delayTime, delegate {
                ActorData.getInstance().AddSessionProtocolKey();
                this.isReSending = false;
                this.ReSendLastPak();
            });
        }
        else
        {
            ActorData.getInstance().AddSessionProtocolKey();
            this.isReSending = false;
            this.ReSendLastPak();
        }
    }

    [PacketHandler(OpcodeType.S2C_ACCEPTALL_TX_FRIENDPHYFORCE, typeof(S2C_AcceptAll_TX_FriendPhyForce))]
    public void ReveiveS2C_AcceptAll_TX_FriendPhyForce(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_AcceptAll_TX_FriendPhyForce packetObject = pak.PacketObject as S2C_AcceptAll_TX_FriendPhyForce;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            int num = packetObject.phyForce - ActorData.getInstance().PhyForce;
            if (num > 0)
            {
                TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9896b5), num));
            }
            ActorData.getInstance().PhyForce = packetObject.phyForce;
            ActorData.getInstance().UserInfo.remainPhyForceAccept = packetObject.remainPhyForceAccept;
            foreach (QQFriendUser_Fresh fresh in packetObject.friendList)
            {
                XSingleton<SocialFriend>.Singleton.UpdateQQFriendGame(fresh);
            }
            FriendPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<FriendPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateQQFriendList();
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_CLEAR_QQFRIEND_COOLDOWN, typeof(S2C_Clear_QQFriend_CoolDown))]
    public void ReveiveS2C_Clear_QQFriend_CoolDown(Packet pak)
    {
        GUIMgr.Instance.UnLock();
        S2C_Clear_QQFriend_CoolDown packetObject = pak.PacketObject as S2C_Clear_QQFriend_CoolDown;
        if (this.CheckResultCodeAndReLogin(packetObject.result))
        {
            ActorData.getInstance().Stone = packetObject.stone;
            XSingleton<SocialFriend>.Singleton.UpdateCoolDownTime(packetObject.targetId);
            SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.UpdateAssitCard(packetObject.targetId);
            }
        }
    }

    [PacketHandler(OpcodeType.S2C_COLLECTEXCHANGE, typeof(FastBuf.S2C_CollectExchange))]
    public void S2C_CollectExchange(Packet pak)
    {
        <S2C_CollectExchange>c__AnonStorey102 storey = new <S2C_CollectExchange>c__AnonStorey102 {
            res = (FastBuf.S2C_CollectExchange) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__91), null);
            if (ActivePanel.inst != null)
            {
                ActivePanel.inst.ResetState(storey.res.activity_type, storey.res.activity_entry, storey.res.subEntry, storey.res.flag);
            }
        }
        else
        {
            Debug.Log("收到领取失败---------------");
        }
    }

    [PacketHandler(OpcodeType.S2C_TXBUYSTOREITEM, typeof(S2C_TXBuyStoreItem))]
    public void S2C_TXBUYSTOREITEM(Packet pak)
    {
        <S2C_TXBUYSTOREITEM>c__AnonStorey103 storey = new <S2C_TXBUYSTOREITEM>c__AnonStorey103 {
            res = (S2C_TXBuyStoreItem) pak.PacketObject
        };
        if (this.CheckResultCodeAndReLogin(storey.res.result))
        {
            GUIMgr.Instance.DoModelGUI("RewardPanel", new Action<GUIEntity>(storey.<>m__92), null);
            TencentStoreCommodity commodity = new TencentStoreCommodity {
                commodityId = storey.res.commodityInfo.commodityId,
                reward_describe = storey.res.commodityInfo.name,
                reward_items = storey.res.commodityInfo.describe,
                flag = storey.res.commodityInfo.flag
            };
            Debug.Log("========res.commodityInfo.canBuy: " + storey.res.commodityInfo.flag);
            if (storey.res.commodityInfo.costStone != -1)
            {
                commodity.reward_Price = storey.res.commodityInfo.costStone;
                commodity.costType = CostType.E_CT_Stone;
            }
            else
            {
                commodity.reward_Price = storey.res.commodityInfo.costGold;
                commodity.costType = CostType.E_CT_Gold;
            }
            commodity.purchaseCountOfDay = storey.res.commodityInfo.purchaseCountOfDay;
            commodity.purchaseCount = storey.res.commodityInfo.purchaseCount;
            commodity.serverPurCntOfDay = storey.res.commodityInfo.serverPurCntOfDay;
            commodity.serverPurCnt = storey.res.commodityInfo.serverPurCnt;
            if (ActivePanel.inst != null)
            {
                ActivePanel.inst.ResetBuyStoreState2(commodity);
            }
        }
        else
        {
            Debug.Log("收到领取失败---------------");
        }
    }

    public void SelectHeroOutlandCombatReq(List<long> _cardPosList)
    {
        Debug.Log("进入战斗 选择英雄 选择英雄数量==" + _cardPosList.Count);
        if (<>f__am$cache26 == null)
        {
            <>f__am$cache26 = c => Debug.Log("英雄id==" + c);
        }
        _cardPosList.ForEach(<>f__am$cache26);
        this.RequestOutlandCombatReq(BattleState.GetInstance().CurGame.battleGameData.gridGameData.entry, BattleState.GetInstance().CurGame.battleGameData.gridGameData.map_entry, BattleState.GetInstance().CurGame.battleGameData.point, _cardPosList, -1L);
    }

    public void Send(Packet pak)
    {
        if (this.IsSessionNoInit(pak))
        {
            this.CloseWaitGUI();
            this.UnlockGUI();
        }
        else
        {
            this._pendingSendQueue.Enqueue(pak);
            this.LockGUI();
            this.TrySend();
        }
    }

    [DebuggerHidden]
    public IEnumerator SendLoop()
    {
        return new <SendLoop>c__Iterator18 { <>f__this = this };
    }

    public void SendQueryCaifuTong()
    {
        Debug.Log("-----SendQueryCaifuTong------");
        Packet pak = new Packet(OpcodeType.C2S_CAN_PAY_FLAG);
        C2S_Can_Pay_Flag flag = new C2S_Can_Pay_Flag();
        pak.PacketObject = flag;
        this.Send(pak);
    }

    public void SendQueryTxBalance()
    {
        string str = GameDefine.getInstance().tx_openKey;
        string str2 = GameDefine.getInstance().tx_qqPayToken;
        string str3 = GameDefine.getInstance().tx_pf;
        string str4 = GameDefine.getInstance().tx_pfKey;
        string str5 = string.Empty;
        int num = 0;
        if (GameDefine.getInstance().isDebugLog)
        {
            Debug.Log(string.Concat(new object[] { "PAY: SendQueryTxBalance ", str, " ", str2, " ", str3, " ", str4, " ", str5, " ", num }));
        }
        GUIMgr.Instance.Lock();
        Packet pak = new Packet(OpcodeType.C2S_QUERYTXBALANCE);
        C2S_QueryTxBalance balance = new C2S_QueryTxBalance {
            openKey = str,
            payToken = str2,
            pf = str3,
            pfKey = str4,
            sig = str5,
            time = num
        };
        pak.PacketObject = balance;
        this.Send(pak);
    }

    public void SendQueryVIP()
    {
        GUIMgr.Instance.Lock();
        if (GameDefine.getInstance().isDebugLog)
        {
        }
        Packet pak = new Packet(OpcodeType.C2S_REQMONTHCARDDATA);
        C2S_ReqMonthCardData data = new C2S_ReqMonthCardData();
        pak.PacketObject = data;
        this.Send(pak);
    }

    public void SendTssData2Server(byte[] data, int dataLength)
    {
        Packet pak = new Packet(OpcodeType.C2S_COMMITANTIDATA);
        C2S_CommitAntiData data2 = new C2S_CommitAntiData {
            data = new List<byte>(data)
        };
        pak.PacketObject = data2;
        this.Send(pak);
        TssSDKInterface instance = TssSDKInterface.GetInstance();
        instance.sendDataLength += data2.data.Count;
    }

    public bool SetCurActiveCompleteIsOk(List<ActiveInfo> activeList)
    {
        bool flag = false;
        int count = activeList.Count;
        for (int i = 0; i < count; i++)
        {
            int num3 = activeList[i].rewards_configs.Count;
            bool flag2 = false;
            for (int j = 0; j < num3; j++)
            {
                if (activeList[i].rewards_configs[j].flag == UniversialRewardDrawFlag.E_UNIREWARD_FLAG_CAN_DRAW)
                {
                    flag2 = true;
                    flag = flag2;
                    break;
                }
                flag2 = false;
                flag = flag2;
            }
            if (flag)
            {
                return flag;
            }
        }
        return flag;
    }

    private int SortActiveList(ActiveInfo info1, ActiveInfo info2)
    {
        return (info1.sort_priority - info2.sort_priority);
    }

    private void Start()
    {
        this.InitVersion(0);
    }

    private void StartSend()
    {
        this.DebugLog("StartSend");
        MemoryStream dest = new MemoryStream();
        BufferWriter writer = new BufferWriter(dest);
        if (this.send_pak.PacketObject == null)
        {
            Debug.LogError(this.send_pak.OpCode + "  PacketObject is null");
            this.onConnectFailed();
        }
        else
        {
            NetConnectIsOk = true;
            ((ISendCommandable) this.send_pak.PacketObject).SetSessionInfo(ActorData.getInstance().SessionInfo);
            this.send_pak.PacketObject.Serialize(writer);
            writer.Dispose();
            int length = ((int) dest.Length) + HEADER_SIZE;
            byte option = 0;
            bool flag = false;
            byte[] sourceArray = null;
            byte[] protobuf = new byte[length];
            MakePacketHeader(ref protobuf, this.version, option, length, (short) this.send_pak.OpCode);
            if (!flag)
            {
                dest.Position = 0L;
                dest.Read(protobuf, HEADER_SIZE, (int) dest.Length);
            }
            else
            {
                byte[] buffer3 = TypeConvertUtil.intToBytes(sourceArray.Length, true);
                protobuf[HEADER_SIZE] = buffer3[0];
                protobuf[HEADER_SIZE + 1] = buffer3[1];
                protobuf[HEADER_SIZE + 2] = buffer3[2];
                protobuf[HEADER_SIZE + 3] = buffer3[3];
                Array.Copy(sourceArray, 0, protobuf, HEADER_SIZE + 4, sourceArray.Length);
            }
            _totalBytesSent += protobuf.Length;
            try
            {
                this._tcpSock.Send(protobuf);
            }
            catch (Exception exception)
            {
                Debug.LogWarning(exception);
            }
        }
    }

    private void StartSending(Packet send_pak)
    {
        this.sendingOpCode = send_pak.OpCode;
        this.isSending = true;
        base.StartCoroutine("SendLoop");
    }

    public void testRuleBoxcollider(Vector2 vec, bool AutoSetMacth)
    {
        this.ruleBoxCollider = vec;
        this.labelAutoSetMacth = AutoSetMacth;
    }

    private void TrySend()
    {
        if (!this.isSending)
        {
            this.send_pak = this._pendingSendQueue.Dequeue();
            ActorData.getInstance().AddSessionProtocolKey();
            this.StartSending(this.send_pak);
        }
    }

    private void UnlockGUI()
    {
        this.isLockGUI = false;
        GUIMgr.Instance.UnLock();
    }

    private void Update()
    {
        this.CheckSendLoop();
    }

    public bool labelAutoSetMacth { get; set; }

    public Vector2 ruleBoxCollider { get; set; }

    public static int TotalBytesReceived
    {
        get
        {
            return _totalBytesReceived;
        }
    }

    public static int TotalBytesSent
    {
        get
        {
            return _totalBytesSent;
        }
    }

    [CompilerGenerated]
    private sealed class <CheckResultCodeAndReLogin>c__AnonStorey141
    {
        internal UpdateMessageBox gui;

        internal void <>m__100(GameObject box)
        {
            Debug.Log("unitylcs start common update");
            if (!GameDefine.getInstance().isYYBUpdateDownloading)
            {
                this.gui.DownloadSlider.value = 0f;
                this.gui.DownloadMsgLabel.text = string.Empty;
                PlatformInterface.mInstance.PlatformYYBCommonDownloadGame();
            }
        }

        internal void <>m__FE(int vari, string text)
        {
            this.gui.OnUpdateProgress(vari, text);
        }

        internal void <>m__FF(GameObject box)
        {
            if (!GameDefine.getInstance().isYYBUpdateDownloading)
            {
                Debug.Log("unitylcs start save update");
                this.gui.DownloadSlider.value = 0f;
                this.gui.DownloadMsgLabel.text = string.Empty;
                PlatformInterface.mInstance.PlatformYYBSaveDownloadGame();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <CheckResultCodeAndReLogin>c__AnonStorey142
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal OpResult result;

        private static void <>m__108(GameObject go)
        {
            BattleStaticEntry.TryClearBattleOnError();
            LoginPanel.CheckSwitchAccountYes();
        }

        internal void <>m__F7(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (GameObject go) {
                    BattleStaticEntry.TryClearBattleOnError();
                    LoginPanel.CheckSwitchAccountYes();
                };
            }
            box.SetDialog(ConfigMgr.getInstance().GetErrorCode(this.result), <>f__am$cache1, null, true);
        }
    }

    [CompilerGenerated]
    private sealed class <getNetWorkEventReceiverList>c__Iterator19 : IEnumerator, IDisposable, IEnumerable, IEnumerator<object>, IEnumerable<object>
    {
        internal object $current;
        internal int $PC;
        internal PlayMakerFSM[] <$s_245>__1;
        internal int <$s_246>__2;
        internal PlayMakerFSM <fsm>__3;
        internal PlayMakerFSM[] <fsms>__0;

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
                    if (UICamera.mainCamera == null)
                    {
                        goto Label_00A5;
                    }
                    this.<fsms>__0 = UICamera.mainCamera.GetComponentsInChildren<PlayMakerFSM>();
                    this.<$s_245>__1 = this.<fsms>__0;
                    this.<$s_246>__2 = 0;
                    break;

                case 1:
                    this.<$s_246>__2++;
                    break;

                default:
                    goto Label_00AC;
            }
            if (this.<$s_246>__2 < this.<$s_245>__1.Length)
            {
                this.<fsm>__3 = this.<$s_245>__1[this.<$s_246>__2];
                this.$current = this.<fsm>__3;
                this.$PC = 1;
                return true;
            }
        Label_00A5:
            this.$PC = -1;
        Label_00AC:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        [DebuggerHidden]
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
            {
                return this;
            }
            return new SocketMgr.<getNetWorkEventReceiverList>c__Iterator19();
        }

        [DebuggerHidden]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
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
    private sealed class <ProcessSweepResult>c__AnonStoreyFD
    {
        internal S2C_OutlandSweepReq res;

        internal void <>m__8C(GUIEntity obj)
        {
            ((RushResultPanel) obj).UpdateData(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivArenaLadderCombat>c__AnonStorey10B
    {
        internal SocketMgr <>f__this;
        internal S2C_ArenaLadderCombat res;

        internal void <>m__9E(bool isWin, BattleNormalGameType _type, BattleNormalGameResult result)
        {
            BattleBack data = new BattleBack {
                result = isWin
            };
            this.<>f__this.RequestArenaLadderCombatEnd(data, this.res.targetId, this.res.enemyType, this.res.targetOrder);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveArenaLadderCombatEnd>c__AnonStorey10C
    {
        internal S2C_ArenaLadderCombatEnd res;

        internal void <>m__9F(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateArenaLadderResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveBattleEnd>c__AnonStorey13E
    {
        internal S2C_FlameBattleEnd res;

        internal void <>m__F0(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateYuanZhengResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveBeginChallengeArenaCombat>c__AnonStorey10E
    {
        internal SocketMgr <>f__this;
        internal S2C_LoLArenaEnterCombat res;

        internal void <>m__A2(bool is_win, BattleNormalGameType type, BattleNormalGameResult result)
        {
            BattleBack data = new BattleBack {
                result = is_win
            };
            this.<>f__this.RequestEndChallengeArenaCombat(data, this.res.targetId, this.res.enemyType, this.res.targetOrder);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveC2S_GuildDupCombatBegin>c__AnonStorey128
    {
        private static Func<BattleGameResultActorInfo, bool> <>f__am$cache2;
        private static Func<BattleGameResultActorInfo, int> <>f__am$cache3;
        private static Func<BattleGameResultActorInfo, int> <>f__am$cache4;
        internal SocketMgr <>f__this;
        internal S2C_GuildDupCombatBegin res;

        private static bool <>m__105(BattleGameResultActorInfo t)
        {
            return (t.hp <= 0L);
        }

        private static int <>m__106(BattleGameResultActorInfo t)
        {
            return t.cardEntry;
        }

        private static int <>m__107(BattleGameResultActorInfo t)
        {
            return t.cardEntry;
        }

        internal void <>m__C7(bool isWin, BattleNormalGameType _type, BattleNormalGameResult result)
        {
            List<int> list;
            BattleBack back = new BattleBack {
                result = isWin
            };
            if (isWin)
            {
                if (<>f__am$cache2 == null)
                {
                    <>f__am$cache2 = t => t.hp <= 0L;
                }
                if (<>f__am$cache3 == null)
                {
                    <>f__am$cache3 = t => t.cardEntry;
                }
                list = result.actorInfoes.attackers.Where<BattleGameResultActorInfo>(<>f__am$cache2).Select<BattleGameResultActorInfo, int>(<>f__am$cache3).ToList<int>();
            }
            else
            {
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = t => t.cardEntry;
                }
                list = result.actorInfoes.attackers.Select<BattleGameResultActorInfo, int>(<>f__am$cache4).ToList<int>();
            }
            back.damage_amount = (ulong) Mathf.Abs(BattleSecurityManager.Instance.GetBattleTotalDemage(true));
            back.securityDataList = BattleSecurityManager.Instance.GetSecurityDataList();
            back.end_time = (uint) TimeMgr.Instance.ServerStampTime;
            this.<>f__this.RequestC2S_GuildDupCombatEnd(back, list, this.res.guildDupId, this.res.guildDupTrenchId);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveChallengeArenaInfo>c__AnonStorey10D
    {
        internal Dictionary<long, CardInfo> card_dic;

        internal int <>m__A0(long id_l, long id_r)
        {
            CardInfo info = null;
            CardInfo info2 = null;
            if (!this.card_dic.TryGetValue(id_l, out info) || !this.card_dic.TryGetValue(id_r, out info2))
            {
                return 0;
            }
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) info.entry);
            card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) info2.entry);
            int num = (_config != null) ? _config.card_position : -1;
            int num2 = (_config2 != null) ? _config2.card_position : -1;
            return (num - num2);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveChallengeSelfBanInfo>c__AnonStorey110
    {
        internal SocketMgr.<ReceiveChallengeSelfBanInfo>c__AnonStorey112 <>f__ref$274;
        internal List<Card> formation_card_list;

        internal void <>m__A4(LoLArenaCardData info)
        {
            <ReceiveChallengeSelfBanInfo>c__AnonStorey111 storey = new <ReceiveChallengeSelfBanInfo>c__AnonStorey111 {
                <>f__ref$274 = this.<>f__ref$274,
                <>f__ref$272 = this,
                info = info
            };
            int index = this.formation_card_list.FindIndex(new Predicate<Card>(storey.<>m__104));
            if ((index >= 0) && (index <= this.<>f__ref$274.data.self_activity_formation.Length))
            {
                this.<>f__ref$274.data.self_activity_formation[index] = 0 == storey.info.is_disable;
            }
        }

        private sealed class <ReceiveChallengeSelfBanInfo>c__AnonStorey111
        {
            internal SocketMgr.<ReceiveChallengeSelfBanInfo>c__AnonStorey110 <>f__ref$272;
            internal SocketMgr.<ReceiveChallengeSelfBanInfo>c__AnonStorey112 <>f__ref$274;
            internal LoLArenaCardData info;

            internal bool <>m__104(Card e)
            {
                return (e.card_id == this.info.card_id);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveChallengeSelfBanInfo>c__AnonStorey112
    {
        internal ChallengeArenaData data;
    }

    [CompilerGenerated]
    private sealed class <ReceiveCommitQuestResult>c__AnonStorey100
    {
        internal AchievementPanel panel;

        internal void <>m__8F(GUIEntity e)
        {
            if (null != this.panel)
            {
                this.panel.UnlockCommitPortal();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveCourageShopBuyResult>c__AnonStorey134
    {
        internal S2C_CourageShopBuy res;

        internal bool <>m__E0(ShopItem e)
        {
            return (e.slot == this.res.buyResult.slot);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveDRAWACTIVITYPRIZE>c__AnonStorey101
    {
        internal S2C_DrawActivityPrize res;

        internal void <>m__90(GUIEntity entity)
        {
            (entity as RewardPanel).ShowActiveReward0(this.res);
            ActorData.getInstance().UpdateActiveData0(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveDrawLotteryCard>c__AnonStorey135
    {
        internal SocketMgr.<ReceiveDrawLotteryCard>c__AnonStorey136 <>f__ref$310;
        internal List<int> card_list;
        internal List<int> item_list;
        internal List<int> item_num_list;
        internal List<int> morph_list;
        internal List<int> morph_num_list;

        internal void <>m__E1()
        {
            this.card_list.Clear();
            this.item_list.Clear();
            foreach (LotteryReward reward in this.<>f__ref$310.res.rewardList)
            {
                if ((reward.item != null) && (reward.item.entry != -1))
                {
                    this.item_list.Add(reward.item.entry);
                    this.item_num_list.Add(reward.item.diff);
                }
                foreach (Card card in reward.card.newCard)
                {
                    this.card_list.Add((int) card.cardInfo.entry);
                }
                foreach (Item item in reward.card.newItem)
                {
                    int cardExCfgByItemPart = CommonFunc.GetCardExCfgByItemPart(item.entry);
                    if (cardExCfgByItemPart == -1)
                    {
                        if (item.entry != -1)
                        {
                            this.item_list.Add(item.entry);
                            this.item_num_list.Add(item.diff);
                        }
                    }
                    else
                    {
                        this.card_list.Add(cardExCfgByItemPart);
                        this.morph_list.Add(cardExCfgByItemPart);
                        this.morph_num_list.Add(item.diff);
                    }
                }
            }
            GameDataMgr.Instance.boostRecruit.ShowRecruitResult(this.card_list, this.item_list, this.item_num_list, this.morph_list, this.morph_num_list, true);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveDrawLotteryCard>c__AnonStorey136
    {
        internal S2C_DrawLotteryCard res;
    }

    [CompilerGenerated]
    private sealed class <ReceiveEndChallengeArenaCombat>c__AnonStorey10F
    {
        internal S2C_LoLArenaCombatEnd res;

        internal void <>m__A3(GUIEntity e)
        {
            (e as ResultPanel).UpdateChallengeArenaLadderResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveFlameBattleSmash>c__AnonStorey140
    {
        internal S2C_FlameBattleSmash res;

        internal void <>m__F2(GUIEntity obj)
        {
            YuanZhengRushPanel panel = (YuanZhengRushPanel) obj;
            panel.Depth = 360;
            panel.UpdateData(this.res.reward);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveFriendCombat>c__AnonStorey124
    {
        internal S2C_FriendCombat res;

        internal void <>m__BE(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateFriendPkResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGainFlameBattleReward>c__AnonStorey13F
    {
        internal S2C_GainFlameBattleReward res;

        internal void <>m__F1(GUIEntity entity)
        {
            (entity as RewardPanel).ShowYuanZhengBattleReward(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGetFriendFormation>c__AnonStorey123
    {
        internal S2C_GetFriendFormation res;

        internal void <>m__BC(GUIEntity obj)
        {
            ((TargetTeamPanel) obj).SetTargetInfo(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGetHeadFrameList>c__AnonStorey146
    {
        internal S2C_GetHeadFrameList res;

        internal void <>m__FB(GUIEntity obj)
        {
            ((PlayerIconFramePanel) obj).SetIconFrames(this.res.headFrame);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGetLeagueOpponentFormation>c__AnonStorey105
    {
        internal S2C_GetLeagueOpponentFormation res;

        internal void <>m__94(GUIEntity obj)
        {
            ((TargetTeamPanel) obj).SetWorldCupTargetInfo(this.res);
        }

        internal void <>m__95(GUIEntity obj)
        {
            SelectHeroPanel panel = (SelectHeroPanel) obj;
            panel.mBattleType = BattleType.WorldCupPk;
            panel.SetWorldCupPkTargetInfo(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGetUserInfo>c__AnonStorey143
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal S2C_GetUserInfo res;

        private static void <>m__109(GameObject go)
        {
        }

        internal void <>m__F8(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate (GameObject go) {
                };
            }
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x578), this.res.userInfo.frozen_reason, TimeMgr.Instance.GetTimeStr(this.res.userInfo.frozen_len), TimeMgr.Instance.GetFrozenTime(this.res.userInfo.frozen_time)), <>f__am$cache1, null, true);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGuildBattleCombat>c__AnonStorey126
    {
        internal SocketMgr <>f__this;
        internal S2C_GuildBattleCombat res;

        internal void <>m__C2(bool isWin, BattleNormalGameType _type, BattleNormalGameResult result)
        {
            BattleBack back = new BattleBack {
                result = isWin,
                damage_amount = (ulong) Mathf.Abs(BattleSecurityManager.Instance.GetBattleTotalDemage(true))
            };
            this.<>f__this.RequestGuildBattleCombatEnd(back, this.res.targetId, this.res.resourceEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGuildBattleCombatEnd>c__AnonStorey127
    {
        internal S2C_GuildBattleCombatEnd res;

        internal void <>m__C3(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateGuildBattleResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveGuildBattleInfo>c__AnonStorey125
    {
        internal S2C_GuildBattleInfo res;

        internal void <>m__C1(GUIEntity entity)
        {
            ((GuildBattlePanel) entity).UpdateBattleInfo(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveLeagueFight>c__AnonStorey106
    {
        internal S2C_LeagueFight res;

        internal void <>m__97(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateWorldCupPkResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveLeagueHistory>c__AnonStorey107
    {
        internal S2C_GetLeagueHistory res;

        internal void <>m__98(GUIEntity obj)
        {
            ((LeagueHistoryPanel) obj).UpdateData(this.res.historyList);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveNewDungeonsCombat>c__AnonStorey11D
    {
        internal SocketMgr <>f__this;
        internal S2C_NewDungeonsCombatReq res;

        internal void <>m__B5(bool arg1, BattleNormalGameType arg2, BattleNormalGameResult arg3)
        {
            BattleBack back = new BattleBack {
                result = arg1
            };
            this.<>f__this.RequestNewDungeonsCombatEnd(this.res.dupData, back);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveNewDungeonsCombatEnd>c__AnonStorey11E
    {
        internal S2C_NewDungeonsCombatEndReq res;

        internal void <>m__B6(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateDungeonsResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandCombatEndReq>c__AnonStoreyF9
    {
        internal S2C_OutlandCombatEndReq res;

        internal void <>m__87(GUIEntity obj)
        {
            GUIMgr.Instance.ExitModelGUI("BattlePausePanel");
            ((ResultOutlandPanel) obj).UpdateOutlandCombatResult(this.res, false);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandCreateMapReq>c__AnonStoreyFE
    {
        internal S2C_OutlandCreateMapReq res;

        internal void <>m__8D(GUIEntity guiE)
        {
            DupLevInfoPanel panel = (DupLevInfoPanel) guiE;
            panel.mIsDupEnter = false;
            panel.OpenTypeIsPush = true;
            panel.UpdateOutlandMapInfo(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandDropEvent>c__AnonStoreyFA
    {
        internal S2C_OutlandDropEvent res;

        internal void <>m__88(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            if (panel != null)
            {
                panel.ShowOutlandBattleReward(this.res, null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandGetFloorBoxReward>c__AnonStoreyFB
    {
        internal S2C_OutlandGetFloorBoxReward res;

        internal void <>m__89(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            if (panel != null)
            {
                panel.ShowOutlandBattleReward(null, this.res);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandGetMonsterInfo>c__AnonStoreyF8
    {
        internal S2C_OutlandGetMonsterInfo res;

        internal void <>m__83(GUIEntity obj)
        {
            TargetTeamPanel panel = (TargetTeamPanel) obj;
            panel.mBattleType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
            panel.SetBgClose(false);
            panel.SetPkBtnStat(true);
            panel.SetOutlandInfo(this.res.monster_info);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandQuit>c__AnonStoreyFC
    {
        internal S2C_OutlandQuit res;

        internal void <>m__103(GUIEntity obj)
        {
            ((ResultOutlandPanel) obj).UpdateOutlandQuitResult(this.res, true);
            if (this.res.gold > 0)
            {
                ActorData.getInstance().Gold = this.res.gold;
            }
            if (this.res.stone > 0)
            {
                ActorData.getInstance().Stone = this.res.stone;
            }
            if (this.res.courage > 0)
            {
                ActorData.getInstance().Courage = this.res.courage;
            }
            if (this.res.phyForce > 0)
            {
                ActorData.getInstance().PhyForce = this.res.phyForce;
            }
            if (this.res.eq > 0)
            {
                ActorData.getInstance().Eq = this.res.eq;
            }
            if (this.res.outland_coin > 0)
            {
                ActorData.getInstance().OutlandCoin = this.res.outland_coin;
            }
            if (this.res.reward.items.Count > 0)
            {
                ActorData.getInstance().UpdateItemList(this.res.reward.items);
            }
            if (this.res.reward.cards.Count > 0)
            {
                ActorData.getInstance().UpdateNewCard(this.res.reward.cards);
            }
        }

        internal void <>m__8A()
        {
            GUIMgr.Instance.PushGUIEntity("ResultOutlandPanel", delegate (GUIEntity obj) {
                ((ResultOutlandPanel) obj).UpdateOutlandQuitResult(this.res, true);
                if (this.res.gold > 0)
                {
                    ActorData.getInstance().Gold = this.res.gold;
                }
                if (this.res.stone > 0)
                {
                    ActorData.getInstance().Stone = this.res.stone;
                }
                if (this.res.courage > 0)
                {
                    ActorData.getInstance().Courage = this.res.courage;
                }
                if (this.res.phyForce > 0)
                {
                    ActorData.getInstance().PhyForce = this.res.phyForce;
                }
                if (this.res.eq > 0)
                {
                    ActorData.getInstance().Eq = this.res.eq;
                }
                if (this.res.outland_coin > 0)
                {
                    ActorData.getInstance().OutlandCoin = this.res.outland_coin;
                }
                if (this.res.reward.items.Count > 0)
                {
                    ActorData.getInstance().UpdateItemList(this.res.reward.items);
                }
                if (this.res.reward.cards.Count > 0)
                {
                    ActorData.getInstance().UpdateNewCard(this.res.reward.cards);
                }
            });
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveOutlandsData>c__AnonStoreyF7
    {
        internal SocketMgr <>f__this;
        internal outland_map_type_config outlandMap;

        internal void <>m__81(GUIEntity obj)
        {
            OutlandSecondPanel panel = (OutlandSecondPanel) obj;
            ActorData.getInstance().tempOutlandMapTypeConfig = this.outlandMap;
            ActorData.getInstance().outlandType = this.outlandMap.entry;
            panel.SetInfo(this.outlandMap);
            if (((ActorData.getInstance().isOutlandGrid && (ActorData.getInstance().outlandPageEntry >= 0)) && ((ActorData.getInstance().outlandType >= 0) && (ActorData.getInstance().outlandType <= 3))) && ((!ActorData.getInstance().outlandAllHerosDeadList[ActorData.getInstance().outlandType] && (ActorData.getInstance().outlandTitles.Count >= 4)) && ActorData.getInstance().outlandTitles[3].is_underway))
            {
                ActorData.getInstance().isOutlandGrid = false;
                ActorData.getInstance().outlandAllHerosDeadList[ActorData.getInstance().outlandType] = false;
                ActorData.getInstance().bOpenThirdPanel = true;
                SocketMgr.Instance.RequestOutlandPageMapReq(ActorData.getInstance().outlandPageEntry);
            }
            else if (this.<>f__this.LoadingAction != null)
            {
                this.<>f__this.LoadingAction();
            }
            if (this.<>f__this.EnterAction != null)
            {
                this.<>f__this.EnterAction();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivePickGift>c__AnonStorey144
    {
        internal S2C_PickGift res;

        internal void <>m__F9(GUIEntity entity)
        {
            (entity as RewardPanel).ShowExchangeReward(this.res);
            ActorData.getInstance().Stone = this.res.stone;
            ActorData.getInstance().Gold = this.res.gold;
            ActorData.getInstance().PhyForce = this.res.phyForce;
            ActorData.getInstance().UpdateNewCard(this.res.cardList);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivePickLeagueReward>c__AnonStorey104
    {
        internal S2C_PickLeagueReward res;

        internal void <>m__93(GUIEntity entity)
        {
            (entity as RewardPanel).ShowBattleReward(this.res.reward);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivePickMailAffix>c__AnonStorey133
    {
        internal S2C_PickMailAffix res;

        internal void <>m__D4(GUIEntity entity)
        {
            (entity as RewardPanel).ShowMailPickAffix(this.res);
            if (this.res.gold > 0)
            {
                ActorData.getInstance().Gold = this.res.gold;
            }
            if (this.res.stone > 0)
            {
                ActorData.getInstance().Stone = this.res.stone;
            }
            if (this.res.courage > 0)
            {
                ActorData.getInstance().Courage = this.res.courage;
            }
            if (this.res.phyForce > 0)
            {
                ActorData.getInstance().PhyForce = this.res.phyForce;
            }
            if (this.res.eq > 0)
            {
                ActorData.getInstance().Eq = this.res.eq;
            }
            if (this.res.itemList.Count > 0)
            {
                ActorData.getInstance().UpdateItemList(this.res.itemList);
            }
            if (this.res.newCard.Count > 0)
            {
                ActorData.getInstance().UpdateNewCard(this.res.newCard);
            }
            if (this.res.arenaLadderScore > 0)
            {
                ActorData.getInstance().ArenaLadderCoin = this.res.arenaLadderScore;
            }
            if (this.res.flamebattleCoin > 0)
            {
                ActorData.getInstance().FlamebattleCoin = this.res.flamebattleCoin;
            }
            if (this.res.outland_coin > 0)
            {
                ActorData.getInstance().OutlandCoin = this.res.outland_coin;
            }
            if (this.res.contribute > 0)
            {
                ActorData.getInstance().mUserGuildMemberData.contribution = this.res.contribute;
            }
            if (this.res.lolarenaScore > 0)
            {
                ActorData.getInstance().ArenaChallengeCoin = this.res.lolarenaScore;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveQuitDup>c__AnonStoreyFF
    {
        internal S2C_DuplicateEndReq res;

        internal bool <>m__8E(SocialUser item)
        {
            foreach (QQCardCoolDown down in this.res.qqCardCoolDown)
            {
                if (item.QQUser.userInfo.leaderInfo.card_id == down.card_id)
                {
                    item.QQUser.cool_down_time = down.cool_down_time;
                }
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveRegistration>c__AnonStorey145
    {
        internal S2C_Registration res;

        internal void <>m__FA(GUIEntity entity)
        {
            (entity as RewardPanel).ShowSignInReward(this.res);
            ActorData.getInstance().UpdateRegistrationData(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveRequestDupReward>c__AnonStorey11F
    {
        internal S2C_PickDuplicateReward res;

        internal bool <>m__B7(DuplicateRewardInfo obj)
        {
            return (obj.duplicateEntry == this.res.duplicateEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_BuyConvoyRobTimes>c__AnonStorey11C
    {
        internal S2C_BuyConvoyRobTimes result;

        internal void <>m__B4(GUIEntity ui)
        {
            (ui as DetainsDartOnDoingPanel).SetInterceptBtnData(this.result.remainRobTimes);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_ConvoyAvengeCombatEnd>c__AnonStorey118
    {
        internal S2C_ConvoyAvengeCombatEnd result;

        internal void <>m__AD(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateDetainsDartBattleBackSelfResult(this.result);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_ConvoyEnd>c__AnonStorey116
    {
        internal SocketMgr <>f__this;
        internal S2C_ConvoyEnd result;

        internal void <>m__AB(GUIEntity entity)
        {
            XSingleton<GameDetainsDartMgr>.Singleton.curDetainsDartState = GameDetainsDartMgr.DetainsDartState.None;
            XSingleton<GameDetainsDartMgr>.Singleton.curEscortState = GameDetainsDartMgr.EscortState.None;
            DetainsDartMainUIPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DetainsDartMainUIPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.SetDataInfo(true, null);
            }
            else
            {
                this.<>f__this.RequestC2S_GetConvoyInfo();
            }
            (entity as RewardPanel).ShowDetainsDartEscortReward(this.result);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_Cross_AcceptFriendPhyForce>c__AnonStorey122
    {
        internal S2C_Cross_AcceptFriendPhyForce result;

        internal bool <>m__BB(SocialUser t)
        {
            if (t.QQUser.userInfo.id == this.result.targetId)
            {
                t.QQUser.isGivePhyForceNow = false;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_Cross_GiveFriendPhyForce>c__AnonStorey121
    {
        internal S2C_Cross_GiveFriendPhyForce result;

        internal bool <>m__BA(SocialUser u)
        {
            if (u.QQUser.userInfo.id == this.result.targetId)
            {
                u.QQUser.alreadyGivePhyForceToday = true;
                return true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GetConvoyInfo>c__AnonStorey115
    {
        internal S2C_GetConvoyInfo result;

        internal void <>m__A9(GUIEntity ui)
        {
            (ui as DetainsDartMainUIPanel).SetDataInfo(false, this.result);
        }

        internal void <>m__AA(GUIEntity ui)
        {
            (ui as DetainsDartMainUIPanel).SetDataInfo(false, this.result);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GetRobTargetList>c__AnonStorey117
    {
        internal S2C_GetRobTargetList result;

        internal void <>m__AC(GUIEntity ui)
        {
            (ui as DetainsDartOnDoingPanel).SetDataInfo(false, this.result.targets);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GiveAll_TX_FriendPhyForce>c__AnonStorey120
    {
        internal S2C_GiveAll_TX_FriendPhyForce result;

        internal bool <>m__B8(SocialUser u)
        {
            if (this.result.targetIds.Contains(u.QQUser.userInfo.id))
            {
                u.QQUser.alreadyGivePhyForceToday = true;
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildDupDistributeHistory>c__AnonStorey130
    {
        internal S2C_GuildDupDistributeHistory result;

        internal void <>m__D0(GUIEntity entity)
        {
            (entity as GuidBattleBossHandRecordPanel).ReceiveGuildDupDistributeHistory(this.result, false);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildDupItemQueueInfo>c__AnonStorey12D
    {
        internal S2C_GuildDupItemQueueInfo result;

        internal void <>m__CC(GUIEntity entity)
        {
            GuidBattleBossHandOutPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossHandOutPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveBossDamageRank(this.result, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildDuplicateDamageRank>c__AnonStorey129
    {
        internal S2C_GuildDuplicateDamageRank result;

        internal void <>m__C8(GUIEntity u)
        {
            GuidBattleDamageBarRankPanel gUIEntity = GUIMgr.Instance.GetGUIEntity<GuidBattleDamageBarRankPanel>();
            if (gUIEntity != null)
            {
                gUIEntity.ReceiveDupDamageRank(this.result, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildDuplicatePassRank>c__AnonStorey12B
    {
        internal S2C_GuildDuplicatePassRank result;

        internal void <>m__CA(GUIEntity entity)
        {
            (entity as PanelGuildDupTimeRank).ReceiveDupRank(this.result, false);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildDupSupportRank>c__AnonStorey12F
    {
        internal S2C_GuildDupSupportRank result;

        internal void <>m__CF(GUIEntity entity)
        {
            GuidBattleBossRicherRankPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleBossRicherRankPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveGuildDupSupportRank(this.result, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildTrenchDamageRank>c__AnonStorey12A
    {
        internal S2C_GuildTrenchDamageRank result;

        internal void <>m__C9(GUIEntity entity)
        {
            GuidBattleDamageBarRankPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<GuidBattleDamageBarRankPanel>();
            if (activityGUIEntity != null)
            {
                activityGUIEntity.ReceiveTrenchDamageRank(this.result, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_GuildWholeSvrTrenchDamageRank>c__AnonStorey12C
    {
        internal S2C_GuildWholeSvrTrenchDamageRank result;

        internal void <>m__CB(GUIEntity entity)
        {
            GuidBattleBossDamageRankPanel panel = entity as GuidBattleBossDamageRankPanel;
            if (panel != null)
            {
                panel.ReceiveBossDamageRank(this.result, 1, 10, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_NewLifeRecvFriendReward>c__AnonStorey132
    {
        internal S2C_NewLifeRecvFriendReward result;

        internal void <>m__D2(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            if (panel != null)
            {
                panel.ShowLifeSkillRecvFriendReward(this.result);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_NewLifeSkillEndHangUp>c__AnonStorey131
    {
        internal S2C_NewLifeSkillEndHangUp result;

        internal void <>m__D1(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            if (panel != null)
            {
                panel.ShowLifeSkillCollectReward(this.result);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_RefreshConvoyFlag>c__AnonStorey11B
    {
        internal S2C_RefreshConvoyFlag result;

        internal void <>m__B3(GUIEntity ui)
        {
            DetainsDartSelFlagAndTeamPanel panel = ui as DetainsDartSelFlagAndTeamPanel;
            panel.SetFreshFlagNeed(this.result.curTimes);
            panel.PlayAnim();
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_ReqGuildDupItemDistribute>c__AnonStorey12E
    {
        internal S2C_ReqGuildDupItemDistribute result;

        internal void <>m__CE(GUIEntity entity)
        {
            (entity as GuidBattleBossHandOutPanel).ReceiveBossDamageRank(this.result, false);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_RobConvoyCombatBegin>c__AnonStorey11A
    {
        internal S2C_RobConvoyCombatBegin result;

        internal void <>m__B2(bool isWin, BattleNormalGameType _type, BattleNormalGameResult resultInfo)
        {
            BattleBack battleBackData = new BattleBack {
                securityDataList = BattleSecurityManager.Instance.GetSecurityDataList(),
                end_time = (uint) TimeMgr.Instance.ServerStampTime,
                result = isWin
            };
            SocketMgr.Instance.RequestC2S_RobConvoyCombatEnd(this.result.targetId, this.result.convoyIndex, battleBackData, this.result.type);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveS2C_RobConvoyCombatEnd>c__AnonStorey119
    {
        internal S2C_RobConvoyCombatEnd result;

        internal void <>m__B1(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateDetainsDartInterceptResult(this.result);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveSoulBox>c__AnonStorey137
    {
        internal SocketMgr.<ReceiveSoulBox>c__AnonStorey138 <>f__ref$312;
        internal List<int> card_list;
        internal List<int> item_list;
        internal List<int> item_num_list;
        internal List<int> morph_list;
        internal List<int> morph_num_list;

        internal void <>m__E2()
        {
            this.card_list.Clear();
            this.item_list.Clear();
            foreach (LotteryReward reward in this.<>f__ref$312.res.rewardList)
            {
                if ((reward.item != null) && (reward.item.entry != -1))
                {
                    this.item_list.Add(reward.item.entry);
                    this.item_num_list.Add(reward.item.diff);
                }
                foreach (Card card in reward.card.newCard)
                {
                    this.card_list.Add((int) card.cardInfo.entry);
                }
                foreach (Item item in reward.card.newItem)
                {
                    int cardExCfgByItemPart = CommonFunc.GetCardExCfgByItemPart(item.entry);
                    if (cardExCfgByItemPart == -1)
                    {
                        if (item.entry != -1)
                        {
                            this.item_list.Add(item.entry);
                            this.item_num_list.Add(item.diff);
                        }
                    }
                    else
                    {
                        this.card_list.Add(cardExCfgByItemPart);
                        this.morph_list.Add(cardExCfgByItemPart);
                        this.morph_num_list.Add(item.diff);
                    }
                }
            }
            GameDataMgr.Instance.boostRecruit.ShowRecruitResult(this.card_list, this.item_list, this.item_num_list, this.morph_list, this.morph_num_list, true);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveSoulBox>c__AnonStorey138
    {
        internal S2C_SoulBox res;

        internal void <>m__E3(GUIEntity obj)
        {
            ((YuanZhengRushPanel) obj).UpdateData(this.res.rewardList, this.res.additionalItem);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivetUseItem>c__AnonStorey113
    {
        internal user_title_config utc;

        internal void <>m__A6(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            string str = GameConstant.DefaultTextColor + string.Format(ConfigMgr.getInstance().GetWord(0x7d9), GameConstant.DefaultTextRedColor + this.utc.name + GameConstant.DefaultTextColor);
            box.SetDialog(str, null, null, true);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceivetUseItem>c__AnonStorey114
    {
        internal S2C_UseItem res;

        internal void <>m__A7(GUIEntity obj)
        {
            RewardPanel panel = obj as RewardPanel;
            ActorData.getInstance().UpdateBattleRewardData(this.res.reward);
            panel.ShowBattleReward(this.res.reward);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveVoidTowerCombatEnd>c__AnonStorey13A
    {
        internal S2C_VoidTowerCombatEndReq res;

        internal void <>m__E6(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateTowerPkResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveVoidTowerSmash>c__AnonStorey139
    {
        internal S2C_VoidTowerSmash res;

        internal void <>m__E4(GUIEntity entity)
        {
            (entity as RewardPanel).ShowBattleReward(this.res.reward);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWarmmatchCombatEnd>c__AnonStorey109
    {
        internal S2C_WarmmatchCombatEndReq res;

        internal void <>m__9C(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateWarmmatchPkResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWarmmatchGains>c__AnonStorey10A
    {
        internal S2C_WarmmatchGains res;

        internal void <>m__9D(GUIEntity entity)
        {
            (entity as RewardPanel).ShowBattleReward(this.res.reward);
            ActorData.getInstance().UpdateBattleRewardData(this.res.reward);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWarmmatchTarget>c__AnonStorey108
    {
        internal S2C_WarmmatchTargetReq res;

        internal void <>m__99(GUIEntity obj)
        {
            ((TargetTeamPanel) obj).SetLianXiSaiTargetInfo(this.res);
        }

        internal void <>m__9A(GUIEntity obj)
        {
            SelectHeroPanel panel = (SelectHeroPanel) obj;
            panel.mBattleType = BattleType.WarmmatchPk;
            panel.SetLSXPkTargetInfo(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWorldBoss>c__AnonStorey13B
    {
        internal S2C_WorldBossReq res;

        internal void <>m__E7(GUIEntity obj)
        {
            obj.Achieve<WorldBossPanel>().UpdateDataBossDYing(this.res.data);
        }

        internal void <>m__E8(GUIEntity obj)
        {
            obj.Achieve<WorldBossPanel>().UpdateBossData(this.res.data);
        }

        internal void <>m__E9(GUIEntity obj)
        {
            obj.Achieve<WorldBossPanel>().UpdateBossData(this.res.data);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWorldBossCombatEnd>c__AnonStorey13C
    {
        internal S2C_WorldBossCombatEndReq res;

        internal void <>m__EB(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateWorldBossResult(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <ReceiveWorldBossGains>c__AnonStorey13D
    {
        internal S2C_WorldBossGains res;

        internal void <>m__ED(GUIEntity entity)
        {
            RewardPanel panel = entity as RewardPanel;
            panel.ShowBattleReward(this.res.reward);
            ActorData.getInstance().UpdateBattleRewardData(this.res.reward);
            if (ActorData.getInstance().UserInfo.name == this.res.data.boss.drangon_knight.name)
            {
                panel.ShowTips();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <S2C_CollectExchange>c__AnonStorey102
    {
        internal S2C_CollectExchange res;

        internal void <>m__91(GUIEntity entity)
        {
            (entity as RewardPanel).ShowActiveReward(this.res);
            ActorData.getInstance().GetNeedItemIsOkOrNot(this.res.exchangeDecItems);
            ActorData.getInstance().UpdateItemList(this.res.exchangeDecItems);
            ActorData.getInstance().UpdateActiveData(this.res);
            if (ActivePanel.inst != null)
            {
                ActivePanel.inst.ResetState(this.res.activity_type, this.res.activity_entry, this.res.subEntry, this.res.flag);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <S2C_TXBUYSTOREITEM>c__AnonStorey103
    {
        internal S2C_TXBuyStoreItem res;

        internal void <>m__92(GUIEntity entity)
        {
            (entity as RewardPanel).ShowActiveReward(this.res);
            ActorData.getInstance().UpdateActiveData(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <SendLoop>c__Iterator18 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SocketMgr <>f__this;
        internal float <connectingTime>__3;
        internal Exception <e>__2;
        internal Exception <e>__4;
        internal bool <failed>__0;
        internal bool <isSessionError>__1;
        internal int <len>__6;
        internal CircleBuffer <receiveBuffer>__5;

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
                    if (!this.<>f__this.IsSessionNoInit(this.<>f__this.send_pak))
                    {
                        this.<failed>__0 = false;
                        this.<isSessionError>__1 = false;
                        this.<>f__this._tcpSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        this.<>f__this._hasRecvBuf = false;
                        this.<>f__this._tcpSock.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, true);
                        this.<>f__this._tcpSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                        this.<>f__this._tcpSock.ReceiveTimeout = SocketMgr.SOCKET_CONNECT_TIME_OUT * 0x3e8;
                        this.<>f__this._tcpSock.SendTimeout = SocketMgr.SOCKET_CONNECT_TIME_OUT * 0x3e8;
                        this.<>f__this.isEndConnect = false;
                        this.<>f__this.DebugLog("Start BeginConnect");
                        try
                        {
                            this.<>f__this._tcpSock.BeginConnect(this.<>f__this.ip, this.<>f__this.port, new AsyncCallback(this.<>f__this.OnEndConnect), null);
                        }
                        catch (Exception exception)
                        {
                            this.<e>__2 = exception;
                            Debug.LogWarning(this.<e>__2);
                        }
                        this.<connectingTime>__3 = 0f;
                        while (!this.<>f__this._tcpSock.Connected && (this.<connectingTime>__3 < SocketMgr.SOCKET_CONNECT_BEGIN_TIME_OUT))
                        {
                            this.<connectingTime>__3 += Time.deltaTime;
                            if (this.<connectingTime>__3 > 2f)
                            {
                                this.<>f__this.DoWaitGUI();
                            }
                            if (!this.<>f__this.isEndConnect)
                            {
                                goto Label_0265;
                            }
                            if (this.<>f__this._tcpSock.Connected)
                            {
                                goto Label_027D;
                            }
                            this.$current = new WaitForSeconds(0.5f);
                            this.$PC = 1;
                            goto Label_0642;
                        Label_01EC:
                            this.<connectingTime>__3 += 0.5f;
                            this.<>f__this.isEndConnect = false;
                            try
                            {
                                this.<>f__this._tcpSock.BeginConnect(this.<>f__this.ip, this.<>f__this.port, new AsyncCallback(this.<>f__this.OnEndConnect), null);
                            }
                            catch (Exception exception2)
                            {
                                this.<e>__4 = exception2;
                                Debug.LogWarning(this.<e>__4);
                            }
                            goto Label_027D;
                        Label_0265:
                            this.$current = 0;
                            this.$PC = 2;
                            goto Label_0642;
                        Label_027D:
                            if (this.<connectingTime>__3 > 2f)
                            {
                                this.<>f__this.DoWaitGUI();
                            }
                        }
                        if (this.<>f__this.IsSessionNoInit(this.<>f__this.send_pak))
                        {
                            this.<failed>__0 = true;
                            this.<isSessionError>__1 = true;
                        }
                        if (!this.<isSessionError>__1 && this.<>f__this._tcpSock.Connected)
                        {
                            this.<>f__this.StartSend();
                            this.<receiveBuffer>__5 = new CircleBuffer();
                            this.<>f__this._size = 0;
                            this.<>f__this._opcode = (OpcodeType) 0;
                            this.<connectingTime>__3 = 0f;
                            while ((this.<>f__this._tcpSock.Connected && !this.<>f__this._hasRecvBuf) && (this.<connectingTime>__3 < SocketMgr.SOCKET_CONNECT_TIME_OUT))
                            {
                                this.$current = new WaitForEndOfFrame();
                                this.$PC = 3;
                                goto Label_0642;
                            Label_035C:
                                this.<connectingTime>__3 += Time.deltaTime;
                                if (this.<connectingTime>__3 > 2f)
                                {
                                    this.<>f__this.DoWaitGUI();
                                }
                                if (this.<>f__this._tcpSock.Poll(1, SelectMode.SelectRead))
                                {
                                    try
                                    {
                                        this.<len>__6 = this.<>f__this._tcpSock.Receive(this.<>f__this.volite_buf, 0, CircleBuffer.MAX_RECEIVE_BUFFER_SIZE, SocketFlags.None);
                                        this.<receiveBuffer>__5.Enlarge(this.<>f__this.volite_buf, this.<len>__6);
                                    }
                                    catch (Exception)
                                    {
                                        this.<>f__this.DebugLog("receving failed:");
                                        this.<failed>__0 = true;
                                        break;
                                    }
                                    this.<>f__this.OnReceive(this.<receiveBuffer>__5);
                                }
                            }
                        }
                        else
                        {
                            this.<>f__this.DebugLog("No Connected");
                        }
                        break;
                    }
                    this.<>f__this.isSending = false;
                    this.<>f__this.CloseWaitGUI();
                    this.<>f__this.UnlockGUI();
                    goto Label_0640;

                case 1:
                    goto Label_01EC;

                case 2:
                    goto Label_027D;

                case 3:
                    goto Label_035C;

                default:
                    goto Label_0640;
            }
            if ((!this.<>f__this._tcpSock.Connected || !this.<>f__this._hasRecvBuf) || (this.<connectingTime>__3 >= SocketMgr.SOCKET_CONNECT_TIME_OUT))
            {
                this.<>f__this.DebugLog(string.Concat(new object[] { "Disconnected Connected: ", this.<>f__this._tcpSock.Connected, " hasbuf:", this.<>f__this._hasRecvBuf, " time:", this.<connectingTime>__3 }));
                this.<failed>__0 = true;
            }
            try
            {
                if (this.<>f__this._tcpSock.Connected)
                {
                    this.<>f__this._tcpSock.Shutdown(SocketShutdown.Both);
                    this.<>f__this._tcpSock.Close();
                }
            }
            catch (Exception)
            {
            }
            this.<>f__this._tcpSock = null;
            this.<>f__this.CloseWaitGUI();
            this.<>f__this.UnlockGUI();
            if (!this.<failed>__0)
            {
                this.<>f__this._reconnectTimes = 0;
                if (!this.<>f__this.isReSending)
                {
                    this.<>f__this.isSending = false;
                }
            }
            else if (this.<isSessionError>__1)
            {
                this.<>f__this._reconnectTimes = 0;
                if (!this.<>f__this.isReSending)
                {
                    this.<>f__this.isSending = false;
                }
            }
            else
            {
                this.<>f__this.isReSending = false;
                this.<>f__this._reconnectTimes++;
                if (this.<>f__this._reconnectTimes < SocketMgr.SOCKET_CONNECT__RECONNECT_TIMES)
                {
                    this.<>f__this.ReSendLastPak();
                }
                else
                {
                    this.<>f__this._reconnectTimes = 0;
                    this.<>f__this.onConnectFailed();
                }
            }
            this.$PC = -1;
        Label_0640:
            return false;
        Label_0642:
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

