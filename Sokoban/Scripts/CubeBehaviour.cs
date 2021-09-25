using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;

namespace Sokoban.Scripts
{
public class CubeBehaviour : MonoBehaviour
{
  private static readonly Cube Cube = new(ResourceManager.Materials.Brick);
  private static readonly GameObject Box = ObjectLoader.Load("Cube").First();

  protected override void Start()
  {
    Box.Transform.Position = new(6, 3, 6);
    Box.Spo = ResourceManager.ShaderPrograms.ParallaxMapping;
    Box.Mesh!.Material = ResourceManager.Materials.RustedIron;
    Box.Mesh!.Material.DiffuseMap = new("Felix.png");
    
    Controller.OnHold(Key.T, dt => Cube.Transform.Scale += dt);
    Controller.OnHold(Key.G, dt => Cube.Transform.Scale -= dt);

    Controller.OnHold(Key.Keypad4, dt => Cube.Transform.Translate(dt, 0, 0));
    Controller.OnHold(Key.Keypad6, dt => Cube.Transform.Translate(-dt, 0, 0));
    Controller.OnHold(Key.Keypad9, dt => Cube.Transform.Translate(0, dt, 0));
    Controller.OnHold(Key.Keypad7, dt => Cube.Transform.Translate(0, -dt, 0));
    Controller.OnHold(Key.Keypad8, dt => Cube.Transform.Translate(0, 0, dt));
    Controller.OnHold(Key.Keypad2, dt => Cube.Transform.Translate(0, 0, -dt));

    Controller.OnHold(Key.KeypadMultiply, dt => Cube.Transform.Rotate(dt, 0, 0));
    Controller.OnHold(Key.KeypadDivide, dt => Cube.Transform.Rotate(-dt, 0, 0));

    Controller.OnHold(Key.KeypadAdd, dt => Cube.Transform.Rotate(0, dt, 0));
    Controller.OnHold(Key.KeypadSubtract, dt => Cube.Transform.Rotate(0, -dt, 0));

    Controller.OnHold(Key.Keypad1, dt => Cube.Transform.Rotate(0, 0, dt));
    Controller.OnHold(Key.Keypad3, dt => Cube.Transform.Rotate(0, 0, -dt));
  }
  protected override void Render(double dt)
  {
    Cube.Draw();
    Box.Draw(() => {
      Box.Mesh!.Material!.DiffuseMap?.Bind(0);
      Box.Mesh.Material.NormalMap?.Bind(1);
      Box.Mesh.Material.HeightMap?.Bind(2);

      Box.Spo!.SetUniform("diffuse_map", 0);
      Box.Spo.SetUniform("normal_map", 1);
      Box.Spo.SetUniform("displacement_map", 2);

      Box.Spo.SetUniform("height_scale",  Box.Transform.Scale);
      Box.Spo.SetUniform("light_position", Camera.Transform.Position);
      Box.Spo.SetUniform("is_discardable", false);
      Box.Spo.SetUniform("model", Box.Transform.View);
    });
  }
}
}