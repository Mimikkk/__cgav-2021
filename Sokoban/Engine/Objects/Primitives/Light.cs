using Silk.NET.Maths;

namespace Sokoban.Engine.Objects.Primitives
{
public class Light
{
  public Transform Transform { get; set; } = new();
  public Color Color { get; set; } = Color.BrightWhite;
}

public record Color(float R, float G, float B)
{
  public static readonly Color BrightWhite = new(255, 255, 255);
  public static readonly Color DimWhite = BrightWhite * 0.2f;

  public static readonly Color BrightSunny = new(R: 255, G: 219, B: 109);

  public static readonly Color Red = new(255, 0, 0);
  public static readonly Color Green = new(0, 255, 0);
  public static readonly Color Blue = new(0, 0, 255);


  public Color(Vector3D<float> vector)
    : this(vector.X, vector.Y, vector.Z) { }

  public Vector3D<float> AsVector3D() => new(R, G, B);
  public static Color operator *(Color color, float value) => new(color.AsVector3D() * value);
};
}