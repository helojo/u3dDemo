using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
    public bool animateSmoothly;
    public Arrangement arrangement;
    public float cellHeight = 200f;
    public float cellWidth = 200f;
    public bool hideInactive = true;
    public bool keepWithinPanel;
    public int maxPerLine;
    protected bool mInitDone;
    protected UIPanel mPanel;
    protected bool mReposition;
    public BetterList<Transform>.CompareFunc onCustomSort;
    public OnReposition onReposition;
    public UIWidget.Pivot pivot;
    [HideInInspector, SerializeField]
    private bool sorted;
    public Sorting sorting;

    public void AddChild(Transform trans)
    {
        this.AddChild(trans, true);
    }

    public void AddChild(Transform trans, bool sort)
    {
        if (trans != null)
        {
            BetterList<Transform> childList = this.GetChildList();
            childList.Add(trans);
            this.ResetPosition(childList);
        }
    }

    public void AddChild(Transform trans, int index)
    {
        if (trans != null)
        {
            if (this.sorting != Sorting.None)
            {
                Debug.LogWarning("The Grid has sorting enabled, so AddChild at index may not work as expected.", this);
            }
            BetterList<Transform> childList = this.GetChildList();
            childList.Insert(index, trans);
            this.ResetPosition(childList);
        }
    }

    public void ConstrainWithinPanel()
    {
        if (this.mPanel != null)
        {
            this.mPanel.ConstrainTargetToBounds(base.transform, true);
        }
    }

    public Transform GetChild(int index)
    {
        BetterList<Transform> childList = this.GetChildList();
        return ((index >= childList.size) ? null : childList[index]);
    }

    public BetterList<Transform> GetChildList()
    {
        Transform transform = base.transform;
        BetterList<Transform> list = new BetterList<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!this.hideInactive || ((child != null) && NGUITools.GetActive(child.gameObject)))
            {
                list.Add(child);
            }
        }
        if (this.sorting != Sorting.None)
        {
            if (this.sorting == Sorting.Alphabetic)
            {
                list.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortByName));
                return list;
            }
            if (this.sorting == Sorting.Horizontal)
            {
                list.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortHorizontal));
                return list;
            }
            if (this.sorting == Sorting.Vertical)
            {
                list.Sort(new BetterList<Transform>.CompareFunc(UIGrid.SortVertical));
                return list;
            }
            if (this.onCustomSort != null)
            {
                list.Sort(this.onCustomSort);
                return list;
            }
            this.Sort(list);
        }
        return list;
    }

    public int GetIndex(Transform trans)
    {
        return this.GetChildList().IndexOf(trans);
    }

    protected virtual void Init()
    {
        this.mInitDone = true;
        this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
    }

    public Transform RemoveChild(int index)
    {
        BetterList<Transform> childList = this.GetChildList();
        if (index < childList.size)
        {
            Transform transform = childList[index];
            childList.RemoveAt(index);
            this.ResetPosition(childList);
            return transform;
        }
        return null;
    }

    public bool RemoveChild(Transform t)
    {
        BetterList<Transform> childList = this.GetChildList();
        if (childList.Remove(t))
        {
            this.ResetPosition(childList);
            return true;
        }
        return false;
    }

    [ContextMenu("Execute")]
    public virtual void Reposition()
    {
        if ((Application.isPlaying && !this.mInitDone) && NGUITools.GetActive(this))
        {
            this.mReposition = true;
        }
        else
        {
            if (this.sorted)
            {
                this.sorted = false;
                if (this.sorting == Sorting.None)
                {
                    this.sorting = Sorting.Alphabetic;
                }
                NGUITools.SetDirty(this);
            }
            if (!this.mInitDone)
            {
                this.Init();
            }
            BetterList<Transform> childList = this.GetChildList();
            this.ResetPosition(childList);
            if (this.keepWithinPanel)
            {
                this.ConstrainWithinPanel();
            }
            if (this.onReposition != null)
            {
                this.onReposition();
            }
        }
    }

    protected void ResetPosition(BetterList<Transform> list)
    {
        this.mReposition = false;
        int b = 0;
        int num2 = 0;
        int a = 0;
        int num4 = 0;
        Transform transform = base.transform;
        int num5 = 0;
        int size = list.size;
        while (num5 < size)
        {
            Transform transform2 = list[num5];
            float z = transform2.localPosition.z;
            Vector3 pos = (this.arrangement != Arrangement.Horizontal) ? new Vector3(this.cellWidth * num2, -this.cellHeight * b, z) : new Vector3(this.cellWidth * b, -this.cellHeight * num2, z);
            if (this.animateSmoothly && Application.isPlaying)
            {
                SpringPosition.Begin(transform2.gameObject, pos, 15f).updateScrollView = true;
            }
            else
            {
                transform2.localPosition = pos;
            }
            a = Mathf.Max(a, b);
            num4 = Mathf.Max(num4, num2);
            if ((++b >= this.maxPerLine) && (this.maxPerLine > 0))
            {
                b = 0;
                num2++;
            }
            num5++;
        }
        if (this.pivot != UIWidget.Pivot.TopLeft)
        {
            float num8;
            float num9;
            Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
            if (this.arrangement == Arrangement.Horizontal)
            {
                num8 = Mathf.Lerp(0f, a * this.cellWidth, pivotOffset.x);
                num9 = Mathf.Lerp(-num4 * this.cellHeight, 0f, pivotOffset.y);
            }
            else
            {
                num8 = Mathf.Lerp(0f, num4 * this.cellWidth, pivotOffset.x);
                num9 = Mathf.Lerp(-a * this.cellHeight, 0f, pivotOffset.y);
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpringPosition component = child.GetComponent<SpringPosition>();
                if (component != null)
                {
                    component.target.x -= num8;
                    component.target.y -= num9;
                }
                else
                {
                    Vector3 localPosition = child.localPosition;
                    localPosition.x -= num8;
                    localPosition.y -= num9;
                    child.localPosition = localPosition;
                }
            }
        }
    }

    protected virtual void Sort(BetterList<Transform> list)
    {
    }

    public static int SortByName(Transform a, Transform b)
    {
        return string.Compare(a.name, b.name);
    }

    public static int SortHorizontal(Transform a, Transform b)
    {
        return a.localPosition.x.CompareTo(b.localPosition.x);
    }

    public static int SortVertical(Transform a, Transform b)
    {
        return b.localPosition.y.CompareTo(a.localPosition.y);
    }

    protected virtual void Start()
    {
        if (!this.mInitDone)
        {
            this.Init();
        }
        bool animateSmoothly = this.animateSmoothly;
        this.animateSmoothly = false;
        this.Reposition();
        this.animateSmoothly = animateSmoothly;
        base.enabled = false;
    }

    protected virtual void Update()
    {
        if (this.mReposition)
        {
            this.Reposition();
        }
        base.enabled = false;
    }

    public bool repositionNow
    {
        set
        {
            if (value)
            {
                this.mReposition = true;
                base.enabled = true;
            }
        }
    }

    public enum Arrangement
    {
        Horizontal,
        Vertical
    }

    public delegate void OnReposition();

    public enum Sorting
    {
        None,
        Alphabetic,
        Horizontal,
        Vertical,
        Custom
    }
}

