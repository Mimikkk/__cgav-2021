using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class SkyboxBehaviour : MonoBehaviour
{
  private static readonly Skybox Skybox = new();
  protected override void EarlyRender(double dt) => Renderer.Render(Skybox);
}
}