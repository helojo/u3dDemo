using System;
using System.Collections.Generic;
using UnityEngine;

public static class Localization
{
    public static bool localizationHasBeenSet = false;
    private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();
    private static string mLanguage;
    private static int mLanguageIndex = -1;
    private static string[] mLanguages = null;
    private static Dictionary<string, string> mOldDictionary = new Dictionary<string, string>();

    private static void AddCSV(BetterList<string> values)
    {
        if (values.size >= 2)
        {
            string[] strArray = new string[values.size - 1];
            for (int i = 1; i < values.size; i++)
            {
                strArray[i - 1] = values[i];
            }
            try
            {
                mDictionary.Add(values[0], strArray);
            }
            catch (Exception exception)
            {
                Debug.LogError("Unable to add '" + values[0] + "' to the Localization dictionary.\n" + exception.Message);
            }
        }
    }

    public static bool Exists(string key)
    {
        if (!localizationHasBeenSet)
        {
            language = PlayerPrefs.GetString("Language", "English");
        }
        string str = key + " Mobile";
        return (mDictionary.ContainsKey(str) || (mOldDictionary.ContainsKey(str) || (mDictionary.ContainsKey(key) || mOldDictionary.ContainsKey(key))));
    }

    public static string Get(string key)
    {
        string str;
        string[] strArray;
        if (!localizationHasBeenSet)
        {
            language = PlayerPrefs.GetString("Language", "English");
        }
        string str2 = key + " Mobile";
        if ((mLanguageIndex != -1) && mDictionary.TryGetValue(str2, out strArray))
        {
            if (mLanguageIndex < strArray.Length)
            {
                return strArray[mLanguageIndex];
            }
        }
        else if (mOldDictionary.TryGetValue(str2, out str))
        {
            return str;
        }
        if ((mLanguageIndex != -1) && mDictionary.TryGetValue(key, out strArray))
        {
            if (mLanguageIndex >= strArray.Length)
            {
                return key;
            }
            return strArray[mLanguageIndex];
        }
        if (mOldDictionary.TryGetValue(key, out str))
        {
            return str;
        }
        return key;
    }

    public static void Load(TextAsset asset)
    {
        Set(asset.name, new ByteReader(asset).ReadDictionary());
    }

    private static bool LoadAndSelect(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            if ((mDictionary.Count == 0) && !LoadDictionary(value))
            {
                return false;
            }
            if (SelectLanguage(value))
            {
                return true;
            }
        }
        if (mOldDictionary.Count > 0)
        {
            return true;
        }
        mOldDictionary.Clear();
        mDictionary.Clear();
        if (string.IsNullOrEmpty(value))
        {
            PlayerPrefs.DeleteKey("Language");
        }
        return false;
    }

    public static bool LoadCSV(TextAsset asset)
    {
        ByteReader reader = new ByteReader(asset);
        BetterList<string> values = reader.ReadCSV();
        if (values.size < 2)
        {
            return false;
        }
        values[0] = "KEY";
        if (!string.Equals(values[0], "KEY"))
        {
            Debug.LogError("Invalid localization CSV file. The first value is expected to be 'KEY', followed by language columns.\nInstead found '" + values[0] + "'", asset);
            return false;
        }
        mLanguages = new string[values.size - 1];
        for (int i = 0; i < mLanguages.Length; i++)
        {
            mLanguages[i] = values[i + 1];
        }
        mDictionary.Clear();
        while (values != null)
        {
            AddCSV(values);
            values = reader.ReadCSV();
        }
        return true;
    }

    private static bool LoadDictionary(string value)
    {
        TextAsset asset = !localizationHasBeenSet ? (Resources.Load("Localization", typeof(TextAsset)) as TextAsset) : null;
        localizationHasBeenSet = true;
        if ((asset != null) && LoadCSV(asset))
        {
            return true;
        }
        if (!string.IsNullOrEmpty(value))
        {
            asset = Resources.Load(value, typeof(TextAsset)) as TextAsset;
            if (asset != null)
            {
                Load(asset);
                return true;
            }
        }
        return false;
    }

    [Obsolete("Use Localization.Get instead")]
    public static string Localize(string key)
    {
        return Get(key);
    }

    private static bool SelectLanguage(string language)
    {
        string[] strArray;
        mLanguageIndex = -1;
        if ((mDictionary.Count != 0) && mDictionary.TryGetValue("KEY", out strArray))
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i] == language)
                {
                    mOldDictionary.Clear();
                    mLanguageIndex = i;
                    mLanguage = language;
                    PlayerPrefs.SetString("Language", mLanguage);
                    UIRoot.Broadcast("OnLocalize");
                    return true;
                }
            }
        }
        return false;
    }

    public static void Set(string languageName, Dictionary<string, string> dictionary)
    {
        mLanguage = languageName;
        PlayerPrefs.SetString("Language", mLanguage);
        mOldDictionary = dictionary;
        localizationHasBeenSet = false;
        mLanguageIndex = -1;
        mLanguages = new string[] { languageName };
        UIRoot.Broadcast("OnLocalize");
    }

    public static Dictionary<string, string[]> dictionary
    {
        get
        {
            if (!localizationHasBeenSet)
            {
                language = PlayerPrefs.GetString("Language", "English");
            }
            return mDictionary;
        }
        set
        {
            localizationHasBeenSet = value != null;
            mDictionary = value;
        }
    }

    public static string[] knownLanguages
    {
        get
        {
            if (!localizationHasBeenSet)
            {
                LoadDictionary(PlayerPrefs.GetString("Language", "English"));
            }
            return mLanguages;
        }
    }

    public static string language
    {
        get
        {
            if (string.IsNullOrEmpty(mLanguage))
            {
                string[] knownLanguages = Localization.knownLanguages;
                mLanguage = PlayerPrefs.GetString("Language", (knownLanguages == null) ? "English" : knownLanguages[0]);
                LoadAndSelect(mLanguage);
            }
            return mLanguage;
        }
        set
        {
            if (mLanguage != value)
            {
                mLanguage = value;
                LoadAndSelect(value);
            }
        }
    }
}

