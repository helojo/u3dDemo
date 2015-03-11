using System;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;

public class FPSShow : MonoBehaviour
{
    private float fixedFps;
    private float fps;
    private Queue<float> lastDeltaTimes;
    private int lastFps;
    private float lastFpsTime;
    public RenderTexture temporary;

    private void FixedUpdate()
    {
        this.lastFps++;
        if ((this.lastFpsTime + 1f) < Time.realtimeSinceStartup)
        {
            this.fixedFps = this.lastFps;
            this.lastFpsTime = Time.realtimeSinceStartup;
            this.lastFps = 0;
        }
    }

    private void OnGUI()
    {
        if (GameDefine.getInstance().isDebugLog)
        {
            GUIStyle style = new GUIStyle {
                normal = { textColor = Color.green },
                alignment = TextAnchor.MiddleLeft
            };
            new GUIStyle { normal = { textColor = Color.black }, alignment = TextAnchor.MiddleLeft };
            float num = 20f;
            string text = "FPS: " + this.fps.ToString("f2") + " FixedPfs:" + this.fixedFps.ToString("f2");
            string str2 = GameDefine.getInstance().gameServerIP + ":" + GameDefine.getInstance().gameServerPort;
            if (ActorData.getInstance().SessionInfo != null)
            {
                str2 = str2 + (!SocketMgr.Instance.isSending ? "Finish: " : " sending: ") + SocketMgr.Instance.sendingOpCode.ToString();
            }
            string str3 = string.Empty;
            if (SocketMgr.Instance != null)
            {
                object[] objArray1 = new object[] { "BoxCollider:", SocketMgr.Instance.labelAutoSetMacth, "Vec:", SocketMgr.Instance.ruleBoxCollider };
                str3 = string.Concat(objArray1);
            }
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 9f), 200f, 30f), str3, style);
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 1f), 200f, 30f), str2, style);
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 2f), 200f, 30f), text, style);
            string str4 = string.Format("Sent:{0}k Received:{1}k ", (((float) SocketMgr.TotalBytesSent) / 1024f).ToString("0.00"), (((float) SocketMgr.TotalBytesReceived) / 1024f).ToString("0.00"));
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 3f), 200f, 30f), str4, style);
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 4f), 200f, 30f), GameDefine.getInstance().WholeVersion, style);
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 5f), 200f, 30f), string.Format("System: RAM:{0}MB VRAM:{1}MB ", SystemInfo.systemMemorySize.ToString("d4"), SystemInfo.graphicsMemorySize.ToString("d4")), style);
            if (TimeMgr.Instance != null)
            {
                string str5 = string.Format("Server NOW:{0:yy/MM/dd HH:mm:ss}", TimeMgr.Instance.ServerDateTime);
                GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 6f), 300f, 30f), str5, style);
            }
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 7f), 300f, 30f), string.Format("QQ:{1}({0})", XSingleton<SocialFriend>.Singleton.State, XSingleton<SocialFriend>.Singleton.Message), style);
            GUI.Label(new Rect(10f, (Screen.height - 10) - (num * 8f), 300f, 30f), string.Format("newBie Lock{0} UILock{1} ", GUIMgr.Instance.NGUICamera.HighPriorityLocked, GUIMgr.Instance.NGUICamera.Locked), style);
        }
    }

    private void Start()
    {
        this.lastDeltaTimes = new Queue<float>();
    }

    private void Update()
    {
        this.lastDeltaTimes.Enqueue(Time.deltaTime / Time.timeScale);
        if (this.lastDeltaTimes.Count > 10)
        {
            this.lastDeltaTimes.Dequeue();
        }
        float num = 0f;
        int count = this.lastDeltaTimes.Count;
        if (count != 0)
        {
            foreach (float num3 in this.lastDeltaTimes)
            {
                num += num3;
            }
            this.fps = ((float) count) / num;
        }
    }
}

