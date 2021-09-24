using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.Map;
using Sokoban.Utilities.Extensions;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => HeightScale = (float)(HeightScale + dt));
    Controller.OnHold(Key.G, dt => HeightScale = (float)(HeightScale - dt));
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
    void RenderQuad(Obstacle obstacle)
    {
      Quad.Transform = obstacle.Transform;
      Quad.Draw(() => Quad.Spo!.SetUniform("height_scale", HeightScale));
    }

    Map.Obstacles.ForEach(RenderQuad);
  }

  private static float HeightScale = 1f;

  private static readonly Material Rock = new() {
    DiffuseMap = new("Rock/Color.png"),
    NormalMap = new("Rock/Normal.png"),
    DisplacementMap = new("Rock/Displacement.png")
  };
  private static readonly Material Fabric = new() {
    DiffuseMap = new("Fabric/Color.png"),
    NormalMap = new("Fabric/Normal.png"),
    DisplacementMap = new("Fabric/Displacement.png")
  };
  private static readonly Material Brick = new() {
    DiffuseMap = new("Brick/Color.jpg"),
    NormalMap = new("Brick/Normal.jpg"),
    DisplacementMap = new("Brick/Displacement.jpg")
  };

  private static readonly Quad Quad = new(Fabric);
  private static readonly GameMap Map = new() {
    Layout = new[,] {
      { ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Wall },
      { ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Empty },
      { ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Empty, ObstacleType.Wall },
      { ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Wall },
      { ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Wall, ObstacleType.Empty, ObstacleType.Wall, ObstacleType.Wall }
    }
  };
}
}