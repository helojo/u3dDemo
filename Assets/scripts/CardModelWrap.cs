using System;
using System.Collections.Generic;

[Serializable]
public class CardModelWrap
{
    public List<CardModelHang> Hangobj = new List<CardModelHang>();
    public string modelReplace;
    public string textureName;

    public bool IsHasOtherResource()
    {
        return false;
    }

    [Serializable]
    public class CardModelHang
    {
        public HangPointType hangPoint;
        public string objName;
    }
}

