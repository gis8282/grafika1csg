using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg
{
    public class Interval : IComparable<Interval>
    {
        private float _a;
        private float _b;
        private int[] _ca = new int[3];
        private int[] _cb = new int[3];
        private float[] _na = new float[3];
        private float[] _nb = new float[3];

        public float A { get { return _a; } }

        //not needed, should be internal visible to Csg.Test
        public float B { get { return _b; } }
        public int[] ColourA { get { return _ca; } set { _ca = value; } }
        public int[] ColourB { get { return _cb; } set { _cb = value; } }
        public float[] NA { get { return _na; } set { _na = value; } }
        public float[] NB { get { return _nb; } set { _nb = value; } }

        protected Interval()
        {
        }

        public Interval(float a, float b)
        {
            this._a = a;
            this._b = b;
        }

        private Interval(Interval arg)
            : this(arg._a, arg._b)
        {
            _ca = arg._ca;
            _cb = arg._cb;
            _na = arg._na;
            _nb = arg._nb;
        }

        public static Interval operator +(Interval arg1, Interval arg2)
        {
            Interval newI = new Interval();

            if (arg1._a < arg2._a)
                CopyLL(newI, arg1);
            else
                CopyLL(newI, arg2);

            if (arg1._b > arg2._b)
                CopyRR(newI, arg1);
            else
                CopyRR(newI, arg2);

            return newI;
        }

        public static List<Interval> operator -(Interval arg1, Interval arg2)
        {
            List<Interval> listI = new List<Interval>();

            if (arg1._a < arg2._a)
            {
                listI.Add(new Interval(arg1._a, arg2._a));
                CopyLL(listI[listI.Count - 1], arg1);
                CopyRL(listI[listI.Count - 1], arg2);
                if (arg2._b < arg1._b)
                {
                    listI.Add(new Interval(arg2._b, arg1._b));
                    CopyLR(listI[listI.Count - 1], arg2);
                    CopyRR(listI[listI.Count - 1], arg1);
                }
            }
            else if (arg1._a < arg2._b && arg2._b < arg1._b)
            {
                listI.Add(new Interval(arg2._b, arg1._b));
                CopyLR(listI[listI.Count - 1], arg2);
                CopyRR(listI[listI.Count - 1], arg1);
            }

            return listI;
        }

        public static Interval operator *(Interval arg1, Interval arg2)
        {
            Interval newI = new Interval();

            if (arg1._a < arg2._a)
                CopyLL(newI, arg2);
            else
                CopyLL(newI, arg1);

            if (arg1._b < arg2._b)
                CopyRR(newI, arg1);
            else
                CopyRR(newI, arg2);

            return newI;
        }

        public static void Copy(Interval newI, Interval arg)
        {
            newI._a = arg._a;
            newI._b = arg._b;
            newI._ca = arg._ca;
            newI._cb = arg._cb;
            newI._na = arg._na;
            newI._nb = arg._nb;
        }

        private static void CopyLL(Interval newI, Interval arg)
        {
            newI._a = arg._a;
            newI._ca = arg._ca;
            newI._na = arg._na;
        }

        private static void CopyLR(Interval newI, Interval arg)
        {
            newI._a = arg._b;
            newI._ca = arg._cb;
            newI._na = arg._nb;
        }

        private static void CopyRL(Interval newI, Interval arg)
        {
            newI._b = arg._a;
            newI._cb = arg._ca;
            newI._nb = arg._na;
        }

        private static void CopyRR(Interval newI, Interval arg)
        {
            newI._b = arg._b;
            newI._cb = arg._cb;
            newI._nb = arg._nb;
        }

        public static List<Interval> Union(List<Interval> arg1, List<Interval> arg2)
        {
            if (arg1.Count == 0)
                return arg2;
            else if (arg2.Count == 0)
                return arg1;

            arg1.AddRange(arg2);
            arg1.Sort();

            List<Interval> newI = new List<Interval>();

            newI.Add(arg1[0]);

            foreach(var interval in arg1.Skip(1))
            {
                if (AreIntersecting(newI.Last(), interval) == true)
                {
                    newI[newI.Count - 1] = newI.Last() + interval;
                }
                else
                {
                    newI.Add(interval);
                }
            }

            return newI;
        }

        public static List<Interval> Difference(List<Interval> arg1, List<Interval> arg2)
        {
            List<Interval> newI = new List<Interval>();
            List<Interval> pomI = new List<Interval>();

            for (int i = 0; i < arg1.Count; ++i)
            {
                for (int j = 0; j < arg2.Count; ++j)
                {
                    if (AreIntersecting(arg1[i], arg2[j]))
                    {
                        pomI = arg1[i] - arg2[j];
                        if (pomI.Count == 1)
                        {
                            Copy(arg1[i], pomI[0]);
                        }
                        else if (pomI.Count > 1)
                        {
                            newI.Add(new Interval(pomI[0]));

                            Copy(arg1[i], pomI[1]);
                        }
                        else
                            goto FIRST_LOOP;

                    }
                }

                newI.Add(new Interval(arg1[i]));
            FIRST_LOOP: ;
            }
            return newI;
        }

        public static List<Interval> Intersection(List<Interval> arg1, List<Interval> arg2)
        {
            //List<Interval> newI = new List<Interval>();

            //for (int i = 0; i < arg1.Count; ++i)
            //{
            //    for (int j = 0; j < arg2.Count; ++j)
            //    {
            //        if (AreIntersecting(arg1[i], arg2[j]))
            //            newI.Add(arg1[i] * arg2[j]);
            //    }
            //}

            var x = arg1.SelectMany(i => arg2.Where(j => AreIntersecting(i, j)).Select(j => i * j));
            return x.ToList();
            //var x = arg1.Where(i => arg2.Where(j => true).Select(j => i * j));

           // return newI;
        }

        private static bool AreIntersecting(Interval arg1, Interval arg2)
        {
            return arg1.Contains(arg2.A) || arg1.Contains(arg2.B) || arg2.Contains(arg1.A);
        }

        private bool Contains(float arg)
        {
            return _a <= arg && arg <= _b;
        }

        public int CompareTo(Interval obj)
        {
            if (_a == obj._a) return 0;
            return (_a - obj._a > 0) ? 1 : -1;
        }
    }
}
