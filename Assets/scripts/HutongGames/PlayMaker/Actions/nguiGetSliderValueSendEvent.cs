namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Get Value in Progress Bar or Slider with compare Event"), ActionCategory("Ngui Actions")]
    public class nguiGetSliderValueSendEvent : FsmStateAction
    {
        public FsmEvent equal;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("Store current value"), UIHint(UIHint.FsmFloat), HasFloatSlider(0f, 1f)]
        public FsmFloat getValue;
        public FsmEvent greaterThan;
        public FsmEvent lessThan;
        [RequiredField, ActionSection("GET VALUE"), Tooltip("NGUI UISlider to Set value")]
        public FsmOwnerDefault NguiUISliderObject;
        private UISlider theNguiUISlider;
        private float tolerance;
        [ActionSection("SEND EVENT")]
        public FsmFloat valueCondition;

        private void doGetvalue()
        {
            this.getValue.Value = this.theNguiUISlider.value;
            if (Mathf.Abs((float) (this.getValue.Value - this.valueCondition.Value)) <= this.tolerance)
            {
                base.Fsm.Event(this.equal);
            }
            else if (this.getValue.Value < this.valueCondition.Value)
            {
                base.Fsm.Event(this.lessThan);
            }
            else if (this.getValue.Value > this.valueCondition.Value)
            {
                base.Fsm.Event(this.greaterThan);
            }
        }

        public override void OnEnter()
        {
            if (this.NguiUISliderObject != null)
            {
                this.theNguiUISlider = base.Fsm.GetOwnerDefaultTarget(this.NguiUISliderObject).GetComponent<UISlider>();
                if (this.theNguiUISlider != null)
                {
                    this.doGetvalue();
                }
                else
                {
                    base.Finish();
                }
                if (!this.everyFrame)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.doGetvalue();
        }

        public override void Reset()
        {
            this.NguiUISliderObject = null;
            this.getValue = null;
            this.everyFrame = false;
        }
    }
}

