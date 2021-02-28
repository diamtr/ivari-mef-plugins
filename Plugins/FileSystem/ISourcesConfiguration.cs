using System.Collections.Generic;

namespace Plugins.FileSystem
{
  public interface ISourcesConfiguration
  {
    void AddPath(string path);
    void AddPathRecursively(string path, int level);
    List<string> GetPaths();
  }
}
