using FastBuf;
using HutongGames.PlayMaker.Actions;
using Newbie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    public static Dictionary<int, CardOriginal> card_dic = new Dictionary<int, CardOriginal>();
    private static Dictionary<int, CardOriginal> combine_dic = new Dictionary<int, CardOriginal>();
    private bool mIsCombeing;
    private static GameObject pool_object = null;
    private static GameObject term_notifier = null;

    public static bool CardCanCombine(CardOriginal co)
    {
        <CardCanCombine>c__AnonStorey181 storey = new <CardCanCombine>c__AnonStorey181();
        if ((co.ex_cfg == null) || (co.base_cfg == null))
        {
            return false;
        }
        storey.entry = co.ex_cfg.item_entry;
        storey.num = co.ex_cfg.combine_need_item_num;
        return (null != ActorData.getInstance().ItemList.Find(new Predicate<Item>(storey.<>m__1D3)));
    }

    public static void ClearCache()
    {
        foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
        {
            CardOriginal original = pair.Value;
            if (null == original)
            {
                return;
            }
            GameObject gameObject = original.gameObject;
            if (null != gameObject)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        card_dic.Clear();
        combine_dic.Clear();
    }

    public static List<CardOriginal> CollectActivatedCardsByStandType(int stand_type)
    {
        List<CardOriginal> list = new List<CardOriginal>();
        foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
        {
            int key = pair.Key;
            CardOriginal co = pair.Value;
            if (((co.ori != null) || CardCanCombine(co)) && ((stand_type <= 0) || (stand_type == co.base_cfg.stand_type)))
            {
                list.Add(co);
            }
        }
        list.Sort(new Comparison<CardOriginal>(CardPool.CompareActivatedCard));
        return list;
    }

    public static List<Card> CollectActivatedOriCards(int stand_type)
    {
        List<Card> list = new List<Card>();
        List<CardOriginal> list2 = new List<CardOriginal>();
        foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
        {
            CardOriginal item = pair.Value;
            if (((null != item) && (item.ori != null)) && ((stand_type <= 0) || (stand_type == item.base_cfg.stand_type)))
            {
                list2.Add(item);
            }
        }
        list2.Sort(new Comparison<CardOriginal>(CardPool.CompareActivatedCard));
        int count = list2.Count;
        for (int i = 0; i != count; i++)
        {
            list.Add(list2[i].ori);
        }
        return list;
    }

    public static List<CardOriginal> CollectInActivatedCardsByStandType(int stand_type)
    {
        List<CardOriginal> list = new List<CardOriginal>();
        foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
        {
            int key = pair.Key;
            CardOriginal co = pair.Value;
            if (((co.ori == null) && !CardCanCombine(co)) && ((stand_type <= 0) || (stand_type == co.base_cfg.stand_type)))
            {
                list.Add(co);
            }
        }
        list.Sort(new Comparison<CardOriginal>(CardPool.CompareInActivatedCard));
        return list;
    }

    public static void CollectObject(GameObject go)
    {
        go.transform.parent = pool.transform;
        go.transform.localPosition = new Vector3(0f, 20000f, 0f);
        go.SetActive(false);
    }

    private static int CompareActivatedCard(CardOriginal left, CardOriginal right)
    {
        int num = ((left.ori != null) || !CardCanCombine(left)) ? 0 : 1;
        int num2 = ((right.ori != null) || !CardCanCombine(right)) ? 0 : 1;
        if (num != num2)
        {
            return (num2 - num);
        }
        if ((num + num2) >= 2)
        {
            return (right.base_cfg.entry - left.base_cfg.entry);
        }
        int num3 = (left.ori != null) ? 1 : 0;
        int num4 = (right.ori != null) ? 1 : 0;
        if (num3 != num4)
        {
            return (num4 - num3);
        }
        int level = left.ori.cardInfo.level;
        int num6 = right.ori.cardInfo.level;
        if (level != num6)
        {
            return (num6 - level);
        }
        int quality = left.ori.cardInfo.quality;
        int num8 = right.ori.cardInfo.quality;
        if (quality != num8)
        {
            return (num8 - quality);
        }
        int curExp = (int) left.ori.cardInfo.curExp;
        return (((int) right.ori.cardInfo.curExp) - curExp);
    }

    private static int CompareInActivatedCard(CardOriginal left, CardOriginal right)
    {
        int num = left.NumOfExistedItem();
        return (right.NumOfExistedItem() - num);
    }

    public static void DisablePoolCollider()
    {
        UnityEngine.Object.Destroy(pool.GetComponent<UIDragCamera>());
    }

    private static void FillCardBaseInfo(GameObject go, CardOriginal co)
    {
        UITexture component = go.transform.FindChild("Icon").GetComponent<UITexture>();
        UISprite sprite = go.transform.FindChild("Job/Icon").GetComponent<UISprite>();
        UILabel label = go.transform.FindChild("Name").GetComponent<UILabel>();
        component.mainTexture = BundleMgr.Instance.CreateHeadIcon(co.base_cfg.image);
        sprite.spriteName = GameConstant.CardJobIcon[(co.base_cfg.class_type >= 0) ? co.base_cfg.class_type : 0];
        label.text = co.base_cfg.name;
    }

    public static void FillCardInfo(GameObject go, CardOriginal co)
    {
        <FillCardInfo>c__AnonStorey182 storey = new <FillCardInfo>c__AnonStorey182 {
            co = co
        };
        if (storey.co.dirty_flag >= 1)
        {
            Transform transform = go.transform.FindChild("Star");
            Transform transform2 = go.transform.FindChild("Equip");
            Transform transform3 = go.transform.FindChild("CallBtn");
            Transform transform4 = go.transform.FindChild("New");
            Transform transform5 = go.transform.FindChild("PartBar");
            Transform transform6 = transform.transform.FindChild("cardUpStarTips");
            Transform transform7 = go.transform.FindChild("cardBreakTips");
            FillCardBaseInfo(go, storey.co);
            if (storey.co.ori == null)
            {
                go.transform.FindChild("Level").GetComponent<UILabel>().text = string.Empty;
                transform.gameObject.SetActive(false);
                transform2.gameObject.SetActive(false);
                transform6.gameObject.SetActive(false);
                transform7.gameObject.SetActive(false);
                if (CardCanCombine(storey.co))
                {
                    transform5.gameObject.SetActive(false);
                    transform5.gameObject.SetActive(true);
                    transform4.gameObject.SetActive(true);
                    transform5.FindChild("Label").GetComponent<UILabel>().text = ConfigMgr.getInstance().GetWord(0xdb);
                    UISprite component = transform5.transform.FindChild("Foreground").GetComponent<UISprite>();
                    component.height = 0x19;
                    component.width = 0xee;
                    component.gameObject.SetActive(true);
                    if ((storey.co.dirty_flag & 1) != 0)
                    {
                        nguiTextureGrey.doChangeEnableGrey(go.transform.FindChild("Icon").GetComponent<UITexture>(), true);
                    }
                }
                else
                {
                    transform.gameObject.SetActive(false);
                    transform2.gameObject.SetActive(false);
                    transform3.gameObject.SetActive(false);
                    if ((storey.co.dirty_flag & 1) != 0)
                    {
                        nguiTextureGrey.doChangeEnableGrey(go.transform.FindChild("Icon").GetComponent<UITexture>(), true);
                        UISprite sprite2 = transform5.FindChild("Foreground").GetComponent<UISprite>();
                        UISprite sprite3 = transform5.FindChild("Background").GetComponent<UISprite>();
                        UILabel label2 = transform5.FindChild("Label").GetComponent<UILabel>();
                        if (storey.co.ex_cfg != null)
                        {
                            Item item = ActorData.getInstance().ItemList.Find(new Predicate<Item>(storey.<>m__1D4));
                            int num = (item != null) ? item.num : 0;
                            label2.text = num.ToString() + " /" + storey.co.ex_cfg.combine_need_item_num.ToString();
                            float num2 = Mathf.Min((float) (((float) num) / ((float) storey.co.ex_cfg.combine_need_item_num)), (float) 1f);
                            sprite2.width = (int) (num2 * (sprite3.width - 6f));
                            transform5.gameObject.SetActive(true);
                            sprite2.gameObject.SetActive(num2 > 0f);
                        }
                        else
                        {
                            sprite2.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                bool flag = FillEquipInfo(go, storey.co);
                if ((storey.co.dirty_flag & 1) != 0)
                {
                    UILabel label3 = go.transform.FindChild("Name").GetComponent<UILabel>();
                    UILabel label4 = go.transform.FindChild("Level").GetComponent<UILabel>();
                    UILabel label5 = go.transform.FindChild("QualityStr").GetComponent<UILabel>();
                    UISprite sprite4 = go.transform.FindChild("QualityBorder").GetComponent<UISprite>();
                    nguiTextureGrey.doChangeEnableGrey(go.transform.FindChild("Icon").GetComponent<UITexture>(), false);
                    label3.text = CommonFunc.GetCardNameByQuality(storey.co.ori.cardInfo.quality, storey.co.base_cfg.name);
                    label4.text = "LV." + storey.co.ori.cardInfo.level.ToString();
                    label5.text = CommonFunc.GetCardQualityStr(storey.co.ori.cardInfo.quality);
                    CommonFunc.SetQualityBorder(sprite4, storey.co.ori.cardInfo.quality);
                    for (int i = 0; i < 5; i++)
                    {
                        int num5 = i + 1;
                        UISprite sprite5 = go.transform.FindChild("Star/" + num5.ToString()).GetComponent<UISprite>();
                        sprite5.gameObject.SetActive(i < storey.co.ori.cardInfo.starLv);
                        sprite5.transform.localPosition = new Vector3(19f * i, 0f, 0f);
                        if (i == storey.co.ori.cardInfo.starLv)
                        {
                            transform6.transform.localPosition = sprite5.transform.localPosition;
                        }
                    }
                    transform5.gameObject.SetActive(false);
                    transform.gameObject.SetActive(true);
                    transform2.gameObject.SetActive(true);
                    transform3.gameObject.SetActive(false);
                    bool flag2 = CommonFunc.CheckCardCanBreak(storey.co.ori);
                    bool flag3 = CommonFunc.CheckCardCanUpStar(storey.co.ori);
                    transform4.gameObject.SetActive((flag || flag2) || flag3);
                    transform7.gameObject.SetActive(flag2);
                    transform6.gameObject.SetActive(flag3);
                    int num4 = -123;
                    if (flag3)
                    {
                        num4 -= 11;
                    }
                    transform.transform.localPosition = new Vector3(num4 - (9.5f * (storey.co.ori.cardInfo.starLv - 1)), transform.transform.localPosition.y, 0f);
                }
            }
        }
    }

    private static bool FillEquipInfo(GameObject go, CardOriginal co)
    {
        if ((co.ori == null) || (co.base_cfg == null))
        {
            return false;
        }
        bool flag = false;
        Transform transform = go.transform.FindChild("Equip");
        for (int i = 0; i != 6; i++)
        {
            Transform transform2 = transform.transform.FindChild(i.ToString());
            if (null != transform2)
            {
                EquipInfo info = co.ori.equipInfo[i];
                if (info != null)
                {
                    item_config _config = ConfigMgr.getInstance().getByEntry<item_config>(info.entry);
                    if (_config != null)
                    {
                        UISprite component = transform2.FindChild("Icon").GetComponent<UISprite>();
                        UISprite sprite2 = transform2.FindChild("QualityBorder").GetComponent<UISprite>();
                        component.spriteName = _config.icon;
                        CommonFunc.SetEquipQualityBorder(sprite2, _config.quality, true);
                        Transform transform3 = transform2.FindChild("BreakTips");
                        transform3.gameObject.SetActive(false);
                        break_equip_config _config2 = ConfigMgr.getInstance().getByEntry<break_equip_config>(_config.entry);
                        if (((_config2 != null) && (_config2.break_equip_entry > -1)) && (CommonFunc.CheckMaterialEnough(_config2.equip_entry) && (_config.quality < (co.ori.cardInfo.quality + 1))))
                        {
                            transform3.gameObject.SetActive(true);
                            flag = true;
                        }
                        Transform transform4 = transform2.FindChild("LevUpTips");
                        if (info.lv <= (co.ori.cardInfo.level - 5))
                        {
                            transform4.gameObject.SetActive(true);
                            flag = true;
                        }
                        else
                        {
                            transform4.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        return flag;
    }

    private void OnClickCard(GameObject go)
    {
        <OnClickCard>c__AnonStorey184 storey = new <OnClickCard>c__AnonStorey184 {
            <>f__this = this,
            co = go.GetComponent<CardOriginal>()
        };
        if (null != storey.co)
        {
            if (storey.co.ori == null)
            {
                if (CardCanCombine(storey.co))
                {
                    <OnClickCard>c__AnonStorey183 storey2 = new <OnClickCard>c__AnonStorey183 {
                        <>f__this = this
                    };
                    if (GuideSystem.MatchEvent(GuideEvent.CardCombine_CombineHero))
                    {
                        GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Hero.tag_guide_select_hero, go);
                    }
                    if (!this.mIsCombeing)
                    {
                        this.mIsCombeing = true;
                        storey2.go_anim = new GameObject();
                        storey2.go_anim.name = "combine_animation";
                        CombineAnimation animation = storey2.go_anim.AddComponent<CombineAnimation>();
                        animation.action_finish = new System.Action(storey2.<>m__1D5);
                        animation.DoAnimaition(storey.co.base_cfg.entry);
                        SocketMgr.Instance.RequestItemMachining(0, storey.co.base_cfg.entry);
                    }
                }
                else
                {
                    GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", new Action<GUIEntity>(storey.<>m__1D6));
                }
            }
            else
            {
                SoundManager.mInstance.PlaySFX("sound_ui_t_8");
                HeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>();
                GameObject obj2 = (null != activityGUIEntity) ? activityGUIEntity.gameObject : null;
                if ((GuideSystem.MatchEvent(GuideEvent.EquipLevelUp_SelectHero) || GuideSystem.MatchEvent(GuideEvent.CardBreak_SelectHero)) || (GuideSystem.MatchEvent(GuideEvent.SkillLevelUp_SelectHero) || GuideSystem.MatchEvent(GuideEvent.Strengthen_SelectHero)))
                {
                    GuideSystem.ActivedGuide.RequestUIResponse(GuideRegister_Hero.tag_guide_select_hero, go);
                }
                this.mIsCombeing = false;
                GUIMgr.Instance.PushGUIEntity("HeroInfoPanel", new Action<GUIEntity>(storey.<>m__1D7));
            }
        }
    }

    public static void RebuildAll(bool immidiate = false)
    {
        HashSet<int> set = new HashSet<int>();
        HashSet<int> set2 = new HashSet<int>();
        IEnumerator enumerator = ConfigMgr.getInstance().getList<card_config>().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                card_config current = (card_config) enumerator.Current;
                <RebuildAll>c__AnonStorey180 storey = new <RebuildAll>c__AnonStorey180();
                if ((current.is_show && (current.stand_type >= 0)) && (!set.Contains(current.entry) && !set2.Contains(current.name_type)))
                {
                    storey.entry = current.entry;
                    Card card = ActorData.getInstance().mCards.Find(new Predicate<Card>(storey.<>m__1D2));
                    SynCardObject(storey.entry, card, 3);
                    set.Add(current.entry);
                    set2.Add(current.name_type);
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
        if (immidiate)
        {
            foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
            {
                CardOriginal co = pair.Value;
                if ((null != co) && co.dirty)
                {
                    FillCardInfo(co.gameObject, co);
                    co.dirty = false;
                }
            }
        }
    }

    public static void RefreshPoolDrawCall()
    {
        pool.GetComponent<UIPanel>().RebuildAllDrawCalls();
    }

    private void RegisterEventListener(GameObject obj)
    {
        UIEventListener.Get(obj).onClick = new UIEventListener.VoidDelegate(this.OnClickCard);
    }

    public static void ResetPoolCollider()
    {
        NGUITools.AddWidgetCollider(pool, true);
        UIDragCamera component = pool.GetComponent<UIDragCamera>();
        if (null == component)
        {
            component = pool.AddComponent<UIDragCamera>();
        }
        component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
    }

    public static void SynCardItemChanged(int item_entry)
    {
        CardOriginal original = null;
        combine_dic.TryGetValue(item_entry, out original);
        if ((null != original) && (original.ori == null))
        {
            SynCardObject(original.base_cfg.entry, null, 1);
        }
    }

    public static void SynCardObject(int entry, Card card, int dirty_flag = 3)
    {
        CardOriginal original = null;
        if (!card_dic.TryGetValue(entry, out original))
        {
            GameObject obj2 = BundleMgr.Instance.LoadResource("CardItem", ".prefab", typeof(GameObject)) as GameObject;
            if (null == obj2)
            {
                throw new UnityException("failed to load prefab named CardItem!");
            }
            obj2 = UnityEngine.Object.Instantiate(obj2) as GameObject;
            obj2.SetActive(false);
            obj2.transform.parent = CardPool.pool.transform;
            obj2.transform.localPosition = new Vector3(0f, (-140f * CardPool.pool.transform.childCount) + 20000f, 0f);
            obj2.transform.localScale = Vector3.one;
            UIDragCamera component = obj2.GetComponent<UIDragCamera>();
            if (null != component)
            {
                component.draggableCamera = GUIMgr.Instance.ListDraggableCamera;
            }
            original = obj2.AddComponent<CardOriginal>();
            original.base_cfg = ConfigMgr.getInstance().getByEntry<card_config>(entry);
            original.ex_cfg = CommonFunc.GetCardExCfg(entry, original.base_cfg.evolve_lv);
            card_dic.Add(entry, original);
            if (original.ex_cfg != null)
            {
                combine_dic.Add(original.ex_cfg.item_entry, original);
            }
            CardPool pool = CardPool.pool.GetComponent<CardPool>();
            if (null != pool)
            {
                pool.RegisterEventListener(obj2);
            }
        }
        original.ori = card;
        original.dirty = true;
        original.dirty_flag = dirty_flag;
        original.CollectEquipMaterial();
        GameDataMgr.Instance.DirtyActiveCard = true;
        GameDataMgr.Instance.DirtyCardEquipLvUp = true;
    }

    public static void SynEquipMaterialChanged(int item_entry)
    {
        foreach (KeyValuePair<int, CardOriginal> pair in card_dic)
        {
            CardOriginal original = pair.Value;
            if ((original.ori != null) && original.equip_lu_set.Contains(item_entry))
            {
                original.dirty = true;
                original.dirty_flag = 1;
            }
        }
    }

    private static GameObject pool
    {
        get
        {
            if (null == pool_object)
            {
                pool_object = new GameObject();
                pool_object.name = "Card2DPool";
                pool_object.transform.parent = GUIMgr.Instance.ListRoot.transform;
                pool_object.transform.localPosition = Vector3.zero;
                pool_object.transform.localScale = Vector3.one;
                pool_object.AddComponent<CardPool>();
                pool_object.AddComponent<UIPanel>();
                MTDLayers.SetlayerRecursively(pool_object, LayerMask.NameToLayer("ListUI"));
                UnityEngine.Object.DontDestroyOnLoad(pool_object);
                term_notifier = new GameObject();
                term_notifier.name = "Card2DPoolTermNotifier";
                UnityEngine.Object.DontDestroyOnLoad(term_notifier);
                term_notifier.AddComponent<CardTermNotifier>().StartMe();
            }
            return pool_object;
        }
    }

    public static Transform root_bounds
    {
        get
        {
            return pool.transform;
        }
    }

    public int StandTypeContext
    {
        get
        {
            int mStandType = 0;
            HeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>();
            if (null != activityGUIEntity)
            {
                mStandType = activityGUIEntity.mStandType;
            }
            return mStandType;
        }
    }

    [CompilerGenerated]
    private sealed class <CardCanCombine>c__AnonStorey181
    {
        internal int entry;
        internal int num;

        internal bool <>m__1D3(Item e)
        {
            return ((e.entry == this.entry) && (e.num >= this.num));
        }
    }

    [CompilerGenerated]
    private sealed class <FillCardInfo>c__AnonStorey182
    {
        internal CardOriginal co;

        internal bool <>m__1D4(Item e)
        {
            return (e.entry == this.co.ex_cfg.item_entry);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickCard>c__AnonStorey183
    {
        internal CardPool <>f__this;
        internal GameObject go_anim;

        internal void <>m__1D5()
        {
            UnityEngine.Object.Destroy(this.go_anim);
            this.<>f__this.mIsCombeing = false;
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickCard>c__AnonStorey184
    {
        internal CardPool <>f__this;
        internal CardOriginal co;

        internal void <>m__1D6(GUIEntity obj)
        {
            ((HeroInfoPanel) obj).ShowCardPartInfo(this.co.ex_cfg);
        }

        internal void <>m__1D7(GUIEntity obj)
        {
            List<long> cardIdList = new List<long> {
                this.co.ori.card_id
            };
            List<Card> list2 = CardPool.CollectActivatedOriCards(this.<>f__this.StandTypeContext);
            HeroInfoPanel panel = (HeroInfoPanel) obj;
            panel.InitCardInfo(this.co.ori);
            panel.SetCurrShowCardList(list2);
            SocketMgr.Instance.RequestCalPower(cardIdList, false, BattleFormationType.BattleFormationType_Num);
            if (CommonFunc.CheckHaveEquipLevUp(this.co.ori))
            {
                GuideSystem.FireEvent(GuideEvent.EquipLevelUp_Function);
            }
            if (CommonFunc.CheckCardCanBreak(this.co.ori))
            {
                GuideSystem.FireEvent(GuideEvent.CardBreak_Function);
            }
            if (Utility.CheckSkillCanbeLevelup(this.co.ori))
            {
                GuideSystem.FireEvent(GuideEvent.SkillLevelUp_Function);
            }
            if (Utility.GetEquipCanbeStrengthen(this.co.ori) != null)
            {
                GuideSystem.FireEvent(GuideEvent.Strengthen_Function);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <RebuildAll>c__AnonStorey180
    {
        internal int entry;

        internal bool <>m__1D2(Card e)
        {
            return (e.cardInfo.entry == this.entry);
        }
    }

    public enum DirtyFlag
    {
        Details = 1,
        Equip = 2
    }
}

