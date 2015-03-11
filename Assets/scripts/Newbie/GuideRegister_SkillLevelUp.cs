namespace Newbie
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideRegister_SkillLevelUp
    {
        [CompilerGenerated]
        private static Func<CardOriginal, bool> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache4;
        [CompilerGenerated]
        private static Func<bool> <>f__am$cache5;
        public static string tag_skill_levelup_change_table = "tag_skill_levelup_change_table";
        public static string tag_skill_levelup_press_button = "tag_skill_levelup_press_button";

        public static GuideController RegisterPortalGuide()
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = () => Utility.CheckHaveCardSkillCanbeLevelup();
            }
            Func<bool> func = <>f__am$cache4;
            GuideController controller = GuideRegister_Hero.RegisterHeroPortal(GuideEvent.SkillLevelUp_Portal, func);
            TalkBoxController controller2 = new TalkBoxController(0x37);
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = delegate {
                    if (Utility.CheckSkillHasLevelup())
                    {
                        GuideSystem.SkipToEvent(GuideEvent.Medicine_Portal, true);
                        return false;
                    }
                    return Utility.CheckHaveCardSkillCanbeLevelup();
                };
            }
            controller2.FSM.condition_reached = <>f__am$cache5;
            controller2.next_step = controller;
            return controller2;
        }

        public static GuideController RegisterSelectHeroGuide()
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (CardOriginal co) {
                    if (co.ori == null)
                    {
                        return false;
                    }
                    return Utility.CheckSkillCanbeLevelup(co.ori);
                };
            }
            Func<CardOriginal, bool> func = <>f__am$cache2;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = () => Utility.CheckHaveCardSkillCanbeLevelup();
            }
            Func<bool> func2 = <>f__am$cache3;
            return GuideRegister_Hero.RegisterSelectHeroGuide(GuideEvent.SkillLevelUp_SelectHero, func2, func);
        }

        public static GuideController RegisterSkillLevelUpGuide()
        {
            <RegisterSkillLevelUpGuide>c__AnonStorey23C storeyc = new <RegisterSkillLevelUpGuide>c__AnonStorey23C();
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            storeyc.ctrl_tab = new FocusController();
            storeyc.ctrl_tab.Visual = mask;
            storeyc.ctrl_tab.FSM.valid_tag = tag_skill_levelup_change_table;
            GuideFSM fSM = storeyc.ctrl_tab.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storeyc.<>m__4EC));
            GuideFSM efsm2 = storeyc.ctrl_tab.FSM;
            efsm2.transition_response = (Action<GameObject>) Delegate.Combine(efsm2.transition_response, new Action<GameObject>(storeyc.<>m__4ED));
            Focus2DMask mask2 = new Focus2DMask();
            mask2.AttachFingure();
            storeyc.ctrl_lvup = new FocusController();
            storeyc.ctrl_lvup.Visual = mask2;
            storeyc.ctrl_lvup.FSM.valid_tag = tag_skill_levelup_press_button;
            GuideFSM efsm3 = storeyc.ctrl_lvup.FSM;
            efsm3.transition_awake = (System.Action) Delegate.Combine(efsm3.transition_awake, new System.Action(storeyc.<>m__4EE));
            GuideFSM efsm4 = storeyc.ctrl_lvup.FSM;
            efsm4.transition_response = (Action<GameObject>) Delegate.Combine(efsm4.transition_response, new Action<GameObject>(storeyc.<>m__4EF));
            storeyc.ctrl_fork = new FlexiableController();
            GuideFSM efsm5 = storeyc.ctrl_fork.FSM;
            efsm5.transition_awake = (System.Action) Delegate.Combine(efsm5.transition_awake, new System.Action(storeyc.<>m__4F0));
            TalkBoxController controller = new TalkBoxController(0x38);
            storeyc.ctrl_tab.next_step = storeyc.ctrl_lvup;
            storeyc.ctrl_lvup.next_step = controller;
            return storeyc.ctrl_fork;
        }

        [CompilerGenerated]
        private sealed class <RegisterSkillLevelUpGuide>c__AnonStorey23C
        {
            internal FlexiableController ctrl_fork;
            internal FocusController ctrl_lvup;
            internal FocusController ctrl_tab;

            internal void <>m__4EC()
            {
                HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null == activityGUIEntity)
                {
                    this.ctrl_tab.Complete();
                }
                else
                {
                    GUIMgr.Instance.ExitModelGUIImmediate("CardBreakSuccessPanel");
                    activityGUIEntity.RequestSkillTabGeneration();
                }
                Utility.NewbiestUnlock();
            }

            internal void <>m__4ED(GameObject go)
            {
                this.ctrl_tab.GoNext();
            }

            internal void <>m__4EE()
            {
                HeroInfoPanel activityGUIEntity = GUIMgr.Instance.GetActivityGUIEntity<HeroInfoPanel>();
                if (null == activityGUIEntity)
                {
                    this.ctrl_lvup.Complete();
                }
                else
                {
                    activityGUIEntity.RequestSkillLevelupGeneration();
                }
                Utility.NewbiestUnlock();
            }

            internal void <>m__4EF(GameObject go)
            {
                GuideSystem.FinishEvent(GuideEvent.SkillLevelUp_Function);
                this.ctrl_lvup.GoNext();
            }

            internal void <>m__4F0()
            {
                if (Utility.CheckUIActivedSkillLevelUp())
                {
                    this.ctrl_fork.next_step = this.ctrl_lvup;
                    this.ctrl_fork.GoNext();
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_tab;
                    this.ctrl_fork.GoNext();
                }
            }
        }
    }
}

