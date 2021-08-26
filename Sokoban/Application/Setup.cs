using System;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Controllers;
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

    private static void SetupOpenGl()
    {
      Gl = GL.GetApi(Window);
      // Gl.Enable(EnableCap.DepthTest);
      // Gl.Enable(EnableCap.Blend);
      // Gl.Enable(EnableCap.TextureCubeMapSeamless);
      // Gl.Enable(EnableCap.CullFace);
      // Gl.Enable(EnableCap.Multisample);
      // Gl.Enable(EnableCap.LineSmooth);
      // Gl.Enable(EnableCap.MultisampleSgis);
      // Gl.Enable(EnableCap.MinmaxExt);
      // Gl.Enable(EnableCap.PolygonSmooth);
      // Gl.Enable(EnableCap.SampleShading);
      // Gl.Hint(HintTarget.MultisampleFilterHintNV, HintMode.Nicest);
      // Gl.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
      // Gl.Hint(HintTarget.LineQualityHintSgix, HintMode.Nicest);
      // Gl.Hint(HintTarget.WideLineHintPgi, HintMode.Nicest);
      // Gl.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
      // Gl.Hint(HintTarget.AlwaysSoftHintPgi, HintMode.Nicest);
    }
  }

  private static Action<double> HandleOnDifferential(Action<float> action, Func<bool>? shouldFire = null) =>
    (shouldFire is null) switch {
      true  => dt => action((float)dt),
      false => dt => shouldFire!().Then(action, (float)dt)
    };
}
}