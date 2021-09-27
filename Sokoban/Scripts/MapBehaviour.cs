using System.Collections.Generic;
using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;
using Sokoban.Scripts.Map.Object;
using Sokoban.Utilities.Extensions;
using static Sokoban.Resources.ResourceManager.Materials;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  protected override void Render(double dt)
  {
    void RenderQuad(Wall obstacle)
    {
      Quad.Transform = obstacle.Transform;
      Quad.Mesh!.Material = obstacle.Material;

      Quad.DrawPbr();
    }
    Map.Walls.ForEach(RenderQuad);
    
    Player.Draw();
    Box.DrawRaw();
  }

  private static readonly Quad Quad = new(Fabric);
  private static readonly GameMap Map = new() {
    LayoutInt = new[,] {
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 1, 1, 2, 0, 0, 0, 0, 0 },
      { 0, 0, 1, 3, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 4, 2, 1, 1, 1, 0, 0 },
      { 0, 0, 2, 1, 3, 1, 3, 1, 0, 0 },
      { 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    }
  };
}
}
