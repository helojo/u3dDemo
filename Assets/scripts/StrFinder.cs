using System;
using System.Collections.Generic;
using UnityEngine;

public class StrFinder
{
    public static string CheckMess(string matchStr, string enterStr)
    {
        string str = enterStr.ToLower();
        string str2 = matchStr.ToLower();
        bool flag = false;
        List<int> list = new List<int>();
        int num = 0;
        int index = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (num >= str2.Length)
            {
                flag = false;
                num = 0;
            }
            if ((str[i] == str2[0]) && ((str.Length - i) >= str2.Length))
            {
                if (flag)
                {
                    list.RemoveRange(index, list.Count - index);
                }
                flag = true;
                index = list.Count;
                num = 0;
            }
            if (flag)
            {
                if (str[i] == str2[num])
                {
                    list.Add(i);
                    num++;
                }
                else if (str[i] != ' ')
                {
                    list.RemoveRange(index, list.Count - index);
                    flag = false;
                    num = 0;
                }
            }
        }
        List<int> list2 = new List<int>();
        for (int j = 0; j < (list.Count - 1); j++)
        {
            if (((j + 1) % str2.Length) != 0)
            {
                int num5 = list[j];
                int num6 = list[j + 1];
                while (num5 < (num6 - 1))
                {
                    list2.Add(++num5);
                }
            }
        }
        char[] chArray = enterStr.ToCharArray();
        for (int k = 0; k < list.Count; k++)
        {
            int num8 = list[k];
            chArray[num8] = '*';
        }
        string str3 = string.Empty;
        for (int m = 0; m < chArray.Length; m++)
        {
            if (list2.IndexOf(m) < 0)
            {
                str3 = str3 + chArray[m];
            }
        }
        enterStr = str3;
        return enterStr;
    }

    public static string GetOccurences(string pattern, string targetString)
    {
        char[] chArray = targetString.ToLower().ToCharArray();
        char[] chArray2 = pattern.ToLower().ToCharArray();
        char[] chArray3 = targetString.ToCharArray();
        PrefixArray array = new PrefixArray(pattern);
        int[] transitionArray = array.TransitionArray;
        int index = 0;
        for (int i = 0; i < chArray.Length; i++)
        {
            if (chArray[i] == chArray2[index])
            {
                index++;
            }
            else
            {
                int num3 = transitionArray[index];
                if (((num3 + 1) > chArray2.Length) && (chArray[i] != chArray2[num3 + 1]))
                {
                    index = 0;
                }
                else
                {
                    index = num3;
                }
            }
            if (index == chArray2.Length)
            {
                for (int j = 0; j < index; j++)
                {
                    chArray3[(i - (chArray2.Length - 1)) + j] = '*';
                }
                index = transitionArray[index - 1];
            }
        }
        return new string(chArray3);
    }

    public class PrefixArray
    {
        private int[] hArray;
        private string pattern;

        public PrefixArray(string pattern)
        {
            if ((pattern == null) || (pattern.Length == 0))
            {
                Debug.LogWarning("The pattern may not be null or 0 lenght");
            }
            else
            {
                this.pattern = pattern;
                this.hArray = new int[pattern.Length];
                this.ComputeHArray();
            }
        }

        private void ComputeHArray()
        {
            char[] array = null;
            char[] chArray2 = this.pattern.ToCharArray();
            char charToMatch = chArray2[0];
            this.hArray[0] = 0;
            for (int i = 1; i < this.pattern.Length; i++)
            {
                array = SubCharArray(i, chArray2);
                this.hArray[i] = GetPrefixLegth(array, charToMatch);
            }
        }

        private static int GetPrefixLegth(char[] array, char charToMatch)
        {
            for (int i = 2; i < array.Length; i++)
            {
                if ((array[i] == charToMatch) && IsSuffixExist(i, array))
                {
                    return (array.Length - i);
                }
            }
            return 0;
        }

        private static bool IsSuffixExist(int index, char[] array)
        {
            int num = 0;
            for (int i = index; i < array.Length; i++)
            {
                if (array[i] != array[num])
                {
                    return false;
                }
                num++;
            }
            return true;
        }

        private static char[] SubCharArray(int endIndex, char[] array)
        {
            char[] chArray = new char[endIndex + 1];
            for (int i = 0; i <= endIndex; i++)
            {
                chArray[i] = array[i];
            }
            return chArray;
        }

        public int[] TransitionArray
        {
            get
            {
                return this.hArray;
            }
        }
    }
}

