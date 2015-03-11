namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Set Value in Progress Bar or Slider in current Ngui UISlider")]
    public class nguiSetSliderValue : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI UISlider to Set value")]
        public FsmOwnerDefault NguiUISliderObject;
        [Tooltip("Set this value to Slider/Progress bar"), UIHint(UIHint.FsmFloat), HasFloatSlider(0f, 1f)]
        public FsmFloat setValue;
        private UISlider theNguiUISlider;

        private void doSetvalue()
        {
            this.theNguiUISlider.value = this.setValue.Value;
        }

        public override void OnEnter()
        {
            if (this.NguiUISliderObject != null)
            {
                this.theNguiUISlider = base.Fsm.GetOwnerDefaultTarget(this.NguiUISliderObject).GetComponent<UISlider>();
                if (this.theNguiUISlider != null)
                {
                    this.doSetvalue();
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
            this.doSetvalue();
        }

        public override void Reset()
        {
            this.NguiUISliderObject = null;
            this.setValue = null;
            this.everyFrame = false;
        }
    }
}

