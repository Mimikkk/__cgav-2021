using Silk.NET.OpenGL;

namespace Sokoban
{
internal static class Program
{
  private static GL Gl;

  private static uint Vbo;
  private static uint Ebo;
  private static uint Vao;
  private static uint Shader;

  private static readonly string VertexShaderSource = @"
        #version 330 core //Using version GLSL version 3.3
        layout (location = 0) in vec4 vPos;
        
        void main()
        {
            gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
        }
        ";

  //Fragment shaders are run on each fragment/pixel of the geometry.
  private static readonly string FragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;
        void main()
        {
            FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }
        ";


  //Vertex data, uploaded to the VBO.
  private static readonly float[] Vertices =
  {
    //X    Y      Z
    0.5f,  0.5f, 0.0f,
    0.5f, -0.5f, 0.0f,
    -0.5f, -0.5f, 0.0f,
    -0.5f,  0.5f, 0.5f
  };

  //Index data, uploaded to the EBO.
  private static readonly uint[] Indices =
  {
    0, 1, 3,
    1, 2, 3
  };

  private static void Main() => Application.Run();
}
}