using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ModelControler
{
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cacheB;
    private List<GameObject> allAnimObjs = new List<GameObject>();
    private GameObject animObj;
    private float animSpeed = 1f;
    private List<ModelAttachObjInfo> AttachedObj = new List<ModelAttachObjInfo>();
    private GameObject deadModel;
    private bool isPasueAnim;
    private GameObject midObj;
    public System.Action OnModelChangeEvent;
    private Transform parentTrans;
    private float scaleTarget;
    private float scaleTargetVelocity;

    public void AddModel(GameObject newObj)
    {
        newObj.transform.parent = this.parentTrans;
        newObj.transform.localRotation = Quaternion.identity;
        newObj.transform.localPosition = Vector3.zero;
        this.allAnimObjs.Add(newObj);
    }

    public void AddScale(float _scale)
    {
        this.scaleTarget *= _scale;
    }

    public GameObject AttachByPrefab(GameObject prefab, HangPointType _point, float delayTime, Vector3 _offset, bool remainActive = false)
    {
        GameObject obj2 = this.GetCurModel().GetComponent<HangControler>().AttachByPrefab(prefab, _point, -1f, delayTime, _offset, true, remainActive);
        if (obj2 != null)
        {
            ModelAttachObjInfo item = new ModelAttachObjInfo {
                obj = obj2,
                point = _point,
                offset = _offset
            };
            this.AttachedObj.Add(item);
        }
        return obj2;
    }

    public void Clear()
    {
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = obj => CardPlayer.DestroyCardPlayer(obj);
        }
        this.allAnimObjs.ForEach(<>f__am$cacheB);
        this.allAnimObjs.Clear();
        this.animObj = null;
    }

    public void DeleteModel(string modelName)
    {
        <DeleteModel>c__AnonStoreyEB yeb = new <DeleteModel>c__AnonStoreyEB {
            modelName = modelName
        };
        GameObject obj2 = this.allAnimObjs.Find(new Predicate<GameObject>(yeb.<>m__59));
        if (obj2 != null)
        {
            CardPlayer.DestroyCardPlayer(obj2);
            this.allAnimObjs.Remove(obj2);
            if (this.allAnimObjs.Count > 0)
            {
                this.animObj = this.allAnimObjs[this.allAnimObjs.Count - 1];
                this.animObj.SetActive(true);
                this.OnChangeModel();
            }
        }
    }

    public GameObject GetCurModel()
    {
        return this.animObj;
    }

    public bool GetIsPauseAnim()
    {
        return this.isPasueAnim;
    }

    public void Init(GameObject parent, GameObject defaultAnimObj)
    {
        this.midObj = new GameObject();
        this.midObj.transform.parent = parent.transform;
        this.midObj.transform.localPosition = Vector3.zero;
        this.midObj.transform.localScale = Vector3.one;
        this.midObj.transform.rotation = Quaternion.identity;
        this.parentTrans = this.midObj.transform;
        this.scaleTarget = 1f;
        this.AddModel(defaultAnimObj);
        this.animObj = defaultAnimObj;
    }

    private void OnAnimSpeedChange()
    {
        float num = !this.isPasueAnim ? this.animSpeed : 0f;
        this.GetCurModel().GetComponent<AnimFSM>().SetSpeed(num);
    }

    private void OnChangeModel()
    {
        <OnChangeModel>c__AnonStoreyEC yec = new <OnChangeModel>c__AnonStoreyEC();
        if (this.OnModelChangeEvent != null)
        {
            this.OnModelChangeEvent();
        }
        yec.hang = this.GetCurModel().GetComponent<HangControler>();
        this.AttachedObj.ForEach(new Action<ModelAttachObjInfo>(yec.<>m__5B));
    }

    public void OnScaleTick()
    {
        float x = this.midObj.transform.localScale.x;
        if (x != this.scaleTarget)
        {
            x = Mathf.SmoothDamp(x, this.scaleTarget, ref this.scaleTargetVelocity, 0.2f);
            this.midObj.transform.localScale = new Vector3(x, x, x);
        }
    }

    public void SetAnimSpeed(float speed)
    {
        this.animSpeed = speed;
        this.OnAnimSpeedChange();
    }

    public void SetPasueAnim(bool isPause)
    {
        if (this.isPasueAnim != isPause)
        {
            this.isPasueAnim = isPause;
            this.OnAnimSpeedChange();
        }
    }

    public void ShowModel(string modelName, bool isPlayBornAnim)
    {
        <ShowModel>c__AnonStoreyEA yea = new <ShowModel>c__AnonStoreyEA {
            modelName = modelName
        };
        bool flag = false;
        Vector3 one = Vector3.one;
        Vector3 zero = Vector3.zero;
        if ((this.animObj == null) || (this.animObj.name != yea.modelName))
        {
            if (this.animObj != null)
            {
                BoxCollider component = this.animObj.GetComponent<BoxCollider>();
                if ((component != null) && component.enabled)
                {
                    flag = true;
                    one = component.size;
                    zero = component.center;
                }
            }
            GameObject newObj = this.allAnimObjs.Find(new Predicate<GameObject>(yea.<>m__58));
            if (newObj == null)
            {
                newObj = CardPlayer.CreateNormalObj(yea.modelName);
                this.AddModel(newObj);
            }
            if (newObj != null)
            {
                this.animObj.SetActive(false);
                this.animObj.GetComponent<MaterialFSM>().ClearTempNaterialChange();
                this.animObj = newObj;
                this.animObj.SetActive(true);
                this.allAnimObjs.Remove(this.animObj);
                this.allAnimObjs.Add(this.animObj);
                if (flag)
                {
                    BoxCollider collider2 = this.animObj.GetComponent<BoxCollider>();
                    if (collider2 == null)
                    {
                        collider2 = this.animObj.AddComponent<BoxCollider>();
                    }
                    collider2.enabled = true;
                    collider2.center = zero;
                    collider2.size = one;
                }
                this.OnChangeModel();
            }
            if (isPlayBornAnim)
            {
                this.GetCurModel().GetComponent<AnimFSM>().PlayAnim(BattleGlobal.BornAnim, 1f, 0f, false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <DeleteModel>c__AnonStoreyEB
    {
        internal string modelName;

        internal bool <>m__59(GameObject obj)
        {
            return (obj.name == this.modelName);
        }
    }

    [CompilerGenerated]
    private sealed class <OnChangeModel>c__AnonStoreyEC
    {
        internal HangControler hang;

        internal void <>m__5B(ModelControler.ModelAttachObjInfo info)
        {
            if (info.obj != null)
            {
                this.hang.AttachToHangPoint(info.obj, info.point, info.offset);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <ShowModel>c__AnonStoreyEA
    {
        internal string modelName;

        internal bool <>m__58(GameObject obj)
        {
            return (obj.name == this.modelName);
        }
    }

    public class ModelAttachObjInfo
    {
        public GameObject obj;
        public Vector3 offset;
        public HangPointType point;
    }
}

