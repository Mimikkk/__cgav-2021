using System;
using System.Runtime.InteropServices;
using Logger;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Sokoban.Utilities;
using Sokoban.Utilities.Extensions;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Objects.Primitives.Textures
{
public class Texture : IDisposable
{
  public string Name { get; }

  public void Bind(int textureSlot)
  {
    App.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
    App.Gl.BindTexture(TextureTarget.Texture2D, Handle);
  }
  public void Dispose() => App.Gl.DeleteTexture(Handle);

  public static Texture Missing => new("Missing.png");

  public Texture(string name)
  {
    Handle = App.Gl.GenTexture();
    Name = name;
    Load();
  }

  private void Load()
  {
    Bind(0);
    LoadImage();
    App.Gl.GenerateTextureMipmap(Handle);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
  }
  private unsafe void LoadImage()
  {

    var image = (Image<Rgba32>)Image.Load(Path.IsFile() ? Path.ToString() : (Filesystem.Textures / "Missing.png").ToString());

    var format = image.PixelType.BitsPerPixel switch {
      8  => PixelFormat.Red,
      24 => PixelFormat.Rgb,
      _  => PixelFormat.Rgba,
    };

    fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
      App.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)format,
        (uint)image.Width, (uint)image.Height, 0, format, PixelType.UnsignedByte, data);
  }

  private uint Handle { get; }
  private Path Path => Filesystem.Textures / Name;
}
}