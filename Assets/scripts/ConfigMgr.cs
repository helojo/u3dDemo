using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class ConfigMgr : XSingleton<ConfigMgr>
{
    private Dictionary<System.Type, Dictionary<int, ConfigExtensible>> _configMap = new Dictionary<System.Type, Dictionary<int, ConfigExtensible>>();
    private Dictionary<System.Type, ArrayList> _configTypeMap = new Dictionary<System.Type, ArrayList>();
    private Dictionary<int, loading_config> m_loading_config;
    private List<string> m_maskwords;
    private Dictionary<OpResult, err_code_config> m_opresult_entry_map;
    private Dictionary<int, word_config> m_word_config;

    public bool CheckContentValid(UIInput input, out string invalidStr)
    {
        invalidStr = string.Empty;
        char[] chArray = new char[] { '*' };
        string maskWord = this.GetMaskWord(input);
        maskWord = input.label.font.trimNotExistChar(maskWord);
        for (int i = 0; i < chArray.Length; i++)
        {
            if (maskWord.IndexOf(chArray[i]) >= 0)
            {
                invalidStr = chArray[i].ToString();
                return false;
            }
        }
        return true;
    }

    private void checkMaskWord()
    {
        if (this.m_maskwords == null)
        {
            this.m_maskwords = new List<string>();
            foreach (maskword_config _config in this.LoadDataArray<maskword_config>())
            {
                this.m_maskwords.Add(_config.key);
            }
        }
    }

    public bool CheckUserNameValid(UIInput input, out string invalidStr)
    {
        invalidStr = string.Empty;
        char[] chArray = new char[] { '$', '#', '@', '*', '%', '^', '&', ':', ';', ' ', ',', '\'', '"', '\0' };
        string maskWord = this.GetMaskWord(input);
        maskWord = input.label.font.trimNotExistChar(maskWord);
        for (int i = 0; i < chArray.Length; i++)
        {
            if (maskWord.IndexOf(chArray[i]) >= 0)
            {
                invalidStr = chArray[i].ToString();
                return false;
            }
        }
        return true;
    }

    public T getByEntry<T>(int id) where T: ConfigExtensible
    {
        Dictionary<int, ConfigExtensible> dictionary;
        ConfigExtensible extensible;
        System.Type key = typeof(T);
        if (!this._configMap.TryGetValue(key, out dictionary))
        {
            ArrayList list;
            this.InitConfig<T>(out list, out dictionary);
        }
        if ((dictionary != null) && dictionary.TryGetValue(id, out extensible))
        {
            return (extensible as T);
        }
        return null;
    }

    public string GetErrorCode(OpResult result)
    {
        err_code_config _config2;
        if (this.m_opresult_entry_map == null)
        {
            this.m_opresult_entry_map = new Dictionary<OpResult, err_code_config>();
            IEnumerator enumerator = this.getList<err_code_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    err_code_config current = (err_code_config) enumerator.Current;
                    this.m_opresult_entry_map.Add(current.err_code, current);
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
        }
        if (this.m_opresult_entry_map.TryGetValue(result, out _config2))
        {
            return _config2.cn;
        }
        return string.Empty;
    }

    public static ConfigMgr getInstance()
    {
        return XSingleton<ConfigMgr>.Singleton;
    }

    public ArrayList getList<T>() where T: ConfigExtensible
    {
        System.Type key = typeof(T);
        ArrayList list = new ArrayList();
        if (!this._configTypeMap.TryGetValue(key, out list))
        {
            Dictionary<int, ConfigExtensible> dictionary;
            this.InitConfig<T>(out list, out dictionary);
        }
        return list;
    }

    public List<T> getListResult<T>() where T: ConfigExtensible
    {
        ArrayList list = this.getList<T>();
        List<T> list2 = new List<T>(list.Count);
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                list2.Add(current as T);
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
        return list2;
    }

    public string GetLoadingTips()
    {
        if (this.m_loading_config == null)
        {
            this.m_loading_config = new Dictionary<int, loading_config>();
            IEnumerator enumerator = this.getList<loading_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    loading_config current = (loading_config) enumerator.Current;
                    current.CN = current.CN.Replace(@"\n", "\n");
                    this.m_loading_config.Add(current.entry, current);
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
        }
        string str = string.Empty;
        int key = UnityEngine.Random.Range(0, this.m_loading_config.Count);
        if (!this.m_loading_config.ContainsKey(key))
        {
            return str;
        }
        loading_config _config2 = this.m_loading_config[key];
        return _config2.CN;
    }

    public string GetMaskWord(string text)
    {
        this.checkMaskWord();
        string targetString = text;
        for (int i = 0; i < this.m_maskwords.Count; i++)
        {
            targetString = StrFinder.GetOccurences(this.m_maskwords[i], targetString);
        }
        return targetString;
    }

    public string GetMaskWord(UIInput input)
    {
        this.checkMaskWord();
        string text = input.text;
        for (int i = 0; i < this.m_maskwords.Count; i++)
        {
            text = StrFinder.GetOccurences(this.m_maskwords[i], text);
        }
        if (input.label.font != null)
        {
            text = input.label.font.trimNotExistChar(text);
        }
        return text;
    }

    public string GetWord(int _entry)
    {
        if (this.m_word_config == null)
        {
            this.m_word_config = new Dictionary<int, word_config>();
            IEnumerator enumerator = this.getList<word_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    word_config current = (word_config) enumerator.Current;
                    current.CN = current.CN.Replace(@"\n", "\n");
                    this.m_word_config.Add(current.entry, current);
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
        }
        string str = string.Empty;
        if (!this.m_word_config.ContainsKey(_entry))
        {
            return str;
        }
        word_config _config2 = this.m_word_config[_entry];
        return _config2.CN;
    }

    private void InitConfig<T>(out ArrayList data, out Dictionary<int, ConfigExtensible> dic) where T: ConfigExtensible
    {
        System.Type key = typeof(T);
        T[] localArray = this.LoadDataArray<T>();
        Dictionary<int, ConfigExtensible> dictionary = new Dictionary<int, ConfigExtensible>();
        ArrayList list = new ArrayList();
        dic = dictionary;
        data = list;
        foreach (T local in localArray)
        {
            if (dictionary.ContainsKey(local.getEntry()))
            {
                Debug.LogError(string.Format("id:{0} in {1} is exists!", local.getEntry(), key.Name));
            }
            else
            {
                dictionary.Add(local.getEntry(), local);
            }
        }
        if (!this._configTypeMap.ContainsKey(key))
        {
            data = new ArrayList(dictionary.Values);
            this._configTypeMap.Add(key, data);
        }
        if (!this._configMap.ContainsKey(key))
        {
            this._configMap.Add(key, dictionary);
        }
    }

    protected T[] LoadDataArray<T>() where T: ConfigExtensible
    {
        string str = typeof(T).ToString();
        string res = "Configs/" + str.Substring(8) + ".tsv";
        TextAsset asset = BundleMgr.Instance.LoadResource(res, ".bytes", typeof(TextAsset)) as TextAsset;
        if (asset == null)
        {
        }
        T[] localArray = this.LoadDataArrayFromMemery<T>(asset.bytes);
        asset = null;
        return localArray;
    }

    protected T[] LoadDataArrayFromMemery<T>(byte[] bytes) where T: ConfigExtensible
    {
        List<T> list = new List<T>();
        MemoryStream stream = new MemoryStream(bytes);
        StreamReader reader = new StreamReader(stream);
        int num = 0;
        while (reader.Peek() >= 0)
        {
            num++;
            string source = reader.ReadLine();
            if (num != 1)
            {
                T item = Activator.CreateInstance(typeof(T)) as T;
                item.Deserialize(source);
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public void Reset()
    {
        this._configTypeMap.Clear();
        this._configMap.Clear();
        this.m_word_config = null;
        this.m_opresult_entry_map = null;
        this.m_loading_config = null;
        this.m_maskwords = null;
    }

    public string this[int key]
    {
        get
        {
            return this.GetWord(key);
        }
    }
}

