using System.Collections.Generic;

namespace IvarI.Plugins.FileSystem
{
  public interface ISourcesConfiguration
  {
    void AddDirectory(string path);
    void AddSubDirectories(string path, int level);
    List<string> GetPaths();
  }
}
