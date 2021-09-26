using System;
using System.Collections.Generic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts.Map;
using Framebuffer = Sokoban.Engine.Renderers.Buffers.Objects.Framebuffer;
using Renderbuffer = Sokoban.Engine.Renderers.Buffers.Objects.Renderbuffer;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;

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
    public static readonly Material RustedIron = new() {
      DiffuseMap = new("RustedIron/Color.png"),
      NormalMap = new("RustedIron/Normal.png"),
      AmbientOcclusionMap = new("RustedIron/AmbientOcclusion.png"),
      ReflectionMap = new("RustedIron/Metallic.png"),
      HeightMap = new("RustedIron/Roughness.png")
    };
    public static readonly Material Plastic = new() {
      DiffuseMap = new("Plastic/Color.png"),
      NormalMap = new("Plastic/Normal.png"),
      AmbientOcclusionMap = new("Plastic/AmbientOcclusion.png"),
      ReflectionMap = new("Plastic/Metallic.png"),
      HeightMap = new("Plastic/Roughness.png")
    };
  }
  public static class Textures
  {
    public static Cubemap Irradiance { get; } = new("Irradiance") {
      InternalFormat = InternalFormat.Rgb16f,
      PixelFormat = PixelFormat.Rgb,
      PixelType = PixelType.Float,
    };
    public static Cubemap Prefilter { get; } = new("Prefilter") {
      InternalFormat = InternalFormat.Rgb16f,
      PixelFormat = PixelFormat.Rgb,
      PixelType = PixelType.Float,
    };
    public static Texture BrdfLUT { get; } = new("BrdfLUT", false) {
      InternalFormat = InternalFormat.RG16f,
      PixelFormat = PixelFormat.RG,
      PixelType = PixelType.Float,
    };

    private static readonly Matrix4X4<float> CaptureProjection = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 2, 1.0f, 0.1f, 10.0f);
    private static readonly IReadOnlyList<Matrix4X4<float>> CaptureViews = new List<Matrix4X4<float>> {
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitX, -Vector3D<float>.UnitY),
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitX, -Vector3D<float>.UnitY),
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitY, Vector3D<float>.UnitZ),
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitY, -Vector3D<float>.UnitZ),
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitZ, -Vector3D<float>.UnitY),
      Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitZ, -Vector3D<float>.UnitY),
    };

    private static Framebuffer CaptureFbo { get; } = new();
    private static Renderbuffer CaptureRbo { get; } = new();

    private static readonly Vector2D<uint> CaptureSize = new(512, 512);

    static Textures()
    {
      Skybox.Spo.Bind();
      Skybox.Spo.SetUniform("environment_map", 0);

      Skybox.CubeMap.Bind();
      App.Gl.GenerateMipmap(TextureTarget.TextureCubeMap);

      var IrradianceSize = new Vector2D<uint>(32, 32);
      Irradiance.Bind();
      Irradiance.LoadMemory(IrradianceSize);
      App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);

      CaptureFbo.Bind();
      CaptureRbo.Bind();
      CaptureRbo.Store(IrradianceSize, InternalFormat.DepthComponent24);

      ShaderPrograms.Irradiance.Bind();
      ShaderPrograms.Irradiance.SetUniform("environment_map", 0);
      ShaderPrograms.Irradiance.SetUniform("projection", CaptureProjection);
      Skybox.CubeMap.Bind();

      App.Gl.Viewport(0, 0, IrradianceSize.X, IrradianceSize.Y);
      CaptureFbo.Bind();
      for (var i = 0; i < 6; ++i)
      {
        ShaderPrograms.Irradiance.SetUniform("view", CaptureViews[i]);
        Irradiance.LoadFromFramebuffer(i, 0, FramebufferAttachment.ColorAttachment0);
        App.Clear();
        Cube.DrawRaw();
      }
      CaptureFbo.Unbind();

      var PrefilterSize = new Vector2D<uint>(128, 128);
      Prefilter.LoadMemory(PrefilterSize);
      App.Gl.GenerateMipmap(GLEnum.TextureCubeMap);

      ShaderPrograms.Prefilter.Bind();
      ShaderPrograms.Prefilter.SetUniform("environment_map", 0);
      ShaderPrograms.Prefilter.SetUniform("projection", CaptureProjection);

      Skybox.CubeMap.Bind();
      CaptureFbo.Bind();

      const uint mipLevels = 5;
      for (var mip = 0; mip < mipLevels; ++mip)
      {
        var width = (uint)(PrefilterSize.X * MathF.Pow(0.5f, mip));
        var height = (uint)(PrefilterSize.Y * MathF.Pow(0.5f, mip));

        CaptureRbo.Bind();
        CaptureRbo.Store(width, height, InternalFormat.DepthComponent24);
        App.Gl.Viewport(0, 0, width, height);

        var roughness = mip / (float)(mipLevels - 1);
        ShaderPrograms.Prefilter.SetUniform("roughness", roughness);
        for (var i = 0; i < 6; ++i)
        {
          ShaderPrograms.Prefilter.SetUniform("view", CaptureViews[i]);
          Prefilter.LoadFromFramebuffer(i, mip, FramebufferAttachment.ColorAttachment0);

          App.Clear();
          Cube.DrawRaw();
        }
      }
      CaptureFbo.Unbind();

      BrdfLUT.Bind(0);
      BrdfLUT.LoadMemory(CaptureSize);
      App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
      App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);

      CaptureRbo.Bind();
      CaptureFbo.Bind();
      CaptureRbo.Store(CaptureSize, InternalFormat.DepthComponent24);
      BrdfLUT.LoadFromFramebuffer(0, FramebufferAttachment.ColorAttachment0);

      App.Gl.Viewport(0, 0, CaptureSize.X, CaptureSize.Y);
      ShaderPrograms.Brdf.Bind();
      App.Clear();
      Quad.DrawRaw();
      CaptureFbo.Unbind();

      App.Gl.Viewport(0, 0, (uint)App.Size.X, (uint)App.Size.Y);
    }
  }
  public static class ShaderPrograms
  {
    public static readonly ShaderProgram Background = new("Background") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
    public static readonly ShaderProgram ParallaxMapping = new("ParallaxMapping") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };
    public static readonly ShaderProgram Pbr = new("Pbr") {
      Vertex = default,
      Fragment = default,
      ShouldLink = true
    };

    public static readonly ShaderProgram Irradiance = new("IrradianceConvolution") {
      Vertex = "Cubemap",
      Fragment = default,
      ShouldLink = true,
    };
    public static readonly ShaderProgram Prefilter = new("Prefilter") {
      Vertex = "Cubemap",
      Fragment = default,
      ShouldLink = true,
    };
    public static readonly ShaderProgram Brdf = new("Brdf") {
      Fragment = default,
      Vertex = default,
      ShouldLink = true,
    };
  }
}
}