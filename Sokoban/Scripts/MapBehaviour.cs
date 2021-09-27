using System.Collections.Generic;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;
using Sokoban.Scripts.Map.Object;
using Sokoban.Utilities.Extensions;
using static Sokoban.Resources.ResourceManager.Materials;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  protected override void Start()
  {
    Quad.Spo = ResourceManager.ShaderPrograms.Pbr;
    
    Controller.OnHold(Key.T, dt => HeightScale += dt);
    Controller.OnHold(Key.G, dt => HeightScale -= dt);
    Controller.OnClick(MouseButton.Middle, _ => {
      if (Quad.Mesh!.Material!.Equals(Brick))
      {
        Quad.Mesh.Material = Rock;
      } else if (Quad.Mesh!.Material!.Equals(Rock))
      {
        Quad.Mesh.Material = Fabric;
      } else if (Quad.Mesh!.Material!.Equals(Fabric))
      {
        Quad.Mesh.Material = Brick;
      }
    });
  }
  protected override void Render(double dt)
  {
    void RenderQuad(Wall obstacle)
    {
      Quad.Transform = obstacle.Transform;
      Quad.Mesh!.Material = obstacle.Material;
      HeightScale.LogLine();
      Quad.Draw(() => {
        Quad.Mesh!.Material!.DiffuseMap!.Bind(0);
        Quad.Mesh!.Material!.NormalMap!.Bind(1);
        Quad.Mesh!.Material!.ReflectionMap!.Bind(2);
        Quad.Mesh!.Material!.HeightMap!.Bind(3);
        Quad.Mesh!.Material!.AmbientOcclusionMap!.Bind(4);

        Quad.Mesh!.Material!.DisplacementMap!.Bind(5);

        ResourceManager.Textures.Irradiance.Bind(6);
        ResourceManager.Textures.Prefilter.Bind(7);
        ResourceManager.Textures.BrdfLUT.Bind(8);

        Quad.Spo!.SetUniform("albedo_map", 0);
        Quad.Spo.SetUniform("normal_map", 1);
        Quad.Spo.SetUniform("metallic_map", 2);
        Quad.Spo.SetUniform("roughness_map", 3);
        Quad.Spo.SetUniform("ambient_occlusion_map", 4);
        Quad.Spo.SetUniform("ambient_occlusion_map", 5);

        Quad.Spo.SetUniform("irradiance_map", 6);
        Quad.Spo.SetUniform("prefilter_map", 7);
        Quad.Spo.SetUniform("brdf_LUT_map", 8);

        Quad.Spo.SetUniform("height_scale", HeightScale);

        Quad.Spo!.SetUniform("lights[0].position", Camera.Transform.Position);
        Quad.Spo!.SetUniform("lights[0].color", new Vector3D<float>(6, 5, 5.5f));

        for (var i = 1; i < Lights.Count + 1; ++i)
        {
          Quad.Spo!.SetUniform($"lights[{i}].position", Lights[i - 1].Transform.Position);
          Quad.Spo!.SetUniform($"lights[{i}].color", Lights[i - 1].Color.AsVector3D());
        }

        Quad.Spo!.SetUniform("model", Quad.Transform.View);
        Quad.Spo!.SetUniform("height_scale", HeightScale);
      });
    }

    Map.Walls.ForEach(RenderQuad);
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

  private static float HeightScale = 1f;

  private static readonly Quad Quad = new(Fabric);
  private static readonly GameMap Map = new() {
    Layout = new[,] {
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall, SpaceType.Wall }
    }
  };
}
}