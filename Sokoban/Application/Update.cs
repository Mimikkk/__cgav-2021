using System;

namespace Sokoban.Application
{
public static partial class App
{
  public static void OnUpdate(Action<float> action, Func<bool>? when = null) =>
    Window.Update += HandleOnDifferential(action, when);
}
}