using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts
{
public class SkyboxBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.VeryHigh;

  protected override void Render(double dt)
  {
    return;
    App.Gl.DepthFunc(DepthFunction.Lequal);
    Skybox.Vao.Bind();
    Skybox.Spo.Bind();

    Skybox.CubeMap.Bind(0);
    Skybox.Spo.SetUniform("environment_map", 0);

    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
  }
}
}