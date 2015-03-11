using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectManager
{
    private static ObjectManager _instance;
    private RenderTexture _rt;
    [CompilerGenerated]
    private static Predicate<TempObjSlot> <>f__am$cache7;
    [CompilerGenerated]
    private static Action<TempObjSlot> <>f__am$cache8;
    [CompilerGenerated]
    private static Action<TempObjSlot> <>f__am$cache9;
    private Camera activeCamera;
    private Dictionary<string, GameObject> cachedObjs = new Dictionary<string, GameObject>();
    private GameObject SnapCamera;
    private List<TempObjSlot> tempObjList = new List<TempObjSlot>();

    private void _CacheObjResource(string name)
    {
        if (!string.IsNullOrEmpty(name) && !this.cachedObjs.ContainsKey(name))
        {
            this._RealCache(this._CreateObj(name), name);
        }
    }

    private void _CacheObjResource(GameObject prefab)
    {
        if ((prefab != null) && !this.cachedObjs.ContainsKey(prefab.name))
        {
            this._RealCache(this._InstantiateObj(prefab), prefab.name);
        }
    }

    private GameObject _CreateObj(string name)
    {
        GameObject obj2 = null;
        UnityEngine.Object original = this._LoadObj(name);
        if (original != null)
        {
            obj2 = UnityEngine.Object.Instantiate(original) as GameObject;
            if (obj2 == null)
            {
                Debug.LogWarning("Can't Instantiate " + name);
                return obj2;
            }
            obj2.name = name;
            return obj2;
        }
        Debug.LogWarning("Can't find " + name);
        return obj2;
    }

    private GameObject _CreateTempObj(GameObject _effect, Vector3 pos, float _remainTime, float _delayTime, bool _remainActive = false)
    {
        if (null != _effect)
        {
            _effect.transform.position = pos;
            if (_delayTime > 0f)
            {
                _effect.SetActive(false);
            }
            TempObjSlot item = new TempObjSlot {
                effect = _effect,
                linkObjectID = -1,
                remainTime = _remainTime,
                delayTime = _delayTime,
                remainActiveAfterBattle = _remainActive
            };
            this.tempObjList.Add(item);
        }
        return _effect;
    }

    private void _DestoryObj(GameObject obj)
    {
        if (obj != null)
        {
            UnityEngine.Object.Destroy(obj);
        }
    }

    private void _DestoryObj(GameObject obj, float time)
    {
        if (obj != null)
        {
            UnityEngine.Object.Destroy(obj, time);
        }
    }

    private GameObject _InstantiateObj(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }
        GameObject obj2 = null;
        if (obj2 == null)
        {
            obj2 = UnityEngine.Object.Instantiate(obj) as GameObject;
            obj2.name = obj.name;
        }
        return obj2;
    }

    private UnityEngine.Object _LoadObj(string name)
    {
        return BundleMgr.Instance.LoadResource(name, ".prefab", typeof(GameObject));
    }

    private void _RealCache(GameObject newObj, string name)
    {
        if (newObj != null)
        {
            newObj.transform.position = new Vector3(-100000f, -100000f, -10000f);
            foreach (EffectTimeDelay delay in newObj.GetComponents<EffectTimeDelay>())
            {
                if (delay.trans != null)
                {
                    delay.trans.gameObject.SetActive(true);
                }
            }
            this._SnapObject(newObj);
            this.cachedObjs.Add(name, newObj);
            if (this.CacheObjsRoot != null)
            {
                newObj.transform.parent = this.CacheObjsRoot.transform;
            }
            newObj.SetActive(false);
        }
    }

    private void _SetLinkeObjID(GameObject effect, int objID)
    {
        <_SetLinkeObjID>c__AnonStorey28B storeyb = new <_SetLinkeObjID>c__AnonStorey28B {
            effect = effect
        };
        TempObjSlot slot = this.tempObjList.Find(new Predicate<TempObjSlot>(storeyb.<>m__5E3));
        if (slot != null)
        {
            slot.linkObjectID = objID;
        }
    }

    private void _SnapObject(GameObject obj)
    {
        if (this._rt == null)
        {
            this.SnapCamera = new GameObject("SnapCamera");
            UnityEngine.Object.DontDestroyOnLoad(this.SnapCamera);
            this.activeCamera = this.SnapCamera.AddComponent<Camera>();
            this._rt = new RenderTexture(0x20, 0x20, 0x18);
            this._rt.Create();
            this.activeCamera.targetTexture = this._rt;
        }
        this.activeCamera.gameObject.SetActive(true);
        this.activeCamera.transform.position = obj.transform.position + ((Vector3) (Vector3.one * 10f));
        this.activeCamera.transform.LookAt(obj.transform.position);
        this.activeCamera.Render();
        this.activeCamera.gameObject.SetActive(false);
    }

    public static void CacheObjResource(string name)
    {
        Instance._CacheObjResource(name);
    }

    public static void CacheObjResource(GameObject prefab)
    {
        Instance._CacheObjResource(prefab);
    }

    private void ClearEffect()
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = delegate (TempObjSlot obj) {
                if (obj.effect != null)
                {
                    UnityEngine.Object.DestroyObject(obj.effect);
                }
            };
        }
        this.tempObjList.ForEach(<>f__am$cache8);
        this.tempObjList.Clear();
        this.cachedObjs.Clear();
    }

    public static void ClearTempObj()
    {
        Instance.ClearEffect();
    }

    public static GameObject CreateObj(string name)
    {
        return Instance._CreateObj(name);
    }

    public static GameObject CreateTempObj(string effectName, Vector3 pos, float _remainTime)
    {
        return Instance._CreateTempObj(CreateObj(effectName), pos, _remainTime, 0f, false);
    }

    public static GameObject CreateTempObj(GameObject _effect, Vector3 pos, float _remainTime)
    {
        return Instance._CreateTempObj(_effect, pos, _remainTime, 0f, false);
    }

    public static GameObject CreateTempObj(string effectName, Vector3 pos, float _remainTime, float _delayTime)
    {
        return Instance._CreateTempObj(CreateObj(effectName), pos, _remainTime, _delayTime, false);
    }

    public static GameObject CreateTempObj(GameObject _effect, Vector3 pos, float _remainTime, float _delayTime)
    {
        return Instance._CreateTempObj(_effect, pos, _remainTime, _delayTime, false);
    }

    public static GameObject CreateTempObj(string effectName, Vector3 pos, float _remainTime, float _delayTime, bool remainActive)
    {
        return Instance._CreateTempObj(CreateObj(effectName), pos, _remainTime, _delayTime, remainActive);
    }

    public static GameObject CreateTempObj(GameObject _effect, Vector3 pos, float _remainTime, float _delayTime, bool remainActive)
    {
        return Instance._CreateTempObj(_effect, pos, _remainTime, _delayTime, remainActive);
    }

    public static void DestoryObj(GameObject obj)
    {
        Instance._DestoryObj(obj);
    }

    public static void DestoryObj(GameObject obj, float time)
    {
        Instance._DestoryObj(obj, time);
    }

    private void HideEffect()
    {
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = delegate (TempObjSlot obj) {
                if ((obj.effect != null) && !obj.remainActiveAfterBattle)
                {
                    obj.effect.SetActive(false);
                }
            };
        }
        this.tempObjList.ForEach(<>f__am$cache9);
    }

    public static void HideTempObj()
    {
        Instance.HideEffect();
    }

    public static GameObject InstantiateObj(GameObject obj)
    {
        return Instance._InstantiateObj(obj);
    }

    public static void SetCacheObjsRoot(GameObject root)
    {
        Instance.CacheObjsRoot = root;
    }

    public static void SetTempObjLinkeObjID(GameObject effect, int objID)
    {
        Instance._SetLinkeObjID(effect, objID);
    }

    public static void SnapObject(GameObject obj)
    {
        Instance._SnapObject(obj);
    }

    public void Tick()
    {
        CardPlayerPool.Instance().UpdatePool();
        foreach (TempObjSlot slot in this.tempObjList)
        {
            float deltaTime = Time.deltaTime;
            if (slot.linkObjectID >= 0)
            {
                bool enable = false;
                if (TimeManager.GetInstance().IsShowTimeObj(slot.linkObjectID))
                {
                    if (BattleGlobal.GetShowTimeScale() < 0.1f)
                    {
                        enable = true;
                    }
                    deltaTime = BattleGlobal.ScaleSpeed_ShowTime(deltaTime);
                }
                else
                {
                    deltaTime = BattleGlobal.ScaleSpeed(deltaTime);
                }
                if (enable != slot.isShowTimeEnable)
                {
                    slot.isShowTimeEnable = enable;
                    BattleGlobalFunc.SetObjShowTimeEnable(slot.effect, enable);
                }
            }
            slot.runningTime += deltaTime;
            if ((slot.delayTime > 0f) && (slot.delayTime < slot.runningTime))
            {
                slot.delayTime = -1f;
                if (slot.effect != null)
                {
                    slot.effect.SetActive(true);
                }
            }
        }
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (TempObjSlot obj) {
                if ((obj.remainTime <= 0f) || (obj.remainTime >= obj.runningTime))
                {
                    return false;
                }
                if (obj.effect != null)
                {
                    DestoryObj(obj.effect);
                }
                return true;
            };
        }
        this.tempObjList.RemoveAll(<>f__am$cache7);
    }

    public GameObject CacheObjsRoot { get; set; }

    public static ObjectManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectManager();
            }
            return _instance;
        }
    }

    [CompilerGenerated]
    private sealed class <_SetLinkeObjID>c__AnonStorey28B
    {
        internal GameObject effect;

        internal bool <>m__5E3(ObjectManager.TempObjSlot obj)
        {
            return (obj.effect == this.effect);
        }
    }

    private class TempObjSlot
    {
        public float delayTime;
        public GameObject effect;
        public bool isShowTimeEnable;
        public int linkObjectID;
        public bool remainActiveAfterBattle;
        public float remainTime;
        public float runningTime;
    }
}

