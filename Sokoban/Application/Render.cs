using System;

namespace Sokoban.Application
{
public static partial class App
{
  public static void OnRender(Action<float> action, Func<bool>? when = null) => 
    Window.Render += HandleOnDifferential(action, when);
}
}