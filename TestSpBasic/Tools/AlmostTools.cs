using System;
using SpaceTest.SpFrwk;

namespace TestSpBasic
{
    static class AlmostTools
    {
        public static bool AlmostEquals(this Double v, Double v2, Double approx)
        {
            return Math.Abs(v2 - v) <= Math.Abs(approx);
        }

        public static bool AlmostEquals(this SpVector v, SpVector v2, SpVector approx)
        {
            return (v2 - v).Length2 <= approx.Length2;
        }

        public static Double AlmostRatio(this Double v, Double v2)
        {
            return Math.Abs((v2 - v) / v);
        }

        public static Double AlmostRatio(this SpVector v, SpVector v2)
        {
            return (v2 - v).Length2 / v.Length2;
        }
    }
}
