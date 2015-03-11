using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialFSMInfoManager : MonoBehaviour
{
    private static MaterialFSMInfoManager _instance;
    public List<MateralFSMInfoTable> allInfoes = new List<MateralFSMInfoTable>();
    private Dictionary<string, MateralFSMInfoTable> infoMap;

    public MateralFSMInfoTable GetStateInfo(string name)
    {
        MateralFSMInfoTable table;
        if (!this.infoMap.TryGetValue(name, out table))
        {
            Debug.LogWarning("Can't find MaterialFSM " + name);
        }
        return table;
    }

    private void Init()
    {
        this.infoMap = new Dictionary<string, MateralFSMInfoTable>();
        this.allInfoes.ForEach(delegate (MateralFSMInfoTable obj) {
            obj.Init();
            this.infoMap.Add(obj.name, obj);
        });
    }

    public static MaterialFSMInfoManager Instance()
    {
        if (_instance == null)
        {
            GameObject target = ObjectManager.CreateObj(BattleGlobal.MaterialFSMPrefab);
            _instance = target.GetComponent<MaterialFSMInfoManager>();
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance.Init();
        }
        return _instance;
    }
}

