﻿using Silk.NET.Input;
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
  private const float LookSensitivity = 0.1f;

  protected override void Start()
  {
    Camera.Position = new(6, 0, 0);
    Camera.ModifyDirection(-90, 0);

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
    Ubo.SetUniform("view", Camera.View);
    Ubo.SetUniform("projection", Camera.Projection);
  }

  private static void UpdatePosition(Vector2D<float> position) => LastMousePosition = position;
  private static void MaybeRotateXY(Vector2D<float> position) => (LastMousePosition != default).Then(RotateXY, position);
  private static void RotateXY(Vector2D<float> position) => Camera.ModifyDirection((position - LastMousePosition) * LookSensitivity);

  private static void MoveForwards(double dt) => Camera.Position += (float)dt * Camera.Front;
  private static void MoveBackwards(double dt) => Camera.Position -= (float)dt * Camera.Front;
  private static void MoveLeft(double dt) => Camera.Position -= (float)dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up));
  private static void MoveRight(double dt) => Camera.Position += (float)dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up));

  private static readonly UniformBuffer Ubo = new("VPBlock") {
    Binding = 0,
    Fields = new(("view", 16), ("projection", 16))
  };
}
}