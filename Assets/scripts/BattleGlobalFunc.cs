using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleGlobalFunc
{
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache2;
    private static string[] animNameList = new string[] { "none", "attack_jinzhan", "attack_yuancheng", "attack_fashu", "fangyu", "attack_jinzhan2", "attack_yuancheng2", "attack_fashu2", "attack_zhanqianji", "attack_zhanqianji1", "attack_zhanqianji2", "skill01", "skill02", "skill03", "attack_zhanqianji3", "skill04" };
    public static System.Action OnSceneStart;

    public static bool AttachChildToBindPoint(GameObject ParentGameObj, string BindName, GameObject ChildGameObj)
    {
        if (((ParentGameObj != null) && (BindName != null)) && (ChildGameObj != null))
        {
            GameObject obj2 = DeepFindChildObjectByName(ParentGameObj, BindName);
            if (obj2 != null)
            {
                Quaternion localRotation = ChildGameObj.transform.localRotation;
                Vector3 localScale = ChildGameObj.transform.localScale;
                ChildGameObj.transform.parent = obj2.transform;
                ChildGameObj.transform.localPosition = Vector3.zero;
                ChildGameObj.transform.localRotation = localRotation;
                ChildGameObj.transform.localScale = localScale;
                return true;
            }
            if (BindName != "Top")
            {
                Debug.LogWarning("can't find the bindPoint " + BindName + " in " + ParentGameObj.name);
            }
        }
        return false;
    }

    public static GameObject AttachEffectToScreen(GameObject prefab, float delayTime, float durtTime)
    {
        GameObject obj2 = ObjectManager.CreateTempObj(ObjectManager.InstantiateObj(prefab), (Vector3) (Vector3.one * -10000f), durtTime, delayTime);
        GameObject obj4 = DeepFindChildObjectByName(Camera.main.gameObject, "pingmu_Tx");
        if (obj4 != null)
        {
            obj2.transform.parent = obj4.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localRotation = Quaternion.identity;
        }
        return obj2;
    }

    public static void ClampUIPosToScreen(ref Vector3 pos, Vector3 size)
    {
        float num = UIRoot.list[0].activeHeight * 0.5f;
        float num2 = (((float) Screen.width) / ((float) Screen.height)) * num;
        pos.x = Mathf.Clamp(pos.x, -num2 + (size.x * 0.5f), num2 - (size.x * 0.5f));
        pos.y = Mathf.Clamp(pos.y, -num + (size.y * 0.5f), num - (size.y * 0.5f));
    }

    public static void ClearExpensiveSceneObj()
    {
        if (!SettingMgr.mInstance.IsEffectEnable)
        {
            List<GameObject> list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Expensive"));
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = obj => UnityEngine.Object.DestroyObject(obj);
            }
            list.ForEach(<>f__am$cache2);
        }
    }

    public static CombatDetailActor CreateDetailInfoByCard(int cardEntry)
    {
        CombatDetailActor actor = CommonFunc.GetCardDtailValue(cardEntry, 1, 3);
        actor.entry = (short) cardEntry;
        actor.isCard = true;
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config == null)
        {
            Debug.LogWarning("Can't fint cardConfig " + cardEntry.ToString());
        }
        actor.equip = new CombatEquip();
        actor.equip.entry = (short) _config.equip_part_0;
        actor.normalSkills = StrParser.ParseDecIntList(_config.normal_skill, -1);
        actor.activeSkill = _config.active_skill;
        actor.foremostSkill = _config.foremost_skill;
        actor.prewarSkill = _config.prewar_skill;
        actor.backupSkill = _config.backup_skill;
        actor.canMove = _config.can_move;
        actor.hate = _config.hate;
        actor.radius = _config.radius;
        actor.critDmg = _config.crit_dmg;
        actor.hitRate = _config.hit_rate;
        actor.energy = _config.energy;
        actor.energyRecoverOnAttack = _config.energy_recover_on_attack;
        actor.energyRemain = _config.energy_remain;
        actor.energyRecover = _config.energy_recover;
        actor.attackSpeed = _config.attack_speed;
        actor.defaultSkill = _config.default_normal_skill;
        actor.dodgeRate = 0;
        actor.physicsDmgReduce = _config.physics_dmg_reduce;
        actor.spellDmgReduce = _config.spell_dmg_reduce;
        return actor;
    }

    public static CombatDetailActor CreateDetailInfoOnMonster(int monsterEntry)
    {
        CombatDetailActor actor = new CombatDetailActor {
            entry = (short) monsterEntry,
            isCard = false
        };
        monster_config _config = ConfigMgr.getInstance().getByEntry<monster_config>(monsterEntry);
        if (_config == null)
        {
            Debug.LogWarning("Can't fint monster_config " + monsterEntry.ToString());
        }
        actor.normalSkills = StrParser.ParseDecIntList(_config.normal_skill, -1);
        actor.activeSkill = _config.active_skill;
        actor.foremostSkill = _config.foremost_skill;
        actor.prewarSkill = _config.prewar_skill;
        actor.backupSkill = _config.backup_skill;
        actor.curHp = (ulong) _config.hp;
        actor.maxHp = (ulong) _config.hp;
        actor.canMove = _config.can_move;
        actor.hate = _config.hate;
        actor.attackSpeed = _config.attack_speed;
        actor.attack = _config.attack;
        actor.tenacity = _config.tenacity;
        actor.critDmg = _config.crit_dmg;
        actor.dodgeRate = _config.dod_rate;
        actor.hitRate = _config.hit_rate;
        actor.critRate = _config.crit_rate;
        actor.spellDefence = _config.spell_defence;
        actor.physicsDefence = _config.physics_defence;
        actor.spellPierce = _config.spell_pierce;
        actor.physicsPierce = _config.physics_pierce;
        actor.energy = _config.energy;
        actor.energyRecoverOnAttack = _config.energy_recover_on_attack;
        actor.energyRemain = _config.energy_remain;
        actor.energyRecover = _config.energy_recover;
        actor.defaultSkill = _config.default_normal_skill;
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(_config.card_entry);
        if (_config2 != null)
        {
            actor.radius = _config2.radius;
        }
        return actor;
    }

    public static GameObject DeepFindChildObjectByName(GameObject parent, string childName)
    {
        bool activeSelf = parent.activeSelf;
        if (!parent.activeSelf)
        {
            parent.SetActive(true);
        }
        foreach (Transform transform in parent.transform.GetComponentsInChildren<Transform>())
        {
            if (transform.name.ToLower() == childName.ToLower())
            {
                parent.SetActive(activeSelf);
                return transform.gameObject;
            }
        }
        parent.SetActive(activeSelf);
        return null;
    }

    public static GameObject FindChild(GameObject obj, string[] ChildNames)
    {
        GameObject gameObject = null;
        foreach (string str in ChildNames)
        {
            gameObject = obj.transform.FindChild(str).gameObject;
            obj = gameObject;
        }
        return gameObject;
    }

    public static GameObject FindChildObjectByName(GameObject parent, string childName)
    {
        Transform transform = parent.transform.FindChild(childName);
        if (transform != null)
        {
            return transform.gameObject;
        }
        return null;
    }

    public static Vector3 GetGlobalScale(Transform trans)
    {
        Vector3 localScale = trans.localScale;
        for (Transform transform = trans.parent; transform != null; transform = transform.parent)
        {
            localScale = Vector3.Scale(localScale, transform.localScale);
        }
        return localScale;
    }

    public static string GetSkillAnimName(SkillAnimType animType)
    {
        return animNameList[(int) animType];
    }

    public static Vector3 GUIToWorld(Vector3 uiPos)
    {
        if ((UICamera.currentCamera != null) && (Camera.main != null))
        {
            Vector3 position = UICamera.currentCamera.WorldToViewportPoint(uiPos);
            return Camera.main.ViewportToWorldPoint(position);
        }
        Debug.LogWarning("GUIToWorld  Camera is null");
        return Vector3.zero;
    }

    public static void PlaySkillAnim(GameObject MainObj, SkillAnimType animType, string animText, float PlayAnimSpeed, float startTime, bool isForceLoop)
    {
        if (((MainObj != null) && (MainObj.GetComponent<AnimFSM>() != null)) && ((animType != SkillAnimType.None) || !string.IsNullOrEmpty(animText)))
        {
            string animName = string.Empty;
            string newAnimName = "attack_default";
            if ((animType == SkillAnimType.FangYu) && SettingMgr.mInstance.IsEffectEnable)
            {
                MaterialFSM component = MainObj.GetComponent<MaterialFSM>();
                if (component != null)
                {
                    component.StartChangeMaterialTemp(MaterialFSM.MaterialBeHurt, BattleGlobal.ScaleTime(0.1f));
                }
            }
            animName = GetSkillAnimName(animType);
            if (!string.IsNullOrEmpty(animText))
            {
                animName = animText;
            }
            if (MainObj.GetComponent<AnimFSM>().IsHasAnim(animName))
            {
                MainObj.GetComponent<AnimFSM>().PlayAnim(animName, PlayAnimSpeed, startTime, isForceLoop);
            }
            else
            {
                MainObj.GetComponent<AnimFSM>().PlayAnim(newAnimName, PlayAnimSpeed, 0f, false);
            }
        }
    }

    public static void RandomPlaceObj(Vector3 centerPos, List<GameObject> objs, bool isLocal, float radius, float range)
    {
        <RandomPlaceObj>c__AnonStoreyED yed;
        yed = new <RandomPlaceObj>c__AnonStoreyED {
            radius = radius,
            isLocal = isLocal,
            randomRange = range - yed.radius,
            placedPoses = new List<Vector2>()
        };
        objs.ForEach(new Action<GameObject>(yed.<>m__60));
    }

    public static Vector3 ScreenToGUI(Vector3 pos)
    {
        int activeHeight = UIRoot.list[0].activeHeight;
        pos.x = ((pos.x - 0.5f) * (((float) Screen.width) / ((float) Screen.height))) * activeHeight;
        pos.y = (pos.y - 0.5f) * activeHeight;
        return pos;
    }

    public static void SetAnimSpeed(GameObject MainObj, SkillAnimType animType, string animText, float PlayAnimSpeed)
    {
        if ((MainObj != null) && ((animType != SkillAnimType.None) || !string.IsNullOrEmpty(animText)))
        {
            string newAnimName = string.Empty;
            newAnimName = GetSkillAnimName(animType);
            if (!string.IsNullOrEmpty(animText))
            {
                newAnimName = animText;
            }
            MainObj.GetComponent<AnimFSM>().SetAnimSpeed(newAnimName, PlayAnimSpeed);
        }
    }

    public static void SetObjShowTimeEnable(GameObject obj, bool enable)
    {
        if (obj != null)
        {
            foreach (Animation animation in obj.GetComponentsInChildren<Animation>())
            {
                IEnumerator enumerator = animation.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        AnimationState current = (AnimationState) enumerator.Current;
                        current.speed = !enable ? ((float) 1) : ((float) 0);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
            foreach (ParticleSystem system in obj.GetComponentsInChildren<ParticleSystem>())
            {
                if (enable)
                {
                    system.Pause();
                }
                else
                {
                    system.Play();
                }
            }
        }
    }

    public static void SetObjTag(GameObject obj, string name)
    {
        if (obj != null)
        {
            obj.tag = name;
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                SetObjTag(obj.transform.GetChild(i).gameObject, name);
            }
        }
    }

    public static void StopUITween<T>(GameObject go) where T: UITweener
    {
        foreach (T local in go.GetComponents<T>())
        {
            local.enabled = false;
        }
    }

    public static Vector3 WorldToGUI(Vector3 pos)
    {
        if (Camera.main == null)
        {
            Debug.LogWarning("WorldToGUI Main Camera is null");
            return new Vector3(-10000f, -10000f, -10000f);
        }
        Vector3 vector = Camera.main.WorldToViewportPoint(pos);
        if (vector.z < 0f)
        {
            return new Vector3(-10000f, -10000f, -10000f);
        }
        pos = ScreenToGUI(vector);
        return pos;
    }

    [CompilerGenerated]
    private sealed class <RandomPlaceObj>c__AnonStoreyED
    {
        internal bool isLocal;
        internal List<Vector2> placedPoses;
        internal float radius;
        internal float randomRange;

        internal void <>m__60(GameObject obj)
        {
            Vector2 zero = Vector2.zero;
            for (int i = 0; i < 10; i++)
            {
                zero.x = UnityEngine.Random.Range(-this.randomRange, this.randomRange);
                zero.y = UnityEngine.Random.Range(-this.randomRange, this.randomRange);
                bool flag = true;
                foreach (Vector2 vector2 in this.placedPoses)
                {
                    if (Vector2.Distance(vector2, zero) < this.radius)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            this.placedPoses.Add(zero);
            if (this.isLocal)
            {
                obj.transform.localPosition = new Vector3(zero.x, obj.transform.localPosition.y, zero.y);
            }
            else
            {
                obj.transform.position = new Vector3(zero.x, obj.transform.position.y, zero.y);
            }
        }
    }
}

