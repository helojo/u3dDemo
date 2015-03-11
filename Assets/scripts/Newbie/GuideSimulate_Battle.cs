namespace Newbie
{
    using Battle;
    using FastBuf;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class GuideSimulate_Battle
    {
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cacheC;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cacheD;
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cacheE;
        [CompilerGenerated]
        private static UIEventListener.VoidDelegate <>f__am$cacheF;
        private static System.Action action_finished = null;
        public static int[] actor_energy_table = new int[] { 950, 800, 650, 500, 350 };
        public static int[] actor_entries_bl = new int[] { 0, 1, 6, 3, 2 };
        public static int[] actor_entries_lm = new int[] { 0x10, 0x18, 0x13, 13, 15 };
        public static int[] actor_weapon_bl = new int[] { 0x100, 0x1f5, 0x2ab, 0x1a1, 0x1c4 };
        public static int[] actor_weapon_lm = new int[] { 0xf9, 0x169, 0x14d, 0x28e, 0x20a };
        public static string guide_tag_use_skill = "guide_tag_use_skill";
        public static HashSet<int> identity_used = new HashSet<int>();
        public static bool is_lm = false;
        private static bool on_guide = false;
        public static GuideController root_controller = null;
        public static bool sim_mode = false;

        private static void CastBossSkill()
        {
            BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAiActor(100).SetEnergy(AiDef.MAX_ENERGY, HitEnergyType.None);
            BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().QuestCastSkill(100);
            ShowGUI(false);
            Pause();
            if (<>f__am$cacheE == null)
            {
                <>f__am$cacheE = delegate (GUIEntity entity) {
                    DupStoryDiag diag = entity as DupStoryDiag;
                    if (<>f__am$cacheF == null)
                    {
                        <>f__am$cacheF = delegate (GameObject go) {
                            GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
                            Resume();
                        };
                    }
                    diag.UpdateData(0xd4, <>f__am$cacheF);
                };
            }
            GUIMgr.Instance.DoModelGUI("DupStoryDiag", <>f__am$cacheE, null);
        }

        public static CombatTeam GenBossTeam()
        {
            CombatDetailActor actor = BattleGlobalFunc.CreateDetailInfoOnMonster(0xc40);
            actor.level = 1;
            actor.attack *= UnityEngine.Random.Range(200, 300);
            actor.hitRate = 0xdbba0;
            return new CombatTeam { actor = { actor } };
        }

        public static void GenerateFullFurry(int id, GameObject go)
        {
            if (on_guide && (root_controller != null))
            {
                root_controller.RequestGeneration(guide_tag_use_skill, go);
            }
        }

        public static CombatTeam GenPlayerTeam()
        {
            CombatTeam team = new CombatTeam();
            int[] numArray = !is_lm ? actor_entries_bl : actor_entries_lm;
            int[] numArray2 = !is_lm ? actor_weapon_bl : actor_weapon_lm;
            int length = numArray.Length;
            for (int i = 0; i != length; i++)
            {
                int cardEntry = numArray[i];
                CombatDetailActor item = BattleGlobalFunc.CreateDetailInfoByCard(cardEntry);
                item.equip.entry = (short) numArray2[i];
                item.energy = actor_energy_table[i];
                item.quality = 6;
                item.level = 50;
                item.maxHp *= (ulong) 0x3e8L;
                item.curHp = item.maxHp;
                item.attack *= UnityEngine.Random.Range(0x640, 0x7d0);
                item.tenacity = 0;
                item.prewarSkill = -1;
                team.actor.Add(item);
            }
            return team;
        }

        public static void OnBattleFinished(bool par1, BattleNormalGameType par2, BattleNormalGameResult par3)
        {
            sim_mode = false;
            ShowGUI(false);
            if (<>f__am$cacheD == null)
            {
                <>f__am$cacheD = entity => (entity as DupStoryDiag).UpdateData(!is_lm ? 0xd5 : 0xd6, new UIEventListener.VoidDelegate(GuideSimulate_Battle.OnStoryDiagEvent_BattleFinished));
            }
            GUIMgr.Instance.DoModelGUI("DupStoryDiag", <>f__am$cacheD, null);
        }

        public static void OnBattleStart()
        {
            BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().GetAiActor(100).IsAuto = false;
            ShowGUI(false);
            Pause();
            if (<>f__am$cacheC == null)
            {
                <>f__am$cacheC = entity => (entity as DupStoryDiag).UpdateData(!is_lm ? 200 : 0xc9, new UIEventListener.VoidDelegate(GuideSimulate_Battle.OnStoryDiagEvent_Battle_Start));
            }
            GUIMgr.Instance.DoModelGUI("DupStoryDiag", <>f__am$cacheC, null);
        }

        public static void OnStoryDiagEvent_Battle_Start(GameObject go)
        {
            ShowGUI(true);
            Resume();
            BattleData battleGameData = BattleState.GetNormalGameInstance().battleGameData;
            battleGameData.OnMsgPhaseStartFinish = (System.Action) Delegate.Remove(battleGameData.OnMsgPhaseStartFinish, new System.Action(GuideSimulate_Battle.OnBattleStart));
            GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
        }

        public static void OnStoryDiagEvent_BattleFinished(GameObject go)
        {
            GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
            GameStateMgr.Instance.ChangeState(string.Empty);
            BattleStaticEntry.ExitBattle();
            if (action_finished != null)
            {
                action_finished();
            }
            action_finished = null;
            root_controller = null;
        }

        private static void Pause()
        {
            BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().PauseGame();
        }

        public static void PrepareFullFurry(int id, System.Action action)
        {
            <PrepareFullFurry>c__AnonStorey23E storeye = new <PrepareFullFurry>c__AnonStorey23E {
                id = id,
                action = action
            };
            if ((!on_guide && (root_controller == null)) && !identity_used.Contains(storeye.id))
            {
                Pause();
                on_guide = true;
                ShowGUI(false);
                GUIMgr.Instance.DoModelGUI("DupStoryDiag", new Action<GUIEntity>(storeye.<>m__4FC), null);
            }
        }

        private static void Resume()
        {
            BattleState.GetNormalGameInstance().battleGameData.battleComObject.GetComponent<BattleCom_Runtime>().ResumeGame();
        }

        private static void ShowGUI(bool show)
        {
            BattlePanel gUIEntity = GUIMgr.Instance.GetGUIEntity<BattlePanel>();
            if (null != gUIEntity)
            {
                gUIEntity.NGUIPanel.alpha = !show ? 0f : 1f;
            }
        }

        public static void StartBattle(System.Action act)
        {
            sim_mode = true;
            root_controller = null;
            identity_used.Clear();
            action_finished = act;
            GUIMgr.Instance.ClearAll();
            CombatCrewData combatData = new CombatCrewData {
                attacker = GenPlayerTeam()
            };
            combatData.defenderList.Add(GenBossTeam());
            combatData.seed = (uint) UnityEngine.Random.Range(0, 0x989680);
            BattleState.GetInstance().DoNormalBattle(combatData, null, BattleNormalGameType.Dup, false, "cj_boss_yanjiang", 0, 2, "xjdh_boss", null, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(GuideSimulate_Battle.OnBattleFinished));
            if (null != BattleState.GetInstance().CurGame)
            {
                BattleState.GetInstance().CurGame.battleGameData.IsStoryBattle = true;
                BattleData battleGameData = BattleState.GetInstance().CurGame.battleGameData;
                battleGameData.OnMsgPhaseStartFinish = (System.Action) Delegate.Combine(battleGameData.OnMsgPhaseStartFinish, new System.Action(GuideSimulate_Battle.OnBattleStart));
            }
        }

        public static void UseSkill(int id, GameObject go)
        {
            if (on_guide && (root_controller != null))
            {
                root_controller.RequestUIResponse(guide_tag_use_skill, go);
            }
        }

        public static GuideController UseSkillGuide()
        {
            <UseSkillGuide>c__AnonStorey23F storeyf = new <UseSkillGuide>c__AnonStorey23F();
            Focus2DMask mask = new Focus2DMask();
            mask.AttachFingure();
            storeyf.ctrl = new FocusController();
            storeyf.ctrl.Visual = mask;
            storeyf.ctrl.FSM.valid_tag = guide_tag_use_skill;
            GuideFSM fSM = storeyf.ctrl.FSM;
            fSM.transition_response = (Action<GameObject>) Delegate.Combine(fSM.transition_response, new Action<GameObject>(storeyf.<>m__4FD));
            return storeyf.ctrl;
        }

        [CompilerGenerated]
        private sealed class <PrepareFullFurry>c__AnonStorey23E
        {
            internal System.Action action;
            internal int id;

            internal void <>m__4FC(GUIEntity entity)
            {
                (entity as DupStoryDiag).UpdateData(this.id + (!GuideSimulate_Battle.is_lm ? 0xca : 0xcf), delegate (GameObject go) {
                    GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
                    GuideSimulate_Battle.ShowGUI(true);
                    GuideSimulate_Battle.identity_used.Add(this.id);
                    GuideSimulate_Battle.root_controller = GuideSimulate_Battle.UseSkillGuide();
                    GuideSimulate_Battle.root_controller.BeginStep();
                    if (this.action != null)
                    {
                        this.action();
                    }
                });
            }

            internal void <>m__502(GameObject go)
            {
                GUIMgr.Instance.ExitModelGUI("DupStoryDiag");
                GuideSimulate_Battle.ShowGUI(true);
                GuideSimulate_Battle.identity_used.Add(this.id);
                GuideSimulate_Battle.root_controller = GuideSimulate_Battle.UseSkillGuide();
                GuideSimulate_Battle.root_controller.BeginStep();
                if (this.action != null)
                {
                    this.action();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UseSkillGuide>c__AnonStorey23F
        {
            private static System.Action <>f__am$cache1;
            internal FocusController ctrl;

            internal void <>m__4FD(GameObject go)
            {
                this.ctrl.Complete();
                GuideSimulate_Battle.root_controller = null;
                GuideSimulate_Battle.on_guide = false;
                GuideSimulate_Battle.Resume();
                if (GuideSimulate_Battle.identity_used.Count >= 5)
                {
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = () => GuideSimulate_Battle.CastBossSkill();
                    }
                    ScheduleMgr.Schedule(3f, <>f__am$cache1);
                }
            }

            private static void <>m__503()
            {
                GuideSimulate_Battle.CastBossSkill();
            }
        }
    }
}

