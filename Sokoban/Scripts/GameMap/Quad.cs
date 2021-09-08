using System;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;

namespace Sokoban.Scripts.GameMap
{
public class Quad : GameObject
{
  public void Draw(Action additionalShaderConfiguration) => base.Draw(() => {
    BaseShaderConfiguration();
    additionalShaderConfiguration();
  });

  public Quad(Material material)
  {
    Spo = QuadSpo;
    Mesh = new Mesh {
      Material = material,
      Vao = QuadVao,
    };
  }

  private static readonly float[] Vertices = {
    -1f, 1f, 0f, 0f, 0f, 1f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
    -1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    -1f, 1f, 0f, 0f, 0f, 1f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, 1f, 0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
  };
  private static readonly ShaderProgram QuadSpo = new("Quad") {
    Vertex = "PBR",
    Fragment = "PBR",
    ShouldLink = true,
  };
  private static readonly VertexArray QuadVao = new() {
    VertexBuffer = new(Vertices),
    Layout = new(3, 3, 2, 3, 3)
  };
  private void BaseShaderConfiguration()
  {
    Mesh!.Material!.DiffuseMap?.Bind(0);
    Mesh.Material.NormalMap?.Bind(1);
    Mesh.Material.DisplacementMap?.Bind(2);

    Spo!.SetUniform("diffuse_map", 0);
    Spo.SetUniform("normal_map", 1);
    Spo.SetUniform("displacement_map", 2);

    Spo.SetUniform("light_position", Camera.Position);
    Spo.SetUniform("is_discardable", false);
  }
}
}