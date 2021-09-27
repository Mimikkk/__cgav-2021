using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Resources;

namespace Sokoban.Scripts.Map.Object
{
public static class Player
{
  public static List<GameObject> Gura { get; } = ObjectLoader.Load("Guwa").ToList();
  public static readonly Transform Transform = new() {
    Position = -Vector3D<float>.UnitY,
  };
  public static Direction Direction { get; set; }

  static Player()
  {
    foreach (var guraPart in Gura) guraPart.Mesh!.Material = Material;
  }

  public static void Draw(Vector3D<float> offset, Vector3D<float> rotation)
  {
    ResourceManager.ShaderPrograms.PbrShaderConfiguration(Material, Transform.OffsetBy(offset).RotatedBy(rotation));
    foreach (var guraPart in Gura) guraPart.Draw();
  }

  private static readonly Material Material = new() {
    DiffuseMap = new("SmolGuraTex.png") {
      InternalFormat = InternalFormat.Rgb,
      PixelFormat = PixelFormat.Rgb,
      PixelType = PixelType.Float
    },
    NormalMap = new("Plastic/Normal.png"),
    AmbientOcclusionMap = new("Plastic/AmbientOcclusion.png"),
    ReflectionMap = new("Plastic/Metallic.png"),
    HeightMap = new("Plastic/AmbientOcclusion.png")
  };
}
}