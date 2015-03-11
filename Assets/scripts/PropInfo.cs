using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PropInfo : MonoBehaviour
{
    private bool checkLengthEvent;
    public float colideDistance;
    public PropEvent coliderEvent;
    public float enterTriggerDistance;
    public PropEvent enterTriggerEvent;
    public float hightDistance;
    public bool loopAnim;
    public Animation propAnim;
    public int propEntry;
    public ParticleSystem ps;
    public PropEvent tDistanceEvent;
    private Transform thisT;
    public float triggerDistance;
    public PropType type;
    public float valueInfo;

    public void Init()
    {
    }

    public void OnDrawGizmos()
    {
        if (this.triggerDistance != 0f)
        {
            Gizmos.DrawLine(base.transform.position, base.transform.position + new Vector3(0f, 0f, -this.triggerDistance));
        }
        if (this.hightDistance != 0f)
        {
            Gizmos.DrawLine(base.transform.position, base.transform.position + new Vector3(0f, this.hightDistance, 0f));
        }
    }

    public void Start()
    {
        this.thisT = base.transform;
        if ((this.propAnim != null) && this.loopAnim)
        {
            this.propAnim.clip.wrapMode = WrapMode.Loop;
            this.propAnim.Play();
        }
        ParkourEvent._instance.AddEventToProp(this);
    }

    public void Update()
    {
        if (ParkourManager._instance.GameStart && ((this.thisT.position.z - ParkourManager._instance.cCtrl.thisT.position.z) <= 100f))
        {
            if ((this.triggerDistance != 0f) && (Mathf.Abs((float) (ParkourManager._instance.cCtrl.thisT.position.z - this.thisT.position.z)) < this.triggerDistance))
            {
                if ((this.tDistanceEvent != null) && !this.checkLengthEvent)
                {
                    this.tDistanceEvent(this);
                    this.tDistanceEvent = null;
                }
                else if ((this.tDistanceEvent != null) && this.checkLengthEvent)
                {
                    this.tDistanceEvent(this);
                }
            }
            if (((Mathf.Abs((float) (this.thisT.position.z - ParkourManager._instance.cCtrl.characterTr.position.z)) < this.enterTriggerDistance) && (this.enterTriggerEvent != null)) && ((Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.x - this.thisT.position.x)) < 1.5f) && (Vector3.Distance(ParkourManager._instance.cCtrl.characterTr.position, this.thisT.position) < this.enterTriggerDistance)))
            {
                this.enterTriggerEvent(this);
                this.enterTriggerEvent = null;
            }
            if ((this.colideDistance != 0f) && (Mathf.Abs((float) (this.thisT.position.z - ParkourManager._instance.cCtrl.characterTr.position.z)) < this.colideDistance))
            {
                if (((this.coliderEvent != null) && (Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.x - this.thisT.position.x)) < 1f)) && !this.checkLengthEvent)
                {
                    this.coliderEvent(this);
                    this.coliderEvent = null;
                }
                else if (((this.coliderEvent != null) && (Mathf.Abs((float) (ParkourManager._instance.cCtrl.characterTr.position.x - this.thisT.position.x)) < 1f)) && this.checkLengthEvent)
                {
                    this.coliderEvent(this);
                }
            }
        }
    }

    public bool isCheckLengthEvent
    {
        set
        {
            this.checkLengthEvent = value;
        }
    }

    public delegate void PropEvent(PropInfo info);
}

