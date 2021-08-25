using System;
using Silk.NET.OpenGL;

namespace Sokoban.Renderers.Buffers
{
public class VertexArray<TVertexType, TIndexType> : IDisposable
  where TVertexType : unmanaged
  where TIndexType : unmanaged
{
  public void Bind() => Application.Gl.BindVertexArray(Handle);
  public void Unbind() => Application.Gl.BindVertexArray(0);
  public void Dispose() => Application.Gl.DeleteVertexArray(Handle);

  private readonly uint Handle;

  public VertexArray(Buffer<TVertexType> vbo, Buffer<TIndexType> ebo)
  {
    Handle = Application.Gl.GenVertexArray();
    Bind();
    vbo.Bind();
    ebo.Bind();
  }

  public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize, int offSet)
  {
    Bind();
    Application.Gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType), (void*)(offSet * sizeof(TVertexType)));
    Application.Gl.EnableVertexAttribArray(index);
  }
}
}