using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Toolbox;
using UnityEngine;

public class TargetInfoPanel : GUIEntity
{
    [CompilerGenerated]
    private static Comparison<CardInfo> <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<Card> <>f__am$cache1;

    private void OnClickBattleSelf()
    {
        <OnClickBattleSelf>c__AnonStorey173 storey = new <OnClickBattleSelf>c__AnonStorey173();
        Debug.LogWarning("OnClickBattleSelf!");
        storey.dataInfo = new ConvoyEnemyInfo();
        storey.dataInfo = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo;
        storey.dataInfo.targetId = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.targetId;
        storey.dataInfo.headEntry = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.headEntry;
        storey.dataInfo.head_frame_entry = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.head_frame_entry;
        storey.dataInfo.lvl = (ushort) XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.lvl;
        storey.dataInfo.name = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.name;
        storey.dataInfo.cardList = XSingleton<GameDetainsDartMgr>.Singleton.curEnermyInfo.cardList;
        if (storey.dataInfo != null)
        {
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", new Action<GUIEntity>(storey.<>m__19D), null);
            GUIMgr.Instance.ExitModelGUI(this);
        }
    }

    private void OnClickFriendForHelp()
    {
        TipsDiag.SetText("当前功能暂未开放，敬请期待！");
    }

    public void SetCardInfo(Transform obj, CardInfo cardInfo)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) cardInfo.entry);
        if (_config != null)
        {
            CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), cardInfo.quality);
            obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            CommonFunc.SetCardJobIcon(obj.FindChild("Job/jobIcon").GetComponent<UISprite>(), _config.class_type);
            obj.FindChild("Level").GetComponent<UILabel>().text = cardInfo.level.ToString();
            for (int i = 0; i < 5; i++)
            {
                UISprite component = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                component.gameObject.SetActive(i < cardInfo.starLv);
                component.transform.localPosition = new Vector3((float) (i * 0x13), 0f, 0f);
            }
            Transform transform = obj.transform.FindChild("Star");
            transform.localPosition = new Vector3(-6.8f - ((cardInfo.starLv - 1) * 9.5f), transform.localPosition.y, 0f);
            transform.gameObject.SetActive(true);
        }
    }

    public void SetTargetTeamInfo(List<Card> cardList)
    {
        int num = 0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (Card card1, Card card2) {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card1.cardInfo.entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) card2.cardInfo.entry);
                int num = (_config == null) ? -1 : _config.card_position;
                int num2 = (_config2 == null) ? -1 : _config2.card_position;
                return num - num2;
            };
        }
        cardList.Sort(<>f__am$cache1);
        for (int i = 0; i < 5; i++)
        {
            Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
            if (num < cardList.Count)
            {
                this.SetCardInfo(transform, cardList[num].cardInfo);
                transform.gameObject.SetActive(true);
                num++;
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void SetTargetTeamInfo2(List<CardInfo> cardList)
    {
        int num = 0;
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate (CardInfo card1, CardInfo card2) {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card1.entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) card2.entry);
                int num = (_config == null) ? -1 : _config.card_position;
                int num2 = (_config2 == null) ? -1 : _config2.card_position;
                return num - num2;
            };
        }
        cardList.Sort(<>f__am$cache0);
        Transform transform = base.transform.FindChild("Team");
        for (int i = 0; i < 8; i++)
        {
            Transform transform2 = base.transform.FindChild("Team/Pos" + (i + 1));
            if (num < cardList.Count)
            {
                this.SetCardInfo(transform2, cardList[num]);
                transform2.gameObject.SetActive(true);
                num++;
            }
            else
            {
                transform2.gameObject.SetActive(false);
            }
        }
        UILabel component = base.transform.FindChild("Team/GuildText").GetComponent<UILabel>();
        int num3 = !string.IsNullOrEmpty(component.text) ? 30 : 0;
        if ((num3 > 0) && (num > 5))
        {
            Vector3 localPosition = component.transform.localPosition;
            component.transform.localPosition = new Vector3(localPosition.x, localPosition.y - 130f, localPosition.z);
        }
        base.transform.FindChild("Bg").GetComponent<UISprite>().height = (num <= 5) ? 0x13e : (0x19d + num3);
    }

    public void UpdateData(ArenaLadderEnemy info)
    {
        UISprite component = base.transform.FindChild("Bg").GetComponent<UISprite>();
        UILabel label = base.transform.FindChild("Head/Level").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Info/Name").GetComponent<UILabel>();
        UILabel label3 = base.transform.FindChild("Info/Label").GetComponent<UILabel>();
        UILabel label4 = base.transform.FindChild("Team/GuildText").GetComponent<UILabel>();
        UIButton button = base.transform.FindChild("Btn_ForHelp").GetComponent<UIButton>();
        UIButton button2 = base.transform.FindChild("Btn_BattleSelf").GetComponent<UIButton>();
        int num = 0;
        button.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        EventDelegate.Add(button.onClick, new EventDelegate.Callback(this.OnClickFriendForHelp));
        EventDelegate.Add(button2.onClick, new EventDelegate.Callback(this.OnClickBattleSelf));
        label2.text = info.name;
        label.text = info.level.ToString();
        string[] textArray1 = new string[] { string.Format(ConfigMgr.getInstance().GetWord(0x281e), "             " + info.order), "\n", string.Format(ConfigMgr.getInstance().GetWord(0x281d), "         " + info.win_count), "\n", string.Format(ConfigMgr.getInstance().GetWord(0x281c), "     " + info.team_power) };
        string str = string.Concat(textArray1);
        label3.text = str;
        UISprite frame = base.transform.FindChild("Head/QualityBorder").GetComponent<UISprite>();
        UISprite sprite3 = base.transform.FindChild("Head/QualityBorder/QIcon").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite3, info.head_frame_entry);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.head_entry);
        if (_config != null)
        {
            base.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        int num2 = (label3.height <= 0x40) ? 0 : (label3.height - 0x40);
        Transform transform = base.transform.FindChild("Team");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - num2, transform.localPosition.y);
        if (info.guild_name != string.Empty)
        {
            label4.text = string.Format(ConfigMgr.getInstance().GetWord(0x281f), info.guild_name);
            component.height = (0x124 + num2) + num;
        }
        else
        {
            component.height = (0x100 + num2) + num;
            label4.text = string.Empty;
        }
        this.SetTargetTeamInfo2(info.cardList);
    }

    internal void UpdateDataForAllRank(EN_RankListType rankType, RankListPlayerInfo info)
    {
        UILabel component = base.transform.FindChild("Head/Level").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Info/Name").GetComponent<UILabel>();
        UILabel label3 = base.transform.FindChild("Info/Label").GetComponent<UILabel>();
        UIButton button = base.transform.FindChild("Btn_ForHelp").GetComponent<UIButton>();
        UIButton button2 = base.transform.FindChild("Btn_BattleSelf").GetComponent<UIButton>();
        if (button != null)
        {
            button.gameObject.SetActive(false);
        }
        if (button2 != null)
        {
            button2.gameObject.SetActive(false);
        }
        label2.text = info.name;
        component.text = info.level.ToString();
        StringBuilder builder = new StringBuilder();
        builder.Append(ConfigMgr.getInstance().GetWord(0xbc1));
        builder.Append("              ");
        builder.Append((int) (info.order + 1));
        builder.Append("\n");
        switch (rankType)
        {
            case EN_RankListType.EN_RANKLIST_LEVEL:
                builder.Append(ConfigMgr.getInstance().GetWord(0x5a4));
                builder.Append(":");
                builder.Append("             ");
                builder.Append(!string.IsNullOrEmpty(info.guildname) ? info.guildname : ConfigMgr.getInstance().GetWord(6));
                break;

            case EN_RankListType.EN_RANKLIST_TOP_FIVE_CARD:
                builder.Append(ConfigMgr.getInstance().GetWord(0xbbf) + ConfigMgr.getInstance().GetWord(0xbbe));
                builder.Append(":");
                builder.Append(" ");
                builder.Append(info.five_power);
                break;

            case EN_RankListType.EN_RANKLIST_ALL_CARD:
                builder.Append(ConfigMgr.getInstance().GetWord(0xbbd));
                builder.Append(":");
                builder.Append("         ");
                builder.Append(info.all_power);
                break;

            case EN_RankListType.EN_RANKLIST_TOWER:
                builder.Append(ConfigMgr.getInstance().GetWord(0xbbc));
                builder.Append(":");
                builder.Append("             ");
                builder.Append(info.tower_num);
                break;

            case EN_RankListType.EN_RANKLIST_AREAN:
                builder.Append(ConfigMgr.getInstance().GetWord(0xbc4));
                builder.Append(":");
                builder.Append("     ");
                builder.Append(info.wins_num);
                builder.Append("\n");
                builder.Append(ConfigMgr.getInstance().GetWord(0xbc2));
                builder.Append("          ");
                builder.Append(info.five_power);
                break;
        }
        label3.text = builder.ToString();
        UISprite frame = base.transform.FindChild("Head/QualityBorder").GetComponent<UISprite>();
        UISprite sprite2 = base.transform.FindChild("Head/QualityBorder/QIcon").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite2, info.head_frame_entry);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        int num = (label3.height <= 0x40) ? 0 : (label3.height - 0x40);
        Transform transform = base.transform.FindChild("Team");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - num, transform.localPosition.y);
        base.transform.FindChild("Bg").GetComponent<UISprite>().height = 0x100 + num;
        base.transform.FindChild("Team/GuildText").GetComponent<UILabel>().text = string.Empty;
        List<CardInfo> cardList = new List<CardInfo>(info.cardinfo.Count);
        if (info.cardinfo != null)
        {
            for (int i = 0; i < info.cardinfo.Count; i++)
            {
                RandListCardInfo info2 = info.cardinfo[i];
                CardInfo item = new CardInfo {
                    entry = (uint) info2.entry,
                    starLv = info2.starLv,
                    level = info2.level,
                    quality = info2.quality
                };
                cardList.Add(item);
            }
            this.SetTargetTeamInfo2(cardList);
        }
    }

    public void UpdateDetainsDartData(ConvoyEnemyInfo info)
    {
        UISprite component = base.transform.FindChild("Bg").GetComponent<UISprite>();
        UILabel label = base.transform.FindChild("Head/Level").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Info/Name").GetComponent<UILabel>();
        UILabel label3 = base.transform.FindChild("Info/Label").GetComponent<UILabel>();
        UILabel label4 = base.transform.FindChild("Team/GuildText").GetComponent<UILabel>();
        UIButton button = base.transform.FindChild("Btn_ForHelp").GetComponent<UIButton>();
        UIButton button2 = base.transform.FindChild("Btn_BattleSelf").GetComponent<UIButton>();
        int num = 60;
        button.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        EventDelegate.Add(button.onClick, new EventDelegate.Callback(this.OnClickFriendForHelp));
        EventDelegate.Add(button2.onClick, new EventDelegate.Callback(this.OnClickBattleSelf));
        label2.text = info.name;
        label.text = info.lvl.ToString();
        UISprite frame = base.transform.FindChild("Head/QualityBorder").GetComponent<UISprite>();
        UISprite sprite3 = base.transform.FindChild("Head/QualityBorder/QIcon").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite3, info.head_frame_entry);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        int num2 = (label3.height <= 0x40) ? 0 : (label3.height - 0x40);
        Transform transform = base.transform.FindChild("Team");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - num2, transform.localPosition.y);
        label4.text = string.Empty;
        component.height = 0x100 + num;
        this.SetTargetTeamInfo2(info.cardList);
    }

    public void UpdateDetainsDartData(GameDetainsDartMgr.EnermyItemData info)
    {
        XSingleton<GameDetainsDartMgr>.Singleton.curBattleEnermyInfo = info;
        UISprite component = base.transform.FindChild("Bg").GetComponent<UISprite>();
        UILabel label = base.transform.FindChild("Head/Level").GetComponent<UILabel>();
        UILabel label2 = base.transform.FindChild("Info/Name").GetComponent<UILabel>();
        UILabel label3 = base.transform.FindChild("Info/Label").GetComponent<UILabel>();
        UILabel label4 = base.transform.FindChild("Team/GuildText").GetComponent<UILabel>();
        UIButton button = base.transform.FindChild("Btn_ForHelp").GetComponent<UIButton>();
        UIButton button2 = base.transform.FindChild("Btn_BattleSelf").GetComponent<UIButton>();
        int num = 60;
        button.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        EventDelegate.Add(button.onClick, new EventDelegate.Callback(this.OnClickFriendForHelp));
        EventDelegate.Add(button2.onClick, new EventDelegate.Callback(this.OnClickBattleSelf));
        label2.text = info.playerName;
        label.text = info.playerLv.ToString();
        UISprite frame = base.transform.FindChild("Head/QualityBorder").GetComponent<UISprite>();
        UISprite sprite3 = base.transform.FindChild("Head/QualityBorder/QIcon").GetComponent<UISprite>();
        CommonFunc.SetPlayerHeadFrame(frame, sprite3, info.mainCardQuality);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.mainCardEntry);
        if (_config != null)
        {
            base.transform.FindChild("Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
        int num2 = (label3.height <= 0x40) ? 0 : (label3.height - 0x40);
        Transform transform = base.transform.FindChild("Team");
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - num2, transform.localPosition.y);
        label4.text = string.Empty;
        component.height = 0x100 + num;
        this.SetTargetTeamInfo2(info.cards);
    }

    [CompilerGenerated]
    private sealed class <OnClickBattleSelf>c__AnonStorey173
    {
        internal ConvoyEnemyInfo dataInfo;

        internal void <>m__19D(GUIEntity obj)
        {
            SelectHeroPanel panel = (SelectHeroPanel) obj;
            panel.Depth = 600;
            panel.mBattleType = BattleType.DetainsDartBattleBack;
            panel.SetDetainsDartBattleBackSelf(this.dataInfo);
        }
    }
}

