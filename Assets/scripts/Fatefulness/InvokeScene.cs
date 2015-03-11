namespace Fatefulness
{
    using System;

    public class InvokeScene
    {
        public Transition from;
        public ObjectMetaData global = new ObjectMetaData();
        public ObjectMetaData register = new ObjectMetaData();
        public ObjectMetaData stack = new ObjectMetaData();
        public Transition to;
    }
}

