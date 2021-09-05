using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class SkyboxBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.VeryHigh;

  protected override void Render(double dt) => Renderer.Render(Skybox);

  private static readonly Skybox Skybox = new() {
    Camera = CameraBehaviour.Camera,
  };
}
}