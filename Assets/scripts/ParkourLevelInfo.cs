using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParkourLevelInfo
{
    public int count;
    public List<string> propPointIndex;

    public string GetPropPointIndex(int mapCount)
    {
        char[] separator = new char[] { ',' };
        string[] strArray = this.propPointIndex[mapCount].Split(separator);
        return strArray[UnityEngine.Random.Range(0, strArray.Length)];
    }
}

