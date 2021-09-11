using System.Collections.Generic;
using System.Linq;
using Assimp;
using Logger;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Utilities;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;

namespace Sokoban.Engine.Objects.Loaders
{
public static partial class ObjectLoader
{
  private static AssimpContext Api { get; } = new();
  private static string Name { get; set; } = null!;
  private static Scene Scene { get; set; } = null!;
  private static string Filepath => $"{Filesystem.Objects / Name}.obj";
  private static readonly List<Mesh> Meshes = new();
  private static readonly List<Material> Materials = new();

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

  private const PostProcessSteps PostProcess = PostProcessSteps.Triangulate
                                               | PostProcessSteps.GenerateNormals
                                               | PostProcessSteps.GenerateUVCoords
                                               | PostProcessSteps.CalculateTangentSpace
                                               | PostProcessSteps.ImproveCacheLocality
                                               | PostProcessSteps.OptimizeGraph
                                               | PostProcessSteps.OptimizeMeshes;

  public static IEnumerable<GameObject> Load(string name)
  {
    Initialize(name);
    Scene = Api.ImportFile(Filepath, PostProcess);
    MaterialLoader.Load();
    MeshLoader.Load();

    return Meshes.Select(Into);
  }

  private static GameObject Into(this Mesh mesh) => new() {
    Name = mesh.Name,
    Mesh = mesh
  };

  private static void Initialize(string name)
  {
    Materials.Clear();
    Meshes.Clear();
    Name = name;
  }
}
}