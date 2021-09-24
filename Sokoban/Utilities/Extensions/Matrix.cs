using Silk.NET.Maths;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static T[,] Pad<T>(this T[,] original, Vector2D<int> padding) =>
    original.Pad(padding.X, padding.Y);
  
  public static T[,] Pad<T>(this T[,] original, int x, int y)
  {
    var (n, m) = (original.GetLength(0), original.GetLength(1));
    var padded = new T[n + x, m + y];

    for (var i = 0; i < n; ++i)
    for (var j = 0; j < m; ++j)
      padded[i + 1, j + 1] = original[i, j];

    return padded;
  }
}
}