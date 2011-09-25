using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Csg
{
    public abstract class Light
    {
        abstract public int[] CalculateLight(float[] spherePosition, float[] sphereNormal, int[] materialColor);

        public static float M { get; set; }

        public int[] LightColor { get; set; }
    }
}
