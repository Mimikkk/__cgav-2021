using System;
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

  public Transform Transform { get; set; } = new();

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