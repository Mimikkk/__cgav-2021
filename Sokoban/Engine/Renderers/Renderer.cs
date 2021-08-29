using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;

namespace Sokoban.Engine.Renderers
{
public static class Renderer
{
  public static void Render(Skybox skybox)
  {
    skybox.ShaderConfiguration();
    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
  }
  
  
  
}
}