using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Buffers
{
public abstract class Buffer<TDataType> : IDisposable where TDataType : unmanaged
{
  public string Name { get; protected init; } = "Unnamed";

  public void Bind() => App.Gl.BindBuffer(BufferType, Handle);
  public void Dispose() => App.Gl.DeleteBuffer(Handle);

  public int Count { get; private init; }
  public uint Size { get; private init; }
  protected IEnumerable<TDataType> Data {
    init {
      var data = value.ToArray();
      Count = data.Length;
      Size = (uint)(data.Length * sizeof(float));
      Load(data);
    }
  }

  protected Buffer(BufferTargetARB bufferType)
  {
    BufferType = bufferType;
    Handle = App.Gl.GenBuffer();
    Bind();
  }
  private unsafe void Load(Span<TDataType> data)
  {
    fixed (void* loaded = data) App.Gl.BufferData(BufferType, Size, loaded, BufferUsageARB.StaticDraw);
  }

  protected BufferTargetARB BufferType { get; }
  protected uint Handle { get; }
}
}