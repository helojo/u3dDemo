namespace Battle
{
    using FastBuf;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    internal class AiUtil
    {
        public static float CalDistanceByX(IActor actor, IActor target, Vector3 direction)
        {
            Vector3 vector = target.Pos - actor.Pos;
            return Vector3.Project(vector, direction).magnitude;
        }

        public static float CalDistanceByXWithDir(AiActor actor, AiActor target)
        {
            Vector3 headDirection = actor.HeadDirection;
            Vector3 lhs = target.Pos - actor.Pos;
            float num = (Vector3.Dot(lhs, headDirection) <= 0f) ? ((float) (-1)) : ((float) 1);
            float b = Vector3.Project(lhs, headDirection).magnitude - target.Radius;
            return (Mathf.Max(0f, b) * num);
        }

        public static float CalDistanceByXWithDirByPos(Vector3 pos, Vector3 headDir, AiActor target)
        {
            Vector3 rhs = headDir;
            Vector3 lhs = target.Pos - pos;
            float num = (Vector3.Dot(lhs, rhs) <= 0f) ? ((float) (-1)) : ((float) 1);
            float b = Vector3.Project(lhs, rhs).magnitude - target.Radius;
            return (Mathf.Max(0f, b) * num);
        }

        public static float CalDistanceByXY(IActor actor, IActor target)
        {
            return Vector3.Distance(actor.Pos, target.Pos);
        }

        public static float CalPosInAxis(Vector3 pos, Vector3 axisDirection)
        {
            return Vector3.Dot(Vector3.Project(pos, axisDirection), axisDirection);
        }

        public static float CalTeamDistance(AiTeam team1, AiTeam team2, Vector3 direction)
        {
            <CalTeamDistance>c__AnonStoreyDC ydc = new <CalTeamDistance>c__AnonStoreyDC {
                team2 = team2,
                direction = direction
            };
            float[] second = new float[] { float.MaxValue };
            return team1.AliveActorList.SelectMany<AiActor, AiActor, float>(new Func<AiActor, IEnumerable<AiActor>>(ydc.<>m__3B), new Func<AiActor, AiActor, float>(ydc.<>m__3C)).Concat<float>(second).Min();
        }

        public static bool CheckIsPlayerInPos(AiActor attacker, Vector3 targetPos, List<AiActor> allActorList)
        {
            foreach (AiActor actor in allActorList)
            {
                if ((attacker.Id != actor.Id) && (Vector3.Distance(targetPos, actor.Pos) < ((attacker.Radius + actor.Radius) * 0.9f)))
                {
                    return true;
                }
            }
            return false;
        }

        public static AiActor FindFarthestTarget(IActor actor, List<AiActor> targetList, Vector3 direction)
        {
            float num = 0f;
            AiActor actor2 = null;
            foreach (AiActor actor3 in targetList)
            {
                if ((IsAvailableTarget(actor3, true, true) && (actor3 != actor)) && actor3.IsCanBeTarget)
                {
                    float num2 = CalDistanceByX(actor, actor3, direction);
                    if (num2 > num)
                    {
                        num = num2;
                        actor2 = actor3;
                    }
                }
            }
            return actor2;
        }

        public static AiActor FindHeadActor(AiTeam team, IActor excludeObj = null)
        {
            float minValue = float.MinValue;
            AiActor actor = null;
            float maxValue = float.MaxValue;
            AiActor actor2 = null;
            foreach (AiActor actor3 in team.ActorList)
            {
                if (((actor3 != excludeObj) && actor3.IsAlive) && (actor3.IsVisible && actor3.IsCanBeTarget))
                {
                    float posInHorizontal = actor3.PosInHorizontal;
                    if (posInHorizontal < maxValue)
                    {
                        maxValue = posInHorizontal;
                        actor2 = actor3;
                    }
                    if (posInHorizontal > minValue)
                    {
                        minValue = posInHorizontal;
                        actor = actor3;
                    }
                }
            }
            return (!team.IsAttacker ? actor2 : actor);
        }

        public static AiActor FindLastActor(AiTeam team, IActor excludeObj = null)
        {
            float minValue = float.MinValue;
            AiActor actor = null;
            float maxValue = float.MaxValue;
            AiActor actor2 = null;
            foreach (AiActor actor3 in team.ActorList)
            {
                if (((actor3 != excludeObj) && actor3.IsAlive) && (actor3.IsVisible && actor3.IsCanBeTarget))
                {
                    float posInHorizontal = actor3.PosInHorizontal;
                    if (posInHorizontal < maxValue)
                    {
                        maxValue = posInHorizontal;
                        actor2 = actor3;
                    }
                    if (posInHorizontal > minValue)
                    {
                        minValue = posInHorizontal;
                        actor = actor3;
                    }
                }
            }
            return (!team.IsAttacker ? actor : actor2);
        }

        public static AiActor FindMaxEnergyNotFullTarget(List<AiActor> targetList, AiActor lastTarget)
        {
            long energy = 0L;
            AiActor actor = null;
            foreach (AiActor actor2 in targetList)
            {
                if ((((lastTarget != actor2) && actor2.IsCanBeTarget) && (actor2.Energy != AiDef.MAX_ENERGY)) && (actor2.Energy > energy))
                {
                    energy = actor2.Energy;
                    actor = actor2;
                }
            }
            if ((actor == null) && (targetList.Count > 0))
            {
                actor = targetList[0];
            }
            return actor;
        }

        public static AiActor FindMaxEnergyValueTarget(List<AiActor> targetList, AiActor lastTarget)
        {
            long energy = 0L;
            AiActor actor = null;
            foreach (AiActor actor2 in targetList)
            {
                if (((lastTarget != actor2) && actor2.IsCanBeTarget) && (actor2.Energy > energy))
                {
                    energy = actor2.Energy;
                    actor = actor2;
                }
            }
            return actor;
        }

        public static AiActor FindMaxHateTarget(IActor actor, AiTeam team, AiActor priorTarget)
        {
            AiActor actor2 = FindHeadActor(team, actor);
            if (actor2 == null)
            {
                return null;
            }
            List<AiActor> aliveActorList = team.AliveActorList;
            AiActor actor3 = actor2;
            foreach (AiActor actor4 in aliveActorList)
            {
                if ((((actor4 != actor2) && (actor4 != actor)) && (actor4.IsCanBeTarget && (CalDistanceByX(actor2, actor4, actor2.HeadDirection) <= AiDef.TARGET_SEARCH_RANGE))) && (actor4.Hate > actor3.Hate))
                {
                    actor3 = actor4;
                }
            }
            if (((priorTarget != null) && priorTarget.IsAlive) && ((Vector3.Distance(actor2.Pos, priorTarget.Pos) < AiDef.TARGET_SEARCH_RANGE) && (priorTarget.Hate >= actor3.Hate)))
            {
                actor3 = priorTarget;
            }
            return actor3;
        }

        public static AiActor FindMinHpTarget(List<AiActor> targetList, AiActor lastTarget)
        {
            float num = 1.1f;
            AiActor actor = null;
            foreach (AiActor actor2 in targetList)
            {
                if ((lastTarget != actor2) && actor2.IsCanBeTarget)
                {
                    float num2 = (float) (((double) actor2.Hp) / ((double) actor2.MaxHp));
                    if (num2 < num)
                    {
                        num = num2;
                        actor = actor2;
                    }
                }
            }
            return actor;
        }

        public static AiActor FindMinHpValueTarget(List<AiActor> targetList, AiActor lastTarget)
        {
            long hp = 0x7fffffffffffffffL;
            AiActor actor = null;
            foreach (AiActor actor2 in targetList)
            {
                if (((lastTarget != actor2) && actor2.IsCanBeTarget) && (actor2.Hp < hp))
                {
                    hp = (long) actor2.Hp;
                    actor = actor2;
                }
            }
            return actor;
        }

        public static AiActor FindNearestTarget(IActor actor, List<AiActor> targetList, Vector3 direction, List<AiActor> oldTargets)
        {
            float maxValue = float.MaxValue;
            AiActor actor2 = null;
            foreach (AiActor actor3 in targetList)
            {
                if ((IsAvailableTarget(actor3, true, true) && (actor3 != actor)) && (((oldTargets == null) || !oldTargets.Contains(actor3)) && actor3.IsCanBeTarget))
                {
                    float num2 = CalDistanceByX(actor, actor3, direction) - actor3.Radius;
                    if (num2 < maxValue)
                    {
                        maxValue = num2;
                        actor2 = actor3;
                    }
                }
            }
            return actor2;
        }

        public static List<AiActor> FindNearTarget(Vector3 pos, List<AiActor> targetList, Vector3 direction, float near_dis, float far_dis)
        {
            List<AiActor> list = new List<AiActor>();
            foreach (AiActor actor in targetList)
            {
                if (IsAvailableTarget(actor, true, true) && actor.IsCanBeTarget)
                {
                    float num = CalDistanceByXWithDirByPos(pos, direction, actor);
                    if ((num <= far_dis) && (num >= near_dis))
                    {
                        list.Add(actor);
                    }
                }
            }
            return list;
        }

        public static AiActor FindRandomTarget(AiActor actor, List<AiActor> targetList, Vector3 direction, float range)
        {
            <FindRandomTarget>c__AnonStoreyDD ydd = new <FindRandomTarget>c__AnonStoreyDD {
                actor = actor,
                direction = direction,
                range = range
            };
            if (targetList.Count == 0)
            {
                return null;
            }
            ydd.targetsInRange = new List<AiActor>();
            targetList.ForEach(new Action<AiActor>(ydd.<>m__3D));
            if (ydd.targetsInRange.Count == 0)
            {
                return null;
            }
            return ydd.targetsInRange[UnityEngine.Random.Range(0, ydd.targetsInRange.Count)];
        }

        public static float GetSkillRange(int entry, float scale)
        {
            skill_config _config = ConfigMgr.getInstance().getByEntry<skill_config>(entry);
            if (_config != null)
            {
                return (_config.range * scale);
            }
            return 0f;
        }

        public static bool IsAvailableTarget(AiActor target, bool isAlive, bool isVisible)
        {
            if (target.IsEmpty)
            {
                return false;
            }
            if (isAlive && !target.IsAlive)
            {
                return false;
            }
            if (isVisible && !target.IsVisible)
            {
                return false;
            }
            return true;
        }

        public static bool IsCollide(AiActor actor1, AiActor actor2)
        {
            return (Vector3.Distance(actor1.Pos, actor2.Pos) < (actor1.Radius + actor2.Radius));
        }

        public static void Log(string format, params object[] args)
        {
        }

        [CompilerGenerated]
        private sealed class <CalTeamDistance>c__AnonStoreyDC
        {
            internal Vector3 direction;
            internal AiTeam team2;

            internal IEnumerable<AiActor> <>m__3B(AiActor actor1)
            {
                return this.team2.AliveActorList;
            }

            internal float <>m__3C(AiActor actor1, AiActor actor2)
            {
                return AiUtil.CalDistanceByX(actor1, actor2, this.direction);
            }
        }

        [CompilerGenerated]
        private sealed class <FindRandomTarget>c__AnonStoreyDD
        {
            internal AiActor actor;
            internal Vector3 direction;
            internal float range;
            internal List<AiActor> targetsInRange;

            internal void <>m__3D(AiActor obj)
            {
                float num = AiUtil.CalDistanceByX(this.actor, obj, this.direction) - obj.Radius;
                if ((num <= (this.range + 0.1)) && obj.IsCanBeTarget)
                {
                    this.targetsInRange.Add(obj);
                }
            }
        }
    }
}

