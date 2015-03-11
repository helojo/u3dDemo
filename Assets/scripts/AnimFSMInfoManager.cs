using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimFSMInfoManager : MonoBehaviour
{
    private static AnimFSMInfoManager _instance;
    public List<AnimFSMInfoTable> allStateTables = new List<AnimFSMInfoTable>();
    private Dictionary<string, AnimFSMInfoTable> tableMap;

    public AnimFSMInfoTable GetStateInfo(string name)
    {
        AnimFSMInfoTable table;
        if (string.IsNullOrEmpty(name))
        {
            name = "battle";
        }
        if (!this.tableMap.TryGetValue(name, out table))
        {
            Debug.LogWarning("Can't find AnimState " + name);
            name = "battle";
            this.tableMap.TryGetValue(name, out table);
        }
        return table;
    }

    private void Init()
    {
        this.tableMap = new Dictionary<string, AnimFSMInfoTable>();
        this.allStateTables.ForEach(delegate (AnimFSMInfoTable obj) {
            obj.Init();
            this.tableMap.Add(obj.name, obj);
        });
    }

    public static AnimFSMInfoManager Instance()
    {
        if (_instance == null)
        {
            GameObject target = ObjectManager.CreateObj(BattleGlobal.AnimFsmPrefab);
            _instance = target.GetComponent<AnimFSMInfoManager>();
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance.Init();
        }
        return _instance;
    }
}

