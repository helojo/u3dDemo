using Battle;
using FastBuf;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TargetTeamPanel : GUIEntity
{
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache5;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache6;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache7;
    [CompilerGenerated]
    private static Comparison<TargetCard> <>f__am$cache8;
    [CompilerGenerated]
    private static Comparison<CardInfo> <>f__am$cache9;
    [CompilerGenerated]
    private static Comparison<Card> <>f__am$cacheA;
    [CompilerGenerated]
    private static Comparison<OutlandMonsterTitle> <>f__am$cacheB;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cacheC;
    [CompilerGenerated]
    private static UIEventListener.VoidDelegate <>f__am$cacheD;
    private BattleType mCurType;
    private GuildBattleTargetInfo mGuildBattleTargetData;
    private S2C_GetFriendFormation mNetFriendFormationData;
    private S2C_WarmmatchTargetReq mS2C_WarmmatchTargetReq;
    private S2C_GetLeagueOpponentFormation mWorldCupPlayerFormation;

    public void BattleGridTirggerResult()
    {
        if ((this.mCurType >= BattleType.OutLandBattle_tollGate_0) && (this.mCurType <= BattleType.OutLandBattle_tollGate_3))
        {
            BattleState.GetInstance().CurGame.battleGameData.OnMsgBattleGridTiggerResult(false, false);
        }
    }

    private Card GetCardInfoBy(TargetCard _targetCard)
    {
        Card card = new Card();
        CardInfo info = new CardInfo {
            entry = _targetCard.card_entry,
            level = _targetCard.card_lv,
            quality = _targetCard.card_quality,
            starLv = _targetCard.card_starlv
        };
        card.cardInfo = info;
        return card;
    }

    private void OnClickGuildBattlePkBtn(GameObject go)
    {
        GUIMgr.Instance.ExitModelGUI(base.name);
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity entity) {
            SelectHeroPanel panel = (SelectHeroPanel) entity;
            panel.Depth = 600;
            panel.mBattleType = BattleType.GuildBattle;
            panel.SetZhuZhanStat(false);
            panel.SetGuildBattleInfo(this.mGuildBattleTargetData);
        }, null);
    }

    private void OnClickInteceptPkBtn(GameObject go)
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.Depth = 600;
                panel.mBattleType = BattleType.DerainsDartInterceptBattle;
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache5, null);
    }

    private void OnClickLXSPkBtn(GameObject go)
    {
        if (this.mS2C_WarmmatchTargetReq != null)
        {
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity obj) {
                SelectHeroPanel panel = (SelectHeroPanel) obj;
                panel.Depth = 600;
                panel.SetLSXPkTargetInfo(this.mS2C_WarmmatchTargetReq);
            }, null);
        }
    }

    private void OnClickOutlandPkBtn(GameObject go)
    {
        GUIMgr.Instance.ExitModelGUI(base.name);
        if (<>f__am$cacheC == null)
        {
            <>f__am$cacheC = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.Depth = 600;
                panel.mBattleType = (BattleType) (0x10 + ActorData.getInstance().outlandType);
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cacheC, null);
    }

    private void OnClickPkBtn(GameObject go)
    {
        if (this.mNetFriendFormationData != null)
        {
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity obj) {
                SelectHeroPanel panel = (SelectHeroPanel) obj;
                panel.Depth = 800;
                panel.mBattleType = BattleType.FriendPk;
                panel.SetPkTargetInfo(this.mNetFriendFormationData);
            }, null);
        }
    }

    private void OnClickRefreshBtn(GameObject go)
    {
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                if (ActorData.getInstance().FlamebattleCoin < 10)
                {
                    box.SetDialog(ConfigMgr.getInstance().GetWord(0x835), null, null, false);
                }
                else
                {
                    if (<>f__am$cacheD == null)
                    {
                        <>f__am$cacheD = mb => SocketMgr.Instance.RequestRefreshFlameBattleTarget();
                    }
                    box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x837), 10), <>f__am$cacheD, null, false);
                }
            };
        }
        GUIMgr.Instance.DoModelGUI("MessageBox", <>f__am$cache6, null);
    }

    private void OnClickWorldPkBtn(GameObject go)
    {
        if (this.mWorldCupPlayerFormation != null)
        {
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", delegate (GUIEntity obj) {
                SelectHeroPanel panel = (SelectHeroPanel) obj;
                panel.Depth = 600;
                panel.SetWorldCupPkTargetInfo(this.mWorldCupPlayerFormation);
            }, null);
        }
    }

    private void OnClickYuanZhengPkBtn(GameObject go)
    {
        if (<>f__am$cache7 == null)
        {
            <>f__am$cache7 = delegate (GUIEntity entity) {
                SelectHeroPanel panel = (SelectHeroPanel) entity;
                panel.Depth = 600;
                panel.mBattleType = BattleType.YuanZhengPk;
                panel.SetZhuZhanStat(false);
            };
        }
        GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache7, null);
    }

    public void SetArenaLadderEnemyInfo(ArenaLadderEnemy info)
    {
        base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.name;
        base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x281c), info.team_power);
        base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = info.level.ToString();
        base.transform.FindChild("Info/WinCount").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x281d), info.win_count);
        base.transform.FindChild("Info/Rank").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x281e), info.order);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.head_entry);
        if (_config != null)
        {
            base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.SetTeamCard2(info.cardList);
        }
    }

    private void SetBaseInfo(BriefUser info)
    {
        if (info != null)
        {
            base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.name;
            base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Empty;
            base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = info.level.ToString();
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.head_entry);
            if (_config != null)
            {
                base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                UISprite component = base.transform.FindChild("Info/Head/QualityBorder").GetComponent<UISprite>();
                UISprite sprite2 = base.transform.FindChild("Info/Head/QualityBorder/QIcon").GetComponent<UISprite>();
                CommonFunc.SetPlayerHeadFrame(component, sprite2, info.head_frame_entry);
            }
        }
    }

    public void SetBgClose(bool _isShow)
    {
        base.transform.FindChild("Close").gameObject.SetActive(_isShow);
    }

    private void SetCardInfo(Transform obj, CardInfo cardInfo)
    {
        if (cardInfo != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) cardInfo.entry);
            if (_config != null)
            {
                obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), cardInfo.quality);
                obj.FindChild("Level").GetComponent<UILabel>().text = cardInfo.level.ToString();
                for (int i = 0; i < 5; i++)
                {
                    UISprite component = obj.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    component.gameObject.SetActive(i < cardInfo.starLv);
                    component.transform.localPosition = new Vector3((float) (i * 0x10), 0f, 0f);
                }
                Transform transform = obj.transform.FindChild("Star");
                CommonFunc.SetCardJobIcon(obj.FindChild("Job/jobIcon").GetComponent<UISprite>(), _config.class_type);
                transform.localPosition = new Vector3(-4f - ((cardInfo.starLv - 1) * 8f), transform.localPosition.y, 0f);
                transform.gameObject.SetActive(true);
            }
        }
    }

    public void SetGuildBattleNodeInfo(GuildBattleTargetInfo info)
    {
        this.mGuildBattleTargetData = info;
        base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.targetName;
        base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x130), info.teamPower);
        base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = info.level.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.SetTeamCard(info.cardList);
            Transform transform = base.transform.FindChild("PkBtn");
            transform.gameObject.SetActive(true);
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickGuildBattlePkBtn);
        }
    }

    public void SetInterceptInfo(ConvoyRobTarget info)
    {
        base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.name;
        base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Empty;
        base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = info.lvl.ToString();
        base.transform.FindChild("Info/WinCount").GetComponent<UILabel>().text = string.Empty;
        base.transform.FindChild("Info/Rank").GetComponent<UILabel>().text = string.Empty;
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.headEntry);
        if (_config != null)
        {
            base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            UISprite component = base.transform.FindChild("Info/Head/QualityBorder").GetComponent<UISprite>();
            UISprite sprite2 = base.transform.FindChild("Info/Head/QualityBorder/QIcon").GetComponent<UISprite>();
            CommonFunc.SetPlayerHeadFrame(component, sprite2, info.head_frame_entry);
            this.SetTeamCard2(info.cardInfo);
            UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickInteceptPkBtn);
        }
    }

    public void SetLianXiSaiTargetInfo(S2C_WarmmatchTargetReq info)
    {
        this.mS2C_WarmmatchTargetReq = info;
        UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLXSPkBtn);
        this.SetTeamCard(info.target.cardList);
        this.SetBaseInfo(info.target.targetInfo);
        base.transform.FindChild("CloseBtn").gameObject.SetActive(false);
        base.transform.FindChild("PkBtn").gameObject.SetActive(false);
    }

    public void SetOutlandInfo(OutlandMonsterInfo info)
    {
        base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.monster_desc;
        base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x130), info.power);
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = (tc1, tc2) => tc2.card_lv - tc1.card_lv;
        }
        info.title.Sort(<>f__am$cacheB);
        OutlandMonsterTitle title = info.title[0];
        base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = title.card_lv.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(title.card_entry);
        if (_config != null)
        {
            base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            List<TargetCard> list = new List<TargetCard>();
            info.title.RemoveAt(0);
            foreach (OutlandMonsterTitle title2 in info.title)
            {
                TargetCard item = new TargetCard {
                    card_entry = title2.card_entry,
                    card_lv = (byte) title2.card_lv
                };
                list.Add(item);
            }
            this.SetTeamTargetCard(list);
            UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickOutlandPkBtn);
        }
    }

    public void SetPkBtnStat(bool _isShow)
    {
        base.transform.FindChild("PkBtn").gameObject.SetActive(_isShow);
    }

    public void SetRefreshBtnStat(bool isCanReset)
    {
        if (isCanReset)
        {
            Transform transform = base.transform.FindChild("RefreshBtn");
            UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickRefreshBtn);
            transform.gameObject.SetActive(isCanReset);
        }
    }

    public void SetTargetInfo(S2C_GetFriendFormation info)
    {
        this.mNetFriendFormationData = info;
        UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickPkBtn);
        this.SetTeamCard(info.cardList);
        this.SetBaseInfo(info.targetInfo);
        base.transform.FindChild("CloseBtn").gameObject.SetActive(true);
        base.transform.FindChild("PkBtn").gameObject.SetActive(true);
    }

    private void SetTeamCard(List<Card> _cardList)
    {
        if (<>f__am$cacheA == null)
        {
            <>f__am$cacheA = delegate (Card tc1, Card tc2) {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) tc1.cardInfo.entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) tc2.cardInfo.entry);
                int num = (_config == null) ? -1 : _config.card_position;
                int num2 = (_config2 == null) ? -1 : _config2.card_position;
                return num - num2;
            };
        }
        _cardList.Sort(<>f__am$cacheA);
        int num = 0;
        for (int i = 0; i < 5; i++)
        {
            Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
            if (num < _cardList.Count)
            {
                this.SetCardInfo(transform, _cardList[i].cardInfo);
                num++;
            }
            else
            {
                transform.transform.gameObject.SetActive(false);
            }
        }
    }

    private void SetTeamCard2(List<CardInfo> _cardList)
    {
        if (<>f__am$cache9 == null)
        {
            <>f__am$cache9 = delegate (CardInfo tc1, CardInfo tc2) {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) tc1.entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) tc2.entry);
                int num = (_config == null) ? -1 : _config.card_position;
                int num2 = (_config2 == null) ? -1 : _config2.card_position;
                return num - num2;
            };
        }
        _cardList.Sort(<>f__am$cache9);
        int num = 0;
        for (int i = 0; i < 5; i++)
        {
            Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
            if (num < _cardList.Count)
            {
                this.SetCardInfo(transform, _cardList[i]);
                num++;
            }
            else
            {
                transform.transform.gameObject.SetActive(false);
            }
        }
    }

    private void SetTeamTargetCard(List<TargetCard> _cardList)
    {
        if (<>f__am$cache8 == null)
        {
            <>f__am$cache8 = delegate (TargetCard tc1, TargetCard tc2) {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(tc1.card_entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(tc2.card_entry);
                int num = (_config == null) ? -1 : _config.card_position;
                int num2 = (_config2 == null) ? -1 : _config2.card_position;
                return num - num2;
            };
        }
        _cardList.Sort(<>f__am$cache8);
        int num = 0;
        for (int i = 0; i < 5; i++)
        {
            Transform transform = base.transform.FindChild("Team/Pos" + (i + 1));
            if (num < _cardList.Count)
            {
                TargetCard card = _cardList[num];
                if (card == null)
                {
                    return;
                }
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(card.card_entry);
                if (_config == null)
                {
                    return;
                }
                UITexture component = transform.FindChild("Icon").GetComponent<UITexture>();
                component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                CommonFunc.SetCardJobIcon(transform.FindChild("Job/jobIcon").GetComponent<UISprite>(), _config.class_type);
                transform.FindChild("Level").GetComponent<UILabel>().text = card.card_lv.ToString();
                if ((this.mCurType < BattleType.OutLandBattle_tollGate_0) || (this.mCurType > BattleType.OutLandBattle_tollGate_3))
                {
                    CommonFunc.SetQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), card.card_quality);
                    for (int j = 0; j < 5; j++)
                    {
                        UISprite sprite3 = transform.transform.FindChild("Star/" + (j + 1)).GetComponent<UISprite>();
                        sprite3.gameObject.SetActive(j < card.card_starlv);
                        sprite3.transform.localPosition = new Vector3((float) (j * 0x10), 0f, 0f);
                    }
                    Transform transform2 = transform.transform.FindChild("Star");
                    transform2.localPosition = new Vector3(-4f - ((card.card_starlv - 1) * 8f), transform2.localPosition.y, 0f);
                    transform2.gameObject.SetActive(true);
                    UISlider slider = transform.transform.FindChild("Hp").GetComponent<UISlider>();
                    UISlider slider2 = transform.transform.FindChild("Cd").GetComponent<UISlider>();
                    Debug.Log(card.card_max_hp + " ----  " + card.card_cur_hp);
                    slider.sliderValue = ((float) card.card_cur_hp) / ((float) card.card_max_hp);
                    if (slider.sliderValue == 0f)
                    {
                        slider2.sliderValue = 0f;
                        transform.transform.FindChild("Dead").gameObject.SetActive(true);
                        nguiTextureGrey.doChangeEnableGrey(component, true);
                    }
                    else
                    {
                        slider2.sliderValue = ((float) card.card_cur_energy) / ((float) AiDef.MAX_ENERGY);
                    }
                    slider.gameObject.SetActive(true);
                    slider2.gameObject.SetActive(true);
                }
                transform.transform.gameObject.SetActive(true);
                num++;
            }
            else
            {
                transform.transform.gameObject.SetActive(false);
            }
        }
    }

    public void SetWorldCupTargetInfo(S2C_GetLeagueOpponentFormation info)
    {
        this.mWorldCupPlayerFormation = info;
        UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickWorldPkBtn);
        this.SetTeamCard(info.cardList);
        base.transform.FindChild("CloseBtn").gameObject.SetActive(false);
        base.transform.FindChild("PkBtn").gameObject.SetActive(false);
        this.SetBaseInfo(info.targetInfo);
    }

    public void SetYuanZhengInfo(FlameBattleTargetData info)
    {
        base.transform.FindChild("Info/Name").GetComponent<UILabel>().text = info.nick_name;
        base.transform.FindChild("Info/FightPower").GetComponent<UILabel>().text = string.Empty;
        base.transform.FindChild("Info/Level").GetComponent<UILabel>().text = info.lv.ToString();
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(info.head_entry);
        if (_config != null)
        {
            base.transform.FindChild("Info/Head/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            UISprite component = base.transform.FindChild("Info/Head/QualityBorder").GetComponent<UISprite>();
            UISprite sprite2 = base.transform.FindChild("Info/Head/QualityBorder/QIcon").GetComponent<UISprite>();
            CommonFunc.SetPlayerHeadFrame(component, sprite2, info.head_frame);
            this.mCurType = BattleType.YuanZhengPk;
            this.SetTeamTargetCard(info.target_card_list);
            UIEventListener.Get(base.transform.FindChild("PkBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickYuanZhengPkBtn);
        }
    }

    public void SetYuanZhengNodeInfo(int node)
    {
        base.transform.FindChild("Info/Progress").GetComponent<UILabel>().text = "( " + ((node / 2) + 1) + "/10 )";
    }

    public BattleType mBattleType
    {
        get
        {
            return this.mCurType;
        }
        set
        {
            this.mCurType = value;
        }
    }
}

