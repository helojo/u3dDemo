using fastJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneDynamicChanger : MonoBehaviour
{
    public string configTextName;
    private List<AddEffectInfo> effectInfoes = new List<AddEffectInfo>();

    private void Start()
    {
        if (!string.IsNullOrEmpty(this.configTextName))
        {
            TextAsset asset = BundleMgr.Instance.LoadResource(this.configTextName, ".txt", typeof(TextAsset)) as TextAsset;
            List<object> list = (List<object>) JSON.Instance.ToObject<Dictionary<string, object>>(asset.text)["AddEffect"];
            foreach (Dictionary<string, object> dictionary2 in list)
            {
                AddEffectInfo item = new AddEffectInfo {
                    resouceName = dictionary2["resouceName"].ToString(),
                    pos = StrParser.ParseVec3(dictionary2["pos"].ToString()),
                    scale = StrParser.ParseVec3(dictionary2["scale"].ToString()),
                    rot = StrParser.ParseVec3(dictionary2["rot"].ToString()),
                    loop = StrParser.ParseBool(dictionary2["loop"].ToString(), false),
                    cdMin = StrParser.ParseFloat(dictionary2["cdMin"].ToString()),
                    cdMax = StrParser.ParseFloat(dictionary2["cdMax"].ToString()),
                    deadTime = StrParser.ParseFloat(dictionary2["deadTime"].ToString()),
                    startDateTime = DateTime.Parse(dictionary2["startTime"].ToString()),
                    endDateTime = DateTime.Parse(dictionary2["endTime"].ToString())
                };
                object obj2 = null;
                if (dictionary2.TryGetValue("sound", out obj2))
                {
                    item.soundName = obj2.ToString();
                }
                item.startTime = UnityEngine.Random.Range(item.cdMin, item.cdMax);
                this.effectInfoes.Add(item);
            }
        }
    }

    private void Update()
    {
        foreach (AddEffectInfo info in this.effectInfoes)
        {
            info.Tick();
        }
    }

    public class AddEffectInfo
    {
        public float cdMax;
        public float cdMin;
        public float deadTime;
        public GameObject effectObj;
        public DateTime endDateTime;
        public bool loop;
        public Vector3 pos;
        public float remainTime;
        public string resouceName;
        public Vector3 rot;
        public Vector3 scale;
        public string soundName;
        public DateTime startDateTime;
        public float startTime;

        public void Tick()
        {
            if (this.effectObj == null)
            {
                if ((DateTime.Compare(TimeMgr.Instance.ServerDateTime, this.startDateTime) >= 0) && (DateTime.Compare(TimeMgr.Instance.ServerDateTime, this.endDateTime) < 0))
                {
                    this.startTime -= Time.deltaTime;
                    if (this.startTime < 0f)
                    {
                        this.effectObj = ObjectManager.CreateObj(this.resouceName);
                        this.effectObj.transform.position = this.pos;
                        this.effectObj.transform.localScale = this.scale;
                        this.effectObj.transform.rotation = Quaternion.Euler(this.rot);
                        this.remainTime = this.deadTime;
                        if (!string.IsNullOrEmpty(this.soundName))
                        {
                            SoundManager.mInstance.PlayMusic(this.soundName);
                        }
                    }
                }
            }
            else if (this.deadTime > 0f)
            {
                this.remainTime -= Time.deltaTime;
                if (this.remainTime < 0f)
                {
                    ObjectManager.DestoryObj(this.effectObj);
                    this.effectObj = null;
                    if (this.loop)
                    {
                        this.startTime = UnityEngine.Random.Range(this.cdMin, this.cdMax);
                    }
                }
            }
        }
    }
}

