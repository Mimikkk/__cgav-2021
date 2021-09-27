using System.Linq;
using Silk.NET.Maths;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Resources;

namespace Sokoban.Scripts.Map.Object
{
public static class Box
{
  private static readonly GameObject Go = ObjectLoader.Load("Box").First();
  static Box()
  {
    Go.Spo = ResourceManager.ShaderPrograms.Pbr;
    Go.Mesh!.Material = ResourceManager.Materials.Cube;
  }
  
}
}