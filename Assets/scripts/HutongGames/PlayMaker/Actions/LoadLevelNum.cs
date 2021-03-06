﻿namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Level), Tooltip("Loads a Level by Index number. Before you can load a level, you have to add it to the list of levels defined in File->Build Settings...")]
    public class LoadLevelNum : FsmStateAction
    {
        [Tooltip("Load the level additively, keeping the current scene.")]
        public bool additive;
        [Tooltip("Keep this GameObject in the new level. NOTE: The GameObject and components is disabled then enabled on load; uncheck Reset On Disable to keep the active state.")]
        public FsmBool dontDestroyOnLoad;
        [Tooltip("The level index in File->Build Settings"), RequiredField]
        public FsmInt levelIndex;
        [Tooltip("Event to send after the level is loaded.")]
        public FsmEvent loadedEvent;

        public override void OnEnter()
        {
            if (this.dontDestroyOnLoad.Value)
            {
                UnityEngine.Object.DontDestroyOnLoad(base.Owner.transform.root.gameObject);
            }
            if (this.additive)
            {
                Application.LoadLevelAdditive(this.levelIndex.Value);
            }
            else
            {
                Application.LoadLevel(this.levelIndex.Value);
            }
            base.Fsm.Event(this.loadedEvent);
            base.Finish();
        }

        public override void Reset()
        {
            this.levelIndex = null;
            this.additive = false;
            this.loadedEvent = null;
            this.dontDestroyOnLoad = 0;
        }
    }
}

