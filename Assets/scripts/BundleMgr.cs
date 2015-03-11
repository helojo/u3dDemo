using FastBuf;
using LevelLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BundleMgr : MonoBehaviour
{
    private HashSet<string> _bundlesManual;
    private static BundleMgr _Instance;
    private Dictionary<string, UnityEngine.Object> activitied_bundles = new Dictionary<string, UnityEngine.Object>();
    public bool completedDownload;
    private Dictionary<string, string[]> dependencies_manual = new Dictionary<string, string[]>();
    public string msgDownload = string.Empty;
    public static string patch_cleaner_key = "mtd_patch_clean";
    private static string path_of_archieve = string.Empty;
    public int progressDownload;
    private List<Operation> scenes_preloaders = new List<Operation>();
    public int totalDownload;

    private List<ExternalPak> AnalyzePakDLs(List<ExternalPak> lst)
    {
        List<ExternalPak> list = this.PakListDeserialization();
        List<ExternalPak> list2 = new List<ExternalPak>();
        <AnalyzePakDLs>c__AnonStorey287 storey = new <AnalyzePakDLs>c__AnonStorey287();
        using (List<ExternalPak>.Enumerator enumerator = lst.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                storey.info = enumerator.Current;
                ExternalPak pak = list.Find(new Predicate<ExternalPak>(storey.<>m__5D7));
                if ((pak == null) || (pak.md5 != storey.info.md5))
                {
                    list2.Add(storey.info);
                }
            }
        }
        return list2;
    }

    private List<ExternalPak> AnalyzePaksInfo(string info_fn)
    {
        List<ExternalPak> list = new List<ExternalPak>();
        string[] strArray = File.ReadAllLines(info_fn);
        for (int i = 0; i != strArray.Length; i++)
        {
            string str = strArray[i];
            if (!string.IsNullOrEmpty(str))
            {
                char[] separator = new char[] { '\t' };
                string[] strArray2 = str.Split(separator);
                if (strArray2.Length != 3)
                {
                    throw new UnityException("failed to analyze pak list infomation!");
                }
                ExternalPak item = new ExternalPak {
                    index = Convert.ToInt32(strArray2[0]),
                    md5 = strArray2[1],
                    addr = strArray2[2]
                };
                if (string.IsNullOrEmpty(item.md5))
                {
                    throw new UnityException("failed to analyze pak md5 infomation!");
                }
                if (string.IsNullOrEmpty(item.addr))
                {
                    throw new UnityException("failed to analyze pak download addres infomation!");
                }
                list.Add(item);
            }
        }
        return list;
    }

    [DebuggerHidden]
    private IEnumerator AsynCheckBundles()
    {
        return new <AsynCheckBundles>c__IteratorAD { <>f__this = this };
    }

    private int CalculateChunkCount(string patch_file)
    {
        if (!File.Exists(patch_file))
        {
            return 0;
        }
        int num = 0;
        FileStream serializationStream = new FileStream(patch_file, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string str = (string) formatter.Deserialize(serializationStream);
            string str2 = (string) formatter.Deserialize(serializationStream);
            num = (int) formatter.Deserialize(serializationStream);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
            return 0;
        }
        finally
        {
            serializationStream.Close();
        }
        return num;
    }

    public void CheckBundles()
    {
        this.totalDownload = 0;
        this.progressDownload = 0;
        this.msgDownload = string.Empty;
        this.completedDownload = false;
        base.StartCoroutine(this.AsynCheckBundles());
    }

    public void ClearCache()
    {
        this.activitied_bundles.Clear();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    private bool ComparePrimaryVersion(string l, string r)
    {
        char[] separator = new char[] { '.' };
        string[] strArray = l.Split(separator);
        char[] chArray2 = new char[] { '.' };
        string[] strArray2 = r.Split(chArray2);
        if ((strArray.Length < 3) || (strArray2.Length < 3))
        {
            return false;
        }
        for (int i = 0; i != 3; i++)
        {
            if (Convert.ToInt32(strArray[i]) != Convert.ToInt32(strArray2[i]))
            {
                return false;
            }
        }
        return true;
    }

    private bool CompareVerion(string l, string r)
    {
        char[] separator = new char[] { '.' };
        string[] strArray = l.Split(separator);
        char[] chArray2 = new char[] { '.' };
        string[] strArray2 = r.Split(chArray2);
        for (int i = 0; (i != strArray.Length) && (i != strArray2.Length); i++)
        {
            if (Convert.ToInt32(strArray[i]) != Convert.ToInt32(strArray2[i]))
            {
                return false;
            }
        }
        return true;
    }

    public Texture2D CreateActiveBackGround(string _Name)
    {
        return this.CreateTextureObject("GUI/Active/" + _Name);
    }

    public Texture2D CreateActiveSpeTypeIcom(string _rePath)
    {
        return this.CreateTextureObject("GUI/" + _rePath);
    }

    public Texture2D CreateBoxIcon(string headName)
    {
        return this.CreateTextureObject("GUI/Box/" + headName);
    }

    public Texture2D CreateBuildingText(string _Name)
    {
        return this.CreateTextureObject("GUI/BuildingText/" + _Name);
    }

    public Texture2D CreateCardQuality(string _Name)
    {
        return this.CreateTextureObject("GUI/CardQuality/" + _Name);
    }

    public GameObject CreateEffectObject(string res)
    {
        GameObject original = Instance.LoadResource(res, ".prefab", typeof(GameObject)) as GameObject;
        if (null == original)
        {
            return null;
        }
        return (UnityEngine.Object.Instantiate(original) as GameObject);
    }

    internal Texture CreateGuildDupIcon(string _Name)
    {
        return this.CreateTextureObject("GUI/GuildDupIcon/" + _Name);
    }

    public Texture2D CreateHeadIcon(string headName)
    {
        return this.CreateTextureObject("GUI/HeadIcon/" + headName);
    }

    public Texture2D CreateItemFromTexture(string iconName)
    {
        return this.CreateTextureObject("GUI/ItemFromIcon/" + iconName);
    }

    public Texture2D CreateItemIcon(string headName)
    {
        bool isHead = false;
        return this.CreateItemIcon(headName, out isHead);
    }

    public Texture2D CreateItemIcon(string headName, out bool isHead)
    {
        isHead = false;
        Texture2D textured = this.CreateTextureObject("GUI/ItemIcon/" + headName);
        if (null == textured)
        {
            isHead = true;
            return this.CreateHeadIcon(headName);
        }
        return textured;
    }

    public Texture2D CreateItemQuality(string _Name)
    {
        return this.CreateTextureObject("GUI/EquipQuality/" + _Name);
    }

    public Texture2D CreateLoadTexture(string res)
    {
        string str = "GUI/Loading/" + res;
        return (Texture2D) Instance.LoadResource(str, ".jpg", typeof(Texture2D));
    }

    public Texture2D CreateLoginTexture(string _Name)
    {
        return this.CreateTextureObject("GUI/Login/" + _Name);
    }

    public Texture2D CreateOutlandIcon(string headName)
    {
        return this.CreateTextureObject("GUI/OutLandIcon/" + headName);
    }

    internal Texture CreatePaokuIcon(string _Name)
    {
        return this.CreateTextureObject("GUI/PaokuIcon/" + _Name);
    }

    public Texture2D CreateShareIcon(string pictureName)
    {
        return this.CreateTextureObject("GUI/Share/" + pictureName);
    }

    public Texture2D CreateSkillIcon(string headName)
    {
        return this.CreateTextureObject("GUI/SkillIcon/" + headName);
    }

    public Texture2D CreateTextureObject(string res)
    {
        return (Texture2D) Instance.LoadResource(res, ".png", typeof(Texture2D));
    }

    public Texture2D CreateTitleIcon(string _Name)
    {
        return this.CreateTextureObject("GUI/TitleIcon/" + _Name);
    }

    public Texture2D CreateTrenchMapIcon(string Name)
    {
        return this.CreateTextureObject("GUI/TrenchMap/" + Name);
    }

    public Texture2D CreateYuanZhengIcon(string headName)
    {
        return this.CreateTextureObject("GUI/YuanZhengIcon/" + headName);
    }

    [DebuggerHidden]
    private IEnumerator DownloadFile(string file, Action<string> actErr)
    {
        return new <DownloadFile>c__IteratorAC { file = file, actErr = actErr, <$>file = file, <$>actErr = actErr };
    }

    [DebuggerHidden]
    private IEnumerator DownloadPatch(string url, Action<string> actErr, bool progressed = false)
    {
        return new <DownloadPatch>c__IteratorAB { url = url, progressed = progressed, actErr = actErr, <$>url = url, <$>progressed = progressed, <$>actErr = actErr, <>f__this = this };
    }

    private ExternalPak FindPakInfo(int index, List<ExternalPak> lst)
    {
        <FindPakInfo>c__AnonStorey288 storey = new <FindPakInfo>c__AnonStorey288 {
            index = index
        };
        ExternalPak item = lst.Find(new Predicate<ExternalPak>(storey.<>m__5D8));
        if (item == null)
        {
            item = new ExternalPak {
                index = storey.index
            };
            lst.Add(item);
        }
        return item;
    }

    private void FormatDownloadMessage(int word_id, params object[] args)
    {
        internal_word_config _config = ConfigMgr.getInstance().getByEntry<internal_word_config>(word_id);
        if (_config != null)
        {
            this.msgDownload = string.Format(_config.CN, args);
        }
    }

    private string GenMd5Str(string path)
    {
        MD5 md = new MD5CryptoServiceProvider();
        return BitConverter.ToString(md.ComputeHash(File.ReadAllBytes(path))).Replace("-", string.Empty);
    }

    private string[] GetDependencies(string key)
    {
        if (this.dependencies_manual.Count <= 0)
        {
            string path = PathOfArchive() + "/dependencies";
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);
                try
                {
                    for (string str2 = reader.ReadLine(); str2 != null; str2 = reader.ReadLine())
                    {
                        int index = str2.IndexOf("\t");
                        if (index != -1)
                        {
                            string str3 = str2.Substring(index + 1);
                            string str4 = str2.Remove(index);
                            char[] separator = new char[] { '|' };
                            string[] strArray = str3.Split(separator);
                            this.dependencies_manual.Add(str4, strArray);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
        }
        string[] strArray2 = null;
        if (!this.dependencies_manual.TryGetValue(key, out strArray2))
        {
            return null;
        }
        return strArray2;
    }

    public UIAtlas LoadAtlasFromResource(string path)
    {
        string res = path;
        GameObject obj2 = Instance.LoadResource(res, ".prefab", typeof(GameObject)) as GameObject;
        if (obj2 == null)
        {
            Debug.LogWarning("LoadAtlasFromResource Failed:" + path);
            return null;
        }
        return obj2.GetComponent<UIAtlas>();
    }

    public Operation LoadGuiPanelAdditiveAsync(string lv)
    {
        return new Operation { ao = Application.LoadLevelAdditiveAsync(lv) };
    }

    public void LoadLevel(string lv)
    {
        this.LoadLevelAsync(lv);
    }

    public void LoadLevelAdditive(string lv)
    {
        this.LoadLevelDependencies(lv);
        Application.LoadLevelAdditive(lv);
    }

    public Operation LoadLevelAdditiveAsync(string lv)
    {
        Operation operation = this.LoadLevelDependencies(lv);
        operation.ao = Application.LoadLevelAdditiveAsync(lv);
        return operation;
    }

    public Operation LoadLevelAsync(string lv)
    {
        Operation operation = this.LoadLevelDependencies(lv);
        operation.ao = Application.LoadLevelAsync(lv);
        return operation;
    }

    private Operation LoadLevelDependencies(string lv)
    {
        Operation item = new Operation();
        if (lv.ToLower().IndexOf("cj_") != -1)
        {
            string str = "assets_scenes_" + lv.ToLower() + ".unity.bundle";
            string path = PathOfArchive() + "/" + str;
            if (this.BundlesManual.Contains(str) && File.Exists(path))
            {
                item.bundle = AssetBundle.CreateFromFile(path);
            }
        }
        this.scenes_preloaders.Add(item);
        return item;
    }

    public UnityEngine.Object LoadResource(string res, string format, System.Type type)
    {
        UnityEngine.Object obj2 = null;
        string key = ("assets_resources_" + res + format).ToLower().Replace("/", "_").Trim();
        if (this.activitied_bundles.ContainsKey(key))
        {
            obj2 = this.activitied_bundles[key];
        }
        if (null != obj2)
        {
            return obj2;
        }
        obj2 = this.LoadResourceFromAssetBundle(key);
        if (null != obj2)
        {
            this.activitied_bundles[key] = obj2;
            return obj2;
        }
        return Resources.Load(res, type);
    }

    public UnityEngine.Object LoadResourceFromAssetBundle(string bundle)
    {
        string path = PathOfArchive() + "/" + bundle;
        if (!this.BundlesManual.Contains(bundle) || !File.Exists(path))
        {
            return null;
        }
        List<AssetBundle> list = new List<AssetBundle>();
        string[] dependencies = this.GetDependencies(bundle);
        if (dependencies != null)
        {
            int length = dependencies.Length;
            for (int j = 0; j != length; j++)
            {
                string str2 = dependencies[j];
                if (!string.IsNullOrEmpty(str2))
                {
                    AssetBundle item = AssetBundle.CreateFromFile(PathOfArchive() + "/" + str2);
                    if (null != item)
                    {
                        list.Add(item);
                    }
                }
            }
        }
        AssetBundle bundle3 = AssetBundle.CreateFromFile(path);
        if (null == bundle3)
        {
            return null;
        }
        UnityEngine.Object mainAsset = null;
        try
        {
            bundle3.LoadAll();
            mainAsset = bundle3.mainAsset;
        }
        catch (Exception)
        {
        }
        bundle3.Unload(false);
        int count = list.Count;
        for (int i = 0; i != count; i++)
        {
            AssetBundle bundle4 = list[i];
            if (null == bundle4)
            {
                bundle4.Unload(false);
            }
        }
        return mainAsset;
    }

    public Texture LoadUIAtlasTexture(string name)
    {
        string key = ("assets_gui_atlases_" + name + ".png").ToLower().Trim();
        UnityEngine.Object obj2 = null;
        if (this.activitied_bundles.ContainsKey(key))
        {
            obj2 = this.activitied_bundles[key];
        }
        if (null == obj2)
        {
            obj2 = this.LoadResourceFromAssetBundle(key);
            if (null != obj2)
            {
                this.activitied_bundles.Add(key, obj2);
            }
        }
        return (obj2 as Texture);
    }

    private void OnDestroy()
    {
        this.activitied_bundles.Clear();
        this.activitied_bundles = null;
    }

    private List<ExternalPak> PakListDeserialization()
    {
        List<ExternalPak> lst = new List<ExternalPak>();
        string path = PathOfArchive() + "/pak_list";
        if (!File.Exists(path))
        {
            this.PakListSerialization(lst);
            return lst;
        }
        MemoryStream serializationStream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        byte[] buffer = File.ReadAllBytes(path);
        serializationStream.Write(buffer, 0, buffer.Length);
        serializationStream.Seek(0L, SeekOrigin.Begin);
        try
        {
            int num = 0;
            num = Convert.ToInt32(formatter.Deserialize(serializationStream));
            for (int i = 0; i != num; i++)
            {
                ExternalPak item = formatter.Deserialize(serializationStream) as ExternalPak;
                lst.Add(item);
            }
        }
        finally
        {
            serializationStream.Close();
        }
        return lst;
    }

    private void PakListSerialization(List<ExternalPak> lst)
    {
        string path = PathOfArchive() + "/pak_list";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        MemoryStream serializationStream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            int count = lst.Count;
            formatter.Serialize(serializationStream, count);
            for (int i = 0; i != count; i++)
            {
                formatter.Serialize(serializationStream, lst[i]);
            }
            serializationStream.Seek(0L, SeekOrigin.Begin);
            int length = (int) serializationStream.Length;
            byte[] buffer = new byte[length];
            serializationStream.Read(buffer, 0, length);
            File.WriteAllBytes(path, buffer);
        }
        finally
        {
            serializationStream.Close();
        }
    }

    public static string PathOfArchive()
    {
        if (string.IsNullOrEmpty(path_of_archieve))
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                {
                    path = Application.dataPath;
                    int startIndex = path.LastIndexOf("Assets");
                    if (startIndex != -1)
                    {
                        path = path.Remove(startIndex);
                    }
                    path = path + "/bundles";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    break;
                }
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    path = Application.persistentDataPath;
                    break;

                default:
                    path = string.Empty;
                    break;
            }
            path_of_archieve = path;
        }
        return path_of_archieve;
    }

    private void RecordPakMd5(int pak_index, string pak_md5)
    {
        List<ExternalPak> lst = this.PakListDeserialization();
        this.FindPakInfo(pak_index, lst).md5 = pak_md5;
        this.PakListSerialization(lst);
    }

    private void RecordVersion(string version)
    {
        string path = PathOfArchive() + "/version";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, version);
    }

    [DebuggerHidden]
    private IEnumerator SetupBundlePak(string pak_file, PakHeader pak_hdr, Action<string> actError)
    {
        return new <SetupBundlePak>c__IteratorAE { pak_file = pak_file, actError = actError, pak_hdr = pak_hdr, <$>pak_file = pak_file, <$>actError = actError, <$>pak_hdr = pak_hdr, <>f__this = this };
    }

    private void Start()
    {
        base.StartCoroutine(this.WaitforLevelLoaded());
    }

    private string SyncSetupBundlePak(string pak_file, string root_path)
    {
        if (!File.Exists(pak_file))
        {
            return "pak file is not exsitly";
        }
        PakHeader header = new PakHeader();
        FileStream serializationStream = new FileStream(pak_file, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            header.version = Convert.ToString(formatter.Deserialize(serializationStream));
            header.platform = Convert.ToString(formatter.Deserialize(serializationStream));
            header.chunk = Convert.ToInt32(formatter.Deserialize(serializationStream));
        }
        catch (Exception exception)
        {
            serializationStream.Close();
            if (File.Exists(pak_file))
            {
                File.Delete(pak_file);
            }
            return exception.Message;
        }
        HashSet<string> bundlesManual = this.BundlesManual;
        for (int i = 0; i != header.chunk; i++)
        {
            try
            {
                BundleChunk chunk = formatter.Deserialize(serializationStream) as BundleChunk;
                string path = root_path + "/" + chunk.key;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                File.WriteAllBytes(path, chunk.data);
                FileCompression.Decompress(path, path);
                if (this.GenMd5Str(path) != chunk.md5)
                {
                    throw new UnityException("failed to verify bundle chunk named " + chunk.key);
                }
                bundlesManual.Add(chunk.key);
            }
            catch (Exception exception2)
            {
                serializationStream.Close();
                if (File.Exists(pak_file))
                {
                    File.Delete(pak_file);
                }
                return exception2.Message;
            }
        }
        this.BundlesManual = bundlesManual;
        serializationStream.Close();
        if (File.Exists(pak_file))
        {
            File.Delete(pak_file);
        }
        return string.Empty;
    }

    [DebuggerHidden]
    private IEnumerator UpdateDownloadProgress(WWW wdl)
    {
        return new <UpdateDownloadProgress>c__IteratorAA { wdl = wdl, <$>wdl = wdl, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator WaitforLevelLoaded()
    {
        return new <WaitforLevelLoaded>c__IteratorA9 { <>f__this = this };
    }

    public HashSet<string> BundlesManual
    {
        get
        {
            if (this._bundlesManual == null)
            {
                this._bundlesManual = new HashSet<string>();
                string path = PathOfArchive() + "/manual";
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, string.Empty);
                }
                foreach (string str2 in File.ReadAllLines(path))
                {
                    this._bundlesManual.Add(str2);
                }
            }
            return this._bundlesManual;
        }
        set
        {
            string contents = string.Empty;
            foreach (string str2 in value)
            {
                contents = contents + str2 + "\n";
            }
            File.WriteAllText(PathOfArchive() + "/manual", contents);
            this._bundlesManual = value;
        }
    }

    public static BundleMgr Instance
    {
        get
        {
            if (null == _Instance)
            {
                GameObject target = new GameObject {
                    name = "BundleMgr_Container"
                };
                UnityEngine.Object.DontDestroyOnLoad(target);
                _Instance = target.AddComponent<BundleMgr>();
            }
            return _Instance;
        }
    }

    [CompilerGenerated]
    private sealed class <AnalyzePakDLs>c__AnonStorey287
    {
        internal BundleMgr.ExternalPak info;

        internal bool <>m__5D7(BundleMgr.ExternalPak e)
        {
            return (e.index == this.info.index);
        }
    }

    [CompilerGenerated]
    private sealed class <AsynCheckBundles>c__IteratorAD : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<string>.Enumerator <$s_957>__18;
        internal List<string>.Enumerator <$s_958>__21;
        internal BundleMgr <>f__this;
        internal Action<string> <act_err>__2;
        internal int <chunk_count>__17;
        internal string <current_ver>__0;
        internal string <err>__1;
        internal string <fn>__13;
        internal int <i>__10;
        internal int <i>__15;
        internal int <index>__12;
        internal string <ln>__11;
        internal bool <new_patch>__9;
        internal BundleMgr.PakHeader <pak_hdr>__20;
        internal string <patch_list>__5;
        internal string <patch>__16;
        internal string <patch>__19;
        internal string <patch>__22;
        internal List<string> <patchs_dl_list>__7;
        internal string[] <patchs>__6;
        internal string <primary_version>__4;
        internal List<string> <target_patchs_list>__8;
        internal string <text>__24;
        internal string <ver_file>__23;
        internal string <ver>__14;
        internal string <version_fn>__3;

        internal void <>m__5D9(string dl_err)
        {
            this.<err>__1 = dl_err;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 15:
                case 0x10:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_958>__21.Dispose();
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
                    this.<current_ver>__0 = GameDefine.getInstance().PatchVersion;
                    if (!File.Exists(BundleMgr.PathOfArchive() + "/version"))
                    {
                        File.WriteAllText(BundleMgr.PathOfArchive() + "/version", this.<current_ver>__0);
                        break;
                    }
                    this.<current_ver>__0 = File.ReadAllText(BundleMgr.PathOfArchive() + "/version");
                    break;

                case 1:
                    this.<err>__1 = string.Empty;
                    this.<act_err>__2 = new Action<string>(this.<>m__5D9);
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.DownloadFile("primary.txt", this.<act_err>__2));
                    this.$PC = 2;
                    goto Label_09AE;

                case 2:
                    if (string.IsNullOrEmpty(this.<err>__1))
                    {
                        this.<version_fn>__3 = BundleMgr.PathOfArchive() + "/primary.txt";
                        this.<primary_version>__4 = File.ReadAllText(this.<version_fn>__3);
                        this.<primary_version>__4 = this.<primary_version>__4.Replace("\n", string.Empty).Replace("\r\n", string.Empty).Replace("\r", string.Empty);
                        Debug.Log("primary version = " + this.<primary_version>__4 + "client version = " + GameDefine.getInstance().clientVersion);
                        if (string.IsNullOrEmpty(this.<primary_version>__4) || !this.<>f__this.CompareVerion(this.<primary_version>__4, GameDefine.getInstance().clientVersion))
                        {
                            this.<>f__this.FormatDownloadMessage(2, new object[0]);
                            this.<>f__this.totalDownload = this.<>f__this.progressDownload = 1;
                            this.<>f__this.BundlesManual = new HashSet<string>();
                            this.$current = new WaitForSeconds(1f);
                            this.$PC = 4;
                        }
                        else
                        {
                            this.$current = this.<>f__this.StartCoroutine(this.<>f__this.DownloadFile("patch.txt", this.<act_err>__2));
                            this.$PC = 5;
                        }
                    }
                    else
                    {
                        this.<>f__this.FormatDownloadMessage(9, new object[0]);
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 3;
                    }
                    goto Label_09AE;

                case 3:
                    this.<>f__this.completedDownload = true;
                    goto Label_09AC;

                case 4:
                    this.<>f__this.completedDownload = true;
                    goto Label_09AC;

                case 5:
                    if (string.IsNullOrEmpty(this.<err>__1))
                    {
                        this.<patch_list>__5 = BundleMgr.PathOfArchive() + "/patch.txt";
                        this.<patchs>__6 = File.ReadAllLines(this.<patch_list>__5);
                        File.Delete(this.<patch_list>__5);
                        this.<patchs_dl_list>__7 = new List<string>();
                        this.<target_patchs_list>__8 = new List<string>();
                        this.<new_patch>__9 = false;
                        this.<i>__10 = this.<patchs>__6.Length - 1;
                        while (this.<i>__10 >= 0)
                        {
                            this.<ln>__11 = this.<patchs>__6[this.<i>__10];
                            this.<index>__12 = this.<ln>__11.IndexOf('\t');
                            if (this.<index>__12 != -1)
                            {
                                this.<fn>__13 = this.<ln>__11.Substring(this.<index>__12 + 1);
                                this.<ver>__14 = this.<ln>__11.Remove(this.<index>__12);
                                if (!string.IsNullOrEmpty(this.<ver>__14))
                                {
                                    if (!this.<>f__this.ComparePrimaryVersion(this.<ver>__14, this.<primary_version>__4) || this.<>f__this.CompareVerion(this.<ver>__14, this.<current_ver>__0))
                                    {
                                        break;
                                    }
                                    this.<patchs_dl_list>__7.Add(this.<fn>__13);
                                }
                            }
                            this.<i>__10--;
                        }
                        if (this.<patchs_dl_list>__7.Count > 0)
                        {
                            this.<>f__this.progressDownload = 0;
                            this.<>f__this.totalDownload = this.<patchs_dl_list>__7.Count;
                            object[] objArray2 = new object[] { this.<patchs_dl_list>__7.Count };
                            this.<>f__this.FormatDownloadMessage(7, objArray2);
                            this.$current = new WaitForSeconds(1f);
                            this.$PC = 7;
                            goto Label_09AE;
                        }
                        goto Label_050A;
                    }
                    this.<>f__this.FormatDownloadMessage(9, new object[0]);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 6;
                    goto Label_09AE;

                case 6:
                    this.<>f__this.completedDownload = true;
                    goto Label_09AC;

                case 7:
                    goto Label_050A;

                case 8:
                    this.<patch>__16 = this.<patchs_dl_list>__7[this.<i>__15];
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.DownloadPatch(this.<patch>__16, this.<act_err>__2, false));
                    this.$PC = 9;
                    goto Label_09AE;

                case 9:
                    if (string.IsNullOrEmpty(this.<err>__1))
                    {
                        goto Label_0609;
                    }
                    this.<>f__this.FormatDownloadMessage(5, new object[0]);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 10;
                    goto Label_09AE;

                case 10:
                    goto Label_066B;

                case 11:
                    this.<>f__this.FormatDownloadMessage(6, new object[0]);
                    this.$current = new WaitForSeconds(2f);
                    this.$PC = 12;
                    goto Label_09AE;

                case 12:
                    this.<chunk_count>__17 = 0;
                    this.<$s_957>__18 = this.<target_patchs_list>__8.GetEnumerator();
                    try
                    {
                        while (this.<$s_957>__18.MoveNext())
                        {
                            this.<patch>__19 = this.<$s_957>__18.Current;
                            this.<chunk_count>__17 += this.<>f__this.CalculateChunkCount(this.<patch>__19);
                        }
                    }
                    finally
                    {
                        this.<$s_957>__18.Dispose();
                    }
                    this.$current = null;
                    this.$PC = 13;
                    goto Label_09AE;

                case 13:
                {
                    this.<>f__this.progressDownload = 0;
                    this.<>f__this.totalDownload = this.<chunk_count>__17;
                    object[] objArray5 = new object[] { this.<chunk_count>__17 };
                    this.<>f__this.FormatDownloadMessage(3, objArray5);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 14;
                    goto Label_09AE;
                }
                case 14:
                    this.<pak_hdr>__20 = new BundleMgr.PakHeader();
                    this.<$s_958>__21 = this.<target_patchs_list>__8.GetEnumerator();
                    num = 0xfffffffd;
                    goto Label_07FF;

                case 15:
                case 0x10:
                    goto Label_07FF;

                case 0x11:
                    this.<>f__this.completedDownload = true;
                    this.<ver_file>__23 = BundleMgr.PathOfArchive() + "/version";
                    if (File.Exists(this.<ver_file>__23))
                    {
                        this.<text>__24 = File.ReadAllText(this.<ver_file>__23);
                        GameDefine.getInstance().SetVersionByString(this.<text>__24);
                    }
                    ConfigMgr.getInstance().Reset();
                    this.$PC = -1;
                    goto Label_09AC;

                default:
                    goto Label_09AC;
            }
            object[] args = new object[] { this.<current_ver>__0 };
            this.<>f__this.FormatDownloadMessage(0, args);
            FileCompression._file_compression_tempare = BundleMgr.PathOfArchive() + "/_mtd_temp_compression";
            this.$current = new WaitForSeconds(1f);
            this.$PC = 1;
            goto Label_09AE;
        Label_050A:
            this.<i>__15 = this.<patchs_dl_list>__7.Count - 1;
            while (this.<i>__15 >= 0)
            {
                object[] objArray3 = new object[] { this.<>f__this.progressDownload++, this.<>f__this.totalDownload };
                this.<>f__this.FormatDownloadMessage(8, objArray3);
                this.$current = null;
                this.$PC = 8;
                goto Label_09AE;
            Label_0609:
                this.<target_patchs_list>__8.Add(BundleMgr.PathOfArchive() + "/" + this.<patch>__16.ToLower().Replace("/", "_").Replace(":", "_").Trim());
                this.<i>__15--;
            }
        Label_066B:
            if (this.<target_patchs_list>__8.Count <= 0)
            {
                goto Label_0900;
            }
            object[] objArray4 = new object[] { this.<>f__this.progressDownload, this.<>f__this.totalDownload };
            this.<>f__this.FormatDownloadMessage(8, objArray4);
            this.$current = new WaitForSeconds(1f);
            this.$PC = 11;
            goto Label_09AE;
        Label_07FF:
            try
            {
                switch (num)
                {
                    case 15:
                        this.<>f__this.RecordVersion(this.<pak_hdr>__20.version);
                        if (string.IsNullOrEmpty(this.<err>__1))
                        {
                            break;
                        }
                        this.<>f__this.FormatDownloadMessage(10, new object[0]);
                        this.$current = new WaitForSeconds(1f);
                        this.$PC = 0x10;
                        flag = true;
                        goto Label_09AE;

                    case 0x10:
                        this.<>f__this.completedDownload = true;
                        ConfigMgr.getInstance().Reset();
                        goto Label_09AC;
                }
                while (this.<$s_958>__21.MoveNext())
                {
                    this.<patch>__22 = this.<$s_958>__21.Current;
                    this.$current = this.<>f__this.StartCoroutine(this.<>f__this.SetupBundlePak(this.<patch>__22, this.<pak_hdr>__20, this.<act_err>__2));
                    this.$PC = 15;
                    flag = true;
                    goto Label_09AE;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_958>__21.Dispose();
            }
        Label_0900:
            this.<>f__this.totalDownload = this.<>f__this.progressDownload = 1;
            this.<>f__this.FormatDownloadMessage(2, new object[0]);
            this.$current = new WaitForSeconds(1f);
            this.$PC = 0x11;
            goto Label_09AE;
        Label_09AC:
            return false;
        Label_09AE:
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

    [CompilerGenerated]
    private sealed class <DownloadFile>c__IteratorAC : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<string> <$>actErr;
        internal string <$>file;
        internal string <arh>__1;
        internal string <lwr>__2;
        internal string <platform>__0;
        internal string <tar>__4;
        internal string <uri>__5;
        internal string <url>__3;
        internal WWW <wdl>__6;
        internal Action<string> actErr;
        internal string file;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                {
                    this.<platform>__0 = "android";
                    this.<arh>__1 = BundleMgr.PathOfArchive();
                    this.<lwr>__2 = this.file.ToLower().Replace("/", "_").Trim();
                    string[] textArray1 = new string[] { ServerInfo.getInstance().patch_url, "/", this.<platform>__0, "/", this.<lwr>__2, "?t=", UnityEngine.Random.Range(0, 0x186a0).ToString() };
                    this.<url>__3 = string.Concat(textArray1);
                    this.<tar>__4 = this.<arh>__1 + "/" + this.<lwr>__2;
                    this.<uri>__5 = Uri.EscapeUriString(this.<url>__3);
                    Debug.Log("download file : " + this.<uri>__5);
                    this.<wdl>__6 = new WWW(this.<uri>__5);
                    this.$current = this.<wdl>__6;
                    this.$PC = 1;
                    goto Label_01E7;
                }
                case 1:
                    if (string.IsNullOrEmpty(this.<wdl>__6.error))
                    {
                        if (!Directory.Exists(this.<arh>__1))
                        {
                            Directory.CreateDirectory(this.<arh>__1);
                        }
                        if (File.Exists(this.<tar>__4))
                        {
                            File.Delete(this.<tar>__4);
                        }
                        File.WriteAllBytes(this.<tar>__4, this.<wdl>__6.bytes);
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_01E7;
                    }
                    if (this.actErr != null)
                    {
                        Debug.Log("download err : " + this.<wdl>__6.error);
                        this.actErr(this.<wdl>__6.error);
                    }
                    break;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_01E7:
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

    [CompilerGenerated]
    private sealed class <DownloadPatch>c__IteratorAB : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<string> <$>actErr;
        internal bool <$>progressed;
        internal string <$>url;
        internal BundleMgr <>f__this;
        internal string <arh>__0;
        internal string <lwr>__1;
        internal string <tar>__2;
        internal string <uri>__3;
        internal WWW <wdl>__4;
        internal Action<string> actErr;
        internal bool progressed;
        internal string url;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<arh>__0 = BundleMgr.PathOfArchive();
                    this.<lwr>__1 = this.url.ToLower().Replace("/", "_").Replace(":", "_").Trim();
                    this.<tar>__2 = this.<arh>__0 + "/" + this.<lwr>__1;
                    this.<uri>__3 = Uri.EscapeUriString(this.url);
                    Debug.Log("download file : " + this.<uri>__3);
                    this.<wdl>__4 = new WWW(this.<uri>__3);
                    if (!this.progressed)
                    {
                        this.$current = this.<wdl>__4;
                        this.$PC = 2;
                        goto Label_020A;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_014C;

                case 3:
                    this.$PC = -1;
                    goto Label_0208;

                default:
                    goto Label_0208;
            }
            this.<>f__this.progressDownload = (int) (100f * this.<wdl>__4.progress);
            if (!this.<wdl>__4.isDone && string.IsNullOrEmpty(this.<wdl>__4.error))
            {
                this.$current = null;
                this.$PC = 1;
                goto Label_020A;
            }
        Label_014C:
            if (!string.IsNullOrEmpty(this.<wdl>__4.error))
            {
                if (this.actErr != null)
                {
                    Debug.Log("download error" + this.<wdl>__4.error);
                    this.actErr(this.<wdl>__4.error);
                }
            }
            else
            {
                if (!Directory.Exists(this.<arh>__0))
                {
                    Directory.CreateDirectory(this.<arh>__0);
                }
                if (File.Exists(this.<tar>__2))
                {
                    File.Delete(this.<tar>__2);
                }
                File.WriteAllBytes(this.<tar>__2, this.<wdl>__4.bytes);
                this.$current = null;
                this.$PC = 3;
                goto Label_020A;
            }
        Label_0208:
            return false;
        Label_020A:
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

    [CompilerGenerated]
    private sealed class <FindPakInfo>c__AnonStorey288
    {
        internal int index;

        internal bool <>m__5D8(BundleMgr.ExternalPak e)
        {
            return (e.index == this.index);
        }
    }

    [CompilerGenerated]
    private sealed class <SetupBundlePak>c__IteratorAE : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal Action<string> <$>actError;
        internal string <$>pak_file;
        internal BundleMgr.PakHeader <$>pak_hdr;
        internal BundleMgr <>f__this;
        internal string <bundle_fn>__6;
        internal BundleMgr.BundleChunk <chunk>__5;
        internal Exception <e>__2;
        internal Exception <e>__8;
        internal BinaryFormatter <formatter>__1;
        internal int <i>__4;
        internal HashSet<string> <manual>__3;
        internal string <md5>__7;
        internal FileStream <stream>__0;
        internal Action<string> actError;
        internal string pak_file;
        internal BundleMgr.PakHeader pak_hdr;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (File.Exists(this.pak_file))
                    {
                        this.<stream>__0 = new FileStream(this.pak_file, FileMode.Open);
                        this.<formatter>__1 = new BinaryFormatter();
                        try
                        {
                            this.pak_hdr.version = Convert.ToString(this.<formatter>__1.Deserialize(this.<stream>__0));
                            this.pak_hdr.platform = Convert.ToString(this.<formatter>__1.Deserialize(this.<stream>__0));
                            this.pak_hdr.chunk = Convert.ToInt32(this.<formatter>__1.Deserialize(this.<stream>__0));
                        }
                        catch (Exception exception)
                        {
                            this.<e>__2 = exception;
                            Debug.Log(this.<e>__2.Message);
                            if (this.actError == null)
                            {
                                this.actError(this.<e>__2.Message);
                            }
                            this.<stream>__0.Close();
                            if (File.Exists(this.pak_file))
                            {
                                File.Delete(this.pak_file);
                            }
                            break;
                        }
                        this.<manual>__3 = this.<>f__this.BundlesManual;
                        this.<i>__4 = 0;
                        while (this.<i>__4 != this.pak_hdr.chunk)
                        {
                            object[] args = new object[] { this.<>f__this.progressDownload, this.<>f__this.totalDownload };
                            this.<>f__this.FormatDownloadMessage(4, args);
                            this.$current = null;
                            this.$PC = 1;
                            goto Label_03AF;
                        Label_01AB:
                            try
                            {
                                this.<chunk>__5 = this.<formatter>__1.Deserialize(this.<stream>__0) as BundleMgr.BundleChunk;
                                if (BundleMgr.patch_cleaner_key == this.<chunk>__5.key)
                                {
                                    this.<manual>__3.Clear();
                                }
                                else
                                {
                                    this.<bundle_fn>__6 = BundleMgr.PathOfArchive() + "/" + this.<chunk>__5.key;
                                    if (File.Exists(this.<bundle_fn>__6))
                                    {
                                        File.Delete(this.<bundle_fn>__6);
                                    }
                                    File.WriteAllBytes(this.<bundle_fn>__6, this.<chunk>__5.data);
                                    FileCompression.Decompress(this.<bundle_fn>__6, this.<bundle_fn>__6);
                                    this.<md5>__7 = this.<>f__this.GenMd5Str(this.<bundle_fn>__6);
                                    if (this.<md5>__7 != this.<chunk>__5.md5)
                                    {
                                        throw new UnityException("failed to verify bundle chunk named " + this.<chunk>__5.key);
                                    }
                                    this.<manual>__3.Add(this.<chunk>__5.key);
                                }
                            }
                            catch (Exception exception2)
                            {
                                this.<e>__8 = exception2;
                                Debug.Log(this.<e>__8.Message);
                                if (this.actError != null)
                                {
                                    this.actError(this.<e>__8.Message);
                                }
                                this.<stream>__0.Close();
                                if (File.Exists(this.pak_file))
                                {
                                    File.Delete(this.pak_file);
                                }
                                break;
                            }
                            this.<>f__this.progressDownload++;
                            this.<i>__4++;
                        }
                        this.<>f__this.BundlesManual = this.<manual>__3;
                        this.<stream>__0.Close();
                        if (File.Exists(this.pak_file))
                        {
                            File.Delete(this.pak_file);
                        }
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_03AF;
                    }
                    if (this.actError != null)
                    {
                        this.actError(string.Empty);
                    }
                    break;

                case 1:
                    goto Label_01AB;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_03AF:
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

    [CompilerGenerated]
    private sealed class <UpdateDownloadProgress>c__IteratorAA : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal WWW <$>wdl;
        internal BundleMgr <>f__this;
        internal WWW wdl;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            this.$PC = -1;
            if (this.$PC == 0)
            {
                this.<>f__this.totalDownload = 100;
                this.<>f__this.progressDownload = 0;
                while (true)
                {
                    if (this.wdl.isDone)
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(this.wdl.error))
                    {
                        this.<>f__this.progressDownload = 0;
                        break;
                    }
                    this.<>f__this.progressDownload = (int) (this.wdl.progress * 100f);
                }
            }
            return false;
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

    [CompilerGenerated]
    private sealed class <WaitforLevelLoaded>c__IteratorA9 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BundleMgr <>f__this;
        internal bool <clear_flag>__0;
        internal int <count>__1;
        internal int <i>__2;
        internal Operation <lao>__3;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    this.<clear_flag>__0 = false;
                    this.<count>__1 = this.<>f__this.scenes_preloaders.Count;
                    this.<i>__2 = 0;
                    while (this.<i>__2 != this.<count>__1)
                    {
                        this.<lao>__3 = this.<>f__this.scenes_preloaders[this.<i>__2];
                        if ((this.<lao>__3 != null) && this.<lao>__3.IsDone())
                        {
                            this.<lao>__3.Release();
                            this.<>f__this.scenes_preloaders.RemoveAt(this.<i>__2);
                            this.<clear_flag>__0 = true;
                            break;
                        }
                        this.<i>__2++;
                    }
                    break;

                default:
                    goto Label_0115;
            }
            if (this.<clear_flag>__0)
            {
                this.<>f__this.ClearCache();
            }
            this.$current = new WaitForSeconds(0.5f);
            this.$PC = 1;
            return true;
            this.$PC = -1;
        Label_0115:
            return false;
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

    public enum AntStatus
    {
        waitting,
        decompress,
        failed
    }

    [Serializable]
    public class BundleChunk
    {
        public byte[] data;
        public string key = string.Empty;
        public string md5 = string.Empty;
    }

    [Serializable]
    public class ExternalPak
    {
        public string addr = string.Empty;
        public int index;
        public string md5 = string.Empty;
    }

    public class PakHeader
    {
        public int chunk;
        public string platform;
        public string version;
    }

    public class SetupAnt
    {
        public int identity = -1;
        public string md5 = string.Empty;
        public string url = string.Empty;
    }

    public class SetupColony
    {
        private Queue<BundleMgr.SetupAnt> ant_queue = new Queue<BundleMgr.SetupAnt>();
        private object ant_queue_mutex = new object();
        public string err = string.Empty;
        public string folder = string.Empty;
        public BundleMgr mgr;
        public bool quit;
        public BundleMgr.AntStatus status;
        public Thread thread;

        public bool Activity()
        {
            return ((this.thread.ThreadState == ThreadState.Running) || (ThreadState.Background == this.thread.ThreadState));
        }

        public void AddTask(string pak_url, string md5, int identity)
        {
            object obj2 = this.ant_queue_mutex;
            lock (obj2)
            {
                BundleMgr.SetupAnt item = new BundleMgr.SetupAnt {
                    url = pak_url,
                    identity = identity,
                    md5 = md5
                };
                this.ant_queue.Enqueue(item);
            }
        }

        private BundleMgr.SetupAnt DequeueTask()
        {
            object obj2 = this.ant_queue_mutex;
            lock (obj2)
            {
                return this.ant_queue.Dequeue();
            }
        }

        public bool HasTask()
        {
            object obj2 = this.ant_queue_mutex;
            lock (obj2)
            {
                return (this.ant_queue.Count > 0);
            }
        }

        public bool IsDone()
        {
            if (this.HasTask())
            {
                return false;
            }
            return (BundleMgr.AntStatus.decompress != this.status);
        }

        public void Kill()
        {
            this.quit = true;
        }

        public int TaskCount()
        {
            object obj2 = this.ant_queue_mutex;
            lock (obj2)
            {
                return this.ant_queue.Count;
            }
        }

        public void ThreadFunc()
        {
            while (!this.quit)
            {
                if (this.HasTask())
                {
                    this.status = BundleMgr.AntStatus.decompress;
                    BundleMgr.SetupAnt ant = this.DequeueTask();
                    this.err = this.mgr.SyncSetupBundlePak(ant.url, this.folder);
                    if (this.quit)
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(this.err))
                    {
                        this.status = BundleMgr.AntStatus.failed;
                        break;
                    }
                    this.mgr.RecordPakMd5(ant.identity, ant.md5);
                    this.status = BundleMgr.AntStatus.waitting;
                }
            }
        }

        public void Todo()
        {
            this.folder = BundleMgr.PathOfArchive();
            this.thread = new Thread(new ThreadStart(this.ThreadFunc));
            this.thread.Start();
        }
    }
}

