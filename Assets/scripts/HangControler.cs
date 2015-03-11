using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class HangControler : MonoBehaviour
{
    private Dictionary<string, GameObject> attachEffects = new Dictionary<string, GameObject>();
    private Dictionary<HangPointType, GameObject> findedPoints = new Dictionary<HangPointType, GameObject>();
    private GameObject majorWeaponObj;
    private GameObject weaponObj;

    public GameObject AttachByPrefab(GameObject prefab, HangPointType point, float durTime, float delayTime, Vector3 offset, bool needSetNoShaderChange, bool remainActive = false)
    {
        if (prefab == null)
        {
            return null;
        }
        GameObject obj2 = ObjectManager.InstantiateObj(prefab);
        if (obj2 == null)
        {
            Debug.LogWarning("Can't find " + prefab.name);
            return null;
        }
        if (needSetNoShaderChange)
        {
            BattleGlobalFunc.SetObjTag(obj2, BattleGlobal.NoShaderChangeTag);
        }
        ObjectManager.CreateTempObj(obj2, Vector3.zero, durTime, delayTime, remainActive);
        this.AttachToHangPoint(obj2, point, offset);
        return obj2;
    }

    public void AttachEffect(string resourceName, string name, HangPointType point)
    {
        GameObject obj2 = ObjectManager.CreateObj(resourceName);
        if (obj2 != null)
        {
            this.AttachToHangPoint(obj2, point, Vector3.zero);
            this.attachEffects.Add(name, obj2);
        }
    }

    public GameObject AttachEffect(string resourceName, string name, HangPointType point, Vector3 offset)
    {
        GameObject obj2 = ObjectManager.CreateObj(resourceName);
        if (obj2 != null)
        {
            this.AttachToHangPoint(obj2, point, offset);
            this.attachEffects.Add(name, obj2);
        }
        return obj2;
    }

    public void AttachToHangPoint(GameObject obj, HangPointType point, Vector3 offset)
    {
        GameObject hangPointObj = this.GetHangPointObj(point);
        if (hangPointObj != null)
        {
            Quaternion localRotation = obj.transform.localRotation;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.parent = hangPointObj.transform;
            if (point == HangPointType.Head)
            {
                obj.transform.localPosition = Vector3.zero;
            }
            else
            {
                obj.transform.localPosition = offset;
            }
            obj.transform.localRotation = localRotation;
        }
    }

    public void ClearEffect()
    {
        foreach (GameObject obj2 in this.attachEffects.Values)
        {
            ObjectManager.DestoryObj(obj2);
        }
        this.attachEffects.Clear();
    }

    public void DetachEffect(string name, HangPointType point, float time)
    {
        GameObject obj2;
        if (this.attachEffects.TryGetValue(name, out obj2))
        {
            foreach (ParticleSystem system in obj2.GetComponentsInChildren<ParticleSystem>())
            {
                system.Stop();
            }
            ObjectManager.DestoryObj(obj2, time);
            this.attachEffects.Remove(name);
        }
    }

    public GameObject GetHangPointObj(HangPointType point)
    {
        GameObject gameObject = null;
        if (!this.findedPoints.TryGetValue(point, out gameObject))
        {
            HangPointType type = point;
            if (type == HangPointType.WeaponAndRHand)
            {
                if (this.majorWeaponObj != null)
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(this.majorWeaponObj, "point01");
                }
                else
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, "point01");
                }
                if (gameObject == null)
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, HangPointType.RHand.ToString());
                }
            }
            else if (type == HangPointType.WeaponAndLHand)
            {
                if (this.weaponObj != null)
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(this.weaponObj, "point01");
                }
                if (gameObject == null)
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, HangPointType.LHand.ToString());
                }
            }
            else if (type == HangPointType.Head)
            {
                gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, "Head");
                if (gameObject == null)
                {
                    gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, "Top");
                }
            }
            else
            {
                gameObject = BattleGlobalFunc.DeepFindChildObjectByName(base.gameObject, point.ToString());
            }
            if (gameObject == null)
            {
                gameObject = base.gameObject;
                Debug.LogWarning(base.gameObject.name + " have not the point " + point.ToString());
            }
            this.findedPoints.Add(point, gameObject);
        }
        return gameObject;
    }

    public Vector3 GetHangPointPos(HangPointType point)
    {
        GameObject hangPointObj = this.GetHangPointObj(point);
        if (hangPointObj != null)
        {
            return hangPointObj.transform.position;
        }
        return base.gameObject.transform.position;
    }

    private void MajorWeaponChange(GameObject weapon)
    {
        this.majorWeaponObj = weapon;
    }

    public void PlaceEffect(string resourceName, HangPointType point)
    {
        ObjectManager.CreateObj(resourceName).transform.position = this.GetHangPointPos(point);
    }

    public GameObject PlaceEffectByPrefab(GameObject prefab, HangPointType point, float durTime, float delayTime, Vector3 offset)
    {
        GameObject obj2 = ObjectManager.InstantiateObj(prefab);
        ObjectManager.CreateTempObj(obj2, this.GetHangPointPos(point), durTime, delayTime);
        if (point != HangPointType.Head)
        {
            Vector3 vector = base.gameObject.transform.TransformDirection(offset.normalized);
            Transform transform = obj2.transform;
            transform.position += (Vector3) (vector * offset.magnitude);
        }
        obj2.transform.rotation = base.gameObject.transform.rotation;
        return obj2;
    }

    private void WeaponChange(GameObject weapon)
    {
        this.weaponObj = weapon;
    }
}

