using System;
using System.IO;
using UnityEngine;

public class SettingMgr : MonoBehaviour
{
    private static string GlobalSection = "GLOBAL";
    private IniParser iniFile;
    public bool IsFirstOpenMainUI;
    [NonSerialized]
    public bool isInited;
    public static SettingMgr mInstance;
    private string mUserName = string.Empty;
    private string settingFilePath = string.Empty;
    public bool showNoticePanel = true;

    private void Awake()
    {
        mInstance = this;
        this.settingFilePath = Path.Combine(Application.persistentDataPath, "setting2.ini");
        this.iniFile = new IniParser(this.settingFilePath, false, false);
    }

    private BattleRenderLevelType BattleRenderLevelAuto()
    {
        if (SystemInfo.systemMemorySize < GameDefine.getInstance().MemorySizeLimit)
        {
            return BattleRenderLevelType.Low;
        }
        return BattleRenderLevelType.Medium;
    }

    public static int DateTimetoUnixTimestamp(DateTime dateTime)
    {
        DateTime time = new DateTime(0x7b2, 1, 1);
        TimeSpan span = (TimeSpan) (dateTime - time.ToLocalTime());
        return (int) span.TotalSeconds;
    }

    private void FixedUpdate()
    {
    }

    public bool GetActiveBool(string key)
    {
        return this.GetCommonBool(key);
    }

    public int GetCardInt(string key)
    {
        return this.GetCommonInt("CARD_" + key);
    }

    public int GetCardInt(string key, int defaultValue)
    {
        return this.GetCommonInt("CARD_" + key, defaultValue);
    }

    public int GetCardLockInt(string key, int defaultValue)
    {
        return this.GetCommonInt("CARDLOCK_" + key, defaultValue);
    }

    public string GetCardString(string key)
    {
        return this.GetCommonString("CARD_" + key);
    }

    public string GetCardString(string key, string defaultValue)
    {
        return this.GetCommonString("CARD_" + key, defaultValue);
    }

    public bool GetCommonBool(string name)
    {
        return this.iniFile.GetSettingBool(this.mUserName, name, false);
    }

    public bool GetCommonBool(string name, bool defaultValue)
    {
        return this.iniFile.GetSettingBool(this.mUserName, name, defaultValue);
    }

    public float GetCommonFloat(string name)
    {
        return this.iniFile.GetSettingFloat(this.mUserName, name, -1f);
    }

    public float GetCommonFloat(string name, float defaultValue)
    {
        return this.iniFile.GetSettingFloat(this.mUserName, name, defaultValue);
    }

    public int GetCommonInt(string name)
    {
        return this.iniFile.GetSettingInt(this.mUserName, name, 0);
    }

    public int GetCommonInt(string name, int defaultValue)
    {
        return this.iniFile.GetSettingInt(this.mUserName, name, defaultValue);
    }

    public long GetCommonLong(string name, long defaultValue)
    {
        return this.iniFile.GetSettingLong(this.mUserName, name, defaultValue);
    }

    public string GetCommonString(string name)
    {
        return this.iniFile.GetSetting(this.mUserName, name, string.Empty);
    }

    public string GetCommonString(string name, string defaultValue)
    {
        return this.iniFile.GetSetting(this.mUserName, name, defaultValue);
    }

    public string GetCommonString(string accountName, string name, string defaultValue)
    {
        return this.iniFile.GetSetting(accountName, name, string.Empty);
    }

    public int GetEquipInt(string key)
    {
        return this.GetCommonInt("EQUIP_" + key);
    }

    public int GetEquipInt(string key, int defaultValue)
    {
        return this.GetCommonInt("EQUIP_" + key, defaultValue);
    }

    public int GetEquipLockInt(string key, int defaultValue)
    {
        return this.GetCommonInt("EQUIPLOCK_" + key, defaultValue);
    }

    public string GetEquipString(string key)
    {
        return this.GetCommonString("EQUIP_" + key);
    }

    public string GetEquipString(string key, string defaultValue)
    {
        return this.GetCommonString("EQUIP_" + key, defaultValue);
    }

    public string GetFormation(string name, string _defaultVal)
    {
        return this.iniFile.GetSetting(this.mUserName, name, _defaultVal);
    }

    public bool GetGlobalBool(string name)
    {
        return this.GetGlobalBool(name, true);
    }

    public bool GetGlobalBool(string name, bool defaultValue)
    {
        if (this.iniFile != null)
        {
            return this.iniFile.GetSettingBool(GlobalSection, name, defaultValue);
        }
        return true;
    }

    public int GetGlobalInt(string name, int defaultValue)
    {
        return this.iniFile.GetSettingInt(GlobalSection, name, defaultValue);
    }

    public string GetGlobalString(string name, string defaultValue)
    {
        return this.iniFile.GetSetting(GlobalSection, name, defaultValue);
    }

    public int GetGuildBattleNoticeInt()
    {
        return this.GetCommonInt("GUILD_BATTLE_TIME_", 0);
    }

    public int GetLastLoginTime()
    {
        return this.GetGlobalInt("LASTLOGINTIME", 0);
    }

    public string GetLastLoginUserName()
    {
        return this.GetGlobalString("LASTLOGINUSERNAME", string.Empty);
    }

    public int GetMailInt(string key)
    {
        return this.GetCommonInt("MAIL_" + key);
    }

    public int GetMailInt(string key, int defaultValue)
    {
        return this.GetCommonInt("MAIL_" + key, defaultValue);
    }

    public string GetMailString(string key)
    {
        return this.GetCommonString("MAIL_" + key);
    }

    public string GetMailString(string key, string defaultValue)
    {
        return this.GetCommonString("MAIL_" + key, defaultValue);
    }

    public int GetNewbieFlag(int key)
    {
        return this.GetCommonInt("NEWBIE_FLAG_" + key.ToString(), -1);
    }

    public bool GetNewbieOption()
    {
        return this.GetCommonBool("NEWBIE_OPTION");
    }

    public int GetNewbieProcess()
    {
        return this.GetCommonInt("NEWBIE_PROCESS_", 0);
    }

    public int GetOpenShopTips(string key, int defaultValue)
    {
        return this.GetCommonInt(TimeMgr.Instance.ServerDateTime.Date.ToString("yyyyMMdd") + key, defaultValue);
    }

    public int GetStoryTalkProcess(string key)
    {
        return this.GetCommonInt("STORYTALKPROCESS_" + key, -1);
    }

    public int GetTitleEntryInt(string key)
    {
        return this.GetCommonInt("TITLE_" + key, 0);
    }

    public void InitWithUserName(string userName)
    {
        int newValue = DateTimetoUnixTimestamp(DateTime.Now);
        if ((this.GetLastLoginUserName() == userName) && ((newValue - this.GetLastLoginTime()) < 900))
        {
            this.showNoticePanel = false;
        }
        else
        {
            this.showNoticePanel = true;
        }
        this.SetLastLoginTime(newValue);
        this.SetLastLoginUserName(userName);
        this.mUserName = userName;
        this.isInited = true;
        this.SetScreenSleep();
        Application.targetFrameRate = 30;
        this.SetQuality(this.BattleRenderLevel);
    }

    private void OnApplicationQuit()
    {
        this.SaveSetting();
    }

    private void OnDestroy()
    {
        this.SaveSetting();
        Screen.sleepTimeout = -2;
        mInstance = null;
    }

    public void SaveSetting()
    {
        if (this.iniFile != null)
        {
            this.iniFile.SaveSettings();
        }
    }

    public void SetActiveBool(string key)
    {
        this.SetCommonBool(key, true);
        this.SaveSetting();
    }

    public void SetCardInt(string key, int newValue)
    {
        this.SetCommonInt("CARD_" + key, newValue);
    }

    public void SetCardLockInt(string key, int newValue)
    {
        this.SetCommonInt("CARDLOCK_" + key, newValue);
    }

    public void SetCardString(string key, string newValue)
    {
        this.SetCommonString("CARD_" + key, newValue);
    }

    public void SetCommonBool(string name, bool _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetCommonFloat(string name, float _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetCommonInt(string name, int _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetCommonLong(string name, long _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetCommonString(string name, string _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetCommonString(string accountName, string name, string _value)
    {
        this.iniFile.AddSetting(accountName, name, _value);
    }

    public void SetEquipInt(string key, int newValue)
    {
        this.SetCommonInt("EQUIP_" + key, newValue);
    }

    public void SetEquipLockInt(string key, int newValue)
    {
        this.SetCommonInt("EQUIPLOCK_" + key, newValue);
    }

    public void SetEquipString(string key, string newValue)
    {
        this.SetCommonString("EQUIP_" + key, newValue);
    }

    public void SetFormation(string name, string _value)
    {
        this.iniFile.AddSetting(this.mUserName, name, _value);
    }

    public void SetGlobalBool(string name, bool _value)
    {
        this.iniFile.AddSetting(GlobalSection, name, _value);
    }

    public void SetGlobalInt(string name, int _value)
    {
        this.iniFile.AddSetting(GlobalSection, name, _value);
    }

    public void SetGlobalString(string name, string _value)
    {
        this.iniFile.AddSetting(GlobalSection, name, _value);
    }

    public void SetGuildBattleNoticeInt(int _value)
    {
        this.SetCommonInt("GUILD_BATTLE_TIME_", _value);
        this.SaveSetting();
    }

    public void SetLastLoginTime(int newValue)
    {
        this.SetGlobalInt("LASTLOGINTIME", newValue);
    }

    public void SetLastLoginUserName(string newValue)
    {
        this.SetGlobalString("LASTLOGINUSERNAME", newValue);
    }

    public void SetMailInt(string key, int newValue)
    {
        this.SetCommonInt("MAIL_" + key, newValue);
        this.SaveSetting();
    }

    public void SetMailString(string key, string newValue)
    {
        this.SetCommonString("MAIL_" + key, newValue);
    }

    public void SetNewbieFlag(int key, int value)
    {
        this.SetCommonInt("NEWBIE_FLAG_" + key.ToString(), value);
        this.SaveSetting();
    }

    public void SetNewbieOption(bool opt)
    {
        this.SetCommonBool("NEWBIE_OPTION", opt);
        this.SaveSetting();
    }

    public void SetNewbieProcess(int _value)
    {
        this.SetCommonInt("NEWBIE_PROCESS_", _value);
        this.SaveSetting();
    }

    public void SetOpenShopTips(string key, int newValue)
    {
        this.SetCommonInt(TimeMgr.Instance.ServerDateTime.Date.ToString("yyyyMMdd") + key, newValue);
        this.SaveSetting();
    }

    private void SetQuality(BattleRenderLevelType level)
    {
        if (level == BattleRenderLevelType.Low)
        {
            QualitySettings.SetQualityLevel(0);
        }
        else if (level == BattleRenderLevelType.High)
        {
            QualitySettings.SetQualityLevel(1);
        }
        else
        {
            QualitySettings.SetQualityLevel(1);
        }
    }

    public void SetScreenSleep()
    {
        if (this.ScreenSleep_Enable)
        {
            Screen.sleepTimeout = -2;
        }
        else
        {
            Screen.sleepTimeout = -1;
        }
    }

    public void SetScreenSleepOnBattle()
    {
        Screen.sleepTimeout = -1;
    }

    public void SetStoryTalkProcess(string key, int _value)
    {
        this.SetCommonInt("STORYTALKPROCESS_" + key, _value);
    }

    public void SetTitleEntryInt(string key, int newValue)
    {
        this.SetCommonInt("TITLE_" + key, newValue);
        this.SaveSetting();
    }

    private void Start()
    {
        SoundManager.mInstance.SFXVolume = !this.SFX_Enable ? ((float) 0) : ((float) 1);
        SoundManager.mInstance.BGMVolume = !this.BGM_Enable ? ((float) 0) : ((float) 1);
    }

    public BattleRenderLevelType BattleRenderLevel
    {
        get
        {
            int globalInt = this.GetGlobalInt("BATTLERENDERLEVEL", -1);
            if (globalInt == -1)
            {
                globalInt = (int) this.BattleRenderLevelAuto();
            }
            return (BattleRenderLevelType) globalInt;
        }
        set
        {
            this.SetGlobalInt("BATTLERENDERLEVEL", (int) value);
            this.SetQuality(value);
        }
    }

    public bool BGM_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_BGM");
        }
        set
        {
            SoundManager.mInstance.BGMVolume = !value ? ((float) 0) : ((float) 1);
            this.SetGlobalBool("SETTING_BGM", value);
        }
    }

    public bool FreeTakeCardPush
    {
        get
        {
            return this.GetGlobalBool("FREE_TAKE_CARD_PUSH");
        }
        set
        {
            this.SetGlobalBool("FREE_TAKE_CARD_PUSH", value);
        }
    }

    public bool GuildShopRefreshPush
    {
        get
        {
            return this.GetGlobalBool("SHOP_FEFRESH_PUSH");
        }
        set
        {
            this.SetGlobalBool("SHOP_FEFRESH_PUSH", value);
        }
    }

    public bool IsEffectEnable
    {
        get
        {
            return (this.BattleRenderLevel != BattleRenderLevelType.Low);
        }
        set
        {
        }
    }

    public bool MeiRiJiangLiPush
    {
        get
        {
            return this.GetGlobalBool("MEIRIJIANGLI_PUSH");
        }
        set
        {
            this.SetGlobalBool("MEIRIJIANGLI_PUSH", value);
        }
    }

    public bool PushNotification_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_ENABLE_PUSHNOTICE");
        }
        set
        {
            this.SetGlobalBool("SETTING_ENABLE_PUSHNOTICE", value);
        }
    }

    public bool PushTiLiFull_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_ENABLE_PUSHTILIFULL");
        }
        set
        {
            this.SetGlobalBool("SETTING_ENABLE_PUSHTILIFULL", value);
        }
    }

    public bool PushYqdChouJiang_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_ENABLE_PUSHYQD");
        }
        set
        {
            this.SetGlobalBool("SETTING_ENABLE_PUSHYQD", value);
        }
    }

    public bool ScreenSleep_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_SCREENNOSLEEP", false);
        }
        set
        {
            if (value)
            {
                Screen.sleepTimeout = -2;
            }
            else
            {
                Screen.sleepTimeout = -1;
            }
            this.SetGlobalBool("SETTING_SCREENNOSLEEP", value);
        }
    }

    public bool SFX_Enable
    {
        get
        {
            return this.GetGlobalBool("SETTING_SFX");
        }
        set
        {
            SoundManager.mInstance.SFXVolume = !value ? ((float) 0) : ((float) 1);
            this.SetGlobalBool("SETTING_SFX", value);
        }
    }

    public bool ShopRefreshPush
    {
        get
        {
            return this.GetGlobalBool("SHOP_GUILDFEFRESH_PUSH");
        }
        set
        {
            this.SetGlobalBool("SHOP_GUILDFEFRESH_PUSH", value);
        }
    }

    public bool SkillFullPush
    {
        get
        {
            return this.GetGlobalBool("SKILL_FULL_PUSH");
        }
        set
        {
            this.SetGlobalBool("SKILL_FULL_PUSH", value);
        }
    }

    public bool WorldBossPush
    {
        get
        {
            return this.GetGlobalBool("WORLDBOSS_PUSH");
        }
        set
        {
            this.SetGlobalBool("WORLDBOSS_PUSH", value);
        }
    }

    public bool WorldCupApplyPush
    {
        get
        {
            return this.GetGlobalBool("WORLDBAPPLY_PUSH");
        }
        set
        {
            this.SetGlobalBool("WORLDBAPPLY_PUSH", value);
        }
    }

    public bool WorldCupBeiAttackPush
    {
        get
        {
            return this.GetGlobalBool("WORLDBEIATTACK_PUSH");
        }
        set
        {
            this.SetGlobalBool("WORLDBEIATTACK_PUSH", value);
        }
    }
}

