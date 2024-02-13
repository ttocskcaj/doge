namespace Silk.NET_Tutorials;

using System.Numerics;
using Silk.NET.OpenGL;
using Shader = Silk.NET_Tutorials.Rendering.Shader;
using Texture = Silk.NET_Tutorials.Rendering.Texture;

public class GrumpyGameObject : CubeObject
{
    public GrumpyGameObject(GL gl, Texture texture, Shader shader)
        : base(gl, texture, shader)
    {
    }

    public override Vector3 BackgroundColour => new (0.7f);
}
