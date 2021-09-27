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
  public void Draw(Action additionalShaderConfiguration) => base.Draw(() => {
    // BaseShaderConfiguration();
    additionalShaderConfiguration();
  });
  public static void DrawRaw()
  {
    QuadVao.Bind();
    App.Gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
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
  private void BaseShaderConfiguration()
  {
    Mesh!.Material!.DiffuseMap?.Bind(0);
    Mesh.Material.NormalMap?.Bind(1);
    Mesh.Material.DisplacementMap?.Bind(2);

    Spo!.SetUniform("diffuse_map", 0);
    Spo.SetUniform("normal_map", 1);
    Spo.SetUniform("displacement_map", 2);

    Spo.SetUniform("model", Transform.View);
    Spo.SetUniform("light_position", Camera.Transform.Position);
    Spo.SetUniform("is_discardable", false);
  }
}
}