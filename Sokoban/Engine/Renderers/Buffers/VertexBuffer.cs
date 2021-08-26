using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.Engine.Renderers.Buffers
{
public class VertexBuffer : BufferObject<float>
    {
        public VertexBuffer(IEnumerable<float> vertices)
            : base(vertices.ToArray(), BufferTargetARB.ArrayBuffer)
        {
        }
        public VertexBuffer(IEnumerable<Vertex> vertices)
            : this(vertices.SelectMany(v => v.Tofloats()))
        {
        }
    }
}
