namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Get Value in Progress Bar or Slider in current Ngui UISlider"), ActionCategory("Ngui Actions")]
    public class nguiGetSliderValue : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [UIHint(UIHint.FsmFloat), Tooltip("Store current value"), HasFloatSlider(0f, 1f)]
        public FsmFloat getValue;
        [Tooltip("NGUI UISlider to Get value"), RequiredField]
        public FsmOwnerDefault NguiUISliderObject;
        private UISlider theNguiUISlider;
        [UIHint(UIHint.Variable)]
        public FsmString valueAsString;

        private void doGetvalue()
        {
            this.getValue.Value = this.theNguiUISlider.value;
            this.valueAsString.Value = this.theNguiUISlider.value.ToString("F2");
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

