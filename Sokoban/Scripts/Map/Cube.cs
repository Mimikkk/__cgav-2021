using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Resources;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;

namespace Sokoban.Scripts.Map
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

  private static readonly ShaderProgram CubeSpo = ResourceManager.ShaderPrograms.PBR;
  private static readonly VertexArray CubeVao = new() {
    VertexBuffer = new(Vertices),
    Layout = new(3, 3, 2)
  };
  private void BaseShaderConfiguration()
  {
    Mesh!.Material!.DiffuseMap?.Bind(0);
    Mesh.Material.NormalMap?.Bind(1);
    Mesh.Material.DisplacementMap?.Bind(2);

    Spo!.SetUniform("diffuse_map", 0);
    Spo.SetUniform("normal_map", 1);
    Spo.SetUniform("displacement_map", 2);

    Spo.SetUniform("height_scale",  Transform.Scale);
    Spo.SetUniform("light_position", Camera.Transform.Position);
    Spo.SetUniform("is_discardable", false);
    Spo.SetUniform("model", Transform.View);
  }

  public Cube(Material material)
  {
    Spo = CubeSpo;
    Mesh = new Mesh {
      Material = material,
      Vao = CubeVao
    };
  }

  public void Draw() => base.Draw(BaseShaderConfiguration);
}
}