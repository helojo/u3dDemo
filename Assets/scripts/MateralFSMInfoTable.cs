using System;
using System.Collections.Generic;

[Serializable]
public class MateralFSMInfoTable
{
    public List<string> excludeNameList = new List<string>();
    public List<MaterialFSMInfo> infoes = new List<MaterialFSMInfo>();
    private Dictionary<string, MaterialFSMInfo> infoMap;
    public string name;

    public MaterialFSMInfo GetFSMInfo(string name)
    {
        return this.infoMap[name];
    }

    public void Init()
    {
        this.infoMap = new Dictionary<string, MaterialFSMInfo>();
        this.infoes.ForEach(obj => this.infoMap.Add(obj.name, obj));
    }
}

