using System;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Sokoban.Controllers;
using Sokoban.Utilities.Extensions;
using static Silk.NET.Windowing.Window;

namespace Sokoban
{
public static class Application
{
  public static void Run() => Window.Run();
  public static GL Gl { get; private set; } = null!;

  static Application()
  {
    Window = Create(Options);
    Controller.OnLoad(Setup);
    Controller.OnLoad(SetupController);
  }

  private static void Setup()
  {
    Window.Center();
    SetupOpenGl();
    Controller.Setup(Window);
  }
  private static void SetupOpenGl() => Gl = GL.GetApi(Window);
  
  private static void SetupController() => Controller.OnHold(Key.Escape, Window.Close);
  
  private static readonly WindowOptions Options = new() {
    API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new APIVersion(new Version(4, 6, 0))),
    Title = "Grafika Komputerowa i Wizualizacja - Sokoban",
    PreferredBitDepth = new Vector4D<int>(24, 24, 24, 24),
    Size = new Vector2D<int>(800, 700),
    Position = new Vector2D<int>(0, 0),
    WindowBorder = WindowBorder.Fixed,
    WindowState = WindowState.Normal,
    PreferredStencilBufferBits = 24,
    ShouldSwapAutomatically = true,
    PreferredDepthBufferBits = 24,
    TransparentFramebuffer = false,
    VideoMode = new VideoMode(60),
    UpdatesPerSecond = 60,
    FramesPerSecond = 60,
    IsEventDriven = true,
    IsVisible = true,
    VSync = true
  };
  public static readonly IWindow Window;
}
}