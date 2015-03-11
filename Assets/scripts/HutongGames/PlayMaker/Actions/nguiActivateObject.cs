namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [Tooltip("Sets NGUI object to be activated or deactivated"), ActionCategory("Ngui Actions")]
    public class nguiActivateObject : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [Tooltip("NGUI object to be activated or deactivated"), RequiredField]
        public FsmOwnerDefault NguiObject;
        [Tooltip("Activate nGui GameObject. If False the game object will be Deactivated"), RequiredField]
        public FsmBool setActive;

        private void DoSetActive()
        {
            if (this.NguiObject != null)
            {
                NGUITools.SetActive(base.Fsm.GetOwnerDefaultTarget(this.NguiObject), this.setActive.Value);
            }
        }

        public override void OnEnter()
        {
            this.DoSetActive();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetActive();
        }

        public override void Reset()
        {
            this.NguiObject = null;
            this.setActive = 0;
            this.everyFrame = false;
        }
    }
}

