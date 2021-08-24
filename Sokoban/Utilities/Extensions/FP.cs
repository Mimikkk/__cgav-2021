using System;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static T Or<T>(this bool predicate, T ifTrue, T ifFalse) => predicate ? ifTrue : ifFalse;

  public static void Then(this bool predicate, Action action)
  {
    if (predicate) action();
  }
  public static void Then<T>(this bool predicate, Action<T> action, T arg)
  {
    if (predicate) action(arg);
  }
}
}