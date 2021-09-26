using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Resources;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts.Map.Object
{
public class Cube : GameObject
{
  private static readonly float[] Vertices = {
    // Position/Normal/TextureCoordinate

    -1, -1, -1, 0, 0, -1, 0, 0,
    1, 1, -1, 0, 0, -1, 1, 1,
    1, -1, -1, 0, 0, -1, 1, 0,
    1, 1, -1, 0, 0, -1, 1, 1,
    -1, -1, -1, 0, 0, -1, 0, 0,
    -1, 1, -1, 0, 0, -1, 0, 1,

    -1, -1, 1, 0, 0, 1, 0, 0,
    1, -1, 1, 0, 0, 1, 1, 0,
    1, 1, 1, 0, 0, 1, 1, 1,
    1, 1, 1, 0, 0, 1, 1, 1,
    -1, 1, 1, 0, 0, 1, 0, 1,
    -1, -1, 1, 0, 0, 1, 0, 0,

    -1, 1, 1, -1, 0, 0, 1, 0,
    -1, 1, -1, -1, 0, 0, 1, 1,
    -1, -1, -1, -1, 0, 0, 0, 1,
    -1, -1, -1, -1, 0, 0, 0, 1,
    -1, -1, 1, -1, 0, 0, 0, 0,
    -1, 1, 1, -1, 0, 0, 1, 0,

    1, 1, 1, 1, 0, 0, 1, 0,
    1, -1, -1, 1, 0, 0, 0, 1,
    1, 1, -1, 1, 0, 0, 1, 1,
    1, -1, -1, 1, 0, 0, 0, 1,
    1, 1, 1, 1, 0, 0, 1, 0,
    1, -1, 1, 1, 0, 0, 0, 0,

    -1, -1, -1, 0, -1, 0, 0, 1,
    1, -1, -1, 0, -1, 0, 1, 1,
    1, -1, 1, 0, -1, 0, 1, 0,
    1, -1, 1, 0, -1, 0, 1, 0,
    -1, -1, 1, 0, -1, 0, 0, 0,
    -1, -1, -1, 0, -1, 0, 0, 1,

    -1, 1, -1, 0, 1, 0, 0, 1,
    1, 1, 1, 0, 1, 0, 1, 0,
    1, 1, -1, 0, 1, 0, 1, 1,
    1, 1, 1, 0, 1, 0, 1, 0,
    -1, 1, -1, 0, 1, 0, 0, 1,
    -1, 1, 1, 0, 1, 0, 0, 0
  };

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

    Spo.SetUniform("height_scale", Transform.Scale);
    Spo.SetUniform("light_position", Camera.Transform.Position);
    Spo.SetUniform("is_discardable", false);
    Spo.SetUniform("model", Transform.View);
  }

  public Cube(Material material)
  {
    Spo = ResourceManager.ShaderPrograms.ParallaxMapping;
    Mesh = new Mesh {
      Material = material,
      Vao = CubeVao
    };
  }

  public static void DrawRaw()
  {
    CubeVao.Bind();
    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
  }
  public void Draw() => base.Draw(BaseShaderConfiguration);
}
}