#nullable enable
using System.Collections.Generic;
using System.Linq;
using Assimp;
using Logger;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers;
using Sokoban.Utilities;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;

namespace Sokoban.utilities
{
internal static class ObjectLoader
{
  private static AssimpContext Api { get; } = new();
  private static string Name { get; set; } = null!;
  private static Scene Scene { get; set; } = null!;
  private static string Filepath => $"{Filesystem.Objects / Name}.obj";
  private static readonly List<Mesh> Meshes = new();
  private static readonly List<Material> Materials = new();

  private const PostProcessSteps PostProcess = PostProcessSteps.Triangulate
                                               | PostProcessSteps.GenerateNormals
                                               | PostProcessSteps.GenerateUVCoords
                                               | PostProcessSteps.ImproveCacheLocality
                                               | PostProcessSteps.CalculateTangentSpace
                                               | PostProcessSteps.OptimizeGraph
                                               | PostProcessSteps.OptimizeMeshes;

  public static IEnumerable<GameObject> Load(string name)
  {
    Initialize(name);
    Scene = Api.ImportFile(Filepath, PostProcess);
    LoadMaterials();
    LoadMeshes();

    return Meshes.Select(m => new GameObject() { Mesh = m, Name = m.Name });
  }

  public static void Log()
  {
    $"Loaded <c17 Scene>: <c22{Scene.RootNode.Name}>".LogLine();
    $"Number of <c15 Meshes>: <c25 {Scene.MeshCount}>".LogLine(2);
    $"Number of <c15 Materials>: <c25 {Scene.MaterialCount}>".LogLine(2);
    $"Loaded <c17 Meshes>".LogLine(2);
    $"Loaded <c17 Materials>".LogLine(2);
    foreach (var mesh in Meshes) mesh.Log(4);
    foreach (var material in Materials) material.Log(4);
  }

  private static void LoadMeshes() => Meshes.AddRange(Scene.Meshes.Select(raw => new Mesh() {
    Name = raw.Name,
    Material = Materials[raw.MaterialIndex],
    Vao = new() {
      VertexBuffer = new(raw.Vertices.Select(p => new Vector3D<float>(p.X, p.Y, p.Z))
        .Zip(raw.TextureCoordinateChannels[0].Select(vec => new Vector2D<float>(vec.X, vec.Y)))
        .Zip(raw.Normals.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)))
        .Zip(raw.Tangents.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)))
        .Zip(raw.BiTangents.Select(n => new Vector3D<float>(n.X, n.Y, n.Z)))
        .Select(ptn
          => new Vertex() {
            Position = ptn.First.First.First.First,
            TextureCoordinate = ptn.First.First.First.Second,
            Normal = ptn.First.First.Second,
            Tangent = ptn.First.Second,
            BiTangent = ptn.Second,
          })),
      IndexBuffer = new(raw.Faces.SelectMany(face => face.Indices).Select(i => (uint)i)),
      Layout = new(3,3,2,3,3),
    },
  }));

  private static void LoadMaterials()
    => Materials.AddRange(Scene.Materials.Select(raw => new Material() {
        Name = raw.Name ?? "Unnamed",
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
        AmbientMap = raw.TextureAmbient.ToTexture(),
        DiffuseMap = raw.TextureDiffuse.ToTexture(),
        DisplacementMap = raw.TextureDisplacement.ToTexture(),
        EmissiveMap = raw.TextureEmissive.ToTexture(),
        HeightMap = raw.TextureHeight.ToTexture(),
        NormalMap = raw.TextureNormal.ToTexture(),
        OpacityMap = raw.TextureOpacity.ToTexture(),
        ReflectionMap = raw.TextureReflection.ToTexture(),
        AmbientOcclusionMap = raw.TextureAmbientOcclusion.ToTexture(),
        LightMap = raw.TextureLightMap.ToTexture()
      })
      .ToList());

  private static void Initialize(string name)
  {
    Materials.Clear();
    Meshes.Clear();
    Name = name;
  }

  private static Vector4D<float> ToColor(this Color4D color4D)
    => new(color4D.R, color4D.G, color4D.B, color4D.A);

  private static Texture? ToTexture(this TextureSlot slot)
    => slot.FilePath == null ? null : new Texture(slot.FilePath.Split("\\\\").Last());
}
}