using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeagueHistoryPanel : GUIEntity
{
    public GameObject _SingleArenaHistroyItem;
    public GameObject _SingleLeagueLogItem;
    [CompilerGenerated]
    private static Comparison<ArenaLadderCombatRecord> <>f__am$cache3;
    private int mMaxCount = 30;

    private void OnClickLog(GameObject obj)
    {
    }

    public override void OnInitialize()
    {
    }

    private int SortByTime(LeagueHistory info1, LeagueHistory info2)
    {
        return (info2.time - info1.time);
    }

    public void UpdateData(List<ArenaLadderCombatRecord> historyList)
    {
        Debug.Log("----------------ArenaLadderCombatRecord historyList:>" + historyList.Count);
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = (a1, a2) => a2.combat_time - a1.combat_time;
        }
        historyList.Sort(<>f__am$cache3);
        UIGrid component = base.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        for (int i = 0; i < this.mMaxCount; i++)
        {
            if (i < historyList.Count)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleArenaHistroyItem) as GameObject;
                obj2.transform.parent = component.transform;
                obj2.transform.localPosition = new Vector3(0f, -i * component.cellHeight, -0.1f);
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                Transform transform = obj2.transform.FindChild("Item");
                transform.GetComponent<UIDragScrollView>().enabled = historyList.Count > 4;
                transform.FindChild("Name").GetComponent<UILabel>().text = historyList[i].target_name;
                transform.FindChild("LvGroup/Level").GetComponent<UILabel>().text = "Lv." + historyList[i].level;
                transform.FindChild("Time").GetComponent<UILabel>().text = TimeMgr.Instance.GetSendTime(historyList[i].combat_time);
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(historyList[i].head_entry);
                if (_config != null)
                {
                    transform.FindChild("HeadIcon/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                }
                UISprite sprite = transform.FindChild("upIcon").GetComponent<UISprite>();
                UISprite sprite2 = transform.FindChild("downIcon").GetComponent<UISprite>();
                UILabel label4 = transform.FindChild("Rank").GetComponent<UILabel>();
                label4.text = historyList[i].order_change.ToString();
                bool flag = false;
                if (historyList[i].order_change > 0)
                {
                    flag = true;
                    sprite.gameObject.SetActive(true);
                    sprite2.gameObject.SetActive(false);
                }
                else if (historyList[i].order_change == 0)
                {
                    label4.text = string.Empty;
                    flag = !historyList[i].is_attacker;
                    sprite.gameObject.SetActive(false);
                    sprite2.gameObject.SetActive(false);
                }
                else
                {
                    sprite.gameObject.SetActive(false);
                    sprite2.gameObject.SetActive(true);
                }
                transform.FindChild("Win").GetComponent<UISprite>().spriteName = !flag ? "Ui_Worldcup_Icon_lose" : "Ui_Worldcup_Icon_win";
            }
        }
    }

    public void UpdateData(List<LeagueHistory> historyList)
    {
        Debug.Log("----------------historyList:>" + historyList.Count);
        historyList.Sort(new Comparison<LeagueHistory>(this.SortByTime));
        UIGrid component = base.gameObject.transform.FindChild("List/Grid").GetComponent<UIGrid>();
        CommonFunc.DeleteChildItem(component.transform);
        for (int i = 0; i < this.mMaxCount; i++)
        {
            if (i < historyList.Count)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleLeagueLogItem) as GameObject;
                obj2.transform.parent = component.transform;
                obj2.transform.localPosition = new Vector3(0f, -i * component.cellHeight, -0.1f);
                obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                obj2.name = "No" + i;
                GUIDataHolder.setData(obj2.gameObject, historyList[i]);
                UIEventListener.Get(obj2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLog);
                obj2.gameObject.SetActive(true);
                UILabel label = obj2.transform.FindChild("Label").GetComponent<UILabel>();
                UISprite sprite = obj2.transform.FindChild("Icon").GetComponent<UISprite>();
                switch (historyList[i].type)
                {
                    case LeagueHistoryType.LeagueHistoryType_AttackAndWin:
                        label.text = string.Format(ConfigMgr.getInstance().GetWord(0xa1be42), GameConstant.BroadmsgColor + TimeMgr.Instance.GetSendTime(historyList[i].time) + GameConstant.C_White, GameConstant.C_Green + historyList[i].opponentName + GameConstant.C_White, GameConstant.C_Red + historyList[i].myRank + GameConstant.C_White);
                        sprite.spriteName = "Ui_Worldcup_Icon_win";
                        break;

                    case LeagueHistoryType.LeagueHistoryType_AttackAndLost:
                        label.text = string.Format(ConfigMgr.getInstance().GetWord(0xa1be43), GameConstant.BroadmsgColor + TimeMgr.Instance.GetSendTime(historyList[i].time) + GameConstant.C_White, GameConstant.C_Green + historyList[i].opponentName + GameConstant.C_White, GameConstant.C_Red + historyList[i].myRank + GameConstant.C_White);
                        sprite.spriteName = "Ui_Worldcup_Icon_lose";
                        break;

                    case LeagueHistoryType.LeagueHistoryType_BeAttackAndWin:
                        label.text = string.Format(ConfigMgr.getInstance().GetWord(0xa1be45), GameConstant.BroadmsgColor + TimeMgr.Instance.GetSendTime(historyList[i].time) + GameConstant.C_White, GameConstant.C_Green + historyList[i].opponentName + GameConstant.C_White, GameConstant.C_Red + historyList[i].myRank + GameConstant.C_White);
                        sprite.spriteName = "Ui_Worldcup_Icon_win";
                        break;

                    case LeagueHistoryType.LeagueHistoryType_BeAttackAndLost:
                        label.text = string.Format(ConfigMgr.getInstance().GetWord(0xa1be44), GameConstant.BroadmsgColor + TimeMgr.Instance.GetSendTime(historyList[i].time) + GameConstant.C_White, GameConstant.C_Green + historyList[i].opponentName + GameConstant.C_White, GameConstant.C_Red + historyList[i].myRank + GameConstant.C_White);
                        sprite.spriteName = "Ui_Worldcup_Icon_lose";
                        break;
                }
            }
        }
    }
}

