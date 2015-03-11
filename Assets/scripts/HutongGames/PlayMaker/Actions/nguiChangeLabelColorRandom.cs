namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Pick random Color and assign in Ngui Label. Userful for display hints/tips in Game")]
    public class nguiChangeLabelColorRandom : FsmStateAction
    {
        [CompoundArray("Colors", "Color", "Weight"), Tooltip("Input your input Colors")]
        public FsmColor[] colors;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, Tooltip("NGUI Label to change Color")]
        public FsmOwnerDefault NguiLabelObject;
        private UILabel theNguiLabel;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat[] weights;

        private void doChangeColorRandom()
        {
            if ((this.colors != null) && (this.colors.Length != 0))
            {
                int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
                if (randomWeightedIndex != -1)
                {
                    this.theNguiLabel.color = this.colors[randomWeightedIndex].Value;
                }
            }
        }

        public override void OnEnter()
        {
            if (this.NguiLabelObject == null)
            {
                base.Finish();
            }
            this.theNguiLabel = base.Fsm.GetOwnerDefaultTarget(this.NguiLabelObject).GetComponent<UILabel>();
            this.doChangeColorRandom();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.doChangeColorRandom();
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.colors = new FsmColor[3];
            this.weights = new FsmFloat[] { 1f, 1f, 1f };
            this.everyFrame = false;
        }
    }
}

