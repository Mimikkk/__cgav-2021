using Silk.NET.Maths;

namespace Sokoban.Engine.Objects.Primitives
{
public class Transform
{
  public Transform OffsetBy(Vector3D<float> position) => new() {
    Orientation = Orientation,
    Position = Position + position,
    Scale = Scale
  };

  public Vector3D<float> Position { get; set; } = Vector3D<float>.Zero;
  public float Scale { get; set; } = 1f;
  public Quaternion<float> Orientation { get; set; } = Quaternion<float>.Identity;
  public Vector3D<float> Rotation {
    init => Orientation = Quaternion<float>.CreateFromYawPitchRoll(value.X, value.Y, value.Z);
  }

  public Vector3D<float> Right => Matrix3X3.CreateFromQuaternion(Orientation).Column1;
  public Vector3D<float> Up => Matrix3X3.CreateFromQuaternion(Orientation).Column2;
  public Vector3D<float> Forward => Matrix3X3.CreateFromQuaternion(Orientation).Column3;

  public void Translate(Vector3D<float> offset) =>
    Position += offset;
  public void Translate(float x, float y, float z) =>
    Translate(new(x, y, z));

  public void Rotate(Vector3D<float> angles) =>
    Rotate(angles.X, angles.Y, angles.Z);
  public void Rotate(float x, float y, float z) =>
    Orientation *= Quaternion<float>.CreateFromYawPitchRoll(x, y, z);

  public void RotateLocal(Vector3D<float> angles) =>
    RotateLocal(angles.X, angles.Y, angles.Z);
  public void RotateLocal(float x, float y, float z) =>
    Orientation = Quaternion<float>.CreateFromYawPitchRoll(x, y, z) * Orientation;

  public Matrix4X4<float> View => Matrix4X4.CreateFromQuaternion(Orientation)
                                  * Matrix4X4.CreateScale(Scale)
                                  * Matrix4X4.CreateTranslation(Position);
}
}