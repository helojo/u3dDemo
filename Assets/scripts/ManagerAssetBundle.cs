using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ManagerAssetBundle : MonoBehaviour
{
    private readonly Dictionary<string, AssetBundle> AssetBundleDict = new Dictionary<string, AssetBundle>();
    private readonly Queue<AssetBundleInfo> BundleQ = new Queue<AssetBundleInfo>();
    private bool IsCoroutineRunning;

    [DebuggerHidden]
    private IEnumerator DownloadAndCache()
    {
        return new <DownloadAndCache>c__Iterator3 { <>f__this = this };
    }

    public void QBundleForDownload(string BundleURL, string AssetName, int VersionNumber)
    {
        this.BundleQ.Enqueue(new AssetBundleInfo(BundleURL, AssetName, VersionNumber));
        if (!this.IsCoroutineRunning)
        {
            base.StartCoroutine(this.DownloadAndCache());
        }
    }

    public void UnloadBundle(int versionNumber, string bundleURL)
    {
        AssetBundle bundle;
        string key = AssetBundleInfo.BundleName(versionNumber, bundleURL);
        if (this.AssetBundleDict.TryGetValue(key, out bundle))
        {
            bundle.Unload(true);
            this.AssetBundleDict.Remove(key);
        }
    }

    public void UnloadBundleCompressedContents(int versionNumber, string bundleURL)
    {
        AssetBundle bundle;
        string key = AssetBundleInfo.BundleName(versionNumber, bundleURL);
        if (this.AssetBundleDict.TryGetValue(key, out bundle))
        {
            bundle.Unload(false);
            this.AssetBundleDict.Remove(key);
        }
    }

    public bool IsDownloadFinished { get; private set; }

    [CompilerGenerated]
    private sealed class <DownloadAndCache>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ManagerAssetBundle <>f__this;
        internal AssetBundle <bundle>__1;
        internal AssetBundleInfo <BundleItem>__0;
        internal WWW <www>__2;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 2:
                    try
                    {
                    }
                    finally
                    {
                        if (this.<www>__2 != null)
                        {
                            this.<www>__2.Dispose();
                        }
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    if (!this.<>f__this.IsCoroutineRunning)
                    {
                        this.<>f__this.IsCoroutineRunning = true;
                        this.<>f__this.IsDownloadFinished = false;
                        goto Label_01D0;
                    }
                    goto Label_0205;

                case 1:
                    break;

                case 2:
                    goto Label_00DB;

                default:
                    goto Label_0205;
            }
        Label_0071:
            if (!Caching.ready)
            {
                this.$current = null;
                this.$PC = 1;
                goto Label_0207;
            }
            this.<BundleItem>__0 = this.<>f__this.BundleQ.Dequeue();
            if (this.<>f__this.AssetBundleDict.TryGetValue(this.<BundleItem>__0.BundleName(), out this.<bundle>__1))
            {
                goto Label_0189;
            }
            this.<www>__2 = WWW.LoadFromCacheOrDownload(this.<BundleItem>__0.BundleURL, this.<BundleItem>__0.VersionNumber);
            num = 0xfffffffd;
        Label_00DB:
            try
            {
                switch (num)
                {
                    case 2:
                        if (this.<www>__2.error != null)
                        {
                            throw new Exception("WWW download had an error:" + this.<www>__2.error);
                        }
                        break;

                    default:
                        this.$current = this.<www>__2;
                        this.$PC = 2;
                        flag = true;
                        goto Label_0207;
                }
                this.<bundle>__1 = this.<www>__2.assetBundle;
                this.<>f__this.AssetBundleDict.Add(this.<BundleItem>__0.BundleName(), this.<bundle>__1);
                this.<www>__2.Dispose();
            }
            finally
            {
                if (!flag)
                {
                }
                if (this.<www>__2 != null)
                {
                    this.<www>__2.Dispose();
                }
            }
        Label_0189:
            if (string.IsNullOrEmpty(this.<BundleItem>__0.AssetName))
            {
                UnityEngine.Object.Instantiate(this.<bundle>__1.mainAsset);
            }
            else
            {
                UnityEngine.Object.Instantiate(this.<bundle>__1.Load(this.<BundleItem>__0.AssetName));
            }
        Label_01D0:
            if (this.<>f__this.BundleQ.Count > 0)
            {
                goto Label_0071;
            }
            this.<>f__this.IsCoroutineRunning = false;
            this.<>f__this.IsDownloadFinished = true;
            this.$PC = -1;
        Label_0205:
            return false;
        Label_0207:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

