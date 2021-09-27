using System;
using System.Collections.Generic;
using System.Linq;
using Silk.NET.Maths;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map.Object;

namespace Sokoban.Scripts
{
public class LightsBehaviour : MonoBehaviour
{
  private static double Angle = 0;

  private static double AngleOffset(int index)
  {
    return (Angle + index * ResourceManager.Lights.Count / Math.PI) % Math.Tau;
  }


  protected override void Start()
  {
    Colors.AddRange(ResourceManager.Lights.Select(x => x.Color));
  }
  private static readonly List<Color> Colors = new();

  protected override void Update(double dt)
  {
    const int radius = 12;
    var lights = ResourceManager.Lights;

    Angle = (Angle + dt) % Math.Tau;

    for (var index = 0; index < lights.Count - 2; index++)
    {
      var light = lights[index];


      var x = radius * Math.Sin(AngleOffset(index));
      var y = radius * Math.Cos(AngleOffset(index));

      light.Transform.Position = new(radius + (float)x, 5, radius + (float)y);
      light.Color = Colors[index] * (float)(Math.Cos(Angle) * Math.Cos(Angle));
    }
    
    var a = lights.Last();
    var (n, m) = (MapBehaviour.Map.PlayerLocation.X, MapBehaviour.Map.PlayerLocation.Y);
    a.Transform.Position = Player.Transform.Position + new Vector3D<float>(2 * n, 1, 2 * m);
  }
}
}