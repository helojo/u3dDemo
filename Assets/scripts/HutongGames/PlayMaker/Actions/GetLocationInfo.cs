namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Device), Tooltip("Gets Location Info from a mobile device. NOTE: Use StartLocationService before trying to get location info.")]
    public class GetLocationInfo : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        public FsmFloat altitude;
        [Tooltip("Event to send if the location cannot be queried.")]
        public FsmEvent errorEvent;
        [UIHint(UIHint.Variable)]
        public FsmFloat horizontalAccuracy;
        [UIHint(UIHint.Variable)]
        public FsmFloat latitude;
        [UIHint(UIHint.Variable)]
        public FsmFloat longitude;
        [UIHint(UIHint.Variable)]
        public FsmVector3 vectorPosition;
        [UIHint(UIHint.Variable)]
        public FsmFloat verticalAccuracy;

        private void DoGetLocationInfo()
        {
            if (Input.location.status != LocationServiceStatus.Running)
            {
                base.Fsm.Event(this.errorEvent);
            }
            else
            {
                float longitude = Input.location.lastData.longitude;
                float latitude = Input.location.lastData.latitude;
                float altitude = Input.location.lastData.altitude;
                this.vectorPosition.Value = new Vector3(longitude, latitude, altitude);
                this.longitude.Value = longitude;
                this.latitude.Value = latitude;
                this.altitude.Value = altitude;
                this.horizontalAccuracy.Value = Input.location.lastData.horizontalAccuracy;
                this.verticalAccuracy.Value = Input.location.lastData.verticalAccuracy;
            }
        }

        public override void OnEnter()
        {
            this.DoGetLocationInfo();
            base.Finish();
        }

        public override void Reset()
        {
            this.longitude = null;
            this.latitude = null;
            this.altitude = null;
            this.horizontalAccuracy = null;
            this.verticalAccuracy = null;
            this.errorEvent = null;
        }
    }
}

