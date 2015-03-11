using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BattleSceneAnimDataManager : MonoBehaviour
{
    private static BattleSceneAnimDataManager _instance;
    private Dictionary<string, BattleSceneAnimData> dataMap = new Dictionary<string, BattleSceneAnimData>();
    public List<BattleSceneAnimData> datas = new List<BattleSceneAnimData>();

    private void Awake()
    {
        _instance = this;
        this.InitData();
    }

    public BattleSceneAnimData GetData(string name)
    {
        BattleSceneAnimData data;
        <GetData>c__AnonStorey156 storey = new <GetData>c__AnonStorey156 {
            name = name
        };
        if (this.dataMap.TryGetValue(storey.name, out data))
        {
            return data;
        }
        return this.datas.Find(new Predicate<BattleSceneAnimData>(storey.<>m__128));
    }

    public static BattleSceneAnimDataManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject target = ObjectManager.CreateObj("BattlePrefabs/BattleSceneAnimData");
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance = target.GetComponent<BattleSceneAnimDataManager>();
        }
        return _instance;
    }

    private void InitData()
    {
        this.dataMap.Clear();
        foreach (BattleSceneAnimData data in this.datas)
        {
            this.dataMap.Add(data.name, data);
        }
    }

    [CompilerGenerated]
    private sealed class <GetData>c__AnonStorey156
    {
        internal string name;

        internal bool <>m__128(BattleSceneAnimData obj)
        {
            return (obj.name == this.name);
        }
    }
}

