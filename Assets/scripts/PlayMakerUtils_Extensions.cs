using System;
using System.Collections;
using UnityEngine;

public static class PlayMakerUtils_Extensions
{
    public static int IndexOf(ArrayList target, object value)
    {
        return IndexOf(target, value, 0, target.Count);
    }

    public static int IndexOf(ArrayList target, object value, int startIndex)
    {
        if (startIndex > target.Count)
        {
            throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
        }
        return IndexOf(target, value, startIndex, target.Count - startIndex);
    }

    public static int IndexOf(ArrayList target, object value, int startIndex, int count)
    {
        Debug.Log(startIndex + " " + count);
        if ((startIndex < 0) || (startIndex >= target.Count))
        {
            throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
        }
        if ((count < 0) || (startIndex > (target.Count - count)))
        {
            throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_Count");
        }
        if (target.Count != 0)
        {
            int num = startIndex + count;
            if (value == null)
            {
                for (int j = startIndex; j < num; j++)
                {
                    if (target[j] == null)
                    {
                        return j;
                    }
                }
                return -1;
            }
            for (int i = startIndex; i < num; i++)
            {
                if ((target[i] != null) && target[i].Equals(value))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static int LastIndexOf(ArrayList target, object value)
    {
        return LastIndexOf(target, value, target.Count - 1, target.Count);
    }

    public static int LastIndexOf(ArrayList target, object value, int startIndex)
    {
        return LastIndexOf(target, value, startIndex, startIndex + 1);
    }

    public static int LastIndexOf(ArrayList target, object value, int startIndex, int count)
    {
        ArrayList list = target;
        if (list.Count != 0)
        {
            if ((startIndex < 0) || (startIndex >= target.Count))
            {
                throw new ArgumentOutOfRangeException("startIndex", "ArgumentOutOfRange_Index");
            }
            if ((count < 0) || (startIndex > (target.Count - count)))
            {
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_Count");
            }
            int num = (startIndex + count) - 1;
            if (value == null)
            {
                for (int j = num; j >= startIndex; j--)
                {
                    if (list[j] == null)
                    {
                        return j;
                    }
                }
                return -1;
            }
            for (int i = num; i >= startIndex; i--)
            {
                if ((list[i] != null) && list[i].Equals(value))
                {
                    return i;
                }
            }
        }
        return -1;
    }
}

