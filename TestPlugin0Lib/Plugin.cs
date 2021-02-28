using System;
using System.ComponentModel.Composition;
using TestSharedInterfacesLib;

namespace TestPlugin0Lib
{
  [Export(typeof(IPlugin))]
  public class Plugin : IPlugin
  {
    public void BeAwesome()
    {
      throw new NotImplementedException("Test plugin-0 is awesome.");
    }
  }
}
