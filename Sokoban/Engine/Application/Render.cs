using System;
using Silk.NET.OpenGL;
using Silk.NET.SDL;

namespace Sokoban.Engine.Application
{
public static partial class App
{
  public static void OnRender(Action<double> action, Func<bool>? when = null) =>
    Window.Render += HandleOnDifferential(action, when);

  public static void OnRender(Action action, Func<bool>? when = null) =>
    Window.Render += HandleOnDifferential(action, when);

  private static void Clear() => Gl.Clear(ClearMask);

  public static void SetDrawMode(PolygonMode mode) => Gl.PolygonMode(MaterialFace.FrontAndBack, mode);
  public static void SetClearColor(Color color) => Gl.ClearColor(color.R, color.G, color.B, color.A);
}
}