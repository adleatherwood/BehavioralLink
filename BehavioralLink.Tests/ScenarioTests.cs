using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class ScenarioTests
    {
        [TestMethod]
        public void TagsAreFound()
        {
            var scenario = Feature.Load(@"
                Feature: tags matter
                @atag
                Scenario: has tag
                Given a tagged scenario")
                .First();

            var actual = scenario.IsTagged("@atag", "@btag");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TagsAreNotFound()
        {
            var scenario = Feature.Load(@"
                Feature: tags matter
                @atag
                Scenario: has tag
                Given a tagged scenario")
                .First();

            var actual = scenario.NotTagged("@btag", "@ctag");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void NamesAreFound()
        {
            var scenario = Feature.Load(@"
                Feature: tags matter
                @atag
                Scenario: has tag
                Given a tagged scenario")
                .First();

            var actual = scenario.IsNamed("Has Tag", "Tag You're It");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void NamesAreNotFound()
        {
            var scenario = Feature.Load(@"
                Feature: tags matter
                @atag
                Scenario: has tag
                Given a tagged scenario")
                .First();

            var actual = scenario.NotNamed("TaGs DoN't MatTer", "TaGs-R-LaMe");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ExceptionsHaveDetail()
        {
            var scenario = Feature.Load(@"
                Feature: Exception messages matter
                Scenario: Exception message is useful
                Given a missing step implementation")
                .First();

            var actual = Try(() => scenario.Execute(new object()));

            Assert.IsTrue(actual.Message.Contains("Exception message is useful"));
            Assert.IsTrue(actual.Message.Contains("a missing step implementation"));
        }

        [TestMethod]
        public void ExceptionsHaveAssertionDetail()
        {
            var scenario = Feature.Load(@"
                Feature: Exception messages matter
                Scenario: Exception message is useful
                Given a failing assertion")
                .First();

            var actual = Try(() => scenario.Execute(new TestContext()));

            Assert.IsTrue(actual.Message.Contains("You shall not pass... this assertion"));
        }

        class TestContext
        {
            public void AFailingAssertion()
            {
                Assert.Fail("You shall not pass... this assertion");
            }
        }

        private Exception Try(Action a)
        {
            try
            {
                a();
                return null;
            }
            catch(Exception e)
            {
                return e;
            }
        }
    }
}