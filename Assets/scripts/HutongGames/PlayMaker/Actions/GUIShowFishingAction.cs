namespace HutongGames.PlayMaker.Actions
{
    using FastBuf;
    using HutongGames.PlayMaker;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ActionCategory("MTD-GUI"), Tooltip("Show fishing")]
    public class GUIShowFishingAction : FsmStateAction
    {
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache0;
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        [CompilerGenerated]
        private static Action<int, bool> <>f__am$cache2;

        public override void OnEnter()
        {
            base.OnEnter();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (GUIEntity ui) {
                    LifeSkillPanel panel = ui as LifeSkillPanel;
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = () => GameStateMgr.Instance.ChangeState("EXIT_FISH_EVENT");
                    }
                    panel.OnClose = <>f__am$cache1;
                    panel.ShowMapData(NewLifeSkillType.NEW_LIFE_SKILL_FISHING);
                    FishingActivity activity = UnityEngine.Object.FindObjectOfType<FishingActivity>();
                    if (activity != null)
                    {
                        activity.Actived(true);
                        if (<>f__am$cache2 == null)
                        {
                            <>f__am$cache2 = delegate (int index, bool flag) {
                            };
                        }
                        activity.OnRelease = <>f__am$cache2;
                    }
                };
            }
            GUIMgr.Instance.PushGUIEntity<LifeSkillPanel>(<>f__am$cache0);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Reset()
        {
            base.Reset();
        }
    }
}

