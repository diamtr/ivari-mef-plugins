using System;
using TestSharedInterfacesLib;

namespace TestPlugin0Lib
{
  public class Plugin : IPlugin
  {
    public void BeAwesome()
    {
      throw new NotImplementedException("Test plugin-0 is awesome.");
    }
  }
}
