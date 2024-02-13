namespace Silk.NET_Tutorials;

using System.Numerics;
using Silk.NET_Tutorials.Rendering;
using Silk.NET.OpenGL;
using Shader = Silk.NET_Tutorials.Rendering.Shader;
using Texture = Silk.NET_Tutorials.Rendering.Texture;

public abstract class GameObject : IDisposable
{

    protected GameObject(
        GL gl,
        Texture texture,
        Shader shader)
    {
        this.Texture = texture;
        this.Shader = shader;
        this.Gl = gl;
    }
    
    public BufferObject<float> Vbo { get; set; }
    
    public BufferObject<uint> Ebo { get; set; }

    public VertexArrayObject<float, uint> Vao { get; set; }

    public Shader Shader { get; set; }

    public Texture Texture { get; set; }

    public Transform Transform { get; set; } = new Transform();

    public GL Gl { get; set; }
    
    public abstract Vector3 BackgroundColour { get; }

    public abstract void OnUpdate(double deltaTime, double gameTime);

    public abstract uint GetLength();

    public void Bind(Camera camera, Lamp lamp)
    {
        this.Vao.Bind();
        this.Shader.Use();
        this.Texture.Bind(TextureUnit.Texture0);
        
        this.Shader.SetUniform("tex", 0);
        this.Shader.SetUniform("view", camera.ViewMatrix);
        this.Shader.SetUniform("projection", camera.ProjectionMatrix);
        this.Shader.SetUniform("model", this.Transform.Matrix);
        this.Shader.SetUniform("lightColour", lamp.Colour);
        this.Shader.SetUniform("backgroundColour", this.BackgroundColour);
        this.Shader.SetUniform("lightPos", lamp.LightPosition);
        this.Shader.SetUniform("viewPos", camera.Position);
    }

    public void Dispose()
    {
        this.Vbo?.Dispose();
        this.Ebo?.Dispose();
        this.Vao?.Dispose();
        this.Shader?.Dispose();
        this.Texture?.Dispose();
    }

    public void DrawDebug()
    {
        // nothing
    }
}
