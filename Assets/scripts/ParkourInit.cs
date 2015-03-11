using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParkourInit
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapE;
    public int chapteIndex;
    public int characterEntry;
    private int index;
    private Vector3 lastPropVector = new Vector3(0f, 0f, 20f);
    private static ParkourInit m_Instance;
    private int mapCount;
    private int mapIndex = 1;
    public List<MapInfo> mapInfos;
    public string mapName = "jiaotang";
    private int mapOffect = 100;
    public static List<ParkourMap> maps = new List<ParkourMap>();
    public float maxRunDistance;
    private GameObject propParentObj;
    public int randomPropIndex = 1;
    private int startPropIndex = 20;

    private void CareatPropAndCheckPropGroup(MapInfo info)
    {
        if (this.propParentObj == null)
        {
            this.propParentObj = new GameObject();
            this.propParentObj.name = "prop_" + this.mapCount;
        }
        GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(BattleGlobal.ParkourPropPrefab + info.propName), new Vector3((info.trackIndex - 1f) * 2.5f, info.hight, this.lastPropVector.z + info.distanceFromLastProp), Quaternion.identity) as GameObject;
        this.lastPropVector = obj2.transform.position;
        if (this.lastPropVector.z > (this.mapCount * 200))
        {
            if (this.mapCount != 0)
            {
                this.propParentObj = new GameObject();
                this.propParentObj.name = "prop_" + this.mapCount;
            }
            this.GetMap(this.propParentObj);
            this.mapCount++;
        }
        if (obj2.GetComponent<PropInfo>() != null)
        {
            obj2.GetComponent<PropInfo>().propEntry = info.propEntry - 1;
        }
        else
        {
            PropInfo[] componentsInChildren = obj2.GetComponentsInChildren<PropInfo>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].propEntry = info.propEntry - 1;
            }
        }
        obj2.transform.parent = this.propParentObj.transform;
        obj2.name = obj2.name.Replace("(Clone)", "_") + this.index;
        this.index++;
    }

    public void DesteryParkourInitInfo()
    {
        maps.Clear();
        m_Instance = null;
    }

    public void DestoryMap()
    {
        if (this.mapIndex <= (maps.Count - 1))
        {
            if ((ParkourManager._instance.runDistance > (200 * (this.mapIndex - 1))) && (this.mapIndex < maps.Count))
            {
                maps[this.mapIndex].SetActive(true);
            }
            if (!maps[this.mapIndex].mapObj.activeSelf || !maps[this.mapIndex].obstacleObj.activeSelf)
            {
                maps[this.mapIndex].SetActive(true);
                Debug.Log(string.Concat(new object[] { "mapindex:", this.mapIndex, "|  ", maps[this.mapIndex].mapObj.name }));
            }
            if ((ParkourManager._instance.runDistance > (200 * this.mapIndex)) && (maps.Count > 0))
            {
                maps[this.mapIndex - 1].SetActive(false);
                maps[this.mapIndex - 1] = null;
            }
            this.mapIndex = (ParkourManager._instance.runDistance <= (200 * this.mapIndex)) ? this.mapIndex : ++this.mapIndex;
        }
    }

    private ParkourMap GetMap(GameObject propGroup)
    {
        if (propGroup == null)
        {
            return null;
        }
        object[] objArray1 = new object[] { BattleGlobal.ParkourMapPrefab, this.mapName, "_", UnityEngine.Random.Range(1, 4) };
        ParkourMap item = new ParkourMap(Resources.Load<GameObject>(string.Concat(objArray1)), propGroup, this.mapCount, this.mapOffect);
        maps.Add(item);
        return item;
    }

    public void InitMap()
    {
        <InitMap>c__AnonStorey290 storey = new <InitMap>c__AnonStorey290();
        this.mapIndex = 1;
        this.startPropIndex = this.mapInfos.Count;
        for (int i = 0; i < this.startPropIndex; i++)
        {
            this.CareatPropAndCheckPropGroup(this.mapInfos[i]);
        }
        foreach (ParkourMap map in maps)
        {
            PropInfo[] componentsInChildren = map.obstacleObj.GetComponentsInChildren<PropInfo>();
            for (int k = 0; k < componentsInChildren.Length; k++)
            {
                map.props.Add(componentsInChildren[k]);
                if (((componentsInChildren[k].type == PropType.JINGBI) || (componentsInChildren[k].type == PropType.HUDUN)) || (componentsInChildren[k].type == PropType.BAOXIANG))
                {
                    ParkourEvent._instance.canFlyProp.Add(componentsInChildren[k]);
                }
            }
        }
        for (int j = 2; j < maps.Count; j++)
        {
            maps[j].SetActive(false);
        }
        storey.coinCount = 0;
        ParkourEvent._instance.canFlyProp.ForEach(new Action<PropInfo>(storey.<>m__5F6));
        Debug.Log("CoinCount:" + storey.coinCount);
    }

    public void LoadMapInfo()
    {
        object[] objArray5;
        object[] objArray4;
        object[] objArray3;
        object[] objArray2;
        object[] objArray1;
        this.mapInfos = new List<MapInfo>();
        char[] separator = new char[] { '|' };
        string[] strArray = ConfigMgr.getInstance().getByEntry<guildrun_config>(this.chapteIndex).prop_groupcount.Split(separator);
        this.mapName = ConfigMgr.getInstance().getByEntry<guildrun_config>(this.chapteIndex).map;
        int randomPropIndex = this.randomPropIndex;
        int num2 = (randomPropIndex != 0) ? (int.Parse(strArray[randomPropIndex - 1]) + 1) : 0;
        int num3 = 0;
        int num4 = 0;
        Debug.Log(ConfigMgr.getInstance().getByEntry<guildrun_config>(this.chapteIndex).chapter_prop);
        string key = ConfigMgr.getInstance().getByEntry<guildrun_config>(this.chapteIndex).chapter_prop;
        if (key != null)
        {
            int num5;
            if (<>f__switch$mapE == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
                dictionary.Add("chapter1prop_config", 0);
                dictionary.Add("chapter2prop_config", 1);
                dictionary.Add("chapter3prop_config", 2);
                dictionary.Add("chapter4prop_config", 3);
                dictionary.Add("chapter5prop_config", 4);
                <>f__switch$mapE = dictionary;
            }
            if (<>f__switch$mapE.TryGetValue(key, out num5))
            {
                ArrayList list;
                switch (num5)
                {
                    case 0:
                    {
                        list = ConfigMgr.getInstance().getList<chapter1prop_config>();
                        num3 = ((randomPropIndex + 1) != strArray.Length) ? int.Parse(strArray[randomPropIndex]) : list.Count;
                        IEnumerator enumerator = list.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                chapter1prop_config current = (chapter1prop_config) enumerator.Current;
                                if (num4 > num3)
                                {
                                    goto Label_021C;
                                }
                                if (num4 < num2)
                                {
                                    num4++;
                                }
                                else
                                {
                                    string p = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(current.prop_id - 1).model_major;
                                    this.mapInfos.Add(new MapInfo(p, current.track, (float) current.distance, current.height, current.prop_id));
                                    num4++;
                                }
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
                        goto Label_021C;
                    }
                    case 1:
                    {
                        list = ConfigMgr.getInstance().getList<chapter2prop_config>();
                        num3 = ((randomPropIndex + 1) < strArray.Length) ? int.Parse(strArray[randomPropIndex]) : list.Count;
                        IEnumerator enumerator2 = list.GetEnumerator();
                        try
                        {
                            while (enumerator2.MoveNext())
                            {
                                chapter2prop_config _config2 = (chapter2prop_config) enumerator2.Current;
                                if (num4 > num3)
                                {
                                    goto Label_0376;
                                }
                                if (num4 < num2)
                                {
                                    num4++;
                                }
                                else
                                {
                                    string str3 = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(_config2.prop_id - 1).model_major;
                                    this.mapInfos.Add(new MapInfo(str3, _config2.track, (float) _config2.distance, _config2.height, _config2.prop_id));
                                    num4++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable2 = enumerator2 as IDisposable;
                            if (disposable2 == null)
                            {
                            }
                            disposable2.Dispose();
                        }
                        goto Label_0376;
                    }
                    case 2:
                    {
                        list = ConfigMgr.getInstance().getList<chapter3prop_config>();
                        num3 = ((randomPropIndex + 1) < strArray.Length) ? int.Parse(strArray[randomPropIndex]) : list.Count;
                        IEnumerator enumerator3 = list.GetEnumerator();
                        try
                        {
                            while (enumerator3.MoveNext())
                            {
                                chapter3prop_config _config3 = (chapter3prop_config) enumerator3.Current;
                                if (num4 > num3)
                                {
                                    goto Label_04D0;
                                }
                                if (num4 < num2)
                                {
                                    num4++;
                                }
                                else
                                {
                                    string str4 = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(_config3.prop_id - 1).model_major;
                                    this.mapInfos.Add(new MapInfo(str4, _config3.track, (float) _config3.distance, _config3.height, _config3.prop_id));
                                    num4++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable3 = enumerator3 as IDisposable;
                            if (disposable3 == null)
                            {
                            }
                            disposable3.Dispose();
                        }
                        goto Label_04D0;
                    }
                    case 3:
                    {
                        list = ConfigMgr.getInstance().getList<chapter4prop_config>();
                        num3 = ((randomPropIndex + 1) < strArray.Length) ? int.Parse(strArray[randomPropIndex]) : list.Count;
                        IEnumerator enumerator4 = list.GetEnumerator();
                        try
                        {
                            while (enumerator4.MoveNext())
                            {
                                chapter4prop_config _config4 = (chapter4prop_config) enumerator4.Current;
                                if (num4 > num3)
                                {
                                    goto Label_062A;
                                }
                                if (num4 < num2)
                                {
                                    num4++;
                                }
                                else
                                {
                                    string str5 = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(_config4.prop_id - 1).model_major;
                                    this.mapInfos.Add(new MapInfo(str5, _config4.track, (float) _config4.distance, _config4.height, _config4.prop_id));
                                    num4++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable4 = enumerator4 as IDisposable;
                            if (disposable4 == null)
                            {
                            }
                            disposable4.Dispose();
                        }
                        goto Label_062A;
                    }
                    case 4:
                    {
                        list = ConfigMgr.getInstance().getList<chapter5prop_config>();
                        num3 = ((randomPropIndex + 1) < strArray.Length) ? int.Parse(strArray[randomPropIndex]) : list.Count;
                        IEnumerator enumerator5 = list.GetEnumerator();
                        try
                        {
                            while (enumerator5.MoveNext())
                            {
                                chapter5prop_config _config5 = (chapter5prop_config) enumerator5.Current;
                                if (num4 > num3)
                                {
                                    goto Label_0784;
                                }
                                if (num4 < num2)
                                {
                                    num4++;
                                }
                                else
                                {
                                    string str6 = ConfigMgr.getInstance().getByEntry<guildrun_prop_config>(_config5.prop_id - 1).model_major;
                                    this.mapInfos.Add(new MapInfo(str6, _config5.track, (float) _config5.distance, _config5.height, _config5.prop_id));
                                    num4++;
                                }
                            }
                        }
                        finally
                        {
                            IDisposable disposable5 = enumerator5 as IDisposable;
                            if (disposable5 == null)
                            {
                            }
                            disposable5.Dispose();
                        }
                        goto Label_0784;
                    }
                }
            }
        }
        goto Label_0800;
    Label_021C:
        objArray1 = new object[] { "chapter1prop_config :", num2, "|", num3, "|", this.mapInfos[this.mapInfos.Count - 1].propName, "|", this.mapInfos.Count };
        Debug.Log(string.Concat(objArray1));
        goto Label_0800;
    Label_0376:
        objArray2 = new object[] { "chapter2prop_config :", num2, "|", num3, "|", this.mapInfos[this.mapInfos.Count - 1].propName, "|", this.mapInfos.Count };
        Debug.Log(string.Concat(objArray2));
        goto Label_0800;
    Label_04D0:
        objArray3 = new object[] { "chapter3prop_config :", num2, "|", num3, "|", this.mapInfos[this.mapInfos.Count - 1].propName, "|", this.mapInfos.Count };
        Debug.Log(string.Concat(objArray3));
        goto Label_0800;
    Label_062A:
        objArray4 = new object[] { "chapter4prop_config :", num2, "|", num3, "|", this.mapInfos[this.mapInfos.Count - 1].propName, "|", this.mapInfos.Count };
        Debug.Log(string.Concat(objArray4));
        goto Label_0800;
    Label_0784:
        objArray5 = new object[] { "chapter5prop_config :", num2, "|", num3, "|", this.mapInfos[this.mapInfos.Count - 1].propName, "|", this.mapInfos.Count };
        Debug.Log(string.Concat(objArray5));
    Label_0800:
        this.mapInfos.ForEach(o => this.maxRunDistance += o.distanceFromLastProp);
        this.InitMap();
    }

    public void SetParkouChapteAndRandomPropIndex(int rPropIndex, int cIndex)
    {
        this.randomPropIndex = rPropIndex;
        this.chapteIndex = cIndex;
    }

    public void UpdateProp()
    {
        if ((this.mapInfos != null) && (this.startPropIndex <= (this.mapInfos.Count - 1)))
        {
            this.CareatPropAndCheckPropGroup(this.mapInfos[this.startPropIndex]);
            this.startPropIndex++;
        }
    }

    public static ParkourInit _instance
    {
        get
        {
            m_Instance = (m_Instance != null) ? m_Instance : new ParkourInit();
            return m_Instance;
        }
    }

    [CompilerGenerated]
    private sealed class <InitMap>c__AnonStorey290
    {
        internal int coinCount;

        internal void <>m__5F6(PropInfo o)
        {
            if (o.type == PropType.JINGBI)
            {
                this.coinCount++;
            }
        }
    }
}

