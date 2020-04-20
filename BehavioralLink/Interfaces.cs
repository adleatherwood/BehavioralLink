using System.IO;
using Gherkin.Pickles;

namespace BehavioralLink
{
    /// <summary>
    /// An abstraction that allows for feature source files to be located
    /// anywhere in the networked universe.  Given a key for a resoure, a
    /// readable stream is returned.
    /// </summary>
    public interface ISourceResolver
    {
        Stream Resolve(string filename);
    }

    /// <summary>
    /// An abstract that allows for custom execution routines for steps.
    /// </summary>
    public interface IStepResolver
    {
        void Resolve<T>(PickleStep step, T context);
    }
}