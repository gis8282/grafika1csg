using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg
{
    public static class FloatArrayExtensions
    {
        public static float[] Normalize(this float[] floatVector3)
        {
            float length = (float)Math.Sqrt(floatVector3[0] * floatVector3[0] + floatVector3[1] * floatVector3[1] + floatVector3[2] * floatVector3[2]);
            return new float[] { floatVector3[0] / length, floatVector3[1] / length, floatVector3[2] / length };
        }
    }
}
