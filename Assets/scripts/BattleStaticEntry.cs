using FastBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

internal class BattleStaticEntry
{
    public static bool IsInBattle;

    public static void DoDupBattle(S2C_DuplicateCombat combatData)
    {
        <DoDupBattle>c__AnonStorey14F storeyf = new <DoDupBattle>c__AnonStorey14F();
        if (IsInBattle)
        {
            Debug.LogWarning("Is In Battle Aleady");
        }
        else
        {
            IsInBattle = true;
            storeyf.trenchEntry = combatData.dupData.trenchEntry;
            bool autoEnable = false;
            List<FastBuf.TrenchData> list = null;
            if (combatData.dupData.dupType == DuplicateType.DupType_Normal)
            {
                ActorData.getInstance().TrenchNormalDataDict.TryGetValue(combatData.dupData.dupEntry, out list);
            }
            else if (combatData.dupData.dupType == DuplicateType.DupType_Elite)
            {
                ActorData.getInstance().TrenchEliteDataDict.TryGetValue(combatData.dupData.dupEntry, out list);
            }
            if (list != null)
            {
                FastBuf.TrenchData data = list.Find(new Predicate<FastBuf.TrenchData>(storeyf.<>m__111));
                if (data != null)
                {
                    autoEnable = data.grade >= 3;
                }
            }
            if (combatData.dupData.dupType == DuplicateType.DupType_Normal)
            {
                trench_normal_config _config = ConfigMgr.getInstance().getByEntry<trench_normal_config>(storeyf.trenchEntry);
                BattleState.GetInstance().DoNormalBattle(combatData.combat_data, combatData.reward_view, BattleNormalGameType.Dup, autoEnable, _config.scene, _config.start_pos, _config.visual_type, _config.start_anim, combatData, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(BattleStaticEntry.OnDupBattleFinish));
            }
            else if (combatData.dupData.dupType == DuplicateType.DupType_Elite)
            {
                trench_elite_config _config2 = ConfigMgr.getInstance().getByEntry<trench_elite_config>(storeyf.trenchEntry);
                BattleState.GetInstance().DoNormalBattle(combatData.combat_data, combatData.reward_view, BattleNormalGameType.DupElite, autoEnable, _config2.scene, _config2.start_pos, _config2.visual_type, _config2.start_anim, combatData, new Action<bool, BattleNormalGameType, BattleNormalGameResult>(BattleStaticEntry.OnDupBattleFinish));
            }
            else
            {
                Debug.LogWarning("UnknowType!");
            }
        }
    }

    public static void DoDupBattleGrid(OutlandActivityData resEnter, bool isNewMap = true)
    {
        outland_config _config = ConfigMgr.getInstance().getByEntry<outland_config>(resEnter.data.entry);
        outland_map_config _config2 = ConfigMgr.getInstance().getByEntry<outland_map_config>(resEnter.data.map_entry);
        if ((_config2 != null) && (_config != null))
        {
            Debug.Log(string.Concat(new object[] { "????????????", resEnter.data.type_entry, "***", resEnter.activity_data.entry, "***", _config2.entry }));
            BattleState.GetInstance().EnterOutLand(resEnter, resEnter.activity_data.entry, _config.scene_name, _config2.enter_point, null, isNewMap);
        }
    }

    public static void ExitBattle()
    {
        IsInBattle = false;
        BattleState.GetInstance().OnLeave();
    }

    public static void ExitDupBattle(S2C_DuplicateEndReq res)
    {
        BattleState.GetInstance().CurGame.battleGameData.battleComObject.GetComponent<BattleCom_PhaseManager>().BeginShowBattleResult(res);
    }

    public static void OnDupBattleFinish(bool isWin, BattleNormalGameType gameType, BattleNormalGameResult result)
    {
        SocketMgr.Instance.RequestQuitDup(isWin, result.deadNumbers);
    }

    public static void OnExitDupBattle(S2C_DuplicateEndReq res)
    {
        <OnExitDupBattle>c__AnonStorey150 storey = new <OnExitDupBattle>c__AnonStorey150 {
            res = res
        };
        ExitBattle();
        GUIMgr.Instance.ClearAll();
        GUIMgr.Instance.DoModelGUI("ResultPanel", new Action<GUIEntity>(storey.<>m__112), null);
        GameDataMgr.Instance.DirtyActorStage = true;
    }

    public static void OnExitGuildDupBattle(S2C_GuildDupCombatEnd result)
    {
        <OnExitGuildDupBattle>c__AnonStorey151 storey = new <OnExitGuildDupBattle>c__AnonStorey151 {
            result = result
        };
        ExitBattle();
        GUIMgr.Instance.ClearAll();
        GUIMgr.Instance.PushGUIEntity<GuildDupEndBattle>(new Action<GUIEntity>(storey.<>m__113));
    }

    public static void StartTest(BattleGameType gameType)
    {
        BattleState.GetInstance().SetGameType(gameType);
        BattleState.GetInstance().OnEnter();
        GameStateMgr.Instance.ChangeState("COMMUNITY_BATTLE_EVENT");
    }

    public static void TryClearBattleOnError()
    {
        if (IsInBattle)
        {
            ExitBattle();
            Debug.Log("TryClearBattleOnError");
        }
    }

    public static void TryExitBattleOnError()
    {
        if (IsInBattle)
        {
            ExitBattle();
            Debug.Log("TryExitBattleOnError");
            GameStateMgr.Instance.ChangeState("BATTLE_EXIT_ERROR_REPEAT");
        }
    }

    [CompilerGenerated]
    private sealed class <DoDupBattle>c__AnonStorey14F
    {
        internal int trenchEntry;

        internal bool <>m__111(FastBuf.TrenchData obj)
        {
            return (obj.entry == this.trenchEntry);
        }
    }

    [CompilerGenerated]
    private sealed class <OnExitDupBattle>c__AnonStorey150
    {
        internal S2C_DuplicateEndReq res;

        internal void <>m__112(GUIEntity obj)
        {
            ((ResultPanel) obj).UpdateResultData(this.res);
        }
    }

    [CompilerGenerated]
    private sealed class <OnExitGuildDupBattle>c__AnonStorey151
    {
        internal S2C_GuildDupCombatEnd result;

        internal void <>m__113(GUIEntity u)
        {
            (u as GuildDupEndBattle).ShowBattleResult(this.result);
        }
    }
}

