namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Battle
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache10;
        [CompilerGenerated]
        private static System.Action <>f__am$cache11;
        [CompilerGenerated]
        private static System.Action <>f__am$cache8;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache9;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cacheA;
        [CompilerGenerated]
        private static System.Action <>f__am$cacheB;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cacheC;
        [CompilerGenerated]
        private static System.Action <>f__am$cacheD;
        [CompilerGenerated]
        private static System.Action <>f__am$cacheE;
        [CompilerGenerated]
        private static System.Action <>f__am$cacheF;
        public static string tag_battle_select_building = "tag_battle_select_building";
        public static string tag_battle_select_duplicate = "tag_battle_select_duplicate";
        public static string tag_battle_select_trench = "tag_battle_select_trench";
        public static string tag_battle_use_skill = "tag_battle_cast_skill";
        public static string tag_formation_select_hero = "tag_formation_select_hero";
        public static string tag_select_assist = "tag_select_assist";
        public static string tag_select_elitetrench_tab = "tag_select_elitetrench_tab";
        public static string tag_start_elitetrench_fight = "tag_start_elitetrench_fight";

        private static GuideController GenSelectBuildingGuide(GuideEvent _event)
        {
            <GenSelectBuildingGuide>c__AnonStorey227 storey = new <GenSelectBuildingGuide>c__AnonStorey227 {
                _event = _event,
                building_mask = new Focus3DMask()
            };
            storey.building_mask.SetExtractFlag(FocusMask.ExtractFlag.PerVertex, true);
            storey.building_mask.AttachGUILock();
            storey.building_mask.AttachFingure();
            storey.building_mask.AttachTips(0x23, TipsGizmo.Anchor.LEFT).SetOffset(new Vector3(30f, 0f, 0f));
            storey.ctrl_building_mask = new FocusController();
            storey.ctrl_building_mask.Visual = storey.building_mask;
            storey.ctrl_building_mask.FSM.valid_tag = tag_battle_select_building;
            GuideFSM fSM = storey.ctrl_building_mask.FSM;
            if (<>f__am$cacheF == null)
            {
                <>f__am$cacheF = delegate {
                    Utility.LockTouchOfMainScene(RequestBuildingGeneration());
                    Utility.LockSwippyOfMainScene();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cacheF);
            GuideFSM efsm2 = storey.ctrl_building_mask.FSM;
            efsm2.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm2.transition_generate, new Action<List<GameObject>>(storey.<>m__483));
            GuideFSM efsm3 = storey.ctrl_building_mask.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storey.<>m__484));
            GuideFSM efsm4 = storey.ctrl_building_mask.FSM;
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = delegate {
                    Utility.UnLockTouchOfMainScene();
                    Utility.UnlockSwippyOfMainScene();
                };
            }
            efsm4.transition_cancel = (System.Action) Delegate.Combine(efsm4.transition_cancel, <>f__am$cache10);
            return storey.ctrl_building_mask;
        }

        private static GuideController GenSelectDuplicate()
        {
            <GenSelectDuplicate>c__AnonStorey228 storey = new <GenSelectDuplicate>c__AnonStorey228();
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().SetOffset(new Vector3(40f, -20f, 0f)).EnableHang(true);
            mask.AttachTips(0x24, TipsGizmo.Anchor.TOP_RIGHT).SetOffset(new Vector3(-190f, -120f, 0f)).EnableHang(true).RedirectInScreenRect(false);
            storey.ctrl_map_mask = new FocusController();
            storey.ctrl_map_mask.Visual = mask;
            storey.ctrl_map_mask.FSM.valid_tag = tag_battle_select_duplicate;
            GuideFSM fSM = storey.ctrl_map_mask.FSM;
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = delegate {
                    Utility.ResetDupMapScroll();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache11);
            GuideFSM efsm2 = storey.ctrl_map_mask.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__487));
            return storey.ctrl_map_mask;
        }

        private static GuideController GenSelectEliateTrench(GuideEvent _event)
        {
            <GenSelectEliateTrench>c__AnonStorey229 storey = new <GenSelectEliateTrench>c__AnonStorey229 {
                _event = _event
            };
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().SetOffset(new Vector3(-10f, 10f, 0f));
            mask.AttachTips(0x25, TipsGizmo.Anchor.RIGHT).SetOffset(new Vector3(-20f, -10f, 0f)).EnableHang(true);
            storey.ctrl_trench = new FocusController();
            storey.ctrl_trench.Visual = mask;
            storey.ctrl_trench.FSM.valid_tag = tag_battle_select_trench;
            GuideFSM fSM = storey.ctrl_trench.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storey.<>m__488));
            return storey.ctrl_trench;
        }

        private static GuideController GenSelectTrench(GuideEvent _event)
        {
            <GenSelectTrench>c__AnonStorey22A storeya = new <GenSelectTrench>c__AnonStorey22A {
                _event = _event
            };
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().SetOffset(new Vector3(-10f, 10f, 0f));
            mask.AttachTips(0x25, TipsGizmo.Anchor.RIGHT).SetOffset(new Vector3(-20f, -10f, 0f)).EnableHang(true);
            storeya.ctrl_trench = new FocusController();
            storeya.ctrl_trench.Visual = mask;
            storeya.ctrl_trench.FSM.valid_tag = tag_battle_select_trench;
            GuideFSM fSM = storeya.ctrl_trench.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storeya.<>m__489));
            return storeya.ctrl_trench;
        }

        public static GuideController RegisterDupMapGuide()
        {
            <RegisterDupMapGuide>c__AnonStorey224 storey = new <RegisterDupMapGuide>c__AnonStorey224();
            TalkBoxController controller = new TalkBoxController(0x22);
            GuideFSM fSM = controller.FSM;
            if (<>f__am$cacheB == null)
            {
                <>f__am$cacheB = delegate {
                    Utility.LockSwippyOfMainScene();
                    GUIMgr.Instance.ExitModelGUIImmediate("ChangePlayerIconPanel");
                    GUIMgr.Instance.ExitModelGUIImmediate("PlayerInfoPanel");
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cacheB);
            storey.ctrl_redirect = new FlexiableController();
            GuideFSM efsm2 = storey.ctrl_redirect.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storey.<>m__477));
            GuideController controller2 = GenSelectBuildingGuide(GuideEvent.Duplicate);
            controller.next_step = storey.ctrl_redirect;
            storey.ctrl_redirect.next_step = controller2;
            return controller;
        }

        public static GuideController RegisterEliteDuplicateGuide()
        {
            <RegisterEliteDuplicateGuide>c__AnonStorey221 storey = new <RegisterEliteDuplicateGuide>c__AnonStorey221 {
                ctrl_talk0 = new TalkBoxController(0x55)
            };
            GuideFSM fSM = storey.ctrl_talk0.FSM;
            if (<>f__am$cache8 == null)
            {
                <>f__am$cache8 = delegate {
                    Utility.LockSwippyOfMainScene();
                    GUIMgr.Instance.ExitModelGUIImmediate("ChangePlayerIconPanel");
                    GUIMgr.Instance.ExitModelGUIImmediate("PlayerInfoPanel");
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache8);
            storey.ctrl_redirect = new FlexiableController();
            GuideFSM efsm2 = storey.ctrl_redirect.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storey.<>m__46F));
            GuideController controller = GenSelectBuildingGuide(GuideEvent.Duplicate_Elite);
            storey.ctrl_talk0.next_step = storey.ctrl_redirect;
            storey.ctrl_redirect.next_step = controller;
            storey.ctrl_fork = new FlexiableController();
            GuideFSM efsm3 = storey.ctrl_fork.FSM;
            efsm3.transition_awake = (System.Action) Delegate.Combine(efsm3.transition_awake, new System.Action(storey.<>m__470));
            GuideFSM efsm4 = storey.ctrl_fork.FSM;
            if (<>f__am$cache9 == null)
            {
                <>f__am$cache9 = () => null == GUIMgr.Instance.GetActivityGUIEntity<ShopTabEntity>();
            }
            efsm4.condition_reached = (Func<bool>) Delegate.Combine(efsm4.condition_reached, <>f__am$cache9);
            return storey.ctrl_fork;
        }

        public static GuideController RegisterEliteStartFightGuide()
        {
            <RegisterEliteStartFightGuide>c__AnonStorey223 storey = new <RegisterEliteStartFightGuide>c__AnonStorey223();
            TalkBoxController controller = new TalkBoxController(0x56);
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().EnableHang(true);
            storey.ctrl_start_button = new FocusController();
            storey.ctrl_start_button.Visual = mask;
            storey.ctrl_start_button.FSM.valid_tag = tag_start_elitetrench_fight;
            GuideFSM fSM = storey.ctrl_start_button.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storey.<>m__474));
            GuideFSM efsm2 = storey.ctrl_start_button.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__475));
            controller.next_step = storey.ctrl_start_button;
            return controller;
        }

        public static GuideController RegisterEliteTrenchGuide()
        {
            <RegisterEliteTrenchGuide>c__AnonStorey222 storey = new <RegisterEliteTrenchGuide>c__AnonStorey222();
            GuideController controller = GenSelectDuplicate();
            GuideController controller2 = GenSelectEliateTrench(GuideEvent.Trench_Elite);
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            storey.ctrl_select_elite_tab = new FocusController();
            storey.ctrl_select_elite_tab.Visual = mask;
            storey.ctrl_select_elite_tab.FSM.valid_tag = tag_select_elitetrench_tab;
            GuideFSM fSM = storey.ctrl_select_elite_tab.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storey.<>m__472));
            controller.next_step = storey.ctrl_select_elite_tab;
            storey.ctrl_select_elite_tab.next_step = controller2;
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = () => Utility.CheckOriginedDupMap();
            }
            controller.FSM.condition_reached = <>f__am$cacheA;
            return controller;
        }

        public static GuideController RegisterFormationGuide()
        {
            <RegisterFormationGuide>c__AnonStorey225 storey = new <RegisterFormationGuide>c__AnonStorey225();
            TalkBoxController controller = new TalkBoxController(0x26);
            GuideFSM fSM = controller.FSM;
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = delegate {
                    Utility.LockSwippyOfSelectHeroPanel();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cacheD);
            storey.ctrl_talk1 = new TalkBoxController(0x54);
            GuideFSM efsm2 = storey.ctrl_talk1.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__47A));
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            mask.AttachTips(40, TipsGizmo.Anchor.LEFT).SetOffset(new Vector3(0f, 40f, 0f));
            storey.ctrl_select_assist = new FocusController();
            storey.ctrl_select_assist.Visual = mask;
            storey.ctrl_select_assist.FSM.valid_tag = tag_select_assist;
            GuideFSM efsm3 = storey.ctrl_select_assist.FSM;
            efsm3.transition_awake = (System.Action) Delegate.Combine(efsm3.transition_awake, new System.Action(storey.<>m__47B));
            GuideFSM efsm4 = storey.ctrl_select_assist.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storey.<>m__47C));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            mask2.AttachTips(0x27, TipsGizmo.Anchor.BOTTOM).SetOffset(new Vector3(0f, 20f, 0f));
            storey.ctrl_selection = new FocusController();
            storey.ctrl_selection.Visual = mask2;
            storey.ctrl_selection.FSM.valid_tag = tag_formation_select_hero;
            GuideFSM efsm5 = storey.ctrl_selection.FSM;
            efsm5.transition_awake = (System.Action) Delegate.Combine(efsm5.transition_awake, new System.Action(storey.<>m__47D));
            GuideFSM efsm6 = storey.ctrl_selection.FSM;
            efsm6.transition_response = (Action<GameObject>) Delegate.Combine(efsm6.transition_response, new Action<GameObject>(storey.<>m__47E));
            storey.ctrl_head = new FlexiableController();
            GuideFSM efsm7 = storey.ctrl_head.FSM;
            efsm7.transition_awake = (System.Action) Delegate.Combine(efsm7.transition_awake, new System.Action(storey.<>m__47F));
            controller.next_step = storey.ctrl_head;
            storey.ctrl_select_assist.next_step = storey.ctrl_talk1;
            return controller;
        }

        public static GuideController RegisterSelectTrenchGuide()
        {
            GuideController controller = GenSelectDuplicate();
            GuideController controller2 = GenSelectTrench(GuideEvent.Trench);
            controller.next_step = controller2;
            if (<>f__am$cacheC == null)
            {
                <>f__am$cacheC = () => Utility.CheckOriginedDupMap();
            }
            controller.FSM.condition_reached = <>f__am$cacheC;
            return controller;
        }

        public static GuideController RegisterUseSkillGuide()
        {
            <RegisterUseSkillGuide>c__AnonStorey226 storey = new <RegisterUseSkillGuide>c__AnonStorey226();
            FocusMask mask = new FocusMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure();
            storey.ctrl_furry = new FocusController();
            storey.ctrl_furry.Visual = mask;
            storey.ctrl_furry.FSM.valid_tag = tag_battle_use_skill;
            GuideFSM fSM = storey.ctrl_furry.FSM;
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = delegate {
                    GuideSystem.FinishEvent(GuideEvent.Battle);
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cacheE);
            GuideFSM efsm2 = storey.ctrl_furry.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__481));
            return storey.ctrl_furry;
        }

        private static GameObject RequestBuildingGeneration()
        {
            GameObject obj2 = GameObject.Find("GameObject/cj_xzjm2/fuben/fuben_feiting");
            GuideSystem.ActivedGuide.RequestGeneration(tag_battle_select_building, obj2);
            return obj2;
        }

        [CompilerGenerated]
        private sealed class <GenSelectBuildingGuide>c__AnonStorey227
        {
            internal GuideEvent _event;
            internal Focus3DMask building_mask;
            internal FocusController ctrl_building_mask;

            internal void <>m__483(List<GameObject> lst)
            {
                GameObject obj2 = Utility.FindMainUIModuleTipsObjectByKey("Ui_Main_fuben");
                if (null != obj2)
                {
                    List<GameObject> objects = new List<GameObject> {
                        obj2
                    };
                    this.building_mask.AttachImage2DTips(objects, true);
                }
            }

            internal void <>m__484(GameObject go)
            {
                Utility.UnLockTouchOfMainScene();
                Utility.UnlockSwippyOfMainScene();
                this.ctrl_building_mask.Complete();
                GuideSystem.FinishEvent(this._event);
            }
        }

        [CompilerGenerated]
        private sealed class <GenSelectDuplicate>c__AnonStorey228
        {
            internal FocusController ctrl_map_mask;

            internal void <>m__487(GameObject go)
            {
                this.ctrl_map_mask.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <GenSelectEliateTrench>c__AnonStorey229
        {
            internal GuideEvent _event;
            internal FocusController ctrl_trench;

            internal void <>m__488(GameObject go)
            {
                this.ctrl_trench.Complete();
                GuideSystem.FinishEvent(this._event);
            }
        }

        [CompilerGenerated]
        private sealed class <GenSelectTrench>c__AnonStorey22A
        {
            internal GuideEvent _event;
            internal FocusController ctrl_trench;

            internal void <>m__489(GameObject go)
            {
                this.ctrl_trench.Complete();
                GuideSystem.FinishEvent(this._event);
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterDupMapGuide>c__AnonStorey224
        {
            internal FlexiableController ctrl_redirect;

            internal void <>m__477()
            {
                Utility.NewbiestLock();
                Utility.TransposeMain3DCamera(new Vector3(4.880725f, -0.4f, 30f), 20f, () => this.ctrl_redirect.GoNext());
            }

            internal void <>m__48B()
            {
                this.ctrl_redirect.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterEliteDuplicateGuide>c__AnonStorey221
        {
            internal FlexiableController ctrl_fork;
            internal FlexiableController ctrl_redirect;
            internal TalkBoxController ctrl_talk0;

            internal void <>m__46F()
            {
                Utility.NewbiestLock();
                GUIMgr.Instance.ClearExceptMainUI();
                Utility.TransposeMain3DCamera(new Vector3(4.880725f, -0.4f, 30f), 20f, () => this.ctrl_redirect.GoNext());
            }

            internal void <>m__470()
            {
                if (ActorData.getInstance().EliteProgress > 0)
                {
                    GuideSystem.SkipToEvent(GuideEvent.Function_Arena, true);
                    this.ctrl_fork.Complete();
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_talk0;
                    this.ctrl_fork.GoNext();
                }
            }

            internal void <>m__48A()
            {
                this.ctrl_redirect.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterEliteStartFightGuide>c__AnonStorey223
        {
            internal FocusController ctrl_start_button;

            internal void <>m__474()
            {
                SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    this.ctrl_start_button.Complete();
                }
                else
                {
                    activityGUIEntity.RequestGenerateStarter();
                }
            }

            internal void <>m__475(GameObject go)
            {
                this.ctrl_start_button.Complete();
                GuideSystem.FinishEvent(GuideEvent.Start_EliteTrench_Fight);
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterEliteTrenchGuide>c__AnonStorey222
        {
            internal FocusController ctrl_select_elite_tab;

            internal void <>m__472(GameObject go)
            {
                this.ctrl_select_elite_tab.GoNext();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterFormationGuide>c__AnonStorey225
        {
            internal FlexiableController ctrl_head;
            internal FocusController ctrl_select_assist;
            internal FocusController ctrl_selection;
            internal TalkBoxController ctrl_talk1;

            internal void <>m__47A(GameObject go)
            {
                this.ctrl_talk1.Complete();
            }

            internal void <>m__47B()
            {
                SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    Utility.UnlockSwippyOfSelectHeroPanel();
                    this.ctrl_select_assist.Cancel();
                }
                else
                {
                    activityGUIEntity.RequestGenerateAssistTab();
                }
            }

            internal void <>m__47C(GameObject go)
            {
                Utility.UnlockSwippyOfSelectHeroPanel();
                GuideSystem.FinishEvent(GuideEvent.Fomation);
                this.ctrl_select_assist.GoNext();
            }

            internal void <>m__47D()
            {
                SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    Utility.UnlockSwippyOfSelectHeroPanel();
                    this.ctrl_selection.Complete();
                }
                else
                {
                    activityGUIEntity.RequestGenerateHeroSelector();
                }
            }

            internal void <>m__47E(GameObject go)
            {
                SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    Utility.UnlockSwippyOfSelectHeroPanel();
                    this.ctrl_selection.Complete();
                }
                else if (activityGUIEntity.SelectionFull())
                {
                    this.ctrl_selection.next_step = this.ctrl_select_assist;
                    this.ctrl_selection.GoNext();
                }
                else
                {
                    this.ctrl_selection.next_step = this.ctrl_selection;
                    this.ctrl_selection.GoNext();
                }
            }

            internal void <>m__47F()
            {
                SelectHeroPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<SelectHeroPanel>();
                if (null == activityGUIEntity)
                {
                    Utility.NewbiestUnlock();
                    Utility.UnlockSwippyOfSelectHeroPanel();
                    this.ctrl_head.Complete();
                }
                else if (activityGUIEntity.SelectionFull())
                {
                    this.ctrl_head.next_step = this.ctrl_select_assist;
                    this.ctrl_head.GoNext();
                }
                else
                {
                    this.ctrl_head.next_step = this.ctrl_selection;
                    this.ctrl_head.GoNext();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterUseSkillGuide>c__AnonStorey226
        {
            internal FocusController ctrl_furry;

            internal void <>m__481(GameObject go)
            {
                Utility.NewbiestUnlock();
                this.ctrl_furry.Complete();
            }
        }
    }
}

