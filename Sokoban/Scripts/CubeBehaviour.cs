using System.Linq;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;
using Sokoban.Scripts.Map.Object;

namespace Sokoban.Scripts
{
public partial class CubeBehaviour : MonoBehaviour
{
  private static readonly GameObject Box = ObjectLoader.Load("Box").First();

  private static void SetupController()
  {
    Controller.OnHold(Key.T, dt => Box.Transform.Scale += dt);
    Controller.OnHold(Key.G, dt => Box.Transform.Scale -= dt);

    Controller.OnHold(Key.Keypad4, dt => Box.Transform.Translate(dt, 0, 0));
    Controller.OnHold(Key.Keypad6, dt => Box.Transform.Translate(-dt, 0, 0));
    Controller.OnHold(Key.Keypad9, dt => Box.Transform.Translate(0, dt, 0));
    Controller.OnHold(Key.Keypad7, dt => Box.Transform.Translate(0, -dt, 0));
    Controller.OnHold(Key.Keypad8, dt => Box.Transform.Translate(0, 0, dt));
    Controller.OnHold(Key.Keypad2, dt => Box.Transform.Translate(0, 0, -dt));

    Controller.OnHold(Key.KeypadMultiply, dt => Box.Transform.Rotate(dt, 0, 0));
    Controller.OnHold(Key.KeypadDivide, dt => Box.Transform.Rotate(-dt, 0, 0));

    Controller.OnHold(Key.KeypadAdd, dt => Box.Transform.Rotate(0, dt, 0));
    Controller.OnHold(Key.KeypadSubtract, dt => Box.Transform.Rotate(0, -dt, 0));

    Controller.OnHold(Key.Keypad1, dt => Box.Transform.Rotate(0, 0, dt));
    Controller.OnHold(Key.Keypad3, dt => Box.Transform.Rotate(0, 0, -dt));
  }
  private static void SetupCube()
  {
    Box.Transform.Position = new(6, 3, 6);
    Box.Spo = ResourceManager.ShaderPrograms.Pbr;
    Box.Mesh!.Material = ResourceManager.Materials.Plastic;
    Box.Mesh!.Material.DiffuseMap = new("Felix.png");
  }

  protected override void Start()
  {
    SetupCube();
    SetupController();
  }
  protected override void Render(double dt)
  {
    Box.Draw(() => {
      Box.Mesh!.Material!.DiffuseMap!.Bind(0);
      Box.Mesh!.Material!.NormalMap!.Bind(1);
      Box.Mesh!.Material!.ReflectionMap!.Bind(2);
      Box.Mesh!.Material!.HeightMap!.Bind(3);
      Box.Mesh!.Material!.AmbientOcclusionMap!.Bind(4);
      ResourceManager.Textures.Irradiance.Bind(5);
      ResourceManager.Textures.Prefilter.Bind(6);
      ResourceManager.Textures.BrdfLUT.Bind(7);

      Box.Spo!.SetUniform("albedo_map", 0);
      Box.Spo.SetUniform("normal_map", 1);
      Box.Spo.SetUniform("metallic_map", 2);
      Box.Spo.SetUniform("roughness_map", 3);
      Box.Spo.SetUniform("ambient_occlusion_map", 4);

      Box.Spo.SetUniform("irradiance_map", 5);
      Box.Spo.SetUniform("prefilter_map", 6);
      Box.Spo.SetUniform("brdf_LUT_map", 7);

      Box.Spo!.SetUniform("light_positions[0]", Camera.Transform.Position);
      Box.Spo!.SetUniform("light_positions[1]", new Vector3D<float>(6, 5, 5.5f));
      Box.Spo!.SetUniform("light_colors[0]", new Vector3D<float>(100, 100, 100));
      Box.Spo!.SetUniform("light_colors[1]", new Vector3D<float>(0, 0, 100));

      Box.Spo!.SetUniform("model", Box.Transform.View);
    });
  }
}
}