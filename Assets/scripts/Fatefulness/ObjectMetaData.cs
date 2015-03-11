namespace Fatefulness
{
    using System;
    using System.Collections.Generic;

    public class ObjectMetaData
    {
        public List<ObjectMetaData> children = new List<ObjectMetaData>();
        public string descript = "...";
        public bool expand;
        public string name = "unknown";
        public object reference;
    }
}

