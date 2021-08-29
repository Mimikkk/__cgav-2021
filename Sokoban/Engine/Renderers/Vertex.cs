using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Sokoban.Engine.Renderers.Buffers.Helpers;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Engine.Renderers
{
public readonly struct Vertex
{
  public Vector3D<float>? Position { get; init; }
  public Vector3D<float>? Normal { get; init; }
  public Vector2D<float>? TextureCoordinate { get; init; }
  public Vector3D<float>? Tangent { get; init; }
  public Vector3D<float>? BiTangent { get; init; }
  public bool ManageLayout {
    init => Layout = new(new[] {
        Position.HasValue ? 3 : 0,
        Normal.HasValue ? 3 : 0,
        TextureCoordinate.HasValue ? 2 : 0,
        Tangent.HasValue ? 3 : 0,
        BiTangent.HasValue ? 3 : 0
      }.Where(x => x > 0)
      .ToArray());
  }

  public Layout Layout { get; private init; }

  public override string ToString() =>
    $"Vertex({Layout.Size}: "
    + (Position.HasValue ? $"{Position};" : "")
    + (Normal.HasValue ? $"{Normal};" : "")
    + (TextureCoordinate.HasValue ? $"{TextureCoordinate};" : "")
    + (Tangent.HasValue ? $"{Tangent};" : "")
    + (BiTangent.HasValue ? $"{BiTangent};" : "")
    + ")";

  public IEnumerable<float> ToFloats() =>
    new[] {
      Position?.ToArray(),
      Normal?.ToArray(),
      TextureCoordinate?.ToArray(),
      Tangent?.ToArray(),
      BiTangent?.ToArray()
    }.SelectMany(x => x ?? Array.Empty<float>());
}
}