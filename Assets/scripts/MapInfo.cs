using System;

public class MapInfo
{
    public float distanceFromLastProp;
    public float hight;
    public int propEntry;
    public string propName;
    public float trackIndex;

    public MapInfo(string p, float t, float d, float h, int e)
    {
        this.propName = p;
        this.trackIndex = t;
        this.distanceFromLastProp = d;
        this.hight = h;
        this.propEntry = e;
    }
}

