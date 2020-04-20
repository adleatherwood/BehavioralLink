using System.Collections.Generic;

namespace BehavioralLink
{
    internal class FuzzyString : IEqualityComparer<string>
    {
        public static FuzzyString Comparer = new FuzzyString();

        public static bool Compare(string x, string y) =>
            Comparer.Equals(x, y);

        public bool Equals(string x, string y) =>
            string.Compare(Fuzz(x), Fuzz(y), true) == 0;

        public int GetHashCode(string value) =>
            Fuzz(value).GetHashCode();

        private static string Fuzz(string value) =>
            (value ?? "").Trim().ToLower();
    }
}