using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Scripts
{
public abstract class MonoBehaviour
{
  public virtual LoadPriority LoadPriority => LoadPriority.Normal;

  public void Run()
  {
    App.OnLoad(Start);
    App.OnUpdate(Update);
    App.OnRender(Render);
  }

  protected virtual void Start() { }
  protected virtual void Update(double dt) { }
  protected virtual void Render(double dt) { }
}
}