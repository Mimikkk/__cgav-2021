using System.Collections.Generic;

namespace Sokoban.Scripts.GameMap
{
public readonly struct Map
{
  private int[,] Layout { get; }

  public Map(int[,] layout)
  {
    Layout = layout;
  }
  
  // public static ToRender = new List<(Direction, (int x,int y))>();
}
}