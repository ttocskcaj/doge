namespace Silk.NET_Tutorials.Rendering;

using Silk.NET.OpenGL;

public class VertexArrayObject<TVertex, TIndex> : IDisposable
    where TVertex : unmanaged
    where TIndex : unmanaged
{
    private readonly GL gl;
    private readonly uint handle;

    public VertexArrayObject(GL gl, BufferObject<TVertex> vbo,  BufferObject<TIndex> ebo)
    {
        this.gl = gl;
        this.handle = this.gl.GenVertexArray();
        this.Bind();

        vbo.Bind();
        ebo.Bind();
    }

    public VertexArrayObject(GL gl, BufferObject<float> vbo)
    {
        this.gl = gl;
        this.handle = this.gl.GenVertexArray();
        this.Bind();

        vbo.Bind();
    }

    public unsafe void VertexAttributePointer(
        uint index,
        int count,
        VertexAttribPointerType type,
        uint vertexSize,
        int offset)
    {
        this.gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint) sizeof(TVertex), (void*) (offset * sizeof(TVertex)));
        this.gl.EnableVertexAttribArray(index);
    }

    public void Bind()
    {
        this.gl.BindVertexArray(this.handle);
    }

    public void Dispose()
    {
    }
}
