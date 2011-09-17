using System.Collections.Generic;
using System;
namespace Csg
{
    public class TreeSphere : TreeNode
    {
        private Sphere _s;
        
        public Sphere S
        {
            get { return _s; }
        }

        public TreeSphere(Sphere s)
        {
            _s = s;
        }

        public override System.Collections.Generic.List<Interval> TraverseTree(float x, float y)
        {
            Sphere s = ((TreeSphere)this).S;
            float[] c = new float[] { s.CurrentPosition[0], s.CurrentPosition[1], s.CurrentPosition[2] };

            if (Math.Pow(x - c[0], 2) + Math.Pow(y - c[1], 2) < s.Radius * s.Radius)
            {
                float z1 = (float)(c[2] - Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                float z2 = (float)(c[2] + Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                List<Interval> ret_val = new List<Interval>();
                Interval interval = new Interval(z1, z2);
                interval.ColourA = s.Color;
                interval.ColourB = s.Color;
                interval.NA = new float[] { (x - c[0]) / s.Radius, (y - c[1]) / s.Radius, (z1 - c[2]) / s.Radius };
                interval.NB = new float[] { (x - c[0]) / -s.Radius, (y - c[1]) / -s.Radius, (z2 - c[2]) / -s.Radius };

                ret_val.Add(interval);
                return ret_val;
            }
            return null;
        }
    }
}
