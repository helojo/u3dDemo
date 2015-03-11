using fastJSON;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardModelWrapDataManager : MonoBehaviour
{
    private static CardModelWrapDataManager _instance;
    private Dictionary<string, ModelWrap> mModelData = new Dictionary<string, ModelWrap>();
    private Dictionary<string, string> weaponHangPointBattle = new Dictionary<string, string>();
    private Dictionary<string, string> weaponHangPointNormal = new Dictionary<string, string>();

    public void ChangeModel(GameObject target, string modelName, int cardQuality)
    {
        ModelWrap baseModelWrap = this.GetBaseModelWrap(modelName);
        if (baseModelWrap != null)
        {
            CardModelWrap wrapByQuality = baseModelWrap.GetWrapByQuality(cardQuality);
            if (wrapByQuality != null)
            {
                SkinnedMeshRenderer componentInChildren = target.GetComponentInChildren<SkinnedMeshRenderer>();
                if ((componentInChildren != null) && !string.IsNullOrEmpty(wrapByQuality.textureName))
                {
                    Texture texture = BundleMgr.Instance.LoadResource("CardModelWrap/Textures/" + wrapByQuality.textureName, ".png", typeof(Texture)) as Texture;
                    componentInChildren.material.mainTexture = texture;
                }
                foreach (CardModelWrap.CardModelHang hang in wrapByQuality.Hangobj)
                {
                    if (!string.IsNullOrEmpty(hang.objName))
                    {
                        UnityEngine.Object original = BundleMgr.Instance.LoadResource("CardModelWrap/Prefabs/" + hang.objName, ".prefab", typeof(GameObject));
                        if (original != null)
                        {
                            GameObject childGameObj = UnityEngine.Object.Instantiate(original) as GameObject;
                            BattleGlobalFunc.AttachChildToBindPoint(target, hang.hangPoint.ToString(), childGameObj);
                        }
                    }
                }
            }
        }
    }

    public ModelWrap GetBaseModelWrap(string modelName)
    {
        ModelWrap wrap = null;
        if (this.mModelData.TryGetValue(modelName, out wrap))
        {
            return wrap;
        }
        return null;
    }

    private string GetWeaponHangPoint(string weaponName, bool isNormal)
    {
        string str = null;
        Dictionary<string, string> dictionary = !isNormal ? this.weaponHangPointBattle : this.weaponHangPointNormal;
        if (dictionary.TryGetValue(weaponName, out str))
        {
            return str;
        }
        return null;
    }

    private void initData(CardModelWrapGlobalMB data)
    {
        data.mSerializedModelData.ForEach(obj => this.mModelData.Add(obj.ModelName, obj));
    }

    private void InitWeaponHangPointData()
    {
        TextAsset asset = BundleMgr.Instance.LoadResource("CardModelWrap/weapon_hangpoint", ".txt", typeof(TextAsset)) as TextAsset;
        Dictionary<string, object> dictionary = JSON.Instance.ToObject<Dictionary<string, object>>(asset.text);
        Dictionary<string, object> dictionary2 = (Dictionary<string, object>) dictionary["normal"];
        if (dictionary2 != null)
        {
            foreach (KeyValuePair<string, object> pair in dictionary2)
            {
                this.weaponHangPointNormal.Add(pair.Key, pair.Value.ToString());
            }
        }
        Dictionary<string, object> dictionary3 = (Dictionary<string, object>) dictionary["battle"];
        if (dictionary3 != null)
        {
            foreach (KeyValuePair<string, object> pair2 in dictionary3)
            {
                this.weaponHangPointBattle.Add(pair2.Key, pair2.Value.ToString());
            }
        }
    }

    private static CardModelWrapDataManager Instance()
    {
        if (_instance == null)
        {
            StaticInit();
        }
        return _instance;
    }

    public static void static_ChangeModel(GameObject target, string modelName, int cardQuality)
    {
        if (CardModelWrapManager.Instance() != null)
        {
            CardModelWrapManager.Instance().ChangeModel(target, modelName, cardQuality);
        }
        else
        {
            Instance().ChangeModel(target, modelName, cardQuality);
        }
    }

    public static ModelWrap static_GetModelWrap(string name)
    {
        ModelWrap wrap = null;
        if (CardModelWrapManager.Instance() != null)
        {
            CardModelWrapManager.Instance().mModelData.TryGetValue(name, out wrap);
            return wrap;
        }
        Instance().TryGetModelData(name, out wrap);
        return wrap;
    }

    public static string static_GetWeaponHangPoint(string weaponName, bool isNormal)
    {
        return Instance().GetWeaponHangPoint(weaponName, isNormal);
    }

    private static void StaticInit()
    {
        GameObject obj2 = ObjectManager.CreateObj("CardModelWrap/GlobalData");
        CardModelWrapGlobalMB component = obj2.GetComponent<CardModelWrapGlobalMB>();
        GameObject target = new GameObject();
        UnityEngine.Object.DontDestroyOnLoad(target);
        _instance = target.AddComponent<CardModelWrapDataManager>();
        _instance.initData(component);
        _instance.InitWeaponHangPointData();
        UnityEngine.Object.DestroyObject(obj2);
    }

    private bool TryGetModelData(string name, out ModelWrap data)
    {
        return this.mModelData.TryGetValue(name, out data);
    }
}

