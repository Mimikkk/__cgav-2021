using System;
using Silk.NET.OpenGL;

namespace Sokoban.Renderers.Buffers
{
public class Buffer<TDataType> : IDisposable where TDataType : unmanaged
{
  public void Bind() => Application.Gl.BindBuffer(Type, Handle);
  public void Unbind() => Application.Gl.BindBuffer(Type, 0);
  public void Dispose() => Application.Gl.DeleteBuffer(Handle);

  public Buffer(Span<TDataType> data, BufferTargetARB type)
  {
    Type = type;
    Handle = Application.Gl.GenBuffer();
    LoadInto(data);
  }

  private unsafe void LoadInto(Span<TDataType> data)
  {
    Bind();
    fixed (void* d = data)
      Application.Gl.BufferData(Type, (nuint)(data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
    Unbind();
  }

  private readonly uint Handle;
  private readonly BufferTargetARB Type;
}
}