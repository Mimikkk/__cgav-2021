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

  protected override void Render(double dt)
  {
    Map.Walls.ForEach(w => w.Draw());
    Map.BoxLocations.ForEach(location => {
      var (x, y) = (location.X, location.Y);
      Box.Draw(new(2 * x, 0, 2 * y), Vector3D<float>.Zero);
    });
    Map.TargetLocations.ForEach(location => {
      var (x, y) = (location.X, location.Y);

      Target.Transform.Position = new Vector3D<float>(2 * x, 0, 2 * y);
      Target.Draw(new(2 * x, 0, 2 * y), Vector3D<float>.Zero);
    });
    var (x, y) = (Map.PlayerLocation.X, Map.PlayerLocation.Y);
    Player.Draw(new(2 * x, 1, 2 * y), Vector3D<float>.Zero);
  }
}
}