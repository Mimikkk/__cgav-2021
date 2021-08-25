using System;
using System.IO;
using System.Numerics;
using Silk.NET.OpenGL;

namespace Sokoban.Renderers.Shaders
{
public class Shader : IDisposable
{
  public Shader(string vertexPath, string fragmentPath)
  {
    uint vertex = LoadShader(ShaderType.VertexShader, vertexPath);
    uint fragment = LoadShader(ShaderType.FragmentShader, fragmentPath);

    Handle = Application.Gl.CreateProgram();

    Application.Gl.AttachShader(Handle, vertex);
    Application.Gl.AttachShader(Handle, fragment);
    Application.Gl.LinkProgram(Handle);
    Application.Gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
    if (status == 0)
      throw new Exception($"Program failed to link with error: {Application.Gl.GetProgramInfoLog(Handle)}");

    Application.Gl.DetachShader(Handle, vertex);
    Application.Gl.DetachShader(Handle, fragment);
    Application.Gl.DeleteShader(vertex);
    Application.Gl.DeleteShader(fragment);
  }

  public void Use()
  {
    Application.Gl.UseProgram(Handle);
  }

  public void SetUniform(string name, int value)
  {
    int location = Application.Gl.GetUniformLocation(Handle, name);
    if (location == -1)
    {
      throw new Exception($"{name} uniform not found on shader.");
    }
    Application.Gl.Uniform1(location, value);
  }

  public unsafe void SetUniform(string name, Matrix4x4 value)
  {
    //A new overload has been created for setting a uniform so we can use the transform in our shader.
    int location = Application.Gl.GetUniformLocation(Handle, name);
    if (location == -1)
    {
      throw new Exception($"{name} uniform not found on shader.");
    }
    Application.Gl.UniformMatrix4(location, 1, false, (float*)&value);
  }

  public void SetUniform(string name, float value)
  {
    int location = Application.Gl.GetUniformLocation(Handle, name);
    if (location == -1)
    {
      throw new Exception($"{name} uniform not found on shader.");
    }
    Application.Gl.Uniform1(location, value);
  }

  public void SetUniform(string name, Vector3 value)
  {
    int location = Application.Gl.GetUniformLocation(Handle, name);
    if (location == -1)
    {
      throw new Exception($"{name} uniform not found on shader.");
    }
    Application.Gl.Uniform3(location, value.X, value.Y, value.Z);
  }

  public void Dispose()
  {
    Application.Gl.DeleteProgram(Handle);
  }

  private uint LoadShader(ShaderType type, string path)
  {
    string src = File.ReadAllText(path);
    uint handle = Application.Gl.CreateShader(type);
    Application.Gl.ShaderSource(handle, src);
    Application.Gl.CompileShader(handle);
    string infoLog = Application.Gl.GetShaderInfoLog(handle);
    if (!string.IsNullOrWhiteSpace(infoLog))
    {
      throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
    }
    return handle;
  }

  private readonly uint Handle;
}
}