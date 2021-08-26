using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Engine.Controllers
{
public static partial class Controller
{
  public static void OnClick(MouseButton button, Action<Vector2D<float>> action, Func<bool>? when = null) =>
    Mouse.Click += HandleClick(button, action, when);

  public static void OnDoubleClick(MouseButton button, Action<Vector2D<float>> action, Func<bool>? when = null) =>
    Mouse.DoubleClick += HandleClick(button, action, when);

  public static void OnScroll(Action<Vector2D<float>> action, Func<bool>? when = null) =>
    Mouse.Scroll += HandleScroll(action, when);

  public static void OnPush(MouseButton button, Action action, Func<bool>? when = null) =>
    Mouse.MouseDown += HandleStateChange(button, action, when);

  public static void OnRelease(MouseButton button, Action action, Func<bool>? when = null) =>
    Mouse.MouseUp += HandleStateChange(button, action, when);

  public static void OnHold(MouseButton button, Action<double> action, Func<bool>? when = null) =>
    OnHoldActions.Add(HandleState(button, action, when));

  public static void OnMove(Action<Vector2D<float>> action, Func<bool>? when = null) =>
    Mouse.MouseMove += HandleMove(action, when);


  private static Action<IMouse, Vector2> HandleMove(Action<Vector2D<float>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, Vector2>>(
      (_, position) => action(position.ToVector2D()),
      (_, position) => predicate!().Then(action, position.ToVector2D())
    );

  private static Action<IMouse, MouseButton> HandleStateChange(MouseButton wanted, Action action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, MouseButton>>(
      (_, stroked) => (stroked == wanted).Then(action),
      (_, stroked) => (predicate!() && stroked == wanted).Then(action)
    );

  private static Action<IMouse, ScrollWheel> HandleScroll(Action<Vector2D<float>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, ScrollWheel>>(
      (_, wheel) => action(wheel.ToVector2D()),
      (_, wheel) => predicate!().Then(action, wheel.ToVector2D())
    );

  private static Action<IMouse, MouseButton, Vector2> HandleClick(MouseButton wanted, Action<Vector2D<float>> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IMouse, MouseButton, Vector2>>(
      (_, stroked, position) => (stroked == wanted).Then(action, position.ToVector2D()),
      (_, stroked, position) => (predicate!() && stroked == wanted).Then(action, position.ToVector2D())
    );
  
  private static Action<double> HandleState(MouseButton wanted, Action<double> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<double>>(
      dt => Mouse.IsButtonPressed(wanted).Then(action, dt),
      dt => (predicate!() && Mouse.IsButtonPressed(wanted)).Then(action, dt)
    );
}
}