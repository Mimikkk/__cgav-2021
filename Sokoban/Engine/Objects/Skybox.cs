#nullable enable
using Silk.NET.OpenGL;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Resources;
using Sokoban.Scripts.Map.Object;

namespace Sokoban.Engine.Objects
{
public static class Skybox
{
  public static readonly Cubemap CubeMap = new("Skybox") {
    InternalFormat = InternalFormat.Rgba,
    PixelFormat = PixelFormat.Rgba,
    PixelType = PixelType.UnsignedByte,
    ShouldLoadFromFile = true,
  };

  public static readonly ShaderProgram Spo = ResourceManager.ShaderPrograms.Background;

  public static void Draw()
  {
    Spo.Bind();
    CubeMap.Bind();
    Spo.SetUniform("environment_map", 0);

    Cube.DrawRaw();
  }
}
}