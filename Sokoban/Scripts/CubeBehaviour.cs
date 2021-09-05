using Silk.NET.Maths;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Engine.Scripts;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts
{
public class CubeBehaviour : MonoBehaviour
{
  protected override void Render(double dt) => Go.Draw(() => {
    if (Go.Mesh?.Material == null || Go.Spo == null) return;

    Go.Mesh.Material.DiffuseMap?.Bind(0);
    Go.Mesh.Material.NormalMap?.Bind(1);

    Go.Spo.SetUniform("model", Go.View);

    Go.Spo.SetUniform("material.diffuse", 0);
    Go.Spo.SetUniform("material.specular", 1);

    Go.Spo.SetUniform("material.shininess", Go.Mesh.Material.Shininess);

    if (Go.Mesh.Material.AmbientColor != null) Go.Spo.SetUniform("light.ambient", Go.Mesh.Material.AmbientColor.Value);
    if (Go.Mesh.Material.DiffuseColor != null) Go.Spo.SetUniform("light.diffuse", Go.Mesh.Material.DiffuseColor.Value);
    if (Go.Mesh.Material.SpecularColor != null) Go.Spo.SetUniform("light.specular", Go.Mesh.Material.SpecularColor.Value);
    Go.Spo.SetUniform("light.position", Camera.Position);
  });

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

  private static readonly GameObject Go = new() {
    Position = Vector3D<float>.One,
    Spo = new ShaderProgram("Cube") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true,
    },
    Mesh = new Mesh {
      Material = new Material {
        DiffuseMap = new("SilkBoxed.png"),
        NormalMap = new("SilkSpecular.png"),
        AmbientColor = new(0.1f, 0.1f, 0.1f, 1.0f),
        DiffuseColor = new(0.5f, 0.5f, 0.5f, 1.0f),
        SpecularColor = Vector4D<float>.One,
        Shininess = 4,
      },
      Vao = new VertexArray {
        VertexBuffer = new(Vertices),
        Layout = new(3, 3, 2),
      },
    },
  };
}
}