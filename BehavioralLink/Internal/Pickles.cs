using Gherkin.Pickles;
using System.Linq;

namespace BehavioralLink
{
    internal static class Pickles
    {
        public static bool HasTag(Pickle p, string tag) =>
            p.Tags
                .Select(PickleTags.Name)
                .Where(FuzzyString.Compare, tag)
                .Any();
    }

    internal static class PickleTags
    {
        public static string Name(PickleTag t) =>
            t.Name;
    }
}
