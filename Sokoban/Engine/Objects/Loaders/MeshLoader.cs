using System.Collections.Generic;
using System.Linq;
using Assimp;
using Silk.NET.Maths;
using Sokoban.Engine.Renderers;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;

namespace Sokoban.Engine.Objects.Loaders
{
public static partial class ObjectLoader
{
  private static class MeshLoader
  {
    public static void Load() => Meshes.AddRange(Scene.Meshes.Select(ToMesh));

    private static Mesh ToMesh(Assimp.Mesh mesh) => new() {
      Name = mesh.Name,
      Material = Materials[mesh.MaterialIndex],
      Vao = new VertexArray {
        VertexBuffer = new(ToVertices(mesh)),
        IndexBuffer = new(mesh.Faces.SelectMany(ToIndices)),
        Layout = new(3, 3, 2, 3, 3)
      }
    };

    private static IEnumerable<Vertex> ToVertices(Assimp.Mesh mesh) => mesh.Vertices.Select(ToVector3D)
      .Zip(mesh.TextureCoordinateChannels[0].Select(ToVector2D))
      .Zip(mesh.Normals.Select(ToVector3D))
      .Zip(mesh.Tangents.Select(ToVector3D))
      .Zip(mesh.BiTangents.Select(ToVector3D))
      .Select(ToVertex);

    private static Vertex ToVertex(((((Vector3D<float>, Vector2D<float>), Vector3D<float>), Vector3D<float>), Vector3D<float>) vertex)
    {
      var ((((position, textureCoordinate), normal), tangent), biTangent) = vertex;
      return new Vertex {
        Position = position,
        TextureCoordinate = textureCoordinate,
        Normal = normal,
        Tangent = tangent,
        BiTangent = biTangent
      };
    }
    private static IEnumerable<uint> ToIndices(Face face) => face.Indices.Select(i => (uint)i);
    private static Vector2D<float> ToVector2D(Assimp.Vector3D vector) => new(vector.X, vector.Y);
    private static Vector3D<float> ToVector3D(Assimp.Vector3D vector) => new(vector.X, vector.Y, vector.Z);
  }
}
}