using System;
using System.IO;
using System.Reflection;

namespace XWorld
{

    public class MathUtil
    {
        const float fPricision = 0.0001f;
        public static bool IsFloatZero(float fValue)
        {
            return Math.Abs(fValue) < fPricision ? true : false;
        }
    }
}

