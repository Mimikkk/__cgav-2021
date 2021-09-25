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
  private static readonly GameObject Box = ObjectLoader.Load("Box").First();

  protected override void Start()
  {
    Box.Transform.Position = new(6, 3, 6);
    Box.Spo = ResourceManager.ShaderPrograms.Pbr;
    Box.Mesh.Material = ResourceManager.Materials.RustedIron;
    Box.Mesh!.Material!.DiffuseMap = new("Felix.png");
    Box.Mesh.Material.Log();
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
      Box.Mesh!.Material!.DiffuseMap!.Bind(0);
      Box.Mesh!.Material!.NormalMap!.Bind(1);
      Box.Mesh!.Material!.ReflectionMap!.Bind(2);
      Box.Mesh!.Material!.HeightMap!.Bind(3);
      Box.Mesh!.Material!.AmbientOcclusionMap!.Bind(4);

      Box.Spo.SetUniform("albedoMap" ,0);
      Box.Spo.SetUniform("normalMap" ,1);
      Box.Spo.SetUniform("metallicMap" ,2);
      Box.Spo.SetUniform("roughnessMap" ,3);
      Box.Spo.SetUniform("aoMap" ,4);

      Box.Spo!.SetUniform("lightPositions[0]", Camera.Transform.Position);
      Box.Spo!.SetUniform("lightColors[0]", new Vector3D<float>(255,255,150));

      Box.Spo!.SetUniform("projection", Camera.Projection);
      Box.Spo!.SetUniform("view", Camera.View);

      Box.Spo!.SetUniform("camPos", Camera.Transform.Position);

      Box.Spo!.SetUniform("model", Box.Transform.View);
    });
  }
}
}