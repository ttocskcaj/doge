namespace Silk.NET_Tutorials;

using System.Numerics;
using Silk.NET_Tutorials.Rendering;
using Silk.NET.OpenGL;
using Shader = Silk.NET_Tutorials.Rendering.Shader;

public class Lamp
{
    private readonly GL gl;
    private readonly Vector3 location;
    private readonly Shader shader;

    private static readonly float[] Vertices =
    {
        // Front face
        -1.0f, -1.0f,  1.0f, // Bottom-left
        1.0f, -1.0f,  1.0f, // Bottom-right
        1.0f,  1.0f,  1.0f, // Top-right
        -1.0f,  1.0f,  1.0f, // Top-left 
        // Back face
        -1.0f, -1.0f, -1.0f,
        1.0f, -1.0f, -1.0f,
        1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        // Top face
        -1.0f,  1.0f, -1.0f,
        1.0f,  1.0f, -1.0f,
        1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
        // Bottom face
        -1.0f, -1.0f, -1.0f,
        1.0f, -1.0f, -1.0f,
        1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,
        // Right face
        1.0f, -1.0f, -1.0f,
        1.0f,  1.0f, -1.0f,
        1.0f,  1.0f,  1.0f,
        1.0f, -1.0f,  1.0f,
        // Left face
        -1.0f, -1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,
    };
    
    private static readonly uint[] Indices =
    {
        // Front face
        0, 1, 2,  0, 2, 3,
        // Back face
        4, 5, 6,  4, 6, 7,
        // Top face
        8, 9, 10, 8, 10, 11,
        // Bottom face
        12, 13, 14, 12, 14, 15,
        // Right face
        16, 17, 18, 16, 18, 19,
        // Left face
        20, 21, 22, 20, 22, 23
    };

    private Vector3 scale = new (0.2f);
    
    private Vector3 colour = new (1, 0.8f, 0.8f);

    public VertexArrayObject<float, uint> Vao { get; set; }

    public Lamp(GL gl, Vector3 location)
    {
        this.gl = gl;
        this.location = location;
        this.shader = new Shader(gl, "lightsource.vert", "lightsource.frag");
        this.Vbo = new BufferObject<float>(this.gl, Vertices, BufferTargetARB.ArrayBuffer);
        this.Ebo = new BufferObject<uint>(this.gl, Indices, BufferTargetARB.ElementArrayBuffer);
        this.Vao = new VertexArrayObject<float, uint>(this.gl, this.Vbo, this.Ebo);
        this.Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0);
    }

    public BufferObject<uint> Ebo { get; set; }

    public BufferObject<float> Vbo { get; set; }
    public Vector3 LightPosition => this.location;

    public void Bind(Camera camera)
    {
        Vao.Bind();
        this.shader.Use();
        
        this.shader.SetUniform("view", camera.ViewMatrix);
        this.shader.SetUniform("projection", camera.ProjectionMatrix);
        this.shader.SetUniform("model",  Matrix4x4.CreateScale(this.scale) * Matrix4x4.CreateTranslation(this.location));
        this.shader.SetUniform("modelColour", this.colour);
    }

    public uint GetLength() => (uint)Indices.Length;

    public Vector3 Colour => this.colour;
}
