using System;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Scripts;
using Sokoban.Scripts.Map;
using Sokoban.Scripts.Map.Object;

namespace Sokoban.Scripts
{
public class PlayerBehaviour : MonoBehaviour
{
  protected override void Start()
  {

    Controller.OnHold(Key.Keypad8, _ => MaybeStartLerp(Direction.Forward));

    Controller.OnHold(Key.Keypad2, _ => MaybeStartLerp(Direction.Backward));
    Controller.OnHold(Key.Keypad6, _ => MaybeStartLerp(Direction.Right));
    Controller.OnHold(Key.Keypad4, _ => MaybeStartLerp(Direction.Left));

    Controller.OnHold(Key.KeypadMultiply, dt => Player.Transform.Rotate(dt, 0, 0));
    Controller.OnHold(Key.KeypadDivide, dt => Player.Transform.Rotate(-dt, 0, 0));

    Controller.OnHold(Key.KeypadAdd, dt => Player.Transform.Rotate(0, dt, 0));
    Controller.OnHold(Key.KeypadSubtract, dt => Player.Transform.Rotate(0, -dt, 0));

    Controller.OnHold(Key.Keypad1, dt => Player.Transform.Rotate(0, 0, dt));
    Controller.OnHold(Key.Keypad3, dt => Player.Transform.Rotate(0, 0, -dt));
  }

  private static double Timer;
  private static bool BobbingDirection;
  protected override void Update(double dt)
  {
    Lerp(dt);

    if (Timer + dt > Math.Tau) BobbingDirection = !BobbingDirection;
    Timer = (Timer + dt) % Math.Tau;
    var prev = Player.Transform;
    Player.Transform.Position += 0.005f * Vector3D<float>.UnitY * MathF.Sin((float)Timer);
  }

  private static double ElapsedLerpTime;
  private static bool IsMoving;
  private static Direction LerpDirection;

  private static readonly Vector3D<float> Forwards = 2 * Vector3D<float>.UnitX;
  private static readonly Vector3D<float> Backwards = -Forwards;

  private static readonly Vector3D<float> Right = 2 * Vector3D<float>.UnitZ;
  private static readonly Vector3D<float> Left = -Right;

  private static Vector3D<float> OldPosition = Vector3D<float>.Zero;
  private static Vector3D<float> TargetPosition = Vector3D<float>.Zero;

  private static float LerpTime {
    get {
      var t = ElapsedLerpTime / LerpDuration;
      return (float)(t * t * (3f - 2f * t));
    }
  }
  private const double LerpDuration = 1f;

  private static Vector3D<float> TargetPositionOffset(Direction direction)
  {
    return direction switch {
      Direction.Forward  => Forwards,
      Direction.Backward => Backwards,
      Direction.Right    => Right,
      Direction.Left     => Left,
    };
  }

  private static Vector2D<int> TargetLocationOffset(Direction direction)
  {
    return direction switch {
      Direction.Forward  => new(1, 0),
      Direction.Backward => new(-1, 0),
      Direction.Right    => new(0, 1),
      Direction.Left     => new(0, -1),
    };
  }
  private static bool HasCollisions(Vector2D<int> target) => MapBehaviour.Map.BoxLocations.Contains(target) || MapBehaviour.Map[target] == SpaceType.Wall;
  private static void MaybeStartLerp(Direction direction)
  {
    if (IsMoving) return;
    if (HasCollisions(MapBehaviour.Map.PlayerLocation + TargetLocationOffset(direction)))
    {
      "Would Collide!".LogLine();
      return;
    }
    StartLerp(direction);
  }
  private static void StartLerp(Direction direction)
  {
    LerpDirection = direction;
    OldPosition = Player.Transform.Position;
    TargetPosition = OldPosition + TargetPositionOffset(LerpDirection);
    IsMoving = true;
  }
  
  private static void Lerp(double dt)
  {
    Vector3D<float> LerpMove()
    {
      return LerpDirection switch {
        Direction.Forward or Direction.Backward => new(Vector3D.Lerp(OldPosition, TargetPosition, LerpTime).X, Player.Transform.Position.Y,
          Player.Transform.Position.Z),
        Direction.Right or Direction.Left => new(Player.Transform.Position.X, Player.Transform.Position.Y,
          Vector3D.Lerp(OldPosition, TargetPosition, LerpTime).Z),
        _ => throw new ArgumentOutOfRangeException()
      };
    }
    Vector3D<float> LerpFinale()
    {
      return LerpDirection switch {
        Direction.Forward or Direction.Backward => new Vector3D<float>(OldPosition.X, Player.Transform.Position.Y, Player.Transform.Position.Z),
        Direction.Right or Direction.Left       => new Vector3D<float>(Player.Transform.Position.X, Player.Transform.Position.Y, OldPosition.Z),
        _                                       => throw new ArgumentOutOfRangeException()
      };
    }


    if (!IsMoving) return;

    if (ElapsedLerpTime > LerpDuration)
    {
      Player.Transform.Position = LerpFinale();
      MapBehaviour.Map.PlayerLocation += TargetLocationOffset(LerpDirection);

      ElapsedLerpTime = 0;
      IsMoving = false;
    } else
    {
      Player.Transform.Position = LerpMove();
      ElapsedLerpTime += dt;
    }
  }
}
}