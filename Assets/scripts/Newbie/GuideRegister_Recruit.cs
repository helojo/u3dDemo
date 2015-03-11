namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Recruit
    {
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache3;
        [CompilerGenerated]
        private static System.Action <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        [CompilerGenerated]
        private static System.Action <>f__am$cache6;
        public static string tag_recruit_comeback = "tag_recruit_comeback";
        public static string tag_recruit_press_button = "tag_recruit_press_button";
        public static string tag_recruit_select_building = "tag_recruit_select_building";

        private static GuideController GenComebackController()
        {
            <GenComebackController>c__AnonStorey23B storeyb = new <GenComebackController>c__AnonStorey23B();
            Focus2DMask mask = new Focus2DMask();
            mask.AttachTips(0x20, TipsGizmo.Anchor.BOTTOM_RIGHT).SetOffset(new Vector3(0f, 20f, 0f));
            mask.AttachFingure();
            storeyb.ctrl_comeback_mask = new FocusController();
            storeyb.ctrl_comeback_mask.Visual = mask;
            storeyb.ctrl_comeback_mask.FSM.valid_tag = tag_recruit_comeback;
            GuideFSM fSM = storeyb.ctrl_comeback_mask.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storeyb.<>m__4E9));
            return storeyb.ctrl_comeback_mask;
        }

        private static GuideController GenPressButtonController(GuideEvent _event)
        {
            <GenPressButtonController>c__AnonStorey23A storeya = new <GenPressButtonController>c__AnonStorey23A {
                _event = _event
            };
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            mask.AttachTips(0x1f, TipsGizmo.Anchor.TOP).SetOffset(new Vector3(0f, 10f, 0f));
            storeya.ctrl_button_mask = new FocusController();
            storeya.ctrl_button_mask.Visual = mask;
            storeya.ctrl_button_mask.FSM.valid_tag = tag_recruit_press_button;
            GuideFSM fSM = storeya.ctrl_button_mask.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storeya.<>m__4E8));
            return storeya.ctrl_button_mask;
        }

        private static GuideController GenSelectBuildingController()
        {
            <GenSelectBuildingController>c__AnonStorey239 storey = new <GenSelectBuildingController>c__AnonStorey239 {
                building_mask = new Focus3DMask()
            };
            storey.building_mask.SetExtractFlag(FocusMask.ExtractFlag.PerVertex, true);
            storey.building_mask.AttachFingure();
            storey.building_mask.AttachTips(30, TipsGizmo.Anchor.LEFT);
            storey.ctrl_building_mask = new FocusController();
            storey.ctrl_building_mask.Visual = storey.building_mask;
            storey.ctrl_building_mask.FSM.valid_tag = tag_recruit_select_building;
            GuideFSM fSM = storey.ctrl_building_mask.FSM;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = delegate {
                    RequestBuildingGeneration();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache6);
            GuideFSM efsm2 = storey.ctrl_building_mask.FSM;
            efsm2.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm2.transition_generate, new Action<List<GameObject>>(storey.<>m__4E6));
            GuideFSM efsm3 = storey.ctrl_building_mask.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storey.<>m__4E7));
            return storey.ctrl_building_mask;
        }

        public static GuideController RegisterGoldRecruit()
        {
            <RegisterGoldRecruit>c__AnonStorey238 storey = new <RegisterGoldRecruit>c__AnonStorey238();
            TalkBoxController controller = new TalkBoxController(0x1b);
            GuideFSM fSM = controller.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    Utility.LockSwippyOfMainScene();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache4);
            TalkBoxController controller2 = new TalkBoxController(0x1c);
            TalkBoxController controller3 = new TalkBoxController(0x1d);
            storey.ctrl_select_building = GenSelectBuildingController();
            storey.ctrl_press_button = GenPressButtonController(GuideEvent.GoldRecruit);
            GuideController controller4 = GenComebackController();
            storey.ctrl_fork = new FlexiableController();
            GuideFSM efsm2 = storey.ctrl_fork.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storey.<>m__4E3));
            controller.next_step = controller2;
            controller2.next_step = controller3;
            controller3.next_step = storey.ctrl_fork;
            storey.ctrl_select_building.next_step = storey.ctrl_press_button;
            storey.ctrl_press_button.next_step = controller4;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    if (GameDataMgr.Instance.boostRecruit.valid)
                    {
                        long time = 0L;
                        if (!GameDataMgr.Instance.boostRecruit.FreeTime(2, out time))
                        {
                            GuideSystem.SkipToEvent(GuideEvent.StoneRecruit, true);
                            return false;
                        }
                        if (GameDataMgr.Instance.boostRecruit.FreeCount(2) <= 0)
                        {
                            GuideSystem.SkipToEvent(GuideEvent.StoneRecruit, true);
                            return false;
                        }
                    }
                    return true;
                };
            }
            controller.FSM.condition_reached = <>f__am$cache5;
            return controller;
        }

        public static GuideController RegisterStoneRecruit()
        {
            <RegisterStoneRecruit>c__AnonStorey237 storey = new <RegisterStoneRecruit>c__AnonStorey237 {
                ctrl_select_building = GenSelectBuildingController(),
                ctrl_press_button = GenPressButtonController(GuideEvent.StoneRecruit)
            };
            GuideController controller = GenComebackController();
            storey.ctrl_fork = new FlexiableController();
            GuideFSM fSM = storey.ctrl_fork.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storey.<>m__4E0));
            storey.ctrl_select_building.next_step = storey.ctrl_press_button;
            storey.ctrl_press_button.next_step = controller;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate {
                    if (GameDataMgr.Instance.boostRecruit.valid)
                    {
                        long time = 0L;
                        if (!GameDataMgr.Instance.boostRecruit.FreeTime(0, out time))
                        {
                            GuideSystem.SkipToEvent(GuideEvent.HelpMe, true);
                            return false;
                        }
                    }
                    return true;
                };
            }
            storey.ctrl_fork.FSM.condition_reached = <>f__am$cache3;
            return storey.ctrl_fork;
        }

        private static void RequestBuildingGeneration()
        {
            List<GameObject> vari = new List<GameObject> {
                GameObject.Find("GameObject/cj_xzjm2/jiuguan"),
                GameObject.Find("cj_zjm_texiao/Effects2/cj_xzjm2_dengguang_tx")
            };
            GuideSystem.ActivedGuide.RequestMultiGeneration(tag_recruit_select_building, vari);
        }

        [CompilerGenerated]
        private sealed class <GenComebackController>c__AnonStorey23B
        {
            internal FocusController ctrl_comeback_mask;

            internal void <>m__4E9(GameObject obj)
            {
                this.ctrl_comeback_mask.Complete();
            }
        }

        [CompilerGenerated]
        private sealed class <GenPressButtonController>c__AnonStorey23A
        {
            internal GuideEvent _event;
            internal FocusController ctrl_button_mask;

            internal void <>m__4E8(GameObject obj)
            {
                GuideSystem.FinishEvent(this._event);
                Utility.EnforceClear();
                Utility.NewbiestLock();
                this.ctrl_button_mask.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <GenSelectBuildingController>c__AnonStorey239
        {
            internal Focus3DMask building_mask;
            internal FocusController ctrl_building_mask;

            internal void <>m__4E6(List<GameObject> lst)
            {
                GameObject obj2 = Utility.FindMainUIModuleTipsObjectByKey("Ui_Main_jiuguan");
                if (null != obj2)
                {
                    List<GameObject> objects = new List<GameObject> {
                        obj2
                    };
                    this.building_mask.AttachImage2DTips(objects, true);
                }
            }

            internal void <>m__4E7(GameObject obj)
            {
                Utility.UnlockSwippyOfMainScene();
                this.ctrl_building_mask.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterGoldRecruit>c__AnonStorey238
        {
            private static System.Action <>f__am$cache3;
            internal FlexiableController ctrl_fork;
            internal GuideController ctrl_press_button;
            internal GuideController ctrl_select_building;

            internal void <>m__4E3()
            {
                GUIMgr.Instance.ExitModelGUIImmediate("PlayerInfoPanel");
                if (Utility.GUIActivited<RecruitPanel>())
                {
                    GuideFSM fSM = this.ctrl_press_button.FSM;
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = () => RecruitPanel.NewbieGenerate();
                    }
                    fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache3);
                    this.ctrl_fork.next_step = this.ctrl_press_button;
                    this.ctrl_fork.GoNext();
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_select_building;
                    this.ctrl_fork.GoNext();
                }
            }

            private static void <>m__4EB()
            {
                RecruitPanel.NewbieGenerate();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterStoneRecruit>c__AnonStorey237
        {
            private static System.Action <>f__am$cache3;
            internal FlexiableController ctrl_fork;
            internal GuideController ctrl_press_button;
            internal GuideController ctrl_select_building;

            internal void <>m__4E0()
            {
                GUIMgr.Instance.ExitModelGUIImmediate("PlayerInfoPanel");
                if (Utility.GUIActivited<RecruitPanel>())
                {
                    GuideFSM fSM = this.ctrl_press_button.FSM;
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = () => RecruitPanel.NewbieGenerate();
                    }
                    fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache3);
                    this.ctrl_fork.next_step = this.ctrl_press_button;
                    this.ctrl_fork.GoNext();
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_select_building;
                    this.ctrl_fork.GoNext();
                }
            }

            private static void <>m__4EA()
            {
                RecruitPanel.NewbieGenerate();
            }
        }
    }
}

