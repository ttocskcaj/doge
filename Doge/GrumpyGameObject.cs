namespace Silk.NET_Tutorials;

using System.Numerics;
using Silk.NET.OpenGL;
using Shader = Silk.NET_Tutorials.Rendering.Shader;
using Texture = Silk.NET_Tutorials.Rendering.Texture;

public class DogeGameObject : CubeObject
{
    public DogeGameObject(GL gl, Texture texture, Shader shader)
        : base(gl, texture, shader)
    {
    }

    public override Vector3 BackgroundColour => new (0.2f);

    public override void OnUpdate(double deltaTime, double gameTime)
    {
    }
}
