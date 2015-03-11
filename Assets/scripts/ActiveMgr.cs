using System;
using UnityEngine;

public class ActiveMgr : MonoBehaviour
{
    public BoxCollider bc;
    public GameObject child;
    public static ActiveMgr inst;
    public static bool press;

    private void OnClick()
    {
        press = true;
        SocketMgr.Instance.GetActiveList();
    }

    public void ShowActive(bool show)
    {
        this.child.SetActive(show);
        this.bc.enabled = show;
    }

    private void Start()
    {
        inst = this;
        press = false;
        SocketMgr.Instance.GetActiveList();
    }
}

