namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Daily
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
        public static string tag_daily_portal_press_button = "tag_daily_portal_press_button";
        public static string tag_daily_press_commit_button = "tag_daily_press_commit_button";
        public static string tag_daily_press_floatbutton = "tag_daily_press_floatbutton";

        public static GuideController RegisterDailyPortal(GuideEvent _event)
        {
            <RegisterDailyPortal>c__AnonStorey22C storeyc = new <RegisterDailyPortal>c__AnonStorey22C {
                _event = _event
            };
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().EnableHang(true);
            storeyc.ctrl_floatbtn = new FocusController();
            storeyc.ctrl_floatbtn.Visual = mask;
            storeyc.ctrl_floatbtn.FSM.valid_tag = tag_daily_press_floatbutton;
            GuideFSM fSM = storeyc.ctrl_floatbtn.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storeyc.<>m__497));
            GuideFSM efsm2 = storeyc.ctrl_floatbtn.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storeyc.<>m__498));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storeyc.ctrl_portal = new FocusController();
            storeyc.ctrl_portal.Visual = mask2;
            storeyc.ctrl_portal.FSM.valid_tag = tag_daily_portal_press_button;
            GuideFSM efsm3 = storeyc.ctrl_portal.FSM;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (List<GameObject> gen_list) {
                    Utility.NewbiestUnlock();
                };
            }
            efsm3.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm3.transition_generate, <>f__am$cache3);
            GuideFSM efsm4 = storeyc.ctrl_portal.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storeyc.<>m__49A));
            storeyc.ctrl_portal_immidiate = new FocusController();
            storeyc.ctrl_portal_immidiate.Visual = mask2;
            storeyc.ctrl_portal_immidiate.FSM.valid_tag = tag_daily_portal_press_button;
            GuideFSM efsm5 = storeyc.ctrl_portal_immidiate.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                    if (null != activityGUIEntity)
                    {
                        activityGUIEntity.RequestDailyGeneration();
                    }
                    Utility.NewbiestUnlock();
                };
            }
            efsm5.transition_awake = (System.Action) Delegate.Combine(efsm5.transition_awake, <>f__am$cache4);
            GuideFSM efsm6 = storeyc.ctrl_portal_immidiate.FSM;
            efsm6.transition_response = (Action<GameObject>) Delegate.Combine(efsm6.transition_response, new Action<GameObject>(storeyc.<>m__49C));
            Focus2DMask mask3 = new Focus2DMask();
            mask3.AttachFingure();
            storeyc.ctrl_award = new FocusController();
            storeyc.ctrl_award.Visual = mask3;
            storeyc.ctrl_award.FSM.valid_tag = tag_daily_press_commit_button;
            GuideFSM efsm7 = storeyc.ctrl_award.FSM;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    GUIMgr.Instance.ExitModelGUIImmediate("CardBreakSuccessPanel");
                };
            }
            efsm7.transition_awake = (System.Action) Delegate.Combine(efsm7.transition_awake, <>f__am$cache5);
            GuideFSM efsm8 = storeyc.ctrl_award.FSM;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = delegate (List<GameObject> gen_list) {
                    Utility.LockDailySwippy(true);
                };
            }
            efsm8.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm8.transition_generate, <>f__am$cache6);
            GuideFSM efsm9 = storeyc.ctrl_award.FSM;
            efsm9.transition_response = (Action<GameObject>) Delegate.Combine(efsm9.transition_response, new Action<GameObject>(storeyc.<>m__49F));
            GuideFSM efsm10 = storeyc.ctrl_award.FSM;
            if (<>f__am$cache7 == null)
            {
                <>f__am$cache7 = delegate {
                    Utility.LockDailySwippy(false);
                    Utility.NewbiestUnlock();
                };
            }
            efsm10.transition_cancel = (System.Action) Delegate.Combine(efsm10.transition_cancel, <>f__am$cache7);
            storeyc.ctrl_talk0 = new TalkBoxController(0x58);
            GuideFSM efsm11 = storeyc.ctrl_talk0.FSM;
            efsm11.transition_response = (Action<GameObject>) Delegate.Combine(efsm11.transition_response, new Action<GameObject>(storeyc.<>m__4A1));
            TalkBoxController controller = new TalkBoxController(0x57);
            GuideFSM efsm12 = controller.FSM;
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate {
                    GUIMgr.Instance.ExitModelGUIImmediate("CardBreakSuccessPanel");
                };
            }
            efsm12.transition_awake = (System.Action) Delegate.Combine(efsm12.transition_awake, <>f__am$cache8);
            controller.FSM.condition_reached = new Func<bool>(storeyc.<>m__4A3);
            storeyc.ctrl_fork = new FlexiableController();
            GuideFSM efsm13 = storeyc.ctrl_fork.FSM;
            efsm13.transition_awake = (System.Action) Delegate.Combine(efsm13.transition_awake, new System.Action(storeyc.<>m__4A4));
            controller.next_step = storeyc.ctrl_fork;
            storeyc.ctrl_floatbtn.next_step = storeyc.ctrl_portal;
            storeyc.ctrl_portal.next_step = storeyc.ctrl_award;
            storeyc.ctrl_portal_immidiate.next_step = storeyc.ctrl_award;
            storeyc.ctrl_award.next_step = storeyc.ctrl_talk0;
            return controller;
        }

        [CompilerGenerated]
        private sealed class <RegisterDailyPortal>c__AnonStorey22C
        {
            internal GuideEvent _event;
            internal FocusController ctrl_award;
            internal FocusController ctrl_floatbtn;
            internal FlexiableController ctrl_fork;
            internal FocusController ctrl_portal;
            internal FocusController ctrl_portal_immidiate;
            internal TalkBoxController ctrl_talk0;

            internal void <>m__497()
            {
                Utility.RequestTitleBarFloatButtonGeneration(this._event, GuideRegister_Daily.tag_daily_press_floatbutton);
            }

            internal void <>m__498(GameObject go)
            {
                this.ctrl_floatbtn.GoNext();
                Utility.NewbiestLock();
            }

            internal void <>m__49A(GameObject go)
            {
                this.ctrl_portal.GoNext();
            }

            internal void <>m__49C(GameObject go)
            {
                this.ctrl_portal_immidiate.GoNext();
            }

            internal void <>m__49F(GameObject go)
            {
                Utility.LockDailySwippy(false);
                GuideSystem.FinishEvent(this._event);
                this.ctrl_award.GoNext();
            }

            internal void <>m__4A1(GameObject go)
            {
                this.ctrl_talk0.Complete();
            }

            internal bool <>m__4A3()
            {
                if (ActorData.getInstance().haveReceiveDailyReward)
                {
                    GuideSystem.FinishEvent(this._event);
                    return false;
                }
                return true;
            }

            internal void <>m__4A4()
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
        }
    }
}

