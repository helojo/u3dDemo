namespace Battle
{
    using System;

    public enum TargetType
    {
        Dead = 3,
        Farthest = 2,
        MaxEnergyButNotFull = 0x63,
        MaxEnergyValue = 7,
        MinHp = 5,
        MinHpValue = 6,
        Nearest = 1,
        NearestCanBeSame = 100,
        Random = 4,
        Self = 0
    }
}

