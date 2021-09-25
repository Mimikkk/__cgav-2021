﻿using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Shaders;

namespace Sokoban.Resources
{
public class ResourceManager
{
  public static class Materials
  {
    public static readonly Material Fabric = new() {
      DiffuseMap = new("Fabric/Color.png"),
      NormalMap = new("Fabric/Normal.png"),
      DisplacementMap = new("Fabric/Displacement.png")
    };
    public static readonly Material Brick = new() {
      DiffuseMap = new("Brick/Color.jpg"),
      NormalMap = new("Brick/Normal.jpg"),
      DisplacementMap = new("Brick/Displacement.jpg")
    };
    public static readonly Material Rock = new() {
      DiffuseMap = new("Rock/Color.png"),
      NormalMap = new("Rock/Normal.png"),
      DisplacementMap = new("Rock/Displacement.png")
    };
  }
  public static class ShaderPrograms
  {
    public static readonly ShaderProgram Skybox = new("Skybox") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
    public static readonly ShaderProgram PBR = new("PBR") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
    public static readonly ShaderProgram Box = new("Box") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
    public static readonly ShaderProgram Target = new("Target") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
  }
}
}