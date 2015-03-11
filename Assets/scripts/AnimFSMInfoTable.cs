using System;
using System.Collections.Generic;

[Serializable]
public class AnimFSMInfoTable
{
    public List<AnimFSMInfo> allStates = new List<AnimFSMInfo>();
    public string name;
    private Dictionary<string, AnimFSMInfo> stateMap;

    public AnimFSMInfo GetState(string name)
    {
        AnimFSMInfo info;
        this.stateMap.TryGetValue(name, out info);
        return info;
    }

    public void Init()
    {
        this.stateMap = new Dictionary<string, AnimFSMInfo>();
        this.allStates.ForEach(obj => this.stateMap.Add(obj.name, obj));
    }
}

