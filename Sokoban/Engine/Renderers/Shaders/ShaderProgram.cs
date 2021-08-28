using System;
using System.Collections.Generic;
using System.Numerics;
using Logger;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Sokoban.Utilities.Extensions;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Renderers.Shaders
{
public class ShaderProgram : IDisposable
{
  public uint Handle { get; }
  private IReadOnlyList<Shader> Shaders { get; }

  public ShaderProgram(params Shader[] shaders)
  {
    Handle = App.Gl.CreateProgram();
    Shaders = shaders;
    AttachShaders();
    Link();
  }

  public void Link()
  {
    App.Gl.LinkProgram(Handle);
    VerifyLinkStatus();
  }
  private void VerifyLinkStatus()
  {
    App.Gl.GetProgram(Handle, GLEnum.LinkStatus, out var status);
    if (status != 0) return;
    $"<c6 Program|> <c124 failed|> <c6 to link with error|>: <c124 {App.Gl.GetProgramInfoLog(Handle)}|>".LogLine();
    throw new Exception();
  }

  public void Bind() => App.Gl.UseProgram(Handle);

  public void SetUniform(string name, int value) =>
    App.Gl.Uniform1(UniformLocation(name), value);
  public void SetUniform(string name, float value) =>
    App.Gl.Uniform1(UniformLocation(name), value);
  public void SetUniform(string name, double value) =>
    App.Gl.Uniform1(UniformLocation(name), value);

  public void SetUniform(string name, Vector2D<float> value) =>
    App.Gl.Uniform2(UniformLocation(name), (Vector2)value);
  public void SetUniform(string name, Vector3D<float> value) =>
    App.Gl.Uniform3(UniformLocation(name), (Vector3)value);
  public void SetUniform(string name, Vector4D<float> value) =>
    App.Gl.Uniform4(UniformLocation(name), (Vector4)value);

  public unsafe void SetUniform(string name, Matrix4X4<float> value, bool transpose = false) =>
    App.Gl.UniformMatrix4(UniformLocation(name), 1, transpose, (float*)&value);

  private int UniformLocation(string name)
  {
    var location = App.Gl.GetUniformLocation(Handle, name);
    if (location == -1) throw new Exception($"{name} uniform not found on shader.");
    return location;
  }
  private int AttributeLocation(string name)
  {
    var location = App.Gl.GetAttribLocation(Handle, name);
    if (location == -1) throw new Exception($"{name} attribute not found on shader.");
    return location;
  }

  private void AttachShaders() => Shaders.ForEach(AttachShader);
  private void AttachShader(Shader shader) => App.Gl.AttachShader(Handle, shader.Handle);

  private void DetachShaders() => Shaders.ForEach(DetachShader);
  private void DetachShader(Shader shader) => App.Gl.DetachShader(Handle, shader.Handle);

  public void Dispose()
  {
    App.Gl.DeleteProgram(Handle);
  }
}
}