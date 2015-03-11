namespace Battle
{
    using System;

    public class CombatEvent
    {
        public int actorID;
        public int changeValue;
        public SkillHitType hitType;
        public long hpValue;
        public SkillHurtType hurtType;
        public int relatedActorID;
        public CombatEventType type;
    }
}

