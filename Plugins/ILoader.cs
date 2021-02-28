using System.Collections.Generic;

namespace Plugins
{
  public interface ILoader
  {
    List<T> Load<T>() where T : class;
  }
}
