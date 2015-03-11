using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldBossPanel : GUIEntity
{
    public GameObject _HistroyGroup;
    public GameObject _SingleBossFightItem;
    public GameObject _ThisGroup;
    [CompilerGenerated]
    private static Action<GUIEntity> <>f__am$cache19;
    private int AttStrengthTimes;
    private bool bInCD;
    private int BossOverTime;
    private string[] BufferOpenTag = new string[] { "Ui_Boss_Label_wkq", "Ui_Boss_Label_ykq" };
    private int CirStrengthTimes;
    private bool CloseTag;
    private int FakeID;
    private Color32[] FontColor = new Color32[] { new Color32(0xfe, 0x60, 0xfe, 0xff), new Color32(0, 0xe1, 0xfe, 0xff), new Color32(0xcc, 0xfe, 0, 0xff), new Color32(0xff, 0xff, 0xff, 0xff), new Color32(240, 0xde, 0x8e, 0xff) };
    private Color32[] FrameColor = new Color32[] { new Color32(0x3f, 0x30, 40, 0xff), new Color32(0x36, 0x27, 0x20, 0xff) };
    private bool isAutoAtk;
    private bool IsInitOverTime;
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    public PanelType mCurPanelType;
    private UILabel mLabelAtkCDTime;
    private world_boss_config mworldBossCfg;
    private FastBuf.WorldBossData mWorldBossData;
    public GameObject RankPart;
    [HideInInspector]
    public int RebornInterval;
    private float RefreshCurTime;
    private const float RefreshTime = 90f;
    private string strBoss = "WorldBossPart/Boss";
    private const int StrengthTimes = 10;
    public GameObject UserRankObj;

    private void ChangeBossHpColor(ulong _CurLife, ulong _MaxLife, UISprite _FrontSp)
    {
        float num = _MaxLife * 0.2f;
        if (_CurLife > num)
        {
            _FrontSp.spriteName = "Ui_Login_Icon_loading";
        }
        else
        {
            _FrontSp.spriteName = "Ui_Boss_Icon_blood";
            TweenAlpha.Begin(_FrontSp.gameObject, 1.5f, 0f).style = UITweener.Style.PingPong;
        }
    }

    private unsafe void ChangeRankerColor(GameObject obj, int index, string Name)
    {
        if (ActorData.getInstance().UserInfo.name == Name)
        {
            obj.transform.FindChild("Me").gameObject.SetActive(true);
        }
        if (index == 0)
        {
            obj.transform.FindChild("Ranker").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
            obj.transform.FindChild("Name").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
            obj.transform.FindChild("Damage").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[0]));
        }
        else if (index == 1)
        {
            obj.transform.FindChild("Ranker").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
            obj.transform.FindChild("Name").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
            obj.transform.FindChild("Damage").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[1]));
        }
        else if (index == 2)
        {
            obj.transform.FindChild("Ranker").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
            obj.transform.FindChild("Name").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
            obj.transform.FindChild("Damage").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[2]));
        }
        else if (ActorData.getInstance().UserInfo.name == Name)
        {
            obj.transform.FindChild("Ranker").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[4]));
            obj.transform.FindChild("Name").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[4]));
            obj.transform.FindChild("Damage").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[4]));
        }
        else
        {
            obj.transform.FindChild("Ranker").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
            obj.transform.FindChild("Name").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
            obj.transform.FindChild("Damage").GetComponent<UILabel>().color = *((Color*) &(this.FontColor[3]));
        }
    }

    private void ClickRankBtn()
    {
        this.RankPart.SetActive(true);
    }

    public void CloseWorldBoss()
    {
        GUIMgr.Instance.ExitModelGUI("MessageBox");
        GUIMgr.Instance.PopGUIEntity();
        GUIMgr.Instance.CloseGUIEntity(base.entity_id);
    }

    public void EnableRewardBtn(bool _enable)
    {
        base.gameObject.transform.FindChild("WorldBossPart/PickGift/PickBtn").gameObject.SetActive(_enable);
    }

    private void HideRankPart()
    {
        this.RankPart.SetActive(false);
    }

    private void InCDState(bool _inCd)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("WorldBossPart/InCD").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("WorldBossPart/NormalBtn").gameObject;
        this.bInCD = _inCd;
        if (_inCd)
        {
            gameObject.SetActive(true);
            obj3.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            obj3.SetActive(true);
        }
    }

    private void OnClickAtkUpBtn()
    {
        if (ActorData.getInstance().Stone < 10)
        {
            <OnClickAtkUpBtn>c__AnonStorey271 storey = new <OnClickAtkUpBtn>c__AnonStorey271 {
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__5AF), null);
        }
        else if (ActorData.getInstance().EncouragePopMsg)
        {
            SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_ATT);
        }
        else if (ActorData.getInstance().EncourageAtkTip)
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                box.SetCheckBtn();
                box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa037b1), 10), box => SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_ATT), null, false);
            }, null);
        }
        else
        {
            SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_ATT);
        }
    }

    private void OnClickAutoFightBtn()
    {
        Debug.Log("OnClickAutoFightBtn");
    }

    private void OnClickClearCDBtn()
    {
        <OnClickClearCDBtn>c__AnonStorey274 storey = new <OnClickClearCDBtn>c__AnonStorey274 {
            <>f__this = this,
            costStone = 10
        };
        if (ActorData.getInstance().Stone >= storey.costStone)
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__5B5), null);
        }
        else
        {
            <OnClickClearCDBtn>c__AnonStorey275 storey2 = new <OnClickClearCDBtn>c__AnonStorey275 {
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey2.<>m__5B6), null);
        }
    }

    private void OnClickCritUpBtn()
    {
        if (ActorData.getInstance().Stone < 10)
        {
            <OnClickCritUpBtn>c__AnonStorey272 storey = new <OnClickCritUpBtn>c__AnonStorey272 {
                title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
            };
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__5B1), null);
        }
        else if (ActorData.getInstance().EncouragePopMsg)
        {
            SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_CRI);
        }
        else if (ActorData.getInstance().EncourageCritTip)
        {
            GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                MessageBox box = (MessageBox) obj;
                box.SetCheckBtn();
                box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa037b2), 10), box => SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_CRI), null, false);
            }, null);
        }
        else
        {
            SocketMgr.Instance.RequestWorldBossEncourage(this.mworldBossCfg.entry, WorldBossEncourageType.E_WBET_CRI);
        }
    }

    private void OnClickPickBtn()
    {
        SocketMgr.Instance.RequestWorldBossGains(this.mWorldBossData.boss.entry);
    }

    private void OnClickRuleBtn()
    {
        GUIMgr.Instance.DoModelGUI("WorldBossRulePanel", null, null);
    }

    private void OnClickStartBtn()
    {
        if (this.mWorldBossData.user.att_times > 0)
        {
            if (<>f__am$cache19 == null)
            {
                <>f__am$cache19 = delegate (GUIEntity obj) {
                    SelectHeroPanel panel = obj.Achieve<SelectHeroPanel>();
                    panel.Depth = 600;
                    panel.mBattleType = BattleType.WorldBoss;
                };
            }
            GUIMgr.Instance.DoModelGUI("SelectHeroPanel", <>f__am$cache19, null);
        }
        else
        {
            <OnClickStartBtn>c__AnonStorey273 storey = new <OnClickStartBtn>c__AnonStorey273 {
                <>f__this = this
            };
            if (this.mWorldBossData.user.buy_times <= 0)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037c5));
            }
            else
            {
                vip_config _config = ConfigMgr.getInstance().getByEntry<vip_config>(ActorData.getInstance().VipType);
                int id = 0;
                if (_config != null)
                {
                    int num2 = 0;
                    id = num2 - this.mWorldBossData.user.buy_times;
                    if (id < 0)
                    {
                        id = 0;
                    }
                }
                else
                {
                    Debug.LogWarning("Vip Cfg Is Null!");
                    id = 0;
                }
                storey.cost = ConfigMgr.getInstance().getByEntry<buy_cost_config>(id).buy_worldboss_cost_stone;
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__5B4), null);
            }
        }
    }

    private void OnClickTabHistroy()
    {
        this._ThisGroup.gameObject.SetActive(false);
        this._HistroyGroup.gameObject.SetActive(true);
    }

    private void OnClickTabThis()
    {
        this._ThisGroup.gameObject.SetActive(true);
        this._HistroyGroup.gameObject.SetActive(false);
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
    }

    public override void OnInitialize()
    {
        this.mLabelAtkCDTime = base.gameObject.transform.FindChild("WorldBossPart/InCD/Time/Val").GetComponent<UILabel>();
        base.gameObject.transform.FindChild("WorldBossPart/Encourage/CritCost/Label").GetComponent<UILabel>().text = 10.ToString();
        base.gameObject.transform.FindChild("WorldBossPart/Encourage/AttCost/Label").GetComponent<UILabel>().text = 10.ToString();
    }

    public override void OnRelease()
    {
        if (this.FakeID > 0)
        {
            FakeCharacter.GetInstance().DestroyCharater(this.FakeID);
        }
        base.OnRelease();
    }

    public override void OnSerialization(GUIPersistence pers)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.m_time += Time.deltaTime;
        if (this.m_time > this.m_updateInterval)
        {
            this.m_time = 0f;
            if (this.mCurPanelType == PanelType.DYing)
            {
                this.RefreshPanel();
            }
            if (this.mLabelAtkCDTime != null)
            {
                if (this.bInCD && (this.mWorldBossData != null))
                {
                    string worldBossSelfDeadTime = TimeMgr.Instance.GetWorldBossSelfDeadTime(this.RebornInterval);
                    if (worldBossSelfDeadTime == string.Empty)
                    {
                        ActorData.getInstance().BossAtkRebornInterval = 0;
                        this.RebornInterval = 0;
                        this.InCDState(false);
                    }
                    else
                    {
                        this.mLabelAtkCDTime.text = worldBossSelfDeadTime;
                    }
                }
                else if (this.isAutoAtk)
                {
                    this.StartWorldBossCombat();
                    this.isAutoAtk = false;
                    return;
                }
                string descTime = TimeMgr.Instance.GetDescTime(this.BossOverTime);
                if (((this.mCurPanelType == PanelType.Boss) && (descTime == string.Empty)) && this.IsInitOverTime)
                {
                    this.isAutoAtk = false;
                    if (!this.CloseTag)
                    {
                        this.CloseTag = true;
                        GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
                        GUIMgr.Instance.DoModelGUI("MessageBox", delegate (GUIEntity obj) {
                            this.bInCD = false;
                            obj.Achieve<MessageBox>().SetDialog(ConfigMgr.getInstance().GetWord(0x9ba3d4), go => this.CloseWorldBoss(), null, true);
                        }, null);
                    }
                }
            }
        }
    }

    private void RefreshPanel()
    {
        this.RefreshCurTime++;
        if (this.RefreshCurTime >= 90f)
        {
            this.RefreshCurTime = 0f;
            SocketMgr.Instance.RequestWorldBoss(0);
        }
    }

    private void SetMemberInfo(Transform obj, List<WolrdBossHero> heros)
    {
        for (int i = 0; i < 5; i++)
        {
            Transform transform = obj.FindChild("Pos" + (i + 1));
            if (i < heros.Count)
            {
                if (heros[i].icon < 0)
                {
                    transform.gameObject.SetActive(false);
                }
                else
                {
                    UITexture component = transform.FindChild("Icon").GetComponent<UITexture>();
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(heros[i].icon);
                    if (_config != null)
                    {
                        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
                        CommonFunc.SetQualityColor(transform.FindChild("QualityBorder").GetComponent<UISprite>(), heros[i].quality);
                        transform.FindChild("Level").GetComponent<UILabel>().text = heros[i].level.ToString();
                        Transform transform2 = transform.FindChild("Star");
                        for (int j = 0; j < 5; j++)
                        {
                            int num3 = j + 1;
                            UISprite sprite2 = transform2.transform.FindChild(num3.ToString()).GetComponent<UISprite>();
                            sprite2.gameObject.SetActive(j < heros[i].star);
                            sprite2.transform.localPosition = new Vector3((float) (j * 0x1a), 0f, 0f);
                        }
                        transform2.gameObject.SetActive(true);
                        transform2.localPosition = new Vector3(transform2.localPosition.x - ((heros[i].star - 1) * 13), transform2.localPosition.y, 0f);
                        transform.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    private void SetPrefabInfo(Transform obj, WorldBossRecorder _data, int rank)
    {
        Transform transform = obj.FindChild("Info/CardList");
        obj.FindChild("Info/Level").GetComponent<UILabel>().text = "LV. " + _data.level.ToString();
        obj.FindChild("Info/Name").GetComponent<UILabel>().text = _data.name;
        UILabel component = obj.FindChild("Info/Guild").GetComponent<UILabel>();
        if (_data.guild_name.Length > 0)
        {
            component.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b8), _data.guild_name);
        }
        else
        {
            component.text = string.Empty;
        }
        UILabel label4 = obj.FindChild("Info/SingleHurt").GetComponent<UILabel>();
        label4.text = _data.dam.ToString();
        UILabel label5 = obj.FindChild("Info/SingleHurt2").GetComponent<UILabel>();
        label5.text = _data.dam.ToString();
        UILabel label6 = obj.FindChild("Info/Rank").GetComponent<UILabel>();
        label6.text = (rank + 1).ToString();
        UILabel label7 = obj.FindChild("Info/SingleHurtLb").GetComponent<UILabel>();
        UISprite sprite = obj.FindChild("Info/TopThreeRank").GetComponent<UISprite>();
        if (rank == -1)
        {
            label4.gameObject.SetActive(false);
            label5.gameObject.SetActive(false);
            label6.gameObject.SetActive(false);
            label7.gameObject.SetActive(false);
            sprite.gameObject.SetActive(false);
            this.SetMemberInfo(transform, _data.heros);
        }
        else
        {
            if (rank < 3)
            {
                this.SetMemberInfo(transform, _data.heros);
                sprite.spriteName = "Ui_Worldcup_Icon_" + (rank + 1);
            }
            sprite.gameObject.SetActive(rank < 3);
            transform.gameObject.SetActive(rank < 3);
            label4.gameObject.SetActive(rank < 3);
            label6.gameObject.SetActive(rank >= 3);
            label5.gameObject.SetActive(rank >= 3);
        }
    }

    private void StartWorldBossCombat()
    {
        List<long> list = new List<long>();
        foreach (Card card in ActorData.getInstance().mDefaultDupHeroList)
        {
            list.Add(card.card_id);
        }
        if (list.Count != 0)
        {
            if (this.mworldBossCfg != null)
            {
                GUIMgr.Instance.ExitModelGUI("MessageBox");
                SocketMgr.Instance.RequestWorldBossCombat(0, list);
            }
            else
            {
                Debug.LogWarning("mworldBossCfg is Null!");
            }
        }
    }

    private void UpdateAtkCount()
    {
        base.gameObject.transform.FindChild("WorldBossPart/NormalBtn/FightBtn/Count").GetComponent<UILabel>().text = string.Format(ConfigMgr.getInstance().GetWord(0x9ba3c7), this.mWorldBossData.user.att_times);
    }

    public void UpdateBossData(FastBuf.WorldBossData _wbData)
    {
        this.mWorldBossData = _wbData;
        if (_wbData.user.pick_reward_flag)
        {
            this.mCurPanelType = PanelType.Gains;
            this.UpdatePanel();
        }
        else
        {
            this.mworldBossCfg = ConfigMgr.getInstance().getByEntry<world_boss_config>(_wbData.boss.entry);
            if (_wbData.boss.state == WorldBossState.E_WBS_ROAR)
            {
                if (ActorData.getInstance().BossAtkRebornInterval > 0)
                {
                    this.RebornInterval = ActorData.getInstance().BossAtkRebornInterval;
                    string worldBossSelfDeadTime = TimeMgr.Instance.GetWorldBossSelfDeadTime(this.RebornInterval);
                }
                else if (_wbData.user.reborn_interval > 0)
                {
                    this.RebornInterval = ((int) _wbData.user.reborn_interval) + TimeMgr.Instance.ServerStampTime;
                }
                else
                {
                    this.RebornInterval = 0;
                }
                this.mCurPanelType = PanelType.Boss;
                this.UpdatePanel();
                if (this.RebornInterval == 0)
                {
                    this.InCDState(false);
                }
                else
                {
                    this.InCDState(true);
                }
                this.UpdateBossOverTime();
            }
        }
        this.UpdateOwnDamage(_wbData.user);
        this.UpdateBossInfo(_wbData.boss);
        this.UpdateRanker(_wbData.boss.ranker);
        this.UpdateKiller(_wbData.boss.drangon_knight);
        this.UpdateEncourageInfo(_wbData.user);
        this.UpdateAtkCount();
        this.UpdateWolrdBossHistory(_wbData.boss.history);
    }

    private void UpdateBossInfo(WorldBossBossData _data)
    {
        GameObject gameObject = base.gameObject.transform.FindChild(this.strBoss).gameObject;
        UILabel component = gameObject.transform.FindChild("Name").GetComponent<UILabel>();
        if (this.mworldBossCfg != null)
        {
            component.text = this.mworldBossCfg.name;
        }
        UITexture texture = gameObject.transform.FindChild("Character").GetComponent<UITexture>();
        world_boss_config _config = ConfigMgr.getInstance().getByEntry<world_boss_config>(_data.entry);
        monster_config _config2 = ConfigMgr.getInstance().getByEntry<monster_config>(_config.monster_card_entry);
        card_config _config3 = ConfigMgr.getInstance().getByEntry<card_config>(_config2.card_entry);
        UISprite sprite = gameObject.transform.FindChild("Hp/Foreground").GetComponent<UISprite>();
        UISprite sprite2 = gameObject.transform.FindChild("Hp/Background").GetComponent<UISprite>();
        UILabel label2 = gameObject.transform.FindChild("Hp/HP").GetComponent<UILabel>();
        UISlider slider = gameObject.transform.FindChild("Hp").GetComponent<UISlider>();
        float val = ((float) _data.curLife) / ((float) _data.maxLife);
        this.ChangeBossHpColor(_data.curLife, _data.maxLife, sprite);
        if (_data.maxLife == 0)
        {
            Debug.LogWarning("maxHp Is Zero!!!");
        }
        else
        {
            if ((_data.curLife / _data.maxLife) == 1L)
            {
                slider.sliderValue = 1f;
            }
            else
            {
                slider.sliderValue = val;
            }
            label2.text = CommonFunc.GetWorldBossHpPercent(_data.curLife, _data.maxLife, val);
        }
    }

    private void UpdateBossOverTime()
    {
        if (this.mworldBossCfg != null)
        {
            this.BossOverTime = this.mWorldBossData.boss.end_time;
            this.IsInitOverTime = true;
        }
    }

    public void UpdateDataBossDYing(FastBuf.WorldBossData _wbData)
    {
        this.UpdateBossData(_wbData);
        this.mCurPanelType = PanelType.DYing;
        this.UpdatePanel();
    }

    public void UpdateEncourageInfo(WorldBossUserData _data)
    {
        UILabel component = base.gameObject.transform.FindChild("WorldBossPart/Encourage/CritAdd/Val").GetComponent<UILabel>();
        UILabel label2 = base.gameObject.transform.FindChild("WorldBossPart/Encourage/AtkAdd/Val").GetComponent<UILabel>();
        UISprite sprite = base.gameObject.transform.FindChild("WorldBossPart/Encourage/CritBtn/IsOpen").GetComponent<UISprite>();
        UISprite sprite2 = base.gameObject.transform.FindChild("WorldBossPart/Encourage/AtkBtn/IsOpen").GetComponent<UISprite>();
        this.AttStrengthTimes = _data.att_strength_times;
        this.CirStrengthTimes = _data.cri_strength_times;
        if (this.CirStrengthTimes > 0)
        {
            sprite.spriteName = this.BufferOpenTag[1];
        }
        else
        {
            sprite.spriteName = this.BufferOpenTag[0];
        }
        if (this.AttStrengthTimes > 0)
        {
            sprite2.spriteName = this.BufferOpenTag[1];
        }
        else
        {
            sprite2.spriteName = this.BufferOpenTag[0];
        }
        int num = 0x3e8;
        float num2 = (_data.att_strength_times * num) / 100;
        label2.text = "+" + num2 + "%";
        num = 0x3e8;
        num2 = (_data.cri_strength_times * num) / 100;
        component.text = "+" + num2 + "%";
        this.UpdateStrengthBtn();
    }

    private void UpdateKiller(WorldBossRanker _rander)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("WorldBossPart/Rank/ThisGroup/Killer").gameObject;
        UILabel component = gameObject.transform.FindChild("Name").GetComponent<UILabel>();
        GameObject obj3 = base.gameObject.transform.FindChild("WorldBossPart/NormalBtn/FightBtn/Count").gameObject;
        if (_rander.name.Length > 0)
        {
            gameObject.SetActive(true);
            component.text = _rander.name;
            obj3.SetActive(false);
        }
        else
        {
            obj3.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void UpdateOwnDamage(WorldBossUserData _data)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("WorldBossPart/Rank/ThisGroup/My").gameObject;
        UILabel component = gameObject.transform.FindChild("Name").GetComponent<UILabel>();
        UILabel label2 = gameObject.transform.FindChild("Damage").GetComponent<UILabel>();
        UILabel label3 = gameObject.transform.FindChild("Ranker").GetComponent<UILabel>();
        if (_data.rank > 0)
        {
            label3.text = _data.rank.ToString();
        }
        else
        {
            label3.text = string.Empty;
        }
        component.text = ActorData.getInstance().UserInfo.name;
        label2.text = _data.damage_amount.ToString();
    }

    public void UpdatePanel()
    {
        GameObject gameObject = base.gameObject.transform.FindChild("WorldBossPart/NormalBtn").gameObject;
        GameObject obj3 = base.gameObject.transform.FindChild("WorldBossPart/PickGift").gameObject;
        GameObject obj4 = base.gameObject.transform.FindChild("WorldBossPart/Encourage/AtkBtn").gameObject;
        GameObject obj5 = base.gameObject.transform.FindChild("WorldBossPart/Encourage/CritBtn").gameObject;
        GameObject obj6 = base.gameObject.transform.FindChild("WorldBossPart/Tips").gameObject;
        switch (this.mCurPanelType)
        {
            case PanelType.Boss:
                gameObject.SetActive(true);
                obj3.SetActive(false);
                obj4.SetActive(true);
                obj5.SetActive(true);
                obj6.SetActive(false);
                break;

            case PanelType.Gains:
                gameObject.SetActive(false);
                obj3.SetActive(true);
                obj4.SetActive(false);
                obj5.SetActive(false);
                obj6.SetActive(false);
                break;

            case PanelType.DYing:
                gameObject.SetActive(false);
                obj3.SetActive(false);
                obj4.SetActive(false);
                obj5.SetActive(false);
                obj6.SetActive(true);
                base.gameObject.transform.FindChild("WorldBossPart/InCD").gameObject.SetActive(false);
                break;
        }
    }

    private void UpdateRanker(List<WorldBossRanker> _RankerList)
    {
        GameObject gameObject = base.gameObject.transform.FindChild("WorldBossPart/Rank/ThisGroup/Root").gameObject;
        int num = 0;
        CommonFunc.DeleteChildItem(gameObject.transform);
        foreach (WorldBossRanker ranker in _RankerList)
        {
            if (ranker.name != string.Empty)
            {
                GameObject obj3 = UnityEngine.Object.Instantiate(this.UserRankObj) as GameObject;
                obj3.transform.parent = gameObject.transform;
                obj3.transform.localScale = Vector3.one;
                obj3.transform.localPosition = new Vector3(0f, (float) (-num * 0x26), 0f);
                UISprite component = obj3.transform.FindChild("Sprite").GetComponent<UISprite>();
                if ((num % 2) == 0)
                {
                    component.gameObject.SetActive(true);
                }
                else
                {
                    component.gameObject.SetActive(false);
                }
                if (ActorData.getInstance().UserInfo.name == ranker.name)
                {
                    obj3.transform.FindChild("Me").gameObject.SetActive(true);
                }
                obj3.transform.FindChild("Name").GetComponent<UILabel>().text = ranker.name;
                obj3.transform.FindChild("Damage").GetComponent<UILabel>().text = ranker.damage_amount.ToString();
                num++;
                obj3.transform.FindChild("Ranker").GetComponent<UILabel>().text = num.ToString();
            }
        }
    }

    private void UpdateStrengthBtn()
    {
        UIButton component = base.gameObject.transform.FindChild("WorldBossPart/Encourage/AtkBtn").GetComponent<UIButton>();
        UIButton button2 = base.gameObject.transform.FindChild("WorldBossPart/Encourage/CritBtn").GetComponent<UIButton>();
        if (this.AttStrengthTimes >= 10)
        {
            component.isEnabled = false;
        }
        else
        {
            component.isEnabled = true;
        }
        if (this.CirStrengthTimes >= 10)
        {
            button2.isEnabled = false;
        }
        else
        {
            button2.isEnabled = true;
        }
    }

    private void UpdateWolrdBossHistory(WolrdBossHistory _history)
    {
        UILabel component = this._HistroyGroup.transform.FindChild("List/Group1").transform.FindChild("Label").GetComponent<UILabel>();
        component.text = ConfigMgr.getInstance().GetWord(0x4e2);
        if (_history.history_winner.that_day > 0)
        {
            DateTime time = TimeMgr.Instance.ConvertToDateTime((long) _history.history_winner.that_day);
            component.text = ConfigMgr.getInstance().GetWord(0x4e2) + ":" + string.Format(ConfigMgr.getInstance().GetWord(0x4e3), time.Year, time.Month, time.Day);
        }
        else
        {
            component.text = ConfigMgr.getInstance().GetWord(0x4e2);
        }
        Transform transform2 = this._HistroyGroup.transform.FindChild("List/Group2");
        Transform transform3 = transform2.FindChild("Recorder");
        transform3.gameObject.SetActive(_history.history_winner.user_id > 0L);
        transform2.FindChild("NullTips").gameObject.SetActive(_history.history_winner.user_id < 0L);
        if (_history.history_winner.user_id > 0L)
        {
            this.SetPrefabInfo(transform3, _history.history_winner, -1);
        }
        Transform transform5 = this._HistroyGroup.transform.FindChild("List/Group4");
        CommonFunc.DeleteChildItem(transform5);
        if (_history.record.Count == 0)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleBossFightItem) as GameObject;
            obj2.transform.parent = transform5.transform;
            obj2.transform.localPosition = new Vector3(0f, 0f, -0.1f);
            obj2.transform.localScale = new Vector3(1f, 1f, 1f);
            obj2.transform.FindChild("Info").gameObject.SetActive(false);
            obj2.transform.FindChild("NullTips").gameObject.SetActive(true);
        }
        else
        {
            float num = 142f;
            for (int i = 0; i < _history.record.Count; i++)
            {
                if (_history.record[i].user_id >= 0L)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate(this._SingleBossFightItem) as GameObject;
                    obj3.transform.parent = transform5.transform;
                    obj3.transform.localPosition = new Vector3(0f, -num * i, -0.1f);
                    obj3.transform.localScale = new Vector3(1f, 1f, 1f);
                    this.SetPrefabInfo(obj3.transform, _history.record[i], i);
                }
            }
        }
    }

    public FastBuf.WorldBossData WorldBossData
    {
        set
        {
            this.mWorldBossData = value;
            this.UpdateAtkCount();
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickAtkUpBtn>c__AnonStorey271
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__5AF(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__5BA(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickClearCDBtn>c__AnonStorey274
    {
        internal WorldBossPanel <>f__this;
        internal int costStone;

        internal void <>m__5B5(GUIEntity obj)
        {
            obj.Achieve<MessageBox>().SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa037b0), this.costStone), delegate (GameObject go) {
                if (this.<>f__this.RebornInterval > 0)
                {
                    SocketMgr.Instance.RequestWorldBossReborn(this.<>f__this.mWorldBossData.boss.entry);
                }
                else
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2abc));
                }
            }, null, false);
        }

        internal void <>m__5BD(GameObject go)
        {
            if (this.<>f__this.RebornInterval > 0)
            {
                SocketMgr.Instance.RequestWorldBossReborn(this.<>f__this.mWorldBossData.boss.entry);
            }
            else
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2abc));
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickClearCDBtn>c__AnonStorey275
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__5B6(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__5BE(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickCritUpBtn>c__AnonStorey272
    {
        private static UIEventListener.VoidDelegate <>f__am$cache1;
        internal string title;

        internal void <>m__5B1(GUIEntity e)
        {
            MessageBox box = e as MessageBox;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
            e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
        }

        private static void <>m__5BB(GameObject _go)
        {
            GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickStartBtn>c__AnonStorey273
    {
        private static UIEventListener.VoidDelegate <>f__am$cache2;
        internal WorldBossPanel <>f__this;
        internal int cost;

        internal void <>m__5B4(GUIEntity obj)
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = go => SocketMgr.Instance.RequestWorldBossBuyTimes(0);
            }
            obj.Achieve<MessageBox>().SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x2876), this.cost, this.<>f__this.mWorldBossData.user.buy_times), <>f__am$cache2, null, false);
        }

        private static void <>m__5BC(GameObject go)
        {
            SocketMgr.Instance.RequestWorldBossBuyTimes(0);
        }
    }

    public enum PanelType
    {
        Boss,
        Results,
        Gains,
        DYing
    }
}

