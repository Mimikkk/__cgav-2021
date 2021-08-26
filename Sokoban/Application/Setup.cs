using System;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Application
{
public static partial class App
{
  public static void OnLoad(Action action) => Window.Load += action;
  public static void OnClose(Action action) => Window.Closing += action;

  private static class Setup
  {
    public static void Run()
    {
      Window.Center();
      SetupOpenGl();
    }
    private static void SetupOpenGl() => Gl = GL.GetApi(Window);
  }

  private static Action<double> HandleOnDifferential(Action<double> action, Func<bool>? shouldFire = null) =>
    (shouldFire is null) switch {
      true  => action,
      false => dt => shouldFire!().Then(action, dt)
    };

  private static Action<double> HandleOnDifferential(Action action, Func<bool>? shouldFire = null) =>
    (shouldFire is null) switch {
      true  => _ => action(),
      false => _ => shouldFire!().Then(action)
    };
}
}