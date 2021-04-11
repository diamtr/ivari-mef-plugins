using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IvarI.Plugins.FileSystem
{
  public class Configuration : ISourcesConfiguration
  {
    private List<string> paths;

    /// <summary>
    /// Add directory path to configuration.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <remarks>Add contained file directory if a file path sended.</remarks>
    public void AddDirectory(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;
      path = this.NormalizePath(path);
      this.paths.Add(path);
    }

    /// <summary>
    /// Add subdirectories paths of sended directory to configuration.
    /// </summary>
    /// <param name="path">Directory path.</param>
    /// <param name="level">Subdirectories level.</param>
    /// <remarks>Add subdirectories of contained file directory if a file path sended.</remarks>
    public void AddSubDirectories(string path, int level = 1)
    {
      if (level < 0)
        throw new ArgumentException($"Parameter {nameof(level)} must be positive.");
      if (string.IsNullOrWhiteSpace(path))
        return;
      path = this.NormalizePath(path);
      this.paths.AddRange(this.GetSubdirectoriesRecursive(path, 0, level));
    }

    private List<string> GetSubdirectoriesRecursive(string path, int level, int maxLevel)
    {
      if (level == maxLevel)
        return new List<string>() { path };
      if (!Directory.Exists(path))
        return new List<string>();
      return Directory.GetDirectories(path)
        .SelectMany(x => this.GetSubdirectoriesRecursive(x, level + 1, maxLevel))
        .ToList();
    }

    private string NormalizePath(string path)
    {
      if (!this.IsDirectory(path))
        path = Path.GetDirectoryName(path);
      return path;
    }

    private bool IsDirectory(string path)
    {
      if (File.Exists(path) || Directory.Exists(path))
        return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
      else
        return !Path.HasExtension(path);
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
