using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Application;
using Sokoban.Controllers;
using Sokoban.Objects;
using Sokoban.Renderers.Buffers;
using Sokoban.Renderers.Shaders;
using Shader = Sokoban.Renderers.Shaders.Shader;

namespace Sokoban
{
internal static class Program
{
  private static void Main()
  {
    App.OnLoad(Behaviour.Start);
    App.OnRender(Behaviour.Render);
    App.Run();
  }
}
}