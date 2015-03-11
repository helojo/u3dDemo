namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Strengthen
    {
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<CardOriginal, bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        public static string tag_equip_strengthen_function = "tag_equip_strengthen_function";
        public static string tag_equip_strengthen_select_equip = "tag_equip_strengthen_select_equip";

        public static GuideController RegisterStrengthenFunction()
        {
            <RegisterStrengthenFunction>c__AnonStorey23D storeyd = new <RegisterStrengthenFunction>c__AnonStorey23D();
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            storeyd.ctrl_select_equip = new FocusController();
            storeyd.ctrl_select_equip.Visual = mask;
            storeyd.ctrl_select_equip.FSM.valid_tag = tag_equip_strengthen_select_equip;
            GuideFSM fSM = storeyd.ctrl_select_equip.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storeyd.<>m__4F8));
            GuideFSM efsm2 = storeyd.ctrl_select_equip.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storeyd.<>m__4F9));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storeyd.ctrl_function = new FocusController();
            storeyd.ctrl_function.Visual = mask2;
            storeyd.ctrl_function.FSM.valid_tag = tag_equip_strengthen_function;
            GuideFSM efsm3 = storeyd.ctrl_function.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storeyd.<>m__4FA));
            TalkBoxController controller = new TalkBoxController(0x3e);
            storeyd.ctrl_select_equip.next_step = storeyd.ctrl_function;
            storeyd.ctrl_function.next_step = controller;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    if (Utility.CheckEquipHasStrengthen())
                    {
                        GuideSystem.SkipToEvent(GuideEvent.Function_Shop, true);
                        return false;
                    }
                    return true;
                };
            }
            storeyd.ctrl_select_equip.FSM.condition_reached = <>f__am$cache5;
            return storeyd.ctrl_select_equip;
        }

        public static GuideController RegisterStrengthenPortal()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate {
                    if (GUIMgr.Instance.ListRoot.gameObject.activeSelf)
                    {
                        return false;
                    }
                    if (Utility.CheckEquipHasStrengthen())
                    {
                        GuideSystem.SkipToEvent(GuideEvent.Function_Shop, true);
                        return false;
                    }
                    return Utility.CheckHaveEquipCanbeStrengthen();
                };
            }
            Func<bool> func = <>f__am$cache2;
            GuideController controller = GuideRegister_Hero.RegisterHeroPortal(GuideEvent.Strengthen_Portal, func);
            return new TalkBoxController(0x3d) { FSM = { condition_reached = func }, next_step = controller };
        }

        public static GuideController RegisterStrengthenSelectHero()
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (CardOriginal co) {
                    if (co.ori == null)
                    {
                        return false;
                    }
                    return null != Utility.GetEquipCanbeStrengthen(co.ori);
                };
            }
            Func<CardOriginal, bool> func = <>f__am$cache3;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = () => Utility.CheckHaveEquipCanbeStrengthen();
            }
            Func<bool> func2 = <>f__am$cache4;
            return GuideRegister_Hero.RegisterSelectHeroGuide(GuideEvent.Strengthen_SelectHero, func2, func);
        }

        [CompilerGenerated]
        private sealed class <RegisterStrengthenFunction>c__AnonStorey23D
        {
            internal FocusController ctrl_function;
            internal FocusController ctrl_select_equip;

            internal void <>m__4F8()
            {
                HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    this.ctrl_select_equip.Complete();
                }
                else
                {
                    activityGUIEntity.RequstStrengthenGeneration();
                }
            }

            internal void <>m__4F9(GameObject go)
            {
                this.ctrl_select_equip.GoNext();
            }

            internal void <>m__4FA(GameObject go)
            {
                GuideSystem.FinishEvent(GuideEvent.Strengthen_Function);
                this.ctrl_function.GoNext();
            }
        }
    }
}

