namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.StateMachine), Tooltip("Each time this action is called it iterate to the next value from Start to End. This lets you safely loop and process anything on each iteratation.")]
    public class Iterate : FsmStateAction
    {
        private bool _up = true;
        [ActionSection("Result"), Tooltip("The current value of the iteration process"), UIHint(UIHint.Variable)]
        public FsmInt currentIndex;
        [Tooltip("End value")]
        public FsmInt endIndex;
        [Tooltip("Event to send when we reached the end.")]
        public FsmEvent finishedEvent;
        [Tooltip("increment value at each iteration, absolute value only, it will itself find if it needs to substract or add")]
        public FsmInt increment;
        [Tooltip("Event to send to get the next child.")]
        public FsmEvent loopEvent;
        private bool started;
        [Tooltip("Start value"), RequiredField]
        public FsmInt startIndex;

        private void DoGetNext()
        {
            if (!this.started)
            {
                this._up = this.startIndex.Value < this.endIndex.Value;
                this.currentIndex.Value = this.startIndex.Value;
                this.started = true;
                if (this.loopEvent != null)
                {
                    base.Fsm.Event(this.loopEvent);
                }
            }
            else
            {
                if (this._up)
                {
                    if (this.currentIndex.Value >= this.endIndex.Value)
                    {
                        this.started = false;
                        base.Fsm.Event(this.finishedEvent);
                        return;
                    }
                    this.currentIndex.Value = Mathf.Max(this.startIndex.Value, this.currentIndex.Value + Mathf.Abs(this.increment.Value));
                }
                else
                {
                    if (this.currentIndex.Value <= this.endIndex.Value)
                    {
                        this.started = false;
                        base.Fsm.Event(this.finishedEvent);
                        return;
                    }
                    this.currentIndex.Value = Mathf.Max(this.endIndex.Value, this.currentIndex.Value - Mathf.Abs(this.increment.Value));
                }
                if (this.loopEvent != null)
                {
                    base.Fsm.Event(this.loopEvent);
                }
            }
        }

        public override void OnEnter()
        {
            this.DoGetNext();
            base.Finish();
        }

        public override void Reset()
        {
            this.startIndex = 0;
            this.endIndex = 10;
            this.currentIndex = null;
            this.loopEvent = null;
            this.finishedEvent = null;
            this.increment = 1;
        }
    }
}

