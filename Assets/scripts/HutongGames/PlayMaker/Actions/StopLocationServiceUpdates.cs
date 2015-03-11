﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Device), Tooltip("Stops location service updates. This could be useful for saving battery life.")]
    public class StopLocationServiceUpdates : FsmStateAction
    {
        public override void OnEnter()
        {
            Input.location.Stop();
            base.Finish();
        }

        public override void Reset()
        {
        }
    }
}

