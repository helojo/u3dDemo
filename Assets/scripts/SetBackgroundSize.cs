using System;
using UnityEngine;

public class SetBackgroundSize : MonoBehaviour
{
    private void Start()
    {
        UITexture component = base.transform.GetComponent<UITexture>();
        if (null != component)
        {
            component.width = GUIMgr.Instance.Root.activeWidth + 6;
            component.height = GUIMgr.Instance.Root.activeHeight + 6;
        }
    }

    private void Update()
    {
    }
}

