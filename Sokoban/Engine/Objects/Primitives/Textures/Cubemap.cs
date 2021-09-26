using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Sokoban.Utilities;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Objects.Primitives.Textures
{
public class Cubemap : IDisposable
{
  public string Name { get; }

  public void Bind(int textureSlot = 0)
  {
    App.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
    App.Gl.BindTexture(TextureTarget.TextureCubeMap, Handle);
  }
  public void Dispose() => App.Gl.DeleteTexture(Handle);

  public Cubemap(string name)
  {
    Handle = App.Gl.GenTexture();
    Name = name;
  }

  public PixelFormat PixelFormat { get; init; }
  public PixelType PixelType { get; init; }
  public InternalFormat InternalFormat { get; init; }
  private const TextureTarget Target = TextureTarget.TextureCubeMapPositiveX;

  private void LoadFromFiles()
  {
    Bind();
    for (var i = 0; i < 6; ++i) LoadImage(FileFaces[i], i);
    SetupParameters();
  }

  public void LoadMemory(Vector2D<uint> size)
  {
    Bind();
    for (var i = 0; i < 6; ++i) LoadMemory(size, i);
    SetupParameters();
  }

  private void SetupParameters()
  {
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
    App.Gl.GenerateTextureMipmap(Handle);
  }

  private unsafe void LoadImage(Path path, int index)
  {
    var image = (Image<Rgba32>)Image.Load(path.ToString());
    var (x, y) = ((uint)image.Height, (uint)image.Width);

    fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
      App.Gl.TexImage2D(Target + index, 0, InternalFormat, x, y, 0, PixelFormat, PixelType, data);
  }

  private unsafe void LoadMemory(Vector2D<uint> size, int index)
  {
    App.Gl.TexImage2D(Target + index, 0, InternalFormat, size.X, size.Y, 0, PixelFormat, PixelType, null);
  }

  public bool ShouldLoadFromFile { init => LoadFromFiles(); }

  public uint Handle { get; }
  private Path Path => Filesystem.Textures / Name;
  private IReadOnlyList<Path> FileFaces => new List<Path> {
    Path / "Right.jpg",
    Path / "Left.jpg",
    Path / "Top.jpg",
    Path / "Bottom.jpg",
    Path / "Front.jpg",
    Path / "Back.jpg"
  };
  public void LoadFromFramebuffer(int i, int level, FramebufferAttachment attachment) =>
    App.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, Target + i, Handle, level);
}
}