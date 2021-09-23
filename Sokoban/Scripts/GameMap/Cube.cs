using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;

namespace Sokoban.Scripts.GameMap
{
public class Cube : GameObject
{
  private static readonly float[] Vertices = {
    //X    Y      Z       Normals             U     V
    -1f, -1f, -1f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
    1f, -1f, -1f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
    1f, 1f, -1f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
    1f, 1f, -1f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
    -1f, 1f, -1f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
    -1f, -1f, -1f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

    -1f, -1f, 1f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
    1f, -1f, 1f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
    1f, 1f, 1f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
    1f, 1f, 1f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
    -1f, 1f, 1f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
    -1f, -1f, 1f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

    -1f, 1f, 1f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
    -1f, 1f, -1f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
    -1f, -1f, -1f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    -1f, -1f, -1f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    -1f, -1f, 1f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
    -1f, 1f, 1f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

    1f, 1f, 1f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
    1f, 1f, -1f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
    1f, -1f, -1f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    1f, -1f, -1f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
    1f, -1f, 1f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
    1f, 1f, 1f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

    -1f, -1f, -1f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
    1f, -1f, -1f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
    1f, -1f, 1f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
    1f, -1f, 1f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
    -1f, -1f, 1f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
    -1f, -1f, -1f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,

    -1f, 1f, -1f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
    1f, 1f, -1f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
    1f, 1f, 1f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
    1f, 1f, 1f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
    -1f, 1f, 1f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
    -1f, 1f, -1f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f
  };

  private static readonly ShaderProgram CubeSpo = new("Cube") {
    Vertex = default,
    Fragment = default,
    ShouldLink = true,
  };
  private static readonly VertexArray CubeVao = new() {
    VertexBuffer = new(Vertices),
    Layout = new(3, 3, 2),
  };
  private void BaseShaderConfiguration()
  {
    Mesh!.Material!.DiffuseMap?.Bind(0);
    Mesh.Material.NormalMap?.Bind(1);

    Spo!.SetUniform("model", Transform.View);
    Spo.SetUniform("material.diffuse", 0);
    Spo.SetUniform("material.specular", 1);

    Spo.SetUniform("material.shininess", Mesh.Material.Shininess);

    if (Mesh.Material.AmbientColor != null) Spo.SetUniform("light.ambient", Mesh.Material.AmbientColor.Value);
    if (Mesh.Material.DiffuseColor != null) Spo.SetUniform("light.diffuse", Mesh.Material.DiffuseColor.Value);
    if (Mesh.Material.SpecularColor != null) Spo.SetUniform("light.specular", Mesh.Material.SpecularColor.Value);
    Spo.SetUniform("light.position", Camera.Position);
  }

  public Cube(Material material)
  {
    Spo = CubeSpo;
    Mesh = new Mesh {
      Material = material,
      Vao = CubeVao,
    };
  }

  public void Draw() => base.Draw(BaseShaderConfiguration);
}
}