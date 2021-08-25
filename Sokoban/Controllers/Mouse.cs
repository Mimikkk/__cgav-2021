using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  public static void OnClick(MouseButton button, Action<Vector2D<double>> action, Func<bool>? when = null) =>
    Mouse.Click += HandleOnClick(button, action, when);

  public static void OnDoubleClick(MouseButton button, Action<Vector2D<double>> action, Func<bool>? when = null) =>
    Mouse.DoubleClick += HandleOnClick(button, action, when);

  public static void OnScroll(Action<Vector2D<double>> action, Func<bool>? when = null) =>
    Mouse.Scroll += HandleOnScroll(action, when);

  public static void OnHold(MouseButton button, Action action, Func<bool>? when = null) =>
    Mouse.MouseDown += HandleMouseState(button, action, when);

  public static void OnRelease(MouseButton button, Action action, Func<bool>? when = null) =>
    Mouse.MouseUp += HandleMouseState(button, action, when);

  public static void OnMove(Action<Vector2D<double>> action, Func<bool>? when = null) =>
    Mouse.MouseMove += HandleMouseMove(action, when);


  private static Action<IMouse, Vector2> HandleMouseMove(Action<Vector2D<double>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, Vector2>>(
      (_, position) => action(position.ToVector2D()),
      (_, position) => predicate!().Then(action, position.ToVector2D())
    );

  private static Action<IMouse, MouseButton> HandleMouseState(MouseButton wanted, Action action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, MouseButton>>(
      (_, stroked) => (stroked == wanted).Then(action),
      (_, stroked) => (predicate!() && stroked == wanted).Then(action)
    );

  private static Action<IMouse, ScrollWheel> HandleOnScroll(Action<Vector2D<double>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, ScrollWheel>>(
      (_, wheel) => action(wheel.ToVector2D()),
      (_, wheel) => predicate!().Then(action, wheel.ToVector2D())
    );

  private static Action<IMouse, MouseButton, Vector2> HandleOnClick(MouseButton wanted, Action<Vector2D<double>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, MouseButton, Vector2>>(
      (_, stroked, position) => (stroked == wanted).Then(action, position.ToVector2D()),
      (_, stroked, position) => (predicate!() && stroked == wanted).Then(action, position.ToVector2D())
    );
}
}