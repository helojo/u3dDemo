namespace HutongGames.PlayMaker.Actions
{
    using FastBuf;
    using HutongGames.PlayMaker;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Tooltip("show mining action"), ActionCategory("MTD-GUI")]
    public class GUIShowMiningAction : FsmStateAction
    {
        [CompilerGenerated]
        private static Action<GUIEntity> <>f__am$cache0;
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        [CompilerGenerated]
        private static System.Action <>f__am$cache2;

        public override void OnEnter()
        {
            base.OnEnter();
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate (GUIEntity ui) {
                    LifeSkillPanel panel = ui as LifeSkillPanel;
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = () => GameStateMgr.Instance.ChangeState("EXIT_MINING_EVENT");
                    }
                    panel.OnClose = <>f__am$cache1;
                    panel.ShowMapData(NewLifeSkillType.NEW_LIFE_SKILL_MINING);
                    MiningAcitivity acitivity = UnityEngine.Object.FindObjectOfType<MiningAcitivity>();
                    if (acitivity != null)
                    {
                        if (<>f__am$cache2 == null)
                        {
                            <>f__am$cache2 = delegate {
                            };
                        }
                        acitivity.OnTapChest = <>f__am$cache2;
                        acitivity.SetActived(true);
                        acitivity.Clear();
                    }
                };
            }
            GUIMgr.Instance.PushGUIEntity<LifeSkillPanel>(<>f__am$cache0);
        }
    }
}

