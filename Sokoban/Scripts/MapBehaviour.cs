using System;
using Logger;
using Silk.NET.Maths;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.Map;
using Sokoban.Scripts.Map.Object;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Scripts
{
public class MapBehaviour : MonoBehaviour
{
  public static readonly GameMap Map = new() {
    LayoutInt = new[,] {
      { 0, 0, 0, 0, 0, 0, 2, 0, 2, 0 },
      { 0, 0, 0, 0, 0, 0, 2, 0, 2, 0 },
      { 0, 0, 1, 1, 2, 1, 0, 0, 0, 1 },
      { 0, 0, 1, 3, 0, 0, 1, 2, 1, 0 },
      { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
      { 0, 0, 0, 4, 2, 1, 0, 0, 0, 0 },
      { 0, 0, 2, 1, 3, 1, 3, 1, 0, 0 },
      { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
      { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    }
  };

  private static double Angle = 0;
  protected override void Render(double dt)
  {
    Angle = (Angle + dt) % Math.Tau;
    
    int x, y;
    Map.Walls.ForEach(w => w.Draw());
    Map.BoxLocations.ForEach(location => {
      var (x, y) = (location.X, location.Y);
      Box.Draw(new(2 * x, 0, 2 * y), Vector3D<float>.Zero);
    });
    for (var index = 0; index < Map.TargetLocations.Count; index++)
    {
      var target = Map.TargetLocations[index];
      (x, y) = (target.X, target.Y);
      var rotation = new Vector3D<float>[
      ] {
        new((float)Angle / 7.0f, (float)Angle / 19.0f, (float)Angle),
        new(0, (float)Angle, (float)Angle / 12.0f),
        new((float)Angle, 0, (float)Angle),
      }[index % 3];

      Target.Draw(new(2 * x, (float)Math.Atan2(x, y) + 0.5f * (float)Math.Cos(Angle), 2 * y), rotation);
    }

    (x, y) = (Map.PlayerLocation.X, Map.PlayerLocation.Y);
    Player.Draw(new(2 * x, 1, 2 * y), Vector3D<float>.Zero);
  }
}
}