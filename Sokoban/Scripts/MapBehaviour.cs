using Silk.NET.Input;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.Map;
using Sokoban.Utilities.Extensions;
using static Sokoban.Resources.ResourceManager.Materials;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => HeightScale = HeightScale + dt);
    Controller.OnHold(Key.G, dt => HeightScale = HeightScale - dt);
    Controller.OnClick(MouseButton.Middle, _ => {
      if (Quad.Mesh!.Material!.Equals(Brick))
      {
        Quad.Mesh.Material = Rock;
      } else if (Quad.Mesh!.Material!.Equals(Rock))
      {
        Quad.Mesh.Material = Fabric;
      } else if (Quad.Mesh!.Material!.Equals(Fabric))
      {
        Quad.Mesh.Material = Brick;
      }
    });
  }
  protected override void Render(double dt)
  {
    void RenderQuad(Wall obstacle)
    {
      Quad.Transform = obstacle.Transform;
      Quad.Mesh!.Material = obstacle.Material;

      Quad.Draw(() => Quad.Spo!.SetUniform("height_scale", HeightScale));
    }

    Map.Walls.ForEach(RenderQuad);
  }

  private static float HeightScale = 1f;

  private static readonly Quad Quad = new(Fabric);
  private static readonly GameMap Map = new() {
    Layout = new[,] {
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Empty, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall },
      { SpaceType.Wall, SpaceType.Wall, SpaceType.Wall, SpaceType.Empty, SpaceType.Wall, SpaceType.Wall }
    }
  };
}
}