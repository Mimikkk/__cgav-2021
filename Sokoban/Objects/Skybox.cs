#nullable enable
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Application;
using Sokoban.Objects.Textures;
using Sokoban.Renderers.Buffers;
using Sokoban.Renderers.Shaders;
using Shader = Sokoban.Renderers.Shaders.Shader;

namespace Sokoban.Objects
{
public class Skybox
{
  private Cubemap Map { get; } = new("skybox");

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
    Layout = new ElementLayout(3)
  };
  
  private static ShaderProgram Spo = new(
    new Renderers.Shaders.Shader(ShaderType.VertexShader, "Skybox"), new Renderers.Shaders.Shader(ShaderType.FragmentShader, "Skybox")
  );

  public void ShaderConfiguration()
  {
    Vao.Bind();
    Spo.Bind();
    Map.Bind();

    App.Gl.DepthFunc(DepthFunction.Lequal);
    Spo.SetUniform("projection", Behaviour.Camera.GetProjectionMatrix());
    Spo.SetUniform("view", Matrix4X4.CreateFromQuaternion(Quaternion<float>.CreateFromRotationMatrix(Behaviour.Camera.GetViewMatrix())));
  }
}
}