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
            List<Interval> ret_val = new List<Interval>();
            Sphere s = ((TreeSphere)this).S;
            float[] c = new float[] { s.CurrentPosition[0], s.CurrentPosition[1], s.CurrentPosition[2] };

            if (Math.Pow(x - c[0], 2) + Math.Pow(y - c[1], 2) < s.Radius * s.Radius)
            {
                float z1 = (float)(c[2] - Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                float z2 = (float)(c[2] + Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                
                Interval interval = new Interval(z1, z2);
                interval.ColourA = s.Color;
                interval.ColourB = s.Color;
                interval.NA = new float[] { (x - c[0]) / s.Radius, (y - c[1]) / s.Radius, (z1 - c[2]) / s.Radius };
                interval.NB = new float[] { (x - c[0]) / -s.Radius, (y - c[1]) / -s.Radius, (z2 - c[2]) / -s.Radius };

                ret_val.Add(interval);
            }
            return ret_val;
        }

        public override bool FindRect(out float x0, out float y0, out float x1, out float y1)
        {
            x0 = S.CurrentPosition[0] - S.Radius;
            y0 = S.CurrentPosition[1] - S.Radius;

            x1 = S.CurrentPosition[0] + S.Radius;
            y1 = S.CurrentPosition[1] + S.Radius;
            return true;
        }
    }
}
