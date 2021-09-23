using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Renderers.Shaders;

namespace Sokoban.Engine.Objects.Primitives
{
public class GameObject
{
  public string Name { get; init; } = "Unnamed";

  public Mesh? Mesh { get; init; }
  public ShaderProgram? Spo { get; set; }

  public Vector3D<float> Position { get; set; } = new(0, 0, 0);
  public Quaternion<float> Orientation { get; set; } = Quaternion<float>.Identity;
  public float Scale { get; set; } = 1f;

  public Matrix4X4<float> View => Matrix4X4<float>.Identity
                                  * Matrix4X4.CreateFromQuaternion(Quaternion<float>.Conjugate(Orientation))
                                  * Matrix4X4.CreateScale(Scale)
                                  * Matrix4X4.CreateTranslation(Position);

  public unsafe void Draw(Action shaderConfiguration, bool customBufferLogic = false)
  {
    if (Mesh == null || Spo == null) return;
    if (!customBufferLogic)
    {
      Spo.Bind();
      Mesh.Vao.Bind();
    }

    shaderConfiguration();

    if (Mesh.IndexCount > 0)
      App.Gl.DrawElements(PrimitiveType.Triangles, Mesh.IndexCount, DrawElementsType.UnsignedInt, null);
    else
      App.Gl.DrawArrays(PrimitiveType.Triangles, 0, Mesh.VertexCount);
  }
}
}