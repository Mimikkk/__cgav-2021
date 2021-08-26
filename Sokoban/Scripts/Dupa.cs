using Logger;
using Silk.NET.Input;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class Dupa : MonoBehaviour
{
  protected override void Start()
  {
    Controller.OnPush(Key.A, () => "Dupa".LogLine(4));
  }
}
}