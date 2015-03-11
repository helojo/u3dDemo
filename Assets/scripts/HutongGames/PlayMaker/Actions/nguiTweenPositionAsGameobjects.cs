namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("TweenPosition Ngui Object from two points of gameobjects in scene"), ActionCategory("Ngui Actions")]
    public class nguiTweenPositionAsGameobjects : FsmStateAction
    {
        public FsmFloat delay;
        public FsmFloat duration;
        private FsmVector3 fromPosition;
        public FsmGameObject fromWhere;
        public FsmInt group;
        public FsmBool ignoreTimeScale;
        public UITweener.Method method;
        public FsmOwnerDefault NguiObject;
        public UITweener.Style playStyle;
        private TweenPosition theTween;
        private FsmVector3 toPosition;
        public FsmGameObject toWhere;

        public override void OnEnter()
        {
            if (this.NguiObject != null)
            {
                base.Fsm.GetOwnerDefaultTarget(this.NguiObject).AddMissingComponent<TweenPosition>();
                this.theTween = base.Fsm.GetOwnerDefaultTarget(this.NguiObject).GetComponent<TweenPosition>();
                if ((this.fromWhere != null) && (this.toWhere != null))
                {
                    this.fromPosition.Value = this.fromWhere.Value.transform.localPosition;
                    this.toPosition.Value = this.toWhere.Value.transform.localPosition;
                    this.theTween.from = this.fromPosition.Value;
                    this.theTween.to = this.toPosition.Value;
                    this.theTween.style = this.playStyle;
                    this.theTween.method = this.method;
                    this.theTween.duration = this.duration.Value;
                    this.theTween.ignoreTimeScale = this.ignoreTimeScale.Value;
                    this.theTween.ResetToBeginning();
                    this.theTween.PlayForward();
                    base.Finish();
                }
            }
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.fromPosition = Vector3.zero;
            this.toPosition = Vector3.one;
            this.fromWhere = null;
            this.toWhere = null;
            this.duration = 1f;
            this.delay = 0f;
            this.group = 0;
            this.ignoreTimeScale = 1;
        }
    }
}

