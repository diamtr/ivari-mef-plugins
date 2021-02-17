using System.Collections.Generic;

namespace ToolBox
{
  public interface IFileSystemSourcesConfiguration
  {
    void AddSource(string path);
    List<string> GetPaths();
  }
}
