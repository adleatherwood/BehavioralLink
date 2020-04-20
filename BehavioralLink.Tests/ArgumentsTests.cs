using System;
using System.Linq;
using Gherkin.Pickles;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class ArgumentsTests
    {
        [TestMethod]
        public void DocsAndTables()
        {
            Feature.Load("Features/Arguments.feature", Resolve.InProjectRoot)
                .Select(s => s.Execute(new Context()))
                .Single();
        }

        class Context
        {
            public string Sentence;
            public string[] Words;
            
            public void TheTextOfAndANumberOfAndTheDocString(string text, int number, string doc)
            {
                Sentence = $"{text} {doc} {number}"; 
            }

            public void TheWordsAreAllListedInOrder() 
            {
                Words = Sentence.Split(' ');
            }

            public void TheyMatchThisTable(PickleTable table)
            {   
                var actual = table.Rows
                    .Select(row => row.Cells.First().Value)
                    .Zip(Words)
                    .All(tuple => tuple.First == tuple.Second);

                Assert.IsTrue(actual);
            }
        }
    }
}