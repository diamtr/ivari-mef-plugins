using NUnit.Framework;
using ToolBox;

namespace Tests
{
  public class Tests
  {
    [Test]
    public void InitFileSystemLoader()
    {
      var loader = new FileSystemLoader();
      var res = loader.Load();
      Assert.IsNotNull(res);
      Assert.IsEmpty(res);
    }
  }
}