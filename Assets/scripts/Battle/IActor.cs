namespace Battle
{
    using System;
    using UnityEngine;

    public class IActor
    {
        public readonly AiManager m_aiManager;
        private readonly bool m_isAttacker = true;

        public IActor(AiManager aiManager, bool isAttacker)
        {
            this.m_aiManager = aiManager;
            this.m_isAttacker = isAttacker;
        }

        public virtual void OnSkillEffectFinish(int skillUniId)
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void Update(float detlaTime)
        {
        }

        public AiActor EnemeyHeadActor
        {
            get
            {
                return AiUtil.FindHeadActor(this.EnemyTeam, null);
            }
        }

        public AiTeam EnemyTeam
        {
            get
            {
                return this.m_aiManager.GetTeam(!this.IsAttacker);
            }
        }

        public Vector3 HeadDirection
        {
            get
            {
                return (!this.IsAttacker ? -this.m_aiManager.PathDirection : this.m_aiManager.PathDirection);
            }
        }

        public virtual int Id
        {
            get
            {
                return -1;
            }
        }

        public virtual bool IsAttacker
        {
            get
            {
                return this.m_isAttacker;
            }
        }

        public virtual Vector3 Pos
        {
            get
            {
                return Vector3.zero;
            }
        }

        public float PosInHorizontal
        {
            get
            {
                return AiUtil.CalPosInAxis(this.RelativePos, this.m_aiManager.PathDirection);
            }
        }

        public float PosInVertical
        {
            get
            {
                return AiUtil.CalPosInAxis(this.RelativePos, this.m_aiManager.VerticalDirection);
            }
        }

        public Vector3 RelativePos
        {
            get
            {
                return (this.Pos - this.m_aiManager.CenterPos);
            }
        }

        public AiTeam SelfTeam
        {
            get
            {
                return this.m_aiManager.GetTeam(this.IsAttacker);
            }
        }
    }
}

