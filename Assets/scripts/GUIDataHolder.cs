using System;
using UnityEngine;

public class GUIDataHolder : MonoBehaviour
{
    private object _data;

    public static object getData(GameObject go)
    {
        return getDataHolder(go)._data;
    }

    private static GUIDataHolder getDataHolder(GameObject go)
    {
        GUIDataHolder component = go.GetComponent<GUIDataHolder>();
        if (component == null)
        {
            component = go.AddComponent<GUIDataHolder>();
        }
        return component;
    }

    public void OnDestroy()
    {
        this._data = null;
    }

    public static void setData(GameObject go, object data)
    {
        getDataHolder(go)._data = data;
    }
}

