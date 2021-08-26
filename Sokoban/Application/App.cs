using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Controllers;
using static Silk.NET.Windowing.Window;

namespace Sokoban.Application
{
public static partial class App
{
  public static void Run() => Window.Run();

  public static GL Gl { get; set; } = null!;

  static App()
  {
    Window = Create(Options);
    OnLoad(() => Controller.Setup(Window));
    OnLoad(Setup.Run);
    OnRender(Clear);
  }

  public static readonly WindowOptions Options = new() {
    Title = "Grafika Komputerowa i Wizualizacja - Sokoban",
    Size = new Vector2D<int>(800, 700),
    API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default,
      new APIVersion(4, 5)),
    FramesPerSecond = 60,
    UpdatesPerSecond = 100,
    PreferredDepthBufferBits = 24,
    PreferredStencilBufferBits = 24,
    ShouldSwapAutomatically = true
  };
  private const ClearBufferMask ClearMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.CoverageBufferBitNV;
  public static IWindow Window;
}
}