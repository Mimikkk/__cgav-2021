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
        VertexBuffer = new(mesh.Vertices.Select(p => new Vector3D<float>(p.X, p.Y, p.Z))
          .Zip(mesh.TextureCoordinateChannels[0].Select(vec => new Vector2D<float>(vec.X, vec.Y)))
          .Zip(mesh.Normals.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)))
          .Select(ptn => new Vertex() {
            Position = ptn.First.First,
            TextureCoordinate = ptn.First.Second,
            Normal = ptn.Second,
          })),
        IndexBuffer = new(mesh.Faces.SelectMany(ToIndices)),
        Layout = new(3, 2, 3),
      }
    };
    private static IEnumerable<uint> ToIndices(Face face) => face.Indices.Select(i => (uint)i);
  }

}
}