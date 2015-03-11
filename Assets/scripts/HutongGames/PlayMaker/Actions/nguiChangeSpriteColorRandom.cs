namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Pick random Color and assign in Ngui Sprite. Make it up"), ActionCategory("Ngui Actions")]
    public class nguiChangeSpriteColorRandom : FsmStateAction
    {
        [Tooltip("Input your input Colors"), CompoundArray("Colors", "Color", "Weight")]
        public FsmColor[] colors;
        private float currentTime;
        [Tooltip("Change Color after an interval Time")]
        public FsmFloat intervalTime;
        [Tooltip("NGUI Label to change Color"), RequiredField]
        public FsmOwnerDefault NguiSpriteObject;
        [Tooltip("When true, runs only once")]
        public bool onceTime;
        private UISprite theNguiSprite;
        [HasFloatSlider(0f, 1f)]
        public FsmFloat[] weights;

        private void doChangeColorRandom()
        {
            if ((this.colors != null) && (this.colors.Length != 0))
            {
                int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
                if (randomWeightedIndex != -1)
                {
                    this.theNguiSprite.color = this.colors[randomWeightedIndex].Value;
                }
            }
        }

        public override void OnEnter()
        {
            if (this.NguiSpriteObject == null)
            {
                base.Finish();
            }
            this.theNguiSprite = base.Fsm.GetOwnerDefaultTarget(this.NguiSpriteObject).GetComponent<UISprite>();
            if (this.theNguiSprite == null)
            {
                Debug.LogError("The Owner is not UISprite");
                base.Finish();
            }
            this.doChangeColorRandom();
            if (this.onceTime)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.currentTime += Time.deltaTime;
            float t = this.currentTime / this.intervalTime.Value;
            Mathf.Lerp(0f, 1f, t);
            if (t > 1f)
            {
                this.doChangeColorRandom();
                t = 0f;
                this.currentTime = 0f;
            }
        }

        public override void Reset()
        {
            this.NguiSpriteObject = null;
            this.colors = new FsmColor[3];
            this.weights = new FsmFloat[] { 1f, 1f, 1f };
            this.intervalTime = 0.5f;
            this.onceTime = false;
        }
    }
}

