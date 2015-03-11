using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class NGUITools
{
    private static float mGlobalVolume = 1f;
    private static AudioListener mListener;
    private static bool mLoaded = false;
    private static Vector3[] mSides = new Vector3[4];

    private static void Activate(Transform t)
    {
        Activate(t, false);
    }

    private static void Activate(Transform t, bool compatibilityMode)
    {
        SetActiveSelf(t.gameObject, true);
        if (compatibilityMode)
        {
            int index = 0;
            int childCount = t.childCount;
            while (index < childCount)
            {
                if (t.GetChild(index).gameObject.activeSelf)
                {
                    return;
                }
                index++;
            }
            int num3 = 0;
            int num4 = t.childCount;
            while (num3 < num4)
            {
                Activate(t.GetChild(num3), true);
                num3++;
            }
        }
    }

    public static GameObject AddChild(GameObject parent)
    {
        return AddChild(parent, true);
    }

    public static T AddChild<T>(GameObject parent) where T: Component
    {
        GameObject obj2 = AddChild(parent);
        obj2.name = GetTypeName<T>();
        return obj2.AddComponent<T>();
    }

    public static GameObject AddChild(GameObject parent, bool undo)
    {
        GameObject obj2 = new GameObject();
        if (parent != null)
        {
            Transform transform = obj2.transform;
            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            obj2.layer = parent.layer;
        }
        return obj2;
    }

    public static T AddChild<T>(GameObject parent, bool undo) where T: Component
    {
        GameObject obj2 = AddChild(parent, undo);
        obj2.name = GetTypeName<T>();
        return obj2.AddComponent<T>();
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate(prefab) as GameObject;
        if ((obj2 != null) && (parent != null))
        {
            Transform transform = obj2.transform;
            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            obj2.layer = parent.layer;
        }
        return obj2;
    }

    public static T AddMissingComponent<T>(this GameObject go) where T: Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static UISprite AddSprite(GameObject go, UIAtlas atlas, string spriteName)
    {
        UISpriteData data = (atlas == null) ? null : atlas.GetSprite(spriteName);
        UISprite sprite = AddWidget<UISprite>(go);
        sprite.type = ((data != null) && data.hasBorder) ? UIBasicSprite.Type.Sliced : UIBasicSprite.Type.Simple;
        sprite.atlas = atlas;
        sprite.spriteName = spriteName;
        return sprite;
    }

    public static T AddWidget<T>(GameObject go) where T: UIWidget
    {
        int num = CalculateNextDepth(go);
        T local = AddChild<T>(go);
        local.width = 100;
        local.height = 100;
        local.depth = num;
        local.gameObject.layer = go.layer;
        return local;
    }

    public static void AddWidgetCollider(GameObject go)
    {
        AddWidgetCollider(go, false);
    }

    public static void AddWidgetCollider(GameObject go, bool considerInactive)
    {
        if (go != null)
        {
            Collider component = go.GetComponent<Collider>();
            BoxCollider box = component as BoxCollider;
            if (box != null)
            {
                UpdateWidgetCollider(box, considerInactive);
            }
            else if (component == null)
            {
                BoxCollider2D colliderd = go.GetComponent<BoxCollider2D>();
                if (colliderd != null)
                {
                    UpdateWidgetCollider(colliderd, considerInactive);
                }
                else
                {
                    UICamera camera = UICamera.FindCameraForLayer(go.layer);
                    if ((camera != null) && ((camera.eventType == UICamera.EventType.World_2D) || (camera.eventType == UICamera.EventType.UI_2D)))
                    {
                        colliderd = go.AddComponent<BoxCollider2D>();
                        colliderd.isTrigger = true;
                        UIWidget widget = go.GetComponent<UIWidget>();
                        if (widget != null)
                        {
                            widget.autoResizeBoxCollider = true;
                        }
                        UpdateWidgetCollider(colliderd, considerInactive);
                    }
                    else
                    {
                        box = go.AddComponent<BoxCollider>();
                        box.isTrigger = true;
                        UIWidget widget2 = go.GetComponent<UIWidget>();
                        if (widget2 != null)
                        {
                            widget2.autoResizeBoxCollider = true;
                        }
                        UpdateWidgetCollider(box, considerInactive);
                    }
                }
            }
        }
    }

    public static int AdjustDepth(GameObject go, int adjustment)
    {
        if (go == null)
        {
            return 0;
        }
        if (go.GetComponent<UIPanel>() != null)
        {
            foreach (UIPanel panel2 in go.GetComponentsInChildren<UIPanel>(true))
            {
                panel2.depth += adjustment;
            }
            return 1;
        }
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>(true);
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            UIWidget widget = componentsInChildren[index];
            widget.depth += adjustment;
            index++;
        }
        return 2;
    }

    public static Color ApplyPMA(Color c)
    {
        if (c.a != 1f)
        {
            c.r *= c.a;
            c.g *= c.a;
            c.b *= c.a;
        }
        return c;
    }

    public static void BringForward(GameObject go)
    {
        switch (AdjustDepth(go, 0x3e8))
        {
            case 1:
                NormalizePanelDepths();
                break;

            case 2:
                NormalizeWidgetDepths();
                break;
        }
    }

    public static void Broadcast(string funcName)
    {
        GameObject[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int index = 0;
        int length = objArray.Length;
        while (index < length)
        {
            objArray[index].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
            index++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        GameObject[] objArray = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int index = 0;
        int length = objArray.Length;
        while (index < length)
        {
            objArray[index].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
            index++;
        }
    }

    public static int CalculateNextDepth(GameObject go)
    {
        int a = -1;
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            a = Mathf.Max(a, componentsInChildren[index].depth);
            index++;
        }
        return (a + 1);
    }

    public static int CalculateNextDepth(GameObject go, bool ignoreChildrenWithColliders)
    {
        if (!ignoreChildrenWithColliders)
        {
            return CalculateNextDepth(go);
        }
        int a = -1;
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            UIWidget widget = componentsInChildren[index];
            if ((widget.cachedGameObject == go) || ((widget.collider == null) && (widget.GetComponent<Collider2D>() == null)))
            {
                a = Mathf.Max(a, widget.depth);
            }
            index++;
        }
        return (a + 1);
    }

    public static int CalculateRaycastDepth(GameObject go)
    {
        UIPanel component = go.GetComponent<UIPanel>();
        if (component != null)
        {
            return (component.depth * 0x3e8);
        }
        UIWidget widget = go.GetComponent<UIWidget>();
        if (widget != null)
        {
            return widget.raycastDepth;
        }
        UIDepthOffset offset = go.GetComponent<UIDepthOffset>();
        if (offset != null)
        {
            return offset.raycastDepth;
        }
        UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
        if (componentsInChildren.Length == 0)
        {
            return 0;
        }
        int a = 0x7fffffff;
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            if (componentsInChildren[index].enabled)
            {
                a = Mathf.Min(a, componentsInChildren[index].raycastDepth);
            }
            index++;
        }
        return a;
    }

    [DebuggerStepThrough, DebuggerHidden]
    private static void CallCreatePanel(Transform t)
    {
        UIWidget component = t.GetComponent<UIWidget>();
        if (component != null)
        {
            component.CreatePanel();
        }
        int index = 0;
        int childCount = t.childCount;
        while (index < childCount)
        {
            CallCreatePanel(t.GetChild(index));
            index++;
        }
    }

    public static UIPanel CreateUI(bool advanced3D)
    {
        return CreateUI(null, advanced3D, -1);
    }

    public static UIPanel CreateUI(bool advanced3D, int layer)
    {
        return CreateUI(null, advanced3D, layer);
    }

    public static UIPanel CreateUI(Transform trans, bool advanced3D, int layer)
    {
        UIRoot root = (trans == null) ? null : FindInParents<UIRoot>(trans.gameObject);
        if ((root == null) && (UIRoot.list.Count > 0))
        {
            root = UIRoot.list[0];
        }
        if (root == null)
        {
            GameObject obj2 = AddChild(null, false);
            root = obj2.AddComponent<UIRoot>();
            if (layer == -1)
            {
                layer = LayerMask.NameToLayer("UI");
            }
            if (layer == -1)
            {
                layer = LayerMask.NameToLayer("2D UI");
            }
            obj2.layer = layer;
            if (advanced3D)
            {
                obj2.name = "UI Root (3D)";
                root.scalingStyle = UIRoot.Scaling.FixedSize;
            }
            else
            {
                obj2.name = "UI Root";
                root.scalingStyle = UIRoot.Scaling.PixelPerfect;
            }
        }
        UIPanel componentInChildren = root.GetComponentInChildren<UIPanel>();
        if (componentInChildren == null)
        {
            Camera[] cameraArray = FindActive<Camera>();
            float a = -1f;
            bool flag = false;
            int num2 = ((int) 1) << root.gameObject.layer;
            for (int i = 0; i < cameraArray.Length; i++)
            {
                Camera camera = cameraArray[i];
                if ((camera.clearFlags == CameraClearFlags.Color) || (camera.clearFlags == CameraClearFlags.Skybox))
                {
                    flag = true;
                }
                a = Mathf.Max(a, camera.depth);
                camera.cullingMask &= ~num2;
            }
            Camera camera2 = AddChild<Camera>(root.gameObject, false);
            camera2.gameObject.AddComponent<UICamera>();
            camera2.clearFlags = !flag ? CameraClearFlags.Color : CameraClearFlags.Depth;
            camera2.backgroundColor = Color.grey;
            camera2.cullingMask = num2;
            camera2.depth = a + 1f;
            if (advanced3D)
            {
                camera2.nearClipPlane = 0.1f;
                camera2.farClipPlane = 4f;
                camera2.transform.localPosition = new Vector3(0f, 0f, -700f);
            }
            else
            {
                camera2.orthographic = true;
                camera2.orthographicSize = 1f;
                camera2.nearClipPlane = -10f;
                camera2.farClipPlane = 10f;
            }
            AudioListener[] listenerArray = FindActive<AudioListener>();
            if ((listenerArray == null) || (listenerArray.Length == 0))
            {
                camera2.gameObject.AddComponent<AudioListener>();
            }
            componentInChildren = root.gameObject.AddComponent<UIPanel>();
        }
        if (trans != null)
        {
            while (trans.parent != null)
            {
                trans = trans.parent;
            }
            if (IsChild(trans, componentInChildren.transform))
            {
                return trans.gameObject.AddComponent<UIPanel>();
            }
            trans.parent = componentInChildren.transform;
            trans.localScale = Vector3.one;
            trans.localPosition = Vector3.zero;
            SetChildLayer(componentInChildren.cachedTransform, componentInChildren.cachedGameObject.layer);
        }
        return componentInChildren;
    }

    private static void Deactivate(Transform t)
    {
        SetActiveSelf(t.gameObject, false);
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject obj2 = obj as GameObject;
                    obj2.transform.parent = null;
                }
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
    }

    public static void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }

    [Obsolete("Use NGUIText.EncodeColor instead")]
    public static string EncodeColor(Color c)
    {
        return NGUIText.EncodeColor24(c);
    }

    public static void Execute<T>(GameObject go, string funcName) where T: Component
    {
        foreach (T local in go.GetComponents<T>())
        {
            MethodInfo method = local.GetType().GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(local, null);
            }
        }
    }

    public static void ExecuteAll<T>(GameObject root, string funcName) where T: Component
    {
        Execute<T>(root, funcName);
        Transform transform = root.transform;
        int index = 0;
        int childCount = transform.childCount;
        while (index < childCount)
        {
            ExecuteAll<T>(transform.GetChild(index).gameObject, funcName);
            index++;
        }
    }

    public static T[] FindActive<T>() where T: Component
    {
        return (UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[]);
    }

    public static Camera FindCameraForLayer(int layer)
    {
        Camera main;
        int num = ((int) 1) << layer;
        for (int i = 0; i < UICamera.list.size; i++)
        {
            main = UICamera.list.buffer[i].cachedCamera;
            if ((main != null) && ((main.cullingMask & num) != 0))
            {
                return main;
            }
        }
        main = Camera.main;
        if ((main != null) && ((main.cullingMask & num) != 0))
        {
            return main;
        }
        Camera[] cameraArray = FindActive<Camera>();
        int index = 0;
        int length = cameraArray.Length;
        while (index < length)
        {
            main = cameraArray[index];
            if ((main != null) && ((main.cullingMask & num) != 0))
            {
                return main;
            }
            index++;
        }
        return null;
    }

    public static T FindInParents<T>(GameObject go) where T: Component
    {
        if (go == null)
        {
            return null;
        }
        T component = go.GetComponent<T>();
        if (component == null)
        {
            for (Transform transform = go.transform.parent; (transform != null) && (component == null); transform = transform.parent)
            {
                component = transform.gameObject.GetComponent<T>();
            }
        }
        return component;
    }

    public static T FindInParents<T>(Transform trans) where T: Component
    {
        if (trans == null)
        {
            return null;
        }
        T component = trans.GetComponent<T>();
        if (component == null)
        {
            for (Transform transform = trans.transform.parent; (transform != null) && (component == null); transform = transform.parent)
            {
                component = transform.gameObject.GetComponent<T>();
            }
        }
        return component;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static bool GetActive(Behaviour mb)
    {
        return (((mb != null) && mb.enabled) && mb.gameObject.activeInHierarchy);
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static bool GetActive(GameObject go)
    {
        return ((go != null) && go.activeInHierarchy);
    }

    public static string GetFuncName(object obj, string method)
    {
        if (obj == null)
        {
            return "<null>";
        }
        string str = obj.GetType().ToString();
        int num = str.LastIndexOf('/');
        if (num > 0)
        {
            str = str.Substring(num + 1);
        }
        return (!string.IsNullOrEmpty(method) ? (str + "/" + method) : str);
    }

    public static string GetHierarchy(GameObject obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }
        string name = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            name = obj.name + @"\" + name;
        }
        return name;
    }

    public static GameObject GetRoot(GameObject go)
    {
        Transform transform = go.transform;
        while (true)
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                return transform.gameObject;
            }
            transform = parent;
        }
    }

    public static Vector3[] GetSides(this Camera cam)
    {
        return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
    }

    public static Vector3[] GetSides(this Camera cam, float depth)
    {
        return cam.GetSides(depth, null);
    }

    public static Vector3[] GetSides(this Camera cam, Transform relativeTo)
    {
        return cam.GetSides(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
    }

    public static Vector3[] GetSides(this Camera cam, float depth, Transform relativeTo)
    {
        mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth));
        mSides[1] = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, depth));
        mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth));
        mSides[3] = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, depth));
        if (relativeTo != null)
        {
            for (int i = 0; i < 4; i++)
            {
                mSides[i] = relativeTo.InverseTransformPoint(mSides[i]);
            }
        }
        return mSides;
    }

    public static string GetTypeName<T>()
    {
        string str = typeof(T).ToString();
        if (str.StartsWith("UI"))
        {
            return str.Substring(2);
        }
        if (str.StartsWith("UnityEngine."))
        {
            str = str.Substring(12);
        }
        return str;
    }

    public static string GetTypeName(UnityEngine.Object obj)
    {
        if (obj == null)
        {
            return "Null";
        }
        string str = obj.GetType().ToString();
        if (str.StartsWith("UI"))
        {
            return str.Substring(2);
        }
        if (str.StartsWith("UnityEngine."))
        {
            str = str.Substring(12);
        }
        return str;
    }

    public static Vector3[] GetWorldCorners(this Camera cam)
    {
        return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), null);
    }

    public static Vector3[] GetWorldCorners(this Camera cam, float depth)
    {
        return cam.GetWorldCorners(depth, null);
    }

    public static Vector3[] GetWorldCorners(this Camera cam, Transform relativeTo)
    {
        return cam.GetWorldCorners(Mathf.Lerp(cam.nearClipPlane, cam.farClipPlane, 0.5f), relativeTo);
    }

    public static Vector3[] GetWorldCorners(this Camera cam, float depth, Transform relativeTo)
    {
        mSides[0] = cam.ViewportToWorldPoint(new Vector3(0f, 0f, depth));
        mSides[1] = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depth));
        mSides[2] = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depth));
        mSides[3] = cam.ViewportToWorldPoint(new Vector3(1f, 0f, depth));
        if (relativeTo != null)
        {
            for (int i = 0; i < 4; i++)
            {
                mSides[i] = relativeTo.InverseTransformPoint(mSides[i]);
            }
        }
        return mSides;
    }

    public static void ImmediatelyCreateDrawCalls(GameObject root)
    {
        ExecuteAll<UIWidget>(root, "Start");
        ExecuteAll<UIPanel>(root, "Start");
        ExecuteAll<UIWidget>(root, "Update");
        ExecuteAll<UIPanel>(root, "Update");
        ExecuteAll<UIPanel>(root, "LateUpdate");
    }

    [Obsolete("Use NGUITools.GetActive instead")]
    public static bool IsActive(Behaviour mb)
    {
        return (((mb != null) && mb.enabled) && mb.gameObject.activeInHierarchy);
    }

    public static bool IsChild(Transform parent, Transform child)
    {
        if ((parent != null) && (child != null))
        {
            while (child != null)
            {
                if (child == parent)
                {
                    return true;
                }
                child = child.parent;
            }
        }
        return false;
    }

    public static byte[] Load(string fileName)
    {
        if (fileAccess)
        {
            string path = Application.persistentDataPath + "/" + fileName;
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
        }
        return null;
    }

    public static void MakePixelPerfect(Transform t)
    {
        UIWidget component = t.GetComponent<UIWidget>();
        if (component != null)
        {
            component.MakePixelPerfect();
        }
        if ((t.GetComponent<UIAnchor>() == null) && (t.GetComponent<UIRoot>() == null))
        {
            t.localPosition = Round(t.localPosition);
            t.localScale = Round(t.localScale);
        }
        int index = 0;
        int childCount = t.childCount;
        while (index < childCount)
        {
            MakePixelPerfect(t.GetChild(index));
            index++;
        }
    }

    public static void MarkParentAsChanged(GameObject go)
    {
        UIRect[] componentsInChildren = go.GetComponentsInChildren<UIRect>();
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            componentsInChildren[index].ParentHasChanged();
            index++;
        }
    }

    public static void NormalizeDepths()
    {
        NormalizeWidgetDepths();
        NormalizePanelDepths();
    }

    public static void NormalizePanelDepths()
    {
        UIPanel[] array = FindActive<UIPanel>();
        int length = array.Length;
        if (length > 0)
        {
            Array.Sort<UIPanel>(array, new Comparison<UIPanel>(UIPanel.CompareFunc));
            int num2 = 0;
            int depth = array[0].depth;
            for (int i = 0; i < length; i++)
            {
                UIPanel panel = array[i];
                if (panel.depth == depth)
                {
                    panel.depth = num2;
                }
                else
                {
                    depth = panel.depth;
                    panel.depth = ++num2;
                }
            }
        }
    }

    public static void NormalizeWidgetDepths()
    {
        UIWidget[] array = FindActive<UIWidget>();
        int length = array.Length;
        if (length > 0)
        {
            Array.Sort<UIWidget>(array, new Comparison<UIWidget>(UIWidget.FullCompareFunc));
            int num2 = 0;
            int depth = array[0].depth;
            for (int i = 0; i < length; i++)
            {
                UIWidget widget = array[i];
                if (widget.depth == depth)
                {
                    widget.depth = num2;
                }
                else
                {
                    depth = widget.depth;
                    widget.depth = ++num2;
                }
            }
        }
    }

    public static WWW OpenURL(string url)
    {
        WWW www = null;
        try
        {
            www = new WWW(url);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
        return www;
    }

    public static WWW OpenURL(string url, WWWForm form)
    {
        if (form == null)
        {
            return OpenURL(url);
        }
        WWW www = null;
        try
        {
            www = new WWW(url, form);
        }
        catch (Exception exception)
        {
            Debug.LogError((exception == null) ? "<null>" : exception.Message);
        }
        return www;
    }

    [Obsolete("Use NGUIText.ParseColor instead")]
    public static Color ParseColor(string text, int offset)
    {
        return NGUIText.ParseColor24(text, offset);
    }

    public static AudioSource PlaySound(AudioClip clip)
    {
        return PlaySound(clip, 1f, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume)
    {
        return PlaySound(clip, volume, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
    {
        if ((SettingMgr.mInstance == null) || SettingMgr.mInstance.SFX_Enable)
        {
            volume *= soundVolume;
            if ((clip != null) && (volume > 0.01f))
            {
                if ((mListener == null) || !GetActive(mListener))
                {
                    AudioListener[] listenerArray = UnityEngine.Object.FindObjectsOfType(typeof(AudioListener)) as AudioListener[];
                    if (listenerArray != null)
                    {
                        for (int i = 0; i < listenerArray.Length; i++)
                        {
                            if (GetActive(listenerArray[i]))
                            {
                                mListener = listenerArray[i];
                                break;
                            }
                        }
                    }
                    if (mListener == null)
                    {
                        Camera main = Camera.main;
                        if (main == null)
                        {
                            main = UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera;
                        }
                        if (main != null)
                        {
                            mListener = main.gameObject.AddComponent<AudioListener>();
                        }
                    }
                }
                if (((mListener != null) && mListener.enabled) && GetActive(mListener.gameObject))
                {
                    AudioSource audio = mListener.audio;
                    if (audio == null)
                    {
                        audio = mListener.gameObject.AddComponent<AudioSource>();
                    }
                    audio.pitch = pitch;
                    audio.PlayOneShot(clip, volume);
                    return audio;
                }
            }
        }
        return null;
    }

    public static void PushBack(GameObject go)
    {
        switch (AdjustDepth(go, -1000))
        {
            case 1:
                NormalizePanelDepths();
                break;

            case 2:
                NormalizeWidgetDepths();
                break;
        }
    }

    public static int RandomRange(int min, int max)
    {
        if (min == max)
        {
            return min;
        }
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static void RegisterUndo(UnityEngine.Object obj, string name)
    {
    }

    public static Vector3 Round(Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    public static bool Save(string fileName, byte[] bytes)
    {
        if (!fileAccess)
        {
            return false;
        }
        string path = Application.persistentDataPath + "/" + fileName;
        if (bytes == null)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }
        FileStream stream = null;
        try
        {
            stream = File.Create(path);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            return false;
        }
        stream.Write(bytes, 0, bytes.Length);
        stream.Close();
        return true;
    }

    public static void SetActive(GameObject go, bool state)
    {
        SetActive(go, state, true);
    }

    public static void SetActive(GameObject go, bool state, bool compatibilityMode)
    {
        if (go != null)
        {
            if (state)
            {
                Activate(go.transform, compatibilityMode);
                CallCreatePanel(go.transform);
            }
            else
            {
                Deactivate(go.transform);
            }
        }
    }

    public static void SetActiveChildren(GameObject go, bool state)
    {
        Transform transform = go.transform;
        if (state)
        {
            int index = 0;
            int childCount = transform.childCount;
            while (index < childCount)
            {
                Activate(transform.GetChild(index));
                index++;
            }
        }
        else
        {
            int num3 = 0;
            int num4 = transform.childCount;
            while (num3 < num4)
            {
                Deactivate(transform.GetChild(num3));
                num3++;
            }
        }
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    public static void SetChildLayer(Transform t, int layer)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            SetChildLayer(child, layer);
        }
    }

    public static void SetDirty(UnityEngine.Object obj)
    {
    }

    public static void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        int index = 0;
        int childCount = transform.childCount;
        while (index < childCount)
        {
            SetLayer(transform.GetChild(index).gameObject, layer);
            index++;
        }
    }

    [Obsolete("Use NGUIText.StripSymbols instead")]
    public static string StripSymbols(string text)
    {
        return NGUIText.StripSymbols(text);
    }

    public static void UpdateWidgetCollider(GameObject go)
    {
        UpdateWidgetCollider(go, false);
    }

    public static void UpdateWidgetCollider(BoxCollider box, bool considerInactive)
    {
        if (box != null)
        {
            GameObject gameObject = box.gameObject;
            UIWidget component = gameObject.GetComponent<UIWidget>();
            if (component != null)
            {
                Vector3[] localCorners = component.localCorners;
                box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
                box.size = localCorners[2] - localCorners[0];
            }
            else
            {
                Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
                box.center = bounds.center;
                box.size = new Vector3(bounds.size.x, bounds.size.y, 0f);
            }
        }
    }

    public static void UpdateWidgetCollider(BoxCollider2D box, bool considerInactive)
    {
        if (box != null)
        {
            GameObject gameObject = box.gameObject;
            UIWidget component = gameObject.GetComponent<UIWidget>();
            if (component != null)
            {
                Vector3[] localCorners = component.localCorners;
                box.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
                box.size = localCorners[2] - localCorners[0];
            }
            else
            {
                Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(gameObject.transform, considerInactive);
                box.center = bounds.center;
                box.size = new Vector2(bounds.size.x, bounds.size.y);
            }
        }
    }

    public static void UpdateWidgetCollider(GameObject go, bool considerInactive)
    {
        if (go != null)
        {
            BoxCollider component = go.GetComponent<BoxCollider>();
            if (component != null)
            {
                UpdateWidgetCollider(component, considerInactive);
            }
            else
            {
                BoxCollider2D box = go.GetComponent<BoxCollider2D>();
                if (box != null)
                {
                    UpdateWidgetCollider(box, considerInactive);
                }
            }
        }
    }

    public static string clipboard
    {
        get
        {
            TextEditor editor = new TextEditor();
            editor.Paste();
            return editor.content.text;
        }
        set
        {
            TextEditor editor = new TextEditor {
                content = new GUIContent(value)
            };
            editor.OnFocus();
            editor.Copy();
        }
    }

    public static bool fileAccess
    {
        get
        {
            return ((Application.platform != RuntimePlatform.WindowsWebPlayer) && (Application.platform != RuntimePlatform.OSXWebPlayer));
        }
    }

    public static float soundVolume
    {
        get
        {
            if (!mLoaded)
            {
                mLoaded = true;
                mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
            }
            return mGlobalVolume;
        }
        set
        {
            if (mGlobalVolume != value)
            {
                mLoaded = true;
                mGlobalVolume = value;
                PlayerPrefs.SetFloat("Sound", value);
            }
        }
    }
}

