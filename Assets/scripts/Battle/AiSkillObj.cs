namespace Battle
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AiSkillObj : IActor
    {
        private readonly Dictionary<System.Type, AiComponent> m_componentMap;
        private readonly int m_id;

        public AiSkillObj(Battle.AiManager aiManager, AiActor caster) : base(aiManager, caster.IsAttacker)
        {
            this.m_componentMap = new Dictionary<System.Type, AiComponent>();
            this.m_id = aiManager.NewActorId();
            this.Caster = caster;
            aiManager.AddActorToMap(this);
        }

        public void AddComponent<T>(AiComponent component)
        {
            System.Type key = typeof(T);
            if (!this.m_componentMap.ContainsKey(key))
            {
                component.Host = this;
                this.m_componentMap.Add(key, component);
            }
        }

        public void Begin()
        {
            foreach (AiComponent component in this.m_componentMap.Values)
            {
                component.Begin();
            }
        }

        internal AiSkill CastSkill(AiActor target, int skillEntry)
        {
            if (target == null)
            {
                return null;
            }
            AiSkill skill = new AiSkill(base.m_aiManager.NewSkillUniId(), skillEntry, this.Caster, target, base.m_aiManager);
            CastNewSkill_Input input = new CastNewSkill_Input {
                skillUniId = skill.SkillUniId,
                skillEntry = skill.SkillEntry,
                casterID = this.Caster.Id,
                mainTargetList = new List<int> { target.Id }
            };
            base.m_aiManager.NoticeCastNewSkill(input);
            return skill;
        }

        public T GetComponent<T>()
        {
            AiComponent component;
            this.m_componentMap.TryGetValue(typeof(T), out component);
            return (T) Convert.ChangeType(component, typeof(T));
        }

        public override void OnSkillEffectFinish(int skillUniId)
        {
            foreach (AiComponent component in this.m_componentMap.Values)
            {
                component.OnSkillEffectFinish(skillUniId);
            }
        }

        public Battle.AiManager AiManager
        {
            get
            {
                return base.m_aiManager;
            }
        }

        public AiActor Caster { get; private set; }

        public override int Id
        {
            get
            {
                return this.m_id;
            }
        }

        public override Vector3 Pos
        {
            get
            {
                return base.m_aiManager.BattleCom.GetActorPos(this.Id);
            }
        }
    }
}

