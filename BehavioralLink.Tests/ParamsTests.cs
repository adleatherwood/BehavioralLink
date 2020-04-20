using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class ParamsTests
    {
        [TestMethod]
        public void TestAllTheExpectedInlineParams()
        {
            Feature.Load(@"
            Feature: Parameters are parse and passed correctly
            Scenario: Pass all the parameters
            Then test an integer of 1234
            Then test a negative integer of -1234
            Then test a decimal of 12.34
            Then test a long of 1234
            Then test a double of 12.34
            Then test a positive bool of true
            Then test a negative bool of false
            Then test a this string of 'this'
            Then test a that string of ""that""
            ")
            .Select(s => s.Execute( new Context()))
            .Single();
        }

        class Context
        {
            public void TestAnIntegerOf(int value)
            {
                Assert.AreEqual(1234, value);
            }

            public void TestANegativeIntegerOf(int value)
            {
                Assert.AreEqual(-1234, value);
            }

            public void TestADecimalOf(decimal value)
            {
                Assert.AreEqual(12.34m, value);
            }

            public void TestALongOf(long value)
            {
                Assert.AreEqual(1234L, value);
            }

            public void TestADoubleOf(double value)
            {
                Assert.AreEqual(12.34, value);
            }

            public void TestAPositiveBoolOf(bool value)
            {
                Assert.IsTrue(value);
            }

            public void TestANegativeBoolOf(bool value)
            {
                Assert.IsFalse(value);
            }

            public void TestAThisStringOf(string value)
            {
                Assert.AreEqual("this", value);
            }

            public void TestAThatStringOf(string value)
            {
                Assert.AreEqual("that", value);
            }
        }
    }
}