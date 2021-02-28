using System.Collections.Generic;

namespace Plugins
{
  public class FileSystemPlainConfiguration : IFileSystemSourcesConfiguration
  {
    private List<string> paths;

    public void AddSource(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;

      this.paths.Add(path);
    }

    public List<string> GetPaths()
    {
      return this.paths;
    }

    public FileSystemPlainConfiguration()
    {
      this.paths = new List<string>();
    }
  }
}
