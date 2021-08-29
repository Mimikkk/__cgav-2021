using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class VertexBuffer : Buffer<float>
{
  public VertexBuffer(IEnumerable<float> vertices)
    : base(BufferTargetARB.ArrayBuffer)
  {
    Data = vertices.ToArray();
  }
  public VertexBuffer(IEnumerable<Vertex> vertices)
    : this(vertices.SelectMany(vertex => vertex.ToFloats())) { }
}
}