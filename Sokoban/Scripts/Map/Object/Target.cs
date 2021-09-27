using System.Collections.Generic;
using System.Linq;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Resources;

namespace Sokoban.Scripts.Map.Object
{
public class Target
{
  public static GameObject Sphere { get; } = ObjectLoader.Load("Icosphere").First();
  public static readonly Transform Transform = new() {
    Scale = 0.5f,
  };

  public static void Draw()
  {
    ResourceManager.ShaderPrograms.PbrShaderConfiguration(Material, Transform);
    Sphere.Draw();
  }

  private static readonly Material Material = ResourceManager.Materials.Plastic;
}
}