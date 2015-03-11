namespace Battle
{
    using System;

    public class AiDef
    {
        public static readonly bool ALLOW_NORMAL_SKILL = true;
        public static readonly int BASE_POINT_OFFSET_UNIT = 0x3e8;
        public static readonly int COST_ENERGY_FOR_ACTIVE_SKILL = 0x3e8;
        public static readonly float DEFAULT_STROLL_PROB = 0.5f;
        public static readonly float FEAR_MOVE_DISTANCE = 5f;
        public static readonly int Hurt_Break_Skill_Scale = 15;
        public static readonly bool INIT_IS_AUTO = true;
        public static readonly int MAX_ACTOR_EACH_SIDE = 100;
        public static readonly int MAX_ACTOR_IN_ROW = 3;
        public static readonly int MAX_ENERGY = 0x3e8;
        public static readonly float MAX_IGNORE_POS_OFFSET = 0.1f;
        public static readonly float MOVE_SPEED = 8f;
        public static readonly int ONE_HUNDRED_PCT_VALUE = 0x2710;
        public static readonly float PK_HP_EX_PCT_VALUE = 10000f;
        public static readonly int Sleep_Hurt_Break_Skill_Scale = 4;
        public static readonly float START_BATTLE_DELAY_FACTOR = 0.2f;
        public static readonly float TARGET_SEARCH_RANGE = 3f;
    }
}

