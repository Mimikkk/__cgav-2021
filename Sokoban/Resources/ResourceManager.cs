using System;
using System.Collections.Generic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Renderers.Shaders;
using Sokoban.Scripts.Map.Object;
using Framebuffer = Sokoban.Engine.Renderers.Buffers.Objects.Framebuffer;
using Renderbuffer = Sokoban.Engine.Renderers.Buffers.Objects.Renderbuffer;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;

namespace Sokoban.Resources
{
public class ResourceManager
{
  public static readonly List<Light> Lights = new() {
    new() {
      Color = Color.Blue * 50,
      Transform = new() {
        Position = new(20, 10, 20),
      },
    },
    new() {
      Color = Color.Green * 50,
      Transform = new() {
        Position = new(0, 10, 0),
      },
    },
    new() {
      Color = Color.BrightSunny * 50,
      Transform = new() {
        Position = new(0, 10, 20),
      },

    },
    new() {
      Color = Color.Red * 50,
      Transform = new() {
        Position = new(20, 10, 0),
      },
    },
    new() {
      Color = Color.BrightSunny * 250,
      Transform = new() {
        Position = new(12, 13, 0),
      }
    },
    new() {
      Color = Color.Blue * 40,
      Transform = new() {
        Position = new(0, 0, 0),
      }
    }
  };
  public static class Materials
  {
    public static readonly Material Cube = new() {
      DiffuseMap = new("Rock/Color.png"),
      DisplacementMap = new("Fabric/Displacement.png"),
      NormalMap = new("Rock/Normal.png"),
      AmbientOcclusionMap = new("Rock/AmbientOcclusion.png"),
      ReflectionMap = new("Rock/Metallic.png"),
      HeightMap = new("Rock/Roughness.png")
    };

    public static readonly Material Fabric = new() {
      DiffuseMap = new("Fabric/Color.png"),
      NormalMap = new("Fabric/Normal.png"),
      DisplacementMap = new("Fabric/Displacement.png"),
      AmbientOcclusionMap = new("Plastic/AmbientOcclusion.png"),
      ReflectionMap = new("Plastic/Metallic.png"),
      HeightMap = new("Plastic/Roughness.png")
    };
    public static readonly Material Brick = new() {
      DiffuseMap = new("Brick/Color.jpg"),
      NormalMap = new("Brick/Normal.jpg"),
      DisplacementMap = new("Brick/Displacement.jpg"),
      AmbientOcclusionMap = new("Plastic/AmbientOcclusion.png"),
      ReflectionMap = new("Plastic/Metallic.png"),
      HeightMap = new("Plastic/Roughness.png")
    };
    public static readonly Material Rock = new() {
      DiffuseMap = new("Rock/Color.png"),
      NormalMap = new("Rock/Normal.png"),
      DisplacementMap = new("Rock/Displacement.png"),
      AmbientOcclusionMap = new("Plastic/AmbientOcclusion.png"),
      ReflectionMap = new("Plastic/Metallic.png"),
      HeightMap = new("Plastic/Roughness.png")
    };
    public static readonly Material RustedIron = new() {
      DiffuseMap = new("RustedIron/Color.png"),
      NormalMap = new("RustedIron/Normal.png"),
      DisplacementMap = new("Rock/Displacement.png"),
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
    public static readonly Material Gold = new() {
      DiffuseMap = new("Gold/Color.png"),
      NormalMap = new("Gold/Normal.png"),
      AmbientOcclusionMap = new("Gold/AmbientOcclusion.png"),
      ReflectionMap = new("Gold/Metallic.png"),
      HeightMap = new("Gold/Roughness.png")
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
    public static void PbrShaderConfiguration(Material material, Transform transform)
    {
      Pbr.Bind();
      Pbr.SetUniform("albedo_map", 0);
      Pbr.SetUniform("normal_map", 1);
      Pbr.SetUniform("metallic_map", 2);
      Pbr.SetUniform("roughness_map", 3);
      Pbr.SetUniform("ambient_occlusion_map", 4);
      Pbr.SetUniform("irradiance_map", 5);
      Pbr.SetUniform("prefilter_map", 6);
      Pbr.SetUniform("brdf_LUT_map", 7);

      material.DiffuseMap!.Bind(0);
      material.NormalMap!.Bind(1);
      material.ReflectionMap!.Bind(2);
      material.HeightMap!.Bind(3);
      material.AmbientOcclusionMap!.Bind(4);
      Textures.Irradiance.Bind(5);
      Textures.Prefilter.Bind(6);
      Textures.BrdfLUT.Bind(7);

      Pbr.SetUniform("lights[0].position", Camera.Transform.Position);
      Pbr.SetUniform("lights[0].color", Camera.Color);

      for (var i = 1; i < Lights.Count + 1; ++i)
      {
        Pbr.SetUniform($"lights[{i}].position", Lights[i - 1].Transform.Position);
        Pbr.SetUniform($"lights[{i}].color", Lights[i - 1].Color.AsVector3D());
      }

      Pbr.SetUniform("model", transform.View);
    }

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