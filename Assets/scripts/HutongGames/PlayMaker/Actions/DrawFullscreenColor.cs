﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Fills the screen with a Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene."), ActionCategory(ActionCategory.GUI)]
    public class DrawFullscreenColor : FsmStateAction
    {
        [RequiredField, Tooltip("Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
        public FsmColor color;

        public override void OnGUI()
        {
            Color color = GUI.color;
            GUI.color = this.color.Value;
            GUI.DrawTexture(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), ActionHelpers.WhiteTexture);
            GUI.color = color;
        }

        public override void Reset()
        {
            this.color = Color.white;
        }
    }
}

