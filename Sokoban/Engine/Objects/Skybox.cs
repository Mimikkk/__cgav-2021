#nullable enable
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Objects.Textures;
using Sokoban.Engine.Renderers.Buffers;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts;
using App = Sokoban.Engine.Application.App;
using Shader = Sokoban.Engine.Renderers.Shaders.Shader;

namespace Sokoban.Engine.Objects
{
public class Skybox
{
  private Cubemap CubeMap { get; } = new("skybox");

  private static readonly float[] Vertices = {
    -1.0f, 1.0f, -1.0f,
    -1.0f, -1.0f, -1.0f,
    1.0f, -1.0f, -1.0f,
    1.0f, -1.0f, -1.0f,
    1.0f, 1.0f, -1.0f,
    -1.0f, 1.0f, -1.0f,

    -1.0f, -1.0f, 1.0f,
    -1.0f, -1.0f, -1.0f,
    -1.0f, 1.0f, -1.0f,
    -1.0f, 1.0f, -1.0f,
    -1.0f, 1.0f, 1.0f,
    -1.0f, -1.0f, 1.0f,

    1.0f, -1.0f, -1.0f,
    1.0f, -1.0f, 1.0f,
    1.0f, 1.0f, 1.0f,
    1.0f, 1.0f, 1.0f,
    1.0f, 1.0f, -1.0f,
    1.0f, -1.0f, -1.0f,

    -1.0f, -1.0f, 1.0f,
    -1.0f, 1.0f, 1.0f,
    1.0f, 1.0f, 1.0f,
    1.0f, 1.0f, 1.0f,
    1.0f, -1.0f, 1.0f,
    -1.0f, -1.0f, 1.0f,

    -1.0f, 1.0f, -1.0f,
    1.0f, 1.0f, -1.0f,
    1.0f, 1.0f, 1.0f,
    1.0f, 1.0f, 1.0f,
    -1.0f, 1.0f, 1.0f,
    -1.0f, 1.0f, -1.0f,

    -1.0f, -1.0f, -1.0f,
    -1.0f, -1.0f, 1.0f,
    1.0f, -1.0f, -1.0f,
    1.0f, -1.0f, -1.0f,
    -1.0f, -1.0f, 1.0f,
    1.0f, -1.0f, 1.0f
  };

  private static VertexArrayObject Vao { get; } = new() {
    VertexBufferObject = new VertexBuffer(Vertices),
    Layout = new Layout(3)
  };

  private static UniformBufferObject Ubo { get; } = new() {
    Binding = 0,
    Fields = new Fields(("projection", 16), ("view", 16)),
  };

  private static ShaderProgram Spo = new(
    new Shader(ShaderType.VertexShader, "Skybox"), new Shader(ShaderType.FragmentShader, "Skybox")
  );

  public void ShaderConfiguration()
  {
    Vao.Bind();
    Spo.Bind();
    CubeMap.Bind();

    App.Gl.DepthFunc(DepthFunction.Lequal);

    var projection = Behaviour.Camera.GetProjectionMatrix();
    var view = Matrix4X4.CreateFromQuaternion(Quaternion<float>.CreateFromRotationMatrix(Behaviour.Camera.GetViewMatrix()));

    Ubo.Bind();
    Ubo.SetUniform("projection", projection);
    Ubo.SetUniform("view", view);
  }
}
}