using System;
using System.Collections.Generic;
using System.Linq;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
  {
    first = (list.Count > 0 ? list[0] : default) ?? throw new InvalidOperationException();
    rest = list.Skip(1).ToList();
  }

  public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
  {
    first = (list.Count > 0 ? list[0] : default) ?? throw new InvalidOperationException();
    second = (list.Count > 1 ? list[1] : default) ?? throw new InvalidOperationException();
    rest = list.Skip(2).ToList();
  }
}
}