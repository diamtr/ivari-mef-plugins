namespace ToolBox.Tests.Engine
{
  public class SourceCodes
  {
    public static string GetSharedInterfaceCode()
    {
      return @"namespace Interfaces
{
  public interface ITestable
  {
    void Test();
  }
}";
    }

    public static string GetPluginCode()
    {
      return @"using System.ComponentModel.Composition;
using Interfaces;

namespace Pl1
{
  [Export(typeof(ITestable))]
  public class SomeClass : ITestable
  {
    public void Test()
    {
    }
  }
}";
    }
  }
}
