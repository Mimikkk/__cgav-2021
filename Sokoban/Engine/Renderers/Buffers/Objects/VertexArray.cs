using System;
using Silk.NET.OpenGL;
using Sokoban.Engine.Renderers.Buffers.Helpers;
using Sokoban.Utilities.Extensions;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class VertexArray : IDisposable
{
  public void Bind()
  {
    App.Gl.BindVertexArray(Handle);
    VertexBuffer?.Bind();
    IndexBuffer?.Bind();
  }
  public void Dispose()
  {
    IndexBuffer?.Dispose();
    VertexBuffer?.Dispose();
    App.Gl.DeleteVertexArray(Handle);
  }

  public VertexBuffer? VertexBuffer { get; init; }
  public IndexBuffer? IndexBuffer { get; init; }
  public Layout Layout {
    private get => _layout;
    init {
      _layout = value;
      ConfigureLayout();
    }
  }

  public uint Size => ((uint?)IndexBuffer?.Count ?? (PerVertexSize != 0 ? VertexBuffer?.Size ?? 0 / PerVertexSize : 0)) * sizeof(float);
  private uint PerVertexSize => Layout.Size * sizeof(float);

  public VertexArray() => Handle = App.Gl.GenVertexArray();

  private unsafe void ConfigureLayout()
  {
    Bind();
    void ConfigureElement(int offset, int element, uint index)
    {
      App.Gl.VertexAttribPointer(index, element, VertexAttribPointerType.Float, false, Layout.Size, (void*)offset);
      App.Gl.EnableVertexAttribArray(index);
    }
    int IncrementOffset(int offset, int element) => offset + element * sizeof(float);

    Layout.Elements.AggregatedForEach(ConfigureElement, IncrementOffset, 0);
  }

  private uint Handle { get; }
  private readonly Layout _layout;
}
}