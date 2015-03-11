using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
    public bool cullContent = true;
    public int itemSize = 100;
    private BetterList<Transform> mChildren = new BetterList<Transform>();
    private bool mHorizontal;
    private UIPanel mPanel;
    private UIScrollView mScroll;
    private Transform mTrans;

    protected bool CacheScrollView()
    {
        this.mTrans = base.transform;
        this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
        this.mScroll = this.mPanel.GetComponent<UIScrollView>();
        if (this.mScroll != null)
        {
            if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
            {
                this.mHorizontal = true;
                goto Label_007C;
            }
            if (this.mScroll.movement == UIScrollView.Movement.Vertical)
            {
                this.mHorizontal = false;
                goto Label_007C;
            }
        }
        return false;
    Label_007C:
        return true;
    }

    protected virtual void OnMove(UIPanel panel)
    {
        this.WrapContent();
    }

    private void ResetChildPositions()
    {
        for (int i = 0; i < this.mChildren.size; i++)
        {
            Transform transform = this.mChildren[i];
            transform.localPosition = !this.mHorizontal ? new Vector3(0f, (float) (-i * this.itemSize), 0f) : new Vector3((float) (i * this.itemSize), 0f, 0f);
        }
    }

    [ContextMenu("Sort Alphabetically")]
    public void SortAlphabetically()
    {
        if (this.CacheScrollView())
        {
            this.mChildren.Clear();
            for (int i = 0; i < this.mTrans.childCount; i++)
            {
                this.mChildren.Add(this.mTrans.GetChild(i));
            }
            this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortByName));
            this.ResetChildPositions();
        }
    }

    [ContextMenu("Sort Based on Scroll Movement")]
    public void SortBasedOnScrollMovement()
    {
        if (this.CacheScrollView())
        {
            this.mChildren.Clear();
            for (int i = 0; i < this.mTrans.childCount; i++)
            {
                this.mChildren.Add(this.mTrans.GetChild(i));
            }
            if (this.mHorizontal)
            {
                this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortHorizontal));
            }
            else
            {
                this.mChildren.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortVertical));
            }
            this.ResetChildPositions();
        }
    }

    protected virtual void Start()
    {
        this.SortBasedOnScrollMovement();
        this.WrapContent();
        if (this.mScroll != null)
        {
            this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
            this.mScroll.restrictWithinPanel = false;
            if (this.mScroll.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
            {
                this.mScroll.dragEffect = UIScrollView.DragEffect.Momentum;
            }
        }
    }

    protected virtual void UpdateItem(Transform item, int index)
    {
    }

    public void WrapContent()
    {
        float num = (this.itemSize * this.mChildren.size) * 0.5f;
        Vector3[] worldCorners = this.mPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mTrans.InverseTransformPoint(position);
        }
        Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        if (this.mHorizontal)
        {
            float num3 = worldCorners[0].x - this.itemSize;
            float num4 = worldCorners[2].x + this.itemSize;
            for (int j = 0; j < this.mChildren.size; j++)
            {
                Transform item = this.mChildren[j];
                float num6 = item.localPosition.x - vector2.x;
                if (num6 < -num)
                {
                    item.localPosition += new Vector3(num * 2f, 0f, 0f);
                    num6 = item.localPosition.x - vector2.x;
                    this.UpdateItem(item, j);
                }
                else if (num6 > num)
                {
                    item.localPosition -= new Vector3(num * 2f, 0f, 0f);
                    num6 = item.localPosition.x - vector2.x;
                    this.UpdateItem(item, j);
                }
                if (this.cullContent)
                {
                    num6 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
                    if (!UICamera.IsPressed(item.gameObject))
                    {
                        NGUITools.SetActive(item.gameObject, (num6 > num3) && (num6 < num4), false);
                    }
                }
            }
        }
        else
        {
            float num7 = worldCorners[0].y - this.itemSize;
            float num8 = worldCorners[2].y + this.itemSize;
            for (int k = 0; k < this.mChildren.size; k++)
            {
                Transform transform2 = this.mChildren[k];
                float num10 = transform2.localPosition.y - vector2.y;
                if (num10 < -num)
                {
                    transform2.localPosition += new Vector3(0f, num * 2f, 0f);
                    num10 = transform2.localPosition.y - vector2.y;
                    this.UpdateItem(transform2, k);
                }
                else if (num10 > num)
                {
                    transform2.localPosition -= new Vector3(0f, num * 2f, 0f);
                    num10 = transform2.localPosition.y - vector2.y;
                    this.UpdateItem(transform2, k);
                }
                if (this.cullContent)
                {
                    num10 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
                    if (!UICamera.IsPressed(transform2.gameObject))
                    {
                        NGUITools.SetActive(transform2.gameObject, (num10 > num7) && (num10 < num8), false);
                    }
                }
            }
        }
    }
}

