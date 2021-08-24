using System;
using Silk.NET.Input;
using Sokoban;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  public static void OnKeyDown(Key key, Action what, Func<bool>? when = null) =>
    Application.Keyboard.KeyDown += HandleKeyState(key, what, when);

  public static void OnKeyUp(Key key, Action what, Func<bool>? when = null) =>
    Application.Keyboard.KeyUp += HandleKeyState(key, what, when);

  private static Action<IKeyboard, Key, int> HandleKeyState(Key wanted, Action action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IKeyboard, Key, int>>(
      (_, stroked, _) => (stroked == wanted).Then(action),
      (_, stroked, _) => (predicate!() && stroked == wanted).Then(action)
    );
}
}