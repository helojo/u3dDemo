using System;

public class AssetBundleInfo
{
    public string AssetName;
    public string BundleURL;
    public int VersionNumber;

    public AssetBundleInfo(string bundleURL, string assetName, int versionNumber)
    {
        this.BundleURL = bundleURL;
        this.AssetName = assetName;
        this.VersionNumber = versionNumber;
    }

    public string BundleName()
    {
        return string.Format("{0} : {1}", this.VersionNumber.ToString().Trim(), this.BundleURL);
    }

    public static string BundleName(int versionNumber, string bundleURL)
    {
        return string.Format("{0} : {1}", versionNumber.ToString().Trim(), bundleURL);
    }
}

