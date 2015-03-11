using System;
using System.Collections.Generic;

[Serializable]
public class BattleSceneAnimData
{
    public List<BattleSceneAnimInfo> infoes = new List<BattleSceneAnimInfo>();
    public string name;
    public float time;

    public BattleSceneAnimData Clone()
    {
        BattleSceneAnimData data = new BattleSceneAnimData {
            name = this.name,
            time = this.time
        };
        foreach (BattleSceneAnimInfo info in this.infoes)
        {
            data.infoes.Add(info.Clone());
        }
        return data;
    }
}

