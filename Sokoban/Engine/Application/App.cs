using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Engine.Controllers;
using static Silk.NET.Windowing.Window;

namespace Sokoban.Engine.Application
{
public static partial class App
{
  public static void Run() => Window.Run();
  public static void Close() => Window.Close();

  public static GL Gl { get; set; } = null!;

  static App()
  {
    Window = Create(Options);
    OnLoad(() => Controller.Setup(Window));
    OnLoad(Setup.Run);
    OnRender(Clear);
  }

  private static readonly WindowOptions Options = new() {
    Title = "Grafika Komputerowa i Wizualizacja - Sokoban",
    Size = new Vector2D<int>(800, 700),
    API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default,
      new APIVersion(4, 6)),
    FramesPerSecond = 60,
    UpdatesPerSecond = 100,
    PreferredBitDepth = new(24),
    PreferredDepthBufferBits = 24,
    PreferredStencilBufferBits = 24,
    ShouldSwapAutomatically = true
  };
  private const ClearBufferMask ClearMask = ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit;
  private static readonly IWindow Window;
}
}