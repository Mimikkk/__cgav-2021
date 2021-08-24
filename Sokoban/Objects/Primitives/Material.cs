using Logger;
using Silk.NET.Maths;

namespace sokoban.Objects.Primitives
{
public class Material
{
  public Material(string name)
  {
    Name = name;
  }
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
    $"<c70 AmbientMap|>: {AmbientTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 DiffuseMap|>: {DiffuseTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 DisplacementMap|>: {DisplacementTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 EmissiveMap|>: {EmissiveTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 HeightMap|>: {HeightTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 NormalMap|>: {NormalTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 OpacityMap|>: {OpacityTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 ReflectionMap|>: {ReflectionTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 AmbientOcclusionMap|>: {AmbientOcclusionTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
    $"<c70 LightMap|>: {LightMapTexture?.Name ?? "<c8 None|>"}".LogLine(4 + depth);
  }
  public string Name { get; }

  public float Opacity { get; set; } = 1;
  public float Reflectivity { get; set; }
  public float Shininess { get; set; }
  public float ShininessStrength { get; set; } = 1;
  public float BumpScaling { get; set; }
  public float TransparencyFactor { get; set; } = 1f;

  public Texture? AmbientTexture { get; set; }
  public Texture? DiffuseTexture { get; set; }
  public Texture? DisplacementTexture { get; set; }
  public Texture? EmissiveTexture { get; set; }
  public Texture? HeightTexture { get; set; }
  public Texture? NormalTexture { get; set; }
  public Texture? OpacityTexture { get; set; }
  public Texture? ReflectionTexture { get; set; }
  public Texture? AmbientOcclusionTexture { get; set; }
  public Texture? LightMapTexture { get; set; }

  public Vector4D<float> AmbientColor { get; set; } = Vector4D<float>.UnitW + Vector4D<float>.UnitX;
  public Vector4D<float> DiffuseColor { get; set; } = Vector4D<float>.One;
  public Vector4D<float> EmissiveColor { get; set; } = Vector4D<float>.UnitW;
  public Vector4D<float> ReflectiveColor { get; set; } = Vector4D<float>.UnitW;
  public Vector4D<float> SpecularColor { get; set; } = Vector4D<float>.One;
  public Vector4D<float> TransparentColor { get; set; } = Vector4D<float>.UnitW;
}
}