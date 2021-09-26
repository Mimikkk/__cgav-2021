using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class Renderbuffer
{
  public uint Handle { get; }
  public const RenderbufferTarget Target = RenderbufferTarget.Renderbuffer;

  public void Bind() => App.Gl.BindRenderbuffer(Target, Handle);
  public void Unbind() => App.Gl.BindRenderbuffer(Target, 0);

  public void Store(Vector2D<uint> Size, InternalFormat format) => Store(Size.X, Size.Y, format);
  public void Store(uint x, uint y, InternalFormat format) => App.Gl.RenderbufferStorage(Target, format, x, y);

  public void Pin(FramebufferAttachment attachment) => App.Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, attachment, Target, Handle);

  public Renderbuffer()
  {
    Handle = App.Gl.GenFramebuffer();
  }
}
}