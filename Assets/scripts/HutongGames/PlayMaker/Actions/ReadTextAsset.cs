namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory(ActionCategory.String), Tooltip("Get the content of a text asset")]
    public class ReadTextAsset : FsmStateAction
    {
        [Tooltip("The content of the text asset")]
        public FsmString content;
        [RequiredField]
        public TextAsset textAsset;

        public override void OnEnter()
        {
            if (this.textAsset != null)
            {
                this.content.Value = this.textAsset.text;
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.textAsset = null;
            this.content = string.Empty;
        }
    }
}

