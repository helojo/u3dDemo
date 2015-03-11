using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class IniParser
{
    private bool fromBundle;
    private string iniFilePath;
    public bool isDurty;
    private Dictionary<SectionPair, string> keyPairs = new Dictionary<SectionPair, string>();

    public IniParser(string iniPath, bool fromBundle, bool IsStream)
    {
        this.fromBundle = fromBundle;
        this.iniFilePath = iniPath;
        List<string> lines = new List<string>();
        if (fromBundle)
        {
            TextAsset asset = Resources.Load(iniPath) as TextAsset;
            if (asset == null)
            {
                Debug.LogError(iniPath + " open failed");
            }
            lines = StrParser.ParseStringList(asset.text, "\n");
        }
        else if (IsStream)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(iniPath);
                while (!www.isDone)
                {
                }
                lines = StrParser.ParseStringList(www.text, "\n");
            }
            else if (File.Exists(iniPath))
            {
                foreach (string str in File.ReadAllLines(iniPath))
                {
                    lines.Add(str);
                }
            }
        }
        else if (File.Exists(iniPath))
        {
            foreach (string str2 in File.ReadAllLines(iniPath))
            {
                lines.Add(str2);
            }
        }
        this.ParseText(lines);
    }

    public void AddSetting(string sectionName, string settingName)
    {
        this.AddSetting(sectionName, settingName, (string) null);
    }

    public void AddSetting(string sectionName, string setting, bool value)
    {
        this.AddSetting(sectionName, setting, value.ToString());
    }

    public void AddSetting(string sectionName, string setting, int value)
    {
        this.AddSetting(sectionName, setting, value.ToString());
    }

    public void AddSetting(string sectionName, string setting, long value)
    {
        this.AddSetting(sectionName, setting, value.ToString());
    }

    public void AddSetting(string sectionName, string setting, float value)
    {
        this.AddSetting(sectionName, setting, value.ToString());
    }

    public void AddSetting(string sectionName, string settingName, string settingValue)
    {
        SectionPair pair;
        pair.Section = sectionName;
        pair.Key = settingName;
        if (this.keyPairs.ContainsKey(pair))
        {
            this.keyPairs.Remove(pair);
        }
        this.keyPairs.Add(pair, settingValue);
        this.isDurty = true;
    }

    public void DeleteSetting(string sectionName, string settingName)
    {
        SectionPair pair;
        pair.Section = sectionName;
        pair.Key = settingName;
        if (this.keyPairs.ContainsKey(pair))
        {
            this.keyPairs.Remove(pair);
        }
        this.isDurty = true;
    }

    public string[] EnumSection(string sectionName)
    {
        ArrayList list = new ArrayList();
        foreach (SectionPair pair in this.keyPairs.Keys)
        {
            if (pair.Section == sectionName)
            {
                list.Add(pair.Key);
            }
        }
        return (string[]) list.ToArray(typeof(string));
    }

    public string GetSetting(string sectionName, string settingName)
    {
        return this.GetSetting(sectionName, settingName, string.Empty);
    }

    public string GetSetting(string sectionName, string settingName, string defaultValue)
    {
        SectionPair pair;
        pair.Section = sectionName;
        pair.Key = settingName;
        if (this.keyPairs.ContainsKey(pair))
        {
            return this.keyPairs[pair];
        }
        return defaultValue;
    }

    public bool GetSettingBool(string sectionName, string settingName, bool defaultValue)
    {
        string setting = this.GetSetting(sectionName, settingName);
        if (string.IsNullOrEmpty(setting))
        {
            return defaultValue;
        }
        return StrParser.ParseBool(setting, defaultValue);
    }

    public float GetSettingFloat(string sectionName, string settingName, float defaultValue)
    {
        string setting = this.GetSetting(sectionName, settingName);
        if (string.IsNullOrEmpty(setting))
        {
            return defaultValue;
        }
        return StrParser.ParseFloat(setting, defaultValue);
    }

    public int GetSettingInt(string sectionName, string settingName, int defaultValue)
    {
        string setting = this.GetSetting(sectionName, settingName);
        if (string.IsNullOrEmpty(setting))
        {
            return defaultValue;
        }
        return StrParser.ParseDecInt(setting, defaultValue);
    }

    public long GetSettingLong(string sectionName, string settingName, long defaultValue)
    {
        string setting = this.GetSetting(sectionName, settingName);
        if (string.IsNullOrEmpty(setting))
        {
            return defaultValue;
        }
        return StrParser.ParseInt64(setting, defaultValue);
    }

    private void ParseText(List<string> lines)
    {
        string str = null;
        string[] strArray = null;
        foreach (string str2 in lines)
        {
            string str3 = str2.Trim();
            if (str3 != string.Empty)
            {
                if (str3.StartsWith("[") && str3.EndsWith("]"))
                {
                    str = str3.Substring(1, str3.Length - 2);
                }
                else
                {
                    SectionPair pair;
                    char[] separator = new char[] { '=' };
                    strArray = str3.Split(separator, 2);
                    string str4 = null;
                    if (str == null)
                    {
                        str = "ROOT";
                    }
                    pair.Section = str;
                    pair.Key = strArray[0].Trim();
                    if (strArray.Length > 1)
                    {
                        str4 = strArray[1].Trim();
                    }
                    this.keyPairs.Add(pair, str4);
                }
            }
        }
    }

    public void SaveSettings()
    {
        if (this.isDurty)
        {
            string iniFilePath = this.iniFilePath;
            this.SaveSettings(iniFilePath);
            this.isDurty = false;
        }
    }

    private void SaveSettings(string newFilePath)
    {
        ArrayList list = new ArrayList();
        string str = string.Empty;
        StringBuilder builder = new StringBuilder();
        foreach (SectionPair pair in this.keyPairs.Keys)
        {
            if (!list.Contains(pair.Section))
            {
                list.Add(pair.Section);
            }
        }
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                string current = (string) enumerator.Current;
                builder.AppendFormat("[{0}]\r\n", current);
                foreach (SectionPair pair2 in this.keyPairs.Keys)
                {
                    if (pair2.Section == current)
                    {
                        str = this.keyPairs[pair2];
                        if (str != null)
                        {
                            str = "=" + str;
                        }
                        builder.Append(pair2.Key);
                        builder.Append(str);
                        builder.Append("\r\n");
                    }
                }
                builder.Append("\r\n");
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
        try
        {
            StreamWriter writer = File.CreateText(newFilePath);
            writer.Write(builder.ToString());
            writer.Flush();
            writer.Close();
        }
        catch (Exception exception)
        {
            Debug.LogWarning("SaveSettings" + exception);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SectionPair
    {
        public string Section;
        public string Key;
    }
}

