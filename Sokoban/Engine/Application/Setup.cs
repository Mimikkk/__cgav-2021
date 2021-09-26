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
      Gl.Enable(GLEnum.Samples, 4);
      Gl.Enable(EnableCap.DepthTest);
      Gl.Enable(EnableCap.Blend);
      Gl.Enable(EnableCap.TextureCubeMapSeamless);
      Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
      Gl.Enable(EnableCap.Multisample);
      Gl.Enable(EnableCap.LineSmooth);
      Gl.Enable(EnableCap.InterlaceSgix);
      Gl.Enable(EnableCap.StencilTest);
      Gl.Enable(EnableCap.TextureCubeMapSeamless);
      Gl.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
      Gl.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
    }
  }

  private static Action<double> HandleOnDifferential(Action<double> action, Func<bool>? shouldFire = null) =>
    (shouldFire is null).Or(action, dt => shouldFire!().Then(action, dt));

  private static Action<double> HandleOnDifferential(Action action, Func<bool>? shouldFire = null) =>
    HandleOnDifferential(_ => action(), shouldFire);
}
}