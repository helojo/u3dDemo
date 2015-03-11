namespace Newbie
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class GuideRegister_Function
    {
        public static string tag_function_select_building = "tag_function_select_building";

        private static GuideController GenSelectBuildingController(GuideEvent _event, string[] host_objects, string tips_key, bool complex_generation)
        {
            <GenSelectBuildingController>c__AnonStorey22E storeye = new <GenSelectBuildingController>c__AnonStorey22E {
                host_objects = host_objects,
                tips_key = tips_key,
                _event = _event,
                mask_building = new Focus3DMask()
            };
            storeye.mask_building.SetExtractFlag(FocusMask.ExtractFlag.PerVertex, true);
            storeye.mask_building.SetExtractFlag(FocusMask.ExtractFlag.Complex, complex_generation);
            storeye.mask_building.AttachGUILock();
            storeye.mask_building.AttachFingure();
            storeye.ctrl_building_mask = new FocusController();
            storeye.ctrl_building_mask.Visual = storeye.mask_building;
            storeye.ctrl_building_mask.FSM.valid_tag = tag_function_select_building;
            GuideFSM fSM = storeye.ctrl_building_mask.FSM;
            fSM.transition_awake = (System.Action) Delegate.Combine(fSM.transition_awake, new System.Action(storeye.<>m__4AC));
            GuideFSM efsm2 = storeye.ctrl_building_mask.FSM;
            efsm2.transition_generate = (Action<List<GameObject>>) Delegate.Combine(efsm2.transition_generate, new Action<List<GameObject>>(storeye.<>m__4AD));
            GuideFSM efsm3 = storeye.ctrl_building_mask.FSM;
            efsm3.transition_response = (Action<GameObject>) Delegate.Combine(efsm3.transition_response, new Action<GameObject>(storeye.<>m__4AE));
            GuideFSM efsm4 = storeye.ctrl_building_mask.FSM;
            efsm4.transition_cancel = (System.Action) Delegate.Combine(efsm4.transition_cancel, new System.Action(storeye.<>m__4AF));
            return storeye.ctrl_building_mask;
        }

        public static GuideController RegisterFunctionArenaGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/jingjichang" };
            return RegisterFunctionGuide(GuideEvent.Function_Arena, CommonFunc.LevelLimitCfg().arena_ladder, 0x45, 0x4e, new Vector3(7.84f, -0.4f, 30f), strArray, "Ui_Main_jingjichang", true);
        }

        public static GuideController RegisterFunctionDungeonsGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/dixiacheng", "cj_zjm_texiao/cj_xzjm2_effect1/dixiacheng" };
            return RegisterFunctionGuide(GuideEvent.Function_Dungeons, CommonFunc.LevelLimitCfg().dungeons, 0x44, 0x4d, new Vector3(-5.42f, -0.4f, 30f), strArray, "Ui_Main_dixiacheng", true);
        }

        public static GuideController RegisterFunctionExpeditionGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/yuanzheng", "cj_zjm_texiao/Effects2/hanqi" };
            return RegisterFunctionGuide(GuideEvent.Function_Expedition, CommonFunc.LevelLimitCfg().flamebattle, 0x47, 80, new Vector3(-21.01938f, -0.4f, 30f), strArray, "Ui_Main_bfwz", true);
        }

        public static GuideController RegisterFunctionGuide(GuideEvent _event, int level, int talk_identity, int talk_identity_post, Vector3 focus_position, string[] host_objects, string tips_key, bool complex_generation = true)
        {
            <RegisterFunctionGuide>c__AnonStorey22F storeyf = new <RegisterFunctionGuide>c__AnonStorey22F {
                focus_position = focus_position,
                level = level,
                _event = _event,
                ctrl_talk0 = new TalkBoxController(talk_identity),
                ctrl_talk1 = new TalkBoxController(talk_identity_post)
            };
            GuideFSM fSM = storeyf.ctrl_talk1.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storeyf.<>m__4B0));
            storeyf.ctrl_redirect = new FlexiableController();
            GuideFSM efsm2 = storeyf.ctrl_redirect.FSM;
            efsm2.transition_awake = (System.Action) Delegate.Combine(efsm2.transition_awake, new System.Action(storeyf.<>m__4B1));
            GuideController controller = GenSelectBuildingController(storeyf._event, host_objects, tips_key, complex_generation);
            storeyf.ctrl_fork = new FlexiableController();
            GuideFSM efsm3 = storeyf.ctrl_fork.FSM;
            efsm3.transition_awake = (System.Action) Delegate.Combine(efsm3.transition_awake, new System.Action(storeyf.<>m__4B2));
            storeyf.ctrl_talk0.next_step = storeyf.ctrl_redirect;
            storeyf.ctrl_redirect.next_step = controller;
            controller.next_step = storeyf.ctrl_talk1;
            return storeyf.ctrl_fork;
        }

        public static GuideController RegisterFunctionGuildGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/gonghui", "cj_zjm_texiao/Effects2_01/cj_xzjm2_gonghui_tx" };
            return RegisterFunctionGuide(GuideEvent.Function_Guild, CommonFunc.LevelLimitCfg().guild, 70, 0x4f, new Vector3(-16.817f, -0.4f, 30f), strArray, "Ui_Main_gonghui", true);
        }

        public static GuideController RegisterFunctionLifeSkillGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/shenghuo", "juese_diaoyu", "cj_zjm_texiao/cj_xzjm2_effect1/water" };
            return RegisterFunctionGuide(GuideEvent.Function_LifeSkill, CommonFunc.LevelLimitCfg().lifeskill, 0x49, 0x52, new Vector3(-21.01938f, -0.4f, 30f), strArray, "Ui_Main_shjn", true);
        }

        public static GuideController RegisterFunctionOutlandGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/waiyu", "cj_zjm_texiao/cj_xzjm2_effect1/wy_effect" };
            return RegisterFunctionGuide(GuideEvent.Function_Outland, CommonFunc.LevelLimitCfg().outland_beginner, 0x48, 0x51, new Vector3(9.79f, -0.4f, 30f), strArray, "Ui_Main_qianghua", true);
        }

        public static GuideController RegisterFunctionShopGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/tiejiangpu" };
            return RegisterFunctionGuide(GuideEvent.Function_Shop, CommonFunc.LevelLimitCfg().shop, 0x43, 0x4c, new Vector3(-12.16f, -0.4f, 30f), strArray, "Ui_Main_wuqidian", true);
        }

        public static GuideController RegisterTowerGuide()
        {
            string[] strArray = new string[] { "GameObject/cj_xzjm2/shilianta", "cj_zjm_texiao/Effects2/cj_xzjm2_chuansongmen_tx" };
            return RegisterFunctionGuide(GuideEvent.Function_Tower, CommonFunc.LevelLimitCfg().void_tower, 0x4b, 0x53, new Vector3(3.847766f, -0.4f, 30f), strArray, "Ui_Main_shilianta", true);
        }

        [CompilerGenerated]
        private sealed class <GenSelectBuildingController>c__AnonStorey22E
        {
            internal GuideEvent _event;
            internal FocusController ctrl_building_mask;
            internal string[] host_objects;
            internal Focus3DMask mask_building;
            internal string tips_key;

            internal void <>m__4AC()
            {
                Utility.LockSwippyOfMainScene();
                List<GameObject> vari = new List<GameObject>();
                int length = this.host_objects.Length;
                for (int i = 0; i != length; i++)
                {
                    GameObject item = GameObject.Find(this.host_objects[i]);
                    if (null != item)
                    {
                        vari.Add(item);
                    }
                }
                if (vari.Count <= 0)
                {
                    this.ctrl_building_mask.Cancel();
                    Utility.UnlockSwippyOfMainScene();
                }
                GuideSystem.ActivedGuide.RequestMultiGeneration(GuideRegister_Function.tag_function_select_building, vari);
            }

            internal void <>m__4AD(List<GameObject> lst_objects)
            {
                GameObject obj2 = Utility.FindMainUIModuleTipsObjectByKey(this.tips_key);
                if (null != obj2)
                {
                    List<GameObject> objects = new List<GameObject> {
                        obj2
                    };
                    this.mask_building.AttachImage2DTips(objects, true);
                }
            }

            internal void <>m__4AE(GameObject go)
            {
                GuideSystem.FinishEvent(this._event);
                Utility.UnlockSwippyOfMainScene();
                this.ctrl_building_mask.GoNext();
            }

            internal void <>m__4AF()
            {
                GuideSystem.FinishEvent(this._event);
                Utility.UnlockSwippyOfMainScene();
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterFunctionGuide>c__AnonStorey22F
        {
            internal GuideEvent _event;
            internal FlexiableController ctrl_fork;
            internal FlexiableController ctrl_redirect;
            internal TalkBoxController ctrl_talk0;
            internal TalkBoxController ctrl_talk1;
            internal Vector3 focus_position;
            internal int level;

            internal void <>m__4B0(GameObject go)
            {
                this.ctrl_talk1.Complete();
            }

            internal void <>m__4B1()
            {
                Utility.NewbiestLock();
                Utility.LockSwippyOfMainScene();
                GUIMgr.Instance.ClearExceptMainUI();
                Utility.TransposeMain3DCamera(this.focus_position, 20f, delegate {
                    Utility.NewbiestUnlock();
                    this.ctrl_redirect.GoNext();
                });
            }

            internal void <>m__4B2()
            {
                int level = ActorData.getInstance().Level;
                if (level > this.level)
                {
                    GuideSystem.FinishEvent(this._event);
                    this.ctrl_fork.Complete();
                }
                else if (level < this.level)
                {
                    GuideSystem.ActivedGuide.RequestCancel();
                    this.ctrl_fork.Complete();
                }
                else
                {
                    this.ctrl_fork.next_step = this.ctrl_talk0;
                    this.ctrl_fork.GoNext();
                }
            }

            internal void <>m__4B3()
            {
                Utility.NewbiestUnlock();
                this.ctrl_redirect.GoNext();
            }
        }
    }
}

