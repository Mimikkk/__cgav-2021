using System.Linq;
using Assimp;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives.Textures;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;

namespace Sokoban.Engine.Objects.Loaders
{
public static partial class ObjectLoader
{
  private static class MaterialLoader
  {
    public static void Load() => Materials.AddRange(Scene.Materials.Select(ToMaterial));

    private static Material ToMaterial(Assimp.Material material) => new() {
      Name = material.Name ?? "Unnamed",
      Opacity = material.Opacity,
      Reflectivity = material.Reflectivity,
      Shininess = material.Shininess,
      ShininessStrength = material.ShininessStrength,
      BumpScaling = material.BumpScaling,
      TransparencyFactor = material.TransparencyFactor,
      AmbientColor = ToVector4D(material.ColorAmbient),
      DiffuseColor = ToVector4D(material.ColorDiffuse),
      EmissiveColor = ToVector4D(material.ColorEmissive),
      ReflectiveColor = ToVector4D(material.ColorReflective),
      SpecularColor = ToVector4D(material.ColorSpecular),
      TransparentColor = ToVector4D(material.ColorTransparent),
      AmbientMap = ToTexture(material.TextureAmbient),
      DiffuseMap = ToTexture(material.TextureDiffuse),
      DisplacementMap = ToTexture(material.TextureDisplacement),
      EmissiveMap = ToTexture(material.TextureEmissive),
      HeightMap = ToTexture(material.TextureHeight),
      NormalMap = ToTexture(material.TextureNormal),
      OpacityMap = ToTexture(material.TextureOpacity),
      ReflectionMap = ToTexture(material.TextureReflection),
      AmbientOcclusionMap = ToTexture(material.TextureAmbientOcclusion),
      LightMap = ToTexture(material.TextureLightMap)
    };
    private static Vector4D<float> ToVector4D(Color4D color4D) =>
      new(color4D.R, color4D.G, color4D.B, color4D.A);
    private static Texture? ToTexture(TextureSlot slot) =>
      slot.FilePath == null ? null : new Texture(slot.FilePath.Split("\\\\").Last());
  }
}
}