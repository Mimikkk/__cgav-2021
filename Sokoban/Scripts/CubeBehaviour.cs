using Silk.NET.Input;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;

namespace Sokoban.Scripts
{
public class CubeBehaviour : MonoBehaviour
{
  private static readonly Cube Cube = new(ResourceManager.Materials.Brick);

  protected override void Start()
  {
    Cube.Transform.Position = new (6,3,6);
    Controller.OnHold(Key.T, dt => Cube.Transform.Scale += dt);
    Controller.OnHold(Key.G, dt => Cube.Transform.Scale -= dt);

    Controller.OnHold(Key.Keypad4, dt => Cube.Transform.Translate(dt,0,0));
    Controller.OnHold(Key.Keypad6, dt => Cube.Transform.Translate(-dt,0,0));
    Controller.OnHold(Key.Keypad9, dt => Cube.Transform.Translate(0,dt,0));
    Controller.OnHold(Key.Keypad7, dt => Cube.Transform.Translate(0,-dt,0));
    Controller.OnHold(Key.Keypad8, dt => Cube.Transform.Translate(0,0,dt));
    Controller.OnHold(Key.Keypad2, dt => Cube.Transform.Translate(0,0,-dt));

    Controller.OnHold(Key.KeypadMultiply, dt => Cube.Transform.Rotate(dt, 0, 0));
    Controller.OnHold(Key.KeypadDivide, dt => Cube.Transform.Rotate(-dt, 0, 0));

    Controller.OnHold(Key.KeypadAdd, dt => Cube.Transform.Rotate(0, dt, 0));
    Controller.OnHold(Key.KeypadSubtract, dt => Cube.Transform.Rotate(0, -dt, 0));

    Controller.OnHold(Key.Keypad1, dt => Cube.Transform.Rotate(0, 0, dt));
    Controller.OnHold(Key.Keypad3, dt => Cube.Transform.Rotate(0, 0, -dt));
  }
  protected override void Render(double dt) => Cube.Draw();
}
}