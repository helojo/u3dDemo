using HutongGames.PlayMaker;
using System;

[Tooltip("Unloads the compressed contents of one asset bundle, to save memory. NOTE: Once you unload the compressed contents, you cannot load any new assets from This bundle"), ActionCategory("Asset Bundle")]
public class UnloadAssetBundleCompressedContents : FsmStateAction
{
    private ManagerAssetBundle asset;
    [RequiredField, Tooltip("Asset Bundle Manager Prefab")]
    public FsmGameObject BundleManager;
    [Tooltip("URL of asset bundle - including .unity3d extension"), RequiredField]
    public FsmString BundleURL;
    [RequiredField, Tooltip("Bundle version number")]
    public FsmInt VersionNumber;

    private void DoBundleUnload()
    {
        if (((this.BundleURL != null) && (this.VersionNumber != null)) && (this.BundleManager != null))
        {
            this.BundleManager.Value.GetComponent<ManagerAssetBundle>().UnloadBundleCompressedContents(this.VersionNumber.Value, this.BundleURL.Value);
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

