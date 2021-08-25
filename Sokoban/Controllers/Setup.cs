using Silk.NET.Input;
using Silk.NET.Windowing;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  public static void Setup(IWindow window)
  {
    Window = window;
    InputContext = Window.CreateInput();
    (InputContext.Keyboards.Count > 0).Then(() => Keyboard = InputContext.Keyboards[0]);
    (InputContext.Mice.Count > 0).Then(() => Mouse = InputContext.Mice[0]);
  }

  private static IMouse Mouse { get; set; } = null!;
  private static IWindow Window { get; set; } = null!;
  private static IKeyboard Keyboard { get; set; } = null!;
  private static IInputContext InputContext { get; set; } = null!;
}
}