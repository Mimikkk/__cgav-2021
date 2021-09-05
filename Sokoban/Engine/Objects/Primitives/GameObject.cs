﻿using System;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts;

namespace Sokoban.Engine.Objects.Primitives
{
public class GameObject
{
  public string Name { get; init; } = "Unnamed";

  public Mesh? Mesh { get; init; }
  public ShaderProgram? Spo { get; set; } = ShaderProgram.Default;

  public Vector3D<float> Position { get; set; } = new(0, 0, 0);
  public Quaternion<float> Rotation { get; set; } = Quaternion<float>.Identity;
  public float Scale { get; set; } = 1f;

  public Matrix4X4<float> View => Matrix4X4<float>.Identity
                                  * Matrix4X4.CreateFromQuaternion(Rotation)
                                  * Matrix4X4.CreateScale(Scale)
                                  * Matrix4X4.CreateTranslation(Position);

  public void Draw(Action shaderConfiguration)
  {
    if (Mesh == null || Spo == null) return;
    Spo.Bind();
    Mesh.Vao.Bind();

    shaderConfiguration();

    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
    // App.Gl.DrawElements(PrimitiveType.Triangles, Mesh.Vao.Size, DrawElementsType.UnsignedInt, null);
  }
}
}