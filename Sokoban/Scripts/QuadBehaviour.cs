using System;
using Assimp;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Scripts;
using Camera = Sokoban.Engine.Objects.Camera;

namespace Sokoban.Scripts
{
public class QuadBehaviour : MonoBehaviour
{
  protected override void Start()
  {
    Controller.OnHold(Key.T, dt => HeightScale = (float)(HeightScale + dt));
    Controller.OnHold(Key.G, dt => HeightScale = (float)(HeightScale - dt));
  }


  protected override void Render(double dt)
  {
    var model = Matrix4X4<float>.Identity;
    for (int i = 0; i < 5; i++)
    {
      model *= Matrix4X4.CreateTranslation(2 * Vector3D<float>.UnitX);

      Go.Draw(() => {
        if (Go.Mesh?.Material == null || Go.Spo == null) return;

        Go.Mesh.Material.DiffuseMap?.Bind(0);
        Go.Mesh.Material.NormalMap?.Bind(1);
        Go.Mesh.Material.DisplacementMap?.Bind(2);

        Go.Spo.SetUniform("diffuse_map", 0);
        Go.Spo.SetUniform("normal_map", 1);
        Go.Spo.SetUniform("displacement_map", 2);

        Go.Spo.SetUniform("model", model);
        Go.Spo.SetUniform("light_position", Camera.Position);
        Go.Spo.SetUniform("height_scale", HeightScale);
        Go.Spo.SetUniform("is_discardable", false);

      });
    }
  }

  private static float HeightScale = 1f;

  private static readonly float[] Vertices = {
    -1f, 1f, 0f, 0f, 0f, 1f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
    -1f, -1f, 0f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    -1f, 1f, 0f, 0f, 0f, 1f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, -1f, 0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 0f, 1f, 0f,
    1f, 1f, 0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f, 0f, 0f, 1f, 0f,
  };

  private static readonly GameObject Go = new() {
    Mesh = new() {
      Material = new() {
        DiffuseMap = new("Rock/Color.png"),
        NormalMap = new("Rock/Normal.png"),
        DisplacementMap = new("Rock/Displacement.png"),
      },
      Vao = new() {
        VertexBuffer = new(Vertices),
        Layout = new(3, 3, 2, 3, 3)
      }
    },
    Spo = new("PBR") {
      Fragment = default,
      Vertex = default,
      ShouldLink = true
    },
  };
}
}