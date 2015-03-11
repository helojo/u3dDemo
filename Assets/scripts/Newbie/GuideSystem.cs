namespace Newbie
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class GuideSystem
    {
        private static GuideEvent actived_event = GuideEvent.Count;
        private static bool guides_registed = false;
        private static GuideController[] handlers_event = new GuideController[0x24];
        private static GuideController nullty_controller = null;
        public static int progress = 0;

        public static bool Finished(GuideEvent _event)
        {
            return (progress > _event);
        }

        public static void FinishEvent(GuideEvent _event)
        {
            if (progress == _event)
            {
                SocketMgr.Instance.RecordNewbieStep(++progress);
            }
        }

        public static void FireEvent(GuideEvent _event)
        {
            if (progress == _event)
            {
                GuideController activedGuide = ActivedGuide;
                if ((NulltyController == activedGuide) || !activedGuide.actived)
                {
                    RegisterAllGuides();
                    int index = (int) _event;
                    if ((index >= 0) && (index < handlers_event.Length))
                    {
                        GuideController controller2 = handlers_event[index];
                        if ((controller2 != null) && controller2.ConditionReachable())
                        {
                            actived_event = _event;
                            controller2.BeginStep();
                        }
                    }
                }
            }
        }

        public static void GuideWithStepController(GuideController controller)
        {
            controller.BeginStep();
        }

        public static bool MatchEvent(GuideEvent _event)
        {
            return (_event == actived_event);
        }

        public static void OnLogon()
        {
            guides_registed = false;
            actived_event = GuideEvent.Count;
            progress = Mathf.Max(ActorData.getInstance().UserInfo.newbie_step, 0);
        }

        public static void RefireEvent(GuideEvent _event)
        {
            if (MatchEvent(_event))
            {
                ActivedGuide.RequestCancel();
                FireEvent(_event);
            }
        }

        private static void RegisterAllGuides()
        {
            if (!guides_registed)
            {
                handlers_event[0] = GuideRegister_Recruit.RegisterGoldRecruit();
                handlers_event[1] = GuideRegister_Recruit.RegisterStoneRecruit();
                handlers_event[2] = GuideRegister_HelpMe.RegisterHelpMeGuide();
                handlers_event[3] = GuideRegister_Battle.RegisterDupMapGuide();
                handlers_event[4] = GuideRegister_Battle.RegisterSelectTrenchGuide();
                handlers_event[0x1b] = GuideRegister_Battle.RegisterEliteDuplicateGuide();
                handlers_event[0x1c] = GuideRegister_Battle.RegisterEliteTrenchGuide();
                handlers_event[5] = GuideRegister_Battle.RegisterFormationGuide();
                handlers_event[0x1d] = GuideRegister_Battle.RegisterEliteStartFightGuide();
                handlers_event[6] = GuideRegister_Battle.RegisterUseSkillGuide();
                handlers_event[7] = GuideRegister_Equip.RegisterEquipLevelUpPortal();
                handlers_event[8] = GuideRegister_Equip.RegisterSelectHero();
                handlers_event[9] = GuideRegister_Equip.RegisterLevelUpFunction();
                handlers_event[13] = GuideRegister_Mission.RegisterMissionPortal(GuideEvent.EquipLevelUp_MissionPortal, 0x84);
                handlers_event[10] = GuideRegister_CardBreak.RegisterCardBreakPortalGuide();
                handlers_event[11] = GuideRegister_CardBreak.RegisterSelectHeroGuide();
                handlers_event[12] = GuideRegister_CardBreak.RegisterCardBreakGuide();
                handlers_event[0x10] = GuideRegister_SkillLevelUp.RegisterPortalGuide();
                handlers_event[0x11] = GuideRegister_SkillLevelUp.RegisterSelectHeroGuide();
                handlers_event[0x12] = GuideRegister_SkillLevelUp.RegisterSkillLevelUpGuide();
                handlers_event[14] = GuideRegister_CardCombine.RegisterCardCombinePortalGuide();
                handlers_event[15] = GuideRegister_CardCombine.RegisterCardCombineGuide();
                handlers_event[20] = GuideRegister_Medicine.RegisterMedicinePortalGuide();
                handlers_event[0x15] = GuideRegister_Medicine.RegisterUseMedicineGuide();
                handlers_event[0x16] = GuideRegister_Strengthen.RegisterStrengthenPortal();
                handlers_event[0x17] = GuideRegister_Strengthen.RegisterStrengthenSelectHero();
                handlers_event[0x18] = GuideRegister_Strengthen.RegisterStrengthenFunction();
                handlers_event[0x13] = GuideRegister_Daily.RegisterDailyPortal(GuideEvent.Duplicate_Daily);
                handlers_event[0x1a] = GuideRegister_Function.RegisterFunctionShopGuide();
                handlers_event[30] = GuideRegister_Function.RegisterFunctionArenaGuide();
                handlers_event[0x1f] = GuideRegister_Function.RegisterFunctionDungeonsGuide();
                handlers_event[0x20] = GuideRegister_Function.RegisterFunctionOutlandGuide();
                handlers_event[0x21] = GuideRegister_Function.RegisterFunctionLifeSkillGuide();
                handlers_event[0x19] = GuideRegister_Guild.GenGuildContrller();
                handlers_event[0x22] = GuideRegister_Function.RegisterTowerGuide();
                handlers_event[0x23] = GuideRegister_Function.RegisterFunctionExpeditionGuide();
                guides_registed = true;
            }
        }

        public static bool Skipable(GuideEvent _event)
        {
            if (_event != progress)
            {
                return false;
            }
            GuideController activedGuide = ActivedGuide;
            return ((NulltyController == activedGuide) || !activedGuide.actived);
        }

        public static void SkipToEvent(GuideEvent _event, bool record = true)
        {
            progress = (int) _event;
            if (record)
            {
                SocketMgr.Instance.RecordNewbieStep(progress);
            }
        }

        public static void TerminateEvent(GuideEvent _event)
        {
            if (MatchEvent(_event))
            {
                ActivedGuide.RequestCancel();
            }
        }

        public static GuideController ActivedGuide
        {
            get
            {
                int index = (int) actived_event;
                if ((index >= 0) && (index < handlers_event.Length))
                {
                    for (GuideController controller = handlers_event[index]; controller != null; controller = controller.next_step)
                    {
                        if (controller.actived)
                        {
                            return controller;
                        }
                    }
                }
                return NulltyController;
            }
        }

        private static GuideController NulltyController
        {
            get
            {
                if (nullty_controller == null)
                {
                    nullty_controller = new GuideController();
                    nullty_controller.FSM.valid_tag = "nullty_guide_controller";
                }
                return nullty_controller;
            }
        }
    }
}

