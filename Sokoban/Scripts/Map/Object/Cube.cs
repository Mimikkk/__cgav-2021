using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
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
}
}