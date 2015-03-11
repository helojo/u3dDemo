namespace Battle
{
    using System;

    public class AiMoveToActor : AiComponent
    {
        private readonly AiActor m_target;

        public AiMoveToActor(AiActor target)
        {
            this.m_target = target;
        }

        public override void Update()
        {
            base.Host.AiManager.NoticeMoveActorToPos(base.Host.Id, this.m_target.Pos, AiDef.MOVE_SPEED, this.m_target.Id);
        }
    }
}

