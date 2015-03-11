using FastBuf;
using System;
using UnityEngine;

public class CardBreakSuccessPanel : GUIEntity
{
    private Card mCard;
    private bool mPopPurpleCardShare;

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    private void SetCardInfo(Transform obj, Card _card, bool isNew)
    {
        if (_card != null)
        {
            card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _card.cardInfo.entry);
            obj.FindChild("Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            CommonFunc.SetQualityBorder(obj.FindChild("QualityBorder").GetComponent<UISprite>(), !isNew ? (_card.cardInfo.quality - 1) : _card.cardInfo.quality);
            obj.transform.FindChild("Level").GetComponent<UILabel>().text = _card.cardInfo.level.ToString();
            Transform transform = obj.transform.FindChild("Star");
            float x = 0f;
            int starLv = _card.cardInfo.starLv;
            for (int i = 0; i < 5; i++)
            {
                int num5 = i + 1;
                UISprite component = transform.FindChild(num5.ToString()).GetComponent<UISprite>();
                component.transform.localPosition = new Vector3(x, 0f, 0f);
                x += 26f;
                component.gameObject.SetActive(i < starLv);
            }
            float num4 = (transform.transform.localPosition.x - (((starLv - 1) * 0x1a) / 2)) - 9f;
            if (starLv > 1)
            {
                num4 += 15f;
            }
            transform.transform.localPosition = new Vector3(num4, transform.transform.localPosition.y, transform.transform.localPosition.z);
        }
    }

    private void SetNewSillInfo(Card _newCard)
    {
        int num = 1;
        if (_newCard.cardInfo.quality == 2)
        {
            num = 2;
        }
        else if (_newCard.cardInfo.quality == 4)
        {
            num = 3;
        }
        skill_config skillCfg = CommonFunc.GetSkillCfg((int) _newCard.cardInfo.entry, num);
        if (skillCfg != null)
        {
            Transform transform = base.transform.FindChild("Group/NewSkill");
            transform.FindChild("Name").GetComponent<UILabel>().text = skillCfg.name;
            transform.FindChild("SkillIcon/Icon").GetComponent<UITexture>().mainTexture = BundleMgr.Instance.CreateSkillIcon(skillCfg.icon);
            transform.FindChild("Desc").GetComponent<UILabel>().text = skillCfg.desc;
            transform.gameObject.SetActive(true);
        }
    }

    private void SetPanelPostion(bool showNewSkill)
    {
        base.transform.FindChild("Group/Bg1").GetComponent<UISprite>().height = !showNewSkill ? 0x175 : 0x218;
        base.transform.FindChild("Group/Bg").GetComponent<UISprite>().height = !showNewSkill ? 0x185 : 550;
        base.transform.FindChild("Group").transform.localPosition = !showNewSkill ? new Vector3(0f, -80f, 0f) : Vector3.zero;
    }

    public void UpdateData(Card _newCard, float oldAgility, float newAgility, float oldStamina, float newStamina, float oldIntelligence, float newIntelligence, float oldStrength, float newStrength)
    {
        if (_newCard != null)
        {
            this.mCard = _newCard;
            if (_newCard.cardInfo.quality >= 4)
            {
                this.mPopPurpleCardShare = true;
                if (SharePanel.IsCanShare())
                {
                    GUIMgr.Instance.DoModelGUI("PushNotifyPanel", entity => (entity as PushNotifyPanel).UpdateData(PushNotifyPanel.ShareType.CardBreakToPurple, this.mCard, null), null);
                }
            }
            bool showNewSkill = ((_newCard.cardInfo.quality == 1) || (_newCard.cardInfo.quality == 2)) || (_newCard.cardInfo.quality == 4);
            this.SetPanelPostion(showNewSkill);
            if (showNewSkill)
            {
                this.SetNewSillInfo(_newCard);
            }
            this.SetCardInfo(base.transform.FindChild("Group/Pre"), _newCard, false);
            this.SetCardInfo(base.transform.FindChild("Group/Curr"), _newCard, true);
            Transform transform = base.transform.FindChild("Group/InfoGroup");
            transform.FindChild("Before1").GetComponent<UILabel>().text = string.Format("{0:N2}", oldStrength);
            transform.FindChild("Before2").GetComponent<UILabel>().text = string.Format("{0:N2}", oldAgility);
            transform.FindChild("Before3").GetComponent<UILabel>().text = string.Format("{0:N2}", oldIntelligence);
            transform.FindChild("Before4").GetComponent<UILabel>().text = string.Format("{0:N2}", oldStamina);
            transform.FindChild("After1").GetComponent<UILabel>().text = string.Format("{0:N2}", newStrength);
            transform.FindChild("After2").GetComponent<UILabel>().text = string.Format("{0:N2}", newAgility);
            transform.FindChild("After3").GetComponent<UILabel>().text = string.Format("{0:N2}", newIntelligence);
            transform.FindChild("After4").GetComponent<UILabel>().text = string.Format("{0:N2}", newStamina);
            transform.FindChild("Add1").GetComponent<UILabel>().text = "+" + string.Format("{0:N2}", newStrength - oldStrength);
            transform.FindChild("Add2").GetComponent<UILabel>().text = "+" + string.Format("{0:N2}", newAgility - oldAgility);
            transform.FindChild("Add3").GetComponent<UILabel>().text = "+" + string.Format("{0:N2}", newIntelligence - oldIntelligence);
            transform.FindChild("Add4").GetComponent<UILabel>().text = "+" + string.Format("{0:N2}", newStamina - oldStamina);
        }
    }
}

