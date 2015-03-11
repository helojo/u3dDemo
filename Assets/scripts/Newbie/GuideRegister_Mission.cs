namespace Newbie
{
    using FastBuf;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Mission
    {
        [CompilerGenerated]
        private static Action<List<GameObject>> <>f__am$cache3;
        [CompilerGenerated]
        private static System.Action <>f__am$cache4;
        [CompilerGenerated]
        private static System.Action <>f__am$cache5;
        [CompilerGenerated]
        private static Action<List<GameObject>> <>f__am$cache6;
        [CompilerGenerated]
        private static System.Action <>f__am$cache7;
        [CompilerGenerated]
        private static System.Action <>f__am$cache8;
        public static string tag_mission_award_button = "tag_mission_award_button";
        public static string tag_mission_portal_press_button = "tag_mission_portal_press_button";
        public static string tag_mission_press_floatbtn = "tag_mission_press_floatbtn";

        public static GuideController RegisterMissionPortal(GuideEvent _event, int mission_entry)
        {
            <RegisterMissionPortal>c__AnonStorey236 storey = new <RegisterMissionPortal>c__AnonStorey236 {
                _event = _event,
                mission_entry = mission_entry
            };
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().EnableHang(true);
            storey.ctrl_floatbtn = new FocusController();
            storey.ctrl_floatbtn.Visual = mask;
            storey.ctrl_floatbtn.FSM.valid_tag = tag_mission_press_floatbtn;
            GuideFSM fSM = storey.ctrl_floatbtn.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storey.<>m__4D1));
            GuideFSM efsm2 = storey.ctrl_floatbtn.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__4D2));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storey.ctrl_portal = new FocusController();
            storey.ctrl_portal.Visual = mask2;
            storey.ctrl_portal.FSM.valid_tag = tag_mission_portal_press_button;
            GuideFSM efsm3 = storey.ctrl_portal.FSM;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (List<GameObject> gen_list) {
                    Utility.NewbiestUnlock();
                };
            }
            efsm3.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm3.transition_generate, <>f__am$cache3);
            GuideFSM efsm4 = storey.ctrl_portal.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storey.<>m__4D4));
            storey.ctrl_portal_immidiate = new FocusController();
            storey.ctrl_portal_immidiate.Visual = mask2;
            storey.ctrl_portal_immidiate.FSM.valid_tag = tag_mission_portal_press_button;
            GuideFSM efsm5 = storey.ctrl_portal_immidiate.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                    if (null != activityGUIEntity)
                    {
                        activityGUIEntity.RequestMissionGeneration();
                    }
                    Utility.NewbiestUnlock();
                };
            }
            efsm5.transition_awake = (System.Action) Delegate.Combine(efsm5.transition_awake, <>f__am$cache4);
            GuideFSM efsm6 = storey.ctrl_portal_immidiate.FSM;
            efsm6.transition_response = (Action<GameObject>) Delegate.Combine(efsm6.transition_response, new Action<GameObject>(storey.<>m__4D6));
            Focus2DMask mask3 = new Focus2DMask();
            mask3.AttachFingure();
            storey.ctrl_award = new FocusController();
            storey.ctrl_award.Visual = mask3;
            storey.ctrl_award.FSM.valid_tag = tag_mission_award_button;
            GuideFSM efsm7 = storey.ctrl_award.FSM;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    GUIMgr.Instance.ExitModelGUIImmediate("CardBreakSuccessPanel");
                };
            }
            efsm7.transition_awake = (System.Action) Delegate.Combine(efsm7.transition_awake, <>f__am$cache5);
            GuideFSM efsm8 = storey.ctrl_award.FSM;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = delegate (List<GameObject> gen_list) {
                    Utility.LockAchievementSwippy(true);
                };
            }
            efsm8.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm8.transition_generate, <>f__am$cache6);
            GuideFSM efsm9 = storey.ctrl_award.FSM;
            efsm9.transition_response = (Action<GameObject>) Delegate.Combine(efsm9.transition_response, new Action<GameObject>(storey.<>m__4D9));
            GuideFSM efsm10 = storey.ctrl_award.FSM;
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate {
                    Utility.LockAchievementSwippy(false);
                    Utility.NewbiestUnlock();
                };
            }
            efsm10.transition_cancel = (System.Action) Delegate.Combine(efsm10.transition_cancel, <>f__am$cache7);
            storey.ctrl_talk0 = new TalkBoxController(0x31);
            GuideFSM efsm11 = storey.ctrl_talk0.FSM;
            efsm11.transition_response = (Action<GameObject>) Delegate.Combine(efsm11.transition_response, new Action<GameObject>(storey.<>m__4DB));
            TalkBoxController controller = new TalkBoxController(0x4a);
            GuideFSM efsm12 = controller.FSM;
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate {
                    GUIMgr.Instance.ExitModelGUIImmediate("CardBreakSuccessPanel");
                };
            }
            efsm12.transition_awake = (System.Action) Delegate.Combine(efsm12.transition_awake, <>f__am$cache8);
            controller.FSM.condition_reached = new Func<bool>(storey.<>m__4DD);
            storey.ctrl_fork = new FlexiableController();
            GuideFSM efsm13 = storey.ctrl_fork.FSM;
            efsm13.transition_awake = (System.Action) Delegate.Combine(efsm13.transition_awake, new System.Action(storey.<>m__4DE));
            controller.next_step = storey.ctrl_fork;
            storey.ctrl_floatbtn.next_step = storey.ctrl_portal;
            storey.ctrl_portal.next_step = storey.ctrl_award;
            storey.ctrl_portal_immidiate.next_step = storey.ctrl_award;
            storey.ctrl_award.next_step = storey.ctrl_talk0;
            return controller;
        }

        [CompilerGenerated]
        private sealed class <RegisterMissionPortal>c__AnonStorey236
        {
            internal GuideEvent _event;
            internal FocusController ctrl_award;
            internal FocusController ctrl_floatbtn;
            internal FlexiableController ctrl_fork;
            internal FocusController ctrl_portal;
            internal FocusController ctrl_portal_immidiate;
            internal TalkBoxController ctrl_talk0;
            internal int mission_entry;

            internal void <>m__4D1()
            {
                Utility.RequestTitleBarFloatButtonGeneration(this._event, GuideRegister_Mission.tag_mission_press_floatbtn);
            }

            internal void <>m__4D2(GameObject go)
            {
                this.ctrl_floatbtn.GoNext();
                Utility.NewbiestLock();
            }

            internal void <>m__4D4(GameObject go)
            {
                this.ctrl_portal.GoNext();
            }

            internal void <>m__4D6(GameObject go)
            {
                this.ctrl_portal_immidiate.GoNext();
            }

            internal void <>m__4D9(GameObject go)
            {
                Utility.LockAchievementSwippy(false);
                GuideSystem.FinishEvent(this._event);
                this.ctrl_award.GoNext();
            }

            internal void <>m__4DB(GameObject go)
            {
                this.ctrl_talk0.Complete();
            }

            internal bool <>m__4DD()
            {
                if (ActorData.getInstance().QuestList.Find(e => this.mission_entry == e.entry) == null)
                {
                    GuideSystem.FinishEvent(this._event);
                    return false;
                }
                return true;
            }

            internal void <>m__4DE()
            {
                if (CommonFunc.IsExpanFuncBar())
                {
                    this.ctrl_fork.next_step = this.ctrl_portal_immidiate;
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_floatbtn;
                }
                this.ctrl_fork.GoNext();
            }

            internal bool <>m__4DF(Quest e)
            {
                return (this.mission_entry == e.entry);
            }
        }
    }
}

