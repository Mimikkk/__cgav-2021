using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives;
using static System.Linq.Enumerable;
using static Sokoban.Utilities.Extensions.Extension;

namespace Sokoban.Scripts.Map
{
public enum ObstacleType { Wall = 0, Empty = 1 }
public enum Direction { Forward, Backward, Right, Left, Top, Bottom }

public record Obstacle(ObstacleType Type, Transform Transform, Direction Direction);

public class GameMap
{
  private record Neighbour(Direction direction, Vector2D<int> coord)
  {
    public static IEnumerable<Neighbour> CloseTo(Vector2D<int> coord) =>
      Neighbours.Zip(Repeating(coord)).Select(OffsetBy);

    private static readonly IReadOnlyList<Neighbour> Neighbours = new List<Neighbour> {
      new(Direction.Forward, new(-1, 0)),
      new(Direction.Backward, new(1, 0)),

      new(Direction.Bottom, new(0, 0)),

      new(Direction.Left, new(0, 1)),
      new(Direction.Right, new(0, -1))
    };
    private static Neighbour OffsetBy((Neighbour neighbour, Vector2D<int> offset) pair)
    {
      var (neighbour, offset) = pair;
      return neighbour with { coord = neighbour.coord + offset };
    }
  }

  public ObstacleType[,] Layout {
    private get { return _layout; }
    set {
      _layout = value.Pad(2, 2);
      Recalculate();
    }
  }

  public List<Obstacle> Obstacles { get; private set; } = new();

  public int Height => Layout.GetLength(0);
  public int Width => Layout.GetLength(1);

  public ObstacleType this[int i, int j] => Layout[i, j];
  public ObstacleType this[Vector2D<int> dim] => this[dim.X, dim.Y];

  private void Recalculate()
  {
    IEnumerable<Obstacle> HandleWall(int i, int j)
    {
      bool ShouldShow(Neighbour neighbour) =>
        neighbour.direction == Direction.Bottom || !InBounds(neighbour.coord);

      return Neighbour.CloseTo(new(i, j))
        .Where(ShouldShow)
        .Zip(Repeating(new Vector2D<int>(i, j)))
        .Select(ToObstacle)
        .Select(obstacle => {
          if (obstacle.Direction == Direction.Bottom) obstacle.Transform.Position += 2 * Vector3D<float>.UnitY;
          else obstacle.Transform.Orientation = Quaternion<float>.Conjugate(obstacle.Transform.Orientation);
          return obstacle;
        })
        .ToList();
    }
    IEnumerable<Obstacle> HandleEmpty(int i, int j)
    {
      bool ShouldShow(Neighbour neighbour) =>
        neighbour.direction == Direction.Bottom || !InBounds(neighbour.coord) || !IsAdjacent(neighbour.coord);

      return Neighbour.CloseTo(new(i, j))
        .Where(ShouldShow)
        .Zip(Repeating(new Vector2D<int>(i, j)))
        .Select(ToObstacle);
    }

    Obstacles = Range(0, Height)
      .SelectMany(i => Range(0, Width)
        .SelectMany(j => this[i, j] switch {
          ObstacleType.Wall  => HandleWall(i, j),
          ObstacleType.Empty => HandleEmpty(i, j)
        }))
      .ToList();
  }

  private bool InBounds(Vector2D<int> coord) => InBounds(coord.X, coord.Y);
  private bool InBounds(int x, int y) => x >= 0 && x < Height && y >= 0 && y < Width;

  private Obstacle ToObstacle((Neighbour, Vector2D<int> position) pair)
  {
    var ((direction, _), position) = pair;
    var (x, y) = (position.X, position.Y);
    var transform = TransformMap[direction].OffsetBy(new(2 * x, 0, 2 * y));

    return new Obstacle(this[position], transform, direction);
  }

  private bool IsAdjacent(Vector2D<int> coord) => this[coord] == ObstacleType.Empty;
  private static readonly IReadOnlyDictionary<Direction, Transform> TransformMap = new Dictionary<Direction, Transform> {
    { Direction.Top, new() { Position = new(0, 1, 0), Rotation = new(0, MathF.PI / 2, 0) } },
    { Direction.Bottom, new() { Position = new(0, -1, 0), Rotation = new(0, -MathF.PI / 2, 0) } },
    { Direction.Forward, new() { Position = new(-1, 0, 0), Rotation = new(MathF.PI / 2, 0, 0) } },
    { Direction.Backward, new() { Position = new(1, 0, 0), Rotation = new(-MathF.PI / 2, 0, 0) } },
    { Direction.Left, new() { Position = new(0, 0, 1), Rotation = new(MathF.PI, 0, 0) } },
    { Direction.Right, new() { Position = new(0, 0, -1), Rotation = new(0, 0, 0) } }
  };
  private ObstacleType[,] _layout = { };
}
}