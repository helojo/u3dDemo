using System;
using System.Collections;
using UnityEngine;

public static class MTDLayers
{
    public static int BattleBossLayer = LayerMask.NameToLayer("BattleBossLighting");
    public static int BattleShowTime = LayerMask.NameToLayer("BattleShowTime");
    public static int CameraH = LayerMask.NameToLayer("CameraH");
    public static int CameraV = LayerMask.NameToLayer("CameraV");
    public static int Default = LayerMask.NameToLayer("Default");
    public static int DynamicLighting = LayerMask.NameToLayer("dongtai");
    public static int GUI = LayerMask.NameToLayer("GUI");
    public static int ListGUI = LayerMask.NameToLayer("ListGUI");
    public static int NoShadow = LayerMask.NameToLayer("NoShadow");
    public static int ThreeDCamera = LayerMask.NameToLayer("ThreeDCamera");
    public static int ThreeDGUI = LayerMask.NameToLayer("3DGUI");

    public static void SetlayerRecursively(GameObject go, int layer)
    {
        go.layer = layer;
        IEnumerator enumerator = go.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                SetlayerRecursively(current.gameObject, layer);
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
}

