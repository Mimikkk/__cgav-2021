using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;

namespace Sokoban.Renderers.Buffers
{
    public class IndexBuffer : BufferObject<uint>
    {
        public IndexBuffer(IEnumerable<uint> indices) 
            : base(indices.ToArray(), BufferTargetARB.ElementArrayBuffer)
        {
        }
    }
}