using System.Collections.Generic;
using System.IO;

namespace ToolBox.Tests.Engine
{
  internal class TestDirectories
  {
    private List<string> memory;

    public TestDirectories()
    {
      this.memory = new List<string>();
    }

    public void CreateIfNotExists(string path)
    {
      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
        this.memory.Add(path);
      }
    }

    public void DeleteIfCreated(string path)
    {
      if (this.memory.Contains(path))
      {
        if (Directory.Exists(path))
          Directory.Delete(path, true);
        this.memory.Remove(path);
      }
    }
  }
}
