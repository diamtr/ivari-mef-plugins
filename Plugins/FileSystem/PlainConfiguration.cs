using System.Collections.Generic;

namespace Plugins.FileSystem
{
  public class PlainConfiguration : ISourcesConfiguration
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

    public PlainConfiguration()
    {
      this.paths = new List<string>();
    }
  }
}
