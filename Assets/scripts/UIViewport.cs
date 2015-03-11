using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Viewport Camera"), RequireComponent(typeof(Camera)), ExecuteInEditMode]
public class UIViewport : MonoBehaviour
{
    public Transform bottomRight;
    public float fullSize = 1f;
    private Camera mCam;
    public Camera sourceCamera;
    public Transform topLeft;

    private void LateUpdate()
    {
        if ((this.topLeft != null) && (this.bottomRight != null))
        {
            float x = this.topLeft.localPosition.x;
            float y = this.bottomRight.localPosition.y;
            float activeWidth = GUIMgr.Instance.Root.activeWidth;
            float activeHeight = GUIMgr.Instance.Root.activeHeight;
            x = (activeWidth * 0.5f) + x;
            y = (activeHeight * 0.5f) + y;
            Vector3 vector = this.sourceCamera.WorldToScreenPoint(this.topLeft.position);
            Vector3 vector2 = this.sourceCamera.WorldToScreenPoint(this.bottomRight.position);
            Rect rect = new Rect(x / activeWidth, y / activeHeight, (vector2.x - vector.x) / ((float) Screen.width), (vector.y - vector2.y) / ((float) Screen.height));
            float num5 = this.fullSize * rect.height;
            if (rect != this.mCam.rect)
            {
                this.mCam.rect = rect;
            }
            if (this.mCam.orthographicSize != num5)
            {
                this.mCam.orthographicSize = num5;
            }
        }
    }

    public void Reposition(Transform tl_tns, Transform br_tns, float vertical_offset)
    {
        this.topLeft = tl_tns;
        this.bottomRight = br_tns;
        if ((this.topLeft != null) && (this.bottomRight != null))
        {
            float x = this.topLeft.localPosition.x;
            float num2 = this.bottomRight.localPosition.x;
            float y = this.topLeft.localPosition.y;
            float num4 = this.bottomRight.localPosition.y;
            base.transform.localPosition = new Vector3((x + num2) * 0.5f, ((y + num4) * 0.5f) + vertical_offset);
        }
    }

    private void Start()
    {
        this.mCam = base.camera;
        if (this.sourceCamera == null)
        {
            this.sourceCamera = Camera.main;
        }
    }
}

