using System;
using System.Collections.Generic;
using Silk.NET.Input;
using Silk.NET.Windowing;
using Sokoban.Application;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  private static void Update(double dt) => OnHoldActions.ForEach(action => action(dt));

  public static void Setup(IWindow window)
  {
    InputContext = window.CreateInput();

    (InputContext.Keyboards.Count > 0).Then(() => Keyboard = InputContext.Keyboards[0]);
    (InputContext.Mice.Count > 0).Then(() => Mouse = InputContext.Mice[0]);
    Mouse.Cursor.CursorMode = CursorMode.Raw;

    App.OnUpdate(Update);
  }

  private static IMouse Mouse { get; set; } = null!;
  private static IKeyboard Keyboard { get; set; } = null!;
  private static IInputContext InputContext { get; set; } = null!;
  private static readonly List<Action<double>> OnHoldActions = new();
}
}