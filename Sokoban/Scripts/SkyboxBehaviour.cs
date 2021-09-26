using Sokoban.Engine.Objects;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class SkyboxBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.VeryHigh;

  protected override void Render(double dt) => Skybox.Draw();
}
}