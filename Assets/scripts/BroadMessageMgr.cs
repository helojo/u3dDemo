using System;
using System.Collections.Generic;
using UnityEngine;

public class BroadMessageMgr : MonoBehaviour
{
    public static List<BroadMessageInfo> gameBroadMessageInfos = new List<BroadMessageInfo>();
    public static BroadMessageMgr Instance;

    private void Awake()
    {
        Instance = this;
    }

    public class BroadMessageInfo
    {
        public string content;
        public BroadMessageMgr.BroadMsgType Type;
    }

    public enum BroadMsgType
    {
        Platform,
        GameTips,
        GameSvrMsg,
        WorldBoss
    }
}

