namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Medicine
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache3;
        [CompilerGenerated]
        private static System.Action <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        [CompilerGenerated]
        private static System.Action <>f__am$cache6;
        [CompilerGenerated]
        private static System.Action <>f__am$cache7;
        public static string tag_medicine_portal_press_floatbtn = "tag_medicine_portal_press_floatbtn";
        public static string tag_medicine_press_function_button = "tag_medicine_press_function_button";
        public static string tag_medicine_press_use_button = "tag_medicine_press_use_button";

        public static GuideController RegisterMedicinePortalGuide()
        {
            <RegisterMedicinePortalGuide>c__AnonStorey234 storey = new <RegisterMedicinePortalGuide>c__AnonStorey234();
            TalkBoxController controller = new TalkBoxController(0x3b);
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().EnableHang(true);
            storey.ctrl_floatbtn = new FocusController();
            storey.ctrl_floatbtn.Visual = mask;
            storey.ctrl_floatbtn.FSM.valid_tag = tag_medicine_portal_press_floatbtn;
            GuideFSM fSM = storey.ctrl_floatbtn.FSM;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate {
                    Utility.RequestTitleBarFloatButtonGeneration(GuideEvent.Medicine_Portal, tag_medicine_portal_press_floatbtn);
                    Utility.NewbiestUnlock();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache3);
            GuideFSM efsm2 = storey.ctrl_floatbtn.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__4C8));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storey.ctrl_portal = new FocusController();
            storey.ctrl_portal.Visual = mask2;
            storey.ctrl_portal.FSM.valid_tag = tag_medicine_press_function_button;
            GuideFSM efsm3 = storey.ctrl_portal.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storey.<>m__4C9));
            storey.ctrl_portal_immdiate = new FocusController();
            storey.ctrl_portal_immdiate.Visual = mask2;
            storey.ctrl_portal_immdiate.FSM.valid_tag = tag_medicine_press_function_button;
            GuideFSM efsm4 = storey.ctrl_portal_immdiate.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                    if (null != activityGUIEntity)
                    {
                        activityGUIEntity.RequestBagGeneration();
                    }
                    Utility.NewbiestUnlock();
                };
            }
            efsm4.transition_awake = (System.Action) Delegate.Combine(efsm4.transition_awake, <>f__am$cache4);
            GuideFSM efsm5 = storey.ctrl_portal_immdiate.FSM;
            efsm5.transition_response = (Action<GameObject>) Delegate.Combine(efsm5.transition_response, new Action<GameObject>(storey.<>m__4CB));
            storey.ctrl_fork = new FlexiableController();
            GuideFSM efsm6 = storey.ctrl_fork.FSM;
            efsm6.transition_awake = (System.Action) Delegate.Combine(efsm6.transition_awake, new System.Action(storey.<>m__4CC));
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = () => Utility.CheckMedicineExistly();
            }
            controller.FSM.condition_reached = <>f__am$cache5;
            controller.next_step = storey.ctrl_fork;
            storey.ctrl_floatbtn.next_step = storey.ctrl_portal;
            return controller;
        }

        public static GuideController RegisterUseMedicineGuide()
        {
            <RegisterUseMedicineGuide>c__AnonStorey235 storey = new <RegisterUseMedicineGuide>c__AnonStorey235 {
                ctrl_talk0 = new TalkBoxController(60)
            };
            GuideFSM fSM = storey.ctrl_talk0.FSM;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = delegate {
                    GuideSystem.FinishEvent(GuideEvent.Medicine_Using);
                    Utility.ForceSwitchBagPage();
                    Utility.ForceEnableBagPageDraggable(false);
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache6);
            GuideFSM efsm2 = storey.ctrl_talk0.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__4CF));
            GuideFSM efsm3 = storey.ctrl_talk0.FSM;
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate {
                    Utility.ForceEnableBagPageDraggable(true);
                };
            }
            efsm3.transition_cancel = (System.Action) Delegate.Combine(efsm3.transition_cancel, <>f__am$cache7);
            return storey.ctrl_talk0;
        }

        [CompilerGenerated]
        private sealed class <RegisterMedicinePortalGuide>c__AnonStorey234
        {
            internal FocusController ctrl_floatbtn;
            internal FlexiableController ctrl_fork;
            internal FocusController ctrl_portal;
            internal FocusController ctrl_portal_immdiate;

            internal void <>m__4C8(GameObject go)
            {
                this.ctrl_floatbtn.GoNext();
                Utility.NewbiestUnlock();
            }

            internal void <>m__4C9(GameObject go)
            {
                this.ctrl_portal.Complete();
                GuideSystem.FinishEvent(GuideEvent.Medicine_Portal);
            }

            internal void <>m__4CB(GameObject go)
            {
                this.ctrl_portal_immdiate.Complete();
                GuideSystem.FinishEvent(GuideEvent.Medicine_Portal);
            }

            internal void <>m__4CC()
            {
                if (CommonFunc.IsExpanFuncBar())
                {
                    this.ctrl_fork.next_step = this.ctrl_portal_immdiate;
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_floatbtn;
                }
                this.ctrl_fork.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterUseMedicineGuide>c__AnonStorey235
        {
            internal TalkBoxController ctrl_talk0;

            internal void <>m__4CF(GameObject go)
            {
                this.ctrl_talk0.Complete();
                Utility.ForceEnableBagPageDraggable(true);
            }
        }
    }
}

