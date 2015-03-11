using System;
using UnityEngine;

public class ResetListPos : MonoBehaviour
{
    private Vector3 LocalPos = Vector3.zero;
    private UIPanel mPanel;
    private Vector2 Offset = Vector2.zero;

    private void GetData()
    {
        this.mPanel = base.gameObject.transform.GetComponent<UIPanel>();
        this.LocalPos = this.mPanel.transform.localPosition;
        this.Offset = this.mPanel.clipOffset;
    }

    public void PanelReset()
    {
        this.WaitResetPanel();
        SpringPanel component = base.gameObject.transform.GetComponent<SpringPanel>();
        if (null != component)
        {
            component.enabled = false;
            component.onFinished = new SpringPanel.OnFinished(this.WaitResetPanel);
        }
    }

    private void WaitResetPanel()
    {
        if (this.mPanel == null)
        {
            this.GetData();
        }
        this.mPanel.transform.localPosition = this.LocalPos;
        this.mPanel.clipOffset = this.Offset;
    }
}

