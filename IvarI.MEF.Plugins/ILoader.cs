using System.Collections.Generic;

namespace IvarI.Plugins
{
  public interface ILoader
  {
    List<T> Load<T>() where T : class;
  }
}
