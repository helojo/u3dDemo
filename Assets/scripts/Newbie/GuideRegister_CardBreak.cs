namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_CardBreak
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache2;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<CardOriginal, bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache6;
        public static string tag_cardbreak_confirm = "tag_cardbreak_confirm";
        public static string tag_cardbreak_press_break_button = "tag_cardbreak_press_break_button";

        public static GuideController RegisterCardBreakGuide()
        {
            <RegisterCardBreakGuide>c__AnonStorey22B storeyb = new <RegisterCardBreakGuide>c__AnonStorey22B();
            TalkBoxController controller = new TalkBoxController(0x34);
            GuideFSM fSM = controller.FSM;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate {
                    GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache2);
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            mask.AttachTips(0x35, TipsGizmo.Anchor.TOP);
            storeyb.ctrl_button = new FocusController();
            storeyb.ctrl_button.Visual = mask;
            storeyb.ctrl_button.FSM.valid_tag = tag_cardbreak_press_break_button;
            GuideFSM efsm2 = storeyb.ctrl_button.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storeyb.<>m__48D));
            GuideFSM efsm3 = storeyb.ctrl_button.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storeyb.<>m__48E));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storeyb.ctrl_confirm = new FocusController();
            storeyb.ctrl_confirm.Visual = mask2;
            storeyb.ctrl_confirm.FSM.valid_tag = tag_cardbreak_confirm;
            GuideFSM efsm4 = storeyb.ctrl_confirm.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storeyb.<>m__48F));
            TalkBoxController controller2 = new TalkBoxController(0x36);
            controller.next_step = storeyb.ctrl_button;
            storeyb.ctrl_button.next_step = storeyb.ctrl_confirm;
            storeyb.ctrl_confirm.next_step = controller2;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate {
                    HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                    if (null == activityGUIEntity)
                    {
                        return false;
                    }
                    if (activityGUIEntity.mCard == null)
                    {
                        return false;
                    }
                    return CommonFunc.CheckCardCanBreak(activityGUIEntity.mCard);
                };
            }
            controller.FSM.condition_reached = <>f__am$cache3;
            return controller;
        }

        public static GuideController RegisterCardBreakPortalGuide()
        {
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = () => CommonFunc.CheckHaveCardCanBreak();
            }
            return GuideRegister_Hero.RegisterHeroPortal(GuideEvent.CardBreak_Portal, <>f__am$cache6);
        }

        public static GuideController RegisterSelectHeroGuide()
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (CardOriginal co) {
                    if (co.ori == null)
                    {
                        return false;
                    }
                    return CommonFunc.CheckCardCanBreak(co.ori);
                };
            }
            Func<CardOriginal, bool> func = <>f__am$cache4;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = () => CommonFunc.CheckHaveCardCanBreak();
            }
            Func<bool> func2 = <>f__am$cache5;
            return GuideRegister_Hero.RegisterSelectHeroGuide(GuideEvent.CardBreak_SelectHero, func2, func);
        }

        [CompilerGenerated]
        private sealed class <RegisterCardBreakGuide>c__AnonStorey22B
        {
            internal FocusController ctrl_button;
            internal FocusController ctrl_confirm;

            internal void <>m__48D()
            {
                GUIMgr.Instance.ExitModelGUIImmediate("MessageBox");
                HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    this.ctrl_button.Complete();
                }
                else
                {
                    activityGUIEntity.RequestCardBreakGeneration();
                }
            }

            internal void <>m__48E(GameObject go)
            {
                this.ctrl_button.GoNext();
            }

            internal void <>m__48F(GameObject go)
            {
                GuideSystem.FinishEvent(GuideEvent.CardBreak_Function);
                this.ctrl_confirm.GoNext();
            }
        }
    }
}

