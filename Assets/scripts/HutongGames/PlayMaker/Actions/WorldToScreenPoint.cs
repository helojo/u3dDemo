namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Transforms position from world space into screen space. NOTE: Uses the MainCamera!"), ActionCategory(ActionCategory.Camera)]
    public class WorldToScreenPoint : FsmStateAction
    {
        [Tooltip("Repeat every frame")]
        public bool everyFrame;
        [Tooltip("Use NGUI Coordinate")]
        public bool nguiCoordinate;
        [Tooltip("Normalize screen coordinates (0-1). Otherwise coordinates are in pixels.")]
        public FsmBool normalize;
        [UIHint(UIHint.Variable), Tooltip("Store the screen position in a Vector3 Variable. Z will equal zero.")]
        public FsmVector3 storeScreenPoint;
        [UIHint(UIHint.Variable), Tooltip("Store the screen X position in a Float Variable.")]
        public FsmFloat storeScreenX;
        [Tooltip("Store the screen Y position in a Float Variable."), UIHint(UIHint.Variable)]
        public FsmFloat storeScreenY;
        [UIHint(UIHint.Variable), Tooltip("World position to transform into screen coordinates.")]
        public FsmVector3 worldPosition;
        [Tooltip("World X position.")]
        public FsmFloat worldX;
        [Tooltip("World Y position.")]
        public FsmFloat worldY;
        [Tooltip("World Z position.")]
        public FsmFloat worldZ;

        private void DoWorldToScreenPoint()
        {
            if (Camera.main == null)
            {
                this.LogError("No MainCamera defined!");
                base.Finish();
            }
            else
            {
                Vector3 zero = Vector3.zero;
                if (!this.worldPosition.IsNone)
                {
                    zero = this.worldPosition.Value;
                }
                if (!this.worldX.IsNone)
                {
                    zero.x = this.worldX.Value;
                }
                if (!this.worldY.IsNone)
                {
                    zero.y = this.worldY.Value;
                }
                if (!this.worldZ.IsNone)
                {
                    zero.z = this.worldZ.Value;
                }
                if (this.nguiCoordinate)
                {
                    zero = Camera.main.WorldToViewportPoint(zero);
                    int activeHeight = UIRoot.list[0].activeHeight;
                    zero.x = ((zero.x - 0.5f) * (((float) Screen.width) / ((float) Screen.height))) * activeHeight;
                    zero.y = (zero.y - 0.5f) * activeHeight;
                }
                else
                {
                    zero = Camera.main.WorldToScreenPoint(zero);
                }
                if (this.normalize.Value)
                {
                    zero.x /= (float) Screen.width;
                    zero.y /= (float) Screen.height;
                }
                this.storeScreenPoint.Value = zero;
                this.storeScreenX.Value = zero.x;
                this.storeScreenY.Value = zero.y;
            }
        }

        public override void OnEnter()
        {
            this.DoWorldToScreenPoint();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoWorldToScreenPoint();
        }

        public override void Reset()
        {
            this.worldPosition = null;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.worldX = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.worldY = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.worldZ = num;
            this.storeScreenPoint = null;
            this.storeScreenX = null;
            this.storeScreenY = null;
            this.everyFrame = false;
            this.nguiCoordinate = false;
        }
    }
}

