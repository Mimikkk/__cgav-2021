using Logger;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Buffers.Objects;

namespace Sokoban.Engine.Objects.Primitives
{
public class Mesh
{
  public string Name { get; init; } = "Unnamed";
  public Material? Material { get; set; }

  public uint IndexCount => (uint)(Vao.IndexBuffer?.Count ?? 0);
  public uint VertexCount => Vao.Size / sizeof(float);

  public VertexArray Vao { get; init; } = null!;

  public void Log(int depth = 0)
  {
    $"Mesh: <c20 {Name}|>".LogLine(depth);
    $"Number of Vertices: <c22 {VertexCount}|>".LogLine(depth + 2);
    $"Number of Indices: <c22 {IndexCount}|>".LogLine(depth + 2);
  }
}
}