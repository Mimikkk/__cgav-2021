using Silk.NET.Input;
using Sokoban.Engine.Application;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class ExitBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.Critical;
  protected override void Start() => Controller.OnRelease(Key.Escape, App.Close);
}
}