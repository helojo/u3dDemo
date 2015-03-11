using Battle;
using FastBuf;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class HeroInfoPanel : GUIEntity
{
    public GameObject _BreakFlashSprite;
    public GameObject _FromPanel;
    public GameObject _JiYouBtn;
    public GameObject _ShuXinPanel;
    public GameObject _SingleFromPrefab;
    public GameObject _SingleSkillItem;
    public GameObject[] _SkillItemList;
    public GameObject _SkillPanel;
    private Transform _SkillTabPoint;
    public GameObject _SkillTips;
    private GameObject _tipsPanel;
    public GameObject _UpStarFlashSprite;
    [CompilerGenerated]
    private static Predicate<Item> <>f__am$cache33;
    private GameObject CurClickSkillObj;
    private Queue<GameObject> EffectList = new Queue<GameObject>();
    private int FakeListIndex;
    private UILabel LabeldescAB;
    private UILabel LabelLv;
    private List<GameObject> LevUpBtnList = new List<GameObject>();
    private float m_time = 1f;
    private float m_updateInterval = 1f;
    private GameObject mBuyBtn;
    private bool mCanBreak;
    public Card mCard;
    private long mCardId = -1L;
    private GameObject mCurrPressItem;
    private List<Card> mCurrShowCardList;
    private bool mDargModel;
    private List<Transform> mEquipGirdList = new List<Transform>();
    private bool mHaveSkillCanLevUp;
    private bool mInitFromOk;
    private bool mInitSkillOk;
    private bool mIsCanClickLevUp = true;
    private bool mIsCardPartShow;
    private bool mIsOnleShow;
    private Dictionary<HeroAttributeType, object> mNewHeroAttributeDict = new Dictionary<HeroAttributeType, object>();
    private Dictionary<HeroAttributeType, object> mOldHeroAttributeDict = new Dictionary<HeroAttributeType, object>();
    private bool mPartEnough;
    private Vector2 mPosition;
    private Vector2 mScaleFactor = new Vector2(20f, 0f);
    private int mShowTabPageIdx;
    private FastBuf.SkillPoint mSkillPoint;
    private UILabel mSkillPointLabel;
    public bool mUpStarOk = true;
    private float nCurAddAgility;
    private int NextGetSkillTime;
    private bool OpenMsgBox;
    public GameObject SkillLevUp;
    private UISprite spriteTipsBackGround;
    private float TipsInterval = 0.25f;
    private Queue<string> TipsMsgList = new Queue<string>();
    private float TipsTime;

    private float AddOnePara(string paraStr, int skillLv)
    {
        if (!paraStr.Contains("|"))
        {
            return (float.Parse(paraStr) + skillLv);
        }
        char[] separator = new char[] { '|' };
        string[] strArray = paraStr.Split(separator);
        if (strArray.Length >= 1)
        {
            return (float.Parse(strArray[0]) + skillLv);
        }
        return (float) (0 + skillLv);
    }

    private string CalculateCurSkillLvEffValue_Base(int descType, string paraStr, int skillLv, int curSkillId, int descTpeSortCnt)
    {
        string str = string.Empty;
        if ((paraStr == null) || (paraStr == string.Empty))
        {
            return (str = string.Empty);
        }
        skill_descs_config _config = ConfigMgr.getInstance().getByEntry<skill_descs_config>(descType);
        if (_config == null)
        {
            return (str = string.Empty);
        }
        float num = 0f;
        switch (_config.suanfa)
        {
            case 0:
                num = this.AddOnePara(paraStr, skillLv);
                break;

            case 1:
                num = this.MutiOneParaDefault(paraStr, skillLv);
                break;

            case 2:
                num = this.CalculateValueOfTwoParasInts(this.getParasInts(paraStr, curSkillId, descTpeSortCnt), skillLv);
                break;

            default:
                num = this.AddOnePara(paraStr, skillLv);
                break;
        }
        return string.Format(_config.desc, num);
    }

    private float CalculateValueOfTwoParasInts(float[] paras, int skillLv)
    {
        if (paras.Length == 2)
        {
            return ((paras[0] * skillLv) + paras[1]);
        }
        return -1f;
    }

    public bool CanbeBreak()
    {
        if (this.mCard == null)
        {
            return false;
        }
        return CommonFunc.CheckCardCanBreak(this.mCard);
    }

    public bool CanbeStrengthen()
    {
        if (this.mCard == null)
        {
            return false;
        }
        return (null != Utility.GetEquipCanbeStrengthen(this.mCard));
    }

    public void CardBreakSuccess(Card _cardInfo)
    {
        <CardBreakSuccess>c__AnonStorey197 storey = new <CardBreakSuccess>c__AnonStorey197 {
            _cardInfo = _cardInfo,
            <>f__this = this
        };
        this.GetHeroAttributeData(this.mCard, true);
        this.GetHeroAttributeData(storey._cardInfo, false);
        this.mInitSkillOk = false;
        this.InitCardInfo(storey._cardInfo);
        this.UpdateShowCardByEntry(storey._cardInfo);
        base.StartCoroutine(this.PushBreakText(this.GetUpStarAddInfo(storey._cardInfo)));
        GUIMgr.Instance.DoModelGUI("CardBreakSuccessPanel", new Action<GUIEntity>(storey.<>m__1FE), null);
    }

    private bool CheckEquipOk(Card _card)
    {
        foreach (EquipInfo info in _card.equipInfo)
        {
            if (info.quality != (_card.cardInfo.quality + 1))
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckExistItem(string[] part, int itemEntry)
    {
        foreach (string str in part)
        {
            if (itemEntry == int.Parse(str))
            {
                return true;
            }
        }
        return false;
    }

    private void CheckSkillState()
    {
        this.mHaveSkillCanLevUp = false;
        if (this.mCard != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.mCard.cardInfo.entry);
            if (_config != null)
            {
                foreach (SkillInfo info in this.mCard.cardInfo.skillInfo)
                {
                    if (this.GetSkillCfg(this.mCard, info) != null)
                    {
                        int skillPos = info.skillPos;
                        int skillIDByCardEntry = CommonFunc.GetSkillIDByCardEntry(_config.entry, (E_CardSkill) skillPos);
                        if (CommonFunc.CanLvUpSkill(this.mCard.cardInfo.quality, skillPos) && (ActorData.getInstance().GetSkillLev(this.mCard.card_id, skillPos) < CommonFunc.GetMaxSkillLv(skillIDByCardEntry, this.mCard.cardInfo.level)))
                        {
                            this.mHaveSkillCanLevUp = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void ClearAllEffect()
    {
        while (this.EffectList.Count > 0)
        {
            GameObject obj2 = this.EffectList.Dequeue();
            if (obj2 != null)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    public void ClearSkillEffect()
    {
        if (this.EffectList.Count > 0)
        {
            GameObject obj2 = this.EffectList.Dequeue();
            if (obj2 != null)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    private int CreateModel(Card _card, UITexture _Texture)
    {
        Debug.Log(_card.card_id);
        return FakeCharacter.GetInstance().SnapCardCharacterWithEquip((int) _card.cardInfo.entry, _card.cardInfo.quality, _card.equipInfo, ModelViewType.normal, _Texture);
    }

    public void EnableLevUpBtn(bool _enable)
    {
        this.UpdateSkillBtnState();
    }

    private bool EquipCanbeLevelUp(EquipInfo info)
    {
        item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
        if (_config == null)
        {
            return false;
        }
        if (_config.quality > (this.mCard.cardInfo.quality + 1))
        {
            return false;
        }
        break_equip_config _config2 = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
        if (_config2 == null)
        {
            return false;
        }
        if (_config2.break_equip_entry < 0)
        {
            return false;
        }
        return (CommonFunc.CheckMaterialEnough(_config2.equip_entry) && (_config.quality < (this.mCard.cardInfo.quality + 1)));
    }

    private List<string> GetAllAddStr(List<EquipInfo> _newList, List<EquipInfo> _OldList)
    {
        List<string> list = new List<string>();
        string str = "[00ff00]";
        string str2 = " + ";
        int num = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        int num10 = 0;
        int num11 = 0;
        foreach (EquipInfo info in _newList)
        {
            EquipInfo info2 = _OldList[num];
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            int num12 = info.lv - info2.lv;
            if (_config.attack_grow > 0)
            {
                num2 += _config.attack_grow * num12;
            }
            if (_config.hp_grow > 0)
            {
                num3 += _config.hp_grow * num12;
            }
            if (_config.physics_defence_grow > 0)
            {
                num4 += _config.physics_defence_grow * num12;
            }
            if (_config.spell_defence_grow > 0)
            {
                num5 += _config.spell_defence_grow * num12;
            }
            if (_config.crit_level_grow > 0)
            {
                num6 += _config.crit_level_grow * num12;
            }
            if (_config.dodge_level_grow > 0)
            {
                num7 += _config.dodge_level_grow * num12;
            }
            if (_config.tenacity_level_grow > 0)
            {
                num8 += _config.tenacity_level_grow * num12;
            }
            if (_config.hit_level_grow > 0)
            {
                num9 += _config.hit_level_grow * num12;
            }
            if (_config.hp_recover_grow > 0)
            {
                num10 += _config.hp_recover_grow * num12;
            }
            if (_config.energy_recover_grow > 0)
            {
                num11 += _config.energy_recover_grow * num12;
            }
            num++;
        }
        if (num2 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x155), str2, num2 }));
        }
        if (num3 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(340), str2, num3 }));
        }
        if (num4 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15a), str2, num4 }));
        }
        if (num5 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15b), str2, num5 }));
        }
        if (num6 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(350), str2, num6 }));
        }
        if (num7 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x162), str2, num7 }));
        }
        if (num8 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x160), str2, num8 }));
        }
        if (num9 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x161), str2, num9 }));
        }
        if (num10 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x163), str2, num10 }));
        }
        if (num11 > 0)
        {
            list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x164), str2, num11 }));
        }
        return list;
    }

    private string GetAttributeDesc(Card _card, bool isOld, bool isNeedReadData)
    {
        string str = string.Empty;
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config == null)
            {
                return str;
            }
            Dictionary<HeroAttributeType, object> dictionary = !isOld ? this.mNewHeroAttributeDict : this.mOldHeroAttributeDict;
            int[] attrs = new int[12];
            CommonFunc.GetCardGemCombineAttr(_card, out attrs);
            int starLv = _card.cardInfo.starLv;
            int level = _card.cardInfo.level;
            int quality = _card.cardInfo.quality;
            int entry = _config.entry;
            card_ex_config cardExCfg = CommonFunc.GetCardExCfg(entry, starLv);
            if (cardExCfg == null)
            {
                return str;
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
                else if (_config.passivity_skill_idx == 3)
                {
                    passivitySkillEntry = _config.skill_3;
                }
            }
            float num6 = 0f;
            float num7 = 0f;
            float num8 = 0f;
            float num9 = 0f;
            float num11 = 0f;
            int skillLevel = 1;
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                skillLevel = _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillLevel;
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Stamina, out num7, out num11);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Agility, out num6, out num11);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Intelligence, out num8, out num11);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Strength, out num9, out num11);
            }
            float num13 = (_config.agility + ((level - 1) * cardExCfg.agility_grow)) + CommonFunc.GetBreakCardAgility(entry, quality);
            float num14 = (_config.stamina + ((level - 1) * cardExCfg.stamina_grow)) + CommonFunc.GetBreakCardStamina(entry, quality);
            float num15 = (_config.intelligence + ((level - 1) * cardExCfg.intelligence_grow)) + CommonFunc.GetBreakCardIntelligence(entry, quality);
            float num16 = (_config.strength + ((level - 1) * cardExCfg.strength_grow)) + CommonFunc.GetBreakCardStrength(entry, quality);
            string str2 = "[503d2e]";
            string str3 = " [930b00]+";
            string str4 = "[503d2e]";
            string str5 = "[483d22]";
            string str6 = string.Empty;
            float num21 = 0f;
            string str13 = str;
            object[] objArray1 = new object[] { str13, str4, ConfigMgr.getInstance().GetWord(0x171), ": ", cardExCfg.strength_grow, "\n" };
            str13 = string.Concat(objArray1);
            object[] objArray2 = new object[] { str13, str4, ConfigMgr.getInstance().GetWord(370), ": ", cardExCfg.agility_grow, "\n" };
            str13 = string.Concat(objArray2);
            object[] objArray3 = new object[] { str13, str4, ConfigMgr.getInstance().GetWord(0x173), ": ", cardExCfg.intelligence_grow, "\n" };
            str13 = string.Concat(objArray3);
            object[] objArray4 = new object[] { str13, str4, ConfigMgr.getInstance().GetWord(0x174), ": ", cardExCfg.stamina_grow, "\n", str2 };
            str = string.Concat(objArray4);
            int cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Strength);
            num9 += cardGemAddSumValue;
            num9 += attrs[0];
            string str8 = (num9 <= 0f) ? string.Empty : (str3 + string.Format("{0:F}", num9));
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Agility);
            num6 += cardGemAddSumValue;
            num6 += attrs[2];
            string str9 = (num6 <= 0f) ? string.Empty : (str3 + string.Format("{0:F}", num6));
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Intelligence);
            num8 += cardGemAddSumValue;
            num8 += attrs[1];
            string str10 = (num8 <= 0f) ? string.Empty : (str3 + string.Format("{0:F}", num8));
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Stamina);
            num7 += cardGemAddSumValue;
            num7 += attrs[3];
            string str11 = (num7 <= 0f) ? string.Empty : (str3 + string.Format("{0:F}", num7));
            if ((num16 > 0f) || (num9 > 0f))
            {
                str13 = str;
                string[] textArray1 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x156), ": ", str5, string.Format("{0:F}", num16), str8, "\n", str2 };
                str = string.Concat(textArray1);
            }
            if ((num13 > 0f) || (num6 > 0f))
            {
                str13 = str;
                string[] textArray2 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x157), ": ", str5, string.Format("{0:N2}", num13), str9, "\n", str2 };
                str = string.Concat(textArray2);
                this.nCurAddAgility = num6;
                num21 = num13;
            }
            if ((num15 > 0f) || (num8 > 0f))
            {
                str13 = str;
                string[] textArray3 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x158), ": ", str5, string.Format("{0:F}", num15), str10, "\n", str2 };
                str = string.Concat(textArray3);
            }
            if ((num14 > 0f) || (num7 > 0f))
            {
                str13 = str;
                string[] textArray4 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x159), ": ", str5, string.Format("{0:F}", num14), str11, "\n", str2 };
                str = string.Concat(textArray4);
            }
            num16 += num9;
            num13 += num6;
            num15 += num8;
            num14 += num7;
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Strength, num16);
                dictionary.Add(HeroAttributeType.Agility, num13);
                dictionary.Add(HeroAttributeType.Intelligence, num15);
                dictionary.Add(HeroAttributeType.Stamina, num14);
            }
            float num22 = 0f;
            float num23 = 0f;
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.MaxHp, out num22, out num23);
            }
            int num17 = _config.hp + ((int) (num14 * CommonFunc.GetStaminaToHp(entry, starLv)));
            int cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Hp);
            cardEquipAddSumValue = (cardEquipAddSumValue + ((int) ((cardEquipAddSumValue + num17) * num23))) + ((int) num22);
            Debug.Log("HP--------------" + cardEquipAddSumValue);
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                str13 = str;
                object[] objArray5 = new object[] { str13, ConfigMgr.getInstance().GetWord(340), ": ", str5, num17, str6, "\n", str2 };
                str = string.Concat(objArray5);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Hp, num17 + cardEquipAddSumValue);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.Attack, out num22, out num23);
            }
            num17 = (int) (((_config.attack + (num16 * cardExCfg.strength_to_attack)) + (num15 * cardExCfg.intelligence_to_attack)) + (num13 * cardExCfg.agility_to_attack));
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Attack) + ((int) num22);
            cardEquipAddSumValue += (int) ((cardEquipAddSumValue + num17) * num23);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Attack);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[4];
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                str13 = str;
                object[] objArray6 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x155), ": ", str5, num17, str6, "\n", str2 };
                str = string.Concat(objArray6);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Attack, num17 + cardEquipAddSumValue);
            }
            float num24 = (num13 * cardExCfg.agility_to_crit) + CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.CritRate);
            float num19 = (((float) _config.crit_rate) / 10000f) + (num24 / ((num24 + (level * 70)) + 75f));
            if (num19 > 0f)
            {
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.CritRate, num19);
            }
            cardEquipAddSumValue = this.GetCritLevel();
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            num17 = (int) (num21 * cardExCfg.agility_to_crit);
            if (cardEquipAddSumValue > 0)
            {
                str13 = str;
                object[] objArray7 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x144), ": ", str5, num17, str6, str2, "(", string.Format("{0:N1}", num19 * 100f), "%)\n", str2 };
                str = string.Concat(objArray7);
            }
            cardEquipAddSumValue = _config.tenacity + CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Tenacity);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Tenacity);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[9];
            num19 = ((float) cardEquipAddSumValue) / ((float) ((cardEquipAddSumValue + (level * 90)) + 70));
            if (num19 > 0f)
            {
                str13 = str;
                string[] textArray5 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x160), ": ", str5, string.Format("{0:N1}", num19 * 100f), "%\n", str2 };
                str = string.Concat(textArray5);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Tenacity, num19);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.PhysicsDefence, out num22, out num23);
            }
            Debug.Log("----------SkillBuffAdd" + num22);
            num17 = _config.physics_defence + ((int) (num16 * cardExCfg.strength_to_phy));
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Phydefence);
            cardEquipAddSumValue = (cardEquipAddSumValue + ((int) ((cardEquipAddSumValue + num17) * num23))) + ((int) num22);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Phydefence);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[6];
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                str13 = str;
                object[] objArray8 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x15a), ": ", str5, num17, str6, "\n", str2 };
                str = string.Concat(objArray8);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Phydefence, num17 + cardEquipAddSumValue);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.SpellDefence, out num22, out num23);
            }
            num17 = _config.spell_defence + ((int) (num15 * cardExCfg.intelligence_to_magicdef));
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.Spellfence);
            cardEquipAddSumValue = (cardEquipAddSumValue + ((int) ((cardEquipAddSumValue + num17) * num23))) + ((int) num22);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.Spellfence);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[5];
            Debug.Log(cardEquipAddSumValue + "------------");
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                str13 = str;
                object[] objArray9 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x15b), ": ", str5, num17, str6, "\n", str2 };
                str = string.Concat(objArray9);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.Spellfence, num17 + cardEquipAddSumValue);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.PhysicsPierce, out num22, out num23);
            }
            num17 = _config.physics_pierce;
            cardEquipAddSumValue = 0;
            cardEquipAddSumValue = (cardEquipAddSumValue + ((int) ((cardEquipAddSumValue + num17) * num23))) + ((int) num22);
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                str13 = str;
                object[] objArray10 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x15c), ": ", str5, num17, str6, "\n", str2 };
                str = string.Concat(objArray10);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.PhysicsPierce, _config.physics_pierce);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.SpellPierce, out num22, out num23);
            }
            if ((_config.spell_pierce > 0) || (num22 > 0f))
            {
                str6 = (num22 <= 0f) ? string.Empty : (str3 + ((int) num22));
                str13 = str;
                object[] objArray11 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x15d), ": ", str5, _config.spell_pierce, str6, "\n", str2 };
                str = string.Concat(objArray11);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.SpellPierce, _config.spell_pierce);
            }
            cardEquipAddSumValue = _config.dod_rate + CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.DodRate);
            num19 = (((float) _config.dod_rate) / 10000f) + (cardEquipAddSumValue / ((cardEquipAddSumValue + (level * 180)) + 80));
            if (num19 > 0f)
            {
                str13 = str;
                string[] textArray6 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x162), ": ", str5, string.Format("{0:N1}", num19 * 100f), "%\n", str2 };
                str = string.Concat(textArray6);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.DodRate, num19);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.HPBack, out num22, out num23);
            }
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.HpRecover) + ((int) num22);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.HpRecover);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[10];
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            num17 = _config.hp_recover;
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                if (num17 == 0)
                {
                    str13 = str;
                    string[] textArray7 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x163), ": ", str5, str6, "\n", str2 };
                    str = string.Concat(textArray7);
                }
                else
                {
                    str13 = str;
                    object[] objArray12 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x163), ": ", str5, num17, str6, "\n", str2 };
                    str = string.Concat(objArray12);
                }
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.HpRecover, num17 + cardEquipAddSumValue);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.EnergyBack, out num22, out num23);
            }
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.EnergyRecover) + ((int) num22);
            cardGemAddSumValue = CommonFunc.GetCardGemAddSumValue(_card.cardGemInfo, GemAddType.EnergyRecover);
            cardEquipAddSumValue += cardGemAddSumValue;
            cardEquipAddSumValue += attrs[11];
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
            num17 = _config.energy_recover;
            if ((num17 > 0) || (cardEquipAddSumValue > 0))
            {
                if (num17 == 0)
                {
                    str13 = str;
                    string[] textArray8 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x164), ": ", str5, str6, "\n", str2 };
                    str = string.Concat(textArray8);
                }
                else
                {
                    str13 = str;
                    object[] objArray13 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x164), ": ", str5, num17, str6, "\n", str2 };
                    str = string.Concat(objArray13);
                }
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.EnergyRecover, num17 + cardEquipAddSumValue);
            }
            num19 = ((float) _config.be_heal_mod) / 10000f;
            num17 = _config.be_heal_mod;
            cardEquipAddSumValue = this.GetBeHealMod();
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : string.Concat(new object[] { str3, cardEquipAddSumValue, str2, "(", string.Format("{0:N1}", cardEquipAddSumValue * 0.01), "%)" });
            str13 = str;
            object[] objArray15 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x4e61), ": ", str5, num17, str6, "\n", str2 };
            str = string.Concat(objArray15);
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.BeHealMod, num19);
            }
            num17 = _config.heal_mod;
            cardEquipAddSumValue = this.GetHealMod();
            str6 = (cardEquipAddSumValue <= 0) ? string.Empty : string.Concat(new object[] { str3, cardEquipAddSumValue, str2, "(", string.Format("{0:N1}", cardEquipAddSumValue * 0.01), "%)" });
            str13 = str;
            object[] objArray17 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x4e62), ": ", str5, num17, str6, "\n", str2 };
            str = string.Concat(objArray17);
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.MoveSpeed, out num22, out num23);
            }
            if ((_config.move_speed > 0f) || (num22 > 0f))
            {
                str6 = (num22 <= 0f) ? string.Empty : (str3 + num22);
                str13 = str;
                object[] objArray18 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x16a), ": ", str5, _config.move_speed, str6, "\n", str2 };
                str = string.Concat(objArray18);
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.MoveSpeed, _config.move_speed);
            }
            if ((_config.passivity_skill_idx != -1) && CommonFunc.SkillIsUnLock((E_CardSkill) _card.cardInfo.skillInfo[_config.passivity_skill_idx].skillPos, _card.cardInfo.quality))
            {
                this.GetPassivitySkillBuffAdd(skillLevel, passivitySkillEntry, SkillLogicEffectType.SuckLv, out num22, out num23);
            }
            cardEquipAddSumValue = CommonFunc.GetCardEquipAddSumValue(_card.equipInfo, EquipAddType.SuckLv) + ((int) num22);
            if ((_config.suck_lv > 0) || (cardEquipAddSumValue > 0))
            {
                str6 = (cardEquipAddSumValue <= 0) ? string.Empty : (str3 + cardEquipAddSumValue);
                string str12 = GameConstant.DefaultTextColor + "(" + string.Format("{0:N1}", (((float) cardEquipAddSumValue) / ((float) (cardEquipAddSumValue + (_card.cardInfo.level * 0x5d)))) * 100f) + "%)";
                if (_config.suck_lv == 0)
                {
                    str13 = str;
                    string[] textArray9 = new string[] { str13, ConfigMgr.getInstance().GetWord(0x16e), ": ", str5, str6, str12 };
                    str = string.Concat(textArray9);
                }
                else
                {
                    str13 = str;
                    object[] objArray19 = new object[] { str13, ConfigMgr.getInstance().GetWord(0x16e), ": ", str5, _config.suck_lv, str6, str12 };
                    str = string.Concat(objArray19);
                }
            }
            if (isNeedReadData)
            {
                dictionary.Add(HeroAttributeType.SuckLv, _config.suck_lv);
            }
        }
        return str;
    }

    private int GetAutoBreakCost(break_equip_config bec)
    {
        int num = 0;
        if (bec != null)
        {
            num += bec.cost_gold;
            if ((bec.need_item_1 > 0) && (bec.need_num_1 > 0))
            {
                num += CommonFunc.GetEquipCombCost(bec.need_item_1, bec.need_num_1);
            }
            if ((bec.need_item_2 > 0) && (bec.need_num_2 > 0))
            {
                num += CommonFunc.GetEquipCombCost(bec.need_item_2, bec.need_num_2);
            }
            if ((bec.need_item_3 > 0) && (bec.need_num_3 > 0))
            {
                num += CommonFunc.GetEquipCombCost(bec.need_item_3, bec.need_num_3);
            }
        }
        return num;
    }

    private int GetBeHealMod()
    {
        int num = 0;
        for (int i = 0; i < this.mCard.cardGemInfo.Count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mCard.cardGemInfo[i].entry);
            if ((_config != null) && (_config.be_heal_mod != -1))
            {
                num += _config.be_heal_mod;
            }
        }
        return num;
    }

    private string GetCardDesc(Card _card)
    {
        return this.GetAttributeDesc(_card, false, false);
    }

    private string GetCardFetterDesc(int _cardNameType)
    {
        string str = string.Empty;
        ArrayList list = ConfigMgr.getInstance().getList<fetter_config>();
        int num = 0;
        IEnumerator enumerator = list.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                fetter_config current = (fetter_config) enumerator.Current;
                if (current.card_name_type == _cardNameType)
                {
                    str = str + current.name;
                    num++;
                    if ((num % 2) == 0)
                    {
                        str = str + "\n";
                    }
                    else
                    {
                        str = str + "     ";
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
        return str;
    }

    public Card GetCardInfo(Card _currCard, bool isForward)
    {
        int num = 0;
        for (int i = 0; i < this.mCurrShowCardList.Count; i++)
        {
            if (this.mCurrShowCardList[i].card_id == _currCard.card_id)
            {
                num = i;
            }
        }
        int num3 = !isForward ? (num - 1) : (num + 1);
        if (num3 < 0)
        {
            num3 = this.mCurrShowCardList.Count - 1;
        }
        else if (num3 >= this.mCurrShowCardList.Count)
        {
            num3 = 0;
        }
        return this.mCurrShowCardList[num3];
    }

    private float GetCardSkillAddValue(Card _card)
    {
        return 0f;
    }

    private int GetCost(int _base, int _Lev, int _quality)
    {
        _base = CommonFunc.GetEquipLvUpBase(_Lev);
        return (_base + ((_Lev - 1) * CommonFunc.GetEquipLvUpGrowLv(_Lev)));
    }

    private int GetCritLevel()
    {
        int num = 0;
        for (int i = 0; i < this.mCard.equipInfo.Count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mCard.equipInfo[i].entry);
            if (_config != null)
            {
                num += _config.crit_level + (_config.crit_level_grow * (this.mCard.equipInfo[i].lv - 1));
            }
        }
        int[] attrs = new int[12];
        CommonFunc.GetCardGemCombineAttr(this.mCard, out attrs);
        num += attrs[7];
        card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) this.mCard.cardInfo.entry);
        if (_config2 == null)
        {
            return 0;
        }
        card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config2.entry, this.mCard.cardInfo.starLv);
        if (cardExCfg != null)
        {
            num += (int) (this.nCurAddAgility * cardExCfg.agility_to_crit);
        }
        return num;
    }

    private EquipInfo GetEquipInfo(int itemEntry, int part)
    {
        return new EquipInfo { part = part, entry = itemEntry, lv = 1, quality = 0 };
    }

    private int GetHealMod()
    {
        int num = 0;
        for (int i = 0; i < this.mCard.cardGemInfo.Count; i++)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.mCard.cardGemInfo[i].entry);
            if ((_config != null) && (_config.heal_mod != -1))
            {
                num += _config.heal_mod;
            }
        }
        return num;
    }

    private void GetHeroAttributeData(Card _card, bool isOld)
    {
        if (isOld)
        {
            this.mOldHeroAttributeDict.Clear();
        }
        else
        {
            this.mNewHeroAttributeDict.Clear();
        }
        string str = this.GetAttributeDesc(_card, isOld, true);
    }

    private int GetOneKeyCost()
    {
        int num = 0;
        foreach (EquipInfo info in this.mCard.equipInfo)
        {
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                int level = this.mCard.cardInfo.level;
                for (int i = info.lv; i < level; i++)
                {
                    num += this.GetCost(_config.lv_up_gold, i, info.quality);
                }
            }
        }
        return num;
    }

    private float[] getParasInts(string paraStr, int curSkillId, int descTpeSortCnt)
    {
        float[] numArray = new float[0];
        if (paraStr.Contains("|"))
        {
            char[] separator = new char[] { '|' };
            string[] strArray = paraStr.Split(separator);
            numArray = new float[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                numArray[i] = float.Parse(strArray[i]);
            }
        }
        return numArray;
    }

    private void GetPassivitySkillBuffAdd(int skillLevel, int passivitySkillEntry, SkillLogicEffectType _effectType, out float _addValue, out float _multipleValue)
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
                        this.GetSubSkillBuffAdd(skillLevel, _config2.buff_entry, _effectType, out num5, out num6);
                        num += num5;
                        num2 += num6;
                    }
                }
                _addValue = num;
                _multipleValue += num2;
            }
        }
    }

    private int GetQualityparam(int _quality)
    {
        switch (((E_QualiytDef) _quality))
        {
            case E_QualiytDef.e_qualitydef_white:
                return GameConstValues.EQUIP_LV_UP_GROW_WHITE;

            case E_QualiytDef.e_qualitydef_green:
                return GameConstValues.EQUIP_LV_UP_GROW_GREEN;

            case E_QualiytDef.e_qualitydef_blue:
                return GameConstValues.EQUIP_LV_UP_GROW_BLUE;

            case E_QualiytDef.e_qualitydef_blue_1:
                return GameConstValues.EQUIP_LV_UP_GROW_BLUE_1;

            case E_QualiytDef.e_qualitydef_purple:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE;

            case E_QualiytDef.e_qualitydef_purple_1:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_1;

            case E_QualiytDef.e_qualitydef_purple_2:
                return GameConstValues.EQUIP_LV_UP_GROW_PURPLE_2;

            case E_QualiytDef.e_qualitydef_orange:
                return GameConstValues.EQUIP_LV_UP_GROW_ORANGE;
        }
        return 0;
    }

    private skill_config GetSkillCfg(Card card, SkillInfo _Skill)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
        if (_config == null)
        {
            return null;
        }
        E_CardSkill skillPos = (E_CardSkill) _Skill.skillPos;
        int id = 0;
        switch (skillPos)
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

    public SkillInfo GetSkillInfo(Card _card, int SkillPos)
    {
        foreach (SkillInfo info in _card.cardInfo.skillInfo)
        {
            if (info.skillPos == SkillPos)
            {
                return info;
            }
        }
        return null;
    }

    private SkillInfo GetSkillInfoByIdx(int skillPos)
    {
        return new SkillInfo { skillLevel = 0, skillPos = skillPos };
    }

    private int GetSkillLv(Card _card, int _pos)
    {
        foreach (SkillInfo info in _card.cardInfo.skillInfo)
        {
            if (info.skillPos == _pos)
            {
                return info.skillLevel;
            }
        }
        return 0;
    }

    private void GetSubSkillBuffAdd(int skillLevel, int buffEntry, SkillLogicEffectType _type, out float _addValue, out float _multipleValue)
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

    private List<string> GetUpStarAddInfo(Card _cardInfo)
    {
        List<string> list = new List<string>();
        string str = "                         [00ff00]";
        string str2 = " + ";
        if ((this.mOldHeroAttributeDict.Count != 0) && (this.mNewHeroAttributeDict.Count != 0))
        {
            int num = 0;
            float num2 = 0f;
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.Hp]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.Hp]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(340), str2, num }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.Phydefence]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.Phydefence]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15a), str2, num }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.Spellfence]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.Spellfence]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15b), str2, num }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.PhysicsPierce]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.PhysicsPierce]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15c), str2, num }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.SpellPierce]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.SpellPierce]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x15d), str2, num }));
            }
            num2 = ((float) this.mNewHeroAttributeDict[HeroAttributeType.CritRate]) - ((float) this.mOldHeroAttributeDict[HeroAttributeType.CritRate]);
            if (num2 > 0f)
            {
                list.Add(str + ConfigMgr.getInstance().GetWord(350) + str2 + string.Format("{0:N1}", num2 * 100f) + "%");
            }
            num2 = ((float) this.mNewHeroAttributeDict[HeroAttributeType.Tenacity]) - ((float) this.mOldHeroAttributeDict[HeroAttributeType.Tenacity]);
            if (num2 > 0f)
            {
                list.Add(str + ConfigMgr.getInstance().GetWord(0x160) + str2 + string.Format("{0:N1}", num2 * 100f) + "%");
            }
            num2 = ((float) this.mNewHeroAttributeDict[HeroAttributeType.DodRate]) - ((float) this.mOldHeroAttributeDict[HeroAttributeType.DodRate]);
            if (num2 > 0f)
            {
                list.Add(str + ConfigMgr.getInstance().GetWord(0x162) + str2 + string.Format("{0:N1}", num2 * 100f) + "%");
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.HpRecover]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.HpRecover]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x163), str2, num }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.EnergyRecover]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.EnergyRecover]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x164), str2, num }));
            }
            num2 = ((float) this.mNewHeroAttributeDict[HeroAttributeType.BeHealMod]) - ((float) this.mOldHeroAttributeDict[HeroAttributeType.BeHealMod]);
            if (num2 > 0f)
            {
                list.Add(str + ConfigMgr.getInstance().GetWord(0x165) + str2 + string.Format("{0:N1}", num2 * 100f) + "%");
            }
            num2 = ((float) this.mNewHeroAttributeDict[HeroAttributeType.MoveSpeed]) - ((float) this.mOldHeroAttributeDict[HeroAttributeType.MoveSpeed]);
            if (num2 > 0f)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x16a), str2, num2 }));
            }
            num = ((int) this.mNewHeroAttributeDict[HeroAttributeType.SuckLv]) - ((int) this.mOldHeroAttributeDict[HeroAttributeType.SuckLv]);
            if (num > 0)
            {
                list.Add(string.Concat(new object[] { str, ConfigMgr.getInstance().GetWord(0x16e), str2, num }));
            }
        }
        return list;
    }

    public void InitCardInfo(Card _card)
    {
        this.mIsOnleShow = false;
        this.mCard = _card;
        if (_card != null)
        {
            this.mCardId = _card.card_id;
            this.mIsCardPartShow = false;
            Transform transform = base.transform.FindChild("InfoPanel");
            GUIDataHolder.setData(base.transform.FindChild("InfoPanel/FromBtn").gameObject, _card);
            this.SetCardInfo(_card, transform);
            this.SetEquipInfo(_card, true);
            this.SetHeroDescPanel(_card);
            this.mIsCanClickLevUp = true;
            if (this.mShowTabPageIdx == 2)
            {
                this.InitSkillList(true);
                this.mInitSkillOk = true;
                this.mInitFromOk = false;
            }
            else
            {
                this.CheckSkillState();
            }
            if (this.mShowTabPageIdx == 3)
            {
                this.InitFromList(_card);
                this.mInitSkillOk = false;
                this.mInitFromOk = true;
            }
        }
    }

    private void InitFromList(Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config.entry, _config.evolve_lv);
                if (cardExCfg != null)
                {
                    item_config ic = ConfigMgr.getInstance().getByEntry<item_config>(cardExCfg.item_entry);
                    if (ic != null)
                    {
                        this.SetItemDetails(ic);
                    }
                }
            }
        }
    }

    private void InitGuiControlEvent()
    {
        UIEventListener.Get(base.transform.FindChild("InfoPanel/FromBtn").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickFromBtn);
        UIEventListener.Get(this._JiYouBtn).onPress = new UIEventListener.BoolDelegate(this.OnClickJiYouBtn);
        UIEventListener.Get(base.transform.FindChild("TabGroup/1").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTab1);
        UIEventListener.Get(base.transform.FindChild("TabGroup/2").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTab2);
        UIEventListener.Get(base.transform.FindChild("TabGroup/3").gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickTab3);
        this.mEquipGirdList.Clear();
        Transform transform5 = base.transform.FindChild("InfoPanel/Equipment");
        for (int i = 0; i < 6; i++)
        {
            Transform item = transform5.FindChild(i.ToString());
            this.mEquipGirdList.Add(item);
        }
    }

    private void InitPartCardSkillList(Card card)
    {
        if (card != null)
        {
            UIGrid component = base.transform.FindChild("LeveUpPanel/List/Grid").GetComponent<UIGrid>();
            int index = 0;
            this.LevUpBtnList.Clear();
            Debug.Log(card.cardInfo.skillInfo.Count + "----------skill cont");
            foreach (SkillInfo info in card.cardInfo.skillInfo)
            {
                if (index < 4)
                {
                    GameObject go = this._SkillItemList[index];
                    GameObject gameObject = go.transform.FindChild("LeveUpBtn").gameObject;
                    this.LevUpBtnList.Add(gameObject);
                    GUIDataHolder.setData(gameObject, info);
                    GUIDataHolder.setData(go, info);
                    UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickPartSkillItemBtn);
                    this.UpdateLockSkillInfo(go, info);
                    gameObject.GetComponent<UIButton>().isEnabled = false;
                    index++;
                }
            }
        }
    }

    private void InitSkillList(bool isCanClickLevUp)
    {
        UIGrid component = base.transform.FindChild("LeveUpPanel/List/Grid").GetComponent<UIGrid>();
        Card mCard = this.mCard;
        if (mCard != null)
        {
            int index = 0;
            this.LevUpBtnList.Clear();
            Debug.Log(mCard.cardInfo.skillInfo.Count + "----------skill cont");
            foreach (SkillInfo info in mCard.cardInfo.skillInfo)
            {
                if (index < 4)
                {
                    GameObject go = this._SkillItemList[index];
                    GameObject gameObject = go.transform.FindChild("LeveUpBtn").gameObject;
                    this.LevUpBtnList.Add(gameObject);
                    GUIDataHolder.setData(gameObject, info);
                    GUIDataHolder.setData(go, info);
                    if (isCanClickLevUp)
                    {
                        UIEventListener.Get(go).onPress = new UIEventListener.BoolDelegate(this.OnClickSkillItemBtn);
                    }
                    else
                    {
                        UIEventListener listener1 = UIEventListener.Get(go);
                        listener1.onPress = (UIEventListener.BoolDelegate) Delegate.Remove(listener1.onPress, new UIEventListener.BoolDelegate(this.OnClickSkillItemBtn));
                    }
                    if (CommonFunc.SkillIsUnLock((E_CardSkill) info.skillPos, mCard.cardInfo.quality))
                    {
                        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickLevUp);
                        gameObject.gameObject.SetActive(!this.mIsOnleShow);
                        this.UpdateUnLockSkillInfo(go, info);
                    }
                    else
                    {
                        this.UpdateLockSkillInfo(go, info);
                    }
                    gameObject.GetComponent<UIButton>().isEnabled = isCanClickLevUp;
                    index++;
                }
            }
            this.UpdateSkillBtnState();
            if (!this.mIsOnleShow)
            {
                base.transform.FindChild("LeveUpPanel/Point").gameObject.SetActive(true);
            }
        }
    }

    private void JumpToDuplicate(GameObject go)
    {
        <JumpToDuplicate>c__AnonStorey18E storeye = new <JumpToDuplicate>c__AnonStorey18E {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storeye.info = obj2 as MapData;
            if (storeye.info != null)
            {
                Debug.Log(storeye.info.entry + "  " + storeye.info.subEntry);
                SocketMgr.Instance.RequestGetDuplicateRemain(storeye.info.entry, DuplicateType.DupType_Normal);
                SocketMgr.Instance.RequestGetDuplicateRemain(storeye.info.entry, DuplicateType.DupType_Elite);
                storeye.IsCardPartShow = this.mIsCardPartShow;
                GUIMgr.Instance.PushGUIEntity("DupLevInfoPanel", new Action<GUIEntity>(storeye.<>m__1F6));
            }
        }
    }

    private void JumpToTower(GameObject go)
    {
        if (ActorData.getInstance().Level < CommonFunc.LevelLimitCfg().void_tower)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0xa037c0), CommonFunc.LevelLimitCfg().void_tower));
        }
        else
        {
            GUIMgr.Instance.PushGUIEntity("TowerPanel", null);
        }
    }

    public void LevelUpEquipSucess(Card _cardInfo)
    {
        this.mCard = _cardInfo;
        this.InitCardInfo(_cardInfo);
        this.UpdateShowCardByEntry(_cardInfo);
    }

    private float MutiOneParaDefault(string paraStr, int skillLv)
    {
        if (!paraStr.Contains("|"))
        {
            return (float.Parse(paraStr) * skillLv);
        }
        char[] separator = new char[] { '|' };
        string[] strArray = paraStr.Split(separator);
        if (strArray.Length >= 1)
        {
            return (float.Parse(strArray[0]) * skillLv);
        }
        return 0f;
    }

    private void On_SimpleTap(Gesture gesture)
    {
        if ((UICamera.hoveredObject != null) && ((UICamera.hoveredObject.name == "Character") && (this.FakeListIndex != -1)))
        {
            FakeCharacter.GetInstance().PlayCharAnimByIndex(this.FakeListIndex, "rest");
        }
    }

    private void On_Swipe(Gesture gesture)
    {
        if ((UICamera.hoveredObject != null) && ((this.mDargModel && (this.FakeListIndex != -1)) && (this.mPosition.x != gesture.position.x)))
        {
            this.mPosition = gesture.position;
            Vector2 vector = (Vector2) (Vector2.Scale(gesture.deltaPosition, -this.mScaleFactor) / 20f);
            FakeCharacter.GetInstance().Roate(this.FakeListIndex, vector.x);
        }
    }

    private void OnClickBuyPoint()
    {
        <OnClickBuyPoint>c__AnonStorey198 storey = new <OnClickBuyPoint>c__AnonStorey198 {
            <>f__this = this
        };
        int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.BUY_SKILL_POINT);
        if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
        }
        else
        {
            storey.Cost = 0;
            buy_cost_config _config = ConfigMgr.getInstance().getByEntry<buy_cost_config>(this.mSkillPoint.buyCount);
            if (_config == null)
            {
                int count = ConfigMgr.getInstance().getList<buy_cost_config>().Count;
                buy_cost_config _config2 = ConfigMgr.getInstance().getByEntry<buy_cost_config>(count - 1);
                storey.Cost = _config2.buy_skill_cost_stone;
            }
            else
            {
                storey.Cost = _config.buy_skill_cost_stone;
            }
            this.OpenMsgBox = true;
            GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1FF), base.gameObject);
        }
    }

    private void OnClickCardBreak()
    {
        <OnClickCardBreak>c__AnonStorey194 storey = new <OnClickCardBreak>c__AnonStorey194 {
            <>f__this = this
        };
        if (this.mCardId >= 0L)
        {
            if (this.mCard.cardInfo.quality >= 6)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x7d8));
            }
            else if (!this.CheckEquipOk(this.mCard))
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287e));
            }
            else
            {
                storey.bcc = CommonFunc.GetBreakCardCfg((int) this.mCard.cardInfo.entry, this.mCard.cardInfo.quality);
                if (storey.bcc != null)
                {
                    int num = storey.bcc.lv_limit;
                    if (this.mCard.cardInfo.level < num)
                    {
                        TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x7d7), num));
                    }
                    else
                    {
                        if (GuideSystem.MatchEvent(GuideEvent.CardBreak_Function))
                        {
                            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_CardBreak.tag_cardbreak_press_break_button, null);
                        }
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1FB), base.gameObject);
                    }
                }
            }
        }
    }

    private void OnClickCardUpStar()
    {
        <OnClickCardUpStar>c__AnonStorey195 storey = new <OnClickCardUpStar>c__AnonStorey195 {
            <>f__this = this
        };
        if (this.mUpStarOk && (this.mCardId >= 0L))
        {
            if (!this.mPartEnough)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x2878));
            }
            else
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.mCard.cardInfo.entry);
                if (_config != null)
                {
                    storey.cec = CommonFunc.GetCardExCfg(_config.entry, this.mCard.cardInfo.starLv);
                    if (storey.cec != null)
                    {
                        GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1FC), base.gameObject);
                    }
                }
            }
        }
    }

    private void OnClickEquipAutoBreakBtn(GameObject go)
    {
        <OnClickEquipAutoBreakBtn>c__AnonStorey190 storey = new <OnClickEquipAutoBreakBtn>c__AnonStorey190 {
            go = go,
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(storey.go);
        if (obj2 != null)
        {
            storey.info = obj2 as EquipInfo;
            if (storey.info != null)
            {
                break_equip_config bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(storey.info.entry);
                if (bec != null)
                {
                    storey.allCost = this.GetAutoBreakCost(bec);
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1F8), null);
                }
            }
        }
    }

    private void OnClickEquipBtn(GameObject go)
    {
        <OnClickEquipBtn>c__AnonStorey192 storey = new <OnClickEquipBtn>c__AnonStorey192 {
            <>f__this = this
        };
        if (GuideSystem.MatchEvent(GuideEvent.Strengthen_Function))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Strengthen.tag_equip_strengthen_select_equip, go);
        }
        Transform transform = go.transform.FindChild("Up");
        object obj2 = GUIDataHolder.getData(go);
        storey.canLevelUp = transform.gameObject.activeSelf;
        storey.partIdx = int.Parse(go.name);
        if (obj2 != null)
        {
            <OnClickEquipBtn>c__AnonStorey193 storey2 = new <OnClickEquipBtn>c__AnonStorey193 {
                <>f__ref$402 = storey,
                <>f__this = this,
                info = obj2 as EquipInfo
            };
            GUIMgr.Instance.PushGUIEntity<BreakEquipPanel>(new Action<GUIEntity>(storey2.<>m__1FA));
        }
    }

    private void OnClickEquipUpBtn(GameObject go)
    {
        <OnClickEquipUpBtn>c__AnonStorey191 storey = new <OnClickEquipUpBtn>c__AnonStorey191 {
            go = go,
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(storey.go);
        if (obj2 != null)
        {
            storey.info = obj2 as EquipInfo;
            if (storey.info != null)
            {
                storey.bec = ConfigMgr.getInstance().getByEntry<break_equip_config>(storey.info.entry);
                if (storey.bec != null)
                {
                    if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Function))
                    {
                        GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Equip.tag_equip_levelup_select_equip, storey.go);
                    }
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__1F9), null);
                }
            }
        }
    }

    private void OnClickFromBtn(GameObject go)
    {
        <OnClickFromBtn>c__AnonStorey18C storeyc = new <OnClickFromBtn>c__AnonStorey18C {
            <>f__this = this
        };
        object obj2 = GUIDataHolder.getData(go);
        if (obj2 != null)
        {
            storeyc.info = obj2 as Card;
            if (storeyc.info != null)
            {
                GUIMgr.Instance.DoModelGUI("DetailsPanel", new Action<GUIEntity>(storeyc.<>m__1F4), base.gameObject);
            }
        }
    }

    private void OnClickGem()
    {
        GUIMgr.Instance.PushGUIEntity("GemWearPanel", delegate (GUIEntity obj) {
            GemWearPanel panel = (GemWearPanel) obj;
            panel.InitCardInfo(this.mCard);
            panel.SetCurrShowCardList(this.mCurrShowCardList);
        });
    }

    private void OnClickJiYouBtn(GameObject go, bool isPressed)
    {
        if (this.mCard != null)
        {
            if (isPressed)
            {
                <OnClickJiYouBtn>c__AnonStorey18B storeyb = new <OnClickJiYouBtn>c__AnonStorey18B();
                if (GUIMgr.Instance.GetGUIEntity<InfoDlag>() != null)
                {
                    GUIMgr.Instance.ExitModelGUI("InfoDlag");
                }
                else
                {
                    storeyb.cc = ConfigMgr.getInstance().getByEntry<card_config>((int) this.mCard.cardInfo.entry);
                    if (storeyb.cc != null)
                    {
                        GUIMgr.Instance.DoModelGUI("InfoDlag", new Action<GUIEntity>(storeyb.<>m__1F3), null);
                    }
                }
            }
            else
            {
                GUIMgr.Instance.ExitModelGUI("InfoDlag");
            }
        }
    }

    private void OnClickLeft()
    {
        if ((this.mUpStarOk && (this.mCurrShowCardList != null)) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, false);
            this.InitCardInfo(cardInfo);
            this.ClearAllEffect();
            if (this.mShowTabPageIdx == 2)
            {
                this.InitSkillList(this.mIsCanClickLevUp);
                this.mInitSkillOk = true;
                this.mInitFromOk = false;
            }
            else if (this.mShowTabPageIdx == 3)
            {
                this.InitFromList(this.mCard);
                this.mInitFromOk = true;
                this.mInitSkillOk = false;
            }
            else
            {
                this.mInitFromOk = false;
                this.mInitSkillOk = false;
            }
            if ((cardInfo != null) && CommonFunc.CheckHaveEquipLevUp(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.EquipLevelUp_Function);
            }
            if ((cardInfo != null) && CommonFunc.CheckCardCanBreak(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.CardBreak_Function);
            }
            if ((cardInfo != null) && Utility.CheckSkillCanbeLevelup(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_Function);
            }
            if ((cardInfo != null) && (Utility.GetEquipCanbeStrengthen(cardInfo) != null))
            {
                GuideSystem.FireEvent(GuideEvent.Strengthen_Function);
            }
        }
    }

    private void OnClickLevUp(GameObject obj)
    {
        if (GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Function))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_SkillLevelUp.tag_skill_levelup_press_button, obj);
        }
        SkillInfo info = (SkillInfo) GUIDataHolder.getData(obj);
        int skillPos = info.skillPos;
        Card cardByID = ActorData.getInstance().GetCardByID(this.mCardId);
        if ((cardByID != null) && (this.mSkillPoint != null))
        {
            if (this.mSkillPoint.totalPoint <= 0)
            {
                if (!this.OpenMsgBox)
                {
                    int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.BUY_SKILL_POINT);
                    if (ActorData.getInstance().UserInfo.vip_level.level >= vipLevelByLockFunc)
                    {
                        this.OnClickBuyPoint();
                    }
                    else
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287a));
                    }
                }
            }
            else
            {
                skill_config skillCfg = this.GetSkillCfg(cardByID, info);
                if (ActorData.getInstance().Gold < CommonFunc.GetSkillLevUpCost(skillPos, info.skillLevel))
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                }
                else
                {
                    card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) cardByID.cardInfo.entry);
                    if (CommonFunc.CanLvUpSkill(cardByID.cardInfo.quality, skillPos))
                    {
                        int maxSkillLv = CommonFunc.GetMaxSkillLv(CommonFunc.GetSkillIDByCardEntry(_config2.entry, (E_CardSkill) skillPos), cardByID.cardInfo.level);
                        if (ActorData.getInstance().GetSkillLev(cardByID.card_id, skillPos) >= maxSkillLv)
                        {
                            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652ac));
                        }
                        else
                        {
                            this.CurClickSkillObj = obj.transform.parent.gameObject;
                            SocketMgr.Instance.RequestSkillLvUp(this.mCardId, skillPos);
                        }
                    }
                    else
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa652ab));
                    }
                }
            }
        }
    }

    private void OnClickPartSkillItemBtn(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            SkillInfo info = GUIDataHolder.getData(go) as SkillInfo;
            Card mCard = this.mCard;
            skill_config skillCfg = this.GetSkillCfg(mCard, info);
            if (skillCfg != null)
            {
                this.mCurrPressItem = go;
                this._SkillTips.transform.position = new Vector3(this._SkillTips.transform.position.x, go.transform.position.y, 0f);
                UILabel component = this._SkillTips.transform.FindChild("Desc").GetComponent<UILabel>();
                UILabel label2 = this._SkillTips.transform.FindChild("Name").GetComponent<UILabel>();
                label2.text = "[F0E8D3]" + skillCfg.name + "：";
                label2.MakePixelPerfect();
                this.LabelLv.text = "Lv." + info.skillLevel;
                component.text = "[DECBAA]" + skillCfg.desc;
                component.MakePixelPerfect();
                this.LabeldescAB.text = string.Empty;
                this.spriteTipsBackGround.SetDimensions(component.width + 80, (((component.height + 0x1c) + 20) + 10) + 15);
                this._SkillTips.gameObject.SetActive(true);
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("InfoDlag");
            this._tipsPanel = null;
            this._SkillTips.gameObject.SetActive(false);
            this.mCurrPressItem = null;
        }
    }

    private void OnClickRight()
    {
        if ((this.mUpStarOk && (this.mCurrShowCardList != null)) && (this.mCurrShowCardList.Count > 1))
        {
            Card cardInfo = this.GetCardInfo(this.mCard, true);
            this.InitCardInfo(cardInfo);
            this.ClearAllEffect();
            if (this.mShowTabPageIdx == 2)
            {
                this.InitSkillList(this.mIsCanClickLevUp);
                this.mInitSkillOk = true;
                this.mInitFromOk = false;
            }
            else if (this.mShowTabPageIdx == 3)
            {
                this.InitFromList(this.mCard);
                this.mInitFromOk = true;
                this.mInitSkillOk = false;
            }
            else
            {
                this.mInitFromOk = false;
                this.mInitSkillOk = false;
            }
            if ((cardInfo != null) && CommonFunc.CheckHaveEquipLevUp(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.EquipLevelUp_Function);
            }
            if ((cardInfo != null) && CommonFunc.CheckCardCanBreak(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.CardBreak_Function);
            }
            if ((cardInfo != null) && Utility.CheckSkillCanbeLevelup(cardInfo))
            {
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_Function);
            }
            if ((cardInfo != null) && (Utility.GetEquipCanbeStrengthen(cardInfo) != null))
            {
                GuideSystem.FireEvent(GuideEvent.Strengthen_Function);
            }
        }
    }

    private void OnClickSkillItemBtn(GameObject go, bool isPressed)
    {
        if (isPressed)
        {
            SkillInfo info = GUIDataHolder.getData(go) as SkillInfo;
            Card cardByID = ActorData.getInstance().GetCardByID(this.mCardId);
            skill_config skillCfg = this.GetSkillCfg(cardByID, info);
            if (skillCfg != null)
            {
                this.mCurrPressItem = go;
                this._SkillTips.transform.position = new Vector3(this._SkillTips.transform.position.x, go.transform.position.y, 0f);
                UILabel component = this._SkillTips.transform.FindChild("Desc").GetComponent<UILabel>();
                UILabel label2 = this._SkillTips.transform.FindChild("Name").GetComponent<UILabel>();
                label2.text = "[F0E8D3]" + skillCfg.name + "：";
                label2.MakePixelPerfect();
                this.LabelLv.text = "Lv." + info.skillLevel;
                int num = skillCfg.skill_desc_1;
                int num2 = skillCfg.skill_desc_2;
                string str2 = skillCfg.skill_desc_arg_1;
                string str3 = skillCfg.skill_desc_arg_2;
                int skillLevel = info.skillLevel;
                string str4 = string.Empty;
                str4 = this.SetDescLabelInfo(num, str2, num2, str3, skillLevel, skillCfg.entry);
                component.text = "[DECBAA]" + skillCfg.desc;
                if ((str4 != string.Empty) && (str4 != null))
                {
                    this.LabeldescAB.text = "[A8FF00]" + str4;
                }
                component.MakePixelPerfect();
                this.LabeldescAB.transform.localPosition = new Vector3(this.LabeldescAB.transform.localPosition.x, (component.transform.localPosition.y - component.height) - 6f, this.LabeldescAB.transform.localPosition.z);
                this.LabeldescAB.MakePixelPerfect();
                this.spriteTipsBackGround.SetDimensions(component.width + 80, (((((component.height + 0x1c) + 20) + 10) + this.LabeldescAB.height) + 6) + 15);
                this._SkillTips.gameObject.SetActive(true);
            }
        }
        else
        {
            GUIMgr.Instance.ExitModelGUI("InfoDlag");
            this._tipsPanel = null;
            this._SkillTips.gameObject.SetActive(false);
            this.mCurrPressItem = null;
        }
    }

    private void OnClickTab1(GameObject go)
    {
        this.ShowTabPage(1);
    }

    private void OnClickTab2(GameObject go)
    {
        this.ShowTabPage(2);
        if (GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Function))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_SkillLevelUp.tag_skill_levelup_change_table, go);
        }
    }

    private void OnClickTab3(GameObject go)
    {
        this.ShowTabPage(3);
    }

    public void OnClosePanel()
    {
        FakeCharacter.GetInstance().DestroyCharater(this.FakeListIndex);
        this.FakeListIndex = -1;
        if (this.mIsOnleShow)
        {
            GUIMgr.Instance.ExitModelGUI(base.name);
        }
        else
        {
            GUIMgr.Instance.PopGUIEntity();
        }
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        if ((this.mCard != null) && !this.mIsCardPartShow)
        {
            this.RecordOldData();
            int level = this.mCard.cardInfo.level;
            this.mCard = ActorData.getInstance().CardList.Find(e => e.card_id == this.mCard.card_id);
            if (this.mCard != null)
            {
                if (level != this.mCard.cardInfo.level)
                {
                    this.LevelUpEquipSucess(this.mCard);
                }
                else
                {
                    this.SetChipInfo();
                }
                this.SetCardPower();
            }
        }
        GUIMgr.Instance.FloatTitleBar();
        if (!this.mIsCardPartShow)
        {
            this.SkillPoint = ActorData.getInstance().CurSkillPoint;
        }
        else
        {
            this.mInitSkillOk = true;
            this.mInitFromOk = true;
        }
        this._SkillTips.SetActive(false);
        this.mUpStarOk = true;
    }

    public override void OnDestroy()
    {
        FakeCharacter.GetInstance().DestroyCharater(this.FakeListIndex);
        this.UnsubscribeEvent();
        if (GUIMgr.Instance.GetGUIEntity<InfoDlag>() != null)
        {
            GUIMgr.Instance.ExitModelGUI("InfoDlag");
        }
        if (null != GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>())
        {
            if (<>f__am$cache33 == null)
            {
                <>f__am$cache33 = delegate (Item item) {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(item.entry);
                    if (_config == null)
                    {
                        return false;
                    }
                    return (_config.type == 3) && (item.num >= 10);
                };
            }
            if (ActorData.getInstance().ItemList.Find(<>f__am$cache33) != null)
            {
            }
        }
    }

    private void OneKeyLevUp()
    {
        int vipLevelByLockFunc = CommonFunc.GetVipLevelByLockFunc(VipLockFunc.ONEKEY_EQU_LEV_UP);
        if (ActorData.getInstance().UserInfo.vip_level.level < vipLevelByLockFunc)
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2aba), vipLevelByLockFunc));
        }
        else if (this.GetOneKeyCost() == 0)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2abb));
        }
        else if (this.GetOneKeyCost() > ActorData.getInstance().Gold)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0xa037b8));
        }
        else
        {
            SocketMgr.Instance.RequestOneKeyEquipLevUp(this.mCard.card_id);
        }
    }

    public override void OnInitialize()
    {
        this.spriteTipsBackGround = this._SkillTips.transform.Find("Background").GetComponent<UISprite>();
        this.LabeldescAB = this._SkillTips.transform.Find("DescAB").GetComponent<UILabel>();
        this.LabelLv = this._SkillTips.transform.Find("LabelLv").GetComponent<UILabel>();
        this._SkillTabPoint = base.transform.FindChild("TabGroup/2/New");
        UIEventListener.Get(base.transform.FindChild("InfoPanel/Hero/Character").gameObject).onPress = new UIEventListener.BoolDelegate(this.OnPressModel);
        this.InitGuiControlEvent();
        EasyTouch.On_Swipe += new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SimpleTap += new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
        this.mSkillPointLabel = base.gameObject.transform.FindChild("LeveUpPanel/Point/SkillPoint").GetComponent<UILabel>();
        this.mBuyBtn = base.gameObject.transform.FindChild("LeveUpPanel/Point/BuyBtn").gameObject;
        this.SetArrowPostion();
    }

    public void OnlyShowCardInfo(Card card, string userName, int power)
    {
        Card card2 = card;
        this.mIsCanClickLevUp = false;
        this.mIsOnleShow = true;
        this.mCard = card2;
        Transform transform = base.transform.FindChild("InfoPanel");
        this.SetCardInfo(card2, transform);
        this.SetEquipInfo(card2, false);
        this.SetHeroDescPanel(card2);
        base.FindChild<Transform>("GemBtn").ActiveSelfObject(false);
        base.transform.FindChild("InfoPanel/BaseInfo/Power").GetComponent<UILabel>().text = power.ToString();
        UILabel component = base.transform.FindChild("InfoPanel/FormDesc").GetComponent<UILabel>();
        component.text = "来自：" + userName;
        component.gameObject.SetActive(true);
        base.transform.FindChild("InfoPanel/FromBtn").gameObject.SetActive(false);
        base.transform.FindChild("InfoPanel/AddStar").gameObject.SetActive(false);
        base.transform.FindChild("InfoPanel/BreakBtn").gameObject.SetActive(false);
        base.transform.FindChild("Left").gameObject.SetActive(false);
        base.transform.FindChild("Right").gameObject.SetActive(false);
        base.transform.FindChild("InfoPanel/OneKeyBtn").GetComponent<BoxCollider>().enabled = false;
        base.transform.FindChild("LeveUpPanel/Point").gameObject.SetActive(false);
        base.transform.FindChild("TabGroup/3").gameObject.SetActive(false);
        this.mInitFromOk = false;
        this.mInitSkillOk = false;
        this._ShuXinPanel.SetActive(true);
        this._SkillPanel.SetActive(false);
        this._FromPanel.SetActive(false);
        this.SetCheckBoxStat(1, true);
        this.SetCheckBoxStat(2, false);
        this._SkillTabPoint.gameObject.SetActive(false);
    }

    private void OnPressModel(GameObject go, bool isPress)
    {
        this.mDargModel = isPress;
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        base.OnSerialization(pers);
        this.mInitFromOk = false;
        this.mInitSkillOk = false;
        base.transform.FindChild("Left").gameObject.SetActive(true);
        base.transform.FindChild("Right").gameObject.SetActive(true);
        this.ClearSkillEffect();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.TipsMsgList.Count > 0)
        {
            this.TipsTime += Time.deltaTime;
            if (this.TipsTime >= this.TipsInterval)
            {
                TipsDiag.PushText("[00ff00]" + this.TipsMsgList.Dequeue(), new Vector2(0f, 0f));
                this.TipsTime = 0f;
            }
        }
        if (this._SkillTips.active && (this.mCurrPressItem != null))
        {
            this._SkillTips.transform.position = new Vector3(this._SkillTips.transform.position.x, this.mCurrPressItem.transform.position.y, 0f);
        }
        this.m_time += Time.deltaTime;
        if (this.m_time > this.m_updateInterval)
        {
            this.m_time = 0f;
            if (!this.mIsOnleShow && !this.mIsCardPartShow)
            {
                this.UpdateSkillPoint(true);
            }
        }
    }

    private void PlayObjAmin()
    {
    }

    [DebuggerHidden]
    private IEnumerator PlaySkillEffect(skill_config skillCfg)
    {
        return new <PlaySkillEffect>c__Iterator60 { skillCfg = skillCfg, <$>skillCfg = skillCfg, <>f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator PushBreakText(List<string> _infoList)
    {
        return new <PushBreakText>c__Iterator5E { _infoList = _infoList, <$>_infoList = _infoList };
    }

    [DebuggerHidden]
    private IEnumerator PushText(List<string> _infoList)
    {
        return new <PushText>c__Iterator5F { _infoList = _infoList, <$>_infoList = _infoList, <>f__this = this };
    }

    public void RecordOldData()
    {
        this.GetHeroAttributeData(this.mCard, true);
    }

    public void RequestCardBreakGeneration()
    {
        if (!GuideSystem.MatchEvent(GuideEvent.CardBreak_Function))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else
        {
            Transform transform = base.transform.FindChild("InfoPanel/BreakBtn");
            if (null != transform)
            {
                UIButton component = transform.GetComponent<UIButton>();
                if ((null != component) && component.enabled)
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_CardBreak.tag_cardbreak_press_break_button, transform.gameObject);
                    return;
                }
            }
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    public void RequestEquipGeneration()
    {
        if (!GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Function))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else
        {
            int count = this.mEquipGirdList.Count;
            for (int i = 0; i != count; i++)
            {
                Transform transform = this.mEquipGirdList[i];
                if (null != transform)
                {
                    EquipInfo info = GUIDataHolder.getData(transform.gameObject) as EquipInfo;
                    if ((info != null) && this.EquipCanbeLevelUp(info))
                    {
                        GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Equip.tag_equip_levelup_select_equip, transform.gameObject);
                        return;
                    }
                }
            }
            GuideSystem.ActivedGuide.RequestCancel();
        }
    }

    public void RequestSkillLevelupGeneration()
    {
        if (!GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_Function))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else if (this.mCard == null)
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else
        {
            E_CardSkill skill = Utility.SkillLevelupSlot(this.mCard);
            if (skill == E_CardSkill.e_card_skill_max)
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                int num = ((int) skill) + 1;
                Transform transform = base.transform.FindChild("LeveUpPanel/List/Grid/Item" + num.ToString() + "/LeveUpBtn");
                if ((null != transform) && transform.gameObject.activeSelf)
                {
                    UIButton component = transform.GetComponent<UIButton>();
                    BoxCollider collider = transform.GetComponent<BoxCollider>();
                    if (((null != component) && component.enabled) && ((null != collider) && collider.enabled))
                    {
                        GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_SkillLevelUp.tag_skill_levelup_press_button, transform.gameObject);
                        return;
                    }
                }
                GuideSystem.ActivedGuide.RequestCancel();
            }
        }
    }

    public void RequestSkillTabGeneration()
    {
        Transform transform = base.transform.FindChild("TabGroup/2/LabelUp");
        if (null == transform)
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        else
        {
            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_SkillLevelUp.tag_skill_levelup_change_table, transform.gameObject);
        }
    }

    public void RequstStrengthenGeneration()
    {
        if (this.mCard != null)
        {
            if (!GuideSystem.MatchEvent(GuideEvent.Strengthen_Function))
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                EquipInfo equipCanbeStrengthen = Utility.GetEquipCanbeStrengthen(this.mCard);
                if (equipCanbeStrengthen == null)
                {
                    GuideSystem.ActivedGuide.RequestCancel();
                }
                else
                {
                    int part = equipCanbeStrengthen.part;
                    if (part >= this.mEquipGirdList.Count)
                    {
                        GuideSystem.ActivedGuide.RequestCancel();
                    }
                    else
                    {
                        Transform transform = this.mEquipGirdList[part];
                        if (null == transform)
                        {
                            GuideSystem.ActivedGuide.RequestCancel();
                        }
                        else
                        {
                            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Strengthen.tag_equip_strengthen_select_equip, transform.gameObject);
                        }
                    }
                }
            }
        }
    }

    public void ResetEquipInfo()
    {
        this.SetEquipInfo(this.mCard, true);
    }

    public void ResetHeroInfoAndPopChange(Card _cardInfo)
    {
        this.GetHeroAttributeData(this.mCard, true);
        this.GetHeroAttributeData(_cardInfo, false);
        this.InitCardInfo(_cardInfo);
        this.UpdateShowCardByEntry(_cardInfo);
        base.StartCoroutine(this.PushBreakText(this.GetUpStarAddInfo(_cardInfo)));
    }

    private void SetArrowPostion()
    {
        if (GUIMgr.Instance.Root.activeWidth < 0x3e8)
        {
            Transform transform = base.transform.FindChild("Left/LBtn");
            Transform transform2 = base.transform.FindChild("Right/RBtn");
            transform.transform.localPosition = new Vector3(20f, -255f, transform.transform.localPosition.z);
            transform2.transform.localPosition = new Vector3(-38f, -255f, transform2.transform.localPosition.z);
        }
    }

    private void SetCardInfo(Card _card, Transform obj)
    {
        <SetCardInfo>c__AnonStorey196 storey = new <SetCardInfo>c__AnonStorey196();
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                base.transform.FindChild("InfoPanel/FormDesc").GetComponent<UILabel>().text = string.Empty;
                if (this.FakeListIndex > 0)
                {
                    FakeCharacter.GetInstance().DestroyCharater(this.FakeListIndex);
                }
                UITexture component = obj.FindChild("Hero/Character").GetComponent<UITexture>();
                this.FakeListIndex = this.CreateModel(_card, component);
                obj.FindChild("Hero/Name").GetComponent<UILabel>().text = GameConstant.C_Label[_card.cardInfo.quality] + CommonFunc.GetCardNameByQuality(_card.cardInfo.quality, _config.name);
                CommonFunc.SetQualityColor(obj.FindChild("Hero/QualityBorder").GetComponent<UISprite>(), _card.cardInfo.quality);
                CommonFunc.SetQualityColor(obj.FindChild("Hero/QualityBorder1").GetComponent<UISprite>(), _card.cardInfo.quality);
                obj.FindChild("BaseInfo/Level").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
                UILabel label4 = obj.FindChild("BaseInfo/Exp").GetComponent<UILabel>();
                if (ConfigMgr.getInstance().getByEntry<card_lv_up_config>(_card.cardInfo.level) != null)
                {
                    label4.text = _card.cardInfo.curExp + " / " + CommonFunc.GetCardLvUpExp(_card.cardInfo.level, _config.lv_up_type);
                }
                break_card_config breakCardCfg = CommonFunc.GetBreakCardCfg((int) this.mCard.cardInfo.entry, this.mCard.cardInfo.quality);
                UILabel label5 = obj.FindChild("BaseInfo/BreakTips").GetComponent<UILabel>();
                if (breakCardCfg != null)
                {
                    if (breakCardCfg.lv_limit == -1)
                    {
                        label5.text = string.Empty;
                    }
                    else
                    {
                        label5.text = string.Format(ConfigMgr.getInstance().GetWord(0x502), breakCardCfg.lv_limit);
                    }
                }
                else
                {
                    label5.text = string.Empty;
                }
                Transform transform = obj.FindChild("Hero");
                for (int i = 0; i < 5; i++)
                {
                    UISprite sprite3 = transform.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                    UISprite sprite4 = transform.transform.FindChild("Star/empty" + (i + 1)).GetComponent<UISprite>();
                    sprite3.gameObject.SetActive(i < _card.cardInfo.starLv);
                }
                storey.cec = CommonFunc.GetCardExCfg(_config.entry, _card.cardInfo.starLv);
                if (storey.cec != null)
                {
                    UILabel label6 = obj.transform.FindChild("AddStar/Count").GetComponent<UILabel>();
                    Transform transform2 = obj.transform.FindChild("AddStar/Slider");
                    UISprite sprite5 = transform2.transform.FindChild("Background").GetComponent<UISprite>();
                    UISprite sprite6 = transform2.transform.FindChild("Foreground").GetComponent<UISprite>();
                    UIButton button = base.transform.FindChild("InfoPanel/BreakBtn").GetComponent<UIButton>();
                    UIButton button2 = obj.transform.FindChild("AddStar/UpStarBtn").GetComponent<UIButton>();
                    button.isEnabled = _card.cardInfo.quality < 6;
                    if (_card.cardInfo.starLv >= 5)
                    {
                        label6.text = ConfigMgr.getInstance().GetWord(0x14f);
                        sprite6.width = sprite5.width - 2;
                        sprite6.gameObject.SetActive(true);
                        button2.isEnabled = false;
                        this._UpStarFlashSprite.gameObject.SetActive(false);
                    }
                    else
                    {
                        Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storey.<>m__1FD));
                        int num2 = (item != null) ? item.num : 0;
                        label6.text = num2 + "/" + storey.cec.evolve_need_item_num;
                        this.mPartEnough = num2 >= storey.cec.evolve_need_item_num;
                        float num3 = ((float) num2) / ((float) storey.cec.evolve_need_item_num);
                        if (num3 > 1f)
                        {
                            num3 = 1f;
                        }
                        sprite6.width = (int) ((sprite5.width - 2) * num3);
                        sprite6.gameObject.SetActive(num3 > 0f);
                        button2.isEnabled = true;
                        if (!this.mIsOnleShow)
                        {
                            this._UpStarFlashSprite.gameObject.SetActive(num3 == 1f);
                        }
                    }
                    UIButton button3 = base.transform.FindChild("InfoPanel/GemBtn").GetComponent<UIButton>();
                    Transform transform3 = obj.FindChild("Hero/Star");
                    variable_config _config4 = ConfigMgr.getInstance().getByEntry<variable_config>(0);
                    if (_config4 != null)
                    {
                        int num4 = _config4.card_gem1_openlv;
                        button3.gameObject.SetActive(ActorData.getInstance().Level >= num4);
                        int num5 = (ActorData.getInstance().Level < num4) ? -90 : -40;
                        transform3.localPosition = new Vector3(289f, (float) num5, 0f);
                    }
                    this.SetCardPower();
                }
            }
        }
    }

    public void SetCardPower()
    {
        base.transform.FindChild("InfoPanel/BaseInfo/Power").GetComponent<UILabel>().text = ActorData.getInstance().GetCardFightPowerById(this.mCardId).ToString();
    }

    private void SetCheckBoxStat(int tab, bool isChecked)
    {
        UIToggle component = base.transform.FindChild("TabGroup/" + tab).GetComponent<UIToggle>();
        component.isChecked = isChecked;
        component.transform.FindChild("New").gameObject.SetActive(false);
        UISprite sprite = component.transform.FindChild("BgDown").GetComponent<UISprite>();
        UISprite sprite2 = component.transform.FindChild("LabelDown").GetComponent<UISprite>();
        sprite.color = !isChecked ? ((Color) new Color32(0xff, 0xff, 0xff, 0)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
        sprite2.color = !isChecked ? ((Color) new Color32(0xff, 0xff, 0xff, 0)) : ((Color) new Color32(0xff, 0xff, 0xff, 0xff));
    }

    private void SetChipInfo()
    {
        <SetChipInfo>c__AnonStorey18A storeya = new <SetChipInfo>c__AnonStorey18A();
        if ((this.mCard != null) && (this.mCard.cardInfo.starLv < 5))
        {
            storeya.cec = CommonFunc.GetCardExCfg((int) this.mCard.cardInfo.entry, this.mCard.cardInfo.starLv);
            if (storeya.cec != null)
            {
                Transform transform = base.transform.FindChild("InfoPanel");
                UILabel component = transform.FindChild("AddStar/Count").GetComponent<UILabel>();
                Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeya.<>m__1F1));
                int num = (item != null) ? item.num : 0;
                component.text = num + "/" + storeya.cec.evolve_need_item_num;
                this.mPartEnough = num >= storeya.cec.evolve_need_item_num;
                float num2 = ((float) num) / ((float) storeya.cec.evolve_need_item_num);
                if (num2 > 1f)
                {
                    num2 = 1f;
                }
                Transform transform2 = transform.FindChild("AddStar/Slider");
                UISprite sprite = transform2.transform.FindChild("Background").GetComponent<UISprite>();
                UISprite sprite2 = transform2.transform.FindChild("Foreground").GetComponent<UISprite>();
                sprite2.width = (int) ((sprite.width - 2) * num2);
                sprite2.gameObject.SetActive(num2 > 0f);
            }
        }
    }

    public void SetCurrShowCardList(List<Card> _CurrShowCardList)
    {
        this.mCurrShowCardList = _CurrShowCardList;
    }

    private string SetDescLabelInfo(int descType_1, string paraStr_1, int descType_2, string paraStr_2, int skillLv, int curSkillId)
    {
        string str = string.Empty;
        if (descType_1 == -1)
        {
            return str;
        }
        if (descType_2 == -1)
        {
            return (str = this.CalculateCurSkillLvEffValue_Base(descType_1, paraStr_1, skillLv, curSkillId, 1));
        }
        return (this.CalculateCurSkillLvEffValue_Base(descType_1, paraStr_1, skillLv, curSkillId, 1) + "\n" + this.CalculateCurSkillLvEffValue_Base(descType_2, paraStr_2, skillLv, curSkillId, 2));
    }

    private void SetDropInfo(GameObject obj, DropFromType _type, int _pram1, int _pram2)
    {
        UISprite component = obj.transform.FindChild("Background").GetComponent<UISprite>();
        UISprite sprite2 = obj.transform.FindChild("Type").GetComponent<UISprite>();
        UILabel label = obj.transform.FindChild("IsOpen").GetComponent<UILabel>();
        UITexture texture = obj.transform.FindChild("Head/Icon").GetComponent<UITexture>();
        bool flag = false;
        UILabel label2 = obj.transform.FindChild("Label").GetComponent<UILabel>();
        switch (_type)
        {
            case DropFromType.E_NORMAL_DUPLICATE:
            {
                duplicate_config _config = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config != null)
                {
                    trench_normal_config _config2 = ConfigMgr.getInstance().getByEntry<trench_normal_config>(_pram2);
                    if (_config2 != null)
                    {
                        label2.text = _config.name + "-" + _config2.name;
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config2.monster_picture);
                        sprite2.gameObject.SetActive(true);
                        flag = (ActorData.getInstance().NormalProgress >= _pram2) && (ActorData.getInstance().Level >= _config.unlock_lv);
                        if (flag)
                        {
                            MapData data = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Normal
                            };
                            GUIDataHolder.setData(obj, data);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                        sprite2.spriteName = !flag ? "Ui_Heroinfo_Label_ptgrey" : "Ui_Heroinfo_Label_pt";
                    }
                }
                break;
            }
            case DropFromType.E_ELITE_DUPLICATE:
            {
                duplicate_config _config3 = ConfigMgr.getInstance().getByEntry<duplicate_config>(_pram1);
                if (_config3 != null)
                {
                    trench_elite_config _config4 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(_pram2);
                    if (_config4 != null)
                    {
                        label2.text = _config3.name + "-" + _config4.name;
                        texture.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config4.monster_picture);
                        flag = (ActorData.getInstance().EliteProgress >= _pram2) && (ActorData.getInstance().Level >= _config3.unlock_lv);
                        if (flag)
                        {
                            MapData data2 = new MapData {
                                entry = _pram1,
                                subEntry = _pram2,
                                type = DuplicateType.DupType_Elite
                            };
                            GUIDataHolder.setData(obj, data2);
                            UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToDuplicate);
                        }
                        sprite2.spriteName = !flag ? "Ui_Heroinfo_Label_jygrey" : "Ui_Heroinfo_Label_jy";
                        sprite2.gameObject.SetActive(true);
                    }
                }
                break;
            }
            case DropFromType.E_WORLDBOSS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_boss;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b0);
                break;

            case DropFromType.E_WARMMATCH:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().world_cup;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b2);
                break;

            case DropFromType.E_VOIDTOWER:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().void_tower;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b3);
                UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.JumpToTower);
                break;

            case DropFromType.E_DUNGEONS:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().dungeons;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b7);
                break;

            case DropFromType.E_OPENBOX:
                flag = ActorData.getInstance().Level >= CommonFunc.LevelLimitCfg().item_box;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b4);
                break;

            case DropFromType.E_SHOP:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b5);
                break;

            case DropFromType.E_GUILDSHOP:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(0x1b6);
                break;

            case DropFromType.E_LOTTERY:
                flag = true;
                label2.text = ConfigMgr.getInstance().GetWord(440);
                break;

            default:
                label2.text = string.Empty;
                break;
        }
        if (flag)
        {
            label.text = ConfigMgr.getInstance().GetWord(220);
            label.color = (Color) new Color32(0, 150, 0x1a, 0xff);
        }
        else
        {
            label.text = ConfigMgr.getInstance().GetWord(0xdd);
        }
        nguiTextureGrey.doChangeEnableGrey(texture, !flag);
    }

    private void SetEquipInfo(Card _card, bool isCanClickEquip)
    {
        if ((_card != null) && (ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry) != null))
        {
            Transform transform = base.transform.FindChild("InfoPanel/Equipment");
            for (int i = 0; i < 6; i++)
            {
                Transform transform2 = this.mEquipGirdList[i];
                EquipInfo info = _card.equipInfo[i];
                UIEventListener.Get(transform2.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEquipBtn);
                transform2.GetComponent<UIButton>().isEnabled = isCanClickEquip;
                this.SetSingleEquipInfo(info);
            }
            this.mCanBreak = CommonFunc.CheckCardCanBreak(_card);
            if (!this.mIsOnleShow)
            {
                this._BreakFlashSprite.gameObject.SetActive(this.mCanBreak);
            }
        }
    }

    private void SetHeroDescPanel(Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                Transform transform = base.transform.FindChild("HeroDescPanel");
                transform.transform.FindChild("List/Group1/Desc").GetComponent<UILabel>().text = _config.describe;
                transform.transform.FindChild("List/Group2/Desc").GetComponent<UILabel>().text = this.GetCardFetterDesc(_config.name_type);
                UILabel component = transform.transform.FindChild("List/Group3/Desc").GetComponent<UILabel>();
                component.text = this.GetCardDesc(_card);
                transform.transform.FindChild("List/Group3/Border").GetComponent<UISprite>().height = component.height + 70;
            }
        }
    }

    private void SetItemDetails(item_config ic)
    {
        if (ic != null)
        {
            char[] separator = new char[] { '|' };
            string[] strArray = ic.drop_from_type.Split(separator);
            char[] chArray2 = new char[] { '|' };
            string[] strArray2 = ic.drop_parm_0.Split(chArray2);
            char[] chArray3 = new char[] { '|' };
            string[] strArray3 = ic.drop_parm_1.Split(chArray3);
            if (strArray.Length == strArray2.Length)
            {
                int index = 0;
                UIGrid component = this._FromPanel.transform.FindChild("List/Grid").GetComponent<UIGrid>();
                CommonFunc.DeleteChildItem(component.transform);
                float cellHeight = component.cellHeight;
                foreach (string str in strArray)
                {
                    if ((str != string.Empty) && (int.Parse(str) <= 11))
                    {
                        GameObject obj2 = UnityEngine.Object.Instantiate(this._SingleFromPrefab) as GameObject;
                        obj2.transform.parent = component.transform;
                        obj2.transform.localPosition = new Vector3(0f, -cellHeight * index, -0.1f);
                        obj2.transform.localScale = new Vector3(1f, 1f, 1f);
                        DropFromType type = (DropFromType) int.Parse(str);
                        int num4 = !(strArray2[index] == string.Empty) ? int.Parse(strArray2[index]) : 0;
                        int num5 = !(strArray3[index] == string.Empty) ? int.Parse(strArray3[index]) : 0;
                        Transform transform = obj2.transform.FindChild("Item");
                        this.SetDropInfo(transform.gameObject, type, num4, num5);
                        index++;
                    }
                }
                if (index <= 0)
                {
                    UILabel label = this._FromPanel.transform.FindChild("NullTips").GetComponent<UILabel>();
                    label.gameObject.SetActive(index == 0);
                    if (strArray[0] != string.Empty)
                    {
                        label.text = ConfigMgr.getInstance().GetWord(int.Parse(strArray[0]));
                    }
                }
            }
        }
    }

    private void SetOnlyReadEquipInfo(Card _card)
    {
        if (_card != null)
        {
            for (int i = 0; i < 6; i++)
            {
                Transform transform = this.mEquipGirdList[i];
                EquipInfo info = _card.equipInfo[i];
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
                if (_config != null)
                {
                    transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                    CommonFunc.SetEquipQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                    transform.FindChild("Up").gameObject.SetActive(false);
                    transform.FindChild("LevUpTips").gameObject.SetActive(false);
                    transform.GetComponent<UIButton>().isEnabled = false;
                    transform.FindChild("Level").GetComponent<UILabel>().text = string.Empty;
                }
            }
        }
    }

    private void SetPartCardInfo(Card _card, card_ex_config _cec)
    {
        <SetPartCardInfo>c__AnonStorey18D storeyd = new <SetPartCardInfo>c__AnonStorey18D {
            _cec = _cec
        };
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
        if (_config != null)
        {
            Transform transform = base.transform.FindChild("InfoPanel");
            if (this.FakeListIndex > 0)
            {
                FakeCharacter.GetInstance().DestroyCharater(this.FakeListIndex);
            }
            UITexture component = transform.FindChild("Hero/Character").GetComponent<UITexture>();
            this.FakeListIndex = this.CreateModel(_card, component);
            transform.FindChild("Hero/Name").GetComponent<UILabel>().text = GameConstant.C_Label[_card.cardInfo.quality] + CommonFunc.GetCardNameByQuality(_card.cardInfo.quality, _config.name);
            transform.FindChild("BaseInfo/BreakTips").GetComponent<UILabel>().text = string.Empty;
            transform.FindChild("BaseInfo/Exp").GetComponent<UILabel>().text = "0";
            transform.FindChild("BaseInfo/Power").GetComponent<UILabel>().text = "未知";
            transform.FindChild("BaseInfo/Level").GetComponent<UILabel>().text = "1";
            transform.FindChild("BreakBtn").GetComponent<UIButton>().isEnabled = false;
            transform.FindChild("AddStar/UpStarBtn").GetComponent<UIButton>().isEnabled = false;
            UILabel label2 = transform.FindChild("AddStar/Count").GetComponent<UILabel>();
            Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storeyd.<>m__1F5));
            int num = (item != null) ? item.num : 0;
            label2.text = num + "/" + storeyd._cec.combine_need_item_num;
            float num2 = ((float) num) / ((float) storeyd._cec.combine_need_item_num);
            if (num2 > 1f)
            {
                num2 = 1f;
            }
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            UISprite sprite = transform.FindChild("AddStar/Slider/Foreground").GetComponent<UISprite>();
            sprite.width = (int) (239f * num2);
            sprite.gameObject.SetActive(num2 > 0f);
            Transform transform2 = transform.FindChild("Hero");
            for (int i = 0; i < 5; i++)
            {
                UISprite sprite2 = transform2.transform.FindChild("Star/" + (i + 1)).GetComponent<UISprite>();
                UISprite sprite3 = transform2.transform.FindChild("Star/empty" + (i + 1)).GetComponent<UISprite>();
                sprite2.gameObject.SetActive(i < _card.cardInfo.starLv);
            }
        }
    }

    private void SetSingleEquipInfo(EquipInfo info)
    {
        if ((info.part >= 0) && (info.part < this.mEquipGirdList.Count))
        {
            Transform transform = this.mEquipGirdList[info.part];
            GUIDataHolder.setData(transform.gameObject, info);
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
            if (_config != null)
            {
                transform.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateItemIcon(_config.icon);
                CommonFunc.SetEquipQualityBorder(transform.FindChild("QualityBorder").GetComponent<UISprite>(), _config.quality, false);
                UISprite component = transform.FindChild("Up").GetComponent<UISprite>();
                transform.FindChild("LevUpTips").GetComponent<UISprite>().gameObject.SetActive(info.lv <= (this.mCard.cardInfo.level - 5));
                transform.FindChild("Level").GetComponent<UILabel>().text = info.lv.ToString();
                if (this.mIsOnleShow)
                {
                    component.gameObject.SetActive(false);
                }
                else if (_config.quality > (this.mCard.cardInfo.quality + 1))
                {
                    component.gameObject.SetActive(false);
                }
                else
                {
                    break_equip_config _config2 = ConfigMgr.getInstance().getByEntry<break_equip_config>(info.entry);
                    if (_config2 != null)
                    {
                        if (_config2.break_equip_entry < 0)
                        {
                            component.gameObject.SetActive(false);
                        }
                        else if (!this.mIsOnleShow)
                        {
                            TweenAlpha alpha = component.GetComponent<TweenAlpha>();
                            if (CommonFunc.CheckMaterialEnough(_config2.equip_entry) && (_config.quality < (this.mCard.cardInfo.quality + 1)))
                            {
                                if (CommonFunc.CheckBreakMaterialEnough(_config2.equip_entry) && (_config.quality < (this.mCard.cardInfo.quality + 1)))
                                {
                                    UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEquipUpBtn);
                                }
                                else
                                {
                                    UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickEquipAutoBreakBtn);
                                }
                                component.gameObject.SetActive(true);
                            }
                            else
                            {
                                component.gameObject.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        component.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void ShowCardInfo(int cardEntry)
    {
        Card card = new Card {
            card_id = -1L
        };
        card.cardInfo.entry = (uint) cardEntry;
        this.mIsOnleShow = true;
        this.mCard = card;
        Transform transform = base.transform.FindChild("InfoPanel");
        this.SetCardInfo(card, transform);
        this.SetHeroDescPanel(card);
        this.mIsCanClickLevUp = false;
    }

    public void ShowCardPartInfo(card_ex_config _cec)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>(_cec.card_entry);
        if (_config != null)
        {
            this.mIsCardPartShow = true;
            this.mHaveSkillCanLevUp = false;
            this.mIsCanClickLevUp = false;
            Card card = new Card {
                cardInfo = { entry = _config.entry, quality = 0, level = 1 },
                equipInfo = null
            };
            card.cardInfo.starLv = _cec.cur_card_star_lv;
            List<EquipInfo> list = new List<EquipInfo>();
            if (_config.equip_part_0 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_0, 0));
            }
            if (_config.equip_part_1 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_1, 1));
            }
            if (_config.equip_part_2 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_2, 2));
            }
            if (_config.equip_part_3 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_3, 3));
            }
            if (_config.equip_part_4 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_4, 4));
            }
            if (_config.equip_part_5 != -1)
            {
                list.Add(this.GetEquipInfo(_config.equip_part_5, 5));
            }
            card.equipInfo = list;
            List<SkillInfo> list2 = new List<SkillInfo>();
            if (_config.skill_0 != -1)
            {
                list2.Add(this.GetSkillInfoByIdx(0));
            }
            if (_config.skill_1 != -1)
            {
                list2.Add(this.GetSkillInfoByIdx(1));
            }
            if (_config.skill_2 != -1)
            {
                list2.Add(this.GetSkillInfoByIdx(2));
            }
            if (_config.skill_3 != -1)
            {
                list2.Add(this.GetSkillInfoByIdx(3));
            }
            card.cardInfo.skillInfo = list2;
            this.mCard = card;
            this.SetPartCardInfo(card, _cec);
            this.SetOnlyReadEquipInfo(card);
            this.SetHeroDescPanel(card);
            this.InitPartCardSkillList(card);
            this.InitFromList(card);
            this.mInitSkillOk = true;
            this.mInitFromOk = true;
            base.transform.FindChild("InfoPanel/GemBtn").gameObject.SetActive(false);
            base.transform.FindChild("Left").gameObject.SetActive(false);
            base.transform.FindChild("Right").gameObject.SetActive(false);
            base.transform.FindChild("LeveUpPanel/Point").gameObject.SetActive(false);
            this._SkillTabPoint.gameObject.SetActive(false);
        }
    }

    public void ShowFromTab()
    {
        this.SetCheckBoxStat(1, false);
        this.SetCheckBoxStat(2, false);
        this.SetCheckBoxStat(3, true);
        this.ShowTabPage(3);
    }

    private void ShowTabPage(int page)
    {
        this.ClearSkillEffect();
        this._SkillTips.gameObject.SetActive(false);
        this.mShowTabPageIdx = page;
        switch (page)
        {
            case 1:
                this._ShuXinPanel.SetActive(true);
                this._SkillPanel.SetActive(false);
                this._FromPanel.SetActive(false);
                break;

            case 2:
                this._ShuXinPanel.SetActive(false);
                this._FromPanel.SetActive(false);
                if (!this.mInitSkillOk)
                {
                    this.InitSkillList(this.mIsCanClickLevUp);
                    this.mInitSkillOk = true;
                }
                this._SkillPanel.SetActive(true);
                break;

            case 3:
                this._ShuXinPanel.SetActive(false);
                this._SkillPanel.SetActive(false);
                if (!this.mInitFromOk)
                {
                    this.InitFromList(this.mCard);
                    this.mInitFromOk = true;
                }
                this._FromPanel.SetActive(true);
                break;
        }
    }

    public bool SkillCanbeLevelup()
    {
        if (this.mCard == null)
        {
            return false;
        }
        return Utility.CheckSkillCanbeLevelup(this.mCard);
    }

    public bool SkillLevelupTabActived()
    {
        Transform transform = base.transform.FindChild("TabGroup/2");
        if (null == transform)
        {
            return false;
        }
        UIToggle component = transform.GetComponent<UIToggle>();
        if (null == component)
        {
            return false;
        }
        return component.isChecked;
    }

    private void UnsubscribeEvent()
    {
        EasyTouch.On_Swipe -= new EasyTouch.SwipeHandler(this.On_Swipe);
        EasyTouch.On_SimpleTap -= new EasyTouch.SimpleTapHandler(this.On_SimpleTap);
    }

    public void UpdateEquipBreakData(Card _cardInfo)
    {
        Card cardByEntry = ActorData.getInstance().GetCardByEntry(_cardInfo.cardInfo.entry);
        this.GetHeroAttributeData(cardByEntry, true);
        this.GetHeroAttributeData(_cardInfo, false);
        this.InitCardInfo(_cardInfo);
        this.UpdateShowCardByEntry(_cardInfo);
        BreakEquipPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BreakEquipPanel>();
        if (activityGUIEntity == null)
        {
        }
        activityGUIEntity.PopAddText(this.GetUpStarAddInfo(_cardInfo));
    }

    public void UpdateEquipInfo(EquipInfo info)
    {
        if (info != null)
        {
            this.SetSingleEquipInfo(info);
        }
    }

    public void UpdateEqupInfo(List<Card> _changeCard)
    {
        if (((this.mCardId > 0L) && (_changeCard.Count == 1)) && (_changeCard[0].card_id == this.mCardId))
        {
            this.SetEquipInfo(_changeCard[0], true);
        }
    }

    public void UpdateLockSkillInfo(GameObject obj, SkillInfo _Skill)
    {
        Card mCard = this.mCard;
        UISprite component = obj.transform.FindChild("TopBorder").GetComponent<UISprite>();
        obj.transform.FindChild("CellBg").GetComponent<UISprite>().color = (Color) new Color32(0xbd, 0xae, 0xa4, 0xff);
        component.color = (Color) new Color32(0xa2, 0x8b, 120, 0xff);
        obj.transform.FindChild("SkillLevel").GetComponent<UILabel>().text = string.Empty;
        UILabel label2 = obj.transform.FindChild("SkillName").GetComponent<UILabel>();
        obj.transform.FindChild("Cost").gameObject.SetActive(false);
        UILabel label3 = obj.transform.FindChild("LockTips").GetComponent<UILabel>();
        label3.gameObject.SetActive(true);
        switch (_Skill.skillPos)
        {
            case 1:
                label3.text = ConfigMgr.getInstance().GetWord(0xa652a7);
                break;

            case 2:
                label3.text = ConfigMgr.getInstance().GetWord(0xa652a8);
                break;

            case 3:
                label3.text = ConfigMgr.getInstance().GetWord(0xa652a9);
                break;

            default:
                Debug.LogWarning("未开放技能!");
                break;
        }
        UITexture texture = obj.transform.FindChild("SkillIcon/Icon").GetComponent<UITexture>();
        obj.transform.FindChild("LeveUpBtn").gameObject.SetActive(false);
        skill_config skillCfg = this.GetSkillCfg(mCard, _Skill);
        if (skillCfg != null)
        {
            label2.text = skillCfg.name;
            texture.mainTexture = BundleMgr.Instance.CreateSkillIcon(skillCfg.icon);
            nguiTextureGrey.doChangeEnableGrey(texture, true);
        }
    }

    public void UpdateOneKeyLevUp(Card _newCard, int _cost)
    {
        <UpdateOneKeyLevUp>c__AnonStorey18F storeyf = new <UpdateOneKeyLevUp>c__AnonStorey18F {
            _cost = _cost
        };
        this.DelayCallBack(2f, new System.Action(storeyf.<>m__1F7));
        List<string> allAddStr = this.GetAllAddStr(_newCard.equipInfo, this.mCard.equipInfo);
        this.mCard = _newCard;
        base.StartCoroutine(this.PushBreakText(allAddStr));
        this.InitCardInfo(this.mCard);
        TipsDiag.SetText("升级成功");
    }

    public void UpdateShowCardByEntry(Card _card)
    {
        if (this.mCurrShowCardList != null)
        {
            for (int i = 0; i < this.mCurrShowCardList.Count; i++)
            {
                if (this.mCurrShowCardList[i].cardInfo.entry == _card.cardInfo.entry)
                {
                    this.mCurrShowCardList[i] = _card;
                    break;
                }
            }
        }
    }

    private void UpdateSkillBtnState()
    {
        this.mHaveSkillCanLevUp = false;
        foreach (GameObject obj2 in this.LevUpBtnList)
        {
            SkillInfo info = (SkillInfo) GUIDataHolder.getData(obj2);
            int skillPos = info.skillPos;
            skill_config skillCfg = this.GetSkillCfg(this.mCard, info);
            int skillIDByCardEntry = CommonFunc.GetSkillIDByCardEntry((int) this.mCard.cardInfo.entry, (E_CardSkill) skillPos);
            if (!CommonFunc.CanLvUpSkill(this.mCard.cardInfo.quality, skillPos) || (ActorData.getInstance().GetSkillLev(this.mCard.card_id, skillPos) >= CommonFunc.GetMaxSkillLv(skillIDByCardEntry, this.mCard.cardInfo.level)))
            {
                obj2.GetComponent<UIButton>().isEnabled = false;
            }
            else
            {
                obj2.GetComponent<UIButton>().isEnabled = true;
                this.mHaveSkillCanLevUp = true;
            }
        }
    }

    public void UpdateSkillInfo(long card_id, int _SkillPos, FastBuf.SkillPoint _SkillPoint, int _SkillLv)
    {
        this.mSkillPoint = _SkillPoint;
        this.CurClickSkillObj.transform.FindChild("SkillLevel").GetComponent<UILabel>().text = "LV." + _SkillLv;
        UILabel component = this.CurClickSkillObj.transform.FindChild("Cost/CostNum").GetComponent<UILabel>();
        Card mCard = this.mCard;
        if (mCard == null)
        {
            Debug.LogWarning("Card IS null!");
        }
        else
        {
            SkillInfo skillInfo = this.GetSkillInfo(mCard, _SkillPos);
            if (skillInfo == null)
            {
                Debug.LogWarning("SkillInfo Is Null!");
            }
            else
            {
                skill_config skillCfg = this.GetSkillCfg(mCard, skillInfo);
                if (skillCfg != null)
                {
                    component.text = CommonFunc.GetSkillLevUpCost(_SkillPos, skillInfo.skillLevel).ToString();
                    this.UpdateSkillPoint(false);
                    this.UpdateSkillBtnState();
                    base.StartCoroutine(this.PlaySkillEffect(skillCfg));
                    this.SetHeroDescPanel(mCard);
                }
            }
        }
    }

    public void UpdateSkillPoint(bool _RefreshTime)
    {
        this.mSkillPoint = ActorData.getInstance().CurSkillPoint;
        if ((this.mSkillPoint != null) && ((this.mSkillPointLabel != null) && (this.mBuyBtn != null)))
        {
            this.NextGetSkillTime = ActorData.getInstance().NextGetSkillTime;
            if (this.mSkillPoint.totalPoint >= this.mSkillPoint.maxPoint)
            {
                this._SkillTabPoint.gameObject.SetActive(this.mHaveSkillCanLevUp);
                this.mBuyBtn.gameObject.SetActive(false);
                this.mSkillPointLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652af), this.mSkillPoint.totalPoint);
            }
            else if (((this.NextGetSkillTime == 0) && (this.mSkillPoint.totalPoint < this.mSkillPoint.maxPoint)) && (this.mSkillPoint.totalPoint >= this.mSkillPoint.maxPoint))
            {
                this.mBuyBtn.gameObject.SetActive(false);
                this.mSkillPointLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652af), this.mSkillPoint.totalPoint);
            }
            else
            {
                TimeSpan span = new TimeSpan(0, 0, this.NextGetSkillTime);
                string str = string.Format(ConfigMgr.getInstance().GetWord(0x67), span.Minutes, span.Seconds);
                if (this.mSkillPoint.totalPoint == 0)
                {
                    this.mSkillPointLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652ae), str);
                    this.mBuyBtn.gameObject.SetActive(true);
                }
                else
                {
                    this.mSkillPointLabel.text = string.Format(ConfigMgr.getInstance().GetWord(0xa652b0), this.mSkillPoint.totalPoint, str);
                    this.mBuyBtn.gameObject.SetActive(false);
                }
                this._SkillTabPoint.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateUnLockSkillInfo(GameObject obj, SkillInfo _Skill)
    {
        Card mCard = this.mCard;
        UILabel component = obj.transform.FindChild("SkillLevel").GetComponent<UILabel>();
        UILabel label2 = obj.transform.FindChild("SkillName").GetComponent<UILabel>();
        UILabel label3 = obj.transform.FindChild("Cost/CostNum").GetComponent<UILabel>();
        UISprite sprite = obj.transform.FindChild("TopBorder").GetComponent<UISprite>();
        obj.transform.FindChild("CellBg").GetComponent<UISprite>().color = (Color) new Color32(0xff, 0xff, 0xff, 0xff);
        sprite.color = (Color) new Color32(0xdb, 0xd1, 0xbd, 0xff);
        obj.transform.FindChild("LockTips").GetComponent<UILabel>().gameObject.SetActive(false);
        obj.transform.FindChild("Cost").gameObject.SetActive(true);
        UITexture texture = obj.transform.FindChild("SkillIcon/Icon").GetComponent<UITexture>();
        component.text = "Lv." + this.GetSkillLv(mCard, _Skill.skillPos);
        skill_config skillCfg = this.GetSkillCfg(mCard, _Skill);
        if (skillCfg != null)
        {
            label3.text = CommonFunc.GetSkillLevUpCost(_Skill.skillPos, _Skill.skillLevel).ToString();
            label2.text = skillCfg.name;
            texture.mainTexture = BundleMgr.Instance.CreateSkillIcon(skillCfg.icon);
            nguiTextureGrey.doChangeEnableGrey(texture, false);
        }
    }

    public void UpStarSucess(Card _cardInfo)
    {
        GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
        this.GetHeroAttributeData(this.mCard, true);
        this.GetHeroAttributeData(_cardInfo, false);
        this.UpdateShowCardByEntry(_cardInfo);
        this.InitCardInfo(_cardInfo);
        base.StartCoroutine(this.PushText(this.GetUpStarAddInfo(_cardInfo)));
    }

    public FastBuf.SkillPoint SkillPoint
    {
        set
        {
            this.mSkillPoint = value;
            this.NextGetSkillTime = 360 - this.mSkillPoint.passTime;
            this.UpdateSkillPoint(true);
        }
    }

    [CompilerGenerated]
    private sealed class <CardBreakSuccess>c__AnonStorey197
    {
        internal Card _cardInfo;
        internal HeroInfoPanel <>f__this;

        internal void <>m__1FE(GUIEntity obj)
        {
            CardBreakSuccessPanel panel = (CardBreakSuccessPanel) obj;
            panel.Depth = 600;
            panel.UpdateData(this._cardInfo, (float) this.<>f__this.mOldHeroAttributeDict[HeroAttributeType.Agility], (float) this.<>f__this.mNewHeroAttributeDict[HeroAttributeType.Agility], (float) this.<>f__this.mOldHeroAttributeDict[HeroAttributeType.Stamina], (float) this.<>f__this.mNewHeroAttributeDict[HeroAttributeType.Stamina], (float) this.<>f__this.mOldHeroAttributeDict[HeroAttributeType.Intelligence], (float) this.<>f__this.mNewHeroAttributeDict[HeroAttributeType.Intelligence], (float) this.<>f__this.mOldHeroAttributeDict[HeroAttributeType.Strength], (float) this.<>f__this.mNewHeroAttributeDict[HeroAttributeType.Strength]);
        }
    }

    [CompilerGenerated]
    private sealed class <JumpToDuplicate>c__AnonStorey18E
    {
        internal HeroInfoPanel <>f__this;
        internal MapData info;
        internal bool IsCardPartShow;

        internal void <>m__1F6(GUIEntity guiE)
        {
            DupLevInfoPanel panel = guiE.Achieve<DupLevInfoPanel>();
            panel.OpenTypeIsPush = true;
            ActorData.getInstance().CurDupEntry = this.info.entry;
            ActorData.getInstance().CurTrenchEntry = this.info.subEntry;
            ActorData.getInstance().CurDupType = this.info.type;
            panel.UpdateData(this.info.entry, this.info.subEntry, this.info.type);
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.<>f__this.mCard.cardInfo.entry);
            if (_config != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config.entry, _config.evolve_lv);
                if (cardExCfg != null)
                {
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(cardExCfg.item_entry);
                    if (_config3 != null)
                    {
                        DupReturnPrePanelPara para = new DupReturnPrePanelPara {
                            enterDuptype = EnterDupType.From_HeroInfoPanel,
                            heroInfoPanelCardInfo = this.<>f__this.mCard
                        };
                        if (this.<>f__this.mCurrShowCardList != null)
                        {
                            para.heroInfoShowCardList = this.<>f__this.mCurrShowCardList;
                        }
                        para.heroPanelPartEntry = _config3.entry;
                        para.mIsCardPartInfo = this.IsCardPartShow;
                        ActorData.getInstance().mCurrDupReturnPrePara = para;
                    }
                    Debug.Log("########################################");
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyPoint>c__AnonStorey198
    {
        internal HeroInfoPanel <>f__this;
        internal int Cost;

        internal void <>m__1FF(GUIEntity obj)
        {
            ((MessageBox) obj).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa652b1), this.Cost, 10, this.<>f__this.mSkillPoint.buyCount), delegate (GameObject box) {
                this.<>f__this.OpenMsgBox = false;
                if (ActorData.getInstance().Stone <= this.Cost)
                {
                    <OnClickBuyPoint>c__AnonStorey199 storey = new <OnClickBuyPoint>c__AnonStorey199 {
                        title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                    };
                    GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__20C), null);
                }
                else
                {
                    SocketMgr.Instance.RequestBuySkillPoint();
                }
            }, go => this.<>f__this.OpenMsgBox = false, false);
        }

        internal void <>m__20A(GameObject box)
        {
            this.<>f__this.OpenMsgBox = false;
            if (ActorData.getInstance().Stone <= this.Cost)
            {
                <OnClickBuyPoint>c__AnonStorey199 storey = new <OnClickBuyPoint>c__AnonStorey199 {
                    title = string.Format(ConfigMgr.getInstance().GetWord(0x9d2abe), new object[0])
                };
                GUIMgr.Instance.DoModelGUI("MessageBox", new Action<GUIEntity>(storey.<>m__20C), null);
            }
            else
            {
                SocketMgr.Instance.RequestBuySkillPoint();
            }
        }

        internal void <>m__20B(GameObject go)
        {
            this.<>f__this.OpenMsgBox = false;
        }

        private sealed class <OnClickBuyPoint>c__AnonStorey199
        {
            private static UIEventListener.VoidDelegate <>f__am$cache1;
            internal string title;

            internal void <>m__20C(GUIEntity e)
            {
                MessageBox box = e as MessageBox;
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = _go => GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
                }
                e.Achieve<MessageBox>().SetDialog(this.title, <>f__am$cache1, null, false);
            }

            private static void <>m__20D(GameObject _go)
            {
                GUIMgr.Instance.PushGUIEntity("VipCardPanel", null);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickCardBreak>c__AnonStorey194
    {
        internal HeroInfoPanel <>f__this;
        internal break_card_config bcc;

        internal void <>m__1FB(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x7d5), this.bcc.cost_gold), delegate (GameObject box) {
                if (GuideSystem.MatchEvent(GuideEvent.CardBreak_Function))
                {
                    GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_CardBreak.tag_cardbreak_confirm, null);
                }
                SocketMgr.Instance.RequestCardBreak(this.<>f__this.mCardId);
            }, null, false);
            if (GuideSystem.MatchEvent(GuideEvent.CardBreak_Function))
            {
                Transform transform = box.transform.FindChild("OkBtn");
                if (null != transform)
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_CardBreak.tag_cardbreak_confirm, transform.gameObject);
                }
            }
        }

        internal void <>m__207(GameObject box)
        {
            if (GuideSystem.MatchEvent(GuideEvent.CardBreak_Function))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_CardBreak.tag_cardbreak_confirm, null);
            }
            SocketMgr.Instance.RequestCardBreak(this.<>f__this.mCardId);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickCardUpStar>c__AnonStorey195
    {
        internal HeroInfoPanel <>f__this;
        internal card_ex_config cec;

        internal void <>m__1FC(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            box.MultiLayered = true;
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0x14e), this.cec.evolve_need_gold), delegate (GameObject go) {
                if (ActorData.getInstance().Gold < this.cec.evolve_need_gold)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                }
                else
                {
                    SocketMgr.Instance.RequestCardEvolve(this.<>f__this.mCardId);
                    this.<>f__this.mUpStarOk = false;
                }
            }, null, false);
        }

        internal void <>m__209(GameObject go)
        {
            if (ActorData.getInstance().Gold < this.cec.evolve_need_gold)
            {
                TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
            }
            else
            {
                SocketMgr.Instance.RequestCardEvolve(this.<>f__this.mCardId);
                this.<>f__this.mUpStarOk = false;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickEquipAutoBreakBtn>c__AnonStorey190
    {
        internal HeroInfoPanel <>f__this;
        internal int allCost;
        internal GameObject go;
        internal EquipInfo info;

        internal void <>m__1F8(GUIEntity obj)
        {
            ((MessageBox) obj).SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa037e0), this.allCost), box => SocketMgr.Instance.RequestEquipBreakQuick(this.<>f__this.mCardId, int.Parse(this.go.name)), box1 => GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", delegate (GUIEntity e) {
                BreakEquipPanel panel = (BreakEquipPanel) e;
                panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
                panel.OpenJingHuaPanel();
                panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
            }), false);
        }

        internal void <>m__201(GameObject box)
        {
            SocketMgr.Instance.RequestEquipBreakQuick(this.<>f__this.mCardId, int.Parse(this.go.name));
        }

        internal void <>m__202(GameObject box1)
        {
            GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", delegate (GUIEntity e) {
                BreakEquipPanel panel = (BreakEquipPanel) e;
                panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
                panel.OpenJingHuaPanel();
                panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
            });
        }

        internal void <>m__203(GUIEntity e)
        {
            BreakEquipPanel panel = (BreakEquipPanel) e;
            panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
            panel.OpenJingHuaPanel();
            panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickEquipBtn>c__AnonStorey192
    {
        internal HeroInfoPanel <>f__this;
        internal bool canLevelUp;
        internal int partIdx;
    }

    [CompilerGenerated]
    private sealed class <OnClickEquipBtn>c__AnonStorey193
    {
        internal HeroInfoPanel.<OnClickEquipBtn>c__AnonStorey192 <>f__ref$402;
        internal HeroInfoPanel <>f__this;
        internal EquipInfo info;

        internal void <>m__1FA(GUIEntity obj)
        {
            BreakEquipPanel panel = (BreakEquipPanel) obj;
            if (panel != null)
            {
                panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
                if (this.<>f__ref$402.canLevelUp)
                {
                    panel.OpenJingHuaPanel();
                }
                panel.UpdateData(this.<>f__this.mCard, this.<>f__ref$402.partIdx, this.info);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickEquipUpBtn>c__AnonStorey191
    {
        internal HeroInfoPanel <>f__this;
        internal break_equip_config bec;
        internal GameObject go;
        internal EquipInfo info;

        internal void <>m__1F9(GUIEntity obj)
        {
            MessageBox box = (MessageBox) obj;
            box.SetDialog(string.Format(ConfigMgr.getInstance().GetWord(0xa037df), this.bec.cost_gold), delegate (GameObject box) {
                bool flag = false;
                if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Function))
                {
                    GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Equip.tag_equip_levelup_confirm, null);
                    flag = true;
                }
                item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.info.entry);
                if (_config != null)
                {
                    if (_config.quality > (this.<>f__this.mCard.cardInfo.quality + 1))
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287f));
                    }
                    else if (ActorData.getInstance().Gold < this.bec.cost_gold)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                    }
                    else if (this.bec.break_equip_entry < 0)
                    {
                        TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287c));
                    }
                    else
                    {
                        SocketMgr.Instance.RequestEquipBreak(this.<>f__this.mCardId, int.Parse(this.go.name));
                        if (flag)
                        {
                            SocketMgr.Instance.RequestGetQuestList();
                        }
                    }
                }
            }, box1 => GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", delegate (GUIEntity e) {
                BreakEquipPanel panel = (BreakEquipPanel) e;
                panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
                panel.OpenJingHuaPanel();
                panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
            }), false);
            if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Function))
            {
                Transform transform = box.transform.FindChild("OkBtn");
                if (null != transform)
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Equip.tag_equip_levelup_confirm, transform.gameObject);
                }
            }
        }

        internal void <>m__204(GameObject box)
        {
            bool flag = false;
            if (GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_Function))
            {
                GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Equip.tag_equip_levelup_confirm, null);
                flag = true;
            }
            item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(this.info.entry);
            if (_config != null)
            {
                if (_config.quality > (this.<>f__this.mCard.cardInfo.quality + 1))
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287f));
                }
                else if (ActorData.getInstance().Gold < this.bec.cost_gold)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9d2a63));
                }
                else if (this.bec.break_equip_entry < 0)
                {
                    TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x287c));
                }
                else
                {
                    SocketMgr.Instance.RequestEquipBreak(this.<>f__this.mCardId, int.Parse(this.go.name));
                    if (flag)
                    {
                        SocketMgr.Instance.RequestGetQuestList();
                    }
                }
            }
        }

        internal void <>m__205(GameObject box1)
        {
            GUIMgr.Instance.PushGUIEntity("BreakEquipPanel", delegate (GUIEntity e) {
                BreakEquipPanel panel = (BreakEquipPanel) e;
                panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
                panel.OpenJingHuaPanel();
                panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
            });
        }

        internal void <>m__206(GUIEntity e)
        {
            BreakEquipPanel panel = (BreakEquipPanel) e;
            panel.SetCurrShowCardList(this.<>f__this.mCurrShowCardList);
            panel.OpenJingHuaPanel();
            panel.UpdateData(this.<>f__this.mCard, int.Parse(this.go.name), this.info);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickFromBtn>c__AnonStorey18C
    {
        internal HeroInfoPanel <>f__this;
        internal Card info;

        internal void <>m__1F4(GUIEntity obj)
        {
            DetailsPanel panel = (DetailsPanel) obj;
            panel.Depth = 400;
            panel.InitList(this.info);
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) this.info.cardInfo.entry);
            if (_config != null)
            {
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(_config.entry, _config.evolve_lv);
                if (cardExCfg != null)
                {
                    item_config _config3 = ConfigMgr.getInstance().getByEntry<item_config>(cardExCfg.item_entry);
                    if (_config3 != null)
                    {
                        DupReturnPrePanelPara para = new DupReturnPrePanelPara {
                            enterDuptype = EnterDupType.From_HeroInfoPanel,
                            heroInfoPanelCardInfo = this.info
                        };
                        if (this.<>f__this.mCurrShowCardList != null)
                        {
                            para.heroInfoShowCardList = this.<>f__this.mCurrShowCardList;
                        }
                        para.heroPanelPartEntry = _config3.entry;
                        ActorData.getInstance().mCurrDupReturnPrePara = para;
                    }
                }
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickJiYouBtn>c__AnonStorey18B
    {
        internal card_config cc;

        internal void <>m__1F3(GUIEntity entity)
        {
            InfoDlag dlag = (InfoDlag) entity;
            ArrayList list = ConfigMgr.getInstance().getList<fetter_config>();
            string str = string.Empty;
            IEnumerator enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    fetter_config current = (fetter_config) enumerator.Current;
                    if (current.card_name_type == this.cc.name_type)
                    {
                        string str2 = str;
                        string[] textArray1 = new string[] { str2, "[ff0000]", current.name, ":[503d2e]", current.desc, "\n" };
                        str = string.Concat(textArray1);
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
            dlag.SetInfoCenterContent2(str);
        }
    }

    [CompilerGenerated]
    private sealed class <PlaySkillEffect>c__Iterator60 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal skill_config <$>skillCfg;
        internal string[] <$s_485>__3;
        internal int <$s_486>__4;
        internal string[] <_infoList>__2;
        internal HeroInfoPanel <>f__this;
        internal GameObject <EffObj>__1;
        internal GameObject <objRoot>__0;
        internal string <text>__5;
        internal skill_config skillCfg;

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
                {
                    this.<objRoot>__0 = this.<>f__this.CurClickSkillObj.transform.FindChild("SkillIcon").gameObject;
                    this.<EffObj>__1 = UnityEngine.Object.Instantiate(this.<>f__this.SkillLevUp) as GameObject;
                    this.<EffObj>__1.transform.parent = this.<objRoot>__0.transform;
                    this.<EffObj>__1.transform.localPosition = Vector3.zero;
                    this.<EffObj>__1.transform.localScale = Vector3.one;
                    this.<>f__this.EffectList.Enqueue(this.<EffObj>__1);
                    char[] separator = new char[] { '|' };
                    this.<_infoList>__2 = this.skillCfg.lv_up_desc.Split(separator);
                    this.<$s_485>__3 = this.<_infoList>__2;
                    this.<$s_486>__4 = 0;
                    while (this.<$s_486>__4 < this.<$s_485>__3.Length)
                    {
                        this.<text>__5 = this.<$s_485>__3[this.<$s_486>__4];
                        if (this.<text>__5 != ConfigMgr.getInstance().GetWord(6))
                        {
                            this.<>f__this.TipsMsgList.Enqueue(this.<text>__5);
                        }
                        this.<$s_486>__4++;
                    }
                    this.$current = new WaitForSeconds(2.5f);
                    this.$PC = 1;
                    return true;
                }
                case 1:
                    this.<>f__this.ClearSkillEffect();
                    this.$PC = -1;
                    break;
            }
            return false;
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
    private sealed class <PushBreakText>c__Iterator5E : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<string> _infoList;
        internal List<string> <$>_infoList;
        internal List<string>.Enumerator <$s_482>__0;
        internal string <text>__1;

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_482>__0.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    Debug.Log(this._infoList.Count + "--------------");
                    this.<$s_482>__0 = this._infoList.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_00E9;

                default:
                    goto Label_00E9;
            }
            try
            {
                while (this.<$s_482>__0.MoveNext())
                {
                    this.<text>__1 = this.<$s_482>__0.Current;
                    TipsDiag.PushText(this.<text>__1);
                    this.$current = new WaitForSeconds(0.25f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_00EB;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_482>__0.Dispose();
            }
            this.$current = null;
            this.$PC = 2;
            goto Label_00EB;
        Label_00E9:
            return false;
        Label_00EB:
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
    private sealed class <PushText>c__Iterator5F : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal List<string> _infoList;
        internal List<string> <$>_infoList;
        internal List<string>.Enumerator <$s_483>__0;
        internal HeroInfoPanel <>f__this;
        internal string <text>__1;

        internal void <>m__208(GUIEntity entity)
        {
            HeroUpStarPanel panel = (HeroUpStarPanel) entity;
            panel.Depth = 500;
            panel.UpdateData(this.<>f__this.mCard);
            this.<>f__this.mUpStarOk = true;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 1:
                    try
                    {
                    }
                    finally
                    {
                        this.<$s_483>__0.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    GUIMgr.Instance.Lock();
                    this.<$s_483>__0 = this._infoList.GetEnumerator();
                    num = 0xfffffffd;
                    break;

                case 1:
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_00FA;

                default:
                    goto Label_00FA;
            }
            try
            {
                while (this.<$s_483>__0.MoveNext())
                {
                    this.<text>__1 = this.<$s_483>__0.Current;
                    TipsDiag.PushText(this.<text>__1);
                    this.$current = new WaitForSeconds(0.25f);
                    this.$PC = 1;
                    flag = true;
                    goto Label_00FC;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                this.<$s_483>__0.Dispose();
            }
            GUIMgr.Instance.UnLock();
            GUIMgr.Instance.DoModelGUI("HeroUpStarPanel", new Action<GUIEntity>(this.<>m__208), null);
            this.$current = null;
            this.$PC = 2;
            goto Label_00FC;
        Label_00FA:
            return false;
        Label_00FC:
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
    private sealed class <SetCardInfo>c__AnonStorey196
    {
        internal card_ex_config cec;

        internal bool <>m__1FD(Item e)
        {
            return (e.entry == this.cec.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <SetChipInfo>c__AnonStorey18A
    {
        internal card_ex_config cec;

        internal bool <>m__1F1(Item e)
        {
            return (e.entry == this.cec.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <SetPartCardInfo>c__AnonStorey18D
    {
        internal card_ex_config _cec;

        internal bool <>m__1F5(Item e)
        {
            return (e.entry == this._cec.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <UpdateOneKeyLevUp>c__AnonStorey18F
    {
        internal int _cost;

        internal void <>m__1F7()
        {
            TipsDiag.SetText(string.Format(ConfigMgr.getInstance().GetWord(0x9d2a61), this._cost));
        }
    }
}

