using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardPlayer : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<GameObject> <>f__am$cache8;
    private int defaultWeapon;
    private bool isShowEquip;
    private CombatEquip mEquip;
    private List<GameObject> mEquipObjList;
    private CardPlayerStateType mState;

    public CardPlayer()
    {
        CombatEquip equip = new CombatEquip {
            entry = -1
        };
        this.mEquip = equip;
        this.mEquipObjList = new List<GameObject>();
        this.isShowEquip = true;
        this.defaultWeapon = -1;
    }

    private static void AttachWeapon(GameObject owner, GameObject weaponObj, List<GameObject> equipObjList, WeaponType weaponType, CardPlayerStateType state, bool isMajor)
    {
        string bindName = GetHangPointName(weaponType, isMajor, state, weaponObj.name);
        if (BattleGlobalFunc.AttachChildToBindPoint(owner, bindName, weaponObj))
        {
            equipObjList.Add(weaponObj);
        }
        else
        {
            UnityEngine.Object.DestroyObject(weaponObj);
        }
        string methodName = !isMajor ? "WeaponChange" : "MajorWeaponChange";
        owner.SendMessage(methodName, weaponObj);
    }

    public static void CacheCardResource(int cardEntry, int cardQuality)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config == null)
        {
            Debug.LogWarning("Can't find card config ID: " + cardEntry.ToString());
        }
        else
        {
            ModelWrap wrap = CardModelWrapDataManager.static_GetModelWrap(_config.model);
            string name = "Characters/" + _config.model;
            if (wrap != null)
            {
                CardModelWrap wrapByQuality = wrap.GetWrapByQuality(cardQuality);
                if ((wrapByQuality != null) && !string.IsNullOrEmpty(wrapByQuality.modelReplace))
                {
                    name = "Characters/" + wrapByQuality.modelReplace;
                }
            }
            ObjectManager.CacheObjResource(name);
        }
    }

    public void ChangeState(CardPlayerStateType _state)
    {
        if (this.mState != _state)
        {
            this.mState = _state;
            this.UnequipAll();
            this.EquipAll();
        }
        AnimFSM component = base.GetComponent<AnimFSM>();
        if (component != null)
        {
            component.SetStateTable((_state != CardPlayerStateType.Battle) ? "normal" : "battle");
        }
    }

    public static GameObject CreateCardPlayer(CardPlayerInitInfo info, CardPlayerStateType state)
    {
        return CreateCardPlayer(info, state, null);
    }

    public static GameObject CreateCardPlayer(CardPlayerInitInfo info, CardPlayerStateType state, Transform parent)
    {
        if (info != null)
        {
            return CreateCardPlayer(info.cardEntry, info.equip, state, parent, info.cardQuality);
        }
        return null;
    }

    public static GameObject CreateCardPlayer(int cardEntry, CombatEquip equip, CardPlayerStateType state, int cardQuality)
    {
        return CreateCardPlayer(cardEntry, equip, state, null, cardQuality);
    }

    public static GameObject CreateCardPlayer(int cardEntry, CombatEquip equip, CardPlayerStateType state, Transform parent, int cardQuality)
    {
        GameObject obj2 = CardPlayerPool.Instance().PopObj(cardEntry);
        if (obj2 == null)
        {
            obj2 = RealCreateCardPlayer(cardEntry, cardQuality);
        }
        if (obj2 != null)
        {
            iTween tween = obj2.GetComponent<iTween>();
            if (tween != null)
            {
                UnityEngine.Object.Destroy(tween);
            }
        }
        if (parent != null)
        {
            Vector3 localScale = obj2.transform.localScale;
            obj2.transform.parent = parent;
            obj2.transform.localScale = localScale;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.localRotation = Quaternion.identity;
        }
        CardPlayer component = obj2.GetComponent<CardPlayer>();
        component.ChangeState(state);
        obj2.GetComponent<HangControler>().ClearEffect();
        AnimFSM mfsm = obj2.GetComponent<AnimFSM>();
        mfsm.ResetSpeed();
        mfsm.ResetAnim();
        mfsm.PlayAnim((state != CardPlayerStateType.Normal) ? BattleGlobal.FightStandAnimName : BattleGlobal.StandAnimName, 1f, 0f, false);
        component.UpdateEquip(equip);
        return obj2;
    }

    public static GameObject CreateCardPlayerWithEquip(int cardEntry, List<EquipInfo> _equip, CardPlayerStateType state, int cardQuality)
    {
        CombatEquip equip = null;
        if ((_equip != null) && (_equip.Count > 0))
        {
            equip = new CombatEquip {
                entry = (short) _equip[0].entry,
                level = (short) _equip[0].lv,
                quality = (short) _equip[0].quality
            };
        }
        return CreateCardPlayer(cardEntry, equip, state, null, cardQuality);
    }

    public static void CreateEquip(int equipId, out GameObject major, out GameObject minor)
    {
        major = null;
        minor = null;
        if (equipId >= 0)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(equipId);
            if (_config == null)
            {
                Debug.LogWarning("Can't find Equip id: " + equipId.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(_config.model_major))
                {
                    major = ObjectManager.CreateObj("Weapon/" + _config.model_major);
                    major.name = _config.model_major;
                }
                if (!string.IsNullOrEmpty(_config.model_minor))
                {
                    minor = ObjectManager.CreateObj("Weapon/" + _config.model_minor);
                    minor.name = _config.model_minor;
                }
            }
        }
    }

    public static GameObject CreateNormalObj(string modelName)
    {
        string name = "Characters/" + modelName;
        GameObject obj2 = ObjectManager.CreateObj(name);
        if (obj2 == null)
        {
            Debug.LogWarning("Can't find card Model: " + name.ToString());
            return null;
        }
        obj2.name = modelName;
        obj2.AddComponent<AnimFSM>().PlayStandAnim();
        obj2.AddComponent<HangControler>();
        obj2.AddComponent<MaterialFSM>();
        return obj2;
    }

    public static void DestroyCardPlayer(GameObject _obj)
    {
        if (_obj != null)
        {
            BoxCollider component = _obj.GetComponent<BoxCollider>();
            if (component != null)
            {
                component.enabled = false;
            }
            ObjectManager.DestoryObj(_obj);
        }
    }

    private void EquipAll()
    {
        if (this.isShowEquip)
        {
            if (this.mEquip.entry >= 0)
            {
                this.EquipWeapon(this.mEquip.entry);
            }
            else
            {
                this.EquipWeapon(this.defaultWeapon);
            }
        }
    }

    private void EquipWeapon(int equipId)
    {
        if (equipId >= 0)
        {
            GameObject obj2;
            GameObject obj3;
            CreateEquip(equipId, out obj2, out obj3);
            if (obj2 != null)
            {
                AttachWeapon(base.gameObject, obj2, this.mEquipObjList, this.GetWeaponType(), this.mState, true);
            }
            if (obj3 != null)
            {
                AttachWeapon(base.gameObject, obj3, this.mEquipObjList, this.GetWeaponType(), this.mState, false);
            }
        }
    }

    private static string GetHangPointName(WeaponType _weaponType, bool isMajorHand, CardPlayerStateType _state, string weaponName)
    {
        string str = CardModelWrapDataManager.static_GetWeaponHangPoint(weaponName, _state == CardPlayerStateType.Normal);
        if (str != null)
        {
            return str;
        }
        if (_state == CardPlayerStateType.Battle)
        {
            if (_weaponType != WeaponType.gong)
            {
                if ((_weaponType == WeaponType.nu) || (_weaponType == WeaponType.qiang))
                {
                    return HangPointType.RHand.ToString();
                }
                if (isMajorHand)
                {
                    return HangPointType.RHand.ToString();
                }
                if (_weaponType == WeaponType.jiandan)
                {
                    return HangPointType.LHand_Dun.ToString();
                }
            }
            return HangPointType.LHand.ToString();
        }
        if (((_weaponType == WeaponType.gong) || (_weaponType == WeaponType.qiang)) || ((_weaponType == WeaponType.nu) || (_weaponType == WeaponType.fazhang)))
        {
            return HangPointType.Back.ToString();
        }
        if (isMajorHand)
        {
            return HangPointType.Back_R.ToString();
        }
        if (_weaponType == WeaponType.jiandan)
        {
            return HangPointType.Back.ToString();
        }
        return HangPointType.Back_L.ToString();
    }

    public WeaponType GetWeaponType()
    {
        int defaultWeapon = this.defaultWeapon;
        if ((this.mEquip != null) && (this.mEquip.entry >= 0))
        {
            defaultWeapon = this.mEquip.entry;
        }
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(defaultWeapon);
        if (_config != null)
        {
            return (WeaponType) _config.sub_type;
        }
        return WeaponType.none;
    }

    private void InitEquip(CombatEquip equip)
    {
        if (equip != null)
        {
            this.mEquip = equip;
        }
    }

    public static GameObject RealCreateCardPlayer(int cardEntry, int cardQuality)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config == null)
        {
            Debug.LogWarning("Can't find card config ID: " + cardEntry.ToString());
            return null;
        }
        ModelWrap wrap = CardModelWrapDataManager.static_GetModelWrap(_config.model);
        string name = "Characters/" + _config.model;
        if (wrap != null)
        {
            CardModelWrap wrapByQuality = wrap.GetWrapByQuality(cardQuality);
            if ((wrapByQuality != null) && !string.IsNullOrEmpty(wrapByQuality.modelReplace))
            {
                name = "Characters/" + wrapByQuality.modelReplace;
            }
        }
        GameObject target = ObjectManager.CreateObj(name);
        if (target == null)
        {
            target = ObjectManager.CreateObj("Characters/Nan01_mt");
            Debug.LogWarning("Can't find card Model: " + name.ToString());
        }
        target.name = _config.model;
        target.AddComponent<AnimFSM>();
        CardPlayer player = target.AddComponent<CardPlayer>();
        player.cardID = cardEntry;
        player.isShowEquip = _config.weapon_show_type == 1;
        if (player.isShowEquip)
        {
            player.defaultWeapon = _config.equip_part_0;
        }
        CardModelWrapDataManager.static_ChangeModel(target, _config.model, cardQuality);
        if (wrap != null)
        {
            player.baseScale = wrap.BaseScale;
            player.baseLength = wrap.BaseLength;
        }
        else
        {
            player.baseScale = 1f;
            player.baseLength = 1f;
        }
        target.AddComponent<MaterialFSM>();
        target.AddComponent<HangControler>();
        player.ResetCardPlayer();
        return target;
    }

    public void ResetCardPlayer()
    {
        base.gameObject.transform.parent = null;
        base.gameObject.transform.rotation = Quaternion.identity;
        base.gameObject.transform.localScale = new Vector3(this.baseScale, this.baseScale, this.baseScale);
        base.gameObject.SendMessage("ResetSpeed");
        base.gameObject.SendMessage("ResetMaterial");
        MTDLayers.SetlayerRecursively(base.gameObject, MTDLayers.DynamicLighting);
    }

    public void UnequipAll()
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = obj => UnityEngine.Object.DestroyObject(obj);
        }
        this.mEquipObjList.ForEach(<>f__am$cache8);
        this.mEquipObjList.Clear();
    }

    public void UpdateEquip(CombatEquip equip)
    {
        this.UnequipAll();
        this.InitEquip(equip);
        this.EquipAll();
    }

    public float baseLength { get; set; }

    public float baseScale { get; set; }

    public int cardID { get; set; }

    public class CardPlayerInitInfo
    {
        public int cardEntry;
        public int cardQuality;
        public CombatEquip equip;
    }
}

