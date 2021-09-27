using System;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Renderers.Buffers.Objects;
using Sokoban.Engine.Scripts;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Scripts
{
public class CameraBehaviour : MonoBehaviour
{
  public override LoadPriority LoadPriority => LoadPriority.Critical;

  private static Vector2D<float> LastMousePosition { get; set; }
  private const float LookSensitivity = 0.005f;

  protected override void Start()
  {
    Camera.Transform.Position = new(12, 8, 6);

    Camera.ModifyDirection(MathF.PI / 2, MathF.PI / 4);
    Controller.OnScroll(scroll => Camera.ModifyZoom(scroll.Y));

    Controller.OnHold(Key.W, MoveForwards);
    Controller.OnHold(Key.S, MoveBackwards);
    Controller.OnHold(Key.A, MoveLeft);
    Controller.OnHold(Key.D, MoveRight);

    Controller.OnMove(MaybeRotateXY);
    Controller.OnMove(UpdatePosition);
  }
  protected override void Render(double dt)
  {
    Ubo.Bind();
    Ubo.SetUniform("position", Camera.Transform.Position);
    Ubo.SetUniform("view", Camera.View);
    Ubo.SetUniform("projection", Camera.Projection);
  }

  private static void UpdatePosition(Vector2D<float> position) => LastMousePosition = position;
  private static void MaybeRotateXY(Vector2D<float> position) => (LastMousePosition != default).Then(RotateXY, position);
  private static void RotateXY(Vector2D<float> position) => Camera.ModifyDirection((position - LastMousePosition) * LookSensitivity);

  private static void MoveForwards(float dt) => Camera.Transform.Translate(5 * dt * Camera.Transform.Forward);
  private static void MoveBackwards(float dt) => Camera.Transform.Translate(5 * -dt * Camera.Transform.Forward);
  private static void MoveLeft(float dt) => Camera.Transform.Translate(5 * -dt * Vector3D.Normalize(Vector3D.Cross(Camera.Transform.Forward, Camera.Transform.Up)));
  private static void MoveRight(float dt) => Camera.Transform.Translate(5 * dt * Vector3D.Normalize(Vector3D.Cross(Camera.Transform.Forward, Camera.Transform.Up)));

  private static readonly UniformBuffer Ubo = new("VPBlock") {
    Binding = 0,
    Fields = new(("position", 4), ("view", 16), ("projection", 16))
  };
}
}