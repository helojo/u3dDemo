using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFSMManager : MonoBehaviour
{
    private static CharacterFSMManager _instance;
    public List<CharacterFSMInfoTable> allCharacterFSMInfo = new List<CharacterFSMInfoTable>();
    public List<ParkourLevelInfo> parkourLevelInfo;
    private Dictionary<CharacterType, CharacterFSMInfo> tabelMap;

    public CharacterFSMInfo GetStateInfo(CharacterType type)
    {
        CharacterFSMInfo info;
        if (type == CharacterType.None)
        {
            Debug.LogError("Can't find type " + type);
            return null;
        }
        if (!this.tabelMap.TryGetValue(type, out info))
        {
            Debug.LogError("Can't find type " + type);
            type = CharacterType.MOVE;
            this.tabelMap.TryGetValue(type, out info);
        }
        return info;
    }

    public void Init()
    {
        this.tabelMap = new Dictionary<CharacterType, CharacterFSMInfo>();
        this.allCharacterFSMInfo.ForEach(obj => this.tabelMap.Add(obj.type, obj.state));
    }

    public static CharacterFSMManager Instance()
    {
        if (_instance == null)
        {
            GameObject target = ObjectManager.CreateObj(BattleGlobal.ParkourFSMPrefab);
            _instance = target.GetComponent<CharacterFSMManager>();
            UnityEngine.Object.DontDestroyOnLoad(target);
            _instance.Init();
        }
        return _instance;
    }
}

