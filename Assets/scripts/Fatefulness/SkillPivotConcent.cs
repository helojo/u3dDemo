namespace Fatefulness
{
    using Battle;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SkillPivotConcent : PivotConcent
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapB;
        protected bool deposited;

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("Preload", Transition.Direction.output);
            base.RegisterPort<Transition>("Cast", Transition.Direction.output);
            base.RegisterPort<Transition>("CastFinish", Transition.Direction.output);
            base.RegisterPort<Transition>("Confirm", Transition.Direction.output);
            base.RegisterPort<Transition>("Hurt", Transition.Direction.output);
            base.RegisterPort<Transition>("Heal", Transition.Direction.output);
            base.RegisterPort<Transition>("Revive", Transition.Direction.output);
            base.RegisterPort<Transition>("Summon", Transition.Direction.output);
            base.RegisterPort<Transition>("Killed", Transition.Direction.output);
            base.RegisterPort<Transition>("EnergyChange", Transition.Direction.output);
            base.RegisterPort<Transition>("AddBuff", Transition.Direction.output);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            io_context.DeserializationBoolean(out this.deposited);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("the type of intent is not matched witch pivot concent!");
            }
            string key = intent.GetString(IntentDecl.port);
            Transition port = base.GetPort(key);
            if (port != null)
            {
                Intent intent2 = base.root_fragment.global_context;
                if (intent2 == null)
                {
                    throw new UnityException("need global context to explain the skill pivot concent!");
                }
                SkillEffectResult result = intent.GetObject<SkillEffectResult>(SkillIntentDecl.skill_effect_result);
                string str2 = key;
                if (str2 != null)
                {
                    int num;
                    if (<>f__switch$mapB == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
                        dictionary.Add("Confirm", 0);
                        dictionary.Add("Hurt", 1);
                        dictionary.Add("Heal", 1);
                        dictionary.Add("Revive", 2);
                        dictionary.Add("Cast", 3);
                        dictionary.Add("CastFinish", 4);
                        dictionary.Add("Killed", 5);
                        dictionary.Add("EnergyChange", 6);
                        dictionary.Add("AddBuff", 7);
                        <>f__switch$mapB = dictionary;
                    }
                    if (<>f__switch$mapB.TryGetValue(str2, out num))
                    {
                        switch (num)
                        {
                            case 0:
                                intent2.PutInt32(SkillIntentDecl.skill_target_id, intent.GetInt32(SkillIntentDecl.skill_target_id));
                                intent2.PutObject(SkillIntentDecl.sub_skill_index, intent.GetObject(SkillIntentDecl.sub_skill_index));
                                intent.PutBoolean(SkillIntentDecl.skill_deposited, this.deposited);
                                base.root_fragment.stack_context.PutObject(SkillIntentDecl.skill_entry, intent.GetObject(SkillIntentDecl.skill_entry));
                                base.root_fragment.stack_context.PutObject(SkillIntentDecl.sub_skill_index, intent.GetObject(SkillIntentDecl.sub_skill_index));
                                break;

                            case 1:
                                if (result == null)
                                {
                                    throw new UnityException("need effect result object!");
                                }
                                intent2.PutEnumerator(SkillIntentDecl.skill_hurt_type, result.hurtType);
                                intent2.PutInt32(SkillIntentDecl.skill_target_id, result.targetID);
                                intent2.PutEnumerator(SkillIntentDecl.skill_hit_type, result.hitType);
                                intent2.PutInt32(SkillIntentDecl.skill_damage, result.changeValue);
                                break;

                            case 2:
                                if (result == null)
                                {
                                    throw new UnityException("need effect result object!");
                                }
                                intent2.PutInt32(SkillIntentDecl.skill_target_id, result.targetID);
                                break;

                            case 3:
                                intent2.PutObject(SkillIntentDecl.skill_target_id_list, intent.GetObject(SkillIntentDecl.skill_target_id_list));
                                intent2.PutInt32(SkillIntentDecl.skill_activity_target_id, intent.GetInt32(SkillIntentDecl.skill_activity_target_id));
                                break;

                            case 4:
                                intent2.PutObject(SkillIntentDecl.skill_target_id_list, intent.GetObject(SkillIntentDecl.skill_target_id_list));
                                intent2.PutInt32(SkillIntentDecl.skill_activity_target_id, intent.GetInt32(SkillIntentDecl.skill_activity_target_id));
                                break;

                            case 6:
                                intent2.PutInt32(SkillIntentDecl.skill_target_id, intent.GetInt32(SkillIntentDecl.skill_target_id));
                                intent2.PutInt32(SkillIntentDecl.skill_caster_id, intent.GetInt32(SkillIntentDecl.skill_caster_id));
                                intent2.PutInt32(SkillIntentDecl.energy_changed_value, intent.GetInt32(SkillIntentDecl.energy_changed_value));
                                break;
                        }
                    }
                }
                port.Request(intent);
            }
        }

        public override string Name()
        {
            return "Fragment";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("deposited", obj => this.deposited = Convert.ToBoolean(obj), () => this.deposited);
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            io_context.SerializationBoolean(this.deposited);
        }
    }
}

