using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class EnumerableTests
    {
        [TestMethod]
        public void WhereAUsesA()
        {
            bool isEqual(int source, int target) => source == target;

            var actual = new [] { 1, 2, 3 }
                .Where(isEqual, 2)
                .Single();

            Assert.AreEqual(2, actual);
        }

        [TestMethod]
        public void IterateAUsesA()
        {
            var actual = 0;

            void add(int source, int amount) => actual = source + amount;

            Enumerable.Repeat(1, 1)
                .Iterate(add, 2)
                .EvaluateAndIgnore();

            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void EvaluateAndIgnoreForceEvaluatesLazyEnumerables()
        {
            IEnumerable<int> numbers()  { foreach (var i in Enumerable.Range(3,1)) yield return i; }

            var actual = 0;

            numbers()
                .Select(n => actual += n)
                .EvaluateAndIgnore();

            Assert.AreEqual(3, actual);
        }
    }
}
