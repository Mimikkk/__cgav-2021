using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
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

  public override string ToString() =>
    "Vertex("
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