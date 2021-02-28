using System.Collections.Generic;

namespace Plugins
{
  public interface IFileSystemSourcesConfiguration
  {
    void AddSource(string path);
    List<string> GetPaths();
  }
}
