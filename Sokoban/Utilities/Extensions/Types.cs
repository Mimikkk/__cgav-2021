﻿using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Maths;

namespace Sokoban.Utilities.Extensions
{
public static partial class Extension
{
  public static Vector2D<double> ToVector2D(this ScrollWheel wheel) => new(wheel.X, wheel.Y);
  public static Vector2D<double> ToVector2D(this Vector2 vector) => new(vector.X, vector.Y);
  public static Vector3D<double> ToVector3D(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
  public static Vector4D<double> ToVector4D(this Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
}
}