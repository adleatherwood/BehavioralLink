using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using Gherkin.Pickles;

namespace BehavioralLink.Mechanics
{
    public static class CommonConventions
    {
        public static readonly Regex CodeCompliant = new Regex(@"[^a-zA-Z0-9_]");
        public static readonly Regex BasicParameters = new Regex(@"['""<](.*?)['"">]|(\d+\.{0,1}\d*)");

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

        public static IEnumerable<string> ExtractBasicParameters(string value)
        {
            var result = BasicParameters.Matches(value)
                .Cast<Match>()
                .Select(match => match.Groups[2].Value != "" ? match.Groups[2].Value : match.Groups[1].Value )
                .ToList();

            return result;
        }

        public static object ConvertText(object value, ParameterInfo parameter)
        {
            var result = System.Convert.ChangeType(value, parameter.ParameterType);

            return result;
        }
    }

    public class DefaultConventionResolver
        : IStepResolver
    {
        public void Resolve<T>(PickleStep step, T context)
        {
            var name = ToName(step);
            var method = Find(name, context);

            if (method == null)
            {
                throw new Exception($"Unable to find step method: \"{name}\" for step: \"{step.Text}\"");
            }

            var parameters = ToParams(method, step);

            try
            {
                method.Invoke(context, parameters);
            }
            catch(Exception e)
            {
                /* NOTE: this captures the message and the stack trace from the original
                 *       exception (most likely an assertion) and makes for a clearer
                 *       test failure message
                 */
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
        }

        public static string ToName(PickleStep step)
        {
            var a = CommonConventions.StripBasicParameters(step.Text);
            var b = CommonConventions.TitlizeWords(a);
            var c = CommonConventions.StripToCodeCompliant(b);

            return c;
        }

        public static MethodInfo Find<T>(string name, T context)
        {
            var found = context.GetType()
                .GetMethods()
                .Where(method => FuzzyString.Compare(method.Name, name))
                .SingleOrDefault();

            return found;
        }

        public static object[] ToParams(MethodInfo method, PickleStep step)
        {
            var inlines = CommonConventions.ExtractBasicParameters(step.Text)
                .Cast<Object>();

            var docs = step.Arguments
                .Select(arg => arg as PickleString)
                .Where(arg => arg != null)
                .Select(arg => arg.Content);

            var tables = step.Arguments
                .Select(arg => arg as PickleTable)
                .Where(arg => arg != null);

            var values = inlines
                .Concat(docs)
                .Concat(tables)
                .Zip(method.GetParameters(), Tuple.Create)
                .Select(tuple => CommonConventions.ConvertText(tuple.Item1, tuple.Item2))
                .ToArray();

            return values;
        }
    }
}