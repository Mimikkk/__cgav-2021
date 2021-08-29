using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Scripts;
using Sokoban.Utilities.Extensions;

namespace Sokoban.Scripts
{
public class CameraBehaviour : MonoBehaviour
{
  public static readonly Camera Camera = new(Vector3D<float>.UnitZ * 6, Vector3D<float>.UnitZ * -1, Vector3D<float>.UnitY);
  private static Vector2D<float> LastMousePosition { get; set; }
  private const float LookSensitivity = 0.1f;

  protected override void Start()
  {
    Controller.OnScroll(Camera.ModifyDirection);

    Controller.OnHold(Key.W, MoveForwards);
    Controller.OnHold(Key.S, MoveBackwards);
    Controller.OnHold(Key.A, MoveLeft);
    Controller.OnHold(Key.D, MoveRight);

    Controller.OnMove(MaybeRotateXY);
    Controller.OnMove(UpdatePosition);
  }

  private static void UpdatePosition(Vector2D<float> position) => LastMousePosition = position;

  private static void MaybeRotateXY(Vector2D<float> position) => (LastMousePosition != default).Then(RotateXY, position);
  private static void RotateXY(Vector2D<float> position) => Camera.ModifyDirection((position - LastMousePosition) * LookSensitivity);

  private static void MoveForwards(double dt) => Camera.Position += (float)dt * Camera.Front;
  private static void MoveBackwards(double dt) => Camera.Position -= (float)dt * Camera.Front;
  private static void MoveLeft(double dt) => Camera.Position -= (float)dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up));
  private static void MoveRight(double dt) => Camera.Position += (float)dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up));
}
}