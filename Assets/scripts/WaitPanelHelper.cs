using System;
using System.Collections.Generic;
using UnityEngine;

public class WaitPanelHelper : MonoBehaviour
{
    private static Dictionary<string, WaitPanelHelper> allWaitPanel = new Dictionary<string, WaitPanelHelper>();
    private WaitPanel curWaitPanel;
    private bool isNeedShow = true;
    private bool isWaitInit;

    private void FixedUpdate()
    {
        if ((this.curWaitPanel != null) && (this.isNeedShow != this.curWaitPanel.gameObject.activeSelf))
        {
            this.curWaitPanel.Hidden = !this.isNeedShow;
        }
    }

    public static void HideAll()
    {
        foreach (KeyValuePair<string, WaitPanelHelper> pair in allWaitPanel)
        {
            if (pair.Value != null)
            {
                pair.Value.HideWaitPanel();
            }
        }
    }

    private void HideWaitPanel()
    {
        this.isNeedShow = false;
    }

    public static void HideWaitPanel(string name)
    {
        WaitPanelHelper helper = null;
        if (allWaitPanel.TryGetValue(name, out helper))
        {
            helper.HideWaitPanel();
        }
    }

    private void Init()
    {
        this.isNeedShow = true;
        this.isWaitInit = true;
        if (this.curWaitPanel == null)
        {
            GUIMgr.Instance.CreateGUIEntity("WaitPanel", false, delegate (GUIEntity obj) {
                obj.Depth = 0x44c;
                UnityEngine.Object.DontDestroyOnLoad(obj.gameObject);
                this.curWaitPanel = (WaitPanel) obj;
                this.isWaitInit = false;
            });
        }
    }

    private void ShowWaitPanel()
    {
        if ((this.curWaitPanel == null) && !this.isWaitInit)
        {
            this.Init();
        }
        this.isNeedShow = true;
    }

    public static void ShowWaitPanel(string name)
    {
        WaitPanelHelper helper = null;
        if (!allWaitPanel.TryGetValue(name, out helper))
        {
            GameObject target = new GameObject(name);
            UnityEngine.Object.DontDestroyOnLoad(target);
            helper = target.AddComponent<WaitPanelHelper>();
            helper.Init();
            allWaitPanel.Add(name, helper);
        }
        helper.ShowWaitPanel();
    }
}

