using FastBuf;
using HutongGames.PlayMaker;
using Newbie;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Toolbox;
using UnityEngine;

public class HeroPanel : GUIEntity
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache5;
    public GameObject CardListDevideItem;
    public List<GameObject> itemsCard = new List<GameObject>();
    private bool mIsFirstInit = true;
    public int mStandType;
    private static float vert_scroll_offset;

    public void CollectAllItems()
    {
        int count = this.itemsCard.Count;
        for (int i = 0; i != count; i++)
        {
            GameObject go = this.itemsCard[i];
            if (null != go)
            {
                CardPool.CollectObject(go);
            }
        }
        this.itemsCard.Clear();
    }

    public void CreateCardList(int _StandType, float scrolling)
    {
        int num = 150;
        Transform transform2 = base.transform.FindChild("ListPanel1").FindChild("Group1");
        this.RemoveAllChildren();
        this.ResetClipViewport(scrolling);
        List<CardOriginal> list = CardPool.CollectActivatedCardsByStandType(_StandType);
        List<CardOriginal> list2 = CardPool.CollectInActivatedCardsByStandType(_StandType);
        int count = list.Count;
        int num3 = (count / 2) + (((count % 2) != 0) ? 1 : 0);
        for (int i = 0; i != num3; i++)
        {
            CardOriginal original = list[i * 2];
            GameObject gameObject = original.gameObject;
            gameObject.SetActive(true);
            gameObject.name = "Card1";
            gameObject.transform.localPosition = new Vector3(-263f, 146f - (num * i), 0f);
            this.itemsCard.Add(gameObject);
            if (((i * 2) + 1) < count)
            {
                CardOriginal original2 = list[(i * 2) + 1];
                GameObject item = original2.gameObject;
                item.SetActive(true);
                item.name = "Card2";
                item.transform.localPosition = new Vector3(131f, 146f - (num * i), 0f);
                this.itemsCard.Add(item);
            }
        }
        GameObject obj4 = UnityEngine.Object.Instantiate(this.CardListDevideItem) as GameObject;
        obj4.transform.parent = transform2;
        obj4.transform.localPosition = new Vector3(0f, (-num * num3) + 185f, -0.1f);
        obj4.transform.localScale = Vector3.one;
        obj4.transform.rotation = Quaternion.identity;
        obj4.SetActive(true);
        UIDragCamera component = obj4.GetComponent<UIDragCamera>();
        if (null != component)
        {
            component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
        }
        int num5 = num3;
        count = list2.Count;
        num3 = (count / 2) + (((count % 2) != 0) ? 1 : 0);
        for (int j = 0; j != num3; j++)
        {
            CardOriginal original3 = list2[j * 2];
            GameObject obj5 = original3.gameObject;
            obj5.SetActive(true);
            obj5.name = "Card1";
            obj5.transform.localPosition = new Vector3(-263f, (146f - (num * ((num5 + j) + 1))) + 86f, 0f);
            this.itemsCard.Add(obj5);
            if (((j * 2) + 1) < count)
            {
                CardOriginal original4 = list2[(j * 2) + 1];
                GameObject obj6 = original4.gameObject;
                obj6.SetActive(true);
                obj6.name = "Card2";
                obj6.transform.localPosition = new Vector3(131f, (146f - (num * ((num5 + j) + 1))) + 86f, 0f);
                this.itemsCard.Add(obj6);
            }
        }
        CardPool.ResetPoolCollider();
        this.mIsFirstInit = false;
    }

    private void InitCardList(int _StandType, float scrolling = 0f)
    {
        GUIMgr.Instance.ListDraggableCamera.rootForBounds = CardPool.root_bounds;
        this.CreateCardList(_StandType, scrolling);
    }

    private void OnClosePanel()
    {
        GUIMgr.Instance.PopGUIEntity();
    }

    public override void OnDeSerialization(GUIPersistence pers)
    {
        GUIMgr.Instance.ListRoot.gameObject.SetActive(true);
        GUIMgr.Instance.FloatTitleBar();
        if (!this.mIsFirstInit)
        {
            base.transform.FindChild("Tab/AllBtn").GetComponent<UIToggle>().isChecked = true;
            base.transform.FindChild("Tab/FangYuBtn/TitleDown").GetComponent<UISprite>().color = (Color) new Color32(0xff, 0xff, 0xff, 0);
            base.transform.FindChild("Tab/ShuChuBtn/TitleDown").GetComponent<UISprite>().color = (Color) new Color32(0xff, 0xff, 0xff, 0);
            base.transform.FindChild("Tab/ZhiLiaoBtn/TitleDown").GetComponent<UISprite>().color = (Color) new Color32(0xff, 0xff, 0xff, 0);
            PlayMakerFSM component = base.transform.GetComponent<PlayMakerFSM>();
            if (component == null)
            {
                return;
            }
            FsmInt num = component.FsmVariables.FindFsmInt("standType");
            int num2 = 0;
            num.Value = num2;
            this.mStandType = num2;
            this.InitCardList(0, vert_scroll_offset);
        }
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = delegate {
                GuideSystem.FireEvent(GuideEvent.EquipLevelUp_SelectHero);
                GuideSystem.FireEvent(GuideEvent.CardBreak_SelectHero);
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_SelectHero);
                GuideSystem.FireEvent(GuideEvent.CardCombine_CombineHero);
                GuideSystem.FireEvent(GuideEvent.Strengthen_SelectHero);
            };
        }
        this.DelayCallBack(0.1f, <>f__am$cache5);
    }

    public override void OnDestroy()
    {
        this.CollectAllItems();
        base.OnDestroy();
    }

    public override void OnInitialize()
    {
        base.multiCamera = true;
        this.InitCardList(0, 0f);
        base.FindChild<UIToggle>("AllBtn").OnUIMouseClick(u => this.ShowCardList(0));
        base.FindChild<UIToggle>("FangYuBtn").OnUIMouseClick(u => this.ShowCardList(1));
        base.FindChild<UIToggle>("ShuChuBtn").OnUIMouseClick(u => this.ShowCardList(2));
        base.FindChild<UIToggle>("ZhiLiaoBtn").OnUIMouseClick(u => this.ShowCardList(3));
    }

    public override void OnSerialization(GUIPersistence pers)
    {
        this.CollectAllItems();
        CardPool.DisablePoolCollider();
        base.OnSerialization(pers);
        this.SerializationViewScrolling(pers);
        GuideSystem.TerminateEvent(GuideEvent.EquipLevelUp_SelectHero);
        GuideSystem.TerminateEvent(GuideEvent.CardBreak_SelectHero);
        GuideSystem.TerminateEvent(GuideEvent.SkillLevelUp_SelectHero);
        GuideSystem.TerminateEvent(GuideEvent.CardCombine_CombineHero);
        GuideSystem.TerminateEvent(GuideEvent.Strengthen_SelectHero);
    }

    public void RemoveAllChildren()
    {
        this.CollectAllItems();
        Transform transform = base.transform.FindChild("ListPanel1/Group1");
        if (null != transform)
        {
            CommonFunc.DeleteChildItem(transform);
        }
    }

    private void ResetClipViewport(float scrolling)
    {
        Transform top = base.transform.FindChild("ListTopLeft");
        Transform bottom = base.transform.FindChild("ListBottomRight");
        GUIMgr.Instance.ResetListViewpot(top, bottom, null, scrolling);
    }

    public void ResetTogglePage()
    {
        base.transform.FindChild("Tab/AllBtn").GetComponent<UIToggle>().isChecked = true;
        base.transform.FindChild("Tab/FangYuBtn").GetComponent<UIToggle>().isChecked = false;
        base.transform.FindChild("Tab/ShuChuBtn").GetComponent<UIToggle>().isChecked = false;
        base.transform.FindChild("Tab/ZhiLiaoBtn").GetComponent<UIToggle>().isChecked = false;
    }

    private void SerializationViewScrolling(GUIPersistence pers)
    {
        Transform lt = base.transform.FindChild("ListTopLeft");
        Transform rb = base.transform.FindChild("ListBottomRight");
        vert_scroll_offset = GUIMgr.Instance.CalculateListViewportScrolling(lt, rb);
    }

    private void SetTabBtnEnable(bool isEnable)
    {
        base.transform.FindChild("Tab/AllBtn").GetComponent<BoxCollider>().enabled = isEnable;
        base.transform.FindChild("Tab/FangYuBtn").GetComponent<BoxCollider>().enabled = isEnable;
        base.transform.FindChild("Tab/ShuChuBtn").GetComponent<BoxCollider>().enabled = isEnable;
        base.transform.FindChild("Tab/ZhiLiaoBtn").GetComponent<BoxCollider>().enabled = isEnable;
    }

    private void ShowCardList(int _standType)
    {
        if (this.mStandType != _standType)
        {
            this.RemoveAllChildren();
            this.InitCardList(_standType, 0f);
            GuideSystem.RefireEvent(GuideEvent.EquipLevelUp_SelectHero);
            GuideSystem.RefireEvent(GuideEvent.CardBreak_SelectHero);
            GuideSystem.RefireEvent(GuideEvent.SkillLevelUp_SelectHero);
            GuideSystem.RefireEvent(GuideEvent.CardCombine_CombineHero);
            GuideSystem.RefireEvent(GuideEvent.Strengthen_SelectHero);
        }
        this.mStandType = _standType;
    }

    public void UpdateData()
    {
        this.InitCardList(0, 0f);
    }

    public void UpdateItemCardInfo(Card _card)
    {
    }
}

