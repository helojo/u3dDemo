namespace Battle
{
    using System;
    using System.Collections.Generic;

    public class CastNewSkill_Input
    {
        public int casterID = -1;
        public List<int> mainTargetList = new List<int>();
        public float offset;
        public int skillEntry = -1;
        public int skillUniId = -1;
    }
}

