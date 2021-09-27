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
  protected override void Render(double dt)
  {
    void RenderQuad(Wall obstacle)
    {
      Quad.Transform = obstacle.Transform;
      Quad.Mesh!.Material = obstacle.Material;

      ResourceManager.ShaderPrograms.PbrShaderConfiguration(Quad.Mesh.Material, Quad.Transform);
      Quad.DrawRaw();
    }
    Map.Walls.ForEach(RenderQuad);
  }

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