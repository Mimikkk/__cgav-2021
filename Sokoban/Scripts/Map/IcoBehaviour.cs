using System;
using System.Collections.Generic;
using Silk.NET.Maths;
using Sokoban.Engine.Scripts;

namespace Sokoban.Scripts.Map
{
public class IcoBehaviour : MonoBehaviour
{
  private static GameMap Map => MapBehaviour.Map;
  private static List<Vector2D<int>> IcoLocations => Map.TargetLocations;

  protected override void Update(double dt)
  {
  }
}
}