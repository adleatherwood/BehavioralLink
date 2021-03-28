using System;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Gherkin.Pickles;

namespace BehavioralLink.Mechanics
{
    public class DefaultConventionResolver
        : IStepResolver
    {
        public void Resolve<T>(PickleStep step, T context)
        {
            var name = ToName(step);
            var method = Find(name, context);

            if (method == null)
            {
                throw new Exception($"Unable to find step method: \"{name}\"");
            }

            var parameters = ToParams(method, step);

            if (typeof(Task).IsAssignableFrom(method.ReturnType))
                ((Task) method.Invoke(context, parameters)).GetAwaiter().GetResult();
            else
                method.Invoke(context, parameters);
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
                .Select(tuple => CommonConventions.CoerceType(tuple.Item1, tuple.Item2))
                .ToArray();

            return values;
        }
    }
}