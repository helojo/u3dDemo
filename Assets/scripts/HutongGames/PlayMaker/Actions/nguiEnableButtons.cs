namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("Ngui Actions"), Tooltip("Sets NGUI object to be activated or deactivated")]
    public class nguiEnableButtons : FsmStateAction
    {
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;
        [RequiredField, CompoundArray("How many", "Object", "setEnable")]
        public FsmGameObject[] nguiObject;
        [Tooltip("Activate nGui GameObject. If False the game object will be Deactivated")]
        public FsmBool[] setEnable;

        private void DoSetEnable()
        {
            if ((this.nguiObject != null) && (this.nguiObject.Length >= 1))
            {
                for (int i = 0; i < this.nguiObject.Length; i++)
                {
                    if (this.nguiObject[i].Value.GetComponent<UIButton>() != null)
                    {
                        this.nguiObject[i].Value.GetComponent<UIButton>().isEnabled = this.setEnable[i].Value;
                    }
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetEnable();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetEnable();
        }

        public override void Reset()
        {
            this.nguiObject = null;
            this.setEnable = null;
            this.everyFrame = false;
        }
    }
}

