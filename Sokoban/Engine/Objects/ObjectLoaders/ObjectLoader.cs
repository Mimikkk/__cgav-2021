using System.Collections.Generic;
using System.Linq;
using Assimp;
using Logger;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Renderers;
using Sokoban.Utilities;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;

namespace Sokoban.Engine.Objects.ObjectLoaders
{
public static class ObjectLoader
{
  private static AssimpContext Api { get; } = new();
  private static string Name { get; set; } = null!;
  private static Scene Scene { get; set; } = null!;
  private static string Filepath => $"{Filesystem.Objects / Name}.obj";
  private static readonly List<Mesh> Meshes = new();
  private static readonly List<Material> Materials = new();

  private static readonly PostProcessSteps PostProcess =
    PostProcessPreset.TargetRealTimeMaximumQuality | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.JoinIdenticalVertices;

  public static IEnumerable<GameObject> Load(string name)
  {
    Initialize(name);
    Scene = Api.ImportFile(Filepath, PostProcess);
    LoadMaterials();
    LoadMeshes();
    Log();
    return Meshes.Select(m => new GameObject {
      Spo = default,
      Name = m.Name,
      Mesh = m,
    });
  }

  public static void Log()
  {
    $"Loaded <c17 Scene|>: <c22{Scene.RootNode.Name}|>".LogLine();
    $"Number of <c15 Meshes|>: <c25 {Scene.MeshCount}|>".LogLine(2);
    $"Number of <c15 Materials|>: <c25 {Scene.MaterialCount}|>".LogLine(2);
    $"Loaded <c17 Meshes|>".LogLine(2);
    $"Loaded <c17 Materials|>".LogLine(2);
    foreach (var mesh in Meshes) mesh.Log(4);
    foreach (var material in Materials) material.Log(4);
  }

  private static void LoadMeshes()
    => Meshes.AddRange(Scene.Meshes.Select(ToMesh));

  private static void LoadMaterials()
    => Materials.AddRange(Scene.Materials.Select(ToMaterial));

  private static void Initialize(string name)
  {
    Materials.Clear();
    Meshes.Clear();
    Name = name;
  }

  private static Texture? ToTexture(this TextureSlot slot)
    => slot.FilePath != null ? new Texture(slot.FilePath.Split("\\\\").Last()) : null;

  private static Mesh ToMesh(Assimp.Mesh raw)
  {
    var vertices = Enumerable.Range(0, raw.Normals.Count)
      .Select(i => new Vertex {
        Position = ToVector3D(raw.Vertices[i]),
        Normal = ToVector3D(raw.Normals[i]),
        TextureCoordinate = ToVector2D(raw.TextureCoordinateChannels[0][i]),
        Tangent = ToVector3D(raw.Tangents[i]),
        BiTangent = ToVector3D(raw.BiTangents[i]),
      });

    var indices = raw.Faces.SelectMany(face => face.Indices).Select(x => (uint)x);

    return new Mesh {
      Name = raw.Name,
      Material = Materials[raw.MaterialIndex],
      Vao = new() {
        IndexBuffer = new(indices),
        VertexBuffer = new(vertices),
        Layout = new(3, 3, 2, 3, 3)
      }
    };
  }
  private static Material ToMaterial(Assimp.Material raw) => new(raw.Name ?? "Unnamed") {
    Opacity = raw.Opacity,
    Reflectivity = raw.Reflectivity,
    Shininess = raw.Shininess,
    ShininessStrength = raw.ShininessStrength,
    BumpScaling = raw.BumpScaling,
    TransparencyFactor = raw.TransparencyFactor,
    AmbientColor = raw.ColorAmbient.ToColor(),
    DiffuseColor = raw.ColorDiffuse.ToColor(),
    EmissiveColor = raw.ColorEmissive.ToColor(),
    ReflectiveColor = raw.ColorReflective.ToColor(),
    SpecularColor = raw.ColorSpecular.ToColor(),
    TransparentColor = raw.ColorTransparent.ToColor(),
    AmbientTexture = raw.TextureAmbient.ToTexture(),
    DiffuseTexture = raw.TextureDiffuse.ToTexture() ?? Texture.Missing,
    DisplacementTexture = raw.TextureDisplacement.ToTexture(),
    EmissiveTexture = raw.TextureEmissive.ToTexture(),
    HeightTexture = raw.TextureHeight.ToTexture(),
    NormalTexture = raw.TextureNormal.ToTexture(),
    OpacityTexture = raw.TextureOpacity.ToTexture(),
    ReflectionTexture = raw.TextureReflection.ToTexture(),
    AmbientOcclusionTexture = raw.TextureAmbientOcclusion.ToTexture(),
    LightMapTexture = raw.TextureLightMap.ToTexture()
  };

  private static Vector3D<float> ToVector3D(Assimp.Vector3D v) => new(v.X, v.Y, v.Z);
  private static Vector2D<float> ToVector2D(Assimp.Vector3D v) => new(v.X, v.Y);
  private static Vector4D<float> ToColor(this Color4D c) => new(c.R, c.G, c.B, c.A);
}
}