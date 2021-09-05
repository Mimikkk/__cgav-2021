using System;
using Silk.NET.Maths;

namespace Sokoban.Engine.Objects
{
public class Camera
{
  public Vector3D<float> Position { get; set; }
  public Vector3D<float> Front { get; set; }
  public Vector3D<float> Up { get; set; }
  public float AspectRatio { get; set; }

  public float Yaw { get; set; } = -90f;
  public float Pitch { get; set; }
  private float Zoom { get; set; } = 45f;

  public Camera(Vector3D<float> position, Vector3D<float> front, Vector3D<float> up)
  {
    Position = position;
    AspectRatio = 800.0f / 700.0f;
    Front = front;
    Up = up;
  }

  public void ModifyZoom(float zoomAmount) =>
    Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);

  public void ModifyDirection(Vector2D<float> offset) => ModifyDirection(offset.X, offset.Y);
  public void ModifyDirection(float x, float y)
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

  public Matrix4X4<float> View =>
    Matrix4X4.CreateLookAt(Position, Position + Front, Up);

  public Matrix4X4<float> Projection =>
    Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);
}
}