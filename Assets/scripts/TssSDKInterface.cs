using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class TssSDKInterface : MonoBehaviour
{
    private static TssSDKInterface _instance;
    private float nextRevDataTime;
    private float nextSendDataTime;
    public int revDataLength;
    private static readonly float revDataMaxTime = 3f;
    public int revTimes;
    public int sendDataLength;
    private static readonly float sendDataMaxTime = 5f;
    public int sendTimes;

    private void DebugLog(string log)
    {
    }

    public static TssSDKInterface GetInstance()
    {
        return _instance;
    }

    public void InitTSSSDK()
    {
        int num = 0x9ec;
        TssSdk.TssSdkInit((uint) num);
        Debug.Log("TssSdkInit gameid: " + num);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        TssSdk.EGAMESTATUS gameStatus = !pauseStatus ? TssSdk.EGAMESTATUS.GAME_STATUS_FRONTEND : TssSdk.EGAMESTATUS.GAME_STATUS_BACKEND;
        TssSdk.TssSdkSetGameStatus(gameStatus);
        Debug.Log("TssSdkSetGameStatus " + gameStatus);
    }

    public static void OnLoginOK(TencentType type, string openID, string appID)
    {
        TssSdk.EENTRYID entryId = (type != TencentType.QQ) ? TssSdk.EENTRYID.ENTRY_ID_MM : TssSdk.EENTRYID.ENTRY_ID_QZONE;
        TssSdk.TssSdkSetUserInfo(entryId, openID, appID);
        Debug.Log(string.Concat(new object[] { "TssSdkSetUserInfo ", entryId, " opendID: ", openID, " appID: ", appID }));
    }

    public void ReceiveGetDataFromServer(byte[] data, int dataLength)
    {
        this.DebugLog(string.Concat(new object[] { "TSS Recv: ", this.revTimes, " ", DateTime.Now.TimeOfDay, " ", (ushort) dataLength, " ", BitConverter.ToString(data).Replace("-", string.Empty) }));
        TssSdk.TssSdkRcvAntiData(data, (ushort) dataLength);
        this.revTimes++;
    }

    private void RequestGetDataFromServer()
    {
    }

    private void SendData2Server()
    {
        int num = TssSdk.tss_get_report_data();
        if (num != 0)
        {
            IntPtr ptr = new IntPtr(num);
            TssSdk.TmpAntiDataInfo info = (TssSdk.TmpAntiDataInfo) Marshal.PtrToStructure(ptr, typeof(TssSdk.TmpAntiDataInfo));
            TssSdk.AntiDataInfo info2 = new TssSdk.AntiDataInfo {
                anti_data_len = info.anti_data_len,
                anti_data = new IntPtr(info.anti_data)
            };
            byte[] destination = new byte[info2.anti_data_len];
            Marshal.Copy(info2.anti_data, destination, 0, info2.anti_data_len);
            SocketMgr.Instance.SendTssData2Server(destination, info2.anti_data_len);
            TssSdk.tss_del_report_data(num);
            this.sendTimes++;
        }
    }

    private void Start()
    {
        _instance = this;
        this.InitTSSSDK();
        this.nextSendDataTime = sendDataMaxTime;
        this.nextRevDataTime = revDataMaxTime;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Update()
    {
        if (ActorData.getInstance().IsInited() && (this.nextSendDataTime > 0f))
        {
            this.nextSendDataTime -= Time.deltaTime;
            if (this.nextSendDataTime <= 0f)
            {
                this.nextSendDataTime = sendDataMaxTime;
                this.SendData2Server();
            }
        }
    }
}

