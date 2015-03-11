namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Equip
    {
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<CardOriginal, bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        public static string tag_equip_levelup_confirm = "tag_equip_levelup_confirm";
        public static string tag_equip_levelup_select_equip = "tag_equip_levelup_select_equip";

        public static GuideController RegisterEquipLevelUpPortal()
        {
            TalkBoxController controller = new TalkBoxController(0x2a);
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = () => CommonFunc.RealCheckCardCanLevUp();
            }
            controller.FSM.condition_reached = <>f__am$cache2;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = () => CommonFunc.RealCheckCardCanLevUp();
            }
            GuideController controller2 = GuideRegister_Hero.RegisterHeroPortal(GuideEvent.EquipLevelUp_Portal, <>f__am$cache3);
            controller.next_step = controller2;
            return controller;
        }

        public static GuideController RegisterLevelUpFunction()
        {
            <RegisterLevelUpFunction>c__AnonStorey22D storeyd = new <RegisterLevelUpFunction>c__AnonStorey22D();
            TalkBoxController controller = new TalkBoxController(0x2e);
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            storeyd.ctrl_select_equip = new FocusController();
            storeyd.ctrl_select_equip.Visual = mask;
            storeyd.ctrl_select_equip.FSM.valid_tag = tag_equip_levelup_select_equip;
            GuideFSM fSM = storeyd.ctrl_select_equip.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storeyd.<>m__4A9));
            GuideFSM efsm2 = storeyd.ctrl_select_equip.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storeyd.<>m__4AA));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storeyd.ctrl_confirm = new FocusController();
            storeyd.ctrl_confirm.Visual = mask2;
            storeyd.ctrl_confirm.FSM.valid_tag = tag_equip_levelup_confirm;
            GuideFSM efsm3 = storeyd.ctrl_confirm.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storeyd.<>m__4AB));
            TalkBoxController controller2 = new TalkBoxController(0x2f);
            controller.next_step = storeyd.ctrl_select_equip;
            storeyd.ctrl_select_equip.next_step = storeyd.ctrl_confirm;
            storeyd.ctrl_confirm.next_step = controller2;
            return controller;
        }

        public static GuideController RegisterSelectHero()
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (CardOriginal co) {
                    if (co.ori == null)
                    {
                        return false;
                    }
                    return CommonFunc.CheckHaveEquipLevUp(co.ori);
                };
            }
            Func<CardOriginal, bool> func = <>f__am$cache4;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = () => CommonFunc.RealCheckCardCanLevUp();
            }
            Func<bool> func2 = <>f__am$cache5;
            return GuideRegister_Hero.RegisterSelectHeroGuide(GuideEvent.EquipLevelUp_SelectHero, func2, func);
        }

        [CompilerGenerated]
        private sealed class <RegisterLevelUpFunction>c__AnonStorey22D
        {
            internal FocusController ctrl_confirm;
            internal FocusController ctrl_select_equip;

            internal void <>m__4A9()
            {
                HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    this.ctrl_select_equip.Complete();
                }
                else
                {
                    activityGUIEntity.RequestEquipGeneration();
                }
            }

            internal void <>m__4AA(GameObject go)
            {
                this.ctrl_select_equip.GoNext();
            }

            internal void <>m__4AB(GameObject go)
            {
                GuideSystem.FinishEvent(GuideEvent.EquipLevelUp_Function);
                this.ctrl_confirm.GoNext();
            }
        }
    }
}

