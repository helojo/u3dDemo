using System;
using UnityEngine;

public class RTClearance : MonoBehaviour
{
    [HideInInspector]
    public bool clearDepth = true;
    [HideInInspector]
    public Color color = new Color(0f, 0f, 0f, 0f);

    private void OnPostRender()
    {
        GL.Clear(this.clearDepth, true, this.color);
        Camera component = base.GetComponent<Camera>();
        if (null != component)
        {
            component.enabled = false;
        }
        UnityEngine.Object.Destroy(this);
    }
}

