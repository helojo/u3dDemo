namespace Battle
{
    using System;
    using UnityEngine;

    public class AiMoveToPos : AiComponent
    {
        private readonly Vector3 m_pos;

        public AiMoveToPos(Vector3 pos)
        {
            this.m_pos = pos;
        }

        public override void Update()
        {
            base.Host.AiManager.NoticeMoveActorToPos(base.Host.Id, this.m_pos, AiDef.MOVE_SPEED, -1);
        }
    }
}

