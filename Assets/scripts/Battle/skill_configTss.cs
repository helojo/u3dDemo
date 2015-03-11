namespace Battle
{
    using FastBuf;
    using System;

    public class skill_configTss
    {
        public float cd;
        public string desc;
        public string effects;
        public int entry;
        public string icon;
        public float init_cd;
        public int is_target_enemy;
        public string lv_up_desc;
        public TssSdtInt max_level;
        public string name;
        public float range;
        public int range_type;
        public float range_value_far;
        public float range_value_near;
        public float self_cd;
        public string skill_ai;
        public string skill_explan;
        public TssSdtInt skill_hit_level;
        public string sub_skill_list;
        public int target_type;
        public int type;

        public skill_configTss(skill_config config)
        {
            this.range_type = config.range_type;
            this.range_value_near = config.range_value_near;
            this.target_type = config.target_type;
            this.sub_skill_list = config.sub_skill_list;
            this.range = config.range;
            this.skill_hit_level = config.skill_hit_level;
            this.skill_ai = config.skill_ai;
            this.max_level = config.max_level;
            this.range_value_far = config.range_value_far;
            this.is_target_enemy = config.is_target_enemy;
            this.self_cd = config.self_cd;
            this.skill_explan = config.skill_explan;
            this.lv_up_desc = config.lv_up_desc;
            this.desc = config.desc;
            this.entry = config.entry;
            this.name = config.name;
            this.init_cd = config.init_cd;
            this.cd = config.cd;
            this.type = config.type;
            this.icon = config.icon;
            this.effects = config.effects;
        }
    }
}

