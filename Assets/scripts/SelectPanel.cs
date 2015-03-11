using FastBuf;
using System;
using System.Runtime.CompilerServices;
using Toolbox;
using UnityEngine;

public class SelectPanel : GUIEntity
{
    private SelectHeroDelegate _SelCallBack;
    public System.Action CloseEvent;
    public GameObject HeroIcon;
    private const int LineCount = 7;
    public UIGrid ListRoot;
    private const int RowCount = 3;
    private float ScaleParam = 1.2f;

    private void ClosePanel()
    {
        if (this.CloseEvent != null)
        {
            this.CloseEvent();
        }
        GUIMgr.Instance.FloatTitleBar();
    }

    private void HideStar(GameObject obj)
    {
        for (int i = 1; i <= 5; i++)
        {
            obj.transform.FindChild("Star/" + i).gameObject.SetActive(false);
        }
    }

    private void OnClickHeroIcon(GameObject obj)
    {
        Card card = (Card) GUIDataHolder.getData(obj);
        if (card != null)
        {
            this._SelCallBack(card);
        }
        this.DelayCallBack(0.2f, delegate {
            GUIMgr.Instance.ExitModelGUI(base.name);
            GUIMgr.Instance.FloatTitleBar();
        });
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        this.UpdateCardList();
        base.OnDeSerialization(pers);
    }

    public override void OnInitialize()
    {
        GUIMgr.Instance.DockTitleBar();
        base.OnInitialize();
        this.UpdateCardList();
    }

    public void SetDelegate(SelectHeroDelegate _delegate)
    {
        this._SelCallBack = _delegate;
    }

    public void UpdateCardList()
    {
        if (base.transform.GetComponent<PlayMakerFSM>() != null)
        {
            int num = 0;
            int num2 = 0;
            GameObject obj2 = null;
            CommonFunc.DeleteChildItem(this.ListRoot.transform);
            foreach (Card card in ActorData.getInstance().CardList)
            {
                card_config _config = ConfigMgr.getInstance().getByEntry<card_config>((int) card.cardInfo.entry);
                if ((num == 0) || ((num % 7) == 0))
                {
                    obj2 = new GameObject();
                    obj2.name = (num2 + 1).ToString();
                    obj2.transform.parent = this.ListRoot.transform;
                    obj2.transform.localPosition = Vector3.zero;
                    obj2.transform.localScale = Vector3.one;
                    num = 0;
                    num2++;
                }
                GameObject go = UnityEngine.Object.Instantiate(this.HeroIcon) as GameObject;
                go.transform.parent = obj2.transform;
                go.transform.localPosition = new Vector3((float) (-412 + (num * 130)), 0f, 0f);
                go.transform.localScale = new Vector3(this.ScaleParam, this.ScaleParam, 1f);
                GUIDataHolder.setData(go, card);
                UIEventListener listener1 = UIEventListener.Get(go);
                listener1.onClick = (UIEventListener.VoidDelegate) Delegate.Combine(listener1.onClick, new UIEventListener.VoidDelegate(this.OnClickHeroIcon));
                this.UpdateHeroIconData(go, card);
                num++;
            }
            UIScrollView component = this.ListRoot.transform.parent.GetComponent<UIScrollView>();
            if (num2 <= 3)
            {
                component.movement = UIScrollView.Movement.Custom;
                component.customMovement = Vector2.zero;
            }
            else
            {
                component.movement = UIScrollView.Movement.Vertical;
            }
            this.ListRoot.repositionNow = true;
        }
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
            UILabel label = _obj.transform.FindChild("Label").GetComponent<UILabel>();
            UISprite sprite = _obj.transform.FindChild("frame").GetComponent<UISprite>();
            _obj.transform.FindChild("Job/Icon").GetComponent<UISprite>().spriteName = GameConstant.CardJobIcon[(_config.class_type >= 0) ? _config.class_type : 0];
            component.mainTexture = BundleMgr.Instance.CreateHeadIcon(_config.image);
            label.text = _data.cardInfo.level.ToString();
            CommonFunc.SetQualityColor(sprite, _data.cardInfo.quality);
            this.HideStar(_obj);
            for (int i = 1; i <= _data.cardInfo.starLv; i++)
            {
                _obj.transform.FindChild("Star/" + i).gameObject.SetActive(true);
            }
            ResetHeroStarPos pos = _obj.transform.FindChild("Star").GetComponent<ResetHeroStarPos>();
            if (pos != null)
            {
                pos.ResetPos(_data.cardInfo.starLv);
            }
        }
    }

    public delegate void SelectHeroDelegate(Card _card);
}

