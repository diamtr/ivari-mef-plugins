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
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace IvarI.Plugins.FileSystem
{
  public class Loader : ILoader
  {
    private ISourcesConfiguration configuration;

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

    public Loader(ISourcesConfiguration configuration)
    {
      this.configuration = configuration;
    }
  }
}
