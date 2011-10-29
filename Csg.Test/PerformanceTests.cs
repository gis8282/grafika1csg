using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csg.Test
{
    [TestClass]
    public class PerformanceTests
    {
        private const int NUM_OF_ITERATIONS = 2000000000;

        [TestMethod]
        public void IEnumerableSelect()
        {
            var seed = 10;
            var result = Enumerable.Range(0, NUM_OF_ITERATIONS).Select(i => seed ^= i);

            foreach (var i in result) ;

            Console.WriteLine(seed);
        }

        [TestMethod]
        public void For()
        {
            var seed = 10;
            
            for (int i = 0; i < NUM_OF_ITERATIONS; i++)
            {
                seed ^= i;
            }

            Console.WriteLine(seed);
        }
    }
}
