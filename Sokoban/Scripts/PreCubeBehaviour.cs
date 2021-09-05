using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Engine.Scripts;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts
{
public class PreCubeBehaviour : MonoBehaviour
{
  protected override void Render(double dt)
  {
    VertexArray.Bind();
    ShaderProgram.Bind();

    Diffuse.Bind(0);
    Specular.Bind(1);

    ShaderProgram.SetUniform("model", Matrix4X4<float>.Identity);
    
    ShaderProgram.SetUniform("material.diffuse", 0);
    ShaderProgram.SetUniform("material.specular", 1);
    ShaderProgram.SetUniform("material.shininess", 32.0f);

    ShaderProgram.SetUniform("light.ambient", AmbientColor);
    ShaderProgram.SetUniform("light.diffuse", DiffuseColor);
    ShaderProgram.SetUniform("light.specular", Vector3D<float>.One);
    ShaderProgram.SetUniform("light.position", Camera.Position);

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

  private static readonly Texture Diffuse = new("SilkBoxed.png");
  private static readonly Texture Specular = new("SilkSpecular.png");

  private static readonly Vector3D<float> DiffuseColor = new(0.5f);
  private static readonly Vector3D<float> AmbientColor = new Vector3D<float>(0.2f) * DiffuseColor;
}
}
