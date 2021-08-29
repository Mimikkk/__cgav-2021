using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Renderers.Buffers.Helpers;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class UniformBuffer : Buffer<float>
{
  public uint Binding { private get; init; }
  public Fields Fields {
    private get => _fields;
    init {
      _fields = value;
      ConfigureFields();
    }
  }

  public UniformBuffer(string name)
    : base(BufferTargetARB.UniformBuffer)
  {
    Name = name;
  }

  public unsafe void SetUniform<T>(string name, T value) where T : unmanaged =>
    App.Gl.BufferSubData(BufferTargetARB.UniformBuffer, Fields.OffsetByName[name], Fields.SizeByName[name], &value);

  private readonly Fields _fields;
  private unsafe void ConfigureFields()
  {
    Bind();
    App.Gl.BindBufferBase(BufferType, Binding, Handle);
    App.Gl.BufferData(BufferType, Fields.Layout.Size, null, BufferUsageARB.DynamicDraw);
  }
}
}