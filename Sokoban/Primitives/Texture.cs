using System;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Sokoban.Utilities;

namespace Sokoban.Primitives
{
public class Texture : IDisposable
{
  public string Name { get; }
  public Texture(string name)
  {
    Handle = Application.Gl.GenTexture();
    Name = name;
    Load();
  }

  public void Bind(int textureSlot = 0)
  {
    Application.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
    Application.Gl.BindTexture(TextureTarget.Texture2D, Handle);
  }
  public void Dispose() => Application.Gl.DeleteTexture(Handle);

  private uint Handle { get; }
  private Path Path => Filesystem.Textures / Name;

  private unsafe void Load()
  {
    Bind();
    const PixelFormat format = PixelFormat.Rgba;

    var image = (Image<Rgba32>)Image.Load(Path.ToString());
    image.Mutate(x => x.Flip(FlipMode.Horizontal));
    fixed (void* data = &MemoryMarshal.GetReference(image.GetPixelRowSpan(0)))
      Application.Gl.TexImage2D(TextureTarget.Texture2D, 0, (int)format,
        (uint)image.Width, (uint)image.Height, 0, format, PixelType.UnsignedByte, data);

    Application.Gl.GenerateTextureMipmap(Handle);
    Application.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
    Application.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
    Application.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)GLEnum.Repeat);
    Application.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
    Application.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
  }

  public static Texture Missing => new("Missing.png");
}
}