using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class TssSdk
{
    private const uint GAME_ID = 0x270f;

    [DllImport("tersafe")]
    public static extern void tss_del_report_data(int info);
    [DllImport("tersafe")]
    private static extern void tss_enable_get_report_data();
    [DllImport("tersafe")]
    public static extern int tss_get_report_data();
    [DllImport("tersafe")]
    private static extern void tss_log_str(string sdk_version);
    [DllImport("tersafe")]
    private static extern AntiDecryptResult tss_sdk_decryptpacket(DecryptPkgInfo info);
    [DllImport("tersafe")]
    private static extern AntiEncryptResult tss_sdk_encryptpacket(EncryptPkgInfo info);
    [DllImport("tersafe")]
    private static extern void tss_sdk_init(InitInfo info);
    [DllImport("tersafe")]
    private static extern void tss_sdk_rcv_anti_data(AntiDataInfo info);
    [DllImport("tersafe")]
    private static extern void tss_sdk_setgamestatus(GameStatusInfo info);
    [DllImport("tersafe")]
    private static extern void tss_sdk_setuserinfo(UserInfoStrStr info);
    public static AntiDecryptResult TssSdkDecrypt(byte[] src, uint src_len, ref byte[] tar, ref uint tar_len)
    {
        AntiDecryptResult result = AntiDecryptResult.ANTI_DECRYPT_FAIL;
        GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
        GCHandle handle2 = GCHandle.Alloc(tar, GCHandleType.Pinned);
        if (handle.IsAllocated && handle2.IsAllocated)
        {
            DecryptPkgInfo info = new DecryptPkgInfo {
                encrypt_data_ = handle.AddrOfPinnedObject(),
                encrypt_data_len = src_len,
                game_pkg_ = handle2.AddrOfPinnedObject(),
                game_pkg_len_ = tar_len
            };
            result = tss_sdk_decryptpacket(info);
            tar_len = info.game_pkg_len_;
        }
        if (handle.IsAllocated)
        {
            handle.Free();
        }
        if (handle2.IsAllocated)
        {
            handle2.Free();
        }
        return result;
    }

    public static AntiEncryptResult TssSdkEncrypt(int cmd_id, byte[] src, uint src_len, ref byte[] tar, ref uint tar_len)
    {
        AntiEncryptResult result = AntiEncryptResult.ANTI_NOT_NEED_ENCRYPT;
        GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
        GCHandle handle2 = GCHandle.Alloc(tar, GCHandleType.Pinned);
        if (handle.IsAllocated && handle2.IsAllocated)
        {
            EncryptPkgInfo info = new EncryptPkgInfo {
                cmd_id_ = cmd_id,
                game_pkg_ = handle.AddrOfPinnedObject(),
                game_pkg_len_ = src_len,
                encrpty_data_ = handle2.AddrOfPinnedObject(),
                encrypt_data_len_ = tar_len
            };
            result = tss_sdk_encryptpacket(info);
            tar_len = info.encrypt_data_len_;
        }
        if (handle.IsAllocated)
        {
            handle.Free();
        }
        if (handle2.IsAllocated)
        {
            handle2.Free();
        }
        return result;
    }

    public static void TssSdkInit(uint gameId)
    {
        InitInfo info;
        info = new InitInfo {
            size_ = Marshal.SizeOf(info),
            game_id_ = gameId,
            send_data_to_svr = null
        };
        tss_sdk_init(info);
        tss_enable_get_report_data();
        tss_log_str(TssSdkVersion.GetSdkVersion());
        tss_log_str(TssSdtVersion.GetSdtVersion());
    }

    public static void TssSdkRcvAntiData(byte[] data, ushort length)
    {
        AntiDataInfo info = new AntiDataInfo();
        GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        if (handle.IsAllocated)
        {
            IntPtr ptr = handle.AddrOfPinnedObject();
            info.anti_data = ptr;
            info.anti_data_len = length;
            tss_sdk_rcv_anti_data(info);
            handle.Free();
        }
    }

    public static void TssSdkSetGameStatus(EGAMESTATUS gameStatus)
    {
        GameStatusInfo info;
        info = new GameStatusInfo {
            size_ = Marshal.SizeOf(info),
            game_status_ = (uint) gameStatus
        };
        tss_sdk_setgamestatus(info);
    }

    public static void TssSdkSetUserInfo(EENTRYID entryId, string uin, string appId)
    {
        UserInfoStrStr str;
        str = new UserInfoStrStr {
            size = Marshal.SizeOf(str),
            entrance_id = (uint) entryId,
            uin = new UIN_STR()
        };
        str.uin.type = 2;
        str.uin.uin.uin = uin;
        str.app_id = new APPID_STR();
        str.app_id.type = 2;
        str.app_id.app_id.app_id = appId;
        tss_sdk_setuserinfo(str);
    }

    [StructLayout(LayoutKind.Explicit, Size=6)]
    public class AntiDataInfo
    {
        [FieldOffset(2)]
        public IntPtr anti_data;
        [FieldOffset(0)]
        public ushort anti_data_len;
    }

    public enum AntiDecryptResult
    {
        ANTI_DECRYPT_OK,
        ANTI_DECRYPT_FAIL
    }

    public enum AntiEncryptResult
    {
        ANTI_ENCRYPT_OK,
        ANTI_NOT_NEED_ENCRYPT
    }

    [StructLayout(LayoutKind.Sequential)]
    public class APPID_INT
    {
        public uint type;
        public TssSdk.AppIdInfoInt app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class APPID_STR
    {
        public uint type;
        public TssSdk.AppIdInfoStr app_id;
    }

    [StructLayout(LayoutKind.Explicit, Size=0x40)]
    public struct AppIdInfoInt
    {
        [FieldOffset(0)]
        public uint app_id;
    }

    [StructLayout(LayoutKind.Explicit, Size=0x40)]
    public struct AppIdInfoStr
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40), FieldOffset(0)]
        public string app_id;
    }

    [StructLayout(LayoutKind.Explicit, Size=0x10)]
    public class DecryptPkgInfo
    {
        [FieldOffset(0)]
        public IntPtr encrypt_data_;
        [FieldOffset(4)]
        public uint encrypt_data_len;
        [FieldOffset(8)]
        public IntPtr game_pkg_;
        [FieldOffset(12)]
        public uint game_pkg_len_;
    }

    public enum EAPPIDTYPE
    {
        APP_ID_TYPE_INT = 1,
        APP_ID_TYPE_STR = 2
    }

    public enum EENTRYID
    {
        ENTRY_ID_MM = 2,
        ENTRY_ID_OTHERS = 3,
        ENTRY_ID_QZONE = 1
    }

    public enum EGAMESTATUS
    {
        GAME_STATUS_BACKEND = 2,
        GAME_STATUS_FRONTEND = 1
    }

    [StructLayout(LayoutKind.Explicit, Size=20)]
    public class EncryptPkgInfo
    {
        [FieldOffset(0)]
        public int cmd_id_;
        [FieldOffset(12)]
        public IntPtr encrpty_data_;
        [FieldOffset(0x10)]
        public uint encrypt_data_len_;
        [FieldOffset(4)]
        public IntPtr game_pkg_;
        [FieldOffset(8)]
        public uint game_pkg_len_;
    }

    public enum EUINTYPE
    {
        UIN_TYPE_INT = 1,
        UIN_TYPE_STR = 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public class GameStatusInfo
    {
        public uint size_;
        public uint game_status_;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class InitInfo
    {
        public uint size_;
        public uint game_id_;
        public TssSdk.SendDataToSvrDelegate send_data_to_svr;
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int SendDataToSvrDelegate(TssSdk.AntiDataInfo info);

    [StructLayout(LayoutKind.Explicit, Size=6)]
    public struct TmpAntiDataInfo
    {
        [FieldOffset(2)]
        public int anti_data;
        [FieldOffset(0)]
        public ushort anti_data_len;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UIN_INT
    {
        public uint type;
        public TssSdk.UinInfoInt uin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UIN_STR
    {
        public uint type;
        public TssSdk.UinInfoStr uin;
    }

    [StructLayout(LayoutKind.Explicit, Size=0x40)]
    public struct UinInfoInt
    {
        [FieldOffset(0)]
        public uint uin;
    }

    [StructLayout(LayoutKind.Explicit, Size=0x40)]
    public struct UinInfoStr
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=0x40), FieldOffset(0)]
        public string uin;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoIntInt
    {
        public uint size;
        public uint entrance_id;
        public TssSdk.UIN_INT uin;
        public TssSdk.APPID_INT app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoIntStr
    {
        public uint size;
        public uint entrance_id;
        public TssSdk.UIN_INT uin;
        public TssSdk.APPID_STR app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoStrInt
    {
        public uint size;
        public uint entrance_id;
        public TssSdk.UIN_STR uin;
        public TssSdk.APPID_INT app_id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class UserInfoStrStr
    {
        public uint size;
        public uint entrance_id;
        public TssSdk.UIN_STR uin;
        public TssSdk.APPID_STR app_id;
    }
}

