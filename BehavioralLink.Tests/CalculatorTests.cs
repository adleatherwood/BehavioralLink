using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void AsRulesAndExamples()
        {
            var actual = Feature.Load("Features/Calculator-Rules.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Single();
        }

        [TestMethod]
        public void AsScenarioOutline()
        {
            var actual = Feature.Load("Features/Calculator-Outline.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Count();

            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void WithBackground()
        {
            var actual = Feature.Load("Features/Calculator-Background.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Single();
        }

        public static readonly Feature Feature = Feature.Load("Features/Calculator.feature", Resolve.InProjectRoot);

        [TestMethod]
        public void AddTwoNumbers()
        {
            Run("Add two numbers");
        }

        [TestMethod]
        public void AddTwoMoreNumbers()
        {
            Run("Add two more numbers");
        }

        private void Run(string scenario)
        {
            Feature
                .Where(s => s.IsNamed(scenario))
                .Select(s => s.Execute(new Context()))
                .Single();
        }

        class Context
        {
            public int Value1;
            public int Value2;
            public int Answer;

            public void IHaveEnteredIntoTheCalculator(int number)
            {
                Value1 = number;
            }

            public void IHaveAlsoEnteredIntoTheCalculator(int number)
            {
                Value2 = number;
            }

            public void IPressAdd()
            {
                Answer = Value1 + Value2;
            }

            public void TheResultShouldBeOnTheScreen(int value)
            {
                Assert.AreEqual(value, Answer);
            }
        }
    }
}