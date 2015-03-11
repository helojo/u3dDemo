namespace Fatefulness
{
    using Battle;
    using System;
    using UnityEngine;

    public class SkillActivityTarget : SkillTargetConstant
    {
        public override void Excude(Intent intent)
        {
            if (intent.Type != Intent.IntentType.review)
            {
                throw new UnityException("forwarding intent is not matched with skill activity target getter!");
            }
            Intent intent2 = base.root_fragment.global_context;
            if (intent2 == null)
            {
                throw new UnityException("need global context to explain the skill activity target getter!");
            }
            int id = intent2.GetInt32(SkillIntentDecl.skill_activity_target_id);
            AiActor actorById = base.AiMgr.GetActorById(id);
            base.ReturnAndStore(intent, "variable", IntentDecl.variable, actorById);
        }

        public override string Name()
        {
            return "Activity Target";
        }
    }
}

