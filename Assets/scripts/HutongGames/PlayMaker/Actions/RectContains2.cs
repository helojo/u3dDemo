namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Rect), Tooltip("Tests if a point is inside a rectangle. It also takes in account negative width and height")]
    public class RectContains2 : FsmStateAction
    {
        [Tooltip("Repeat every frame.")]
        public bool everyFrame;
        [Tooltip("Event to send if the Point is outside the Rectangle.")]
        public FsmEvent falseEvent;
        [Tooltip("Point to test.")]
        public FsmVector3 point;
        [Tooltip("Rectangle to test."), RequiredField]
        public FsmRect rectangle;
        [Tooltip("Store the result in a variable."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;
        [Tooltip("Event to send if the Point is inside the Rectangle.")]
        public FsmEvent trueEvent;
        [Tooltip("Specify/override X value.")]
        public FsmFloat x;
        [Tooltip("Specify/override Y value.")]
        public FsmFloat y;

        private void DoRectContains()
        {
            if (!this.rectangle.IsNone)
            {
                Vector3 point = this.point.Value;
                if (!this.x.IsNone)
                {
                    point.x = this.x.Value;
                }
                if (!this.y.IsNone)
                {
                    point.y = this.y.Value;
                }
                Rect rect = this.rectangle.Value;
                if (rect.width < 0f)
                {
                    rect.x = this.rectangle.Value.x + this.rectangle.Value.width;
                    rect.width = -this.rectangle.Value.width;
                }
                if (rect.height < 0f)
                {
                    rect.y = this.rectangle.Value.y + this.rectangle.Value.height;
                    rect.height = -this.rectangle.Value.height;
                }
                bool flag = rect.Contains(point);
                this.storeResult.Value = flag;
                base.Fsm.Event(!flag ? this.falseEvent : this.trueEvent);
            }
        }

        public override void OnEnter()
        {
            this.DoRectContains();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoRectContains();
        }

        public override void Reset()
        {
            FsmRect rect = new FsmRect {
                UseVariable = true
            };
            this.rectangle = rect;
            FsmVector3 vector = new FsmVector3 {
                UseVariable = true
            };
            this.point = vector;
            FsmFloat num = new FsmFloat {
                UseVariable = true
            };
            this.x = num;
            num = new FsmFloat {
                UseVariable = true
            };
            this.y = num;
            this.storeResult = null;
            this.trueEvent = null;
            this.falseEvent = null;
            this.everyFrame = false;
        }
    }
}

