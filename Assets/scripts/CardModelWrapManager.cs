using System;
using System.Collections.Generic;
using UnityEngine;

public class CardModelWrapManager : MonoBehaviour
{
    private static CardModelWrapManager _instance;
    public Dictionary<string, ModelWrap> mModelData = new Dictionary<string, ModelWrap>();
    [SerializeField]
    public List<ModelWrap> mSerializedModelData = new List<ModelWrap>();

    private void Awake()
    {
        this.Load();
        _instance = this;
    }

    public void ChangeModel(GameObject target, string modelName, int cardQuality)
    {
        ModelWrap wrap;
        if (this.mModelData.TryGetValue(modelName, out wrap))
        {
            CardModelWrap wrapByQuality = wrap.GetWrapByQuality(cardQuality);
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

    public static CardModelWrapManager Instance()
    {
        return _instance;
    }

    public void Load()
    {
        this.mModelData.Clear();
        foreach (ModelWrap wrap in this.mSerializedModelData)
        {
            this.mModelData.Add(wrap.ModelName, wrap);
        }
    }

    public void Save()
    {
        this.mSerializedModelData.Clear();
        foreach (KeyValuePair<string, ModelWrap> pair in this.mModelData)
        {
            this.mSerializedModelData.Add(pair.Value);
        }
    }

    public static void StaticInit()
    {
        GameObject target = ObjectManager.CreateObj("Misc/CardModelWrap");
        target.name = "CardModelWrap";
        UnityEngine.Object.DontDestroyOnLoad(target);
        _instance = target.GetComponent<CardModelWrapManager>();
    }
}

