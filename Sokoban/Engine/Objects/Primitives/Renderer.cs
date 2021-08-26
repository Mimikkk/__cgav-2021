using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Sokoban.Engine.Renderers.Buffers;
using App = Sokoban.Engine.Application.App;

namespace Sokoban.Engine.Objects.Primitives
{
public static class Renderer
{
    private static unsafe void Draw(VertexArrayObject vao, PrimitiveType primitiveType)
    {
        vao.Bind();
        App.Gl.DrawElements(primitiveType, vao.Size, DrawElementsType.UnsignedInt, null);
    }

    public static unsafe void Draw(Skybox skybox)
    {
        skybox.ShaderConfiguration();
        App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
    public static void Clear() => App.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

    public static void SetDrawMode(PolygonMode mode) => App.Gl.PolygonMode(MaterialFace.FrontAndBack, mode);
    public static void SetClearColor(Color color) => App.Gl.ClearColor(color.R, color.G, color.B, color.A);
}
}