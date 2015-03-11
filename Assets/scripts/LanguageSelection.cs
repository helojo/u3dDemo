using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Language Selection"), RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
    private UIPopupList mList;

    private void OnChange()
    {
        Localization.language = UIPopupList.current.value;
    }

    private void Start()
    {
        this.mList = base.GetComponent<UIPopupList>();
        if (Localization.knownLanguages != null)
        {
            this.mList.items.Clear();
            int index = 0;
            int length = Localization.knownLanguages.Length;
            while (index < length)
            {
                this.mList.items.Add(Localization.knownLanguages[index]);
                index++;
            }
            this.mList.value = Localization.language;
        }
        EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.OnChange));
    }
}

