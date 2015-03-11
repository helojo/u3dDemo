using Battle;
using FastBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Toolbox;
using UnityEngine;

public class CommonFunc
{
    private const uint DayToSec = 0x15180;
    private const uint HourToSec = 0xe10;
    private static Color[] HpColorList = new Color[] { new Color(1f, 0.11f, 0f), new Color(1f, 0.86f, 0f), new Color(0.07f, 1f, 0f), new Color(0f, 1f, 0.68f), new Color(1f, 0f, 1f) };
    private static string[] HpSpriteList = new string[] { "jm_zd_a01-4", "jm_zd_a01-5", "jm_zd_a01-6", "jm_zd_a01-7", "jm_zd_a01-8" };
    private const uint MinToSec = 60;
    private const uint MonthToSec = 0x278d00;
    private const uint WeekToSec = 0x93a80;

    public static void CalOpenTime(string begin, int duration, int loop, int cd, out SMT_TM data)
    {
        List<int> configEntry = GetConfigEntry(begin);
        if (configEntry.Count < 5)
        {
            data = new SMT_TM();
            data.type = SMT_TM.MTTM_Type.TM_OPEN;
        }
        else
        {
            DateTime time = new DateTime(configEntry[0], configEntry[1], configEntry[2], configEntry[3], configEntry[4], 0);
            long num = TimeMgr.Instance.ConvertToTimeStamp(time);
            long end = TimeMgr.Instance.ConvertToTimeStamp(TimeMgr.Instance.ServerDateTime) - 10L;
            SMT_TM smt_tm = new SMT_TM();
            if (loop == 0)
            {
                if (end < num)
                {
                    CalTimeDelta(num, end, out smt_tm);
                    smt_tm.type = SMT_TM.MTTM_Type.TM_CLOSE;
                    data = smt_tm;
                }
                else
                {
                    long num3 = num + (60L * duration);
                    if (end > num3)
                    {
                        smt_tm.type = SMT_TM.MTTM_Type.TM_NEVERMORE;
                        data = smt_tm;
                    }
                    else
                    {
                        CalTimeDelta(num, end, out smt_tm);
                        smt_tm.type = SMT_TM.MTTM_Type.TM_OPEN;
                        data = smt_tm;
                    }
                }
            }
            else if (end < num)
            {
                CalTimeDelta(num, end, out smt_tm);
                smt_tm.type = SMT_TM.MTTM_Type.TM_CLOSE;
                data = smt_tm;
            }
            else
            {
                int num4 = (int) ((60L * duration) + (60L * cd));
                if (num4 == 0)
                {
                    data = smt_tm;
                    Debug.LogWarning("duration and cd is zero!");
                }
                else
                {
                    int num5 = (int) (end - num);
                    int num6 = num5 / num4;
                    int num7 = ((int) num) + (num4 * num6);
                    int num8 = num7 + ((int) (60L * duration));
                    if (end < num8)
                    {
                        CalTimeDelta(end, (long) num8, out smt_tm);
                        smt_tm.type = SMT_TM.MTTM_Type.TM_OPEN;
                        data = smt_tm;
                    }
                    else
                    {
                        CalTimeDelta(end, num8 + (60L * cd), out smt_tm);
                        smt_tm.type = SMT_TM.MTTM_Type.TM_CLOSE;
                        data = smt_tm;
                    }
                }
            }
        }
    }

    private static void CalTimeDelta(long begin, long end, out SMT_TM data)
    {
        int num = (begin <= end) ? ((int) (end - begin)) : ((int) (begin - end));
        SMT_TM smt_tm = new SMT_TM {
            day = (int) (((long) num) / 0x15180L),
            hour = (int) (((long) num) / 0xe10L),
            minute = (int) (((long) num) / 60L),
            second = num
        };
        data = smt_tm;
    }

    public static bool CanLvUpSkill(int _Cardquality, int _SkillIndex)
    {
        E_QualiytDef def = (E_QualiytDef) _Cardquality;
        E_CardSkill skill = (E_CardSkill) _SkillIndex;
        switch (def)
        {
            case E_QualiytDef.e_qualitydef_white:
                return (E_CardSkill.e_card_skill_0 >= skill);

            case E_QualiytDef.e_qualitydef_green:
                return (E_CardSkill.e_card_skill_1 >= skill);

            case E_QualiytDef.e_qualitydef_blue:
                return (E_CardSkill.e_card_skill_2 >= skill);

            case E_QualiytDef.e_qualitydef_purple:
                return (E_CardSkill.e_card_skill_3 >= skill);
        }
        if (E_CardSkill.e_card_skill_max <= skill)
        {
            return false;
        }
        return true;
    }

    public static bool CardExpIsFull(Card _info)
    {
        if (_info.cardInfo.level == ActorData.getInstance().Level)
        {
            card_lv_up_config _config = ConfigMgr.getInstance().getByEntry<card_lv_up_config>(_info.cardInfo.level);
            if ((_config != null) && (_info.cardInfo.curExp >= _config.stage1_exp))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckAchievementCompleted()
    {
        if (ActorData.getInstance().Level < LevelLimitCfg().achievement)
        {
            return false;
        }
        return GameDataMgr.Instance.AchievementStage;
    }

    public static bool CheckActiveCompleted()
    {
        if (ActorData.getInstance().Level < 0)
        {
            return false;
        }
        return GameDataMgr.Instance.ActiveStage;
    }

    public static bool CheckBreakMaterialEnough(int itemEntry)
    {
        <CheckBreakMaterialEnough>c__AnonStorey1A9 storeya = new <CheckBreakMaterialEnough>c__AnonStorey1A9 {
            bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(itemEntry)
        };
        if (storeya.bec == null)
        {
            return false;
        }
        if (((storeya.bec.need_item_1 > 0) && (storeya.bec.need_num_1 > 0)) && (ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__235)) == null))
        {
            return false;
        }
        if (((storeya.bec.need_item_2 > 0) && (storeya.bec.need_num_2 > 0)) && (ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__236)) == null))
        {
            return false;
        }
        if (((storeya.bec.need_item_3 > 0) && (storeya.bec.need_num_3 > 0)) && (ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__237)) == null))
        {
            return false;
        }
        return true;
    }

    public static bool CheckCardCanBreak(Card _card)
    {
        break_card_config breakCardCfg = GetBreakCardCfg((int) _card.cardInfo.entry, _card.cardInfo.quality);
        if (breakCardCfg == null)
        {
            return false;
        }
        int num = breakCardCfg.lv_limit;
        if (_card.cardInfo.level < num)
        {
            return false;
        }
        if (_card.cardInfo.quality >= 6)
        {
            return false;
        }
        foreach (EquipInfo info in _card.equipInfo)
        {
            if (info.quality != (_card.cardInfo.quality + 1))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckCardCanLevUp()
    {
        return (GameDataMgr.Instance.CardEquipLvUpFlag || GameDataMgr.Instance.CardActivityFlag);
    }

    public static bool CheckCardCanUpStar(Card _card)
    {
        <CheckCardCanUpStar>c__AnonStorey1AA storeyaa = new <CheckCardCanUpStar>c__AnonStorey1AA();
        if (_card == null)
        {
            return false;
        }
        if (_card.cardInfo.starLv >= 5)
        {
            return false;
        }
        storeyaa.cec = GetCardExCfg((int) _card.cardInfo.entry, _card.cardInfo.starLv);
        if (storeyaa.cec == null)
        {
            return false;
        }
        Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyaa.<>m__238));
        if (item == null)
        {
            return false;
        }
        return (item.num >= storeyaa.cec.evolve_need_item_num);
    }

    public static bool CheckDailyCompleted()
    {
        if (ActorData.getInstance().Level < LevelLimitCfg().daily_misson)
        {
            return false;
        }
        return GameDataMgr.Instance.DailyStage;
    }

    public static bool CheckEquipUpMaterial(int entry, int count)
    {
        <CheckEquipUpMaterial>c__AnonStorey1A8 storeya = new <CheckEquipUpMaterial>c__AnonStorey1A8 {
            entry = entry
        };
        Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__231));
        if ((item != null) && (item.num >= count))
        {
            return true;
        }
        int num = (item == null) ? 0 : item.num;
        bool flag = false;
        storeya.ic = ConfigMgr.getInstance().getByEntry<item_config>(storeya.entry);
        if (storeya.ic == null)
        {
            return flag;
        }
        if (((storeya.ic.type == 1) && (storeya.ic.param_0 > 0)) && (storeya.ic.param_1 > 0))
        {
            Item item2 = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__232));
            int num2 = (item2 == null) ? 0 : item2.num;
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(storeya.ic.param_0);
            if ((_config != null) && (_config.type == 2))
            {
                if (num2 >= (storeya.ic.param_1 * count))
                {
                    flag = true;
                }
                else if ((num + (num2 / storeya.ic.param_1)) >= count)
                {
                    flag = true;
                }
            }
            else if ((_config != null) && (_config.type == 1))
            {
                flag = CheckEquipUpMaterial(storeya.ic.param_0, storeya.ic.param_1);
            }
            else
            {
                return false;
            }
        }
        if (!flag)
        {
            return false;
        }
        if (((storeya.ic.type == 1) && (storeya.ic.param_2 > 0)) && (storeya.ic.param_3 > 0))
        {
            Item item3 = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__233));
            int num3 = (item3 == null) ? 0 : item3.num;
            item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(storeya.ic.param_2);
            if ((_config2 != null) && (_config2.type == 2))
            {
                if (num3 >= (storeya.ic.param_3 * count))
                {
                    flag = true;
                }
                else if ((num + (num3 / storeya.ic.param_3)) >= count)
                {
                    flag = true;
                }
            }
            else if ((_config2 != null) && (_config2.type == 1))
            {
                flag = CheckEquipUpMaterial(storeya.ic.param_2, storeya.ic.param_3);
            }
            else
            {
                return false;
            }
        }
        if (!flag)
        {
            return false;
        }
        if (((storeya.ic.type != 1) || (storeya.ic.param_4 <= 0)) || (storeya.ic.param_5 <= 0))
        {
            return flag;
        }
        Item item4 = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__234));
        int num4 = (item4 == null) ? 0 : item4.num;
        item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(storeya.ic.param_4);
        if ((_config3 != null) && (_config3.type == 2))
        {
            if ((num4 < (storeya.ic.param_5 * count)) && ((num + (num4 / storeya.ic.param_5)) < count))
            {
                return flag;
            }
            return true;
        }
        return (((_config3 != null) && (_config3.type == 1)) && CheckEquipUpMaterial(storeya.ic.param_4, storeya.ic.param_5));
    }

    public static bool CheckHaveCardCanBreak()
    {
        foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
        {
            CardOriginal original = pair.Value;
            if (((null != original) && (original.ori != null)) && CheckCardCanBreak(original.ori))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckHaveCardCanUpStar()
    {
        foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
        {
            <CheckHaveCardCanUpStar>c__AnonStorey1AB storeyab = new <CheckHaveCardCanUpStar>c__AnonStorey1AB();
            CardOriginal original = pair.Value;
            if (((null != original) && (original.ori != null)) && (original.ori.cardInfo.starLv < 5))
            {
                storeyab.cec = GetCardExCfg((int) original.ori.cardInfo.entry, original.ori.cardInfo.starLv);
                if (storeyab.cec != null)
                {
                    Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyab.<>m__239));
                    if ((item != null) && (item.num >= storeyab.cec.evolve_need_item_num))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool CheckHaveEquipLevUp(Card _card)
    {
        if (_card != null)
        {
            if (ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry) == null)
            {
                return false;
            }
            for (int i = 0; i < 6; i++)
            {
                <CheckHaveEquipLevUp>c__AnonStorey1A7 storeya = new <CheckHaveEquipLevUp>c__AnonStorey1A7();
                EquipInfo info = _card.equipInfo[i];
                if ((info.quality <= _card.cardInfo.quality) && (ConfigMgr.getInstance().getByEntry<item_config>(info.entry) != null))
                {
                    storeya.bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                    if ((storeya.bec != null) && (storeya.bec.break_equip_entry >= 0))
                    {
                        XSingleton<EquipBreakMateMgr>.Singleton.Update(_card, i);
                        if (ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__230)) != null)
                        {
                            return true;
                        }
                        if (CheckMaterialEnough(storeya.bec.equip_entry))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public static string CheckHostIP(string host, List<string> backIPs)
    {
        if ((backIPs == null) || (backIPs.Count == 0))
        {
            Debug.Log("backIPs is empty");
            return host;
        }
        try
        {
            IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
            if ((hostAddresses.Length > 0) && backIPs.Contains(hostAddresses[0].ToString()))
            {
                Debug.Log("CheckHostIP OK " + host + " IP: " + hostAddresses[0].ToString());
                return hostAddresses[0].ToString();
            }
        }
        catch (Exception exception)
        {
            Debug.LogWarning("CheckHostIP exception " + exception.ToString());
        }
        host = backIPs[0];
        Debug.Log("CheckHostIP Change " + host);
        return host;
    }

    public static bool CheckIsFrozenFun(ELimitFuncType type)
    {
        <CheckIsFrozenFun>c__AnonStorey1A6 storeya = new <CheckIsFrozenFun>c__AnonStorey1A6();
        if (((type >= ELimitFuncType.E_LimitFunc_None) && (type < ELimitFuncType.E_LimitFunc_Num)) && (type < ActorData.getInstance().UserInfo.limit_data.data.Count))
        {
            storeya.endTime = (int) ActorData.getInstance().UserInfo.limit_data.data[(int) type].time;
            storeya.timeLen = (int) ActorData.getInstance().UserInfo.limit_data.data[(int) type].len;
            if ((storeya.endTime > 0) && (storeya.endTime > TimeMgr.Instance.ServerStampTime))
            {
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storeya.<>m__22F), null);
                return true;
            }
        }
        return false;
    }

    public static bool CheckMaterialEnough(int itemEntry)
    {
        break_equip_config _config = ConfigMgr.getInstance().getByEntry<break_equip_config>(itemEntry);
        if (_config == null)
        {
            return false;
        }
        if (((_config.need_item_1 > 0) && (_config.need_num_1 > 0)) && !CheckEquipUpMaterial(_config.need_item_1, _config.need_num_1))
        {
            return false;
        }
        if (((_config.need_item_2 > 0) && (_config.need_num_2 > 0)) && !CheckEquipUpMaterial(_config.need_item_2, _config.need_num_2))
        {
            return false;
        }
        if (((_config.need_item_3 > 0) && (_config.need_num_3 > 0)) && !CheckEquipUpMaterial(_config.need_item_3, _config.need_num_3))
        {
            return false;
        }
        return true;
    }

    public static bool CheckTitleBarFunList()
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        return ((activityGUIEntity != null) && activityGUIEntity._FunBtn.active);
    }

    public static bool CheckTitleBarStat()
    {
        return (GUIMgr.Instance.GetActivityGUIEntity<TitleBar>() != null);
    }

    public static void DeleteChildItem(Transform _root)
    {
        if (_root != null)
        {
            List<Transform> list = new List<Transform>();
            IEnumerator enumerator = _root.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    list.Add(current);
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
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                UnityEngine.Object.DestroyImmediate(list[i].gameObject);
            }
        }
    }

    public static bool EmailGestRUE(string emailsg)
    {
        string pattern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        return Regex.IsMatch(emailsg, pattern);
    }

    public static int GetArenaLadderFreshCoin(int time)
    {
        ArrayList list = ConfigMgr.getInstance().getList<arena_shop_refresh_config>();
        for (int i = 0; i != list.Count; i++)
        {
            arena_shop_refresh_config _config = (arena_shop_refresh_config) list[i];
            if (((time + 1) < _config.count) && (i > 0))
            {
                arena_shop_refresh_config _config2 = (arena_shop_refresh_config) list[i - 1];
                return _config2.cost_value;
            }
        }
        arena_shop_refresh_config _config3 = (arena_shop_refresh_config) list[list.Count - 1];
        return _config3.cost_value;
    }

    public static int GetAtkCountFromList(List<FastBuf.TrenchData> _List, int _Entry)
    {
        if (_List != null)
        {
            foreach (FastBuf.TrenchData data in _List)
            {
                if (data.entry == _Entry)
                {
                    return data.remain;
                }
            }
        }
        return 0;
    }

    public static float GetBreakCardAgility(int entry, int quality)
    {
        float num = 0f;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<break_card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                break_card_config current = (break_card_config) enumerator.Current;
                if ((current.break_card_quality >= 0) && ((current.card_entry == entry) && (current.break_card_quality <= quality)))
                {
                    num += current.agility;
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
        return num;
    }

    public static break_card_config GetBreakCardCfg(int entry, int quality)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<break_card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                break_card_config current = (break_card_config) enumerator.Current;
                if ((current.card_entry == entry) && (current.cur_card_quality == quality))
                {
                    return current;
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
        return null;
    }

    public static float GetBreakCardIntelligence(int entry, int quality)
    {
        float num = 0f;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<break_card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                break_card_config current = (break_card_config) enumerator.Current;
                if ((current.break_card_quality >= 0) && ((current.card_entry == entry) && (current.break_card_quality <= quality)))
                {
                    num += current.intelligence;
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
        return num;
    }

    public static float GetBreakCardStamina(int entry, int quality)
    {
        float num = 0f;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<break_card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                break_card_config current = (break_card_config) enumerator.Current;
                if ((current.break_card_quality >= 0) && ((current.card_entry == entry) && (current.break_card_quality <= quality)))
                {
                    num += current.stamina;
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
        return num;
    }

    public static float GetBreakCardStrength(int entry, int quality)
    {
        float num = 0f;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<break_card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                break_card_config current = (break_card_config) enumerator.Current;
                if ((current.break_card_quality >= 0) && ((current.card_entry == entry) && (current.break_card_quality <= quality)))
                {
                    num += current.strength;
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
        return num;
    }

    public static int GetCardCurrMaxHp(Card _card)
    {
        if (_card == null)
        {
            return 0;
        }
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
        if (_config == null)
        {
            return 0;
        }
        int starLv = _card.cardInfo.starLv;
        int level = _card.cardInfo.level;
        int quality = _card.cardInfo.quality;
        int entry = _config.entry;
        card_ex_config cardExCfg = GetCardExCfg(entry, starLv);
        if (cardExCfg == null)
        {
            return 0;
        }
        int passivitySkillEntry = -1;
        if (_config.passivity_skill_idx != -1)
        {
            if (_config.passivity_skill_idx == 0)
            {
                passivitySkillEntry = _config.skill_0;
            }
            else if (_config.passivity_skill_idx == 1)
            {
                passivitySkillEntry = _config.skill_1;
            }
            else if (_config.passivity_skill_idx == 2)
            {
                passivitySkillEntry = _config.skill_2;
            }
        }
        float num7 = 0f;
        float num11 = 0f;
        int skillLevel = 1;
        if ((_config.passivity_skill_idx != -1) && SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
        {
            skillLevel = _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillLevel;
        }
        if ((_config.passivity_skill_idx != -1) && SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
        {
            GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Stamina, out num7, out num11);
        }
        float num13 = (_config.stamina + ((level - 1) * cardExCfg.stamina_grow)) + GetBreakCardStamina(entry, quality);
        num13 += num7;
        float num18 = 0f;
        float num19 = 0f;
        if ((_config.passivity_skill_idx != -1) && SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
        {
            GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.MaxHp, out num18, out num19);
        }
        int num14 = _config.hp + ((int) (num13 * GetStaminaToHp(entry, starLv)));
        int cardEquipAddSumValue = GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Hp);
        cardEquipAddSumValue = (cardEquipAddSumValue + ((int) ((cardEquipAddSumValue + num14) * num19))) + ((int) num18);
        return (cardEquipAddSumValue + num14);
    }

    public static CombatDetailActor GetCardDtailValue(int cardEntry, int cardStarLv, int cardLv)
    {
        CombatDetailActor actor = new CombatDetailActor();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(cardEntry);
        if (_config != null)
        {
            int num = cardStarLv;
            int num2 = cardLv;
            int quality = _config.quality;
            int num4 = cardEntry;
            card_ex_config cardExCfg = GetCardExCfg(num4, num);
            if (cardExCfg == null)
            {
                return actor;
            }
            float num5 = (_config.stamina + ((num2 - 1) * cardExCfg.stamina_grow)) + GetBreakCardStamina(num4, quality);
            float num6 = (_config.agility + ((num2 - 1) * cardExCfg.agility_grow)) + GetBreakCardAgility(num4, quality);
            float num7 = (_config.intelligence + ((num2 - 1) * cardExCfg.intelligence_grow)) + GetBreakCardIntelligence(num4, quality);
            float num8 = (_config.strength + ((num2 - 1) * cardExCfg.strength_grow)) + GetBreakCardStrength(num4, quality);
            actor.maxHp = (ulong) (_config.hp + (num5 * GetStaminaToHp(num4, num)));
            actor.curHp = actor.maxHp;
            actor.attack = (int) (((_config.attack + (num8 * cardExCfg.strength_to_attack)) + (num7 * cardExCfg.intelligence_to_attack)) + (num6 * cardExCfg.agility_to_attack));
            actor.physicsDefence = _config.physics_defence + ((int) (num8 * cardExCfg.strength_to_phy));
            actor.spellDefence = _config.spell_defence + ((int) (num7 * cardExCfg.intelligence_to_magicdef));
            actor.physicsPierce = _config.physics_pierce;
            actor.spellPierce = _config.spell_pierce;
            float num9 = num6 * cardExCfg.agility_to_crit;
            actor.critRate = (int) ((((float) _config.crit_rate) / 10000f) + (num9 / ((num9 + (num2 * 70)) + 75f)));
            actor.critRate *= actor.tenacity = (int) (((float) _config.tenacity) / ((float) ((_config.tenacity + (num2 * 90)) + 70)));
            actor.dodgeRate = ((int) (((float) _config.dod_rate) / 10000f)) + (_config.dod_rate / ((_config.dod_rate + (num2 * 300)) + 80));
            actor.hpRecover = _config.hp_recover;
            actor.energyRecover = _config.energy_recover;
            actor.beHealMod = (int) (((float) _config.be_heal_mod) / 10000f);
            actor.suckLv = _config.suck_lv;
        }
        return actor;
    }

    public static int GetCardEquipAddSumValue(List<EquipInfo> equipInfo, EquipAddType type)
    {
        int num = 0;
        foreach (EquipInfo info in equipInfo)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                switch (type)
                {
                    case EquipAddType.Hp:
                        num += _config.hp + ((info.lv - 1) * _config.hp_grow);
                        break;

                    case EquipAddType.Tenacity:
                        num += _config.tenacity_level + ((info.lv - 1) * _config.tenacity_level_grow);
                        break;

                    case EquipAddType.Attack:
                        num += _config.attack + ((info.lv - 1) * _config.attack_grow);
                        break;

                    case EquipAddType.Phydefence:
                        num += _config.physics_defence + ((info.lv - 1) * _config.physics_defence_grow);
                        break;

                    case EquipAddType.Spellfence:
                        num += _config.spell_defence + ((info.lv - 1) * _config.spell_defence_grow);
                        break;

                    case EquipAddType.CritRate:
                        num += _config.crit_level + ((info.lv - 1) * _config.crit_level_grow);
                        break;

                    case EquipAddType.DodRate:
                        num += _config.dodge_level + ((info.lv - 1) * _config.dodge_level_grow);
                        break;

                    case EquipAddType.HpRecover:
                        num += _config.hp_recover + ((info.lv - 1) * _config.hp_recover_grow);
                        break;

                    case EquipAddType.EnergyRecover:
                        num += _config.energy_recover + ((info.lv - 1) * _config.energy_recover_grow);
                        break;

                    case EquipAddType.CritLevel:
                        num += _config.crit_level + ((info.lv - 1) * _config.crit_level_grow);
                        break;

                    case EquipAddType.CritDmg:
                        num += _config.critdmg_level;
                        break;

                    case EquipAddType.SuckLv:
                        num += _config.suck_lv + ((info.lv - 1) * _config.suck_grow);
                        break;
                }
            }
        }
        return num;
    }

    public static card_ex_config GetCardExCfg(int cardEntry, int cardStarLv)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_ex_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_ex_config current = (card_ex_config) enumerator.Current;
                if ((current.card_entry == cardEntry) && (current.cur_card_star_lv == cardStarLv))
                {
                    return current;
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
        return null;
    }

    public static int GetCardExCfgByItemPart(int itemEntry)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_ex_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_ex_config current = (card_ex_config) enumerator.Current;
                if (current.item_entry == itemEntry)
                {
                    return current.card_entry;
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
        return -1;
    }

    public static int GetCardGemAddSumValue(List<CardGemInfo> gemInfo, GemAddType type)
    {
        int num = 0;
        foreach (CardGemInfo info in gemInfo)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                switch (type)
                {
                    case GemAddType.Strength:
                        num += (_config.strength >= 0) ? _config.strength : 0;
                        break;

                    case GemAddType.Intelligence:
                        num += (_config.intelligence >= 0) ? _config.intelligence : 0;
                        break;

                    case GemAddType.Agility:
                        num += (_config.agility >= 0) ? _config.agility : 0;
                        break;

                    case GemAddType.Stamina:
                        num += (_config.stamina >= 0) ? _config.stamina : 0;
                        break;

                    case GemAddType.Attack:
                        num += (_config.attack >= 0) ? _config.attack : 0;
                        break;

                    case GemAddType.Spellfence:
                        num += (_config.spell_defence >= 0) ? _config.spell_defence : 0;
                        break;

                    case GemAddType.Phydefence:
                        num += (_config.physics_defence >= 0) ? _config.physics_defence : 0;
                        break;

                    case GemAddType.CritLevel:
                        num += (_config.crit_level >= 0) ? _config.crit_level : 0;
                        break;

                    case GemAddType.CritDemage:
                        num += (_config.critdmg_level >= 0) ? _config.critdmg_level : 0;
                        break;

                    case GemAddType.Tenacity:
                        num += (_config.tenacity_level >= 0) ? _config.tenacity_level : 0;
                        break;

                    case GemAddType.HpRecover:
                        num += (_config.hp_recover >= 0) ? _config.hp_recover : 0;
                        break;

                    case GemAddType.EnergyRecover:
                        num += (_config.energy_recover >= 0) ? _config.energy_recover : 0;
                        break;
                }
            }
        }
        return num;
    }

    public static void GetCardGemCombineAttr(Card _card, out int[] attrs)
    {
        attrs = new int[12];
        if ((_card != null) && ((_card.cardGemInfo != null) && (_card.cardGemInfo.Count != 0)))
        {
            card_gem_attr_config _config = ConfigMgr.getInstance().getByEntry<card_gem_attr_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                int[] numArray = new int[6];
                for (int i = 0; i < 6; i++)
                {
                    CardGemInfo info = _card.cardGemInfo[i];
                    item_config _config2 = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
                    if (((_config2 != null) && (_config2.quality >= 0)) && (_config2.quality < 6))
                    {
                        for (int k = 0; k <= _config2.quality; k++)
                        {
                            numArray[k]++;
                        }
                    }
                }
                int num3 = 3;
                int[] numArray2 = new int[] { 2, 4, 6 };
                int[] numArray5 = new int[,] { { _config.attrtype_2_1, _config.attrtype_2_2, _config.attrtype_2_3, _config.attrtype_2_4, _config.attrtype_2_5, _config.attrtype_2_6 }, { _config.attrtype_4_1, _config.attrtype_4_2, _config.attrtype_4_3, _config.attrtype_4_4, _config.attrtype_4_5, _config.attrtype_4_6 }, { _config.attrtype_6_1, _config.attrtype_6_2, _config.attrtype_6_3, _config.attrtype_6_4, _config.attrtype_6_5, _config.attrtype_6_6 } };
                int[,] numArray3 = numArray5;
                int[] numArray6 = new int[,] { { _config.attrvalue_2_1, _config.attrvalue_2_2, _config.attrvalue_2_3, _config.attrvalue_2_4, _config.attrvalue_2_5, _config.attrvalue_2_6 }, { _config.attrvalue_4_1, _config.attrvalue_4_2, _config.attrvalue_4_3, _config.attrvalue_4_4, _config.attrvalue_4_5, _config.attrvalue_4_6 }, { _config.attrvalue_6_1, _config.attrvalue_6_2, _config.attrvalue_6_3, _config.attrvalue_6_4, _config.attrvalue_6_5, _config.attrvalue_6_6 } };
                int[,] numArray4 = numArray6;
                for (int j = 0; j < num3; j++)
                {
                    for (int m = 5; m >= 0; m--)
                    {
                        if (numArray[m] >= numArray2[j])
                        {
                            int index = numArray3[j, m];
                            if ((index > -1) && (index < 12))
                            {
                                attrs[index] += numArray4[j, m];
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    public static int GetCardGemHoleMinLv()
    {
        int num = -1;
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if ((_config != null) && (_config.card_gem1_openlv != -1))
        {
            num++;
        }
        return num;
    }

    public static int GetCardLvUpExp(int cardlv, int lvUpType)
    {
        int num = 0;
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_lv_up_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_lv_up_config current = (card_lv_up_config) enumerator.Current;
                if (current.lv == cardlv)
                {
                    switch (lvUpType)
                    {
                        case 1:
                            num = current.stage1_exp;
                            break;

                        case 2:
                            num = current.stage2_exp;
                            break;

                        case 3:
                            num = current.stage3_exp;
                            break;

                        case 4:
                            num = current.stage4_exp;
                            break;
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
        return num;
    }

    public static string GetCardNameByQuality(int quality, string name)
    {
        if (quality < 0)
        {
            quality = 0;
        }
        string str = name;
        switch (((E_QualiytDef) quality))
        {
            case E_QualiytDef.e_qualitydef_blue_1:
                return (str = str + GameConstant.C_Label[quality] + "+1");

            case E_QualiytDef.e_qualitydef_purple:
                return str;

            case E_QualiytDef.e_qualitydef_purple_1:
                return (str = str + GameConstant.C_Label[quality] + "+1");

            case E_QualiytDef.e_qualitydef_purple_2:
                return (str = str + GameConstant.C_Label[quality] + "+2");
        }
        return str;
    }

    public static string GetCardQualityStr(int quality)
    {
        string str = string.Empty;
        switch (((E_QualiytDef) quality))
        {
            case E_QualiytDef.e_qualitydef_blue_1:
                return (str = str + "+1");

            case E_QualiytDef.e_qualitydef_purple:
                return str;

            case E_QualiytDef.e_qualitydef_purple_1:
                return (str = str + "+1");

            case E_QualiytDef.e_qualitydef_purple_2:
                return (str = str + "+2");
        }
        return str;
    }

    public static List<int> GetConfigEntry(string _arg)
    {
        List<int> list = new List<int>();
        if (!string.IsNullOrEmpty(_arg))
        {
            string str = string.Empty;
            _arg = _arg + '|';
            foreach (char ch in _arg)
            {
                if (ch != '|')
                {
                    str = str + ch.ToString();
                }
                else
                {
                    int item = Convert.ToInt32(str.ToString());
                    list.Add(item);
                    str = string.Empty;
                }
            }
        }
        return list;
    }

    public static int GetEquipCombCost(int entry, int count)
    {
        Item itemByEntry = XSingleton<UserItemPackageMgr>.Singleton.GetItemByEntry(entry);
        int num = (itemByEntry == null) ? 0 : itemByEntry.num;
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(entry);
        if (_config == null)
        {
            Debug.Log("error item entry:" + entry);
            return 0;
        }
        if (num >= count)
        {
            return 0;
        }
        int num2 = _config.combine_price * (count - num);
        if (((_config.type == 1) && (_config.param_0 >= 0)) && (_config.param_1 >= 0))
        {
            num2 += GetEquipCombCost(_config.param_0, _config.param_1);
        }
        if (((_config.type == 1) && (_config.param_0 >= 2)) && (_config.param_3 >= 0))
        {
            num2 += GetEquipCombCost(_config.param_2, _config.param_3);
        }
        if (((_config.type == 1) && (_config.param_0 >= 4)) && (_config.param_5 >= 0))
        {
            num2 += GetEquipCombCost(_config.param_4, _config.param_5);
        }
        return num2;
    }

    public static int GetEquipLvUpBase(int nLv)
    {
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_1)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_1;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_2)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_2;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_3)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_3;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_4)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_4;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_5)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_5;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_6)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_6;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_7)
        {
            return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_7;
        }
        return GameConstValues.EQUIP_LV_UP_LV_LIMIT_BASE_8;
    }

    public static int GetEquipLvUpGrowLv(int nLv)
    {
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_1)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_WHITE;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_2)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_GREEN;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_3)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_BLUE;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_4)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_BLUE_1;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_5)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_PURPLE;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_6)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_1;
        }
        if (nLv < GameConstValues.EQUIP_LV_UP_LV_LIMIT_7)
        {
            return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_2;
        }
        return GameConstValues.EQUIP_LV_UP_GROW_ORANGE;
    }

    public static int GetGradeFromList(List<FastBuf.TrenchData> _List, int _Entry)
    {
        if (_List != null)
        {
            foreach (FastBuf.TrenchData data in _List)
            {
                if (data.entry == _Entry)
                {
                    if (data.grade < 0)
                    {
                        return 0;
                    }
                    return data.grade;
                }
            }
        }
        return 0;
    }

    public static List<string> GetItemAttributeDesc(EquipInfo _info)
    {
        return GetItemAttributeDesc(_info.entry, _info.lv);
    }

    public static List<string> GetItemAttributeDesc(int _itemEntry, int itemLv)
    {
        List<string> list = new List<string>();
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(_itemEntry);
        if (_config != null)
        {
            int num = itemLv - 1;
            if (_config.attack > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(320) + ":" + (_config.attack + (num * _config.attack_grow)));
            }
            if (_config.hp > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x141) + ":" + (_config.hp + (num * _config.hp_grow)));
            }
            if (_config.physics_defence > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x142) + ":" + (_config.physics_defence + (num * _config.physics_defence_grow)));
            }
            if (_config.spell_defence > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x143) + ":" + (_config.spell_defence + (num * _config.spell_defence_grow)));
            }
            if (_config.crit_level > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x144) + ":" + (_config.crit_level + (num * _config.crit_level_grow)));
            }
            if (_config.dodge_level > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x145) + ":" + (_config.dodge_level + (num * _config.dodge_level_grow)));
            }
            if (_config.tenacity_level > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x146) + ":" + (_config.tenacity_level + (num * _config.tenacity_level_grow)));
            }
            if (_config.hit_level > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x147) + ":" + (_config.hit_level + (num * _config.hit_level_grow)));
            }
            if (_config.hp_recover > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x148) + ":" + (_config.hp_recover + (num * _config.hp_recover_grow)));
            }
            if (_config.energy_recover > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x149) + ":" + (_config.energy_recover + (num * _config.energy_recover_grow)));
            }
            if (_config.suck_lv > 0)
            {
                list.Add(ConfigMgr.getInstance().GetWord(0x16e) + ":" + (_config.suck_lv + (num * _config.suck_grow)));
            }
        }
        return list;
    }

    public static int GetItemMaxPileNum(int itemEntry)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(itemEntry);
        if ((_config != null) && (_config.max_pile_num > 0))
        {
            return _config.max_pile_num;
        }
        return 0x3e7;
    }

    public static string GetItemNameByQuality(int quality)
    {
        if ((quality >= 0) && (quality < 7))
        {
            return GameConstant.C_Label[quality];
        }
        return GameConstant.C_Label[0];
    }

    public static int GetLolArenaLadderFreshCoin(int time)
    {
        ArrayList list = ConfigMgr.getInstance().getList<lolarena_shop_refresh_config>();
        for (int i = 0; i != list.Count; i++)
        {
            lolarena_shop_refresh_config _config = (lolarena_shop_refresh_config) list[i];
            if (((time + 1) < _config.count) && (i > 0))
            {
                lolarena_shop_refresh_config _config2 = (lolarena_shop_refresh_config) list[i - 1];
                return _config2.cost_value;
            }
        }
        lolarena_shop_refresh_config _config3 = (lolarena_shop_refresh_config) list[list.Count - 1];
        return _config3.cost_value;
    }

    public static int GetMaxCardOrEquipLvByQuality(E_QualiytDef eQuality)
    {
        return GetMaxCardOrEquipLvByQuality((int) eQuality);
    }

    public static int GetMaxCardOrEquipLvByQuality(int eQuality)
    {
        switch (((E_QualiytDef) eQuality))
        {
            case E_QualiytDef.e_qualitydef_white:
                return GameConstValues.MAX_LV_WHITE;

            case E_QualiytDef.e_qualitydef_green:
                return GameConstValues.MAX_LV_GREEN;

            case E_QualiytDef.e_qualitydef_blue:
                return GameConstValues.MAX_LV_BLUE;

            case E_QualiytDef.e_qualitydef_blue_1:
                return GameConstValues.MAX_LV_BLUE_1;

            case E_QualiytDef.e_qualitydef_purple:
                return GameConstValues.MAX_LV_PURPLE;

            case E_QualiytDef.e_qualitydef_purple_1:
                return GameConstValues.MAX_LV_PURPLE_1;

            case E_QualiytDef.e_qualitydef_purple_2:
                return GameConstValues.MAX_LV_PURPLE_2;

            case E_QualiytDef.e_qualitydef_orange:
                return GameConstValues.MAX_LV_ORANGE;
        }
        return -1;
    }

    public static int GetMaxHoleCount()
    {
        int num = 0;
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config != null)
        {
            if (_config.card_gem1_openlv != -1)
            {
                num++;
            }
            if (_config.card_gem2_openlv != -1)
            {
                num++;
            }
            if (_config.card_gem3_openlv != -1)
            {
                num++;
            }
            if (_config.card_gem4_openlv != -1)
            {
                num++;
            }
            if (_config.card_gem5_openlv != -1)
            {
                num++;
            }
            if (_config.card_gem6_openlv != -1)
            {
                num++;
            }
        }
        return num;
    }

    public static int GetMaxSkillLv(int nSkillID, int nCardLv)
    {
        skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(nSkillID);
        if (_config == null)
        {
            return 0;
        }
        int num = nCardLv - (GetUserMaxLv() - _config.max_level);
        return ((nCardLv >= num) ? num : nCardLv);
    }

    public static int GetOutlandLadderFreshCoin(int time)
    {
        ArrayList list = ConfigMgr.getInstance().getList<outland_shop_refresh_config>();
        for (int i = 0; i != list.Count; i++)
        {
            outland_shop_refresh_config _config = (outland_shop_refresh_config) list[i];
            if (((time + 1) < _config.count) && (i > 0))
            {
                outland_shop_refresh_config _config2 = (outland_shop_refresh_config) list[i - 1];
                return _config2.cost_value;
            }
        }
        outland_shop_refresh_config _config3 = (outland_shop_refresh_config) list[list.Count - 1];
        return _config3.cost_value;
    }

    public static void GetPassivitySkillBuffAdd(int skillLevel, int passivitySkillEntry, SkillLogicEffectType _effectType, out float _addValue, out float _multipleValue)
    {
        _addValue = 0f;
        _multipleValue = 0f;
        if (passivitySkillEntry != -1)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(passivitySkillEntry);
            if (_config != null)
            {
                float num = 0f;
                float num2 = 0f;
                char[] separator = new char[] { '|' };
                string[] strArray = _config.sub_skill_list.Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    int id = int.Parse(strArray[i]);
                    sub_skill_config _config2 = ConfigMgr.getInstance().getByEntry<sub_skill_config>(id);
                    if ((_config2 != null) && (_config2.buff_entry >= 0))
                    {
                        float num5 = 0f;
                        float num6 = 0f;
                        GetSubSkillBuffAdd(skillLevel, _config2.buff_entry, _effectType, out num5, out num6);
                        num += num5;
                        num2 += num6;
                    }
                }
                _addValue = num;
                _multipleValue += num2;
            }
        }
    }

    public static int GetPkHpExPct()
    {
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config != null)
        {
            return _config.pk_hp_ex_pct;
        }
        return 1;
    }

    public static string GetRamdomName()
    {
        List<string> list = new List<string>();
        List<string> list2 = new List<string>();
        List<string> list3 = new List<string>();
        IEnumerator enumerator = ConfigMgr.getInstance().getList<ramdom_name_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ramdom_name_config current = (ramdom_name_config) enumerator.Current;
                if (current.name_first != string.Empty)
                {
                    list.Add(current.name_first);
                }
                if (current.name_last != string.Empty)
                {
                    list2.Add(current.name_last);
                }
                if (current.sp_symbol != string.Empty)
                {
                    list3.Add(current.sp_symbol);
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
        string str2 = list[UnityEngine.Random.Range(0, list.Count)];
        string str3 = list2[UnityEngine.Random.Range(0, list2.Count)];
        if (UnityEngine.Random.Range(0, list3.Count) == 3)
        {
            string str4 = list3[UnityEngine.Random.Range(0, list3.Count)];
            int num2 = UnityEngine.Random.Range(1, 4);
            if (StrParser.GetAsciiLength(str2 + str3) < 12)
            {
                switch (num2)
                {
                    case 1:
                        return (str4 + str2 + str3);

                    case 2:
                        return (str2 + str4 + str3);
                }
                return (str2 + str3 + str4);
            }
            return (str2 + str3);
        }
        return (str2 + str3);
    }

    public static int GetRefreshMailInterval()
    {
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config != null)
        {
            return _config.refresh_mail_interval;
        }
        return 60;
    }

    public static string GetShowStr(int num, int standar = 0x186a0)
    {
        if ((standar != 0) && (num >= standar))
        {
            if ((num % standar) == 0)
            {
                return (((10 * num) / standar) + "万");
            }
            object[] objArray1 = new object[] { (10 * num) / standar, ".", 10 * (num % standar), "万" };
            return string.Concat(objArray1);
        }
        return num.ToString();
    }

    public static skill_config GetSkillCfg(int _cardEntry, int _SkillPos)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_cardEntry);
        if (_config == null)
        {
            return null;
        }
        E_CardSkill skill = (E_CardSkill) _SkillPos;
        int id = 0;
        switch (skill)
        {
            case E_CardSkill.e_card_skill_0:
                id = _config.skill_0;
                break;

            case E_CardSkill.e_card_skill_1:
                id = _config.skill_1;
                break;

            case E_CardSkill.e_card_skill_2:
                id = _config.skill_2;
                break;

            case E_CardSkill.e_card_skill_3:
                id = _config.skill_3;
                break;

            default:
                Debug.LogWarning("未开放技能!");
                break;
        }
        skill_config _config2 = ConfigMgr.getInstance().getByEntry<skill_config>(id);
        if (_config2 == null)
        {
            return null;
        }
        return _config2;
    }

    public static int GetSkillIDByCardEntry(int nCardEntry, E_CardSkill eCardSkill)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(nCardEntry);
        if (_config != null)
        {
            switch (eCardSkill)
            {
                case E_CardSkill.e_card_skill_0:
                    return _config.skill_0;

                case E_CardSkill.e_card_skill_1:
                    return _config.skill_1;

                case E_CardSkill.e_card_skill_2:
                    return _config.skill_2;

                case E_CardSkill.e_card_skill_3:
                    return _config.skill_3;
            }
        }
        return -1;
    }

    public static int GetSkillLevUpCost(int _skillPos, int _level)
    {
        if (_skillPos == 0)
        {
            return (((_level - 1) * 500) + 500);
        }
        if (_skillPos == 1)
        {
            return (((_level + 2) * 500) + 500);
        }
        if (_skillPos == 2)
        {
            return (((_level + 4) * 500) + 500);
        }
        if (_skillPos == 3)
        {
            return (((_level + 6) * 500) + 500);
        }
        return 0;
    }

    public static float GetStaminaToHp(int entry, int starLv)
    {
        card_ex_config cardExCfg = GetCardExCfg(entry, starLv);
        if (cardExCfg == null)
        {
            return 0f;
        }
        return cardExCfg.stamina_to_hp;
    }

    public static int GetStartTime(string _time)
    {
        char[] separator = new char[] { '|' };
        string[] strArray = _time.Split(separator);
        DateTime time = DateTime.Parse(strArray[0] + "-" + strArray[1] + "-" + strArray[2] + " " + strArray[3] + ":" + strArray[4] + ":0");
        return (int) TimeMgr.Instance.ConvertToTimeStamp(time);
    }

    public static void GetSubSkillBuffAdd(int skillLevel, int buffEntry, SkillLogicEffectType _type, out float _addValue, out float _multipleValue)
    {
        _addValue = 0f;
        _multipleValue = 0f;
        float num = 0f;
        float num2 = 0f;
        buff_config _config = ConfigMgr.getInstance().getByEntry<buff_config>(buffEntry);
        if (_config != null)
        {
            if ((_config.effectType1 > 0) && (_config.effectType1 == _type))
            {
                if (_config.effectOperateType1 == 0)
                {
                    num += _config.effectValue1 + ((skillLevel - 1) * _config.effectValue1_lvup);
                }
                else
                {
                    num2 += _config.effectValue1 / 10000f;
                }
            }
            if ((_config.effectType2 > 0) && (_config.effectType2 == _type))
            {
                if (_config.effectOperateType2 == 0)
                {
                    num += _config.effectValue2 + ((skillLevel - 1) * _config.effectValue2_lvup);
                }
                else
                {
                    num2 += _config.effectValue2 / 10000f;
                }
            }
            if ((_config.effectType3 > 0) && (_config.effectType3 == _type))
            {
                if (_config.effectOperateType3 == 0)
                {
                    num += _config.effectValue3 + ((skillLevel - 1) * _config.effectValue3_lvup);
                }
                else
                {
                    num2 += _config.effectValue3 / 10000f;
                }
            }
            _addValue = num;
            _multipleValue = num2;
        }
    }

    public static int GetTitleBarAbsoluteDepth()
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        return ((activityGUIEntity == null) ? 3 : activityGUIEntity.AbsoluteDepth);
    }

    public static int GetTitleBarDepth()
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        return ((activityGUIEntity == null) ? 3 : activityGUIEntity.Depth);
    }

    public static int GetUserMaxLv()
    {
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config != null)
        {
            return _config.max_user_level;
        }
        return 0;
    }

    public static int GetVipLevelByLockFunc(VipLockFunc _func)
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<vip_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                vip_config current = (vip_config) enumerator.Current;
                foreach (int num in GetConfigEntry(current.func))
                {
                    if (num == _func)
                    {
                        return (current.entry + 1);
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
        return 1;
    }

    public static int GetVipMinLevelEliteBuyTimes()
    {
        IEnumerator enumerator = ConfigMgr.getInstance().getList<vip_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                vip_config current = (vip_config) enumerator.Current;
                if (current.elite_buy_times > 0)
                {
                    return (current.entry + 1);
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
        return 1;
    }

    public static double GetWorldBossHp(WorldBossBossData _data)
    {
        world_boss_config _config = ConfigMgr.getInstance().getByEntry<world_boss_config>(_data.entry);
        monster_config _config2 = ConfigMgr.getInstance().getByEntry<monster_config>(_config.monster_card_entry);
        card_config _config3 = ConfigMgr.getInstance().getByEntry<card_config>(_config2.card_entry);
        double num = 1000000.0;
        return ((10.0 * num) + (((_data.level - 1) * 100) * num));
    }

    public static string GetWorldBossHpPercent(ulong _curVal, ulong _maxVal, float val)
    {
        return string.Format("{0:0.00}%", (((double) _curVal) / ((double) _maxVal)) * 100.0);
    }

    public static int GetWorldCupRepeatedCount()
    {
        if (ActorData.getInstance().Level >= LevelLimitCfg().league_matches)
        {
            IEnumerator enumerator = ConfigMgr.getInstance().getList<league_match_config>().GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    league_match_config current = (league_match_config) enumerator.Current;
                    if (current.is_league)
                    {
                        int startTime = GetStartTime(current.start_time);
                        int num2 = Mathf.FloorToInt(((float) (TimeMgr.Instance.ServerStampTime - startTime)) / ((float) current.repeated_period));
                        if (num2 < 0)
                        {
                            num2 = 0;
                        }
                        startTime += current.repeated_period * num2;
                        int num3 = startTime + current.apply_duration;
                        int num4 = num3 + current.regroup_duration;
                        int num5 = num4 + current.pre_match_duration;
                        int num6 = num5 + current.pre_match_finish_duration;
                        int num7 = num6 + current.final_match_duration;
                        int num8 = num7 + current.final_match_finish_duration;
                        int num9 = startTime + current.repeated_period;
                        if ((TimeMgr.Instance.ServerStampTime >= startTime) && (TimeMgr.Instance.ServerStampTime < num3))
                        {
                            return num2;
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
        }
        return -1;
    }

    public static int GetYuanZhengRefreshCoin(int time)
    {
        ArrayList list = ConfigMgr.getInstance().getList<flame_shop_refresh_config>();
        for (int i = 0; i != list.Count; i++)
        {
            flame_shop_refresh_config _config = (flame_shop_refresh_config) list[i];
            if (((time + 1) < _config.count) && (i > 0))
            {
                flame_shop_refresh_config _config2 = (flame_shop_refresh_config) list[i - 1];
                return _config2.cost_value;
            }
        }
        flame_shop_refresh_config _config3 = (flame_shop_refresh_config) list[list.Count - 1];
        return _config3.cost_value;
    }

    public static bool IsExpanFuncBar()
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (null == activityGUIEntity)
        {
            return false;
        }
        return activityGUIEntity.IsFuncBarExpan();
    }

    public static level_limit_config LevelLimitCfg()
    {
        return (level_limit_config) ConfigMgr.getInstance().getList<level_limit_config>()[0];
    }

    public static bool RealCheckActiveCard()
    {
        return ActorData.getInstance().HaveInAcitveCard();
    }

    public static bool RealCheckCardCanLevUp()
    {
        foreach (Card card in ActorData.getInstance().CardList)
        {
            if (CheckHaveEquipLevUp(card))
            {
                return true;
            }
        }
        return false;
    }

    public static void ResetClippingPanel(Transform _panel)
    {
        UIPanel component = _panel.GetComponent<UIPanel>();
        component.transform.localPosition = Vector3.zero;
        component.clipOffset = Vector3.zero;
    }

    public static void SetCardJobIcon(UISprite icon, int _type)
    {
        int index = ((_type >= 0) && (_type <= 9)) ? _type : 0;
        icon.spriteName = GameConstant.CardJobIcon[index];
    }

    public static void SetEquipQualityBorder(UISprite _sp, int _Quality, bool isMini = false)
    {
        switch (_Quality)
        {
            case 0:
                _sp.spriteName = "Ui_Zhuangbei_Frame_0";
                break;

            case 1:
                _sp.spriteName = "Ui_Zhuangbei_Frame_1";
                break;

            case 2:
                _sp.spriteName = "Ui_Zhuangbei_Frame_2";
                break;

            case 3:
                _sp.spriteName = "Ui_Zhuangbei_Frame_3";
                break;

            case 4:
                _sp.spriteName = "Ui_Zhuangbei_Frame_4";
                break;

            case 5:
                _sp.spriteName = "Ui_Zhuangbei_Frame_5";
                break;

            case 6:
                _sp.spriteName = "Ui_Zhuangbei_Frame_6";
                break;

            case 7:
                _sp.spriteName = "Ui_Zhuangbei_Frame_7";
                break;

            default:
                _sp.spriteName = "Ui_Zhuangbei_Frame_0";
                Debug.LogWarning("Quality Is Errory , " + _Quality);
                break;
        }
        if (isMini)
        {
            _sp.spriteName = _sp.spriteName + "mini";
        }
    }

    public static void SetPlayerHeadFrame(UISprite frame, UISprite _Icon, int headFrameEntry)
    {
        headbox_config _config = ConfigMgr.getInstance().getByEntry<headbox_config>((headFrameEntry != -1) ? headFrameEntry : 0);
        if (_config != null)
        {
            frame.spriteName = _config.icon1;
            _Icon.spriteName = !string.IsNullOrEmpty(_config.icon2) ? _config.icon2 : "liuzunjing";
            _Icon.transform.gameObject.SetActive(!string.IsNullOrEmpty(_config.icon2));
        }
        else
        {
            Debug.Log("headbox_config Error:" + headFrameEntry);
        }
    }

    public static void SetPreOutlandFight()
    {
        if (!ActorData.getInstance().isPreOutlandFight)
        {
            ActorData.getInstance().isPreOutlandFight = (ActorData.getInstance().Level >= LevelLimitCfg().outland_beginner) && (ActorData.getInstance().Level < LevelLimitCfg().outlan);
            if ((ActorData.getInstance().outlandTitles != null) && (ActorData.getInstance().outlandTitles.Count >= 4))
            {
                ActorData.getInstance().isPreOutlandFight = ActorData.getInstance().isPreOutlandFight || (ActorData.getInstance().outlandTitles[3].is_underway && (ActorData.getInstance().outlandTitles[3].underway_entry != -1));
            }
        }
    }

    public static void SetQualityBorder(UISprite _sp, int _Quality)
    {
        switch (_Quality)
        {
            case 0:
                _sp.spriteName = "Ui_Hero_Frame_1";
                break;

            case 1:
                _sp.spriteName = "Ui_Hero_Frame_2";
                break;

            case 2:
                _sp.spriteName = "Ui_Hero_Frame_3";
                break;

            case 3:
                _sp.spriteName = "Ui_Hero_Frame_4";
                break;

            case 4:
                _sp.spriteName = "Ui_Hero_Frame_5";
                break;

            case 5:
                _sp.spriteName = "Ui_Hero_Frame_6";
                break;

            case 6:
                _sp.spriteName = "Ui_Hero_Frame_7";
                break;

            case 7:
                _sp.spriteName = "Ui_Hero_Frame_1";
                break;

            default:
                _sp.spriteName = "Ui_Hero_Frame_1";
                Debug.LogWarning("Quality Is Errory , " + _Quality);
                break;
        }
    }

    public static unsafe void SetQualityColor(UISprite _sp, int _Quality)
    {
        if ((_Quality < GameConstant.ConstQuantityColor.Length) && (_Quality >= 0))
        {
            _sp.color = *((Color*) &(GameConstant.ConstQuantityColor[_Quality]));
        }
        else
        {
            Debug.LogWarning("Quality Is Errory , " + _Quality);
            _sp.color = Color.white;
        }
    }

    public static void SetTencentVipPic(UISprite _sprite)
    {
        if (null != _sprite)
        {
            switch (ActorData.getInstance().UserInfo.qq_vip_type)
            {
                case 1:
                    _sprite.spriteName = "vip1";
                    break;

                case 2:
                    _sprite.spriteName = "vip2";
                    break;

                case 3:
                    _sprite.spriteName = "vip3";
                    break;

                case 4:
                    _sprite.spriteName = "vip4";
                    break;

                case 5:
                    _sprite.spriteName = "vip5";
                    break;

                case 6:
                    _sprite.spriteName = "vip6";
                    break;

                case 7:
                    _sprite.spriteName = "vip7";
                    break;

                case 8:
                    _sprite.spriteName = "svip1";
                    break;

                case 9:
                    _sprite.spriteName = "svip2";
                    break;

                case 10:
                    _sprite.spriteName = "svip3";
                    break;

                case 11:
                    _sprite.spriteName = "svip4";
                    break;

                case 12:
                    _sprite.spriteName = "svip5";
                    break;

                case 13:
                    _sprite.spriteName = "svip6";
                    break;

                case 14:
                    _sprite.spriteName = "svip7";
                    break;

                default:
                    _sprite.spriteName = string.Empty;
                    break;
            }
            _sprite.MakePixelPerfect();
        }
    }

    public static void SetTitleBarDepth(int depth)
    {
        TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
        if (activityGUIEntity != null)
        {
            activityGUIEntity.Depth = depth;
        }
    }

    public static void ShowFuncList(bool _show)
    {
    }

    public static void ShowTitlebar(bool show)
    {
    }

    public static bool SkillIsUnLock(E_CardSkill _EcardSkill, int _cardQuality)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(1);
        if (_EcardSkill == E_CardSkill.e_card_skill_0)
        {
            return true;
        }
        if (_EcardSkill == E_CardSkill.e_card_skill_1)
        {
            return (_cardQuality >= 1);
        }
        if (_EcardSkill == E_CardSkill.e_card_skill_2)
        {
            return (_cardQuality >= 2);
        }
        return ((_EcardSkill == E_CardSkill.e_card_skill_3) && (_cardQuality >= 4));
    }

    public static int SortByPosition(Card card1, Card card2)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card1.cardInfo.entry);
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) card2.cardInfo.entry);
        return (_config.card_position - _config2.card_position);
    }

    public static void TencentLoginCallBack(int type)
    {
        switch (type)
        {
            case 0x7d0:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51d));
                break;

            case 0x7d1:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51e));
                break;

            case 0x7d2:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51f));
                break;

            case 0x7d4:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x521));
                break;

            case 0x3e8:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x515));
                break;

            case 0x3e9:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x516));
                break;

            case 0x3ea:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x517));
                break;

            case 0x3eb:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x518));
                break;

            case 0x3ec:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x519));
                break;

            case 0x3ed:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51a));
                break;

            case 0x3ee:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51b));
                break;

            case 0x3ef:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51c));
                break;

            case -1:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x526));
                break;
        }
    }

    public static void TencentShareCallBack(int type)
    {
        Debug.Log("Share callBack type = " + type);
        if (type == 0)
        {
            PushNotifyPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<PushNotifyPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ShareSuccess();
            }
        }
        else
        {
            PushNotifyPanel panel2 = GUIMgr.Instance.GetActivityGUIEntity<PushNotifyPanel>();
            if (null != panel2)
            {
                panel2.ShareFaile();
            }
        }
        switch (type)
        {
            case 0x7d0:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51d));
                break;

            case 0x7d1:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51e));
                break;

            case 0x7d4:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x521));
                break;

            case 0x3e8:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x515));
                break;

            case 0x3ea:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x517));
                break;

            case 0x3eb:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x518));
                break;

            case 0x3ec:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x519));
                break;

            case 0x3ed:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51a));
                break;

            case 0x3ee:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51b));
                break;

            case 0x3ef:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x51c));
                break;

            case -1:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x526));
                break;
        }
    }

    public int TryToGetValue(string key)
    {
        System.Type type = typeof(variable_config);
        variable_config _config = ConfigMgr.getInstance().getByEntry<variable_config>(0);
        if (_config == null)
        {
            return -1;
        }
        FieldInfo field = type.GetField(key);
        if (field == null)
        {
            return -1;
        }
        return (int) field.GetValue(_config);
    }

    public static void UpdateBossHpColor(UISprite frontHp, UISprite BackHp, int HpLevel)
    {
        if (HpLevel <= 0)
        {
            BackHp.gameObject.SetActive(false);
            frontHp.spriteName = HpSpriteList[HpSpriteList.Length - 1];
        }
        else
        {
            BackHp.gameObject.SetActive(true);
            int index = HpLevel % HpSpriteList.Length;
            if (index == 0)
            {
                BackHp.spriteName = HpSpriteList[HpSpriteList.Length - 1];
                frontHp.spriteName = HpSpriteList[index];
            }
            else
            {
                BackHp.spriteName = HpSpriteList[index - 1];
                frontHp.spriteName = HpSpriteList[index];
            }
        }
    }

    [CompilerGenerated]
    private sealed class <CheckBreakMaterialEnough>c__AnonStorey1A9
    {
        internal break_equip_config bec;

        internal bool <>m__235(Item e)
        {
            return ((e.entry == this.bec.need_item_1) && (e.num >= this.bec.need_num_1));
        }

        internal bool <>m__236(Item e)
        {
            return ((e.entry == this.bec.need_item_2) && (e.num >= this.bec.need_num_2));
        }

        internal bool <>m__237(Item e)
        {
            return ((e.entry == this.bec.need_item_3) && (e.num >= this.bec.need_num_3));
        }
    }

    [CompilerGenerated]
    private sealed class <CheckCardCanUpStar>c__AnonStorey1AA
    {
        internal card_ex_config cec;

        internal bool <>m__238(Item e)
        {
            return (e.entry == this.cec.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckEquipUpMaterial>c__AnonStorey1A8
    {
        internal int entry;
        internal item_config ic;

        internal bool <>m__231(Item e)
        {
            return (e.entry == this.entry);
        }

        internal bool <>m__232(Item e)
        {
            return (e.entry == this.ic.param_0);
        }

        internal bool <>m__233(Item e)
        {
            return (e.entry == this.ic.param_2);
        }

        internal bool <>m__234(Item e)
        {
            return (e.entry == this.ic.param_4);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckHaveCardCanUpStar>c__AnonStorey1AB
    {
        internal card_ex_config cec;

        internal bool <>m__239(Item e)
        {
            return (e.entry == this.cec.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckHaveEquipLevUp>c__AnonStorey1A7
    {
        internal break_equip_config bec;

        internal bool <>m__230(Item e)
        {
            return (e.entry == this.bec.break_equip_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <CheckIsFrozenFun>c__AnonStorey1A6
    {
        internal int endTime;
        internal int timeLen;

        internal void <>m__22F(GUIEntity e)
        {
            ((MessageBox) e).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x587), TimeMgr.Instance.GetTimeStr(this.timeLen), TimeMgr.Instance.GetFrozenTime(this.endTime)), null, null, true);
        }
    }
}

