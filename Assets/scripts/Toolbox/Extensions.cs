namespace Toolbox
{
    using HutongGames.PlayMaker.Actions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Extensions
    {
        public static void ActiveSelfObject(this Component com, bool active)
        {
            if (com != null)
            {
                com.gameObject.SetActive(active);
            }
        }

        public static void ColorState(this UITexture texture, ColorStates state)
        {
            texture.color = Color.white;
            switch (state)
            {
                case ColorStates.Normal:
                case ColorStates.TextureGrey:
                    nguiTextureGrey.doChangeEnableGrey(texture, state == ColorStates.TextureGrey);
                    break;

                case ColorStates.Black:
                    nguiTextureGrey.doChangeEnableGrey(texture, false);
                    texture.color = Color.gray;
                    break;
            }
        }

        public static void DelayCallBack(this MonoBehaviour behaviour, float delay, System.Action callBack)
        {
            if (behaviour.gameObject.activeSelf && behaviour.enabled)
            {
                behaviour.StartCoroutine(RunWorker(delay, callBack));
            }
        }

        public static void Disable(this UIButton bt, bool disable)
        {
            bt.state = disable ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal;
            Collider component = bt.GetComponent<Collider>();
            if (component != null)
            {
                component.enabled = !disable;
            }
        }

        public static T FindChild<T>(this Transform trans, string name) where T: Component
        {
            return FindInAllChild<T>(trans, name);
        }

        private static T FindInAllChild<T>(Transform trans, string name) where T: Component
        {
            Transform transform = trans.FindChild(name);
            if (transform != null)
            {
                return transform.GetComponent<T>();
            }
            for (int i = 0; i < trans.childCount; i++)
            {
                T local = FindInAllChild<T>(trans.GetChild(i), name);
                if (local != null)
                {
                    return local;
                }
            }
            return null;
        }

        public static void LoadTexture(this UITexture texture, string url)
        {
            texture.LoadTexture(url, null);
        }

        public static void LoadTexture(this UITexture texture, string url, Action<bool> onCompleted)
        {
            WebTextureLoader component = texture.GetComponent<WebTextureLoader>();
            if (component == null)
            {
                component = texture.gameObject.AddComponent<WebTextureLoader>();
            }
            component.OnCompleted = onCompleted;
            component.Url = url;
        }

        public static void OnLongPress(this Component ui, Action<object> callBack)
        {
            ui.OnLongPress(callBack, null);
        }

        public static void OnLongPress(this Component ui, Action<object> callBack, object userState)
        {
            UILongPress component = ui.GetComponent<UILongPress>();
            if (component == null)
            {
                component = ui.gameObject.AddComponent<UILongPress>();
            }
            component.callBack = callBack;
            component.userState = userState;
        }

        public static UIMouseClick OnUIMouseClick(this Component ui, Action<object> callBack)
        {
            UIMouseClick component = ui.GetComponent<UIMouseClick>();
            if (component == null)
            {
                component = ui.gameObject.AddComponent<UIMouseClick>();
            }
            component.click = callBack;
            return component;
        }

        public static UIMouseClick OnUIMouseClick(this Component ui, Action<object> callBack, object userState)
        {
            UIMouseClick component = ui.GetComponent<UIMouseClick>();
            if (component == null)
            {
                component = ui.gameObject.AddComponent<UIMouseClick>();
            }
            component.click = callBack;
            component.userState = userState;
            return component;
        }

        public static void OnUIMousePress(this Component ui, Action<bool, object> callBack)
        {
            ui.OnUIMousePress(callBack, null);
        }

        public static void OnUIMousePress(this Component ui, Action<bool, object> callBack, object userState)
        {
            UIMousePress component = ui.GetComponent<UIMousePress>();
            if (component == null)
            {
                component = ui.gameObject.AddComponent<UIMousePress>();
            }
            component.Press = callBack;
            component.userState = userState;
        }

        public static void OnUIMousePressOver(this Component ui, Action<object> callBack)
        {
            ui.OnUIMousePressOver(callBack, null);
        }

        public static void OnUIMousePressOver(this Component ui, Action<object> callBack, object userState)
        {
            UIMousePressOver component = ui.GetComponent<UIMousePressOver>();
            if (component == null)
            {
                component = ui.gameObject.AddComponent<UIMousePressOver>();
            }
            component.Press = callBack;
            component.userState = userState;
        }

        public static void ResetClip(this UIPanel panel)
        {
            Vector3 zero = Vector3.zero;
            panel.transform.localPosition = zero;
            panel.clipOffset = zero;
        }

        [DebuggerHidden]
        private static IEnumerator RunWorker(float delay, System.Action callBack)
        {
            return new <RunWorker>c__IteratorB8 { delay = delay, callBack = callBack, <$>delay = delay, <$>callBack = callBack };
        }

        public static int[] SplitToInt(this string sources, char key)
        {
            char[] separator = new char[] { key };
            string[] strArray = sources.Split(separator);
            List<int> list = new List<int>();
            foreach (string str in strArray)
            {
                int result = 0;
                if (int.TryParse(str, out result))
                {
                    list.Add(result);
                }
            }
            return list.ToArray();
        }

        public static void Text(this UIButton bt, string text)
        {
            UILabel label = bt.transform.FindChild<UILabel>("Label");
            if (label != null)
            {
                label.text = text;
            }
        }

        public static Vector3 ZeroY(this Vector3 v)
        {
            return new Vector3(v.x, 0f, v.z);
        }

        [CompilerGenerated]
        private sealed class <RunWorker>c__IteratorB8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal System.Action <$>callBack;
            internal float <$>delay;
            internal System.Action callBack;
            internal float delay;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(this.delay);
                        this.$PC = 1;
                        return true;

                    case 1:
                        if (this.callBack != null)
                        {
                            this.callBack();
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

