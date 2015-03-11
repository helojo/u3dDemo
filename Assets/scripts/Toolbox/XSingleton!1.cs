namespace Toolbox
{
    using System;

    public class XSingleton<T> where T: new()
    {
        public static void Reset()
        {
            SingleNested<T>.Reset();
        }

        public static T Singleton
        {
            get
            {
                return SingleNested<T>._instance;
            }
        }

        private class SingleNested
        {
            public static T _instance;

            static SingleNested()
            {
                T local = default(T);
                XSingleton<T>.SingleNested._instance = (local == null) ? Activator.CreateInstance<T>() : default(T);
            }

            public static void Reset()
            {
                T local = default(T);
                XSingleton<T>.SingleNested._instance = (local == null) ? Activator.CreateInstance<T>() : default(T);
            }
        }
    }
}

