using System.IO;

namespace BehavioralLink.Mechanics
{
    public class InProjectRootFinder : ISourceResolver
    {
        public Stream Resolve(string name)
        {
            var relative = Path.Combine("../../../", name);

            return new FileInfo(relative).OpenRead();
        }
    }
}
