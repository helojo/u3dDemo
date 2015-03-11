using Holoville.HOTween;
using LevelLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class GUIMgr : MonoBehaviour
{
    [CompilerGenerated]
    private static Predicate<GUIPersistence> <>f__am$cache18;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache19;
    [CompilerGenerated]
    private static Func<System.Type, string> <>f__am$cache1A;
    [CompilerGenerated]
    private static Predicate<GUIPersistence> <>f__am$cache1B;
    [CompilerGenerated]
    private static Predicate<GUIPersistence> <>f__am$cache1C;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1D;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cache1E;
    private Color fi_color = new Color(0f, 0f, 0f, 1f);
    private Color fo_color = new Color(0f, 0f, 0f, 0.01f);
    private static Action<string> GetCopyBufferString_CallBack;
    private int idPool;
    private static GUIMgr instance;
    public static bool IsQuitMsgBoxShow;
    private List<GUIPersistence> lstGUIPersistences = new List<GUIPersistence>();
    private int maxDepth = 50;
    private GUINavigationController navController = new GUINavigationController();
    private Camera othCamera;
    private Camera othListCamera;
    private int rcBlur;
    private Camera tdCamera;
    private float termLock;
    private UITexture texPostEffectBack;
    private UITexture texPostEffectFront;
    private Texture2D texWhite1x1;
    private UICamera uiCamera;
    private UIDraggableCamera uiDraggableCamera;
    private UIRoot uiDummyRoot;
    private UICamera uiListCamera;
    private UIRoot uiListRoot;
    private UIRoot uiRoot;
    private UIViewport uiViewport;

    private void Awake()
    {
        instance = this;
        Holoville.HOTween.HOTween.Init(false, false, true);
        Holoville.HOTween.HOTween.EnableOverwriteManager(true);
        UnityEngine.Object.DontDestroyOnLoad(this.ListRoot.gameObject);
    }

    public float CalculateListViewportScrolling(Transform lt, Transform rb)
    {
        Vector3 localPosition = this.othListCamera.transform.localPosition;
        float y = lt.localPosition.y;
        float num2 = rb.localPosition.y;
        return (localPosition.y - (0.5f * (y + num2)));
    }

    public void CheckMultiCameraStatus()
    {
        GUIEntity entity = this.Topest();
        this.ListRoot.gameObject.SetActive((null != entity) && entity.multiCamera);
    }

    private void CheckQuit()
    {
        if ((!IsQuitMsgBoxShow && (Application.platform == RuntimePlatform.Android)) && (Input.GetKeyDown(KeyCode.Escape) && (null == instance.GetActivityGUIEntity("AdPicutrePanel"))))
        {
            if (GameDefine.getInstance().isUpdateMsgBoxShow)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x186b1));
            }
            else
            {
                IsQuitMsgBoxShow = true;
                if (<>f__am$cache19 == null)
                {
                    <>f__am$cache19 = delegate (GUIEntity obj) {
                        MessageBox box = (MessageBox) obj;
                        if (<>f__am$cache1D == null)
                        {
                            <>f__am$cache1D = delegate (GameObject box) {
                                IsQuitMsgBoxShow = false;
                                Application.Quit();
                            };
                        }
                        if (<>f__am$cache1E == null)
                        {
                            <>f__am$cache1E = box => IsQuitMsgBoxShow = false;
                        }
                        box.SetDialog(ConfigMgr.getInstance().GetWord(0x1c), <>f__am$cache1D, <>f__am$cache1E, false);
                        box.Depth = 0x514;
                    };
                }
                Instance.DoModelGUI("MessageBox", <>f__am$cache19, null);
            }
        }
    }

    public void ClearAll()
    {
        this.tdCamera = null;
        this.navController.Clear();
        foreach (GUIPersistence persistence in this.lstGUIPersistences)
        {
            if (null != persistence.entity)
            {
                persistence.entity.OnRelease();
                persistence.entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_RELEASE);
                UnityEngine.Object.Destroy(persistence.entity.gameObject);
            }
        }
        this.lstGUIPersistences.Clear();
        this.FrontPostEffectPanel.gameObject.SetActive(false);
        this.BackPostEffectPanel.gameObject.SetActive(false);
        this.rcBlur = 0;
    }

    public void ClearExceptMainUI()
    {
        this.Camera3D.enabled = true;
        this.navController.Clear();
        GUIPersistence item = null;
        GUIPersistence persistence2 = null;
        foreach (GUIPersistence persistence3 in this.lstGUIPersistences)
        {
            if ("MainUI" == persistence3.name)
            {
                item = persistence3;
            }
            else if ("TitleBar" == persistence3.name)
            {
                persistence2 = persistence3;
            }
            else if (null != persistence3.entity)
            {
                persistence3.entity.OnRelease();
                persistence3.entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_RELEASE);
                UnityEngine.Object.Destroy(persistence3.entity.gameObject);
            }
        }
        this.lstGUIPersistences.Clear();
        this.lstGUIPersistences.Add(item);
        this.lstGUIPersistences.Add(persistence2);
        this.FrontPostEffectPanel.gameObject.SetActive(false);
        this.BackPostEffectPanel.gameObject.SetActive(false);
        this.rcBlur = 0;
        TitleBar instance = TitleBar.Instance;
        if (null != instance)
        {
            instance.ShowTitleBar(true);
            instance.HideFunBar();
        }
    }

    private void CloseGUIEntity(GUIPersistence pers)
    {
        this.lstGUIPersistences.Remove(pers);
        if (null != pers.entity)
        {
            pers.entity.OnRelease();
            pers.entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_RELEASE);
            UnityEngine.Object.Destroy(pers.entity.gameObject);
            pers.entity = null;
        }
    }

    public void CloseGUIEntity(int id)
    {
        <CloseGUIEntity>c__AnonStorey1F5 storeyf = new <CloseGUIEntity>c__AnonStorey1F5 {
            id = id
        };
        GUIPersistence pers = this.ReserveFindPersistences(new Predicate<GUIPersistence>(storeyf.<>m__332));
        if (pers != null)
        {
            this.CloseGUIEntity(pers);
        }
    }

    public void CloseUniqueGUIEntity(GUIEntity entity)
    {
        <CloseUniqueGUIEntity>c__AnonStorey1F1 storeyf = new <CloseUniqueGUIEntity>c__AnonStorey1F1 {
            entity = entity,
            <>f__this = this
        };
        storeyf.entity.BlendAnimations(new System.Action(storeyf.<>m__32D), true, 1f);
    }

    public void CloseUniqueGUIEntity(string name)
    {
        GUIEntity gUIEntity = this.GetGUIEntity(name);
        if (null != gUIEntity)
        {
            this.CloseUniqueGUIEntity(gUIEntity);
        }
    }

    public void CloseUniqueGUIEntityImmediate(string name)
    {
        <CloseUniqueGUIEntityImmediate>c__AnonStorey1F0 storeyf = new <CloseUniqueGUIEntityImmediate>c__AnonStorey1F0 {
            <>f__this = this,
            entity = this.GetGUIEntity(name)
        };
        if (null != storeyf.entity)
        {
            storeyf.entity.Serialization(this.GetGUIPersistences(storeyf.entity.entity_id));
            storeyf.entity.BlendAnimations(new System.Action(storeyf.<>m__32C), true, 1f);
        }
    }

    private UITexture CreateEffectRT(string name)
    {
        GameObject obj2 = new GameObject();
        obj2.AddComponent<UIPanel>();
        obj2.transform.parent = this.ActivityCamera.transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.transform.localScale = Vector3.one;
        obj2.name = name;
        obj2.layer = LayerMask.NameToLayer("GUI");
        Quaternion quaternion = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        GameObject obj3 = new GameObject();
        UITexture texture = obj3.AddComponent<UITexture>();
        obj3.transform.parent = obj2.transform;
        obj3.transform.position = Vector3.zero;
        obj3.transform.rotation = quaternion;
        obj3.transform.localScale = Vector3.one;
        obj3.layer = LayerMask.NameToLayer("GUI");
        texture.depth = -1;
        texture.type = UIBasicSprite.Type.Sliced;
        obj3.name = "Texture";
        UIStretch stretch = obj3.AddComponent<UIStretch>();
        stretch.style = UIStretch.Style.Both;
        stretch.runOnlyOnce = false;
        return texture;
    }

    public int CreateGUIEntity(string name, bool uniqueness, Action<GUIEntity> actCompletd)
    {
        <CreateGUIEntity>c__AnonStorey1F2 storeyf;
        storeyf = new <CreateGUIEntity>c__AnonStorey1F2 {
            name = name,
            uniqueness = uniqueness,
            actCompletd = actCompletd,
            <>f__this = this,
            entity = null,
            id = this.idPool++,
            actManaged = new System.Action(storeyf.<>m__32E)
        };
        for (int i = this.lstGUIPersistences.Count - 1; i >= 0; i--)
        {
            GUIPersistence persistence = this.lstGUIPersistences[i];
            if (((persistence != null) && (storeyf.name == persistence.name)) && ((null != persistence.entity) && persistence.entity.Hidden))
            {
                if (storeyf.uniqueness && persistence.uniqueness)
                {
                    persistence.entity.ResetAnimations();
                    persistence.entity.Hidden = false;
                    if (storeyf.actCompletd != null)
                    {
                        storeyf.actCompletd(persistence.entity);
                    }
                    return persistence.id;
                }
                storeyf.entity = persistence.entity;
                storeyf.entity.ResetAnimations();
                storeyf.entity.Hidden = false;
                persistence.entity = null;
                break;
            }
        }
        if (null == storeyf.entity)
        {
            base.StartCoroutine(this.SynLoadGUILayout(storeyf.name, new Action<GUIEntity>(storeyf.<>m__32F)));
        }
        else
        {
            storeyf.actManaged();
        }
        return storeyf.id;
    }

    public void DockTitleBar()
    {
        TitleBar activityGUIEntity = instance.GetActivityGUIEntity<TitleBar>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.Depth = 3;
        }
    }

    public void DoModelGUI<T>(Action<GUIEntity> actCompleted = null, GameObject dock = null) where T: GUIEntity
    {
        this.DoModelGUI(typeof(T).Name, actCompleted, dock);
    }

    [Obsolete("use DoModelGUI<T> i")]
    public void DoModelGUI(string name, Action<GUIEntity> actCompleted = null, GameObject dock = null)
    {
        <DoModelGUI>c__AnonStorey1EC storeyec = new <DoModelGUI>c__AnonStorey1EC {
            actCompleted = actCompleted,
            dock = dock,
            <>f__this = this
        };
        this.Lock();
        storeyec.depth = Mathf.Max(this.FloatingDepth, CommonFunc.GetTitleBarAbsoluteDepth());
        this.CreateGUIEntity(name, false, new Action<GUIEntity>(storeyec.<>m__328));
    }

    public GUIEntity EnchainyTopEntity()
    {
        return this.navController.TopestEntity();
    }

    public void ExitModelGUI<T>() where T: GUIEntity
    {
        this.ExitModelGUI(typeof(T).Name);
    }

    public void ExitModelGUI(GUIEntity entity)
    {
        <ExitModelGUI>c__AnonStorey1EE storeyee = new <ExitModelGUI>c__AnonStorey1EE {
            entity = entity,
            <>f__this = this
        };
        this.Lock();
        if (storeyee.entity.FadeMask)
        {
            this.FadeOutMaskBoard(0f, storeyee.entity.AnimationsDuration * 0.75f, UITweener.Method.EaseOut, null);
            if (this.rcBlur >= 1)
            {
                List<GameObject> nudities = new List<GameObject>();
                GUIEntity entity2 = this.FindNudities(ref nudities, new Predicate<GUIPersistence>(storeyee.<>m__329));
                nudities.Add(this.FrontPostEffectPanel.gameObject);
                nudities.Add(this.BackPostEffectPanel.gameObject);
                if (null != entity2)
                {
                    this.FadeMaskBoardToFront(nudities, entity2.Depth, 0f, 0.35f, UITweener.Method.Linear, null);
                }
            }
        }
        if (null != storeyee.entity)
        {
            storeyee.entity.Serialization(this.GetGUIPersistences(storeyee.entity.entity_id));
            storeyee.entity.BlendAnimations(new System.Action(storeyee.<>m__32A), true, 1f);
        }
    }

    [Obsolete("use ExitModelGUI<T> i")]
    public void ExitModelGUI(string name)
    {
        GUIEntity gUIEntity = this.GetGUIEntity(name);
        if (null != gUIEntity)
        {
            this.ExitModelGUI(gUIEntity);
        }
    }

    public void ExitModelGUIImmediate(GUIEntity entity)
    {
        if (null != entity)
        {
            if (entity.FadeMask)
            {
                this.rcBlur--;
            }
            entity.Serialization(this.GetGUIPersistences(entity.entity_id));
            if (entity.actClose != null)
            {
                entity.actClose();
            }
            entity.actClose = null;
            this.CloseGUIEntity(entity.entity_id);
        }
    }

    public void ExitModelGUIImmediate(string name)
    {
        GUIEntity gUIEntity = this.GetGUIEntity(name);
        if (null != gUIEntity)
        {
            this.ExitModelGUIImmediate(gUIEntity);
        }
    }

    public void FadeInMaskBoard(List<GameObject> nudity, int depth, float delay, float duration, UITweener.Method method, System.Action actCompleted = null)
    {
    }

    public void FadeMaskBoardToFront(List<GameObject> nudity, int depth, float delay, float duration, UITweener.Method method, System.Action actCompleted = null)
    {
    }

    public void FadeOutMaskBoard(float delay, float duration, UITweener.Method method, System.Action actCompleted = null)
    {
    }

    public void FetchGUIEntity(GUIPersistence _pers)
    {
        if (null == _pers.entity)
        {
            for (int i = this.lstGUIPersistences.Count - 1; i >= 0; i--)
            {
                GUIPersistence persistence = this.lstGUIPersistences[i];
                if (((persistence != null) && ((persistence.id != _pers.id) && (_pers.name == persistence.name))) && (!persistence.uniqueness && ((null != persistence.entity) && persistence.entity.Hidden)))
                {
                    _pers.entity = persistence.entity;
                    _pers.entity.entity_id = _pers.id;
                    persistence.entity = null;
                    break;
                }
            }
        }
    }

    private GUIEntity FindNudities(ref List<GameObject> nudities, Predicate<GUIPersistence> match)
    {
        for (int i = this.lstGUIPersistences.Count - 1; i >= 0; i--)
        {
            GUIPersistence persistence = this.lstGUIPersistences[i];
            if (((persistence != null) && (null != persistence.entity)) && !persistence.entity.Hidden)
            {
                nudities.Add(persistence.entity.gameObject);
                if (((match == null) || match(persistence)) && persistence.entity.FadeMask)
                {
                    return persistence.entity;
                }
            }
        }
        return null;
    }

    public void FloatTitleBar()
    {
        TitleBar activityGUIEntity = instance.GetActivityGUIEntity<TitleBar>();
        if (null != activityGUIEntity)
        {
            activityGUIEntity.Depth = 200;
        }
    }

    public T GetActivityGUIEntity<T>() where T: GUIEntity
    {
        T gUIEntity = this.GetGUIEntity<T>();
        if (null == gUIEntity)
        {
            return null;
        }
        return (!gUIEntity.gameObject.activeSelf ? null : gUIEntity);
    }

    public GUIEntity GetActivityGUIEntity(string name)
    {
        GUIEntity gUIEntity = this.GetGUIEntity(name);
        if (null == gUIEntity)
        {
            return null;
        }
        return (!gUIEntity.gameObject.activeSelf ? null : gUIEntity);
    }

    public static string GetCopyBufferString(Action<string> callBack)
    {
        GetCopyBufferString_CallBack = callBack;
        return string.Empty;
    }

    public T GetGUIEntity<T>() where T: GUIEntity
    {
        GUIEntity gUIEntity = this.GetGUIEntity(typeof(T).Name);
        if (null == gUIEntity)
        {
            return null;
        }
        return gUIEntity.Achieve<T>();
    }

    public GUIEntity GetGUIEntity(int id)
    {
        GUIPersistence gUIPersistences = this.GetGUIPersistences(id);
        return ((gUIPersistences != null) ? gUIPersistences.entity : null);
    }

    public GUIEntity GetGUIEntity(string name)
    {
        GUIPersistence gUIPersistences = this.GetGUIPersistences(name);
        return ((gUIPersistences != null) ? gUIPersistences.entity : null);
    }

    public GUIPersistence GetGUIPersistences(int id)
    {
        <GetGUIPersistences>c__AnonStorey1F3 storeyf = new <GetGUIPersistences>c__AnonStorey1F3 {
            id = id
        };
        return this.ReserveFindPersistences(new Predicate<GUIPersistence>(storeyf.<>m__330));
    }

    public GUIPersistence GetGUIPersistences(string name)
    {
        <GetGUIPersistences>c__AnonStorey1F4 storeyf = new <GetGUIPersistences>c__AnonStorey1F4 {
            name = name
        };
        return this.ReserveFindPersistences(new Predicate<GUIPersistence>(storeyf.<>m__331));
    }

    public bool IsTopest(GUIEntity entity)
    {
        if (null == entity)
        {
            return false;
        }
        if (<>f__am$cache1C == null)
        {
            <>f__am$cache1C = e => ((null != e.entity) && !e.entity.Hidden) && (e.entity.Depth < 200);
        }
        GUIPersistence persistence = this.ReserveFindPersistences(<>f__am$cache1C);
        if (persistence == null)
        {
            return false;
        }
        return (entity == persistence.entity);
    }

    public void Lock()
    {
        this.NGUICamera.Locked = true;
        this.NGUIListCamera.Locked = true;
        EasyTouch.instance.Locked = true;
        this.termLock = 0f;
    }

    public void OpenUniqueGUIEntity(string name, Action<GUIEntity> actCompleted)
    {
        <OpenUniqueGUIEntity>c__AnonStorey1EF storeyef = new <OpenUniqueGUIEntity>c__AnonStorey1EF {
            actCompleted = actCompleted,
            <>f__this = this
        };
        this.Lock();
        storeyef.depth = this.Depth;
        this.CreateGUIEntity(name, true, new Action<GUIEntity>(storeyef.<>m__32B));
    }

    public void PopGUIEntity()
    {
        this.Lock();
        this.navController.Pop(entity => this.UnLock());
    }

    public void PushGUIEntity<T>(Action<GUIEntity> actCompleted) where T: GUIEntity
    {
        this.PushGUIEntity(typeof(T).Name, actCompleted);
    }

    [Obsolete("use PushGUIEntity<T>")]
    public void PushGUIEntity(string name, Action<GUIEntity> actCompleted)
    {
        <PushGUIEntity>c__AnonStorey1EB storeyeb = new <PushGUIEntity>c__AnonStorey1EB {
            actCompleted = actCompleted,
            <>f__this = this
        };
        GUIPersistence persistence = this.navController.TopestPersistence();
        if ((persistence != null) && (name == persistence.name))
        {
            if (storeyeb.actCompleted != null)
            {
                storeyeb.actCompleted(persistence.entity);
            }
        }
        else
        {
            this.Lock();
            this.navController.Push(name, new Action<GUIEntity>(storeyeb.<>m__326));
        }
    }

    private GUIPersistence ReserveFindPersistences(Predicate<GUIPersistence> match)
    {
        for (int i = this.lstGUIPersistences.Count - 1; i >= 0; i--)
        {
            GUIPersistence persistence = this.lstGUIPersistences[i];
            if ((persistence != null) && match(persistence))
            {
                return persistence;
            }
        }
        return null;
    }

    public void ResetListViewpot(Transform top, Transform bottom, Transform bounds, float vertical_offset = 0f)
    {
        Instance.ListViewport.Reposition(top, bottom, vertical_offset);
        SpringPosition component = Instance.ListViewport.GetComponent<SpringPosition>();
        if (component != null)
        {
            component.enabled = false;
        }
        if (bounds != null)
        {
            Instance.ListDraggableCamera.rootForBounds = bounds;
        }
    }

    public static void SetCopyBufferString(string exportData)
    {
    }

    private void SwapPostEffectRT()
    {
        UITexture texPostEffectFront = this.texPostEffectFront;
        this.texPostEffectFront = this.texPostEffectBack;
        this.texPostEffectBack = texPostEffectFront;
    }

    [DebuggerHidden]
    private IEnumerator SynLoadGUILayout(string name, Action<GUIEntity> actCompleted)
    {
        return new <SynLoadGUILayout>c__Iterator7E { name = name, actCompleted = actCompleted, <$>name = name, <$>actCompleted = actCompleted, <>f__this = this };
    }

    public GUIEntity Topest()
    {
        if (<>f__am$cache1B == null)
        {
            <>f__am$cache1B = e => ((null != e.entity) && !e.entity.Hidden) && (e.entity.GetType() != typeof(NewbieEntity));
        }
        GUIPersistence persistence = this.ReserveFindPersistences(<>f__am$cache1B);
        if (persistence == null)
        {
            return null;
        }
        return persistence.entity;
    }

    public void UnLock()
    {
        if (!SocketMgr.Instance.isLockGUI && !PayMgr.IsInPurchase)
        {
            this.NGUICamera.Locked = false;
            this.NGUIListCamera.Locked = false;
            EasyTouch.instance.Locked = false;
        }
    }

    private void Update()
    {
        this.termLock += Time.deltaTime;
        if (this.termLock > 6f)
        {
            this.termLock = 0f;
            this.UnLock();
        }
        this.CheckMultiCameraStatus();
        this.CheckQuit();
    }

    public void UpdateUIData(params string[] types)
    {
        foreach (GUIPersistence persistence in this.lstGUIPersistences)
        {
            if ((persistence.entity != null) && types.Contains<string>(persistence.entity.GetType().Name))
            {
                persistence.entity.OnUpdateUIData();
            }
        }
    }

    public void UpdateUIData(params System.Type[] type)
    {
        if (<>f__am$cache1A == null)
        {
            <>f__am$cache1A = t => t.Name;
        }
        this.UpdateUIData(type.Select<System.Type, string>(<>f__am$cache1A).ToArray<string>());
    }

    public void UpDateUIData()
    {
        foreach (GUIPersistence persistence in this.lstGUIPersistences)
        {
            if (persistence.entity != null)
            {
                persistence.entity.OnUpdateUIData();
            }
        }
    }

    public void UpDateUIData<T>() where T: GUIEntity
    {
        foreach (GUIPersistence persistence in this.lstGUIPersistences)
        {
            if ((persistence.entity != null) && (persistence.entity is T))
            {
                persistence.entity.OnUpdateUIData();
            }
        }
    }

    public Camera ActivityCamera
    {
        get
        {
            if (null == this.othCamera)
            {
                this.othCamera = this.Root.transform.FindChild("Camera").GetComponent<Camera>();
            }
            return this.othCamera;
        }
    }

    public Camera ActivityListCamera
    {
        get
        {
            if (null == this.othListCamera)
            {
                this.othListCamera = this.ListRoot.transform.FindChild("Camera").GetComponent<Camera>();
            }
            return this.othListCamera;
        }
    }

    public UIPanel BackPostEffectPanel
    {
        get
        {
            return this.BackPostEffectRT.transform.parent.GetComponent<UIPanel>();
        }
    }

    public UITexture BackPostEffectRT
    {
        get
        {
            if (null == this.texPostEffectBack)
            {
                this.texPostEffectBack = this.CreateEffectRT("Back_Blur_Effect_RT");
            }
            return this.texPostEffectBack;
        }
    }

    public Camera Camera3D
    {
        get
        {
            if (null == this.tdCamera)
            {
                GameObject obj2 = GameObject.Find("Camera 3D");
                if (null != obj2)
                {
                    this.tdCamera = obj2.GetComponent<Camera>();
                }
            }
            return this.tdCamera;
        }
    }

    public bool Camera3DEnabledOfMainScene
    {
        get
        {
            if (null != this.GetActivityGUIEntity<MainUI>())
            {
                Camera camera = this.Camera3D;
                if (null != camera)
                {
                    return camera.enabled;
                }
            }
            return true;
        }
        set
        {
            if (null != this.GetActivityGUIEntity<MainUI>())
            {
                Camera camera = this.Camera3D;
                if (null != camera)
                {
                    camera.enabled = value;
                }
            }
        }
    }

    public int Depth
    {
        get
        {
            GUIPersistence persistence = this.ReserveFindPersistences(e => ((null != e.entity) && !e.entity.Hidden) && (e.entity.Depth < this.maxDepth));
            if (persistence != null)
            {
                return persistence.entity.AbsoluteDepth;
            }
            return 0;
        }
    }

    public UIRoot DummyRoot
    {
        get
        {
            if (null == this.uiDummyRoot)
            {
                this.uiDummyRoot = GameObject.Find("DummyRoot").GetComponent<UIRoot>();
            }
            return this.uiDummyRoot;
        }
    }

    public int FloatingDepth
    {
        get
        {
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = e => (null != e.entity) && !e.entity.Hidden;
            }
            GUIPersistence persistence = this.ReserveFindPersistences(<>f__am$cache18);
            if (persistence == null)
            {
                return 0;
            }
            int absoluteDepth = persistence.entity.AbsoluteDepth;
            if (absoluteDepth < this.maxDepth)
            {
                absoluteDepth += this.navController.FloatingDepth;
            }
            return absoluteDepth;
        }
    }

    public UIPanel FrontPostEffectPanel
    {
        get
        {
            return this.FrontPostEffectRT.transform.parent.GetComponent<UIPanel>();
        }
    }

    public UITexture FrontPostEffectRT
    {
        get
        {
            if (null == this.texPostEffectFront)
            {
                this.texPostEffectFront = this.CreateEffectRT("Front_Blur_Effect_RT");
            }
            return this.texPostEffectFront;
        }
    }

    public static GUIMgr Instance
    {
        get
        {
            return instance;
        }
    }

    public UIDraggableCamera ListDraggableCamera
    {
        get
        {
            if (null == this.uiDraggableCamera)
            {
                this.uiDraggableCamera = this.NGUIListCamera.GetComponent<UIDraggableCamera>();
            }
            return this.uiDraggableCamera;
        }
    }

    public UIRoot ListRoot
    {
        get
        {
            if (null == this.uiListRoot)
            {
                this.uiListRoot = GameObject.Find("ListRoot").GetComponent<UIRoot>();
            }
            return this.uiListRoot;
        }
    }

    private UIViewport ListViewport
    {
        get
        {
            if (null == this.uiViewport)
            {
                this.uiViewport = this.NGUIListCamera.GetComponent<UIViewport>();
            }
            return this.uiViewport;
        }
    }

    public UICamera NGUICamera
    {
        get
        {
            if (null == this.uiCamera)
            {
                this.uiCamera = this.ActivityCamera.GetComponent<UICamera>();
            }
            return this.uiCamera;
        }
    }

    public UICamera NGUIListCamera
    {
        get
        {
            if (null == this.uiListCamera)
            {
                this.uiListCamera = this.ActivityListCamera.GetComponent<UICamera>();
            }
            return this.uiListCamera;
        }
    }

    public UIRoot Root
    {
        get
        {
            if (null == this.uiRoot)
            {
                this.uiRoot = GameObject.Find("UI Root").GetComponent<UIRoot>();
            }
            return this.uiRoot;
        }
    }

    private Texture2D WhiteTexture
    {
        get
        {
            if (null == this.texWhite1x1)
            {
                this.texWhite1x1 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                this.texWhite1x1.SetPixel(1, 1, new Color(1f, 1f, 1f, 0.7f));
                this.texWhite1x1.Apply();
            }
            return this.texWhite1x1;
        }
    }

    [CompilerGenerated]
    private sealed class <CloseGUIEntity>c__AnonStorey1F5
    {
        internal int id;

        internal bool <>m__332(GUIPersistence e)
        {
            return (e.id == this.id);
        }
    }

    [CompilerGenerated]
    private sealed class <CloseUniqueGUIEntity>c__AnonStorey1F1
    {
        internal GUIMgr <>f__this;
        internal GUIEntity entity;

        internal void <>m__32D()
        {
            this.entity.Hidden = true;
            this.entity.Serialization(this.<>f__this.GetGUIPersistences(this.entity.entity_id));
        }
    }

    [CompilerGenerated]
    private sealed class <CloseUniqueGUIEntityImmediate>c__AnonStorey1F0
    {
        internal GUIMgr <>f__this;
        internal GUIEntity entity;

        internal void <>m__32C()
        {
            this.<>f__this.CloseGUIEntity(this.entity.entity_id);
        }
    }

    [CompilerGenerated]
    private sealed class <CreateGUIEntity>c__AnonStorey1F2
    {
        internal GUIMgr <>f__this;
        internal Action<GUIEntity> actCompletd;
        internal System.Action actManaged;
        internal GUIEntity entity;
        internal int id;
        internal string name;
        internal bool uniqueness;

        internal void <>m__32E()
        {
            GUIPersistence item = new GUIPersistence {
                name = this.name,
                id = this.id,
                uniqueness = this.uniqueness,
                entity = this.entity
            };
            this.entity.entity_id = this.id;
            this.<>f__this.lstGUIPersistences.Add(item);
            if (this.actCompletd != null)
            {
                this.actCompletd(this.entity);
            }
        }

        internal void <>m__32F(GUIEntity para_entity)
        {
            this.entity = para_entity;
            para_entity.OnPrepareAnimate();
            para_entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_PREPARE_ANIMATE);
            para_entity.OnInitialize();
            para_entity.SendFsmEvent(GUI_FSM_EVENT.ON_GUI_INITIALIZE);
            this.actManaged();
        }
    }

    [CompilerGenerated]
    private sealed class <DoModelGUI>c__AnonStorey1EC
    {
        internal GUIMgr <>f__this;
        internal Action<GUIEntity> actCompleted;
        internal int depth;
        internal GameObject dock;

        internal void <>m__328(GUIEntity entity)
        {
            <DoModelGUI>c__AnonStorey1ED storeyed = new <DoModelGUI>c__AnonStorey1ED {
                <>f__ref$492 = this,
                entity = entity
            };
            storeyed.entity.Depth = ++this.depth;
            storeyed.entity.DeSerialization(this.<>f__this.GetGUIPersistences(storeyed.entity.entity_id));
            if (this.actCompleted != null)
            {
                this.actCompleted(storeyed.entity);
            }
            if (storeyed.entity.FadeMask)
            {
                this.<>f__this.FadeInMaskBoard(null, storeyed.entity.Depth, 0f, 0.28f, UITweener.Method.Linear, null);
            }
            storeyed.entity.BlendAnimations(new System.Action(storeyed.<>m__335), false, 1f);
        }

        private sealed class <DoModelGUI>c__AnonStorey1ED
        {
            internal GUIMgr.<DoModelGUI>c__AnonStorey1EC <>f__ref$492;
            internal GUIEntity entity;

            internal void <>m__335()
            {
                if (null != this.<>f__ref$492.dock)
                {
                    this.entity.transform.parent = this.<>f__ref$492.dock.transform;
                }
                this.<>f__ref$492.<>f__this.UnLock();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ExitModelGUI>c__AnonStorey1EE
    {
        internal GUIMgr <>f__this;
        internal GUIEntity entity;

        internal bool <>m__329(GUIPersistence e)
        {
            return (e.id != this.entity.entity_id);
        }

        internal void <>m__32A()
        {
            if (this.entity.actClose != null)
            {
                this.entity.actClose();
            }
            this.entity.actClose = null;
            this.<>f__this.CloseGUIEntity(this.entity.entity_id);
            this.<>f__this.UnLock();
        }
    }

    [CompilerGenerated]
    private sealed class <GetGUIPersistences>c__AnonStorey1F3
    {
        internal int id;

        internal bool <>m__330(GUIPersistence e)
        {
            return (e.id == this.id);
        }
    }

    [CompilerGenerated]
    private sealed class <GetGUIPersistences>c__AnonStorey1F4
    {
        internal string name;

        internal bool <>m__331(GUIPersistence e)
        {
            return (e.name == this.name);
        }
    }

    [CompilerGenerated]
    private sealed class <OpenUniqueGUIEntity>c__AnonStorey1EF
    {
        internal GUIMgr <>f__this;
        internal Action<GUIEntity> actCompleted;
        internal int depth;

        internal void <>m__32B(GUIEntity entity)
        {
            entity.DeSerialization(this.<>f__this.GetGUIPersistences(entity.entity_id));
            entity.BlendAnimations(() => this.<>f__this.UnLock(), false, 1f);
            entity.Depth = ++this.depth;
            if (this.actCompleted != null)
            {
                this.actCompleted(entity);
            }
        }

        internal void <>m__336()
        {
            this.<>f__this.UnLock();
        }
    }

    [CompilerGenerated]
    private sealed class <PushGUIEntity>c__AnonStorey1EB
    {
        internal GUIMgr <>f__this;
        internal Action<GUIEntity> actCompleted;

        internal void <>m__326(GUIEntity entity)
        {
            this.<>f__this.UnLock();
            if (this.actCompleted != null)
            {
                this.actCompleted(entity);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SynLoadGUILayout>c__Iterator7E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<GUIEntity> <$>actCompleted;
        internal string <$>name;
        internal GUIMgr <>f__this;
        internal Transform <child>__4;
        internal int <count>__2;
        internal Transform <entity_tns>__1;
        internal GUIEntity <entity>__5;
        internal int <i>__3;
        internal Operation <lao>__0;
        internal Action<GUIEntity> actCompleted;
        internal string name;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<lao>__0 = BundleMgr.Instance.LoadGuiPanelAdditiveAsync(this.name);
                    if ((this.<lao>__0 != null) && (this.<lao>__0.ao != null))
                    {
                        break;
                    }
                    goto Label_01CA;

                case 1:
                    break;

                default:
                    goto Label_01CA;
            }
            while (!this.<lao>__0.IsDone())
            {
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            this.<entity_tns>__1 = null;
            this.<count>__2 = this.<>f__this.ActivityCamera.transform.childCount;
            this.<i>__3 = this.<count>__2 - 1;
            while (this.<i>__3 >= 0)
            {
                this.<child>__4 = this.<>f__this.ActivityCamera.transform.GetChild(this.<i>__3);
                if ((null != this.<child>__4) && (this.<child>__4.name == this.name))
                {
                    this.<entity_tns>__1 = this.<child>__4;
                    break;
                }
                this.<i>__3--;
            }
            if (null == this.<entity_tns>__1)
            {
                this.<lao>__0.Release();
                this.<lao>__0 = null;
            }
            else
            {
                this.<entity>__5 = this.<entity_tns>__1.GetComponent<GUIEntity>();
                if (null == this.<entity>__5)
                {
                    Debug.LogError("can not find the gui entity component");
                }
                else
                {
                    if (this.actCompleted != null)
                    {
                        this.actCompleted(this.<entity>__5);
                    }
                    this.<entity>__5.Ready();
                }
                this.<lao>__0.Release();
                this.<lao>__0 = null;
                this.$PC = -1;
            }
        Label_01CA:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    private class CompareSubEntities : IComparer<GUIEntity>
    {
        public int Compare(GUIEntity l, GUIEntity r)
        {
            return (r.Depth - l.Depth);
        }
    }
}

