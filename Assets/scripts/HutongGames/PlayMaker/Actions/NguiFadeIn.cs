namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [Tooltip("Fades in a group of NGUI widgets. Fades in all children. Sets the widgets to Active before Fade in"), Obsolete("NGUI Fade In and NGUI Fade Out are obsolete for NGUI v3+. NGUI v3 made breaking changes to how tween fading works, which broke the functionality of these two actions"), ActionCategory("NGUI")]
    public class NguiFadeIn : FsmStateAction
    {
        [Tooltip("Duration of the fade in, in seconds"), RequiredField]
        public FsmFloat Duration;
        private float FadeInTimer;
        [Tooltip("NGUI Parent widget. All children of this widget will be faded in"), RequiredField]
        public FsmOwnerDefault NguiParent;

        private void AttachFadeIn(GameObject parent)
        {
            NGUITools.SetActive(parent, true);
            int childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject gameObject = parent.transform.GetChild(i).gameObject;
                if (!gameObject.active)
                {
                    NGUITools.SetActive(gameObject, true);
                }
                TweenFade component = gameObject.GetComponent<TweenFade>();
                if (component != null)
                {
                    component.Reset();
                }
                else
                {
                    component = gameObject.AddComponent<TweenFade>();
                }
                component.AttachFadeIn(this.Duration.Value);
            }
        }

        private void DoFadeIn()
        {
            Debug.LogError("NGUI Fade In and NGUI Fade Out are obsolete for NGUI v3+. NGUI v3 made breaking changes to how tween fading works, which broke the functionality of these two actions");
        }

        public override void OnEnter()
        {
            this.DoFadeIn();
        }

        public override void OnUpdate()
        {
            this.FadeInTimer += Time.deltaTime;
            if (this.FadeInTimer > (this.Duration.Value + 0.5f))
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.NguiParent = null;
            this.Duration = null;
            this.FadeInTimer = 0f;
        }
    }
}

