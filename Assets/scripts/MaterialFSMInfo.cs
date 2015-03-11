using System;
using System.Collections.Generic;

[Serializable]
public class MaterialFSMInfo
{
    public bool disableParticle;
    public List<string> excludeTagNameList = new List<string>();
    public string name;
    public int priority;
    public string shaderName;
}

