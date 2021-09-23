using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static bool InBounds<T>(this T value, T leftBound, T rightBound) where T : IComparable<T> =>
    value.CompareTo(leftBound) > 0 && value.CompareTo(rightBound) < 0;

  public static bool InLeftBound<T>(this T value, T leftBound, T rightBound) where T : IComparable<T> =>
    value.CompareTo(leftBound) >= 0 && value.CompareTo(rightBound) < 0;

  public static bool InRightBound<T>(this T value, T leftBound, T rightBound) where T : IComparable<T> =>
    value.CompareTo(leftBound) > 0 && value.CompareTo(rightBound) <= 0;

  public static bool InInclusiveBounds<T>(this T value, T leftBound, T rightBound) where T : IComparable<T> =>
    value.CompareTo(leftBound) >= 0 && value.CompareTo(rightBound) <= 0;

  public static T Or<T>(this bool predicate, T ifTrue, T ifFalse) => predicate switch {
    true  => ifTrue,
    false => ifFalse
  };

  public static void Then(this bool predicate, Action action)
  {
    if (predicate) action();
  }
  public static void Then<T>(this bool predicate, Action<T> action, T arg)
  {
    if (predicate) action(arg);
  }

  public static IEnumerable<(T Element, int Index)> Enumerate<T>(this IEnumerable<T> source)
  {
    return source.Select((item, index) => (item, index));
  }

  public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
  {
    source.Enumerate().ToImmutableList().ForEach(pair => action(pair.Element, pair.Index));
  }

  public static void ForEach<T>(this IEnumerable<T> source, Action<T, uint> action)
  {
    source.Enumerate().ToImmutableList().ForEach(pair => action(pair.Element, (uint)pair.Index));
  }

  public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
  {
    source.ToImmutableList().ForEach(action);
  }

  public static void ForEach<T, Y>(this IEnumerable<T> source, Func<T, Y> action)
  {
    source.ToImmutableList().ForEach(a => action(a));
  }

  public static void AggregatedForEach<X, T>(this IEnumerable<T> source, Action<X, T, int> action, Func<X, T, X> what, X initial)
  {
    void AggregatedAction(T element, int index)
    {
      action(initial, element, index);
      initial = what(initial, element);
    }
    source.ToImmutableList().ForEach(AggregatedAction);
  }

  public static void AggregatedForEach<X, T>(this IEnumerable<T> source, Action<X, T, uint> action, Func<X, T, X> what, X initial)
  {
    void AggregatedAction(T element, uint index)
    {
      action(initial, element, index);
      initial = what(initial, element);
    }
    source.ToImmutableList().ForEach(AggregatedAction);
  }

  public static void AggregatedForEach<X, T>(this IEnumerable<T> source, Action<X, T> action, Func<X, T, X> what, X initial)
  {
    void AggregatedAction(T element)
    {
      action(initial, element);
      initial = what(initial, element);
    }
    source.ToImmutableList().ForEach(AggregatedAction);
  }
}
}