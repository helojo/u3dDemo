namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using UnityEngine;

    [ActionCategory("ArrayMaker/ArrayList"), Tooltip("Split a text asset or string into an arrayList")]
    public class SplitTextToArrayList : ArrayListActions
    {
        [CheckForComponent(typeof(PlayMakerArrayListProxy)), ActionSection("Set up"), RequiredField, Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        public FsmOwnerDefault gameObject;
        [Tooltip("Split is ignored if this value is not empty. Each chars taken in account for split")]
        public FsmString OrThisChar;
        [Tooltip("Text Asset is ignored if this is set.")]
        public FsmString OrThisString;
        [ActionSection("Value"), Tooltip("Parse the line as a specific type")]
        public ArrayMakerParseStringAs parseAsType;
        [Tooltip("the range of parsing")]
        public FsmInt parseRange;
        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;
        [Tooltip("Split"), ActionSection("Split")]
        public SplitSpecialChars split;
        [Tooltip("From where to start parsing, leave to 0 to start from the beginning")]
        public FsmInt startIndex;
        [Tooltip("Text asset source"), ActionSection("Source")]
        public TextAsset textAsset;

        public override void OnEnter()
        {
            if (base.SetUpArrayListProxyPointer(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.reference.Value))
            {
                this.splitText();
            }
            base.Finish();
        }

        public override void Reset()
        {
            this.gameObject = null;
            this.reference = null;
            this.startIndex = null;
            this.parseRange = null;
            this.textAsset = null;
            this.split = SplitSpecialChars.NewLine;
            this.parseAsType = ArrayMakerParseStringAs.String;
        }

        public void splitText()
        {
            if (base.isProxyValid())
            {
                string text;
                string[] strArray;
                if (this.OrThisString.Value.Length == 0)
                {
                    if (this.textAsset == null)
                    {
                        return;
                    }
                    text = this.textAsset.text;
                }
                else
                {
                    text = this.OrThisString.Value;
                }
                base.proxy.arrayList.Clear();
                if (this.OrThisChar.Value.Length != 0)
                {
                    strArray = text.Split(this.OrThisChar.Value.ToCharArray());
                }
                else
                {
                    char ch = '\n';
                    switch (this.split)
                    {
                        case SplitSpecialChars.Tab:
                            ch = '\t';
                            break;

                        case SplitSpecialChars.Space:
                            ch = ' ';
                            break;
                    }
                    char[] separator = new char[] { ch };
                    strArray = text.Split(separator);
                }
                int num = this.startIndex.Value;
                int length = strArray.Length;
                if (this.parseRange.Value > 0)
                {
                    length = Mathf.Min(length - num, this.parseRange.Value);
                }
                string[] c = new string[length];
                int index = 0;
                for (int i = num; i < (num + length); i++)
                {
                    c[index] = strArray[i];
                    index++;
                }
                if (this.parseAsType == ArrayMakerParseStringAs.String)
                {
                    base.proxy.arrayList.InsertRange(0, c);
                }
                else if (this.parseAsType == ArrayMakerParseStringAs.Int)
                {
                    int[] numArray = new int[c.Length];
                    int num5 = 0;
                    foreach (string str2 in c)
                    {
                        int.TryParse(str2, out numArray[num5]);
                        num5++;
                    }
                    base.proxy.arrayList.InsertRange(0, numArray);
                }
                else if (this.parseAsType == ArrayMakerParseStringAs.Float)
                {
                    float[] numArray2 = new float[c.Length];
                    int num7 = 0;
                    foreach (string str3 in c)
                    {
                        float.TryParse(str3, out numArray2[num7]);
                        num7++;
                    }
                    base.proxy.arrayList.InsertRange(0, numArray2);
                }
            }
        }

        public enum ArrayMakerParseStringAs
        {
            String,
            Int,
            Float
        }

        public enum SplitSpecialChars
        {
            NewLine,
            Tab,
            Space
        }
    }
}

