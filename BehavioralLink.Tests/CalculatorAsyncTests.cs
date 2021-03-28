using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class CalculatorAsyncTests
    {
        [TestMethod]
        public void AsRulesAndExamplesAsync()
        {
            var actual = Feature.Load("Features/Calculator-Rules.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Single();
        }

        [TestMethod]
        public void AsScenarioOutlineAsync()
        {
            var actual = Feature.Load("Features/Calculator-Outline.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Count();

            Assert.AreEqual(3, actual);
        }

        [TestMethod]
        public void WithBackgroundAsync()
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

            public async Task IHaveEnteredIntoTheCalculator(int number)
            {
                await Task.Delay(100);
                Value1 = number;
            }

            public Task IHaveAlsoEnteredIntoTheCalculator(int number)
            {
                Value2 = number;
                return Task.FromResult(0);
            }

            /* NOTE: returning values from context methods doesn't really make much sense.
             *       this library did not take an opinion on this before.  so to maintain
             *       consistency, we're not taking an opinion on it here either.
             */
            public async Task<int> IPressAdd()
            {
                Answer = Value1 + Value2;
                return await Task.FromResult(0);
            }

            public Task<int> TheResultShouldBeOnTheScreen(int value)
            {
                Assert.AreEqual(value, Answer);
                return Task.FromResult(0);
            }
        }
    }
}