﻿using System;
using Logger;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;

namespace Sokoban.Engine.Renderers.Buffers
{
public class UniformBufferObject : IDisposable
{
  public readonly uint Index;
  public Fields Fields {
    get => _fields;
    init {
      _fields = value;
      ConfigureFields();
    }
  }

  public void Bind() => App.Gl.BindBuffer(BufferTargetARB.UniformBuffer, Handle);
  public void Dispose() => App.Gl.DeleteVertexArray(Handle);

  public UniformBufferObject(uint binding)
  {
    Index = binding;
    Handle = App.Gl.GenBuffer();
  }

  public unsafe void SetUniform<T>(string name, T value) where T : unmanaged =>
    App.Gl.BufferSubData(BufferTargetARB.UniformBuffer, Fields.OffsetByName[name], Fields.SizeByName[name], &value);

  private uint Handle { get; }
  private readonly Fields _fields;
  private unsafe void ConfigureFields()
  {
    Bind();
    App.Gl.BindBufferBase(BufferTargetARB.UniformBuffer, Index, Handle);
    App.Gl.BufferData(BufferTargetARB.UniformBuffer, Fields.Layout.Size, null, BufferUsageARB.DynamicDraw);
    Fields.Layout.Size.LogLine();
  }
}
}