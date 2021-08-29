using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Scripts
{
public abstract class MonoBehaviour
{
  protected MonoBehaviour()
  {
    App.OnLoad(Start);
    App.OnUpdate(Update);
    App.OnRender(EarlyRender);
    App.OnRender(Render);
  }

  protected virtual void Start() { }
  protected virtual void Update(double dt) { }
  protected virtual void EarlyRender(double dt) { }
  protected virtual void Render(double dt) { }
}
}