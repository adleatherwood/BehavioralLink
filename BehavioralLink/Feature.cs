using Gherkin;
using Gherkin.Pickles;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using Gherkin.Ast;
using System;

namespace BehavioralLink
{
    public partial class Feature : IEnumerable<Scenario>
    {
        internal Feature(GherkinDocument document, IEnumerable<Scenario> scenarios) =>
            (Document, Scenarios) = (document, scenarios);

        public readonly GherkinDocument Document;
        public readonly IEnumerable<Scenario> Scenarios;

        public IEnumerator<Scenario> GetEnumerator() =>
            Scenarios.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Executes every scenario in a feature using the given context factory.
        /// </summary>
        public void Execute<T>(Func<T> newContext)
            where T: class =>
            this.Scenarios
                .Iterate(s => s.Execute(newContext()))
                .EvaluateAndIgnore();

        /// <summary>
        /// Executes every scenario in a feature with the given context type.
        /// </summary>
        public void Execute<T>()
            where T: class, new() =>
            this.Scenarios
                .Iterate(s => s.Execute(new T()))
                .EvaluateAndIgnore();
    }

    public partial class Feature
    {
        /// <summary>
        /// Used to load a named feature from an abstract location.
        /// </summary>
        public static Feature Load(string name, ISourceResolver finder)
        {
            using (var stream = finder.Resolve(name))
            {
                return Load(stream);
            }
        }

        /// <summary>
        /// Used to load from a file.
        /// </summary>
        public static Feature Load(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                return Load(stream);
            }
        }

        /// <summary>
        /// Used to load a feature directly from a string.
        /// </summary>
        public static Feature Load(string source)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(source);
                writer.Flush();

                stream.Position = 0;

                return Load(stream);
            }
        }

        public static Feature Load(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return Load(reader);
            }
        }

        public static Feature Load(TextReader reader)
        {
            var parser = new Parser();
            var document = parser.Parse(reader);

            var compiler = new Compiler();
            var pickles = compiler.Compile(document)
                .Select(BehavioralLink.Scenarios.Create)
                .ToList();

            return new Feature(document, pickles);
        }
    }
}
