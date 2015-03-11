namespace Battle
{
    using System;
    using System.Collections.Generic;

    public class AiFindEnemy_Nearest_XY : AiFindEnemy
    {
        public AiActor FindTarget(AiActor actor, AiManager aiManager)
        {
            List<AiActor> list = !actor.IsAttacker ? aiManager.AttackerAiList : aiManager.DefenderAiList;
            float maxValue = float.MaxValue;
            AiActor actor2 = null;
            foreach (AiActor actor3 in list)
            {
                if (AiUtil.IsAvailableTarget(actor3, true, true))
                {
                    float num2 = AiUtil.CalDistanceByX(actor, actor3, aiManager.PathDirection);
                    if (num2 < maxValue)
                    {
                        maxValue = num2;
                        actor2 = actor3;
                    }
                }
            }
            return actor2;
        }
    }
}

