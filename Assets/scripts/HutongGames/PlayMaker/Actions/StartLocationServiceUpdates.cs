namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Device), Tooltip("Starts location service updates. Last location coordinates can be retrieved with GetLocationInfo.")]
    public class StartLocationServiceUpdates : FsmStateAction
    {
        public FsmFloat desiredAccuracy;
        [Tooltip("Event to send if the location services fail to start.")]
        public FsmEvent failedEvent;
        [Tooltip("Maximum time to wait in seconds before failing.")]
        public FsmFloat maxWait;
        private float startTime;
        [Tooltip("Event to send when the location services have started.")]
        public FsmEvent successEvent;
        public FsmFloat updateDistance;

        public override void OnEnter()
        {
            this.startTime = FsmTime.RealtimeSinceStartup;
            Input.location.Start(this.desiredAccuracy.Value, this.updateDistance.Value);
        }

        public override void OnUpdate()
        {
            if (((Input.location.status == LocationServiceStatus.Failed) || (Input.location.status == LocationServiceStatus.Stopped)) || ((FsmTime.RealtimeSinceStartup - this.startTime) > this.maxWait.Value))
            {
                base.Fsm.Event(this.failedEvent);
                base.Finish();
            }
            if (Input.location.status == LocationServiceStatus.Running)
            {
                base.Fsm.Event(this.successEvent);
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.maxWait = 20f;
            this.desiredAccuracy = 10f;
            this.updateDistance = 10f;
            this.successEvent = null;
            this.failedEvent = null;
        }
    }
}

