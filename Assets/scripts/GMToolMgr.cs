using System;
using System.Text;
using UnityEngine;

public class GMToolMgr : MonoBehaviour
{
    private Vector2 logScrollPosition = Vector2.zero;
    private bool showGM;
    public static bool showGMBtn;
    private bool showLog;
    private const int textSize = 0x1c;
    public StringBuilder wlogsb = new StringBuilder();

    private void DoLogWindow(int windowId)
    {
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        this.logScrollPosition = GUILayout.BeginScrollView(this.logScrollPosition, new GUILayoutOption[0]);
        if (GUILayout.Button("Close", new GUILayoutOption[0]))
        {
            this.showLog = !this.showLog;
        }
        if (GUILayout.Button("Clear", new GUILayoutOption[0]))
        {
            this.wlogsb.Remove(0, this.wlogsb.Length);
        }
        GUILayout.Label(this.wlogsb.ToString(), new GUILayoutOption[0]);
        GUILayout.EndScrollView();
    }

    private void HandleHandleLogCallbackhandler(string condition, string stackTrace, LogType type)
    {
        if (this.wlogsb.Length > 0x2000)
        {
            this.wlogsb.Remove(0, this.wlogsb.Length / 2);
        }
        this.wlogsb.Append(type.ToString() + ":" + condition + "\n");
        if (((type == LogType.Assert) || (type == LogType.Exception)) || (type == LogType.Error))
        {
            this.wlogsb.Append(stackTrace + "\n");
        }
    }

    private void OnGUI()
    {
        if (showGMBtn)
        {
            if (GUI.Button(new Rect(0f, 300f, 60f, 28f), "GM"))
            {
                this.showGM = !this.showGM;
            }
            if (GUI.Button(new Rect(0f, 328f, 60f, 28f), "Log"))
            {
                this.showLog = !this.showLog;
            }
            if (this.showLog)
            {
                GUILayout.Window(1, new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), new GUI.WindowFunction(this.DoLogWindow), "Log Window", new GUILayoutOption[0]);
            }
            if (this.showGM)
            {
                this.lastGmCmd = GUI.TextField(new Rect(60f, 0f, 128f, 28f), this.lastGmCmd);
                if (GUI.Button(new Rect(198f, 0f, 60f, 28f), "Send"))
                {
                    Debug.Log(this.lastGmCmd);
                    if ("permitcode" == this.lastGmCmd.Trim())
                    {
                        GameDefine.OpenActiveCode = !GameDefine.OpenActiveCode;
                    }
                    else
                    {
                        SocketMgr.Instance.RequestGmCommand(this.lastGmCmd);
                    }
                }
            }
        }
    }

    private void Start()
    {
        Application.RegisterLogCallback(new Application.LogCallback(this.HandleHandleLogCallbackhandler));
    }

    private void Update()
    {
    }

    public string lastGmCmd
    {
        get
        {
            return WWW.UnEscapeURL(PlayerPrefs.GetString("_lastGmCmd"));
        }
        set
        {
            PlayerPrefs.SetString("_lastGmCmd", WWW.EscapeURL(value));
        }
    }
}

