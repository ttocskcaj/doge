namespace Silk.NET_Tutorials.Rendering;

using Silk.NET.OpenGL;

public class BufferObject<T> : IDisposable where T : unmanaged
{
    private readonly uint handle;
    private readonly BufferTargetARB bufferType;
    private readonly GL gl;

    public unsafe BufferObject(GL gl, Span<T> data, BufferTargetARB bufferType)
    {
        this.gl = gl;
        this.bufferType = bufferType;

        this.handle = this.gl.GenBuffer();
        this.Bind();

        fixed (void* d = data)
        {
            this.gl.BufferData(bufferType, (nuint) (data.Length * sizeof(T)), d, BufferUsageARB.StaticDraw);
        }
    }

    public void Bind()
    {
        this.gl.BindBuffer(this.bufferType, this.handle);
    }

    public void Dispose()
    {
        this.gl.DeleteBuffer(this.handle);
    }
}
