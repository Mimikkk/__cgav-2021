using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Resources;

namespace Sokoban.Scripts.Map.Object
{
public class Target
{
  public static GameObject Go { get; } = ObjectLoader.Load("Icosphere").First();
  public static readonly Transform Transform = new() {
    Scale = 0.5f,
  };

  public static void Draw(Vector3D<float> offset, Vector3D<float> rotation)
  {
    ResourceManager.ShaderPrograms.PbrShaderConfiguration(Material, Transform.OffsetBy(offset).RotatedBy(rotation));
    Go.Draw();
  }

  private static readonly Material Material = ResourceManager.Materials.Plastic;
}
}