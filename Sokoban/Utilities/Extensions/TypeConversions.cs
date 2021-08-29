using System;
using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static Vector2D<float> ToVector2D(this ScrollWheel wheel) => new(wheel.X, wheel.Y);
  public static Vector2D<float> ToVector2D(this Vector2 vector) => new(vector.X, vector.Y);
  public static Vector3D<float> ToVector3D(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
  public static Vector4D<float> ToVector4D(this Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);

  public static T[] ToArray<T>(this Vector2D<T> vector) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T> =>
    new[] { vector.X, vector.Y };
  public static T[] ToArray<T>(this Vector3D<T> vector) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T> =>
    new[] { vector.X, vector.Y, vector.Z };
  public static T[] ToArray<T>(this Vector4D<T> vector) where T : unmanaged, IFormattable, IEquatable<T>, IComparable<T> =>
    new[] { vector.X, vector.Y, vector.Z, vector.W };
}
}