using HutongGames.PlayMaker;
using System;

[ActionCategory("Asset Bundle"), Tooltip("Unloads one asset bundle")]
public class UnloadAssetBundle : FsmStateAction
{
    private ManagerAssetBundle asset;
    [RequiredField, Tooltip("Asset Bundle Manager Prefab")]
    public FsmGameObject BundleManager;
    [RequiredField, Tooltip("URL of asset bundle - including .unity3d extension")]
    public FsmString BundleURL;
    [RequiredField, Tooltip("Bundle version number")]
    public FsmInt VersionNumber;

    private void DoBundleUnload()
    {
        if (((this.BundleURL != null) && (this.VersionNumber != null)) && (this.BundleManager != null))
        {
            this.BundleManager.Value.GetComponent<ManagerAssetBundle>().UnloadBundle(this.VersionNumber.Value, this.BundleURL.Value);
        }
    }

    public override void OnUpdate()
    {
        this.DoBundleUnload();
        base.Finish();
    }

    public override void Reset()
    {
        this.BundleManager = null;
        this.BundleURL = null;
        this.VersionNumber = null;
    }
}

