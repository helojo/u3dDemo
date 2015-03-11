using FastBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealTimeSkillManager
{
    [CompilerGenerated]
    private static Func<KeyValuePair<int, RealTimeSkill>, bool> <>f__am$cache2;
    private Dictionary<int, RealTimeSkill> allSkill = new Dictionary<int, RealTimeSkill>();
    public BattleData battleGameData;

    public void AddSkill(int skillID, RealTimeSkill newSkill)
    {
        this.allSkill.Add(skillID, newSkill);
    }

    public void BreakSkill(int skillID)
    {
        RealTimeSkill skill;
        if (this.allSkill.TryGetValue(skillID, out skill))
        {
            skill.Stop();
        }
    }

    public void CastNewSkill(int skillEntry, int skillID, int casterID, List<int> targetID, float _moveCollsionDis)
    {
        if (this.allSkill.ContainsKey(skillID))
        {
            Debug.LogWarning("same SkillID already exists " + skillID.ToString());
        }
        else
        {
            RealTimeSkill skill = new GameObject { name = "skill " + skillEntry.ToString() }.AddComponent<RealTimeSkill>();
            this.allSkill.Add(skillID, skill);
            skill.Init(skillEntry, skillID, this.battleGameData, casterID, targetID, _moveCollsionDis);
            skill.StartProcess();
        }
    }

    public void Clear()
    {
        foreach (KeyValuePair<int, RealTimeSkill> pair in this.allSkill)
        {
            if (pair.Value != null)
            {
                pair.Value.Clear();
                UnityEngine.Object.Destroy(pair.Value.gameObject);
            }
        }
        this.allSkill.Clear();
    }

    public void EventOnDoSkillCasting(int skillID, string effectName, List<int> targetList)
    {
        RealTimeSkill skill;
        if (this.allSkill.TryGetValue(skillID, out skill))
        {
            skill.DoAICasting(effectName, targetList);
        }
    }

    public RealTimeSkill GetSkill(int skillID)
    {
        RealTimeSkill skill;
        if (this.allSkill.TryGetValue(skillID, out skill))
        {
        }
        return skill;
    }

    public bool IsAnySkillRunning()
    {
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = obj => obj.Value != null;
        }
        return this.allSkill.Any<KeyValuePair<int, RealTimeSkill>>(<>f__am$cache2);
    }

    public void OnMsgSkillShowTimeClean()
    {
        foreach (KeyValuePair<int, RealTimeSkill> pair in this.allSkill)
        {
            if (pair.Value != null)
            {
                pair.Value.OnFinishShowTime();
            }
        }
    }

    public void PrepareCardSkillResources(int cardEntry)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config != null)
        {
            this.PrepareSkillResources(_config.foremost_skill);
            foreach (int num in StrParser.ParseDecIntList(_config.normal_skill, -1))
            {
                this.PrepareSkillResources(num);
            }
            this.PrepareSkillResources(_config.active_skill);
        }
    }

    private void PrepareSkillResources(int skillEntry)
    {
        skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(skillEntry);
        if (_config != null)
        {
            RealSkillInfoManager.CacheSkillResource(_config.effects);
            AiFragmentManager.GetInstance().CacheSkillFragment(_config.skill_ai);
        }
    }

    public void SkillTick()
    {
        if (this.battleGameData != null)
        {
            BattleCom_Runtime component = this.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>();
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, RealTimeSkill> pair in this.allSkill)
            {
                if ((pair.Value != null) && pair.Value.IsFinish())
                {
                    pair.Value.OnFinish();
                    component.OnSkillFinish(pair.Value.casterID, pair.Value.skillID);
                    UnityEngine.Object.DestroyObject(pair.Value.gameObject, 1f);
                    list.Add(pair.Key);
                }
            }
            list.ForEach(obj => this.allSkill.Remove(obj));
        }
    }
}

