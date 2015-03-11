using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Panel")]
public class UIPanel : UIRect
{
    public bool alwaysOnScreen;
    public bool anchorOffset;
    public bool cullWhileDragging;
    [NonSerialized]
    public Vector4 drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);
    [NonSerialized]
    public BetterList<UIDrawCall> drawCalls = new BetterList<UIDrawCall>();
    public bool generateNormals;
    public bool hasAlpha;
    public static BetterList<UIPanel> list = new BetterList<UIPanel>();
    [HideInInspector, SerializeField]
    private float mAlpha = 1f;
    private int mAlphaFrameID;
    private Camera mCam;
    [SerializeField]
    private Vector2 mClipOffset = Vector2.zero;
    [HideInInspector, SerializeField]
    private UIDrawCall.Clipping mClipping;
    [HideInInspector, SerializeField]
    private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);
    [SerializeField, HideInInspector]
    private Vector2 mClipSoftness = new Vector2(4f, 4f);
    private static Vector3[] mCorners = new Vector3[4];
    private float mCullTime;
    [SerializeField, HideInInspector]
    private int mDepth;
    private bool mForced;
    private bool mHalfPixelOffset;
    private int mLayer = -1;
    private int mMatrixFrame = -1;
    private Vector2 mMax = Vector2.zero;
    private Vector2 mMin = Vector2.zero;
    private UIPanel mParentPanel;
    private bool mRebuild;
    private bool mResized;
    [SerializeField, HideInInspector]
    private int mSortingOrder;
    private bool mSortWidgets;
    private static float[] mTemp = new float[4];
    private static int mUpdateFrame = -1;
    private float mUpdateTime;
    public OnClippingMoved onClipMove;
    public OnGeometryUpdated onGeometryUpdated;
    public RenderQueue renderQueue;
    public bool showInPanelTool = true;
    public int startingRenderQueue = 0xbb8;
    [NonSerialized]
    public BetterList<UIWidget> widgets = new BetterList<UIWidget>();
    public bool widgetsAreStatic;
    [NonSerialized]
    public Matrix4x4 worldToLocal = Matrix4x4.identity;

    public void AddWidget(UIWidget w)
    {
        if (this.widgets.size == 0)
        {
            this.widgets.Add(w);
        }
        else if (this.mSortWidgets)
        {
            this.widgets.Add(w);
            this.SortWidgets();
        }
        else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) == -1)
        {
            this.widgets.Insert(0, w);
        }
        else
        {
            int size = this.widgets.size;
            while (size > 0)
            {
                if (UIWidget.PanelCompareFunc(w, this.widgets[--size]) != -1)
                {
                    this.widgets.Insert(size + 1, w);
                    break;
                }
            }
        }
        this.FindDrawCall(w);
    }

    public bool Affects(UIWidget w)
    {
        if (w != null)
        {
            UIPanel panel = w.panel;
            if (panel == null)
            {
                return false;
            }
            for (UIPanel panel2 = this; panel2 != null; panel2 = panel2.mParentPanel)
            {
                if (panel2 == panel)
                {
                    return true;
                }
                if (!panel2.hasCumulativeClipping)
                {
                    return false;
                }
            }
        }
        return false;
    }

    private void Awake()
    {
        base.mGo = base.gameObject;
        base.mTrans = base.transform;
        this.mHalfPixelOffset = (((Application.platform == RuntimePlatform.WindowsPlayer) || (Application.platform == RuntimePlatform.XBOX360)) || (Application.platform == RuntimePlatform.WindowsWebPlayer)) || (Application.platform == RuntimePlatform.WindowsEditor);
        if (this.mHalfPixelOffset)
        {
            this.mHalfPixelOffset = SystemInfo.graphicsShaderLevel < 40;
        }
    }

    public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
    {
        Vector4 finalClipRegion = this.finalClipRegion;
        float num = finalClipRegion.z * 0.5f;
        float num2 = finalClipRegion.w * 0.5f;
        Vector2 minRect = new Vector2(min.x, min.y);
        Vector2 maxRect = new Vector2(max.x, max.y);
        Vector2 minArea = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
        Vector2 maxArea = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
        if (this.clipping == UIDrawCall.Clipping.SoftClip)
        {
            minArea.x += this.clipSoftness.x;
            minArea.y += this.clipSoftness.y;
            maxArea.x -= this.clipSoftness.x;
            maxArea.y -= this.clipSoftness.y;
        }
        return (Vector3) NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
    }

    public override float CalculateFinalAlpha(int frameID)
    {
        if (this.mAlphaFrameID != frameID)
        {
            this.mAlphaFrameID = frameID;
            UIRect parent = base.parent;
            base.finalAlpha = (base.parent == null) ? this.mAlpha : (parent.CalculateFinalAlpha(frameID) * this.mAlpha);
        }
        return base.finalAlpha;
    }

    public static int CompareFunc(UIPanel a, UIPanel b)
    {
        if (((a == b) || (a == null)) || (b == null))
        {
            return 0;
        }
        if (a.mDepth < b.mDepth)
        {
            return -1;
        }
        if (a.mDepth > b.mDepth)
        {
            return 1;
        }
        return ((a.GetInstanceID() >= b.GetInstanceID()) ? 1 : -1);
    }

    public bool ConstrainTargetToBounds(Transform target, bool immediate)
    {
        Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
        return this.ConstrainTargetToBounds(target, ref targetBounds, immediate);
    }

    public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
    {
        Vector3 vector = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
        if (vector.sqrMagnitude <= 0f)
        {
            return false;
        }
        if (immediate)
        {
            target.localPosition += vector;
            targetBounds.center += vector;
            SpringPosition component = target.GetComponent<SpringPosition>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
        else
        {
            SpringPosition position2 = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
            position2.ignoreTimeScale = true;
            position2.worldSpace = false;
        }
        return true;
    }

    private void FillAllDrawCalls()
    {
        for (int i = 0; i < this.drawCalls.size; i++)
        {
            UIDrawCall.Destroy(this.drawCalls.buffer[i]);
        }
        this.drawCalls.Clear();
        Material mat = null;
        Texture tex = null;
        Shader shader = null;
        UIDrawCall item = null;
        if (this.mSortWidgets)
        {
            this.SortWidgets();
        }
        for (int j = 0; j < this.widgets.size; j++)
        {
            UIWidget widget = this.widgets.buffer[j];
            if (widget.isVisible && widget.hasVertices)
            {
                Material material = widget.material;
                Texture mainTexture = widget.mainTexture;
                Shader shader2 = widget.shader;
                if (((mat != material) || (tex != mainTexture)) || (shader != shader2))
                {
                    if ((item != null) && (item.verts.size != 0))
                    {
                        this.drawCalls.Add(item);
                        item.UpdateGeometry();
                        item = null;
                    }
                    mat = material;
                    tex = mainTexture;
                    shader = shader2;
                }
                if (((mat != null) || (shader != null)) || (tex != null))
                {
                    if (item == null)
                    {
                        item = UIDrawCall.Create(this, mat, tex, shader);
                        item.depthStart = widget.depth;
                        item.depthEnd = item.depthStart;
                        item.panel = this;
                    }
                    else
                    {
                        int depth = widget.depth;
                        if (depth < item.depthStart)
                        {
                            item.depthStart = depth;
                        }
                        if (depth > item.depthEnd)
                        {
                            item.depthEnd = depth;
                        }
                    }
                    widget.drawCall = item;
                    if (this.generateNormals)
                    {
                        widget.WriteToBuffers(item.verts, item.uvs, item.cols, item.norms, item.tans);
                    }
                    else
                    {
                        widget.WriteToBuffers(item.verts, item.uvs, item.cols, null, null);
                    }
                }
            }
            else
            {
                widget.drawCall = null;
            }
        }
        if ((item != null) && (item.verts.size != 0))
        {
            this.drawCalls.Add(item);
            item.UpdateGeometry();
        }
    }

    private bool FillDrawCall(UIDrawCall dc)
    {
        if (dc != null)
        {
            dc.isDirty = false;
            int index = 0;
            while (index < this.widgets.size)
            {
                UIWidget widget = this.widgets[index];
                if (widget == null)
                {
                    this.widgets.RemoveAt(index);
                }
                else
                {
                    if (widget.drawCall == dc)
                    {
                        if (widget.isVisible && widget.hasVertices)
                        {
                            if (this.generateNormals)
                            {
                                widget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans);
                            }
                            else
                            {
                                widget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);
                            }
                        }
                        else
                        {
                            widget.drawCall = null;
                        }
                    }
                    index++;
                }
            }
            if (dc.verts.size != 0)
            {
                dc.UpdateGeometry();
                return true;
            }
        }
        return false;
    }

    public static UIPanel Find(Transform trans)
    {
        return Find(trans, false, -1);
    }

    public static UIPanel Find(Transform trans, bool createIfMissing)
    {
        return Find(trans, createIfMissing, -1);
    }

    public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
    {
        UIPanel component = null;
        while ((component == null) && (trans != null))
        {
            component = trans.GetComponent<UIPanel>();
            if (component != null)
            {
                return component;
            }
            if (trans.parent == null)
            {
                break;
            }
            trans = trans.parent;
        }
        return (!createIfMissing ? null : NGUITools.CreateUI(trans, false, layer));
    }

    public UIDrawCall FindDrawCall(UIWidget w)
    {
        Material material = w.material;
        Texture mainTexture = w.mainTexture;
        int depth = w.depth;
        for (int i = 0; i < this.drawCalls.size; i++)
        {
            UIDrawCall call = this.drawCalls.buffer[i];
            int num3 = (i != 0) ? (this.drawCalls.buffer[i - 1].depthEnd + 1) : -2147483648;
            int num4 = ((i + 1) != this.drawCalls.size) ? (this.drawCalls.buffer[i + 1].depthStart - 1) : 0x7fffffff;
            if ((num3 <= depth) && (num4 >= depth))
            {
                if ((call.baseMaterial == material) && (call.mainTexture == mainTexture))
                {
                    if (w.isVisible)
                    {
                        w.drawCall = call;
                        if (w.hasVertices)
                        {
                            call.isDirty = true;
                        }
                        return call;
                    }
                }
                else
                {
                    this.mRebuild = true;
                }
                return null;
            }
        }
        this.mRebuild = true;
        return null;
    }

    private void FindParent()
    {
        Transform parent = base.cachedTransform.parent;
        this.mParentPanel = (parent == null) ? null : NGUITools.FindInParents<UIPanel>(parent.gameObject);
    }

    public override Vector3[] GetSides(Transform relativeTo)
    {
        if ((this.mClipping == UIDrawCall.Clipping.None) && !this.anchorOffset)
        {
            return base.GetSides(relativeTo);
        }
        Vector2 viewSize = this.GetViewSize();
        Vector2 vector2 = (this.mClipping == UIDrawCall.Clipping.None) ? Vector2.zero : (((Vector2) this.mClipRange) + this.mClipOffset);
        float x = vector2.x - (0.5f * viewSize.x);
        float y = vector2.y - (0.5f * viewSize.y);
        float num3 = x + viewSize.x;
        float num4 = y + viewSize.y;
        float num5 = (x + num3) * 0.5f;
        float num6 = (y + num4) * 0.5f;
        Matrix4x4 localToWorldMatrix = base.cachedTransform.localToWorldMatrix;
        mCorners[0] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(x, num6));
        mCorners[1] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num5, num4));
        mCorners[2] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num3, num6));
        mCorners[3] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num5, y));
        if (relativeTo != null)
        {
            for (int i = 0; i < 4; i++)
            {
                mCorners[i] = relativeTo.InverseTransformPoint(mCorners[i]);
            }
        }
        return mCorners;
    }

    public Vector2 GetViewSize()
    {
        bool flag = this.mClipping != UIDrawCall.Clipping.None;
        Vector2 vector = !flag ? new Vector2((float) Screen.width, (float) Screen.height) : new Vector2(this.mClipRange.z, this.mClipRange.w);
        if (!flag)
        {
            UIRoot root = base.root;
            if (root != null)
            {
                vector = (Vector2) (vector * root.GetPixelSizeAdjustment(Screen.height));
            }
        }
        return vector;
    }

    private Vector2 GetWindowSize()
    {
        UIRoot root = base.root;
        Vector2 vector = new Vector2((float) Screen.width, (float) Screen.height);
        if (root != null)
        {
            vector = (Vector2) (vector * root.GetPixelSizeAdjustment(Screen.height));
        }
        return vector;
    }

    public override void Invalidate(bool includeChildren)
    {
        this.mAlphaFrameID = -1;
        base.Invalidate(includeChildren);
    }

    private void InvalidateClipping()
    {
        this.mResized = true;
        this.mMatrixFrame = -1;
        this.mCullTime = (this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f;
        for (int i = 0; i < list.size; i++)
        {
            UIPanel panel = list[i];
            if ((panel != this) && (panel.parentPanel == this))
            {
                panel.InvalidateClipping();
            }
        }
    }

    public bool IsVisible(UIWidget w)
    {
        if (((this.mClipping == UIDrawCall.Clipping.None) || (this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)) && !w.hideIfOffScreen)
        {
            if (this.clipCount == 0)
            {
                return true;
            }
            if (this.mParentPanel != null)
            {
                return this.mParentPanel.IsVisible(w);
            }
        }
        UIPanel mParentPanel = this;
        Vector3[] worldCorners = w.worldCorners;
        while (mParentPanel != null)
        {
            if (!this.IsVisible(worldCorners[0], worldCorners[1], worldCorners[2], worldCorners[3]))
            {
                return false;
            }
            mParentPanel = mParentPanel.mParentPanel;
        }
        return true;
    }

    public bool IsVisible(Vector3 worldPos)
    {
        if (this.mAlpha < 0.001f)
        {
            return false;
        }
        if ((this.mClipping != UIDrawCall.Clipping.None) && (this.mClipping != UIDrawCall.Clipping.ConstrainButDontClip))
        {
            this.UpdateTransformMatrix();
            Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
            if (vector.x < this.mMin.x)
            {
                return false;
            }
            if (vector.y < this.mMin.y)
            {
                return false;
            }
            if (vector.x > this.mMax.x)
            {
                return false;
            }
            if (vector.y > this.mMax.y)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        this.UpdateTransformMatrix();
        a = this.worldToLocal.MultiplyPoint3x4(a);
        b = this.worldToLocal.MultiplyPoint3x4(b);
        c = this.worldToLocal.MultiplyPoint3x4(c);
        d = this.worldToLocal.MultiplyPoint3x4(d);
        mTemp[0] = a.x;
        mTemp[1] = b.x;
        mTemp[2] = c.x;
        mTemp[3] = d.x;
        float num = Mathf.Min(mTemp);
        float num2 = Mathf.Max(mTemp);
        mTemp[0] = a.y;
        mTemp[1] = b.y;
        mTemp[2] = c.y;
        mTemp[3] = d.y;
        float num3 = Mathf.Min(mTemp);
        float num4 = Mathf.Max(mTemp);
        if (num2 < this.mMin.x)
        {
            return false;
        }
        if (num4 < this.mMin.y)
        {
            return false;
        }
        if (num > this.mMax.x)
        {
            return false;
        }
        if (num3 > this.mMax.y)
        {
            return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        if (mUpdateFrame != Time.frameCount)
        {
            mUpdateFrame = Time.frameCount;
            for (int i = 0; i < list.size; i++)
            {
                list[i].UpdateSelf();
            }
            int a = 0xbb8;
            for (int j = 0; j < list.size; j++)
            {
                UIPanel panel = list.buffer[j];
                if (panel.renderQueue == RenderQueue.Automatic)
                {
                    panel.startingRenderQueue = a;
                    panel.UpdateDrawCalls();
                    a += panel.drawCalls.size;
                }
                else if (panel.renderQueue == RenderQueue.StartAt)
                {
                    panel.UpdateDrawCalls();
                    if (panel.drawCalls.size != 0)
                    {
                        a = Mathf.Max(a, panel.startingRenderQueue + panel.drawCalls.size);
                    }
                }
                else
                {
                    panel.UpdateDrawCalls();
                    if (panel.drawCalls.size != 0)
                    {
                        a = Mathf.Max(a, panel.startingRenderQueue + 1);
                    }
                }
            }
        }
    }

    protected override void OnAnchor()
    {
        if (this.mClipping != UIDrawCall.Clipping.None)
        {
            float num;
            float num2;
            float num3;
            float num4;
            Transform cachedTransform = base.cachedTransform;
            Transform parent = cachedTransform.parent;
            Vector2 viewSize = this.GetViewSize();
            Vector2 localPosition = cachedTransform.localPosition;
            if (((base.leftAnchor.target == base.bottomAnchor.target) && (base.leftAnchor.target == base.rightAnchor.target)) && (base.leftAnchor.target == base.topAnchor.target))
            {
                Vector3[] sides = base.leftAnchor.GetSides(parent);
                if (sides != null)
                {
                    num = NGUIMath.Lerp(sides[0].x, sides[2].x, base.leftAnchor.relative) + base.leftAnchor.absolute;
                    num3 = NGUIMath.Lerp(sides[0].x, sides[2].x, base.rightAnchor.relative) + base.rightAnchor.absolute;
                    num2 = NGUIMath.Lerp(sides[3].y, sides[1].y, base.bottomAnchor.relative) + base.bottomAnchor.absolute;
                    num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, base.topAnchor.relative) + base.topAnchor.absolute;
                }
                else
                {
                    Vector2 localPos = base.GetLocalPos(base.leftAnchor, parent);
                    num = localPos.x + base.leftAnchor.absolute;
                    num2 = localPos.y + base.bottomAnchor.absolute;
                    num3 = localPos.x + base.rightAnchor.absolute;
                    num4 = localPos.y + base.topAnchor.absolute;
                }
            }
            else
            {
                if (base.leftAnchor.target != null)
                {
                    Vector3[] vectorArray2 = base.leftAnchor.GetSides(parent);
                    if (vectorArray2 != null)
                    {
                        num = NGUIMath.Lerp(vectorArray2[0].x, vectorArray2[2].x, base.leftAnchor.relative) + base.leftAnchor.absolute;
                    }
                    else
                    {
                        num = base.GetLocalPos(base.leftAnchor, parent).x + base.leftAnchor.absolute;
                    }
                }
                else
                {
                    num = this.mClipRange.x - (0.5f * viewSize.x);
                }
                if (base.rightAnchor.target != null)
                {
                    Vector3[] vectorArray3 = base.rightAnchor.GetSides(parent);
                    if (vectorArray3 != null)
                    {
                        num3 = NGUIMath.Lerp(vectorArray3[0].x, vectorArray3[2].x, base.rightAnchor.relative) + base.rightAnchor.absolute;
                    }
                    else
                    {
                        num3 = base.GetLocalPos(base.rightAnchor, parent).x + base.rightAnchor.absolute;
                    }
                }
                else
                {
                    num3 = this.mClipRange.x + (0.5f * viewSize.x);
                }
                if (base.bottomAnchor.target != null)
                {
                    Vector3[] vectorArray4 = base.bottomAnchor.GetSides(parent);
                    if (vectorArray4 != null)
                    {
                        num2 = NGUIMath.Lerp(vectorArray4[3].y, vectorArray4[1].y, base.bottomAnchor.relative) + base.bottomAnchor.absolute;
                    }
                    else
                    {
                        num2 = base.GetLocalPos(base.bottomAnchor, parent).y + base.bottomAnchor.absolute;
                    }
                }
                else
                {
                    num2 = this.mClipRange.y - (0.5f * viewSize.y);
                }
                if (base.topAnchor.target != null)
                {
                    Vector3[] vectorArray5 = base.topAnchor.GetSides(parent);
                    if (vectorArray5 != null)
                    {
                        num4 = NGUIMath.Lerp(vectorArray5[3].y, vectorArray5[1].y, base.topAnchor.relative) + base.topAnchor.absolute;
                    }
                    else
                    {
                        num4 = base.GetLocalPos(base.topAnchor, parent).y + base.topAnchor.absolute;
                    }
                }
                else
                {
                    num4 = this.mClipRange.y + (0.5f * viewSize.y);
                }
            }
            num -= localPosition.x + this.mClipOffset.x;
            num3 -= localPosition.x + this.mClipOffset.x;
            num2 -= localPosition.y + this.mClipOffset.y;
            num4 -= localPosition.y + this.mClipOffset.y;
            float x = Mathf.Lerp(num, num3, 0.5f);
            float y = Mathf.Lerp(num2, num4, 0.5f);
            float z = num3 - num;
            float w = num4 - num2;
            float num9 = Mathf.Max(2f, this.mClipSoftness.x);
            float num10 = Mathf.Max(2f, this.mClipSoftness.y);
            if (z < num9)
            {
                z = num9;
            }
            if (w < num10)
            {
                w = num10;
            }
            this.baseClipRegion = new Vector4(x, y, z, w);
        }
    }

    protected override void OnDisable()
    {
        for (int i = 0; i < this.drawCalls.size; i++)
        {
            UIDrawCall dc = this.drawCalls.buffer[i];
            if (dc != null)
            {
                UIDrawCall.Destroy(dc);
            }
        }
        this.drawCalls.Clear();
        list.Remove(this);
        this.mAlphaFrameID = -1;
        this.mMatrixFrame = -1;
        if (list.size == 0)
        {
            UIDrawCall.ReleaseAll();
            mUpdateFrame = -1;
        }
        base.OnDisable();
    }

    protected override void OnInit()
    {
        base.OnInit();
        if (base.rigidbody == null)
        {
            Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
        this.FindParent();
        this.mRebuild = true;
        this.mAlphaFrameID = -1;
        this.mMatrixFrame = -1;
        list.Add(this);
        list.Sort(new BetterList<UIPanel>.CompareFunc(UIPanel.CompareFunc));
    }

    protected override void OnStart()
    {
        this.mLayer = base.mGo.layer;
        UICamera camera = UICamera.FindCameraForLayer(this.mLayer);
        this.mCam = (camera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : camera.cachedCamera;
    }

    public override void ParentHasChanged()
    {
        base.ParentHasChanged();
        this.FindParent();
    }

    [ContextMenu("Force Refresh")]
    public void RebuildAllDrawCalls()
    {
        this.mRebuild = true;
    }

    public void Refresh()
    {
        this.mRebuild = true;
        if (list.size > 0)
        {
            list[0].LateUpdate();
        }
    }

    public void RemoveWidget(UIWidget w)
    {
        if (this.widgets.Remove(w) && (w.drawCall != null))
        {
            int depth = w.depth;
            if ((depth == w.drawCall.depthStart) || (depth == w.drawCall.depthEnd))
            {
                this.mRebuild = true;
            }
            w.drawCall.isDirty = true;
            w.drawCall = null;
        }
    }

    public void SetDirty()
    {
        for (int i = 0; i < this.drawCalls.size; i++)
        {
            this.drawCalls.buffer[i].isDirty = true;
        }
        this.Invalidate(true);
    }

    public override void SetRect(float x, float y, float width, float height)
    {
        int num = Mathf.FloorToInt(width + 0.5f);
        int num2 = Mathf.FloorToInt(height + 0.5f);
        num = (num >> 1) << 1;
        num2 = (num2 >> 1) << 1;
        Transform cachedTransform = base.cachedTransform;
        Vector3 localPosition = cachedTransform.localPosition;
        localPosition.x = Mathf.Floor(x + 0.5f);
        localPosition.y = Mathf.Floor(y + 0.5f);
        if (num < 2)
        {
            num = 2;
        }
        if (num2 < 2)
        {
            num2 = 2;
        }
        this.baseClipRegion = new Vector4(localPosition.x, localPosition.y, (float) num, (float) num2);
        if (base.isAnchored)
        {
            cachedTransform = cachedTransform.parent;
            if (base.leftAnchor.target != null)
            {
                base.leftAnchor.SetHorizontal(cachedTransform, x);
            }
            if (base.rightAnchor.target != null)
            {
                base.rightAnchor.SetHorizontal(cachedTransform, x + width);
            }
            if (base.bottomAnchor.target != null)
            {
                base.bottomAnchor.SetVertical(cachedTransform, y);
            }
            if (base.topAnchor.target != null)
            {
                base.topAnchor.SetVertical(cachedTransform, y + height);
            }
        }
    }

    public void SortWidgets()
    {
        this.mSortWidgets = false;
        this.widgets.Sort(new BetterList<UIWidget>.CompareFunc(UIWidget.PanelCompareFunc));
    }

    private void UpdateDrawCalls()
    {
        Vector3 localPosition;
        Transform cachedTransform = base.cachedTransform;
        bool usedForUI = this.usedForUI;
        if (this.clipping != UIDrawCall.Clipping.None)
        {
            this.drawCallClipRange = this.finalClipRegion;
            this.drawCallClipRange.z *= 0.5f;
            this.drawCallClipRange.w *= 0.5f;
        }
        else
        {
            this.drawCallClipRange = Vector4.zero;
        }
        if (this.drawCallClipRange.z == 0f)
        {
            this.drawCallClipRange.z = Screen.width * 0.5f;
        }
        if (this.drawCallClipRange.w == 0f)
        {
            this.drawCallClipRange.w = Screen.height * 0.5f;
        }
        if (this.halfPixelOffset)
        {
            this.drawCallClipRange.x -= 0.5f;
            this.drawCallClipRange.y += 0.5f;
        }
        if (usedForUI)
        {
            Transform parent = base.cachedTransform.parent;
            localPosition = base.cachedTransform.localPosition;
            if (parent != null)
            {
                float num = Mathf.Round(localPosition.x);
                float num2 = Mathf.Round(localPosition.y);
                this.drawCallClipRange.x += localPosition.x - num;
                this.drawCallClipRange.y += localPosition.y - num2;
                localPosition.x = num;
                localPosition.y = num2;
                localPosition = parent.TransformPoint(localPosition);
            }
            localPosition += this.drawCallOffset;
        }
        else
        {
            localPosition = cachedTransform.position;
        }
        Quaternion rotation = cachedTransform.rotation;
        Vector3 lossyScale = cachedTransform.lossyScale;
        for (int i = 0; i < this.drawCalls.size; i++)
        {
            UIDrawCall call = this.drawCalls.buffer[i];
            Transform transform3 = call.cachedTransform;
            transform3.position = localPosition;
            transform3.rotation = rotation;
            transform3.localScale = lossyScale;
            call.renderQueue = (this.renderQueue != RenderQueue.Explicit) ? (this.startingRenderQueue + i) : this.startingRenderQueue;
            call.alwaysOnScreen = this.alwaysOnScreen && ((this.mClipping == UIDrawCall.Clipping.None) || (this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip));
            call.sortingOrder = this.mSortingOrder;
        }
    }

    private void UpdateLayers()
    {
        if (this.mLayer != base.cachedGameObject.layer)
        {
            this.mLayer = base.mGo.layer;
            UICamera camera = UICamera.FindCameraForLayer(this.mLayer);
            this.mCam = (camera == null) ? NGUITools.FindCameraForLayer(this.mLayer) : camera.cachedCamera;
            NGUITools.SetChildLayer(base.cachedTransform, this.mLayer);
            for (int i = 0; i < this.drawCalls.size; i++)
            {
                this.drawCalls.buffer[i].gameObject.layer = this.mLayer;
            }
        }
    }

    private void UpdateSelf()
    {
        this.mUpdateTime = RealTime.time;
        this.UpdateTransformMatrix();
        this.UpdateLayers();
        this.UpdateWidgets();
        if (this.mRebuild)
        {
            this.mRebuild = false;
            this.FillAllDrawCalls();
        }
        else
        {
            int index = 0;
            while (index < this.drawCalls.size)
            {
                UIDrawCall dc = this.drawCalls.buffer[index];
                if (dc.isDirty && !this.FillDrawCall(dc))
                {
                    UIDrawCall.Destroy(dc);
                    this.drawCalls.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }
    }

    private void UpdateTransformMatrix()
    {
        int frameCount = Time.frameCount;
        if (this.mMatrixFrame != frameCount)
        {
            this.mMatrixFrame = frameCount;
            this.worldToLocal = base.cachedTransform.worldToLocalMatrix;
            Vector2 vector = (Vector2) (this.GetViewSize() * 0.5f);
            float num2 = this.mClipOffset.x + this.mClipRange.x;
            float num3 = this.mClipOffset.y + this.mClipRange.y;
            this.mMin.x = num2 - vector.x;
            this.mMin.y = num3 - vector.y;
            this.mMax.x = num2 + vector.x;
            this.mMax.y = num3 + vector.y;
        }
    }

    private void UpdateWidgets()
    {
        bool flag = !this.cullWhileDragging ? (this.mCullTime > this.mUpdateTime) : false;
        bool flag2 = false;
        if (this.mForced != flag)
        {
            this.mForced = flag;
            this.mResized = true;
        }
        bool hasCumulativeClipping = this.hasCumulativeClipping;
        int index = 0;
        int size = this.widgets.size;
        while (index < size)
        {
            UIWidget w = this.widgets.buffer[index];
            if ((w.panel == this) && w.enabled)
            {
                int frameCount = Time.frameCount;
                if (w.UpdateTransform(frameCount) || this.mResized)
                {
                    bool visibleByAlpha = flag || (w.CalculateCumulativeAlpha(frameCount) > 0.001f);
                    w.UpdateVisibility(visibleByAlpha, flag || ((!hasCumulativeClipping && !w.hideIfOffScreen) || this.IsVisible(w)));
                }
                if (w.UpdateGeometry(frameCount, this.hasAlpha))
                {
                    flag2 = true;
                    if (!this.mRebuild)
                    {
                        if (w.drawCall != null)
                        {
                            w.drawCall.isDirty = true;
                        }
                        else
                        {
                            this.FindDrawCall(w);
                        }
                    }
                }
            }
            index++;
        }
        if (flag2 && (this.onGeometryUpdated != null))
        {
            this.onGeometryUpdated();
        }
        this.mResized = false;
    }

    public override float alpha
    {
        get
        {
            return this.mAlpha;
        }
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mAlpha != num)
            {
                this.mAlphaFrameID = -1;
                this.mResized = true;
                this.mAlpha = num;
                this.SetDirty();
            }
        }
    }

    public Vector4 baseClipRegion
    {
        get
        {
            return this.mClipRange;
        }
        set
        {
            if (((Mathf.Abs((float) (this.mClipRange.x - value.x)) > 0.001f) || (Mathf.Abs((float) (this.mClipRange.y - value.y)) > 0.001f)) || ((Mathf.Abs((float) (this.mClipRange.z - value.z)) > 0.001f) || (Mathf.Abs((float) (this.mClipRange.w - value.w)) > 0.001f)))
            {
                this.mResized = true;
                this.mCullTime = (this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f;
                this.mClipRange = value;
                this.mMatrixFrame = -1;
                UIScrollView component = base.GetComponent<UIScrollView>();
                if (component != null)
                {
                    component.UpdatePosition();
                }
                if (this.onClipMove != null)
                {
                    this.onClipMove(this);
                }
            }
        }
    }

    public override bool canBeAnchored
    {
        get
        {
            return (this.mClipping != UIDrawCall.Clipping.None);
        }
    }

    public int clipCount
    {
        get
        {
            int num = 0;
            for (UIPanel panel = this; panel != null; panel = panel.mParentPanel)
            {
                if (panel.mClipping == UIDrawCall.Clipping.SoftClip)
                {
                    num++;
                }
            }
            return num;
        }
    }

    public Vector2 clipOffset
    {
        get
        {
            return this.mClipOffset;
        }
        set
        {
            if ((Mathf.Abs((float) (this.mClipOffset.x - value.x)) > 0.001f) || (Mathf.Abs((float) (this.mClipOffset.y - value.y)) > 0.001f))
            {
                this.mClipOffset = value;
                this.InvalidateClipping();
                if (this.onClipMove != null)
                {
                    this.onClipMove(this);
                }
            }
        }
    }

    public UIDrawCall.Clipping clipping
    {
        get
        {
            return this.mClipping;
        }
        set
        {
            if (this.mClipping != value)
            {
                this.mResized = true;
                this.mClipping = value;
                this.mMatrixFrame = -1;
            }
        }
    }

    [Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
    public Vector4 clipRange
    {
        get
        {
            return this.baseClipRegion;
        }
        set
        {
            this.baseClipRegion = value;
        }
    }

    [Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
    public bool clipsChildren
    {
        get
        {
            return this.hasCumulativeClipping;
        }
    }

    public Vector2 clipSoftness
    {
        get
        {
            return this.mClipSoftness;
        }
        set
        {
            if (this.mClipSoftness != value)
            {
                this.mClipSoftness = value;
            }
        }
    }

    public int depth
    {
        get
        {
            return this.mDepth;
        }
        set
        {
            if (this.mDepth != value)
            {
                this.mDepth = value;
                list.Sort(new BetterList<UIPanel>.CompareFunc(UIPanel.CompareFunc));
            }
        }
    }

    public Vector3 drawCallOffset
    {
        get
        {
            if ((this.mHalfPixelOffset && (this.mCam != null)) && this.mCam.isOrthoGraphic)
            {
                Vector2 windowSize = this.GetWindowSize();
                float y = (1f / windowSize.y) / this.mCam.orthographicSize;
                return new Vector3(-y, y);
            }
            return Vector3.zero;
        }
    }

    public Vector4 finalClipRegion
    {
        get
        {
            Vector2 viewSize = this.GetViewSize();
            if (this.mClipping != UIDrawCall.Clipping.None)
            {
                return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
            }
            return new Vector4(0f, 0f, viewSize.x, viewSize.y);
        }
    }

    public bool halfPixelOffset
    {
        get
        {
            return this.mHalfPixelOffset;
        }
    }

    public bool hasClipping
    {
        get
        {
            return (this.mClipping == UIDrawCall.Clipping.SoftClip);
        }
    }

    public bool hasCumulativeClipping
    {
        get
        {
            return (this.clipCount != 0);
        }
    }

    public float height
    {
        get
        {
            return this.GetViewSize().y;
        }
    }

    public override Vector3[] localCorners
    {
        get
        {
            if (this.mClipping == UIDrawCall.Clipping.None)
            {
                Vector2 viewSize = this.GetViewSize();
                float x = -0.5f * viewSize.x;
                float y = -0.5f * viewSize.y;
                float num3 = x + viewSize.x;
                float num4 = y + viewSize.y;
                Transform cachedTransform = (this.mCam == null) ? null : this.mCam.transform;
                if (cachedTransform != null)
                {
                    mCorners[0] = cachedTransform.TransformPoint(x, y, 0f);
                    mCorners[1] = cachedTransform.TransformPoint(x, num4, 0f);
                    mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
                    mCorners[3] = cachedTransform.TransformPoint(num3, y, 0f);
                    cachedTransform = base.cachedTransform;
                    for (int i = 0; i < 4; i++)
                    {
                        mCorners[i] = cachedTransform.InverseTransformPoint(mCorners[i]);
                    }
                }
                else
                {
                    mCorners[0] = new Vector3(x, y);
                    mCorners[1] = new Vector3(x, num4);
                    mCorners[2] = new Vector3(num3, num4);
                    mCorners[3] = new Vector3(num3, y);
                }
            }
            else
            {
                float num6 = (this.mClipOffset.x + this.mClipRange.x) - (0.5f * this.mClipRange.z);
                float num7 = (this.mClipOffset.y + this.mClipRange.y) - (0.5f * this.mClipRange.w);
                float num8 = num6 + this.mClipRange.z;
                float num9 = num7 + this.mClipRange.w;
                mCorners[0] = new Vector3(num6, num7);
                mCorners[1] = new Vector3(num6, num9);
                mCorners[2] = new Vector3(num8, num9);
                mCorners[3] = new Vector3(num8, num7);
            }
            return mCorners;
        }
    }

    public static int nextUnusedDepth
    {
        get
        {
            int a = -2147483648;
            for (int i = 0; i < list.size; i++)
            {
                a = Mathf.Max(a, list[i].depth);
            }
            return ((a != -2147483648) ? (a + 1) : 0);
        }
    }

    public UIPanel parentPanel
    {
        get
        {
            return this.mParentPanel;
        }
    }

    public int sortingOrder
    {
        get
        {
            return this.mSortingOrder;
        }
        set
        {
            if (this.mSortingOrder != value)
            {
                this.mSortingOrder = value;
                this.UpdateDrawCalls();
            }
        }
    }

    public bool usedForUI
    {
        get
        {
            return ((this.mCam != null) && this.mCam.isOrthoGraphic);
        }
    }

    public float width
    {
        get
        {
            return this.GetViewSize().x;
        }
    }

    public override Vector3[] worldCorners
    {
        get
        {
            if (this.mClipping == UIDrawCall.Clipping.None)
            {
                Vector2 viewSize = this.GetViewSize();
                float x = -0.5f * viewSize.x;
                float y = -0.5f * viewSize.y;
                float num3 = x + viewSize.x;
                float num4 = y + viewSize.y;
                Transform transform = (this.mCam == null) ? null : this.mCam.transform;
                if (transform != null)
                {
                    mCorners[0] = transform.TransformPoint(x, y, 0f);
                    mCorners[1] = transform.TransformPoint(x, num4, 0f);
                    mCorners[2] = transform.TransformPoint(num3, num4, 0f);
                    mCorners[3] = transform.TransformPoint(num3, y, 0f);
                }
            }
            else
            {
                float num5 = (this.mClipOffset.x + this.mClipRange.x) - (0.5f * this.mClipRange.z);
                float num6 = (this.mClipOffset.y + this.mClipRange.y) - (0.5f * this.mClipRange.w);
                float num7 = num5 + this.mClipRange.z;
                float num8 = num6 + this.mClipRange.w;
                Transform cachedTransform = base.cachedTransform;
                mCorners[0] = cachedTransform.TransformPoint(num5, num6, 0f);
                mCorners[1] = cachedTransform.TransformPoint(num5, num8, 0f);
                mCorners[2] = cachedTransform.TransformPoint(num7, num8, 0f);
                mCorners[3] = cachedTransform.TransformPoint(num7, num6, 0f);
            }
            return mCorners;
        }
    }

    public delegate void OnClippingMoved(UIPanel panel);

    public delegate void OnGeometryUpdated();

    public enum RenderQueue
    {
        Automatic,
        StartAt,
        Explicit
    }
}

