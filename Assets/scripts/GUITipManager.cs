using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GUITipManager : MonoBehaviour
{
    private Queue<int> _delTemp = new Queue<int>();
    [SerializeField]
    public Camera CurrentUICamera;
    private Dictionary<int, GUITipUI> Tips = new Dictionary<int, GUITipUI>();
    [SerializeField]
    public Transform UIRoot;

    private void Awake()
    {
        Current = this;
    }

    private T Create<T>(int id) where T: GUITipUI, new()
    {
        System.Type type = typeof(T);
        GUITipAttribute[] customAttributes = type.GetCustomAttributes(typeof(GUITipAttribute), false) as GUITipAttribute[];
        string resourcesName = string.Empty;
        if ((customAttributes != null) && (customAttributes.Length > 0))
        {
            resourcesName = customAttributes[0].ResourcesName;
        }
        if (string.IsNullOrEmpty(resourcesName))
        {
            return null;
        }
        GameObject obj2 = UnityEngine.Object.Instantiate(BundleMgr.Instance.LoadResource("GUI/GUITips/" + resourcesName, ".prefab", typeof(GameObject))) as GameObject;
        if (obj2 == null)
        {
            return null;
        }
        obj2.transform.parent = base.transform;
        obj2.transform.localScale = Vector3.one;
        obj2.transform.localRotation = Quaternion.identity;
        T local = Activator.CreateInstance<T>();
        local.Root = obj2;
        local.Key = id;
        local.OnCreate();
        this.Tips.Add(id, local);
        return local;
    }

    public void DrawItemTip(int id, Vector3 pos, Vector3 offset, int itemID)
    {
        GUITipUI pui;
        if (!this.Tips.TryGetValue(id, out pui))
        {
            GUITipItem item = this.Create<GUITipItem>(id);
            if (item == null)
            {
                return;
            }
            item.ItemID = itemID;
            pui = item;
        }
        pui.OnDraw();
        pui.UpdatePos(pos, offset);
    }

    private void LateUpdate()
    {
        foreach (KeyValuePair<int, GUITipUI> pair in this.Tips)
        {
            if (pair.Value.LastUpdate)
            {
                pair.Value.LastUpdate = false;
            }
            else
            {
                this._delTemp.Enqueue(pair.Key);
            }
        }
        while (this._delTemp.Count > 0)
        {
            GUITipUI pui = this.Tips[this._delTemp.Dequeue()];
            if (pui != null)
            {
                UnityEngine.Object.Destroy(pui.Root);
                this.Tips.Remove(pui.Key);
            }
        }
    }

    public Vector3 OffsetPos(Transform trans)
    {
        Vector3 localScale = GUIMgr.Instance.Root.transform.localScale;
        Vector3 position = trans.position;
        return new Vector3(position.x / localScale.x, position.y / localScale.y, position.z / localScale.z);
    }

    private void OnDestory()
    {
        foreach (KeyValuePair<int, GUITipUI> pair in this.Tips)
        {
            UnityEngine.Object.Destroy(pair.Value.Root);
        }
        this.Tips.Clear();
        Current = null;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static GUITipManager Current
    {
        [CompilerGenerated]
        get
        {
            return <Current>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            <Current>k__BackingField = value;
        }
    }
}

