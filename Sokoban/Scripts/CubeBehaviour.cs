using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.GameMap;

namespace Sokoban.Scripts
{
public class CubeBehaviour : MonoBehaviour
{
  private static readonly Material Material = new() {
    DiffuseMap = new("SilkBoxed.png"),
    NormalMap = new("SilkSpecular.png"),
    AmbientColor = new(0.1f, 0.1f, 0.1f, 1.0f),
    DiffuseColor = new(0.5f, 0.5f, 0.5f, 1.0f),
    SpecularColor = Vector4D<float>.One,
    Shininess = 4,
  };
  private static readonly Cube Cube = new(Material);

  protected override void Render(double dt) => Cube.Draw();
}
}