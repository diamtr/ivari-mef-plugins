using System.Collections.Generic;

namespace ToolBox
{
  public class FileSystemLoader : ILoader
  {
    private IFileSystemSourcesConfiguration configuration;

    public List<object> Load()
    {
      return new List<object>();
    }

    public FileSystemLoader()
    {
    }

    public FileSystemLoader(IFileSystemSourcesConfiguration configuration)
    {
      this.configuration = configuration;
    }
  }
}
