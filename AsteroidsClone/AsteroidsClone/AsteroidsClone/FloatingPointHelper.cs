using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsClone
{
    static class FloatingPointHelper
    {
        public static bool AlmostEqual(float f1, float f2, float epsilon)
        {
            float abs1 = Math.Abs(f1);
            float abs2 = Math.Abs(f2);
            float diff = Math.Abs(f1 - f2);

            if (f1 == f2) return true;

            else if (f1 * f2 == 0)
            {
                return diff < (epsilon * epsilon);
            }

            else
            {
                return diff / (abs1 + abs2) < epsilon;
            }
        }
    }
}
