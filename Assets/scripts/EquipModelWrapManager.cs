using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipModelWrapManager : MonoBehaviour
{
    private static EquipModelWrapManager _instance;
    public List<EquipModelWrapInfo> data;
    private Dictionary<string, EquipModelWrapInfo> dataMap = new Dictionary<string, EquipModelWrapInfo>();
    public GameObject testModel;

    private void Awake()
    {
        _instance = this;
        this.InitData();
    }

    public static void ChangeEquipModel(GameObject equip, EquipModelWrapInfo wrap)
    {
        foreach (MeshRenderer renderer in equip.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.name.StartsWith("wea"))
            {
                renderer.material.shader = Shader.Find("MT/Fumo");
                renderer.material.SetTexture("_AddTex", wrap.texture);
                renderer.material.SetFloat("_AddTex_uv_x", wrap.uv_x);
                renderer.material.SetFloat("_AddTex_uv_y", wrap.uv_y);
                renderer.material.SetColor("_AddColor", wrap.color);
            }
        }
    }

    public void ChangeEquipModel(GameObject equip, string wrapName)
    {
        if (!string.IsNullOrEmpty(wrapName))
        {
            EquipModelWrapInfo info;
            if (this.dataMap.TryGetValue(wrapName, out info))
            {
                ChangeEquipModel(equip, info);
            }
            else
            {
                Debug.LogWarning("can't find equip wrap " + wrapName);
            }
        }
    }

    public static EquipModelWrapManager GetInstance()
    {
        return _instance;
    }

    private void InitData()
    {
        foreach (EquipModelWrapInfo info in this.data)
        {
            this.dataMap.Add(info.name, info);
        }
    }
}

