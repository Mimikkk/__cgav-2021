using System;
using System.Collections.Generic;
using Silk.NET.Input;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  public static void OnPush(Key key, Action what, Func<bool>? when = null) =>
    Keyboard.KeyDown += HandleStateChange(key, what, when);

  public static void OnRelease(Key key, Action what, Func<bool>? when = null) =>
    Keyboard.KeyUp += HandleStateChange(key, what, when);

  public static void OnHold(Key key, Action<float> action, Func<bool>? when = null) =>
    OnHoldActions.Add(HandleState(key, action, when));

  private static Action<IKeyboard, Key, int> HandleStateChange(Key wanted, Action action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<IKeyboard, Key, int>>(
      (_, stroked, _) => (stroked == wanted).Then(action),
      (_, stroked, _) => (predicate!() && stroked == wanted).Then(action)
    );

  private static Action<float> HandleState(Key wanted, Action<float> action, Func<bool>? predicate = null) =>
    (predicate is null).Or<Action<float>>(
      (dt) => Keyboard.IsKeyPressed(wanted).Then(action, dt),
      (dt) => (predicate!() && Keyboard.IsKeyPressed(wanted)).Then(action, dt)
    );
}
}