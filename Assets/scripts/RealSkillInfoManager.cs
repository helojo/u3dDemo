using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RealSkillInfoManager : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<SkillEffectInfo> <>f__am$cache2;
    private static Dictionary<string, RealTimeBufferInfo> G_bufferInfoCache = new Dictionary<string, RealTimeBufferInfo>();
    private static Dictionary<string, RealTimeSkillInfo> G_skillInfoCache = new Dictionary<string, RealTimeSkillInfo>();

    private static void CacheSkillActionResource(RealTimeSkillActionInfo info)
    {
        switch (info.actionType)
        {
            case SkillActionType.PlayAnimAndEffect:
                if (SettingMgr.mInstance.IsEffectEnable)
                {
                    if (<>f__am$cache2 == null)
                    {
                        <>f__am$cache2 = obj => ObjectManager.CacheObjResource(obj.effectPrefab);
                    }
                    info.effectInfos.ForEach(<>f__am$cache2);
                }
                break;

            case SkillActionType.Bullet:
                if (SettingMgr.mInstance.IsEffectEnable)
                {
                    ObjectManager.CacheObjResource(info.bulleteffect);
                }
                break;
        }
    }

    public static void CacheSkillResource(string effectName)
    {
        RealTimeSkillInfo skillData = GetSkillData(effectName);
        if (skillData != null)
        {
            foreach (RealTimeSkillActionInfo info2 in skillData.actionInfoes)
            {
                CacheSkillActionResource(info2);
                if ((info2 != null) && !string.IsNullOrEmpty(info2.soundName))
                {
                    SoundManager.mInstance.PlaySFX(info2.soundName, false, true);
                }
            }
        }
    }

    public static void Clear()
    {
        G_skillInfoCache.Clear();
        G_bufferInfoCache.Clear();
    }

    public static RealTimeBufferInfo GetBufferData(string name)
    {
        RealTimeBufferInfo component = null;
        if (string.IsNullOrEmpty(name))
        {
            name = "blank";
        }
        if (!G_bufferInfoCache.TryGetValue(name, out component))
        {
            GameObject obj2 = BundleMgr.Instance.LoadResource("Skills/" + name, ".prefab", typeof(GameObject)) as GameObject;
            if (obj2 != null)
            {
                component = obj2.GetComponent<RealTimeBufferInfo>();
                G_bufferInfoCache.Add(name, component);
                return component;
            }
            Debug.LogWarning("Can't find buffer " + name);
        }
        return component;
    }

    public static RealTimeSkillInfo GetSkillData(string effectName)
    {
        RealTimeSkillInfo component = null;
        if (string.IsNullOrEmpty(effectName))
        {
            return null;
        }
        if (!G_skillInfoCache.TryGetValue(effectName, out component))
        {
            GameObject obj2 = BundleMgr.Instance.LoadResource("Skills/" + effectName, ".prefab", typeof(GameObject)) as GameObject;
            if (obj2 == null)
            {
                Debug.LogWarning("Can't find skill " + effectName);
                obj2 = BundleMgr.Instance.LoadResource("Skills/test_addBuffer", ".prefab", typeof(GameObject)) as GameObject;
            }
            if (obj2 != null)
            {
                component = obj2.GetComponent<RealTimeSkillInfo>();
                G_skillInfoCache.Add(effectName, component);
            }
        }
        return component;
    }
}

