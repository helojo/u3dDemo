namespace Toolbox
{
    using System;
    using System.Collections.Generic;

    public class GRandomer
    {
        private static Random _r = new Random();

        public static T RandomArray<T>(T[] arrary)
        {
            if ((arrary != null) && (arrary.Length > 0))
            {
                return arrary[RandomMinAndMax(0, arrary.Length)];
            }
            return default(T);
        }

        public static T RandomList<T>(List<T> list)
        {
            if ((list != null) && (list.Count != 0))
            {
                return list[RandomMinAndMax(0, list.Count)];
            }
            return default(T);
        }

        public static int RandomMinAndMax(int min, int max)
        {
            return _r.Next(min, max);
        }

        public static bool RandomPro10000(int pro)
        {
            return (RandomMinAndMax(0, 0x2710) < pro);
        }
    }
}

