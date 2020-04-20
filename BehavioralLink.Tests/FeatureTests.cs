using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BehavioralLink.Tests
{
    [TestClass]
    public class FeatureTests
    {
        [TestMethod]
        public void LoadFromFileSucceeds()
        {
            var file = new FileInfo("../../../Features/Calculator.feature");
            var feature = Feature.Load(file);

            Assert.IsNotNull(feature.Document);
        }

        [TestMethod]
        public void LoadFromStringSucceeds()
        {
            var feature = Feature.Load(@"
            Feature: A feature
            Scenario: A situation
            Given A setup
            ");

            Assert.IsNotNull(feature.Document);
        }

        [TestMethod]
        public void LoadFromResolverSucceeds()
        {
            var feature = Feature.Load("Features/Calculator.feature", Resolve.InProjectRoot);

            Assert.IsNotNull(feature.Document);
        }

        [TestMethod]
        public void LoadFromStreamSucceeds()
        {
            var file = new FileInfo("../../../Features/Calculator.feature");
            
            using (var stream = file.OpenRead())
            {
                var feature = Feature.Load(stream);

                Assert.IsNotNull(feature.Document);
            }
        }

        [TestMethod]
        public void LoadFromReaderSucceeds()
        {
            var file = new FileInfo("../../../Features/Calculator.feature");
            
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                var feature = Feature.Load(reader);

                Assert.IsNotNull(feature.Document);
            }
        }
    }
}