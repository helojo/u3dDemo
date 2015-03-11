namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_Hero
    {
        [CompilerGenerated]
        private static Action<List<GameObject>> <>f__am$cache3;
        [CompilerGenerated]
        private static System.Action <>f__am$cache4;
        [CompilerGenerated]
        private static System.Action <>f__am$cache5;
        public static string tag_guide_select_hero = "tag_guide_select_hero";
        public static string tag_hero_portal_press_floatbtn = "tag_hero_portal_press_floatbtn";
        public static string tag_hero_portal_press_function_button = "tag_hero_portal_press_function_button";

        public static GuideController RegisterHeroPortal(GuideEvent _event, Func<bool> func_reached)
        {
            <RegisterHeroPortal>c__AnonStorey232 storey = new <RegisterHeroPortal>c__AnonStorey232 {
                _event = _event
            };
            HangerMask mask = new HangerMask();
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().EnableHang(true);
            storey.ctrl_floatbtn = new FocusController();
            storey.ctrl_floatbtn.Visual = mask;
            storey.ctrl_floatbtn.FSM.valid_tag = tag_hero_portal_press_floatbtn;
            GuideFSM fSM = storey.ctrl_floatbtn.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storey.<>m__4BD));
            GuideFSM efsm2 = storey.ctrl_floatbtn.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storey.<>m__4BE));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            mask2.AttachTips(0x2d, TipsGizmo.Anchor.LEFT).SetOffset(new Vector3(-10f, 20f, 0f));
            storey.ctrl_portal = new FocusController();
            storey.ctrl_portal.Visual = mask2;
            storey.ctrl_portal.FSM.valid_tag = tag_hero_portal_press_function_button;
            GuideFSM efsm3 = storey.ctrl_portal.FSM;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = delegate (List<GameObject> gen_list) {
                    Utility.NewbiestUnlock();
                };
            }
            efsm3.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm3.transition_generate, <>f__am$cache3);
            GuideFSM efsm4 = storey.ctrl_portal.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storey.<>m__4C0));
            storey.ctrl_portal_immidiate = new FocusController();
            storey.ctrl_portal_immidiate.Visual = mask2;
            storey.ctrl_portal_immidiate.FSM.valid_tag = tag_hero_portal_press_function_button;
            GuideFSM efsm5 = storey.ctrl_portal_immidiate.FSM;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate {
                    TitleBar activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<TitleBar>();
                    if (null != activityGUIEntity)
                    {
                        activityGUIEntity.RequestHeroGeneration();
                    }
                    Utility.NewbiestUnlock();
                };
            }
            efsm5.transition_awake = (System.Action) Delegate.Combine(efsm5.transition_awake, <>f__am$cache4);
            GuideFSM efsm6 = storey.ctrl_portal_immidiate.FSM;
            efsm6.transition_response = (Action<GameObject>) Delegate.Combine(efsm6.transition_response, new Action<GameObject>(storey.<>m__4C2));
            storey.ctrl_fork = new FlexiableController();
            GuideFSM efsm7 = storey.ctrl_fork.FSM;
            efsm7.transition_awake = (System.Action) Delegate.Combine(efsm7.transition_awake, new System.Action(storey.<>m__4C3));
            storey.ctrl_fork.FSM.condition_reached = func_reached;
            storey.ctrl_floatbtn.next_step = storey.ctrl_portal;
            return storey.ctrl_fork;
        }

        public static GuideController RegisterSelectHeroGuide(GuideEvent _event, Func<bool> func_reached, Func<CardOriginal, bool> func_specific)
        {
            <RegisterSelectHeroGuide>c__AnonStorey233 storey = new <RegisterSelectHeroGuide>c__AnonStorey233 {
                func_specific = func_specific,
                _event = _event
            };
            HangerMask mask = new HangerMask {
                hang_layer = LayerMask.NameToLayer("ListUI")
            };
            mask.SetExtractFlag(FocusMask.ExtractFlag.Enforce, false);
            mask.AttachFingure().SetLayer("ListUI");
            storey.ctrl_select_hero = new FocusController();
            storey.ctrl_select_hero.Visual = mask;
            storey.ctrl_select_hero.FSM.valid_tag = tag_guide_select_hero;
            GuideFSM fSM = storey.ctrl_select_hero.FSM;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    Utility.NewbiestUnlock();
                };
            }
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, <>f__am$cache5);
            GuideFSM efsm2 = storey.ctrl_select_hero.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storey.<>m__4C5));
            GuideFSM efsm3 = storey.ctrl_select_hero.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storey.<>m__4C6));
            storey.ctrl_select_hero.FSM.condition_reached = func_reached;
            return storey.ctrl_select_hero;
        }

        [CompilerGenerated]
        private sealed class <RegisterHeroPortal>c__AnonStorey232
        {
            internal GuideEvent _event;
            internal FocusController ctrl_floatbtn;
            internal FlexiableController ctrl_fork;
            internal FocusController ctrl_portal;
            internal FocusController ctrl_portal_immidiate;

            internal void <>m__4BD()
            {
                Utility.RequestTitleBarFloatButtonGeneration(this._event, GuideRegister_Hero.tag_hero_portal_press_floatbtn);
                Utility.NewbiestUnlock();
            }

            internal void <>m__4BE(GameObject go)
            {
                this.ctrl_floatbtn.GoNext();
                Utility.NewbiestLock();
            }

            internal void <>m__4C0(GameObject go)
            {
                this.ctrl_portal.Complete();
                GuideSystem.FinishEvent(this._event);
            }

            internal void <>m__4C2(GameObject go)
            {
                this.ctrl_portal_immidiate.Complete();
                GuideSystem.FinishEvent(this._event);
            }

            internal void <>m__4C3()
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

        [CompilerGenerated]
        private sealed class <RegisterSelectHeroGuide>c__AnonStorey233
        {
            internal GuideEvent _event;
            internal FocusController ctrl_select_hero;
            internal Func<CardOriginal, bool> func_specific;

            internal void <>m__4C5()
            {
                List<CardOriginal> list = Utility.IterateHeroListEntity(this.func_specific, true);
                if (list.Count <= 0)
                {
                    Utility.NewbiestUnlock();
                    this.ctrl_select_hero.Complete();
                }
                else
                {
                    this.ctrl_select_hero.RequestGeneration(GuideRegister_Hero.tag_guide_select_hero, list[0].gameObject);
                    Utility.NewbiestUnlock();
                }
            }

            internal void <>m__4C6(GameObject go)
            {
                this.ctrl_select_hero.Complete();
                GuideSystem.FinishEvent(this._event);
            }
        }
    }
}

