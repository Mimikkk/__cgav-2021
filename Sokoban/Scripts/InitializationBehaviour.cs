using Silk.NET.OpenGL;
using Sokoban.Engine.Renderers.Buffers.Helpers;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Engine.Scripts;
using App = Sokoban.Engine.Application.App;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Scripts
{
internal class InitializationBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.Normal;

  private static readonly float[] Vertices = {
    0.5f, 0.5f, 0.0f,
    0.5f, -0.5f, 0.0f,
    -0.5f, -0.5f, 0.0f,
    -0.5f, 0.5f, 0.5f
  };
  private static readonly uint[] Indices = {
    0, 1, 3,
    1, 2, 3
  };

  private static ShaderProgram ShaderProgram = null!;
  private static VertexArray VertexArray = null!;

  protected override void Start()
  {
    VertexArray = new VertexArray {
      VertexBuffer = new VertexBuffer(Vertices),
      IndexBuffer = new IndexBuffer(Indices),
      Layout = new Layout(3)
    };
    ShaderProgram = new ShaderProgram("Basic") {
      Fragment = default,
      Vertex = default,
      ShouldLink = true
    };
  }
  protected override unsafe void Render(double dt)
  {
    ShaderProgram.Bind();
    VertexArray.Bind();
    App.Gl.DrawElements(PrimitiveType.Triangles, VertexArray.Size, DrawElementsType.UnsignedInt, null);
  }
}
}