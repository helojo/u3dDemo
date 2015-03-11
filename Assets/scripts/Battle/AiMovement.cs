namespace Battle
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class AiMovement
    {
        public static Vector3 CalDestPos(AiActor attacker, AiActor target, List<AiActor> allActorList)
        {
            return target.Pos;
        }
    }
}

