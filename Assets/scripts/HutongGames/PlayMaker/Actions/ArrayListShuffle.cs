namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections;
    using UnityEngine;

    [Tooltip("Shuffle elements from an ArrayList Proxy component"), ActionCategory("ArrayMaker/ArrayList")]
    public class ArrayListShuffle : ArrayListActions
    {
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component to shuffle"), CheckForComponent(typeof(PlayMakerArrayListProxy)), RequiredField, ActionSection("Set up")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component to copy from ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Optional range for the shuffling, starting at the start index if greater than 0. Leave it to 0 for no effect, that is will shuffle the whole array")]
        public FsmInt shufflingRange;
        [Tooltip("Optional start Index for the shuffling. Leave it to 0 for no effect")]
        public FsmInt startIndex;

        public void DoArrayListShuffle(ArrayList source)
        {
            if (base.isProxyValid())
            {
                int message = 0;
                int b = base.proxy.arrayList.Count - 1;
                if (this.startIndex.Value > 0)
                {
                    message = Mathf.Min(this.startIndex.Value, b);
                }
                if (this.shufflingRange.Value > 0)
                {
                    b = Mathf.Min((int) (base.proxy.arrayList.Count - 1), (int) (message + this.shufflingRange.Value));
                }
                Debug.Log(message);
                Debug.Log(b);
                for (int i = b; i > message; i--)
                {
                    int num4 = UnityEngine.Random.Range(message, i + 1);
                    object obj2 = base.proxy.arrayList[i];
                    base.proxy.arrayList[i] = base.proxy.arrayList[num4];
                    base.proxy.arrayList[num4] = obj2;
                }
            }
        }

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.DoArrayListShuffle(base.proxy.arrayList);
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.startIndex = 0;
            this.shufflingRange = 0;
        }
    }
}

