using Logger;
using Silk.NET.Maths;

namespace Sokoban.Engine.Objects.Primitives.Textures
{
public class Material
{
  public void Log(int depth = 0)
  {
    $"<c20 Material|>: <c22 {Name}|>".LogLine(depth);
    $"<c88 Values|>".LogLine(2 + depth);
    $"<c70 Opacity|>: {Opacity}".LogLine(4 + depth);
    $"<c70 Reflectivity|>: {Reflectivity}".LogLine(4 + depth);
    $"<c70 Shininess|>: {Shininess}".LogLine(4 + depth);
    $"<c70 BumpScaling|>: {BumpScaling}".LogLine(4 + depth);
    $"<c70 ShininessStrength|>: {ShininessStrength}".LogLine(4 + depth);
    $"<c70 TransparencyFactor|>: {TransparencyFactor}".LogLine(4 + depth);
    $"<c88 Colors|>".LogLine(2 + depth);
    $"<c70 Ambient|>: {AmbientColor}".LogLine(4 + depth);
    $"<c70 Diffuse|>: {DiffuseColor}".LogLine(4 + depth);
    $"<c70 Emissive|>: {EmissiveColor}".LogLine(4 + depth);
    $"<c70 Reflective|>: {ReflectiveColor}".LogLine(4 + depth);
    $"<c70 Specular|>: {SpecularColor}".LogLine(4 + depth);
    $"<c70 Transparent|>: {TransparentColor}".LogLine(4 + depth);
    $"<c88 Maps|>".LogLine(2 + depth);
    $"<c70 AmbientMap|>: {AmbientMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 DiffuseMap|>: {DiffuseMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 DisplacementMap|>: {DisplacementMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 EmissiveMap|>: {EmissiveMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 HeightMap|>: {HeightMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 NormalMap|>: {NormalMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 OpacityMap|>: {OpacityMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 ReflectionMap|>: {ReflectionMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 AmbientOcclusionMap|>: {AmbientOcclusionMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 LightMap|>: {LightMap?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
  }
  public string? Name { get; init;}

  public float Opacity { get; set; } = 1;
  public float Reflectivity { get; set; }
  public float Shininess { get; set; }
  public float ShininessStrength { get; set; } = 1;
  public float BumpScaling { get; set; }
  public float TransparencyFactor { get; set; } = 1f;

  public Texture? AmbientMap { get; set; }
  public Texture? DiffuseMap { get; set; }
  public Texture? DisplacementMap { get; set; }
  public Texture? EmissiveMap { get; set; }
  public Texture? HeightMap { get; set; }
  public Texture? NormalMap { get; set; }
  public Texture? OpacityMap { get; set; }
  public Texture? ReflectionMap { get; set; }
  public Texture? AmbientOcclusionMap { get; set; }
  public Texture? LightMap { get; set; }

  public Vector4D<float> AmbientColor { get; set; } = Vector4D<float>.UnitW + Vector4D<float>.UnitX;
  public Vector4D<float> DiffuseColor { get; set; } = Vector4D<float>.One;
  public Vector4D<float> EmissiveColor { get; set; } = Vector4D<float>.UnitW;
  public Vector4D<float> ReflectiveColor { get; set; } = Vector4D<float>.UnitW;
  public Vector4D<float> SpecularColor { get; set; } = Vector4D<float>.One;
  public Vector4D<float> TransparentColor { get; set; } = Vector4D<float>.UnitW;
}
}