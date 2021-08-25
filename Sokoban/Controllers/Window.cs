using System;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Controllers
{
public static partial class Controller
{
  public static void OnLoad(Action action) => Window.Load += action;

  public static void OnRender(Action<double> action, Func<bool>? when = null) =>
    Window.Render += HandleOnDifferential(action, when);

  public static void OnUpdate(Action<double> action, Func<bool>? when = null) =>
    Window.Update += HandleOnDifferential(action, when);

  private static Action<double> HandleOnDifferential(Action<double> action, Func<bool>? when = null) =>
    (when is null) switch {
      true  => action,
      false => dt => when!().Then(action, dt)
    };
}
}