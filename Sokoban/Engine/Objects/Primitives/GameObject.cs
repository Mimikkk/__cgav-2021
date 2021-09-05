using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts;

namespace Sokoban.Engine.Objects.Primitives
{
public class GameObject
{
  public string Name { get; init; } = "Unnamed";

  public Mesh? Mesh { get; init; }
  public ShaderProgram? Spo { get; set; } = ShaderProgram.Default;

  public Vector3D<float> Position { get; set; } = new(0, 0, 0);
  public Quaternion<float> Rotation { get; set; } = Quaternion<float>.Identity;
  public float Scale { get; set; } = 1f;

  public Matrix4X4<float> ViewMatrix => Matrix4X4<float>.Identity
                                        * Matrix4X4.CreateFromQuaternion(Rotation)
                                        * Matrix4X4.CreateScale(Scale)
                                        * Matrix4X4.CreateTranslation(Position);

  public void ShaderConfiguration()
  {
    Mesh?.Vao.Bind();
    Spo?.Bind();

    Mesh?.Material?.DiffuseTexture?.Bind(0);
    Mesh?.Material?.NormalTexture?.Bind(1);
    Mesh?.Material?.DisplacementTexture?.Bind(2);

    Spo?.SetUniform("diffuseMap", 0);
    Spo?.SetUniform("normalMap", 1);
    Spo?.SetUniform("depthMap", 2);

    Spo?.SetUniform("projection", CameraBehaviour.Camera.GetProjectionMatrix());
    Spo?.SetUniform("view", CameraBehaviour.Camera.GetViewMatrix());
    Spo?.SetUniform("model", ViewMatrix);

    Spo?.SetUniform("viewPos", CameraBehaviour.Camera.Position);
    Spo?.SetUniform("lightPos", Vector3D<float>.One);
    Spo?.SetUniform("heightScale", Scale);
  }
  public unsafe void Draw()
  {
    ShaderConfiguration();
    if (Mesh != null) App.Gl.DrawElements(PrimitiveType.Triangles, Mesh.Vao.Size, DrawElementsType.UnsignedInt, null);
  }
}
}