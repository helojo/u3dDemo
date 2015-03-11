namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ActionCategory("NGUI"), Tooltip("Fades out a group of NGUI widgets. Fades out all children. Sets the widgets to Inactive after fadeout"), Obsolete("NGUI Fade In and NGUI Fade Out are obsolete for NGUI v3+. NGUI v3 made breaking changes to how tween fading works, which broke the functionality of these two actions")]
    public class NguiFadeOut : FsmStateAction
    {
        [Tooltip("Duration of the fade out, in seconds"), RequiredField]
        public FsmFloat Duration;
        private float FadeOutTimer;
        [Tooltip("NGUI Parent widget. All children of this widget will be faded out"), RequiredField]
        public FsmOwnerDefault NguiParent;

        private void AttachFadeOut(GameObject parent, bool deactivate = true)
        {
            int childCount = parent.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject gameObject = parent.transform.GetChild(i).gameObject;
                if (gameObject.active)
                {
                    TweenFade component = gameObject.GetComponent<TweenFade>();
                    if (component != null)
                    {
                        component.Reset();
                    }
                    else
                    {
                        component = gameObject.AddComponent<TweenFade>();
                    }
                    component.AttachFadeout(this.Duration.Value, deactivate);
                }
            }
        }

        private void DoFadeOut()
        {
            Debug.LogError("NGUI Fade In and NGUI Fade Out are obsolete for NGUI v3+. NGUI v3 made breaking changes to how tween fading works, which broke the functionality of these two actions");
        }

        public override void OnEnter()
        {
            this.DoFadeOut();
        }

        public override void OnUpdate()
        {
            this.FadeOutTimer += Time.deltaTime;
            if (this.FadeOutTimer > (this.Duration.Value + 0.5f))
            {
                base.Finish();
            }
        }

        public override void Reset()
        {
            this.NguiParent = null;
            this.Duration = null;
            this.FadeOutTimer = 0f;
        }
    }
}

