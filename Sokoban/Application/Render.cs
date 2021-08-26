using System;

namespace Sokoban.Application
{
public static partial class App
{
  public static void OnRender(Action<double> action, Func<bool>? when = null) => 
    Window.Render += HandleOnDifferential(action, when);

  public static void OnRender(Action action, Func<bool>? when = null) => 
    Window.Render += HandleOnDifferential(action, when);

  private static void Clear() => Gl.Clear(ClearMask);   
}
}