using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
    public string keyName;
    private UIToggle mCheck;
    private UIPopupList mList;

    private void Awake()
    {
        this.mList = base.GetComponent<UIPopupList>();
        this.mCheck = base.GetComponent<UIToggle>();
    }

    private void OnDisable()
    {
        if (this.mCheck != null)
        {
            EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
        }
        if (this.mList != null)
        {
            EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
        }
        if ((this.mCheck == null) && (this.mList == null))
        {
            UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UIToggle toggle = componentsInChildren[index];
                if (toggle.value)
                {
                    PlayerPrefs.SetString(this.key, toggle.name);
                    break;
                }
                index++;
            }
        }
    }

    private void OnEnable()
    {
        if (this.mList != null)
        {
            EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
        }
        if (this.mCheck != null)
        {
            EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
        }
        if (this.mList != null)
        {
            string str = PlayerPrefs.GetString(this.key);
            if (!string.IsNullOrEmpty(str))
            {
                this.mList.value = str;
            }
        }
        else if (this.mCheck != null)
        {
            this.mCheck.value = PlayerPrefs.GetInt(this.key, 1) != 0;
        }
        else
        {
            string str2 = PlayerPrefs.GetString(this.key);
            UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
            int index = 0;
            int length = componentsInChildren.Length;
            while (index < length)
            {
                UIToggle toggle = componentsInChildren[index];
                toggle.value = toggle.name == str2;
                index++;
            }
        }
    }

    public void SaveSelection()
    {
        PlayerPrefs.SetString(this.key, UIPopupList.current.value);
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt(this.key, !UIToggle.current.value ? 0 : 1);
    }

    private string key
    {
        get
        {
            return (!string.IsNullOrEmpty(this.keyName) ? this.keyName : ("NGUI State: " + base.name));
        }
    }
}

