using System;
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
            Then test an enum of ""Yellow""
            Then test an enum of 1
            Then test a multi-word enum of 'Unicorn White'
            Then test a utc offset of ""2020-08-02T14:30:15Z""
            Then test a local offset of ""2020-08-02T14:30:15""
            Then test a utc date time of ""2020-08-02T14:30:15Z""
            Then test a local date time of ""2020-08-02T14:30:15""
            ")
            .Select(s => s.Execute( new Context()))
            .Single();
        }

        enum Color
        {
            Red = 0,
            Yellow = 1,
            Green = 2,
            UnicornWhite = 3
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

            public void TestAnEnumOf(Color value)
            {
                Assert.AreEqual(Color.Yellow, value);
            }

            public void TestAMultiWordEnumOf(Color value)
            {
                Assert.AreEqual(Color.UnicornWhite, value);
            }

            public void TestAUtcOffsetOf(DateTimeOffset date)
            {
                var expected = new DateTimeOffset(2020,8,2,14,30,15,0, TimeSpan.Zero);

                Assert.AreEqual(expected, date);
            }

            public void TestALocalOffsetOf(DateTimeOffset date)
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                var expected = new DateTimeOffset(2020,8,2,14,30,15,0, offset);

                Assert.AreEqual(expected, date);
            }

            public void TestAUtcDateTimeOf(DateTime date)
            {
                var expected = new DateTime(2020,8,2,14,30,15,0, DateTimeKind.Utc);

                Assert.AreEqual(expected, date);
            }

            public void TestALocalDateTimeOf(DateTime date)
            {
                var expected = new DateTime(2020,8,2,14,30,15,0, DateTimeKind.Local);

                Assert.AreEqual(expected, date);
            }
        }
    }
}