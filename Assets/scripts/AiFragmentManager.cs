using Fatefulness;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class AiFragmentManager
{
    private static AiFragmentManager _instance;
    private List<FramentData> fragmentPool = new List<FramentData>();

    public void CacheBufferFragment(string aiKey)
    {
        this.CacheFragment(aiKey);
    }

    public void CacheFragment(string aiKey)
    {
        if (!string.IsNullOrEmpty(aiKey))
        {
            Fragment fragment = Fragment.Load("Fate", aiKey);
            if (fragment != null)
            {
                this.PushFrament(aiKey, fragment);
            }
        }
    }

    public void CacheSkillFragment(string aiKey)
    {
        this.CacheFragment(aiKey);
    }

    public void ClearData()
    {
        this.fragmentPool.Clear();
    }

    public Fragment GetFrament(string aiKey)
    {
        <GetFrament>c__AnonStoreyD3 yd = new <GetFrament>c__AnonStoreyD3 {
            aiKey = aiKey
        };
        Fragment fragment = null;
        if (!string.IsNullOrEmpty(yd.aiKey))
        {
            int index = this.fragmentPool.FindIndex(new Predicate<FramentData>(yd.<>m__2A));
            if (index < 0)
            {
                return Fragment.Load("Fate", yd.aiKey);
            }
            fragment = this.fragmentPool[index]._fragment;
            this.fragmentPool.RemoveAt(index);
        }
        return fragment;
    }

    public static AiFragmentManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new AiFragmentManager();
        }
        return _instance;
    }

    public void PushFrament(string aiKey, Fragment _fragment)
    {
        if (!string.IsNullOrEmpty(aiKey) && (_fragment != null))
        {
            FramentData item = new FramentData(aiKey, _fragment);
            _fragment.Reset();
            this.fragmentPool.Add(item);
        }
    }

    [CompilerGenerated]
    private sealed class <GetFrament>c__AnonStoreyD3
    {
        internal string aiKey;

        internal bool <>m__2A(AiFragmentManager.FramentData mn)
        {
            return mn.Key.Equals(this.aiKey);
        }
    }

    public class FramentData
    {
        public Fragment _fragment;
        public string Key;

        public FramentData(string key, Fragment fragment)
        {
            this.Key = key;
            this._fragment = fragment;
        }
    }
}

