using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plugins.FileSystem
{
  public class Configuration : ISourcesConfiguration
  {
    private List<string> paths;

    public void AddPath(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;

      this.paths.Add(path);
    }

    public void AddPathRecursively(string path, int level)
    {
      if (level < 0)
        throw new ArgumentException($"Parameter {nameof(level)} must be positive.");

      if (string.IsNullOrWhiteSpace(path))
        return;

      this.paths.AddRange(this.GetSubdirectoriesRecursive(path, 0, level));
    }

    private List<string> GetSubdirectoriesRecursive(string path, int level, int maxLevel)
    {
      if (level == maxLevel)
        return new List<string>() { path };

      return Directory.GetDirectories(path)
        .SelectMany(x => this.GetSubdirectoriesRecursive(x, level + 1, maxLevel))
        .ToList();
    }

    public List<string> GetPaths()
    {
      return this.paths;
    }

    public Configuration()
    {
      this.paths = new List<string>();
    }
  }
}
