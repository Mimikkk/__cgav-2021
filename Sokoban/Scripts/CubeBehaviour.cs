using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Engine.Application;
using Sokoban.Engine.Controllers;
using Sokoban.Engine.Objects;
using Sokoban.Engine.Objects.Loaders;
using Sokoban.Engine.Objects.Primitives;
using Sokoban.Engine.Objects.Primitives.Textures;
using Sokoban.Engine.Scripts;
using Sokoban.Resources;
using Sokoban.Scripts.Map;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;
using Renderbuffer = Sokoban.Engine.Renderers.Buffers.Objects.Renderbuffer;
using Framebuffer = Sokoban.Engine.Renderers.Buffers.Objects.Framebuffer;
using Texture = Sokoban.Engine.Objects.Primitives.Textures.Texture;

namespace Sokoban.Scripts
{
public partial class CubeBehaviour : MonoBehaviour
{
  private static readonly Cube Cube = new(ResourceManager.Materials.Brick);
  private static readonly GameObject Box = ObjectLoader.Load("Cube").First();

  private static void SetupController()
  {
    Controller.OnHold(Key.T, dt => Cube.Transform.Scale += dt);
    Controller.OnHold(Key.G, dt => Cube.Transform.Scale -= dt);

    Controller.OnHold(Key.Keypad4, dt => Cube.Transform.Translate(dt, 0, 0));
    Controller.OnHold(Key.Keypad6, dt => Cube.Transform.Translate(-dt, 0, 0));
    Controller.OnHold(Key.Keypad9, dt => Cube.Transform.Translate(0, dt, 0));
    Controller.OnHold(Key.Keypad7, dt => Cube.Transform.Translate(0, -dt, 0));
    Controller.OnHold(Key.Keypad8, dt => Cube.Transform.Translate(0, 0, dt));
    Controller.OnHold(Key.Keypad2, dt => Cube.Transform.Translate(0, 0, -dt));

    Controller.OnHold(Key.KeypadMultiply, dt => Cube.Transform.Rotate(dt, 0, 0));
    Controller.OnHold(Key.KeypadDivide, dt => Cube.Transform.Rotate(-dt, 0, 0));

    Controller.OnHold(Key.KeypadAdd, dt => Cube.Transform.Rotate(0, dt, 0));
    Controller.OnHold(Key.KeypadSubtract, dt => Cube.Transform.Rotate(0, -dt, 0));

    Controller.OnHold(Key.Keypad1, dt => Cube.Transform.Rotate(0, 0, dt));
    Controller.OnHold(Key.Keypad3, dt => Cube.Transform.Rotate(0, 0, -dt));
  }
  private static void SetupCube()
  {
    Box.Transform.Position = new(6, 3, 6);
    Box.Spo = ResourceManager.ShaderPrograms.Pbr;
    Box.Mesh!.Material = ResourceManager.Materials.RustedIron;
  }

  protected override void Start()
  {
    SetupCube();

    SetupMiddleware();

    SetupController();
  }
  protected override void Render(double dt)
  {
    // Cube.Draw();
    Box.Draw(() => {
      Box.Mesh!.Material!.DiffuseMap!.Bind(0);
      Box.Mesh!.Material!.NormalMap!.Bind(1);
      Box.Mesh!.Material!.ReflectionMap!.Bind(2);
      Box.Mesh!.Material!.HeightMap!.Bind(3);
      Box.Mesh!.Material!.AmbientOcclusionMap!.Bind(4);
      Irradiance.Bind(5);
      Prefilter.Bind(6);
      BrdfLUT.Bind(7);
    
      Box.Spo.SetUniform("albedo_map", 0);
      Box.Spo.SetUniform("normal_map", 1);
      Box.Spo.SetUniform("metallic_map", 2);
      Box.Spo.SetUniform("roughness_map", 3);
      Box.Spo.SetUniform("ambient_occlusion_map", 4);
   
      Box.Spo.SetUniform("irradiance_map" ,5);
      Box.Spo.SetUniform("prefilter_map" ,6);
      Box.Spo.SetUniform("brdf_LUT_map" ,7);
    
      Box.Spo!.SetUniform("light_positions[0]", Camera.Transform.Position);
      Box.Spo!.SetUniform("light_positions[1]", new Vector3D<float>(6, 5, 5.5f));
      Box.Spo!.SetUniform("light_colors[0]", new Vector3D<float>(100, 100, 100));
      Box.Spo!.SetUniform("light_colors[1]", new Vector3D<float>(0, 0, 100));
    
      Box.Spo!.SetUniform("model", Box.Transform.View);
    });

    Skybox.Spo.Bind();

    ResourceManager.ShaderPrograms.Background.Bind();
    App.Gl.DepthFunc(DepthFunction.Lequal);
    Skybox.CubeMap.Bind(0);
    Skybox.Spo.SetUniform("environment_map", 0);

    Cube.DrawRaw();
  }
}

public partial class CubeBehaviour : MonoBehaviour
{
  private static Matrix4X4<float> CaptureProjection = Matrix4X4.CreatePerspectiveFieldOfView(MathF.PI / 2, 1.0f, 0.1f, 10.0f);
  private static IReadOnlyList<Matrix4X4<float>> CaptureViews = new List<Matrix4X4<float>> {
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitX, -Vector3D<float>.UnitY),
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitX, -Vector3D<float>.UnitY),
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitY, Vector3D<float>.UnitZ),
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitY, -Vector3D<float>.UnitZ),
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, Vector3D<float>.UnitZ, -Vector3D<float>.UnitY),
    Matrix4X4.CreateLookAt(Vector3D<float>.Zero, -Vector3D<float>.UnitZ, -Vector3D<float>.UnitY),
  };
  private static List<Vector3D<float>> LightPositions = new() {
    new(-10, 10, 10),
  };
  private static List<Vector3D<float>> LightColors = new() {
    new(100, 100, 100),
  };

  private static Framebuffer CaptureFbo { get; set; } = new();
  private static Renderbuffer CaptureRbo { get; set; } = new();
  private static HDRTexture HDR { get; set; } = new("NewportLoft");
  private static Cubemap Environment { get; set; } = new("Environment") {
    InternalFormat = InternalFormat.Rgb16f,
    PixelFormat = PixelFormat.Rgb,
    PixelType = PixelType.Float,
  };
  private static Cubemap Irradiance { get; set; } = new("Irradiance") {
    InternalFormat = InternalFormat.Rgb16f,
    PixelFormat = PixelFormat.Rgb,
    PixelType = PixelType.Float,
  };
  private static Cubemap Prefilter { get; set; } = new("Prefilter") {
    InternalFormat = InternalFormat.Rgb16f,
    PixelFormat = PixelFormat.Rgb,
    PixelType = PixelType.Float,
  };
  private static Texture BrdfLUT { get; set; } = new("BrdfLUT", false) {
    InternalFormat = InternalFormat.RG16f,
    PixelFormat = PixelFormat.RG,
    PixelType = PixelType.Float,
  };

  private static readonly Vector2D<uint> CaptureSize = new(512, 512);

  private static void SetupMiddleware()
  {
    ResourceManager.ShaderPrograms.Background.Bind();
    ResourceManager.ShaderPrograms.Background.SetUniform("environment_map", 0);
    Skybox.CubeMap.Bind(0);

    CaptureFbo.Bind();
    CaptureRbo.Bind();
    CaptureRbo.Store(CaptureSize, InternalFormat.DepthComponent24);
    CaptureRbo.Pin(FramebufferAttachment.DepthAttachment);
    Environment.LoadMemory(CaptureSize);

    ResourceManager.ShaderPrograms.EquirectangularToCubemap.Bind();

    HDR.Bind(0);
    ResourceManager.ShaderPrograms.EquirectangularToCubemap.SetUniform("equirectangular_map", 0);
    ResourceManager.ShaderPrograms.EquirectangularToCubemap.SetUniform("projection", CaptureProjection);

    App.Gl.Viewport(0, 0, CaptureSize.X, CaptureSize.Y);
    CaptureFbo.Bind();
    for (var i = 0; i < 6; ++i)
    {
      ResourceManager.ShaderPrograms.EquirectangularToCubemap.SetUniform("view", CaptureViews[i]);
      Environment.LoadFromFramebuffer(i, 0, FramebufferAttachment.ColorAttachment0);
      App.Clear();
      Cube.DrawRaw();
    }
    CaptureFbo.Unbind();

    Environment.Bind();
    App.Gl.GenerateMipmap(TextureTarget.TextureCubeMap);


    var IrradianceSize = new Vector2D<uint>(32, 32);
    Irradiance.LoadMemory(IrradianceSize);

    App.Gl.Viewport(0, 0, IrradianceSize.X, IrradianceSize.Y);
    CaptureFbo.Bind();
    CaptureRbo.Bind();
    CaptureRbo.Store(IrradianceSize, InternalFormat.DepthComponent24);

    ResourceManager.ShaderPrograms.Irradiance.Bind();

    Environment.Bind();
    ResourceManager.ShaderPrograms.Irradiance.SetUniform("environment_map", 0);
    ResourceManager.ShaderPrograms.Irradiance.SetUniform("projection", CaptureProjection);
    for (var i = 0; i < 6; ++i)
    {
      ResourceManager.ShaderPrograms.Irradiance.SetUniform("view", CaptureViews[i]);
      Irradiance.LoadFromFramebuffer(i, 0, FramebufferAttachment.ColorAttachment0);
      App.Clear();
      Cube.DrawRaw();
    }
    CaptureFbo.Unbind();

    var PrefilterSize = new Vector2D<uint>(128, 128);
    Prefilter.LoadMemory(PrefilterSize);

    ResourceManager.ShaderPrograms.Prefilter.Bind();
    ResourceManager.ShaderPrograms.Prefilter.SetUniform("environment_map", 0);
    ResourceManager.ShaderPrograms.Prefilter.SetUniform("projection", CaptureProjection);
    Environment.Bind();
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
      ResourceManager.ShaderPrograms.Prefilter.SetUniform("roughness", roughness);
      for (var i = 0; i < 6; ++i)
      {
        ResourceManager.ShaderPrograms.Prefilter.SetUniform("view", CaptureViews[i]);
        Prefilter.LoadFromFramebuffer(i, mip, FramebufferAttachment.ColorAttachment0);

        App.Clear();
        Cube.DrawRaw();
      }
    }
    CaptureFbo.Unbind();

    BrdfLUT.Bind(0);
    BrdfLUT.LoadMemory(512, 512); // AccessError at 512...

    CaptureRbo.Bind();
    CaptureFbo.Bind();
    CaptureRbo.Store(new(512, 512), InternalFormat.DepthComponent24);
    BrdfLUT.LoadFromFramebuffer(0, FramebufferAttachment.ColorAttachment0);

    App.Gl.Viewport(0, 0, 512, 512);
    ResourceManager.ShaderPrograms.Brdf.Bind();
    App.Clear();
    Quad.DrawRaw();
    CaptureFbo.Unbind();

    App.Gl.Viewport(0, 0, 800, 700);

  }
}
}