namespace HutongGames.PlayMaker.Actions
{
    using HutongGames.PlayMaker;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Tooltip("Sets a color variable based on a string value"), ActionCategory(ActionCategory.Color)]
    public class ColorFromString : FsmStateAction
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$map7;
        [RequiredField, UIHint(UIHint.Variable), Tooltip("The color variable to updated")]
        public FsmColor color;
        [RequiredField, Tooltip("String to read the value from")]
        public FsmString ColorString;
        [Tooltip("Default color to assign if no match")]
        public FsmColor DefaultColor;
        [Tooltip("When true, runs on every frame")]
        public bool everyFrame;

        private void DoSetColor()
        {
            if ((this.ColorString != null) && (this.color != null))
            {
                string key = this.ColorString.Value.ToLower();
                if (key != null)
                {
                    int num;
                    if (<>f__switch$map7 == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(11);
                        dictionary.Add("black", 0);
                        dictionary.Add("white", 1);
                        dictionary.Add("red", 2);
                        dictionary.Add("green", 3);
                        dictionary.Add("blue", 4);
                        dictionary.Add("yellow", 5);
                        dictionary.Add("cyan", 6);
                        dictionary.Add("magenta", 7);
                        dictionary.Add("clear", 8);
                        dictionary.Add("gray", 9);
                        dictionary.Add("grey", 10);
                        <>f__switch$map7 = dictionary;
                    }
                    if (<>f__switch$map7.TryGetValue(key, out num))
                    {
                        switch (num)
                        {
                            case 0:
                                this.color.Value = Color.black;
                                return;

                            case 1:
                                this.color.Value = Color.white;
                                return;

                            case 2:
                                this.color.Value = Color.red;
                                return;

                            case 3:
                                this.color.Value = Color.green;
                                return;

                            case 4:
                                this.color.Value = Color.blue;
                                return;

                            case 5:
                                this.color.Value = Color.yellow;
                                return;

                            case 6:
                                this.color.Value = Color.cyan;
                                return;

                            case 7:
                                this.color.Value = Color.magenta;
                                return;

                            case 8:
                                this.color.Value = Color.clear;
                                return;

                            case 9:
                                this.color.Value = Color.gray;
                                return;

                            case 10:
                                this.color.Value = Color.grey;
                                return;
                        }
                    }
                }
                if (this.DefaultColor != null)
                {
                    this.color.Value = this.DefaultColor.Value;
                }
            }
        }

        public override void OnEnter()
        {
            this.DoSetColor();
            if (!this.everyFrame)
            {
                base.Finish();
            }
        }

        public override void OnUpdate()
        {
            this.DoSetColor();
        }

        public override void Reset()
        {
            this.ColorString = null;
            this.color = null;
            this.DefaultColor = null;
            this.everyFrame = false;
        }
    }
}

