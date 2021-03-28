using Gherkin.Pickles;
using System;
using System.Linq;

namespace BehavioralLink
{
    /* NOTE: the "pickles" language makes no sense at all to a test author.
     *       this is an attempt to normalize the language to what the author
     *       is looking at, which is features & scenarios.
     */
    public partial class Scenario
    {
        internal Scenario(Pickle pickle) => Pickle = pickle;

        public readonly Pickle Pickle;
    }

    public static class Scenarios
    {
        public static Scenario Create(Pickle pickle) =>
            new Scenario(pickle);

        /// <summary>
        /// An inclusion filter for scenarios containing any of the given tags.
        /// </summary>
        public static bool IsTagged(this Scenario scenario, params string[] tags) =>
            tags.Any(tag => Pickles.HasTag(scenario.Pickle, tag));

        /// <summary>
        /// A exclusion filter for scenarios containing any of the given tags.
        /// </summary>
        public static bool NotTagged(this Scenario scenario, params string[] tags) =>
            !IsTagged(scenario, tags);

        /// <summary>
        /// A inclusion filter for scenarios containing any of the given names.
        /// </summary>
        public static bool IsNamed(this Scenario scenario, params string[] names) =>
            names.Contains(scenario.Pickle.Name, FuzzyString.Comparer);

        /// <summary>
        /// A exclusion filter for scenarios containing any of the given names.
        /// </summary>
        public static bool NotNamed(this Scenario scenario, params string[] names) =>
            !IsNamed(scenario, names);

        /// <summary>
        /// Executes a scenario passing the given context to each step and using the
        /// default conventions for resolve step methods.
        /// </summary>
        public static Outcome<T> Execute<T>(this Scenario scenario, T context)
            where T : class =>
            Execute(scenario, context, Resolve.ByDefaultConvention);

        /// <summary>
        /// Executes a scenario passing the given context to each step.
        /// </summary>
        public static Outcome<T> Execute<T>(this Scenario scenario, T context, IStepResolver resolver)
            where T : class
        {
            scenario.Pickle.Steps
                .Iterate(step => TryResolve(scenario.Pickle, step, resolver, context))
                .EvaluateAndIgnore();

            return Outcome.Create(context, scenario);
        }

        private static void TryResolve<T>(Pickle pickle, PickleStep step, IStepResolver resolver, T context)
        {
            try
            {
                resolver.Resolve(step, context);
            }
            catch(Exception e)
            {
                var plocs = String.Join(",",
                    pickle.Locations.Select(loc => $"{loc.Line}:{loc.Column}"));

                var slocs = String.Join(",",
                    step.Locations.Select(loc => $"{loc.Line}:{loc.Column}"));

                var message = $@"

Scenario: ({plocs}) {pickle.Name}
    Step: ({slocs}) {step.Text}
   Error: {(e.InnerException ?? e).Message}

  Test Trace:
{(e.InnerException ?? e).StackTrace}

";
                throw new Exception(message);
            }
        }
    }
}
