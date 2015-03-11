namespace Battle
{
    using System;
    using System.Collections.Generic;

    public class AiFindEnemy_Farthest_XY : AiFindEnemy
    {
        public AiActor FindTarget(AiActor actor, AiManager aiManager)
        {
            List<AiActor> list = !actor.IsAttacker ? aiManager.AttackerAiList : aiManager.DefenderAiList;
            float num = 0f;
            AiActor actor2 = null;
            foreach (AiActor actor3 in list)
            {
                if (AiUtil.IsAvailableTarget(actor3, true, true))
                {
                    float num2 = AiUtil.CalDistanceByX(actor, actor3, aiManager.PathDirection);
                    if (num2 > num)
                    {
                        num = num2;
                        actor2 = actor3;
                    }
                }
            }
            return actor2;
        }
    }
}

