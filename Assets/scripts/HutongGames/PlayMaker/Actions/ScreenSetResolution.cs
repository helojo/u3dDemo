namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.Application), Tooltip("Set the resolution")]
    public class ScreenSetResolution : FsmStateAction
    {
        [Tooltip("Full Screen mode")]
        public FsmBool fullScreen;
        [Tooltip("The resolution height")]
        public FsmInt height;
        [UIHint(UIHint.Variable), Tooltip("The current resolution ( width, height, refreshRate )")]
        public FsmVector3 orResolution;
        [Tooltip("The current resolution refresh rate"), UIHint(UIHint.Variable)]
        public FsmInt preferedRefreshRate;
        [Tooltip("The resolution width")]
        public FsmInt width;

        public override void OnEnter()
        {
            if (!this.orResolution.IsNone)
            {
                if (this.preferedRefreshRate.IsNone)
                {
                    Screen.SetResolution((int) this.orResolution.Value.x, (int) this.orResolution.Value.y, this.fullScreen.Value);
                }
                else
                {
                    Screen.SetResolution((int) this.orResolution.Value.x, (int) this.orResolution.Value.y, this.fullScreen.Value, (int) this.orResolution.Value.z);
                }
            }
            else if (this.preferedRefreshRate.IsNone)
            {
                Screen.SetResolution(this.width.Value, this.height.Value, this.fullScreen.Value);
            }
            else
            {
                Screen.SetResolution(this.width.Value, this.height.Value, this.fullScreen.Value, this.preferedRefreshRate.Value);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.width = null;
            this.height = null;
            this.preferedRefreshRate = new FsmInt();
            this.preferedRefreshRate.UseVariable = true;
            this.orResolution = null;
            this.fullScreen = null;
        }
    }
}

