using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleGlobal
{
    [CompilerGenerated]
    private static Func<float, float> <>f__am$cache2C;
    [CompilerGenerated]
    private static Func<float, float> <>f__am$cache2D;
    [CompilerGenerated]
    private static Func<float, float> <>f__am$cache2E;
    public static readonly float AnimationFadeTime = 0.1f;
    public static readonly string AnimFsmPrefab = "BattlePrefabs/AnimFSMInfo";
    public static readonly float AutoCountDownTime = 5f;
    public static readonly string BornAnim = "chuchang";
    public static readonly string BossTeamPosName = "BattlePrefabs/TeamPos_1";
    public static readonly string DeadAnimName = "dead";
    public static readonly int DefaultModelLayer = 9;
    public static readonly float DefaultMoveSpeed = 4.5f;
    public static readonly float DefaultMoveSpeedOfOutBattleScale = 1.5f;
    public static readonly string DefaultTeamPosName = "BattlePrefabs/TeamPos_0";
    public static readonly string DropEffectPrefab = "BattlePrefabs/Diaoluo_Tx";
    public static readonly string DropPrefab = "BattlePrefabs/diaoluo_xiangzi";
    public static readonly string EnergyEffectPrefab = "juese_nuqi_tx";
    private static float exterBaseTimeScale = 1f;
    public static readonly Vector3 FighterBoxColliderCenter = new Vector3(0f, 0.5f, 0f);
    public static readonly Vector3 FighterBoxColliderSize = new Vector3(1f, 1f, 1f);
    public static readonly float FighterDeadDurTime = 0.5f;
    public static readonly float FighterHPChangeDelayTime = 0f;
    public static readonly int FighterNumberMax = 100;
    public static readonly int FighterNumberOneSide = 6;
    public static readonly string FightStandAnimName = "stand";
    private static float innerBaseTimeScale = 1f;
    public static readonly string MaterialFSMPrefab = "BattlePrefabs/MaterialFSMInfo";
    public static readonly string MonsterPosSceneObjName = "BattleGroup/monsterPos_";
    public static readonly string MoveAnimName = "move";
    public static readonly string NoShaderChangeTag = "NoShaderChange";
    public static readonly string ParkourFSMPrefab = "BattlePrefabs/ParkourFSMPrefab";
    public static readonly string ParkourMapPrefab = "ParkourMap/Map/";
    public static readonly string ParkourPropPrefab = "ParkourMap/Prop/";
    public static readonly float PauseTimeScale = 0.0001f;
    public static readonly int PhaseLeaveIndex = 3;
    public static readonly int PhaseMaxNumber = 10;
    public static readonly float PlayerMoveSpeed = 10f;
    public static readonly string RestAnimName = "rest";
    public static readonly string ReviveAnimName = "revive";
    public static readonly int RoundMaxNumber = 30;
    private static float ShowTime_TimeScale;
    public static readonly int ShowTimeCameraLayer = 0x10000;
    public static readonly int ShowTimeModelLayer = 0x10;
    public static Func<float, float> speedScaleFunc;
    public static readonly string StandAnimName = "stand_normal";
    public static readonly float SwipFighterTime = 0.25f;
    public static readonly float TalkBoxShowTime = 60f;
    private static float TimeScale = 1f;

    static BattleGlobal()
    {
        if (<>f__am$cache2C == null)
        {
            <>f__am$cache2C = new Func<float, float>(BattleGlobal.<speedScaleFunc>m__5C);
        }
        speedScaleFunc = <>f__am$cache2C;
        ShowTime_TimeScale = 1f;
    }

    [CompilerGenerated]
    private static float <speedScaleFunc>m__5C(float arg)
    {
        return ScaleSpeed(arg);
    }

    public static float GetShowTimeScale()
    {
        return ShowTime_TimeScale;
    }

    public static float GetTimeScale()
    {
        return TimeScale;
    }

    public static Func<float, float> GetTimeScaleFunc(bool isShowTimeEnable)
    {
        if (isShowTimeEnable)
        {
            if (<>f__am$cache2D == null)
            {
                <>f__am$cache2D = arg => ScaleTime_ShowTime(arg);
            }
            return <>f__am$cache2D;
        }
        if (<>f__am$cache2E == null)
        {
            <>f__am$cache2E = arg => ScaleTime(arg);
        }
        return <>f__am$cache2E;
    }

    public static int MonsterFromGlobalToLocalPos(int pos)
    {
        return (pos - FighterNumberMax);
    }

    public static float ScaleSpeed(float speed)
    {
        return (speed * TimeScale);
    }

    public static float ScaleSpeed_ShowTime(float speed)
    {
        return ScaleSpeed(speed * ShowTime_TimeScale);
    }

    public static float ScaleTime(float time)
    {
        return (time / TimeScale);
    }

    public static float ScaleTime_ShowTime(float time)
    {
        return ScaleTime(time / ShowTime_TimeScale);
    }

    public static void SetInnerTimeScale(float scale)
    {
        innerBaseTimeScale = scale;
        TimeScale = exterBaseTimeScale * innerBaseTimeScale;
    }

    public static void SetShowTimeScale(float scale)
    {
        ShowTime_TimeScale = scale;
    }

    public static void SetTimeScale(float scale)
    {
        exterBaseTimeScale = scale;
        TimeScale = exterBaseTimeScale * innerBaseTimeScale;
    }
}

