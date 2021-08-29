using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class IndexBuffer : Buffer<uint>
{
  public IndexBuffer(IEnumerable<uint> indices)
    : base(BufferTargetARB.ElementArrayBuffer)
  {
    Data = indices;
  }
}
}