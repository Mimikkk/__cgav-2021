using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Application;
using Sokoban.Controllers;
using Sokoban.Objects;
using Sokoban.Renderers.Buffers;
using Sokoban.Renderers.Shaders;
using Shader = Sokoban.Renderers.Shaders.Shader;

namespace Sokoban
{
public class Behaviour
{
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

  private static Skybox Skybox = null!;
  public static Camera Camera = null!;
  private static ShaderProgram ShaderProgram = null!;
  private static VertexArrayObject VertexArrayObject = null!;
  private static Vector2D<float> LastMousePosition { get; set; }

  public static void Start()
  {
    Controller.OnRelease(Key.Escape, App.Window.Close);

    Camera = new Camera(Vector3D<float>.UnitZ * 6, Vector3D<float>.UnitZ * -1, Vector3D<float>.UnitY);
    Skybox = new Skybox();

    Controller.OnScroll(Camera.ModifyDirection);
    Controller.OnHold(Key.W, dt => Camera.Position += dt * Camera.Front);
    Controller.OnHold(Key.S, dt => Camera.Position -= dt * Camera.Front);
    Controller.OnHold(Key.A, dt => Camera.Position -= dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up)));
    Controller.OnHold(Key.D, dt => Camera.Position += dt * Vector3D.Normalize(Vector3D.Cross(Camera.Front, Camera.Up)));
    Controller.OnMove(position => {
      const float lookSensitivity = 0.1f;
      if (LastMousePosition == default) { LastMousePosition = position; } else
      {
        var xOffset = (position.X - LastMousePosition.X) * lookSensitivity;
        var yOffset = (position.Y - LastMousePosition.Y) * lookSensitivity;
        LastMousePosition = position;

        Camera.ModifyDirection(xOffset, yOffset);
      }
    });

    VertexArrayObject = new VertexArrayObject {
      VertexBufferObject = new VertexBuffer(Vertices),
      IndexBufferObject = new IndexBuffer(Indices),
      Layout = new ElementLayout(3),
    };

    ShaderProgram = new ShaderProgram(
      new Shader(ShaderType.FragmentShader, "Basic"),
      new Shader(ShaderType.VertexShader, "Basic")
    );
  }
  public static unsafe void Render(float dt)
  {
    App.Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

    Skybox.ShaderConfiguration();
    App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);

    ShaderProgram.Bind();
    VertexArrayObject.Bind();
    App.Gl.DrawElements(PrimitiveType.Triangles, VertexArrayObject.Size, DrawElementsType.UnsignedInt, null);
  }
}
}