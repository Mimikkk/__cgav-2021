using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.GameMap;
using Sokoban.Utilities.Extensions;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => Degrees = (float)(Degrees + 10 * dt));
    Controller.OnHold(Key.G, dt => Degrees = (float)(Degrees - 10 * dt));
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

  private enum Direction { Forward, Backward, Right, Left, Top, Bottom }
  private record Neighbour(Direction direction, Vector2D<int> coord)
  {
    public static IEnumerable<Neighbour> Grid(Vector2D<int> coord)
    {
      return Neighbours.Select(neighbour => neighbour with { coord = coord + neighbour.coord });
    }
    public bool IsSafe(Vector2D<int> dim) => coord.X >= 0 && coord.X < dim.X && coord.Y >= 0 && coord.Y < dim.Y;
    public bool IsAdjacent(int[,] gameMap) => gameMap[coord.X, coord.Y] == 1;

    private static readonly IReadOnlyList<Neighbour> Neighbours = new List<Neighbour> {
      new(Direction.Forward, new(0, 0)),
      new(Direction.Left, new(0, 0)),

      new(Direction.Bottom, new(0, 0)),

      new(Direction.Right, new(0, 0)),
      new(Direction.Backward, new(0, 0))
    };
  }
  private record Transform(Vector3D<float> offset, Vector3D<float> rotation);
  private static float Degrees = 0;

  private static IReadOnlyDictionary<Direction, Transform> TransformMap => new Dictionary<Direction, Transform> {
    {
      Direction.Top,
      new(new Vector3D<float>(0, 1, 0), new Vector3D<float>(0, -MathF.PI / 2, 0))
    }, {
      Direction.Bottom,
      new(new Vector3D<float>(0, -1, 0), new Vector3D<float>(0, MathF.PI / 2, 0))
    }, {
      Direction.Forward,
      new(new Vector3D<float>(-1, 0, 0), new Vector3D<float>(-MathF.PI / 2, 0, 0))
    }, {
      Direction.Backward,
      new(new Vector3D<float>(1, 0, 0), new Vector3D<float>(MathF.PI / 2, 0, 0))
    }, {
      Direction.Left,
      new(new Vector3D<float>(0, 0, 1), new Vector3D<float>(MathF.PI, 0, 0))
    }, {
      Direction.Right,
      new(new Vector3D<float>(0, 0, -1), new Vector3D<float>(0, 0, 0))
    }
  };

  protected override void Render(double dt)
  {
    $"{Degrees}".LogLine();
    var gameMap = new[,] {
      { 0, 0, 1, 1, 1 },
      { 0, 0, 1, 1, 1 },
      { 0, 0, 1, 1, 1 },
    };
    var dim = new Vector2D<int>(gameMap.GetLength(0), gameMap.GetLength(1));
    var (n, m) = (dim.X, dim.Y);
    for (var i = 0; i < dim.X; ++i)
    {
      for (var j = 0; j < dim.Y; ++j)
      {
        if (gameMap[i, j] != 1) continue;

        IReadOnlyList<Direction> neighbours = Neighbour.Grid(new(i, j))
          .Where(n => n.IsSafe(dim) && n.IsAdjacent(gameMap))
          .Select(n => n.direction)
          .ToList();

        neighbours.ForEach((direction) => {
          var (offset, rotation) = TransformMap[direction];

          var position = new Vector3D<float>(2 * i, 0, 2 * j) + offset;

          var orientation = Quaternion<float>.Identity
                            * Quaternion<float>.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);

          var model = Matrix4X4<float>.Identity
                      * Matrix4X4.CreateFromQuaternion(Quaternion<float>.Conjugate(orientation))
                      * Matrix4X4.CreateTranslation(position);

          Quad.Draw(() => {
            Quad.Spo!.SetUniform("model", model);
            Quad.Spo!.SetUniform("height_scale", HeightScale);
          });
        });

      }
    }
  }

  private static float HeightScale = 1f;

  private static readonly Material Rock = new() {
    DiffuseMap = new("Rock/Color.png"),
    NormalMap = new("Rock/Normal.png"),
    DisplacementMap = new("Rock/Displacement.png"),
  };

  private static readonly Material Fabric = new() {
    DiffuseMap = new("Fabric/Color.png"),
    NormalMap = new("Fabric/Normal.png"),
    DisplacementMap = new("Fabric/Displacement.png"),
  };

  private static readonly Material Brick = new() {
    DiffuseMap = new("Brick/Color.jpg"),
    NormalMap = new("Brick/Normal.jpg"),
    DisplacementMap = new("Brick/Displacement.jpg"),
  };

  private static readonly Quad Quad = new(Brick);
}
}