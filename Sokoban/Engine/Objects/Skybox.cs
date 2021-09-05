#nullable enable
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Buffers.Helpers;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts;
using App = Sokoban.Engine.Application.App;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

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

  private static readonly VertexArray Vao = new() {
    VertexBuffer = new VertexBuffer(Vertices),
    Layout = new Layout(3)
  };

  private static readonly UniformBuffer Ubo = new("SkyboxBlock") {
    Fields = new Fields(("view", 16), ("projection", 16))
  };

  private static readonly ShaderProgram Spo = new("Skybox") {
    Vertex = default,
    Fragment = default,
    ShouldLink = true
  };

  public void ShaderConfiguration()
  {
    App.Gl.DepthFunc(DepthFunction.Lequal);

    Vao.Bind();
    Spo.Bind();
    CubeMap.Bind();

    Ubo.Bind();
    Ubo.SetUniform("view", Matrix4X4.CreateFromQuaternion(Quaternion<float>.CreateFromRotationMatrix(CameraBehaviour.Camera.View)));
    Ubo.SetUniform("projection", CameraBehaviour.Camera.Projection);
  }
}
}