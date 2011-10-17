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
        public void Union_GivenTwoIntervals0123_Returns0123()
        {
            var result = Interval.Union(new List<Interval>() { new Interval(_0, _1) }, new List<Interval> { new Interval(_2, _3) });
            var expected = CreateIntervalList(_0, _1, _2, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Union_GivenTwoIntervals0213_Returns03()
        {
            var result = Interval.Union(new List<Interval>() { new Interval(_0, _2) }, new List<Interval> { new Interval(_1, _3) });
            var expected = CreateIntervalList(_0, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Union_GivenTwoIntervals0312_Returns03()
        {
            var result = Interval.Union(new List<Interval>() { new Interval(_0, _3) }, new List<Interval> { new Interval(_1, _2) });
            var expected = CreateIntervalList(_0, _3);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_GivenTwoIntervals0123_ReturnsEmpty()
        {
            var result = Interval.Intersection(new List<Interval>() { new Interval(_0, _1) }, new List<Interval> { new Interval(_2, _3) });
            var expected = CreateIntervalList();

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_GivenTwoIntervals0213_Returns12()
        {
            var result = Interval.Intersection(new List<Interval> { new Interval(_0, _2) }, new List<Interval> { new Interval(_1, _3) });
            var expected = CreateIntervalList(_1, _2);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Intersection_GivenTwoIntervals0312_Returns12()
        {
            var result = Interval.Intersection(new List<Interval>() { new Interval(_0, _3) }, new List<Interval> { new Interval(_1, _2) });
            var expected = CreateIntervalList(_1, _2);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Difference_GivenTwoIntervals0123_Returns03()
        {
            var result = Interval.Difference(new List<Interval>() { new Interval(_0, _1) }, new List<Interval> { new Interval(_2, _3) });
            var expected = CreateIntervalList(_0, _1);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Differenc_GivenTwoIntervals0213_Returns01()
        {
            var result = Interval.Difference(new List<Interval> { new Interval(_0, _2) }, new List<Interval> { new Interval(_1, _3) });
            var expected = CreateIntervalList(_0, _1);

            AssertAreEqual(expected, result);
        }

        [TestMethod]
        public void Difference_GivenTwoIntervals0312_Returns0123()
        {
            var result = Interval.Difference(new List<Interval>() { new Interval(_0, _3) }, new List<Interval> { new Interval(_1, _2) });
            var expected = CreateIntervalList(_0, _1, _2, _3);

            AssertAreEqual(expected, result);
        }
    }
}
