namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Change Text in Ngui Label randomly after interval time"), ActionCategory("Ngui Actions")]
    public class nguiChangeLabelTextRandom : FsmStateAction
    {
        private float currentTime;
        [Tooltip("Change text after an interval Time")]
        public FsmFloat intervalTime;
        [Tooltip("NGUI Label to change Text"), RequiredField]
        public FsmOwnerDefault NguiLabelObject;
        [Tooltip("When true, runs only once")]
        public bool onceTime;
        [CompoundArray("Strings", "String", "Weight")]
        public FsmString[] strings;
        private UILabel theNguiLabel;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat[] weights;

        private void doChangeText()
        {
            if ((this.strings != null) && (this.strings.Length != 0))
            {
                int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
                if (randomWeightedIndex != -1)
                {
                    this.theNguiLabel.text = this.strings[randomWeightedIndex].Value;
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
            if (this.theNguiLabel != null)
            {
                this.doChangeText();
                if (this.onceTime)
                {
                    base.Finish();
                }
            }
        }

        public override void OnUpdate()
        {
            this.currentTime += Time.deltaTime;
            float t = this.currentTime / this.intervalTime.Value;
            Mathf.Lerp(0f, 1f, t);
            if (t > 1f)
            {
                this.doChangeText();
                t = 0f;
                this.currentTime = 0f;
            }
        }

        public override void Reset()
        {
            this.NguiLabelObject = null;
            this.strings = new FsmString[3];
            this.weights = new FsmFloat[] { 1f, 1f, 1f };
            this.onceTime = true;
            this.intervalTime = 1f;
        }
    }
}

