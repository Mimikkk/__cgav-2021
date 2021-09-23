using System;
using Logger;
using Silk.NET.Maths;
using Sokoban.Engine.Application;

namespace Sokoban.Engine.Objects
{
public static class Camera
{
  public static Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
  public static Quaternion<float> Orientation { get; set; } = Quaternion<float>.Identity;
  public static Quaternion<float> World { get; set; } = Quaternion<float>.Inverse(Quaternion<float>.Identity);

  public static Vector3D<float> Right => Vector3D.Cross(Forward, Up);
  public static Vector3D<float> Up => Matrix3X3.CreateFromQuaternion(Orientation).Column2;
  public static Vector3D<float> Forward => Matrix3X3.CreateFromQuaternion(Orientation).Column3;

  private static float Yaw { get; set; }
  private static float AspectRatio { get; set; } = App.Size.X / (float)App.Size.Y;

  private static float Zoom { get; set; } = 45f;

  public static void ModifyZoom(float zoomAmount) =>
    Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);

  public static void ModifyDirection(Vector2D<float> offset) => ModifyDirection(offset.X, offset.Y);
  public static void ModifyDirection(float x, float y)
  {
    Orientation *= Quaternion<float>.CreateFromYawPitchRoll(x, 0, 0);
    Orientation = Quaternion<float>.CreateFromYawPitchRoll(0, -y, 0) * Orientation;
    Orientation = Quaternion<float>.Normalize(Orientation);
  }

  public static Matrix4X4<float> View =>
    Matrix4X4.CreateLookAt(Position, Position + Forward, Up);

  public static Matrix4X4<float> Projection =>
    Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);
}
}