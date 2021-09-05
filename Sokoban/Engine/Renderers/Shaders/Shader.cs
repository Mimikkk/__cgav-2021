using System;
using Logger;
using Silk.NET.OpenGL;
using Sokoban.Utilities;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Shaders
{
public class Shader : IDisposable
{
  public override string ToString()
  {
    return $"Shader({Name}: {Type})";
  }

  public Shader(ShaderType type, string name)
  {
    Type = type;
    Name = name;
    Handle = App.Gl.CreateShader(Type);

    App.Gl.ShaderSource(Handle, Source);
    App.Gl.CompileShader(Handle);
    VerifyCompilation();
  }

  private void VerifyCompilation()
  {
    var infoLog = App.Gl.GetShaderInfoLog(Handle);
    if (string.IsNullOrWhiteSpace(infoLog)) return;
    $"<c4 Shader {Name}|>::<c6 Error compiling shader of type|> <c124 {Type}|>, <c6 failed with a message|> <c124 {infoLog}|>".LogLine();
    throw new Exception();
  }

  public uint Handle { get; }
  public ShaderType Type { get; }
  public string Name { get; }

  private string Source => Path.LoadFileToString();
  private Path Path => Filesystem.Shaders / $"{Name}{Extension}";
  private string Extension => Type switch {
    ShaderType.FragmentShader       => ".frag",
    ShaderType.VertexShader         => ".vert",
    ShaderType.GeometryShader       => ".geom",
    ShaderType.TessEvaluationShader => ".tese",
    ShaderType.TessControlShader    => ".tesc",
    ShaderType.ComputeShader        => ".comp",
    _                               => throw new ArgumentOutOfRangeException($"Unsupported ShaderType : {Type}")
  };

  public void Dispose()
  {
    App.Gl.DeleteShader(Handle);
  }
}
}