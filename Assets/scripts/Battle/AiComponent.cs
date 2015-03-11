namespace Battle
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AiComponent
    {
        protected AiComponent()
        {
        }

        public virtual void Begin()
        {
        }

        public virtual void OnSkillEffectFinish(int skillUniId)
        {
        }

        public abstract void Update();

        public AiSkillObj Host { get; set; }
    }
}

