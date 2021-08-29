using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Sokoban.Utilities;
using Sokoban.Utilities.Extensions;
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
    Load();
  }

  private void Load()
  {
    Bind();
    Faces.ForEach(LoadImage);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
    App.Gl.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
    App.Gl.GenerateTextureMipmap(Handle);
  }

  private unsafe void LoadImage(Path path, int index)
  {
    var image = (Image<Rgba32>)Image.Load(path.ToString());
    fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
      App.Gl.TexImage2D(TextureTarget.TextureCubeMapPositiveX + index, 0, (int)InternalFormat.Rgba,
        (uint)image.Width, (uint)image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
  }

  private uint Handle { get; }
  private Path Path => Filesystem.Textures / Name;
  private IReadOnlyList<Path> Faces => new List<Path> {
    Path / "Right.jpg",
    Path / "Left.jpg",
    Path / "Top.jpg",
    Path / "Bottom.jpg",
    Path / "Front.jpg",
    Path / "Back.jpg"
  };
}
}