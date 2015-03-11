namespace Battle
{
    public interface AiFindEnemy
    {
        AiActor FindTarget(AiActor actor, AiManager aiManager);
    }
}

