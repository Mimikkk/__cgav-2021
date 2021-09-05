using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Engine.Application
{
public static partial class App
{
  public static void OnLoad(Action action) => Window.Load += action;
  public static void OnClose(Action action) => Window.Closing += action;
  public static Vector2D<int> Size => Window.Size;

  private static class Setup
  {
    public static void Run()
    {
      Window.Center();
      SetupOpenGl();
    }
    private static void SetupOpenGl()
    {
      Gl = GL.GetApi(Window);
      Gl.Enable(EnableCap.DepthTest);
      Gl.Enable(EnableCap.Blend);
      Gl.Enable(EnableCap.TextureCubeMapSeamless);
    }
  }

  private static Action<double> HandleOnDifferential(Action<double> action, Func<bool>? shouldFire = null) =>
    (shouldFire is null).Or(action, dt => shouldFire!().Then(action, dt));

  private static Action<double> HandleOnDifferential(Action action, Func<bool>? shouldFire = null) =>
    HandleOnDifferential(_ => action(), shouldFire);
}
}