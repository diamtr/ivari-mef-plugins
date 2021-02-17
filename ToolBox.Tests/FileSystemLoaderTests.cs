using NUnit.Framework;

namespace ToolBox.Tests
{
  public class FileSystemLoaderTests
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