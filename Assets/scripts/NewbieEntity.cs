using FastBuf;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NewbieEntity : GUIEntity
{
    private BoxCollider _bbox;
    private Camera _camera_oth;
    private Camera _camera_perpv;
    private UITexture _dummy_bg_tex;
    private UIPanel _dummy_gui_panel;
    private UIPanel _extract_gui_panel;
    private UITexture _extract_tex;
    private GameObject _fingure_object;
    private GameObject _go_tips;
    private BoxCollider _gui_lock;
    private UIPanel _hang_gui_panel;
    private BoxCollider _lbox;
    private BoxCollider _rbox;
    private BattleMainTalkPop _talk_box;
    private BoxCollider _tbox;
    private List<OriginalInfo> ori_info_list = new List<OriginalInfo>();

    private void CalculateAABBProjected(Camera camera, Vector3 min, Vector3 max, ref float left, ref float right, ref float top, ref float bottom)
    {
        float activeHeight = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
        float num2 = (activeHeight * Screen.width) / ((float) Screen.height);
        Vector3[] vectorArray = new Vector3[] { new Vector3(min.x, max.y, min.z), new Vector3(max.x, max.y, min.z), new Vector3(min.x, min.y, min.z), new Vector3(max.x, min.y, min.z), new Vector3(min.x, max.y, max.z), new Vector3(max.x, max.y, max.z), new Vector3(min.x, min.y, max.z), new Vector3(max.x, min.y, max.z) };
        float maxValue = float.MaxValue;
        float minValue = float.MinValue;
        float a = float.MinValue;
        float num6 = float.MaxValue;
        for (int i = 0; i != vectorArray.Length; i++)
        {
            Vector3 vector = camera.WorldToScreenPoint(vectorArray[i]);
            vector.x -= num2 * 0.5f;
            vector.y -= activeHeight * 0.5f;
            vectorArray[i] = vector;
            maxValue = Mathf.Min(maxValue, vector.x);
            minValue = Mathf.Max(minValue, vector.x);
            a = Mathf.Max(a, vector.y);
            num6 = Mathf.Min(num6, vector.y);
        }
        left = Mathf.Min(left, maxValue);
        right = Mathf.Max(right, minValue);
        top = Mathf.Max(top, a);
        bottom = Mathf.Min(bottom, num6);
    }

    private Bounds CalculateMeshObjectBounds(Camera camera, Transform tns)
    {
        float maxValue = float.MaxValue;
        float minValue = float.MinValue;
        float top = float.MinValue;
        float bottom = float.MaxValue;
        foreach (Component component in tns.GetComponentsInChildren(typeof(Renderer)))
        {
            Renderer renderer = component as Renderer;
            if (null != renderer)
            {
                Bounds bounds = renderer.bounds;
                Vector3 min = bounds.min;
                Vector3 max = bounds.max;
                this.CalculateAABBProjected(camera, min, max, ref maxValue, ref minValue, ref top, ref bottom);
            }
        }
        return new Bounds { center = new Vector3(Mathf.Lerp(maxValue, minValue, 0.5f), Mathf.Lerp(bottom, top, 0.5f), 0f), size = new Vector3(minValue - maxValue, top - bottom, 1f) };
    }

    private Bounds CalculateMeshObjectBoundsPerVertex(Camera camera, Transform tns)
    {
        <CalculateMeshObjectBoundsPerVertex>c__AnonStorey241 storey = new <CalculateMeshObjectBoundsPerVertex>c__AnonStorey241 {
            camera = camera,
            l = float.MaxValue,
            r = float.MinValue,
            t = float.MinValue,
            b = float.MaxValue
        };
        Component[] componentsInChildren = tns.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
        Component[] componentArray2 = tns.GetComponentsInChildren(typeof(MeshFilter));
        UIRoot root = NGUITools.FindInParents<UIRoot>(base.gameObject);
        storey.heightSceen = root.activeHeight;
        storey.widthScreen = (storey.heightSceen * Screen.width) / ((float) Screen.height);
        Action<Transform, Mesh> action = new Action<Transform, Mesh>(storey.<>m__50A);
        foreach (Component component in componentsInChildren)
        {
            SkinnedMeshRenderer renderer = component as SkinnedMeshRenderer;
            if (null != renderer)
            {
                Mesh sharedMesh = renderer.sharedMesh;
                if (null != sharedMesh)
                {
                    action(renderer.transform, sharedMesh);
                }
            }
        }
        foreach (Component component2 in componentArray2)
        {
            MeshFilter filter = component2 as MeshFilter;
            if (null != filter)
            {
                Mesh mesh = filter.mesh;
                if (null != mesh)
                {
                    action(filter.transform, mesh);
                }
            }
        }
        return new Bounds { center = new Vector3(Mathf.Lerp(storey.l, storey.r, 0.5f), Mathf.Lerp(storey.b, storey.t, 0.5f), 0f), size = new Vector3(storey.r - storey.l, storey.t - storey.b, 1f) };
    }

    private void CalculateMeshObjectsBounds(Camera camera, Transform[] tns, ref Bounds[] hangs, ref Bounds bounds, bool per_vertex)
    {
        float maxValue = float.MaxValue;
        float minValue = float.MinValue;
        float a = float.MinValue;
        float num4 = float.MaxValue;
        for (int i = 0; i != tns.Length; i++)
        {
            Transform transform = tns[i];
            if (null != transform)
            {
                BoxCollider component = transform.GetComponent<BoxCollider>();
                if ((null != component) && component.enabled)
                {
                    float left = float.MaxValue;
                    float right = float.MinValue;
                    float top = float.MinValue;
                    float bottom = float.MaxValue;
                    Vector3 center = component.center;
                    Vector3 extents = component.extents;
                    Vector3 min = transform.localToWorldMatrix.MultiplyPoint(center - extents);
                    Vector3 max = transform.localToWorldMatrix.MultiplyPoint(center + extents);
                    this.CalculateAABBProjected(camera, min, max, ref left, ref right, ref top, ref bottom);
                    hangs[i].center = new Vector3(Mathf.Lerp(left, right, 0.5f), Mathf.Lerp(bottom, top, 0.5f), 0f);
                    hangs[i].size = new Vector3(right - left, top - bottom, 1f);
                }
                else
                {
                    hangs[i] = !per_vertex ? this.CalculateMeshObjectBounds(camera, transform) : this.CalculateMeshObjectBoundsPerVertex(camera, transform);
                }
                maxValue = Mathf.Min(maxValue, hangs[i].min.x);
                minValue = Mathf.Max(minValue, hangs[i].max.x);
                a = Mathf.Max(a, hangs[i].max.y);
                num4 = Mathf.Min(num4, hangs[i].min.y);
            }
        }
        bounds.center = new Vector3(Mathf.Lerp(maxValue, minValue, 0.5f), Mathf.Lerp(num4, a, 0.5f), 0f);
        bounds.size = new Vector3(minValue - maxValue, a - num4, 1f);
    }

    private void CalculateRelativeBounds(Transform[] tns, ref Bounds[] hangs, ref Bounds bounds)
    {
        float maxValue = float.MaxValue;
        float minValue = float.MinValue;
        float a = float.MinValue;
        float num4 = float.MaxValue;
        for (int i = 0; i != tns.Length; i++)
        {
            Component[] componentsInChildren = tns[i].GetComponentsInChildren(typeof(UITexture));
            Transform[] transformArray = null;
            if (componentsInChildren.Length > 0)
            {
                transformArray = new Transform[componentsInChildren.Length];
                for (int j = 0; j != componentsInChildren.Length; j++)
                {
                    transformArray[j] = componentsInChildren[j].transform.parent;
                    componentsInChildren[j].transform.parent = null;
                }
            }
            hangs[i] = NGUIMath.CalculateRelativeWidgetBounds(base.transform, tns[i]);
            maxValue = Mathf.Min(maxValue, hangs[i].min.x);
            minValue = Mathf.Max(minValue, hangs[i].max.x);
            a = Mathf.Max(a, hangs[i].max.y);
            num4 = Mathf.Min(num4, hangs[i].min.y);
            if (componentsInChildren.Length > 0)
            {
                for (int k = 0; k != componentsInChildren.Length; k++)
                {
                    componentsInChildren[k].transform.parent = transformArray[k];
                }
            }
        }
        bounds.center = new Vector3(Mathf.Lerp(maxValue, minValue, 0.5f), Mathf.Lerp(num4, a, 0.5f), 0f);
        bounds.size = new Vector3(minValue - maxValue, a - num4, 1f);
    }

    private void CopyCameraProperties(Camera from, Camera to)
    {
        if ((null != from) && (null != to))
        {
            to.transform.localScale = from.transform.localScale;
            to.transform.position = from.transform.position;
            to.transform.rotation = from.transform.rotation;
            to.aspect = from.aspect;
            to.nearClipPlane = from.nearClipPlane;
            to.farClipPlane = from.farClipPlane;
            to.fieldOfView = from.fieldOfView;
        }
    }

    public Bounds Generate2DMaskSampler(int extract_flag, List<GameObject> host_objects)
    {
        Bounds bounds = new Bounds();
        if (host_objects != null)
        {
            int count = host_objects.Count;
            if (count <= 0)
            {
                return bounds;
            }
            Bounds[] hangs = new Bounds[count];
            Transform[] tns = new Transform[count];
            this.DummyUIPanel.gameObject.SetActive(true);
            this.DummyBackgroundTexture.gameObject.SetActive(true);
            for (int i = 0; i != count; i++)
            {
                GameObject go = host_objects[i];
                if (null != go)
                {
                    OriginalInfo item = new OriginalInfo {
                        obj = go,
                        layer = go.layer,
                        paraent = go.transform.parent
                    };
                    UIWidget component = go.GetComponent<UIWidget>();
                    if (null != component)
                    {
                        item.panel = component.panel;
                    }
                    Vector3 position = go.transform.position;
                    Vector3 localScale = go.transform.localScale;
                    Quaternion rotation = go.transform.rotation;
                    go.transform.parent = this.DummyUIPanel.transform;
                    go.transform.position = position;
                    go.transform.localScale = localScale;
                    go.transform.rotation = rotation;
                    go.SetActive(false);
                    go.SetActive(true);
                    if ((extract_flag & 2) != 0)
                    {
                        TweenAlpha alpha = TweenAlpha.Begin(go, 1f, 0.4f);
                        alpha.from = 0.5f;
                        alpha.to = 1.5f;
                        alpha.duration = 0.5f;
                        alpha.method = UITweener.Method.EaseInOut;
                        alpha.style = UITweener.Style.PingPong;
                    }
                    tns[i] = go.transform;
                    this.ori_info_list.Add(item);
                }
            }
            this.CalculateRelativeBounds(tns, ref hangs, ref bounds);
            if ((extract_flag & 1) != 0)
            {
                float l = bounds.center.x - (bounds.size.x * 0.5f);
                float r = bounds.center.x + (bounds.size.x * 0.5f);
                float t = bounds.center.y + (bounds.size.y * 0.5f);
                float b = bounds.center.y - (bounds.size.y * 0.5f);
                this.GenerateColliderGroup(l, r, t, b);
            }
        }
        return bounds;
    }

    public void Generate2DTips(int extract_flag, List<GameObject> host_objects)
    {
        this.DummyUIPanel.gameObject.SetActive(true);
        this.DummyBackgroundTexture.gameObject.SetActive(false);
        int count = host_objects.Count;
        for (int i = 0; i != count; i++)
        {
            GameObject go = host_objects[i];
            if (null != go)
            {
                OriginalInfo item = new OriginalInfo {
                    obj = go,
                    layer = go.layer,
                    paraent = go.transform.parent
                };
                UIWidget component = go.GetComponent<UIWidget>();
                if (null != component)
                {
                    item.panel = component.panel;
                }
                Vector3 position = go.transform.position;
                Vector3 localScale = go.transform.localScale;
                Quaternion rotation = go.transform.rotation;
                go.transform.parent = this.DummyUIPanel.transform;
                go.transform.position = position;
                go.transform.localScale = localScale;
                go.transform.rotation = rotation;
                go.SetActive(false);
                go.SetActive(true);
                if ((extract_flag & 2) != 0)
                {
                    TweenAlpha alpha = TweenAlpha.Begin(go, 1f, 0.4f);
                    alpha.from = 0.5f;
                    alpha.to = 1.5f;
                    alpha.duration = 0.5f;
                    alpha.method = UITweener.Method.EaseInOut;
                    alpha.style = UITweener.Style.PingPong;
                }
                this.ori_info_list.Add(item);
            }
        }
    }

    public Bounds Generate3DMaskSampler(int extract_flag, List<GameObject> host_objects)
    {
        Bounds bounds = new Bounds();
        int count = host_objects.Count;
        if (count > 0)
        {
            Bounds[] hangs = new Bounds[count];
            Transform transform = null;
            Transform[] tns = new Transform[count];
            int layer = LayerMask.NameToLayer("DummyMask");
            this.ExtractPanel.gameObject.SetActive(true);
            this.PerspectiveCamera.enabled = true;
            this.ResetMaskDimentions();
            this.CopyCameraProperties(Camera.main, this.PerspectiveCamera);
            for (int i = 0; i != count; i++)
            {
                GameObject go = host_objects[i];
                if (null != go)
                {
                    OriginalInfo item = new OriginalInfo {
                        obj = go,
                        layer = go.layer,
                        paraent = go.transform.parent
                    };
                    tns[i] = go.transform;
                    if (null == transform)
                    {
                        transform = go.transform;
                    }
                    this.ori_info_list.Add(item);
                    MTDLayers.SetlayerRecursively(go, layer);
                }
            }
            if ((extract_flag & 2) != 0)
            {
                DynamicTweenAlpha alpha = UITweener.Begin<DynamicTweenAlpha>(this.ExtractTexture.gameObject, 1f);
                alpha.from = 0.5f;
                alpha.to = 1.5f;
                alpha.duration = 0.5f;
                alpha.method = UITweener.Method.EaseInOut;
                alpha.style = UITweener.Style.PingPong;
            }
            if ((extract_flag & 1) != 0)
            {
                this.ExtractTexture.shader = null;
                this.ExtractTexture.material = new Material(Shader.Find("Unlit/GUI3D - ColliderMask"));
                this.ExtractTexture.mainTexture = this.PerspectiveCamera.targetTexture;
            }
            else
            {
                this.ExtractTexture.shader = null;
                this.ExtractTexture.material = new Material(Shader.Find("Unlit/GUI3D - MaskGloss"));
                this.ExtractTexture.mainTexture = this.PerspectiveCamera.targetTexture;
            }
            bool flag = 0 != (extract_flag & 8);
            if (((extract_flag & 0x10) != 0) && (null != transform))
            {
                Transform[] transformArray1 = new Transform[] { transform };
                this.CalculateMeshObjectsBounds(this.PerspectiveCamera, transformArray1, ref hangs, ref bounds, flag);
            }
            else
            {
                this.CalculateMeshObjectsBounds(this.PerspectiveCamera, tns, ref hangs, ref bounds, flag);
            }
            if ((extract_flag & 1) != 0)
            {
                float l = bounds.center.x - (bounds.size.x * 0.5f);
                float r = bounds.center.x + (bounds.size.x * 0.5f);
                float t = bounds.center.y + (bounds.size.y * 0.5f);
                float b = bounds.center.y - (bounds.size.y * 0.5f);
                this.GenerateColliderGroup(l, r, t, b);
            }
        }
        return bounds;
    }

    private void GenerateColliderGroup(float l, float r, float t, float b)
    {
        UIRoot root = NGUITools.FindInParents<UIRoot>(base.gameObject);
        float activeHeight = root.activeHeight;
        float activeWidth = root.activeWidth;
        this.LeftBoxCollider.gameObject.SetActive(true);
        this.LeftBoxCollider.center = new Vector3(-activeWidth * 0.5f, 0f, 0f);
        this.LeftBoxCollider.size = new Vector3(((activeWidth * 0.5f) + l) * 2f, activeHeight, 1f);
        this.LeftBoxCollider.transform.localPosition = Vector3.zero;
        this.LeftBoxCollider.transform.localScale = Vector3.one;
        this.RightBoxCollider.gameObject.SetActive(true);
        this.RightBoxCollider.size = new Vector3(((activeWidth * 0.5f) - r) * 2f, activeHeight, 1f);
        this.RightBoxCollider.center = new Vector3(activeWidth * 0.5f, 0f, 0f);
        this.RightBoxCollider.transform.localPosition = Vector3.zero;
        this.RightBoxCollider.transform.localScale = Vector3.one;
        this.TopBoxCollider.gameObject.SetActive(true);
        this.TopBoxCollider.size = new Vector3(activeWidth, ((activeHeight * 0.5f) - t) * 2f, 1f);
        this.TopBoxCollider.center = new Vector3(0f, activeHeight * 0.5f, 0f);
        this.TopBoxCollider.transform.localPosition = Vector3.zero;
        this.TopBoxCollider.transform.localScale = Vector3.one;
        this.BottomBoxCollider.gameObject.SetActive(true);
        this.BottomBoxCollider.size = new Vector3(activeWidth, ((activeHeight * 0.5f) + b) * 2f, 1f);
        this.BottomBoxCollider.center = new Vector3(0f, -activeHeight * 0.5f, 0f);
        this.BottomBoxCollider.transform.localPosition = Vector3.zero;
        this.BottomBoxCollider.transform.localScale = Vector3.one;
    }

    public Bounds GenerateHangerMask(int extract_flag, List<GameObject> host_objects, int hang_layer)
    {
        Bounds bounds = new Bounds();
        if (host_objects != null)
        {
            int count = host_objects.Count;
            if (count <= 0)
            {
                return bounds;
            }
            Bounds[] hangs = new Bounds[count];
            Transform[] tns = new Transform[count];
            this.ExtractPanel.gameObject.SetActive(true);
            this.HangerPanel.gameObject.SetActive(true);
            this.OrthograhpicCamera.enabled = true;
            this.OrthograhpicCamera.transform.localPosition = Vector3.zero;
            for (int i = 0; i != count; i++)
            {
                GameObject original = host_objects[i];
                if (null != original)
                {
                    GameObject go = UnityEngine.Object.Instantiate(original) as GameObject;
                    MTDLayers.SetlayerRecursively(go, LayerMask.NameToLayer("DummyMask"));
                    go.transform.parent = this.HangerPanel.transform;
                    go.transform.position = original.transform.position;
                    go.transform.rotation = original.transform.rotation;
                    go.transform.localScale = original.transform.localScale;
                    tns[i] = original.transform;
                }
            }
            if ((extract_flag & 2) != 0)
            {
                DynamicTweenAlpha alpha = UITweener.Begin<DynamicTweenAlpha>(this.ExtractTexture.gameObject, 1f);
                alpha.from = 0.5f;
                alpha.to = 1.2f;
                alpha.duration = 0.6f;
                alpha.method = UITweener.Method.EaseInOut;
                alpha.style = UITweener.Style.PingPong;
            }
            GameObject obj4 = host_objects[0];
            this.ExtractPanel.transform.parent = obj4.transform;
            this.ExtractPanel.transform.position = Vector3.zero;
            this.ExtractTexture.shader = null;
            this.ExtractTexture.material = new Material(Shader.Find("Unlit/GUI - MaskGloss"));
            this.ExtractTexture.mainTexture = this.OrthograhpicCamera.targetTexture;
            MTDLayers.SetlayerRecursively(this.ExtractPanel.gameObject, hang_layer);
            this.CalculateRelativeBounds(tns, ref hangs, ref bounds);
        }
        return bounds;
    }

    public Bounds GenerateMaskBounds(int extract_flag, List<GameObject> host_objects)
    {
        Bounds bounds = new Bounds();
        int count = host_objects.Count;
        if (count > 0)
        {
            Bounds[] hangs = new Bounds[count];
            Transform[] tns = new Transform[count];
            for (int i = 0; i != count; i++)
            {
                GameObject obj2 = host_objects[i];
                if (null != obj2)
                {
                    tns[i] = obj2.transform;
                }
            }
            this.CalculateRelativeBounds(tns, ref hangs, ref bounds);
        }
        return bounds;
    }

    public void LockAllGUI()
    {
        this.GUILayerLock.gameObject.SetActive(true);
        Utility.RegisterMainUILockObject(this.GUILayerLock.gameObject);
    }

    public override void OnInitialize()
    {
        float activeWidth = GUIMgr.Instance.Root.activeWidth;
        float activeHeight = GUIMgr.Instance.Root.activeHeight;
        this.ExtractTexture.transform.localScale = new Vector3(activeWidth, activeHeight, 1f);
        RenderTexture texture = new RenderTexture(Mathf.FloorToInt(activeWidth), Mathf.FloorToInt(activeHeight), 8, RenderTextureFormat.ARGB32);
        this.PerspectiveCamera.targetTexture = texture;
        this.OrthograhpicCamera.targetTexture = texture;
    }

    public override void OnRelease()
    {
        this.RecoverHostObjects();
    }

    private void RecoverHostObjects()
    {
        int count = this.ori_info_list.Count;
        for (int i = 0; i != count; i++)
        {
            OriginalInfo info = this.ori_info_list[i];
            if (info != null)
            {
                GameObject go = info.obj;
                if (null != go)
                {
                    go.transform.parent = info.paraent;
                    MTDLayers.SetlayerRecursively(go, info.layer);
                    TweenAlpha component = go.GetComponent<TweenAlpha>();
                    if (null != component)
                    {
                        component.value = 1f;
                        component.enabled = false;
                    }
                    go.SetActive(false);
                    go.SetActive(true);
                }
            }
        }
        this.ori_info_list.Clear();
        this.ExtractPanel.transform.parent = base.transform;
        this.TipsObject.transform.parent = base.transform;
        this.FingureObject.transform.parent = base.transform;
        MTDLayers.SetlayerRecursively(this.ExtractPanel.gameObject, LayerMask.NameToLayer("GUI"));
        MTDLayers.SetlayerRecursively(this.TipsObject, LayerMask.NameToLayer("TipsUI"));
        MTDLayers.SetlayerRecursively(this.FingureObject, LayerMask.NameToLayer("GUI"));
        count = this.HangerPanel.transform.childCount;
        for (int j = 0; j != count; j++)
        {
            Transform child = this.HangerPanel.transform.GetChild(j);
            if (null != child)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
    }

    public void Reset()
    {
        this.RecoverHostObjects();
        this.LeftBoxCollider.gameObject.SetActive(false);
        this.RightBoxCollider.gameObject.SetActive(false);
        this.TopBoxCollider.gameObject.SetActive(false);
        this.BottomBoxCollider.gameObject.SetActive(false);
        this.PerspectiveCamera.enabled = false;
        this.OrthograhpicCamera.enabled = false;
        this.ExtractTexture.mainTexture = null;
        this.ExtractPanel.gameObject.SetActive(false);
        this.TipsObject.SetActive(false);
        this.TalkBoxPopo.gameObject.SetActive(false);
        this.FingureObject.SetActive(false);
        this.DummyUIPanel.gameObject.SetActive(false);
        this.HangerPanel.gameObject.SetActive(false);
        this.GUILayerLock.gameObject.SetActive(false);
        Utility.RegisterMainUILockObject(null);
        base.ResetDepth();
        UIPanel component = this.TipsObject.GetComponent<UIPanel>();
        component.depth += 2;
        UIPanel local2 = this.FingureObject.GetComponent<UIPanel>();
        local2.depth += 2;
    }

    private void ResetMaskDimentions()
    {
        int activeWidth = GUIMgr.Instance.Root.activeWidth;
        int activeHeight = GUIMgr.Instance.Root.activeHeight;
        this.ExtractTexture.width = activeWidth;
        this.ExtractTexture.height = activeHeight;
        this.ExtractTexture.transform.localScale = Vector3.one;
        this.ExtractTexture.transform.localPosition = Vector3.zero;
        this.ExtractTexture.transform.localRotation = Quaternion.identity;
        this.PerspectiveCamera.transform.localScale = Vector3.one;
    }

    public void ShowFingure(Bounds host_bounds, Vector3 offset, GameObject hang_object, int layer)
    {
        this.FingureObject.SetActive(true);
        Vector3 pos = host_bounds.center + offset;
        this.FingureObject.transform.localPosition = pos;
        if (null != hang_object)
        {
            Vector3 vector2 = this.FingureObject.transform.position;
            this.FingureObject.transform.parent = hang_object.transform;
            this.FingureObject.transform.localPosition = Vector3.zero + offset;
        }
        MTDLayers.SetlayerRecursively(this.FingureObject, layer);
        Transform transform = this.FingureObject.transform.FindChild("obj");
        if (null != transform)
        {
            TweenPosition position = TweenPosition.Begin(transform.gameObject, 0.6f, pos);
            position.method = UITweener.Method.EaseOut;
            position.style = UITweener.Style.PingPong;
            position.from = new Vector3(2.5f, -2.5f, 0f);
            position.to = Vector3.zero;
            TweenScale scale = TweenScale.Begin(transform.gameObject, 0.6f, pos);
            scale.method = UITweener.Method.EaseOut;
            scale.style = UITweener.Style.PingPong;
            scale.from = Vector3.one;
            scale.to = new Vector3(0.78f, 0.78f, 1f);
        }
    }

    public void ShowTalkBox(int identity, System.Action call_back)
    {
        <ShowTalkBox>c__AnonStorey240 storey = new <ShowTalkBox>c__AnonStorey240 {
            call_back = call_back,
            <>f__this = this
        };
        tutorial_dialog_config _config = ConfigMgr.getInstance().getByEntry<tutorial_dialog_config>(identity);
        if (_config != null)
        {
            this.TalkBoxPopo.gameObject.SetActive(true);
            this.TalkBoxPopo.ShowTalkPop(_config.talk_pos, -1, string.Empty, _config.content, new System.Action(storey.<>m__509), null);
        }
    }

    public void ShowTips(int identity, Bounds host_bounds, TipsGizmo.Anchor anchor, Vector3 offset, int layer, GameObject hang_object, bool screen_redirect)
    {
        tutorial_dialog_config _config = ConfigMgr.getInstance().getByEntry<tutorial_dialog_config>(identity);
        if (_config != null)
        {
            this.TipsObject.SetActive(true);
            this.TipsObject.transform.FindChild("text").GetComponent<UILabel>().text = _config.content;
            Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.transform, this.TipsObject.transform);
            float x = 0f;
            float y = 0f;
            float num3 = host_bounds.center.x;
            float num4 = host_bounds.center.y;
            float num5 = bounds.size.x;
            float num6 = bounds.size.y;
            float num7 = host_bounds.size.x;
            float num8 = host_bounds.size.y;
            switch (anchor)
            {
                case TipsGizmo.Anchor.LEFT:
                    x = (num3 - (num7 * 0.5f)) - (num5 * 0.5f);
                    y = num4;
                    break;

                case TipsGizmo.Anchor.RIGHT:
                    x = (num3 + (num7 * 0.5f)) + (num5 * 0.5f);
                    y = num4;
                    break;

                case TipsGizmo.Anchor.TOP:
                    x = num3;
                    y = (num4 + (num8 * 0.5f)) + (num6 * 0.5f);
                    break;

                case TipsGizmo.Anchor.BOTTOM:
                    x = num3;
                    y = (num4 - (num8 * 0.5f)) - (num6 * 0.5f);
                    break;

                case TipsGizmo.Anchor.TOP_LEFT:
                    x = (num3 - (num7 * 0.5f)) - (num5 * 0.5f);
                    y = (num4 + (num8 * 0.5f)) + (num6 * 0.5f);
                    break;

                case TipsGizmo.Anchor.BOTTOM_LEFT:
                    x = (num3 - (num7 * 0.5f)) - (num5 * 0.5f);
                    y = (num4 - (num8 * 0.5f)) - (num6 * 0.5f);
                    break;

                case TipsGizmo.Anchor.TOP_RIGHT:
                    x = (num3 + (num7 * 0.5f)) + (num5 * 0.5f);
                    y = (num4 + (num8 * 0.5f)) + (num6 * 0.5f);
                    break;

                case TipsGizmo.Anchor.BOTTOM_RIGHT:
                    x = (num3 + (num7 * 0.5f)) + (num5 * 0.5f);
                    y = (num4 - (num8 * 0.5f)) - (num6 * 0.5f);
                    break;
            }
            Vector3 vector = new Vector3(x, y, 0f) + offset;
            if (screen_redirect)
            {
                float activeWidth = GUIMgr.Instance.Root.activeWidth;
                float activeHeight = GUIMgr.Instance.Root.activeHeight;
                float b = (activeHeight * 0.5f) - (num6 * 0.5f);
                float num12 = (-activeHeight * 0.5f) + (num6 * 0.5f);
                float num13 = (-activeWidth * 0.5f) + (num5 * 0.5f);
                float num14 = (activeWidth * 0.5f) - (num5 * 0.5f);
                vector.x = Mathf.Min(vector.x, num14);
                vector.x = Mathf.Max(vector.x, num13);
                vector.y = Mathf.Min(vector.y, b);
                vector.y = Mathf.Max(vector.y, num12);
            }
            this.TipsObject.transform.localPosition = vector;
            if (null != hang_object)
            {
                Vector3 position = this.TipsObject.transform.position;
                this.TipsObject.transform.parent = hang_object.transform;
                this.TipsObject.transform.position = position;
            }
            MTDLayers.SetlayerRecursively(this.TipsObject, layer);
        }
    }

    private BoxCollider BottomBoxCollider
    {
        get
        {
            if (null == this._bbox)
            {
                this._bbox = base.transform.FindChild("bBox").GetComponent<BoxCollider>();
            }
            return this._bbox;
        }
    }

    private UITexture DummyBackgroundTexture
    {
        get
        {
            if (null == this._dummy_bg_tex)
            {
                this._dummy_bg_tex = base.transform.FindChild("Dummy/bg").GetComponent<UITexture>();
            }
            return this._dummy_bg_tex;
        }
    }

    private UIPanel DummyUIPanel
    {
        get
        {
            if (null == this._dummy_gui_panel)
            {
                this._dummy_gui_panel = base.transform.FindChild("Dummy").GetComponent<UIPanel>();
            }
            return this._dummy_gui_panel;
        }
    }

    private UIPanel ExtractPanel
    {
        get
        {
            if (null == this._extract_gui_panel)
            {
                this._extract_gui_panel = base.transform.FindChild("Extract").GetComponent<UIPanel>();
            }
            return this._extract_gui_panel;
        }
    }

    private UITexture ExtractTexture
    {
        get
        {
            if (null == this._extract_tex)
            {
                this._extract_tex = base.transform.FindChild("Extract/extractTex").GetComponent<UITexture>();
            }
            return this._extract_tex;
        }
    }

    private GameObject FingureObject
    {
        get
        {
            if (null == this._fingure_object)
            {
                this._fingure_object = base.transform.FindChild("fingure").gameObject;
            }
            return this._fingure_object;
        }
    }

    private BoxCollider GUILayerLock
    {
        get
        {
            if (null == this._gui_lock)
            {
                this._gui_lock = base.transform.FindChild("GUILayerLock").GetComponent<BoxCollider>();
            }
            return this._gui_lock;
        }
    }

    private UIPanel HangerPanel
    {
        get
        {
            if (null == this._hang_gui_panel)
            {
                this._hang_gui_panel = base.transform.FindChild("othCamera/Dummy").GetComponent<UIPanel>();
            }
            return this._hang_gui_panel;
        }
    }

    private BoxCollider LeftBoxCollider
    {
        get
        {
            if (null == this._lbox)
            {
                this._lbox = base.transform.FindChild("lBox").GetComponent<BoxCollider>();
            }
            return this._lbox;
        }
    }

    private Camera OrthograhpicCamera
    {
        get
        {
            if (null == this._camera_oth)
            {
                this._camera_oth = base.transform.FindChild("othCamera").GetComponent<Camera>();
            }
            return this._camera_oth;
        }
    }

    private Camera PerspectiveCamera
    {
        get
        {
            if (null == this._camera_perpv)
            {
                this._camera_perpv = base.transform.FindChild("perpvCamera").GetComponent<Camera>();
            }
            return this._camera_perpv;
        }
    }

    private BoxCollider RightBoxCollider
    {
        get
        {
            if (null == this._rbox)
            {
                this._rbox = base.transform.FindChild("rBox").GetComponent<BoxCollider>();
            }
            return this._rbox;
        }
    }

    private BattleMainTalkPop TalkBoxPopo
    {
        get
        {
            if (null == this._talk_box)
            {
                this._talk_box = base.transform.FindChild("TalkBox").GetComponent<BattleMainTalkPop>();
            }
            return this._talk_box;
        }
    }

    private GameObject TipsObject
    {
        get
        {
            if (null == this._go_tips)
            {
                this._go_tips = base.transform.FindChild("TipsBox").gameObject;
            }
            return this._go_tips;
        }
    }

    private BoxCollider TopBoxCollider
    {
        get
        {
            if (null == this._tbox)
            {
                this._tbox = base.transform.FindChild("tBox").GetComponent<BoxCollider>();
            }
            return this._tbox;
        }
    }

    [CompilerGenerated]
    private sealed class <CalculateMeshObjectBoundsPerVertex>c__AnonStorey241
    {
        internal float b;
        internal Camera camera;
        internal float heightSceen;
        internal float l;
        internal float r;
        internal float t;
        internal float widthScreen;

        internal void <>m__50A(Transform tnf, Mesh mesh)
        {
            float maxValue = float.MaxValue;
            float minValue = float.MinValue;
            float a = float.MinValue;
            float num4 = float.MaxValue;
            for (int i = 0; i != mesh.vertexCount; i++)
            {
                Vector3 v = mesh.vertices[i];
                v = tnf.localToWorldMatrix.MultiplyPoint(v);
                v = this.camera.WorldToScreenPoint(v);
                v.x -= this.widthScreen * 0.5f;
                v.y -= this.heightSceen * 0.5f;
                maxValue = Mathf.Min(maxValue, v.x);
                minValue = Mathf.Max(minValue, v.x);
                a = Mathf.Max(a, v.y);
                num4 = Mathf.Min(num4, v.y);
            }
            this.l = Mathf.Min(this.l, maxValue);
            this.r = Mathf.Max(this.r, minValue);
            this.t = Mathf.Max(this.t, a);
            this.b = Mathf.Min(this.b, num4);
        }
    }

    [CompilerGenerated]
    private sealed class <ShowTalkBox>c__AnonStorey240
    {
        internal NewbieEntity <>f__this;
        internal System.Action call_back;

        internal void <>m__509()
        {
            this.<>f__this.TalkBoxPopo.gameObject.SetActive(false);
            if (this.call_back != null)
            {
                this.call_back();
            }
        }
    }

    public class OriginalInfo
    {
        [HideInInspector]
        public int layer;
        [HideInInspector]
        public GameObject obj;
        [HideInInspector]
        public UIPanel panel;
        [HideInInspector]
        public Transform paraent;
    }
}

