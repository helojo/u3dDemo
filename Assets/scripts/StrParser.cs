using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public static class StrParser
{
    private static CultureInfo _provider;
    public static readonly Color INVALID_COLOR;
    public static readonly string[,] spcChars;
    public static readonly char[] splitter = new char[] { ',', '|' };

    static StrParser()
    {
        string[] textArray1 = new string[,] { { "#quot#", "\"" }, { "#lt#", "<" }, { "#gt#", ">" }, { "#amp#", "&" }, { "#apos#", "'" } };
        spcChars = textArray1;
        INVALID_COLOR = new Color(1f, 1f, 1f, 0f);
        _provider = CultureInfo.InvariantCulture;
    }

    public static string ClipStringByAsciiLength(string str, int length)
    {
        string str2 = string.Empty;
        int num = 0;
        char[] chArray = str.ToCharArray();
        for (int i = 0; i < chArray.Length; i++)
        {
            if ((chArray[i] >= '一') && (chArray[i] <= 0x9fa5))
            {
                str2 = str2 + chArray[i];
                num += 2;
            }
            else
            {
                str2 = str2 + chArray[i];
                num++;
            }
            if (num >= length)
            {
                return str2;
            }
        }
        return str2;
    }

    public static int GetAsciiLength(string str)
    {
        int num = 0;
        char[] chArray = str.ToCharArray();
        for (int i = 0; i < chArray.Length; i++)
        {
            if ((chArray[i] >= '一') && (chArray[i] <= 0x9fa5))
            {
                num += 2;
            }
            else
            {
                num++;
            }
        }
        return num;
    }

    public static string Null2Empty(string str)
    {
        return ((str != null) ? str : string.Empty);
    }

    public static bool ParseBool(string str, bool defVal)
    {
        bool flag;
        if (str == "1")
        {
            return true;
        }
        if (str == "0")
        {
            return false;
        }
        if (bool.TryParse(str, out flag))
        {
            return flag;
        }
        return defVal;
    }

    public static Color ParseColor(string str)
    {
        if (str == null)
        {
            return INVALID_COLOR;
        }
        string[] strArray = str.Split(splitter);
        if (strArray.Length < 4)
        {
            return INVALID_COLOR;
        }
        return new Color(float.Parse(strArray[0]) / 255f, float.Parse(strArray[1]) / 255f, float.Parse(strArray[2]) / 255f, float.Parse(strArray[3]) / 255f);
    }

    public static int ParseDecInt(string str)
    {
        return ParseDecInt(str, 0);
    }

    public static int ParseDecInt(string str, int defVal)
    {
        int num;
        if (int.TryParse(str, NumberStyles.Integer, _provider, out num))
        {
            return num;
        }
        return defVal;
    }

    public static List<int> ParseDecIntList(string str, int defVal)
    {
        List<int> list = new List<int>();
        if (str != null)
        {
            string[] strArray = str.Split(splitter);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(ParseDecInt(strArray[i], defVal));
            }
        }
        return list;
    }

    public static double ParseDouble(string str, double defVal)
    {
        double num;
        if (double.TryParse(str, out num))
        {
            return num;
        }
        return defVal;
    }

    public static T ParseEnum<T>(string val) where T: struct
    {
        System.Type enumType = typeof(T);
        return (T) Enum.Parse(enumType, val, true);
    }

    public static float ParseFloat(string str)
    {
        return ParseFloat(str, 0f);
    }

    public static float ParseFloat(string str, float defVal)
    {
        float result = 0f;
        if (float.TryParse(str, out result))
        {
            return result;
        }
        return defVal;
    }

    public static List<float> ParseFloatList(string str, float defVal)
    {
        List<float> list = new List<float>();
        if (str != null)
        {
            string[] strArray = str.Split(splitter);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(ParseFloat(strArray[i], defVal));
            }
        }
        return list;
    }

    public static int ParseHexInt(string str, int defVal)
    {
        int num;
        if (int.TryParse(str, NumberStyles.HexNumber, _provider, out num))
        {
            return num;
        }
        return defVal;
    }

    public static List<int> ParseHexIntList(string str, int defVal)
    {
        List<int> list = new List<int>();
        if (str != null)
        {
            string[] strArray = str.Split(splitter);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(ParseHexInt(strArray[i], defVal));
            }
        }
        return list;
    }

    public static long ParseInt64(string str, long defVal)
    {
        long num;
        if (long.TryParse(str, out num))
        {
            return num;
        }
        return defVal;
    }

    public static List<long> ParseLongList(string str, long defVal)
    {
        List<long> list = new List<long>();
        if (str != null)
        {
            string[] strArray = str.Split(splitter);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(ParseInt64(strArray[i], defVal));
            }
        }
        return list;
    }

    public static Rect ParseRect(string str)
    {
        if (str == null)
        {
            return new Rect(0f, 0f, 0f, 0f);
        }
        string[] strArray = str.Split(splitter);
        if (strArray.Length < 4)
        {
            return new Rect(0f, 0f, 0f, 0f);
        }
        return new Rect(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]), float.Parse(strArray[3]));
    }

    public static List<string> ParseStringList(string str)
    {
        return ParseStringList(str, ";");
    }

    public static List<string> ParseStringList(string str, string seperator)
    {
        List<string> list = new List<string>();
        if (str != null)
        {
            string[] strArray = str.Split(seperator.ToCharArray());
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(strArray[i]);
            }
        }
        return list;
    }

    public static Vector2 ParseVec2(string str)
    {
        return ParseVec2(str, splitter);
    }

    public static Vector2 ParseVec2(string str, char[] split)
    {
        if (str == null)
        {
            return Vector2.zero;
        }
        string[] strArray = str.Split(split);
        if (strArray.Length < 2)
        {
            return Vector2.zero;
        }
        return new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1]));
    }

    public static Vector3 ParseVec3(string str)
    {
        if (str == null)
        {
            return (Vector3) Vector2.zero;
        }
        string[] strArray = str.Split(splitter);
        if (strArray.Length < 3)
        {
            return Vector3.zero;
        }
        return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
    }

    public static List<Vector2> ParseVector2List(string str)
    {
        List<Vector2> list = new List<Vector2>();
        if (str != null)
        {
            string[] strArray = str.Split(splitter);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(ParseVec2(strArray[i], ";".ToCharArray()));
            }
        }
        return list;
    }

    public static string RandomString(int size)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < size; i++)
        {
            char ch = Convert.ToChar(Convert.ToInt32(UnityEngine.Random.Range(0x41, 0x5b)));
            builder.Append(ch);
        }
        return builder.ToString();
    }
}

