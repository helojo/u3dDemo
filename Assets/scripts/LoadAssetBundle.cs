using HutongGames.PlayMaker;
using System;

[ActionCategory("Asset Bundle"), Tooltip("Loads one asset bundle")]
public class LoadAssetBundle : FsmStateAction
{
    private ManagerAssetBundle asset;
    [RequiredField, Tooltip("Asset Name")]
    public FsmString AssetName;
    [RequiredField, Tooltip("Asset Bundle Manager Prefab")]
    public FsmGameObject BundleManager;
    [RequiredField, Tooltip("URL of asset bundle - including .unity3d extension")]
    public FsmString BundleURL;
    private bool FinishedBundle;
    private bool IsDownload;
    [RequiredField, Tooltip("Bundle version number")]
    public FsmInt VersionNumber;

    private void DoBundleDownload()
    {
        if (((this.BundleURL != null) && (this.AssetName != null)) && ((this.VersionNumber != null) && (this.BundleManager != null)))
        {
            if (this.asset == null)
            {
                this.asset = this.BundleManager.Value.GetComponent<ManagerAssetBundle>();
                this.asset.QBundleForDownload(this.BundleURL.Value, this.AssetName.Value, this.VersionNumber.Value);
            }
            if (this.asset.IsDownloadFinished)
            {
                this.IsDownload = true;
            }
        }
    }

    public override void OnUpdate()
    {
        if (!this.FinishedBundle && !this.IsDownload)
        {
            this.DoBundleDownload();
        }
        if (!this.FinishedBundle && this.IsDownload)
        {
            this.FinishedBundle = true;
            base.Finish();
        }
    }

    public override void Reset()
    {
        this.BundleManager = null;
        this.BundleURL = null;
        this.AssetName = null;
        this.VersionNumber = null;
    }
}

