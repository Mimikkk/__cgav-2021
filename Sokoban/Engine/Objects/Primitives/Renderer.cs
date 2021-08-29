using Silk.NET.OpenGL;
using Silk.NET.SDL;
using App = Sokoban.Engine.Application.App;
using VertexArray = Sokoban.Engine.Renderers.Buffers.Objects.VertexArray;

namespace Sokoban.Engine.Objects.Primitives
{
public static class Renderer
{
    private static unsafe void Draw(VertexArray vao, PrimitiveType primitiveType)
    {
        vao.Bind();
        App.Gl.DrawElements(primitiveType, vao.Size, DrawElementsType.UnsignedInt, null);
    }

    public static unsafe void Draw(Skybox skybox)
    {
        skybox.ShaderConfiguration();
        App.Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    public static void Clear() => App.Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));

}
}