using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

  public static IEnumerable<(T item, int index)> Enumerate<T>(this IEnumerable<T> source) =>
    source.Select((item, index) => (item, index));

  public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action) =>
    source.Select((e, i) => (e, i)).ToImmutableList().ForEach(pair => action(pair.e, pair.i));

  public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) =>
    source.ToImmutableList().ForEach(action);

  public static void ForEach<T,Y>(this IEnumerable<T> source, Func<T, Y> action) =>
    source.ToImmutableList().ForEach(a => action(a));
}
}