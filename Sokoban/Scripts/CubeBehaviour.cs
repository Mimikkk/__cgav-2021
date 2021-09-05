﻿using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Engine.Scripts;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts
{
public class CubeBehaviour : MonoBehaviour
{
  protected override void Render(double dt)
  {
    VertexArray.Bind();
    ShaderProgram.Bind();

    Diffuse.Bind(0);
    Specular.Bind(1);

    MatrixUniforms.Bind();
    MatrixUniforms.SetUniform("model", Matrix4X4<float>.Identity);
    MatrixUniforms.SetUniform("view", CameraBehaviour.Camera.View);
    MatrixUniforms.SetUniform("projection", CameraBehaviour.Camera.Projection);
    
    ShaderProgram.SetUniform("viewPos", CameraBehaviour.Camera.Position);

    ShaderProgram.SetUniform("material.diffuse", 0);
    ShaderProgram.SetUniform("material.specular", 1);
    ShaderProgram.SetUniform("material.shininess", 32.0f);

    ShaderProgram.SetUniform("light.ambient", AmbientColor);
    ShaderProgram.SetUniform("light.diffuse", DiffuseColor);
    ShaderProgram.SetUniform("light.specular", Vector3D<float>.One);
    ShaderProgram.SetUniform("light.position", CameraBehaviour.Camera.Position);

    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
  }

  private static readonly float[] Vertices = {
    //X    Y      Z       Normals             U     V
    -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
    0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
    0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
    0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
    -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

    -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
    0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
    0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
    0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
    -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
    -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

    -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
    -0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
    -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    -0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
    -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

    0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
    0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
    0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
    0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

    -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
    0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
    0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
    0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
    -0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,

    -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
    0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
    0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
    0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
    -0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
    -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f
  };

  private static readonly ShaderProgram ShaderProgram = new("Cube") {
    Fragment = default,
    Vertex = default,
    ShouldLink = true,
  };
  private static readonly VertexArray VertexArray = new() {
    VertexBuffer = new(Vertices),
    Layout = new(3, 3, 2),
  };
  private static readonly UniformBuffer MatrixUniforms = new("MatrixBlock") {
    Binding = 1,
    Fields = new(("model", 16), ("view", 16), ("projection", 16)),
  };

  private static readonly Texture Diffuse = new("SilkBoxed.png");
  private static readonly Texture Specular = new("SilkSpecular.png");

  private static readonly Vector3D<float> DiffuseColor = new(0.5f);
  private static readonly Vector3D<float> AmbientColor = new Vector3D<float>(0.2f) * DiffuseColor;
}
}

// namespace Sokoban.Scripts
// {
// public class CubeBehaviour : MonoBehaviour
// {
//   //
//   //
//   // protected override void Render(double dt) => Go.Draw(() => {
//   //   Go.Mesh?.Material?.DiffuseMap?.Bind(0);
//   //   Go.Mesh?.Material?.NormalMap?.Bind(1);
//   //
//   //   MatrixUniforms.Bind();
//   //   MatrixUniforms.SetUniform("model", Matrix4X4<float>.Identity);
//   //   MatrixUniforms.SetUniform("view", Camera.View);
//   //   MatrixUniforms.SetUniform("projection", Camera.Projection);
//   //
//   //   Go.Spo?.SetUniform("viewPos", Camera.Position);
//   //
//   //   Go.Spo?.SetUniform("material.diffuse", 0);
//   //   Go.Spo?.SetUniform("material.specular", 1);
//   //   Go.Spo?.SetUniform("material.shininess", Go.Mesh!.Material!.Shininess);
//   //
//   //   Go.Spo?.SetUniform("light.ambient", Go.Mesh!.Material!.AmbientColor);
//   //   Go.Spo?.SetUniform("light.diffuse", Go.Mesh!.Material!.DiffuseColor);
//   //   Go.Spo?.SetUniform("light.specular", Go.Mesh!.Material!.SpecularColor);
//   //   Go.Spo?.SetUniform("light.position", Camera.Position);
//   // });
//   //
//   // private static readonly float[] Vertices = {
//   //   //X    Y      Z       Normals             U     V
//   //   -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
//   //   0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
//   //   0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
//   //   0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
//   //   -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
//   //   -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
//   //
//   //   -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
//   //   0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
//   //   0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
//   //   0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
//   //   -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
//   //   -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
//   //
//   //   -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
//   //   -0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
//   //   -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
//   //   -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
//   //   -0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
//   //   -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
//   //
//   //   0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
//   //   0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
//   //   0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
//   //   0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
//   //   0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
//   //   0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
//   //
//   //   -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
//   //   0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
//   //   0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
//   //   0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
//   //   -0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
//   //   -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
//   //
//   //   -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
//   //   0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
//   //   0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
//   //   0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
//   //   -0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
//   //   -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f
//   // };
//   //
//   // private static Camera Camera => CameraBehaviour.Camera;
//   // private static readonly GameObject Go = new() {
//   //   Name = "Cube",
//   //   Mesh = new Mesh {
//   //     Material = new Material {
//   //       Shininess = 32,
//   //       DiffuseMap = new("SilkBoxed.png"),
//   //       NormalMap = new("SilkSpecular.png"),
//   //       AmbientColor = new(0.2f),
//   //       DiffuseColor = new(0.5f),
//   //     },
//   //     Vao = new VertexArray {
//   //       VertexBuffer = new(Vertices),
//   //       Layout = new(3, 3, 2),
//   //     },
//   //   },
//   //   Spo = new ShaderProgram("Cube") {
//   //     Fragment = default,
//   //     Vertex = default,
//   //     ShouldLink = true,
//   //   },
//   // };
//   //
//   // private static readonly UniformBuffer MatrixUniforms = new("MatrixBlock") {
//   //   Binding = 1,
//   //   Fields = new(("model", 16), ("view", 16), ("projection", 16)),
//   // };
// }
// }