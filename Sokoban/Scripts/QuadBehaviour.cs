using System.Collections.Generic;
using System.Linq;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.GameMap;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;

namespace Sokoban.Scripts
{
public class QuadBehaviour : MonoBehaviour
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
      new(Direction.Forward, -Vector2D<int>.UnitX),
      new(Direction.Left, -Vector2D<int>.UnitY),
      new(Direction.Bottom, Vector2D<int>.Zero),
      new(Direction.Right, Vector2D<int>.UnitY),
      new(Direction.Backward, Vector2D<int>.UnitX)
    };
  }

  private static readonly IReadOnlyDictionary<Direction, Matrix4X4<float>> TransformMap = new Dictionary<Direction, Matrix4X4<float>> {
    {
      Direction.Forward,
      Matrix4X4.CreateTranslation(-Vector3D<float>.UnitX - Vector3D<float>.UnitZ) * Matrix4X4.CreateRotationY(Scalar.DegreesToRadians(90f))
    }, {
      Direction.Backward,
      Matrix4X4.CreateTranslation(Vector3D<float>.UnitX - Vector3D<float>.UnitZ) * Matrix4X4.CreateRotationY(Scalar.DegreesToRadians(270f))
    }, {
      Direction.Right,
      Matrix4X4.CreateTranslation(Vector3D<float>.Zero) * Matrix4X4.CreateRotationX(Scalar.DegreesToRadians(0f))
    }, {
      Direction.Top,
      Matrix4X4.CreateTranslation(Vector3D<float>.UnitY - Vector3D<float>.UnitZ) * Matrix4X4.CreateRotationX(Scalar.DegreesToRadians(90f))
    }, {
      Direction.Bottom,
      Matrix4X4.CreateTranslation(-Vector3D<float>.UnitY - Vector3D<float>.UnitZ) * Matrix4X4.CreateRotationX(Scalar.DegreesToRadians(270f))
    }, {
      Direction.Left,
      Matrix4X4.CreateTranslation(-2 * Vector3D<float>.UnitZ) * Matrix4X4.CreateRotationX(Scalar.DegreesToRadians(180f))
    },
  };

  protected override void Render(double dt)
  {
    var gameMap = new[,] {
      { 0, 1, 1, 1, 0, 1 },
      { 1, 1, 1, 1, 1, 1 },
      { 1, 1, 1, 0, 1, 1 },
      { 1, 1, 1, 0, 1, 1 },
      { 1, 1, 1, 1, 1, 1 },
      { 1, 1, 1, 1, 1, 1 },
    };
    var dim = new Vector2D<int>(gameMap.GetLength(0), gameMap.GetLength(1));
    var (n, m) = (dim.X, dim.Y);
    for (var i = 0; i < dim.X; ++i)
    {
      for (var j = 0; j < dim.Y; ++j)
      {
        var model = Matrix4X4.CreateTranslation(new Vector3D<float>(2 * i, 2 * j, 0));

        // IReadOnlyList<Direction> neighbours = Neighbour.Grid(new(i, j))
        // .Where(n => n.IsSafe(dim) && n.IsAdjacent(gameMap)).Select(n=>n.direction)
        // .ToList();

        if (gameMap[i, j] == 1)
        {
          Quad.Draw(() => {
            Quad.Spo!.SetUniform("height_scale", HeightScale);
            Quad.Spo.SetUniform("model", model * TransformMap[Direction.Bottom]);
          });
        }
      }
    }
  }

  protected override void Update(double dt)
  {
    HeightScale += (float)dt;
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