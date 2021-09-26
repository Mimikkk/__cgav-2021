using Silk.NET.OpenGL;
using Sokoban.Engine.Application;

namespace Sokoban.Engine.Renderers.Buffers.Objects
{
public class Framebuffer
{
  public uint Handle { get; }
  public const FramebufferTarget Target = FramebufferTarget.Framebuffer;

  public void Bind()
  {
    App.Gl.BindFramebuffer(Target, Handle);
  }
  public void Unbind()
  {
    App.Gl.BindFramebuffer(Target, 0);
  }

  public Framebuffer()
  {
    Handle = App.Gl.GenFramebuffer();
  }
}
}