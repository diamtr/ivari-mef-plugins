/*
 * Copyright 2021 Ivan Dmitriev
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
    /// Get default file system sources configuration.
    /// </summary>
    /// <returns>Default instance of sources configuration.</returns>
    /// <remarks>Include first level directories in .\plugins</remarks>
    public static ISourcesConfiguration GetDefault()
    {
      var conf = new Configuration();
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
      conf.AddSubDirectories(path);
      return conf;
    }

    /// <summary>
    /// Add the directory path to configuration.
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
    /// Add subdirectories paths of the directory to configuration.
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
