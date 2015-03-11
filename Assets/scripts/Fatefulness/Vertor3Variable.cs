namespace Fatefulness
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Vertor3Variable : Concentrator
    {
        [CompilerGenerated]
        private static Dictionary<string, int> <>f__switch$mapC;
        private Vector3 variable = Vector3.zero;

        private void ComponentXModify(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.variable.x = intent.GetFloat(IntentDecl.variable);
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.variable.x);
                    break;
            }
        }

        private void ComponentYModify(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.variable.y = intent.GetFloat(IntentDecl.variable);
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.variable.y);
                    break;
            }
        }

        private void ComponentZModify(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.variable.z = intent.GetFloat(IntentDecl.variable);
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.variable.z);
                    break;
            }
        }

        public override void Concreate()
        {
            base.Concreate();
            base.RegisterPort<Transition>("variable", Transition.Direction.both);
            base.RegisterPort<Transition>("X", Transition.Direction.both);
            base.RegisterPort<Transition>("Y", Transition.Direction.both);
            base.RegisterPort<Transition>("Z", Transition.Direction.both);
        }

        public override void Excude(Intent intent)
        {
            string str = intent.GetString(IntentDecl.port);
            if (string.IsNullOrEmpty(str))
            {
                this.VariableModify(intent);
            }
            else
            {
                string key = str;
                if (key != null)
                {
                    int num;
                    if (<>f__switch$mapC == null)
                    {
                        Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
                        dictionary.Add("variable", 0);
                        dictionary.Add("X", 1);
                        dictionary.Add("Y", 2);
                        dictionary.Add("Z", 3);
                        <>f__switch$mapC = dictionary;
                    }
                    if (<>f__switch$mapC.TryGetValue(key, out num))
                    {
                        switch (num)
                        {
                            case 0:
                                this.VariableModify(intent);
                                break;

                            case 1:
                                this.ComponentXModify(intent);
                                break;

                            case 2:
                                this.ComponentYModify(intent);
                                break;

                            case 3:
                                this.ComponentZModify(intent);
                                break;
                        }
                    }
                }
            }
        }

        public override Transition Launcher()
        {
            return base.GetPort("variable");
        }

        public override string Name()
        {
            return "Vector3";
        }

        public override void Reset()
        {
            base.Reset();
            this.variable = Vector3.zero;
        }

        private void VariableModify(Intent intent)
        {
            switch (intent.Type)
            {
                case Intent.IntentType.forward:
                    this.variable = intent.GetVector3(IntentDecl.variable);
                    break;

                case Intent.IntentType.review:
                    base.ReturnAndStore(intent, "variable", IntentDecl.variable, this.variable);
                    break;
            }
        }
    }
}

