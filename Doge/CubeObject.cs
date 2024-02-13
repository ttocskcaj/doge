namespace Silk.NET_Tutorials;

using System.Numerics;
using Silk.NET_Tutorials.Rendering;
using Silk.NET.OpenGL;
using Shader = Silk.NET_Tutorials.Rendering.Shader;
using Texture = Silk.NET_Tutorials.Rendering.Texture;

public abstract class CubeObject : GameObject
{
     // Vertex data: position (X, Y, Z), texture coordinates (U, V), normal (X, Y, Z)
    private static readonly float[] Vertices =
    {
        // Front face
        -1.0f, -1.0f,  1.0f,  0.0f, 0.0f,  0.0f,  0.0f,  1.0f, // Bottom-left
        1.0f, -1.0f,  1.0f,  1.0f, 0.0f,  0.0f,  0.0f,  1.0f, // Bottom-right
        1.0f,  1.0f,  1.0f,  1.0f, 1.0f,  0.0f,  0.0f,  1.0f, // Top-right
        -1.0f,  1.0f,  1.0f,  0.0f, 1.0f,  0.0f,  0.0f,  1.0f, // Top-left 
        // Back face
        -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,  0.0f,  0.0f, -1.0f,
        1.0f, -1.0f, -1.0f,  1.0f, 0.0f,  0.0f,  0.0f, -1.0f,
        1.0f,  1.0f, -1.0f,  1.0f, 1.0f,  0.0f,  0.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,  0.0f, 1.0f,  0.0f,  0.0f, -1.0f,
        // Top face
        -1.0f,  1.0f, -1.0f,  0.0f, 0.0f,  0.0f,  1.0f,  0.0f,
        1.0f,  1.0f, -1.0f,  1.0f, 0.0f,  0.0f,  1.0f,  0.0f,
        1.0f,  1.0f,  1.0f,  1.0f, 1.0f,  0.0f,  1.0f,  0.0f,
        -1.0f,  1.0f,  1.0f,  0.0f, 1.0f,  0.0f,  1.0f,  0.0f,
        // Bottom face
        -1.0f, -1.0f, -1.0f,  0.0f, 0.0f,  0.0f, -1.0f,  0.0f,
        1.0f, -1.0f, -1.0f,  1.0f, 0.0f,  0.0f, -1.0f,  0.0f,
        1.0f, -1.0f,  1.0f,  1.0f, 1.0f,  0.0f, -1.0f,  0.0f,
        -1.0f, -1.0f,  1.0f,  0.0f, 1.0f,  0.0f, -1.0f,  0.0f,
        // Right face
        1.0f, -1.0f, -1.0f,  0.0f, 0.0f,  1.0f,  0.0f,  0.0f,
        1.0f,  1.0f, -1.0f,  1.0f, 0.0f,  1.0f,  0.0f,  0.0f,
        1.0f,  1.0f,  1.0f,  1.0f, 1.0f,  1.0f,  0.0f,  0.0f,
        1.0f, -1.0f,  1.0f,  0.0f, 1.0f,  1.0f,  0.0f,  0.0f,
        // Left face
        -1.0f, -1.0f, -1.0f,  0.0f, 0.0f, -1.0f,  0.0f,  0.0f,
        -1.0f,  1.0f, -1.0f,  1.0f, 0.0f, -1.0f,  0.0f,  0.0f,
        -1.0f,  1.0f,  1.0f,  1.0f, 1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f, -1.0f,  1.0f,  0.0f, 1.0f, -1.0f,  0.0f,  0.0f,
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
        20, 21, 22, 20, 22, 23,
    };

    public CubeObject(GL gl, Texture texture, Shader shader) : base(gl, texture, shader)
    {
        this.Ebo = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
        this.Vbo = new BufferObject<float>(gl, Vertices, BufferTargetARB.ArrayBuffer);
        this.Vao = new VertexArrayObject<float, uint>(gl, this.Vbo, this.Ebo);

        this.Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 0);
        this.Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 8, 3);
        this.Vao.VertexAttributePointer(2, 3, VertexAttribPointerType.Float, 8, 5);
    }
    
    public override uint GetLength() => (uint)Indices.Length;
    

    public override void OnUpdate(double deltaTime, double gameTime)
    {
        var offset = Quaternion.CreateFromYawPitchRoll(MathHelper.DegreesToRadians((float)deltaTime * 60), 0f, 0f);
        this.Transform.Rotation = Quaternion.Multiply(this.Transform.Rotation, offset);
    }
}
