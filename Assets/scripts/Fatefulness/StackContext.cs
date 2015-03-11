namespace Fatefulness
{
    using System;

    public class StackContext
    {
        public int from = -1;
        public ObjectMetaData global = new ObjectMetaData();
        public int id = -1;
        public ObjectMetaData register = new ObjectMetaData();
        public ObjectMetaData stack = new ObjectMetaData();
        public int to = -1;
    }
}

