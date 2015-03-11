using System;
using UnityEngine;

[Serializable]
public class ModelWrap
{
    public string animName;
    public float animTime;
    public float BaseLength = 1f;
    public float BaseScale = 1f;
    public float CameraOrthGSize = 1f;
    public string halfAnimName;
    public float halfAnimTime;
    public float halfCameraOrthSize = 1f;
    public float halfModelScale = 1f;
    public Vector3 halfViewPos;
    public float halfViewRotY;
    public bool isShowInEditor;
    public string ModelName;
    public float modelScale = 1f;
    public string sideAnimName;
    public float sideAnimTime;
    public float sideCameraOrthSize = 1f;
    public float sideModelScale = 1f;
    public Vector3 sideViewPos;
    public float sideViewRotY;
    public Vector3 viewPos;
    public float viewRotY;
    public CardModelWrap[] wraps;

    public CardModelWrap GetWrapByQuality(int quality)
    {
        this.TryInitWrap();
        if (quality < 0)
        {
            quality = 0;
        }
        return this.wraps[quality];
    }

    private void TryInitWrap()
    {
        if ((this.wraps == null) || (this.wraps.Length < CardModelWrapMB.CardQualityMax))
        {
            GameObject obj2 = BundleMgr.Instance.LoadResource(string.Format("CardModelWrap/wrap_{0}", this.ModelName), ".prefab", typeof(GameObject)) as GameObject;
            if (obj2 != null)
            {
                this.wraps = obj2.GetComponent<CardModelWrapMB>().wraps;
            }
            else
            {
                this.wraps = new CardModelWrap[CardModelWrapMB.CardQualityMax];
                for (int i = 0; i < CardModelWrapMB.CardQualityMax; i++)
                {
                    this.wraps[i] = new CardModelWrap();
                }
            }
        }
    }
}

