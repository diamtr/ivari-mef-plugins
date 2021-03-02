using System.Collections.Generic;

namespace Plugins.FileSystem
{
  public interface ISourcesConfiguration
  {
    void AddDirectory(string path);
    void AddSubDirectories(string path, int level);
    List<string> GetPaths();
  }
}
