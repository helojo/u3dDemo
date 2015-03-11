using FastBuf;
using System;
using UnityEngine;

public class HeroUpStarPanel : GUIEntity
{
    private void SetCardInfo(Transform obj, card_config cc, int _starLv, int _level, int _Quality)
    {
        obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(cc.image);
        CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), _Quality);
        obj.transform.FindChild("Level").GetComponent<UILabel>().text = _level.ToString();
        Transform transform = obj.transform.FindChild("Star");
        for (int i = 0; i < 5; i++)
        {
            UISprite component = transform.FindChild(string.Empty + (i + 1)).GetComponent<UISprite>();
            component.gameObject.SetActive(i < _starLv);
            component.transform.localPosition = new Vector3((float) (i * 0x16), 0f, 0f);
        }
        transform.localPosition = new Vector3(-113f - ((_starLv - 1) * 11f), transform.localPosition.y, 0f);
    }

    private void SetUpStarDesc(Card _card)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            if (_config != null)
            {
                int starLv = _card.cardInfo.starLv;
                int level = _card.cardInfo.level;
                int quality = _config.quality;
                int entry = _config.entry;
                card_ex_config cardExCfg = CommonFunc.GetCardExCfg(entry, starLv);
                if (cardExCfg != null)
                {
                    card_ex_config _config3 = CommonFunc.GetCardExCfg(entry, starLv - 1);
                    if (_config3 != null)
                    {
                        float num5 = (_config.stamina + ((level - 1) * cardExCfg.stamina_grow)) + CommonFunc.GetBreakCardStamina(entry, quality);
                        float num6 = (_config.agility + ((level - 1) * cardExCfg.agility_grow)) + CommonFunc.GetBreakCardAgility(entry, quality);
                        float num7 = (_config.intelligence + ((level - 1) * cardExCfg.intelligence_grow)) + CommonFunc.GetBreakCardIntelligence(entry, quality);
                        float num8 = (_config.strength + ((level - 1) * cardExCfg.strength_grow)) + CommonFunc.GetBreakCardStrength(entry, quality);
                        Debug.Log(cardExCfg.strength_grow + "    " + _config3.strength_grow);
                        UILabel component = base.transform.FindChild("InfoGroup/Add1").GetComponent<UILabel>();
                        UILabel label2 = base.transform.FindChild("InfoGroup/Add2").GetComponent<UILabel>();
                        UILabel label3 = base.transform.FindChild("InfoGroup/Add3").GetComponent<UILabel>();
                        UILabel label4 = base.transform.FindChild("InfoGroup/Add4").GetComponent<UILabel>();
                        if (level > 1)
                        {
                            string[] textArray1 = new string[] { "[503d2e]( ", ConfigMgr.getInstance().GetWord(0x156), " +", string.Format("{0:N2}", (cardExCfg.strength_grow - _config3.strength_grow) * (level - 1)).ToString(), " )" };
                            component.text = string.Concat(textArray1);
                            string[] textArray2 = new string[] { "[503d2e]( ", ConfigMgr.getInstance().GetWord(0x157), " +", string.Format("{0:N2}", (cardExCfg.agility_grow - _config3.agility_grow) * (level - 1)).ToString(), " )" };
                            label2.text = string.Concat(textArray2);
                            string[] textArray3 = new string[] { "[503d2e]( ", ConfigMgr.getInstance().GetWord(0x158), " +", string.Format("{0:N2}", (cardExCfg.intelligence_grow - _config3.intelligence_grow) * (level - 1)).ToString(), " )" };
                            label3.text = string.Concat(textArray3);
                            string[] textArray4 = new string[] { "[503d2e]( ", ConfigMgr.getInstance().GetWord(0x159), " +", string.Format("{0:N2}", (cardExCfg.stamina_grow - _config3.stamina_grow) * (level - 1)).ToString(), " )" };
                            label4.text = string.Concat(textArray4);
                        }
                        else
                        {
                            component.text = string.Empty;
                            label2.text = string.Empty;
                            label3.text = string.Empty;
                            label4.text = string.Empty;
                        }
                        UILabel label5 = base.transform.FindChild("InfoGroup/Before1").GetComponent<UILabel>();
                        UILabel label6 = base.transform.FindChild("InfoGroup/Before2").GetComponent<UILabel>();
                        UILabel label7 = base.transform.FindChild("InfoGroup/Before3").GetComponent<UILabel>();
                        UILabel label8 = base.transform.FindChild("InfoGroup/Before4").GetComponent<UILabel>();
                        UILabel label9 = base.transform.FindChild("InfoGroup/After1").GetComponent<UILabel>();
                        UILabel label10 = base.transform.FindChild("InfoGroup/After2").GetComponent<UILabel>();
                        UILabel label11 = base.transform.FindChild("InfoGroup/After3").GetComponent<UILabel>();
                        UILabel label12 = base.transform.FindChild("InfoGroup/After4").GetComponent<UILabel>();
                        label5.text = _config3.strength_grow.ToString();
                        label6.text = _config3.agility_grow.ToString();
                        label7.text = _config3.intelligence_grow.ToString();
                        label8.text = _config3.stamina_grow.ToString();
                        label9.text = string.Format("{0:N2}", cardExCfg.strength_grow);
                        label10.text = string.Format("{0:N2}", cardExCfg.agility_grow);
                        label11.text = string.Format("{0:N2}", cardExCfg.intelligence_grow);
                        label12.text = string.Format("{0:N2}", cardExCfg.stamina_grow);
                    }
                }
            }
        }
    }

    public void UpdateData(Card info)
    {
        card_config cc = ConfigMgr.getInstance().getByEntry<card_config>((int) info.cardInfo.entry);
        if (cc != null)
        {
            this.SetCardInfo(base.transform.FindChild("Pre"), cc, info.cardInfo.starLv - 1, info.cardInfo.level, info.cardInfo.quality);
            this.SetCardInfo(base.transform.FindChild("Curr"), cc, info.cardInfo.starLv, info.cardInfo.level, info.cardInfo.quality);
            this.SetUpStarDesc(info);
        }
    }
}

