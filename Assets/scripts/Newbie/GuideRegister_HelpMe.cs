namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_HelpMe
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache3;
        [CompilerGenerated]
        private static System.Action <>f__am$cache4;
        [CompilerGenerated]
        private static System.Action <>f__am$cache5;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache6;
        public static string tag_helpme_press_change_button = "tag_helpme_press_change_button";
        public static string tag_helpme_press_head_icon = "tag_helpme_press_head_icon";
        public static string tag_helpme_press_portal = "tag_helpme_press_portal";

        public static GuideController RegisterHelpMeGuide()
        {
            <RegisterHelpMeGuide>c__AnonStorey231 storey = new <RegisterHelpMeGuide>c__AnonStorey231();
            TalkBoxController controller = new TalkBoxController(0x3f);
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            mask.AttachTips(0x40, TipsGizmo.Anchor.BOTTOM_RIGHT);
            storey.ctrl_portal = new FocusController();
            storey.ctrl_portal.Visual = mask;
            storey.ctrl_portal.FSM.valid_tag = tag_helpme_press_portal;
            GuideFSM fSM = storey.ctrl_portal.FSM;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate {
                    Utility.RequestHeadIconMaskGeneration();
                    Utility.NewbiestUnlock();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache3);
            GuideFSM efsm2 = storey.ctrl_portal.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__4B7));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            mask2.AttachTips(0x41, TipsGizmo.Anchor.RIGHT);
            storey.ctrl_button = new FocusController();
            storey.ctrl_button.Visual = mask2;
            storey.ctrl_button.FSM.valid_tag = tag_helpme_press_change_button;
            GuideFSM efsm3 = storey.ctrl_button.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storey.<>m__4B8));
            TalkBoxController controller2 = new TalkBoxController(0x42);
            HangerMask mask3 = new HangerMask();
            mask3.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask3.AttachFingure().EnableHang(true);
            storey.ctrl_head = new FocusController();
            storey.ctrl_head.Visual = mask3;
            storey.ctrl_head.FSM.valid_tag = tag_helpme_press_head_icon;
            GuideFSM efsm4 = storey.ctrl_head.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    Utility.LockSwippyOfChangePlayerIconPanel();
                    Utility.RequestChangeHeadIconMaskGeneration();
                    Utility.NewbiestUnlock();
                };
            }
            efsm4.transition_awake = (System.Action) Delegate.Combine(efsm4.transition_awake, <>f__am$cache4);
            GuideFSM efsm5 = storey.ctrl_head.FSM;
            efsm5.transition_response = (Action<GameObject>) Delegate.Combine(efsm5.transition_response, new Action<GameObject>(storey.<>m__4BA));
            GuideFSM efsm6 = storey.ctrl_head.FSM;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    Utility.UnLockSwippyOfChangePlayerIconPanel();
                    GuideSystem.FinishEvent(GuideEvent.HelpMe);
                    GuideSystem.FireEvent(GuideEvent.Duplicate);
                };
            }
            efsm6.transition_cancel = (System.Action) Delegate.Combine(efsm6.transition_cancel, <>f__am$cache5);
            controller.next_step = storey.ctrl_portal;
            storey.ctrl_portal.next_step = storey.ctrl_button;
            storey.ctrl_button.next_step = controller2;
            controller2.next_step = storey.ctrl_head;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = () => ActorData.getInstance().CardList.Count > 1;
            }
            controller.FSM.condition_reached = <>f__am$cache6;
            return controller;
        }

        [CompilerGenerated]
        private sealed class <RegisterHelpMeGuide>c__AnonStorey231
        {
            internal FocusController ctrl_button;
            internal FocusController ctrl_head;
            internal FocusController ctrl_portal;

            internal void <>m__4B7(GameObject go)
            {
                this.ctrl_portal.GoNext();
            }

            internal void <>m__4B8(GameObject go)
            {
                this.ctrl_button.GoNext();
            }

            internal void <>m__4BA(GameObject go)
            {
                Utility.UnLockSwippyOfChangePlayerIconPanel();
                GuideSystem.FinishEvent(GuideEvent.HelpMe);
                this.ctrl_head.Complete();
                GuideSystem.FireEvent(GuideEvent.Duplicate);
            }
        }
    }
}

