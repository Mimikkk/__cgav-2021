using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Scripts
{
public abstract class MonoBehaviour
{
  protected MonoBehaviour()
  {
    App.OnLoad(Start);
    App.OnUpdate(Update);
    App.OnRender(Render);
  }

  protected abstract void Start();
  protected abstract void Update(double dt);
  protected abstract void Render(double dt);
}
}