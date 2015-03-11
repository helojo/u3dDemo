namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillDoSubSkill : Concentrator
    {
        protected AiManager aimgr;
        protected AiSkill skill_object;
        protected int sub_skill_entry = -1;

        public override Concentrator Clone()
        {
            Concentrator concentrator = base.Clone();
            if (concentrator == null)
            {
                return null;
            }
            concentrator.SetSegmentValue("entry", this.sub_skill_entry);
            return concentrator;
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("input", Transition.Direction.input);
            base.RegisterPort<Transition>("variable", Transition.Direction.input);
        }

        public override void Deserialization(IOContext io_context)
        {
            base.Deserialization(io_context);
            this.DoDeSerialization(io_context);
        }

        protected virtual void DoDeSerialization(IOContext io_context)
        {
            io_context.DeserializationInt32(out this.sub_skill_entry);
        }

        protected virtual void DoSerialization(IOContext io_context)
        {
            io_context.SerializationInt32(this.sub_skill_entry);
        }

        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.forward)
            {
                throw new UnityException("reviewing intent is not matched with subskill concentrator!");
            }
            if (this.sub_skill_entry >= 0)
            {
                Transition port = base.GetPort("variable");
                if (port == null)
                {
                    throw new UnityException("unexisted transition port named variable");
                }
                Intent intent2 = new Intent(Intent.IntentType.review);
                port.Review(intent2);
                AiActor target = intent2.GetObject<AiActor>(IntentDecl.variable);
                if (target != null)
                {
                    Intent intent3 = base.root_fragment.global_context;
                    if (intent3 == null)
                    {
                        throw new UnityException("need global context to explain the subskill concentrator!");
                    }
                    if (this.aimgr == null)
                    {
                        this.aimgr = intent3.GetObject<AiManager>(SkillIntentDecl.skill_ai_manager);
                        if (this.aimgr == null)
                        {
                            throw new UnityException("need global context about AIManager to explain the subskill concentrator!");
                        }
                    }
                    if (this.skill_object == null)
                    {
                        this.skill_object = intent3.GetObject<AiSkill>(SkillIntentDecl.skill_ai_skill);
                    }
                    AiActor caster = null;
                    if (caster == null)
                    {
                        int id = intent3.GetInt32(SkillIntentDecl.skill_caster_id);
                        caster = this.aimgr.GetActorById(id);
                        if (caster == null)
                        {
                            throw new UnityException("can not find the skill actor id = " + id);
                        }
                    }
                    int skillLV = 1;
                    int skillHitLV = 1;
                    AiBuffInfo buffCombatInfo = null;
                    if (this.skill_object == null)
                    {
                        AiBuff buff = intent3.GetObject<AiBuff>(SkillIntentDecl.skill_ai_buffer);
                        if (buff != null)
                        {
                            skillLV = (int) buff.skillLV;
                            skillHitLV = (int) buff.skillHitLV;
                            buffCombatInfo = buff.combatInfo;
                        }
                        else
                        {
                            Debug.LogWarning("No skillLV!");
                        }
                    }
                    this.aimgr.OnEventSubEffect(this.skill_object, this.sub_skill_entry, target, caster, skillLV, skillHitLV, buffCombatInfo);
                }
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("input");
        }

        public override string Name()
        {
            return "DoSubSkill";
        }

        public override void RegisterSerializableSegment()
        {
            base.RegisterSerializableSegment("entry", obj => this.sub_skill_entry = Convert.ToInt32(obj), () => this.sub_skill_entry);
        }

        public override void Reset()
        {
            base.Reset();
            this.aimgr = null;
            this.skill_object = null;
        }

        public override void Serialization(IOContext io_context)
        {
            base.Serialization(io_context);
            this.DoSerialization(io_context);
        }
    }
}

