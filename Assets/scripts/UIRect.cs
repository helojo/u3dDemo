using System;
using UnityEngine;

public abstract class UIRect : MonoBehaviour
{
    public AnchorPoint bottomAnchor = new AnchorPoint();
    private bool bottomAnchorEnable;
    [NonSerialized]
    public float finalAlpha = 1f;
    public AnchorPoint leftAnchor = new AnchorPoint();
    private bool leftAnchorEnable;
    private bool mAnchorsCached;
    protected bool mChanged = true;
    protected BetterList<UIRect> mChildren = new BetterList<UIRect>();
    protected GameObject mGo;
    private Camera mMyCam;
    private UIRect mParent;
    protected bool mParentFound;
    private UIRoot mRoot;
    private bool mRootSet;
    private static Vector3[] mSides = new Vector3[4];
    protected bool mStarted;
    protected Transform mTrans;
    protected bool mUpdateAnchors;
    private int mUpdateFrame = -1;
    public AnchorPoint rightAnchor = new AnchorPoint(1f);
    private bool rightAnchorEnable;
    public AnchorPoint topAnchor = new AnchorPoint(1f);
    private bool topAnchorEnable;
    public AnchorUpdate updateAnchors = AnchorUpdate.OnUpdate;

    protected UIRect()
    {
    }

    public abstract float CalculateFinalAlpha(int frameID);
    private void FindCameraFor(AnchorPoint ap)
    {
        if ((ap.target == null) || (ap.rect != null))
        {
            ap.targetCam = null;
        }
        else
        {
            ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
        }
    }

    protected Vector3 GetLocalPos(AnchorPoint ac, Transform trans)
    {
        if ((this.anchorCamera == null) || (ac.targetCam == null))
        {
            return this.cachedTransform.localPosition;
        }
        Vector3 position = this.mMyCam.ViewportToWorldPoint(ac.targetCam.WorldToViewportPoint(ac.target.position));
        if (trans != null)
        {
            position = trans.InverseTransformPoint(position);
        }
        position.x = Mathf.Floor(position.x + 0.5f);
        position.y = Mathf.Floor(position.y + 0.5f);
        return position;
    }

    public virtual Vector3[] GetSides(Transform relativeTo)
    {
        if (this.anchorCamera != null)
        {
            return this.anchorCamera.GetSides(relativeTo);
        }
        Vector3 position = this.cachedTransform.position;
        for (int i = 0; i < 4; i++)
        {
            mSides[i] = position;
        }
        if (relativeTo != null)
        {
            for (int j = 0; j < 4; j++)
            {
                mSides[j] = relativeTo.InverseTransformPoint(mSides[j]);
            }
        }
        return mSides;
    }

    public virtual void Invalidate(bool includeChildren)
    {
        this.mChanged = true;
        if (includeChildren)
        {
            for (int i = 0; i < this.mChildren.size; i++)
            {
                this.mChildren.buffer[i].Invalidate(true);
            }
        }
    }

    protected abstract void OnAnchor();
    protected virtual void OnDisable()
    {
        if (this.mParent != null)
        {
            this.mParent.mChildren.Remove(this);
        }
        this.mParent = null;
        this.mRoot = null;
        this.mRootSet = false;
        this.mParentFound = false;
    }

    protected virtual void OnEnable()
    {
        this.mAnchorsCached = false;
        if (this.updateAnchors == AnchorUpdate.OnEnable)
        {
            this.mUpdateAnchors = true;
        }
        if (this.mStarted)
        {
            this.OnInit();
        }
    }

    protected virtual void OnInit()
    {
        this.mChanged = true;
        this.mRootSet = false;
        this.mParentFound = false;
        if (this.parent != null)
        {
            this.mParent.mChildren.Add(this);
        }
        if (this.leftAnchor.target != null)
        {
            this.leftAnchorEnable = true;
        }
        if (this.bottomAnchor.target != null)
        {
            this.bottomAnchorEnable = true;
        }
        if (this.rightAnchor.target != null)
        {
            this.rightAnchorEnable = true;
        }
        if (this.topAnchor.target != null)
        {
            this.topAnchorEnable = true;
        }
    }

    protected abstract void OnStart();
    protected virtual void OnUpdate()
    {
    }

    public virtual void ParentHasChanged()
    {
        this.mParentFound = false;
        UIRect rect = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
        if (this.mParent != rect)
        {
            if (this.mParent != null)
            {
                this.mParent.mChildren.Remove(this);
            }
            this.mParent = rect;
            if (this.mParent != null)
            {
                this.mParent.mChildren.Add(this);
            }
            this.mRootSet = false;
        }
    }

    public void ResetAnchors()
    {
        this.mAnchorsCached = true;
        this.leftAnchor.rect = (this.leftAnchor.target == null) ? null : this.leftAnchor.target.GetComponent<UIRect>();
        this.bottomAnchor.rect = (this.bottomAnchor.target == null) ? null : this.bottomAnchor.target.GetComponent<UIRect>();
        this.rightAnchor.rect = (this.rightAnchor.target == null) ? null : this.rightAnchor.target.GetComponent<UIRect>();
        this.topAnchor.rect = (this.topAnchor.target == null) ? null : this.topAnchor.target.GetComponent<UIRect>();
        this.mMyCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
        this.FindCameraFor(this.leftAnchor);
        this.FindCameraFor(this.bottomAnchor);
        this.FindCameraFor(this.rightAnchor);
        this.FindCameraFor(this.topAnchor);
        this.mUpdateAnchors = true;
    }

    public void SetAnchor(GameObject go)
    {
        Transform transform = (go == null) ? null : go.transform;
        this.leftAnchor.target = transform;
        this.rightAnchor.target = transform;
        this.topAnchor.target = transform;
        this.bottomAnchor.target = transform;
        this.ResetAnchors();
        this.UpdateAnchors();
    }

    public void SetAnchor(Transform t)
    {
        this.leftAnchor.target = t;
        this.rightAnchor.target = t;
        this.topAnchor.target = t;
        this.bottomAnchor.target = t;
        this.ResetAnchors();
        this.UpdateAnchors();
    }

    public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
    {
        Transform transform = (go == null) ? null : go.transform;
        this.leftAnchor.target = transform;
        this.rightAnchor.target = transform;
        this.topAnchor.target = transform;
        this.bottomAnchor.target = transform;
        this.leftAnchor.relative = 0f;
        this.rightAnchor.relative = 1f;
        this.bottomAnchor.relative = 0f;
        this.topAnchor.relative = 1f;
        this.leftAnchor.absolute = left;
        this.rightAnchor.absolute = right;
        this.bottomAnchor.absolute = bottom;
        this.topAnchor.absolute = top;
        this.ResetAnchors();
        this.UpdateAnchors();
    }

    public abstract void SetRect(float x, float y, float width, float height);
    protected void Start()
    {
        this.mStarted = true;
        this.OnInit();
        this.OnStart();
    }

    public void Update()
    {
        if (!this.mAnchorsCached)
        {
            this.ResetAnchors();
        }
        int frameCount = Time.frameCount;
        if (this.mUpdateFrame != frameCount)
        {
            if ((this.updateAnchors == AnchorUpdate.OnUpdate) || this.mUpdateAnchors)
            {
                this.mUpdateFrame = frameCount;
                this.mUpdateAnchors = false;
                bool flag = false;
                if (this.leftAnchorEnable)
                {
                    flag = true;
                    if ((this.leftAnchor.rect != null) && (this.leftAnchor.rect.mUpdateFrame != frameCount))
                    {
                        this.leftAnchor.rect.Update();
                    }
                }
                if (this.bottomAnchorEnable)
                {
                    flag = true;
                    if ((this.bottomAnchor.rect != null) && (this.bottomAnchor.rect.mUpdateFrame != frameCount))
                    {
                        this.bottomAnchor.rect.Update();
                    }
                }
                if (this.rightAnchorEnable)
                {
                    flag = true;
                    if ((this.rightAnchor.rect != null) && (this.rightAnchor.rect.mUpdateFrame != frameCount))
                    {
                        this.rightAnchor.rect.Update();
                    }
                }
                if (this.topAnchorEnable)
                {
                    flag = true;
                    if ((this.topAnchor.rect != null) && (this.topAnchor.rect.mUpdateFrame != frameCount))
                    {
                        this.topAnchor.rect.Update();
                    }
                }
                if (flag)
                {
                    this.OnAnchor();
                }
            }
            this.OnUpdate();
        }
    }

    public void UpdateAnchors()
    {
        if (this.isAnchored)
        {
            this.OnAnchor();
        }
    }

    public abstract float alpha { get; set; }

    public Camera anchorCamera
    {
        get
        {
            if (!this.mAnchorsCached)
            {
                this.ResetAnchors();
            }
            return this.mMyCam;
        }
    }

    public GameObject cachedGameObject
    {
        get
        {
            if (this.mGo == null)
            {
                this.mGo = base.gameObject;
            }
            return this.mGo;
        }
    }

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    public virtual bool canBeAnchored
    {
        get
        {
            return true;
        }
    }

    public bool isAnchored
    {
        get
        {
            return ((((this.leftAnchor.target != null) || (this.rightAnchor.target != null)) || ((this.topAnchor.target != null) || (this.bottomAnchor.target != null))) && this.canBeAnchored);
        }
    }

    public virtual bool isAnchoredHorizontally
    {
        get
        {
            return ((this.leftAnchor.target != null) || ((bool) this.rightAnchor.target));
        }
    }

    public virtual bool isAnchoredVertically
    {
        get
        {
            return ((this.bottomAnchor.target != null) || ((bool) this.topAnchor.target));
        }
    }

    public bool isFullyAnchored
    {
        get
        {
            return ((((this.leftAnchor.target != null) && (this.rightAnchor.target != null)) && (this.topAnchor.target != null)) && ((bool) this.bottomAnchor.target));
        }
    }

    public abstract Vector3[] localCorners { get; }

    public UIRect parent
    {
        get
        {
            if (!this.mParentFound)
            {
                this.mParentFound = true;
                this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
            }
            return this.mParent;
        }
    }

    public UIRoot root
    {
        get
        {
            if (this.parent != null)
            {
                return this.mParent.root;
            }
            if (!this.mRootSet)
            {
                this.mRootSet = true;
                this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
            }
            return this.mRoot;
        }
    }

    public abstract Vector3[] worldCorners { get; }

    [Serializable]
    public class AnchorPoint
    {
        public int absolute;
        [NonSerialized]
        public UIRect rect;
        public float relative;
        public Transform target;
        [NonSerialized]
        public Camera targetCam;

        public AnchorPoint()
        {
        }

        public AnchorPoint(float relative)
        {
            this.relative = relative;
        }

        public Vector3[] GetSides(Transform relativeTo)
        {
            if (this.target != null)
            {
                if (this.rect != null)
                {
                    return this.rect.GetSides(relativeTo);
                }
                if (this.target.camera != null)
                {
                    return this.target.camera.GetSides(relativeTo);
                }
            }
            return null;
        }

        public void Set(float relative, float absolute)
        {
            this.relative = relative;
            this.absolute = Mathf.FloorToInt(absolute + 0.5f);
        }

        public void SetHorizontal(Transform parent, float localPos)
        {
            if (this.rect != null)
            {
                Vector3[] sides = this.rect.GetSides(parent);
                float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
                this.absolute = Mathf.FloorToInt((localPos - num) + 0.5f);
            }
            else
            {
                Vector3 position = this.target.position;
                if (parent != null)
                {
                    position = parent.InverseTransformPoint(position);
                }
                this.absolute = Mathf.FloorToInt((localPos - position.x) + 0.5f);
            }
        }

        public void SetToNearest(float abs0, float abs1, float abs2)
        {
            this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
        }

        public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
        {
            float num = Mathf.Abs(abs0);
            float num2 = Mathf.Abs(abs1);
            float num3 = Mathf.Abs(abs2);
            if ((num < num2) && (num < num3))
            {
                this.Set(rel0, abs0);
            }
            else if ((num2 < num) && (num2 < num3))
            {
                this.Set(rel1, abs1);
            }
            else
            {
                this.Set(rel2, abs2);
            }
        }

        public void SetVertical(Transform parent, float localPos)
        {
            if (this.rect != null)
            {
                Vector3[] sides = this.rect.GetSides(parent);
                float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
                this.absolute = Mathf.FloorToInt((localPos - num) + 0.5f);
            }
            else
            {
                Vector3 position = this.target.position;
                if (parent != null)
                {
                    position = parent.InverseTransformPoint(position);
                }
                this.absolute = Mathf.FloorToInt((localPos - position.y) + 0.5f);
            }
        }
    }

    public enum AnchorUpdate
    {
        OnEnable,
        OnUpdate
    }
}

