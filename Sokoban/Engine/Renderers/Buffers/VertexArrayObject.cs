using System;
using Silk.NET.OpenGL;
using Sokoban.Utilities.Extensions;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Buffers
{
public class VertexArrayObject : IDisposable
{
  public VertexBuffer? VertexBufferObject { private get; init; }
  public IndexBuffer? IndexBufferObject { private get; init; }
  public Layout Layout {
    private get => _layout;
    init {
      _layout = value;
      ConfigureLayout();
    }
  }

  public uint Size => IndexBufferObject?.Count ?? (PerVertexSize != 0 ? VertexBufferObject?.Count ?? 0 / PerVertexSize : 0);
  private uint PerVertexSize => Layout.Size * sizeof(float);


  public VertexArrayObject() => Handle = App.Gl.GenVertexArray();

  private unsafe void ConfigureLayout()
  {
    Bind();
    void ConfigureElement(int offset, int element, uint index)
    {
      App.Gl.VertexAttribPointer(index, element, VertexAttribPointerType.Float, false, Layout.Size, (void*)offset);
      App.Gl.EnableVertexAttribArray(index);
    }
    int IncrementOffset(int offset, int element)
    {
      return offset + element * sizeof(float);
    }

    Layout.Elements.AggregatedForEach(ConfigureElement, IncrementOffset, 0);
  }

  public void Bind()
  {
    App.Gl.BindVertexArray(Handle);
    VertexBufferObject?.Bind();
    IndexBufferObject?.Bind();
  }
  public void Dispose()
  {
    IndexBufferObject?.Dispose();
    VertexBufferObject?.Dispose();
    App.Gl.DeleteVertexArray(Handle);
  }

  private uint Handle { get; }
  private readonly Layout _layout;
}
}