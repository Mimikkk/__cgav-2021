using System;
using Silk.NET.Maths;

namespace Sokoban.Objects
{
public class Camera
{
  public Vector3D<double> Position { get; set; }
  public Vector3D<double> Front { get; set; }
  public Vector3D<double> Up { get; set; }
  public double AspectRatio { get; set; }

  public double Yaw { get; set; } = -90f;
  public double Pitch { get; set; }
  private double Zoom { get; set; } = 45f;

  public Camera(Vector3D<double> position, Vector3D<double> front, Vector3D<double> up, double aspectRatio)
  {
    Position = position;
    AspectRatio = aspectRatio;
    Front = front;
    Up = up;
  }

  public void ModifyZoom(double zoomAmount) =>
    Zoom = Math.Clamp(Zoom - zoomAmount, 1.0f, 45f);

  public void ModifyDirection(double xOffset, double yOffset)
  {
    Yaw += xOffset;
    Pitch -= yOffset;
    Pitch = Math.Clamp(Pitch, -89f, 89f);

    var cameraDirection = new Vector3D<double> {
      X = Math.Cos(Scalar.DegreesToRadians(Yaw)) * Math.Cos(Scalar.DegreesToRadians(Pitch)),
      Y = Math.Sin(Scalar.DegreesToRadians(Pitch)),
      Z = Math.Sin(Scalar.DegreesToRadians(Yaw)) * Math.Cos(Scalar.DegreesToRadians(Pitch)),
    };
    Front = Vector3D.Normalize(cameraDirection);
  }

  public Matrix4X4<double> GetViewMatrix() =>
    Matrix4X4.CreateLookAt(Position, Position + Front, Up);

  public Matrix4X4<double> GetProjectionMatrix() =>
    Matrix4X4.CreatePerspectiveFieldOfView(Scalar.DegreesToRadians(Zoom), AspectRatio, 0.1f, 100.0f);
}
}