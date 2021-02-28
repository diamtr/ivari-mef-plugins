using System.Collections.Generic;

namespace ToolBox
{
  public interface ILoader
  {
    List<T> Load<T>() where T : class;
  }
}
