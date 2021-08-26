using System;

namespace Sokoban.Application
{
public static partial class App
{
  public static void OnUpdate(Action<double> action, Func<bool>? when = null) =>
    Window.Update += HandleOnDifferential(action, when);

  public static void OnUpdate(Action action, Func<bool>? when = null) =>
    Window.Update += HandleOnDifferential(action, when);
}
}