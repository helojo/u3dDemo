namespace Newbie
{
    using FastBuf;
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Utility
    {
        public static bool CheckCommunitySceneActived()
        {
            PlayMakerFSM component = GUIMgr.Instance.Root.GetComponent<PlayMakerFSM>();
            if (null == component)
            {
                return false;
            }
            return ("CommunityState" == component.ActiveStateName);
        }

        public static bool CheckEquipHasStrengthen()
        {
            foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
            {
                CardOriginal original = pair.Value;
                if ((null != original) && (original.ori != null))
                {
                    int count = original.ori.equipInfo.Count;
                    for (int i = 0; i != count; i++)
                    {
                        EquipInfo info = original.ori.equipInfo[i];
                        if ((info != null) && (info.lv > 1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckHaveCardCanbeCombine()
        {
            return (null != GetCardCanbeCombine());
        }

        public static bool CheckHaveCardSkillCanbeLevelup()
        {
            foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
            {
                CardOriginal original = pair.Value;
                if (((null != original) && (original.ori != null)) && CheckSkillCanbeLevelup(original.ori))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckHaveEquipCanbeStrengthen()
        {
            foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
            {
                CardOriginal original = pair.Value;
                if (((null != original) && (original.ori != null)) && (GetEquipCanbeStrengthen(original.ori) != null))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckMedicineExistly()
        {
            List<Item> itemList = ActorData.getInstance().ItemList;
            int count = itemList.Count;
            for (int i = 0; i != count; i++)
            {
                Item item = itemList[i];
                if ((item != null) && (((item.entry == 700) || (item.entry == 0x2bd)) || (item.entry == 0x2be)))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckOriginedDupMap()
        {
            DupMap activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DupMap>();
            if (null == activityGUIEntity)
            {
                return false;
            }
            Transform transform = activityGUIEntity.transform.FindChild("TrenchMap");
            if (null == transform)
            {
                return false;
            }
            return !transform.gameObject.activeSelf;
        }

        public static bool CheckSkillCanbeLevelup(Card card)
        {
            return (E_CardSkill.e_card_skill_max != SkillLevelupSlot(card));
        }

        public static bool CheckSkillHasLevelup()
        {
            foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
            {
                CardOriginal original = pair.Value;
                if ((null != original) && (original.ori != null))
                {
                    int count = original.ori.cardInfo.skillInfo.Count;
                    for (int i = 0; i != count; i++)
                    {
                        SkillInfo info = original.ori.cardInfo.skillInfo[i];
                        if ((info != null) && (info.skillLevel > 1))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckUIActivedSkillLevelUp()
        {
            HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
            if (null == activityGUIEntity)
            {
                return false;
            }
            return activityGUIEntity.SkillLevelupTabActived();
        }

        public static void EnforceClear()
        {
            NewbieEntity activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<NewbieEntity>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.Reset();
            }
        }

        public static GameObject FindMainUIModuleTipsObjectByKey(string key)
        {
            MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
            if (null == activityGUIEntity)
            {
                return null;
            }
            return activityGUIEntity.FindBuildingIconObjectByKey(key);
        }

        public static void ForceEnableBagPageDraggable(bool enable)
        {
            BagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.multiCamera = enable;
            }
        }

        public static void ForceSwitchBagPage()
        {
            BagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ForceSwitchXiaohao();
            }
        }

        public static bool GenerateDupMapEliteTab()
        {
            DupMap activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DupMap>();
            if (null == activityGUIEntity)
            {
                return false;
            }
            Transform transform = activityGUIEntity.transform.FindChild("TrenchMap");
            if ((null == transform) || !transform.gameObject.activeSelf)
            {
                return false;
            }
            Transform transform2 = transform.FindChild("CheckPart/Toggle2");
            if ((null == transform2) || !transform2.gameObject.activeSelf)
            {
                return false;
            }
            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_select_elitetrench_tab, transform2.gameObject);
            return true;
        }

        public static bool GenerateDupMapEliteTrenchNode()
        {
            DupMap activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DupMap>();
            if (null == activityGUIEntity)
            {
                return false;
            }
            Transform transform = activityGUIEntity.transform.FindChild("TrenchMap");
            if ((null == transform) || !transform.gameObject.activeSelf)
            {
                return false;
            }
            Transform transform2 = transform.FindChild("EliteMapPos/0/1");
            if ((null == transform2) || !transform2.gameObject.activeSelf)
            {
                return false;
            }
            GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Battle.tag_battle_select_trench, transform2.gameObject);
            return true;
        }

        public static CardOriginal GetCardCanbeCombine()
        {
            foreach (KeyValuePair<int, CardOriginal> pair in CardPool.card_dic)
            {
                CardOriginal co = pair.Value;
                if (((null != co) && (co.ori == null)) && CardPool.CardCanCombine(co))
                {
                    return co;
                }
            }
            return null;
        }

        public static EquipInfo GetEquipCanbeStrengthen(Card card)
        {
            if (!CommonFunc.CheckHaveEquipLevUp(card))
            {
                int count = card.equipInfo.Count;
                for (int i = 0; i != count; i++)
                {
                    EquipInfo info = card.equipInfo[i];
                    if ((info != null) && (ConfigMgr.getInstance().getByEntry<item_config>(info.entry) != null))
                    {
                        int lv = info.lv;
                        int gold = ActorData.getInstance().Gold;
                        int num5 = CommonFunc.GetEquipLvUpBase(lv) + (CommonFunc.GetEquipLvUpGrowLv(lv) * (lv - 1));
                        if (((gold >= num5) && (lv < card.cardInfo.level)) && (lv < ActorData.getInstance().Level))
                        {
                            return info;
                        }
                    }
                }
            }
            return null;
        }

        public static string GetRootFSMStringVarible(string key)
        {
            PlayMakerFSM component = GUIMgr.Instance.Root.GetComponent<PlayMakerFSM>();
            if (null == component)
            {
                return string.Empty;
            }
            FsmString str = component.FsmVariables.FindFsmString(key);
            if (str == null)
            {
                return string.Empty;
            }
            return str.Value;
        }

        public static bool GUIActivited<T>() where T: GUIEntity
        {
            return (null != GUIMgr.Instance.GetActivityGUIEntity<T>());
        }

        public static List<CardOriginal> IterateHeroListEntity(Func<CardOriginal, bool> visitor, bool unique)
        {
            List<CardOriginal> list = new List<CardOriginal>();
            if (visitor != null)
            {
                HeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroPanel>();
                if (null == activityGUIEntity)
                {
                    return list;
                }
                int count = activityGUIEntity.itemsCard.Count;
                for (int i = 0; i != count; i++)
                {
                    GameObject obj2 = activityGUIEntity.itemsCard[i];
                    if (null != obj2)
                    {
                        CardOriginal component = obj2.GetComponent<CardOriginal>();
                        if ((null != obj2) && visitor(component))
                        {
                            list.Add(component);
                            if (unique)
                            {
                                return list;
                            }
                        }
                    }
                }
            }
            return list;
        }

        public static void LockAchievementSwippy(bool locked)
        {
            AchievementPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<AchievementPanel>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("List");
                if (null != transform)
                {
                    UIScrollView component = transform.GetComponent<UIScrollView>();
                    if (null != component)
                    {
                        component.enabled = !locked;
                    }
                }
            }
        }

        public static void LockDailySwippy(bool locked)
        {
            Daily activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<Daily>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("List");
                if (null != transform)
                {
                    UIScrollView component = transform.GetComponent<UIScrollView>();
                    if (null != component)
                    {
                        component.enabled = !locked;
                    }
                }
            }
        }

        public static void LockSwippyOfChangePlayerIconPanel()
        {
            ChangePlayerIconPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChangePlayerIconPanel>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("List");
                if (null != transform)
                {
                    transform.GetComponent<UIScrollView>().enabled = false;
                }
            }
        }

        public static void LockSwippyOfMainScene()
        {
            MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.lockSwippy = true;
            }
        }

        public static void LockSwippyOfSelectHeroPanel()
        {
            SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("Scroll View");
                if (null != transform)
                {
                    transform.GetComponent<UIScrollView>().enabled = false;
                }
            }
        }

        public static void LockTouchOfMainScene(GameObject expl)
        {
        }

        public static void NewbiestLock()
        {
            GUIMgr.Instance.NGUICamera.HighPriorityLocked = true;
            GUIMgr.Instance.NGUIListCamera.HighPriorityLocked = true;
            EasyTouch.instance.HighPriorityLocked = true;
        }

        public static void NewbiestUnlock()
        {
            GUIMgr.Instance.NGUICamera.HighPriorityLocked = false;
            GUIMgr.Instance.NGUIListCamera.HighPriorityLocked = false;
            EasyTouch.instance.HighPriorityLocked = false;
        }

        public static void RegisterMainUILockObject(GameObject go)
        {
            MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.layerLockObject = go;
            }
        }

        public static void RequestChangeHeadIconMaskGeneration()
        {
            if (!GuideSystem.MatchEvent(GuideEvent.HelpMe))
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                ChangePlayerIconPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChangePlayerIconPanel>();
                if (null == activityGUIEntity)
                {
                    GuideSystem.ActivedGuide.RequestCancel();
                }
                else
                {
                    Transform transform = activityGUIEntity.transform.FindChild("List/Grid");
                    if (null == transform)
                    {
                        GuideSystem.ActivedGuide.RequestCancel();
                    }
                    else if (transform.childCount < 1)
                    {
                        GuideSystem.ActivedGuide.RequestCancel();
                    }
                    else
                    {
                        Transform child = transform.GetChild(0);
                        if (null == child)
                        {
                            GuideSystem.ActivedGuide.RequestCancel();
                        }
                        else if (child.childCount < 1)
                        {
                            GuideSystem.ActivedGuide.RequestCancel();
                        }
                        else
                        {
                            Transform transform3 = child.GetChild(0);
                            if (null == transform3)
                            {
                                GuideSystem.ActivedGuide.RequestCancel();
                            }
                            else
                            {
                                GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_HelpMe.tag_helpme_press_change_button, transform3.gameObject);
                            }
                        }
                    }
                }
            }
        }

        public static void RequestHeadIconMaskGeneration()
        {
            MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("TopLeft/PlayerBtn");
                if (null != transform)
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_HelpMe.tag_helpme_press_portal, transform.gameObject);
                }
            }
        }

        public static void RequestItemUsingButtonGeneration()
        {
            BagPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<BagPanel>();
            if (null == activityGUIEntity)
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                Transform transform = activityGUIEntity.transform.FindChild("InfoPanel/DetailsBtn");
                if (null == transform)
                {
                    GuideSystem.ActivedGuide.RequestCancel();
                }
                else
                {
                    GuideSystem.ActivedGuide.RequestGeneration(GuideRegister_Medicine.tag_medicine_press_use_button, transform.gameObject);
                }
            }
        }

        public static void RequestTitleBarFloatButtonGeneration(GuideEvent _event, string tag)
        {
            TitleBar instance = TitleBar.Instance;
            if (null != instance)
            {
                Transform transform = instance.transform.FindChild("TopRight/Group/Btn");
                if (null == transform)
                {
                    NewbiestUnlock();
                    GuideSystem.ActivedGuide.RequestCancel();
                }
                else
                {
                    GuideSystem.ActivedGuide.RequestGeneration(tag, transform.gameObject);
                }
            }
            else
            {
                NewbiestUnlock();
                GuideSystem.ActivedGuide.RequestCancel();
            }
        }

        public static void ResetAllWidgets(GameObject go, UIPanel panel)
        {
            if (null != panel)
            {
                UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>();
                int length = componentsInChildren.Length;
                for (int i = 0; i != length; i++)
                {
                    UIWidget widget = componentsInChildren[i];
                    if (null != widget)
                    {
                        widget.panel = null;
                        widget.CreatePanel();
                    }
                }
            }
        }

        public static void ResetDupMapScroll()
        {
            ActorData.getInstance().DupListVal = 0f;
            DupMap activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<DupMap>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.ResetMapScroll();
            }
        }

        public static E_CardSkill SkillLevelupSlot(Card card)
        {
            int count = card.cardInfo.skillInfo.Count;
            int quality = card.cardInfo.quality;
            int level = card.cardInfo.level;
            for (int i = 0; i != count; i++)
            {
                SkillInfo info = card.cardInfo.skillInfo[i];
                if ((info != null) && (info.skillLevel < level))
                {
                    E_CardSkill skillPos = (E_CardSkill) info.skillPos;
                    if (CommonFunc.SkillIsUnLock(skillPos, quality))
                    {
                        return skillPos;
                    }
                }
            }
            return E_CardSkill.e_card_skill_max;
        }

        public static void TransposeMain3DCamera(Vector3 pos, float speed, System.Action call_back)
        {
            <TransposeMain3DCamera>c__AnonStorey242 storey = new <TransposeMain3DCamera>c__AnonStorey242 {
                call_back = call_back
            };
            GameObject go = GameObject.Find("Camera 3D");
            if (null == go)
            {
                GuideSystem.ActivedGuide.RequestCancel();
            }
            else
            {
                float num = Vector3.Distance(go.transform.position, pos);
                TweenPosition position = TweenPosition.Begin(go, 1f, pos);
                position.duration = num / speed;
                position.SetOnFinished(new EventDelegate.Callback(storey.<>m__50B));
            }
        }

        public static void UnLockSwippyOfChangePlayerIconPanel()
        {
            ChangePlayerIconPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<ChangePlayerIconPanel>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("List");
                if (null != transform)
                {
                    transform.GetComponent<UIScrollView>().enabled = true;
                }
            }
        }

        public static void UnlockSwippyOfMainScene()
        {
            MainUI activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<MainUI>();
            if (null != activityGUIEntity)
            {
                activityGUIEntity.lockSwippy = false;
            }
        }

        public static void UnlockSwippyOfSelectHeroPanel()
        {
            SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
            if (null != activityGUIEntity)
            {
                Transform transform = activityGUIEntity.transform.FindChild("Scroll View");
                if (null != transform)
                {
                    transform.GetComponent<UIScrollView>().enabled = true;
                }
            }
        }

        public static void UnLockTouchOfMainScene()
        {
        }

        [CompilerGenerated]
        private sealed class <TransposeMain3DCamera>c__AnonStorey242
        {
            internal System.Action call_back;

            internal void <>m__50B()
            {
                if (this.call_back != null)
                {
                    this.call_back();
                }
            }
        }
    }
}

