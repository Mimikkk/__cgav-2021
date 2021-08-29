using System.Collections.Generic;
using Silk.NET.Maths;
using Sokoban.Engine.Renderers.Buffers.Helpers;

namespace Sokoban.Engine.Renderers
{
public readonly struct Vertex
{
  private Vector3D<float> Position { get; }
  public Vector3D<float> Normal { get; }
  public Vector2D<float> TextureCoordinate { get; }
  public Vector3D<float> Tangent { get; }
  public Vector3D<float> BiTangent { get; }

  public static Layout Layout = new(3, 3, 2, 3, 3);


  public Vertex(Vector3D<float> position, Vector3D<float> normal, Vector2D<float> textureCoordinate,
    Vector3D<float> tangent, Vector3D<float> biTangent)
  {
    Position = position;
    Normal = normal;
    TextureCoordinate = textureCoordinate;
    Tangent = tangent;
    BiTangent = biTangent;
  }

  public override string ToString()
  {
    return "TangentVertex("
           + $"{Position.X},{Position.Y},{Position.Z};"
           + $"{Normal.X},{Normal.Y},{Normal.Z};"
           + $"{TextureCoordinate.X},{TextureCoordinate.Y};"
           + $"{Tangent.X},{Tangent.Y},{Tangent.Z};"
           + $"{BiTangent.X},{BiTangent.Y},{BiTangent.Z})";
  }

  // [Pos,TexCords,Norm, Tan, BiTan] Format
  public IEnumerable<float> Tofloats()
  {
    return new[] {
      Position.X, Position.Y, Position.Z,
      Normal.X, Normal.Y, Normal.Z,
      TextureCoordinate.X, TextureCoordinate.Y,
      Tangent.X, Tangent.Y, Tangent.Z,
      BiTangent.X, BiTangent.Y, BiTangent.Z
    };
  }
}
}