using FastBuf;
using System;
using System.Collections;
using UnityEngine;

public class WorldCupRulePanel : GUIEntity
{
    private void ExitPanel()
    {
    }

    private arenaladder_daily_reward_config GetArenaRewardCfg(int currRank)
    {
        ArrayList list = ConfigMgr.getInstance().getList<arenaladder_daily_reward_config>();
        for (int i = 0; i < list.Count; i++)
        {
            arenaladder_daily_reward_config _config = (arenaladder_daily_reward_config) list[i];
            char[] separator = new char[] { '|' };
            string[] strArray = _config.range.Split(separator);
            if (strArray.Length == 1)
            {
                if (int.Parse(strArray[0]) == currRank)
                {
                    return _config;
                }
            }
            else
            {
                int num3 = int.Parse(strArray[0]);
                int num4 = int.Parse(strArray[1]);
                if ((currRank >= num3) && (currRank <= num4))
                {
                    return _config;
                }
            }
        }
        return null;
    }

    private lolarena_daily_reward_config GetLoLArenaRewardCfg(int currRank)
    {
        ArrayList list = ConfigMgr.getInstance().getList<lolarena_daily_reward_config>();
        for (int i = 0; i < list.Count; i++)
        {
            lolarena_daily_reward_config _config = (lolarena_daily_reward_config) list[i];
            char[] separator = new char[] { '|' };
            string[] strArray = _config.range.Split(separator);
            if (strArray.Length == 1)
            {
                if (int.Parse(strArray[0]) == currRank)
                {
                    return _config;
                }
            }
            else
            {
                int num3 = int.Parse(strArray[0]);
                int num4 = int.Parse(strArray[1]);
                if ((currRank >= num3) && (currRank <= num4))
                {
                    return _config;
                }
            }
        }
        return null;
    }

    public void SetHongBaoRule()
    {
        base.transform.FindChild("List/HongBaoRuleLabel").gameObject.SetActive(true);
    }

    private void SetLoLRewardInfo(Transform obj, lolarena_daily_reward_config cfg)
    {
        if (cfg != null)
        {
            obj.FindChild("1/Num").GetComponent<UILabel>().text = cfg.stone.ToString();
            obj.FindChild("2/Num").GetComponent<UILabel>().text = cfg.gold.ToString();
            obj.FindChild("3/Num").GetComponent<UILabel>().text = "X" + cfg.arenaladder_score;
            if (cfg.arenaladder_score < 1)
            {
                obj.FindChild("3").gameObject.SetActive(false);
            }
            if (cfg.item_num < 1)
            {
                obj.FindChild("4").gameObject.SetActive(false);
            }
            else
            {
                UITexture component = obj.FindChild("4/Icon/Sprite").GetComponent<UITexture>();
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(cfg.item_entry);
                if (_config != null)
                {
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                }
                obj.FindChild("4/Num").GetComponent<UILabel>().text = "X" + cfg.item_num;
            }
        }
    }

    public void SetOutlandRule()
    {
        base.transform.FindChild("List").gameObject.SetActive(false);
        base.transform.FindChild("UserAgreeList").gameObject.SetActive(false);
        base.transform.FindChild("OutlandLabelList").gameObject.SetActive(true);
        base.transform.FindChild("PaokuLabelList").gameObject.SetActive(false);
    }

    public void SetPaokuRule()
    {
        base.transform.FindChild("List").gameObject.SetActive(false);
        base.transform.FindChild("UserAgreeList").gameObject.SetActive(false);
        base.transform.FindChild("OutlandLabelList").gameObject.SetActive(false);
        base.transform.FindChild("PaokuLabelList").gameObject.SetActive(true);
    }

    private void SetRewardInfo(Transform obj, arenaladder_daily_reward_config cfg)
    {
        if (cfg != null)
        {
            obj.FindChild("1/Num").GetComponent<UILabel>().text = cfg.stone.ToString();
            obj.FindChild("2/Num").GetComponent<UILabel>().text = cfg.gold.ToString();
            obj.FindChild("3/Num").GetComponent<UILabel>().text = "X" + cfg.arenaladder_score;
            if (cfg.arenaladder_score < 1)
            {
                obj.FindChild("3").gameObject.SetActive(false);
            }
            if (cfg.item_num < 1)
            {
                obj.FindChild("4").gameObject.SetActive(false);
            }
            else
            {
                UITexture component = obj.FindChild("4/Icon/Sprite").GetComponent<UITexture>();
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(cfg.item_entry);
                if (_config != null)
                {
                    component.mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                }
                obj.FindChild("4/Num").GetComponent<UILabel>().text = "X" + cfg.item_num;
            }
        }
    }

    public void SetSoulBoxRule()
    {
        base.transform.FindChild("List/WorldCupLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/ArenaLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/YuanZhengLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/SoulBoxRuleLabel").gameObject.SetActive(true);
    }

    public void SetUserAgreeLabel()
    {
        UILabel component = base.transform.FindChild("TopLabel").gameObject.GetComponent<UILabel>();
        if (component != null)
        {
            component.text = ConfigMgr.getInstance().GetWord(0x4e30);
        }
        base.transform.FindChild("List").gameObject.SetActive(false);
        base.transform.FindChild("UserAgreeList").gameObject.SetActive(true);
    }

    public void SetYuanZhengRule()
    {
        base.transform.FindChild("List/WorldCupLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/ArenaLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/YuanZhengLabel").gameObject.SetActive(true);
    }

    public void ShowArenaLadderRule(int myRank, int myTopRank)
    {
        base.transform.FindChild("List/WorldCupLabel").gameObject.SetActive(false);
        base.transform.FindChild("List/YuanZhengLabel").gameObject.SetActive(false);
        Transform transform = base.transform.FindChild("List/ArenaLabel");
        UILabel component = transform.FindChild("TopInfo/Label1").GetComponent<UILabel>();
        arenaladder_daily_reward_config arenaRewardCfg = this.GetArenaRewardCfg(myRank);
        Transform transform2 = transform.FindChild("TopInfo/Reward");
        if (arenaRewardCfg != null)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = arenaRewardCfg.range.Split(separator);
            if (strArray.Length == 1)
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x84b), myRank, int.Parse(strArray[0]));
            }
            else
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x848), myRank, int.Parse(strArray[0]), int.Parse(strArray[1]));
            }
            this.SetRewardInfo(transform2, arenaRewardCfg);
        }
        else
        {
            component.text = string.Format(ConfigMgr.getInstance().GetWord(0x84a), myRank);
            transform2.gameObject.SetActive(false);
        }
        transform.FindChild("TopInfo/Rank").GetComponent<UILabel>().text = myTopRank.ToString();
        Transform transform3 = transform.FindChild("Rule/3/Reward1");
        Transform transform4 = transform.FindChild("Rule/3/Reward2");
        Transform transform5 = transform.FindChild("Rule/3/Reward3");
        arenaladder_daily_reward_config cfg = ConfigMgr.getInstance().getByEntry<arenaladder_daily_reward_config>(0);
        arenaladder_daily_reward_config _config3 = ConfigMgr.getInstance().getByEntry<arenaladder_daily_reward_config>(1);
        arenaladder_daily_reward_config _config4 = ConfigMgr.getInstance().getByEntry<arenaladder_daily_reward_config>(2);
        if (cfg != null)
        {
            this.SetRewardInfo(transform3, cfg);
        }
        if (_config3 != null)
        {
            this.SetRewardInfo(transform4, _config3);
        }
        if (_config4 != null)
        {
            this.SetRewardInfo(transform5, _config4);
        }
        transform.gameObject.SetActive(true);
    }

    public void ShowLoLArenaLadderRule(int myRank, int myTopRank)
    {
        Transform transform = base.transform.FindChild("List/LoLArenaLabel");
        UILabel component = transform.FindChild("TopInfo/Label1").GetComponent<UILabel>();
        lolarena_daily_reward_config loLArenaRewardCfg = this.GetLoLArenaRewardCfg(myRank);
        Transform transform2 = transform.FindChild("TopInfo/Reward");
        if (loLArenaRewardCfg != null)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = loLArenaRewardCfg.range.Split(separator);
            if (strArray.Length == 1)
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x891), myRank, int.Parse(strArray[0]));
            }
            else
            {
                component.text = string.Format(ConfigMgr.getInstance().GetWord(0x88e), myRank, int.Parse(strArray[0]), int.Parse(strArray[1]));
            }
            this.SetLoLRewardInfo(transform2, loLArenaRewardCfg);
        }
        else
        {
            component.text = string.Format(ConfigMgr.getInstance().GetWord(0x84a), myRank);
            transform2.gameObject.SetActive(false);
        }
        transform.FindChild("TopInfo/Rank").GetComponent<UILabel>().text = myTopRank.ToString();
        Transform transform3 = transform.FindChild("Rule/3/Reward1");
        Transform transform4 = transform.FindChild("Rule/3/Reward2");
        Transform transform5 = transform.FindChild("Rule/3/Reward3");
        lolarena_daily_reward_config cfg = ConfigMgr.getInstance().getByEntry<lolarena_daily_reward_config>(0);
        lolarena_daily_reward_config _config3 = ConfigMgr.getInstance().getByEntry<lolarena_daily_reward_config>(1);
        lolarena_daily_reward_config _config4 = ConfigMgr.getInstance().getByEntry<lolarena_daily_reward_config>(2);
        if (cfg != null)
        {
            this.SetLoLRewardInfo(transform3, cfg);
        }
        if (_config3 != null)
        {
            this.SetLoLRewardInfo(transform4, _config3);
        }
        if (_config4 != null)
        {
            this.SetLoLRewardInfo(transform5, _config4);
        }
        transform.gameObject.SetActive(true);
    }

    public void ShowWorldCup(bool _isWorldCup)
    {
        base.transform.FindChild("List/WorldCupLabel").gameObject.SetActive(_isWorldCup);
        base.transform.FindChild("List/ArenaLabel").gameObject.SetActive(!_isWorldCup);
    }
}

