using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class FakeCharacter : MonoBehaviour
{
    private static FakeCharacter _instance;
    private static float baseHeight = 100f;
    private GameObject dirLightObj;
    private Dictionary<int, FakeObject> fakeCharacters = new Dictionary<int, FakeObject>();
    public static bool isEditorMode;
    private int maxFakeCharIndex = 1;
    private static float stepHeight = 20f;
    private static float stepWidth = 20f;
    private List<WaitCreateCardInfo> waitCreateCardList = new List<WaitCreateCardInfo>();

    private void Awake()
    {
        _instance = this;
        base.StartCoroutine("CreateCardLoop");
    }

    public GameObject BindEffect2CardFeet(GameObject card_go, GameObject effectPrefab, bool isbinding)
    {
        return this.BindEffect2CardFeet(card_go, effectPrefab, isbinding, Vector3.zero, false);
    }

    public GameObject BindEffect2CardFeet(GameObject card_go, GameObject effectPrefab, bool isbinding, Vector3 offset)
    {
        return this.BindEffect2CardFeet(card_go, effectPrefab, isbinding, offset, false);
    }

    public GameObject BindEffect2CardFeet(GameObject card_go, GameObject effectPrefab, bool isbinding, Vector3 offset, bool _isScene)
    {
        if (effectPrefab == null)
        {
            Debug.LogWarning("BindEffect2CardFeet Invalide Paramter effectPrefab is null ");
            return null;
        }
        if (card_go == null)
        {
            Debug.LogWarning("BindEffect2CardFeet Invalide Paramter  card_go is null ");
            return null;
        }
        Transform[] componentsInChildren = card_go.transform.GetComponentsInChildren<Transform>();
        GameObject gameObject = null;
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.name == "Feet")
            {
                gameObject = transform.gameObject;
                break;
            }
        }
        if (gameObject == null)
        {
            Debug.LogWarning(card_go.name + " Can't Find Feet Point ");
            gameObject = card_go;
        }
        if ((null == gameObject) || (null == effectPrefab))
        {
            return null;
        }
        GameObject obj3 = UnityEngine.Object.Instantiate(effectPrefab) as GameObject;
        Quaternion localRotation = obj3.transform.localRotation;
        Vector3 localScale = obj3.transform.localScale;
        if (isbinding)
        {
            obj3.transform.parent = gameObject.transform;
        }
        else
        {
            obj3.transform.parent = card_go.transform.parent;
        }
        obj3.transform.localRotation = localRotation;
        obj3.transform.localScale = localScale;
        obj3.transform.localPosition = offset;
        return obj3;
    }

    [DebuggerHidden]
    private IEnumerator CreateCardLoop()
    {
        return new <CreateCardLoop>c__Iterator4E { <>f__this = this };
    }

    public void createLight()
    {
        if (this.dirLightObj == null)
        {
            this.dirLightObj = new GameObject("FakeLight");
            UnityEngine.Object.DontDestroyOnLoad(this.dirLightObj);
            this.dirLightObj.transform.parent = base.transform;
            Light component = this.dirLightObj.GetComponent<Light>();
            if (null == component)
            {
                component = this.dirLightObj.AddComponent<Light>();
            }
            component.type = LightType.Directional;
            component.color = new Color(1f, 0.933f, 0.81176f, 1f);
            MTDLayers.SetlayerRecursively(this.dirLightObj, MTDLayers.ThreeDGUI);
            component.cullingMask = MTDLayers.ThreeDGUI;
            this.dirLightObj.transform.localRotation = Quaternion.Euler(new Vector3(31f, 251f, 21f));
            component.intensity = 0.6f;
        }
    }

    public void DeleteRootObj(GameObject _root)
    {
        List<GameObject> list = new List<GameObject>();
        IEnumerator enumerator = _root.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                list.Add(current.gameObject);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        foreach (GameObject obj2 in list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
    }

    public void DestroyCharater(int index)
    {
        <DestroyCharater>c__AnonStorey159 storey = new <DestroyCharater>c__AnonStorey159 {
            index = index
        };
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(storey.index, out obj2);
        if (obj2 != null)
        {
            obj2.ReleaseFakeObj();
            this.fakeCharacters.Remove(storey.index);
        }
        else
        {
            this.waitCreateCardList.RemoveAll(new Predicate<WaitCreateCardInfo>(storey.<>m__132));
        }
    }

    public void destroyLight()
    {
        if (this.dirLightObj != null)
        {
            UnityEngine.Object.Destroy(this.dirLightObj);
        }
    }

    public GameObject FindFakeObjByIndex(int index)
    {
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(index, out obj2);
        if (obj2 != null)
        {
            return obj2.fakeObj;
        }
        return null;
    }

    public void getCharacterPositionAndRotateByType(int index, ModelViewType viewType, out Vector3 position, out Quaternion localRotate)
    {
        position = Vector3.one;
        localRotate = Quaternion.identity;
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(index, out obj2);
        if (obj2 != null)
        {
            obj2.getPositionAndRotateByType(viewType, out position, out localRotate);
        }
    }

    public static void getHeightByIndex(int idx, out float xPos, out float yPos)
    {
        if (isEditorMode)
        {
            xPos = 0f;
            yPos = 0f;
        }
        else
        {
            float num = stepHeight * idx;
            xPos = (num / 3000f) * stepWidth;
            yPos = baseHeight + (num % 3000f);
        }
    }

    public static FakeCharacter GetInstance()
    {
        return _instance;
    }

    public GameObject GetObjectByID(int ID)
    {
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(ID, out obj2);
        if (obj2 == null)
        {
            return null;
        }
        return obj2.fakeObj;
    }

    private void LateUpdate()
    {
        foreach (KeyValuePair<int, FakeObject> pair in this.fakeCharacters)
        {
            FakeObject obj2 = pair.Value;
            if (obj2 != null)
            {
                obj2.CheckValidationRTT();
            }
        }
    }

    public void PlayCharAnimByIndex(int index, string name)
    {
        FakeObject obj2 = null;
        if (this.fakeCharacters.TryGetValue(index, out obj2))
        {
            obj2.fakeObj.GetComponent<AnimFSM>().PlayAnim(name, 1f, 0f, false);
        }
    }

    private void RealSnapCharacter(int index, GameObject sampleModel, ModelViewType viewType, UITexture uiTex, float zDepth, int backgroundidx)
    {
        uiTex.enabled = true;
        FakeObject obj2 = new FakeObject();
        obj2.Init(index, sampleModel, viewType, uiTex, zDepth, backgroundidx);
        this.fakeCharacters.Add(index, obj2);
    }

    public void Roate(int index, float delta)
    {
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(index, out obj2);
        if (obj2 != null)
        {
            obj2.fakeObj.transform.Rotate(new Vector3(0f, delta, 0f));
        }
    }

    public void SetCameraBackgroundColor(int index, Color col)
    {
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(index, out obj2);
        if (obj2 != null)
        {
            obj2.fakeCam.backgroundColor = col;
        }
    }

    public int SnapCardCharacter(CardPlayer.CardPlayerInitInfo _info, ModelViewType _viewType, UITexture _uiTex, int _backgroundidx)
    {
        GameObject obj2;
        this.maxFakeCharIndex++;
        WaitCreateCardInfo info = new WaitCreateCardInfo {
            index = this.maxFakeCharIndex,
            viewType = _viewType,
            info = _info,
            uiTex = _uiTex,
            backgroundidx = _backgroundidx
        };
        if (info.info != null)
        {
            obj2 = CardPlayer.CreateCardPlayer(info.info, CardPlayerStateType.Normal);
        }
        else
        {
            obj2 = null;
            Debug.LogWarning("SnapCardCharacter Failed");
        }
        this.RealSnapCharacter(info.index, obj2, info.viewType, info.uiTex, 0f, info.backgroundidx);
        return this.maxFakeCharIndex;
    }

    public int SnapCardCharacter(int _cardEntry, int _cardQuality, CombatEquip _equip, ModelViewType _viewType, UITexture _uiTex)
    {
        return this.SnapCardCharacter(_cardEntry, _cardQuality, _equip, _viewType, _uiTex, -1);
    }

    public int SnapCardCharacter(int _cardEntry, int _cardQuality, CombatEquip _equip, ModelViewType _viewType, UITexture _uiTex, int _backgroundidx)
    {
        CardPlayer.CardPlayerInitInfo info = new CardPlayer.CardPlayerInitInfo {
            cardEntry = _cardEntry,
            cardQuality = _cardQuality,
            equip = _equip
        };
        return this.SnapCardCharacter(info, _viewType, _uiTex, _backgroundidx);
    }

    public int SnapCardCharacterWithEquip(int _cardEntry, int _cardQuality, List<EquipInfo> _equip, ModelViewType _viewType, UITexture _uiTex)
    {
        CombatEquip equip = null;
        if ((_equip != null) && (_equip.Count > 0))
        {
            equip = new CombatEquip {
                entry = (short) _equip[0].entry,
                level = (short) _equip[0].lv,
                quality = (short) _equip[0].quality
            };
        }
        return this.SnapCardCharacter(_cardEntry, _cardQuality, equip, _viewType, _uiTex, -1);
    }

    public int SnapCharacter(GameObject sampleModel, ModelViewType viewType, UITexture uiTex, float zDepth, int backgroundidx)
    {
        this.maxFakeCharIndex++;
        this.RealSnapCharacter(this.maxFakeCharIndex, sampleModel, viewType, uiTex, zDepth, backgroundidx);
        return this.maxFakeCharIndex;
    }

    public void UpdateCharacterEquip(int index, List<EquipInfo> _equip)
    {
        CombatEquip equip = null;
        if ((_equip != null) && (_equip.Count > 0))
        {
            equip = new CombatEquip {
                entry = (short) _equip[0].entry,
                level = (short) _equip[0].lv,
                quality = (short) _equip[0].quality
            };
        }
        FakeObject obj2 = null;
        if (this.fakeCharacters.TryGetValue(index, out obj2))
        {
            obj2.fakeObj.GetComponent<CardPlayer>().UpdateEquip(equip);
        }
    }

    public void UpdateLayer(int index)
    {
        FakeObject obj2 = null;
        this.fakeCharacters.TryGetValue(index, out obj2);
    }

    public void UpdateSnapCardCharacterWithEquip(int indexer, int _cardEntry, int _cardQuality, List<EquipInfo> _equip, ModelViewType _viewType, UITexture _uiTex)
    {
        CombatEquip equip = null;
        if ((_equip != null) && (_equip.Count > 0))
        {
            equip = new CombatEquip {
                entry = (short) _equip[0].entry,
                level = (short) _equip[0].lv,
                quality = (short) _equip[0].quality
            };
        }
    }

    [CompilerGenerated]
    private sealed class <CreateCardLoop>c__Iterator4E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal FakeCharacter <>f__this;
        internal FakeCharacter.WaitCreateCardInfo <info>__0;
        internal GameObject <newObj>__1;

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
                    break;

                case 1:
                    break;
                    this.$PC = -1;
                    goto Label_0101;

                default:
                    goto Label_0101;
            }
            if (this.<>f__this.waitCreateCardList.Count > 0)
            {
                this.<info>__0 = this.<>f__this.waitCreateCardList[0];
                if (this.<info>__0.info != null)
                {
                    this.<newObj>__1 = CardPlayer.CreateCardPlayer(this.<info>__0.info, CardPlayerStateType.Normal);
                }
                else
                {
                    this.<newObj>__1 = null;
                    Debug.LogWarning("SnapCardCharacter Failed");
                }
                this.<>f__this.RealSnapCharacter(this.<info>__0.index, this.<newObj>__1, this.<info>__0.viewType, this.<info>__0.uiTex, 0f, this.<info>__0.backgroundidx);
                this.<>f__this.waitCreateCardList.RemoveAt(0);
            }
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            return true;
        Label_0101:
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

    [CompilerGenerated]
    private sealed class <DestroyCharater>c__AnonStorey159
    {
        internal int index;

        internal bool <>m__132(FakeCharacter.WaitCreateCardInfo obj)
        {
            return (obj.index == this.index);
        }
    }

    public class FakeObject
    {
        public Camera fakeCam;
        private float fakeDepth;
        public GameObject fakeObj;
        private GameObject fakeParent;
        public int heightRTT;
        public UITexture uiTex;
        public int widthRTT;

        public void CheckValidationRTT()
        {
            if ((null != this.fakeCam) && ((this.widthRTT > 0) && (this.heightRTT > 0)))
            {
                this.CreateRTT4Camera(this.fakeCam, this.widthRTT, this.heightRTT);
            }
        }

        private void CreateRTT4Camera(Camera camera, int width, int height)
        {
            if (null != camera)
            {
                RenderTexture targetTexture = camera.targetTexture;
                if ((null != targetTexture) && !targetTexture.IsCreated())
                {
                    targetTexture.Release();
                    RenderTexture texture2 = null;
                    camera.targetTexture = texture2;
                    targetTexture = texture2;
                }
                if (null == targetTexture)
                {
                    targetTexture = new RenderTexture(width, height, 8, RenderTextureFormat.ARGB32);
                }
                targetTexture.name = camera.name + "/RTT";
                targetTexture.isPowerOfTwo = (width == height) && (0x100 == width);
                targetTexture.hideFlags = HideFlags.DontSave;
                camera.targetTexture = targetTexture;
                if (null != this.uiTex)
                {
                    this.uiTex.mainTexture = camera.targetTexture;
                    this.uiTex.material.SetTexture("_MainTex", this.uiTex.mainTexture);
                }
            }
        }

        private string getFaction(int faction)
        {
            if (faction == 1)
            {
                return "Ui_Cards_Bg_lm";
            }
            if (faction == 2)
            {
                return "Ui_Cards_Bg_bl";
            }
            return "Ui_Cards_Bg_g";
        }

        private Vector3 getFakeObjPositionByHeight(int idx, Vector3 pos, float zDepth)
        {
            float num;
            float num2;
            FakeCharacter.getHeightByIndex(idx, out num, out num2);
            return new Vector3(pos.x + num, num2 + pos.y, pos.z + zDepth);
        }

        public void getPositionAndRotateByType(ModelViewType viewType, out Vector3 position, out Quaternion localRotate)
        {
            Vector3 zero = Vector3.zero;
            float y = 0f;
            ModelWrap wrap = CardModelWrapDataManager.static_GetModelWrap(this.fakeObj.name);
            if (wrap != null)
            {
                zero = (viewType != ModelViewType.normal) ? wrap.halfViewPos : wrap.viewPos;
                y = (viewType != ModelViewType.normal) ? wrap.halfViewRotY : wrap.viewRotY;
            }
            position = new Vector3(zero.x, zero.y, zero.z + this.fakeDepth);
            localRotate = Quaternion.Euler(new Vector3(0f, y, 0f));
        }

        public void Init(int index, GameObject sampleObj, ModelViewType viewType, UITexture _uiTex, float zDepth, int backgroundidx)
        {
            backgroundidx = -1;
            this.uiTex = _uiTex;
            if (viewType != ModelViewType.side)
            {
                Shader shader = Shader.Find("Unlit/Texture");
                this.uiTex.material = new Material(shader);
            }
            else
            {
                Shader shader2 = Shader.Find("Unlit/Transparent Colored");
                this.uiTex.material = new Material(shader2);
            }
            this.InitFakeObject(index, sampleObj, viewType, this.uiTex.width, this.uiTex.height, zDepth, backgroundidx);
            this.uiTex.material.hideFlags = HideFlags.DontSave;
            this.uiTex.mainTexture = this.fakeCam.targetTexture;
            this.uiTex.material.SetTexture("_MainTex", this.uiTex.mainTexture);
            this.uiTex.gameObject.SetActive(true);
        }

        private void InitCamSetting(int index, GameObject parent, ModelViewType viewType, int sizeX, int sizeY, bool isPersptive)
        {
            GameObject obj2 = new GameObject("FakeCamera") {
                transform = { parent = parent.transform }
            };
            this.fakeCam = obj2.GetComponent<Camera>();
            if (null == this.fakeCam)
            {
                this.fakeCam = obj2.AddComponent<Camera>();
            }
            this.fakeCam.nearClipPlane = 0.3f;
            this.fakeCam.farClipPlane = 100f;
            this.fakeCam.aspect = ((float) sizeX) / ((float) sizeY);
            this.fakeCam.clearFlags = CameraClearFlags.Color;
            this.fakeCam.backgroundColor = Color.clear;
            this.fakeCam.isOrthoGraphic = !isPersptive;
            this.fakeCam.orthographicSize = 1f;
            if (isPersptive)
            {
                this.fakeCam.fieldOfView = 35f;
            }
            this.fakeCam.transform.localPosition = new Vector3(0f, 0.73f, 1.65f);
            this.fakeCam.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            this.CreateRTT4Camera(this.fakeCam, sizeX, sizeY);
            if (viewType != ModelViewType.side)
            {
                GameObject obj3 = ObjectManager.CreateObj("EffectPrefabs/FakeBK");
                obj3.transform.parent = parent.transform;
                obj3.transform.localPosition = new Vector3(0f, 0f, -10f);
            }
            this.widthRTT = sizeX;
            this.heightRTT = sizeY;
        }

        private void InitFake3DObject(int index, ModelViewType viewType, GameObject parent, GameObject sampleObj, float zDepth)
        {
            this.fakeDepth = zDepth;
            this.fakeObj = sampleObj;
            this.fakeObj.transform.parent = parent.transform;
            if (null != this.fakeCam)
            {
                Vector3 zero = Vector3.zero;
                float y = 0f;
                float x = 1f;
                ModelWrap wrap = CardModelWrapDataManager.static_GetModelWrap(this.fakeObj.name);
                string animName = null;
                float startTime = -1f;
                if (wrap != null)
                {
                    switch (viewType)
                    {
                        case ModelViewType.normal:
                            zero = wrap.viewPos;
                            y = wrap.viewRotY;
                            this.fakeCam.orthographicSize = wrap.CameraOrthGSize;
                            animName = wrap.animName;
                            startTime = wrap.animTime;
                            x = wrap.modelScale;
                            break;

                        case ModelViewType.half:
                            zero = wrap.halfViewPos;
                            y = wrap.halfViewRotY;
                            this.fakeCam.orthographicSize = wrap.halfCameraOrthSize;
                            animName = wrap.halfAnimName;
                            startTime = wrap.halfAnimTime;
                            x = wrap.halfModelScale;
                            break;

                        case ModelViewType.side:
                            zero = wrap.sideViewPos;
                            y = wrap.sideViewRotY;
                            this.fakeCam.orthographicSize = wrap.sideCameraOrthSize;
                            animName = wrap.sideAnimName;
                            startTime = wrap.sideAnimTime;
                            x = wrap.sideModelScale;
                            break;
                    }
                }
                AnimFSM component = this.fakeObj.GetComponent<AnimFSM>();
                if ((component != null) && !string.IsNullOrEmpty(animName))
                {
                    if (startTime > 0f)
                    {
                        component.PlayAnim(animName, 0f, startTime, false);
                    }
                    else
                    {
                        component.PlayAnim(animName, 1f, 0f, false);
                    }
                }
                MTDLayers.SetlayerRecursively(this.fakeObj, MTDLayers.ThreeDGUI);
                this.fakeObj.transform.localPosition = new Vector3(zero.x, zero.y, zero.z + zDepth);
                this.fakeObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, y, 0f));
                this.fakeObj.transform.localScale = new Vector3(x, x, x);
            }
        }

        private void InitFakeObject(int index, GameObject sampleObj, ModelViewType viewType, int sizeX, int sizeY, float zDepth, int backgroundidx)
        {
            this.fakeParent = new GameObject("fakeobj" + index);
            this.fakeParent.transform.parent = FakeCharacter.GetInstance().transform;
            this.fakeParent.transform.position = this.getFakeObjPositionByHeight(index, Vector3.zero, zDepth);
            this.InitCamSetting(index, this.fakeParent, viewType, sizeX, sizeY, backgroundidx != -1);
            this.InitFake3DObject(index, viewType, this.fakeParent, sampleObj, zDepth);
            if (backgroundidx != -1)
            {
                GameObject obj2 = BundleMgr.Instance.CreateEffectObject("GUI/Prefab/CardAttribute_BG");
                Debug.Log((("is null" + obj2) == null) ? "failed" : "ok");
                if (null != obj2)
                {
                    Vector3 vector = (Vector3) (obj2.transform.localScale * this.fakeCam.orthographicSize);
                    obj2.transform.parent = this.fakeParent.transform;
                    obj2.transform.localPosition = new Vector3(0f, 0.67f, -40f);
                    obj2.transform.localScale = vector;
                }
            }
        }

        public void ReleaseFakeObj()
        {
            NGUITools.Destroy(this.uiTex.material);
            NGUITools.Destroy(this.uiTex.mainTexture);
            if (this.fakeObj != null)
            {
                this.fakeObj.transform.parent = null;
                CardPlayer.DestroyCardPlayer(this.fakeObj);
                this.fakeObj = null;
            }
            UnityEngine.Object.DestroyImmediate(this.fakeParent);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WaitCreateCardInfo
    {
        public int index;
        public CardPlayer.CardPlayerInitInfo info;
        public UITexture uiTex;
        public ModelViewType viewType;
        public int backgroundidx;
    }
}

