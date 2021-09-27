using System;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Resources;
using Camera = Sokoban.Engine.Objects.Camera;
using Material = Sokoban.Engine.Objects.Primitives.Textures.Material;
using Mesh = Sokoban.Engine.Objects.Primitives.Mesh;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts.Map.Object
{
public class Quad : GameObject
{
  public static void DrawRaw()
  {
    QuadVao.Bind();
    App.Gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 6);
  }
  public Quad(Material material)
  {
    Spo = ResourceManager.ShaderPrograms.ParallaxMapping;
    Mesh = new Mesh {
      Material = material,
      Vao = QuadVao
    };
  }

  private static readonly float[] Vertices = {
    -1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
    -1, -1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0,
    1, -1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0,
    -1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0,
    1, -1, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0,
    1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0
  };

  private static readonly VertexArray QuadVao = new() {
    VertexBuffer = new(Vertices),
    Layout = new(3, 3, 2, 3, 3)
  };
}
}