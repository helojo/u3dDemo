﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Gets the value of any public property or field on the targeted Unity Object and stores it in a variable. E.g., Drag and drop any component attached to a Game Object to access its properties."), ActionCategory(ActionCategory.UnityObject)]
    public class GetProperty : FsmStateAction
    {
        public bool everyFrame;
        public FsmProperty targetProperty;

        public override void OnEnter()
        {
            this.targetProperty.GetValue();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.targetProperty.GetValue();
        }

        public override void Reset()
        {
            this.targetProperty = new FsmProperty();
            this.everyFrame = false;
        }
    }
}

