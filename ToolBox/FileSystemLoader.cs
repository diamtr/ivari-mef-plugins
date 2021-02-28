﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace ToolBox
{
  public class FileSystemLoader : ILoader
  {
    private IFileSystemSourcesConfiguration configuration;

    public List<T> Load<T>() where T : class
    {
      if (this.configuration == null)
        throw new Exception("Load failed. Configuration is required.");

      var plugins = new List<T>();
      var catalog = new AggregateCatalog();
      foreach (var path in this.configuration.GetPaths())
        catalog.Catalogs.Add(new DirectoryCatalog(path));

      var import = new ImportDefinition(x => true, typeof(T).FullName, ImportCardinality.ZeroOrMore, false, false);

      using (var container = new CompositionContainer(catalog))
      {
        var exports = container.GetExports(import);
        plugins.AddRange(exports.Select(x => x.Value as T).Where(x => x != null));
      }

      return plugins;
    }

    public FileSystemLoader(IFileSystemSourcesConfiguration configuration)
    {
      this.configuration = configuration;
    }
  }
}
