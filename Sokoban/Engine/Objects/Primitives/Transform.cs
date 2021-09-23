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
  public float Scale { get; set; } = 1;
  public Quaternion<float> Orientation { get; set; } = Quaternion<float>.Identity;
  public Vector3D<float> Rotation {
    init => Orientation = Quaternion<float>.CreateFromYawPitchRoll(value.X, value.Y, value.Z);
  }
  
  public Matrix4X4<float> View => Matrix4X4.CreateFromQuaternion(Quaternion<float>.Conjugate(Orientation))
                                  * Matrix4X4.CreateScale(Scale)
                                  * Matrix4X4.CreateTranslation(Position);
}
}