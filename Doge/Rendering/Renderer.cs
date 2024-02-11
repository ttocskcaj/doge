namespace Silk.NET_Tutorials.Rendering;

using System.Drawing;
using Silk.NET.OpenGL;

public class Renderer
{
    private readonly GL gl;
    private readonly Camera camera;

    public Renderer(GL gl, Camera camera)
    {
        this.gl = gl;
        this.camera = camera;
    }

    public unsafe void Render(List<GameObject> gameObjects,  Lamp lamp)
    {
        this.gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        this.gl.ClearColor(Color.SlateGray);

        foreach (var go in gameObjects)
        {
            go.Bind(this.camera, lamp);
            this.gl.DrawElements(PrimitiveType.Triangles, go.GetLength(), DrawElementsType.UnsignedInt, null);

            go.DrawDebug();
        }
        
        lamp.Bind(this.camera);
        this.gl.DrawElements(PrimitiveType.Triangles, lamp.GetLength(), DrawElementsType.UnsignedInt, null);

        this.camera.DrawDebug();
    }
}
