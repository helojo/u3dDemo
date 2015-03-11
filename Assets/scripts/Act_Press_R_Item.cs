using FastBuf;
using System;
using UnityEngine;

public class Act_Press_R_Item : MonoBehaviour
{
    public bool bItemCollect;
    private const string CARD_QUALITY_ATLAS = "CardQuality";
    public int cardStar;
    private string curAtlasQualityName = string.Empty;
    public LeftCntState curPressentLeftState = LeftCntState.Other;
    private const string EQUIP_QUALITY_ATLAS = "EquipQuality";
    private item_config ic;
    public string IconId;
    public UITexture IconTexcture;
    private const string ITEM_QUALITY_ATLAS = "";
    public string itemDescribe = "永恒的黄金纺成的细线\n[228400]（装备进化材料）";
    public Texture2D itemIconTex;
    public string itemId;
    public string itemName = "恒金线";
    public int itemNum;
    public int itemType;
    public GameObject kapian;
    public UISprite leftNullSprite;
    public UILabel nameLabel;
    public UILabel num;
    public PressItem PressItemTemp;
    public int quality;
    public UISprite qualityUIS;
    public UILabel shouXin;
    public UITexture spriteQuality_new;
    public GameObject[] star = new GameObject[5];
    private ActiveTips tips;
    private Transform tr;

    private void Awake()
    {
        this.tr = base.transform;
    }

    private void OnPress(bool _press)
    {
        if (_press)
        {
            this.tips.gameObject.SetActive(true);
            this.tips.uip.depth = 0xbd;
            this.tips.tr.parent = this.tr;
            this.tips.tr.localPosition = Vector3.zero;
            this.tips.tr.parent = ActivePanel.inst.tr;
            this.tips.nameLabel.text = this.itemName;
            this.tips.describeLabel.text = this.itemDescribe;
            this.tips.ResetSize();
            if (this.tips.iconUIT != null)
            {
                this.tips.iconUIT.mainTexture = this.IconTexcture.mainTexture;
                this.tips.texQuality_new.mainTexture = this.spriteQuality_new.mainTexture;
                if (this.itemType == 0x63)
                {
                    this.tips.iconUIT.SetDimensions(0x5f, 0x5f);
                    this.tips.texQuality_new.SetDimensions(90, 90);
                }
                else
                {
                    this.tips.iconUIT.SetDimensions(0x5f, 0x5f);
                    this.tips.texQuality_new.SetDimensions(90, 90);
                }
                if (((this.itemType == 2) || (this.itemType == 3)) || (this.itemType == 0x67))
                {
                    this.tips.spriteCardType.SetActive(true);
                }
                else
                {
                    this.tips.spriteCardType.SetActive(false);
                }
            }
        }
        else
        {
            this.tips.gameObject.SetActive(false);
        }
    }

    public void ResetData(ActiveShowType clientShowType)
    {
        if (clientShowType == ActiveShowType.present)
        {
            this.num.text = string.Empty;
        }
        else if (clientShowType == ActiveShowType.recharge)
        {
            this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
        }
        else if (clientShowType == ActiveShowType.collectExchange)
        {
            if (this.PressItemTemp != null)
            {
                if (((this.PressItemTemp.affixType == AffixType.AffixType_Card) || (this.PressItemTemp.affixType == AffixType.AffixType_Equip)) || (this.PressItemTemp.affixType == AffixType.AffixType_Item))
                {
                    this.num.text = ActorData.getInstance().GetCurUserHadPressItemNum(this.PressItemTemp) + "/" + this.itemNum.ToString();
                }
                else
                {
                    this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
                }
            }
            else
            {
                this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
            }
        }
        else
        {
            this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
        }
        this.SetLeftCntStateInfo();
        if (this.itemType == 1)
        {
            for (int i = 0; i < this.cardStar; i++)
            {
                this.star[i].SetActive(true);
            }
        }
        else if ((this.itemType == 2) || (this.itemType == 3))
        {
            this.IconTexcture.SetDimensions(0x5f, 0x5f);
        }
        else
        {
            this.kapian.SetActive(false);
            this.IconTexcture.SetDimensions(80, 80);
        }
        if (this.itemType == 0x63)
        {
            this.IconTexcture.mainTexture = BundleMgr.Instance.CreateActiveSpeTypeIcom(this.IconId);
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + this.quality.ToString());
            this.spriteQuality_new.SetDimensions(90, 90);
        }
        else if (this.itemType == 100)
        {
            this.IconTexcture.mainTexture = this.itemIconTex;
            int num2 = this.quality + 1;
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateCardQuality("Ui_Hero_Frame_" + num2.ToString());
            this.spriteQuality_new.SetDimensions(0x67, 0x67);
            this.IconTexcture.SetDimensions(0x5f, 0x5f);
            if (this.PressItemTemp != null)
            {
                this.num.text = ActorData.getInstance().GetCurUserHadPressItemNum(this.PressItemTemp) + "/" + this.itemNum.ToString();
            }
            else
            {
                this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
            }
        }
        else if (this.itemType == 0x65)
        {
            this.IconTexcture.mainTexture = this.itemIconTex;
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + this.quality.ToString());
            this.spriteQuality_new.SetDimensions(90, 90);
            if (this.PressItemTemp != null)
            {
                this.num.text = ActorData.getInstance().GetCurUserHadPressItemNum(this.PressItemTemp) + "/" + this.itemNum.ToString();
            }
            else
            {
                this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
            }
        }
        else if (this.itemType == 0x66)
        {
            this.IconTexcture.mainTexture = this.itemIconTex;
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + this.quality.ToString());
            this.spriteQuality_new.SetDimensions(90, 90);
        }
        else if (this.itemType == 0x67)
        {
            this.IconTexcture.mainTexture = this.itemIconTex;
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + this.quality.ToString());
            this.spriteQuality_new.SetDimensions(90, 90);
            if (this.PressItemTemp != null)
            {
                this.num.text = ActorData.getInstance().GetCurUserHadPressItemNum(this.PressItemTemp) + "/" + this.itemNum.ToString();
            }
            else
            {
                this.num.text = CommonFunc.GetShowStr(this.itemNum, 0x186a0);
            }
            this.kapian.SetActive(true);
            this.IconTexcture.SetDimensions(0x5f, 0x5f);
        }
        else
        {
            this.IconTexcture.mainTexture = BundleMgr.Instance.CreateItemIcon(this.IconId);
            this.spriteQuality_new.mainTexture = BundleMgr.Instance.CreateItemQuality("Ui_Zhuangbei_Frame_" + this.quality.ToString());
            this.spriteQuality_new.SetDimensions(90, 90);
        }
        this.nameLabel.text = this.itemName;
    }

    private void SetLeftCntStateInfo()
    {
        switch (this.curPressentLeftState)
        {
            case LeftCntState.LeftNullOfToday:
                this.shouXin.gameObject.SetActive(false);
                this.leftNullSprite.gameObject.SetActive(true);
                this.leftNullSprite.spriteName = "Ui_Shop_Icon_jrsq";
                this.leftNullSprite.gameObject.GetComponent<UIWidget>().MakePixelPerfect();
                break;

            case LeftCntState.LeftNull:
                this.shouXin.gameObject.SetActive(false);
                this.leftNullSprite.gameObject.SetActive(true);
                this.leftNullSprite.spriteName = "Ui_Shop_Icon_sq";
                this.leftNullSprite.gameObject.GetComponent<UIWidget>().MakePixelPerfect();
                break;

            case LeftCntState.Other:
                this.shouXin.gameObject.SetActive(false);
                this.leftNullSprite.gameObject.SetActive(false);
                this.leftNullSprite.spriteName = string.Empty;
                break;

            default:
                this.shouXin.gameObject.SetActive(false);
                this.leftNullSprite.gameObject.SetActive(false);
                this.leftNullSprite.spriteName = string.Empty;
                break;
        }
    }

    private void Start()
    {
        this.tips = ActivePanel.inst.TipsDlg;
    }

    public enum LeftCntState
    {
        LeftNullOfToday,
        LeftNull,
        Other
    }
}

