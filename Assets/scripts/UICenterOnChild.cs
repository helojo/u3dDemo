using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
    public bool EnableRecenter = true;
    private GameObject mCenteredObject;
    private UIScrollView mScrollView;
    public float nextPageThreshold;
    public SpringPanel.OnFinished onFinished;
    public float springStrength = 8f;

    public void CenterOn(Transform target)
    {
        if ((this.mScrollView != null) && (this.mScrollView.panel != null))
        {
            Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
            Vector3 panelCenter = (Vector3) ((worldCorners[2] + worldCorners[0]) * 0.5f);
            this.CenterOn(target, panelCenter);
        }
    }

    private void CenterOn(Transform target, Vector3 panelCenter)
    {
        if (((target != null) && (this.mScrollView != null)) && (this.mScrollView.panel != null))
        {
            Transform cachedTransform = this.mScrollView.panel.cachedTransform;
            this.mCenteredObject = target.gameObject;
            Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
            Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
            Vector3 vector3 = vector - vector2;
            if (!this.mScrollView.canMoveHorizontally)
            {
                vector3.x = 0f;
            }
            if (!this.mScrollView.canMoveVertically)
            {
                vector3.y = 0f;
            }
            vector3.z = 0f;
            SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - vector3, this.springStrength).onFinished = this.onFinished;
        }
        else
        {
            this.mCenteredObject = null;
        }
    }

    private void OnDragFinished()
    {
        if (base.enabled)
        {
            this.Recenter();
        }
    }

    private void OnEnable()
    {
        if (this.EnableRecenter)
        {
            this.Recenter();
        }
    }

    private void OnValidate()
    {
        this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
    }

    public void Recenter()
    {
        Transform transform = base.transform;
        if (transform.childCount != 0)
        {
            if (this.mScrollView == null)
            {
                this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
                if (this.mScrollView == null)
                {
                    Debug.LogWarning(string.Concat(new object[] { base.GetType(), " requires ", typeof(UIScrollView), " on a parent object in order to work" }), this);
                    base.enabled = false;
                    return;
                }
                this.mScrollView.onDragFinished = new UIScrollView.OnDragFinished(this.OnDragFinished);
                if (this.mScrollView.horizontalScrollBar != null)
                {
                    this.mScrollView.horizontalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
                }
                if (this.mScrollView.verticalScrollBar != null)
                {
                    this.mScrollView.verticalScrollBar.onDragFinished = new UIProgressBar.OnDragFinished(this.OnDragFinished);
                }
            }
            if (this.mScrollView.panel != null)
            {
                Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
                Vector3 panelCenter = (Vector3) ((worldCorners[2] + worldCorners[0]) * 0.5f);
                Vector3 vector2 = panelCenter - ((Vector3) (this.mScrollView.currentMomentum * (this.mScrollView.momentumAmount * 0.1f)));
                this.mScrollView.currentMomentum = Vector3.zero;
                float maxValue = float.MaxValue;
                Transform target = null;
                int index = 0;
                int num3 = 0;
                int childCount = transform.childCount;
                while (num3 < childCount)
                {
                    Transform child = transform.GetChild(num3);
                    if (child.gameObject.activeInHierarchy)
                    {
                        float num5 = Vector3.SqrMagnitude(child.position - vector2);
                        if (num5 < maxValue)
                        {
                            maxValue = num5;
                            target = child;
                            index = num3;
                        }
                    }
                    num3++;
                }
                if (((this.nextPageThreshold > 0f) && (UICamera.currentTouch != null)) && ((this.mCenteredObject != null) && (this.mCenteredObject.transform == transform.GetChild(index))))
                {
                    Vector2 totalDelta = UICamera.currentTouch.totalDelta;
                    float x = 0f;
                    UIScrollView.Movement movement = this.mScrollView.movement;
                    if (movement == UIScrollView.Movement.Horizontal)
                    {
                        x = totalDelta.x;
                    }
                    else if (movement == UIScrollView.Movement.Vertical)
                    {
                        x = totalDelta.y;
                    }
                    else
                    {
                        x = totalDelta.magnitude;
                    }
                    if (x > this.nextPageThreshold)
                    {
                        if (index > 0)
                        {
                            target = transform.GetChild(index - 1);
                        }
                    }
                    else if ((x < -this.nextPageThreshold) && (index < (transform.childCount - 1)))
                    {
                        target = transform.GetChild(index + 1);
                    }
                }
                this.CenterOn(target, panelCenter);
            }
        }
    }

    public GameObject centeredObject
    {
        get
        {
            return this.mCenteredObject;
        }
    }
}

