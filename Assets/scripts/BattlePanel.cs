using Battle;
using FastBuf;
using Holoville.HOTween;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattlePanel : GUIEntity
{
    public GameObject _AutoBtn;
    public GameObject _BossAutoBtn;
    private float _DelayTime = 0.15f;
    public UILabel _DupTips;
    public GameObject _GoBtn;
    private static BattlePanel _instance;
    public UILabel _ItemCount;
    public GameObject _NewHpChangeTips;
    public GameObject _PauseBtn;
    public GameObject _TimeGroup;
    public UILabel _TimeLabel;
    private GameObject BossHpObj;
    private GameObject BoxObj;
    public bool FightIsOver;
    private GameObject GoldObj;
    public const int IconSize = 0x7c;
    public BattleUIControlImplBase impl;
    public Transform[] mCardGirdList;
    private Dictionary<int, Tweener> mCdTweenerDict = new Dictionary<int, Tweener>();
    private Dictionary<int, GameObject> mFighterHPBarMap = new Dictionary<int, GameObject>();
    private List<BattleFighter> mFighters;
    private Dictionary<int, IconInfo> mHeroIdxDict = new Dictionary<int, IconInfo>();
    private Dictionary<int, Tweener> mHeroIdxTweener = new Dictionary<int, Tweener>();
    private Dictionary<int, Tweener> mHpTweenerDict = new Dictionary<int, Tweener>();
    private Dictionary<int, IconInfo> mIconDict = new Dictionary<int, IconInfo>();
    public Transform[] mInBossCardGirdList;
    private bool mInitIsOk;
    private bool mIsBoss;
    private bool misRunning;
    private List<PopNumberData> mPopNumberDataList = new List<PopNumberData>();
    private bool mRunHpChange = true;
    public GameObject SingleHPBar;
    public GameObject SingleHPChangeTip;

    public void AddItemCount()
    {
        if (this._ItemCount != null)
        {
            this._ItemCount.text = (int.Parse(this._ItemCount.text) + 1).ToString();
        }
    }

    public Vector3 GetBoxPos()
    {
        if (this.BoxObj == null)
        {
            return Vector3.zero;
        }
        Transform transform = this.BoxObj.transform.FindChild("Sprite");
        if (transform == null)
        {
            return Vector3.zero;
        }
        return transform.position;
    }

    public static BattlePanel GetInstance()
    {
        return _instance;
    }

    public void InitFighters(List<BattleFighter> fighters)
    {
        if (this.impl.battleGameData.gameType == BattleGameType.Grid)
        {
            this._PauseBtn.gameObject.SetActive(true);
        }
        else
        {
            this._PauseBtn.gameObject.SetActive(((((this.impl.battleGameData.normalGameType != BattleNormalGameType.WorldBoss) && (this.impl.battleGameData.normalGameType != BattleNormalGameType.WorldCupPk)) && ((this.impl.battleGameData.normalGameType != BattleNormalGameType.ArenaLadder) && (this.impl.battleGameData.normalGameType != BattleNormalGameType.GuildBattle))) && ((this.impl.battleGameData.normalGameType != BattleNormalGameType.DetainDartIntercept) && (this.impl.battleGameData.normalGameType != BattleNormalGameType.DetainDartBattleBack))) && (this.impl.battleGameData.normalGameType != BattleNormalGameType.GuildDup));
        }
        this.mFighters = fighters;
        this.mHeroIdxDict.Clear();
        if (this.mIsBoss)
        {
            this.ResetIconDict();
            int count = fighters.Count;
            for (int i = 0; i < this.mInBossCardGirdList.Length; i++)
            {
                if (i < count)
                {
                    this.InitGirdInfo(i, fighters[i]);
                }
                this.mInBossCardGirdList[i].gameObject.SetActive(i < count);
            }
        }
        else
        {
            this.SetIconPos(fighters.Count);
            int num3 = fighters.Count;
            for (int j = 0; j < this.mCardGirdList.Length; j++)
            {
                if (j < num3)
                {
                    this.InitGirdInfo(j, fighters[j]);
                    card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(fighters[j].CardEntry);
                    if (_config != null)
                    {
                        GUIDataHolder.setData(this.mCardGirdList[j].gameObject, _config.card_position);
                    }
                }
                this.mCardGirdList[j].gameObject.SetActive(j < num3);
            }
        }
        base.transform.FindChild("BottomCenter").gameObject.SetActive(!this.mIsBoss);
        base.transform.FindChild("LeftButton").gameObject.SetActive(this.mIsBoss);
        base.transform.FindChild("RightButton").gameObject.SetActive(this.mIsBoss);
        this.SetAutoBtnStat();
    }

    private void InitGirdInfo(int i, BattleFighter _fighterInfo)
    {
        this.mIconDict[i]._HpSprite.fillAmount = ((float) _fighterInfo.HP) / ((float) _fighterInfo.MaxHP);
        this.mIconDict[i]._CdSprite.fillAmount = ((float) _fighterInfo.Energy) / ((float) AiDef.MAX_ENERGY);
        this.mIconDict[i]._PosIndex = _fighterInfo.PosIndex;
        this.mHeroIdxDict.Add(_fighterInfo.PosIndex, this.mIconDict[i]);
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_fighterInfo.CardEntry);
        if (_config != null)
        {
            this.mIconDict[i]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            this.mIconDict[i]._IconName = _config.image;
            this.mIconDict[i]._AngryIconName = _config.image + "_nu";
            this.mIconDict[i]._NeedSetAngryStat = true;
            this.mIconDict[i]._QualityBorder.mainTexture = BundleMgr.Instance.CreateCardQuality("Ui_Hero_Frame_" + (_fighterInfo.detailActor.quality + 1));
        }
        this.mIconDict[i]._IconBtn.GetComponent<BoxCollider>().enabled = false;
        GUIDataHolder.setData(this.mIconDict[i]._IconBtn.gameObject, _fighterInfo.PosIndex);
        UIEventListener.Get(this.mIconDict[i]._IconBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickIconUseSkill);
        if ((ActorData.getInstance().mFriendAssistCardEntry != -1) && (ActorData.getInstance().mFriendAssistCardEntry == _fighterInfo.CardEntry))
        {
            if (ActorData.getInstance().mFriendAssistIsQQ)
            {
                this.mIconDict[i]._AssistIcon.spriteName = (GameDefine.getInstance().GetTencentType() != TencentType.QQ) ? "Ui_Login_Icon_weixin" : "Ui_Login_Icon_QQ";
            }
            else
            {
                this.mIconDict[i]._AssistIcon.spriteName = "Ui_Fuben_Icon_help1";
            }
            this.mIconDict[i]._AssistIcon.MakePixelPerfect();
            this.mIconDict[i]._AssistIcon.gameObject.SetActive(true);
        }
        else
        {
            this.mIconDict[i]._AssistIcon.gameObject.SetActive(false);
        }
        this.mInitIsOk = true;
    }

    private void InsertPopHpChangeTips(GameObject BattleFighterObj, SkillEffectResult info)
    {
        PopNumberData item = new PopNumberData {
            BattleFighterObj = BattleFighterObj,
            info = info
        };
        this.mPopNumberDataList.Add(item);
    }

    public void OnBattleRunningChange(bool isRunning)
    {
        this.misRunning = isRunning;
        if (isRunning)
        {
            if (this.impl.battleGameData.gameType == BattleGameType.Normal)
            {
                if (!this.impl.battleGameData.IsBossBattle)
                {
                    if (this.impl.battleGameData.IsDupBattle() || (this.impl.battleGameData.normalGameType == BattleNormalGameType.Dungeons))
                    {
                        this._DupTips.text = (this.impl.battleGameData.CurBattlePhase + 1) + "/3";
                        this._DupTips.gameObject.SetActive(true);
                    }
                }
                else
                {
                    this._DupTips.text = string.Empty;
                    this._DupTips.gameObject.SetActive(false);
                }
            }
            else
            {
                this._DupTips.text = string.Empty;
                this._DupTips.gameObject.SetActive(false);
            }
            this.ResetCountDownTime();
            this.SetIconAngerState(true);
            this._TimeGroup.gameObject.SetActive(true);
        }
        else
        {
            if (this.impl.battleGameData.IsDupBattle() || (this.impl.battleGameData.normalGameType == BattleNormalGameType.Dungeons))
            {
                this._DupTips.text = string.Empty;
                this._DupTips.gameObject.SetActive(false);
            }
            this.SetIconAngerState(false);
            this._TimeGroup.gameObject.SetActive(false);
        }
        if (this.impl.battleGameData.normalGameType == BattleNormalGameType.TowerPk)
        {
            this.SetTowerLayerInfo(isRunning);
        }
    }

    private void OnClickAutoBtn(GameObject go)
    {
        UIToggle component = go.GetComponent<UIToggle>();
        switch (this.impl.battleGameData.normalGameType)
        {
            case BattleNormalGameType.PK:
            case BattleNormalGameType.FriendPK:
            case BattleNormalGameType.ArenaLadder:
            case BattleNormalGameType.WorldCupPk:
            case BattleNormalGameType.GuildBattle:
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2f44));
                component.value = !component.value;
                return;
        }
        if (!this.impl.battleGameData.isAutoEnable && this.impl.battleGameData.IsDupBattle())
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2f45));
            component.value = !component.value;
        }
        else if (!this.impl.battleGameData.isAutoEnable)
        {
            component.value = !component.value;
        }
        else
        {
            if (component.value && this._GoBtn.activeSelf)
            {
                this.SetGoBtn(false);
                this.impl.QuestGotoNextPhase();
            }
            this.impl.battleGameData.isAuto = component.value;
            component.transform.FindChild("Background").gameObject.SetActive(!component.value);
            component.transform.FindChild("Checked").gameObject.SetActive(component.value);
        }
    }

    private void OnClickGoBtn(GameObject go)
    {
        this._GoBtn.SetActive(false);
        this.impl.QuestGotoNextPhase();
    }

    public void OnClickIconUseSkill(GameObject obj)
    {
        if (((this.impl != null) && (this.impl.battleGameData != null)) && this.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().isRuning)
        {
            object obj2 = GUIDataHolder.getData(obj);
            if ((this.impl != null) && (obj2 != null))
            {
                int fighterIndex = (int) obj2;
                if ((this.mHeroIdxDict[fighterIndex]._CdSprite.fillAmount >= 1f) && this.impl.QuestCastSkill(fighterIndex))
                {
                    if (GuideSimulate_Battle.sim_mode)
                    {
                        GuideSimulate_Battle.UseSkill(fighterIndex, obj);
                    }
                    else if (GuideSystem.MatchEvent(GuideEvent.Battle))
                    {
                        GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Battle.tag_battle_use_skill, obj);
                    }
                    base.StartCoroutine(this.PlayCastSkillEffect(fighterIndex));
                }
            }
        }
    }

    private void OnClickPauseBtn()
    {
        if (GuideSystem.MatchEvent(GuideEvent.Battle))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        if (!this.FightIsOver)
        {
            BattleSecurityManager.Instance.StartPause();
            GUIMgr.Instance.DoModelGUI("BattlePausePanel", null, null);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _instance = null;
    }

    public void OnFighterChange(List<BattleFighter> fighters)
    {
        bool isBigBoss = false;
        this.mIsBoss = false;
        foreach (BattleFighter fighter in fighters)
        {
            if (fighter.IsBigBoss)
            {
                isBigBoss = true;
                this.mIsBoss = true;
                if (this.mFighters != null)
                {
                    this.InitFighters(this.mFighters);
                }
                break;
            }
        }
        this.SetCtrlAsBattleType(isBigBoss);
        foreach (BattleFighter fighter2 in fighters)
        {
            GameObject obj2 = null;
            if (fighter2.IsBigBoss)
            {
                this.UpdateBossHp(fighter2);
            }
            else
            {
                if (!this.mFighterHPBarMap.TryGetValue(fighter2.PosIndex, out obj2))
                {
                    obj2 = UnityEngine.Object.Instantiate(this.SingleHPBar) as GameObject;
                    this.mFighterHPBarMap.Add(fighter2.PosIndex, obj2);
                    obj2.AddComponent<HPBar>().AttachToObj(fighter2.GetAnimObj());
                }
                obj2.GetComponent<HealthBar>().mountPoint = fighter2.GetHang().GetHangPointObj(HangPointType.Top);
                PlayMakerFSM component = obj2.GetComponent<PlayMakerFSM>();
                component.FsmVariables.FindFsmBool("isplayer").Value = fighter2.isPlayer;
                component.SendEvent("START_INIT");
                obj2.SetActive(false);
            }
        }
    }

    public void OnFighterInfoChange(int posIdx, BattleFighter fighter, SkillEffectResult info)
    {
        if ((info != null) && (fighter != null))
        {
            if ((info.effectType == SubSkillType.Heal) || (info.effectType == SubSkillType.Hurt))
            {
                this.SetHpProgressBar(posIdx, (float) (((double) fighter.HP) / ((double) fighter.MaxHP)), fighter);
            }
            else if ((info.effectType == SubSkillType.AddEnergy) || (info.effectType == SubSkillType.SubEnergy))
            {
                this.SetCdProgressBar(posIdx, ((float) info.value) / ((float) AiDef.MAX_ENERGY), fighter);
            }
            this.PopHPChangeTip(fighter.GetHang().GetHangPointObj(HangPointType.Top), info);
        }
    }

    private void OnInitGuiControl()
    {
        this.mIconDict.Clear();
        for (int i = 0; i < this.mCardGirdList.Length; i++)
        {
            IconInfo info = new IconInfo {
                _Icon = this.mCardGirdList[i].FindChild("Icon/Sprite").GetComponent<UITexture>(),
                _QualityBorder = this.mCardGirdList[i].FindChild("Icon/QualityBorder").GetComponent<UITexture>(),
                _hpSlider = this.mCardGirdList[i].FindChild("hp").GetComponent<UISlider>(),
                _HpSprite = this.mCardGirdList[i].FindChild("hp/Foreground").GetComponent<UISprite>(),
                _cdSlider = this.mCardGirdList[i].FindChild("cd").GetComponent<UISlider>(),
                _CdSprite = this.mCardGirdList[i].FindChild("cd/Foreground").GetComponent<UISprite>(),
                _nuqi = this.mCardGirdList[i].FindChild("nuqi"),
                _nuqi_chixu = this.mCardGirdList[i].FindChild("UI_nuqi_chixu"),
                _hp_low = this.mCardGirdList[i].FindChild("UI_hplow_binsi"),
                _nuqi_shunjian = this.mCardGirdList[i].FindChild("Ui_nuqi_shunjian"),
                _nuqi_touxiangstar = this.mCardGirdList[i].FindChild("ui_touxiang_star"),
                _nuqiBg = this.mCardGirdList[i].FindChild("SpriteAngerBorder"),
                _IconBtn = this.mCardGirdList[i].FindChild("Icon"),
                _AssistIcon = this.mCardGirdList[i].FindChild("AssistIcon").GetComponent<UISprite>(),
                _IsDead = false
            };
            this.mIconDict.Add(i, info);
        }
        UIEventListener.Get(this._GoBtn).onClick = new UIEventListener.VoidDelegate(this.OnClickGoBtn);
        UIEventListener.Get(this._AutoBtn).onClick = new UIEventListener.VoidDelegate(this.OnClickAutoBtn);
        UIEventListener.Get(this._BossAutoBtn.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickAutoBtn);
    }

    public override void OnInitialize()
    {
        _instance = this;
        this.mFighterHPBarMap.Clear();
        this.OnInitGuiControl();
    }

    public void OnMsgFighterDestory(int posIdx)
    {
        GameObject obj2 = null;
        if (this.mFighterHPBarMap.TryGetValue(posIdx, out obj2))
        {
            UnityEngine.Object.Destroy(obj2);
            this.mFighterHPBarMap.Remove(posIdx);
        }
    }

    public void OnMsgFighterUIVisible(int posIdx, bool visible)
    {
        GameObject obj2 = null;
        if (this.mFighterHPBarMap.TryGetValue(posIdx, out obj2))
        {
            if (visible)
            {
                obj2.GetComponent<HPBar>().Update();
            }
            obj2.SetActive(visible);
        }
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        if (GuideSystem.MatchEvent(GuideEvent.Battle))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.mInitIsOk && (this.impl != null))
        {
            if (this.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().mAIManager != null)
            {
                foreach (IconInfo info in this.mHeroIdxDict.Values)
                {
                    bool isActiveAngry = this.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().IsActorCanUseActiveSkill(info._PosIndex);
                    bool flag2 = this.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().IsActorEnergyFull(info._PosIndex);
                    bool flag3 = this.impl.battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().IsActorHpLow(info._PosIndex);
                    if ((!this.mHeroIdxDict[info._PosIndex]._nuqi_chixu.gameObject.activeSelf && isActiveAngry) && (info._IconBtn.GetComponent<BoxCollider>().enabled == isActiveAngry))
                    {
                        this.mHeroIdxDict[info._PosIndex]._nuqi_chixu.gameObject.SetActive(true);
                    }
                    else if (this.mHeroIdxDict[info._PosIndex]._nuqi_chixu.gameObject.activeSelf && !isActiveAngry)
                    {
                        this.mHeroIdxDict[info._PosIndex]._nuqi_chixu.gameObject.SetActive(false);
                    }
                    if (!info._IconBtn.GetComponent<BoxCollider>().enabled && isActiveAngry)
                    {
                        this.SetIconState(isActiveAngry, info._PosIndex);
                    }
                    else if ((!flag2 && info._IconBtn.GetComponent<BoxCollider>().enabled) && !isActiveAngry)
                    {
                        this.SetIconState(flag2, info._PosIndex);
                    }
                    if (flag3 && !this.mHeroIdxDict[info._PosIndex]._hp_low.gameObject.activeSelf)
                    {
                        this.mHeroIdxDict[info._PosIndex]._hp_low.gameObject.SetActive(true);
                    }
                    else if (!flag3 && this.mHeroIdxDict[info._PosIndex]._hp_low.gameObject.activeSelf)
                    {
                        this.mHeroIdxDict[info._PosIndex]._hp_low.gameObject.SetActive(false);
                    }
                    if ((this.impl.battleGameData.isAuto && info._IconBtn.active) && isActiveAngry)
                    {
                        this.OnClickIconUseSkill(info._IconBtn.gameObject);
                    }
                }
                int battleRemainTimeInt = this.impl.battleGameData.GetBattleRemainTimeInt();
                object[] args = new object[] { battleRemainTimeInt / 60, battleRemainTimeInt % 60, ((battleRemainTimeInt / 60) <= 9) ? "0" : string.Empty, (((battleRemainTimeInt / 60) != 0) || ((battleRemainTimeInt % 60) >= 10)) ? string.Empty : "[ff0000]", ((battleRemainTimeInt % 60) >= 10) ? string.Empty : "0" };
                this._TimeLabel.text = string.Format("{3}{2}{0}:{4}{1}", args);
            }
            else
            {
                foreach (IconInfo info2 in this.mHeroIdxDict.Values)
                {
                    if (this.mHeroIdxDict[info2._PosIndex]._nuqi_chixu.gameObject.activeSelf)
                    {
                        this.mHeroIdxDict[info2._PosIndex]._nuqi_chixu.gameObject.SetActive(false);
                    }
                    if (this.mHeroIdxDict[info2._PosIndex]._hp_low.gameObject.activeSelf)
                    {
                        this.mHeroIdxDict[info2._PosIndex]._hp_low.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    [DebuggerHidden]
    public IEnumerator PlayAngryIcon(int posIdx)
    {
        return new <PlayAngryIcon>c__Iterator68 { posIdx = posIdx, <$>posIdx = posIdx, <>f__this = this };
    }

    [DebuggerHidden]
    public IEnumerator PlayCastSkillEffect(int posIdx)
    {
        return new <PlayCastSkillEffect>c__Iterator69 { posIdx = posIdx, <$>posIdx = posIdx, <>f__this = this };
    }

    private void PopHPChangeTip(GameObject BattleFighterObj, SkillEffectResult info)
    {
        int type = -1;
        if (info.IsHpChange())
        {
            if (info.changeValue != 0)
            {
                int[] numArray = new int[] { 1, 3, 5, 0x12, 20, 0x15, 0x16 };
                type = numArray[(int) info.hitType];
            }
            else if (info.hitType == SkillHitType.Absorb)
            {
                type = 0x12;
            }
            else if (info.hitType == SkillHitType.Miss)
            {
                type = 3;
            }
            else if (info.hitType == SkillHitType.DefendBadState)
            {
                if (info.DefendBadHurt && (info.hurtType == SkillHurtType.Magic))
                {
                    type = 0x15;
                }
                else if (info.DefendBadHurt && (info.hurtType == SkillHurtType.Physics))
                {
                    type = 0x16;
                }
                else
                {
                    type = 20;
                }
            }
        }
        else if (info.IsEnergyChange())
        {
            if (info.changeValue != 0)
            {
                if (info.energyChangeType == HitEnergyType.KillEnemy)
                {
                    type = 14;
                }
                else if (((info.energyChangeType == HitEnergyType.BySkill) || (info.energyChangeType == HitEnergyType.RestoreOnBattleWin)) || (info.energyChangeType == HitEnergyType.ByAIFate))
                {
                    type = (info.changeValue <= 0) ? 7 : 6;
                }
            }
        }
        else if (info.hitType == SkillHitType.Miss)
        {
            type = 3;
        }
        else if (info.effectType == SubSkillType.State)
        {
            buff_config _config = ConfigMgr.getInstance().getByEntry<buff_config>(info.bufferEntry);
            if (_config != null)
            {
                int[] numArray2 = new int[] { 
                    -1, -1, 10, 0x13, 11, 10, 15, -1, -1, 8, 9, -1, 13, 0x10, -1, -1, 
                    0x11, -1, -1, -1, -1, -1, -1, -1, -1, -1
                 };
                type = numArray2[_config.type];
            }
        }
        if (type >= 0)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this._NewHpChangeTips) as GameObject;
            obj2.GetComponent<SingleHPChangeTips>().AttachToObj(BattleFighterObj, info, type);
        }
    }

    [DebuggerHidden]
    public IEnumerator PushHpChange()
    {
        return new <PushHpChange>c__Iterator67 { <>f__this = this };
    }

    public void ResetCountDownTime()
    {
        this._TimeGroup.gameObject.SetActive(true);
    }

    private void ResetIconDict()
    {
        this.mIconDict.Clear();
        for (int i = 0; i < this.mInBossCardGirdList.Length; i++)
        {
            IconInfo info = new IconInfo {
                _Icon = this.mInBossCardGirdList[i].FindChild("Icon/Sprite").GetComponent<UITexture>(),
                _QualityBorder = this.mInBossCardGirdList[i].FindChild("Icon/QualityBorder").GetComponent<UITexture>(),
                _hpSlider = this.mInBossCardGirdList[i].FindChild("hp").GetComponent<UISlider>(),
                _HpSprite = this.mInBossCardGirdList[i].FindChild("hp/Foreground").GetComponent<UISprite>(),
                _cdSlider = this.mInBossCardGirdList[i].FindChild("cd").GetComponent<UISlider>(),
                _CdSprite = this.mInBossCardGirdList[i].FindChild("cd/Foreground").GetComponent<UISprite>(),
                _nuqi = this.mInBossCardGirdList[i].FindChild("nuqi"),
                _nuqi_chixu = this.mInBossCardGirdList[i].FindChild("UI_nuqi_chixu"),
                _hp_low = this.mInBossCardGirdList[i].FindChild("UI_hplow_binsi"),
                _nuqi_shunjian = this.mInBossCardGirdList[i].FindChild("Ui_nuqi_shunjian"),
                _nuqi_touxiangstar = this.mInBossCardGirdList[i].FindChild("ui_touxiang_star"),
                _nuqiBg = this.mInBossCardGirdList[i].FindChild("SpriteAngerBorder"),
                _IconBtn = this.mInBossCardGirdList[i].FindChild("Icon"),
                _AssistIcon = this.mInBossCardGirdList[i].FindChild("AssistIcon").GetComponent<UISprite>()
            };
            this.mIconDict.Add(i, info);
        }
    }

    public void ResetItemCount()
    {
        if (this._ItemCount != null)
        {
            this._ItemCount.text = "0";
        }
    }

    public void SetAutoBtnsActive(bool active)
    {
        this._AutoBtn.SetActive(false);
        this._BossAutoBtn.SetActive(false);
    }

    private void SetAutoBtnStat()
    {
        BattleNormalGameType normalGameType = this.impl.battleGameData.normalGameType;
        Debug.Log(normalGameType);
        bool flag = false;
        switch (normalGameType)
        {
            case BattleNormalGameType.PK:
            case BattleNormalGameType.FriendPK:
            case BattleNormalGameType.ArenaLadder:
            case BattleNormalGameType.WorldCupPk:
            case BattleNormalGameType.GuildBattle:
                flag = true;
                break;

            default:
                flag = false;
                break;
        }
        if (this.impl.battleGameData.isAutoEnable)
        {
            this._AutoBtn.SetActive(!this.impl.battleGameData.IsBossBattle);
            this._BossAutoBtn.SetActive(this.impl.battleGameData.IsBossBattle);
            UIToggle component = this._AutoBtn.GetComponent<UIToggle>();
            component.value = this.impl.battleGameData.isAuto || flag;
            UIToggle toggle2 = this._BossAutoBtn.GetComponent<UIToggle>();
            toggle2.value = this.impl.battleGameData.isAuto || flag;
            if (!this.impl.battleGameData.IsBossBattle)
            {
                component.transform.FindChild("Background").gameObject.SetActive(!component.value && !flag);
                component.transform.FindChild("Checked").gameObject.SetActive(component.value || flag);
                component.transform.FindChild("Locked").gameObject.SetActive(flag);
            }
            else
            {
                toggle2.transform.FindChild("Background").gameObject.SetActive(!toggle2.value && !flag);
                toggle2.transform.FindChild("Checked").gameObject.SetActive(toggle2.value || flag);
                toggle2.transform.FindChild("Locked").gameObject.SetActive(flag);
            }
        }
        else if (flag)
        {
            this._AutoBtn.SetActive(true);
            this._BossAutoBtn.SetActive(false);
            UIToggle toggle3 = this._AutoBtn.GetComponent<UIToggle>();
            toggle3.value = true;
            toggle3.transform.FindChild("Background").gameObject.SetActive(false);
            toggle3.transform.FindChild("Checked").gameObject.SetActive(true);
            toggle3.transform.FindChild("Locked").gameObject.SetActive(true);
        }
        else
        {
            this._BossAutoBtn.SetActive(this.impl.battleGameData.IsBossBattle);
            this._AutoBtn.SetActive(!this.impl.battleGameData.IsBossBattle);
            if (this.impl.battleGameData.IsBossBattle)
            {
                UIToggle toggle4 = this._BossAutoBtn.GetComponent<UIToggle>();
                toggle4.value = false;
                toggle4.transform.FindChild("Background").gameObject.SetActive(true);
                toggle4.transform.FindChild("Checked").gameObject.SetActive(false);
                toggle4.transform.FindChild("Locked").gameObject.SetActive(flag);
            }
            else
            {
                UIToggle toggle5 = this._AutoBtn.GetComponent<UIToggle>();
                toggle5.value = false;
                toggle5.transform.FindChild("Background").gameObject.SetActive(true);
                toggle5.transform.FindChild("Checked").gameObject.SetActive(false);
                toggle5.transform.FindChild("Locked").gameObject.SetActive(flag);
            }
        }
        if (this.impl.battleGameData.IsStoryBattle)
        {
            this.SetAutoBtnsActive(false);
        }
        if (this.impl.battleGameData.IsStoryBattle)
        {
            base.transform.FindChild("TopLeft").gameObject.SetActive(false);
            base.transform.FindChild("TopRight").gameObject.SetActive(false);
        }
    }

    private void SetCdProgressBar(int posIdx, float percent, BattleFighter fighter)
    {
        if (this.mHeroIdxDict.ContainsKey(posIdx))
        {
            if (fighter.IsDead())
            {
                this.SetDeadState(posIdx);
            }
            else if (this.mHeroIdxDict.ContainsKey(posIdx))
            {
                if (this.mCdTweenerDict.ContainsKey(posIdx))
                {
                    this.mCdTweenerDict[posIdx].Kill();
                    this.mCdTweenerDict[posIdx] = Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._CdSprite, this._DelayTime, "fillAmount", percent);
                }
                else
                {
                    this.mCdTweenerDict.Add(posIdx, Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._CdSprite, this._DelayTime, "fillAmount", percent));
                }
                if (percent >= 1f)
                {
                    if (!this.mHeroIdxDict[posIdx]._NeedSetAngryStat)
                    {
                    }
                }
                else
                {
                    this.mHeroIdxDict[posIdx]._IsAutoStartSkill = false;
                    if (!this.mHeroIdxDict[posIdx]._NeedSetAngryStat)
                    {
                        this.mHeroIdxDict[posIdx]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.mIconDict[posIdx]._IconName);
                        this.mHeroIdxDict[posIdx]._Icon.width = 0x7c;
                        this.mHeroIdxDict[posIdx]._Icon.height = 0x7c;
                        this.mHeroIdxDict[posIdx]._NeedSetAngryStat = true;
                        this.mHeroIdxDict[posIdx]._nuqiBg.gameObject.SetActive(false);
                        this.mHeroIdxDict[posIdx]._nuqi.gameObject.SetActive(false);
                        this.mHeroIdxDict[posIdx]._nuqi_chixu.gameObject.SetActive(false);
                        this.mHeroIdxDict[posIdx]._hp_low.gameObject.SetActive(false);
                        TweenAlpha.Begin(this.mHeroIdxDict[posIdx]._Icon.gameObject, 0.1f, 1f).method = UITweener.Method.Linear;
                        TweenScale.Begin(this.mHeroIdxDict[posIdx]._Icon.gameObject, 0.1f, Vector3.one).method = UITweener.Method.Linear;
                        this.mHeroIdxDict[posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = false;
                        this.mHeroIdxDict[posIdx]._Icon.color = Color.white;
                    }
                }
            }
        }
    }

    private void SetCtrlAsBattleType(bool isBigBoss)
    {
        if (this.impl != null)
        {
            this.BoxObj = base.gameObject.transform.FindChild("TopRight/Box").gameObject;
            this.GoldObj = base.gameObject.transform.FindChild("TopRight/Gold").gameObject;
            this.BossHpObj = base.gameObject.transform.FindChild("TopRight/BossHP").gameObject;
            if (isBigBoss)
            {
                this.BoxObj.SetActive(this.impl.battleGameData.normalGameType != BattleNormalGameType.WorldBoss);
                this.GoldObj.SetActive(false);
                this.BossHpObj.SetActive(true);
            }
            else
            {
                this.BossHpObj.SetActive(false);
                if (this.impl.battleGameData.normalGameType == BattleNormalGameType.ArenaLadder)
                {
                    this.BoxObj.SetActive(false);
                }
                else
                {
                    this.BoxObj.SetActive(true);
                }
            }
        }
    }

    private void SetDeadState(int posIdx)
    {
        if (this.mHeroIdxDict.ContainsKey(posIdx))
        {
            this.mHeroIdxDict[posIdx]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.mHeroIdxDict[posIdx]._IconName);
            this.mHeroIdxDict[posIdx]._Icon.width = 0x7c;
            this.mHeroIdxDict[posIdx]._Icon.height = 0x7c;
            Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._HpSprite, 0f, "fillAmount", 0);
            Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._CdSprite, 0f, "fillAmount", 0);
            nguiTextureGrey.doChangeEnableGrey(this.mHeroIdxDict[posIdx]._Icon, true);
            this.mHeroIdxDict[posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = false;
            this.mHeroIdxDict[posIdx]._nuqiBg.gameObject.SetActive(false);
            this.mHeroIdxDict[posIdx]._nuqi.gameObject.SetActive(false);
            this.mHeroIdxDict[posIdx]._nuqi_chixu.gameObject.SetActive(false);
            this.mHeroIdxDict[posIdx]._hp_low.gameObject.SetActive(false);
            nguiTextureGrey.doChangeEnableGrey(this.mHeroIdxDict[posIdx]._QualityBorder, true);
            this.mHeroIdxDict[posIdx]._NeedSetAngryStat = true;
            if (this.mHeroIdxTweener.ContainsKey(posIdx))
            {
                this.mHeroIdxTweener[posIdx].Kill();
            }
            this.mHeroIdxDict[posIdx]._IsDead = true;
        }
    }

    public void SetGoBtn(bool isShow)
    {
        this._GoBtn.SetActive(isShow);
        if (isShow)
        {
            this._TimeGroup.gameObject.SetActive(false);
        }
    }

    private void SetHpProgressBar(int posIdx, float percent, BattleFighter fighter)
    {
        if (fighter.IsBigBoss)
        {
            this.UpdateBossHp(fighter);
        }
        if (fighter.IsDead())
        {
            this.SetDeadState(posIdx);
        }
        else
        {
            if ((fighter != null) && fighter.isPlayer)
            {
                if ((percent > 0f) && (percent < 0.1))
                {
                    percent = 0.1f;
                }
                if (!this.mHeroIdxDict.ContainsKey(posIdx))
                {
                    return;
                }
                if (this.mHpTweenerDict.ContainsKey(posIdx))
                {
                    this.mHpTweenerDict[posIdx].Kill();
                    this.mHpTweenerDict[posIdx] = Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._HpSprite, this._DelayTime, "fillAmount", percent);
                }
                else
                {
                    this.mHpTweenerDict.Add(posIdx, Holoville.HOTween.HOTween.To(this.mHeroIdxDict[posIdx]._HpSprite, this._DelayTime, "fillAmount", percent));
                }
                if (percent <= 0f)
                {
                    this.SetDeadState(posIdx);
                }
                else if (this.mHeroIdxDict[posIdx]._IsDead && (percent > 0f))
                {
                    nguiTextureGrey.doChangeEnableGrey(this.mHeroIdxDict[posIdx]._Icon, false);
                    nguiTextureGrey.doChangeEnableGrey(this.mHeroIdxDict[posIdx]._QualityBorder, false);
                    this.mHeroIdxDict[posIdx]._IsDead = false;
                }
            }
            GameObject obj2 = null;
            if (this.mFighterHPBarMap.TryGetValue(posIdx, out obj2))
            {
                PlayMakerFSM component = obj2.GetComponent<PlayMakerFSM>();
                component.FsmVariables.FindFsmFloat("hpValue").Value = percent;
                component.SendEvent("SET_HP");
            }
        }
    }

    public void SetIconAngerState(bool isActive)
    {
        foreach (KeyValuePair<int, Tweener> pair in this.mHeroIdxTweener)
        {
            if (!isActive)
            {
                pair.Value.Pause();
            }
            else
            {
                pair.Value.Play();
            }
        }
    }

    private void SetIconPos(int cardCount)
    {
        int num = 140;
        for (int i = 0; i < cardCount; i++)
        {
            this.mCardGirdList[(cardCount - i) - 1].transform.localPosition = new Vector3((float) (num * i), this.mCardGirdList[(cardCount - i) - 1].transform.localPosition.y, 0f);
        }
        Transform transform = base.transform.FindChild("BottomCenter/Group");
        transform.transform.localPosition = new Vector3((float) ((((-num * cardCount) / 2) + (num / 2)) - 30), transform.transform.localPosition.y, transform.transform.localPosition.z);
    }

    private void SetIconPos2(List<Transform> girdList)
    {
        int num = 130;
        for (int i = 0; i < girdList.Count; i++)
        {
            girdList[(girdList.Count - i) - 1].transform.localPosition = new Vector3((float) (num * i), girdList[(girdList.Count - i) - 1].transform.localPosition.y, 0f);
        }
        Transform transform = base.transform.FindChild("BottomCenter/Group");
        transform.transform.localPosition = new Vector3(((transform.transform.localPosition.x - ((num * girdList.Count) / 2)) + (num / 2)) - 30f, transform.transform.localPosition.y, transform.transform.localPosition.z);
    }

    private void SetIconState(bool isActiveAngry, int posIdx)
    {
        <SetIconState>c__AnonStorey1A5 storeya = new <SetIconState>c__AnonStorey1A5 {
            posIdx = posIdx,
            <>f__this = this
        };
        if (isActiveAngry)
        {
            if (this.mHeroIdxDict[storeya.posIdx]._NeedSetAngryStat)
            {
                this.mHeroIdxDict[storeya.posIdx]._NeedSetAngryStat = false;
                if (GuideSimulate_Battle.sim_mode)
                {
                    GuideSimulate_Battle.PrepareFullFurry(storeya.posIdx, new System.Action(storeya.<>m__223));
                }
                else
                {
                    GuideSystem.FireEvent(GuideEvent.Battle);
                    base.StartCoroutine(this.PlayAngryIcon(storeya.posIdx));
                }
            }
        }
        else
        {
            this.mHeroIdxDict[storeya.posIdx]._IsAutoStartSkill = false;
            this.mHeroIdxDict[storeya.posIdx]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.mIconDict[storeya.posIdx]._IconName);
            this.mHeroIdxDict[storeya.posIdx]._Icon.width = 0x7c;
            this.mHeroIdxDict[storeya.posIdx]._Icon.height = 0x7c;
            this.mHeroIdxDict[storeya.posIdx]._NeedSetAngryStat = true;
            this.mHeroIdxDict[storeya.posIdx]._nuqiBg.gameObject.SetActive(false);
            this.mHeroIdxDict[storeya.posIdx]._nuqi.gameObject.SetActive(false);
            TweenAlpha.Begin(this.mHeroIdxDict[storeya.posIdx]._Icon.gameObject, 0.1f, 1f).method = UITweener.Method.Linear;
            TweenScale.Begin(this.mHeroIdxDict[storeya.posIdx]._Icon.gameObject, 0.1f, Vector3.one).method = UITweener.Method.Linear;
            this.mHeroIdxDict[storeya.posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = false;
            this.mHeroIdxDict[storeya.posIdx]._Icon.color = Color.white;
        }
    }

    public void SetPosGroupStat(bool isShow)
    {
        if (this.mIsBoss)
        {
            base.transform.FindChild("LeftButton").gameObject.SetActive(isShow);
            base.transform.FindChild("RightButton").gameObject.SetActive(isShow);
        }
        else
        {
            base.transform.FindChild("BottomCenter").gameObject.SetActive(isShow);
        }
        this._PauseBtn.gameObject.SetActive(isShow);
    }

    public void SetTowerLayerInfo(bool isShow)
    {
        IExtensible packObj = this.impl.battleGameData.packObj;
        if (packObj != null)
        {
            S2C_VoidTowerCombat combat = packObj as S2C_VoidTowerCombat;
            if (combat != null)
            {
                Transform transform = base.transform.FindChild("TopCenter/Tower");
                transform.transform.FindChild("Label").GetComponent<UILabel>().text = (((combat.dupData.trench_entry * 3) + this.impl.battleGameData.CurBattlePhase) + 1) + "/100";
                transform.gameObject.SetActive(isShow);
            }
        }
    }

    private int SortByPosition(BattleFighter card1, BattleFighter card2)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(card1.CardEntry);
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>(card2.CardEntry);
        return (_config.card_position - _config2.card_position);
    }

    private int SortByPosition2(Transform gird1, Transform gird2)
    {
        object obj2 = GUIDataHolder.getData(gird1.gameObject);
        object obj3 = GUIDataHolder.getData(gird2.gameObject);
        int num = (obj2 == null) ? 0 : ((int) obj2);
        int num2 = (obj3 == null) ? 0 : ((int) obj3);
        return (num - num2);
    }

    private void UpdateBossHp(BattleFighter _fight)
    {
        UISlider component = this.BossHpObj.transform.FindChild("Progress Bar").GetComponent<UISlider>();
        UILabel label = this.BossHpObj.transform.FindChild("Progress Bar/Label").GetComponent<UILabel>();
        float val = ((float) _fight.HP) / ((float) _fight.MaxHP);
        component.value = val;
        long hP = (long) _fight.HP;
        long maxHP = (long) _fight.MaxHP;
        label.text = CommonFunc.GetWorldBossHpPercent((ulong) hP, (ulong) maxHP, val);
    }

    public void UpdateFighterHPInfo(int posIdx, BattleFighter fighter)
    {
        if (fighter != null)
        {
            this.SetHpProgressBar(posIdx, (float) (((double) fighter.HP) / ((double) fighter.MaxHP)), fighter);
            this.SetCdProgressBar(posIdx, ((float) fighter.Energy) / ((float) AiDef.MAX_ENERGY), fighter);
        }
    }

    public int RemainTime
    {
        get
        {
            if (this.impl != null)
            {
                return Mathf.CeilToInt(this.impl.battleGameData.GetBattleRemainTime());
            }
            return 0;
        }
    }

    [CompilerGenerated]
    private sealed class <PlayAngryIcon>c__Iterator68 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>posIdx;
        internal BattlePanel <>f__this;
        internal BattleFighter <fighter>__1;
        internal BattleCom_FighterManager <fighterManager>__0;
        internal int posIdx;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = true;
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqiBg.gameObject.SetActive(true);
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_chixu.gameObject.SetActive(true);
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_shunjian.gameObject.SetActive(true);
                    this.<fighterManager>__0 = this.<>f__this.impl.battleGameData.battleComObject.GetComponent<BattleCom_FighterManager>();
                    this.<fighter>__1 = this.<fighterManager>__0.GetFighter(this.posIdx);
                    if ((this.<fighter>__1 != null) && !this.<fighter>__1.IsDead())
                    {
                        this.<fighter>__1.OnSubShowEffect(BattleGlobal.EnergyEffectPrefab);
                    }
                    if (GuideSimulate_Battle.sim_mode)
                    {
                        GuideSimulate_Battle.GenerateFullFurry(this.posIdx, this.<>f__this.mHeroIdxDict[this.posIdx]._IconBtn.gameObject);
                    }
                    else if (GuideSystem.MatchEvent(GuideEvent.Battle))
                    {
                        GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_battle_use_skill, this.<>f__this.mHeroIdxDict[this.posIdx]._IconBtn.gameObject);
                    }
                    TweenAlpha.Begin(this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.gameObject, 0.2f, 0f);
                    TweenScale.Begin(this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.gameObject, 0.2f, new Vector3(1.5f, 1.5f, 1.5f)).method = UITweener.Method.EaseInOut;
                    this.$current = new WaitForSeconds(0.2f);
                    this.$PC = 1;
                    goto Label_061A;

                case 1:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.<>f__this.mHeroIdxDict[this.posIdx]._AngryIconName);
                    this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.width = 0x80;
                    this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.height = 0x95;
                    TweenAlpha.Begin(this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.gameObject, 0.1f, 1f).method = UITweener.Method.Linear;
                    TweenScale.Begin(this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.gameObject, 0.1f, Vector3.one).method = UITweener.Method.Linear;
                    this.$current = new WaitForSeconds(0.3f);
                    this.$PC = 2;
                    goto Label_061A;

                case 2:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._IsAutoStartSkill = true;
                    this.<>f__this.mHeroIdxDict[this.posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = true;
                    if (this.<>f__this.mHeroIdxDict[this.posIdx]._HpSprite.fillAmount == 0f)
                    {
                        this.<>f__this.mHeroIdxDict[this.posIdx]._CdSprite.fillAmount = 0f;
                        this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.mainTexture = BundleMgr.Instance.CreateHeadIcon(this.<>f__this.mHeroIdxDict[this.posIdx]._IconName);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.width = 0x7c;
                        this.<>f__this.mHeroIdxDict[this.posIdx]._Icon.height = 0x7c;
                        nguiTextureGrey.doChangeEnableGrey(this.<>f__this.mHeroIdxDict[this.posIdx]._Icon, true);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._IconBtn.GetComponent<BoxCollider>().enabled = false;
                        this.<>f__this.mHeroIdxDict[this.posIdx]._nuqiBg.gameObject.SetActive(false);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi.gameObject.SetActive(false);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_chixu.gameObject.SetActive(false);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._hp_low.gameObject.SetActive(false);
                        this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_shunjian.gameObject.SetActive(false);
                        nguiTextureGrey.doChangeEnableGrey(this.<>f__this.mHeroIdxDict[this.posIdx]._QualityBorder, true);
                        if (this.<>f__this.mHeroIdxTweener.ContainsKey(this.posIdx))
                        {
                            this.<>f__this.mHeroIdxTweener[this.posIdx].Kill();
                        }
                    }
                    this.$current = new WaitForSeconds(0.5f);
                    this.$PC = 3;
                    goto Label_061A;

                case 3:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_shunjian.gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 4;
                    goto Label_061A;

                case 4:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_061A:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayCastSkillEffect>c__Iterator69 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal int <$>posIdx;
        internal BattlePanel <>f__this;
        internal int posIdx;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_touxiangstar.gameObject.SetActive(true);
                    this.$current = new WaitForSeconds(1f);
                    this.$PC = 1;
                    goto Label_00A9;

                case 1:
                    this.<>f__this.mHeroIdxDict[this.posIdx]._nuqi_touxiangstar.gameObject.SetActive(false);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_00A9;

                case 2:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_00A9:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PushHpChange>c__Iterator67 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal BattlePanel <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    break;

                case 2:
                    this.<>f__this.mPopNumberDataList.RemoveAt(0);
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_012E;

                default:
                    goto Label_012E;
            }
            while (this.<>f__this.mRunHpChange)
            {
                if (this.<>f__this.mPopNumberDataList.Count == 0)
                {
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 1;
                    goto Label_0130;
                }
                if ((this.<>f__this.mPopNumberDataList[0].info != null) && (this.<>f__this.mPopNumberDataList[0].BattleFighterObj != null))
                {
                    this.<>f__this.PopHPChangeTip(this.<>f__this.mPopNumberDataList[0].BattleFighterObj, this.<>f__this.mPopNumberDataList[0].info);
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 2;
                    goto Label_0130;
                }
            }
            this.$current = null;
            this.$PC = 3;
            goto Label_0130;
        Label_012E:
            return false;
        Label_0130:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SetIconState>c__AnonStorey1A5
    {
        internal BattlePanel <>f__this;
        internal int posIdx;

        internal void <>m__223()
        {
            this.<>f__this.StartCoroutine(this.<>f__this.PlayAngryIcon(this.posIdx));
        }
    }

    public class IconInfo
    {
        public string _AngryIconName;
        public UISprite _AssistIcon;
        public UISlider _cdSlider;
        public UISprite _CdSprite;
        public Transform _hp_low;
        public UISlider _hpSlider;
        public UISprite _HpSprite;
        public UITexture _Icon;
        public Transform _IconBtn;
        public string _IconName;
        public bool _IsAutoStartSkill;
        public bool _IsDead;
        public bool _NeedSetAngryStat;
        public Transform _nuqi;
        public Transform _nuqi_chixu;
        public Transform _nuqi_shunjian;
        public Transform _nuqi_touxiangstar;
        public Transform _nuqiBg;
        public int _PosIndex;
        public UITexture _QualityBorder;
    }

    public class PopNumberData
    {
        public GameObject BattleFighterObj;
        public SkillEffectResult info;
    }
}

