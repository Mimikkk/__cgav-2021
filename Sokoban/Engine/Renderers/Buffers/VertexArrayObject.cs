#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.OpenGL;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Buffers
{
public class VertexArrayObject : IDisposable
{
  public VertexBuffer? VertexBufferObject { get; init; }
  public IndexBuffer? IndexBufferObject { get; init; }

  public uint Size => IndexBufferObject?.Count ?? (PerVertexSize != 0 ? VertexBufferObject?.Count ?? 0 / PerVertexSize : 0);
  private uint PerVertexSize => Layout.Size * sizeof(float);

  public ElementLayout Layout {
    get => _layout;
    init {
      _layout = value;
      ReconfigureLayout();
    }
  }

  public VertexArrayObject() => Handle = App.Gl.GenVertexArray();

  private unsafe void ReconfigureLayout()
  {
    Bind();
    var offset = 0;
    for (uint i = 0; i < Layout.Elements.Count; ++i)
    {
      var element = Layout.Elements[(int)i];
      App.Gl.VertexAttribPointer(i, element, VertexAttribPointerType.Float, false,
        Layout.Size * sizeof(float), (void*)(offset * sizeof(float)));
      App.Gl.EnableVertexAttribArray(i);
      offset += element;
    }
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
  private readonly ElementLayout _layout;
}

public readonly struct ElementLayout
{
  public uint Size { get; }
  public IReadOnlyList<int> Elements { get; }

  public ElementLayout(params int[] elements)
  {
    Size = (uint)elements.Sum();
    Elements = elements;
  }
}
}