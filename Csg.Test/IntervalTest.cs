using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csg.Test
{
    [TestClass]
    public class IntervalTest
    {
        public const float _0 = 0f, _1 = 1f, _2 = 2f, _3 = 3f;

        private void AssertAreEqual(List<Interval> expected, List<Interval> actual)
        {
            if (expected.Count != actual.Count)
            {
                Assert.Fail("Collection of different sizes");
            }
            if (Enumerable.Range(0, expected.Count).Any(i => actual[i].A != expected[i].A || actual[i].B != expected[i].B))
            {
                Assert.Fail("Not all intervals are equal");
            }
        }

        private List<Interval> CreateIntervalList(params float[] args)
        {
            Assert.AreEqual(0, args.Length % 2);

            List<Interval> retVal = new List<Interval>();

            for (int i = 0; i < args.Length; i += 2)
            {
                retVal.Add(new Interval(args[i], args[i + 1]));
            }
            return retVal;
        }

        [TestMethod]
        //Given intervals (0, 1) and (2, 3) returns intervals (0, 1) and (2, 3)
        public void Union_Given01And23_Returns0123()
        {
            var result = Interval.Union(CreateIntervalList(_0, _1), CreateIntervalList(_2, _3));
            var expected = CreateIntervalList(_0, _1, _2, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Union_Given02And13_Returns03()
        {
            var result = Interval.Union(CreateIntervalList(_0, _2), CreateIntervalList(_1, _3));
            var expected = CreateIntervalList(_0, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Union_Given03And12_Returns03()
        {
            var result = Interval.Union(CreateIntervalList(_0, _3), CreateIntervalList(_1, _2));
            var expected = CreateIntervalList(_0, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_Given01And23_ReturnsEmpty()
        {
            var result = Interval.Intersection(CreateIntervalList(_0, _1), CreateIntervalList(_2, _3));
            var expected = CreateIntervalList();

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_Given02And13_Returns12()
        {
            var result = Interval.Intersection(CreateIntervalList(_0, _2), CreateIntervalList(_1, _3));
            var expected = CreateIntervalList(_1, _2);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_Given03And12_Returns12()
        {
            var result = Interval.Intersection(CreateIntervalList(_0, _3), CreateIntervalList(_1, _2));
            var expected = CreateIntervalList(_1, _2);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Difference_Given01And23_Returns03()
        {
            var result = Interval.Difference(CreateIntervalList(_0, _1), CreateIntervalList(_2, _3));
            var expected = CreateIntervalList(_0, _1);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Differenc_Given02And13_Returns01()
        {
            var result = Interval.Difference(CreateIntervalList(_0, _2), CreateIntervalList(_1, _3));
            var expected = CreateIntervalList(_0, _1);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Difference_Given03And12_Returns0123()
        {
            var result = Interval.Difference(CreateIntervalList(_0, _3), CreateIntervalList(_1, _2));
            var expected = CreateIntervalList(_0, _1, _2, _3);

            AssertAreEqual(expected, result);
        }
    }
}
