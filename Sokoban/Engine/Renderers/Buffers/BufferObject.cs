using System;
using Silk.NET.OpenGL;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Buffers
{
public abstract class BufferObject<TDataType> : IDisposable where TDataType : unmanaged
{
  private uint Handle { get; }
  private BufferTargetARB BufferType { get; }
  public readonly uint Count;
  public readonly uint Size;

  protected unsafe BufferObject(Span<TDataType> data, BufferTargetARB bufferType)
  {
    BufferType = bufferType;

    Handle = App.Gl.GenBuffer();
    Bind();
    fixed (void* d = data)
    {
      Count = (uint)data.Length;
      Size = (uint)(Count * sizeof(TDataType));
      App.Gl.BufferData(bufferType, (nuint)(data.Length * sizeof(TDataType)), d, BufferUsageARB.StaticDraw);
    }
  }

  public void Bind() { App.Gl.BindBuffer(BufferType, Handle); }

  public void Dispose() { App.Gl.DeleteBuffer(Handle); }
}
}