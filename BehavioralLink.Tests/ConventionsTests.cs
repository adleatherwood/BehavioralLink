using System.Linq;
using BehavioralLink.Mechanics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class ConventionsTests
    {
        [TestMethod]
        public void StripBasicParametersRemovesIntegers()
        {
            var actual = CommonConventions.StripBasicParameters("abc 1234 def");

            Assert.AreEqual("abc  def", actual);
        }

        [TestMethod]
        public void StripBasicParametersRemovesDecimals()
        {
            var actual = CommonConventions.StripBasicParameters("abc 12.34 def");

            Assert.AreEqual("abc  def", actual);
        }

        [TestMethod]
        public void StripBasicParametersRemovesDoubleQuotedStrings()
        {
            var actual = CommonConventions.StripBasicParameters("abc \"1234\" def");

            Assert.AreEqual("abc  def", actual);
        }

        [TestMethod]
        public void StripBasicParametersRemovesSingleQuotedStrings()
        {
            var actual = CommonConventions.StripBasicParameters("abc '1234' def");

            Assert.AreEqual("abc  def", actual);
        }

        [TestMethod]
        public void StripBasicParametersRemovesCarrotedStrings()
        {
            var actual = CommonConventions.StripBasicParameters("abc <1234> def");

            Assert.AreEqual("abc  def", actual);
        }

        [TestMethod]
        public void TitlizeWordsDoesTheThing()
        {
            var actual = CommonConventions.TitlizeWords("the quick brown fox");

            Assert.AreEqual("The Quick Brown Fox", actual);
        }

        [TestMethod]
        public void ExtractBasicParametersExtractsNumbersAndStrings()
        {
            var actual = string.Join(' ',
                CommonConventions.ExtractBasicParameters("abc 1 def '12' ghi \"123\" jkl 12.34"));

            Assert.AreEqual("1 12 123 12.34", actual);
        }
    }
}