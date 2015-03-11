namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Logic), Tooltip("Tests if the value of a GameObject variable changed. Use this to send an event on change, or store a bool that can be used in other operations.")]
    public class GameObjectChanged : FsmStateAction
    {
        [Tooltip("Event to send if the variable changes.")]
        public FsmEvent changedEvent;
        [Tooltip("The GameObject variable to watch for a change."), RequiredField, UIHint(UIHint.Variable)]
        public FsmGameObject gameObjectVariable;
        private GameObject previousValue;
        [Tooltip("Set to True if the variable changes."), UIHint(UIHint.Variable)]
        public FsmBool storeResult;

        public override void OnEnter()
        {
            if (this.gameObjectVariable.IsNone)
            {
                base.Finish();
            }
            else
            {
                this.previousValue = this.gameObjectVariable.Value;
            }
        }

        public override void OnUpdate()
        {
            this.storeResult.Value = false;
            if (this.gameObjectVariable.Value != this.previousValue)
            {
                this.storeResult.Value = true;
                base.Fsm.Event(this.changedEvent);
            }
        }

        public override void Reset()
        {
            this.gameObjectVariable = null;
            this.changedEvent = null;
            this.storeResult = null;
        }
    }
}

