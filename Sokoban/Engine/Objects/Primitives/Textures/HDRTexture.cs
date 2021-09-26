using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Sokoban.Engine.Application;
using Sokoban.Utilities;
using Sokoban.Utilities.Extensions;
using StbImageSharp;
using Path = Sokoban.Utilities.Path;

namespace Sokoban.Engine.Objects.Primitives.Textures
{
public class HDRTexture
{
  public string Name { get; }

  public void Bind(int textureSlot = 0)
  {
    App.Gl.ActiveTexture(TextureUnit.Texture0 + textureSlot);
    App.Gl.BindTexture(TextureTarget.Texture2D, Handle);
  }

  public void Dispose() => App.Gl.DeleteTexture(Handle);

  public HDRTexture(string name)
  {
    Handle = App.Gl.GenTexture();
    Name = name;
    Load();
  }

  private unsafe void Load()
  {
    Bind();
    using var stream = File.OpenRead(Path.ToString());

    var image = ImageResult.FromStream(stream);
    fixed (void* data = image.Data.AsSpan())
      App.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb16f, (uint)image.Width,
        (uint)image.Height, 0, PixelFormat.Rgb, PixelType.Float, data);

    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
    App.Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
  }

  private uint Handle { get; }
  private Path Path => Filesystem.Textures / (Name + ".hdr");
}
}