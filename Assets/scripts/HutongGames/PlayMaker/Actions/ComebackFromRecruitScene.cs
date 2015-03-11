namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;

    [ActionCategory("MTD-GUI"), Tooltip("come back from recruit scene")]
    public class ComebackFromRecruitScene : FsmStateAction
    {
        public FsmEvent onCompleted;

        public override void OnEnter()
        {
            switch (RecruitPanel.actived_function)
            {
                case RecruitPanel.function.stone:
                case RecruitPanel.function.gold:
                    GUIMgr.Instance.PushGUIEntity("RecruitPanel", delegate (GUIEntity entity) {
                        base.Fsm.Event(this.onCompleted);
                        base.Finish();
                    });
                    break;

                default:
                    GUIMgr.Instance.DoModelGUI<SoulBox>(delegate (GUIEntity entity) {
                        entity.Depth = 300;
                        base.Fsm.Event(this.onCompleted);
                        base.Finish();
                    }, null);
                    break;
            }
            if (this.onCompleted == null)
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.onCompleted = null;
        }
    }
}

