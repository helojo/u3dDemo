using FastBuf;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChangePlayerIconPanel : GUIEntity
{
    [CompilerGenerated]
    private static Comparison<Card> <>f__am$cache5;
    private List<GameObject> CardList = new List<GameObject>();
    private bool IconChanged;
    private const int LineCount = 5;
    public UIGrid ListRoot;
    private const int RowCount = 3;
    private Card SelectHero;
    public GameObject SingleHeroIcon;

    private void ExitPanel(GameObject go)
    {
        if (GuideSystem.MatchEvent(GuideEvent.HelpMe))
        {
            GuideSystem.ActivedGuide.RequestCancel();
        }
        GUIMgr.Instance.ExitModelGUI("ChangePlayerIconPanel");
    }

    private void OnClickHeroIcon(GameObject obj)
    {
        if (GuideSystem.MatchEvent(GuideEvent.HelpMe))
        {
            GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_HelpMe.tag_helpme_press_head_icon, obj);
        }
        Card card = (Card) GUIDataHolder.getData(obj);
        this.SelectHero = card;
        this.IconChanged = true;
        SocketMgr.Instance.RequestSetHead((int) this.SelectHero.cardInfo.entry);
    }

    public override void OnInitialize()
    {
        UIEventListener.Get(base.transform.FindChild("Close").gameObject).onClick = new UIEventListener.VoidDelegate(this.ExitPanel);
        this.UpdateCardList();
    }

    private void SetHead()
    {
        if (this.SelectHero == null)
        {
            TipsDiag.SetText(ConfigMgr.getInstance().GetWord(0x9a));
        }
        else
        {
            SocketMgr.Instance.RequestSetHead((int) this.SelectHero.cardInfo.entry);
        }
    }

    public void UpdateCardList()
    {
        int num = 0;
        CommonFunc.DeleteChildItem(this.ListRoot.transform);
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = delegate (Card c1, Card c2) {
                if (c2.cardInfo.quality != c1.cardInfo.quality)
                {
                    return c2.cardInfo.quality - c1.cardInfo.quality;
                }
                if (c2.cardInfo.level != c1.cardInfo.level)
                {
                    return c2.cardInfo.level - c1.cardInfo.level;
                }
                if (c2.cardInfo.starLv != c1.cardInfo.starLv)
                {
                    return c2.cardInfo.starLv - c1.cardInfo.starLv;
                }
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) c1.cardInfo.entry);
                card_config _config2 = ConfigMgr.getInstance().getByEntry<card_config>((int) c2.cardInfo.entry);
                int num = (_config == null) ? 0 : _config.card_position;
                int num2 = (_config2 == null) ? 0 : _config2.card_position;
                if (num != num2)
                {
                    return num - num2;
                }
                return 0;
            };
        }
        ActorData.getInstance().CardList.Sort(<>f__am$cache5);
        int num2 = ActorData.getInstance().CardList.Count / 5;
        if ((ActorData.getInstance().CardList.Count % 5) != 0)
        {
            num2++;
        }
        for (int i = 0; i < num2; i++)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(this.SingleHeroIcon) as GameObject;
            obj2.transform.parent = this.ListRoot.transform;
            obj2.transform.localPosition = new Vector3(0f, 0f, 0f);
            obj2.transform.localScale = Vector3.one;
            for (int j = 0; j < 5; j++)
            {
                Transform transform = obj2.transform.FindChild("Item" + (j + 1));
                transform.GetComponent<UIToggle>().group = 1;
                if (num < ActorData.getInstance().CardList.Count)
                {
                    GUIDataHolder.setData(transform.gameObject, ActorData.getInstance().CardList[num]);
                    UIEventListener.Get(transform.gameObject).onClick = new UIEventListener.VoidDelegate(this.OnClickHeroIcon);
                    this.UpdateHeroIconData(transform.gameObject, ActorData.getInstance().CardList[num]);
                    num++;
                }
                else
                {
                    transform.gameObject.SetActive(false);
                }
            }
        }
        this.ListRoot.Reposition();
    }

    private void UpdateHeroIconData(GameObject _obj, Card _data)
    {
        card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) _data.cardInfo.entry);
        if (_config == null)
        {
            Debug.LogWarning("CardCfg Is Null! Entry is " + _data.cardInfo.entry);
        }
        else
        {
            UITexture component = _obj.transform.FindChild("Icon").GetComponent<UITexture>();
            CommonFunc.SetQualityBorder(_obj.transform.FindChild("frame").GetComponent<UISprite>(), _data.cardInfo.quality);
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
        }
    }
}

