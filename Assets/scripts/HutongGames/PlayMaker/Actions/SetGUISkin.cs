﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.GUI), Tooltip("Sets the GUISkin used by GUI elements.")]
    public class SetGUISkin : FsmStateAction
    {
        public FsmBool applyGlobally;
        [RequiredField]
        public GUISkin skin;

        public override void OnGUI()
        {
            if (this.skin != null)
            {
                GUI.skin = this.skin;
            }
            if (this.applyGlobally.Value)
            {
                PlayMakerGUI.GUISkin = this.skin;
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.skin = null;
            this.applyGlobally = 1;
        }
    }
}

