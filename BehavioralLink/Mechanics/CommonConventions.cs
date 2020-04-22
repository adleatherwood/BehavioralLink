using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BehavioralLink.Mechanics
{
    public static class CommonConventions
    {
        public static readonly Regex CodeCompliant = new Regex(@"[^a-zA-Z0-9_]");
        public static readonly Regex BasicParameters = new Regex(@"['""<](.*?)['"">]|(-{0,1}\d+\.{0,1}\d*)|(True)|(False)", RegexOptions.IgnoreCase);

        public static string StripBasicParameters(string value)
        {
            var result = BasicParameters.Replace(value, "");

            return result;
        }

        public static string TitlizeWords(string value)
        {
            var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

            return result;
        }

        public static string StripToCodeCompliant(string value)
        {
            var result = CodeCompliant.Replace(value, "");

            return result;
        }

        public static string StripSpaces(string value)
        {
            var result = value.Replace(" ", "");

            return result;
        }

        public static IEnumerable<string> ExtractBasicParameters(string value)
        {
            var result = BasicParameters.Matches(value)
                .Cast<Match>()
                .Select(match =>
                    match.Groups[4].Value != "" ? match.Groups[4].Value
                    : match.Groups[3].Value != "" ? match.Groups[3].Value
                    : match.Groups[2].Value != "" ? match.Groups[2].Value
                    : match.Groups[1].Value )
                .ToList();

            return result;
        }

        public static object CoerceType(object value, ParameterInfo parameter)
        {
            var targetType = parameter.ParameterType;
            var result = targetType.IsEnum
                ? Enum.Parse(targetType, 
                    StripSpaces(value.ToString()))
                : targetType == typeof(DateTimeOffset)
                ? DateTimeOffset.Parse(value as string)
                : targetType == typeof(DateTime)
                ? DateTimeOffset.Parse(value as string).DateTime
                : System.Convert.ChangeType(value, targetType);

            return result;
        }
    }
}