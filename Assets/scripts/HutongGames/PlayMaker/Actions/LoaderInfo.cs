namespace HutongGames.PlayMaker.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LoaderInfo
    {
        public Queue<GameObject> ChildQ = new Queue<GameObject>();

        public GameObject LoaderGO { get; set; }
    }
}

