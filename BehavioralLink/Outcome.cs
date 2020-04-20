namespace BehavioralLink
{
    public class Outcome<T>
    {
        internal Outcome(T context, Scenario scenario) =>
            (Context, Scenario) = (context, scenario);
        public readonly T Context;
        public readonly Scenario Scenario;
    }

    public static class Outcome
    {
        public static Outcome<T> Create<T>(T context, Scenario scenario) =>
            new Outcome<T>(context, scenario);
    }
}