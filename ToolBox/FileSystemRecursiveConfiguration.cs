using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToolBox
{
  public class FileSystemRecursiveConfiguration : IFileSystemSourcesConfiguration
  {
    private List<string> paths;

    public int Depth { get; set; }

    public void AddSource(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;
      this.paths.AddRange(this.GetSubdirectoriesRecursive(path, 0));
    }

    private List<string> GetSubdirectoriesRecursive(string path, int level)
    {
      if (level == this.Depth)
        return new List<string>() { path };

      return Directory.GetDirectories(path)
        .SelectMany(x => this.GetSubdirectoriesRecursive(x, level + 1))
        .ToList();
    }

    public List<string> GetPaths()
    {
      return this.paths;
    }

    public FileSystemRecursiveConfiguration()
    {
      this.paths = new List<string>();
      this.Depth = 1;
    }
  }
}
