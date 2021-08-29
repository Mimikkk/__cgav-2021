using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class SkyboxBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.VeryHigh;

  private static readonly Skybox Skybox = new();
  protected override void Render(double dt) => Renderer.Render(Skybox);
}
}