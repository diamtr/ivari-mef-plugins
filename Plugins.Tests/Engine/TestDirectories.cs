using System.Collections.Generic;
using System.IO;

namespace Plugins.Tests.Engine
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

    public void TryClearAll()
    {
      foreach (var path in this.memory)
      {
        try
        {
          if (Directory.Exists(path))
            Directory.Delete(path, true);
        }
        catch { }
      }
      this.memory.Clear();
    }
  }
}
