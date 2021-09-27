using System;
using Silk.NET.Maths;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects.Primitives;

namespace Sokoban.Engine.Objects
{
public static class Camera
{
  public static Transform Transform { get; set; } = new();
  public static Color Color { get; set; } = new(255, 255, 255);
  private static float AspectRatio => App.Size.X / (float)App.Size.Y;

  private static float Zoom { get; set; } = 45f;

  public static void ModifyZoom(float zoomAmount) =>
    Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);

  public static void ModifyDirection(Vector2D<float> offset) => ModifyDirection(offset.X, offset.Y);
  public static void ModifyDirection(float x, float y)
  {
    Transform.Rotate(x, 0, 0);
    Transform.RotateLocal(0, -y, 0);
  }

  public static Matrix4X4<float> View =>
    Matrix4X4.CreateLookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);

  public static Matrix4X4<float> Projection =>
    Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);
}
}