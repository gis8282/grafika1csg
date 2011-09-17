using System;
using System.Collections.Generic;
using System.Text;

namespace Csg
{
    class Interval : IComparable<Interval>
    {
        private float _a;
        private float _b;
        private int[] _ca = new int[3];
        private int[] _cb = new int[3];
        private float[] _na = new float[3];
        private float[] _nb = new float[3];

        public float A { get { return _a; } }
        public int[] ColourA { get { return _ca; } set { _ca = value; } }
        public int[] ColourB { get { return _cb; } set { _cb = value; } }
        public float[] NA { get { return _na; } set { _na = value; } }
        public float[] NB { get { return _nb; } set { _nb = value; } }

        public Interval() 
        { 
        }

        public Interval(float a, float b)
        {
            this._a = a;
            this._b = b;
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
            if (listI.Count == 0)
                listI = null;

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
                CopyRR(newI, arg2);
            else
                CopyRR(newI, arg1);

            return newI;
        }

        public static Interval Copy(Interval arg)
        {
            Interval newI = new Interval(arg._a, arg._b);

            newI._ca = arg._ca;
            newI._cb = arg._cb;
            newI._na = arg._na;
            newI._nb = arg._nb;

            return newI;
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
            List<Interval> newI = new List<Interval>();
            int len = 0;
            if (arg1 == null)
                return arg2;
            else if (arg2 == null)
                return arg1;
            else if (arg1 == null && arg2 == null)
                return null;
            arg1.AddRange(arg2);
            arg1.Sort();

            newI.Add(arg1[len]);
            for (int i = 1; i < arg1.Count; ++i)
            {
                if (Intersect(newI[len], arg1[i]) == true)
                {
                    newI.Add(newI[len] + arg1[i]);
                    newI.RemoveAt(len);
                }
                else
                {
                    newI.Add(arg1[i]);
                    ++len;
                }
            }
            return newI;            
        }

        public static List<Interval> Difference(List<Interval> arg1, List<Interval> arg2)
        {
            List<Interval> newI = new List<Interval>();
            List<Interval> pomI = new List<Interval>();

            if (arg1 == null)
                return null;
            else if (arg2 == null)
                return arg1;
            else if (arg1 == null && arg2 == null)
                return null;

            for (int i = 0; i < arg1.Count; ++i)
            {
                for (int j = 0; j < arg2.Count; ++j)
                {
                    if (Intersect(arg1[i], arg2[j]))
                    {
                        pomI = arg1[i] - arg2[j];
                        if (pomI != null)
                        {
                            if (pomI.Count == 1)
                            {
                                arg1[i]._a = pomI[0]._a;
                                arg1[i]._b = pomI[0]._b;
                                arg1[i]._ca = pomI[0]._ca;
                                arg1[i]._cb = pomI[0]._cb;
                                arg1[i]._na = pomI[0]._na;
                                arg1[i]._nb = pomI[0]._nb;
                            }
                            if (pomI.Count > 1)
                            {
                                newI.Add(Copy(pomI[0]));
                                arg1[i]._a = pomI[1]._a;
                                arg1[i]._b = pomI[1]._b;
                                arg1[i]._ca = pomI[1]._ca;
                                arg1[i]._cb = pomI[1]._cb;
                                arg1[i]._na = pomI[1]._na;
                                arg1[i]._nb = pomI[1]._nb;
                            }
                        }
                        else
                            goto FIRST_LOOP;
                        
                    }
                }

                newI.Add(Copy(arg1[i]));
            FIRST_LOOP: ;
            }
            return newI;
        }
      
        public static List<Interval> Intersection(List<Interval> arg1, List<Interval> arg2)
        {
            List<Interval> newI = new List<Interval>();
            if (arg1 == null)
                return null;
            else if (arg2 == null)
                return null;
            else if (arg1 == null && arg2 == null)
                return null;

            for (int i = 0; i < arg1.Count; ++i)
            {
                for (int j = 0; j < arg2.Count; ++j)
                {
                    if (Intersect(arg1[i], arg2[j]))
                        newI.Add(arg1[i] * arg2[j]);
                }
            }

            return newI;
        }

        protected static bool Intersect(Interval arg1, Interval arg2)
        {
            if (arg1._a <= arg2._a && arg2._a <= arg1._b)
                return true;
            if (arg1._a <= arg2._b && arg2._b <= arg1._b)
                return true;
            if (arg2._a <= arg1._a && arg1._a <= arg2._b)
                return true;
            if (arg2._a <= arg1._b && arg1._b <= arg2._b)
                return true;
            return false;
        }

        public int CompareTo(Interval obj)
        {
            if (_a == obj._a) return 0;
            return (_a - obj._a > 0) ? 1 : -1;
        }
    }
}
