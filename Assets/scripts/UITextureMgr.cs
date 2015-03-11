using System;
using System.Collections.Generic;
using UnityEngine;

public class UITextureMgr : MonoBehaviour
{
    private Dictionary<string, UITextureBlock> dicTextures = new Dictionary<string, UITextureBlock>();
    public static UITextureMgr Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        float time = Time.time;
        List<string> list = new List<string>();
        foreach (KeyValuePair<string, UITextureBlock> pair in this.dicTextures)
        {
            UITextureBlock block = pair.Value;
            if ((block.refCount <= 0) && ((time - block.time) >= 5f))
            {
                if ((null != block.material) && (null != block.material.mainTexture))
                {
                    block.material.mainTexture = null;
                    block.material = null;
                }
                if (null != block.texture)
                {
                    Resources.UnloadAsset(block.texture);
                    block.texture = null;
                }
                list.Add(pair.Key);
            }
        }
        foreach (string str in list)
        {
            this.dicTextures.Remove(str);
        }
    }

    public Texture LoadTexture(string key, string tex_n)
    {
        if (this.dicTextures.ContainsKey(key))
        {
            UITextureBlock block = this.dicTextures[key];
            block.refCount++;
            return block.texture;
        }
        AssetBundle bundle = AssetBundle.CreateFromFile(tex_n);
        if (null == bundle)
        {
            Debug.Log("failed to load texture bundle named" + tex_n);
            return null;
        }
        Texture mainAsset = bundle.mainAsset as Texture;
        if (null == mainAsset)
        {
            Debug.Log("failed to load texture bundle named" + tex_n);
            return null;
        }
        bundle.Unload(false);
        UITextureBlock block2 = new UITextureBlock {
            texture = mainAsset,
            refCount = 1
        };
        this.dicTextures.Add(key, block2);
        return mainAsset;
    }

    public void ManageredMaterial(Material mat, string tex_key)
    {
        if ((null != mat) && this.dicTextures.ContainsKey(tex_key))
        {
            UITextureBlock block = this.dicTextures[tex_key];
            block.material = mat;
            block.material.mainTexture = block.texture;
        }
    }

    private void OnDestroy()
    {
    }

    public void ReleaseTexture(string key, int count)
    {
        if (this.dicTextures.ContainsKey(key))
        {
            UITextureBlock block = this.dicTextures[key];
            block.refCount = Mathf.Max(block.refCount - count, 0);
            if (block.refCount <= 0)
            {
                block.time = Time.time;
            }
        }
    }
}

