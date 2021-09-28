using System.Collections.Generic;
using System.Linq;
using Silk.NET.Input;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map.Object;

namespace Sokoban.Scripts.Map
{
public class IcoBehaviour : MonoBehaviour
{
  private static readonly List<GameObject> Gos = ObjectLoader.Load("BoxStack").ToList();

  private static float HeightScale;
  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => HeightScale += dt);
    Controller.OnHold(Key.G, dt => HeightScale -= dt);
    foreach (var go in Gos)
    {
      go.Mesh.Material = Resources.ResourceManager.Materials.Rock;
      go.Spo = ResourceManager.ShaderPrograms.ParallaxMapping;
      go.Transform.Position = new(1, 5, 3);
    }
  }

  protected override void Render(double dt)
  {
    foreach (var go in Gos)
    {
      ResourceManager.ShaderPrograms.ParallaxMapping.Bind();

      go.Mesh.Material.DiffuseMap?.Bind(0);
      go.Mesh.Material.NormalMap?.Bind(1);
      go.Mesh.Material.DisplacementMap?.Bind(2);

      go.Spo.SetUniform("diffuse_map", 0);
      go.Spo.SetUniform("normal_map", 1);
      go.Spo.SetUniform("displacement_map", 2);

      go.Spo.SetUniform("light_position", Camera.Transform.Position);
      go.Spo.SetUniform("height_scale", HeightScale);
      go.Spo.SetUniform("is_discardable", false);

      go.Spo!.SetUniform("model", go.Transform.View);
      go.Draw();
    }
  }
}
}