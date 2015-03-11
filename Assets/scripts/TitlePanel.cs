using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TitlePanel : GUIEntity
{
    [CompilerGenerated]
    private static Comparison<TitleData> <>f__am$cache9;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private Dictionary<int, int> mCompleteTitleDict = new Dictionary<int, int>();
    private GameObject mDefaultTitleGo;
    private bool mIsStart = true;
    private Dictionary<TitleData, GameObject> mUpdateTitleDict = new Dictionary<TitleData, GameObject>();
    public string SetTitle;
    public GameObject SingleTitleItem;
    private bool TitleChanged;

    public void initTitleList()
    {
        Debug.Log(ActorData.getInstance().TitleList.Count + "   ---------");
        UITable component = base.transform.FindChild("List/Grid").GetComponent<UITable>();
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = (t1, t2) => t2.title_entry - t1.title_entry;
        }
        ActorData.getInstance().TitleList.Sort(<>f__am$cache9);
        this.mCompleteTitleDict.Clear();
        foreach (TitleData data in ActorData.getInstance().TitleList)
        {
            user_title_config _config = ConfigMgr.getInstance().getByEntry<user_title_config>(data.title_entry);
            if (_config != null)
            {
                if (this.mCompleteTitleDict.ContainsKey(_config.group_id))
                {
                    if (ConfigMgr.getInstance().getByEntry<user_title_config>(this.mCompleteTitleDict[_config.group_id]).group_sub_id < _config.group_sub_id)
                    {
                        this.mCompleteTitleDict[_config.group_id] = data.title_entry;
                    }
                }
                else
                {
                    this.mCompleteTitleDict.Add(_config.group_id, data.title_entry);
                }
            }
        }
        this.mUpdateTitleDict.Clear();
        foreach (TitleData data2 in ActorData.getInstance().TitleList)
        {
            if ((data2.end_time <= 0) || (TimeMgr.Instance.ServerStampTime <= data2.end_time))
            {
                user_title_config utc = ConfigMgr.getInstance().getByEntry<user_title_config>(data2.title_entry);
                if ((utc != null) && (utc.is_visible || (utc.addit_type_1 == 2)))
                {
                    GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleTitleItem) as GameObject;
                    obj2.transform.parent = component.transform;
                    obj2.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                    obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                    this.SetTitleInfo(obj2.transform, utc);
                    if ((data2.end_time > TimeMgr.Instance.ServerStampTime) && !this.mUpdateTitleDict.ContainsKey(data2))
                    {
                        this.mUpdateTitleDict.Add(data2, obj2);
                    }
                }
            }
        }
        if (this.mUpdateTitleDict.Count > 0)
        {
            this.mIsStart = true;
        }
        Debug.Log(this.mCompleteTitleDict.Count + "   ====================");
        ArrayList list = ConfigMgr.getInstance().getList<user_title_config>();
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                user_title_config current = (user_title_config) enumerator.Current;
                if (current.is_visible && !ActorData.getInstance().mTitleEntryList.Contains(current.entry))
                {
                    if (dictionary.ContainsKey(current.group_id))
                    {
                        user_title_config _config5 = ConfigMgr.getInstance().getByEntry<user_title_config>(dictionary[current.group_id]);
                        user_title_config _config6 = ConfigMgr.getInstance().getByEntry<user_title_config>(current.entry);
                        if (((_config5 != null) && (_config6 != null)) && (_config5.group_sub_id > _config6.group_sub_id))
                        {
                            dictionary[current.group_id] = current.entry;
                        }
                    }
                    else
                    {
                        dictionary.Add(current.group_id, current.entry);
                    }
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
        foreach (int num in dictionary.Values)
        {
            user_title_config _config7 = ConfigMgr.getInstance().getByEntry<user_title_config>(num);
            if (_config7 != null)
            {
                GameObject obj3 = UnityEngine.Object.Instantiate(this.SingleTitleItem) as GameObject;
                obj3.transform.parent = component.transform;
                obj3.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                this.SetTitleInfo(obj3.transform, _config7);
            }
        }
        component.repositionNow = true;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        this.SetCurrTitle();
        SocketMgr.Instance.RequestGetQuestList();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mIsStart)
        {
            this.m_time += Time.deltaTime;
            if (this.m_time > this.m_updateInterval)
            {
                this.m_time = 0f;
                foreach (KeyValuePair<TitleData, GameObject> pair in this.mUpdateTitleDict)
                {
                    UILabel component = pair.Value.transform.FindChild("RemainnTime").GetComponent<UILabel>();
                    Transform transform = pair.Value.transform.FindChild("SelectBtn");
                    if (pair.Key.end_time > TimeMgr.Instance.ServerStampTime)
                    {
                        component.text = ConfigMgr.getInstance().GetWord(0x2d5) + TimeMgr.Instance.GetRemainTime3(pair.Key.end_time);
                        if (!transform.active)
                        {
                            transform.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        component.text = ConfigMgr.getInstance().GetWord(0x272b);
                        if (transform.active)
                        {
                            transform.gameObject.SetActive(false);
                        }
                        this.SetCurrTitle();
                        if (this.mDefaultTitleGo != null)
                        {
                            this.mDefaultTitleGo.transform.FindChild("SelectBtn").GetComponent<UIToggle>().isChecked = true;
                        }
                        PlayerInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<PlayerInfoPanel>();
                        if (activityGUIEntity != null)
                        {
                            activityGUIEntity.UpdateTitle();
                        }
                    }
                }
            }
        }
    }

    private void SelectBtnClick(GameObject go)
    {
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            int num = (int) obj2;
            if (ActorData.getInstance().UserInfo.titleEntry != num)
            {
                SocketMgr.Instance.RequestSetUserTitle(num);
            }
            if (num != 0x16)
            {
                SettingMgr.mInstance.SetTitleEntryInt(ActorData.getInstance().SessionInfo.userid + " " + num, 1);
                go.transform.FindChild("Tips").gameObject.SetActive(false);
            }
        }
    }

    public void SetCurrTitle()
    {
        string name;
        UILabel component = base.transform.FindChild("Info/Title").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Info/Desc").GetComponent<UILabel>();
        UITexture texture = base.transform.FindChild("Info/Icon").GetComponent<UITexture>();
        int id = 0x16;
        if ((TimeMgr.Instance.ServerStampTime < ActorData.getInstance().UserInfo.title_time) || (ActorData.getInstance().UserInfo.title_time == ulong.MaxValue))
        {
            id = ActorData.getInstance().UserInfo.titleEntry;
        }
        else
        {
            ActorData.getInstance().UserInfo.titleEntry = id;
        }
        user_title_config _config = ConfigMgr.getInstance().getByEntry<user_title_config>(id);
        if (_config == null)
        {
            name = string.Empty;
            component.text = name;
            this.SetTitle = name;
            label2.text = string.Empty;
            texture.mainTexture = BundleMgr.Instance.CreateTitleIcon("0");
        }
        else
        {
            name = _config.name;
            component.text = name;
            this.SetTitle = name;
            label2.text = _config.description;
            texture.mainTexture = BundleMgr.Instance.CreateTitleIcon(_config.icon.ToString());
        }
    }

    private void SetDescriptInfo(Transform obj, user_title_config utc)
    {
        <SetDescriptInfo>c__AnonStorey21F storeyf = new <SetDescriptInfo>c__AnonStorey21F {
            utc = utc
        };
        UILabel component = obj.transform.FindChild("tween/descript/Desc").GetComponent<UILabel>();
        component.text = storeyf.utc.achieve_description;
        if (ActorData.getInstance().mTitleEntryList.Contains(storeyf.utc.entry))
        {
            component.text = component.text + "   [930b00]" + ConfigMgr.getInstance().GetWord(840);
        }
        else
        {
            Quest quest = ActorData.getInstance().QuestList.Find(new Predicate<Quest>(storeyf.<>m__467));
            quest_config _config = ConfigMgr.getInstance().getByEntry<quest_config>(storeyf.utc.quest_entry);
            if (_config != null)
            {
                switch (storeyf.utc.group_id)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    {
                        int num = 0;
                        if (quest != null)
                        {
                            num = quest.countList[0];
                        }
                        string text = component.text;
                        object[] objArray1 = new object[] { text, "   [930b00](", _config.count_base + num, "/", _config.count_base + _config.finish_count1, ")" };
                        component.text = string.Concat(objArray1);
                        break;
                    }
                    case 13:
                    case 14:
                    case 15:
                    {
                        bool flag = false;
                        if (quest != null)
                        {
                            flag = quest.is_finish;
                        }
                        component.text = component.text + (!flag ? ("   " + ConfigMgr.getInstance().GetWord(0x349)) : ("   " + ConfigMgr.getInstance().GetWord(840)));
                        break;
                    }
                }
            }
        }
    }

    private void SetTitleInfo(Transform obj, user_title_config utc)
    {
        if (utc != null)
        {
            obj.transform.FindChild("Name").GetComponent<UILabel>().text = utc.name;
            obj.transform.FindChild("Desc").GetComponent<UILabel>().text = utc.description;
            UIToggle component = obj.FindChild("SelectBtn").GetComponent<UIToggle>();
            component.group = 1;
            UISprite sprite = obj.FindChild("Border").GetComponent<UISprite>();
            UISprite sprite2 = obj.FindChild("TextBorder").GetComponent<UISprite>();
            UITexture texture = obj.FindChild("Icon").GetComponent<UITexture>();
            texture.mainTexture = BundleMgr.Instance.CreateTitleIcon(utc.icon.ToString());
            UILabel label3 = obj.FindChild("RemainnTime").GetComponent<UILabel>();
            if (utc.entry == 0x16)
            {
                this.mDefaultTitleGo = obj.gameObject;
            }
            if (ActorData.getInstance().mTitleEntryList.Contains(utc.entry))
            {
                UIEventListener listener = obj.FindChild("SelectBtn").GetComponent<UIEventListener>();
                GUIDataHolder.setData(component.gameObject, utc.entry);
                listener.onClick = new UIEventListener.VoidDelegate(this.SelectBtnClick);
                if (ActorData.getInstance().UserInfo.titleEntry == utc.entry)
                {
                    component.isChecked = true;
                }
                nguiTextureGrey.doChangeEnableGrey(texture, false);
            }
            else
            {
                component.gameObject.SetActive(false);
                nguiTextureGrey.doChangeEnableGrey(texture, true);
                label3.text = string.Empty;
            }
            GameObject gameObject = obj.FindChild("SelectBtn/Tips").gameObject;
            bool flag = false;
            if (utc.entry != 0x16)
            {
                flag = SettingMgr.mInstance.GetTitleEntryInt(ActorData.getInstance().SessionInfo.userid + " " + utc.entry) == 0;
            }
            gameObject.SetActive(flag);
            this.SetDescriptInfo(obj, utc);
        }
    }

    public void UpdateTitle()
    {
        this.SetCurrTitle();
    }

    [CompilerGenerated]
    private sealed class <SetDescriptInfo>c__AnonStorey21F
    {
        internal user_title_config utc;

        internal bool <>m__467(Quest e)
        {
            return (e.entry == this.utc.quest_entry);
        }
    }
}

