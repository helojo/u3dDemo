namespace LevelLoader
{
    using System;
    using UnityEngine;

    public class Operation
    {
        public AsyncOperation ao;
        public AssetBundle bundle;

        public bool IsDone()
        {
            return ((this.ao == null) || this.ao.isDone);
        }

        public void Release()
        {
            if (null != this.bundle)
            {
                this.bundle.Unload(false);
            }
            this.bundle = null;
        }
    }
}

