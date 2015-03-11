using System;
using UnityEngine;

public class MovementControler
{
    private GameObject animObj;
    private bool isRuning;
    private float modelScale = 1f;
    private string moveAnimName;
    private float moveSpeed = 1f;
    private Vector3? moveTargetPos;
    private NavMeshAgent navAgent;
    private Transform objTrans;
    public System.Action OnMoveFinishEvent;
    private float radius;
    private static readonly float RadiusScale = 0.5f;
    private float ShowTimeScale = 1f;
    private float speedScale = 1f;
    private Vector3? turnTargetPos;

    private GameObject GetAnimObj()
    {
        return this.animObj;
    }

    private Transform GetAnimObjTrans()
    {
        return this.objTrans;
    }

    public bool GetCollsionEnable()
    {
        return ((this.navAgent != null) && (this.navAgent.obstacleAvoidanceType != ObstacleAvoidanceType.NoObstacleAvoidance));
    }

    public float GetMoveAcceleration()
    {
        return this.navAgent.acceleration;
    }

    private string GetMoveAnim()
    {
        return (!string.IsNullOrEmpty(this.moveAnimName) ? this.moveAnimName : BattleGlobal.MoveAnimName);
    }

    public float GetMoveSpeed()
    {
        return this.moveSpeed;
    }

    public Vector3 GetPosition()
    {
        return this.GetAnimObjTrans().position;
    }

    public float GetRadius()
    {
        return (this.radius * this.modelScale);
    }

    public void Init(float _radius, GameObject parent)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(parent.transform.position, out hit, 50f, 1))
        {
            parent.transform.position = hit.position;
        }
        this.objTrans = parent.transform;
        this.radius = _radius;
        this.navAgent = parent.AddComponent<NavMeshAgent>();
        this.navAgent.radius = this.radius * RadiusScale;
        this.navAgent.acceleration = 50f;
        this.navAgent.angularSpeed = 360f;
        this.navAgent.autoBraking = true;
        this.navAgent.autoRepath = true;
        this.navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        if (this.objTrans.GetComponent<BattleFighter>().PosIndex < BattleGlobal.FighterNumberMax)
        {
            this.navAgent.avoidancePriority = 0x15;
        }
        else
        {
            this.navAgent.avoidancePriority = 0x33;
        }
        this.navAgent.speed = BattleGlobal.DefaultMoveSpeed;
        this.navAgent.Warp(this.objTrans.transform.position);
        if (_radius < 0.01f)
        {
            this.navAgent.radius = 0.01f;
            this.navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            this.navAgent.avoidancePriority = 60;
        }
        this.moveSpeed = this.navAgent.speed;
    }

    public bool IsMoveing()
    {
        return this.isRuning;
    }

    private void OnMoveFinish()
    {
        if (this.OnMoveFinishEvent != null)
        {
            this.OnMoveFinishEvent();
        }
        this.StopMove();
    }

    private void OnSpeedChange()
    {
        if (this.navAgent != null)
        {
            this.navAgent.speed = (this.speedScale * this.moveSpeed) * this.ShowTimeScale;
        }
    }

    private void OnStartMove()
    {
        if (!this.isRuning)
        {
            this.isRuning = true;
            GameObject animObj = this.GetAnimObj();
            if (animObj != null)
            {
                animObj.GetComponent<AnimFSM>().PlayAnim(this.GetMoveAnim(), 1f, 0f, false);
            }
        }
    }

    private void OnStopMove()
    {
        this.isRuning = false;
        GameObject animObj = this.GetAnimObj();
        if (animObj != null)
        {
            animObj.GetComponent<AnimFSM>().StopCurAnim(this.GetMoveAnim());
        }
    }

    public void SetAnimObj(GameObject _animObj)
    {
        this.animObj = _animObj;
    }

    public void SetCollsionEnable(bool enable)
    {
        if (this.navAgent != null)
        {
            this.navAgent.obstacleAvoidanceType = !enable ? ObstacleAvoidanceType.NoObstacleAvoidance : ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
    }

    public void SetDead(bool _isDead)
    {
        if (_isDead)
        {
            this.navAgent.enabled = false;
        }
        else
        {
            this.navAgent.enabled = true;
        }
    }

    public void SetModelScale(float _modelScale)
    {
        this.modelScale = _modelScale;
    }

    public void SetMoveAcceleration(float acc)
    {
        this.navAgent.acceleration = acc;
    }

    public void SetMoveAnim(string moveAnim)
    {
        this.moveAnimName = moveAnim;
    }

    public void SetMoveSpeed(float _speed)
    {
        if (this.moveSpeed != _speed)
        {
            this.moveSpeed = _speed;
            this.OnSpeedChange();
        }
    }

    public void SetPosForce(Vector3 pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 50f, this.navAgent.walkableMask))
        {
            this.navAgent.Warp(hit.position);
        }
    }

    public void SetShowTimeSpeedScale(float _scale)
    {
        if (this.ShowTimeScale != _scale)
        {
            this.ShowTimeScale = _scale;
            this.OnSpeedChange();
        }
    }

    public void SetSpeedScale(float _scale)
    {
        if (this.speedScale != _scale)
        {
            this.speedScale = _scale;
            this.OnSpeedChange();
        }
    }

    public void StartBattle()
    {
    }

    public void StartMoveToPos(Vector3 _targetPos, float stopDis)
    {
        if (this.navAgent != null)
        {
            if (stopDis > 0f)
            {
                this.navAgent.stoppingDistance = (stopDis + this.radius) * RadiusScale;
            }
            else
            {
                this.navAgent.stoppingDistance = 0f;
            }
            this.navAgent.SetDestination(_targetPos);
            this.OnStartMove();
        }
        this.turnTargetPos = null;
    }

    public void StartTurnToPos(Vector3 _targetPos)
    {
        this.turnTargetPos = new Vector3?(_targetPos);
        if (!this.isRuning && this.turnTargetPos.HasValue)
        {
            this.GetAnimObjTrans().LookAt(this.turnTargetPos.Value);
        }
        this.turnTargetPos = null;
    }

    public void StopMove()
    {
        if ((this.navAgent != null) && this.navAgent.enabled)
        {
            this.navAgent.Stop(true);
        }
        if (this.isRuning)
        {
            this.OnStopMove();
        }
    }

    public void Tick()
    {
        if (this.isRuning)
        {
            Vector3 position = this.GetPosition();
            position.y = 0f;
            Vector3 destination = this.navAgent.destination;
            destination.y = 0f;
            if (Vector3.Distance(position, destination) < 0.1f)
            {
                this.OnMoveFinish();
            }
        }
    }
}

