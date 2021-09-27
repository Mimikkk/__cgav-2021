using System.Collections.Generic;
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

      Box.Spo!.SetUniform("lights[0].position", Camera.Transform.Position);
      Box.Spo!.SetUniform("lights[0].color", new Vector3D<float>(6, 5, 5.5f));

      for (var i = 1; i < Lights.Count + 1; ++i)
      {
        Box.Spo!.SetUniform($"lights[{i}].position", Lights[i-1].Transform.Position);
        Box.Spo!.SetUniform($"lights[{i}].color", Lights[i-1].Color.AsVector3D());
      }

      Box.Spo!.SetUniform("model", Box.Transform.View);
    });
  }

  private static readonly List<Light> Lights = new() {
    new() {
      Color = Color.Red * 0.4f,
      Transform = new() {
        Position = new(6, 8, 6),
      },
    },
    new() {
      Color = Color.Green * 1f,
      Transform = new() {
        Position = new(6, 6, 8),
      },
    },
  };
}

public record Color(float R, float G, float B)
{
  public static readonly Color BrightWhite = new(255, 255, 255);
  public static readonly Color DimWhite = BrightWhite * 0.2f;
  public static readonly Color Red = new(255, 0, 0);
  public static readonly Color Green = new(0, 255, 0);
  public static readonly Color Blue = new(0, 0, 255);


  public Color(Vector3D<float> vector)
    : this(vector.X, vector.Y, vector.Z) { }

  public Vector3D<float> AsVector3D() => new(R, G, B);
  public static Color operator *(Color color, float value) => new(color.AsVector3D() * value);
};

public class Light
{
  public Transform Transform { get; set; } = new();
  public Color Color { get; set; } = Color.BrightWhite;
}
}