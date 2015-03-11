using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class TypeConvertUtil
{
    public static int bytesToint(byte[] bytes, int offset, bool flag)
    {
        if (flag)
        {
            return (((bytes[offset] | (bytes[offset + 1] << 8)) | (bytes[offset + 2] << 0x10)) | (bytes[offset + 3] << 0x18));
        }
        return ((((bytes[offset] << 0x18) | (bytes[offset + 1] << 0x10)) | (bytes[offset + 2] << 8)) | bytes[offset + 3]);
    }

    public static short bytesToShort(byte[] bytes, bool flag)
    {
        if (flag)
        {
            return (short) ((bytes[0] & 0xff) | ((bytes[1] & 0xff) << 8));
        }
        return (short) (((bytes[0] & 0xff) << 8) | (bytes[1] & 0xff));
    }

    public static short bytesToShort(byte[] bytes, int offset, bool flag)
    {
        if (flag)
        {
            return (short) (bytes[offset] | (bytes[offset + 1] << 8));
        }
        return (short) ((bytes[offset] << 8) | bytes[offset + 1]);
    }

    public static Stream BytesToStream(byte[] bytes)
    {
        return new MemoryStream(bytes);
    }

    public static byte[] bytesUnite(byte[] bs1, byte[] bs2)
    {
        byte[] buffer = new byte[bs1.Length + bs2.Length];
        int index = 0;
        for (int i = 0; i < bs1.Length; i++)
        {
            buffer[index] = bs1[i];
            index++;
        }
        for (int j = 0; j < bs2.Length; j++)
        {
            buffer[index] = bs2[j];
            index++;
        }
        return buffer;
    }

    public static string Color2Hex(Color color)
    {
        string[] textArray1 = new string[] { "[", Mathf.CeilToInt(color.r * 255f).ToString("X2"), Mathf.CeilToInt(color.g * 255f).ToString("X2"), Mathf.CeilToInt(color.b * 255f).ToString("X2"), "]" };
        return string.Concat(textArray1);
    }

    public static string CurrencyDuoConvert(CurrencyType type, int value, bool case_reward = true)
    {
        int num = 0;
        switch (type)
        {
            case CurrencyType.Money:
                num = 0x2a30;
                break;

            case CurrencyType.Stone:
                num = 0x2a31;
                break;

            case CurrencyType.Courage:
                num = 0x2a33;
                break;

            case CurrencyType.Title:
                num = 0x2a32;
                break;

            case CurrencyType.FriendShip:
                num = 0x2a39;
                break;

            case CurrencyType.SkillBook:
                num = 0x2a34;
                break;

            case CurrencyType.Wine:
                num = 0x2a36;
                break;

            case CurrencyType.Badge:
            case CurrencyType.ShopBadge:
                num = 0x2a38;
                break;

            case CurrencyType.PhyForce:
                num = 0x2a37;
                break;

            case CurrencyType.Exp:
                num = 0x2a3a;
                break;

            default:
                return string.Empty;
        }
        string word = ConfigMgr.getInstance().GetWord(num);
        if (case_reward)
        {
            word = word + "x";
        }
        return (word + value.ToString());
    }

    public static string formatMd5(string input)
    {
        string str = input;
        return str.Insert(20, "-").Insert(0x10, "-").Insert(12, "-").Insert(8, "-");
    }

    public static string getMd5Hash(string input)
    {
        byte[] buffer = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            builder.Append(buffer[i].ToString("x2"));
        }
        return builder.ToString();
    }

    public static byte[] intToBytes(int s, bool flag)
    {
        byte[] buffer = new byte[4];
        if (flag)
        {
            buffer[0] = (byte) s;
            buffer[1] = (byte) (s >> 8);
            buffer[2] = (byte) (s >> 0x10);
            buffer[3] = (byte) (s >> 0x18);
            return buffer;
        }
        buffer[0] = (byte) (s >> 0x18);
        buffer[1] = (byte) (s >> 0x10);
        buffer[2] = (byte) (s >> 8);
        buffer[3] = (byte) s;
        return buffer;
    }

    public static byte[] shortToBytes(short s, bool flag)
    {
        byte[] buffer = new byte[2];
        if (flag)
        {
            buffer[0] = (byte) s;
            buffer[1] = (byte) (s >> 8);
            return buffer;
        }
        buffer[0] = (byte) (s >> 8);
        buffer[1] = (byte) s;
        return buffer;
    }

    public static byte[] StreamToBytes(Stream sm)
    {
        byte[] buffer = new byte[sm.Length];
        sm.Read(buffer, 0, buffer.Length);
        sm.Seek(0L, SeekOrigin.Begin);
        return buffer;
    }

    public enum CurrencyType
    {
        Unknown,
        Money,
        Stone,
        Courage,
        Title,
        FriendShip,
        SkillBook,
        Wine,
        Badge,
        ShopBadge,
        PhyForce,
        Exp
    }
}

