using System;
using Silk.NET.Maths;
using Sokoban.Engine.Application;

namespace Sokoban.Engine.Objects
{
public static class Camera
{
  public static Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
  public static Quaternion<float> Orientation { get; set; } = Quaternion<float>.Identity;

  public static Vector3D<float> Front { get; set; } = -Vector3D<float>.UnitZ;
  public static Vector3D<float> Up { get; set; } = Vector3D<float>.UnitY;
  private static float AspectRatio { get; set; } = App.Size.X / (float)App.Size.Y;


  private static float Yaw { get; set; } = -90f;
  private static float Pitch { get; set; }
  private static float Zoom { get; set; } = 45f;

  public static void ModifyZoom(float zoomAmount) =>
    Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);

  public static void ModifyDirection(Vector2D<float> offset) => ModifyDirection(offset.X, offset.Y);
  public static void ModifyDirection(float x, float y)
  {
    Yaw += x;
    Pitch -= y;
    Pitch = Math.Clamp(Pitch, -89f, 89f);

    var cameraDirection = new Vector3D<float> {
      X = MathF.Cos(Scalar.DegreesToRadians(Yaw)) * MathF.Cos(Scalar.DegreesToRadians(Pitch)),
      Y = MathF.Sin(Scalar.DegreesToRadians(Pitch)),
      Z = MathF.Sin(Scalar.DegreesToRadians(Yaw)) * MathF.Cos(Scalar.DegreesToRadians(Pitch))
    };
    Front = Vector3D.Normalize(cameraDirection);
  }

  public static Matrix4X4<float> View =>
    Matrix4X4.CreateLookAt(Position, Position + Front, Up);

  public static Matrix4X4<float> Projection =>
    Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);
}
}