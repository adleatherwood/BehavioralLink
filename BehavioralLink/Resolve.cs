using BehavioralLink.Mechanics;

namespace BehavioralLink
{
    public static class Resolve
    {
        /// <summary>
        /// Attempts to adjust relative path finding to the project folder, which
        /// is, by default, three directories up from the bin directory where the
        /// actual execution of the tests takes place.
        /// </summary>
        public static InProjectRootFinder InProjectRoot = new InProjectRootFinder();

        /// <summary>
        /// A step resolver that resolves steps to static functions located on the context
        /// object passed to each scenario execution.  See documentation for details.
        /// </summary>
        public static DefaultConventionResolver ByDefaultConvention = new DefaultConventionResolver();
    }
}
