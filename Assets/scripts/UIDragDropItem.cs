using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
    public bool cloneOnDrag;
    protected UIButton mButton;
    protected Collider mCollider;
    protected UIDragScrollView mDragScrollView;
    protected UIGrid mGrid;
    protected Transform mParent;
    protected float mPressTime;
    protected UIRoot mRoot;
    protected UITable mTable;
    protected int mTouchID = -2147483648;
    protected Transform mTrans;
    [HideInInspector]
    public float pressAndHoldDelay = 1f;
    public Restriction restriction;

    private void OnDrag(Vector2 delta)
    {
        if (base.enabled && (this.mTouchID == UICamera.currentTouchID))
        {
            this.OnDragDropMove((Vector3) (delta * this.mRoot.pixelSizeAdjustment));
        }
    }

    protected virtual void OnDragDropMove(Vector3 delta)
    {
        this.mTrans.localPosition += delta;
    }

    protected virtual void OnDragDropRelease(GameObject surface)
    {
        if (!this.cloneOnDrag)
        {
            this.mTouchID = -2147483648;
            if (this.mButton != null)
            {
                this.mButton.isEnabled = true;
            }
            else if (this.mCollider != null)
            {
                this.mCollider.enabled = true;
            }
            UIDragDropContainer container = (surface == null) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface);
            if (container != null)
            {
                this.mTrans.parent = (container.reparentTarget == null) ? container.transform : container.reparentTarget;
                Vector3 localPosition = this.mTrans.localPosition;
                localPosition.z = 0f;
                this.mTrans.localPosition = localPosition;
            }
            else
            {
                this.mTrans.parent = this.mParent;
            }
            this.mParent = this.mTrans.parent;
            this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
            this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
            if (this.mDragScrollView != null)
            {
                this.mDragScrollView.enabled = true;
            }
            NGUITools.MarkParentAsChanged(base.gameObject);
            if (this.mTable != null)
            {
                this.mTable.repositionNow = true;
            }
            if (this.mGrid != null)
            {
                this.mGrid.repositionNow = true;
            }
        }
        else
        {
            NGUITools.Destroy(base.gameObject);
        }
    }

    protected virtual void OnDragDropStart()
    {
        if (this.mDragScrollView != null)
        {
            this.mDragScrollView.enabled = false;
        }
        if (this.mButton != null)
        {
            this.mButton.isEnabled = false;
        }
        else if (this.mCollider != null)
        {
            this.mCollider.enabled = false;
        }
        this.mTouchID = UICamera.currentTouchID;
        this.mParent = this.mTrans.parent;
        this.mRoot = NGUITools.FindInParents<UIRoot>(this.mParent);
        this.mGrid = NGUITools.FindInParents<UIGrid>(this.mParent);
        this.mTable = NGUITools.FindInParents<UITable>(this.mParent);
        if (UIDragDropRoot.root != null)
        {
            this.mTrans.parent = UIDragDropRoot.root;
        }
        Vector3 localPosition = this.mTrans.localPosition;
        localPosition.z = 0f;
        this.mTrans.localPosition = localPosition;
        TweenPosition component = base.GetComponent<TweenPosition>();
        if (component != null)
        {
            component.enabled = false;
        }
        SpringPosition position2 = base.GetComponent<SpringPosition>();
        if (position2 != null)
        {
            position2.enabled = false;
        }
        NGUITools.MarkParentAsChanged(base.gameObject);
        if (this.mTable != null)
        {
            this.mTable.repositionNow = true;
        }
        if (this.mGrid != null)
        {
            this.mGrid.repositionNow = true;
        }
    }

    private void OnDragEnd()
    {
        if (base.enabled && (this.mTouchID == UICamera.currentTouchID))
        {
            this.OnDragDropRelease(UICamera.hoveredObject);
        }
    }

    private void OnDragStart()
    {
        if (base.enabled && (this.mTouchID == -2147483648))
        {
            if (this.restriction != Restriction.None)
            {
                if (this.restriction == Restriction.Horizontal)
                {
                    Vector2 totalDelta = UICamera.currentTouch.totalDelta;
                    float introduced5 = Mathf.Abs(totalDelta.x);
                    if (introduced5 < Mathf.Abs(totalDelta.y))
                    {
                        return;
                    }
                }
                else if (this.restriction == Restriction.Vertical)
                {
                    Vector2 vector2 = UICamera.currentTouch.totalDelta;
                    float introduced6 = Mathf.Abs(vector2.x);
                    if (introduced6 > Mathf.Abs(vector2.y))
                    {
                        return;
                    }
                }
                else if ((this.restriction == Restriction.PressAndHold) && ((this.mPressTime + this.pressAndHoldDelay) > RealTime.time))
                {
                    return;
                }
            }
            if (this.cloneOnDrag)
            {
                GameObject obj2 = NGUITools.AddChild(base.transform.parent.gameObject, base.gameObject);
                obj2.transform.localPosition = base.transform.localPosition;
                obj2.transform.localRotation = base.transform.localRotation;
                obj2.transform.localScale = base.transform.localScale;
                UIButtonColor component = obj2.GetComponent<UIButtonColor>();
                if (component != null)
                {
                    component.defaultColor = base.GetComponent<UIButtonColor>().defaultColor;
                }
                UICamera.currentTouch.dragged = obj2;
                UIDragDropItem item = obj2.GetComponent<UIDragDropItem>();
                item.Start();
                item.OnDragDropStart();
            }
            else
            {
                this.OnDragDropStart();
            }
        }
    }

    private void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            this.mPressTime = RealTime.time;
        }
    }

    protected virtual void Start()
    {
        this.mTrans = base.transform;
        this.mCollider = base.collider;
        this.mButton = base.GetComponent<UIButton>();
        this.mDragScrollView = base.GetComponent<UIDragScrollView>();
    }

    public enum Restriction
    {
        None,
        Horizontal,
        Vertical,
        PressAndHold
    }
}

