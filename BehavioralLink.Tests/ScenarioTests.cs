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
    }
}