using System.Collections.Generic;

namespace Plugins.FileSystem
{
  public interface ISourcesConfiguration
  {
    void AddSource(string path);
    List<string> GetPaths();
  }
}
