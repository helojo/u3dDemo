using HutongGames.PlayMaker;
using System;

[Tooltip("Loads multiple asset bundles"), ActionCategory("Asset Bundle")]
public class LoadAssetBundleList : FsmStateAction
{
    private ManagerAssetBundle asset;
    [Tooltip("Name of the assets to load from the asset bundle"), RequiredField]
    public FsmString[] AssetNames;
    [RequiredField, Tooltip("Asset Bundle Manager Prefab")]
    public FsmGameObject BundleManager;
    [RequiredField, Tooltip("URL of asset bundle - including .unity3d extension")]
    public FsmString BundleURL;
    private bool FinishedBundle;
    private bool IsDownload;
    [Tooltip("Bundle version number"), RequiredField]
    public FsmInt VersionNumber;

    private void DoBundleDownload()
    {
        if (((this.BundleURL != null) && (this.AssetNames != null)) && (this.VersionNumber != null))
        {
            if (this.asset == null)
            {
                this.asset = this.BundleManager.Value.GetComponent<ManagerAssetBundle>();
                int length = this.AssetNames.Length;
                for (int i = 0; i < length; i++)
                {
                    this.asset.QBundleForDownload(this.BundleURL.Value, this.AssetNames[i].Value, this.VersionNumber.Value);
                }
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
        this.AssetNames = null;
        this.VersionNumber = null;
    }
}

